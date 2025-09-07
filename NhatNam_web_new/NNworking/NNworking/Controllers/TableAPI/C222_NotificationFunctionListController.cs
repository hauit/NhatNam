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
    [Route("api/C222_NotificationFunctionList/{action}", Name = "C222_NotificationFunctionListApi")]
    public class C222_NotificationFunctionListController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c222_notificationfunctionlist = _context.C222_NotificationFunctionList.Select(i => new {
                i.ID,
                i.Name,
                i.Subject,
                i.STT,
                i.TimeSend
            }).OrderByDescending(x=>x.ID);
            return Request.CreateResponse(DataSourceLoader.Load(c222_notificationfunctionlist, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C222_NotificationFunctionList();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C222_NotificationFunctionList.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C222_NotificationFunctionList.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C222_NotificationFunctionList not found");

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
            var model = _context.C222_NotificationFunctionList.FirstOrDefault(item => item.ID == key);

            _context.C222_NotificationFunctionList.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C222_NotificationFunctionList model, IDictionary values) {
            string ID = nameof(C222_NotificationFunctionList.ID);
            string NAME = nameof(C222_NotificationFunctionList.Name);
            string SUBJECT = nameof(C222_NotificationFunctionList.Subject);
            string STT = nameof(C222_NotificationFunctionList.STT);
            string TIME_SEND = nameof(C222_NotificationFunctionList.TimeSend);
            string DATA = nameof(C222_NotificationFunctionList.Data);
            string Body = nameof(C222_NotificationFunctionList.Body);
            string DataTime = nameof(C222_NotificationFunctionList.DataTime);
            string DataTimeFrom = nameof(C222_NotificationFunctionList.DataTimeFrom);
            string DataTimeTo = nameof(C222_NotificationFunctionList.DataTimeTo);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(NAME)) {
                model.Name = Convert.ToString(values[NAME]);
            }

            if(values.Contains(SUBJECT)) {
                model.Subject = Convert.ToString(values[SUBJECT]);
            }

            if(values.Contains(STT)) {
                model.STT = Convert.ToInt32(values[STT]);
            }

            if(values.Contains(TIME_SEND)) {
                model.TimeSend = Convert.ToString(values[TIME_SEND]);
            }

            if(values.Contains(Body)) {
                model.Body = Convert.ToString(values[Body]);
            }

            if(values.Contains(DATA)) {
                model.Data = Convert.ToInt32(values[DATA]);
            }
            
            if(values.Contains(DataTime)) {
                model.DataTime = Convert.ToInt32(values[DataTime]);
            }
            
            if(values.Contains(DataTimeFrom)) {
                model.DataTimeFrom = Convert.ToInt32(values[DataTimeFrom]);
            }

            if(values.Contains(DataTimeTo)) {
                model.DataTimeTo = Convert.ToInt32(values[DataTimeTo]);
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