using Newtonsoft.Json;
using NNworking.Models;
using NNworking.Models.Import;
using NNworking.Models.ObjectBase;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NNworking.Controllers.AllDept
{
    [RoutePrefix("Chung")]
    public class AllDeptController : BaseController
    {
        NN_DatabaseEntities db = new NN_DatabaseEntities();
        // GET: AllDept
        [Route("Index.html")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("Quan-ly-tai-khoan.html")]
        public ActionResult UserList()
        {
            CheckPermissAndRedirect();
            return View();
        }

        [Route("Quan-ly-ca-lam-viec.html")]
        public ActionResult ShiftRgister()
        {
            CheckPermissAndRedirect();
            return View();
        }

        [Route("Quan-ly-them-gio.html")]
        public ActionResult OvertimeRegister()
        {
            CheckPermissAndRedirect();
            return View();
        }

        [Route("Quan-ly-cong-viec.html")]
        public ActionResult DailyReport()
        {
            CheckPermissAndRedirect();
            return View();
        }

        [Route("May-gia-cong.html")]
        public ActionResult MachineList()
        {
            CheckPermissAndRedirect();
            ViewBag.Title = "Danh sách máy";
            return View("~/Views/AllDept/MachineManagement/MachineList.cshtml");
        }

        [Route("may-su-co.html")]
        public ActionResult MachineListError()
        {
            CheckPermissAndRedirect();
            ViewBag.Title = "Danh sách máy sự cố";
            return View("~/Views/AllDept/MachineManagement/MachineListError.cshtml");
        }

        [Route("Danh-sach-doi-tac.html")]
        public ActionResult PartnerList()
        {
            CheckPermissAndRedirect();
            ViewBag.Title = "Danh sách nhà cung cấp, khách hàng";
            return View("~/Views/AllDept/Partner/Partner.cshtml");
        }

        [Route("Kieu-doi-tac.html")]
        public ActionResult PartnerTypeList()
        {
            CheckPermissAndRedirect();
            ViewBag.Title = "Kiểu đối tác";
            return View("~/Views/AllDept/Partner/PartnerType.cshtml");
        }

        [Route("Nhom-may-gia-cong.html")]
        public ActionResult MachineGroupList()
        {
            CheckPermissAndRedirect();
            ViewBag.Title = "Danh sách nhóm máy tại các bộ phận";
            return View("~/Views/AllDept/MachineManagement/MachineGroupList.cshtml");
        }

        [Route("Kieu-loi.html")]
        public ActionResult ErrorType()
        {
            CheckPermissAndRedirect();
            ViewBag.Title = "Kiểu lỗi";
            return View("~/Views/AllDept/ErrorNotify/ErrorType.cshtml");
        }

        [Route("Ma-danh-gia-hang-loi.html")]
        public ActionResult EvaluateIndex()
        {
            CheckPermissAndRedirect();
            ViewBag.Title = "mã đánh giá hàng lỗi";
            return View("~/Views/AllDept/ErrorNotify/EvaluateIndex.cshtml");
        }

        [Route("Hang-bao-loi.html")]
        public ActionResult ErrorItemNotify()
        {
            CheckPermissAndRedirect();
            ViewBag.Title = "Hàng báo lỗi";
            return View("~/Views/AllDept/ErrorNotify/ErrorItemNotifyNew.cshtml");
        }

        [Route("nguyen-nhan-doi-sach-hang-loi.html")]
        public ActionResult ErrorItemNotify_2()
        {
            CheckPermissAndRedirect();
            ViewBag.Title = "Hàng báo lỗi";
            return View("~/Views/AllDept/ErrorNotify/ErrorItemNotifyRaiseDept.cshtml");
        }

        [Route("noi-dung-loi.html")]
        public ActionResult ErrorContent()
        {
            CheckPermissAndRedirect();
            ViewBag.Title = "Hàng báo lỗi";
            return View("~/Views/AllDept/ErrorNotify/SubData/ErrorContent.cshtml");
        }

        [Route("thoi-diem-phat-hien-loi.html")]
        public ActionResult DeceidedTime()
        {
            CheckPermissAndRedirect();
            ViewBag.Title = "Hàng báo lỗi";
            return View("~/Views/AllDept/ErrorNotify/SubData/DeceidedTime.cshtml");
        }

        [Route("nhan-dinh-nguyen-nhan-loi.html")]
        public ActionResult PredictErrorCause()
        {
            CheckPermissAndRedirect();
            ViewBag.Title = "Hàng báo lỗi";
            return View("~/Views/AllDept/ErrorNotify/SubData/PredictErrorCause.cshtml");
        }

        [Route("huong-xu-li-loi.html")]
        public ActionResult ErrorProcess()
        {
            CheckPermissAndRedirect();
            ViewBag.Title = "Hàng báo lỗi";
            return View("~/Views/AllDept/ErrorNotify/SubData/ErrorProcess.cshtml");
        }

        [Route("so-lan-xuat-hien-loi.html")]
        public ActionResult ErrorHappenFrequency()
        {
            CheckPermissAndRedirect();
            ViewBag.Title = "Hàng báo lỗi";
            return View("~/Views/AllDept/ErrorNotify/SubData/ErrorHappenFrequency.cshtml");

        }
        [Route("Bo-phan.html")]
        public ActionResult Department()
        {
            CheckPermissAndRedirect();
            ViewBag.Title = "Danh sách bộ phận tại các phòng";
            return View("~/Views/AllDept/MachineManagement/Department.cshtml");
        }

        [Route("Ma-WTS.html")]
        public ActionResult WorkIDManagement()
        {
            CheckPermissAndRedirect();
            ViewBag.Title = "Quản lý mã wts các bộ phận";
            return View("~/Views/AllDept/WorkID/WorkIDManagement.cshtml");
        }

        [Route("thoi-gian-tieu-chuan-gia-cong.html")]
        public ActionResult StandTimeList()
        {
            CheckPermissAndRedirect();
            ViewBag.Title = "Quản lý thời gian tiêu chuẩn gia công";
            return View("~/Views/AllDept/TGTC/TGTC_GiaCong.cshtml");
        }

        [Route("du-lieu-wts-cac-bo-phan.html")]
        public ActionResult GetAllWTS()
        {
            CheckPermissAndRedirect();
            ViewBag.Title = "Lấy dữ liệu WTS";
            return View("~/Views/AllDept/Report/DetailWTS.cshtml");
        }

        public ActionResult cbStaff()
        {
            CheckPermissAndRedirect();
            return PartialView("~/Views/AllDept/_cbStaff.cshtml");
        }

        public ActionResult UpdateTGTC()
        {
            CheckPermissAndRedirect();
            return View("~/Views/AllDept/Report/UpdateTGTC.cshtml");
        }

        [Route("cong-suat-may.html")]
        public ActionResult Capacity()
        {
            CheckPermissAndRedirect();
            return View("~/Views/AllDept/Report/Capacity.cshtml");
        }

        [Route("ti-le-lam-viec-voi-optiondata.html")]
        public ActionResult Capacity2()
        {
            CheckPermissAndRedirect();
            return View("~/Views/AllDept/Report/Capacity2.cshtml");

        }
        [Route("dang-ki-ca-hang-ngay.html")]
        public ActionResult StaffShift()
        {
            CheckPermissAndRedirect();
            return View("~/Views/AllDept/ShiftRgister.cshtml");
        }

        [Route("danh-sach-ca.html")]
        public ActionResult ShiftList()
        {
            CheckPermissAndRedirect();
            return View("~/Views/AllDept/ShiftList.cshtml");
        }

        [Route("vi-tri-de-hang.html")]
        public ActionResult InventoryPlaceOfItem()
        {
            CheckPermissAndRedirect();
            return View("~/Views/Tool/InventoryPlaceOfItem.cshtml");
        }

        [Route("danh-sach-don-hang-hoan-thanh.html")]
        public ActionResult FinishOrderNumber()
        {
            CheckPermissAndRedirect();
            return View("~/Views/Index/FinishOrderNumber.cshtml");
        }

        [Route("danh-sach-bien-ban-cuoc-hop.html")]
        public ActionResult MeetingList()
        {
            CheckPermissAndRedirect();
            return View("~/Views/AllDept/Meeting/Page/MeetingList.cshtml");
        }

        [Route("Chi-tiet-cuoc-hop-{id?}.html")]
        public ActionResult MeetingDetail(int id)
        {
            CheckPermissAndRedirect();
            ViewBag.ID = id;
            return View("~/Views/AllDept/Meeting/Page/MeetingDetail.cshtml");
        }

        [Route("danh-sach-chuc-nang-PM.html")]
        public ActionResult FunctionalList()
        {
            CheckPermissAndRedirect();
            return View("~/Views/AllDept/Permission/FunctionalList.cshtml");
        }

        [Route("cap-quyen-truy-cap.html")]
        public ActionResult FunctionPermission()
        {
            CheckPermissAndRedirect();
            return View("~/Views/AllDept/Permission/FunctionPermission.cshtml");
        }

        [Route("danh-sach-xe.html")]
        public ActionResult CarsList()
        {
            CheckPermissAndRedirect();
            return View("~/Views/AllDept/CarRegister/CarsList.cshtml");
        }

        [Route("dang-ki-xe.html")]
        public ActionResult CarRegister()
        {
            CheckPermissAndRedirect();
            return View("~/Views/AllDept/CarRegister/CarRegister.cshtml");
        }

        [Route("xac-nhan-dang-ki-xe.html")]
        public ActionResult CarRegisterApproved()
        {
            CheckPermissAndRedirect();
            return View("~/Views/AllDept/CarRegister/CarRegisterApproved.cshtml");
        }

        [Route("cong-viec-hang-ngay.html")]
        public ActionResult TechJob_Daily()
        {
            CheckPermissAndRedirect();
            return View("~/Views/AllDept/TechDept/TechJob_Daily.cshtml");
        }

        [Route("noi-dung-thay-doi.html")]
        public ActionResult TechJob_ChangePpoint()
        {
            CheckPermissAndRedirect();
            return View("~/Views/AllDept/TechDept/TechJob_ChangePpoint.cshtml");
        }

        [Route("Cong-viec-hang-ngay-phong-co-dien.html")]
        public ActionResult MechanicalElectronical_WorkDaily()
        {
            CheckPermissAndRedirect();
            return View("~/Views/AllDept/MechanicalElectronical/MechanicalElectronical_WorkDaily.cshtml");
        }

        [Route("Bao-cao-ngay.html")]
        public ActionResult LeaderDailyReport()
        {
            CheckPermissAndRedirect();
            return View("~/Views/AllDept/LeaderReport/PSX/DailyReport.cshtml");
        }

        [Route("Bao-cao-tuan.html")]
        public ActionResult LeaderWeeklyReport()
        {
            CheckPermissAndRedirect();
            return View("~/Views/AllDept/LeaderReport/PSX/WeeklyReport.cshtml");
        }

        [Route("Bao-cao-thang.html")]
        public ActionResult LeaderMonthlyReport()
        {
            CheckPermissAndRedirect();
            return View("~/Views/AllDept/LeaderReport/PSX/MonthlyReport.cshtml");
        }

        [Route("Bao-cao-ngay-PHT.html")]
        public ActionResult LeaderPHTDailyReport()
        {
            CheckPermissAndRedirect();
            return View("~/Views/AllDept/LeaderReport/PHT/DailyReport.cshtml");
        }

        [Route("Bao-cao-tuan-PHT.html")]
        public ActionResult LeaderPHTWeeklyReport()
        {
            CheckPermissAndRedirect();
            return View("~/Views/AllDept/LeaderReport/PHT/WeeklyReport.cshtml");
        }

        [Route("Bao-cao-thang-PHT.html")]
        public ActionResult LeaderPHTMonthlyReport()
        {
            CheckPermissAndRedirect();
            return View("~/Views/AllDept/LeaderReport/PHT/MonthlyReport.cshtml");
        }

        [Route("Quan-ly-cong-viec-redmine.html")]
        public ActionResult Redmine()
        {
            return View("~/Views/AllDept/Redmine.cshtml");
        }


        [HttpPost]
        public JsonResult check(string check)
        {
            var pass = Session["pass"].ToString().Trim();
            var SecID = Session["SecID"].ToString().Trim();
            var StaffID = Session["StaffID"].ToString().Trim();
            var BP = Session["BP"].ToString().Trim();
            var Group = Session["Group"].ToString().Trim();
            var GroupID = Session["GroupID"].ToString().Trim();
            var Level = Session["Level"].ToString().Trim();
            var user = Session["user"].ToString().Trim();
            Session["pass"] = pass;
            Session["SecID"] = SecID;
            Session["StaffID"] = StaffID;
            Session["BP"] = BP;
            Session["Group"] = Group;
            Session["GroupID"] = GroupID;
            Session["Level"] = Level;
            Session["user"] = user;
            return Json(check, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateUser( string staffID)
        {
            try
            {
                var data = db.C222_Staff.Where(x => x.StaffID == staffID).FirstOrDefault();
                if (data == null)
                {
                    throw new ArgumentException("Không tìm thấy mã nhân viên");
                }

                var user = new C222_Users();
                user.UserName =
                CreateUserName(data);
                user.StaffID = data.StaffID;
                user.Password = "T+bfE8dmm/d3Lg074AWMfg==";
                user.UserGroupID = 0;
                user.DepartmentID = 0;
                db.C222_Users.Add(user);
                db.SaveChanges();
                return Json(new { Status = "OK", Value = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { Status = "NG",Value = ex.Message},JsonRequestBehavior.AllowGet);
            }
        }

        private string CreateUserName(C222_Staff data)
        {
            string userName = data.StaffID;
            var i = data.StaffName.Trim().Split(' ');
            var name = i[i.Length - 1];
            userName = name + userName;
            return userName;
        }

        [HttpPost]
        public JsonResult ResetPass(string staffID)
        {
            try
            {
                var data = db.C222_Users.Where(x => x.StaffID == staffID).FirstOrDefault();
                if(data == null)
                {
                    throw new ArgumentException("Không tìm thấy mã nhân viên");
                }

                data.Password = "T+bfE8dmm/d3Lg074AWMfg==";
                db.SaveChanges();
                return Json(new { Status = "OK", Value = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Value = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ImportStaff()
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count == 0)
            {
                return Json(new { Status = "NG", Values = "Chưa chọn file import." });
            }

            List<clsError> Error = new List<clsError>();
            try
            {
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    string fname;

                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    fname = Path.Combine(Server.MapPath("~/Files/"), fname);
                    file.SaveAs(fname);
                    string name = ChangeName(fname);
                    System.IO.File.Move(fname, name);
                    IImport import = new ImportStaffList();
                    import.ImportData(name, string.Empty, out Error);
                }
                return Json(new { Status = "OK", Values = "Import thành công!", Errors = Error });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = "Error occurred. Error details: " + ex.Message, Errors = Error });
            }
        }

        [HttpPost]
        public JsonResult ImportGiaoNhan(string staffID,string fromDept, string toDept)
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count == 0)
            {
                return Json(new { Status = "NG", Values = "Chưa chọn file import." });
            }

            List<clsError> Error = new List<clsError>();
            try
            {
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    string fname;

                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    fname = Path.Combine(Server.MapPath("~/Files/"), fname);
                    file.SaveAs(fname);
                    string name = ChangeName(fname);
                    System.IO.File.Move(fname, name);
                    //return Json(new { Status = "OK", Values = name, Errors = Error });
                    ImportGiaoNhanData(name, staffID, fromDept, toDept, out Error);
                }
                return Json(new { Status = "OK", Values = "Import thành công!", Errors = Error });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = "Error occurred. Error details: " + ex.Message, Errors = Error });
            }
        }

        [HttpPost]
        public JsonResult ImportStaffShift()
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count == 0)
            {
                return Json(new { Status = "NG", Values = "Chưa chọn file import." });
            }

            List<clsError> Error = new List<clsError>();
            try
            {
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    string fname;

                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    fname = Path.Combine(Server.MapPath("~/Files/"), fname);
                    file.SaveAs(fname);
                    string name = ChangeName(fname);
                    System.IO.File.Move(fname, name);
                    ImportStaffShiftData(name, out Error);
                }
                return Json(new { Status = "OK", Values = "Import thành công!", Errors = Error });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = "Error occurred. Error details: " + ex.Message, Errors = Error });
            }
        }
        
        [HttpPost]
        public JsonResult ImportStokerTool(string staffID)
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count == 0)
            {
                return Json(new { Status = "NG", Values = "Chưa chọn file import." });
            }

            List<clsError> Error = new List<clsError>();
            try
            {
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    string fname;

                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    fname = Path.Combine(Server.MapPath("~/Files/"), fname);
                    file.SaveAs(fname);
                    string name = ChangeName(fname);
                    System.IO.File.Move(fname, name);
                    IImport import = new ImportStokerTool();
                    import.ImportData(name, staffID, out Error);
                }
                return Json(new { Status = "OK", Values = "Import xong!", Errors = Error });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = "Error occurred. Error details: " + ex.Message, Errors = Error });
            }
        }
        
        [HttpPost]
        public JsonResult ImportStokerInput(string staffID)
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count == 0)
            {
                return Json(new { Status = "NG", Values = "Chưa chọn file import." });
            }

            List<clsError> Error = new List<clsError>();
            try
            {
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    string fname;

                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    fname = Path.Combine(Server.MapPath("~/Files/"), fname);
                    file.SaveAs(fname);
                    string name = ChangeName(fname);
                    System.IO.File.Move(fname, name);
                    //return Json(new { Status = "OK", Values = name, Errors = Error });
                    IImport import = new ImportStokerInput();
                    import.ImportData(name, staffID, out Error);
                }
                return Json(new { Status = "OK", Values = "Import xong!", Errors = Error });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = "Error occurred. Error details: " + ex.Message, Errors = Error });
            }
        }

        [HttpPost]
        public JsonResult ImportNNDS(string staffID)
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count == 0)
            {
                return Json(new { Status = "NG", Values = "Chưa chọn file import." });
            }

            List<clsError> Error = new List<clsError>();
            try
            {
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    string fname;

                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    fname = Path.Combine(Server.MapPath("~/Files/"), fname);
                    file.SaveAs(fname);
                    string name = ChangeName(fname);
                    System.IO.File.Move(fname, name);
                    //return Json(new { Status = "OK", Values = name, Errors = Error });
                    IImport import = new ImportNNDS();
                    import.ImportData(name, staffID, out Error);
                }
                return Json(new { Status = "OK", Values = "Import xong!", Errors = Error });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = "Error occurred. Error details: " + ex.Message, Errors = Error });
            }
        }

        [HttpPost]
        public JsonResult ImportStokerOutput(string staffID)
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count == 0)
            {
                return Json(new { Status = "NG", Values = "Chưa chọn file import." });
            }

            List<clsError> Error = new List<clsError>();
            try
            {
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    string fname;

                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    fname = Path.Combine(Server.MapPath("~/Files/"), fname);
                    file.SaveAs(fname);
                    string name = ChangeName(fname);
                    System.IO.File.Move(fname, name);
                    //return Json(new { Status = "OK", Values = name, Errors = Error });
                    IImport import = new ImportStokerOutput();
                    import.ImportData(name, staffID, out Error);
                }
                return Json(new { Status = "OK", Values = "Import xong!", Errors = Error });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = "Error occurred. Error details: " + ex.Message, Errors = Error });
            }
        }

        [HttpPost]
        public JsonResult ImportBaoLoi_New(string staffID)
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count == 0)
            {
                return Json(new { Status = "NG", Values = "Chưa chọn file import." });
            }

            List<clsError> Error = new List<clsError>();
            try
            {
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    string fname;

                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    fname = Path.Combine(Server.MapPath("~/Files/"), fname);
                    file.SaveAs(fname);
                    string name = ChangeName(fname);
                    System.IO.File.Move(fname, name);
                    //return Json(new { Status = "OK", Values = name, Errors = Error });
                    IImport import = new ImportBaoLoi_New();
                    import.ImportData(name, staffID, out Error);
                }
                return Json(new { Status = "OK", Values = "Import xong!", Errors = Error });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = "Error occurred. Error details: " + ex.Message, Errors = Error });
            }
        }

        [HttpPost]
        public JsonResult ImportBaoLoi(string staffID)
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count == 0)
            {
                return Json(new { Status = "NG", Values = "Chưa chọn file import." });
            }

            List<clsError> Error = new List<clsError>();
            try
            {
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    string fname;

                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    fname = Path.Combine(Server.MapPath("~/Files/"), fname);
                    file.SaveAs(fname);
                    string name = ChangeName(fname);
                    System.IO.File.Move(fname, name);
                    //return Json(new { Status = "OK", Values = name, Errors = Error });
                    ImportBaoLoiData(name, staffID, out Error);
                }
                return Json(new { Status = "OK", Values = "Import xong!", Errors = Error });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = "Error occurred. Error details: " + ex.Message, Errors = Error });
            }
        }

        private bool ImportBaoLoiData(string fname, string staffID, out List<clsError> Error)
        {
            Error = new List<clsError>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage(new FileInfo(fname)))
            {
                var b = package.Workbook.Worksheets.Count;
                if (b == 0)
                {
                    return false;
                }

                foreach (var item in package.Workbook.Worksheets)
                {
                    if (item.Name.ToUpper() != "SHEET1")
                    {
                        continue;
                    }

                    int line = 1;
                    NN_DatabaseEntities db = new NN_DatabaseEntities();
                    while (line < 1000000)
                    {
                        line++;
                        try
                        {
                            //var PartNo = item.Cells["A" + line].Value == null ? string.Empty : item.Cells["A" + line].Value.ToString();
                            var MONo =                  item.Cells["A" + line].Value == null ? string.Empty : item.Cells["A" + line].Value.ToString().Trim();
                            var OptionID =              item.Cells["B" + line].Value == null ? string.Empty : item.Cells["B" + line].Value.ToString().Trim();
                            var PartID =                item.Cells["C" + line].Value == null ? string.Empty : item.Cells["C" + line].Value.ToString().Trim();
                            var ErrorNumber =           item.Cells["D" + line].Value == null ? string.Empty : item.Cells["D" + line].Value.ToString().Trim();
                            var NotifyDept =            item.Cells["E" + line].Value == null ? string.Empty : item.Cells["E" + line].Value.ToString().Trim();
                            var ErrorTypeID =           item.Cells["F" + line].Value == null ? string.Empty : item.Cells["F" + line].Value.ToString().Trim();
                            var ErrorContent =          item.Cells["G" + line].Value == null ? string.Empty : item.Cells["G" + line].Value.ToString().Trim();
                            var NotifyDate =            item.Cells["H" + line].Value == null ? string.Empty : item.Cells["H" + line].Value.ToString().Trim();
                            var ReceiveErrorItemDate =  item.Cells["I" + line].Value == null ? string.Empty : item.Cells["I" + line].Value.ToString().Trim();
                            var Note =                  item.Cells["J" + line].Value == null ? string.Empty : item.Cells["J" + line].Value.ToString().Trim();
                            var Supplier =              item.Cells["K" + line].Value == null ? string.Empty : item.Cells["K" + line].Value.ToString().Trim();
                            var RaiseErrorDept =        item.Cells["L" + line].Value == null ? string.Empty : item.Cells["L" + line].Value.ToString().Trim();
                            var ErrorComment =          item.Cells["M" + line].Value == null ? string.Empty : item.Cells["M" + line].Value.ToString().Trim();
                            var CauseOfError =          item.Cells["N" + line].Value == null ? string.Empty : item.Cells["N" + line].Value.ToString().Trim();
                            var evaluate =              item.Cells["O" + line].Value == null ? string.Empty : item.Cells["O" + line].Value.ToString().Trim();

                            if (string.IsNullOrEmpty(MONo))
                            {
                                break;
                            }
                            int qty;
                            if (!int.TryParse(ErrorNumber, out qty))
                            {
                                throw new ArgumentException("Số lượng lỗi phải là kiểu số");
                            }
                            
                            var errorType = db.C242_ErrorType.Where(x => x.ErrorType.ToLower().Trim() == ErrorTypeID.ToLower()).FirstOrDefault();
                            if (errorType == null)
                            {
                                throw new ArgumentException("Kiểu lỗi không đúng");
                            }

                            DateTime notifyDate;
                            if(!DateTime.TryParse(NotifyDate,out notifyDate))
                            {
                                throw new ArgumentException("Ngày thông báo lỗi không đúng định dạng");
                            }

                            DateTime receiveErrorItemDate;
                            if (!DateTime.TryParse(ReceiveErrorItemDate, out receiveErrorItemDate))
                            {
                                throw new ArgumentException("Ngày tiếp nhận lỗi không đúng định dạng");
                            }

                            var dept = db.sp_222_PartnerGetAll().Where(x => x.Code.ToLower().Trim() == NotifyDept.ToLower()).Any();
                            if (!dept)
                            {
                                throw new ArgumentException("Bộ phận báo lỗi không nằm trong danh sách bộ phận");
                            }

                            dept = db.sp_222_PartnerGetAll().Where(x => x.Code.ToLower().Trim() == RaiseErrorDept.ToLower()).Any();
                            if (!dept)
                            {
                                throw new ArgumentException("Bộ phận gây lỗi không nằm trong danh sách bộ phận");
                            }

                            dept = db.View_222_Partner.Where(x => x.Code.ToLower().Trim() == Supplier.ToLower()).Any();
                            if (!dept)
                            {
                                throw new ArgumentException("Nhà cung cấp không nằm trong danh sách trong phần mềm");
                            }

                            int eval;
                            if (!int.TryParse(evaluate, out eval))
                            {
                                throw new ArgumentException("Mã đánh giá phải là kiểu số");
                            }

                            dept = db.C242_ErrorItemEvaluate.Where(x => x.EvaluateIndex == eval).Any();
                            if (!dept)
                            {
                                throw new ArgumentException("Mã đánh giá không đúng");
                            }

                            C242_ErrorItemNotify obj = new C242_ErrorItemNotify();
                            obj.OptionID = OptionID;
                            obj.PartID = PartID;
                            obj.MONo = MONo;
                            obj.ErrorNumber = qty;
                            obj.NotifyDept = NotifyDept;
                            obj.ErrorTypeID = errorType.ID;
                            obj.ErrorContent = ErrorContent;
                            obj.NotifyDate = notifyDate;
                            obj.ReceiveErrorItemDate = receiveErrorItemDate;
                            obj.Note = Note;
                            obj.Supplier = Supplier;
                            obj.RaiseErrorDept = RaiseErrorDept;
                            obj.ErrorComment = ErrorComment;
                            obj.CauseOfError = CauseOfError;
                            obj.Evaluate = eval;
                            db.C242_ErrorItemNotify.Add(obj);
                        }
                        catch (Exception ex)
                        {
                            Error.Add(new clsError(line, "Not OK", ex.Message));
                        }
                    }

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Error.Add(new clsError(line + 1, "Not OK", "Không nhập được dữ liệu. Vui lòng thử lại sau"));
                    }
                }
            }

            return true;
        }

        private bool ImportStaffShiftData(string fname, out List<clsError> Error)
        {
            Error = new List<clsError>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage(new FileInfo(fname)))
            {
                var b = package.Workbook.Worksheets.Count;
                if (b == 0)
                {
                    return false;
                }

                var listShift = db.C222_Shift.ToList();
                foreach (var item in package.Workbook.Worksheets)
                {
                    if (item.Name.ToUpper() != "SHEET1")
                    {
                        continue;
                    }

                    int line = 1;
                    NN_DatabaseEntities db = new NN_DatabaseEntities();

                    List<C242_InventoryReceivedDetail> listInserted = new List<C242_InventoryReceivedDetail>();
                    while (line < 1000000)
                    {
                        line++;
                        try
                        {
                            var staffID = item.Cells["A" + line].Value == null ? string.Empty : item.Cells["A" + line].Value.ToString().Trim();
                            var date = item.Cells["B" + line].Value == null ? string.Empty : item.Cells["B" + line].Value.ToString().Trim();
                            var shift = item.Cells["C" + line].Value == null ? string.Empty : item.Cells["C" + line].Value.ToString().Trim();
                            var startWork = item.Cells["D" + line].Value == null ? string.Empty : item.Cells["D" + line].Value.ToString().Trim();
                            var finishWork = item.Cells["E" + line].Value == null ? string.Empty : item.Cells["E" + line].Value.ToString().Trim();

                            if (string.IsNullOrEmpty(staffID))
                            {
                                break;
                            }

                            var correctShift = listShift.Where(x => x.Shift.ToLower() == shift.ToLower()).FirstOrDefault();
                            if (correctShift == null)
                            {
                                throw new ArgumentException("Ca làm việc không đúng");
                            }

                            DateTime Date;
                            if (!DateTime.TryParse(date, out Date))
                            {
                                throw new ArgumentException("Ngày không đúng định dạng");
                            }

                            DateTime StartWork;
                            if (!DateTime.TryParse(startWork, out StartWork))
                            {
                                StartWork = correctShift.StartTime;
                                //throw new ArgumentException("Ngày không đúng định dạng");
                            }

                            DateTime FinishWork;
                            if (!DateTime.TryParse(finishWork, out FinishWork))
                            {
                                FinishWork = correctShift.StartTime;
                                //throw new ArgumentException("Ngày không đúng định dạng");
                            }

                            bool milk = correctShift.Milk == true ? true : false;
                            C222_StaffShift obj = new C222_StaffShift();
                            obj.StaffID = staffID;
                            obj.Date = Date;
                            obj.Shift = shift;
                            obj.StartWork = StartWork;
                            obj.FinishWork = FinishWork;
                            db.C222_StaffShift.Add(obj);
                        }
                        catch (Exception ex)
                        {
                            Error.Add(new clsError(line, "Not OK", ex.Message));
                        }
                    }

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Error.Add(new clsError(line + 1, "Not OK", "Không nhập được dữ liệu. Vui lòng thử lại sau"));
                    }
                }
            }

            return true;
        }

        private bool ImportGiaoNhanData(string fname, string staffID, string fromDept, string toDept, out List<clsError> Error)
        {
            Error = new List<clsError>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage(new FileInfo(fname)))
            {
                var b = package.Workbook.Worksheets.Count;
                if (b == 0)
                {
                    return false;
                }

                foreach (var item in package.Workbook.Worksheets)
                {
                    if (item.Name.ToUpper() != "SHEET1")
                    {
                        continue;
                    }

                    int line = 1;
                    NN_DatabaseEntities db = new NN_DatabaseEntities();
                    C242_InventoryReceived po = new C242_InventoryReceived();
                    po.Date = DateTime.Now;
                    po.Deleted = false;
                    po.Import = true;
                    po.ImportFrom = fromDept;
                    po.Note = string.Empty;
                    po.ReceiveDept = toDept;
                    po.StaffID = staffID;
                    db.C242_InventoryReceived.Add(po);
                    db.SaveChanges();

                    List<C242_InventoryReceivedDetail> listInserted = new List<C242_InventoryReceivedDetail>();
                    while (line < 1000000)
                    {
                        line++;
                        try
                        {
                            //var PartNo = item.Cells["A" + line].Value == null ? string.Empty : item.Cells["A" + line].Value.ToString();
                            var OrderNumber = item.Cells["A" + line].Value == null ? string.Empty : item.Cells["A" + line].Value.ToString();
                            var Qty = item.Cells["B" + line].Value == null ? string.Empty : item.Cells["B" + line].Value.ToString();
                            var Note = item.Cells["C" + line].Value == null ? string.Empty : item.Cells["C" + line].Value.ToString();
                            var GiaDe = item.Cells["D" + line].Value == null ? string.Empty : item.Cells["D" + line].Value.ToString();
                            var NguoiNhan = item.Cells["E" + line].Value == null ? string.Empty : item.Cells["E" + line].Value.ToString();

                            if (string.IsNullOrEmpty(OrderNumber))
                            {
                                break;
                            }

                            //var mono = db.View_242_BusOder.Where(x => x.MONo.ToLower().Trim() == OrderNumber.Trim().ToLower()).Any();

                            if (string.IsNullOrEmpty(GiaDe))
                            {
                                throw new ArgumentException("Giá để không đươc trống");
                            }

                            int qty;
                            if(!int.TryParse(Qty,out qty))
                            {
                                throw new ArgumentException("Qty phải là kiểu số");
                            }
                            
                            C242_InventoryReceivedDetail obj = new C242_InventoryReceivedDetail();
                            obj.VoucherID = po.ID;
                            obj.Deleted = false;
                            obj.GiaDe = GiaDe;
                            obj.Note = Note;
                            obj.OrderNumber = OrderNumber;
                            obj.PartNo = string.Empty;
                            var data = db.C242_BusOder.Where(x => x.BOderNo.ToLower() == obj.OrderNumber.ToLower() && !x.Deleted).FirstOrDefault();
                            if (data != null)
                            {
                                obj.PartNo = data.PartID;
                            }
                            else
                            {
                                throw new ArgumentException($@"Số order {OrderNumber} không tồn tại trong bảng danh sách order");
                            }

                            obj.NguoiNhan = NguoiNhan;
                            obj.Qty = qty;
                            obj.Price = 0;
                            obj.StatusID = 0;
                            //Error.Add(new clsError(line, "Not OK", "Add obj error"));
                            db.C242_InventoryReceivedDetail.Add(obj);
                            //Error.Add(new clsError(line, "Not OK", "Add obj OK"));
                            clsBase ba = new clsBase();
                            ba.CheckAndUpdateBusOrder(obj,db);
                        }
                        catch (Exception ex)
                        {
                            Error.Add(new clsError(line, "Not OK", ex.Message + ex.StackTrace));
                        }
                    }

                    try
                    {
                        db.SaveChanges();
                    }
                    catch(Exception ex)
                    {
                        var db1 = new NN_DatabaseEntities();
                        db1.C242_InventoryReceived.Remove(po);
                        db1.SaveChanges();
                        Error.Add(new clsError(line + 1, "Not OK", "Không nhập được dữ liệu. Vui lòng thử lại sau"));
                    }
                }
            }

            return true;
        }

        //TODO: Đang làm giở ngày 20190822: lấy lịch sử chát giữa 2 user
        [HttpPost]
        public JsonResult GetChattingHistory(string fromClient, string toClient)
        {
            try
            {
                var limitDate = DateTime.Now.AddDays(-3);
                var data = db.WorkingNotifycations.Where(x => (x.FromClient == fromClient && x.ToClient == toClient) || (x.FromClient == toClient && x.ToClient == fromClient) && x.NotifyTime >= limitDate).ToList();
                if(data.Count == 0)
                {
                    throw new ArgumentException("Không có lịch sử chát.");
                }

                foreach(var item in data)
                {
                    if(item.FromClient == fromClient)
                    {
                        continue;
                    }

                    item.ViewStatus = true;
                }

                db.SaveChanges();
                return Json(new { Status = "OK", Values = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetHangBaoLoiByMONo(string order)
        {
            try
            {
                var data = db.C242_ErrorItemNotify.Where(x => x.MONo.ToLower() == order.ToLower() && !x.Deleted).ToList();
                return Json(new { Status = "OK", Values = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ReadAllMessage(string user1, string user2)
        {
            try
            {
                var data = db.WorkingNotifycations.Where(x => ((x.FromClient == user1 && x.ToClient == user2) || (x.FromClient == user2 && x.ToClient == user1)) && !x.ViewStatus).ToList();
                if (data.Count == 0)
                {
                    throw new ArgumentException("Không có lịch sử chát.");
                }

                foreach (var item in data)
                {
                    item.ViewStatus = true;
                }

                db.SaveChanges();
                return Json(new { Status = "OK", Values = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private bool CheckUserWhoReceiveMsg(string toUser)
        {
            bool msgToMachine = false;
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var data = db.C222_Users.Where(x => x.UserName.ToLower() == toUser.ToLower()).FirstOrDefault();
            if(data == null)
            {
                return msgToMachine;
            }

            if(data.UserName.ToLower() == data.StaffID.ToLower())
            {
                msgToMachine = true;
            }

            return msgToMachine;
        }

        [HttpPost]
        public JsonResult UpdateTGTCToOptionData(string partid, string optionid, float protime, float clamptime)
        {
            try
            {
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var data = db.View_242_OptionData.Where(x => x.PartID.ToLower() == partid.ToLower() && x.OptionID.ToLower() == optionid.ToLower()).FirstOrDefault();
                if(data == null)
                {
                    throw new ArgumentException("Không tìm thấy thông tin chi tiết, nguyên công");
                }

                var data_new = db.C242_OptionData.Find(data.ID);
                data_new.ProTime = Math.Round(protime,2);
                data_new.ClampTime = Math.Round(clamptime, 2);
                db.SaveChanges();
                return Json(new { Status = "OK", Values = String.Empty }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ListAvailableUser(string groupMachine)
        {
            try
            {
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                string fromStaffID = Session["StaffID"].ToString().Trim();
                var data = db.C222_Staff.Where(x=>x.StaffID.ToLower() == fromStaffID.ToLower()).FirstOrDefault();
                if(data == null)
                {
                    return Json(new { Status = "NG", Values = "Không chấp nhận chế độ ẩn danh!" }, JsonRequestBehavior.AllowGet);
                }

                var returnData = db.C222_Users.Take(0).ToList();
                returnData = db.C222_Users.ToList();
                return Json(new { Status = "OK", Values = returnData }, JsonRequestBehavior.AllowGet);
                
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ImportSuppendStaff()
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count == 0)
            {
                return Json(new { Status = "NG", Values = "Chưa chọn file import."});
            }

            try
            {
                HttpFileCollectionBase files = Request.Files;
                List<clsError> Error = new List<clsError>();
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    string fname;
                    
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }
                        
                    fname = Path.Combine(Server.MapPath("~/Files/"), fname);
                    file.SaveAs(fname);
                    string name = ChangeName(fname);
                    System.IO.File.Move(fname, name);
                }
                return Json(new {Status = "OK",Values = "Import thành công!", Errors = Error });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = "Error occurred. Error details: " + ex.Message });
            }
        }

        [HttpGet]
        public JsonResult GetMonth_FinishOrderNumber()
        {
            try
            {
                var list = db.sp_242_BusOder_Month_FinishNumber().ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<sp_242_BusOder_Month_FinishNumber_Result>(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetMachineWTSRunningTime()
        {
            try
            {
                var list = db.sp_242_MachineWTS_GetRunningTime().ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<sp_242_MachineWTS_GetRunningTime_Result>(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetBusOrderList(string month)
        {
            try
            {
                var list = db.View_242_BusOder.Where(x=> x.Deadline.Value.ToString("yyyyMM") == month).ToString();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new List<View_242_BusOder>(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetBusOrderInfor(string orderNo)
        {
            try
            {
                var list = db.View_242_BusOder.Where(x => x.MONo.ToUpper() == orderNo.ToUpper()).FirstOrDefault();
                return Json(new { Status = "OK", Values = list }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult GetDetailMeeting(int id)
        {
            try
            {
                var meeting = db.View_222_MeetingContent.Where(x => x.ID == id).ToList();
                var meetingComment = db.View_222_MeetingComment.Where(x => x.MeetingID == id).OrderBy(x=>x.ID).ToList();
                List<View_222_MeetingComment_File> listComment = new List<View_222_MeetingComment_File>();
                foreach(var item in meetingComment)
                {
                    var serializedParent = JsonConvert.SerializeObject(item); 
                    View_222_MeetingComment_File obj  = JsonConvert.DeserializeObject<View_222_MeetingComment_File>(serializedParent);                    
                    obj.FilesUploaded = db.C222_MeetingUploadFile.Where(x => x.CommentID == obj.ID).ToList();
                    listComment.Add(obj);
                }

                var listStaff = new List<string>();
                if (meeting.Count > 0)
                {
                    listStaff = GetListStaff(meeting[0]);
                }

                return Json(new { Status = "OK", Values = string.Empty, MeetingContent = meeting, MeetingComment=listComment, StaffList = listStaff }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message, MeetingContent = string.Empty, MeetingComment = string.Empty, StaffList = string.Empty }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult InputMeetingContent(string date, string deadline, string content,string subject, string evaluate, string command, string staff)
        {
            try
            {
                List<string> listFiles = UploadFiles(Request.Files);
                
                var obj = new C222_MeetingContent();
                DateTime fromDate;
                if(!DateTime.TryParse(date,out fromDate))
                {
                    fromDate = DateTime.ParseExact(date.Substring(0, 24),
                                  "ddd MMM d yyyy HH:mm:ss",
                                  System.Globalization.CultureInfo.InvariantCulture).Date;
                }
                DateTime deadline1;
                if (!DateTime.TryParse(deadline, out deadline1))
                {
                    deadline1 = DateTime.ParseExact(deadline.Substring(0, 24),
                                  "ddd MMM d yyyy HH:mm:ss",
                                  System.Globalization.CultureInfo.InvariantCulture).Date;
                }

                obj.Date = fromDate;
                obj.Deadline = deadline1;
                obj.Solution = content;
                obj.Command = command;
                obj.Evaluate = evaluate;
                obj.Deleted = false;
                obj.Confirmed = false;
                obj.Staff = staff;
                obj.Subject = subject;
                db.C222_MeetingContent.Add(obj);
                db.SaveChanges();

                try
                {
                    foreach (var item in listFiles)
                    {
                        C222_MeetingUploadFile itemObj = new C222_MeetingUploadFile();
                        itemObj.MeetingID = obj.ID;
                        itemObj.CommentID = 0;
                        itemObj.Deleted = false;
                        itemObj.FilePath = item;
                        itemObj.FileName = item.Replace($@"~/Files/Stored/", "");
                        db.C222_MeetingUploadFile.Add(itemObj);
                    }

                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    db.C222_MeetingContent.Remove(obj);
                    db.SaveChanges();
                    return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { Status = "OK", Values = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult InputPermissionContent( string[] staff,int[] fList)
        {
            try
            {
                foreach(var item in staff)
                {
                    foreach(var func in fList)
                    {
                        var obj = new C222_WebPermission();
                        obj.StaffID = item;
                        obj.UrlID = func;
                        db.C222_WebPermission.Add(obj);
                    }
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
        [ValidateInput(false)]
        public JsonResult InputMeetingComment(string content, string staff, int meetingID)
        {
            try
            {
                List<string> listFiles = UploadFiles(Request.Files);

                var obj = new C222_MeetingComment();
                obj.Comment = content;
                obj.MeetingID = meetingID;
                obj.StaffID = Session["StaffID"].ToString();
                bool allMem = false;
                if (string.IsNullOrEmpty(staff))
                {
                    allMem = true;
                }

                if (allMem)
                {
                    GetMettingStaff(meetingID,ref staff);
                }
                obj.ToStaff = staff;
                obj.Date = DateTime.Now;
                db.C222_MeetingComment.Add(obj);
                db.SaveChanges();
                try
                {
                    foreach (var item in listFiles)
                    {
                        C222_MeetingUploadFile itemObj = new C222_MeetingUploadFile();
                        itemObj.MeetingID = 0;
                        itemObj.CommentID = obj.ID;
                        itemObj.Deleted = false;
                        itemObj.FilePath = item;
                        itemObj.FileName = item.Replace($@"~/Files/Stored/","");
                        db.C222_MeetingUploadFile.Add(itemObj);
                    }

                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    db.C222_MeetingComment.Remove(obj);
                    db.SaveChanges();
                    return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = "OK", Values = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private void GetMettingStaff(int meetingID, ref string staff)
        {
            var listStaff = db.View_222_MeetingContent.Where(x => x.ID == meetingID).FirstOrDefault();
            if (listStaff == null )
            {
                return;
            }

            string staffID = Session["StaffID"].ToString();
            var c222_staff = db.C222_Staff.Where(x => listStaff.Staff.IndexOf(x.StaffID) != -1 && x.StaffID != staffID).ToList();
            if(c222_staff.Count == 0)
            {
                return;
            }

            for(int i = 0; i < c222_staff.Count; i ++)
            {
                staff = $@"{c222_staff[i].StaffID},";
            }
        }

        private string CombineStaff(string[] staff)
        {
            string result = string.Empty;
            result += $@"{staff[0]};";
            for(int i = 1; i < staff.Length; i++)
            {
                result += $@"{staff[i]};";
            }

            return result;
        }

        private List<string> GetListStaff(View_222_MeetingContent view_222_MeetingContent)
        {
            var listStaff = view_222_MeetingContent.Staff.Split(',');
            var result = new List<string>();

            if(listStaff.Length > 0)
            {
                NN_DatabaseEntities db1 = new NN_DatabaseEntities();
                foreach(var item in listStaff)
                {
                    var staff = db.C222_Staff.Where(x => x.StaffID == item).FirstOrDefault();
                    if(staff == null)
                    {
                        continue;
                    }

                    result.Add($@"{staff.StaffID} - {staff.StaffName}");
                }
            }

            return result;
        }

        private string ChangeName(string fname)
        {
            string path = Path.GetDirectoryName(fname);
            string extention = Path.GetExtension(fname);
            string newName = $"{path}/{DateTime.Now.ToString("yyyyMMddHHmmss")}{extention}";
            return newName;
        }
        
        //select * from [242_BusOder]

        //select * from [242_MO]


        //select * from [242_MachinePlanning]


        //select * from [242_InventoryReceivedDetail]


        //select * from [242_ErrorItemNotify_New]


        //select * from [242_WTS]
    }
}
