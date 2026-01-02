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
    [Route("api/C242_MaintenanceAssignment/{action}", Name = "C242_MaintenanceAssignmentApi")]
    public class C242_MaintenanceAssignmentController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public async Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions) {
            var c242_maintenanceassignment = _context.C242_MaintenanceAssignment.Select(i => new {
                i.ID,
                i.MachineId,
                i.AssignDate,
                i.AssignedStaff,
                i.Status,
                i.Note,
                i.CreatedAt
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ID" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Request.CreateResponse(await DataSourceLoader.LoadAsync(c242_maintenanceassignment, loadOptions));
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post(FormDataCollection form) {
            var model = new C242_MaintenanceAssignment();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_MaintenanceAssignment.Add(model);
            await _context.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Created, new { result.ID });
        }

        [HttpPut]
        public async Task<HttpResponseMessage> Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = await _context.C242_MaintenanceAssignment.FirstOrDefaultAsync(item => item.ID == key);
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
            var model = await _context.C242_MaintenanceAssignment.FirstOrDefaultAsync(item => item.ID == key);

            _context.C242_MaintenanceAssignment.Remove(model);
            await _context.SaveChangesAsync();
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetToday() {
            var today = DateTime.Today;
            
            var assignments = await _context.C242_MaintenanceAssignment
                .Where(a => DbFunctions.TruncateTime(a.AssignDate) == today)
                .Select(a => new {
                    a.ID,
                    a.MachineId,
                    MachineName = _context.C222_Machine
                        .Where(m => m.MachineID == a.MachineId)
                        .Select(m => m.MachineName)
                        .FirstOrDefault(),
                    a.AssignDate,
                    a.AssignedStaff,
                    a.Status,
                    a.Note
                })
                .ToListAsync();

            return Request.CreateResponse(HttpStatusCode.OK, assignments);
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetByMachineAndDate(string machineId, string assignDate) {
            var date = DateTime.Parse(assignDate).Date;
            
            var assignment = await _context.C242_MaintenanceAssignment
                .Where(a => a.MachineId == machineId && DbFunctions.TruncateTime(a.AssignDate) == date)
                .Select(a => new {
                    a.ID,
                    a.MachineId,
                    MachineName = _context.C222_Machine
                        .Where(m => m.MachineID == a.MachineId)
                        .Select(m => m.MachineName)
                        .FirstOrDefault(),
                    a.AssignDate,
                    a.AssignedStaff,
                    a.Status,
                    a.Note
                })
                .FirstOrDefaultAsync();

            if (assignment == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Assignment not found");

            return Request.CreateResponse(HttpStatusCode.OK, assignment);
        }

        [HttpPut]
        public async Task<HttpResponseMessage> Complete(int id) {
            var assignment = await _context.C242_MaintenanceAssignment
                .FirstOrDefaultAsync(a => a.ID == id);

            if (assignment == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Assignment not found");

            assignment.Status = "Done";
            await _context.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.OK, new { Message = "Assignment completed" });
        }


        private void PopulateModel(C242_MaintenanceAssignment model, IDictionary values) {
            string ID = nameof(C242_MaintenanceAssignment.ID);
            string MACHINE_ID = nameof(C242_MaintenanceAssignment.MachineId);
            string ASSIGN_DATE = nameof(C242_MaintenanceAssignment.AssignDate);
            string ASSIGNED_STAFF = nameof(C242_MaintenanceAssignment.AssignedStaff);
            string STATUS = nameof(C242_MaintenanceAssignment.Status);
            string NOTE = nameof(C242_MaintenanceAssignment.Note);
            string CREATED_AT = nameof(C242_MaintenanceAssignment.CreatedAt);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(MACHINE_ID)) {
                model.MachineId = Convert.ToString(values[MACHINE_ID]);
            }

            if(values.Contains(ASSIGN_DATE)) {
                model.AssignDate = Convert.ToDateTime(values[ASSIGN_DATE]);
            }

            if(values.Contains(ASSIGNED_STAFF)) {
                model.AssignedStaff = Convert.ToString(values[ASSIGNED_STAFF]);
            }

            if(values.Contains(STATUS)) {
                model.Status = Convert.ToString(values[STATUS]);
            }

            if(values.Contains(NOTE)) {
                model.Note = Convert.ToString(values[NOTE]);
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