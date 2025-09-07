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
    [Route("api/C222_StockerItem/{action}", Name = "C222_StockerItemApi")]
    public class C222_StockerItemController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            var c222_stockeritem = _context.C222_StockerItem.ToList();
            if (queryParams.Count == 0)
            {
                return Request.CreateResponse(DataSourceLoader.Load(c222_stockeritem, loadOptions));
            }

            if (!queryParams.ContainsKey("catergory"))
            {
                return Request.CreateResponse(DataSourceLoader.Load(c222_stockeritem, loadOptions));
            }

            string catergory = queryParams["catergory"];
            var catObj = _context.C222_StokerCatergory.Where(x => x.Alias.ToLower() == catergory.ToLower()).FirstOrDefault();
            if (catObj == null)
            {
                return Request.CreateResponse(DataSourceLoader.Load(c222_stockeritem, loadOptions));
            }

            c222_stockeritem = c222_stockeritem.Where(x => x.Catergory == catObj.ID).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c222_stockeritem, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C222_StockerItem();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C222_StockerItem.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C222_StockerItem.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C222_StockerItem not found");

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
            var model = _context.C222_StockerItem.FirstOrDefault(item => item.ID == key);

            _context.C222_StockerItem.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C222_StockerItem model, IDictionary values) {
            string ID = nameof(C222_StockerItem.ID);
            string ITEM_ID = nameof(C222_StockerItem.ItemID);
            string ITEM_NAME = nameof(C222_StockerItem.ItemName);
            string PURCHASING_UNIT = nameof(C222_StockerItem.PurchasingUnit);
            string USING_UNIT = nameof(C222_StockerItem.UsingUnit);
            string MOQ = nameof(C222_StockerItem.MOQ);
            string ROUNDING = nameof(C222_StockerItem.Rounding);
            string CATERGORY = nameof(C222_StockerItem.Catergory);
            string SUPPLIER = nameof(C222_StockerItem.Supplier);
            string MAKER = nameof(C222_StockerItem.Maker);
            string TYPE = nameof(C222_StockerItem.Type);
            string NOTE = nameof(C222_StockerItem.Note);
            string LENGHT = nameof(C222_StockerItem.Lenght);
            string ME_CUT_LENGHT = nameof(C222_StockerItem.MeCutLenght);
            string ME_NUMBER = nameof(C222_StockerItem.MeNumber);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(ITEM_ID)) {
                model.ItemID = Convert.ToString(values[ITEM_ID]);
            }

            if(values.Contains(ITEM_NAME)) {
                model.ItemName = Convert.ToString(values[ITEM_NAME]);
            }

            if(values.Contains(PURCHASING_UNIT)) {
                model.PurchasingUnit = Convert.ToInt32(values[PURCHASING_UNIT]);
            }

            if(values.Contains(USING_UNIT)) {
                model.UsingUnit = Convert.ToInt32(values[USING_UNIT]);
            }

            if(values.Contains(MOQ)) {
                model.MOQ = Convert.ToInt32(values[MOQ]);
            }

            if(values.Contains(ROUNDING)) {
                model.Rounding = Convert.ToInt32(values[ROUNDING]);
            }

            if(values.Contains(CATERGORY)) {
                model.Catergory = Convert.ToInt32(values[CATERGORY]);
            }

            if(values.Contains(SUPPLIER)) {
                model.Supplier = Convert.ToInt32(values[SUPPLIER]);
            }

            if(values.Contains(MAKER)) {
                model.Maker = Convert.ToInt32(values[MAKER]);
            }

            if(values.Contains(TYPE)) {
                model.Type = Convert.ToInt32(values[TYPE]);
            }

            if(values.Contains(NOTE)) {
                model.Note = Convert.ToString(values[NOTE]);
            }

            if(values.Contains(LENGHT)) {
                model.Lenght = Convert.ToInt32(values[LENGHT]);
            }

            if(values.Contains(ME_CUT_LENGHT)) {
                model.MeCutLenght = Convert.ToInt32(values[ME_CUT_LENGHT]);
            }

            if(values.Contains(ME_NUMBER)) {
                model.MeNumber = Convert.ToInt32(values[ME_NUMBER]);
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