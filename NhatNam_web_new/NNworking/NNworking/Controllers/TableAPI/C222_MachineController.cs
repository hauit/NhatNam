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
    [Route("api/C222_Machine/{action}", Name = "C222_MachineApi")]
    public class C222_MachineController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c222_machine = _context.C222_Machine.ToList();
            return Request.CreateResponse(DataSourceLoader.Load(c222_machine, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C222_Machine();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C222_Machine.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C222_Machine.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C222_Machine not found");

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
            var model = _context.C222_Machine.FirstOrDefault(item => item.ID == key);

            _context.C222_Machine.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C222_Machine model, IDictionary values) {
            string ID = nameof(C222_Machine.ID);
            string MACHINE_ID = nameof(C222_Machine.MachineID);
            string MAKER_MACHINE_NO = nameof(C222_Machine.MakerMachineNo);
            string ACC_NAME = nameof(C222_Machine.AccName);
            string DENSAN_NAME = nameof(C222_Machine.DensanName);
            string DENSAN_CODE = nameof(C222_Machine.DensanCode);
            string DMACHINE = nameof(C222_Machine.Dmachine);
            string MACHINE_NAME = nameof(C222_Machine.MachineName);
            string MACHINE_GR = nameof(C222_Machine.MachineGR);
            string MAKER = nameof(C222_Machine.Maker);
            string CHARGE = nameof(C222_Machine.Charge);
            string RATIO = nameof(C222_Machine.Ratio);
            string KAKOKIKAI = nameof(C222_Machine.kakokikai);
            string MULTI_PALET = nameof(C222_Machine.MultiPalet);
            string MGROUP = nameof(C222_Machine.MGroup);
            string SGROUP = nameof(C222_Machine.SGroup);
            string TRANFER = nameof(C222_Machine.tranfer);
            string MACHINE_GROUP = nameof(C222_Machine.MachineGroup);
            string MACHINE_WTS = nameof(C222_Machine.MachineWTS);
            string ORIGINALMACHINE = nameof(C222_Machine.OriginalMachine);
            string ACTIVE = nameof(C222_Machine.Active);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if (values.Contains(ORIGINALMACHINE))
            {
                model.OriginalMachine = Convert.ToString(values[ORIGINALMACHINE]);
            }
            if (values.Contains(MACHINE_ID)) {
                model.MachineID = Convert.ToString(values[MACHINE_ID]);
            }

            if(values.Contains(MAKER_MACHINE_NO)) {
                model.MakerMachineNo = Convert.ToString(values[MAKER_MACHINE_NO]);
            }

            if(values.Contains(ACC_NAME)) {
                model.AccName = Convert.ToString(values[ACC_NAME]);
            }

            if(values.Contains(DENSAN_NAME)) {
                model.DensanName = Convert.ToString(values[DENSAN_NAME]);
            }

            if(values.Contains(DENSAN_CODE)) {
                model.DensanCode = Convert.ToString(values[DENSAN_CODE]);
            }

            if(values.Contains(DMACHINE)) {
                model.Dmachine = Convert.ToString(values[DMACHINE]);
            }

            if(values.Contains(MACHINE_NAME)) {
                model.MachineName = Convert.ToString(values[MACHINE_NAME]);
            }

            if(values.Contains(MACHINE_GR)) {
                model.MachineGR = Convert.ToString(values[MACHINE_GR]);
            }

            if(values.Contains(MAKER)) {
                model.Maker = Convert.ToString(values[MAKER]);
            }

            if(values.Contains(CHARGE)) {
                model.Charge = Convert.ToDecimal(values[CHARGE]);
            }

            if(values.Contains(RATIO)) {
                model.Ratio = Convert.ToDouble(values[RATIO]);
            }

            if(values.Contains(KAKOKIKAI)) {
                model.kakokikai = Convert.ToBoolean(values[KAKOKIKAI]);
            }

            if(values.Contains(MULTI_PALET)) {
                model.MultiPalet = Convert.ToBoolean(values[MULTI_PALET]);
            }

            if(values.Contains(MGROUP)) {
                model.MGroup = Convert.ToString(values[MGROUP]);
            }

            if(values.Contains(SGROUP)) {
                model.SGroup = Convert.ToString(values[SGROUP]);
            }

            if(values.Contains(TRANFER)) {
                model.tranfer = Convert.ToBoolean(values[TRANFER]);
            }

            if(values.Contains(MACHINE_GROUP)) {
                model.MachineGroup = Convert.ToString(values[MACHINE_GROUP]);
            }

            if(values.Contains(MACHINE_WTS)) {
                model.MachineWTS = Convert.ToString(values[MACHINE_WTS]);
            }

            if(values.Contains(ACTIVE)) {
                model.Active = Convert.ToBoolean(values[ACTIVE]);
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