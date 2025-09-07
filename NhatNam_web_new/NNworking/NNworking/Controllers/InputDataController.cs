using NNworking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NNworking.Controllers
{
    [RoutePrefix("Nhap-du-lieu")]
    public class InputDataController : BaseController
    {
        // GET: InputData
        [Route("Danh-sach-nhan-vien.html")]
        public ActionResult StaffList()
        {
            //CheckPermissAndRedirect();
            return View("~/Views/InputData/StaffListTest.cshtml");
        }

        [Route("Danh-sach-tai-khoan.html")]
        public ActionResult UserList()
        {
            //CheckPermissAndRedirect();
            return View();
        }

        [HttpGet]
        public JsonResult GetStaffIDList()
        {
            try
            {
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var data = db.View_222_Staff.Select(x => x.StaffID ).ToList();
                return Json(new { Status = "OK", Values = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ResetPass(string staffID)
        {
            try
            {
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var activeUser = db.View_222_Users.Where(x => x.StaffID == staffID).Any();
                if(!activeUser)
                {
                    throw new ArgumentException("Không tìm thấy mã nhân viên");
                }

                var data = db.C222_Users.Where(x => x.StaffID == staffID).FirstOrDefault();
                if (data == null)
                {
                    throw new ArgumentException("Nhân viên này chưa có user");
                }

                data.Password = "stdToiA7XxI5n1fQY3YEGA==";
                db.SaveChanges();
                return Json(new { Status = "OK", Values = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CreateUser(string staffID)
        {
            try
            {
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var activeUser = db.View_222_Users.Where(x => x.StaffID == staffID).Any();
                if (activeUser)
                {
                    throw new ArgumentException("Nhân viên đã được tạo tài khoản");
                }

                var data = db.View_222_Staff.Where(x => x.StaffID == staffID).FirstOrDefault();
                if (data == null)
                {
                    throw new ArgumentException("Không tìm thấy mã nhân viên");
                }

                var user = new C222_Users();
                user.UserName = CreateUserName(data);
                user.StaffID = data.StaffID;
                user.Password = "stdToiA7XxI5n1fQY3YEGA==";
                user.UserGroupID = 0;
                user.DepartmentID = 0;
                db.C222_Users.Add(user);
                db.SaveChanges();
                return Json(new { Status = "OK", Values = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private string CreateUserName(View_222_Staff data)
        {
            string userName = data.StaffID;
            var i = data.StaffName.Trim().Split(' ');
            var name = i[i.Length - 1];
            userName = name + userName;
            return data.StaffID;
        }
    }
}