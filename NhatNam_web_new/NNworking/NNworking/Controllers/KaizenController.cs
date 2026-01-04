using NNworking.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace NNworking.Controllers
{
    public class KaizenController : Controller
    {
        public static string ModuleName
        {
            get { return nameof(KaizenController); }
        }

        // GET: Kaizen
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Input workflow for this module
        /// </summary>
        /// <returns></returns>
        public ActionResult WorkFolowSetup()
        {
            ViewBag.ModuleName = ModuleName;
            return View();
        }

        /// <summary>
        /// List kaizen
        /// </summary>
        /// status: status of kaizen to filter
        /// <returns></returns>
        public ActionResult KaiZenDetail(int id)
        {
            string staffID = Session["StaffID"].ToString().Trim();
            using (NN_DatabaseEntities _context = new NN_DatabaseEntities())
            {
                var kaizen = _context.C222_Kaizen.Where(k => k.ID == id).FirstOrDefault();
                if (kaizen == null)
                {
                    return HttpNotFound();
                }
                var historyApp = GetApprovalHistory(kaizen.ID, _context);
                var kaizenTemp = new C222_Kaizen();
                kaizenTemp.InputDate = kaizen.InputDate;
                kaizenTemp.Note = $@"Nhập cải tiến bởi {kaizen.StaffID}";
                kaizenTemp.ID = -2;
                historyApp.Insert(0, kaizenTemp);

                ViewBag.HisoryApp = historyApp;
                List<string> editableData = GetEditableData(staffID, kaizen);
                ViewBag.EdiableData = editableData;
                ViewBag.Viewer = staffID;
                ViewBag.KaizenID = kaizen.ID;
                ViewBag.InputDate = kaizen.InputDate.ToString("dd/MM/yyyy");
                ViewBag.Subject = kaizen.Subject;
                ViewBag.StaffID = kaizen.StaffID;
                var staffInfor = _context.C222_Staff.Where(s => s.StaffID == kaizen.StaffID).FirstOrDefault();
                if (staffInfor == null)
                {
                    ViewBag.StaffName = string.Empty;
                    ViewBag.DepartmentID = string.Empty;
                    ViewBag.DeptCode = string.Empty;
                }
                else
                {
                    ViewBag.StaffName = staffInfor.StaffName;
                    ViewBag.DepartmentID = staffInfor.DepartmentID;
                    ViewBag.DeptCode = staffInfor.DeptCode;
                }
                ViewBag.PartID = kaizen.PartID;
                ViewBag.OptionID = kaizen.OptionID;
                ViewBag.KaizenType = kaizen.KaizenType;
                ViewBag.Note = kaizen.Note;
                ViewBag.CurrentProcess = kaizen.CurrentProcess;
                ViewBag.KaizenProcess = kaizen.KaizenProcess;
                ViewBag.AppliedPredictResult = kaizen.AppliedPredictResult;
                ViewBag.ManagerComment = kaizen.ManagerComment;
                ViewBag.TechnicianComment = kaizen.TechnicianComment;
                ViewBag.KaizenDeptComment = kaizen.KaizenDeptComment;
                ViewBag.BeforeApplied = kaizen.BeforeApplied;
                ViewBag.AfterApplied = kaizen.AfterApplied;
                ViewBag.KaizenEffectiveness = kaizen.KaizenEffectiveness;
            }
            return View("KaizenDetail2");
        }

        private List<C222_Kaizen> GetApprovalHistory(int iD, NN_DatabaseEntities _context)
        {
            var data = (from a in _context.C222_WorkFolowInstance
                       join b in _context.C222_WorkFolowInstanceHistory
                           on new {a.ID, a.ItemID} equals new { ID = (int)b.InstanceID, ItemID = iD }
                       select new
                       {
                           b.ActionDate,
                           b.StepAction,
                           b.ActionBy ,
                           b.StatusAfterAction,
                           a.Status
                       }).ToList();
            List<C222_Kaizen> result = new List<C222_Kaizen>();
            if (data.Count == 0)
            {
                return result;
            }

            for (int i = 0; i < data.Count; i ++)
            {
                C222_Kaizen kaizen = new C222_Kaizen();
                kaizen.InputDate = (DateTime)data[i].ActionDate;
                string note = $@"Cải tiến đã {(data[i].StepAction == (int)StatusAfterAction.Approval ? "Chấp nhận" : "từ chối")} bởi {data[i].ActionBy}";
                kaizen.Note = note;
                kaizen.ID = (int)data[i].StatusAfterAction;
                result.Add(kaizen);
                    
                if(i == (data.Count - 1) && data[i].Status == (int)StatusAfterAction.Pending)
                {
                    C222_Kaizen kaizenLast = new C222_Kaizen();
                    kaizenLast.InputDate = DateTime.Now.Date.AddMonths(10);
                    kaizenLast.Note = "Đang chờ";
                    kaizenLast.ID = -1;
                    result.Add(kaizenLast);
                }
            }

            return result;
        }

        private List<string> GetEditableData(string viewer, C222_Kaizen kaizen)
        {
            List<string> result = new List<string>();
            using (NN_DatabaseEntities _context = new NN_DatabaseEntities())
            {
                //var data = _context.C222_WorkFolowInstance.Where(item => item.ID == kaizen.ID).FirstOrDefault();
                //var data1 = _context.C222_WorkFolowStep.Where(item => item.ID == data.CurrentStep).FirstOrDefault();
                //string alo = ModuleName;
                var data = (from a in _context.C222_WorkFolowInstance
                            join b in _context.C222_WorkFolowStep
                                on new { a.ItemID, a.WorkFollow, a.CurrentStep, a.ModuleName } equals new { ItemID = kaizen.ID, WorkFollow = b.WorkFollowID, CurrentStep = b.StepOder, ModuleName = KaizenController.ModuleName }
                            join c in _context.C222_WorkFolowRole
                                 on b.RoleID equals c.ID into roleGroup
                            from c in roleGroup.DefaultIfEmpty()
                            select new
                            {
                                CurrentStep = a.CurrentStep,
                                ManagerCheck = b.ManagerCheck,
                                OptionStep = b.OptionStep,
                                StaffID = c.StaffID
                            }).FirstOrDefault();
                if (data.ManagerCheck == true)
                {
                    var per = _context.C222_Staff.Where(x => (x.StaffID == kaizen.StaffID) && (x.ngduyet == viewer || x.ngduyet2 == viewer || x.ngduyet3 == viewer)).Any();
                    if (per)
                    {
                        return new List<string>() { "ManagerComment" };
                    }
                }

                if (data.OptionStep == true)
                {
                    // Chỗ này là bộ phận kỹ thuật
                    var per = _context.C222_Staff.Where(x => x.ngduyet == viewer || x.ngduyet2 == viewer || x.ngduyet3 == viewer).Any();
                    if (per)
                    {
                        return new List<string>() { "TechnicianComment" };
                    }
                }

                //var data2 = _context.C222_WorkFolowRole.Where(x => x.ID == data1.RoleID).FirstOrDefault();
                if (data.StaffID.IndexOf(viewer) != -1)
                {
                    return new List<string>() { "KaizenDeptComment" };
                }
            }
            return result;
        }

        public ActionResult KaiZenAdd()
        {
            ViewBag.KaizenStatus = 0;
            return View();
        }

        public ActionResult KaizenList(int kaizenStatus)
        {
            ViewBag.KaizenStatus = kaizenStatus;
            return View();
        }

        public JsonResult KaiZenApprove(string viewer, C222_Kaizen data, List<string> editedData, bool approval)
        {
            try
            {
                //Kiem tra xem viewer co phia la nguoi cdc phep phe duyet cho step hien tai hay khong
                CheckViewerPermission(viewer, data);
                // luu lai comment cua viewer
                using (NN_DatabaseEntities _context = new NN_DatabaseEntities())
                {
                    var model = _context.C222_Kaizen.Where(item => item.ID == data.ID).FirstOrDefault();
                    if (model == null)
                    {
                        throw new ArgumentException($"Không tìm thấy kaizen có id {data.ID}");
                    }

                    PopulateModel(model, data, editedData);
                    C222_WorkFolowInstanceHistory history = new C222_WorkFolowInstanceHistory();
                    history.Commment = string.Empty;
                    history.ActionBy = viewer;
                    history.ActionDate = DateTime.Now;
                    history.ModuleName = ModuleName;
                    var instance = (from a in _context.C222_WorkFolowInstance
                                    join b in _context.C222_WorkFolowStep
                                        on new { a.WorkFollow, a.CurrentStep, a.ItemID, a.ModuleName } equals new { WorkFollow = b.WorkFollowID, CurrentStep = b.StepOder, ItemID = data.ID, ModuleName = KaizenController.ModuleName }
                                    select new
                                    {
                                        InstanceID = a.ID,
                                        WorkFollow = a.WorkFollow,
                                        CurrentStep = a.CurrentStep,
                                        LastStep = b.IsFinal
                                    }).FirstOrDefault();
                    var instanceObject = _context.C222_WorkFolowInstance.Find(instance.InstanceID);
                    if (approval)
                    {
                        history.StepAction = (int)StatusAfterAction.Approval;
                        //Neu gia tri la approve thì update current step +1 trong workflow instance
                        history.StatusAfterAction = (int)StatusAfterAction.Approval;
                        if (!instance.LastStep)
                        {
                            history.StatusAfterAction = CalculateNextStep(model, instanceObject);
                        }
                        else
                        {
                            instanceObject.Status = (int)StatusAfterAction.Approval;
                        }


                    }
                    else
                    {
                        history.StepAction = (int)StatusAfterAction.Reject;
                        history.StatusAfterAction = (int)StatusAfterAction.Reject;
                    }
                    //Luu lai workflow history ve viec approve nay
                    //TODO: kiểm tra lại trường hợp nếu đã đánh giá rồi thì không được đánh giá nữa
                    history.StepID = instance.CurrentStep;
                    history.InstanceID = instance.InstanceID;
                    _context.C222_WorkFolowInstanceHistory.Add(history);
                    _context.SaveChanges();
                    return Json(new { Status = "OK", Values = "Cập nhật thành công" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = "Error occurred. Error details: ", Errors = string.Empty });
            }

        }

        private int CalculateNextStep(C222_Kaizen model, C222_WorkFolowInstance instanceObject)
        {
            using (NN_DatabaseEntities _context = new NN_DatabaseEntities())
            {
                while (true)
                {
                    instanceObject.CurrentStep += 1;
                    var stepObj = _context.C222_WorkFolowStep.Where(x => x.WorkFollowID == instanceObject.WorkFollow && x.StepOder == instanceObject.CurrentStep).FirstOrDefault();
                    if (stepObj == null)
                    {
                        instanceObject.CurrentStep -= 1;
                        instanceObject.Status = (int)StatusAfterAction.Approval;
                        return (int)StatusAfterAction.Approval;
                    }

                    if (stepObj.IsFinal)
                    {
                        return (int)StatusAfterAction.Pending;
                    }

                    //Khong can chek data exist vi da check o tren roi
                    if (!VerifyNewStep(model, instanceObject, stepObj))
                    {
                        continue;
                    }
                    else
                    {
                        return (int)StatusAfterAction.Pending;
                    }
                }
            }
        }

        private bool VerifyNewStep(C222_Kaizen model, C222_WorkFolowInstance instanceObject, C222_WorkFolowStep stepObj)
        {
            if (!stepObj.OptionStep)
            {
                return true;
            }

            if (!string.IsNullOrEmpty(model.PartID))
            {
                return true;
            }

            return false;
        }

        private void CheckViewerPermission(string viewer, C222_Kaizen data)
        {
            List<string> editableData = GetEditableData(viewer, data);
            if (editableData.Count <= 0)
            {
                throw new ArgumentException("Bạn không có quyền duyệt cải tiến");
            }

            ////TODO: truong hop ma current step da duoc phe duyet roi thi khong the phe duyet lai nen không can check
            //using (NN_DatabaseEntities _context1 = new NN_DatabaseEntities())
            //{
            //    var instanceObj = _context1.C222_WorkFolowInstance.Where(item => item.ItemID == data.ID).FirstOrDefault();
            //    if(instanceObj == null)
            //    {
            //        throw new ArgumentException("Không tìm thấy instance của workflow");
            //    }

            //    var instanceApproved = _context1.C222_WorkFolowInstanceHistory.Where(item => item.InstanceID == instanceObj.ID && item.StepID == instanceObj.CurrentStep).Any();
            //    if (instanceApproved)
            //    {
            //        throw new ArgumentException("Bước này đã được đánh giá. Không thể đánh giá lại");
            //    }
            //}
        }

        private void PopulateModel(C222_Kaizen model, C222_Kaizen data, List<string> editedData)
        {
            string MANAGER_COMMENT = nameof(C222_Kaizen.ManagerComment);
            string KAIZEN_DEPT_COMMENT = nameof(C222_Kaizen.KaizenDeptComment);
            string TechnicianComment = nameof(C222_Kaizen.TechnicianComment);

            if (editedData.Where(x => x.ToLower() == MANAGER_COMMENT.ToLower()).Any())
            {
                model.ManagerComment = data.ManagerComment;
            }

            if (editedData.Where(x => x.ToLower() == KAIZEN_DEPT_COMMENT.ToLower()).Any())
            {
                model.KaizenDeptComment = data.KaizenDeptComment;
            }

            if (editedData.Where(x => x.ToLower() == TechnicianComment.ToLower()).Any())
            {
                model.TechnicianComment = data.TechnicianComment;
            }
        }
    }

    public enum StatusAfterAction
    {
        Pending = 1,
        Approval = 2,
        Reject = 3
    }
}