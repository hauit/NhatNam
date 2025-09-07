using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace NNworking.Models.Controllers
{
    [Route("api/C242_YCKP/{action}", Name = "C242_YCKPApi")]
    public class C242_YCKPController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c242_yckp = _context.View_242_YCKP.ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c242_yckp, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C242_YCKP();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_YCKP.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_YCKP.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C242_YCKP not found");

            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete]
        public void Delete(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_YCKP.FirstOrDefault(item => item.ID == key);

            model.UpdateStaff =  Convert.ToString(form.Get("extraParam"));
            model.Deleted = true;
            _context.SaveChanges();
        }


        private void PopulateModel(C242_YCKP model, IDictionary values) {
            string ID = nameof(C242_YCKP.ID);
            string DATE = nameof(C242_YCKP.Date);
            string ORDER_NO = nameof(C242_YCKP.OrderNo);
            string OPTION_ID = nameof(C242_YCKP.OptionID);
            string MACHINE_ID = nameof(C242_YCKP.MachineID);
            string DATE_YCKP = nameof(C242_YCKP.DateYCKP);
            string YCKPCONTENT = nameof(C242_YCKP.YCKPContent);
            string SOLUTION_CONTENT = nameof(C242_YCKP.SolutionContent);
            string DATE_YCKPDEADLINE = nameof(C242_YCKP.DateYCKPDeadline);
            string YCKPPROCESS_TIME = nameof(C242_YCKP.YCKPProcessTime);
            string INPUT_STAFF = nameof(C242_YCKP.InputStaff);
            string PROCESS_STAFF = nameof(C242_YCKP.ProcessStaff);
            string UpdateStaff = nameof(C242_YCKP.UpdateStaff);
            string Deleted = nameof(C242_YCKP.Deleted);
            string RaiseDept = nameof(C242_YCKP.RaiseDept);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(DATE)) {
                model.Date = Convert.ToDateTime(values[DATE]);
            }

            if(values.Contains(ORDER_NO)) {
                model.OrderNo = Convert.ToString(values[ORDER_NO]);
            }

            if(values.Contains(OPTION_ID)) {
                model.OptionID = Convert.ToString(values[OPTION_ID]);
            }

            if(values.Contains(MACHINE_ID)) {
                model.MachineID = Convert.ToString(values[MACHINE_ID]);
            }

            if(values.Contains(DATE_YCKP)) {
                model.DateYCKP = Convert.ToDateTime(values[DATE_YCKP]);
            }

            if(values.Contains(YCKPCONTENT)) {
                model.YCKPContent = Convert.ToString(values[YCKPCONTENT]);
            }

            if(values.Contains(SOLUTION_CONTENT)) {
                model.SolutionContent = Convert.ToString(values[SOLUTION_CONTENT]);
            }

            if(values.Contains(DATE_YCKPDEADLINE)) {
                model.DateYCKPDeadline = Convert.ToDateTime(values[DATE_YCKPDEADLINE]);
            }

            if(values.Contains(YCKPPROCESS_TIME)) {
                model.YCKPProcessTime = Convert.ToDateTime(values[YCKPPROCESS_TIME]);
            }

            if(values.Contains(INPUT_STAFF)) {
                model.InputStaff = Convert.ToString(values[INPUT_STAFF]);
            }

            if(values.Contains(RaiseDept)) {
                model.RaiseDept = Convert.ToString(values[RaiseDept]);
            }

            if(values.Contains(PROCESS_STAFF)) {
                model.ProcessStaff = Convert.ToString(values[PROCESS_STAFF]);
            }

            if(values.Contains(UpdateStaff)) {
                model.UpdateStaff = Convert.ToString(values[UpdateStaff]);
            }

            if(values.Contains(Deleted)) {
                model.Deleted = Convert.ToBoolean(values[Deleted]);
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