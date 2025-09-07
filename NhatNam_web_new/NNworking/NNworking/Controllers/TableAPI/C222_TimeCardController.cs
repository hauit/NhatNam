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
    [Route("api/C222_TimeCard/{action}", Name = "C222_TimeCardApi")]
    public class C222_TimeCardController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c222_timecard = _context.C222_TimeCard.Select(i => new {
                i.ID,
                i.StaffID,
                i.Date,
                i.InTime,
                i.OutTime,
                i.Deleted
            });
            return Request.CreateResponse(DataSourceLoader.Load(c222_timecard, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C222_TimeCard();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C222_TimeCard.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C222_TimeCard.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C222_TimeCard not found");

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
            var model = _context.C222_TimeCard.FirstOrDefault(item => item.ID == key);

            _context.C222_TimeCard.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C222_TimeCard model, IDictionary values) {
            string ID = nameof(C222_TimeCard.ID);
            string STAFF_ID = nameof(C222_TimeCard.StaffID);
            string DATE = nameof(C222_TimeCard.Date);
            string IN_TIME = nameof(C222_TimeCard.InTime);
            string OUT_TIME = nameof(C222_TimeCard.OutTime);
            string DELETED = nameof(C222_TimeCard.Deleted);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(STAFF_ID)) {
                model.StaffID = Convert.ToString(values[STAFF_ID]);
            }

            if(values.Contains(DATE)) {
                model.Date = Convert.ToDateTime(values[DATE]);
            }

            if(values.Contains(IN_TIME)) {
                model.InTime = Convert.ToDateTime(values[IN_TIME]);
            }

            if(values.Contains(OUT_TIME)) {
                model.OutTime = Convert.ToDateTime(values[OUT_TIME]);
            }

            if(values.Contains(DELETED)) {
                model.Deleted = Convert.ToBoolean(values[DELETED]);
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