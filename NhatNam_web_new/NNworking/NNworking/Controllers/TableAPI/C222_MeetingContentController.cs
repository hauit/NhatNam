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
    [Route("api/C222_MeetingContent/{action}", Name = "C222_MeetingContentApi")]
    public class C222_MeetingContentController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c222_meetingcontent = _context.View_222_MeetingContent.Select(i => new {
                i.ID,
                i.Date,
                i.Staff,
                i.Subject,
                i.Solution,
                i.Command,
                i.Deadline,
                i.Evaluate,
                i.Confirmed,
                i.Deleted
            }).OrderByDescending(x=>x.Date);
            return Request.CreateResponse(DataSourceLoader.Load(c222_meetingcontent, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage GetStaffMetting(DataSourceLoadOptions loadOptions)
        {
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            var mettingID = queryParams["mettingID"];
            int id;
            if(!int.TryParse(mettingID,out id))
            {
                id = 0;
            }

            var metting = _context.View_222_MeetingContent.Where(x=>x.ID == id).FirstOrDefault();
            mettingID = string.Empty;
            if(metting != null)
            {
                mettingID = metting.Staff;
            }

            var c222_staff = _context.C222_Staff.Where(x => mettingID.IndexOf(x.StaffID) != -1).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c222_staff, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C222_MeetingContent();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C222_MeetingContent.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C222_MeetingContent.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C222_MeetingContent not found");

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
            var model = _context.C222_MeetingContent.FirstOrDefault(item => item.ID == key);
            model.Deleted = true;
            //_context.C222_MeetingContent.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C222_MeetingContent model, IDictionary values) {
            string ID = nameof(C222_MeetingContent.ID);
            string DATE = nameof(C222_MeetingContent.Date);
            string STAFF = nameof(C222_MeetingContent.Staff);
            string SUBJECT = nameof(C222_MeetingContent.Subject);
            string SOLUTION = nameof(C222_MeetingContent.Solution);
            string COMMAND = nameof(C222_MeetingContent.Command);
            string DEADLINE = nameof(C222_MeetingContent.Deadline);
            string EVALUATE = nameof(C222_MeetingContent.Evaluate);
            string CONFIRMED = nameof(C222_MeetingContent.Confirmed);
            string DELETED = nameof(C222_MeetingContent.Deleted);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(DATE)) {
                model.Date = Convert.ToDateTime(values[DATE]);
            }

            if(values.Contains(STAFF)) {
                model.Staff = Convert.ToString(values[STAFF]);
            }

            if(values.Contains(SUBJECT)) {
                model.Subject = Convert.ToString(values[SUBJECT]);
            }

            if(values.Contains(SOLUTION)) {
                model.Solution = Convert.ToString(values[SOLUTION]);
            }

            if(values.Contains(COMMAND)) {
                model.Command = Convert.ToString(values[COMMAND]);
            }

            if(values.Contains(DEADLINE)) {
                model.Deadline = Convert.ToDateTime(values[DEADLINE]);
            }

            if(values.Contains(EVALUATE)) {
                model.Evaluate = Convert.ToString(values[EVALUATE]);
            }

            if(values.Contains(CONFIRMED)) {
                model.Confirmed = Convert.ToBoolean(values[CONFIRMED]);
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