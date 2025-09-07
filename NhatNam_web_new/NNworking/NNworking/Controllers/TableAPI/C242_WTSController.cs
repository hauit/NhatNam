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
    [Route("api/C242_WTS/{action}", Name = "C242_WTSApi")]
    public class C242_WTSController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage GetPlan(DataSourceLoadOptions loadOptions) {
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            var shift = queryParams["shift"];
            var machineID = queryParams["machineID"];
            var date = DateTime.ParseExact(queryParams["date"].Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture).Date;
            try
            {
                var a = _context.Sp_242_MachinePlanning_GetPlanforMachine(date, shift, machineID).ToList();
                return Request.CreateResponse(DataSourceLoader.Load(a, loadOptions));
            }
            catch(Exception ex)
            {

            }
            var c242_wts = _context.C242_WTS.Select(i => new {
                i.ID,
                i.IDPlan,
                i.Date,
                i.StaffID,
                i.Shift,
                i.MachineID,
                i.OptionID,
                i.MONo,
                i.WorkID,
                i.Time,
                i.OKQty,
                i.NGQty,
                i.Note,
                i.Deleted
            });
            return Request.CreateResponse(DataSourceLoader.Load(c242_wts, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage sp_242_WTS_GetUpdateTGTC(DataSourceLoadOptions loadOptions)
        {
            var a = _context.sp_242_WTS_GetUpdateTGTC().ToList();
            var obj = new sp_242_WTS_GetUpdateTGTC_Result() { PartID = "hau test", OptionID = "ASTG01", ProTime_New = 5.5 };
            a.Add(obj);
            var obj1 = new sp_242_WTS_GetUpdateTGTC_Result() { PartID = "hau test1", OptionID = "ASTG01", ProTime_New = 6.5 };
            a.Add(obj1);
            return Request.CreateResponse(DataSourceLoader.Load(a, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage DetailWTS(DataSourceLoadOptions loadOptions)
        {
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            var machineID = queryParams["staffName"];
            var staffID = queryParams["staffID"];
            DateTime date;
            if(!DateTime.TryParse(queryParams["date"],out date))
            {
                date = DateTime.ParseExact(queryParams["date"].Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture).Date;
            }
            var a = _context.View_242_WTS.Where(x=>x.Date == date && x.StaffID == staffID).ToList();
            if(string.IsNullOrEmpty(machineID))
            {
                a = _context.View_242_WTS.Where(x => x.Date == date && x.MachineID == staffID).ToList();
            }

            return Request.CreateResponse(DataSourceLoader.Load(a, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage sp_242_WTS_GetUpdateTGTC_Detail(DataSourceLoadOptions loadOptions)
        {
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            var partID = queryParams["partid"];
            var optionID = queryParams["OptionID"];
            var a = _context.sp_242_WTS_GetUpdateTGTC_Detail(partID,optionID).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(a, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage GetAllWTS(DataSourceLoadOptions loadOptions)
        {
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            var dateFrom = DateTime.ParseExact(queryParams["dateFrom"].Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture).Date;
            var dateTo = DateTime.ParseExact(queryParams["dateTo"].Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture).Date;
            var c242_wts = _context.C242_WTS.Where(x => x.Date >= dateFrom && x.Date <= dateTo && x.Status == true).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c242_wts, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage GetWTSForCheck(DataSourceLoadOptions loadOptions)
        {
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            DateTime fromDate = DateTime.Now.Date;
            DateTime toDate = DateTime.Now.Date;
            if (queryParams.ContainsKey("fromDate"))
            {
                fromDate = DateTime.ParseExact(queryParams["fromDate"].Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture).Date;
            }

            if (queryParams.ContainsKey("toDate"))
            {
                toDate = DateTime.ParseExact(queryParams["toDate"].Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture).Date;
            }
            
            var staffID = queryParams["staffID"];
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            //var data = db.C242_WTS.Where(x => x.StaffID == staffID && x.Date >= fromDate && x.Date <= toDate).ToList();
            var data = db.sp_242_WTS_GetTotalWTSForChecking(staffID, fromDate, toDate).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(data, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage GetDetailWTSForCheck(DataSourceLoadOptions loadOptions)
        {
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            DateTime fromDate = DateTime.Now.Date;
            DateTime toDate = DateTime.Now.Date;
            if (queryParams.ContainsKey("fromDate"))
            {
                fromDate = DateTime.ParseExact(queryParams["fromDate"].Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture).Date;
            }

            if (queryParams.ContainsKey("toDate"))
            {
                toDate = DateTime.ParseExact(queryParams["toDate"].Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture).Date;
            }

            var staffID = queryParams["staffID"];
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var data = db.sp_242_WTS_GetTotalWTSForChecking(staffID, fromDate, toDate).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(data, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage GetDetailWTSGT(DataSourceLoadOptions loadOptions)
        {
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            var shift = queryParams["shift"];
            DateTime date = DateTime.Now.Date;
            if (((DateTime.Now.Hour * 60) + DateTime.Now.Minute) <= 380)
            {
                date = date.AddDays(-1);
            }
            if(queryParams.ContainsKey("date"))
            {
                if(!DateTime.TryParse(queryParams["date"],out date))
                {
                    date = DateTime.ParseExact(queryParams["date"].Substring(0, 24),
                                  "ddd MMM d yyyy HH:mm:ss",
                                  System.Globalization.CultureInfo.InvariantCulture).Date;
                }
            }

            var staffID = queryParams["staffID"];
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var data = db.View_242_WTS.Where(x => x.StaffID == staffID && x.Date == date && x.Time != null).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(data, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage GetDetailWTS(DataSourceLoadOptions loadOptions)
        {
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            var shift = queryParams["shift"];
            var machineID = queryParams["machineID"];
            var staffID = queryParams["staffID"];
            var dept = queryParams["dept"];

            var date = DateTime.ParseExact(queryParams["date"].Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture).Date;
            var c242_wts = _context.View_242_WTS.Where(x => x.Date == date && x.Shift.ToLower() == shift.ToLower() && x.MachineID.ToLower() == machineID.ToLower()).ToList();
            if (dept.ToLower() != "xuong")
            {
                if(!string.IsNullOrEmpty(staffID))
                {
                    machineID = staffID;
                }

                c242_wts = _context.View_242_WTS.Where(x => x.Date == date && x.Shift.ToLower() == shift.ToLower() && x.StaffID.ToLower() == machineID.ToLower()).ToList();
            }
            return Request.CreateResponse(DataSourceLoader.Load(c242_wts, loadOptions));
        }


        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C242_WTS();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_WTS.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_WTS.FirstOrDefault(item => item.ID == key);
            if (model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C242_WTS not found");

            if ((bool)model.Status)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "WTS đã được duyệt, không thể sửa. Nếu có thay đổi vui lòng xóa đi nhập lại để quản lý kiểm soát được");
                //throw new ArgumentException("WTS đã được duyệt, không thể sửa. Nếu có thay đổi vui lòng xóa đi nhập lại để quản lý kiểm soát được");
            }

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
            var model = _context.C242_WTS.FirstOrDefault(item => item.ID == key);

            model.Deleted = true;
            //_context.C242_WTS.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C242_WTS model, IDictionary values) {
            string ID = nameof(C242_WTS.ID);
            string IDPLAN = nameof(C242_WTS.IDPlan);
            string DATE = nameof(C242_WTS.Date);
            string STAFF_ID = nameof(C242_WTS.StaffID);
            string SHIFT = nameof(C242_WTS.Shift);
            string MACHINE_ID = nameof(C242_WTS.MachineID);
            string OPTION_ID = nameof(C242_WTS.OptionID);
            string MONO = nameof(C242_WTS.MONo);
            string WORK_ID = nameof(C242_WTS.WorkID);
            string TIME = nameof(C242_WTS.Time);
            string ProTime = nameof(C242_WTS.ProTime);
            string ClampTime = nameof(C242_WTS.ClampTime);
            string OKQTY = nameof(C242_WTS.OKQty);
            string NGQTY = nameof(C242_WTS.NGQty);
            string NOTE = nameof(C242_WTS.Note);
            string DELETED = nameof(C242_WTS.Deleted);

            if (values.Contains(ProTime))
            {
                model.ProTime = Convert.ToDouble(values[ProTime]);
            }

            if (values.Contains(ClampTime))
            {
                model.ClampTime = Convert.ToDouble(values[ClampTime]);
            }

            if (values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(IDPLAN)) {
                model.IDPlan = Convert.ToInt32(values[IDPLAN]);
            }

            if(values.Contains(DATE)) {
                model.Date = Convert.ToDateTime(values[DATE]);
            }

            if(values.Contains(STAFF_ID)) {
                model.StaffID = Convert.ToString(values[STAFF_ID]);
            }

            if(values.Contains(SHIFT)) {
                model.Shift = Convert.ToString(values[SHIFT]);
            }

            if(values.Contains(MACHINE_ID)) {
                model.MachineID = Convert.ToString(values[MACHINE_ID]);
            }

            if(values.Contains(OPTION_ID)) {
                model.OptionID = Convert.ToString(values[OPTION_ID]);
            }

            if(values.Contains(MONO)) {
                model.MONo = Convert.ToString(values[MONO]);
            }

            if(values.Contains(WORK_ID)) {
                model.WorkID = Convert.ToString(values[WORK_ID]);
            }

            if(values.Contains(TIME)) {
                model.Time = Convert.ToDecimal(values[TIME]);
            }

            if(values.Contains(OKQTY)) {
                model.OKQty = Convert.ToInt32(values[OKQTY]);
            }

            if(values.Contains(NGQTY)) {
                model.NGQty = Convert.ToInt32(values[NGQTY]);
            }

            if(values.Contains(NOTE)) {
                model.Note = Convert.ToString(values[NOTE]);
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