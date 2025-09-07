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
    [Route("api/C222_ReportList/{action}", Name = "C222_ReportListApi")]
    public class C222_ReportListController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c222_reportlist = _context.C222_ReportList.Select(i => new {
                i.ID,
                i.Caption,
                i.Alias,
                i.QueryStr,
                i.Note
            });
            return Request.CreateResponse(DataSourceLoader.Load(c222_reportlist, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C222_ReportList();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C222_ReportList.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C222_ReportList.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C222_ReportList not found");

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
            var model = _context.C222_ReportList.FirstOrDefault(item => item.ID == key);

            _context.C222_ReportList.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C222_ReportList model, IDictionary values) {
            string ID = nameof(C222_ReportList.ID);
            string CAPTION = nameof(C222_ReportList.Caption);
            string ALIAS = nameof(C222_ReportList.Alias);
            string QUERY_STR = nameof(C222_ReportList.QueryStr);
            string NOTE = nameof(C222_ReportList.Note);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(CAPTION)) {
                model.Caption = Convert.ToString(values[CAPTION]);
            }

            if(values.Contains(ALIAS)) {
                model.Alias = Convert.ToString(values[ALIAS]);
            }

            if(values.Contains(QUERY_STR)) {
                model.QueryStr = Convert.ToString(values[QUERY_STR]);
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