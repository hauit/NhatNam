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
    [Route("api/C222_MeetingComment/{action}", Name = "C222_MeetingCommentApi")]
    public class C222_MeetingCommentController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c222_meetingcomment = _context.C222_MeetingComment.Select(i => new {
                i.ID,
                i.MeetingID,
                i.Date,
                i.StaffID,
                i.Comment,
                i.ToStaff
            });
            return Request.CreateResponse(DataSourceLoader.Load(c222_meetingcomment, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C222_MeetingComment();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C222_MeetingComment.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C222_MeetingComment.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C222_MeetingComment not found");

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
            var model = _context.C222_MeetingComment.FirstOrDefault(item => item.ID == key);

            _context.C222_MeetingComment.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C222_MeetingComment model, IDictionary values) {
            string ID = nameof(C222_MeetingComment.ID);
            string MEETING_ID = nameof(C222_MeetingComment.MeetingID);
            string DATE = nameof(C222_MeetingComment.Date);
            string STAFF_ID = nameof(C222_MeetingComment.StaffID);
            string COMMENT = nameof(C222_MeetingComment.Comment);
            string TO_STAFF = nameof(C222_MeetingComment.ToStaff);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(MEETING_ID)) {
                model.MeetingID = Convert.ToInt32(values[MEETING_ID]);
            }

            if(values.Contains(DATE)) {
                model.Date = Convert.ToDateTime(values[DATE]);
            }

            if(values.Contains(STAFF_ID)) {
                model.StaffID = Convert.ToString(values[STAFF_ID]);
            }

            if(values.Contains(COMMENT)) {
                model.Comment = Convert.ToString(values[COMMENT]);
            }

            if(values.Contains(TO_STAFF)) {
                model.ToStaff = Convert.ToString(values[TO_STAFF]);
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