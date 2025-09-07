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
    [Route("api/C222_ShiftRegister/{action}", Name = "C222_ShiftRegisterApi")]
    public class C222_ShiftRegisterController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c222_shiftregister = _context.C222_ShiftRegister.ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c222_shiftregister, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage GetMealStatics(DataSourceLoadOptions loadOptions)
        {
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            var dateFrom = DateTime.ParseExact(queryParams["dateFrom"].Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture).Date;
            var dateTo = DateTime.ParseExact(queryParams["dateTo"].Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture).Date;
            var c242_wts = _context.sp_222_MealStatics(dateFrom,dateTo).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c242_wts, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C222_ShiftRegister();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C222_ShiftRegister.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C222_ShiftRegister.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C222_ShiftRegister not found");

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
            var model = _context.C222_ShiftRegister.FirstOrDefault(item => item.ID == key);

            _context.C222_ShiftRegister.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C222_ShiftRegister model, IDictionary values) {
            string ID = nameof(C222_ShiftRegister.ID);
            string DATE = nameof(C222_ShiftRegister.Date);
            string STAFF_ID = nameof(C222_ShiftRegister.StaffID);
            string SHIFT = nameof(C222_ShiftRegister.Shift);
            string INCOME = nameof(C222_ShiftRegister.Income);
            string OUTCOME = nameof(C222_ShiftRegister.Outcome);
            string WORK_TIME = nameof(C222_ShiftRegister.WorkTime);
            string LUNCH = nameof(C222_ShiftRegister.Lunch);
            string DINNER = nameof(C222_ShiftRegister.Dinner);
            string MILK = nameof(C222_ShiftRegister.Milk);
            string DELETED = nameof(C222_ShiftRegister.Deleted);
            string USER_DELETE = nameof(C222_ShiftRegister.UserDelete);
            string BREAKFAST = nameof(C222_ShiftRegister.Breakfast);
            string MachineID = nameof(C222_ShiftRegister.MachineID);
            string Note = nameof(C222_ShiftRegister.Note);

            if (values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(DATE)) {
                model.Date = Convert.ToDateTime(values[DATE]);
            }

            if(values.Contains(STAFF_ID)) {
                model.StaffID = Convert.ToString(values[STAFF_ID]);
            }

            if(values.Contains(SHIFT)) {
                model.Shift = Convert.ToString(values[SHIFT]);
            }

            if(values.Contains(INCOME)) {
                model.Income = Convert.ToDateTime(values[INCOME]);
            }

            if(values.Contains(OUTCOME)) {
                model.Outcome = Convert.ToDateTime(values[OUTCOME]);
            }

            if(values.Contains(WORK_TIME)) {
                model.WorkTime = Convert.ToInt32(values[WORK_TIME]);
            }

            if(values.Contains(LUNCH)) {
                model.Lunch = Convert.ToBoolean(values[LUNCH]);
            }

            if(values.Contains(DINNER)) {
                model.Dinner = Convert.ToBoolean(values[DINNER]);
            }

            if(values.Contains(MILK)) {
                model.Milk = Convert.ToBoolean(values[MILK]);
            }

            if(values.Contains(DELETED)) {
                model.Deleted = Convert.ToBoolean(values[DELETED]);
            }

            if(values.Contains(USER_DELETE)) {
                model.UserDelete = Convert.ToString(values[USER_DELETE]);
            }

            if(values.Contains(BREAKFAST)) {
                model.Breakfast = Convert.ToBoolean(values[BREAKFAST]);
            }

            if (values.Contains(MachineID))
            {
                model.MachineID = Convert.ToString(values[MachineID]);
            }

            if (values.Contains(Note))
            {
                model.Note = Convert.ToString(values[Note]);
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