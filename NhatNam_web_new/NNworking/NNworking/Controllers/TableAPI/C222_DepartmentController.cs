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
    [Route("api/C222_Department/{action}", Name = "C222_DepartmentApi")]
    public class C222_DepartmentController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c222_department = _context.C222_Department.Select(i => new {
                i.ID,
                i.DeptCode,
                i.DeptName,
                i.Deleted,
                i.Dept,
                i.SecID
            });
            return Request.CreateResponse(DataSourceLoader.Load(c222_department, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C222_Department();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            model.Dept = model.DeptCode;
            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));
            var result = _context.C222_Department.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.DeptCode);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C222_Department.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C222_Department not found");

            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            model.Dept = model.DeptCode;
            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete]
        public void Delete(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C222_Department.FirstOrDefault(item => item.ID == key);

            _context.C222_Department.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C222_Department model, IDictionary values) {
            string ID = nameof(C222_Department.ID);
            string DEPT_CODE = nameof(C222_Department.DeptCode);
            string DEPT_NAME = nameof(C222_Department.DeptName);
            string DELETED = nameof(C222_Department.Deleted);
            string DEPT = nameof(C222_Department.Dept);
            string SEC_ID = nameof(C222_Department.SecID);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(DEPT_CODE)) {
                model.DeptCode = Convert.ToString(values[DEPT_CODE]);
            }

            if(values.Contains(DEPT_NAME)) {
                model.DeptName = Convert.ToString(values[DEPT_NAME]);
            }

            if(values.Contains(DELETED)) {
                model.Deleted = Convert.ToBoolean(values[DELETED]);
            }

            if(values.Contains(DEPT)) {
                model.Dept = Convert.ToString(values[DEPT]);
            }

            if(values.Contains(SEC_ID)) {
                model.SecID = Convert.ToString(values[SEC_ID]);
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