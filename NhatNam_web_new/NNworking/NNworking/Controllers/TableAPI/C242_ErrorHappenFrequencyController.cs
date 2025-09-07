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
    [Route("api/C242_ErrorHappenFrequency/{action}", Name = "C242_ErrorHappenFrequencyApi")]
    public class C242_ErrorHappenFrequencyController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c242_errorhappenfrequency = _context.C242_ErrorHappenFrequency.Select(i => new {
                i.ID,
                i.FrequencyIndex,
                i.Des
            });
            return Request.CreateResponse(DataSourceLoader.Load(c242_errorhappenfrequency, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C242_ErrorHappenFrequency();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_ErrorHappenFrequency.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_ErrorHappenFrequency.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C242_ErrorHappenFrequency not found");

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
            var model = _context.C242_ErrorHappenFrequency.FirstOrDefault(item => item.ID == key);

            _context.C242_ErrorHappenFrequency.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C242_ErrorHappenFrequency model, IDictionary values) {
            string ID = nameof(C242_ErrorHappenFrequency.ID);
            string FREQUENCY_INDEX = nameof(C242_ErrorHappenFrequency.FrequencyIndex);
            string DES = nameof(C242_ErrorHappenFrequency.Des);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(FREQUENCY_INDEX)) {
                model.FrequencyIndex = Convert.ToInt32(values[FREQUENCY_INDEX]);
            }

            if(values.Contains(DES)) {
                model.Des = Convert.ToString(values[DES]);
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