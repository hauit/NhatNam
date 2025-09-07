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
    [Route("api/C242_ToolReturn/{action}", Name = "C242_ToolReturnApi")]
    public class C242_ToolReturnController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c242_toolreturn = _context.C242_ToolReturn.Select(i => new {
                i.ID,
                i.ToolID,
                i.ToolName,
                i.ToolMakerName,
                i.Quantity,
                i.ReturnTime,
                i.Acception,
                i.AcceptTime
            });
            return Request.CreateResponse(DataSourceLoader.Load(c242_toolreturn, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage GetToolNeedToReturn(DataSourceLoadOptions loadOptions)
        {
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            var machineID = queryParams["machineID"];

            var c242_toolreturn = _context.sp_242_ToolNeedToReturn(machineID).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c242_toolreturn, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C242_ToolReturn();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_ToolReturn.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_ToolReturn.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C242_ToolReturn not found");

            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            if(model.Acception && model.AcceptTime == null)
            {
                model.AcceptTime = DateTime.Now;
            }

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete]
        public void Delete(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_ToolReturn.FirstOrDefault(item => item.ID == key);

            _context.C242_ToolReturn.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C242_ToolReturn model, IDictionary values) {
            string ID = nameof(C242_ToolReturn.ID);
            string TOOL_ID = nameof(C242_ToolReturn.ToolID);
            string TOOL_NAME = nameof(C242_ToolReturn.ToolName);
            string TOOL_MAKER_NAME = nameof(C242_ToolReturn.ToolMakerName);
            string QUANTITY = nameof(C242_ToolReturn.Quantity);
            string RETURN_TIME = nameof(C242_ToolReturn.ReturnTime);
            string ACCEPTION = nameof(C242_ToolReturn.Acception);
            string ACCEPT_TIME = nameof(C242_ToolReturn.AcceptTime);
            string ReturnTool = nameof(C242_ToolReturn.ReturnTool);

            if (values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(TOOL_ID)) {
                model.ToolID = Convert.ToString(values[TOOL_ID]);
            }

            if(values.Contains(TOOL_NAME)) {
                model.ToolName = Convert.ToString(values[TOOL_NAME]);
            }

            if(values.Contains(TOOL_MAKER_NAME)) {
                model.ToolMakerName = Convert.ToString(values[TOOL_MAKER_NAME]);
            }

            if(values.Contains(QUANTITY)) {
                model.Quantity = Convert.ToInt32(values[QUANTITY]);
            }

            //if(values.Contains(RETURN_TIME)) {
            //    model.ReturnTime = Convert.ToDateTime(values[RETURN_TIME]);
            //}

            if (values.Contains(ReturnTool))
            {
                model.ReturnTool = Convert.ToBoolean(values[ReturnTool]);
                if(model.ReturnTool)
                {
                    model.ReturnTime = DateTime.Now;
                }
            }

            if (values.Contains(ACCEPTION)) {
                model.Acception = Convert.ToBoolean(values[ACCEPTION]);
            }

            if(values.Contains(ACCEPT_TIME)) {
                model.AcceptTime = Convert.ToDateTime(values[ACCEPT_TIME]);
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