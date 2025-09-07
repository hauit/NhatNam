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
    [Route("api/C222_NotificationSendingEmail/{action}", Name = "C222_NotificationSendingEmailApi")]
    public class C222_NotificationSendingEmailController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c222_notificationsendingemail = _context.C222_NotificationSendingEmail.Select(i => new {
                i.ID,
                i.Email,
                i.FunctionSTT,
                i.SendType
            }).OrderByDescending(x=>x.ID);
            return Request.CreateResponse(DataSourceLoader.Load(c222_notificationsendingemail, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C222_NotificationSendingEmail();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C222_NotificationSendingEmail.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C222_NotificationSendingEmail.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C222_NotificationSendingEmail not found");

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
            var model = _context.C222_NotificationSendingEmail.FirstOrDefault(item => item.ID == key);

            _context.C222_NotificationSendingEmail.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C222_NotificationSendingEmail model, IDictionary values) {
            string ID = nameof(C222_NotificationSendingEmail.ID);
            string EMAIL = nameof(C222_NotificationSendingEmail.Email);
            string FUNCTION_STT = nameof(C222_NotificationSendingEmail.FunctionSTT);
            string SEND_TYPE = nameof(C222_NotificationSendingEmail.SendType);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(EMAIL)) {
                model.Email = Convert.ToString(values[EMAIL]);
            }

            if(values.Contains(FUNCTION_STT)) {
                model.FunctionSTT = Convert.ToInt32(values[FUNCTION_STT]);
            }

            if(values.Contains(SEND_TYPE)) {
                model.SendType = Convert.ToString(values[SEND_TYPE]);
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