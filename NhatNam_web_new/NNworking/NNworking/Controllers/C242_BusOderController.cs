using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using NNworking.Models;

namespace NNworking.Controllers
{
    [Route("api/C242_BusOder/{action}", Name = "C242_BusOderApi")]
    public class C242_BusOderController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public async Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions) {
            var c242_busoder = _context.View_242_BusOder.Where(m => m.Deleted == false);
            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ID" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Request.CreateResponse(await DataSourceLoader.LoadAsync(c242_busoder, loadOptions));
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post(FormDataCollection form) {
            var model = new C242_BusOder();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_BusOder.Add(model);
            await _context.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Created, new { result.ID });
        }

        [HttpPut]
        public async Task<HttpResponseMessage> Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = await _context.C242_BusOder.FirstOrDefaultAsync(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "Object not found");

            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete]
        public async Task Delete(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = await _context.C242_BusOder.FirstOrDefaultAsync(item => item.ID == key);

            _context.C242_BusOder.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(C242_BusOder model, IDictionary values) {
            string ID = nameof(C242_BusOder.ID);
            string BODER_NO = nameof(C242_BusOder.BOderNo);
            string TEMP_ORDER = nameof(C242_BusOder.TempOrder);
            string ODER_TYPE = nameof(C242_BusOder.OderType);
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
            string NOI_CAT = nameof(C242_BusOder.NoiCat);
            string THVAT_LIEU = nameof(C242_BusOder.THVatLieu);
            string THPHOI = nameof(C242_BusOder.THPhoi);
            string STATUS = nameof(C242_BusOder.Status);
            string PAID = nameof(C242_BusOder.Paid);
            string PAY_DATE = nameof(C242_BusOder.PayDate);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(BODER_NO)) {
                model.BOderNo = Convert.ToString(values[BODER_NO]);
            }

            if(values.Contains(TEMP_ORDER)) {
                model.TempOrder = values[TEMP_ORDER] != null ? Convert.ToBoolean(values[TEMP_ORDER]) : (bool?)null;
            }

            if(values.Contains(ODER_TYPE)) {
                model.OderType = values[ODER_TYPE] != null ? Convert.ToInt32(values[ODER_TYPE]) : (int?)null;
            }

            if(values.Contains(DATE)) {
                model.Date = values[DATE] != null ? Convert.ToDateTime(values[DATE]) : (DateTime?)null;
            }

            if(values.Contains(PLAN_NO)) {
                model.PlanNo = Convert.ToString(values[PLAN_NO]);
            }

            if(values.Contains(PART_ID)) {
                model.PartID = Convert.ToString(values[PART_ID]);
            }

            if(values.Contains(QTY)) {
                model.Qty = values[QTY] != null ? Convert.ToInt32(values[QTY]) : (int?)null;
            }

            if(values.Contains(DEADLINE)) {
                model.Deadline = values[DEADLINE] != null ? Convert.ToDateTime(values[DEADLINE]) : (DateTime?)null;
            }

            if(values.Contains(RAW_QTY)) {
                model.RawQty = values[RAW_QTY] != null ? Convert.ToInt32(values[RAW_QTY]) : (int?)null;
            }

            if(values.Contains(HELISERT_QTY)) {
                model.HelisertQty = values[HELISERT_QTY] != null ? Convert.ToInt32(values[HELISERT_QTY]) : (int?)null;
            }

            if(values.Contains(BLAST_QTY)) {
                model.BlastQty = values[BLAST_QTY] != null ? Convert.ToInt32(values[BLAST_QTY]) : (int?)null;
            }

            if(values.Contains(MONO)) {
                model.MONo = Convert.ToString(values[MONO]);
            }

            if(values.Contains(MOQTY)) {
                model.MOQty = values[MOQTY] != null ? Convert.ToInt32(values[MOQTY]) : (int?)null;
            }

            if(values.Contains(STARTED)) {
                model.Started = values[STARTED] != null ? Convert.ToBoolean(values[STARTED]) : (bool?)null;
            }

            if(values.Contains(FINISHED)) {
                model.Finished = values[FINISHED] != null ? Convert.ToBoolean(values[FINISHED]) : (bool?)null;
            }

            if(values.Contains(FINISH_DATE)) {
                model.FinishDate = values[FINISH_DATE] != null ? Convert.ToDateTime(values[FINISH_DATE]) : (DateTime?)null;
            }

            if(values.Contains(CHANGE)) {
                model.Change = Convert.ToString(values[CHANGE]);
            }

            if(values.Contains(CHANGE_DATE)) {
                model.ChangeDate = values[CHANGE_DATE] != null ? Convert.ToDateTime(values[CHANGE_DATE]) : (DateTime?)null;
            }

            if(values.Contains(IMPORTED)) {
                model.Imported = values[IMPORTED] != null ? Convert.ToBoolean(values[IMPORTED]) : (bool?)null;
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
                model.CuttingStatus = values[CUTTING_STATUS] != null ? Convert.ToBoolean(values[CUTTING_STATUS]) : (bool?)null;
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

            if(values.Contains(NOI_CAT)) {
                model.NoiCat = Convert.ToString(values[NOI_CAT]);
            }

            if(values.Contains(THVAT_LIEU)) {
                model.THVatLieu = values[THVAT_LIEU] != null ? Convert.ToDateTime(values[THVAT_LIEU]) : (DateTime?)null;
            }

            if(values.Contains(THPHOI)) {
                model.THPhoi = values[THPHOI] != null ? Convert.ToDateTime(values[THPHOI]) : (DateTime?)null;
            }

            if(values.Contains(STATUS)) {
                model.Status = Convert.ToString(values[STATUS]);
            }

            if(values.Contains(PAID)) {
                model.Paid = values[PAID] != null ? Convert.ToBoolean(values[PAID]) : (bool?)null;
            }

            if(values.Contains(PAY_DATE)) {
                model.PayDate = values[PAY_DATE] != null ? Convert.ToDateTime(values[PAY_DATE]) : (DateTime?)null;
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