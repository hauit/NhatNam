using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
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
    [Route("api/C242_Prepare_Processing/{action}", Name = "C242_Prepare_ProcessingApi")]
    public class C242_Prepare_ProcessingController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public async Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions)
        {
            var fiveDaysAgo = DateTime.Now.AddDays(-5).Date;
            var c242_prepare_processing = _context.C242_Prepare_Processing
                .Where(i => i.Date >= fiveDaysAgo)
                .OrderByDescending(i => i.Date)
                .Select(i => new
                {
                    i.ID,
                    i.Date,
                    i.MachineID,
                    i.PartID,
                    i.OptionID,
                    i.Command,
                    i.Priority,
                    i.CB_StaffID,
                    i.CB_Start,
                    i.CB_End,
                    i.CT_StaffID,
                    i.CT_Start,
                    i.CT_End,
                    i.SX_StaffID,
                    i.SX_Start,
                    i.SX_End,
                    i.Note
                });

            return Request.CreateResponse(await DataSourceLoader.LoadAsync(c242_prepare_processing, loadOptions));
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post(FormDataCollection form)
        {
            var model = new C242_Prepare_Processing();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_Prepare_Processing.Add(model);
            await _context.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Created, new { result.ID });
        }

        [HttpPut]
        public async Task<HttpResponseMessage> Put(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var model = await _context.C242_Prepare_Processing.FirstOrDefaultAsync(item => item.ID == key);
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
            var model = await _context.C242_Prepare_Processing.FirstOrDefaultAsync(item => item.ID == key);

            _context.C242_Prepare_Processing.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(C242_Prepare_Processing model, IDictionary values)
        {
            string ID = nameof(C242_Prepare_Processing.ID);
            string DATE = nameof(C242_Prepare_Processing.Date);
            string MACHINE_ID = nameof(C242_Prepare_Processing.MachineID);
            string PART_ID = nameof(C242_Prepare_Processing.PartID);
            string OPTION_ID = nameof(C242_Prepare_Processing.OptionID);
            string COMMAND = nameof(C242_Prepare_Processing.Command);
            string PRIORITY = nameof(C242_Prepare_Processing.Priority);
            string CB_STAFF_ID = nameof(C242_Prepare_Processing.CB_StaffID);
            string CB_START = nameof(C242_Prepare_Processing.CB_Start);
            string CB_END = nameof(C242_Prepare_Processing.CB_End);
            string CT_STAFF_ID = nameof(C242_Prepare_Processing.CT_StaffID);
            string CT_START = nameof(C242_Prepare_Processing.CT_Start);
            string CT_END = nameof(C242_Prepare_Processing.CT_End);
            string SX_STAFF_ID = nameof(C242_Prepare_Processing.SX_StaffID);
            string SX_START = nameof(C242_Prepare_Processing.SX_Start);
            string SX_END = nameof(C242_Prepare_Processing.SX_End);
            string NOTE = nameof(C242_Prepare_Processing.Note);

            if (values.Contains(ID))
            {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if (values.Contains(DATE))
            {
                model.Date = Convert.ToDateTime(values[DATE]);
            }

            if (values.Contains(MACHINE_ID))
            {
                model.MachineID = Convert.ToString(values[MACHINE_ID]);
            }

            if (values.Contains(PART_ID))
            {
                model.PartID = Convert.ToString(values[PART_ID]);
            }

            if (values.Contains(OPTION_ID))
            {
                model.OptionID = Convert.ToString(values[OPTION_ID]);
            }

            if (values.Contains(COMMAND))
            {
                model.Command = Convert.ToString(values[COMMAND]);
            }

            if (values.Contains(PRIORITY))
            {
                model.Priority = Convert.ToString(values[PRIORITY]);
            }

            if (values.Contains(CB_STAFF_ID))
            {
                model.CB_StaffID = Convert.ToString(values[CB_STAFF_ID]);
            }

            if (values.Contains(CB_START))
            {
                model.CB_Start = Convert.ToDateTime(values[CB_START]);
            }

            if (values.Contains(CB_END))
            {
                model.CB_End = Convert.ToDateTime(values[CB_END]);
            }

            if (values.Contains(CT_STAFF_ID))
            {
                model.CT_StaffID = Convert.ToString(values[CT_STAFF_ID]);
            }

            if (values.Contains(CT_START))
            {
                model.CT_Start = Convert.ToDateTime(values[CT_START]);
            }

            if (values.Contains(CT_END))
            {
                model.CT_End = Convert.ToDateTime(values[CT_END]);
            }

            if (values.Contains(SX_STAFF_ID))
            {
                model.SX_StaffID = Convert.ToString(values[SX_STAFF_ID]);
            }

            if (values.Contains(SX_START))
            {
                model.SX_Start = Convert.ToDateTime(values[SX_START]);
            }

            if (values.Contains(SX_END))
            {
                model.SX_End = Convert.ToDateTime(values[SX_END]);
            }

            if (values.Contains(NOTE))
            {
                model.Note = Convert.ToString(values[NOTE]);
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