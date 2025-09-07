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
    [Route("api/C222_StaffShift/{action}", Name = "C222_StaffShiftApi")]
    public class C222_StaffShiftController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c222_staffshift = _context.C222_StaffShift.Select(i => new {
                i.ID,
                i.StaffID,
                i.Date,
                i.Shift,
                i.Lunch,
                i.Dinner,
                i.Milk,
                i.ShiftTimeStart,
                i.ShiftTimeFinish,
                i.StartWork,
                i.FinishWork
            });
            return Request.CreateResponse(DataSourceLoader.Load(c222_staffshift, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C222_StaffShift();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C222_StaffShift.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C222_StaffShift.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C222_StaffShift not found");

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
            var model = _context.C222_StaffShift.FirstOrDefault(item => item.ID == key);

            _context.C222_StaffShift.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C222_StaffShift model, IDictionary values) {
            string ID = nameof(C222_StaffShift.ID);
            string STAFF_ID = nameof(C222_StaffShift.StaffID);
            string DATE = nameof(C222_StaffShift.Date);
            string SHIFT = nameof(C222_StaffShift.Shift);
            string LUNCH = nameof(C222_StaffShift.Lunch);
            string DINNER = nameof(C222_StaffShift.Dinner);
            string MILK = nameof(C222_StaffShift.Milk);
            string SHIFT_TIME_START = nameof(C222_StaffShift.ShiftTimeStart);
            string SHIFT_TIME_FINISH = nameof(C222_StaffShift.ShiftTimeFinish);
            string START_WORK = nameof(C222_StaffShift.StartWork);
            string FINISH_WORK = nameof(C222_StaffShift.FinishWork);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(STAFF_ID)) {
                model.StaffID = Convert.ToString(values[STAFF_ID]);
            }

            if(values.Contains(DATE)) {
                model.Date = Convert.ToDateTime(values[DATE]);
            }

            if(values.Contains(SHIFT)) {
                model.Shift = Convert.ToString(values[SHIFT]);
            }

            if(values.Contains(LUNCH)) {
                model.Lunch = Convert.ToString(values[LUNCH]);
            }

            if(values.Contains(MILK)) {
                model.Milk = Convert.ToBoolean(values[MILK]);
            }

            if(values.Contains(DINNER)) {
                model.Dinner = Convert.ToString(values[DINNER]);
            }

            if(values.Contains(SHIFT_TIME_START)) {
                model.ShiftTimeStart = Convert.ToDateTime(values[SHIFT_TIME_START]);
            }

            if(values.Contains(SHIFT_TIME_FINISH)) {
                model.ShiftTimeFinish = Convert.ToDateTime(values[SHIFT_TIME_FINISH]);
            }

            if(values.Contains(START_WORK)) {
                model.StartWork = Convert.ToDateTime(values[START_WORK]);
            }

            if(values.Contains(FINISH_WORK)) {
                model.FinishWork = Convert.ToDateTime(values[FINISH_WORK]);
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