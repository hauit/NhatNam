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
    [Route("api/C222_LeaderWeeklyInput/{action}", Name = "C222_LeaderWeeklyInputApi")]
    public class C222_LeaderWeeklyInputController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c222_leaderweeklyinput = _context.C222_LeaderWeeklyInput.Select(i => new {
                i.ID,
                i.Date,
                i.Week,
                i.DeptCode,
                i.InputStaff,
                i.MachineRate,
                i.EmployeeRate,
                i.ErrorRate,
                i.CriticalError,
                i.ReciveQty,
                i.CompleteQty,
                i.IncompleteQty,
                i.ErrorQty,
                i.ErrorComplete,
                i.ErrorIncomplete,
                i.IssueOnWeek,
                i.Kaizen
            });
            return Request.CreateResponse(DataSourceLoader.Load(c222_leaderweeklyinput, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage GetPHT(DataSourceLoadOptions loadOptions)
        {
            var c222_leaderdailyinput = _context.C222_LeaderWeeklyInput.Where(x => x.DeptCode == "1014").ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c222_leaderdailyinput, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage GetPSX(DataSourceLoadOptions loadOptions)
        {
            var c222_leaderdailyinput = _context.C222_LeaderWeeklyInput.Where(x => x.DeptCode == "1013").ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c222_leaderdailyinput, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C222_LeaderWeeklyInput();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C222_LeaderWeeklyInput.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C222_LeaderWeeklyInput.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C222_LeaderWeeklyInput not found");

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
            var model = _context.C222_LeaderWeeklyInput.FirstOrDefault(item => item.ID == key);

            _context.C222_LeaderWeeklyInput.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C222_LeaderWeeklyInput model, IDictionary values) {
            string ID = nameof(C222_LeaderWeeklyInput.ID);
            string DATE = nameof(C222_LeaderWeeklyInput.Date);
            string WEEK = nameof(C222_LeaderWeeklyInput.Week);
            string DEPT_CODE = nameof(C222_LeaderWeeklyInput.DeptCode);
            string INPUT_STAFF = nameof(C222_LeaderWeeklyInput.InputStaff);
            string MACHINE_RATE = nameof(C222_LeaderWeeklyInput.MachineRate);
            string EMPLOYEE_RATE = nameof(C222_LeaderWeeklyInput.EmployeeRate);
            string ERROR_RATE = nameof(C222_LeaderWeeklyInput.ErrorRate);
            string CRITICAL_ERROR = nameof(C222_LeaderWeeklyInput.CriticalError);
            string RECIVE_QTY = nameof(C222_LeaderWeeklyInput.ReciveQty);
            string COMPLETE_QTY = nameof(C222_LeaderWeeklyInput.CompleteQty);
            string INCOMPLETE_QTY = nameof(C222_LeaderWeeklyInput.IncompleteQty);
            string ERROR_QTY = nameof(C222_LeaderWeeklyInput.ErrorQty);
            string ERROR_COMPLETE = nameof(C222_LeaderWeeklyInput.ErrorComplete);
            string ERROR_INCOMPLETE = nameof(C222_LeaderWeeklyInput.ErrorIncomplete);
            string ISSUE_ON_WEEK = nameof(C222_LeaderWeeklyInput.IssueOnWeek);
            string KAIZEN = nameof(C222_LeaderWeeklyInput.Kaizen);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(DATE)) {
                model.Date = Convert.ToDateTime(values[DATE]);
            }

            if(values.Contains(WEEK)) {
                model.Week = Convert.ToInt32(values[WEEK]);
            }

            if(values.Contains(DEPT_CODE)) {
                model.DeptCode = Convert.ToString(values[DEPT_CODE]);
            }

            if(values.Contains(INPUT_STAFF)) {
                model.InputStaff = Convert.ToString(values[INPUT_STAFF]);
            }

            if(values.Contains(MACHINE_RATE)) {
                model.MachineRate = Convert.ToDouble(values[MACHINE_RATE]);
            }

            if(values.Contains(EMPLOYEE_RATE)) {
                model.EmployeeRate = Convert.ToDouble(values[EMPLOYEE_RATE]);
            }

            if(values.Contains(ERROR_RATE)) {
                model.ErrorRate = Convert.ToDouble(values[ERROR_RATE]);
            }

            if(values.Contains(CRITICAL_ERROR)) {
                model.CriticalError = Convert.ToInt32(values[CRITICAL_ERROR]);
            }

            if(values.Contains(RECIVE_QTY)) {
                model.ReciveQty = Convert.ToInt32(values[RECIVE_QTY]);
            }

            if(values.Contains(COMPLETE_QTY)) {
                model.CompleteQty = Convert.ToInt32(values[COMPLETE_QTY]);
            }

            if(values.Contains(INCOMPLETE_QTY)) {
                model.IncompleteQty = Convert.ToInt32(values[INCOMPLETE_QTY]);
            }

            if(values.Contains(ERROR_QTY)) {
                model.ErrorQty = Convert.ToInt32(values[ERROR_QTY]);
            }

            if(values.Contains(ERROR_COMPLETE)) {
                model.ErrorComplete = Convert.ToInt32(values[ERROR_COMPLETE]);
            }

            if(values.Contains(ERROR_INCOMPLETE)) {
                model.ErrorIncomplete = Convert.ToInt32(values[ERROR_INCOMPLETE]);
            }

            if(values.Contains(ISSUE_ON_WEEK)) {
                model.IssueOnWeek = Convert.ToInt32(values[ISSUE_ON_WEEK]);
            }

            if(values.Contains(KAIZEN)) {
                model.Kaizen = Convert.ToInt32(values[KAIZEN]);
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