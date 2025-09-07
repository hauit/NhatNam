using Newtonsoft.Json;
using NNworking.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NNworking.Controllers
{
    [RoutePrefix("WTS")]
    public class WTSController : BaseController
    {
        [Route("WTS-nhan-vien-truc-tiep.html")]
        // GET: WTS
        public ActionResult Index()
        {
            return View("WTSTT");
        }

        [Route("WTS-nhan-vien-gian-tiep.html")]
        public ActionResult WTSGT()
        {
            return View("WTSGT");
        }

        [Route("Duyet-WTS-nhan-vien.html")]
        public ActionResult CheckWTS()
        {
            return View("CheckWTS");
        }

        [Route("hau-CheckAndInputToPlanning.html")]
        public ActionResult HauCheckAndInputToPlanning()
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            DateTime dateTime = DateTime.Now.Date;
            var list = db.View_242_WTS.Where(x=>x.Date == dateTime && !x.OptionID.ToUpper().StartsWith("A") && !x.OptionID.ToUpper().StartsWith("G")).ToList();
            foreach(var item in list)
            {
                sp_242_WTS_GetAll_Result data = new sp_242_WTS_GetAll_Result();
                data.ID = item.ID;
                data.Date = item.Date;
                data.MONo = item.MONo;
                data.Shift = item.Shift;
                data.StaffID = item.StaffID;
                data.PartID = item.PartID;
                data.OptionID = item.OptionID;
                CheckAndInputToPlanning(data, dateTime);
            }
            return View("WTSTT");
        }
        //[Route("Chi-tiet-WTS-nhan-vien-{date}-{staffID}-{shift?}.html")]
        public ActionResult DetailWTS(string staffID, string shift, DateTime date)
        {
            ViewBag.Date = date.ToString("yyyy-MM-dd");
            ViewBag.StaffID = staffID;
            ViewBag.Shift = shift;
            return View("DetailWTS");
        }

        [HttpPost]
        public JsonResult InputWTSGT(string workID, string shift,bool ovt)
        {
            try
            {
                string user = Session["user"].ToString();
                string ID = string.Empty;
                if (Request.Cookies[user] != null)
                {
                    var i = Request.Cookies[user]["ID"].ToString().Trim().Split('-');
                    ID = i[0];
                }

                var updateWTS = workID.ToUpper() == "END" || !string.IsNullOrEmpty(ID);
                if (updateWTS)
                {
                    var update = UpdateWTS(ID);
                    RemoveCookie();
                    if (workID.ToUpper() == "END")
                    {
                        return Json(new { Status = "OK", Values = string.Empty }, JsonRequestBehavior.AllowGet);
                    }
                }

                ID = InputWTSToDatabase(workID, shift, ovt);
                AddnewCookie(ID, workID);
                return Json(new { Status = "OK", Values = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetHistory()
        {
            DateTime date = DateTime.Now.Date;
            if (((DateTime.Now.Hour * 60) + DateTime.Now.Minute) <= 380)
            {
                date = date.AddDays(-1);
            }
            string StaffID = Session["StaffID"].ToString();

            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var data = db.View_242_WTS.Where(x => x.StaffID == StaffID && x.Date == date).ToList();
            if (data.Count > 0)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        private bool UpdateWTS(string iD)
        {
            try
            {
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var dataValue = db.C242_WTS.Find(int.Parse(iD));
                dataValue.FinishTime = DateTime.Now;
                dataValue.Time = (decimal)((DateTime)dataValue.FinishTime - (DateTime)dataValue.StartTime).TotalMinutes;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string InputWTSToDatabase(string workID, string shift, bool ovt)
        {
            if (string.IsNullOrEmpty(shift))
            {
                shift = "T0";
            }

            DateTime date = DateTime.Now.Date;
            bool nightShift = (shift == "T2" || shift == "T3") && ((date.Hour * 60 + date.Minute) <= 420);
            if (nightShift)
            {
                date = date.AddDays(-1);
            }

            C242_WTS wts = new C242_WTS();
            wts.OVT = ovt;
            wts.WorkID = workID;
            wts.StaffID = Session["StaffID"].ToString();
            wts.Shift = shift;
            wts.Date = date;
            wts.StartTime = DateTime.Now;
            wts.MONo = string.Empty;
            wts.IDPlan = -1;
            wts.OptionID = "GT001";
            wts.Status = false;
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            db.C242_WTS.Add(wts);
            db.SaveChanges();
            return wts.ID.ToString();
        }

        private void AddnewCookie(string ID, string WorkID)
        {
            try
            {
                HttpCookie userInfo = new HttpCookie(Session["user"].ToString());
                userInfo["ID"] = ID + "-" + WorkID;
                userInfo.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(userInfo);
            }
            catch { }
        }

        private void RemoveCookie()
        {
            try
            {
                HttpCookie userInfo = new HttpCookie(Session["user"].ToString());
                userInfo["ID"] = string.Empty;
                userInfo.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(userInfo);
            }
            catch { }
        }

        [HttpPost]
        public JsonResult InputWTS(sp_242_WTS_GetAll_Result data, string dataNew)
        {
            try
            {
                if (data.ID == 0)
                {
                    InputWTSToDatabase(data, dataNew);
                    return Json(new { Status = "OK", Values = string.Empty }, JsonRequestBehavior.AllowGet);
                }

                UpdateWTS(data, dataNew);
                return Json(new { Status = "OK", Values = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult EdittWTS(sp_242_WTS_GetAll_Result data, string dataNew)
        {
            try
            {
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var obj = db.C242_WTS.Where(x => x.ID == data.ID).FirstOrDefault();
                if (obj == null)
                {
                    throw new ArgumentException("Không tìm thấy WTS");
                }

                if((bool)obj.Status)
                {
                    throw new ArgumentException("WTS đã được duyệt, không thể sửa. Nếu có thay đổi vui lòng xóa đi nhập lại để quản lý kiểm soát được");
                }

                obj = ValidateValue(obj, dataNew);
                db.SaveChanges();
                return Json(new { Status = "OK", Values = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult InputMachineWTSToDatabase(sp_242_WTS_GetAll_Result data, string date)
        {
            try
            {
                if(string.IsNullOrEmpty(data.Shift))
                {
                    throw new ArgumentException("Chưa chọn ca làm việc, vui lòng xem lại");
                }

                if (string.IsNullOrEmpty(data.MachineID))
                {
                    throw new ArgumentException("Chưa chọn máy, vui lòng xem lại");
                }

                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var workType = db.C242_Work.Where(x => x.WorkID.ToUpper() == data.WorkID.ToUpper()).FirstOrDefault();
                if (workType == null)
                {
                    throw new ArgumentException("Không tìm thấy thông tin mã công việc");
                }

                if (string.IsNullOrEmpty(data.MONo))
                {
                    data.MONo = DateTime.Now.ToString("GyyyyMM");
                    data.OptionID = "ASTG01";
                    data.MachineID = "KOCO";
                    data.OKQty = 0;
                    data.NGQty = 0;
                }

                //if (!workType.DirectWork)
                //{
                    //data.OKQty = 0;
                    //data.NGQty = 0;
                //}

                DateTime dateTime;
                if (!DateTime.TryParse(date, out dateTime))
                {
                    dateTime = DateTime.ParseExact(date.Substring(0, 24),
                                  "ddd MMM d yyyy HH:mm:ss",
                                  System.Globalization.CultureInfo.InvariantCulture).Date;
                }

                CheckAndInputToPlanning(data, dateTime);

                data.Date = dateTime.Date;
                InputWTSToDatabase(data, string.Empty);
                return Json(new { Status = "OK", Values = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private void CheckAndInputToPlanning(sp_242_WTS_GetAll_Result data, DateTime dateTime)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var plan = db.C242_MachinePlanning.Where(x => x.Date == dateTime
                            && x.Order.ToUpper() == data.MONo
                            && x.MayGC == data.StaffID
                            && x.NC.ToUpper() == data.OptionID.ToUpper()
                            && x.Shift.ToUpper() == data.Shift.ToUpper()
                            && !x.NC.ToUpper().StartsWith("A")
                            && !x.Deleted).FirstOrDefault();
            if(plan != null)
            {
                return;
            }

            int slgLenh = 10000;
            var order = db.View_242_BusOder.Where(x => x.BOderNo.ToUpper() == data.MONo.ToUpper()).FirstOrDefault();
            if(order != null)
            {
                slgLenh = order.MOQty == null ? slgLenh : (int)order.MOQty;
            }

            plan = new C242_MachinePlanning();
            plan.Date = dateTime;
            plan.Order = data.MONo.ToUpper();
            plan.NC = data.OptionID.ToUpper();
            plan.TTNC = GetTTNC(data);
            plan.Shift = data.Shift;
            plan.MayGC = data.StaffID.ToUpper();
            plan.Slglenh = slgLenh;
            db.C242_MachinePlanning.Add(plan);
            db.SaveChanges();
        }

        private string GetTTNC(sp_242_WTS_GetAll_Result data)
        {
            string ttnc = data.OptionID.ToUpper();
            if (!data.OptionID.ToLower().StartsWith("a"))
            {
                return ttnc;
            }

            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var data1 = db.View_242_OptionData.Where(x => x.PartID.ToLower() == data.PartID.ToLower() && x.OptionID.ToLower().StartsWith("a")).ToList().Count();
            ttnc = ttnc.Replace("ASTG", string.Empty);
            int tt;
            if (!int.TryParse(ttnc, out tt))
            {
                return ttnc;
            }

            return $@"{tt}/{data1}";
        }

        [HttpPost]
        public JsonResult DeleteWTS(int id)
        {
            try
            {
                List<string> listStaff = new List<string>() { "0005","1556","0038","0087", "0002", "0007","0038","0164" };
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var model = db.C242_WTS.Find(id);
                model.Deleted = true;
                string staffID = Session["StaffID"].ToString().Trim();
                var specStaff = listStaff.Where(x => x.ToUpper() == staffID.ToUpper()).Any();
                if (!specStaff)
                {
                    //throw new ArgumentException("Bạn không có quyển xóa wts");
                }
                //if ((bool)model.Status && !specStaff)
                //{
                //    throw new ArgumentException("WTS đã được duyệt thì không thể xóa! vui lòng báo cáo quản lý.");
                //}
                //_context.C242_WTS.Remove(model);
                db.SaveChanges();
                return Json(new
                {
                    Status = "OK",
                    Values = "OK"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Status = "NG",
                    Values = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult OKWTS(int id)
        {
            try
            {

                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var model = db.C242_WTS.Find(id);
                model.Status = true;
                model.LeaderCheck = Session["StaffID"].ToString();
                model.CheckTime = DateTime.Now;
                //_context.C242_WTS.Remove(model);
                db.SaveChanges();
                return Json(new
                {
                    Status = "OK",
                    Values = "OK"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Status = "NG",
                    Values = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult OKWTSGT(List<sp_242_WTS_GetTotalWTSForChecking_Result> data)
        {
            try
            {
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                foreach (var item in data)
                {
                    GetAndSetOKWTS(item,db);
                }
                db.SaveChanges();
                return Json(new { Status = "OK", Values = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private void GetAndSetOKWTS(sp_242_WTS_GetTotalWTSForChecking_Result data, NN_DatabaseEntities db)
        {
            var model = db.C242_WTS.Where(x=>x.StaffID == data.StaffID && x.Date == data.Date && x.Time != null && x.Time != 0).ToList();
            foreach(var item in model)
            {
                item.Status = true;
                item.LeaderCheck = Session["StaffID"].ToString();
                item.CheckTime = DateTime.Now;
            }
        }

        [HttpPost]
        public JsonResult GetListMachine(string date,string department)
        {
            try
            {
                var dateTime = DateTime.ParseExact(date.Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture);
                if(dateTime.Hour < 6)
                {
                    dateTime = dateTime.AddDays(-1);
                }

                dateTime = dateTime.Date;
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var data1 = db.sp_GetMachineHavePlan(dateTime, department).ToList();
                return Json(new { Status = "OK", Values = data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetMachinePrepare(string date,string department)
        {
            try
            {
                var dateTime = DateTime.ParseExact(date.Substring(0, 24),
                              "ddd MMM d yyyy HH:mm:ss",
                              System.Globalization.CultureInfo.InvariantCulture);
                if(dateTime.Hour < 6)
                {
                    dateTime = dateTime.AddDays(-1);
                }

                dateTime = dateTime.Date;
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var data1 = db.sp_242_MachinePlan_Preparation(dateTime).ToList();
                return Json(new { Status = "OK", Values = data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetListDepartment()
        {
            try
            {
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var data = db.C222_Department.ToList();
                return Json(new { Status = "OK", Values = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private void InputWTSToDatabase(sp_242_WTS_GetAll_Result data, string dataNew)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var obj = new C242_WTS();
            bool noError = true;
            try
            {
                obj.Deleted = false;
                obj.Date = data.Date;
                if (data.Date == null)
                {
                    obj.Date = GetDateFromPlan((int)data.IDPlan);
                }

                var user = Session["StaffID"].ToString();
                var kh = db.C222_Staff.Where(x => x.StaffID == user && x.SecID == "1010").Any();
                var kcs = db.C222_Staff.Where(x => x.StaffID == data.StaffID && x.SecID == "1014").Any();
                if (!kh && kcs)
                {
                    var wtsDate = DateTime.Now.Date;
                    if (DateTime.Now.Hour < 6)
                    {
                        wtsDate = wtsDate.AddDays(-1);
                    }

                    if (obj.Date != wtsDate)
                    {
                        throw new ArgumentException($@"Nhân viên không được nhập WTS khác ngày");
                    }
                }

                obj.WorkWhilePLJ1 = data.WorkWhilePLJ1;
                obj.IDPlan = (int)data.IDPlan;
                obj.MachineID = data.MachineID;
                obj.ProTime = data.ProTime;
                obj.ClampTime = data.ClampTime;
                obj.MONo = data.MONo;
                obj.NGQty = data.NGQty == null ? 0 : data.NGQty;
                obj.PartID = data.PartID;
                if (string.IsNullOrEmpty(obj.PartID) && !obj.MONo.ToUpper().StartsWith("G"))
                {
                    GetPartIdFromOrder(obj);
                }
                obj.OKQty = data.OKQty == null ? 0 : data.OKQty;
                obj.NGNCTruoc = data.NGNCTruoc == null ? 0 : data.NGNCTruoc;
                obj.OptionID = data.OptionID;
                obj.Shift = data.Shift;
                obj.StaffID = data.StaffID;
                obj.Time = data.Time;
                obj.WorkID = data.WorkID;
                obj.Note = data.Note;
                obj.StartTime = DateTime.Now;
                obj.FinishTime = DateTime.Now;
                if (!string.IsNullOrEmpty(dataNew))
                {
                    obj = ValidateValue(obj, dataNew);
                }

                CheckAndInputStandTime(obj, db);
                if (obj.OKQty + obj.NGQty > 0)
                {
                    NhapQuaSoLuongLenh(obj, ref noError);
                    //NhapQuaSoLuongOKCuaNCTruoc(obj,ref noError);
                }

            }
            catch(Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            finally
            {
                if (noError)
                {
                    db.C242_WTS.Add(obj);
                    db.SaveChanges();
                }
                //if (obj.OptionID.StartsWith("A"))
                //{
                //    db.C242_WTS.Add(obj);
                //    db.SaveChanges();
                //}
                //else
                //{
                //    if (noError)
                //    {
                //        db.C242_WTS.Add(obj);
                //        db.SaveChanges();
                //    }
                //}
            }
        }

        private void GetPartIdFromOrder(C242_WTS obj)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var qty = db.View_242_BusOder.Where(x => x.BOderNo.ToLower() == obj.MONo.ToLower()).FirstOrDefault(); // 
            if(qty == null)
            {
                throw new ArgumentException("Không tìm thấy thông tin order trong dữ liệu. Vui lòng nhập đầy đủ cả tên chi tiết!");
            }

            obj.PartID = qty.PartID;
        }

        private void CheckAndInputStandTime(C242_WTS obj, NN_DatabaseEntities db)
        {
            var workType = db.C242_Work.Where(x => x.WorkID.ToUpper() == obj.WorkID.ToUpper()).FirstOrDefault();

            if(workType == null)
            {
                return;
            }

            if (workType.MassWork == false)
            {
                return;
            }

            string may = obj.MachineID.ToLower();
            if (!obj.OptionID.ToLower().StartsWith("a"))
            {
                may = obj.StaffID.ToLower();
            }

            var standTime = db.C242_StandTime.Where(x => x.PartNo.ToLower() == obj.PartID.ToLower() && x.OptionID.ToLower() == obj.OptionID.ToLower() && x.MachineID.ToLower() == may).FirstOrDefault();
            if(standTime == null)
            {
                standTime = new C242_StandTime();
                standTime.Date = DateTime.Now.Date;
                standTime.MachineID = may;
                standTime.OptionID = obj.OptionID;
                standTime.PartNo = obj.PartID;
                standTime.ProTime = obj.ProTime;
                standTime.ClampTime = obj.ClampTime;
                standTime.PJ61_PKN1 = 0;
                standTime.PBJ1 = 0;
                standTime.PH71 = 0;
                standTime.PU71 = 0;
                standTime.PZE1 = 0;
                standTime.Status = true;
                standTime.NumberOption = 0;
                standTime.Note2 = string.Empty;
                standTime.Note = string.Empty;
                standTime.MachineGroup = string.Empty;
                db.C242_StandTime.Add(standTime);
                return;
            }

            if(obj.ProTime < standTime.ProTime )
                standTime.ProTime = obj.ProTime;
            if (obj.ClampTime < standTime.ClampTime)
                standTime.ClampTime = obj.ClampTime;
        }

        private void NhapQuaSoLuongLenh(C242_WTS obj, ref bool noError)
        {
            int currentQty = 0;
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var qty = db.View_242_BusOder.Where(x => x.BOderNo.ToLower() == obj.MONo.ToLower()).FirstOrDefault(); // 
            if(qty == null)
            {
                return;
            }
            
            var data = db.sp_WTS_CompareQtyWithMOQty(obj.OptionID, obj.PartID, obj.MONo).FirstOrDefault();
            if (data != null)
            {
                currentQty = (int)data.CurrentQty;
            }

            var quaSlg = qty.Qty < (currentQty + obj.OKQty + obj.NGQty + obj.NGNCTruoc);
            if (quaSlg)
            {
                //if (obj.OptionID.StartsWith("A"))
                //{
                //    return;
                //}
                noError = false;
                throw new ArgumentException($@"Số lượng đã nhập của nguyên công này là {currentQty}. Số lượng NG của NC trước: {obj.NGNCTruoc}. Số lượng OK sẽ nhập: {obj.OKQty}, số lượng NG sẽ nhập: {obj.NGQty}. Số lượng lệnh chỉ có: {qty.Qty}");
            }
        }

        private void NhapQuaSoLuongOKCuaNCTruoc(C242_WTS obj,ref bool noError)
        {
            int currentNC;
            string nc = GetLastNC(obj);
            if (nc.ToLower() == obj.OptionID.ToLower())
            {
                return;
            }
            
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            //string NCName = obj.OptionID.Substring(0, obj.OptionID.Length - 2);
            int lastQty = 0;
            int currentQty = 0;
            var data = db.sp_WTS_CompareQtyWithLastOption_Old(obj.OptionID, nc, obj.PartID, obj.MONo).FirstOrDefault();
            if (data != null)
            {
                lastQty = data.LastQty;
                currentQty = (int)data.CurrentQty;
            }

            var quaSlg = lastQty < (currentQty + obj.OKQty);
            if (quaSlg)
            {
                if (obj.OptionID.StartsWith("A"))
                {
                    return;
                }

                noError = false;
                throw new ArgumentException($@"Số lượng ok của nguyên công trước là {lastQty}. Số lượng ok đã nhập của NC này là {currentQty}, số lượng ok nhập tiếp là {obj.OKQty}. Không thể nhập quá số lượng ok của NC trước.");
            }

            //NN_DatabaseEntities db = new NN_DatabaseEntities();
            //int lastQty = 0;
            //int currentQty = 0;
            //var data = db.sp_WTS_CompareQtyWithLastOption(obj.OptionID, "ASTG01", obj.PartID, obj.MONo).ToList();
            //var currentInfor = data.Where(x => x.OptionID.ToLower() == obj.OptionID.ToLower()).FirstOrDefault();
            //if (currentInfor == null)
            //{
            //    throw new ArgumentException($@"Nguyên công {obj.OptionID} của chi tiết {obj.PartID} chưa được đăng kí trong OptionData. Vui lòng đăng kí trước khi nhập WTS.");
            //}

            //if(currentInfor.STT == 1)
            //{
            //    return;
            //}

            //currentQty = currentInfor.OKQty;
            //lastQty = data.Where(x => x.STT == (currentInfor.STT - 1)).FirstOrDefault().OKQty;
            //var quaSlg = lastQty < (currentQty + obj.OKQty);
            //if(quaSlg)
            //{
            //    throw new ArgumentException($@"Số lượng ok của nguyên công trước là {lastQty}. Số lượng ok đã nhập của NC này là {currentQty}, số lượng ok nhập tiếp là {obj.OKQty}. Không thể nhập quá số lượng ok của NC trước.");
            //}
        }

        private string GetLastNC(C242_WTS obj)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var data = db.View_242_OptionData.Where(x=>x.PartID == obj.PartID).OrderBy(x=>x.OptionID).ToList();
            for(int i = 0; i < data.Count; i++)
            {
                if(data[i].OptionID.ToLower() == obj.OptionID.ToLower())
                {
                    if (i == 0)
                    {
                        return obj.OptionID; 
                    }

                    return data[i - 1].OptionID;
                }
            }

            return obj.OptionID;
        }

        private DateTime GetDateFromPlan(int iDPlan)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var plann = db.C242_MachinePlanning.Where(x => x.ID == iDPlan).FirstOrDefault();
            if (plann == null)
            {
                throw new ArgumentException("Không tìm thấy thông tin kế hoạch");
            }

            return (DateTime)plann.Date;
        }

        private void UpdateWTS(sp_242_WTS_GetAll_Result data, string dataNew)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var dataValue = db.C242_WTS.Find(data.ID);
            dataValue = ValidateValue(dataValue, dataNew);
            db.SaveChanges();
        }

        public JsonResult List_WorkIDGT(string dept)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var Work = db.C242_Work.Where(x => x.DeptCode.ToLower().IndexOf(dept) != -1).OrderBy(x => x.SortingNumber).ToList();
            return Json(Work, JsonRequestBehavior.AllowGet);
        }

        public JsonResult List_WorkID(string dept)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            List<string> Line1 = new List<string>() { "PJ61", "PZE1", "PBJ1", "PU71", "PH71", "PLJ1" };
            var bp = db.C222_Department.Where(x => x.DeptCode.ToLower() == dept.ToLower()).FirstOrDefault();
            dept = bp == null ? string.Empty : bp.DeptCode;
            var Work = db.C242_Work.Where(x => !string.IsNullOrEmpty(x.DeptCode) && x.DeptCode.ToLower().IndexOf(dept.ToLower()) != -1).OrderBy(x => x.SortingNumber).ToList();
            return Json(Work, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLT(string mono,string optionID)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var data = db.sp_242_WTS_GetLT(mono,optionID).FirstOrDefault();
            if(data == null)
            {
                data = new sp_242_WTS_GetLT_Result();
                data.QtyLT = 0;
                data.OKQtyLT = 0;
                data.NGQtyLT = 0;
            }

            return Json(new { Values = data,Status="OK!"}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListOptionID(string dept,string partID)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var data = db.View_242_Option.Where(x => x.DeptCode.ToLower() == dept).ToList();
            if (data.Count > 0)
            {
                string optionid = data[0].OptionID.Substring(0,1);
                if (!optionid.ToUpper().StartsWith("A"))
                {
                    var data1 = db.View_242_OptionData.Where(x => x.PartID.ToUpper() == partID.ToUpper() && x.OptionID.ToUpper().StartsWith(optionid));
                    return Json(new { Values = data1, Status = "OK!" }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { Values = data,Status="OK!"}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetShiftList()
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var data = db.C222_Shift.ToList();
            return Json(new { Values = data, Status = "OK!" }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetPartIDByMONo(string mono)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var data = db.C242_BusOder.Where(x => x.MONo.ToLower() == mono && !x.Deleted).FirstOrDefault();
            string partID = string.Empty;
            if(data != null)
            {
                partID = data.PartID;
            }

            return Json(new { Values = partID, Status = "OK!" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOptionIdData(string mono, string optionID)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var data = db.sp_242_Optiondata_GetForWTS(mono, optionID).ToList();
            if (data.Count == 0)
            {
                return Json(new { Values = string.Empty, Status = "NG!" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Values = data, Status = "OK!" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult List_MachineID(string dept)
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var Work = db.C222_Machine.Where(x => x.MachineGroup.ToLower() == dept.ToLower()).ToList();
            return Json(Work, JsonRequestBehavior.AllowGet);
        }

        private C242_WTS ValidateValue(C242_WTS model, string dataNew)
        {
            var values = JsonConvert.DeserializeObject<IDictionary>(dataNew);
            string ID = nameof(C242_WTS.ID);
            string IDPLAN = nameof(C242_WTS.IDPlan);
            string DATE = nameof(C242_WTS.Date);
            string STAFF_ID = nameof(C242_WTS.StaffID);
            string SHIFT = nameof(C242_WTS.Shift);
            string MACHINE_ID = nameof(C242_WTS.MachineID);
            string OPTION_ID = nameof(C242_WTS.OptionID);
            string MONO = nameof(C242_WTS.MONo);
            string WORK_ID = nameof(C242_WTS.WorkID);
            string TIME = nameof(C242_WTS.Time);
            string OKQTY = nameof(C242_WTS.OKQty);
            string NGQTY = nameof(C242_WTS.NGQty);
            string NOTE = nameof(C242_WTS.Note);
            string DELETED = nameof(C242_WTS.Deleted);

            if (values.Contains(ID))
            {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if (values.Contains(IDPLAN))
            {
                model.IDPlan = Convert.ToInt32(values[IDPLAN]);
            }

            if (values.Contains(DATE))
            {
                model.Date = Convert.ToDateTime(values[DATE]);
            }

            if (values.Contains(STAFF_ID))
            {
                model.StaffID = Convert.ToString(values[STAFF_ID]);
            }

            if (values.Contains(SHIFT))
            {
                model.Shift = Convert.ToString(values[SHIFT]);
            }

            if (values.Contains(MACHINE_ID))
            {
                model.MachineID = Convert.ToString(values[MACHINE_ID]);
            }

            if (values.Contains(OPTION_ID))
            {
                model.OptionID = Convert.ToString(values[OPTION_ID]);
            }

            if (values.Contains(MONO))
            {
                model.MONo = Convert.ToString(values[MONO]);
            }

            if (values.Contains(WORK_ID))
            {
                model.WorkID = Convert.ToString(values[WORK_ID]);
            }

            if (values.Contains(TIME))
            {
                model.Time = Convert.ToDecimal(values[TIME]);
            }

            if (values.Contains(OKQTY))
            {
                model.OKQty = Convert.ToInt32(values[OKQTY]);
            }

            if (values.Contains(NGQTY))
            {
                model.NGQty = Convert.ToInt32(values[NGQTY]);
            }

            if (values.Contains(NOTE))
            {
                model.Note = Convert.ToString(values[NOTE]);
            }

            if (values.Contains(DELETED))
            {
                model.Deleted = Convert.ToBoolean(values[DELETED]);
            }
            return model;
        }
    }
}