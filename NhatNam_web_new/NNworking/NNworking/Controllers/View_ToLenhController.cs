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
    [Route("api/View_ToLenh/{action}", Name = "View_ToLenhApi")]
    public class View_ToLenhController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
           var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            var order = queryParams["order"];
            try
            {
                var view_tolenh = _context.sp_242_ToLenh(order).ToList();
                return Request.CreateResponse(DataSourceLoader.Load(view_tolenh, loadOptions));
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(DataSourceLoader.Load(new List<sp_242_ToLenh_Result>(), loadOptions));
            }
        }
        
        protected override void Dispose(bool disposing) {
            if (disposing) {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}