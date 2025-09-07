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
    [Route("api/C242_MachinePlan_Preparation/{action}", Name = "C242_MachinePlan_PreparationApi")]
    public class C242_MachinePlan_PreparationController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c242_machineplan_preparation = _context.C242_MachinePlan_Preparation.Select(i => new {
                i.ID,
                i.Date,
                i.PartID,
                i.MachineID,
                i.OptionID,
                i.PBJ1_Staff,
                i.PBJ1_Time,
                i.STT,
                i.Note,
                i.Phoi_KH,
                i.File_CB,
                i.File_KT,
                i.CT_CB,
                i.CT_KT,
                i.DG_CB,
                i.DG_KT,
                i.Dao_CB,
                i.Dao_KT,
                i.Confirmation
            });
            return Request.CreateResponse(DataSourceLoader.Load(c242_machineplan_preparation, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C242_MachinePlan_Preparation();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_MachinePlan_Preparation.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_MachinePlan_Preparation.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C242_MachinePlan_Preparation not found");

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
            var model = _context.C242_MachinePlan_Preparation.FirstOrDefault(item => item.ID == key);

            _context.C242_MachinePlan_Preparation.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C242_MachinePlan_Preparation model, IDictionary values) {
            string ID = nameof(C242_MachinePlan_Preparation.ID);
            string DATE = nameof(C242_MachinePlan_Preparation.Date);
            string PART_ID = nameof(C242_MachinePlan_Preparation.PartID);
            string MACHINE_ID = nameof(C242_MachinePlan_Preparation.MachineID);
            string OPTION_ID = nameof(C242_MachinePlan_Preparation.OptionID);
            string PBJ1_STAFF = nameof(C242_MachinePlan_Preparation.PBJ1_Staff);
            string PBJ1_TIME = nameof(C242_MachinePlan_Preparation.PBJ1_Time);
            string STT = nameof(C242_MachinePlan_Preparation.STT);
            string NOTE = nameof(C242_MachinePlan_Preparation.Note);
            string PHOI_KH = nameof(C242_MachinePlan_Preparation.Phoi_KH);
            string FILE_CB = nameof(C242_MachinePlan_Preparation.File_CB);
            string FILE_KT = nameof(C242_MachinePlan_Preparation.File_KT);
            string CT_CB = nameof(C242_MachinePlan_Preparation.CT_CB);
            string CT_KT = nameof(C242_MachinePlan_Preparation.CT_KT);
            string DG_CB = nameof(C242_MachinePlan_Preparation.DG_CB);
            string DG_KT = nameof(C242_MachinePlan_Preparation.DG_KT);
            string DAO_CB = nameof(C242_MachinePlan_Preparation.Dao_CB);
            string DAO_KT = nameof(C242_MachinePlan_Preparation.Dao_KT);
            string CONFIRMATION = nameof(C242_MachinePlan_Preparation.Confirmation);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(DATE)) {
                model.Date = Convert.ToDateTime(values[DATE]);
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

            if(values.Contains(PBJ1_STAFF)) {
                model.PBJ1_Staff = Convert.ToString(values[PBJ1_STAFF]);
            }

            if(values.Contains(PBJ1_TIME)) {
                model.PBJ1_Time = Convert.ToString(values[PBJ1_TIME]);
            }

            if(values.Contains(STT)) {
                model.STT = Convert.ToInt32(values[STT]);
            }

            if(values.Contains(NOTE)) {
                model.Note = Convert.ToString(values[NOTE]);
            }

            if(values.Contains(PHOI_KH)) {
                model.Phoi_KH = Convert.ToBoolean(values[PHOI_KH]);
            }

            if(values.Contains(FILE_CB)) {
                model.File_CB = Convert.ToBoolean(values[FILE_CB]);
            }

            if(values.Contains(FILE_KT)) {
                model.File_KT = Convert.ToBoolean(values[FILE_KT]);
            }

            if(values.Contains(CT_CB)) {
                model.CT_CB = Convert.ToBoolean(values[CT_CB]);
            }

            if(values.Contains(CT_KT)) {
                model.CT_KT = Convert.ToBoolean(values[CT_KT]);
            }

            if(values.Contains(DG_CB)) {
                model.DG_CB = Convert.ToBoolean(values[DG_CB]);
            }

            if(values.Contains(DG_KT)) {
                model.DG_KT = Convert.ToBoolean(values[DG_KT]);
            }

            if(values.Contains(DAO_CB)) {
                model.Dao_CB = Convert.ToBoolean(values[DAO_CB]);
            }

            if(values.Contains(DAO_KT)) {
                model.Dao_KT = Convert.ToBoolean(values[DAO_KT]);
            }

            if(values.Contains(CONFIRMATION)) {
                model.Confirmation = Convert.ToString(values[CONFIRMATION]);
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