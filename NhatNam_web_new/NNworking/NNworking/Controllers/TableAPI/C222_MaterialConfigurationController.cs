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
    [Route("api/C222_MaterialConfiguration/{action}", Name = "C222_MaterialConfigurationApi")]
    public class C222_MaterialConfigurationController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c222_materialconfiguration = _context.C222_MaterialConfiguration.Select(i => new {
                i.ID,
                i.MaterialID,
                i.MaterialConfiguration,
                i.Note
            });
            return Request.CreateResponse(DataSourceLoader.Load(c222_materialconfiguration, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C222_MaterialConfiguration();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C222_MaterialConfiguration.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C222_MaterialConfiguration.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C222_MaterialConfiguration not found");

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
            var model = _context.C222_MaterialConfiguration.FirstOrDefault(item => item.ID == key);

            _context.C222_MaterialConfiguration.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C222_MaterialConfiguration model, IDictionary values) {
            string ID = nameof(C222_MaterialConfiguration.ID);
            string MATERIAL_ID = nameof(C222_MaterialConfiguration.MaterialID);
            string MATERIAL_CONFIGURATION = nameof(C222_MaterialConfiguration.MaterialConfiguration);
            string NOTE = nameof(C222_MaterialConfiguration.Note);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(MATERIAL_ID)) {
                model.MaterialID = Convert.ToString(values[MATERIAL_ID]);
            }

            if(values.Contains(MATERIAL_CONFIGURATION)) {
                model.MaterialConfiguration = Convert.ToString(values[MATERIAL_CONFIGURATION]);
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