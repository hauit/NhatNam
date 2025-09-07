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
    [Route("api/C222_Staff/{action}", Name = "C222_StaffApi")]
    public class C222_StaffController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c222_staff = _context.C222_Staff.ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c222_staff, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C222_Staff();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C222_Staff.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C222_Staff.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C222_Staff not found");

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
            var model = _context.C222_Staff.FirstOrDefault(item => item.ID == key);

            _context.C222_Staff.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C222_Staff model, IDictionary values) {
            string ID = nameof(C222_Staff.ID);
            string STAFF_ID = nameof(C222_Staff.StaffID);
            string STAFF_NAME = nameof(C222_Staff.StaffName);
            string BIRTHDAY = nameof(C222_Staff.Birthday);
            string HIRE_DATE = nameof(C222_Staff.HireDate);
            string STOP_DATE = nameof(C222_Staff.StopDate);
            string SEC_ID = nameof(C222_Staff.SecID);
            string SEC_NAME = nameof(C222_Staff.SecName);
            string GROUP_ID = nameof(C222_Staff.GroupID);
            string SUB_GROUP = nameof(C222_Staff.Sub_Group);
            string PERSONAL_INF = nameof(C222_Staff.PersonalInf);
            string SEX = nameof(C222_Staff.Sex);
            string DEPARTMENT_ID = nameof(C222_Staff.DepartmentID);
            string PHOTO = nameof(C222_Staff.Photo);
            string EDUCATION = nameof(C222_Staff.Education);
            string PHONE_NUMBER = nameof(C222_Staff.PhoneNumber);
            string ADDRESS = nameof(C222_Staff.Address);
            string LEVEL = nameof(C222_Staff.level);
            string STAFF_NOTE = nameof(C222_Staff.StaffNote);
            string BO_PHAN = nameof(C222_Staff.BoPhan);
            string VITRI = nameof(C222_Staff.VITRI);
            string TT_GT = nameof(C222_Staff.TT_GT);
            string EMAIL = nameof(C222_Staff.Email);
            string EMAIL_HC = nameof(C222_Staff.EmailHC);
            string NGDUYET = nameof(C222_Staff.ngduyet);
            string NGDUYET2 = nameof(C222_Staff.ngduyet2);
            string NGDUYET3 = nameof(C222_Staff.ngduyet3);
            string BUS_STATION = nameof(C222_Staff.BusStation);
            string BUS_STATION_ORIGINAL = nameof(C222_Staff.BusStation_Original);
            string STATUS = nameof(C222_Staff.Status);
            string CREATE_DATE = nameof(C222_Staff.CreateDate);
            string STOP_DATE1 = nameof(C222_Staff.StopDate1);
            string NUMBER = nameof(C222_Staff.Number);
            string EMAIL_SOFT = nameof(C222_Staff.EmailSoft);
            string DELETED = nameof(C222_Staff.Deleted);
            string DeptCode = nameof(C222_Staff.DeptCode);

            if (values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if (values.Contains(DeptCode))
            {
                model.DeptCode = Convert.ToString(values[DeptCode]);
            }

            if (values.Contains(STAFF_ID)) {
                model.StaffID = Convert.ToString(values[STAFF_ID]);
            }

            if(values.Contains(STAFF_NAME)) {
                model.StaffName = Convert.ToString(values[STAFF_NAME]);
            }

            if(values.Contains(BIRTHDAY)) {
                model.Birthday = Convert.ToDateTime(values[BIRTHDAY]);
            }

            if(values.Contains(HIRE_DATE)) {
                model.HireDate = Convert.ToDateTime(values[HIRE_DATE]);
            }

            if(values.Contains(STOP_DATE)) {
                model.StopDate = Convert.ToDateTime(values[STOP_DATE]);
            }

            if(values.Contains(SEC_ID)) {
                model.SecID = Convert.ToString(values[SEC_ID]);
            }

            if(values.Contains(SEC_NAME)) {
                model.SecName = Convert.ToString(values[SEC_NAME]);
            }

            if(values.Contains(GROUP_ID)) {
                model.GroupID = Convert.ToString(values[GROUP_ID]);
            }

            if(values.Contains(SUB_GROUP)) {
                model.Sub_Group = Convert.ToString(values[SUB_GROUP]);
            }

            if(values.Contains(PERSONAL_INF)) {
                model.PersonalInf = Convert.ToString(values[PERSONAL_INF]);
            }

            if(values.Contains(SEX)) {
                model.Sex = Convert.ToBoolean(values[SEX]);
            }

            if(values.Contains(DEPARTMENT_ID)) {
                model.DepartmentID = Convert.ToString(values[DEPARTMENT_ID]);
            }

            if(values.Contains(PHOTO)) {
                model.Photo = Convert.ToString(values[PHOTO]);
            }

            if(values.Contains(EDUCATION)) {
                model.Education = Convert.ToString(values[EDUCATION]);
            }

            if(values.Contains(PHONE_NUMBER)) {
                model.PhoneNumber = Convert.ToString(values[PHONE_NUMBER]);
            }

            if(values.Contains(ADDRESS)) {
                model.Address = Convert.ToString(values[ADDRESS]);
            }

            if(values.Contains(LEVEL)) {
                model.level = Convert.ToInt32(values[LEVEL]);
            }

            if(values.Contains(STAFF_NOTE)) {
                model.StaffNote = Convert.ToString(values[STAFF_NOTE]);
            }

            if(values.Contains(BO_PHAN)) {
                model.BoPhan = Convert.ToString(values[BO_PHAN]);
            }

            if(values.Contains(VITRI)) {
                model.VITRI = Convert.ToString(values[VITRI]);
            }

            if(values.Contains(TT_GT)) {
                model.TT_GT = Convert.ToString(values[TT_GT]);
            }

            if(values.Contains(EMAIL)) {
                model.Email = Convert.ToString(values[EMAIL]);
            }

            if(values.Contains(EMAIL_HC)) {
                model.EmailHC = Convert.ToString(values[EMAIL_HC]);
            }

            if(values.Contains(NGDUYET)) {
                model.ngduyet = Convert.ToString(values[NGDUYET]);
            }

            if(values.Contains(NGDUYET2)) {
                model.ngduyet2 = Convert.ToString(values[NGDUYET2]);
            }

            if(values.Contains(NGDUYET3)) {
                model.ngduyet3 = Convert.ToString(values[NGDUYET3]);
            }

            if(values.Contains(BUS_STATION)) {
                model.BusStation = Convert.ToString(values[BUS_STATION]);
            }

            if(values.Contains(BUS_STATION_ORIGINAL)) {
                model.BusStation_Original = Convert.ToString(values[BUS_STATION_ORIGINAL]);
            }

            if(values.Contains(STATUS)) {
                model.Status = Convert.ToBoolean(values[STATUS]);
            }

            if(values.Contains(CREATE_DATE)) {
                model.CreateDate = Convert.ToDateTime(values[CREATE_DATE]);
            }

            if(values.Contains(STOP_DATE1)) {
                model.StopDate1 = Convert.ToDateTime(values[STOP_DATE1]);
            }

            if(values.Contains(NUMBER)) {
                model.Number = Convert.ToInt32(values[NUMBER]);
            }

            if(values.Contains(EMAIL_SOFT)) {
                model.EmailSoft = Convert.ToString(values[EMAIL_SOFT]);
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