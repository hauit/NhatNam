using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using NNworking.Models;

namespace NNworking.Controllers
{
    [Route("api/C222_WorkFolowStep/{action}", Name = "C222_WorkFolowStepApi")]
    public class C222_WorkFolowStepController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public async Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions) {
            var C222_WorkFolowStep = _context.C222_WorkFolowStep.Select(i => new {
                i.ID,
                i.WorkFollowID,
                i.StepOder,
                i.StepName,
                i.RoleID,
                i.ActionType,
                i.NextStepOnApprove,
                i.NextStepOnReject,
                i.IsFinal
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ID" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Request.CreateResponse(await DataSourceLoader.LoadAsync(C222_WorkFolowStep, loadOptions));
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post(FormDataCollection form) {
            var model = new C222_WorkFolowStep();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C222_WorkFolowStep.Add(model);
            await _context.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Created, new { result.ID });
        }

        [HttpPut]
        public async Task<HttpResponseMessage> Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = await _context.C222_WorkFolowStep.FirstOrDefaultAsync(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "Object not found");

            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete]
        public async Task Delete(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = await _context.C222_WorkFolowStep.FirstOrDefaultAsync(item => item.ID == key);

            _context.C222_WorkFolowStep.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(C222_WorkFolowStep model, IDictionary values) {
            string ID = nameof(C222_WorkFolowStep.ID);
            string WORK_FOLLOW_ID = nameof(C222_WorkFolowStep.WorkFollowID);
            string STEP_ODER = nameof(C222_WorkFolowStep.StepOder);
            string STEP_NAME = nameof(C222_WorkFolowStep.StepName);
            string ROLE_ID = nameof(C222_WorkFolowStep.RoleID);
            string ACTION_TYPE = nameof(C222_WorkFolowStep.ActionType);
            string NEXT_STEP_ON_APPROVE = nameof(C222_WorkFolowStep.NextStepOnApprove);
            string NEXT_STEP_ON_REJECT = nameof(C222_WorkFolowStep.NextStepOnReject);
            string ManagerCheck = nameof(C222_WorkFolowStep.ManagerCheck);
            string IS_FINAL = nameof(C222_WorkFolowStep.IsFinal);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(WORK_FOLLOW_ID)) {
                model.WorkFollowID = values[WORK_FOLLOW_ID] != null ? Convert.ToInt32(values[WORK_FOLLOW_ID]) : (int?)null;
            }

            if(values.Contains(STEP_ODER)) {
                model.StepOder = values[STEP_ODER] != null ? Convert.ToInt32(values[STEP_ODER]) : (int?)null;
            }

            if(values.Contains(STEP_NAME)) {
                model.StepName = Convert.ToString(values[STEP_NAME]);
            }

            if(values.Contains(ROLE_ID)) {
                model.RoleID = values[ROLE_ID] != null ? Convert.ToInt32(values[ROLE_ID]) : (int?)null;
            }

            if(values.Contains(ACTION_TYPE)) {
                model.ActionType = values[ACTION_TYPE] != null ? Convert.ToInt32(values[ACTION_TYPE]) : (int?)null;
            }

            if(values.Contains(NEXT_STEP_ON_APPROVE)) {
                model.NextStepOnApprove = values[NEXT_STEP_ON_APPROVE] != null ? Convert.ToInt32(values[NEXT_STEP_ON_APPROVE]) : (int?)null;
            }

            if(values.Contains(NEXT_STEP_ON_REJECT)) {
                model.NextStepOnReject = values[NEXT_STEP_ON_REJECT] != null ? Convert.ToInt32(values[NEXT_STEP_ON_REJECT]) : (int?)null;
            }

            if(values.Contains(ManagerCheck)) {
                model.ManagerCheck = Convert.ToBoolean(values[ManagerCheck]);
            }

            if (values.Contains(IS_FINAL))
            {
                model.IsFinal = Convert.ToBoolean(values[IS_FINAL]);
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