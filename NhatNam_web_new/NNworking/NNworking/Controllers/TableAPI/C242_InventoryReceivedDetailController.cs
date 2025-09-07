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
    [Route("api/C242_InventoryReceivedDetail/{action}", Name = "C242_InventoryReceivedDetailApi")]
    public class C242_InventoryReceivedDetailController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            int importID;
            if(!int.TryParse(queryParams["importID"],out importID))
            {
                importID = 0;
            }
            var c242_inventoryreceiveddetail = _context.C242_InventoryReceivedDetail.Where(x=>x.VoucherID == importID).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c242_inventoryreceiveddetail, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage GetKhoTPUnverify(DataSourceLoadOptions loadOptions) {
            var from = DateTime.Now.AddYears(-1).Date;
            var to = DateTime.Now.Date;
            var c242_inventoryreceiveddetail = _context.sp_242_GetKhoTPReceivedForVerification(from,to).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c242_inventoryreceiveddetail, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C242_InventoryReceivedDetail();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);
            CheckAndUpdateBusOrder(model);
            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_InventoryReceivedDetail.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_InventoryReceivedDetail.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C242_InventoryReceivedDetail not found");

            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);
            CheckAndUpdateBusOrder(model);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private void CheckAndUpdateBusOrder(C242_InventoryReceivedDetail model)
        {
            clsBase ba = new clsBase();
            ba.CheckAndUpdateBusOrder(model,_context);
        }

        [HttpDelete]
        public void Delete(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_InventoryReceivedDetail.FirstOrDefault(item => item.ID == key);

            _context.C242_InventoryReceivedDetail.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C242_InventoryReceivedDetail model, IDictionary values) {
            string ID = nameof(C242_InventoryReceivedDetail.ID);
            string VOUCHER_ID = nameof(C242_InventoryReceivedDetail.VoucherID);
            string PART_NO = nameof(C242_InventoryReceivedDetail.PartNo);
            string ORDER_NUMBER = nameof(C242_InventoryReceivedDetail.OrderNumber);
            string QTY = nameof(C242_InventoryReceivedDetail.Qty);
            string PRICE = nameof(C242_InventoryReceivedDetail.Price);
            string NOTE = nameof(C242_InventoryReceivedDetail.Note);
            string NguoiNhan = nameof(C242_InventoryReceivedDetail.NguoiNhan);
            string STATUS_ID = nameof(C242_InventoryReceivedDetail.StatusID);
            string DELETED = nameof(C242_InventoryReceivedDetail.Deleted);
            string GiaDe = nameof(C242_InventoryReceivedDetail.GiaDe);
            string ReceivedConfirm = nameof(C242_InventoryReceivedDetail.ReceivedConfirm);
            string ReceivedConfirmStaff = nameof(C242_InventoryReceivedDetail.ReceivedConfirmStaff);
            string ReceivedConfirmTime = nameof(C242_InventoryReceivedDetail.ReceivedConfirmTime);

            if (values.Contains(ReceivedConfirm))
            {
                model.ReceivedConfirm = Convert.ToBoolean(values[ReceivedConfirm]);
                if((bool)model.ReceivedConfirm)
                {
                    model.ReceivedConfirmTime = DateTime.Now;
                }
            }

            if (values.Contains(ReceivedConfirmStaff))
            {
                model.ReceivedConfirmStaff = Convert.ToString(values[ReceivedConfirmStaff]);
            }

            if (values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if (values.Contains(GiaDe))
            {
                model.GiaDe = Convert.ToString(values[GiaDe]);
            }

            if (values.Contains(VOUCHER_ID)) {
                model.VoucherID = Convert.ToInt32(values[VOUCHER_ID]);
            }

            if(values.Contains(PART_NO)) {
                model.PartNo = Convert.ToString(values[PART_NO]);
            }

            if(values.Contains(ORDER_NUMBER)) {
                if (string.IsNullOrEmpty(model.PartNo))
                {
                    model.PartNo = string.Empty;
                    model.OrderNumber = Convert.ToString(values[ORDER_NUMBER]);
                    var data = _context.View_242_BusOder.Where(x => x.BOderNo.ToLower() == model.OrderNumber.ToLower()).FirstOrDefault();
                    if (data != null)
                    {
                        model.PartNo = data.PartID;
                    }
                }
            }

            if(values.Contains(QTY)) {
                model.Qty = Convert.ToInt32(values[QTY]);
            }

            if(values.Contains(PRICE)) {
                model.Price = Convert.ToDecimal(values[PRICE]);
            }

            if(values.Contains(NOTE)) {
                model.Note = Convert.ToString(values[NOTE]);
            }

            if (values.Contains(NguoiNhan))
            {
                model.NguoiNhan = Convert.ToString(values[NguoiNhan]);
            }

            if (values.Contains(STATUS_ID)) {
                model.StatusID = Convert.ToInt32(values[STATUS_ID]);
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