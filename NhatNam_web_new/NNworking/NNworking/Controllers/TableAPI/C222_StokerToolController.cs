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
    [Route("api/C222_StokerTool/{action}", Name = "C222_StokerToolApi")]
    public class C222_StokerToolController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);

            var c222_stokertool = _context.Sp_222_stokertool_InOut().ToList();
            //var c222_stokertool = _context.C222_StokerOutput().ToList();
            //if(queryParams.Count > 0)
            //{
            //    if (!queryParams.ContainsKey("catergory"))
            //    {
            //        return Request.CreateResponse(DataSourceLoader.Load(c222_stokertool, loadOptions));
            //    }

            //    string catergory = queryParams["catergory"];
            //    c222_stokertool = c222_stokertool.Where(x=>x.CatergoryID.ToLower() == catergory.ToLower());
            //    return Request.CreateResponse(DataSourceLoader.Load(c222_stokertool, loadOptions));
            //}
            return Request.CreateResponse(DataSourceLoader.Load(c222_stokertool, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C222_StokerTool();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C222_StokerTool.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ToolNo);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToString(form.Get("key"));
            var model = _context.C222_StokerTool.FirstOrDefault(item => item.ToolNo == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C222_StokerTool not found");

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
            var key = Convert.ToString(form.Get("key"));
            var model = _context.C222_StokerTool.FirstOrDefault(item => item.ToolNo == key);

            _context.C222_StokerTool.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C222_StokerTool model, IDictionary values) {
            string ID = nameof(C222_StokerTool.ID);
            string TOOL_NO = nameof(C222_StokerTool.ToolNo);
            string TOOL_NAME = nameof(C222_StokerTool.ToolName);
            string UNIT = nameof(C222_StokerTool.Unit);
            string CATERGORY_ID = nameof(C222_StokerTool.CatergoryID);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(TOOL_NO)) {
                model.ToolNo = Convert.ToString(values[TOOL_NO]);
            }

            if(values.Contains(TOOL_NAME)) {
                model.ToolName = Convert.ToString(values[TOOL_NAME]);
            }

            if(values.Contains(UNIT)) {
                model.Unit = Convert.ToInt32(values[UNIT]);
            }

            if(values.Contains(CATERGORY_ID)) {
                model.CatergoryID = Convert.ToString(values[CATERGORY_ID]);
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