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
    [Route("api/C242_ErrorItemNotify/{action}", Name = "C242_ErrorItemNotifyApi")]
    public class C242_ErrorItemNotifyController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c242_erroritemnotify = _context.View_242_ErrorItemNotify.ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c242_erroritemnotify, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C242_ErrorItemNotify();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_ErrorItemNotify.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_ErrorItemNotify.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C242_ErrorItemNotify not found");

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
            var model = _context.C242_ErrorItemNotify.FirstOrDefault(item => item.ID == key);

            _context.C242_ErrorItemNotify.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C242_ErrorItemNotify model, IDictionary values) {
            string ID = nameof(C242_ErrorItemNotify.ID);
            string MONO = nameof(C242_ErrorItemNotify.MONo);
            string OptionID = nameof(C242_ErrorItemNotify.OptionID);
            string PART_ID = nameof(C242_ErrorItemNotify.PartID);
            string ERROR_NUMBER = nameof(C242_ErrorItemNotify.ErrorNumber);
            string NOTIFY_DEPT = nameof(C242_ErrorItemNotify.NotifyDept);
            string ERROR_TYPE_ID = nameof(C242_ErrorItemNotify.ErrorTypeID);
            string ERROR_CONTENT = nameof(C242_ErrorItemNotify.ErrorContent);
            string NOTIFY_DATE = nameof(C242_ErrorItemNotify.NotifyDate);
            string RECEIVE_ERROR_ITEM_DATE = nameof(C242_ErrorItemNotify.ReceiveErrorItemDate);
            string NOTE = nameof(C242_ErrorItemNotify.Note);
            string SUPPLIER = nameof(C242_ErrorItemNotify.Supplier);
            string RAISE_ERROR_DEPT = nameof(C242_ErrorItemNotify.RaiseErrorDept);
            string ERROR_COMMENT = nameof(C242_ErrorItemNotify.ErrorComment);
            string CAUSE_OF_ERROR = nameof(C242_ErrorItemNotify.CauseOfError);
            string DELETED = nameof(C242_ErrorItemNotify.Deleted);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(MONO)) {
                model.MONo = Convert.ToString(values[MONO]);
            }

            if (values.Contains(OptionID))
            {
                model.OptionID = Convert.ToString(values[OptionID]);
            }

            if (values.Contains(PART_ID)) {
                model.PartID = Convert.ToString(values[PART_ID]);
            }

            if(values.Contains(ERROR_NUMBER)) {
                model.ErrorNumber = Convert.ToInt32(values[ERROR_NUMBER]);
            }

            if(values.Contains(NOTIFY_DEPT)) {
                model.NotifyDept = Convert.ToString(values[NOTIFY_DEPT]);
            }

            if(values.Contains(ERROR_TYPE_ID)) {
                model.ErrorTypeID = Convert.ToInt32(values[ERROR_TYPE_ID]);
            }

            if(values.Contains(ERROR_CONTENT)) {
                model.ErrorContent = Convert.ToString(values[ERROR_CONTENT]);
            }

            if(values.Contains(NOTIFY_DATE)) {
                model.NotifyDate = Convert.ToDateTime(values[NOTIFY_DATE]);
            }

            if(values.Contains(RECEIVE_ERROR_ITEM_DATE)) {
                model.ReceiveErrorItemDate = Convert.ToDateTime(values[RECEIVE_ERROR_ITEM_DATE]);
            }

            if(values.Contains(NOTE)) {
                model.Note = Convert.ToString(values[NOTE]);
            }

            if(values.Contains(SUPPLIER)) {
                model.Supplier = Convert.ToString(values[SUPPLIER]);
            }

            if(values.Contains(RAISE_ERROR_DEPT)) {
                model.RaiseErrorDept = Convert.ToString(values[RAISE_ERROR_DEPT]);
            }

            if(values.Contains(ERROR_COMMENT)) {
                model.ErrorComment = Convert.ToString(values[ERROR_COMMENT]);
            }

            if(values.Contains(CAUSE_OF_ERROR)) {
                model.CauseOfError = Convert.ToString(values[CAUSE_OF_ERROR]);
            }

            if(values.Contains(DELETED)) {
                model.Deleted = Convert.ToBoolean(values[DELETED]);
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