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
    [Route("api/C242_ToolBorrow/{action}", Name = "C242_ToolBorrowApi")]
    public class C242_ToolBorrowController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c242_toolborrow = _context.C242_ToolBorrow.Where(x => x.Acception == false || x.Acception == null).OrderByDescending(x=>x.InputDate).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c242_toolborrow, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C242_ToolBorrow();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_ToolBorrow.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_ToolBorrow.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C242_ToolBorrow not found");

            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            var notify = new WorkingNotifycation();
            if ((bool)model.Acception)
            {
                notify = NotifyToMachineAfterAcception(model);
            }

            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            try
            {
                _context.SaveChanges();
            }
            catch(Exception ex)
            {

            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private WorkingNotifycation NotifyToMachineAfterAcception(C242_ToolBorrow model)
        {
            ////Thêm thông báo yêu cầu mượn dao
            var notify = new WorkingNotifycation();
            notify.FromClient = "1556";
            notify.FromDevice = "";
            notify.ToDevice = string.Empty;
            notify.ToClient = model.MachineID;
            notify.NotifyContent = $@"Đồng ý yêu cầu mượn dao máy: {model.MachineID}, chi tiết: {model.PartNo}, nguyên công: {model.OptionID}, thời điểm có dao: {model.AvailableTime}";
            notify.WorkTopic = "MuonDao";
            notify.NotifyCaption = $"Chấp nhận yêu cầu mượn dao";
            notify.NotifyTime = DateTime.Now;
            _context.WorkingNotifycations.Add(notify);
            return notify;
        }

        [HttpDelete]
        public void Delete(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_ToolBorrow.FirstOrDefault(item => item.ID == key);

            _context.C242_ToolBorrow.Remove(model);
            _context.SaveChanges();
        }
        
        private void PopulateModel(C242_ToolBorrow model, IDictionary values) {
            string ID = nameof(C242_ToolBorrow.ID);
            string DATE = nameof(C242_ToolBorrow.Date);
            string SHIFT = nameof(C242_ToolBorrow.Shift);
            string MACHINE_ID = nameof(C242_ToolBorrow.MachineID);
            string PART_NO = nameof(C242_ToolBorrow.PartNo);
            string OPTION_ID = nameof(C242_ToolBorrow.OptionID);
            string TIME_NEED_TO_GET = nameof(C242_ToolBorrow.TimeNeedToGet);
            string INPUT_DATE = nameof(C242_ToolBorrow.InputDate);
            string ACCEPTION = nameof(C242_ToolBorrow.Acception);
            string AVAILABLE_TIME = nameof(C242_ToolBorrow.AvailableTime);
            string Gotten = nameof(C242_ToolBorrow.Gotten);

            if (values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if (values.Contains(Gotten))
            {
                model.Gotten = Convert.ToBoolean(values[Gotten]);
            }

            if (values.Contains(DATE)) {
                model.Date = Convert.ToDateTime(values[DATE]);
            }

            if(values.Contains(SHIFT)) {
                model.Shift = Convert.ToString(values[SHIFT]);
            }

            if(values.Contains(MACHINE_ID)) {
                model.MachineID = Convert.ToString(values[MACHINE_ID]);
            }

            if(values.Contains(PART_NO)) {
                model.PartNo = Convert.ToString(values[PART_NO]);
            }

            if(values.Contains(OPTION_ID)) {
                model.OptionID = Convert.ToString(values[OPTION_ID]);
            }

            if(values.Contains(TIME_NEED_TO_GET)) {
                model.TimeNeedToGet = Convert.ToDateTime(values[TIME_NEED_TO_GET]);
            }

            if(values.Contains(INPUT_DATE)) {
                model.InputDate = Convert.ToDateTime(values[INPUT_DATE]);
            }

            if(values.Contains(ACCEPTION)) {
                model.Acception = Convert.ToBoolean(values[ACCEPTION]);
            }

            if(values.Contains(AVAILABLE_TIME)) {
                model.AvailableTime = Convert.ToDateTime(values[AVAILABLE_TIME]);
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