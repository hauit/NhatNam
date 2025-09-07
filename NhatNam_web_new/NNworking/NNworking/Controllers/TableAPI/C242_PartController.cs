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
    [Route("api/C242_Part/{action}", Name = "C242_PartApi")]
    public class C242_PartController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c242_part = _context.C242_Part.Select(i => new {
                i.ID,
                i.PartNo,
                i.PartName,
                i.CustomerID,
                i.SupplierID,
                i.UpQty,
                i.GiaThanh,
                i.Deleted,
                i.CatID,
                i.Unit,
                i.IsTool
            });
            return Request.CreateResponse(DataSourceLoader.Load(c242_part, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage GetExpireOrder(DataSourceLoadOptions loadOptions)
        {
            var date = DateTime.Now.Date.AddDays(5);
            var c242_part = _context.View_242_BusOder.Where(x => x.Finished != true && x.Date <= date).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c242_part, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage sp_242_WTS_DirectWorkPercen(DataSourceLoadOptions loadOptions)
        {
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            var fromdate = DateTime.ParseExact(queryParams["fromdate"].Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture).Date;
            var todate = DateTime.ParseExact(queryParams["todate"].Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture).Date;
            var c242_part = _context.sp_242_WTS_DirectWorkPercen(fromdate, todate).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c242_part, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage sp_242_WTS_StandTimeWorkPercen(DataSourceLoadOptions loadOptions)
        {
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            var fromdate = DateTime.ParseExact(queryParams["fromdate"].Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture).Date;
            var todate = DateTime.ParseExact(queryParams["todate"].Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture).Date;
            try
            {
                var c242_part = _context.sp_242_WTS_StandTimeWorkPercen(fromdate, todate).ToList();
                return Request.CreateResponse(DataSourceLoader.Load(c242_part, loadOptions));
            }
            catch(Exception ex)
            {

            }

            return Request.CreateResponse(DataSourceLoader.Load(new List<sp_242_WTS_StandTimeWorkPercen_Result>(), loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C242_Part();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_Part.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_Part.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C242_Part not found");

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
            var model = _context.C242_Part.FirstOrDefault(item => item.ID == key);

            _context.C242_Part.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C242_Part model, IDictionary values) {
            string ID = nameof(C242_Part.ID);
            string PART_NO = nameof(C242_Part.PartNo);
            string PART_NAME = nameof(C242_Part.PartName);
            string CUSTOMER_ID = nameof(C242_Part.CustomerID);
            string SUPPLIER_ID = nameof(C242_Part.SupplierID);
            string UP_QTY = nameof(C242_Part.UpQty);
            string GIA_THANH = nameof(C242_Part.GiaThanh);
            string DELETED = nameof(C242_Part.Deleted);
            string CAT_ID = nameof(C242_Part.CatID);
            string UNIT = nameof(C242_Part.Unit);
            string IS_TOOL = nameof(C242_Part.IsTool);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(PART_NO)) {
                model.PartNo = Convert.ToString(values[PART_NO]);
            }

            if(values.Contains(PART_NAME)) {
                model.PartName = Convert.ToString(values[PART_NAME]);
            }

            if(values.Contains(CUSTOMER_ID)) {
                model.CustomerID = Convert.ToString(values[CUSTOMER_ID]);
            }

            if(values.Contains(SUPPLIER_ID)) {
                model.SupplierID = Convert.ToString(values[SUPPLIER_ID]);
            }

            if(values.Contains(UP_QTY)) {
                model.UpQty = Convert.ToInt32(values[UP_QTY]);
            }

            if(values.Contains(GIA_THANH)) {
                model.GiaThanh = Convert.ToInt32(values[GIA_THANH]);
            }

            if(values.Contains(DELETED)) {
                model.Deleted = Convert.ToBoolean(values[DELETED]);
            }

            if(values.Contains(CAT_ID)) {
                model.CatID = Convert.ToInt32(values[CAT_ID]);
            }

            if(values.Contains(UNIT)) {
                model.Unit = Convert.ToString(values[UNIT]);
            }

            if(values.Contains(IS_TOOL)) {
                model.IsTool = Convert.ToBoolean(values[IS_TOOL]);
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