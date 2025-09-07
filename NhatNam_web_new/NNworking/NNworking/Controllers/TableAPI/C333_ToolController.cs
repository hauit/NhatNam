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
    [Route("api/C333_Tool/{action}", Name = "C333_ToolApi")]
    public class C333_ToolController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c333_tool = _context.C333_Tool.Select(i => new {
                i.ID,
                i.ToolID,
                i.ToolName,
                i.MakerToolName,
                i.Price,
                i.FrequencyID,
                i.ToolGroupID,
                i.SafeQty,
                i.MakerID,
                i.CreateAt
            });
            return Request.CreateResponse(DataSourceLoader.Load(c333_tool, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C333_Tool();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C333_Tool.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C333_Tool.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C333_Tool not found");

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
            var model = _context.C333_Tool.FirstOrDefault(item => item.ID == key);

            _context.C333_Tool.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C333_Tool model, IDictionary values) {
            string ID = nameof(C333_Tool.ID);
            string TOOL_ID = nameof(C333_Tool.ToolID);
            string TOOL_NAME = nameof(C333_Tool.ToolName);
            string MAKER_TOOL_NAME = nameof(C333_Tool.MakerToolName);
            string PRICE = nameof(C333_Tool.Price);
            string FREQUENCY_ID = nameof(C333_Tool.FrequencyID);
            string TOOL_GROUP_ID = nameof(C333_Tool.ToolGroupID);
            string SAFE_QTY = nameof(C333_Tool.SafeQty);
            string MAKER_ID = nameof(C333_Tool.MakerID);
            string CREATE_AT = nameof(C333_Tool.CreateAt);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(TOOL_ID)) {
                model.ToolID = Convert.ToString(values[TOOL_ID]);
            }

            if(values.Contains(TOOL_NAME)) {
                model.ToolName = Convert.ToString(values[TOOL_NAME]);
            }

            if(values.Contains(MAKER_TOOL_NAME)) {
                model.MakerToolName = Convert.ToString(values[MAKER_TOOL_NAME]);
            }

            if(values.Contains(PRICE)) {
                model.Price = Convert.ToDecimal(values[PRICE]);
            }

            if(values.Contains(FREQUENCY_ID)) {
                model.FrequencyID = Convert.ToString(values[FREQUENCY_ID]);
            }

            if(values.Contains(TOOL_GROUP_ID)) {
                model.ToolGroupID = Convert.ToString(values[TOOL_GROUP_ID]);
            }

            if(values.Contains(SAFE_QTY)) {
                model.SafeQty = Convert.ToInt32(values[SAFE_QTY]);
            }

            if(values.Contains(MAKER_ID)) {
                model.MakerID = Convert.ToString(values[MAKER_ID]);
            }

            if(values.Contains(CREATE_AT)) {
                model.CreateAt = Convert.ToDateTime(values[CREATE_AT]);
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