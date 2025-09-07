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
    [Route("api/C242_BusOder/{action}", Name = "C242_BusOderApi")]
    public class C242_BusOderController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c242_busoder = _context.View_242_BusOder.Select(i => new {
                i.ID,
                i.BOderNo,
                i.Date,
                i.PlanNo,
                i.PartID,
                i.Qty,
                i.Deadline,
                i.RawQty,
                i.HelisertQty,
                i.BlastQty,
                i.MONo,
                i.MOQty,
                i.Started,
                i.Finished,
                i.FinishDate,
                i.Change,
                i.ChangeDate,
                i.Imported,
                i.ImportFrom,
                i.Note,
                i.CuttingOrder,
                i.CuttingStatus,
                i.Deleted,
                i.OrderGoc,
                i.OrderCat
            });
            return Request.CreateResponse(DataSourceLoader.Load(c242_busoder, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage GetByMonth(DataSourceLoadOptions loadOptions)
        {
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            string month = queryParams["month"];

            var c242_busoder = _context.sp_242_BusOder_GetByMonth(month).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c242_busoder, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C242_BusOder();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_BusOder.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_BusOder.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C242_BusOder not found");

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
            var model = _context.C242_BusOder.FirstOrDefault(item => item.ID == key);

            _context.C242_BusOder.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C242_BusOder model, IDictionary values) {
            string ID = nameof(C242_BusOder.ID);
            string BODER_NO = nameof(C242_BusOder.BOderNo);
            string DATE = nameof(C242_BusOder.Date);
            string PLAN_NO = nameof(C242_BusOder.PlanNo);
            string PART_ID = nameof(C242_BusOder.PartID);
            string QTY = nameof(C242_BusOder.Qty);
            string DEADLINE = nameof(C242_BusOder.Deadline);
            string RAW_QTY = nameof(C242_BusOder.RawQty);
            string HELISERT_QTY = nameof(C242_BusOder.HelisertQty);
            string BLAST_QTY = nameof(C242_BusOder.BlastQty);
            string MONO = nameof(C242_BusOder.MONo);
            string MOQTY = nameof(C242_BusOder.MOQty);
            string STARTED = nameof(C242_BusOder.Started);
            string FINISHED = nameof(C242_BusOder.Finished);
            string FINISH_DATE = nameof(C242_BusOder.FinishDate);
            string CHANGE = nameof(C242_BusOder.Change);
            string CHANGE_DATE = nameof(C242_BusOder.ChangeDate);
            string IMPORTED = nameof(C242_BusOder.Imported);
            string IMPORT_FROM = nameof(C242_BusOder.ImportFrom);
            string NOTE = nameof(C242_BusOder.Note);
            string CUTTING_ORDER = nameof(C242_BusOder.CuttingOrder);
            string CUTTING_STATUS = nameof(C242_BusOder.CuttingStatus);
            string DELETED = nameof(C242_BusOder.Deleted);
            string ORDER_GOC = nameof(C242_BusOder.OrderGoc);
            string ORDER_CAT = nameof(C242_BusOder.OrderCat);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(BODER_NO)) {
                model.BOderNo = Convert.ToString(values[BODER_NO]);
            }

            if(values.Contains(DATE)) {
                model.Date = Convert.ToDateTime(values[DATE]);
            }

            if(values.Contains(PLAN_NO)) {
                model.PlanNo = Convert.ToString(values[PLAN_NO]);
            }

            if(values.Contains(PART_ID)) {
                model.PartID = Convert.ToString(values[PART_ID]);
            }

            if(values.Contains(QTY)) {
                model.Qty = Convert.ToInt32(values[QTY]);
            }

            if(values.Contains(DEADLINE)) {
                model.Deadline = Convert.ToDateTime(values[DEADLINE]);
            }

            if(values.Contains(RAW_QTY)) {
                model.RawQty = Convert.ToInt32(values[RAW_QTY]);
            }

            if(values.Contains(HELISERT_QTY)) {
                model.HelisertQty = Convert.ToInt32(values[HELISERT_QTY]);
            }

            if(values.Contains(BLAST_QTY)) {
                model.BlastQty = Convert.ToInt32(values[BLAST_QTY]);
            }

            if(values.Contains(MONO)) {
                model.MONo = Convert.ToString(values[MONO]);
            }

            if(values.Contains(MOQTY)) {
                model.MOQty = Convert.ToInt32(values[MOQTY]);
            }

            if(values.Contains(STARTED)) {
                model.Started = Convert.ToBoolean(values[STARTED]);
            }

            if(values.Contains(FINISHED)) {
                model.Finished = Convert.ToBoolean(values[FINISHED]);
            }

            if(values.Contains(FINISH_DATE)) {
                model.FinishDate = Convert.ToDateTime(values[FINISH_DATE]);
            }

            if(values.Contains(CHANGE)) {
                model.Change = Convert.ToString(values[CHANGE]);
            }

            if(values.Contains(CHANGE_DATE)) {
                model.ChangeDate = Convert.ToDateTime(values[CHANGE_DATE]);
            }

            if(values.Contains(IMPORTED)) {
                model.Imported = Convert.ToBoolean(values[IMPORTED]);
            }

            if(values.Contains(IMPORT_FROM)) {
                model.ImportFrom = Convert.ToString(values[IMPORT_FROM]);
            }

            if(values.Contains(NOTE)) {
                model.Note = Convert.ToString(values[NOTE]);
            }

            if(values.Contains(CUTTING_ORDER)) {
                model.CuttingOrder = Convert.ToString(values[CUTTING_ORDER]);
            }

            if(values.Contains(CUTTING_STATUS)) {
                model.CuttingStatus = Convert.ToBoolean(values[CUTTING_STATUS]);
            }

            if(values.Contains(DELETED)) {
                model.Deleted = Convert.ToBoolean(values[DELETED]);
            }

            if(values.Contains(ORDER_GOC)) {
                model.OrderGoc = Convert.ToString(values[ORDER_GOC]);
            }

            if(values.Contains(ORDER_CAT)) {
                model.OrderCat = Convert.ToString(values[ORDER_CAT]);
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