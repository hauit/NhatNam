using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using NNworking.Models.ObjectBase;
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
    [Route("api/C222_MaterialStock_Input/{action}", Name = "C222_MaterialStock_InputApi")]
    public class C222_MaterialStock_InputController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c222_materialstock_input = _context.C222_MaterialStock_Input.Select(i => new {
                i.ID,
                i.Date,
                i.MaterialID,
                i.MaterialConfiguration,
                i.Weight,
                i.Unit,
                i.Note
            });
            return Request.CreateResponse(DataSourceLoader.Load(c222_materialstock_input, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C222_MaterialStock_Input();
            IObjectBase objectBase = new MaterialStock_InputObjectBase();
            var obj1 = (object)model;
            objectBase.SetDefaultValue(ref obj1);
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C222_MaterialStock_Input.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C222_MaterialStock_Input.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C222_MaterialStock_Input not found");

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
            var model = _context.C222_MaterialStock_Input.FirstOrDefault(item => item.ID == key);

            _context.C222_MaterialStock_Input.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C222_MaterialStock_Input model, IDictionary values) {
            string ID = nameof(C222_MaterialStock_Input.ID);
            string DATE = nameof(C222_MaterialStock_Input.Date);
            string MATERIAL_ID = nameof(C222_MaterialStock_Input.MaterialID);
            string MATERIAL_CONFIGURATION = nameof(C222_MaterialStock_Input.MaterialConfiguration);
            string WEIGHT = nameof(C222_MaterialStock_Input.Weight);
            string UNIT = nameof(C222_MaterialStock_Input.Unit);
            string NOTE = nameof(C222_MaterialStock_Input.Note);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(DATE)) {
                model.Date = Convert.ToDateTime(values[DATE]);
            }

            if(values.Contains(MATERIAL_ID)) {
                model.MaterialID = Convert.ToString(values[MATERIAL_ID]);
            }

            if(values.Contains(MATERIAL_CONFIGURATION)) {
                model.MaterialConfiguration = Convert.ToString(values[MATERIAL_CONFIGURATION]);
            }

            if(values.Contains(WEIGHT)) {
                model.Weight = Convert.ToInt32(values[WEIGHT]);
            }

            if(values.Contains(UNIT)) {
                model.Unit = Convert.ToString(values[UNIT]);
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