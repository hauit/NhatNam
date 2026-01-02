using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
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
using System.Web.Http;
using System.Web.Http.ModelBinding;
using NNworking.Models;

namespace NNworking.Controllers
{
    [Route("api/C222_Evaluation_Reviewer/{action}", Name = "C222_Evaluation_ReviewerApi")]
    public class C222_Evaluation_ReviewerController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public async Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions) {
            var c222_evaluation_reviewer = _context.C222_Evaluation_Reviewer.Select(i => new {
                i.Id,
                i.StaffId,
                i.SecName,
                i.DeptName,
                i.Manager
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "Id" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Request.CreateResponse(await DataSourceLoader.LoadAsync(c222_evaluation_reviewer, loadOptions));
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetReviewerInfo(DataSourceLoadOptions loadOptions)
        {
            try
            {
                var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
                string staffId = queryParams.ContainsKey("StaffID") ? queryParams["StaffID"] : "";

                var reviewer = await _context.C222_Evaluation_Reviewer
                    .Where(r => r.StaffId == staffId)
                    .Select(r => new 
                    {
                        r.StaffId,
                        r.SecName,
                        r.DeptName,
                        r.Manager
                    })
                    .FirstOrDefaultAsync();

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = true,
                    data = new
                    {
                        staffId = reviewer.StaffId,
                        secName = reviewer.SecName,
                        deptName = reviewer.DeptName,
                        manager = reviewer.Manager,
                        displayMessage = $"Xin chào {reviewer.SecName}, khối {reviewer.DeptName}"
                    }
                });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(
                    HttpStatusCode.InternalServerError, 
                    new { success = false, message = "Đã xảy ra lỗi khi lấy thông tin", error = ex.Message }
                );
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post(FormDataCollection form) {
            var model = new C222_Evaluation_Reviewer();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            var managerPositions = new List<string> { "Phó phòng", "Trưởng phòng", "PGĐ" };
            model.Manager = model.SecName != null && managerPositions.Contains(model.SecName.Trim());
            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C222_Evaluation_Reviewer.Add(model);
            await _context.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Created, new { result.Id });
        }

        [HttpPut]
        public async Task<HttpResponseMessage> Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = await _context.C222_Evaluation_Reviewer.FirstOrDefaultAsync(item => item.Id == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "Object not found");

            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete]
        public async Task Delete(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = await _context.C222_Evaluation_Reviewer.FirstOrDefaultAsync(item => item.Id == key);

            _context.C222_Evaluation_Reviewer.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(C222_Evaluation_Reviewer model, IDictionary values) {
            string ID = nameof(C222_Evaluation_Reviewer.Id);
            string STAFF_ID = nameof(C222_Evaluation_Reviewer.StaffId);
            string SEC_NAME = nameof(C222_Evaluation_Reviewer.SecName);
            string DEPT_NAME = nameof(C222_Evaluation_Reviewer.DeptName);
            string MANAGER = nameof(C222_Evaluation_Reviewer.Manager);

            if(values.Contains(ID)) {
                model.Id = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(STAFF_ID)) {
                model.StaffId = Convert.ToString(values[STAFF_ID]);
            }

            if(values.Contains(SEC_NAME)) {
                model.SecName = Convert.ToString(values[SEC_NAME]);
            }

            if(values.Contains(DEPT_NAME)) {
                model.DeptName = Convert.ToString(values[DEPT_NAME]);
            }

            if(values.Contains(MANAGER)) {
                model.Manager = Convert.ToBoolean(values[MANAGER]);
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState) {
            var messages = new List<string>();

            foreach(var entry in modelState) {
                foreach(var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}