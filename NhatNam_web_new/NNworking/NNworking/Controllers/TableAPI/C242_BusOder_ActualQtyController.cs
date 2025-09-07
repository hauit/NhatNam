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
    [Route("api/C242_BusOder_ActualQty/{action}", Name = "C242_BusOder_ActualQtyApi")]
    public class C242_BusOder_ActualQtyController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c242_busoder_actualqty = _context.C242_BusOder_ActualQty.OrderByDescending(x=>x.ID).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c242_busoder_actualqty, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C242_BusOder_ActualQty();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_BusOder_ActualQty.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_BusOder_ActualQty.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C242_BusOder_ActualQty not found");

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
            var model = _context.C242_BusOder_ActualQty.FirstOrDefault(item => item.ID == key);

            _context.C242_BusOder_ActualQty.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C242_BusOder_ActualQty model, IDictionary values) {
            string ID = nameof(C242_BusOder_ActualQty.ID);
            string BUS_ODER = nameof(C242_BusOder_ActualQty.BusOder);
            string QTY = nameof(C242_BusOder_ActualQty.Qty);
            string date = nameof(C242_BusOder_ActualQty.date);
            string dept = nameof(C242_BusOder_ActualQty.dept);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(BUS_ODER)) {
                model.BusOder = Convert.ToString(values[BUS_ODER]);
            }

            if(values.Contains(dept)) {
                model.dept = Convert.ToString(values[dept]);
            }

            if(values.Contains(date)) {
                model.date = Convert.ToDateTime(values[date]);
            }

            if(values.Contains(QTY)) {
                model.Qty = Convert.ToInt32(values[QTY]);
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