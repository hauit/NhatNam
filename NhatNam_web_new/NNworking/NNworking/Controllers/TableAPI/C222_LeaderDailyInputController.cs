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
    [Route("api/C222_LeaderDailyInput/{action}", Name = "C222_LeaderDailyInputApi")]
    public class C222_LeaderDailyInputController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c222_leaderdailyinput = _context.C222_LeaderDailyInput.Select(i => new {
                i.ID,
                i.Date,
                i.DeptCode,
                i.InputStaff,
                i.WorkingEmployee,
                i.Staff2Machine,
                i.WorkingMachine,
                i.ProblemMachine,
                i.EmptyMachine,
                i.FaultyProduct,
                i.IncompleteError,
                i.ErrorOnDay,
                i.IssueWithOther,
                i.UrgentItem,
                i.UrgentItemCompleted,
                i.NeedQty,
                i.FinishQty
            });
            return Request.CreateResponse(DataSourceLoader.Load(c222_leaderdailyinput, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage GetPHT(DataSourceLoadOptions loadOptions)
        {
            var c222_leaderdailyinput = _context.C222_LeaderDailyInput.Where(x => x.DeptCode == "1014").ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c222_leaderdailyinput, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage GetPSX(DataSourceLoadOptions loadOptions)
        {
            var c222_leaderdailyinput = _context.C222_LeaderDailyInput.Where(x => x.DeptCode == "1013").ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c222_leaderdailyinput, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C222_LeaderDailyInput();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C222_LeaderDailyInput.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C222_LeaderDailyInput.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C222_LeaderDailyInput not found");

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
            var model = _context.C222_LeaderDailyInput.FirstOrDefault(item => item.ID == key);

            _context.C222_LeaderDailyInput.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C222_LeaderDailyInput model, IDictionary values) {
            string ID = nameof(C222_LeaderDailyInput.ID);
            string DATE = nameof(C222_LeaderDailyInput.Date);
            string DEPT_CODE = nameof(C222_LeaderDailyInput.DeptCode);
            string INPUT_STAFF = nameof(C222_LeaderDailyInput.InputStaff);
            string WORKING_EMPLOYEE = nameof(C222_LeaderDailyInput.WorkingEmployee);
            string STAFF2MACHINE = nameof(C222_LeaderDailyInput.Staff2Machine);
            string WORKING_MACHINE = nameof(C222_LeaderDailyInput.WorkingMachine);
            string PROBLEM_MACHINE = nameof(C222_LeaderDailyInput.ProblemMachine);
            string EMPTY_MACHINE = nameof(C222_LeaderDailyInput.EmptyMachine);
            string FAULTY_PRODUCT = nameof(C222_LeaderDailyInput.FaultyProduct);
            string INCOMPLETE_ERROR = nameof(C222_LeaderDailyInput.IncompleteError);
            string ERROR_ON_DAY = nameof(C222_LeaderDailyInput.ErrorOnDay);
            string ISSUE_WITH_OTHER = nameof(C222_LeaderDailyInput.IssueWithOther);
            string URGENT_ITEM = nameof(C222_LeaderDailyInput.UrgentItem);
            string URGENT_ITEM_COMPLETED = nameof(C222_LeaderDailyInput.UrgentItemCompleted);
            string NEED_QTY = nameof(C222_LeaderDailyInput.NeedQty);
            string FINISH_QTY = nameof(C222_LeaderDailyInput.FinishQty);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(DATE)) {
                model.Date = Convert.ToDateTime(values[DATE]);
            }

            if(values.Contains(DEPT_CODE)) {
                model.DeptCode = Convert.ToString(values[DEPT_CODE]);
            }

            if(values.Contains(INPUT_STAFF)) {
                model.InputStaff = Convert.ToString(values[INPUT_STAFF]);
            }

            if(values.Contains(WORKING_EMPLOYEE)) {
                model.WorkingEmployee = Convert.ToInt32(values[WORKING_EMPLOYEE]);
            }

            if(values.Contains(STAFF2MACHINE)) {
                model.Staff2Machine = Convert.ToInt32(values[STAFF2MACHINE]);
            }

            if(values.Contains(WORKING_MACHINE)) {
                model.WorkingMachine = Convert.ToInt32(values[WORKING_MACHINE]);
            }

            if(values.Contains(PROBLEM_MACHINE)) {
                model.ProblemMachine = Convert.ToInt32(values[PROBLEM_MACHINE]);
            }

            if(values.Contains(EMPTY_MACHINE)) {
                model.EmptyMachine = Convert.ToInt32(values[EMPTY_MACHINE]);
            }

            if(values.Contains(FAULTY_PRODUCT)) {
                model.FaultyProduct = Convert.ToInt32(values[FAULTY_PRODUCT]);
            }

            if(values.Contains(INCOMPLETE_ERROR)) {
                model.IncompleteError = Convert.ToInt32(values[INCOMPLETE_ERROR]);
            }

            if(values.Contains(ERROR_ON_DAY)) {
                model.ErrorOnDay = Convert.ToInt32(values[ERROR_ON_DAY]);
            }

            if(values.Contains(ISSUE_WITH_OTHER)) {
                model.IssueWithOther = Convert.ToInt32(values[ISSUE_WITH_OTHER]);
            }

            if(values.Contains(URGENT_ITEM)) {
                model.UrgentItem = Convert.ToInt32(values[URGENT_ITEM]);
            }

            if(values.Contains(URGENT_ITEM_COMPLETED)) {
                model.UrgentItemCompleted = Convert.ToInt32(values[URGENT_ITEM_COMPLETED]);
            }

            if(values.Contains(NEED_QTY)) {
                model.NeedQty = Convert.ToInt32(values[NEED_QTY]);
            }

            if(values.Contains(FINISH_QTY)) {
                model.FinishQty = Convert.ToInt32(values[FINISH_QTY]);
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