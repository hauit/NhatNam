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
    [Route("api/C242_InventoryInputHistory/{action}", Name = "C242_InventoryInputHistoryApi")]
    public class C242_InventoryInputHistoryController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c242_inventoryinputhistory = _context.C242_InventoryInputHistory.Select(i => new {
                i.ID,
                i.Date,
                i.OrderNo,
                i.Qty,
                i.GiaDe,
                i.Note
            });
            return Request.CreateResponse(DataSourceLoader.Load(c242_inventoryinputhistory, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C242_InventoryInputHistory();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_InventoryInputHistory.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_InventoryInputHistory.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C242_InventoryInputHistory not found");

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
            var model = _context.C242_InventoryInputHistory.FirstOrDefault(item => item.ID == key);

            _context.C242_InventoryInputHistory.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C242_InventoryInputHistory model, IDictionary values) {
            string ID = nameof(C242_InventoryInputHistory.ID);
            string DATE = nameof(C242_InventoryInputHistory.Date);
            string ORDER_NO = nameof(C242_InventoryInputHistory.OrderNo);
            string QTY = nameof(C242_InventoryInputHistory.Qty);
            string GIA_DE = nameof(C242_InventoryInputHistory.GiaDe);
            string NOTE = nameof(C242_InventoryInputHistory.Note);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(DATE)) {
                model.Date = Convert.ToDateTime(values[DATE]);
            }

            if(values.Contains(ORDER_NO)) {
                model.OrderNo = Convert.ToString(values[ORDER_NO]);
            }

            if(values.Contains(QTY)) {
                model.Qty = Convert.ToInt32(values[QTY]);
            }

            if(values.Contains(GIA_DE)) {
                model.GiaDe = Convert.ToString(values[GIA_DE]);
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