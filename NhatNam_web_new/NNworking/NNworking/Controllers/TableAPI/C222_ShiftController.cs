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
    [Route("api/C222_Shift/{action}", Name = "C222_ShiftApi")]
    public class C222_ShiftController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c222_shift = _context.C222_Shift.Select(i => new {
                i.ID,
                i.Shift,
                i.Description,
                i.Time,
                i.StartTime,
                i.FinishTime,
                i.Start,
                i.Finish
            });
            return Request.CreateResponse(DataSourceLoader.Load(c222_shift, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage GetForNNDS(DataSourceLoadOptions loadOptions) {
            var c222_shift = _context.C222_Shift.Where(x => x.Shift.ToLower() == "t1" || x.Shift.ToLower() == "t2").ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c222_shift, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C222_Shift();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C222_Shift.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C222_Shift.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C222_Shift not found");

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
            var model = _context.C222_Shift.FirstOrDefault(item => item.ID == key);

            _context.C222_Shift.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C222_Shift model, IDictionary values) {
            string ID = nameof(C222_Shift.ID);
            string SHIFT = nameof(C222_Shift.Shift);
            string DESCRIPTION = nameof(C222_Shift.Description);
            string TIME = nameof(C222_Shift.Time);
            string MILK = nameof(C222_Shift.Milk);
            string START_TIME = nameof(C222_Shift.StartTime);
            string FINISH_TIME = nameof(C222_Shift.FinishTime);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(SHIFT)) {
                model.Shift = Convert.ToString(values[SHIFT]);
            }

            if(values.Contains(DESCRIPTION)) {
                model.Description = Convert.ToString(values[DESCRIPTION]);
            }

            if(values.Contains(TIME)) {
                model.Time = Convert.ToInt32(values[TIME]);
            }

            if(values.Contains(MILK)) {
                model.Milk = Convert.ToBoolean(values[MILK]);
            }

            if(values.Contains(START_TIME)) {
                model.StartTime = Convert.ToDateTime(values[START_TIME]);
            }

            if(values.Contains(FINISH_TIME)) {
                model.FinishTime = Convert.ToDateTime(values[FINISH_TIME]);
            }

            model.Start = model.StartTime.Hour * 60 + model.StartTime.Minute;
            model.Finish = model.FinishTime.Hour * 60 + model.FinishTime.Minute;
            if(model.FinishTime.Date > model.StartTime.Date)
            {
                model.Finish = (model.StartTime.Hour + 24) * 60 + model.FinishTime.Minute;
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