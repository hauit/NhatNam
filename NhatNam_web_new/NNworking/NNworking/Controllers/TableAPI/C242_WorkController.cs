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
    [Route("api/C242_Work/{action}", Name = "C242_WorkApi")]
    public class C242_WorkController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public HttpResponseMessage Get(DataSourceLoadOptions loadOptions) {
            var c242_work = _context.C242_Work.Select(i => new {
                i.ID,
                i.WorkID,
                i.WorkName,
                i.DirectWork,
                i.MassWork,
                i.OptionDataWork,
                i.InspecWork,
                i.StopWork,
                i.GroupID,
                i.DeptCode,
                i.DeptOption,
                i.SortingNumber
            });
            return Request.CreateResponse(DataSourceLoader.Load(c242_work, loadOptions));
        }

        [HttpGet]
        public HttpResponseMessage GetOptionDept(DataSourceLoadOptions loadOptions)
        {
            var c242_work = _context.C242_Option.Select(i => new {
                i.OptionID
            }).ToList();

            List<C242_Option> data = new List<C242_Option>();
            foreach(var item in c242_work)
            {
                var obj = new C242_Option();
                obj.OptionID = item.OptionID.Substring(0, 1).ToUpper();
                var exited = data.Where(x => x.OptionID == obj.OptionID).Any();
                if(exited)
                {
                    continue;
                }

                data.Add(obj);
            }

            return Request.CreateResponse(DataSourceLoader.Load(data, loadOptions));
        }

        [HttpPost]
        public HttpResponseMessage Post(FormDataCollection form) {
            var model = new C242_Work();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            var result = _context.C242_Work.Add(model);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, result.ID);
        }

        [HttpPut]
        public HttpResponseMessage Put(FormDataCollection form) {
            var key = Convert.ToInt32(form.Get("key"));
            var model = _context.C242_Work.FirstOrDefault(item => item.ID == key);
            if(model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "C242_Work not found");

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
            var model = _context.C242_Work.FirstOrDefault(item => item.ID == key);

            _context.C242_Work.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(C242_Work model, IDictionary values) {
            string ID = nameof(C242_Work.ID);
            string WORK_ID = nameof(C242_Work.WorkID);
            string WORK_NAME = nameof(C242_Work.WorkName);
            string DIRECT_WORK = nameof(C242_Work.DirectWork);
            string MACH_WORK = nameof(C242_Work.MachWork);
            string MACH_IN_DWORK = nameof(C242_Work.MachInDWork);
            string MACHINE_RUN = nameof(C242_Work.MachineRun);
            string GROUP_ID = nameof(C242_Work.GroupID);
            string DEPT_CODE = nameof(C242_Work.DeptCode);
            string SORTING_NUMBER = nameof(C242_Work.SortingNumber);
            string OptionDataWork = nameof(C242_Work.OptionDataWork);
            string MassWork = nameof(C242_Work.MassWork);
            string StopWork = nameof(C242_Work.StopWork);
            string InspecWork = nameof(C242_Work.InspecWork);
            string DeptOption = nameof(C242_Work.DeptOption);

            if (values.Contains(ID)) {
                //model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(WORK_ID)) {
                model.WorkID = Convert.ToString(values[WORK_ID]);
            }

            if (values.Contains(DeptOption))
            {
                model.DeptOption = Convert.ToString(values[DeptOption]);
            }

            if (values.Contains(WORK_NAME)) {
                model.WorkName = Convert.ToString(values[WORK_NAME]);
            }

            if(values.Contains(DIRECT_WORK)) {
                model.DirectWork = Convert.ToBoolean(values[DIRECT_WORK]);
            }

            if (values.Contains(InspecWork))
            {
                model.InspecWork = Convert.ToBoolean(values[InspecWork]);
            }

            if (values.Contains(OptionDataWork))
            {
                model.OptionDataWork = Convert.ToBoolean(values[OptionDataWork]);
            }

            if (values.Contains(MassWork))
            {
                model.MassWork = Convert.ToBoolean(values[MassWork]);
            }

            if (values.Contains(StopWork))
            {
                model.StopWork = Convert.ToBoolean(values[StopWork]);
            }

            if (values.Contains(MACH_WORK)) {
                model.MachWork = Convert.ToBoolean(values[MACH_WORK]);
            }

            if(values.Contains(MACH_IN_DWORK)) {
                model.MachInDWork = Convert.ToBoolean(values[MACH_IN_DWORK]);
            }

            if(values.Contains(MACHINE_RUN)) {
                model.MachineRun = Convert.ToBoolean(values[MACHINE_RUN]);
            }

            if(values.Contains(GROUP_ID)) {
                model.GroupID = Convert.ToString(values[GROUP_ID]);
            }

            if(values.Contains(DEPT_CODE)) {
                model.DeptCode = Convert.ToString(values[DEPT_CODE]);
            }

            if(values.Contains(SORTING_NUMBER)) {
                model.SortingNumber = Convert.ToInt32(values[SORTING_NUMBER]);
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