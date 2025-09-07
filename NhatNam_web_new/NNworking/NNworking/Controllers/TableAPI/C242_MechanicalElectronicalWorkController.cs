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
    [Route("api/C242_MechanicalElectronicalWork/{action}", Name = "C242_MechanicalElectronicalWorkApi")]
    public class C242_MechanicalElectronicalWorkController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c242_mechanicalelectronicalwork = _context.C242_MechanicalElectronicalWork.ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c242_mechanicalelectronicalwork, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C242_MechanicalElectronicalWork();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_MechanicalElectronicalWork.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_MechanicalElectronicalWork.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C242_MechanicalElectronicalWork not found");

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
            var model = _context.C242_MechanicalElectronicalWork.FirstOrDefault(item => item.ID == key);

            _context.C242_MechanicalElectronicalWork.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C242_MechanicalElectronicalWork model, IDictionary values) {
            string ID = nameof(C242_MechanicalElectronicalWork.ID);
            string WORK_CONTENT = nameof(C242_MechanicalElectronicalWork.WorkContent);
            string NATURE = nameof(C242_MechanicalElectronicalWork.Nature);
            string REQUEST_DATE = nameof(C242_MechanicalElectronicalWork.RequestDate);
            string REQUEST_STAFF = nameof(C242_MechanicalElectronicalWork.RequestStaff);
            string REQUEST_DEPT = nameof(C242_MechanicalElectronicalWork.RequestDept);
            string DEADLINE = nameof(C242_MechanicalElectronicalWork.Deadline);
            string COMPLETE_DATE = nameof(C242_MechanicalElectronicalWork.CompleteDate);
            string MAIN_PIC = nameof(C242_MechanicalElectronicalWork.MainPIC);
            string STATUS = nameof(C242_MechanicalElectronicalWork.Status);
            string RESULT = nameof(C242_MechanicalElectronicalWork.Result);
            string IN_COMPLETE_REASON = nameof(C242_MechanicalElectronicalWork.InCompleteReason);
            string NOTE = nameof(C242_MechanicalElectronicalWork.Note);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(WORK_CONTENT)) {
                model.WorkContent = Convert.ToString(values[WORK_CONTENT]);
            }

            if(values.Contains(NATURE)) {
                model.Nature = Convert.ToString(values[NATURE]);
            }

            if(values.Contains(REQUEST_DATE)) {
                model.RequestDate = Convert.ToDateTime(values[REQUEST_DATE]);
            }

            if(values.Contains(REQUEST_STAFF)) {
                model.RequestStaff = Convert.ToString(values[REQUEST_STAFF]);
            }

            if(values.Contains(REQUEST_DEPT)) {
                model.RequestDept = Convert.ToString(values[REQUEST_DEPT]);
            }

            if(values.Contains(DEADLINE)) {
                model.Deadline = Convert.ToDateTime(values[DEADLINE]);
            }

            if(values.Contains(COMPLETE_DATE)) {
                model.CompleteDate = Convert.ToDateTime(values[COMPLETE_DATE]);
            }

            if(values.Contains(MAIN_PIC)) {
                model.MainPIC = Convert.ToString(values[MAIN_PIC]);
            }

            if(values.Contains(STATUS)) {
                model.Status = Convert.ToString(values[STATUS]);
            }

            if(values.Contains(RESULT)) {
                model.Result = Convert.ToString(values[RESULT]);
            }

            if(values.Contains(IN_COMPLETE_REASON)) {
                model.InCompleteReason = Convert.ToString(values[IN_COMPLETE_REASON]);
            }

            if(values.Contains(NOTE)) {
                model.Note = Convert.ToString(values[NOTE]);
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