using Newtonsoft.Json.Linq;
using NNworking.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NNworking.Controllers
{
    public class AnodWTSController : BaseController
    {
        // GET: AnodWTS
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetPartNoByMONo(string mono)
        {
            //NN_DatabaseEntities db = new NN_DatabaseEntities();
            //var data = db.sp_242_GetOptionIDByMONo_For_AnodWTS(mono).FirstOrDefault();
            //if (data == null)
            //{
            //    return Json("NG", JsonRequestBehavior.AllowGet);
            //}

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [Route("PdfViewer/{fileName?}.html")]
        public FileResult PdfView(string fileName)
        {
            if(fileName.IndexOf(" ") != -1)
            {
                fileName = fileName.Substring(0, fileName.Length - 2);
            }

            fileName = fileName.Replace(" ", "+");
            ViewBag.FileName = fileName;
            ViewBag.Notification = string.Empty;

            try
            {
                fileName = GetFilePath(fileName);
                if(string.IsNullOrEmpty(fileName))
                {
                    ViewBag.Notification = $"Không tìm thấy bản vẽ của chi tiết {ViewBag.FileName}.";
                    return null;
                }

                byte[] bytes = System.IO.File.ReadAllBytes(fileName);
                return File(bytes, "application/pdf");
            }
            catch (Exception ex)
            {
                ViewBag.Notification = ex.Message;
                return null;
            }
        }

        private string GetFilePath(string fileName)
        {
            string path = @"\\192.168.0.5\Processing\Anod\XLBM\Dang ky NC moi\Scan Ban ve";
            var folders = Directory.GetDirectories(path);
            foreach (var folder in folders)
            {
                var files = Directory.GetFiles(folder);
                foreach (var file in files)
                {
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                    if (fileNameWithoutExtension.ToLower() == fileName.ToLower())
                    {
                        fileName = file;
                        return fileName;
                    }
                }
            }

            return string.Empty;
        }

        public JsonResult StartToCalculateTime()
        {
            try
            {
                HttpCookie userInfo = new HttpCookie("Ma_DT");
                userInfo["Time"] = DateTime.Now.ToString();
                userInfo.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(userInfo);

                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult FinishToCalculateTime()
        {
            try
            {
                HttpCookie userInfo = new HttpCookie("Ma_DT");
                userInfo["Time"] = DateTime.Now.ToString();
                userInfo.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(userInfo);

                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult InputOrderNotInPlan(string shift, string machineID, string mono, string partID, int slg, string optionID)
        {
            try
            {
                if (string.IsNullOrEmpty(optionID))
                {
                    optionID = "STG01";
                }

                if (string.IsNullOrEmpty(machineID))
                {
                    machineID = "FINISH";
                }

                DateTime date = ((shift == "T2" || shift == "T2") && DateTime.Now.Hour <= 6) ? DateTime.Now.AddDays(-1).Date : DateTime.Now.Date;
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var dataExisted = db.C242_MachinePlanning.Where(x =>
                        x.Order.ToLower() == mono.ToLower()
                        && x.NC.ToLower() == optionID.ToLower()
                        && x.Shift.ToLower() == shift.ToLower()
                        && x.MayGC.ToLower() == machineID.ToLower()
                        && x.Date == date
                ).Any();

                if (dataExisted)
                {
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }

                //var idData = db.C242_MachinePlanning.Max(item => item.ID);
                var maxid = db.C242_MachinePlanning.Max(item => item.ID);
                C242_MachinePlanning addNew = new C242_MachinePlanning();
                addNew.Shift = shift;
                addNew.Date = date;
                addNew.Fac_MachineID = machineID;
                addNew.MayGC = machineID;
                addNew.Order = mono;
                addNew.TenChiTiet = partID;
                addNew.Slglenh = slg;
                addNew.K = string.Format("N{0}", maxid);
                addNew.Dept = "MA";
                addNew.NC = optionID;
                addNew.Cua_Factory = string.Empty;
                addNew.Cua_GCTime = DateTime.Now;
                addNew.Cua_MachineGroup = string.Empty;
                addNew.Cua_Plantime = 0;
                addNew.Cua_PrepareDate = string.Empty;
                addNew.Cua_Size = string.Empty;
                addNew.Cua_State = false;
                addNew.Cua_TotalTime = 0;
                db.C242_MachinePlanning.Add(addNew);
                db.SaveChanges();

                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public string GetOptionID(int? ID)
        {
            switch (ID)
            {
                case 1: return "XOXTR";
                case 2: return "XOXBK";
                case 3: return "XOXBO";
                case 4: return "XOXCA";
                case 5: return "XOXPC";
                case 6: return "XOXNK";
                case 7: return "XOX10";
                case 8: return "XOX30";
                case 101: return "STG01";
                case 102: return "STG02";
                case 103: return "STG03";
                case 104: return "STG04";
                case 105: return "STG05";
                case 106: return "STG06";
                case 107: return "STG07";
                case 108: return "STG08";
                case 109: return "STG09";
                default: return "-1";
            } 
        }
        [HttpPost]
        public JsonResult StartWorking(string workID, string shift, string machineID, string staffID, string[] list, string note)
        {
            try
            {
                DateTime date = DateTime.Now;
                var listWorkID = new List<string>() { "AA41" , "HTBM" , "GN" };
                if ((machineID.ToUpper() == "KO_CO" || list == null) && !listWorkID.Where(x => x == workID).Any())
                {
                    C242_WTS c242_CuaWTS = new C242_WTS();
                    if ((shift == "T2" || shift == "T3") && (date.Hour * 60 + date.Minute) <= 620)
                    {
                        date = date.AddDays(-1);
                    }
                    date = date.Date;
                    
                    c242_CuaWTS.Shift = shift;
                    //c242_CuaWTS.OptionID = "STG00";
                    c242_CuaWTS.MachineID = machineID;
                    c242_CuaWTS.StaffID = staffID;
                    c242_CuaWTS.Date = date;
                    c242_CuaWTS.MONo = date.ToString("GyyyyMM");
                    c242_CuaWTS.OptionID = "GT";
                    c242_CuaWTS.WorkID = workID;
                    c242_CuaWTS.OKQty = 0;
                    c242_CuaWTS.NGQty = 0;
                    c242_CuaWTS.Time = 0;
                    c242_CuaWTS.ProTime = 0;
                    c242_CuaWTS.ClampTime = 0;
                    c242_CuaWTS.StartTime = DateTime.Now;
                    var deviceName = Request.UserHostAddress;
                    c242_CuaWTS.Note2 = deviceName;
                    NN_DatabaseEntities db = new NN_DatabaseEntities();
                    db.C242_WTS.Add(c242_CuaWTS);
                    string result = c242_CuaWTS.ID + "-0-0";
                    AddNewCookie(result, staffID, workID);
                    db.SaveChanges();
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }

                if (workID == "AA41")
                {
                    string listResult1 = InputDataFoAA41(staffID, machineID, shift, note);
                    AddNewCookie(listResult1, staffID, workID);
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }

                if(!DanhBongWTSExisted(list, workID))
                {
                    throw new ArgumentException($@"Chưa nhập WTS đánh bóng, không thể nhập WTS mạ");
                }

                if (workID == "HTBM")
                {
                    bool checkHTBM = CheckStaffMeMa(staffID);
                    if (!checkHTBM)
                    {
                        throw new ArgumentException("Mã nhân viên phải được nhập vào mục đăng kí mẻ mạ thì mới được nhập WTS hệ thống bể mạ");
                    }

                    AddNewCookie(shift, staffID, "HTBM");
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }

                if (workID == "GN")
                {
                    //NN_DatabaseEntities db1 = new NN_DatabaseEntities();
                    //bool checkGN = db1.sp_242_WTSMa_GetTrackingOrderViaStaffID(date.ToString("yyyyMMdd"), staffID).Any();
                    //if (!checkGN)
                    //{
                    //    throw new ArgumentException($@"Không tìm thấy dữ liệu giao nhận của nhân viên {staffID}");
                    //}

                    //AddNewCookie(shift, staffID, "GN");
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(machineID))
                {
                    machineID = "Anod";
                }

                string listResult = string.Empty;
                foreach (string item in list)
                {
                    string item1 = item.Replace("\"", string.Empty);
                    var info = item1.Split('-');
                    if (info.Length < 2)
                    {
                        continue;
                    }

                    int planID = int.Parse(info[0]);
                    NN_DatabaseEntities db = new NN_DatabaseEntities();
                    var c242_Plan = db.C242_MachinePlanning.Find(planID);
                    C242_WTS c242_CuaWTS = new C242_WTS();
                    ////TODO: chưa nhập ngoài kế hoạch thì ko check nguyên công
                    //CheckCorrectOptionID(c242_Plan.TenChiTiet, c242_Plan.NC);
                    c242_CuaWTS.Shift = shift;
                    c242_CuaWTS.MachineID = machineID;
                    c242_CuaWTS.StaffID = staffID;
                    c242_CuaWTS.Note1 = c242_Plan.TenChiTiet + "+" + note;
                    c242_CuaWTS.MONo = c242_Plan.Order;
                    c242_CuaWTS.OptionID = c242_Plan.NC;
                    //c242_CuaWTS.To = c242_Plan.Slglenh.ToString();
                    if (string.IsNullOrEmpty(c242_CuaWTS.OptionID))
                    {
                        c242_CuaWTS.OptionID = "STG00";
                    }

                    c242_CuaWTS.OKQty = 0;
                    c242_CuaWTS.NGQty = 0;
                    c242_CuaWTS.Time = 0;
                    c242_CuaWTS.ProTime = 0;
                    c242_CuaWTS.ClampTime = 0;
                    //c242_CuaWTS.SubOKQty = 0;
                    //c242_CuaWTS.SubNGQty = 0;
                    c242_CuaWTS.OptionID = c242_CuaWTS.OptionID;
                    c242_CuaWTS.WorkID = workID;
                    var deviceName = Request.UserHostAddress;
                    c242_CuaWTS.Note2 = deviceName;
                    listResult += InsertToDataBase(c242_CuaWTS, c242_Plan) + "+";
                }
                AddNewCookie(listResult, staffID, workID);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        private bool DanhBongWTSExisted(string[] list, string workID)
        {
            var result = true;
            if(workID.ToUpper() == "AA51" && list.Length > 0)
            {
                string item = list[0].Replace("\"", string.Empty);
                var info = item.Split('-');
                int planID = int.Parse(info[0]);
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var plan = db.C242_MachinePlanning.Where(x => x.ID == planID).FirstOrDefault();
                if(plan == null)
                    result = false;
                else
                {
                    var option = db.C242_OptionData.Where(x => x.PartID.ToLower() == plan.TenChiTiet.ToLower() && x.OptionID.ToUpper().StartsWith("XHLO")).FirstOrDefault();
                    if(option != null)
                    {
                        result = db.C242_WTS.Where(x => x.MONo == plan.Order && x.OptionID.ToUpper() == option.OptionID && x.OKQty > 0).Any();
                    }
                }
            }


            return result;
        }

        private string InputDataFoAA41(string staffID, string machineID, string shift, string note)
        {
            string listResult1 = string.Empty;
            DateTime date = DateTime.Now;
            C242_WTS c242_CuaWTS = new C242_WTS();
            if ((shift == "T2" || shift == "T3") && (date.Hour * 60 + date.Minute) <= 620)
            {
                date = date.AddDays(-1);
            }
            date = date.Date;
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            //var MonoList = db.Proc_242_GetMONoByShift(shift, date).ToList();
            //foreach (var row in MonoList)
            //{
            //    c242_CuaWTS.Shift = shift;
            //    //c242_CuaWTS.OptionID = "STG00";
            //    c242_CuaWTS.MachineID = machineID;
            //    c242_CuaWTS.StaffID = staffID;
            //    c242_CuaWTS.Date = date;
            //    c242_CuaWTS.MONo = row.ToString();
            //    var data = db.C242_DLMa.Where(s => s.MONo == c242_CuaWTS.MONo).FirstOrDefault();
            //    c242_CuaWTS.OptionID = GetOptionID(data.OptionID);
            //    c242_CuaWTS.WorkID = "AA41";
            //    c242_CuaWTS.OKQty = 0;
            //    c242_CuaWTS.NGQty = 0;
            //    c242_CuaWTS.Time = 0;
            //    c242_CuaWTS.ProTime = 0;
            //    c242_CuaWTS.ClampTime = 0;
            //    c242_CuaWTS.StartTime = DateTime.Now;
            //    var deviceName = Request.UserHostAddress;
            //    c242_CuaWTS.Note2 = deviceName;
            //    db.C242_WTS.Add(c242_CuaWTS);
            //    db.SaveChanges();
            //    listResult1 += c242_CuaWTS.ID + "-0-0" + "+";
            //}

            return listResult1;
        }

        private void FiniGN(string staffID, int bufferTime)
        {
            string staffCookie = staffID.Replace(';', '+');
            var shift = Request.Cookies[staffCookie]["List"].ToString();
            var startTime = DateTime.Parse(Request.Cookies[staffCookie]["StartTime"].ToString());
            var finishTime = DateTime.Now;
            DateTime date = DateTime.Now;
            if ((shift == "T2" || shift == "T3") && (date.Hour * 60 + date.Minute) <= 620)
            {
                date = date.AddDays(-1);
            }

            date = date.Date;
            
            //NN_DatabaseEntities db1 = new NN_DatabaseEntities();
            //foreach (var staff in staffID.Split(';'))
            //{
            //    var listMONo = db1.sp_242_WTSMa_GetTrackingOrderViaStaffID(date.ToString("yyyyMMdd"), staff).ToList();
            //    if(listMONo.Count == 0) break;
            //    var time = bufferTime / listMONo.Count;
            //    foreach (var item in listMONo)
            //    {
            //        C242_WTS C242_WTS = new C242_WTS();
            //        C242_WTS.StartTime = startTime;
            //        C242_WTS.FinishTime = finishTime;
            //        C242_WTS.Time = time;
            //        C242_WTS.WorkID = "A561";
            //        C242_WTS.StaffID = staff;
            //        C242_WTS.MachineID = "Anod";
            //        C242_WTS.Shift = shift;
            //        C242_WTS.Date = date;
            //        C242_WTS.Note2 = string.Empty;
            //        C242_WTS.MONo = item;
            //        C242_WTS.Note1 = string.Empty;
            //        C242_WTS.OptionID = "GT";////TODO: need to confirm 
            //        C242_WTS.OKQty = 0;
            //        C242_WTS.NGQty = 0;
            //        //C242_WTS.SubNGQty = 0;
            //        //C242_WTS.SubOKQty = 0;
            //        C242_WTS.ProTime = 0;
            //        C242_WTS.ClampTime = 0;
            //        db1.C242_WTS.Add(C242_WTS);
            //    }
            //}

            //db1.SaveChanges();
        }

        private bool CheckStaffMeMa(string staffID)
        {
            bool result = true;
            var listStaff = staffID.Split(';');
            if (listStaff.Length == 0)
            {
                return false;
            }

            DateTime date = DateTime.Now.Date;
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            foreach (var item in listStaff)
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }

                var data = db.C242_DLMa.Where(x => x.Date == date && x.StaffID.IndexOf(item) != -1).FirstOrDefault();
                if (data == null)
                {
                    return false;
                }
            }

            return result;
        }

        [HttpPost]
        public JsonResult FinishedWork(string oldWork, string staffID, string[] arrayRessult, int bufferTime, int Finish)
        {
            try
            {
                if (bufferTime > 60 && !string.IsNullOrEmpty(oldWork) && oldWork != "GN" && oldWork != "AA41")
                {
                    return Json("Không được bù giờ quá 60 phút.", JsonRequestBehavior.AllowGet);
                }

                var shift = Request.Cookies[staffID.Replace(';', '+')];
                if (shift == null)
                {
                    return Json("OK1", JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(oldWork))
                {
                    oldWork = GetOldWork(staffID);
                }

                if (oldWork == "GN")
                {
                    FiniGN(staffID, bufferTime);
                    RemoveCookie(staffID);
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }

                if (oldWork == "HTBM")
                {
                    FiniHTBM(staffID, bufferTime);
                    RemoveCookie(staffID);
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }


                var listWorking = Request.Cookies[staffID]["List"].ToString().Split('+');
                int a = (listWorking.Length - 1);
                if (oldWork != "AA41")
                {
                    if (a > 1)
                    {
                        bufferTime = bufferTime / a;
                    }
                }

                if (arrayRessult == null || arrayRessult.Length == 0)
                {
                    NN_DatabaseEntities db = new NN_DatabaseEntities();
                    foreach (var item in listWorking)
                    {
                        if (string.IsNullOrEmpty(item))
                        {
                            continue;
                        }

                        var workItem = item.Split('-');
                        var c242_CuaWTS = db.C242_WTS.Find(int.Parse(workItem[0]));
                        c242_CuaWTS.FinishTime = DateTime.Now;
                        c242_CuaWTS.Time = (decimal)CalculateTime(DateTime.Parse(c242_CuaWTS.StartTime.ToString()), DateTime.Now, 1) + bufferTime;
                        if (oldWork == "AA41")
                        {
                            //var MonoNum = db.Proc_242_CountMONoByShift(c242_CuaWTS.Shift, c242_CuaWTS.date).ToArray();
                            c242_CuaWTS.Time = (decimal)Math.Round((CalculateTime(DateTime.Parse(c242_CuaWTS.StartTime.ToString()), DateTime.Now, 1) + bufferTime) / listWorking.Count());
                        }
                        c242_CuaWTS.OKQty = 0;
                        c242_CuaWTS.NGQty = 0;
                    }
                    db.SaveChanges();

                    if (Finish == 1)
                    {
                        RemoveCookie(staffID);
                    }

                    return Json("OK", JsonRequestBehavior.AllowGet);
                }

                string listResult = string.Empty;
                int result = 0;
                foreach (string item in arrayRessult)
                {
                    var info = item.Replace("\"", string.Empty).Split('-');
                    if (info.Length < 2)
                    {
                        continue;
                    }

                    int idInfo;
                    if (!int.TryParse(info[1], out idInfo))
                    {
                        continue;
                    }


                    for (int i = 0; i < listWorking.Length; i++)
                    {
                        var workItem = listWorking[i].Split('-');
                        int id;
                        if (!int.TryParse(workItem[0], out id))
                        {
                            continue;
                        }

                        if (workItem.Length < 3)
                        {
                            continue;
                        }

                        if (workItem[2] != info[0])
                        {
                            continue;
                        }

                        NN_DatabaseEntities db = new NN_DatabaseEntities();
                        var c242_CuaWTS = db.C242_WTS.Find(int.Parse(workItem[0]));
                        c242_CuaWTS.FinishTime = DateTime.Now;
                        c242_CuaWTS.Time = (decimal)CalculateTime(DateTime.Parse(c242_CuaWTS.StartTime.ToString()), DateTime.Now, arrayRessult.Length) + bufferTime;
                        c242_CuaWTS.OKQty = 0;
                        c242_CuaWTS.NGQty = 0;
                        //c242_CuaWTS.SubOKQty = idInfo;
                        //c242_CuaWTS.SubNGQty = int.Parse(info[2]);
                        int curentOK = 0;
                        int planID = int.Parse(workItem[1].Trim());
                        var plan = db.C242_MachinePlanning.Where(x => x.ID == planID).FirstOrDefault();
                        int qty = 0;
                        if (plan == null)
                        {
                            continue;
                        }

                        qty = int.Parse(plan.Slglenh.ToString());
                        //if (c242_CuaWTS.SubNGQty + c242_CuaWTS.SubOKQty > qty)
                        //{
                        //    throw new ArgumentException("Số lượng lũy tiến lớn hơn số lượng lệnh!");
                        //}

                        //c242_CuaWTS.To = (curentOK + c242_CuaWTS.OKQty).ToString() + @"/" + qty;

                        if (c242_CuaWTS.WorkID == "AA71")
                        {
                            c242_CuaWTS.OKQty = idInfo;
                            c242_CuaWTS.NGQty = int.Parse(info[2]);
                            //c242_CuaWTS.SubOKQty = 0;
                            //c242_CuaWTS.SubNGQty = 0;
                        }
                        db.SaveChanges();
                        break;
                    }

                    result++;
                }

                if (result == arrayRessult.Length)
                {
                    if (Finish == 1)
                    {
                        RemoveCookie(staffID);
                    }

                    return Json("OK", JsonRequestBehavior.AllowGet);
                }

                return Json("Chưa kết thúc được công việc. Vui lòng thử lại", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        private string GetOldWork(string staffID)
        {
            string staffCookie = staffID.Replace(';', '+');
            var oldWork = Request.Cookies[staffCookie]["ID"].ToString();
            oldWork = oldWork.Substring(oldWork.Length - 4, 4);
            return oldWork;
        }

        private void FiniHTBM(string staffID, int buffer)
        {
            string staffCookie = staffID.Replace(';', '+');
            var shift = Request.Cookies[staffCookie]["List"].ToString();
            var startTime = DateTime.Parse(Request.Cookies[staffCookie]["StartTime"].ToString());
            var finishTime = DateTime.Now;
            DateTime date = DateTime.Now;
            if ((shift == "T2" || shift == "T3") && (date.Hour * 60 + date.Minute) <= 620)
            {
                date = date.AddDays(-1);
            }

            date = date.Date;
            var time = (finishTime - startTime).TotalMinutes + buffer;

            NN_DatabaseEntities db = new NN_DatabaseEntities();
            foreach (var staff in staffID.Split(';'))
            {
                if (string.IsNullOrEmpty(staff))
                {
                    continue;
                }

                //var listOrder = db.sp_242_GetAllMeMaUnprecedentedData(shift, date, staff).ToList();
                //if (listOrder.Count == 0)
                //{
                //    throw new ArgumentException("Không tìm thấy dữ liệu mẻ mạ. Hoặc đã nhập WTS cho các mẻ mạ cũ, chưa có mẻ mạ mới.");
                //}
                //int timePerOrder = (int)time / listOrder.Count;


                //foreach (var item in listOrder)
                //{
                //    C242_WTS C242_WTS = new C242_WTS();
                //    C242_WTS.StartTime = startTime;
                //    C242_WTS.FinishTime = finishTime;
                //    C242_WTS.Time = timePerOrder;
                //    C242_WTS.WorkID = "AA61";
                //    C242_WTS.StaffID = staff;
                //    C242_WTS.MachineID = "Anod-Sys";
                //    C242_WTS.Shift = shift;
                //    C242_WTS.Date = date;
                //    C242_WTS.Note2 = date.ToString("yyyyMMdd") + item.MeMa.ToString();
                //    C242_WTS.MONo = item.MONo;
                //    C242_WTS.Note1 = item.PartNo;
                //    C242_WTS.OptionID = item.OptionID2;
                //    C242_WTS.OKQty = 0;
                //    C242_WTS.NGQty = 0;
                //    //C242_WTS.SubNGQty = 0;
                //    //C242_WTS.SubOKQty = 0;
                //    C242_WTS.ProTime = 0;
                //    C242_WTS.ClampTime = 0;
                //    db.C242_WTS.Add(C242_WTS);
                //}
            }

            db.SaveChanges();
        }

        [HttpPost]
        public JsonResult GetPlanForCuttingMachineByPartNo(string shift, string machine, string partNo, string mono, string optionID)
        {
            try
            {
                if (string.IsNullOrEmpty(machine))
                {
                    machine = "FINISH";
                }

                //List<sp_242_GetCuttingMachinePlanning_ByDateAndMachineAndMONo_Result> listPlan1 = GetPlanByMONo(machine, mono).Where(x => x.Số_NC.ToLower() == optionID.ToLower()).ToList();
                return Json(null, JsonRequestBehavior.AllowGet);

                //var data = db.C242_MachinePlanning.Where(x => x.Số_Order.ToLower() == mono.ToLower() && x.Số_NC.ToLower() == optionID.ToLower()).ToList();
                //return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("NG", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetWorkID()
        {
            try
            {
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var data = db.C222_Work.Where(x => x.DeptCode == "mạ").OrderBy(x => x.WorkName).ToList();////TODO: dữ liệu mạ
                if (data.Count == 0)
                {
                    throw new ArgumentException("Không tìm thấy mã công việc");
                }

                return Json(new { status = "OK", value = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "NG", value = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult InputWTSForAnodStaff(string mono, string staffID, string part, string slg, string slg_OK, string shift, string optionID, string congdoan, string[] list)
        {
            try
            {
                var liststaff = staffID.Split(';').Where(x => x.Trim() != string.Empty).ToList();
                int qty;
                int ok;
                if (!int.TryParse(slg, out qty))
                {
                    throw new ArgumentException("Số lượng lệnh phải là kiểu số");
                }

                if (!int.TryParse(slg_OK, out ok))
                {
                    throw new ArgumentException("Số lượng ok phải là kiểu số");
                }

                NN_DatabaseEntities db = new NN_DatabaseEntities();
                switch (congdoan.ToLower())
                {
                    case "treo":
                        //// TODO: nhập WTS người cho mạ, hỏi lại nguyên công, mã công việc treo
                        foreach (var item in liststaff)
                        {
                            C242_WTS c242_WTS = CreateObjOfTreoForInput(mono, item, part, slg, ok, shift, optionID);
                            db.C242_WTS.Add(c242_WTS);
                        }
                        break;
                    case "hoanthien":
                        var start = Request.Cookies["Ma_DT"];
                        if (start == null)
                        {
                            throw new ArgumentException("Chưa bấm nút bắt đầu để tính thời gian");
                        }

                        DateTime startTime = DateTime.Parse(start["Time"].ToString());
                        var time = (DateTime.Now - startTime).TotalMinutes;
                        DateTime date = CalculateDate(shift);
                        ////TODO: hàng thay đổi thiết kế thì tìm theo tên chi tiết, nếu ko có => lấy số cuối cùng, trừ đi 1 => tìm lại => ko co thì TGTC tính = 1'
                        //// Kiểm tra chi tiết có đuôi mà nhập NG > 0 

                        double totalTime = 0;

                        int i1 = 0;

                        db.SaveChanges();
                        FinishToCalculateTime();
                        break;
                }

                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult History(string StaffID, string MachineID, string Shift)
        {
            if (StaffID.IndexOf(';') != -1)
            {
                StaffID = StaffID.Split(';')[0];
            }

            if (StaffID.Length > 0)
            {
                DateTime date = DateTime.Now.Date;
                if (Shift == "T2" || Shift == "T3")
                {
                    int time = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
                    if (time < 380)
                    {
                        date = date.AddDays(-1);
                    }
                }

                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var LisHistory = db.C242_WTS.Where(a => a.StaffID == StaffID && a.Shift.ToLower() == Shift.ToLower() && a.Date == date);
                return Json(LisHistory, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetMeMa(string mema)
        {
            try
            {
                DateTime date = DateTime.Now;
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                //var LisHistory = db.sp_242_DLMa_GetDetail(mema, date.Date).ToList();
                //if (LisHistory.Count == 0 && date.Hour <= 20)
                //{
                //    date = date.AddDays(-1);
                //    LisHistory = db.sp_242_DLMa_GetDetail(mema, date.Date).ToList();
                //}

                return Json(new { Status = "OK", Value = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Value = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private void RemoveNGQtyInPrevousTime(C242_WTS c242_WTS)
        {
            //var data1 = db.C242_WTS.Where(x => x.StaffID == "1556").ToList();
            //foreach(var item in data1)
            //{
            //    item.time = 1000;
            //}
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var data = db.C242_WTS.Where(x => x.MONo.ToLower() == c242_WTS.MONo.ToLower() && x.WorkID.ToUpper() == "AA71" && x.NGQty > 0).ToList();
            if (data.Count == 0 || c242_WTS.OKQty == 0 || c242_WTS.OKQty == null)
            {
                return;
            }

            int ng = (int)c242_WTS.OKQty;
            foreach (var item in data)
            {
                ng = ng - (int)item.NGQty;
                if (ng <= 0)
                {
                    item.NGQty = 0;
                    continue;
                }

            }

            if (ng > 0)
            {
                throw new ArgumentException("Số lượng mạ sửa lớn hơn số lượng lỗi đã mạ trước đây");
            }
        }

        private C242_WTS CalculateTimeOfWTSForHoanThien(C242_WTS c242_WTS)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            C242_OptionData c242_Opp = db.C242_OptionData.Where(x => x.PartID.ToLower() == c242_WTS.Note1.ToLower()).FirstOrDefault();
            if (c242_Opp == null)
            {
                throw new ArgumentException("Không tìm thấy thông tin của chi tiết trong Optiondata");
            }

            c242_WTS.Time = (decimal)(c242_WTS.OKQty * c242_Opp.TimeComplete);
            return c242_WTS;
        }

        private C242_WTS CreateObjOfTreoForInput(string mono, string staffID, string part, string slg, int ok, string shift, string optionID)
        {
            C242_WTS c242_WTS = new C242_WTS();
            c242_WTS.Date = CalculateDate(shift);

            c242_WTS.StaffID = staffID;
            c242_WTS.MONo = mono;
            c242_WTS.Shift = shift;
            //c242_WTS.SlgMoc = ok;
            c242_WTS.Note2 = slg;
            c242_WTS.Note1 = part;
            c242_WTS.OptionID = optionID;
            c242_WTS = CalculateTimeOfWTSForTreo(c242_WTS);

            return c242_WTS;
        }

        private DateTime CalculateDate(string shift)
        {
            DateTime date = DateTime.Now.Date;
            if ((shift == "T2" || shift == "T3") && (DateTime.Now.Minute + DateTime.Now.Hour * 60) <= 380)
            {
                date = date.AddDays(-1);
            }

            return date;
        }

        private C242_WTS CalculateTimeOfWTSForTreo(C242_WTS c242_WTS)
        {
            c242_WTS.WorkID = "AA51";
            c242_WTS.OKQty = 0;
            c242_WTS.NGQty = 0;
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            C242_OptionData c242_Opp = db.C242_OptionData.Where(x => x.PartID.ToLower() == c242_WTS.Note1.ToLower()).FirstOrDefault();
            if (c242_Opp == null)
            {
                throw new ArgumentException("Không tìm thấy thông tin của chi tiết trong Optiondata");
            }

            //c242_WTS.Time = (decimal)(c242_WTS.SlgMoc * c242_Opp.TimeTreo); /// TODO: tính toán số lượng móc
            return c242_WTS;
        }

        private double CalculateTime(DateTime StartTime, DateTime FinishTime, int number)
        {
            if (number <= 0)
            {
                throw new ArgumentException("Không có công việc hiện tại!");
            }

            int Time = 0;
            int startM = 0;
            int FinishM = 0;
            if (StartTime.Second < 30)
            {
                startM = StartTime.Minute;
            }
            else
            {
                startM = StartTime.Minute + 1;
            }

            if (FinishTime.Second < 30)
            {
                FinishM = FinishTime.Minute;
            }
            else
            {
                FinishM = FinishTime.Minute + 1;
            }

            if (StartTime.Day == FinishTime.Day)
            {
                Time = (FinishTime.Hour * 60 + FinishM) - (StartTime.Hour * 60 + startM);
            }
            else
            {
                Time = ((FinishTime.Hour + 24) * 60 + FinishM) - (StartTime.Hour * 60 + startM);
            }

            return Time / number;
        }

        private void AddNewCookie(string result, string staffID, string workID)
        {
            staffID = staffID.Replace(';', '+');
            HttpCookie userInfo = new HttpCookie(staffID);
            userInfo["ID"] = staffID + "-" + workID;
            userInfo["List"] = result;
            userInfo["StartTime"] = DateTime.Now.ToString();
            userInfo.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(userInfo);
        }

        private void RemoveCookie(string staffID)
        {
            staffID = staffID.Replace(';', '+');
            HttpCookie userInfo = new HttpCookie(staffID);
            userInfo.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(userInfo);
        }

        private string InsertToDataBase(C242_WTS c242_CuaWTS, C242_MachinePlanning c242_Plan)
        {
            //if (!CheckTrackingDataOfOrder(c242_CuaWTS.MONo, "Anod",string.Empty,c242_CuaWTS.MachineID))
            //{
            //    throw new ArgumentException("Không có dữ liệu giao nhận. Yêu cầu nhập dữ liệu giao nhận trước khi nhập WTS");
            //}

            DateTime date = DateTime.Now;
            if ((c242_CuaWTS.Shift == "T2" || c242_CuaWTS.Shift == "T3") && (date.Hour * 60 + date.Minute) <= 620)
            {
                date = date.AddDays(-1);
            }

            //c242_CuaWTS.PlanOption = c242_Plan.TTNC;
            date = date.Date;
            c242_CuaWTS.Date = date;
            c242_CuaWTS.Note1 = c242_Plan.TenChiTiet;
            c242_CuaWTS.StartTime = DateTime.Now;
            var deviceName = Request.UserHostAddress;
            c242_CuaWTS.Note2 = deviceName;
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            db.C242_WTS.Add(c242_CuaWTS);
            db.SaveChanges();
            return c242_CuaWTS.ID + "-" + c242_Plan.ID + "-" + c242_Plan.K;
        }
    }

    public class MONoNGValue
    {
        public string MONo { get; set; }
        public int NG { get; set; }
        public int SP { get; set; }
    }
}