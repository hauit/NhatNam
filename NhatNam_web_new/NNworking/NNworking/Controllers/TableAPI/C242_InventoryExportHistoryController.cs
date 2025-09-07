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
    [Route("api/C242_InventoryExportHistory/{action}", Name = "C242_InventoryExportHistoryApi")]
    public class C242_InventoryExportHistoryController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c242_inventoryexporthistory = _context.C242_InventoryExportHistory.Select(i => new {
                i.ID,
                i.Date,
                i.ExportOrder,
                i.SharedOrder,
                i.SharedQty,
                i.Note
            });
            return Request.CreateResponse(DataSourceLoader.Load(c242_inventoryexporthistory, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C242_InventoryExportHistory();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_InventoryExportHistory.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_InventoryExportHistory.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C242_InventoryExportHistory not found");

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
            var model = _context.C242_InventoryExportHistory.FirstOrDefault(item => item.ID == key);

            _context.C242_InventoryExportHistory.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C242_InventoryExportHistory model, IDictionary values) {
            string ID = nameof(C242_InventoryExportHistory.ID);
            string DATE = nameof(C242_InventoryExportHistory.Date);
            string EXPORT_ORDER = nameof(C242_InventoryExportHistory.ExportOrder);
            string SHARED_ORDER = nameof(C242_InventoryExportHistory.SharedOrder);
            string SHARED_QTY = nameof(C242_InventoryExportHistory.SharedQty);
            string NOTE = nameof(C242_InventoryExportHistory.Note);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(DATE)) {
                model.Date = Convert.ToDateTime(values[DATE]);
            }

            if(values.Contains(EXPORT_ORDER)) {
                model.ExportOrder = Convert.ToString(values[EXPORT_ORDER]);
            }

            if(values.Contains(SHARED_ORDER)) {
                model.SharedOrder = Convert.ToString(values[SHARED_ORDER]);
            }

            if(values.Contains(SHARED_QTY)) {
                model.SharedQty = Convert.ToInt32(values[SHARED_QTY]);
            }

            if(values.Contains(NOTE)) {
                model.Note = Convert.ToString(values[NOTE]);
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