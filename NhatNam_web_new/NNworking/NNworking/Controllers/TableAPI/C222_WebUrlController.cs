using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using NNworking.Controllers;
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
    [Route("api/C222_WebUrl/{action}", Name = "C222_WebUrlApi")]
    public class C222_WebUrlController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c222_weburl = _context.C222_WebUrl.Select(i => new {
                i.ID,
                i.Controller,
                i.Action,
                i.LocalPath,
                i.Note
            });
            return Request.CreateResponse(DataSourceLoader.Load(c222_weburl, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C222_WebUrl();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));
            
            model.LocalPath = string.Empty;
            var result = _context.C222_WebUrl.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C222_WebUrl.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C222_WebUrl not found");

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
            var model = _context.C222_WebUrl.FirstOrDefault(item => item.ID == key);

            _context.C222_WebUrl.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C222_WebUrl model, IDictionary values) {
            string ID = nameof(C222_WebUrl.ID);
            string CONTROLLER = nameof(C222_WebUrl.Controller);
            string ACTION = nameof(C222_WebUrl.Action);
            string LOCAL_PATH = nameof(C222_WebUrl.LocalPath);
            string NOTE = nameof(C222_WebUrl.Note);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(CONTROLLER)) {
                model.Controller = Convert.ToString(values[CONTROLLER]);
            }

            if(values.Contains(ACTION)) {
                model.Action = Convert.ToString(values[ACTION]);
            }

            if(values.Contains(LOCAL_PATH)) {
                model.LocalPath = Convert.ToString(values[LOCAL_PATH]);
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