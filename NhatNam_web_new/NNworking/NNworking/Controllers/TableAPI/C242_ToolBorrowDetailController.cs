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
    [Route("api/C242_ToolBorrowDetail/{action}", Name = "C242_ToolBorrowDetailApi")]
    public class C242_ToolBorrowDetailController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c242_toolborrowdetail = _context.C242_ToolBorrowDetail.Select(i => new {
                i.ID,
                i.BorrowNo,
                i.BorrowNew,
                i.BorrowMore,
                i.Quantity,
                i.Note,
                i.ToolDataID,
                i.ToolName,
                i.MakerToolName
            });
            return Request.CreateResponse(DataSourceLoader.Load(c242_toolborrowdetail, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage GetByBorrowNo(DataSourceLoadOptions loadOptions)
        {
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            int id = int.Parse(queryParams["id"]);

            var c242_toolborrowdetail = _context.C242_ToolBorrowDetail.Where(x => x.BorrowNo == id).ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c242_toolborrowdetail, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C242_ToolBorrowDetail();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_ToolBorrowDetail.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_ToolBorrowDetail.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C242_ToolBorrowDetail not found");

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
            var model = _context.C242_ToolBorrowDetail.FirstOrDefault(item => item.ID == key);

            _context.C242_ToolBorrowDetail.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C242_ToolBorrowDetail model, IDictionary values) {
            string ID = nameof(C242_ToolBorrowDetail.ID);
            string BORROW_NO = nameof(C242_ToolBorrowDetail.BorrowNo);
            string BORROW_NEW = nameof(C242_ToolBorrowDetail.BorrowNew);
            string BORROW_MORE = nameof(C242_ToolBorrowDetail.BorrowMore);
            string QUANTITY = nameof(C242_ToolBorrowDetail.Quantity);
            string NOTE = nameof(C242_ToolBorrowDetail.Note);
            string TOOL_DATA_ID = nameof(C242_ToolBorrowDetail.ToolDataID);
            string TOOL_NAME = nameof(C242_ToolBorrowDetail.ToolName);
            string MAKER_TOOL_NAME = nameof(C242_ToolBorrowDetail.MakerToolName);
            string Available = nameof(C242_ToolBorrowDetail.Available);

            if (values.Contains(Available))
            {
                model.Available = Convert.ToBoolean(values[Available]);
            }

            if (values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(BORROW_NO)) {
                model.BorrowNo = Convert.ToInt32(values[BORROW_NO]);
            }

            if(values.Contains(BORROW_NEW)) {
                model.BorrowNew = Convert.ToBoolean(values[BORROW_NEW]);
            }

            if(values.Contains(BORROW_MORE)) {
                model.BorrowMore = Convert.ToBoolean(values[BORROW_MORE]);
            }

            if(values.Contains(QUANTITY)) {
                model.Quantity = Convert.ToInt32(values[QUANTITY]);
            }

            if(values.Contains(NOTE)) {
                model.Note = Convert.ToString(values[NOTE]);
            }

            if(values.Contains(TOOL_DATA_ID)) {
                model.ToolDataID = Convert.ToString(values[TOOL_DATA_ID]);
            }

            if(values.Contains(TOOL_NAME)) {
                model.ToolName = Convert.ToString(values[TOOL_NAME]);
            }

            if(values.Contains(MAKER_TOOL_NAME)) {
                model.MakerToolName = Convert.ToString(values[MAKER_TOOL_NAME]);
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