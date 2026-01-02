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
    [Route("api/C242_MaintenanceBug/{action}", Name = "C242_MaintenanceBugApi")]
    public class C242_MaintenanceBugController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public async Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions) {
            var c242_maintenancebug = _context.C242_MaintenanceBug.Select(i => new {
                i.ID,
                i.MachineId,
                i.CheckId,
                i.Description,
                i.Reporter,
                i.FixWay,
                i.FixDetail,
                i.Status,
                i.CreatedAt
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ID" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Request.CreateResponse(await DataSourceLoader.LoadAsync(c242_maintenancebug, loadOptions));
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post(FormDataCollection form) {
            var model = new C242_MaintenanceBug();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_MaintenanceBug.Add(model);
            await _context.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Created, new { result.ID });
        }

        [HttpPut]
        public async Task<HttpResponseMessage> Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = await _context.C242_MaintenanceBug.FirstOrDefaultAsync(item => item.ID == key);
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
            var model = await _context.C242_MaintenanceBug.FirstOrDefaultAsync(item => item.ID == key);

            _context.C242_MaintenanceBug.Remove(model);
            await _context.SaveChangesAsync();
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetOpen() {
            var bugs = await _context.C242_MaintenanceBug
                .Where(b => b.Status == "Open" || b.Status == "Processing")
                .Select(b => new {
                    b.ID,
                    b.MachineId,
                    MachineName = _context.C222_Machine
                        .Where(m => m.MachineID == b.MachineId)
                        .Select(m => m.MachineName)
                        .FirstOrDefault(),
                    b.CheckId,
                    CheckDate = _context.C242_MaintenanceCheck
                        .Where(c => c.ID.ToString() == b.CheckId)
                        .Select(c => c.CreatedAt)
                        .FirstOrDefault(),
                    b.Description,
                    b.Reporter,
                    b.FixWay,
                    b.FixDetail,
                    b.Status,
                    b.CreatedAt
                })
                .OrderBy(b => b.Status)
                .ThenByDescending(b => b.CreatedAt)
                .ToListAsync();

            return Request.CreateResponse(HttpStatusCode.OK, bugs);
        }


        private void PopulateModel(C242_MaintenanceBug model, IDictionary values) {
            string ID = nameof(C242_MaintenanceBug.ID);
            string MACHINE_ID = nameof(C242_MaintenanceBug.MachineId);
            string CHECK_ID = nameof(C242_MaintenanceBug.CheckId);
            string DESCRIPTION = nameof(C242_MaintenanceBug.Description);
            string REPORTER = nameof(C242_MaintenanceBug.Reporter);
            string FIX_WAY = nameof(C242_MaintenanceBug.FixWay);
            string FIX_DETAIL = nameof(C242_MaintenanceBug.FixDetail);
            string STATUS = nameof(C242_MaintenanceBug.Status);
            string CREATED_AT = nameof(C242_MaintenanceBug.CreatedAt);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(MACHINE_ID)) {
                model.MachineId = Convert.ToString(values[MACHINE_ID]);
            }

            if(values.Contains(CHECK_ID)) {
                model.CheckId = Convert.ToString(values[CHECK_ID]);
            }

            if(values.Contains(DESCRIPTION)) {
                model.Description = Convert.ToString(values[DESCRIPTION]);
            }

            if(values.Contains(REPORTER)) {
                model.Reporter = Convert.ToString(values[REPORTER]);
            }

            if(values.Contains(FIX_WAY)) {
                model.FixWay = Convert.ToString(values[FIX_WAY]);
            }

            if(values.Contains(FIX_DETAIL)) {
                model.FixDetail = Convert.ToString(values[FIX_DETAIL]);
            }

            if(values.Contains(STATUS)) {
                model.Status = Convert.ToString(values[STATUS]);
            }

            if(values.Contains(CREATED_AT)) {
                model.CreatedAt = values[CREATED_AT] != null ? Convert.ToDateTime(values[CREATED_AT]) : (DateTime?)null;
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