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
    [Route("api/C242_TechJob1/{action}", Name = "C242_TechJob1Api")]
    public class C242_TechJob1Controller : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c242_techjob1 = _context.C242_TechJob1.ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c242_techjob1, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C242_TechJob1();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_TechJob1.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_TechJob1.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C242_TechJob1 not found");

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
            var model = _context.C242_TechJob1.FirstOrDefault(item => item.ID == key);

            _context.C242_TechJob1.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C242_TechJob1 model, IDictionary values) {
            string ID = nameof(C242_TechJob1.ID);
            string ITEM_CODE = nameof(C242_TechJob1.ItemCode);
            string ORDER_NO = nameof(C242_TechJob1.OrderNo);
            string CONTENT_CHANGED = nameof(C242_TechJob1.ContentChanged);
            string VALIDATE_CHANGE = nameof(C242_TechJob1.ValidateChange);
            string APPROVED_DATE = nameof(C242_TechJob1.ApprovedDate);
            string DRAWING_NO = nameof(C242_TechJob1.DrawingNo);
            string CHANGED_STAFF = nameof(C242_TechJob1.ChangedStaff);
            string RequestDept = nameof(C242_TechJob1.RequestDept);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(ITEM_CODE)) {
                model.ItemCode = Convert.ToString(values[ITEM_CODE]);
            }

            if(values.Contains(ORDER_NO)) {
                model.OrderNo = Convert.ToString(values[ORDER_NO]);
            }

            if(values.Contains(CONTENT_CHANGED)) {
                model.ContentChanged = Convert.ToString(values[CONTENT_CHANGED]);
            }

            if(values.Contains(VALIDATE_CHANGE)) {
                model.ValidateChange = Convert.ToString(values[VALIDATE_CHANGE]);
            }

            if(values.Contains(APPROVED_DATE)) {
                model.ApprovedDate = Convert.ToDateTime(values[APPROVED_DATE]);
            }

            if(values.Contains(DRAWING_NO)) {
                model.DrawingNo = Convert.ToString(values[DRAWING_NO]);
            }

            if(values.Contains(CHANGED_STAFF)) {
                model.ChangedStaff = Convert.ToString(values[CHANGED_STAFF]);
            }

            if(values.Contains(RequestDept)) {
                model.RequestDept = Convert.ToString(values[RequestDept]);
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