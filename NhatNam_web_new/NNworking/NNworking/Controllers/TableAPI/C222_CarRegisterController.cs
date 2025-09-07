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
    [Route("api/C222_CarRegister/{action}", Name = "C222_CarRegisterApi")]
    public class C222_CarRegisterController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c222_carregister = _context.C222_CarRegister.Where(x=>x.Deleted != true).OrderByDescending(x=>x.ID).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c222_carregister, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C222_CarRegister();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C222_CarRegister.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C222_CarRegister.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C222_CarRegister not found");

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
            var model = _context.C222_CarRegister.FirstOrDefault(item => item.ID == key);

            model.Deleted = true;
            //_context.C222_CarRegister.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C222_CarRegister model, IDictionary values) {
            string ID = nameof(C222_CarRegister.ID);
            string DATE = nameof(C222_CarRegister.Date);
            string REG_STAFF = nameof(C222_CarRegister.RegStaff);
            string NEED_DATE = nameof(C222_CarRegister.NeedDate);
            string EstimatedEndTime = nameof(C222_CarRegister.EstimatedEndTime);
            string REASON = nameof(C222_CarRegister.Reason);
            string ACCEPTED = nameof(C222_CarRegister.Accepted);
            string Completed = nameof(C222_CarRegister.Completed);
            string CAR = nameof(C222_CarRegister.Car);
            string START_TIME = nameof(C222_CarRegister.StartTime);
            string FINISH_TIME = nameof(C222_CarRegister.FinishTime);
            string NOTE = nameof(C222_CarRegister.Note);
            string SupportReason = nameof(C222_CarRegister.SupportReason);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(DATE)) {
                model.Date = Convert.ToDateTime(values[DATE]);
            }

            if(values.Contains(REG_STAFF)) {
                model.RegStaff = Convert.ToString(values[REG_STAFF]);
            }

            if(values.Contains(NEED_DATE)) {
                model.NeedDate = Convert.ToDateTime(values[NEED_DATE]);
            }

            if(values.Contains(EstimatedEndTime)) {
                model.EstimatedEndTime = Convert.ToDateTime(values[EstimatedEndTime]);
            }

            if(values.Contains(REASON)) {
                model.Reason = Convert.ToString(values[REASON]);
            }

            if(values.Contains(ACCEPTED)) {
                model.Accepted = Convert.ToBoolean(values[ACCEPTED]);
            }

            if(values.Contains(Completed)) {
                model.Completed = Convert.ToBoolean(values[Completed]);
            }

            if(values.Contains(CAR)) {
                model.Car = Convert.ToString(values[CAR]);
            }

            if(values.Contains(START_TIME)) {
                model.StartTime = Convert.ToDateTime(values[START_TIME]);
            }

            if(values.Contains(FINISH_TIME)) {
                model.FinishTime = Convert.ToDateTime(values[FINISH_TIME]);
            }

            if(values.Contains(NOTE)) {
                model.Note = Convert.ToString(values[NOTE]);
            }

            if(values.Contains(SupportReason)) {
                model.SupportReason = Convert.ToString(values[SupportReason]);
                if (String.IsNullOrEmpty(model.SupportReason))
                {
                    model.Support = false;
                }
                else
                {
                    model.Support = true;
                }
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