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
    [Route("api/C242_InventoryReceived/{action}", Name = "C242_InventoryReceivedApi")]
    public class C242_InventoryReceivedController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage GetInventoryReceived(DataSourceLoadOptions loadOptions) {
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            var partNo = queryParams["partNo"];
            var c242_inventoryreceived = _context.sp_242_InventoryReceived_GetAll(partNo).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c242_inventoryreceived, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage sp_242_Inventory_PlaceOfItem(DataSourceLoadOptions loadOptions)
        {
            var c242_inventoryreceived = _context.sp_242_Inventory_PlaceOfItem().ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c242_inventoryreceived, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage GetInventoryExport(DataSourceLoadOptions loadOptions)
        {
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            var partNo = queryParams["partNo"];
            var c242_inventoryreceived = _context.sp_242_InventoryExport_GetAll(partNo).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c242_inventoryreceived, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage GetInventory(DataSourceLoadOptions loadOptions)
        {
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            var c242_inventoryreceived = _context.sp_242_Inventory().ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c242_inventoryreceived, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage GetTrackingHistory(DataSourceLoadOptions loadOptions)
        {
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            var c242_inventoryreceived = _context.SP_242_Tracking().ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c242_inventoryreceived, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage GetAllPartner(DataSourceLoadOptions loadOptions)
        {
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            var c242_inventoryreceived = _context.sp_222_PartnerGetAll().ToList();/// _context.sp_222_PartnerGetAll().ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c242_inventoryreceived, loadOptions));

        }
        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C242_InventoryReceived();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            model.Date = DateTime.Now;
            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_InventoryReceived.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_InventoryReceived.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C242_InventoryReceived not found");

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
            var model = _context.C242_InventoryReceived.FirstOrDefault(item => item.ID == key);

            _context.C242_InventoryReceived.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C242_InventoryReceived model, IDictionary values) {
            string ID = nameof(C242_InventoryReceived.ID);
            string DATE = nameof(C242_InventoryReceived.Date);
            string STAFF_ID = nameof(C242_InventoryReceived.StaffID);
            string IMPORT = nameof(C242_InventoryReceived.Import);
            string RECEIVE_DEPT = nameof(C242_InventoryReceived.ReceiveDept);
            string NOTE = nameof(C242_InventoryReceived.Note);
            string DELETED = nameof(C242_InventoryReceived.Deleted);
            string IMPORT_FROM = nameof(C242_InventoryReceived.ImportFrom);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(DATE)) {
                model.Date = Convert.ToDateTime(values[DATE]);
            }

            if(values.Contains(STAFF_ID)) {
                model.StaffID = Convert.ToString(values[STAFF_ID]);
            }

            if(values.Contains(IMPORT)) {
                model.Import = Convert.ToBoolean(values[IMPORT]);
            }

            if(values.Contains(RECEIVE_DEPT)) {
                model.ReceiveDept = Convert.ToString(values[RECEIVE_DEPT]);
            }

            if(values.Contains(NOTE)) {
                model.Note = Convert.ToString(values[NOTE]);
            }

            if(values.Contains(DELETED)) {
                model.Deleted = Convert.ToBoolean(values[DELETED]);
            }

            if(values.Contains(IMPORT_FROM)) {
                model.ImportFrom = Convert.ToString(values[IMPORT_FROM]);
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