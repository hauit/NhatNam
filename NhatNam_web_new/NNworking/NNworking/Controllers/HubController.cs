using NNworking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NNworking.Controllers
{
    public class HubController : Controller
    {
        // GET: Hub
        public JsonResult ListAvailableUser(string groupMachine)
        {
            try
            {
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                string fromStaffID = Session["StaffID"].ToString().Trim();
                var data = db.View_222_Staff.Where(x => x.StaffID.ToLower() == fromStaffID.ToLower()).FirstOrDefault();
                if (data == null)
                {
                    return Json(new { Status = "NG", Values = "Không chấp nhận chế độ ẩn danh!" }, JsonRequestBehavior.AllowGet);
                }

                var returnData = db.View_222_Users.Take(0).ToList();
                returnData = db.View_222_Users.ToList();
                return Json(new { Status = "OK", Values = returnData }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}