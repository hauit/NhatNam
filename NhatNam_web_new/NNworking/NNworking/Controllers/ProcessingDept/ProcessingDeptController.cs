using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NNworking.Hubs;
using NNworking.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Mvc;

namespace NNworking.Controllers.ProcessingDept
{
    [RoutePrefix("Phong-San-Xuat")]
    public class ProcessingDeptController : BaseController
    {
        // GET: ProcessingDept
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult MachineWTS()
        {
            CheckPermissAndRedirect();
            return View("~/Views/ProcessingDept/MachineWTS/MachineWTS.cshtml");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult MachinePlanning()
        {
            CheckPermissAndRedirect();
            return View();
        }

        public ActionResult ChatBetweenMachineAndPlan()
        {
            ViewBag.Title = "Trao đổi giữa nhân viên đứng máy với kế hoạch";
            return View();
        }

        [Route("Ke-hoach-gia-cong.html")]
        public ActionResult Khgc()
        {
            CheckPermissAndRedirect();
            return View();
        }

        [Route("Ban-giao-ca.html")]
        public ActionResult ShiftHandOver()
        {
            CheckPermissAndRedirect();
            return View();
        }

        [Route("yeu-cau-khac-phuc.html")]
        public ActionResult YCKP()
        {
            CheckPermissAndRedirect();
            return View();
        }

        [Route("Chi-tiet-nguy-co-gay-dao.html")]
        public ActionResult Confirm0194()
        {
            CheckPermissAndRedirect();
            ViewBag.Title = "Chi tiết cần kiểm soát việc gãy dao endchip25";
            return View();
        }

        [Route("Thong-ke-bao-cao.html")]
        public ActionResult ProcessingDeptReport()
        {
            CheckPermissAndRedirect();
            return View("~/Views/ProcessingDept/ProcessingDeptReport/ProcessingDeptReport.cshtml");

        }

        [Route("Thong-ke-hang-muc-anh-huong-den-TGTC.html")]
        public ActionResult StandTimeAffectation()
        {
            CheckPermissAndRedirect();
            return View("~/Views/ProcessingDept/ProcessingDeptReport/StandTimeAffectation.cshtml");

        }

        [Route("danh-sach-dao.html")]
        public ActionResult ToolList()
        {
            CheckPermissAndRedirect();
            return View("~/Views/ProcessingDept/ToolManagement/ToolList.cshtml");

        }

        [Route("muon-dao.html")]
        public ActionResult ToolBorrow()
        {
            CheckPermissAndRedirect();
            return View("~/Views/ProcessingDept/ToolManagement/ToolBorrow.cshtml");

        }

        [Route("tra-dao.html")]
        public ActionResult ToolReturn()
        {
            CheckPermissAndRedirect();
            return View("~/Views/ProcessingDept/ToolManagement/ToolReturn.cshtml");

        }

        [Route("Nhac-nho-cong-viec.html")]
        public ActionResult TechnicalWarningList()
        {
            CheckPermissAndRedirect();
            return View("~/Views/ProcessingDept/ToolManagement/ToolBorrow.cshtml");

        }

        public ActionResult ProcessingStatistic()
        {
            CheckPermissAndRedirect();
            return View("~/Views/ProcessingDept/ProcessingDeptReport/ProcessingStatistic.cshtml");
        }

        [Route("Danh-sach-order-qua-han.html")]
        public ActionResult OrderExpire()
        {
            CheckPermissAndRedirect();
            return View("~/Views/ProcessingDept/ProcessingDeptReport/ExpireOrder.cshtml");
        }

        public ActionResult DanhGiaThucTeGC()
        {
            CheckPermissAndRedirect();
            return View("DanhGiaThucTeGC");
        }

        public ActionResult FactoryEvaluationOverview()
        {
            CheckPermissAndRedirect();
            return View();
        }

        public ActionResult UrgentProcessingPlan()
        {
            CheckPermissAndRedirect();
            return View();
        }

        [HttpPost]
        public JsonResult PrePareForHandOver(string date, string shift, string repicient)
        {
            try
            {
                DateTime from;
                if (!DateTime.TryParse(date, out from))
                {
                    from = DateTime.ParseExact(date.Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture).Date;
                }
                
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                string handOverPersion = Session["StaffID"].ToString();
                var existed = db.C242_ShiftHandOver.Where(x => x.Date == from && x.Shift == shift && x.HandOverPersion == handOverPersion).ToList();
                if(existed.Count > 0)
                {
                    for(int i = 0; i < existed.Count; i++)
                    {
                        db.C242_ShiftHandOver.Remove(existed[i]);
                    }

                    db.SaveChanges();
                }

                var items = db.C242_HandOverItem.ToList();
                foreach(var item in items)
                {
                    var obj = new C242_ShiftHandOver();
                    obj.Date = from;
                    obj.Shift = shift;
                    obj.HandOverPersion = handOverPersion;
                    obj.HandOverItem = item.ID;
                    obj.HandOverRecipient = repicient;
                    obj.HandOverContent = string.Empty;
                    obj.HandOverRecipientConfirm = false;
                    db.C242_ShiftHandOver.Add(obj);
                }
                db.SaveChanges();

                return Json(new { Status = "OK", Values = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult HandOverConfirmation(string date, string shift)
        {
            try
            {
                DateTime from;
                if (!DateTime.TryParse(date, out from))
                {
                    from = DateTime.ParseExact(date.Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture).Date;
                }
                
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                string confirm = Session["StaffID"].ToString();
                var existed = db.C242_ShiftHandOver.Where(x => x.Date == from && x.Shift == shift && x.HandOverRecipient == confirm).ToList();
                if(existed.Count == 0)
                {
                    throw new ArgumentException("Không tìm thấy thông tin bàn giao ca. Vui lòng liên hệ quản lý ca trước!");
                }
                
                foreach(var item in existed)
                {
                    item.HandOverRecipientConfirm = true;
                }
                db.SaveChanges();

                return Json(new { Status = "OK", Values = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="machineID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetToolNeedToReTurn(string machineID)
        {
            try
            {
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var list = db.sp_242_ToolNeedToReturn(machineID).ToList();
                return Json(new { Status = "OK", Value = list }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetDataOfStandTimeAffectation(string fromDate, string toDate, string machineGroup, string machineID, string workID)
        {
            try
            {
                DateTime from;
                if (!DateTime.TryParse(fromDate, out from))
                {
                    from = DateTime.ParseExact(fromDate.Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture).Date;
                }

                DateTime to;
                if (!DateTime.TryParse(toDate, out to))
                {
                    to = DateTime.ParseExact(toDate.Substring(0, 24),
                                  "ddd MMM d yyyy HH:mm:ss",
                                  System.Globalization.CultureInfo.InvariantCulture).Date;
                }

                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var data = db.sp_242_StandTimeManualChecking_GetTotalForReport_BySubGroup_Detail(from,to).ToList();
                if (!string.IsNullOrEmpty(workID))
                {
                    data = data.Where(x => x.WorkID.ToUpper() == workID.ToUpper()).ToList();
                }

                if (!string.IsNullOrEmpty(machineID))
                {
                    data = data.Where(x => x.MachineID.ToUpper() == machineID.ToUpper()).ToList();
                }

                if (!string.IsNullOrEmpty(machineGroup))
                {
                    data = data.Where(x => x.MachineGroup.ToUpper() == machineGroup.ToUpper()).ToList();
                }

                var list = db.sp_242_StandTimeManualChecking_GetTotalForReport(from, to).ToList();
                return Json(new { Status = "OK", List = list, Detail = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult sp_242_StandTimeManualChecking_GetTotalForReport_BySubGroup(string fromDate, string toDate, string machineGroup, string machineID, string workID)
        {
            try
            {
                DateTime from;
                if (!DateTime.TryParse(fromDate, out from))
                {
                    from = DateTime.ParseExact(fromDate.Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture);
                }

                DateTime to;
                if (!DateTime.TryParse(toDate, out to))
                {
                    to = DateTime.ParseExact(toDate.Substring(0, 24),
                                  "ddd MMM d yyyy HH:mm:ss",
                                  System.Globalization.CultureInfo.InvariantCulture);
                }

                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var data = db.sp_242_StandTimeManualChecking_GetTotalForReport_BySubGroup_Detail(from, to).ToList();
                if (!string.IsNullOrEmpty(workID))
                {
                    data = data.Where(x => x.WorkID.ToUpper() == workID.ToUpper()).ToList();
                }

                if (!string.IsNullOrEmpty(machineID))
                {
                    data = data.Where(x => x.MachineID.ToUpper() == machineID.ToUpper()).ToList();
                }

                if (!string.IsNullOrEmpty(machineGroup))
                {
                    data = data.Where(x => x.MachineGroup.ToUpper() == machineGroup.ToUpper()).ToList();
                }

                var list = db.sp_242_StandTimeManualChecking_GetTotalForReport_BySubGroup(workID, from, to).ToList();
                return Json(new { Status = "OK", List = list, Detail = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetMachinePlanForFactory(string Shift, string MachineID, string date)
        {
            DateTime from;
            if (!DateTime.TryParse(date, out from))
            {
                from = DateTime.ParseExact(date.Substring(0, 24),
                          "ddd MMM d yyyy HH:mm:ss",
                          System.Globalization.CultureInfo.InvariantCulture);
            }

            NN_DatabaseEntities db = new NN_DatabaseEntities();
            if (from == null)
            {
                from = DateTime.Now;
            }

            var list = db.sp_GetMachinePlanForFactory_TGTC(from.Date, Shift, MachineID).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPartNeedConfirmBy0194(string Shift, string MachineID, string date)
        {
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataForCheckStandTime(int plannID, string wtsID)
        {
            try
            {
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var plan = db.C242_MachinePlanning.Where(x => x.ID == plannID).FirstOrDefault();
                if (plan == null)
                {
                    return Json(new { Status = "NG", Values = "Không tìm thấy kế hoạch" }, JsonRequestBehavior.AllowGet);
                }

                var currentCheck = db.sp_242_StandTimeManualChecking_Current(plannID, wtsID).ToList();
                string htmlCode = string.Empty;
                if (currentCheck.Count > 0)
                {
                    htmlCode = GenerateHTMLCodeForCheckingStandTime(currentCheck, plannID).Trim();
                    return Json(new { Status = "OK", Values = htmlCode, CheckList = currentCheck }, JsonRequestBehavior.AllowGet);
                }

                var affectationList = db.C242_StandTimeAffectationCode.Where(x => x.WorkID.ToUpper() == wtsID.ToUpper()).OrderBy(x => x.STT).ToList();
                if (affectationList.Count == 0)
                {
                    return Json(new { Status = "NG", Values = $"Không tìm thấy danh sách hạng mục của mã công việc:{wtsID}" }, JsonRequestBehavior.AllowGet);
                }

                htmlCode = GenerateHTMLCodeForCheckingStandTime(affectationList, plannID).Trim();
                return Json(new { Status = "OK", Values = htmlCode, CheckList = affectationList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CheckAndCreateUserForMachine(string machineID)
        {
            try
            {
                //NN123456
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var existedUser = db.C222_Users.Where(x => x.UserName.ToLower() == machineID.ToLower()).Any();
                if(existedUser)
                {
                    throw new ArgumentException("Đã có user, không thể tạo mới.");
                }

                var existedStaff = db.C222_Staff.Where(x => x.StaffID.ToLower() == machineID.ToLower()).Any();
                if(!existedStaff)
                {
                    var staff = new C222_Staff();
                    staff.StaffID = machineID;
                    staff.StaffName = machineID;
                    staff.CreateDate = DateTime.Now.AddYears(-20).Date;
                    staff.SecName = "PROCESSING";
                    staff.Sub_Group = "MachineID";
                    staff.SecID = "0242";
                    staff.GroupID = string.Empty;
                    db.C222_Staff.Add(staff);
                }

                var user = new C222_Users();
                user.StaffID = machineID;
                user.UserName = machineID;
                user.UserGroupID = 1000;
                user.DepartmentID = 0;
                user.Password = "m2macBU5Vc45n1fQY3YEGA==";
                db.C222_Users.Add(user);
                db.SaveChanges();
                return Json(new { Status = "OK", Values = "OK"}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SendReturnToolItem(C242_ToolReturn data)
        {
            try
            {
                //data.ReturnTool = true;
                //data.ReturnTime = DateTime.Now;
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                db.C242_ToolReturn.Add(data);

                //// Lấy machineid từ số phiếu mượn
                var borroNo = db.C242_ToolBorrow.Where(x => x.ID == data.BorrowNo).FirstOrDefault();
                if(borroNo != null)
                {
                    ////Thêm thông báo yêu cầu mượn dao
                    var notify = new WorkingNotifycation();
                    notify.FromClient = borroNo.MachineID;
                    notify.ToDevice = string.Empty;
                    notify.FromDevice = string.Empty;
                    notify.NotifyContent = $"Yêu cầu trả dao cho máy {borroNo.MachineID}";
                    notify.WorkTopic = "ToolBorrow";
                    notify.ToClient = "1556";
                    notify.NotifyCaption = $"Yêu cầu trả dao máy {borroNo.MachineID}";
                    db.WorkingNotifycations.Add(notify);
                }

                db.SaveChanges();
                return Json(new { Status = "OK", Values = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private string GenerateHTMLCodeForCheckingStandTime(List<sp_242_StandTimeManualChecking_Current_Result> currentCheck, int plannID)
        {
            string htmlCode = "";
            if (currentCheck.Count == 0)
            {
                return htmlCode;
            }

            htmlCode = CreateHeaderOfHtmlCode(currentCheck[0].WorkID);

            Dictionary<int, int> rowSpan = new Dictionary<int, int>();
            foreach (var item in currentCheck)
            {
                if (rowSpan.ContainsKey((int)item.STT))
                {
                    continue;
                }

                var number = currentCheck.Where(x => x.STT == item.STT).ToList().Count;
                rowSpan.Add((int)item.STT, number);
            }

            foreach (var item in currentCheck)
            {
                htmlCode += @"
                <tr>";
                if (rowSpan.ContainsKey((int)item.STT))
                {
                    htmlCode += $@"        <td rowspan='{rowSpan[(int)item.STT]}'>{item.STT}</td>        ";
                    htmlCode += $@"        <td rowspan='{rowSpan[(int)item.STT]}'>{item.Categories}</td>      ";
                    rowSpan.Remove((int)item.STT);
                }

                htmlCode += $@"        <td>{item.ContentText}</td>   ";
                htmlCode += $@"        <td>{item.Sub_WorkID}</td>   ";
                if (item.Checking)
                {
                    htmlCode += $@"        <td><input type='checkbox' id='CheckingStandTime_{item.WorkID}_{item.Sub_WorkID}' value='' checked onclick='CheckingStandTime('{plannID}', '{item.WorkID}', '{item.Sub_WorkID}')' /></td>   ";
                }
                else
                {
                    htmlCode += $@"        <td><input type='checkbox' id='CheckingStandTime_{item.WorkID}_{item.Sub_WorkID}' value='' onclick='CheckingStandTime('{plannID}', '{item.WorkID}', '{item.Sub_WorkID}')' /></td>   ";
                }

                htmlCode += @"    </tr>
                ";
            }

            htmlCode = CreateButtomOfHtmlCode(htmlCode, currentCheck[0].Leader, currentCheck[0].CheckingTime);
            return htmlCode;
        }

        private string CreateButtomOfHtmlCode(string htmlCode, string leader, DateTime checkingTime)
        {
            string date = $@"new Date({checkingTime.Year}, {checkingTime.Month - 1}, {checkingTime.Day}, {checkingTime.Hour},{checkingTime.Minute})";
            htmlCode += @" </tbody>
            </table><br>";

            htmlCode += $@" Dự kiến chạy:<div id='ExpectedProcessingTime'></div></br>
                Trưởng nhóm tiếp nhận:<input type='text' id='LeaderConfirm' value='" + leader + @"' /></br>
                ";
            htmlCode += @"
                <script>
                var now = new Date();
                $('#ExpectedProcessingTime').dxDateBox({
                        type: 'datetime',
                        value: " + date + @"
                    });
    
                </script>
                ";
            return htmlCode;
        }

        private string GenerateHTMLCodeForCheckingStandTime(List<C242_StandTimeAffectationCode> affectationList, int plannID)
        {
            string htmlCode = "";
            if (affectationList.Count == 0)
            {
                return htmlCode;
            }

            htmlCode = CreateHeaderOfHtmlCode(affectationList[0].WorkID);

            Dictionary<int, int> rowSpan = new Dictionary<int, int>();
            foreach (var item in affectationList)
            {
                if (rowSpan.ContainsKey(item.STT))
                {
                    continue;
                }

                var number = affectationList.Where(x => x.STT == item.STT).ToList().Count;
                rowSpan.Add(item.STT, number);
            }

            foreach (var item in affectationList)
            {
                htmlCode += @"
                <tr>";
                if (rowSpan.ContainsKey(item.STT))
                {
                    htmlCode += $@"        <td rowspan='{rowSpan[item.STT]}'>{item.STT}</td>        ";
                    htmlCode += $@"        <td rowspan='{rowSpan[item.STT]}'>{item.Categories}</td>      ";
                    rowSpan.Remove(item.STT);
                }

                htmlCode += $@"        <td>{item.ContentText}</td>   ";
                htmlCode += $@"        <td>{item.Sub_WorkID}</td>   ";
                htmlCode += $@"        <td><input type='checkbox' id='CheckingStandTime_{item.WorkID}_{item.Sub_WorkID}' value='' onclick='CheckingStandTime('{plannID}', '{item.WorkID}', '{item.Sub_WorkID}')' /></td>   ";

                htmlCode += @"    </tr>
                ";
            }

            htmlCode = CreateButtomOfHtmlCode(htmlCode);
            return htmlCode;
        }

        private string CreateButtomOfHtmlCode(string htmlCode)
        {
            htmlCode += @" </tbody>
            </table><br>";

            htmlCode += $@" Dự kiến chạy:<div id='ExpectedProcessingTime'></div></br>
                Trưởng nhóm tiếp nhận:<input type='text' id='LeaderConfirm' /></br>
                ";
            htmlCode += @"
                <script>
                var now = new Date();
                $('#ExpectedProcessingTime').dxDateBox({
                        type: 'datetime',
                        value: now
                    });
    
                </script>
                ";
            return htmlCode;
        }

        private string CreateHeaderOfHtmlCode(string workID)
        {
            return $@"<table class='table table-advance'>
                <thead>
                    <th colspan='5' style='text-align:center'><h4>{workID}</h4></th>
                </thead>
                <thead>
                    <th>STT</th>
                    <th>Hạng mục</th>
                    <th>Nội dung</th>
                    <th>Mã</th>
                    <th>Xác nhận</th>
                </thead>
                <tbody>";
        }

        public JsonResult GetToolListForPartProcessing(int id)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            try
            {
                var plan = db.C242_MachinePlanning.Find(id);
                //var toolListForProcess = db.C242_ListToolNeededForProcessing.Where(x => x.MachineID.ToLower() == plan.MayGC.ToLower() && x.OptionID == plan.TTNC && x.PartID.ToLower() == plan.TenChiTiet.ToLower()).ToList();
                var toolListForProcess = db.C242_ToolsNeedForProcessing.Where(x => x.MachineID.ToLower() == plan.MayGC.ToLower() && x.OptionID.ToLower() == plan.NC.ToLower() && x.PartNo.ToLower() == plan.TenChiTiet.ToLower()).ToList();
                return Json(new { Status = "OK", Value = toolListForProcess }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Value = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SendListToolForBorrow(C242_ToolBorrow toolBorrow, LinkedList<C242_ToolBorrowDetail> listDetail)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            try
            {
                DateTime date = DateTime.Now;
                bool nightShift = (date.Hour * 60 + date.Minute) <= 380 && (toolBorrow.Shift == "T2" || toolBorrow.Shift == "T3");
                if (nightShift)
                {
                    date = date.Date.AddDays(-1);
                }

                toolBorrow.Date = date.Date;
                toolBorrow.InputDate = DateTime.Now;
                db.C242_ToolBorrow.Add(toolBorrow);
                db.SaveChanges();
                var toolNeededForProcessing = db.C242_ToolsNeedForProcessing.Where(x => x.MachineID.ToLower() == toolBorrow.MachineID.ToLower()
                    && x.PartNo.ToLower() == toolBorrow.PartNo.ToLower()
                    && x.OptionID.ToLower() == toolBorrow.OptionID.ToLower()
                ).ToList();

                foreach (var item in listDetail)
                {
                    item.BorrowNo = toolBorrow.ID;
                    //// Lấy tool maker name => toolId: với điều kiện là quan hệ 1-1(quan hệ 1-n là chuôi, ko phải dao)
                    //// Nếu là mượn chuôi thì để trống toolID, bộ phận quan rlys dao sẽ tự nhập toolID  khi kiểm duyệt dữ liệu
                    var toolList = db.C333_Tool.Where(x => x.MakerToolName.ToLower() == item.MakerToolName.ToLower()).ToList();
                    if (toolList.Count == 1)
                    {
                        item.ToolDataID = toolList[0].ToolID;
                    }

                    db.C242_ToolBorrowDetail.Add(item);

                    //// Add vào bảng 333_Borrow để các báo cáo cũ ko bị ảnh hưởng ==> hủy. đợi lúc nào người quản lý xác nhận cho mượn thì mới nhận
                    C333_Borrow data = new C333_Borrow();
                    //db.C333_Borrow.Add(data);

                    //// Kiểm tra xem dữ liệu đã tồn tại trong "danh sách dao cần có để gia công 1 chi tiết" chưa. Nếu chưa thì nhập vào đấy để lần sau mượn dao đỡ phải nhập
                    //// Làm 1 danh sách mượn dao, lưu trữ nhưng thông tin mượn, trả dao của máy để nhân viên có thể tra cứu
                    //var existedData = toolNeededForProcessing.Where(x => x.TenDao.ToLower() == item.MakerToolName.ToLower()).Any();
                    //if (existedData)
                    //{
                    //    continue;
                    //}

                    var toolNeedForProcessing = new C242_ToolsNeedForProcessing();
                    //toolNeedForProcessing.KHDao = item.ToolName;
                    toolNeedForProcessing.MachineID = toolBorrow.MachineID;
                    toolNeedForProcessing.OptionID = toolBorrow.OptionID;
                    toolNeedForProcessing.PartNo = toolBorrow.PartNo;
                    //toolNeedForProcessing.TenDao = item.MakerToolName;
                    toolNeedForProcessing.ToolID = item.ToolDataID;
                    toolNeedForProcessing.Quantity = item.Quantity == null ? 0 : (int)item.Quantity;
                    toolNeedForProcessing.Note = item.Note;
                    db.C242_ToolsNeedForProcessing.Add(toolNeedForProcessing);
                }

                ////Thêm thông báo yêu cầu mượn dao
                var notify = new WorkingNotifycation();
                notify.FromClient = toolBorrow.MachineID;
                notify.ToDevice = string.Empty;
                notify.FromDevice = string.Empty;
                notify.NotifyContent = $"Yêu cầu mượn dao cho máy {toolBorrow.MachineID}, giờ mong muốn lấy dao: {toolBorrow.TimeNeedToGet}";
                notify.WorkTopic = "ToolBorrow";
                notify.ToClient = "1556";
                notify.NotifyCaption = $"Yêu cầu mượn dao máy {toolBorrow.MachineID}";
                db.WorkingNotifycations.Add(notify);

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    db.C242_ToolBorrow.Remove(toolBorrow);
                    return Json(new { Status = "NG", Value = ex.Message }, JsonRequestBehavior.AllowGet);
                }

                ////TODO: Cần gửi thông báo cho bộ phận quản lý dao để họ xác nhận thông tin và trả lời
                var context = GlobalHost.ConnectionManager.GetHubContext<TechnicalNotify>();
                context.Clients.Group(notify.ToClient).messageToToolManagement(notify); //// alert message để người quản lý dao biết
                context.Clients.Group(notify.ToClient).acceptNewNotify(notify); //// Điền thông báo lên hình cái chuông

                //// Sau khi bộ phận quản lý dao trả lời thì thông báo lại cho nhân viên ở máy gia công
                return Json(new { Status = "OK", Value = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Value = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetToolListForChoosing()
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            try
            {
                var toolList = db.C333_Tool.Select(x => x.MakerToolName).ToList();
                var toolListInfo = db.C333_Tool.ToList();
                return Json(new { Status = "OK", Value = "", ToolList = toolList, ToolListInfo = toolListInfo }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Value = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetExistedTool(string makerToolName, string machineID)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            try
            {
                var data = db.C333_Tool.Where(x => x.MakerToolName.ToLower() == makerToolName.ToLower()).ToList();
                if (data.Count == 0)
                {
                    return Json(new { Status = "OK", Value = "" }, JsonRequestBehavior.AllowGet);
                }

                if (data.Count == 1)
                {
                    string toolID = data[0].ToolID;
                    var toolListInfo = db.sp_333_Tool_GetListExistedToolInMachine(machineID, toolID);
                    return Json(new { Status = "OK", Value = toolListInfo }, JsonRequestBehavior.AllowGet);
                }

                ////TODO: kiem tra lai xem lam the nao de biet duoc la chuoi da buon hay chua
                return Json(new { Status = "OK", Value = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Value = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetShiftHaveMachinePlan(string machineid, string date)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            DateTime from;
            if (!DateTime.TryParse(date, out from))
            {
                from = DateTime.ParseExact(date.Substring(0, 24),
                          "ddd MMM d yyyy HH:mm:ss",
                          System.Globalization.CultureInfo.InvariantCulture);
            }
            if (from == null)
            {
                from = DateTime.Now;
            }

            var listShift = db.sp_GetShiftHaveMachinePlan(machineid, from).ToList();

            return Json(listShift, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataOfReport(int reportID, string from, string to)
        {
            try
            {
                DateTime fromDate;
                if (!DateTime.TryParse(from, out fromDate))
                {
                    fromDate = DateTime.ParseExact(from.Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture);
                }

                DateTime toDate;
                if (!DateTime.TryParse(to, out toDate))
                {
                    toDate = DateTime.ParseExact(to.Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture);
                }

                var data = GetReportDetailData(reportID, fromDate.Date, toDate.Date);
                return Json(new { Status = "OK", Values = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private dynamic GetReportDetailData(int reportID, DateTime fromDate, DateTime toDate)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            switch (reportID)
            {
                case 1:
                    return db.sp_242_ManualExcutionChecking_GetAll(fromDate, toDate).ToList();
                case 2:
                    return db.sp_242_ManualExcutionChecking_GetTotal(fromDate, toDate).ToList();
                case 3:
                    return db.sp_242_ManualExcutionChecking_DoesNotFollowWTSQty(fromDate, toDate).ToList();
            }

            return null;
        }

        public JsonResult GetDataForCheckExecution(bool heightPrice, int planID, string number)
        {
            try
            {
                int numberPart;
                if (!int.TryParse(number, out numberPart))
                {
                    throw new ArgumentException("Số chi tiết cho 1 lần check phải là kiểu số!");
                }

                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var plan = db.C242_MachinePlanning.Where(x => x.ID == planID).FirstOrDefault();
                var listShift = db.C242_ManualExcution.Where(x => x.HeightPrice == heightPrice).ToList();
                if (plan == null)
                {
                    throw new ArgumentException("Không tìm thấy kế hoạch!");
                }

                string mono = plan.Order;
                string partID = plan.TenChiTiet;
                string optionID = plan.NC;
                var currentCheck = db.sp_242_ManualExcutionChecking_GetChecked(mono, partID, optionID).ToList();
                currentCheck = CompareCheckedWithMOQty(currentCheck, plan, numberPart);
                Dictionary<int, string> listHtmlCode = CreateHtmlCode(currentCheck, plan, heightPrice, numberPart);
                var checkedData = (from c in currentCheck
                                   group c by new { c.MONo, c.PartNo, c.OptionID, c.PartNoIndex, c.Checkednumber, Checked = c.Checkednumber >= c.Checkingnumber, HtmlCode = string.Empty } into d
                                   select new
                                   {
                                       Index = d.Take(1).Select(x => x.PartNoIndex).FirstOrDefault(),
                                       Key = d.Key,
                                       Data = d
                                   }).ToList();

                checkedData.OrderBy(x => x.Index).ToList();
                //string result = CreateManualCheckPanel(listShift, currentCheck, heightPrice, plan);
                return Json(new { Status = "OK", Values = checkedData, HtmlCodes = listHtmlCode.Values.ToList() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private Dictionary<int, string> CreateHtmlCode(List<sp_242_ManualExcutionChecking_GetChecked_Result> currentCheck, C242_MachinePlanning plan, bool heightPrice, int numberPart)
        {
            Dictionary<int, string> listHtmlCode = new Dictionary<int, string>();
            if (currentCheck.Count == 0)
            {
                return listHtmlCode;
            }

            int slgLenh = (int)plan.Slglenh / numberPart;
            if (plan.Slglenh % numberPart > 0)
            {
                slgLenh += 1;
            }

            for (int i = 1; i <= slgLenh; i++)
            {
                string htmlCode = string.Empty;
                var listCurrentCheckForItem = currentCheck.Where(x => x.PartNoIndex == i).ToList();
                if (numberPart > 1)
                {
                    int start = (i - 1) * numberPart + 1;
                    int finish = numberPart * i;
                    listCurrentCheckForItem = currentCheck.Where(x => x.PartNoIndex >= start && x.PartNoIndex <= finish).ToList();
                }

                htmlCode = CreateHtmlcodeForItem(listCurrentCheckForItem, heightPrice, plan, i, numberPart);
                listHtmlCode.Add(i, htmlCode);
            }

            return listHtmlCode;
        }

        private string CreateHtmlcodeForItem(List<sp_242_ManualExcutionChecking_GetChecked_Result> listCurrentCheckForItem, bool heightPrice, C242_MachinePlanning plan, int partNoIndex, int numberPart)
        {
            string result = string.Empty;
            string index = partNoIndex.ToString();
            if (numberPart > 1)
            {
                index = $"{(partNoIndex - 1) * numberPart + 1}-{numberPart * partNoIndex}";
            }

            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var needCheck = db.C242_ManualExcution.Where(x => x.HeightPrice == heightPrice).ToList();
            result += "<div class=\"row\">                                           ";
            result += "    <div class=\"col-lg-3\">                                  ";
            if (heightPrice)
                result += $@"        Check các hạng mục với giá thành cao, chi tiết {listCurrentCheckForItem[0].PartNo}, order: {listCurrentCheckForItem[0].MONo}, nguyên công: {listCurrentCheckForItem[0].OptionID} cái thứ {index}/{plan.Slglenh}  ";
            else
                result += $@"        Check các hạng mục với giá thành thấp, chi tiết: {listCurrentCheckForItem[0].PartNo}, order: {listCurrentCheckForItem[0].MONo}, nguyên công: {listCurrentCheckForItem[0].OptionID} cái thứ {index}/{plan.Slglenh}  ";
            result += "    </div>                                                  ";
            result += "    <div class=\"col-lg-9\">                                  ";
            result += "        <table class=\"table table-advance\">                 ";
            result += "            <thead>                                         ";
            result += "            <th>STT</th>                                    ";
            result += "            <th>Hạng mục check thao tác</th>                ";
            result += "            <th>Dụng cụ</th>                                ";
            result += "            <th>Tích x/n</th>                               ";
            result += "            </thead>                                        ";
            result += "            <tbody>                                         ";
            var listResult = CreateFullListCheckingItem(listCurrentCheckForItem, needCheck);
            result += GenerateHtmlForOnePart(plan.ID, partNoIndex, listResult, needCheck);
            result += "            </tbody>                                        ";
            result += "        </table>                                            ";
            result += "    </div>                                                  ";
            result += "</div>                                                      ";

            return result;
        }

        private List<sp_242_ManualExcutionChecking_GetChecked_Result> CompareCheckedWithMOQty(List<sp_242_ManualExcutionChecking_GetChecked_Result> currentCheck, C242_MachinePlanning plan, int numberPart)
        {
            int slgLenh = (int)plan.Slglenh;
            //slgLenh = (int)plan.Slglenh / numberPart;
            //if (plan.Slglenh % numberPart > 0)
            //{
            //    slgLenh += 1;
            //}

            for (int i = 1; i <= slgLenh; i++)
            {
                var check = currentCheck.Where(x => x.PartNoIndex == i).Any();
                if (check)
                {
                    continue;
                }

                sp_242_ManualExcutionChecking_GetChecked_Result obj = new sp_242_ManualExcutionChecking_GetChecked_Result();
                obj.PartNoIndex = i;
                obj.MONo = plan.Order;
                obj.PartNo = plan.TenChiTiet;
                obj.OptionID = plan.NC;
                obj.MachineID = plan.MayGC;
                obj.StaffID = string.Empty;
                obj.Checkingconten = string.Empty;
                obj.Checkingnumber = 1;
                obj.CheckedContent = string.Empty;
                obj.Checkednumber = 0;
                currentCheck.Add(obj);
            }

            currentCheck = currentCheck.OrderBy(x => x.PartNoIndex).ToList();
            return currentCheck;
        }

        private string CreateManualCheckPanel(List<C242_ManualExcution> listItem, List<sp_242_ManualExcutionChecking_GetChecked_Result> currentCheck, bool heightPrice, C242_MachinePlanning plan)
        {
            listItem = listItem.OrderBy(x => x.ID).ToList();
            string result = string.Empty;
            int slg = (int)plan.Slglenh;
            int planID = (int)plan.ID;
            for (int i = 1; i <= slg; i++)
            {
                result += "<div class=\"row\">                                           ";
                result += "    <div class=\"col-lg-3\">                                  ";
                if (heightPrice)
                    result += $@"        Check các hạng mục với giá thành cao, chi tiết {plan.TenChiTiet}, order: {plan.Order}, nguyên công: {plan.NC} cái thứ {i}/{slg}  ";
                else
                    result += $@"        Check các hạng mục với giá thành thấp, chi tiết: {plan.TenChiTiet}, order: {plan.Order}, nguyên công: {plan.NC} cái thứ {i}/{slg}  ";
                result += "    </div>                                                  ";
                result += "    <div class=\"col-lg-9\">                                  ";
                result += "        <table class=\"table table-advance\">                 ";
                result += "            <thead>                                         ";
                result += "            <th>STT</th>                                    ";
                result += "            <th>Hạng mục check thao tác</th>                ";
                result += "            <th>Dụng cụ</th>                                ";
                result += "            <th>Tích x/n</th>                               ";
                result += "            </thead>                                        ";
                result += "            <tbody>                                         ";
                var alreadyCheck = currentCheck.Where(x => x.PartNoIndex == i && x.Checked == true).OrderBy(y => y.ExcutionConten).ToList();
                var listResult = CreateFullListCheckingItem(alreadyCheck, listItem);
                result += GenerateHtmlForOnePart(planID, i, listResult, listItem);
                result += "            </tbody>                                        ";
                result += "        </table>                                            ";
                result += "    </div>                                                  ";
                result += "</div>                                                      ";
            }
            result += "<input class=\"btn VP\" style=\"margin-top: 10px;\" type=\"Button\" tabindex=\"\" id=\"CheckOK\" name=\"\"  value=\"Xong\" onclick=\"InputManualChecking()\"> ";
            return result;
        }

        private string GenerateHtmlForOnePart(int planID, int partIndex, List<sp_242_ManualExcutionChecking_GetChecked_Result> listResult, List<C242_ManualExcution> listItem)
        {
            string result = string.Empty;
            for (int j = 0; j < listResult.Count; j++)
            {

                if (listResult[j] == null)
                {
                    result += "                <tr>                                        ";
                    result += "                    <td>" + (j + 1) + "</td>                               ";
                    result += "                    <td>" + listItem[j].CheckingContent + "</td>                               ";
                    result += "                    <td>" + listItem[j].CheckingTool + "</td>                               ";
                    result += $"                    <td><input type=\"checkbox\" id=\"manualCheck_{partIndex}_{j + 1}\" value=\"\" onclick=\"ManualCheck('{planID}', '{partIndex}', '{j + 1}', '{listItem[j].ID}')\" /></td>                               ";
                    result += "                </tr>                                       ";
                    continue;
                }

                result += "                <tr>                                        ";
                result += "                    <td>" + (j + 1) + "</td>                               ";
                result += "                    <td>" + listResult[j].CheckingContent + "</td>                               ";
                result += "                    <td>" + listResult[j].CheckingTool + "</td>                               ";
                result += $"                    <td><input type=\"checkbox\" id=\"manualCheck_{partIndex}_{j + 1}\" checked disabled='disabled' value=\"\" onclick=\"ManualCheck('{planID}', '{partIndex}', '{j + 1}', '{listResult[j].ExcutionConten}')\" /></td>                               ";
                result += "                </tr>                                       ";

            }

            return result;
        }

        private List<sp_242_ManualExcutionChecking_GetChecked_Result> CreateFullListCheckingItem(List<sp_242_ManualExcutionChecking_GetChecked_Result> alreadyCheck, List<C242_ManualExcution> listItem)
        {

            var listResult = new List<sp_242_ManualExcutionChecking_GetChecked_Result>();
            for (int j = 0; j < listItem.Count; j++)
            {
                var item = alreadyCheck.Where(x => x.ExcutionConten == listItem[j].ID).FirstOrDefault();
                listResult.Add(item);
            }

            return listResult;
        }

        [HttpPost]
        public JsonResult CheckingExecution(int plannID, bool check, int content, int partIndex, string staffID, string pass, string numberOfPart, string shift)
        {
            try
            {
                int numberPart;
                if (!int.TryParse(numberOfPart, out numberPart))
                {
                    throw new ArgumentException("Số chi tiết cho 1 lần check phải là kiểu số!");
                }

                if (!CheckUser(staffID, pass))
                {
                    throw new ArgumentException("Tài khoản hoặc mật khẩu không đúng!");
                }

                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var plann = db.C242_MachinePlanning.Where(x => x.ID == plannID).FirstOrDefault();
                if (plann == null)
                {
                    throw new ArgumentException("Không tìm thấy kế hoạch.");
                }

                if (numberPart > 1)
                {
                    partIndex = (partIndex - 1) * numberPart + 1;
                }

                for (int i = partIndex; i < numberPart + partIndex; i++)
                {
                    if (i > plann.Slglenh)
                    {
                        break;
                    }

                    InputData(check, content, i, staffID, plann, db, shift);
                }

                db.SaveChanges();
                return Json(new { Status = "OK", Values = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private void InputData(bool check, int content, int partIndex, string staffID, C242_MachinePlanning plann, NN_DatabaseEntities db, string shift)
        {
            DateTime date = DateTime.Now;
            bool nightShift = (date.Hour * 60 + date.Minute) <= 380 && (shift == "T2" || shift == "T3");
            if (nightShift)
            {
                date = date.Date.AddDays(-1);
            }

            var data = new C242_ManualExcutionChecking();
            data.Shift = shift;
            data.Date = date;
            data.Checked = check;
            data.CheckTime = DateTime.Now;
            data.ExcutionConten = content;
            data.MONo = plann.Order;
            data.MOQty = (int)plann.Slglenh;
            data.OptionID = plann.NC;
            data.MachineID = plann.MayGC;
            data.PartNo = plann.TenChiTiet;
            data.PartNoIndex = partIndex;
            data.StaffID = staffID;
            data.Tool = string.Empty;
            db.C242_ManualExcutionChecking.Add(data);
        }

        public JsonResult CheckStandTimeFinish(int plannID, string[] listResult, string leader, string expectedProcessingTime, string pass, string staffID)
        {
            try
            {
                if (!CheckUser(staffID, pass))
                {
                    throw new ArgumentException("Tài khoản hoặc mật khẩu không đúng!");
                }

                DateTime processingTime = DateTime.ParseExact(expectedProcessingTime.Substring(0, 24),
                                "ddd MMM d yyyy HH:mm:ss",
                                System.Globalization.CultureInfo.InvariantCulture);

                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var plann = db.C242_MachinePlanning.Where(x => x.ID == plannID).FirstOrDefault();
                foreach (var item in listResult)
                {
                    var b = item.Replace("\"", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);
                    var a = b.Split(',');
                    string workID = a[0];
                    string sub_WorkID = a[1];
                    C242_StandTimeManualChecking obj = db.C242_StandTimeManualChecking.Where(x => x.PlannID == plannID && x.WorkID == workID && x.Sub_WorkID == sub_WorkID).FirstOrDefault();
                    if (obj == null)
                    {
                        obj = new C242_StandTimeManualChecking();
                        db.C242_StandTimeManualChecking.Add(obj);
                    }

                    obj.Date = plann.Date.Value;
                    obj.Shift = plann.Shift;
                    obj.MachineID = plann.MayGC;
                    obj.PartID = plann.TenChiTiet;
                    obj.OptionID = plann.NC;
                    obj.PlannID = plannID;
                    obj.MONo = plann.Order;
                    obj.WorkID = workID;
                    obj.Sub_WorkID = sub_WorkID;
                    obj.Staff = Session["StaffID"].ToString().Trim().ToUpper();
                    obj.Leader = leader;
                    obj.Checking = bool.Parse(a[2]);
                    obj.CheckingTime = DateTime.Now;
                    obj.ExpectedProcessingTime = processingTime;
                }

                db.SaveChanges();
                var context = GlobalHost.ConnectionManager.GetHubContext<TechnicalNotify>();
                context.Clients.All.ReloadStandTimeManualChecking();

                return Json(new { Status = "OK", Values = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult NeedToMakeExcutionCheck(int id)
        {
            try
            {
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                string staffID = Session["StaffID"].ToString().Trim().ToUpper();
                var leader = db.C222_Staff.Where(x => x.StaffID == staffID && x.level >= 2 && x.SecID == "0242" && x.Sub_Group.ToLower() == "machine").ToList();
                if (leader.Count == 0)
                {
                    throw new ArgumentException("Bạn không có quyền yêu cầu nhân viên check thao tác.");
                }

                var plann = db.C242_MachinePlanning.Where(x => x.ID == id).FirstOrDefault();
                if (plann == null)
                {
                    throw new ArgumentException("Không tìm thấy kế hoạch.");
                }

                plann.Fac_ThaoTac = true;
                db.SaveChanges();

                return Json(new { Status = "OK", Values = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private bool CheckUser(string staffID, string pass)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            clsBase check = new clsBase();
            pass = check.EncryptPassword(pass, "Ktd@");
            var a = (from c in db.C222_Users where c.UserName != c.StaffID && c.UserName == staffID && c.Password == pass select c).ToList();

            if (a.Count == 0)
            {
                return false;
            }

            return true;
        }

        [HttpPost]
        public JsonResult ListMachineByGroup(string Group, string shift)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            DateTime date = DateTime.Now;
            bool nightShift = (date.Hour * 60 + date.Minute) <= 380 && (shift == "T2" || shift == "T3");
            if (nightShift)
            {
                date = date.Date.AddDays(-1);
            }

            string groupName = string.Empty;
            var ListMachine1 = db.sp_AcquireMachine_CompareWithPlan(Group, date.Date, shift).ToList();
            return Json(ListMachine1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListMachineByGroupInMachineList(string Group)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var ListMachine1 = db.C222_Machine.Where(x=>x.MachineGroup.ToLower() == Group.ToLower()).ToList();
            return Json(ListMachine1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListMachineHaveChatHistoryByGroup(string Group)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var ListMachine1 = db.sp_AcquireMachineChatHistory(Group).ToList();
            return Json(ListMachine1, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMachineChatHistory(string machineID, string date)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            try
            {
                DateTime from;
                if (!DateTime.TryParse(date, out from))
                {
                    from = DateTime.ParseExact(date.Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture);
                }

                from = from.AddDays(-1);
                var data = db.WorkingNotifycations.Where(x => (x.FromClient.ToLower() == machineID.ToLower() || x.ToClient.ToLower() == machineID.ToLower()) && x.NotifyTime >= from && x.WorkTopic.ToLower() == "chat").ToList();
                return Json(new { Status = "OK", Values = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ListMachineGroup()
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var data = db.C222_GroupMachine.Select(x => x.GroupName).ToList();
            var ListGroupMachine = new List<string>() { "Phay 1", "Phay 2", "Tiện", "Đột dập", "Via" };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Machine
        /// </summary>
        /// <param name="StaffID"></param>
        /// <param name="Shift"></param>
        /// <param name="MachineID"></param>
        /// <returns></returns>
        public JsonResult GetMachinePlanForCheckMachineProcess(string Shift, string MachineID, string date)
        {

            DateTime fromDate;
            if (!DateTime.TryParse(date, out fromDate))
            {
                fromDate = DateTime.Now;
            }

            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var list = db.sp_GetResultOfCheckMachineProcess(fromDate.Date, Shift, MachineID).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult sp_GetAllMachineWTS_Cache()
        {
            try
            {
                DateTime fromDate = DateTime.Now;
                DateTime toDate = DateTime.Now;
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                //var a = db.sp_GetAllMachineWWTS_Cache(fromDate.Date, toDate.Date).ToList();
                return Json(new { Status = "NG", Values = "Tên nhóm không đúng" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Upload(string dept, string shift, string date)
        {
            if (Request.Files.Count == 0)
            {
                return Json("No file to upload", JsonRequestBehavior.AllowGet);
            }

            DateTime from;
            if (!DateTime.TryParse(date, out from))
            {
                from = DateTime.ParseExact(date.Substring(0, 24),
                          "ddd MMM d yyyy HH:mm:ss",
                          System.Globalization.CultureInfo.InvariantCulture);
            }
            var myFile = Request.Files[0];
            var targetLocation = Server.MapPath("~/Files/");
            List<string> listError = new List<string>();
            try
            {
                var time = DateTime.Now.ToFileTimeUtc();
                var path = Path.Combine(targetLocation, time + ".xlsx");
                var exten = Path.GetExtension(path).Replace(".", string.Empty);
                if (exten.ToLower() != "xls" && exten.ToLower() != "xlsx")
                {
                    throw new ArgumentException("Chỉ import dữ liệu từ file Excel.");
                }

                //Uncomment to save the file
                myFile.SaveAs(path);
                ImportDataFromExcel(path, dept, shift, from);
                System.IO.File.Delete(path);
            }
            catch (Exception ex)
            {
                return Json(new { status = "NG", value = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { status = "OK", value = "Import ok" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult MachinePlanningValidateRow(sp_GetMachinePlanForFactory_TGTC_Result data, string dataNew)
        {
            try
            {
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var dataValue = db.C242_MachinePlanning.Find(data.ID);
                dataValue = ValidateValue(dataValue, dataNew);
                db.SaveChanges();
                return Json(new { Status = "OK", Values = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetFactoryEvaluationOverview(string date, string shift, string machine)
        {
            try
            {
                DateTime from;
                if (!DateTime.TryParse(date, out from))
                {
                    from = DateTime.ParseExact(date.Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture);
                }

                bool nightShift = (from.Hour * 60 + from.Minute) <= 380 && (shift == "T2" || shift == "T3");
                if (nightShift)
                {
                    from = from.Date.AddDays(-1);
                }

                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var data = db.sp_242_MachinePlanning_GetValuationOverview(shift, from.Date, machine).ToList();
                return Json(new { Status = "OK", Values = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUrgentProcessingPlan(string date, string shift, string group)
        {
            try
            {
                DateTime from;
                if (!DateTime.TryParse(date, out from))
                {
                    from = DateTime.ParseExact(date.Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture);
                }

                bool nightShift = (from.Hour * 60 + from.Minute) <= 380 && (shift == "T2" || shift == "T3");
                if (nightShift)
                {
                    from = from.Date.AddDays(-1);
                }

                NN_DatabaseEntities db = new NN_DatabaseEntities();
                switch (group.ToLower())
                {
                    case "phay 1": group = "view_sp_AcquireMachineInPhay1"; break;
                    case "phay 2": group = "view_sp_AcquireMachineInPhay1"; break;
                    case "tiện": group = "view_sp_AcquireMachineInPhay1"; break;
                    default: throw new ArgumentException("Nhóm máy không phải của xưởng gia công");
                }

                var data = db.sp_242_MachinePlanning_GetUrgentProcessingPlan(shift, from.Date, group).ToList();
                return Json(new { Status = "OK", Values = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private C242_MachinePlanning ValidateValue(C242_MachinePlanning data, string dataNew)
        {

            //foreach (var item in data)
            //{
            //    JObject value = JObject.Parse("{}");
            //    if (item != null)
            //    {
            //        value = JObject.Parse(item);
            //        var Start = value.ToObject<sp_GetAllMachineWWTS_Cache_Result>();
            //    }
            //}

            //NN_DatabaseEntities db = new NN_DatabaseEntities();
            //var key = Convert.ToInt32(form.Get("key"));
            //var model = db.C242_MachinePlanning.FirstOrDefault(item => item.ID == key);
            //if (model == null)
            //    return Json("NG", JsonRequestBehavior.AllowGet);

            //var values = JsonConvert.DeserializeObject<IDictionary>(form.Get("values"));
            //PopulateModel(model, values);

            //Validate(model);

            //db.SaveChanges();
            var obj = JsonConvert.DeserializeObject<Dictionary<string, string>>(dataNew);
            string Fac_TTGC = "TTGC";
            string Fac_Phoi = "Phoi";
            string Fac_File = "Fille";
            string Fac_Dao = "Dao";
            string Fac_Jig = "CBJig";
            string Fac_OK_Old = "OK_Old";
            string Fac_NG_Old = "NG_Old";
            string Fac_GCDo = nameof(sp_GetMachinePlanForFactory_TGTC_Result.Fac_GCDo);
            string Fac_GCXong = nameof(sp_GetMachinePlanForFactory_TGTC_Result.Fac_GCXong);
            if (dataNew.Contains(Fac_TTGC))
            {
                data.Fac_TTGC = Convert.ToBoolean(obj[Fac_TTGC]);
            }

            if (dataNew.Contains(Fac_Phoi))
            {
                data.Fac_Phoi = Convert.ToBoolean(obj[Fac_Phoi]);
            }

            if (dataNew.Contains(Fac_File))
            {
                data.Fac_File = Convert.ToBoolean(obj[Fac_File]);
            }

            if (dataNew.Contains(Fac_Dao))
            {
                data.Fac_Dao = Convert.ToBoolean(obj[Fac_Dao]);
            }

            if (dataNew.Contains(Fac_Jig))
            {
                data.Fac_Jig = Convert.ToBoolean(obj[Fac_Jig]);
            }

            if (dataNew.Contains(Fac_OK_Old))
            {
                data.Fac_OK_Old = Convert.ToInt16(obj[Fac_OK_Old]);
            }

            if (dataNew.Contains(Fac_NG_Old))
            {
                data.Fac_NG_Old = Convert.ToInt16(obj[Fac_NG_Old]);
            }

            if (dataNew.Contains(Fac_GCXong))
            {
                data.Fac_GCXong = Convert.ToBoolean(obj[Fac_GCXong]);
                if (data.Fac_GCXong == true)
                {
                    data.Fac_GCDo = false;
                }
            }

            if (dataNew.Contains(Fac_GCDo))
            {
                data.Fac_GCDo = Convert.ToBoolean(obj[Fac_GCDo]);
                if (data.Fac_GCDo == true)
                {
                    data.Fac_GCXong = false;
                }
            }

            return data;
        }

        private void ImportFactory(string path, DateTime date, string shift)
        {
            List<ClsImportError> listImportResult = new List<ClsImportError>();
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            OleDbDataReader dReader;
            OleDbConnection excelConnection = null;
            string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                                    path +
                                                    ";Extended Properties=Excel 12.0;Persist Security Info=False";
            excelConnection = new OleDbConnection(excelConnectionString);
            DateTime time = DateTime.Now;
            try
            {
                OleDbCommand cmd =
                new OleDbCommand("select DISTINCT * from [Sheet1$] ",// where [Thực xếp] <> \"\"",// and convert(dateTime,[Bắt đầu]) <= Convert(DateTime,\"" + time.ToString() + "\") and Convert(DateTime,\"" + time.ToString() + "\") <= convert(DateTime,[Kết thúc])",
                                    excelConnection);
                excelConnection.Open();

                dReader = cmd.ExecuteReader();
                int i = 1;
                string Oder = "";
                string TTNC = "";
                string NC = "";
                string MachineID = "";
                string Slg = "0";
                string Start = "01/01/1900";
                string Finish = "01/01/1900";
                string TGGC = "0";
                string TGGL = "0";
                string KHCT = "";
                string TH = "01/01/2050";
                string TT = "0";
                string SoJig = "";
                string DKM = "";
                List<C242_MachinePlanning> lastPlan = GetLastMachinePlan(date, shift);
                while (dReader.Read())
                {
                    i++;
                    try
                    {
                        C242_MachinePlanning tablePlanning = new C242_MachinePlanning();
                        if (string.IsNullOrEmpty(dReader["Thực xếp"].ToString().Trim()))
                        {
                            continue;
                        }

                        Oder = dReader["Số Order"].ToString().Trim();
                        TTNC = dReader["Số NC"].ToString().Trim();
                        NC = dReader["NC"].ToString().Trim();
                        MachineID = dReader["Máy GC"].ToString().Trim();
                        Slg = dReader["Slg lệnh"].ToString().Trim();
                        DKM = dReader["ĐKM gốc"].ToString().Trim();
                        var a = dReader["Bắt đầu"].ToString().Trim();
                        KHCT = dReader["Ký hiệu chi tiết"].ToString().Trim();
                        SoJig = dReader["Số Jig"].ToString().Trim();
                        TT = dReader["Tình trạng"].ToString().Trim().Length == 0 ? "0" : dReader["Tình trạng"].ToString().Trim();
                        if (dReader["TG GC"].ToString().Trim() != "")
                        {
                            TGGC = dReader["TG GC"].ToString().Trim();
                        }
                        if (dReader["TG GL"].ToString().Trim() != "")
                        {
                            TGGL = dReader["TG GL"].ToString().Trim();
                        }
                        string K = "";
                        if (dReader["Thực xếp"].ToString().Trim() == "1")
                        {
                            K = "K" + dReader["Thu tu GC"].ToString().Trim();
                        }
                        else if (dReader["Thực xếp"].ToString().Trim() == "1CT")
                        {
                            K = "C" + dReader["Thu tu GC"].ToString().Trim();
                        }
                        else if (dReader["Thực xếp"].ToString().Trim() == "2")
                        {
                            K = "B" + dReader["Thu tu GC"].ToString().Trim();
                        }
                        else if (dReader["Thực xếp"].ToString().Trim() == "5")
                        {
                            K = "L" + dReader["Thu tu GC"].ToString().Trim();
                        }

                        #region xử lý lệnh chưa có

                        if (Oder.ToUpper().Trim() == "CHUA CO")
                        {
                            Oder = "NULL" + KHCT;
                        }

                        #endregion

                        tablePlanning.Date = date.Date;
                        tablePlanning.Order = Oder;
                        tablePlanning.NC = NC;
                        tablePlanning.Fac_MachineID = MachineID;
                        tablePlanning.Slglenh = int.Parse(Slg);
                        tablePlanning.TGGC = float.Parse(TGGC);
                        tablePlanning.TenChiTiet = KHCT;
                        tablePlanning.K = K;
                        tablePlanning.Shift = shift;
                        tablePlanning.BatDau = DateTime.Parse(dReader["Bắt đầu"].ToString().Trim());
                        tablePlanning.KetThuc = DateTime.Parse(dReader["Kết thúc"].ToString().Trim());
                        tablePlanning.ThoiHan = DateTime.Parse(dReader["Thời hạn"].ToString().Trim());
                        tablePlanning.TinhTrang = int.Parse(TT);
                        tablePlanning.SoJig = SoJig;
                        tablePlanning.TTNC = TTNC;
                        tablePlanning.DKmay = DKM;
                        tablePlanning.Dept = "Fac";
                        tablePlanning.Fac_NgayHTtheoKH = tablePlanning.KetThuc;
                        tablePlanning.Note = dReader["Ghi chú"].ToString().Trim();
                        tablePlanning.TongTG = dReader["Check time"].ToString().Trim();
                        var NCtruoc = "STG" + (int.Parse(tablePlanning.NC.Substring(3, 2)) - 1).ToString("0#");
                        var last_machine = string.Empty;
                        var last_date = string.Empty;
                        View_242_WTS dtNCtruoc = new View_242_WTS();
                        if (tablePlanning.TenChiTiet.ToUpper().StartsWith("JG"))
                        {
                            dtNCtruoc = db.View_242_WTS.Where(x => x.OptionID.ToLower() == NCtruoc.ToLower() && x.PartID.ToLower() == tablePlanning.TenChiTiet.ToLower()).FirstOrDefault();
                        }
                        else
                        {
                            dtNCtruoc = db.View_242_WTS.Where(x => x.OptionID.ToLower() == NCtruoc.ToLower() && x.MONo.ToLower() == tablePlanning.Order.ToLower()).FirstOrDefault();
                        }

                        if (dtNCtruoc != null)
                        {
                            last_machine = dtNCtruoc.MachineID;
                            last_date = dtNCtruoc.Date.Value.ToShortDateString();
                        }
                        tablePlanning.Fac_TTFile = dReader["tinh trang file"].ToString().Trim();

                        tablePlanning.Fac_NGTruoc = last_machine + "-" + last_date;
                        var checkMachine = db.C222_Machine.Where(x => x.MachineID.ToLower().StartsWith(tablePlanning.Fac_MachineID.ToLower()) || tablePlanning.Fac_MachineID.ToLower().StartsWith(x.MachineID.ToLower())).Any();

                        if (!checkMachine)
                        {
                            listImportResult.Add(new ClsImportError(i, "Không tìm thấy machineID trong bảng MachineList"));
                            continue;
                        }

                        var dtDuplicate = db.C242_MachinePlanning.Where(x => x.Date == tablePlanning.Date
                                && x.Shift == tablePlanning.Shift
                                && x.Order == tablePlanning.Order
                                && x.NC == tablePlanning.NC
                                && x.TenChiTiet == tablePlanning.TenChiTiet).FirstOrDefault();
                        if (dtDuplicate != null)
                        {
                            listImportResult.Add(new ClsImportError(i, "Kế hoạch đã được xếp. Không được nhập trùng"));
                            continue;
                        }

                        var oldQty = db.C242_MachinePlanning.Where(x => x.Order == tablePlanning.Order
                                && x.NC == tablePlanning.NC
                                && x.TenChiTiet == tablePlanning.TenChiTiet).FirstOrDefault();
                        bool needToChange = tablePlanning.Slglenh > oldQty.Slglenh;
                        if (needToChange)
                        {
                            db.sp_242_MachinePlanning_UpdateNewQty(tablePlanning.NC, tablePlanning.Order, tablePlanning.TenChiTiet, tablePlanning.Slglenh);
                        }

                        tablePlanning = GetCurrentPreparationForProcessing(lastPlan, tablePlanning);
                        db.C242_MachinePlanning.Add(tablePlanning);
                    }
                    catch (Exception ex)
                    {
                        listImportResult.Add(new ClsImportError(i, ex.Message));
                    }
                }

                db.SaveChanges();
                if (excelConnection.State == ConnectionState.Open)
                {
                    excelConnection.Close();
                }
            }
            catch (Exception ex)
            {
                if (excelConnection.State == ConnectionState.Open)
                {
                    excelConnection.Close();
                }
            }
        }

        private List<C242_MachinePlanning> GetLastMachinePlan(DateTime date, string shift)
        {
            shift = date.ToString("yyyyMMdd") + shift;
            List<C242_MachinePlanning> lastMachinePlan = new List<C242_MachinePlanning>();
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var data = db.sp_242_MachineWTS_GetLastShiftOfPlan(shift).ToList();
            if (data.Count == 0)
            {
                return lastMachinePlan;
            }

            date = (DateTime)data[data.Count - 1].date;
            shift = data[data.Count - 1].shift;
            lastMachinePlan = db.C242_MachinePlanning.Where(x => x.Date == date && x.Shift.ToLower() == shift.ToLower() && x.Dept.ToUpper() == "FAC").ToList();
            return lastMachinePlan;
        }

        private C242_MachinePlanning GetCurrentPreparationForProcessing(List<C242_MachinePlanning> data1, C242_MachinePlanning tablePlanning)
        {
            var item = data1.Where(x => x.NC == tablePlanning.NC && x.Order == tablePlanning.Order && x.TenChiTiet == tablePlanning.TenChiTiet && x.MayGC == tablePlanning.MayGC).FirstOrDefault();
            if (item == null)
            {
                return tablePlanning;
            }

            tablePlanning.Fac_Phoi = item.Fac_Phoi;
            tablePlanning.Fac_File = item.Fac_File;
            tablePlanning.Fac_Dao = item.Fac_Dao;
            tablePlanning.Fac_Jig = item.Fac_Jig;
            tablePlanning.Fac_NG_Old = item.Fac_NG_Old;
            tablePlanning.Fac_OK_Old = item.Fac_OK_Old;

            return tablePlanning;
        }

        private string DeceidedMachine(string machineID)
        {
            if (machineID.Length > 3 && machineID.Length <= 4)
            {
                machineID = machineID.Substring(0, 3);
                return machineID;
            }

            if (machineID.Length > 4)
            {
                machineID = machineID.Substring(0, 4);
                return machineID;
            }

            return machineID;
        }

        private void CheckAndUpdateNewQty(NN_DatabaseEntities db, C242_MachinePlanning tablePlanning)
        {
            var list = db.C242_MachinePlanning.Where(x => x.Order == tablePlanning.Order
                                    && x.NC == tablePlanning.NC
                                    && x.TenChiTiet == tablePlanning.TenChiTiet).ToList();
        }

        void ImportDataFromExcel(string path, string dept, string shift, DateTime from)
        {
            switch (dept.ToUpper())
            {
                case "XUONG":
                    ImportFactory(path, from, shift);
                    break;
            }
        }
    }
}