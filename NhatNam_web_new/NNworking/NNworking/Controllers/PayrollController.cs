using NNworking.Models;
using NNworking.Models.Import;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NNworking.Controllers
{
    [RoutePrefix("Nhan-su")]
    public class PayrollController : BaseController
    {
        [Route("Index.html")]
        // GET: Payroll
        public ActionResult Index()
        {
            return View();
        }

        [Route("Dang-ki-ca-lam-viec.html")]
        public ActionResult ShiftRegister()
        {
            return View();
        }

        [Route("Time-card.html")]
        public ActionResult TimeCard()
        {
            return View();
        }

        [Route("Time-card-chi-tiet.html")]
        public ActionResult TimeCardDetail()
        {
            return View();
        }

        [Route("Lich-lam-viec-cong-ty.html")]
        public ActionResult WorkingDay()
        {
            return View();
        }

        [Route("Dang-ki-them-gio.html")]
        public ActionResult OTPlan()
        {
            return View();
        }

        [Route("Dang-ki-nghi.html")]
        public ActionResult AbsentRegister()
        {
            return View();
        }

        [Route("Ke-hoach-lam-viec.html")]
        public ActionResult EmployeePlan()
        {
            return View();
        }

        [Route("Thong-ke-com-theo-DK-ca.html")]
        public ActionResult MealStatics()
        {
            return View("~/Views/Payroll/Report/MealStatics.cshtml");
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult ImportShiftRegister(string date)
        {
            List<clsError> Error = new List<clsError>();
            try
            {
                string name = UploadImportFiles(Request.Files);
                IImport import = new ImportShiftRegister();
                import.ImportData(name, Session["StaffID"].ToString(), out Error);
                if(Error.Count > 0)
                {
                    throw new ArgumentException("Có lỗi trong quá trình import. Vui lòng xem chi tiết lỗi ở từng dòng! ");
                }

                return Json(new { Status = "OK", Values = "Import thành công", Errors = Error }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = "Error occurred. Error details: " + ex.Message, Errors = Error });
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult ImportTimeCard(string date)
        {
            List<clsError> Error = new List<clsError>();
            try
            {
                string name = UploadImportFiles(Request.Files);
                IImport import = new ImportTimeCard();
                import.ImportData(name, Session["StaffID"].ToString(), out Error);
                if(Error.Count > 0)
                {
                    throw new ArgumentException("Có lỗi trong quá trình import. Vui lòng xem chi tiết lỗi ở từng dòng! ");
                }

                return Json(new { Status = "OK", Values = string.Empty, Errors = Error }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = "Error occurred. Error details: " + ex.Message, Errors = Error });
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult ImportTimeCardDetail(string date)
        {
            List<clsError> Error = new List<clsError>();
            try
            {
                string name = UploadImportFiles(Request.Files);
                IImport import = new ImportMaterialStockInput();
                import.ImportData(name, Session["StaffID"].ToString(), out Error);
                if(Error.Count > 0)
                {
                    throw new ArgumentException("Có lỗi trong quá trình import. Vui lòng xem chi tiết lỗi ở từng dòng! ");
                }

                return Json(new { Status = "OK", Values = string.Empty, Errors = Error }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = "Error occurred. Error details: " + ex.Message, Errors = Error });
            }
        }
    }
}