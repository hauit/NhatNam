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
    [RoutePrefix("Kho-vat-lieu")]
    public class MaterialStockController : BaseController
    {
        [Route("Cau-hinh-vat-lieu.html")]
        // GET: MaterialStock
        public ActionResult MaterialConfiguration()
        {
            return View();
        }

        [Route("Danh-sach-vat-lieu.html")]
        // GET: MaterialStock
        public ActionResult MaterialStock()
        {
            return View();
        }

        [Route("Nhap-vat-lieu")]
        // GET: MaterialStock
        public ActionResult MaterialStock_Input()
        {
            return View();
        }

        [Route("Xuat-vat-lieu.html")]
        // GET: MaterialStock
        public ActionResult MaterialStock_Output()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Import_MaterialStock_Input(string date)
        {
            List<clsError> Error = new List<clsError>();
            try
            {
                string name = UploadImportFiles(Request.Files);
                var obj = new C222_MeetingContent();
                DateTime fromDate;
                if(!DateTime.TryParse(date,out fromDate))
                {
                    fromDate = DateTime.ParseExact(date.Substring(0, 24),
                                  "ddd MMM d yyyy HH:mm:ss",
                                  System.Globalization.CultureInfo.InvariantCulture).Date;
                }

                IImport import = new ImportMaterialStockInput();
                import.ImportData(name, Session["StaffID"].ToString(), fromDate, out Error);
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
        public JsonResult Import_MaterialStock_Output(string date)
        {
            List<clsError> Error = new List<clsError>();
            try
            {
                string name = UploadImportFiles(Request.Files);
                var obj = new C222_MeetingContent();
                DateTime fromDate;
                if(!DateTime.TryParse(date,out fromDate))
                {
                    fromDate = DateTime.ParseExact(date.Substring(0, 24),
                                  "ddd MMM d yyyy HH:mm:ss",
                                  System.Globalization.CultureInfo.InvariantCulture).Date;
                }

                IImport import = new ImportMaterialStockOutput();
                import.ImportData(name, Session["StaffID"].ToString(), fromDate, out Error);
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