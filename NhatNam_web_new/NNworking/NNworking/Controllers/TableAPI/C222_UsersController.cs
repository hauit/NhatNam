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
    [Route("api/C222_Users/{action}", Name = "C222_UsersApi")]
    public class C222_UsersController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c222_users = _context.C222_Users.Select(i => new {
                i.UserName,
                i.Password,
                i.UserGroupID,
                i.DepartmentID,
                i.StaffID
            });
            return Request.CreateResponse(DataSourceLoader.Load(c222_users, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C222_Users();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C222_Users.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.UserName);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToString(form.Get("key"));
            var model = _context.C222_Users.FirstOrDefault(item => item.UserName == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C222_Users not found");

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
            var key = Convert.ToString(form.Get("key"));
            var model = _context.C222_Users.FirstOrDefault(item => item.UserName == key);

            _context.C222_Users.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C222_Users model, IDictionary values) {
            string USER_NAME = nameof(C222_Users.UserName);
            string PASSWORD = nameof(C222_Users.Password);
            string USER_GROUP_ID = nameof(C222_Users.UserGroupID);
            string DEPARTMENT_ID = nameof(C222_Users.DepartmentID);
            string STAFF_ID = nameof(C222_Users.StaffID);

            if(values.Contains(USER_NAME)) {
                model.UserName = Convert.ToString(values[USER_NAME]);
            }

            if(values.Contains(PASSWORD)) {
                model.Password = Convert.ToString(values[PASSWORD]);
            }

            if(values.Contains(USER_GROUP_ID)) {
                model.UserGroupID = Convert.ToInt32(values[USER_GROUP_ID]);
            }

            if(values.Contains(DEPARTMENT_ID)) {
                model.DepartmentID = Convert.ToInt32(values[DEPARTMENT_ID]);
            }

            if(values.Contains(STAFF_ID)) {
                model.StaffID = Convert.ToString(values[STAFF_ID]);
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