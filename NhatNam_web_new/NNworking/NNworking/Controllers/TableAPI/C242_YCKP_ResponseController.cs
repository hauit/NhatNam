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
    [Route("api/C242_YCKP_Response/{action}", Name = "C242_YCKP_ResponseApi")]
    public class C242_YCKP_ResponseController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public async Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions)
        {
            var c242_yckp_response = _context.C242_YCKP_Response.Select(i => new
            {
                i.ID,
                i.UpdatedStaff,
                i.UpdatedDept,
                i.Date,
                i.Status,
                i.UpdatedResponse,
                i.UpdatedReason,
                i.UpdatedSolution,
                i.CausedDept,
                i.CausedDetail
            });

            return Request.CreateResponse(await DataSourceLoader.LoadAsync(c242_yckp_response, loadOptions));
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetById(int id)
        {
            var response = await _context.C242_YCKP_Response
                .Where(i => i.ID == id)
                .Select(i => new
                {
                    i.ID,
                    i.UpdatedStaff,
                    i.UpdatedDept,
                    i.Date,
                    i.Status,
                    i.UpdatedResponse,
                    i.UpdatedReason,
                    i.UpdatedSolution,
                    i.CausedDept,
                    i.CausedDetail
                })
                .FirstOrDefaultAsync();

            if (response == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Record not found");
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post(FormDataCollection form)
        {
            var model = new C242_YCKP_Response();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_YCKP_Response.Add(model);
            await _context.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Created, new { result.ID });
        }

        [HttpPut]
        public async Task<HttpResponseMessage> Put(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var model = await _context.C242_YCKP_Response.FirstOrDefaultAsync(item => item.ID == key);
            if (model == null)
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
        public async Task Delete(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var model = await _context.C242_YCKP_Response.FirstOrDefaultAsync(item => item.ID == key);

            _context.C242_YCKP_Response.Remove(model);
            await _context.SaveChangesAsync();
        }

        private void PopulateModel(C242_YCKP_Response model, IDictionary values)
        {
            string ID = nameof(C242_YCKP_Response.ID);
            string UPDATED_STAFF = nameof(C242_YCKP_Response.UpdatedStaff);
            string UPDATED_DEPT = nameof(C242_YCKP_Response.UpdatedDept);
            string DATE = nameof(C242_YCKP_Response.Date);
            string STATUS = nameof(C242_YCKP_Response.Status);
            string UPDATED_RESPONSE = nameof(C242_YCKP_Response.UpdatedResponse);
            string UPDATED_REASON = nameof(C242_YCKP_Response.UpdatedReason);
            string UPDATED_SOLUTION = nameof(C242_YCKP_Response.UpdatedSolution);
            string CAUSED_DEPT = nameof(C242_YCKP_Response.CausedDept);
            string CAUSED_DETAIL = nameof(C242_YCKP_Response.CausedDetail);

            if (values.Contains(ID))
            {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if (values.Contains(UPDATED_STAFF))
            {
                model.UpdatedStaff = Convert.ToString(values[UPDATED_STAFF]);
            }

            if (values.Contains(UPDATED_DEPT))
            {
                model.UpdatedDept = Convert.ToString(values[UPDATED_DEPT]);
            }

            if (values.Contains(DATE))
            {
                model.Date = Convert.ToDateTime(values[DATE]);
            }

            if (values.Contains(STATUS))
            {
                model.Status = Convert.ToString(values[STATUS]);
            }

            if (values.Contains(UPDATED_RESPONSE))
            {
                model.UpdatedResponse = Convert.ToString(values[UPDATED_RESPONSE]);
            }

            if (values.Contains(UPDATED_REASON))
            {
                model.UpdatedReason = Convert.ToString(values[UPDATED_REASON]);
            }

            if (values.Contains(UPDATED_SOLUTION))
            {
                model.UpdatedSolution = Convert.ToString(values[UPDATED_SOLUTION]);
            }

            if (values.Contains(CAUSED_DEPT))
            {
                model.CausedDept = Convert.ToString(values[CAUSED_DEPT]);
            }

            if (values.Contains(CAUSED_DETAIL))
            {
                model.CausedDetail = Convert.ToString(values[CAUSED_DETAIL]);
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState)
        {
            var messages = new List<string>();

            foreach (var entry in modelState)
            {
                foreach (var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}