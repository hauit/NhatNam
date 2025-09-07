using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using NNworking.Models.ObjectBase;
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
    [Route("api/C242_ErrorItemNotify_New/{action}", Name = "C242_ErrorItemNotify_NewApi")]
    public class C242_ErrorItemNotify_NewController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c242_erroritemnotify_new = _context.C242_ErrorItemNotify_View.OrderByDescending(x=>x.ID).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c242_erroritemnotify_new, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage GetNotify(DataSourceLoadOptions loadOptions) {
            var c242_erroritemnotify_new = _context.C242_ErrorItemNotify_View.ToList().OrderByDescending(x=>x.ID);
            return Request.CreateResponse(DataSourceLoader.Load(c242_erroritemnotify_new, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C242_ErrorItemNotify_New();
            IObjectBase objectBase = new ErrorItemNotifyObjectBase();
            var obj1 = (object)model;
            objectBase.SetDefaultValue(ref obj1);
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_ErrorItemNotify_New.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        private void SetDefaultValue(ref C242_ErrorItemNotify_New model)
        {
            model.Customer = string.Empty;
            model.DateRaiseErr = DateTime.Now;
            model.DateWrite = DateTime.Now;
            model.DeceidedTime = 0;
            model.DecisionToFine = false;
            model.ErrorCause = string.Empty;
            model.ErrorContent = 0;
            model.ErrorDes = string.Empty;
            model.ErrorHappenFrequency = 0;
            model.ErrorNo = string.Empty;
            model.ErrorProcess = 0;
            model.ErrorQty = 0;
            model.ErrorType = string.Empty;
            model.ID = 0;
            model.ManagerRemedies = string.Empty;
            model.NotifyDept = string.Empty;
            model.NotifyStaff = string.Empty;
            model.NotPenalizeDes = string.Empty;
            model.OptionID = string.Empty;
            model.OrderNo = string.Empty;
            model.PartID = string.Empty;
            model.PredictErrorCause = 0;
            model.Qty = 0;
            model.RaiseErrorDept = string.Empty;
            model.RaiseErrorStaff = string.Empty;
            model.Remedies = string.Empty;
            model.Remedies1 = string.Empty;
            model.Remedies2 = string.Empty;
            model.Supplier = string.Empty;
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            //List<string> pq = new List<string>() { "0038", "0637" };

            string staffID = Convert.ToString(form.Get("extraParam"));
            var per = _context.View_222_Staff.Where(x => x.StaffID == staffID && x.ErrorNoteEdit == true).Any();
            if (!per)
            {
                 return Request.CreateResponse(HttpStatusCode.Conflict, "bạn không có quyền sửa dữ liệu");
            }

            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_ErrorItemNotify_New.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C242_ErrorItemNotify_New not found");

            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);
            
            //Nếu được update bởi bộ phận gây lỗi thì kiểm tra phần xử phạt
            if(!model.DecisionToFine && string.IsNullOrEmpty(model.NotPenalizeDes)) 
                return Request.CreateResponse(HttpStatusCode.Conflict, "Không xử lý phạt thì phải có lí do");

            //Nếu nhập NN đối sách thì phải nhập cách xử lý
            bool nnds_Null = string.IsNullOrEmpty(model.Remedies) && string.IsNullOrEmpty(model.Remedies1) && string.IsNullOrEmpty(model.Remedies2);
            if(!nnds_Null && string.IsNullOrEmpty(model.NotPenalizeDes)) 
                return Request.CreateResponse(HttpStatusCode.Conflict, "Đã nhập NN-ĐS thì phải nhập cách xử lý");

            if (model.NotPenalizeDes == "inputUpdate")
            {
                model.NotPenalizeDes = string.Empty;
            }

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete]
        public HttpResponseMessage Delete(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            List<string> pq = new List<string>() { "0038", "0637" };
            string staffID = Convert.ToString(form.Get("extraParam"));
            var per = pq.Where(x => x == staffID).Any();
            if (!per)
            {
                 return Request.CreateResponse(HttpStatusCode.Conflict, "bạn không có quyền xóa dữ liệu");
            }

            //var machine = _context.C222_Staff.Where(x => x.StaffID.ToLower() == staffID.ToLower() && x.StaffName.ToLower() == staffID.ToLower()).Any();
            //if (machine)
            //{
            //     return Request.CreateResponse(HttpStatusCode.Conflict, "Vui lòng sử dùng đúng tài khoản của mình để xóa dữ liệu");
            //}
            var model = _context.C242_ErrorItemNotify_New.FirstOrDefault(item => item.ID == key);
            model.UpdateStaff =  Convert.ToString(form.Get("extraParam"));
            model.Deleted = true;

            //_context.C242_ErrorItemNotify_New.Remove(model);
            _context.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK);
        }


        private void PopulateModel(C242_ErrorItemNotify_New model, IDictionary values) {
            string ID = nameof(C242_ErrorItemNotify_New.ID);
            string ERROR_NO = nameof(C242_ErrorItemNotify_New.ErrorNo);
            string DATE_RAISE_ERR = nameof(C242_ErrorItemNotify_New.DateRaiseErr);
            string DATE_WRITE = nameof(C242_ErrorItemNotify_New.DateWrite);
            string NOTIFY_DEPT = nameof(C242_ErrorItemNotify_New.NotifyDept);
            string RAISE_ERROR_DEPT = nameof(C242_ErrorItemNotify_New.RaiseErrorDept);
            string NOTIFY_STAFF = nameof(C242_ErrorItemNotify_New.NotifyStaff);
            string SUPPLIER = nameof(C242_ErrorItemNotify_New.Supplier);
            string RAISE_ERROR_STAFF = nameof(C242_ErrorItemNotify_New.RaiseErrorStaff);
            string CUSTOMER = nameof(C242_ErrorItemNotify_New.Customer);
            string ERROR_TYPE = nameof(C242_ErrorItemNotify_New.ErrorType);
            string ORDER_NO = nameof(C242_ErrorItemNotify_New.OrderNo);
            string PART_ID = nameof(C242_ErrorItemNotify_New.PartID);
            string QTY = nameof(C242_ErrorItemNotify_New.Qty);
            string ERROR_QTY = nameof(C242_ErrorItemNotify_New.ErrorQty);
            string OPTION_ID = nameof(C242_ErrorItemNotify_New.OptionID);
            string ERROR_DES = nameof(C242_ErrorItemNotify_New.ErrorDes);
            string DECISION_TO_FINE = nameof(C242_ErrorItemNotify_New.DecisionToFine);
            string NOT_PENALIZE_DES = nameof(C242_ErrorItemNotify_New.NotPenalizeDes);
            string ERROR_PROCESS = nameof(C242_ErrorItemNotify_New.ErrorProcess);
            string ERROR_CAUSE = nameof(C242_ErrorItemNotify_New.ErrorCause);
            string REMEDIES = nameof(C242_ErrorItemNotify_New.Remedies);
            string REMEDIES1 = nameof(C242_ErrorItemNotify_New.Remedies1);
            string REMEDIES2 = nameof(C242_ErrorItemNotify_New.Remedies2);
            string MANAGER_REMEDIES = nameof(C242_ErrorItemNotify_New.ManagerRemedies);
            string ERROR_CONTENT = nameof(C242_ErrorItemNotify_New.ErrorContent);
            string DECEIDED_TIME = nameof(C242_ErrorItemNotify_New.DeceidedTime);
            string PREDICT_ERROR_CAUSE = nameof(C242_ErrorItemNotify_New.PredictErrorCause);
            string ERROR_HAPPEN_FREQUENCY = nameof(C242_ErrorItemNotify_New.ErrorHappenFrequency);
            string PIC = nameof(C242_ErrorItemNotify_New.PIC);
            string Completed = nameof(C242_ErrorItemNotify_New.Completed);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(PIC)) {
                model.PIC = Convert.ToString(values[PIC]);
            }

            if(values.Contains(Completed)) {
                model.Completed = Convert.ToBoolean(values[Completed]);
            }

            if(values.Contains(ERROR_NO)) {
                model.ErrorNo = Convert.ToString(values[ERROR_NO]);
            }

            if(values.Contains(DATE_RAISE_ERR)) {
                model.DateRaiseErr = Convert.ToDateTime(values[DATE_RAISE_ERR]);
            }

            if(values.Contains(DATE_WRITE)) {
                model.DateWrite = Convert.ToDateTime(values[DATE_WRITE]);
            }

            if(values.Contains(NOTIFY_DEPT)) {
                model.NotifyDept = Convert.ToString(values[NOTIFY_DEPT]);
            }

            if(values.Contains(RAISE_ERROR_DEPT)) {
                model.RaiseErrorDept = Convert.ToString(values[RAISE_ERROR_DEPT]);
            }

            if(values.Contains(NOTIFY_STAFF)) {
                model.NotifyStaff = Convert.ToString(values[NOTIFY_STAFF]);
            }

            if(values.Contains(SUPPLIER)) {
                model.Supplier = Convert.ToString(values[SUPPLIER]);
            }

            if(values.Contains(RAISE_ERROR_STAFF)) {
                model.RaiseErrorStaff = Convert.ToString(values[RAISE_ERROR_STAFF]);
            }

            if(values.Contains(CUSTOMER)) {
                model.Customer = Convert.ToString(values[CUSTOMER]);
            }

            if(values.Contains(ERROR_TYPE)) {
                model.ErrorType = Convert.ToString(values[ERROR_TYPE]);
            }

            if(values.Contains(ORDER_NO)) {
                model.OrderNo = Convert.ToString(values[ORDER_NO]);
            }

            if(values.Contains(PART_ID)) {
                model.PartID = Convert.ToString(values[PART_ID]);
            }

            if(values.Contains(QTY)) {
                model.Qty = Convert.ToInt32(values[QTY]);
            }

            if(values.Contains(ERROR_QTY)) {
                model.ErrorQty = Convert.ToInt32(values[ERROR_QTY]);
            }

            if(values.Contains(OPTION_ID)) {
                model.OptionID = Convert.ToString(values[OPTION_ID]);
            }

            if(values.Contains(ERROR_DES)) {
                model.ErrorDes = Convert.ToString(values[ERROR_DES]);
            }

            if(values.Contains(DECISION_TO_FINE)) {
                model.DecisionToFine = Convert.ToBoolean(values[DECISION_TO_FINE]);
            }

            if(values.Contains(NOT_PENALIZE_DES)) {
                model.NotPenalizeDes = Convert.ToString(values[NOT_PENALIZE_DES]);
            }

            if(values.Contains(ERROR_PROCESS)) {
                model.ErrorProcess = Convert.ToInt32(values[ERROR_PROCESS]);
            }

            if(values.Contains(ERROR_CAUSE)) {
                model.ErrorCause = Convert.ToString(values[ERROR_CAUSE]);
            }

            if(values.Contains(REMEDIES)) {
                model.Remedies = Convert.ToString(values[REMEDIES]);
            }

            if(values.Contains(REMEDIES1)) {
                model.Remedies1 = Convert.ToString(values[REMEDIES1]);
            }

            if(values.Contains(REMEDIES2)) {
                model.Remedies2 = Convert.ToString(values[REMEDIES2]);
            }

            if(values.Contains(MANAGER_REMEDIES)) {
                model.ManagerRemedies = Convert.ToString(values[MANAGER_REMEDIES]);
            }

            if(values.Contains(ERROR_CONTENT)) {
                model.ErrorContent = Convert.ToInt32(values[ERROR_CONTENT]);
            }

            if(values.Contains(DECEIDED_TIME)) {
                model.DeceidedTime = Convert.ToInt32(values[DECEIDED_TIME]);
            }

            if(values.Contains(PREDICT_ERROR_CAUSE)) {
                model.PredictErrorCause = Convert.ToInt32(values[PREDICT_ERROR_CAUSE]);
            }

            if(values.Contains(ERROR_HAPPEN_FREQUENCY)) {
                model.ErrorHappenFrequency = Convert.ToInt32(values[ERROR_HAPPEN_FREQUENCY]);
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