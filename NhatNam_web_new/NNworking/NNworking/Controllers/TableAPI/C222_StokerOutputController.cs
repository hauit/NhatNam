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
    [Route("api/C222_StokerOutput/{action}", Name = "C222_StokerOutputApi")]
    public class C222_StokerOutputController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c222_stokeroutput = _context.View_222_StokerOutput.ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c222_stokeroutput, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage sp_222_StockerOutput_OutputPreparation(DataSourceLoadOptions loadOptions) {
            var c222_stokeroutput = _context.sp_222_StockerOutput_OutputPreparation().ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c222_stokeroutput, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage sp_222_StockerOutput_OutputPreparation_Detail(DataSourceLoadOptions loadOptions) {
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            string partID = queryParams["partID"];
            var needQty = int.Parse(queryParams["needQty"]);
            var c222_stokeroutput = _context.sp_222_StockerOutput_OutputPreparation_Detail(partID).ToList();
            int calQty = needQty - 0;
            //bool giayXN = false;
            for(int i = 0; i < c222_stokeroutput.Count; i++)
            {
                //if (c222_stokeroutput[i].GiayXN)
                //{
                //    giayXN = true;
                //}
                if (c222_stokeroutput[i].AvailableQty == 0)
                {
                    continue;
                }

                if(calQty == 0)
                {
                    break;
                }
                c222_stokeroutput[i].NeedQty = needQty;
                if(calQty >= (int)c222_stokeroutput[i].AvailableQty)
                {
                    c222_stokeroutput[i].UseQty = (int)c222_stokeroutput[i].AvailableQty;
                    calQty = calQty - c222_stokeroutput[i].UseQty;
                }
                else
                {
                    c222_stokeroutput[i].UseQty = calQty;
                    calQty = 0;
                }
            }
            return Request.CreateResponse(DataSourceLoader.Load(c222_stokeroutput, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C222_StokerOutput();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C222_StokerOutput.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C222_StokerOutput.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C222_StokerOutput not found");

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
            var model = _context.C222_StokerOutput.FirstOrDefault(item => item.ID == key);

            _context.C222_StokerOutput.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C222_StokerOutput model, IDictionary values) {
            string ID = nameof(C222_StokerOutput.ID);
            string DATE = nameof(C222_StokerOutput.Date);
            string STAFF_ID = nameof(C222_StokerOutput.StaffID);
            string QTY = nameof(C222_StokerOutput.Qty);
            string TO_DEPT = nameof(C222_StokerOutput.ToDept);
            string NOTE = nameof(C222_StokerOutput.Note);
            string DELETED = nameof(C222_StokerOutput.Deleted);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(DATE)) {
                model.Date = Convert.ToDateTime(values[DATE]).Date;
            }

            if(values.Contains(STAFF_ID)) {
                model.StaffID = Convert.ToString(values[STAFF_ID]);
            }

            if(values.Contains(QTY)) {
                model.Qty = Convert.ToInt32(values[QTY]);
            }

            if(values.Contains(TO_DEPT)) {
                model.ToDept = Convert.ToString(values[TO_DEPT]);
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