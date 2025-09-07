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
    [Route("api/C222_StockerOutputDEtail/{action}", Name = "C222_StockerOutputDEtailApi")]
    public class C222_StockerOutputDEtailController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            var c222_stockeroutputdetail = _context.C222_StockerOutputDEtail.ToList();

            if (queryParams.ContainsKey("OrderNo"))
            {
                string order = queryParams["OrderNo"];
                c222_stockeroutputdetail = _context.C222_StockerOutputDEtail.Where(x=>x.OderCan.ToLower() == order.ToLower()).ToList();
            }

            return Request.CreateResponse(DataSourceLoader.Load(c222_stockeroutputdetail, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C222_StockerOutputDEtail();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C222_StockerOutputDEtail.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C222_StockerOutputDEtail.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C222_StockerOutputDEtail not found");

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
            var model = _context.C222_StockerOutputDEtail.FirstOrDefault(item => item.ID == key);

            _context.C222_StockerOutputDEtail.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C222_StockerOutputDEtail model, IDictionary values) {
            string ID = nameof(C222_StockerOutputDEtail.ID);
            string NUMBER = nameof(C222_StockerOutputDEtail.Number);
            string ODER_CAN = nameof(C222_StockerOutputDEtail.OderCan);
            string SLG_CAN = nameof(C222_StockerOutputDEtail.SlgCan);
            string ORDER_CO = nameof(C222_StockerOutputDEtail.OrderCo);
            string SLG_CO = nameof(C222_StockerOutputDEtail.SlgCo);
            string GIAY_XN = nameof(C222_StockerOutputDEtail.GiayXN);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(NUMBER)) {
                model.Number = Convert.ToString(values[NUMBER]);
            }

            if(values.Contains(ODER_CAN)) {
                model.OderCan = Convert.ToString(values[ODER_CAN]);
            }

            if(values.Contains(SLG_CAN)) {
                model.SlgCan = Convert.ToInt32(values[SLG_CAN]);
            }

            if(values.Contains(ORDER_CO)) {
                model.OrderCo = Convert.ToString(values[ORDER_CO]);
            }

            if(values.Contains(SLG_CO)) {
                model.SlgCo = Convert.ToInt32(values[SLG_CO]);
            }

            if(values.Contains(GIAY_XN)) {
                model.GiayXN = Convert.ToBoolean(values[GIAY_XN]);
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