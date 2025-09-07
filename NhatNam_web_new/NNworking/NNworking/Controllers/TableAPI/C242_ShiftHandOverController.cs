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
    [Route("api/C242_ShiftHandOver/{action}", Name = "C242_ShiftHandOverApi")]
    public class C242_ShiftHandOverController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            var shift = queryParams["shift"];
            DateTime date;
            if(!DateTime.TryParse(queryParams["date"],out date))
            {
                date = DateTime.ParseExact(queryParams["date"].Substring(0, 24),
                    "ddd MMM d yyyy HH:mm:ss",
                    System.Globalization.CultureInfo.InvariantCulture).Date;
            }
            var c242_shifthandover = _context.View_242_ShiftHandOver.Where(x=>x.Date == date).ToList();
            if (!string.IsNullOrEmpty(shift))
            {
                c242_shifthandover = c242_shifthandover.Where(x => x.Shift == shift).ToList();
            }
            return Request.CreateResponse(DataSourceLoader.Load(c242_shifthandover, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C242_ShiftHandOver();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_ShiftHandOver.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_ShiftHandOver.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C242_ShiftHandOver not found");

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
            var model = _context.C242_ShiftHandOver.FirstOrDefault(item => item.ID == key);

            _context.C242_ShiftHandOver.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C242_ShiftHandOver model, IDictionary values) {
            string ID = nameof(C242_ShiftHandOver.ID);
            string DATE = nameof(C242_ShiftHandOver.Date);
            string SHIFT = nameof(C242_ShiftHandOver.Shift);
            string HAND_OVER_PERSION = nameof(C242_ShiftHandOver.HandOverPersion);
            string HAND_OVER_ITEM = nameof(C242_ShiftHandOver.HandOverItem);
            string HAND_OVER_CONTENT = nameof(C242_ShiftHandOver.HandOverContent);
            string HAND_OVER_RECIPIENT = nameof(C242_ShiftHandOver.HandOverRecipient);
            string HAND_OVER_RECIPIENT_CONFIRM = nameof(C242_ShiftHandOver.HandOverRecipientConfirm);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(DATE)) {
                model.Date = Convert.ToDateTime(values[DATE]);
            }

            if(values.Contains(SHIFT)) {
                model.Shift = Convert.ToString(values[SHIFT]);
            }

            if(values.Contains(HAND_OVER_PERSION)) {
                model.HandOverPersion = Convert.ToString(values[HAND_OVER_PERSION]);
            }

            if(values.Contains(HAND_OVER_ITEM)) {
                model.HandOverItem = Convert.ToInt32(values[HAND_OVER_ITEM]);
            }

            if(values.Contains(HAND_OVER_CONTENT)) {
                model.HandOverContent = Convert.ToString(values[HAND_OVER_CONTENT]);
            }

            if(values.Contains(HAND_OVER_RECIPIENT)) {
                model.HandOverRecipient = Convert.ToString(values[HAND_OVER_RECIPIENT]);
            }

            if(values.Contains(HAND_OVER_RECIPIENT_CONFIRM)) {
                model.HandOverRecipientConfirm = Convert.ToBoolean(values[HAND_OVER_RECIPIENT_CONFIRM]);
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