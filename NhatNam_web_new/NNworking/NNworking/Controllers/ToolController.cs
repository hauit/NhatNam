using NNworking.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NNworking.Controllers
{
    [RoutePrefix("Kho")]
    public class ToolController : BaseController
    {
        // GET: Tool
        public ActionResult Index()
        {
            CheckPermissAndRedirect();
            return View();
        }

        public ActionResult ToolList()
        {
            CheckPermissAndRedirect();
            return View("~/Views/Tool/InputData/ToolList.cshtml");
        }

        public ActionResult ToolCatergory()
        {
            CheckPermissAndRedirect();
            return View("~/Views/Tool/InputData/ToolCatergory.cshtml");
        }

        public ActionResult ToolType()
        {
            CheckPermissAndRedirect();
            return View("~/Views/Tool/InputData/ToolType.cshtml");
        }

        [Route("Vi-tri-gia-de-hang.html")]
        public ActionResult GiaDeHang()
        {
            CheckPermissAndRedirect();
            return View("~/Views/Tool/InputData/GiaDeHang.cshtml");

        }

        [Route("so-luong-don-hang-kiem-ke.html")]
        public ActionResult InventoryData()
        {
            CheckPermissAndRedirect();
            return View("~/Views/Tool/InputData/InventoryData.cshtml");

        }

        [Route("Xac-nhan-hang-nhap-kho.html")]
        public ActionResult InputVerification()
        {
            CheckPermissAndRedirect();
            return View("~/Views/Tool/InputVerification.cshtml");

        }

        [Route("nhap-kho.html")]
        public ActionResult StokerInput()
        {
            CheckPermissAndRedirect();
            return View("~/Views/Stocker/StokerInput.cshtml");
        }

        [Route("xuat-kho.html")]
        public ActionResult StokerOutput()
        {
            CheckPermissAndRedirect();
            return View("~/Views/Stocker/StokerOutput.cshtml");
        }
        [Route("tinh-toan-xuat-kho.html")]
        public ActionResult StockerOutputCalculation()
        {
            CheckPermissAndRedirect();
            return View("~/Views/Stocker/StockerOutputCalculation.cshtml");
        }

        public ActionResult InventoryImport()
        {
            CheckPermissAndRedirect();
            return View();
        }

        public ActionResult InventoryExport()
        {
            CheckPermissAndRedirect();
            return View();
        }

        public ActionResult InventoryAvailable()
        {
            CheckPermissAndRedirect();
            return View();
        }

        public ActionResult TrackingHistory()
        {
            CheckPermissAndRedirect();
            return View();
        }

        [HttpGet]
        public JsonResult GetOrderInformation(string order)
        {
            try
            {
                var db = new NN_DatabaseEntities();
                var data = db.View_242_BusOder.Where(x => x.MONo.ToLower() == order.ToLower()).FirstOrDefault();
                if(data == null)
                {
                    throw new ArgumentException("Không tìm thấy order trong busOrderList. Vui lòng kiểm tra lại order");
                }

                return Json(new { Status = "OK", Values = data });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult InputTrackingManual(string fromDept, string toDept, string order, string partID, string slg, string vitri, string ghichu, string ngNhan)
        {
            try
            {
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                C242_InventoryReceived po = new C242_InventoryReceived();
                po.Date = DateTime.Now;
                po.Deleted = false;
                po.Import = true;
                po.ImportFrom = fromDept;
                po.Note = string.Empty;
                po.ReceiveDept = toDept;
                po.StaffID = Session["StaffID"].ToString();
                db.C242_InventoryReceived.Add(po);
                db.SaveChanges();
                try
                {
                    C242_InventoryReceivedDetail obj = new C242_InventoryReceivedDetail();
                    obj.VoucherID = po.ID;
                    obj.Deleted = false;
                    obj.GiaDe = vitri;
                    obj.Note = ghichu;
                    obj.OrderNumber = order;
                    obj.PartNo = string.Empty;
                    var data = db.View_242_BusOder.Where(x => x.BOderNo.ToLower() == obj.OrderNumber.ToLower()).FirstOrDefault();
                    if (data != null)
                    {
                        obj.PartNo = data.PartID;
                    }
                    else
                    {
                        throw new ArgumentException($@"Số order {order} không tồn tại trong bảng danh sách order");
                    }

                    obj.NguoiNhan = ngNhan;
                    obj.Qty = 0;
                    int qty;
                    if (int.TryParse(slg,out qty))
                    {
                        obj.Qty = qty;
                    }

                    obj.Price = 0;
                    obj.StatusID = 0;
                    db.C242_InventoryReceivedDetail.Add(obj);
                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    db.C242_InventoryReceived.Remove(po);
                    throw new ArgumentException(ex.Message);
                }

                return Json(new { Status = "OK", Values = string.Empty });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult InputInventoryData(string date, string dept, string order, int qty)
        {
            try
            {
                DateTime inputDate;
                if(!DateTime.TryParse(date,out inputDate))
                {
                    inputDate = DateTime.ParseExact(date.Substring(0, 24),
                                  "ddd MMM d yyyy HH:mm:ss",
                                  System.Globalization.CultureInfo.InvariantCulture).Date;
                }

                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var bOrder = db.C242_BusOder.Where(x => x.BOderNo.ToUpper() == order.ToUpper() && !x.Deleted).FirstOrDefault();
                if(bOrder != null)
                {
                    var inventoryData = db.C242_BusOder_ActualQty.Where(x => x.BusOder.ToUpper() == order.ToUpper() && x.date == inputDate).ToList();
                    int inventoryQty = 0;
                    foreach(var item in inventoryData)
                    {
                        inventoryQty += item.Qty == null ? 0 : (int)item.Qty;
                    }

                    inventoryQty += qty;

                    if(inventoryQty > bOrder.MOQty)
                    {
                        throw new ArgumentException($@"Tổng số lượng đã nhập: {inventoryQty - qty}. Số  lượng sẽ nhập thêm là: {qty}. Số lượng lệnh chỉ có: {bOrder.MOQty}. Vượt quá số lượng lệnh vui lòng xem lại!");
                    }
                }

                var obj = new C242_BusOder_ActualQty();
                obj.BusOder = order;
                obj.date = inputDate;
                obj.dept = dept;
                obj.Qty = qty;
                db.C242_BusOder_ActualQty.Add(obj);
                db.SaveChanges();
                return Json(new { Status = "OK", Values = string.Empty });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ImportStockerOutput(string order, List<sp_222_StockerOutput_OutputPreparation_Detail_Result> data )
        {
            try
            {
                if(data.Count == 0)
                {
                    throw new ArgumentException("Chưa nhập dữ liệu chi tiết.");
                }

                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var obj = new C222_StokerOutput();
                obj.Date = DateTime.Now.Date;
                obj.Note = string.Empty;
                obj.OrderNo = order;
                obj.Number = $@"{obj.OrderNo}-{obj.Date.ToString("yyMMdd")}-{DateTime.Now.ToString("yyMMddHHmm")}";
                obj.Qty = data[0].NeedQty;
                obj.StaffID = "";
                obj.ToDept = "QM";
                db.C222_StokerOutput.Add(obj);
                foreach(var item in data)
                {
                    if(item.UseQty == 0)
                    {
                        continue;
                    }

                    var objDetail = new C222_StockerOutputDEtail();
                    objDetail.Number = obj.Number;
                    objDetail.OderCan = obj.OrderNo;
                    objDetail.OrderCo = item.OrderNo;
                    objDetail.SlgCan = obj.Qty;
                    objDetail.SlgCo = item.UseQty;
                    objDetail.GiayXN = item.GiayXN;
                    db.C222_StockerOutputDEtail.Add(objDetail);
                }
                db.SaveChanges();
                return Json(new { Status = "OK", Values = "Cập nhật thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = "Error occurred. Error details: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ImportSuppendStaffPhu()
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count == 0)
            {
                return Json(new { Status = "NG", Values = "Chưa chọn file import." });
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
                    //return Json(new { Status = "OK", Values = name, Errors = Error });
                    ImportGiaoNhan(name, out Error);
                }
                return Json(new { Status = "OK", Values = "Import thành công!", Errors = Error });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = "Error occurred. Error details: " + ex.Message });
            }
        }

        private bool ImportGiaoNhan(string fname, out List<clsError> Error)
        {
            Error = new List<clsError>();
            ExcelPackage.License.SetNonCommercialPersonal("Alo1234");
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
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
                    List<C242_InventoryReceivedDetail> listInserted = new List<C242_InventoryReceivedDetail>();
                    while (line < 1000000)
                    {
                        line++;
                        try
                        {
                            var staff = item.Cells["A" + line].Value;
                            string staffID = staff == null ? string.Empty : staff.ToString().Trim();
                            if (string.IsNullOrEmpty(staffID))
                            {
                                break;
                            }

                            DateTime date = DateTime.Parse(item.Cells["B" + line].Value.ToString().Trim()).Date;

                            var trua = item.Cells["C" + line].Value;
                            var toi = item.Cells["D" + line].Value;
                            var dem = item.Cells["E" + line].Value;
                            var anTrua = trua == null ? string.Empty : (!string.IsNullOrEmpty(trua.ToString()) ? "CO" : string.Empty);
                            var anToi = toi == null ? string.Empty : (!string.IsNullOrEmpty(toi.ToString()) ? "CO" : string.Empty);
                            var anDem = toi == null ? string.Empty : (!string.IsNullOrEmpty(toi.ToString()) ? "CO" : string.Empty);

                            C242_InventoryReceivedDetail obj;
                            ////TODO: Confirm inserted information
                            var inserted = listInserted.Any();
                            if (inserted)
                            {
                                continue;
                            }

                            ////TODO: Check existed data
                            obj = db.C242_InventoryReceivedDetail.Where(x => x.PartNo == staffID).FirstOrDefault();
                            if (obj == null)
                            {
                                ////TODO: Add new data
                                listInserted.Add(obj);
                                continue;
                            }

                            ////TODO: Update Data
                        }
                        catch (Exception ex)
                        {
                            Error.Add(new clsError(line, "Not OK", ex.Message));
                        }
                    }

                    db.SaveChanges();
                }
            }

            return true;
        }

        private string ChangeName(string fname)
        {
            string path = Path.GetDirectoryName(fname);
            string extention = Path.GetExtension(fname);
            string newName = $"{path}\\{DateTime.Now.ToString("yyyyMMddHHmmss")}{extention}";
            return newName;
        }
    }
}