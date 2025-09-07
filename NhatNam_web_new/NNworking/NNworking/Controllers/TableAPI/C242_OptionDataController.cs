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
    [Route("api/C242_OptionData/{action}", Name = "C242_OptionDataApi")]
    public class C242_OptionDataController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c242_optiondata = _context.C242_OptionData.Select(i => new {
                i.ID,
                i.InputDate,
                i.PartID,
                i.MachineID,
                i.OptionID,
                i.LastOption,
                i.JigID,
                i.ToolQty,
                i.ProTime,
                i.ClampTime,
                i.TechDate,
                i.UpdateDay,
                i.StaffID,
                i.Note,
                i.AondNote,
                i.Memo,
                i.CLUpdateday,
                i.JigType,
                i.Jig,
                i.Tich,
                i.Doc,
                i.TimeTreo,
                i.TimeComplete,
                i.Deleted
            });
            return Request.CreateResponse(DataSourceLoader.Load(c242_optiondata, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C242_OptionData();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_OptionData.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_OptionData.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C242_OptionData not found");

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
            var model = _context.C242_OptionData.FirstOrDefault(item => item.ID == key);

            _context.C242_OptionData.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C242_OptionData model, IDictionary values) {
            string ID = nameof(C242_OptionData.ID);
            string INPUT_DATE = nameof(C242_OptionData.InputDate);
            string PART_ID = nameof(C242_OptionData.PartID);
            string MACHINE_ID = nameof(C242_OptionData.MachineID);
            string OPTION_ID = nameof(C242_OptionData.OptionID);
            string LAST_OPTION = nameof(C242_OptionData.LastOption);
            string JIG_ID = nameof(C242_OptionData.JigID);
            string TOOL_QTY = nameof(C242_OptionData.ToolQty);
            string PRO_TIME = nameof(C242_OptionData.ProTime);
            string CLAMP_TIME = nameof(C242_OptionData.ClampTime);
            string TECH_DATE = nameof(C242_OptionData.TechDate);
            string UPDATE_DAY = nameof(C242_OptionData.UpdateDay);
            string STAFF_ID = nameof(C242_OptionData.StaffID);
            string NOTE = nameof(C242_OptionData.Note);
            string AOND_NOTE = nameof(C242_OptionData.AondNote);
            string MEMO = nameof(C242_OptionData.Memo);
            string CLUPDATEDAY = nameof(C242_OptionData.CLUpdateday);
            string JIG_TYPE = nameof(C242_OptionData.JigType);
            string JIG = nameof(C242_OptionData.Jig);
            string TICH = nameof(C242_OptionData.Tich);
            string DOC = nameof(C242_OptionData.Doc);
            string TIME_TREO = nameof(C242_OptionData.TimeTreo);
            string TIME_COMPLETE = nameof(C242_OptionData.TimeComplete);
            string DELETED = nameof(C242_OptionData.Deleted);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(INPUT_DATE)) {
                model.InputDate = Convert.ToDateTime(values[INPUT_DATE]);
            }

            if(values.Contains(PART_ID)) {
                model.PartID = Convert.ToString(values[PART_ID]);
            }

            if(values.Contains(MACHINE_ID)) {
                model.MachineID = Convert.ToString(values[MACHINE_ID]);
            }

            if(values.Contains(OPTION_ID)) {
                model.OptionID = Convert.ToString(values[OPTION_ID]);
            }

            if(values.Contains(LAST_OPTION)) {
                model.LastOption = Convert.ToBoolean(values[LAST_OPTION]);
            }

            if(values.Contains(JIG_ID)) {
                model.JigID = Convert.ToString(values[JIG_ID]);
            }

            if(values.Contains(TOOL_QTY)) {
                model.ToolQty = Convert.ToInt32(values[TOOL_QTY]);
            }

            if(values.Contains(PRO_TIME)) {
                model.ProTime = Convert.ToDouble(values[PRO_TIME]);
            }

            if(values.Contains(CLAMP_TIME)) {
                model.ClampTime = Convert.ToDouble(values[CLAMP_TIME]);
            }

            if(values.Contains(TECH_DATE)) {
                model.TechDate = Convert.ToBoolean(values[TECH_DATE]);
            }

            if(values.Contains(UPDATE_DAY)) {
                model.UpdateDay = Convert.ToDateTime(values[UPDATE_DAY]);
            }

            if(values.Contains(STAFF_ID)) {
                model.StaffID = Convert.ToString(values[STAFF_ID]);
            }

            if(values.Contains(NOTE)) {
                model.Note = Convert.ToString(values[NOTE]);
            }

            if(values.Contains(AOND_NOTE)) {
                model.AondNote = Convert.ToString(values[AOND_NOTE]);
            }

            if(values.Contains(MEMO)) {
                model.Memo = Convert.ToString(values[MEMO]);
            }

            if(values.Contains(CLUPDATEDAY)) {
                model.CLUpdateday = Convert.ToDateTime(values[CLUPDATEDAY]);
            }

            if(values.Contains(JIG_TYPE)) {
                model.JigType = Convert.ToString(values[JIG_TYPE]);
            }

            if(values.Contains(JIG)) {
                model.Jig = Convert.ToBoolean(values[JIG]);
            }

            if(values.Contains(TICH)) {
                model.Tich = Convert.ToDouble(values[TICH]);
            }

            if(values.Contains(DOC)) {
                model.Doc = Convert.ToDouble(values[DOC]);
            }

            if(values.Contains(TIME_TREO)) {
                model.TimeTreo = Convert.ToDouble(values[TIME_TREO]);
            }

            if(values.Contains(TIME_COMPLETE)) {
                model.TimeComplete = Convert.ToDouble(values[TIME_COMPLETE]);
            }

            if(values.Contains(DELETED)) {
                model.Deleted = Convert.ToBoolean(values[DELETED]);
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