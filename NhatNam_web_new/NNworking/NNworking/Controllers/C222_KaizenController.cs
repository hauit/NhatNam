using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using NNworking.Models;
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
using System.Web.UI.WebControls.WebParts;

namespace NNworking.Controllers
{
    [Route("api/C222_Kaizen/{action}", Name = "C222_KaizenApi")]
    public class C222_KaizenController : ApiController
    {
        private NN_DatabaseEntities _context = new NN_DatabaseEntities();

        [HttpGet]
        public async Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions)
        {
            var c222_kaizen = _context.C222_Kaizen.Where(x => x.ID == 0).ToList();

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ID" };
            // loadOptions.PaginateViaPrimaryKey = true;
            return Request.CreateResponse(DataSourceLoader.Load(c222_kaizen, loadOptions));
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetDataTemplate(DataSourceLoadOptions loadOptions)
        {
            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            int kaizenStatus;
            string staffID = string.Empty;

            if (!queryParams.ContainsKey("StaffID)"))
            {
                staffID = queryParams["StaffID"];
            }

            if (!queryParams.ContainsKey("KaizenStatus)"))
            {
                kaizenStatus = 0;
            }

            if (!int.TryParse(queryParams["KaizenStatus"], out kaizenStatus))
            {
                kaizenStatus = 0;
            }
            else
            {
                kaizenStatus = Convert.ToInt32(queryParams["KaizenStatus"]);
            }

            var c222_kaizen = _context.C222_Kaizen.Where(x => x.ID == 0).ToList();
            if (kaizenStatus > 0)
            {
                GetDataByType(kaizenStatus, out c222_kaizen, staffID);
            }

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ID" };
            // loadOptions.PaginateViaPrimaryKey = true;
            return Request.CreateResponse(DataSourceLoader.Load(c222_kaizen, loadOptions));
        }

        private void GetDataByType(int dataType, out List<C222_Kaizen> c222_kaizen, string staffID)
        {
            switch (dataType)
            {
                case (int)StatusAfterAction.Pending:
                case (int)StatusAfterAction.Reject:
                case (int)StatusAfterAction.Approval:
                    GetKaizenByteType(out c222_kaizen, dataType);
                    break;
                case (int)StatusAfterAction.Personal:
                    GetPersonalKaizen(out c222_kaizen, staffID);
                    break;
                default:
                    c222_kaizen = _context.C222_Kaizen.Where(x => x.ID == 0).ToList();
                    break;
            }
        }

        private void GetPersonalKaizen(out List<C222_Kaizen> c222_kaizen, string staffID)
        {
            var data = _context.sp_222_Kaizen_GetDataForApproval(staffID)
                .Select(x => x.ID)
                .Distinct().ToList();
            c222_kaizen = _context.C222_Kaizen
                .Where(x => data.Contains(x.ID)).ToList();
        }

        private void GetKaizenByteType(out List<C222_Kaizen> c222_kaizen, int dataType)
        {
            var pendingInstance = _context.C222_WorkFolowInstance
                .Where(x => x.ModuleName == KaizenController.ModuleName && x.Status == dataType)
                .Select(x => x.ItemID)
                .Distinct();
            c222_kaizen = _context.C222_Kaizen
                .Where(x => pendingInstance.Contains(x.ID)).ToList();
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post(FormDataCollection form)
        {
            var model = new C222_Kaizen();
            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    model.InputDate = DateTime.Now;
                    model.BeforeApplied = string.Empty;
                    model.AfterApplied = string.Empty;
                    model.KaizenEffectiveness = string.Empty;
                    var result = _context.C222_Kaizen.Add(model);
                    await _context.SaveChangesAsync();
                    BaseModel.InputWorkFolloIntance(_context, model.StaffID, result.ID, KaizenController.ModuleName);
                    await _context.SaveChangesAsync();

                    transaction.Commit();
                    return Request.CreateResponse(HttpStatusCode.Created, new { result.ID });
                }
                catch
                {
                    transaction.Rollback();
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Không nhập được dữ liệu. Vui lòng liên hệ Admin");
                }
            }
        }

        [HttpPut]
        public async Task<HttpResponseMessage> Put(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var model = await _context.C222_Kaizen.FirstOrDefaultAsync(item => item.ID == key);
            if (model == null)
                return Request.CreateResponse(HttpStatusCode.Conflict, "Object not found");

            var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            PopulateModel(model, values);

            Validate(model);
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();
            //// Check and update workflow history if needed

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete]
        public async Task Delete(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));
            var model = await _context.C222_Kaizen.FirstOrDefaultAsync(item => item.ID == key);

            _context.C222_Kaizen.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(C222_Kaizen model, IDictionary values)
        {
            string ID = nameof(C222_Kaizen.ID);
            string INPUT_DATE = nameof(C222_Kaizen.InputDate);
            string STAFF_ID = nameof(C222_Kaizen.StaffID);
            string SUBJECT = nameof(C222_Kaizen.Subject);
            string PART_ID = nameof(C222_Kaizen.PartID);
            string OPTION_ID = nameof(C222_Kaizen.OptionID);
            string KAIZEN_TYPE = nameof(C222_Kaizen.KaizenType);
            string NOTE = nameof(C222_Kaizen.Note);
            string CURRENT_PROCESS = nameof(C222_Kaizen.CurrentProcess);
            string KAIZEN_PROCESS = nameof(C222_Kaizen.KaizenProcess);
            string APPLIED_PREDICT_RESULT = nameof(C222_Kaizen.AppliedPredictResult);
            string MANAGER_COMMENT = nameof(C222_Kaizen.ManagerComment);
            string KAIZEN_DEPT_COMMENT = nameof(C222_Kaizen.KaizenDeptComment);

            if (values.Contains(ID))
            {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if (values.Contains(INPUT_DATE))
            {
                model.InputDate = Convert.ToDateTime(values[INPUT_DATE]);
            }

            if (values.Contains(STAFF_ID))
            {
                model.StaffID = Convert.ToString(values[STAFF_ID]);
            }

            if (values.Contains(SUBJECT))
            {
                model.Subject = Convert.ToString(values[SUBJECT]);
            }

            if (values.Contains(PART_ID))
            {
                model.PartID = Convert.ToString(values[PART_ID]);
            }

            if (values.Contains(OPTION_ID))
            {
                model.OptionID = Convert.ToString(values[OPTION_ID]);
            }

            if (values.Contains(KAIZEN_TYPE))
            {
                model.KaizenType = values[KAIZEN_TYPE] != null ? Convert.ToInt32(values[KAIZEN_TYPE]) : (int?)null;
            }

            if (values.Contains(NOTE))
            {
                model.Note = Convert.ToString(values[NOTE]);
            }

            if (values.Contains(CURRENT_PROCESS))
            {
                model.CurrentProcess = Convert.ToString(values[CURRENT_PROCESS]);
            }

            if (values.Contains(KAIZEN_PROCESS))
            {
                model.KaizenProcess = Convert.ToString(values[KAIZEN_PROCESS]);
            }

            if (values.Contains(APPLIED_PREDICT_RESULT))
            {
                model.AppliedPredictResult = Convert.ToString(values[APPLIED_PREDICT_RESULT]);
            }

            if (values.Contains(MANAGER_COMMENT))
            {
                model.ManagerComment = Convert.ToString(values[MANAGER_COMMENT]);
            }

            if (values.Contains(KAIZEN_DEPT_COMMENT))
            {
                model.KaizenDeptComment = Convert.ToString(values[KAIZEN_DEPT_COMMENT]);
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