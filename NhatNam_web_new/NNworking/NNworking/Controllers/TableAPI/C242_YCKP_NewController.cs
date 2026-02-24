using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using NNworking.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace NNworking.Controllers
{
    [Route("api/C242_YCKP_New/{action}", Name = "C242_YCKP_NewApi")]
    public class C242_YCKP_NewController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public async Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions)
        {
            // Lấy ID của bản ghi mới nhất trong mỗi nhóm
            var latestIds = await _context.View_242_YCKPXL
                    .GroupBy(i => new { i.OrderNo, i.YCKPDate, i.InputStaff })
                    .Select(g => g.OrderByDescending(x => x.ID).Select(x => x.ID).FirstOrDefault())
                    .ToListAsync();

            // Truy vấn các bản ghi chưa bị xóa và có ID trong danh sách mới nhất
            var c242_yckp_new = _context.View_242_YCKPXL
                .Where(i => latestIds.Contains(i.ID) &&
                            i.Deleted == false
                )
                .OrderBy(x => x.YCKPDeadline);

            return Request.CreateResponse(await DataSourceLoader.LoadAsync(c242_yckp_new, loadOptions));
        }

        // Lấy danh sách lịch sử xử lý YCKP theo OrderNo, InputStaff và YCKPDate
        [HttpGet]
        public async Task<HttpResponseMessage> GetList()
        {
            try
            {
                var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);

                string orderNo = queryParams.ContainsKey("OrderNo") ? queryParams["OrderNo"] : "";
                string inputStaff = queryParams.ContainsKey("InputStaff") ? queryParams["InputStaff"] : "";
                string yckpDateStr = queryParams.ContainsKey("YCKPDate") ? queryParams["YCKPDate"] : "";

                DateTime yckpDate;
                DateTime.TryParse(yckpDateStr, out yckpDate);

                // Tạo khoảng thời gian +/- 1 giây để tìm kiếm chính xác theo ngày
                var query = _context.C242_YCKP_New.AsQueryable();
                var nextSecond = yckpDate.AddSeconds(1);
                var prevSecond = yckpDate.AddSeconds(-1);

                var list = await query.Where(x =>
                            x.InputStaff == inputStaff &&
                            x.OrderNo == orderNo &&
                            x.YCKPDate > prevSecond &&
                            x.YCKPDate < nextSecond
                    ).Select(x => new
                    {
                        x.ProcessStaff,
                        x.ProcessDept,
                        x.YCKPProcessTime,
                        x.Status,
                        x.ResponseId,
                    }).ToListAsync();

                return Request.CreateResponse(HttpStatusCode.OK, list);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // Lấy danh sách YCKP đang chờ xử lý (YCKPXL)
        [HttpGet]
        public async Task<HttpResponseMessage> GetYCKPXL(DataSourceLoadOptions loadOptions)
        {
            try
            {
                var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);

                // Lấy ID của bản ghi mới nhất trong mỗi nhóm
                var latestIds = await _context.View_242_YCKPXL
                    .GroupBy(i => new { i.OrderNo, i.YCKPDate, i.InputStaff })
                    .Select(g => g.OrderByDescending(x => x.ID).Select(x => x.ID).FirstOrDefault())
                    .ToListAsync();

                // Lấy các YCKP đang xử lý, chưa bị xóa, sắp xếp theo deadline
                var c242_yckp_new = _context.View_242_YCKPXL
                    .Where(i => latestIds.Contains(i.ID) &&
                                i.Status == "Đang xử lý" &&
                                i.Deleted == false
                    )
                    .OrderBy(x => x.YCKPDeadline);

                return Request.CreateResponse(await DataSourceLoader.LoadAsync(c242_yckp_new, loadOptions));
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // Lấy danh sách YCKP đã xử lý (YCKPDXL) theo nhân viên
        [HttpGet]
        public async Task<HttpResponseMessage> GetYCKPDXL()
        {
            try
            {
                var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
                string currentStaffId = queryParams.ContainsKey("StaffID") ? queryParams["StaffID"] : "";

                // Lấy các YCKP đã được xử lý bởi nhân viên hiện tại
                var c242_yckp_new = await _context.View_242_YCKPXL
                    .Where(i => i.UpdatedStaff == currentStaffId)
                    .OrderBy(x => x.YCKPDeadline)
                    .ToListAsync();

                return Request.CreateResponse(HttpStatusCode.OK, c242_yckp_new);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // Lấy chi tiết YCKP bao gồm lịch sử xử lý và file đính kèm
        [HttpGet]
        public async Task<HttpResponseMessage> GetYCKPXLWithDetails()
        {
            try
            {
                var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);

                string orderNo = queryParams.ContainsKey("OrderNo") ? queryParams["OrderNo"] : "";
                string inputStaff = queryParams.ContainsKey("InputStaff") ? queryParams["InputStaff"] : "";
                string yckpDateStr = queryParams.ContainsKey("YCKPDate") ? queryParams["YCKPDate"] : "";

                // Parse ngày và tạo khoảng thời gian tìm kiếm
                DateTime yckpDate;
                DateTime.TryParse(yckpDateStr, out yckpDate);
                var nextSecond = yckpDate.AddSeconds(1);
                var prevSecond = yckpDate.AddSeconds(-1);

                // Lấy lịch sử xử lý YCKP
                var yckpRecords = await _context.View_242_YCKPXL
                    .Where(x =>
                            x.InputStaff == inputStaff &&
                            x.OrderNo == orderNo &&
                            x.YCKPDate > prevSecond &&
                            x.YCKPDate < nextSecond
                    )
                    .OrderBy(x => x.YCKPProcessTime)
                    .Select(x => new
                    {
                        x.OrderNo,
                        x.OptionID,
                        x.MachineID,
                        x.PartId,
                        x.Deadline,
                        x.RankLevel,
                        x.ProcessProduct,
                        x.YCKPTimes,
                        x.ProcessStatus,
                        x.YCKPContent,
                        x.CauseContent,
                        x.SolutionContent,
                        x.YCKPDate,
                        x.YCKPDeadline,
                        x.YCKPProcessTime,
                        x.InputStaff,
                        x.InputDept,
                        x.Status,
                        x.UpdatedStaff,
                        x.UpdatedDept,
                        x.UpdatedResponse,
                        x.UpdatedReason,
                        x.UpdatedSolution,
                        x.CausedDept,
                        x.CausedDetail
                    })
                    .ToListAsync();

                // Kiểm tra không tìm thấy bản ghi
                if (yckpRecords == null || !yckpRecords.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Không tìm thấy bản ghi YCKP");
                }

                // Lấy danh sách file đính kèm
                var allFiles = await _context.C242_YCKP_Files
                    .Where(x =>
                        x.StaffId == inputStaff &&
                        x.OrderNo == orderNo &&
                        x.Date > prevSecond &&
                        x.Date < nextSecond
                    )
                    .Select(x => new
                    {
                        x.Path
                    })
                    .ToListAsync();

                var result = new
                {
                    History = yckpRecords,
                    Files = allFiles
                };

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                    $"Đã xảy ra lỗi: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post(FormDataCollection form)
        {
            var model = new C242_YCKP_New();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            DateTime vietnamNow = DateTime.Today;
            if (model.YCKPDate != null)
            {
                model.YCKPDate = vietnamNow;
            }
            model.YCKPProcessTime = vietnamNow;

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_YCKP_New.Add(model);
            await _context.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Created, new { result.ID });
        }

        [HttpPut]
        public async Task<HttpResponseMessage> Put(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var model = await _context.C242_YCKP_New.FirstOrDefaultAsync(item => item.ID == key);
            if (model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "Object not found");

            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            model.YCKPProcessTime = DateTime.Today;

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete]
        public async Task Delete(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var model = await _context.C242_YCKP_New.FirstOrDefaultAsync(item => item.ID == key);

            model.Deleted = true;
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(C242_YCKP_New model, IDictionary values)
        {
            string ID = nameof(C242_YCKP_New.ID);
            string ORDER_NO = nameof(C242_YCKP_New.OrderNo);
            string OPTION_ID = nameof(C242_YCKP_New.OptionID);
            string MACHINE_ID = nameof(C242_YCKP_New.MachineID);
            string PART_ID = nameof(C242_YCKP_New.PartId);
            string DEADLINE = nameof(C242_YCKP_New.Deadline);
            string RANK_LEVEL = nameof(C242_YCKP_New.RankLevel);
            string PROCESS_PRODUCT = nameof(C242_YCKP_New.ProcessProduct);
            string YCKPTIMES = nameof(C242_YCKP_New.YCKPTimes);
            string PROCESS_STATUS = nameof(C242_YCKP_New.ProcessStatus);
            string YCKPCONTENT = nameof(C242_YCKP_New.YCKPContent);
            string CAUSE_CONTENT = nameof(C242_YCKP_New.CauseContent);
            string SOLUTION_CONTENT = nameof(C242_YCKP_New.SolutionContent);
            string YCKPDATE = nameof(C242_YCKP_New.YCKPDate);
            string YCKPDEADLINE = nameof(C242_YCKP_New.YCKPDeadline);
            string YCKPPROCESS_TIME = nameof(C242_YCKP_New.YCKPProcessTime);
            string INPUT_STAFF = nameof(C242_YCKP_New.InputStaff);
            string INPUT_DEPT = nameof(C242_YCKP_New.InputDept);
            string PROCESS_STAFF = nameof(C242_YCKP_New.ProcessStaff);
            string PROCESS_DEPT = nameof(C242_YCKP_New.ProcessDept);
            string RESPONSE_ID = nameof(C242_YCKP_New.ResponseId);
            string STATUS = nameof(C242_YCKP_New.Status);
            string DELETED = nameof(C242_YCKP_New.Deleted);

            if (values.Contains(ID))
            {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if (values.Contains(ORDER_NO))
            {
                model.OrderNo = Convert.ToString(values[ORDER_NO]);
            }

            if (values.Contains(OPTION_ID))
            {
                model.OptionID = Convert.ToString(values[OPTION_ID]);
            }

            if (values.Contains(MACHINE_ID))
            {
                model.MachineID = Convert.ToString(values[MACHINE_ID]);
            }

            if (values.Contains(PART_ID))
            {
                model.PartId = Convert.ToString(values[PART_ID]);
            }

            if (values.Contains(DEADLINE))
            {
                model.Deadline = Convert.ToDateTime(values[DEADLINE]);
            }

            if (values.Contains(RANK_LEVEL))
            {
                model.RankLevel = Convert.ToString(values[RANK_LEVEL]);
            }

            if (values.Contains(PROCESS_PRODUCT))
            {
                model.ProcessProduct = Convert.ToInt32(values[PROCESS_PRODUCT]);
            }

            if (values.Contains(YCKPTIMES))
            {
                model.YCKPTimes = Convert.ToInt32(values[YCKPTIMES]);
            }

            if (values.Contains(PROCESS_STATUS))
            {
                model.ProcessStatus = Convert.ToString(values[PROCESS_STATUS]);
            }

            if (values.Contains(YCKPCONTENT))
            {
                model.YCKPContent = Convert.ToString(values[YCKPCONTENT]);
            }

            if (values.Contains(CAUSE_CONTENT))
            {
                model.CauseContent = Convert.ToString(values[CAUSE_CONTENT]);
            }

            if (values.Contains(SOLUTION_CONTENT))
            {
                model.SolutionContent = Convert.ToString(values[SOLUTION_CONTENT]);
            }

            if (values.Contains(YCKPDATE))
            {
                model.YCKPDate = Convert.ToDateTime(values[YCKPDATE]);
            }

            if (values.Contains(YCKPDEADLINE))
            {
                model.YCKPDeadline = Convert.ToDateTime(values[YCKPDEADLINE]);
            }

            if (values.Contains(YCKPPROCESS_TIME))
            {
                model.YCKPProcessTime = Convert.ToDateTime(values[YCKPPROCESS_TIME]);
            }

            if (values.Contains(INPUT_STAFF))
            {
                model.InputStaff = Convert.ToString(values[INPUT_STAFF]);
            }

            if (values.Contains(INPUT_DEPT))
            {
                model.InputDept = Convert.ToString(values[INPUT_DEPT]);
            }

            if (values.Contains(PROCESS_STAFF))
            {
                model.ProcessStaff = Convert.ToString(values[PROCESS_STAFF]);
            }

            if (values.Contains(PROCESS_DEPT))
            {
                model.ProcessDept = Convert.ToString(values[PROCESS_DEPT]);
            }

            if (values.Contains(RESPONSE_ID))
            {
                model.ResponseId = Convert.ToString(values[RESPONSE_ID]);
            }

            if (values.Contains(STATUS))
            {
                model.Status = Convert.ToString(values[STATUS]);
            }

            if (values.Contains(DELETED))
            {
                model.Deleted = Convert.ToBoolean(values[DELETED]);
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState)
        {
            var messages = new List<string>();

            foreach (var entry in modelState)
            {
                foreach (var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}