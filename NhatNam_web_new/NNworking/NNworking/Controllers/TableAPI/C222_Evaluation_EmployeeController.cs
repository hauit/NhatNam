using DevExpress.Xpo.DB.Helpers;
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
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace NNworking.Controllers
{
    [Route("api/C222_Evaluation_Employee/{action}", Name = "C222_Evaluation_EmployeeApi")]
    public class C222_Evaluation_EmployeeController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public async Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions)
        {
            var c222_evaluation_employee = _context.C222_Evaluation_Employee.Select(i => new
            {
                i.Id,
                i.StaffId,
                i.ProcessId,
                i.Date,
                i.NV_TC1,
                i.NV_TC2,
                i.NV_TC3,
                i.NV_TC4,
                i.NV_TC5,
                i.NV_TC6,
                i.NV_TC7,
                i.NV_TC8,
                i.NV_TC9,
                i.NV_TC10,
                i.NV_TC11,
                i.QL_TC1,
                i.QL_TC2,
                i.QL_TC3,
                i.QL_TC4,
                i.QL_TC5,
                i.QL_TC6,
                i.QL_TC7,
                i.QL_TC8,
                i.QL_TC9
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "Id" };
            // loadOptions.PaginateViaPrimaryKey = true;
                
            return Request.CreateResponse(await DataSourceLoader.LoadAsync(c222_evaluation_employee, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage GetEvaluationEmployeeData(DataSourceLoadOptions loadOptions)
        {
            try
            {
                var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
                var date = DateTime.ParseExact(queryParams["date"].Substring(0, 24),
                                  "ddd MMM d yyyy HH:mm:ss",
                                  System.Globalization.CultureInfo.InvariantCulture).Date;

                var result = _context.sp_EvaluationEmployee(date).ToList();
                return Request.CreateResponse(DataSourceLoader.Load(result, loadOptions));
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(
                    HttpStatusCode.InternalServerError,
                    new { success = false, message = "Đã xảy ra lỗi khi lấy dữ liệu", error = ex.Message }
                );
            }
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetByProcessId(DataSourceLoadOptions loadOptions)
        {
            try
            {
                var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
                string staffId = queryParams.ContainsKey("StaffID") ? queryParams["StaffID"] : "";

                var query = _context.C222_Evaluation_Employee
                    .Where(e => e.ProcessId == staffId)
                    .Select(e => new
                    {
                        e.Id,
                        e.StaffId,
                        e.ProcessId,
                        e.Date,
                        e.NV_TC1,
                        e.NV_TC2,
                        e.NV_TC3,
                        e.NV_TC4,
                        e.NV_TC5,
                        e.NV_TC6,
                        e.NV_TC7,
                        e.NV_TC8,
                        e.NV_TC9,
                        e.NV_TC10,
                        e.NV_TC11,
                        e.QL_TC1,
                        e.QL_TC2,
                        e.QL_TC3,
                        e.QL_TC4,
                        e.QL_TC5,
                        e.QL_TC6,
                        e.QL_TC7,
                        e.QL_TC8,
                        e.QL_TC9
                    });

                return Request.CreateResponse(await DataSourceLoader.LoadAsync(query, loadOptions));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(
                    HttpStatusCode.InternalServerError,
                    new { success = false, message = "Đã xảy ra lỗi khi lấy dữ liệu", error = ex.Message }
                );
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post(FormDataCollection form)
        {
            var model = new C222_Evaluation_Employee();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);
            
            model.Date = DateTime.Today;

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C222_Evaluation_Employee.Add(model);
            await _context.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Created, new { result.Id });
        }

        [HttpPut]
        public async Task<HttpResponseMessage> Put(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var model = await _context.C222_Evaluation_Employee.FirstOrDefaultAsync(item => item.Id == key);
            if (model == null)
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
        public async Task Delete(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var model = await _context.C222_Evaluation_Employee.FirstOrDefaultAsync(item => item.Id == key);

            _context.C222_Evaluation_Employee.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(C222_Evaluation_Employee model, IDictionary values)
        {
            string ID = nameof(C222_Evaluation_Employee.Id);
            string STAFF_ID = nameof(C222_Evaluation_Employee.StaffId);
            string PROCESS_ID = nameof(C222_Evaluation_Employee.ProcessId);
            string DATE = nameof(C222_Evaluation_Employee.Date);
            string NV_TC1 = nameof(C222_Evaluation_Employee.NV_TC1);
            string NV_TC2 = nameof(C222_Evaluation_Employee.NV_TC2);
            string NV_TC3 = nameof(C222_Evaluation_Employee.NV_TC3);
            string NV_TC4 = nameof(C222_Evaluation_Employee.NV_TC4);
            string NV_TC5 = nameof(C222_Evaluation_Employee.NV_TC5);
            string NV_TC6 = nameof(C222_Evaluation_Employee.NV_TC6);
            string NV_TC7 = nameof(C222_Evaluation_Employee.NV_TC7);
            string NV_TC8 = nameof(C222_Evaluation_Employee.NV_TC8);
            string NV_TC9 = nameof(C222_Evaluation_Employee.NV_TC9);
            string NV_TC10 = nameof(C222_Evaluation_Employee.NV_TC10);
            string NV_TC11 = nameof(C222_Evaluation_Employee.NV_TC11);
            string QL_TC1 = nameof(C222_Evaluation_Employee.QL_TC1);
            string QL_TC2 = nameof(C222_Evaluation_Employee.QL_TC2);
            string QL_TC3 = nameof(C222_Evaluation_Employee.QL_TC3);
            string QL_TC4 = nameof(C222_Evaluation_Employee.QL_TC4);
            string QL_TC5 = nameof(C222_Evaluation_Employee.QL_TC5);
            string QL_TC6 = nameof(C222_Evaluation_Employee.QL_TC6);
            string QL_TC7 = nameof(C222_Evaluation_Employee.QL_TC7);
            string QL_TC8 = nameof(C222_Evaluation_Employee.QL_TC8);
            string QL_TC9 = nameof(C222_Evaluation_Employee.QL_TC9);

            if (values.Contains(ID))
            {
                model.Id = Convert.ToInt32(values[ID]);
            }

            if (values.Contains(STAFF_ID))
            {
                model.StaffId = Convert.ToString(values[STAFF_ID]);
            }

            if (values.Contains(PROCESS_ID))
            {
                model.ProcessId = Convert.ToString(values[PROCESS_ID]);
            }

            if (values.Contains(DATE))
            {
                model.Date = Convert.ToDateTime(values[DATE]);
            }

            if (values.Contains(NV_TC1))
            {
                model.NV_TC1 = values[NV_TC1] != null ? Convert.ToInt32(values[NV_TC1]) : (int?)null;
            }

            if (values.Contains(NV_TC2))
            {
                model.NV_TC2 = values[NV_TC2] != null ? Convert.ToInt32(values[NV_TC2]) : (int?)null;
            }

            if (values.Contains(NV_TC3))
            {
                model.NV_TC3 = values[NV_TC3] != null ? Convert.ToInt32(values[NV_TC3]) : (int?)null;
            }

            if (values.Contains(NV_TC4))
            {
                model.NV_TC4 = values[NV_TC4] != null ? Convert.ToInt32(values[NV_TC4]) : (int?)null;
            }

            if (values.Contains(NV_TC5))
            {
                model.NV_TC5 = values[NV_TC5] != null ? Convert.ToInt32(values[NV_TC5]) : (int?)null;
            }

            if (values.Contains(NV_TC6))
            {
                model.NV_TC6 = values[NV_TC6] != null ? Convert.ToInt32(values[NV_TC6]) : (int?)null;
            }

            if (values.Contains(NV_TC7))
            {
                model.NV_TC7 = values[NV_TC7] != null ? Convert.ToInt32(values[NV_TC7]) : (int?)null;
            }

            if (values.Contains(NV_TC8))
            {
                model.NV_TC8 = values[NV_TC8] != null ? Convert.ToInt32(values[NV_TC8]) : (int?)null;
            }

            if (values.Contains(NV_TC9))
            {
                model.NV_TC9 = values[NV_TC9] != null ? Convert.ToInt32(values[NV_TC9]) : (int?)null;
            }

            if (values.Contains(NV_TC10))
            {
                model.NV_TC10 = values[NV_TC10] != null ? Convert.ToInt32(values[NV_TC10]) : (int?)null;
            }

            if (values.Contains(NV_TC11))
            {
                model.NV_TC11 = values[NV_TC11] != null ? Convert.ToInt32(values[NV_TC11]) : (int?)null;
            }

            if (values.Contains(QL_TC1))
            {
                model.QL_TC1 = values[QL_TC1] != null ? Convert.ToInt32(values[QL_TC1]) : (int?)null;
            }

            if (values.Contains(QL_TC2))
            {
                model.QL_TC2 = values[QL_TC2] != null ? Convert.ToInt32(values[QL_TC2]) : (int?)null;
            }

            if (values.Contains(QL_TC3))
            {
                model.QL_TC3 = values[QL_TC3] != null ? Convert.ToInt32(values[QL_TC3]) : (int?)null;
            }

            if (values.Contains(QL_TC4))
            {
                model.QL_TC4 = values[QL_TC4] != null ? Convert.ToInt32(values[QL_TC4]) : (int?)null;
            }

            if (values.Contains(QL_TC5))
            {
                model.QL_TC5 = values[QL_TC5] != null ? Convert.ToInt32(values[QL_TC5]) : (int?)null;
            }

            if (values.Contains(QL_TC6))
            {
                model.QL_TC6 = values[QL_TC6] != null ? Convert.ToInt32(values[QL_TC6]) : (int?)null;
            }

            if (values.Contains(QL_TC7))
            {
                model.QL_TC7 = values[QL_TC7] != null ? Convert.ToInt32(values[QL_TC7]) : (int?)null;
            }

            if (values.Contains(QL_TC8))
            {
                model.QL_TC8 = values[QL_TC8] != null ? Convert.ToInt32(values[QL_TC8]) : (int?)null;
            }

            if (values.Contains(QL_TC9))
            {
                model.QL_TC9 = values[QL_TC9] != null ? Convert.ToInt32(values[QL_TC9]) : (int?)null;
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