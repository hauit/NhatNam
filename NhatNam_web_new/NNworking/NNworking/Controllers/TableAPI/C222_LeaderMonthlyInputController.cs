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
    [Route("api/C222_LeaderMonthlyInput/{action}", Name = "C222_LeaderMonthlyInputApi")]
    public class C222_LeaderMonthlyInputController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c222_leadermonthlyinput = _context.C222_LeaderMonthlyInput.Select(i => new {
                i.ID,
                i.Date,
                i.Month,
                i.DeptCode,
                i.InputStaff,
                i.ReciveQty,
                i.CompleteQty,
                i.IncompleteQty,
                i.MachineCapa,
                i.ErrorRate,
                i.CriticalIssue,
                i.IncompleteIssue,
                i.KaizenApplied
            });
            return Request.CreateResponse(DataSourceLoader.Load(c222_leadermonthlyinput, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage GetPHT(DataSourceLoadOptions loadOptions)
        {
            var c222_leaderdailyinput = _context.C222_LeaderMonthlyInput.Where(x => x.DeptCode == "1014").ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c222_leaderdailyinput, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage GetPSX(DataSourceLoadOptions loadOptions)
        {
            var c222_leaderdailyinput = _context.C222_LeaderMonthlyInput.Where(x => x.DeptCode == "1013").ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c222_leaderdailyinput, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C222_LeaderMonthlyInput();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C222_LeaderMonthlyInput.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C222_LeaderMonthlyInput.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C222_LeaderMonthlyInput not found");

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
            var model = _context.C222_LeaderMonthlyInput.FirstOrDefault(item => item.ID == key);

            _context.C222_LeaderMonthlyInput.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C222_LeaderMonthlyInput model, IDictionary values) {
            string ID = nameof(C222_LeaderMonthlyInput.ID);
            string DATE = nameof(C222_LeaderMonthlyInput.Date);
            string MONTH = nameof(C222_LeaderMonthlyInput.Month);
            string DEPT_CODE = nameof(C222_LeaderMonthlyInput.DeptCode);
            string INPUT_STAFF = nameof(C222_LeaderMonthlyInput.InputStaff);
            string RECIVE_QTY = nameof(C222_LeaderMonthlyInput.ReciveQty);
            string COMPLETE_QTY = nameof(C222_LeaderMonthlyInput.CompleteQty);
            string INCOMPLETE_QTY = nameof(C222_LeaderMonthlyInput.IncompleteQty);
            string MACHINE_CAPA = nameof(C222_LeaderMonthlyInput.MachineCapa);
            string ERROR_RATE = nameof(C222_LeaderMonthlyInput.ErrorRate);
            string CRITICAL_ISSUE = nameof(C222_LeaderMonthlyInput.CriticalIssue);
            string INCOMPLETE_ISSUE = nameof(C222_LeaderMonthlyInput.IncompleteIssue);
            string KAIZEN_APPLIED = nameof(C222_LeaderMonthlyInput.KaizenApplied);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(DATE)) {
                model.Date = Convert.ToDateTime(values[DATE]);
            }

            if(values.Contains(MONTH)) {
                model.Month = Convert.ToInt32(values[MONTH]);
            }

            if(values.Contains(DEPT_CODE)) {
                model.DeptCode = Convert.ToString(values[DEPT_CODE]);
            }

            if(values.Contains(INPUT_STAFF)) {
                model.InputStaff = Convert.ToString(values[INPUT_STAFF]);
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

            if(values.Contains(MACHINE_CAPA)) {
                model.MachineCapa = Convert.ToDouble(values[MACHINE_CAPA]);
            }

            if(values.Contains(ERROR_RATE)) {
                model.ErrorRate = Convert.ToDouble(values[ERROR_RATE]);
            }

            if(values.Contains(CRITICAL_ISSUE)) {
                model.CriticalIssue = Convert.ToInt32(values[CRITICAL_ISSUE]);
            }

            if(values.Contains(INCOMPLETE_ISSUE)) {
                model.IncompleteIssue = Convert.ToInt32(values[INCOMPLETE_ISSUE]);
            }

            if(values.Contains(KAIZEN_APPLIED)) {
                model.KaizenApplied = Convert.ToInt32(values[KAIZEN_APPLIED]);
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