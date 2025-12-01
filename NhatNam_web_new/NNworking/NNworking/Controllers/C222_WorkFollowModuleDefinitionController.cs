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
    [Route("api/C222_WorkFolowModuleDefinition/{action}", Name = "C222_WorkFolowModuleDefinitionApi")]
    public class C222_WorkFolowModuleDefinitionController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public async Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions) {
            var c222_WorkFolowmoduledefinition = _context.C222_WorkFolowModuleDefinition.Select(i => new {
                i.ID,
                i.DefinitionID,
                i.ModuleName,
                i.Note,
                i.Active,
                i.Deleted
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ID" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Request.CreateResponse(await DataSourceLoader.LoadAsync(c222_WorkFolowmoduledefinition, loadOptions));
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post(FormDataCollection form) {
            var model = new C222_WorkFolowModuleDefinition();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C222_WorkFolowModuleDefinition.Add(model);
            await _context.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Created, new { result.ID });
        }

        [HttpPut]
        public async Task<HttpResponseMessage> Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = await _context.C222_WorkFolowModuleDefinition.FirstOrDefaultAsync(item => item.ID == key);
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
            var model = await _context.C222_WorkFolowModuleDefinition.FirstOrDefaultAsync(item => item.ID == key);

            _context.C222_WorkFolowModuleDefinition.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(C222_WorkFolowModuleDefinition model, IDictionary values) {
            string ID = nameof(C222_WorkFolowModuleDefinition.ID);
            string DEFINITION_ID = nameof(C222_WorkFolowModuleDefinition.DefinitionID);
            string MODULE_NAME = nameof(C222_WorkFolowModuleDefinition.ModuleName);
            string NOTE = nameof(C222_WorkFolowModuleDefinition.Note);
            string ACTIVE = nameof(C222_WorkFolowModuleDefinition.Active);
            string DELETED = nameof(C222_WorkFolowModuleDefinition.Deleted);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(DEFINITION_ID)) {
                model.DefinitionID = values[DEFINITION_ID] != null ? Convert.ToInt32(values[DEFINITION_ID]) : (int?)null;
            }

            if(values.Contains(MODULE_NAME)) {
                model.ModuleName = Convert.ToString(values[MODULE_NAME]);
            }

            if(values.Contains(NOTE)) {
                model.Note = Convert.ToString(values[NOTE]);
            }

            if(values.Contains(ACTIVE)) {
                model.Active = values[ACTIVE] != null ? Convert.ToBoolean(values[ACTIVE]) : (bool?)null;
            }

            if(values.Contains(DELETED)) {
                model.Deleted = values[DELETED] != null ? Convert.ToBoolean(values[DELETED]) : (bool?)null;
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