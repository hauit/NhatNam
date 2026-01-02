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
    [Route("api/C242_MaintenanceCheck/{action}", Name = "C242_MaintenanceCheckApi")]
    public class C242_MaintenanceCheckController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public async Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions) {
            var c242_maintenancecheck = _context.C242_MaintenanceCheck.Select(i => new {
                i.ID,
                i.MachineId,
                i.CreatedAt,
                i.PrevValue,
                i.AfterValue,
                i.Quantity,
                i.OilV5,
                i.Oil68,
                i.LeakOK,
                i.ElectricalOK,
                i.BodyOK,
                i.ProcessStaff,
                i.CheckedStaff,
                i.Deleted
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ID" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Request.CreateResponse(await DataSourceLoader.LoadAsync(c242_maintenancecheck, loadOptions));
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post(FormDataCollection form) {
            var model = new C242_MaintenanceCheck();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            // Set CreatedAt if not provided
            if (!model.CreatedAt.HasValue)
                model.CreatedAt = DateTime.Now;

            var result = _context.C242_MaintenanceCheck.Add(model);
            await _context.SaveChangesAsync();

            var bugCreated = false;
            var bugDescription = form.Get("bugDescription");

            // Auto-create bug if safety checks failed
            if (!string.IsNullOrEmpty(bugDescription) && 
                (model.LeakOK == "Không đạt" || model.ElectricalOK == "Không đạt" || model.BodyOK == "Không đạt")) {
                
                var bug = new C242_MaintenanceBug {
                    MachineId = model.MachineId,
                    CheckId = result.ID.ToString(),
                    Description = bugDescription,
                    Reporter = model.ProcessStaff,
                    Status = "Open",
                    CreatedAt = DateTime.Now
                };

                _context.C242_MaintenanceBug.Add(bug);
                await _context.SaveChangesAsync();
                bugCreated = true;
            }

            // Update assignment status to Done
            var today = DateTime.Today;
            var assignment = await _context.C242_MaintenanceAssignment
                .FirstOrDefaultAsync(a => 
                    a.MachineId == model.MachineId && 
                    DbFunctions.TruncateTime(a.AssignDate) == today);

            if (assignment != null) {
                assignment.Status = "Done";
                await _context.SaveChangesAsync();
            }

            return Request.CreateResponse(HttpStatusCode.Created, new { 
                result.ID,
                BugCreated = bugCreated,
                Message = bugCreated ? "Đã hoàn thành kiểm tra và tạo báo cáo sự cố" : "Đã hoàn thành kiểm tra"
            });
        }

        [HttpPut]
        public async Task<HttpResponseMessage> Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = await _context.C242_MaintenanceCheck.FirstOrDefaultAsync(item => item.ID == key);
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
            var model = await _context.C242_MaintenanceCheck.FirstOrDefaultAsync(item => item.ID == key);

            _context.C242_MaintenanceCheck.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(C242_MaintenanceCheck model, IDictionary values) {
            string ID = nameof(C242_MaintenanceCheck.ID);
            string MACHINE_ID = nameof(C242_MaintenanceCheck.MachineId);
            string CREATED_AT = nameof(C242_MaintenanceCheck.CreatedAt);
            string PREV_VALUE = nameof(C242_MaintenanceCheck.PrevValue);
            string AFTER_VALUE = nameof(C242_MaintenanceCheck.AfterValue);
            string QUANTITY = nameof(C242_MaintenanceCheck.Quantity);
            string OIL_V5 = nameof(C242_MaintenanceCheck.OilV5);
            string OIL68 = nameof(C242_MaintenanceCheck.Oil68);
            string LEAK_OK = nameof(C242_MaintenanceCheck.LeakOK);
            string ELECTRICAL_OK = nameof(C242_MaintenanceCheck.ElectricalOK);
            string BODY_OK = nameof(C242_MaintenanceCheck.BodyOK);
            string PROCESS_STAFF = nameof(C242_MaintenanceCheck.ProcessStaff);
            string CHECKED_STAFF = nameof(C242_MaintenanceCheck.CheckedStaff);
            string DELETED = nameof(C242_MaintenanceCheck.Deleted);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(MACHINE_ID)) {
                model.MachineId = Convert.ToString(values[MACHINE_ID]);
            }

            if(values.Contains(CREATED_AT)) {
                model.CreatedAt = values[CREATED_AT] != null ? Convert.ToDateTime(values[CREATED_AT]) : (DateTime?)null;
            }

            if(values.Contains(PREV_VALUE)) {
                model.PrevValue = Convert.ToString(values[PREV_VALUE]);
            }

            if(values.Contains(AFTER_VALUE)) {
                model.AfterValue = Convert.ToString(values[AFTER_VALUE]);
            }

            if(values.Contains(QUANTITY)) {
                model.Quantity = Convert.ToString(values[QUANTITY]);
            }

            if(values.Contains(OIL_V5)) {
                model.OilV5 = Convert.ToString(values[OIL_V5]);
            }

            if(values.Contains(OIL68)) {
                model.Oil68 = Convert.ToString(values[OIL68]);
            }

            if(values.Contains(LEAK_OK)) {
                model.LeakOK = Convert.ToString(values[LEAK_OK]);
            }

            if(values.Contains(ELECTRICAL_OK)) {
                model.ElectricalOK = Convert.ToString(values[ELECTRICAL_OK]);
            }

            if(values.Contains(BODY_OK)) {
                model.BodyOK = Convert.ToString(values[BODY_OK]);
            }

            if(values.Contains(PROCESS_STAFF)) {
                model.ProcessStaff = Convert.ToString(values[PROCESS_STAFF]);
            }

            if(values.Contains(CHECKED_STAFF)) {
                model.CheckedStaff = Convert.ToString(values[CHECKED_STAFF]);
            }

            if(values.Contains(DELETED)) {
                model.Deleted = values[DELETED] != null ? Convert.ToBoolean(values[DELETED]) : (bool?)null;
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