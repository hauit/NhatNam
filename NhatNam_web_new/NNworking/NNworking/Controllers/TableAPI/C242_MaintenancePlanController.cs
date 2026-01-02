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
    [Route("api/C242_MaintenancePlan/{action}", Name = "C242_MaintenancePlanApi")]
    public class C242_MaintenancePlanController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public async Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions)
        {
            var c242_maintenanceplan = _context.C242_MaintenancePlan.Select(i => new
            {
                i.ID,
                i.MachineId,
                i.FrequencyMonth,
                i.StartDate
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ID" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Request.CreateResponse(await DataSourceLoader.LoadAsync(c242_maintenanceplan, loadOptions));
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post(FormDataCollection form)
        {
            var model = new C242_MaintenancePlan();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_MaintenancePlan.Add(model);
            await _context.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Created, new { result.ID });
        }

        [HttpPut]
        public async Task<HttpResponseMessage> Put(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var model = await _context.C242_MaintenancePlan.FirstOrDefaultAsync(item => item.ID == key);
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
            var model = await _context.C242_MaintenancePlan.FirstOrDefaultAsync(item => item.ID == key);

            _context.C242_MaintenancePlan.Remove(model);
            await _context.SaveChangesAsync();
        }

        [HttpPost]
        public async Task<HttpResponseMessage> GenerateAssignments()
        {
            var today = DateTime.Today;
            var createdCount = 0;

            // Get all active maintenance plans
            var plans = await _context.C242_MaintenancePlan
                .Where(p => p.StartDate.HasValue && p.FrequencyMonth.HasValue && p.FrequencyMonth > 0)
                .ToListAsync();

            foreach (var plan in plans)
            {
                var startDate = plan.StartDate.Value.Date;
                var frequency = plan.FrequencyMonth.Value;

                // Calculate if maintenance is due today
                bool isDueToday = false;

                // Start from the start date and add frequency months until we reach or pass today
                var nextDueDate = startDate;
                while (nextDueDate <= today)
                {
                    if (nextDueDate == today)
                    {
                        isDueToday = true;
                        break;
                    }
                    nextDueDate = nextDueDate.AddMonths(frequency);
                }

                if (isDueToday)
                {
                    // Check if assignment already exists for today
                    var existingAssignment = await _context.C242_MaintenanceAssignment
                        .FirstOrDefaultAsync(a =>
                            a.MachineId == plan.MachineId &&
                            DbFunctions.TruncateTime(a.AssignDate) == today);

                    if (existingAssignment == null)
                    {
                        // Create new assignment
                        var assignment = new C242_MaintenanceAssignment
                        {
                            MachineId = plan.MachineId,
                            AssignDate = today,
                            AssignedStaff = "Tự động", // Auto-assigned
                            Status = "Assigned",
                            Note = $"Bảo trì định kỳ {plan.FrequencyMonth} tháng",
                            CreatedAt = DateTime.Now
                        };

                        _context.C242_MaintenanceAssignment.Add(assignment);
                        createdCount++;
                    }
                }
            }

            await _context.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                Message = $"Đã tạo {createdCount} công việc bảo trì cho hôm nay",
                Count = createdCount
            });
        }


        private void PopulateModel(C242_MaintenancePlan model, IDictionary values)
        {
            string ID = nameof(C242_MaintenancePlan.ID);
            string MACHINE_ID = nameof(C242_MaintenancePlan.MachineId);
            string FREQUENCY_MONTH = nameof(C242_MaintenancePlan.FrequencyMonth);
            string START_DATE = nameof(C242_MaintenancePlan.StartDate);

            if (values.Contains(ID))
            {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if (values.Contains(MACHINE_ID))
            {
                model.MachineId = Convert.ToString(values[MACHINE_ID]);
            }

            if (values.Contains(FREQUENCY_MONTH))
            {
                model.FrequencyMonth = values[FREQUENCY_MONTH] != null ? Convert.ToInt32(values[FREQUENCY_MONTH]) : (int?)null;
            }

            if (values.Contains(START_DATE))
            {
                model.StartDate = values[START_DATE] != null ? Convert.ToDateTime(values[START_DATE]) : (DateTime?)null;
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