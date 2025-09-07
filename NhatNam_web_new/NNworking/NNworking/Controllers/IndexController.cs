using NNworking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NNworking.Controllers
{
    [RoutePrefix("Trang-chu")]
    public class IndexController : BaseController
    {
        private NN_DatabaseEntities db = new NN_DatabaseEntities();

        // GET: Dashboard
        [Route("Index.html")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("Thong-Ke-them-gio.html")]
        public ActionResult WTSChart()
        {
            CheckPermissAndRedirect();
            return View("~/Views/Index/SubItem/WTSChart.cshtml");
        }

        [Route("Ti-le-thoi-gian-gia-cong-hang-loat.html")]
        public ActionResult WTSPLJ1Chart()
        {
            CheckPermissAndRedirect();
            return View("~/Views/Index/SubItem/WTSPLJ1Chart.cshtml");
        }

        [Route("Thong-bao/{caption}-{content?}.html")]
        public ActionResult MessageBox(string caption, string content, string fromClient)
        {
            CheckPermissAndRedirect();
            ViewBag.Caption = caption;
            ViewBag.Content = content;
            ViewBag.FromClient = fromClient;
            return View("~/Views/Index/SubItem/MessageBox.cshtml");
        }

        [HttpPost]
        public JsonResult GetReportList()
        {
            try
            {
                string group = Session["Group"].ToString().Trim().ToUpper();
                string secID = Session["SecID"].ToString().Trim().ToUpper();
                //var list = db.C222_ReportList.Where(x => 
                //    ((x.SubGroup.ToUpper() == group || x.SubGroup.ToUpper() == "ALL") && x.SecID == secID)
                //    || x.SecID.ToUpper() == "ALL"
                //).ToList();

                //for(int i = 0; i < list.Count; i++)
                //{
                //    list[i].Alias = ConvertFromAccentCharToUnsignedChar(list[i].Caption) + $"-{list[i].ID}";
                //}

                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("NG", JsonRequestBehavior.AllowGet);
            }
        }

        private string ConvertFromAccentCharToUnsignedChar( string sourceString)
        {
            sourceString = sourceString.ToLower();
            Dictionary<string, string> listData = new Dictionary<string, string>(){
                { "a", "áàạảãâấầậẩẫăắằặẳẵ" },
                { "e", "éèẹẻẽêếềệểễ" },
                { "o", "óòọỏõôốồộổỗơớờợởỡ" },
                { "u", "úùụủũưứừựửữ" },
                { "i", "íìịỉĩ" },
                { "d", "đ" },
                { "y", "ýỳỵỷỹ" }
            };
            
            foreach(var item in sourceString)
            {
                var needToConvert = listData.Where(x => x.Value.IndexOf(item) != -1).FirstOrDefault();//.ToDictionary(x=>x.Key,x=>x.Value);
                if(needToConvert.Key == null)
                {
                    continue;
                }

                string key = needToConvert.Key;
                sourceString = sourceString.Replace(item.ToString(), needToConvert.Key);
            }

            return sourceString.Replace(' ','-');
        }

        [HttpGet]
        public JsonResult GetWorkingTimeTotal(string fromDate, string toDate)
        {
            try
            {
                DateTime from;
                if(!DateTime.TryParse(fromDate,out from))
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

                //from = from.AddDays(-from.Day + 1).AddMonths(-1).Date;
                //to = to.AddDays(-to.Day).Date;
                var list = db.sp_242_WTS_RateTimeWorkInTotal(from, to).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("NG", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult sp_242_GetWorkingTimePLJ1Total(string fromDate, string toDate)
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
                
                var list = db.sp_242_GetWorkingTimePLJ1Total(from, to).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("NG", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult sp_242_WTS_RateTimeWorkInTotal_Machine(string fromDate, string toDate, string machineID)
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
                
                var list = db.sp_242_WTS_RateTimeWorkInTotal_Machine(from, to, machineID).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("NG", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult sp_222_GetWorkingTimeTotal_OVT_ByGroup(string fromDate, string toDate, string subGroup)
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

                from = from.AddDays(-from.Day + 1).AddMonths(-1).Date;
                to = to.AddDays(-to.Day).Date;
                //var list = db.sp_222_GetWorkingTimeTotal_OVT_ByGroup(from, to, subGroup).ToList();
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("NG", JsonRequestBehavior.AllowGet);
            }
        }

        [Route("Thong-ke-bao-cao/{reportID?}.html")]
        public ActionResult ReportDetail(string reportID)
        {
            //ViewBag.Data = GetReportDetailData(reportID);
            ViewBag.ReportID = reportID;
            return View("IndexReportDetail");
        }

        public JsonResult ReportID1DetailData1(string reportID,bool json)
        {
            try
            {
                //var data = GetReportDetailData(reportID);
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("NG", JsonRequestBehavior.AllowGet);
            }
        }

        //private dynamic GetReportDetailData(string reportID)
        //{
        //    var data = db.C222_ReportList.Where(x=>x.Alias == reportID).FirstOrDefault();
        //    if(data == null)
        //    {
        //        return null;
        //    }

        //    switch(data.ID)
        //    {
        //        case 1:
        //            return db.sp_GetAllOrderTranferToF3ButNotSetFinish().ToList();
        //        case 2:
        //            return db.sp_GetAllOrderWhichIncludedInDensanButNotInBusOrderList().ToList();
        //        case 3:
        //            return db.sp_242_WTS_GetAllOrderFinishedWTSButNotMakeTraking().ToList();
        //        case 4:
        //            return db.sp_242_ConfigureData_GetWeightDoesNotRounding().ToList();
        //    }

        //    return null;
        //}
    }

    internal class IndexWorkFollowNote
    {
        public string Caption { get; set; }
        public string Content { get; set; }
    }
}