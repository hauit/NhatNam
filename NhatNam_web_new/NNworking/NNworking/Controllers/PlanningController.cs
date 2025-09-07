using NNworking.Models;
using NNworking.Models.Import;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NNworking.Controllers
{
    [RoutePrefix("Ke-hoach")]
    public class PlanningController : BaseController
    {
        // GET: Planning
        public ActionResult Index()
        {
            return View();
        }

        //[Route("Thong-tin-san-xuat-don-hang-{order}.html")]
        public ActionResult ToLenh(string order)
        {
            ViewBag.Order = order;
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var data = db.C242_MachinePlanning_view.Where(x=>x.Order.ToLower() == order.ToLower() && x.BatDau != null).FirstOrDefault();
            //ViewBag.PartNote = string.Empty;
            if (data != null)
            {
                var partList = db.C242_Part.Where(x => x.PartNo.ToLower() == data.TenChiTiet.ToLower()).FirstOrDefault();
                string customer = partList == null ? string.Empty : partList.CustomerID;
                ViewBag.GiaThanh = GetGiaThanh(data.TenChiTiet);
                ViewBag.PathsCT = GetCTGCFilePath(customer,data.TenChiTiet);
                ViewBag.Paths = GetHDGCFilePath(customer,data.TenChiTiet);
                ViewBag.PathsDraw = GetDrawPath(customer,data.TenChiTiet);

                ViewBag.PartNote = GetPartNote(data.TenChiTiet);
                ViewBag.OrderNote = GetOrderNote(order);
            }

            return View();
        }

        [Route("chuan-bi-cho-ke-hoach.html")]
        public ActionResult PlanPreparation()
        {
            return View();
        }

        [Route("phe-duyet-ke-hoach.html")]
        public ActionResult CheckPlan()
        {
            return View();
        }

        [Route("option-data.html")]
        public ActionResult OptionDataList()
        {
            return View();
        }

        private string GetGiaThanh(string tenChiTiet)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var data = db.View_242_Part.Where(x => x.PartNo.ToLower() == tenChiTiet.ToLower()).FirstOrDefault();
            return data == null ? string.Empty : (data.GiaThanh.ToString() == "1" ? "Đắt" : "Rẻ");
        }

        private string GetOrderNote(string order)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var data = db.View_242_BusOder.Where(x => x.BOderNo.ToLower() == order.ToLower()).FirstOrDefault();
            return data == null ? string.Empty : data.Note;
        }

        private string GetPartNote(string tenChiTiet)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var data = db.C242_PartData.Where(x => x.PartID.ToLower() == tenChiTiet.ToLower()).FirstOrDefault();
            return data == null ? string.Empty : data.Note;
        }

        [HttpPost]
        public JsonResult GetToLenhData(string order)
        {
            try
            {
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var data = db.View_ToLenh.Where(x=>x.BOderNo.ToLower() == order.ToLower()).ToList();
                return Json(new { Status = "OK", Values = data });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult PlanOK(string fromDate, string toDate)
        {
            try
            {
                var fromdate = DateTime.ParseExact(fromDate.Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture).Date;
                var todate = DateTime.ParseExact(toDate.Substring(0, 24),
                                  "ddd MMM d yyyy HH:mm:ss",
                                  System.Globalization.CultureInfo.InvariantCulture).Date;

                NN_DatabaseEntities db = new NN_DatabaseEntities();
                db.sp_242_MachinePlanning_PlanOK(fromdate, todate);
                return Json(new { Status = "OK", Values = "Update thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message });
            }
        }

        //[Route("PdfViewer/{path}.html")]
        public FileResult PdfView(string path, string name)
        {
            try
            {
                string dir = string.Empty;
                dir = GetDirFromPathName(name);
                path = $@"{dir}\{path}";
                //path = $@"D:\ihoadon.vn_0201311397-001_355_02042024.pdf";
                //path = $@"D:\DATA_KH(BAN_VE)\QM\4B-PT0940310.pdf";
                byte[] bytes = System.IO.File.ReadAllBytes(path);
                return File(bytes, "application/pdf");
            }
            catch (Exception ex)
            {
                ViewBag.Notification = ex.Message;
                return null;
            }
        }

        private string GetDirFromPathName(string name)
        {
            //// TODO: need to change to pattern matching for more safe
            string result = string.Empty;
            switch (name)
            {
                case nameof(RoodPathHDGC):
                    result = RoodPathHDGC;
                    break;
                case nameof(RoodPathCTGC):
                    result = RoodPathCTGC;
                    break;
                case nameof(RoodPathDraw):
                    result = RoodPathDraw;
                    break;
            }

            return result;
        }

        private Dictionary<string, string> GetCTGCFilePath(string customer, string part)
        {
            Dictionary<string, string> listFile = new Dictionary<string, string>();
            try
            {
                //fileName = $@"{roodPath}\{machineID.Substring(0, machineID.Length - 2)}\NESTING+LAYOUT\{DateTime.Now.Year}\Thang {DateTime.Now.Month}\{fileName.Substring(0,5).Replace("_","-")}-{DateTime.Now.ToString("yy")}\{fileName}.pdf";
                //var files = Directory.GetFiles(RoodPathCTGC);
                var files = Directory.GetFiles($@"{RoodPathCTGC}\{customer}\{part}");
                foreach (var item in files)
                {
                    var fileName = Path.GetFileName(item);
                    if(fileName.ToLower().IndexOf(part.ToLower()) == -1)
                    {
                        continue;
                    }
                    listFile.Add($@"{customer}\{part}\{fileName}", nameof(RoodPathCTGC));
                }
            }
            catch(Exception ex)
            {

            }
            if(listFile.Count == 0)
            {
                listFile.Add(string.Empty, string.Empty);
            }
            return listFile;
        }

        private Dictionary<string,string> GetHDGCFilePath(string customer, string part)
        {
            Dictionary<string, string> listFile = new Dictionary<string, string>();
            try
            {
                //fileName = $@"{roodPath}\{machineID.Substring(0, machineID.Length - 2)}\NESTING+LAYOUT\{DateTime.Now.Year}\Thang {DateTime.Now.Month}\{fileName.Substring(0,5).Replace("_","-")}-{DateTime.Now.ToString("yy")}\{fileName}.pdf";
                var files = Directory.GetFiles($@"{RoodPathHDGC}\{customer}\{part}");
                foreach (var item in files)
                {
                    var fileName = Path.GetFileName(item);
                    if (fileName.ToLower().IndexOf(part.ToLower()) != -1)
                    {
                        listFile.Add($@"{customer}\{part}\{fileName}", nameof(RoodPathHDGC));
                    }
                }
            }
            catch(Exception ex)
            {
            }

            if (listFile.Count == 0)
            {
                listFile.Add(string.Empty, string.Empty);
            }
            return listFile;
        }

        private Dictionary<string,string> GetDrawPath(string customer, string part)
        {
            Dictionary<string, string> listFile = new Dictionary<string, string>();
            try
            {
                //fileName = $@"{roodPath}\{machineID.Substring(0, machineID.Length - 2)}\NESTING+LAYOUT\{DateTime.Now.Year}\Thang {DateTime.Now.Month}\{fileName.Substring(0,5).Replace("_","-")}-{DateTime.Now.ToString("yy")}\{fileName}.pdf";
                var files = Directory.GetFiles($@"{RoodPathDraw}\{customer}");
                foreach (var item in files)
                {
                    if (item.ToLower().IndexOf(part.ToLower()) != -1)
                    {
                        var fileName = Path.GetFileName(item);
                        listFile.Add($@"{customer}\{fileName}", nameof(RoodPathDraw));
                    }
                }
            }
            catch(Exception ex)
            {

            }

            if (listFile.Count == 0)
            {
                listFile.Add(string.Empty, string.Empty);
            }
            return listFile;
        }

        [HttpPost]
        public JsonResult ImportMachinePlaning()
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
                string shift = Request.Form.AllKeys[0];
                var date = DateTime.ParseExact(Request.Form.AllKeys[1].Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture);
                if(string.IsNullOrEmpty(shift))
                {
                    throw new ArgumentException("chưa chọn ca để import!");
                }

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
                    ImportData(name, out Error, shift, date);
                }
                return Json(new { Status = "OK", Values = "Import thành công!", Errors = Error });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = "Error occurred. Error details: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ImportPlanPreparation()
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
                //var date = DateTime.ParseExact(Request.Form.AllKeys[0].Substring(0, 24),
                //              "ddd MMM d yyyy HH:mm:ss",
                //              System.Globalization.CultureInfo.InvariantCulture);
                var date = Request.Form.AllKeys[0];
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
                    IImport import = new ImportPlanPreparation();
                    import.ImportData(name, date, out Error);
                }
                return Json(new { Status = "OK", Values = "Import thành công!", Errors = Error });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = "Error occurred. Error details: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult ImportTDTK(string date)
        {
            List<clsError> Error = new List<clsError>();
            try
            {
                string name = UploadImportFiles(Request.Files);
                IImport import = new ImportBusOrder();
                import.ImportData(name, Session["StaffID"].ToString(), out Error,(int)ImportBOderType.ImportTDTK);
                if(Error.Count > 0)
                {
                    throw new ArgumentException("Có lỗi trong quá trình import. Vui lòng xem chi tiết lỗi ở từng dòng! ");
                }

                return Json(new { Status = "OK", Values = "Import OK", Errors = Error }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = "Error occurred. Error details: " + ex.Message, Errors = Error });
            }
        }

        private string ChangeName(string fname)
        {
            string path = Path.GetDirectoryName(fname);
            string extention = Path.GetExtension(fname);
            string newName = $"{path}/{DateTime.Now.ToString("yyyyMMddHHmmss")}{extention}";
            return newName;
        }

        private bool ImportData(string fname, out List<clsError> Error,string shift,DateTime date)
        {
            Error = new List<clsError>();
            OleDbDataReader dReader;
            OleDbConnection excelConnection = null;
            bool result = true;
            try
            {
                string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                                        fname +
                                                        ";Extended Properties=Excel 12.0;Persist Security Info=False";
                excelConnection = new OleDbConnection(excelConnectionString);
                OleDbCommand cmd =
                    new OleDbCommand("select * from [Sheet1$] ",
                                        excelConnection);
                excelConnection.Open();

                dReader = cmd.ExecuteReader();
                int line = 0;
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                List<C242_MachinePlanning> listInserted = new List<C242_MachinePlanning>();
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
                while (dReader.Read())
                {
                    try
                    {
                        i++;
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
                        //TH = DateTime.Parse(DateTime.Parse(dReader["Thời hạn"].ToString().Trim()).ToShortDateString().Trim() + " 08:00:00").ToString("MM/dd/yyyy hh:mm:ss");
                        var a = dReader["Bắt đầu"].ToString().Trim();
                        //Start = DateTime.Parse(DateTime.Parse(dReader["Bắt đầu"].ToString().Trim()).ToShortDateString().Trim()).ToString("MM/dd/yyyy");
                        //Finish = DateTime.Parse(DateTime.Parse(dReader["Kết thúc"].ToString().Trim()).ToShortDateString().Trim() + " 23:59:59").ToString("MM/dd/yyyy hh:mm:ss");
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
                        tablePlanning.Date = date;

                        tablePlanning.Order = Oder;
                        if (string.IsNullOrEmpty(tablePlanning.Order))
                        {
                            continue;
                        }

                        tablePlanning.NC = NC;
                        tablePlanning.MayGC = MachineID;
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
                        tablePlanning.Fac_TTFile = dReader["tinh trang file"].ToString().Trim();

                        tablePlanning.Fac_NGTruoc = string.Empty;

                        var inserted = listInserted.Where(x => x.Order == tablePlanning.Order 
                            && x.Date == tablePlanning.Date 
                            && x.NC == tablePlanning.NC 
                            && x.TenChiTiet == tablePlanning.TenChiTiet 
                            && x.Shift == tablePlanning.Shift).Any();
                        if (inserted)
                        {
                            continue;
                        }

                        db.C242_MachinePlanning.Add(tablePlanning);
                    }
                    catch (Exception ex)
                    {
                        Error.Add(new clsError(line, "Not OK", ex.Message));
                    }
                }

                db.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                if (excelConnection.State == System.Data.ConnectionState.Open)
                {
                    excelConnection.Close();
                }
                Error.Add(new clsError(0, "NG", ex.Message));
                return result;
            }
        }
    }

}