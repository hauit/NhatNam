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
    [Route("api/C242_TrackingViTri/{action}", Name = "C242_TrackingViTriApi")]
    public class C242_TrackingViTriController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c242_trackingvitri = _context.C242_TrackingViTri.Select(i => new {
                i.ID,
                i.DeptCode,
                i.GiaCode,
                i.GiaDe,
                i.Tang,
                i.Thung
            });
            return Request.CreateResponse(DataSourceLoader.Load(c242_trackingvitri, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C242_TrackingViTri();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_TrackingViTri.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_TrackingViTri.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C242_TrackingViTri not found");

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
            var model = _context.C242_TrackingViTri.FirstOrDefault(item => item.ID == key);

            _context.C242_TrackingViTri.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C242_TrackingViTri model, IDictionary values) {
            string ID = nameof(C242_TrackingViTri.ID);
            string DEPT_CODE = nameof(C242_TrackingViTri.DeptCode);
            string GIA_CODE = nameof(C242_TrackingViTri.GiaCode);
            string GIA_DE = nameof(C242_TrackingViTri.GiaDe);
            string TANG = nameof(C242_TrackingViTri.Tang);
            string THUNG = nameof(C242_TrackingViTri.Thung);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(DEPT_CODE)) {
                model.DeptCode = Convert.ToString(values[DEPT_CODE]);
            }

            //if(values.Contains(GIA_CODE)) {
            //    model.GiaCode = Convert.ToString(values[GIA_CODE]);
            //}

            if(values.Contains(GIA_DE)) {
                model.GiaDe = Convert.ToString(values[GIA_DE]);
            }

            if(values.Contains(TANG)) {
                model.Tang = Convert.ToString(values[TANG]);
            }

            if(values.Contains(THUNG)) {
                model.Thung = Convert.ToString(values[THUNG]);
            }

            model.GiaCode = $@"{model.DeptCode}-{model.GiaDe}{model.Tang}{model.Thung}";
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