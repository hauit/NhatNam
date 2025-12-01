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
    [Route("api/C222_WorkFolowDefinition/{action}", Name = "C222_WorkFolowDefinitionApi")]
    public class C222_WorkFolowDefinitionController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public async Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions) {
            var c222_WorkFolowdefinition = _context.C222_WorkFolowDefinition.Select(i => new {
                i.ID,
                i.WorkflowName,
                i.Active
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ID" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Request.CreateResponse(await DataSourceLoader.LoadAsync(c222_WorkFolowdefinition, loadOptions));
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post(FormDataCollection form) {
            var model = new C222_WorkFolowDefinition();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            try
            {
                var result = _context.C222_WorkFolowDefinition.Add(model);
                //await _context.SaveChangesAsync();
                _context.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.Created, new { result.ID });
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));
            }
        }

        [HttpPut]
        public async Task<HttpResponseMessage> Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = await _context.C222_WorkFolowDefinition.FirstOrDefaultAsync(item => item.ID == key);
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
            var model = await _context.C222_WorkFolowDefinition.FirstOrDefaultAsync(item => item.ID == key);

            _context.C222_WorkFolowDefinition.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(C222_WorkFolowDefinition model, IDictionary values) {
            string ID = nameof(C222_WorkFolowDefinition.ID);
            string WORKFLOW_NAME = nameof(C222_WorkFolowDefinition.WorkflowName);
            string ACTIVE = nameof(C222_WorkFolowDefinition.Active);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(WORKFLOW_NAME)) {
                model.WorkflowName = Convert.ToString(values[WORKFLOW_NAME]);
            }

            if(values.Contains(ACTIVE)) {
                model.Active = values[ACTIVE] != null ? Convert.ToBoolean(values[ACTIVE]) : (bool?)null;
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