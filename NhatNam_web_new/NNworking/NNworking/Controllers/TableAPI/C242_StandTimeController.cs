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
    [Route("api/C242_StandTime/{action}", Name = "C242_StandTimeApi")]
    public class C242_StandTimeController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c242_standtime = _context.C242_StandTime.Select(i => new {
                i.ID,
                i.Date,
                i.PartNo,
                i.MachineID,
                i.OptionID,
                i.PJ61_PKN1,
                i.PZE1,
                i.PBJ1,
                i.PU71,
                i.PH71,
                i.Note,
                i.Note2,
                i.NumberOption,
                i.Status,
                i.MachineGroup
            });
            return Request.CreateResponse(DataSourceLoader.Load(c242_standtime, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C242_StandTime();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_StandTime.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_StandTime.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C242_StandTime not found");

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
            var model = _context.C242_StandTime.FirstOrDefault(item => item.ID == key);

            _context.C242_StandTime.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C242_StandTime model, IDictionary values) {
            string ID = nameof(C242_StandTime.ID);
            string DATE = nameof(C242_StandTime.Date);
            string PART_NO = nameof(C242_StandTime.PartNo);
            string MACHINE_ID = nameof(C242_StandTime.MachineID);
            string OPTION_ID = nameof(C242_StandTime.OptionID);
            string PJ61_PKN1 = nameof(C242_StandTime.PJ61_PKN1);
            string PZE1 = nameof(C242_StandTime.PZE1);
            string PBJ1 = nameof(C242_StandTime.PBJ1);
            string PU71 = nameof(C242_StandTime.PU71);
            string PH71 = nameof(C242_StandTime.PH71);
            string NOTE = nameof(C242_StandTime.Note);
            string NOTE2 = nameof(C242_StandTime.Note2);
            string NUMBER_OPTION = nameof(C242_StandTime.NumberOption);
            string STATUS = nameof(C242_StandTime.Status);
            string MACHINE_GROUP = nameof(C242_StandTime.MachineGroup);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(DATE)) {
                model.Date = Convert.ToDateTime(values[DATE]);
            }

            if(values.Contains(PART_NO)) {
                model.PartNo = Convert.ToString(values[PART_NO]);
            }

            if(values.Contains(MACHINE_ID)) {
                model.MachineID = Convert.ToString(values[MACHINE_ID]);
            }

            if(values.Contains(OPTION_ID)) {
                model.OptionID = Convert.ToString(values[OPTION_ID]);
            }

            if(values.Contains(PJ61_PKN1)) {
                model.PJ61_PKN1 = Convert.ToDouble(values[PJ61_PKN1]);
            }

            if(values.Contains(PZE1)) {
                model.PZE1 = Convert.ToDouble(values[PZE1]);
            }

            if(values.Contains(PBJ1)) {
                model.PBJ1 = Convert.ToDouble(values[PBJ1]);
            }

            if(values.Contains(PU71)) {
                model.PU71 = Convert.ToDouble(values[PU71]);
            }

            if(values.Contains(PH71)) {
                model.PH71 = Convert.ToDouble(values[PH71]);
            }

            if(values.Contains(NOTE)) {
                model.Note = Convert.ToString(values[NOTE]);
            }

            if(values.Contains(NOTE2)) {
                model.Note2 = Convert.ToString(values[NOTE2]);
            }

            if(values.Contains(NUMBER_OPTION)) {
                model.NumberOption = Convert.ToInt32(values[NUMBER_OPTION]);
            }

            if(values.Contains(STATUS)) {
                model.Status = Convert.ToBoolean(values[STATUS]);
            }

            if(values.Contains(MACHINE_GROUP)) {
                model.MachineGroup = Convert.ToString(values[MACHINE_GROUP]);
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