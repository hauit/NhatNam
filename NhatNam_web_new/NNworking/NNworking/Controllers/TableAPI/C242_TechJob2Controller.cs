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
    [Route("api/C242_TechJob2/{action}", Name = "C242_TechJob2Api")]
    public class C242_TechJob2Controller : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c242_techjob2 = _context.C242_TechJob2.ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c242_techjob2, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C242_TechJob2();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_TechJob2.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_TechJob2.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C242_TechJob2 not found");

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
            var model = _context.C242_TechJob2.FirstOrDefault(item => item.ID == key);

            _context.C242_TechJob2.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C242_TechJob2 model, IDictionary values) {
            string ID = nameof(C242_TechJob2.ID);
            string JOB_CONTENT = nameof(C242_TechJob2.JobContent);
            string WORK_DES = nameof(C242_TechJob2.WorkDes);
            string COMPLETED_PERCEN = nameof(C242_TechJob2.CompletedPercen);
            string DEADLINE = nameof(C242_TechJob2.Deadline);
            string NOTE = nameof(C242_TechJob2.Note);
            string RequestDept = nameof(C242_TechJob2.RequestDept);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(JOB_CONTENT)) {
                model.JobContent = Convert.ToString(values[JOB_CONTENT]);
            }

            if(values.Contains(WORK_DES)) {
                model.WorkDes = Convert.ToString(values[WORK_DES]);
            }

            if(values.Contains(COMPLETED_PERCEN)) {
                model.CompletedPercen = Convert.ToString(values[COMPLETED_PERCEN]);
            }

            if(values.Contains(DEADLINE)) {
                model.Deadline = Convert.ToDateTime(values[DEADLINE]);
            }

            if(values.Contains(NOTE)) {
                model.Note = Convert.ToString(values[NOTE]);
            }

            if(values.Contains(RequestDept)) {
                model.RequestDept = Convert.ToString(values[RequestDept]);
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