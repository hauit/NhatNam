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
    [Route("api/C222_WorkFolowInstance/{action}", Name = "C222_WorkFolowInstanceApi")]
    public class C222_WorkFolowInstanceController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public async Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions) {
            var c222_WorkFolowinstance = _context.C222_WorkFolowInstance.Select(i => new {
                i.ID,
                i.WorkFollow,
                i.ModuleName,
                i.CurrentStep,
                i.Status,
                i.CreateBy,
                i.CreateDate,
                i.Note
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ID" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Request.CreateResponse(await DataSourceLoader.LoadAsync(c222_WorkFolowinstance, loadOptions));
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post(FormDataCollection form) {
            var model = new C222_WorkFolowInstance();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C222_WorkFolowInstance.Add(model);
            await _context.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Created, new { result.ID });
        }

        [HttpPut]
        public async Task<HttpResponseMessage> Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = await _context.C222_WorkFolowInstance.FirstOrDefaultAsync(item => item.ID == key);
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
            var model = await _context.C222_WorkFolowInstance.FirstOrDefaultAsync(item => item.ID == key);

            _context.C222_WorkFolowInstance.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(C222_WorkFolowInstance model, IDictionary values) {
            string ID = nameof(C222_WorkFolowInstance.ID);
            string WORK_FOLLOW = nameof(C222_WorkFolowInstance.WorkFollow);
            string ItemID = nameof(C222_WorkFolowInstance.ItemID);
            string MODULE_NAME = nameof(C222_WorkFolowInstance.ModuleName);
            string CURRENT_STEP = nameof(C222_WorkFolowInstance.CurrentStep);
            string STATUS = nameof(C222_WorkFolowInstance.Status);
            string CREATE_BY = nameof(C222_WorkFolowInstance.CreateBy);
            string CREATE_DATE = nameof(C222_WorkFolowInstance.CreateDate);
            string NOTE = nameof(C222_WorkFolowInstance.Note);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(WORK_FOLLOW)) {
                model.WorkFollow = values[WORK_FOLLOW] != null ? Convert.ToInt32(values[WORK_FOLLOW]) : (int?)null;
            }

            if (values.Contains(ItemID))
            {
                model.ItemID = Convert.ToInt32(values[ItemID]);
            }

            if (values.Contains(MODULE_NAME)) {
                model.ModuleName = Convert.ToString(values[MODULE_NAME]);
            }

            if(values.Contains(CURRENT_STEP)) {
                model.CurrentStep = values[CURRENT_STEP] != null ? Convert.ToInt32(values[CURRENT_STEP]) : (int?)null;
            }

            if(values.Contains(STATUS)) {
                model.Status = values[STATUS] != null ? Convert.ToInt32(values[STATUS]) : (int?)null;
            }

            if(values.Contains(CREATE_BY)) {
                model.CreateBy = Convert.ToString(values[CREATE_BY]);
            }

            if(values.Contains(CREATE_DATE)) {
                model.CreateDate = values[CREATE_DATE] != null ? Convert.ToDateTime(values[CREATE_DATE]) : (DateTime?)null;
            }

            if(values.Contains(NOTE)) {
                model.Note = Convert.ToString(values[NOTE]);
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