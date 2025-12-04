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
    [Route("api/C222_WorkFolowInstanceHistory/{action}", Name = "C222_WorkFolowInstanceHistoryApi")]
    public class C222_WorkFolowInstanceHistoryController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public async Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions)
        {
            var c222_WorkFolowinstancehistory = _context.C222_WorkFolowInstanceHistory.Select(i => new
            {
                i.ID,
                i.InstanceID,
                i.StepID,
                i.StepAction,
                i.ActionBy,
                i.ActionDate,
                i.Commment,
                i.StatusAfterAction,
                i.ModuleName
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ID" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Request.CreateResponse(await DataSourceLoader.LoadAsync(c222_WorkFolowinstancehistory, loadOptions));
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post(FormDataCollection form)
        {
            var model = new C222_WorkFolowInstanceHistory();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C222_WorkFolowInstanceHistory.Add(model);
            await _context.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Created, new { result.ID });
        }

        [HttpPut]
        public async Task<HttpResponseMessage> Put(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var model = await _context.C222_WorkFolowInstanceHistory.FirstOrDefaultAsync(item => item.ID == key);
            if (model == null)
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
        public async Task Delete(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var model = await _context.C222_WorkFolowInstanceHistory.FirstOrDefaultAsync(item => item.ID == key);

            _context.C222_WorkFolowInstanceHistory.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(C222_WorkFolowInstanceHistory model, IDictionary values)
        {
            string ID = nameof(C222_WorkFolowInstanceHistory.ID);
            string INSTANCE_ID = nameof(C222_WorkFolowInstanceHistory.InstanceID);
            string STEP_ID = nameof(C222_WorkFolowInstanceHistory.StepID);
            string STEP_ACTION = nameof(C222_WorkFolowInstanceHistory.StepAction);
            string ACTION_BY = nameof(C222_WorkFolowInstanceHistory.ActionBy);
            string ACTION_DATE = nameof(C222_WorkFolowInstanceHistory.ActionDate);
            string COMMMENT = nameof(C222_WorkFolowInstanceHistory.Commment);
            string STATUS_AFTER_ACTION = nameof(C222_WorkFolowInstanceHistory.StatusAfterAction);
            string MODULE_NAME = nameof(C222_WorkFolowInstanceHistory.ModuleName);

            if (values.Contains(ID))
            {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if (values.Contains(INSTANCE_ID))
            {
                model.InstanceID = values[INSTANCE_ID] != null ? Convert.ToInt32(values[INSTANCE_ID]) : (int?)null;
            }

            if (values.Contains(STEP_ID))
            {
                model.StepID = values[STEP_ID] != null ? Convert.ToInt32(values[STEP_ID]) : (int?)null;
            }

            if (values.Contains(STEP_ACTION))
            {
                model.StepAction = values[STEP_ACTION] != null ? Convert.ToInt32(values[STEP_ACTION]) : (int?)null;
            }

            if (values.Contains(ACTION_BY))
            {
                model.ActionBy = Convert.ToString(values[ACTION_BY]);
            }

            if (values.Contains(ACTION_DATE))
            {
                model.ActionDate = values[ACTION_DATE] != null ? Convert.ToDateTime(values[ACTION_DATE]) : (DateTime?)null;
            }

            if (values.Contains(COMMMENT))
            {
                model.Commment = Convert.ToString(values[COMMMENT]);
            }

            if (values.Contains(STATUS_AFTER_ACTION))
            {
                model.StatusAfterAction = values[STATUS_AFTER_ACTION] != null ? Convert.ToInt32(values[STATUS_AFTER_ACTION]) : (int?)null;
            }

            if (values.Contains(MODULE_NAME))
            {
                model.ModuleName = Convert.ToString(values[MODULE_NAME]);
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState)
        {
            var messages = new List<string>();

            foreach (var entry in modelState)
            {
                foreach (var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}