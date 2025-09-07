using NNworking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NNworking.Controllers
{
    public class LoginController : Controller
    {
        NN_DatabaseEntities db = new NN_DatabaseEntities();
        //
        // GET: /Login/

        public ActionResult Index()
        {
            return View("Login");
        }

        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {
            try
            {
                clsBase check = new clsBase();
                string user = fc["taikhoan"];
                string pass = check.EncryptPassword(fc["matkhau"], "Ktd@");
                var a111 = db.View_222_Users.ToList();
                var a = (from c in db.View_222_Users where c.UserName == user && c.Password == pass select c).ToList();

                if (a.Count == 0)
                {
                    return View("Login");
                }
                else
                {
                    string StaffID = a[0].StaffID.ToString();
                    var b = db.View_222_Staff.Where(c => c.StaffID == StaffID).FirstOrDefault();
                    string b1 = b.SecID.ToString();
                    Session["SecID"] = b.SecID.ToString();
                    Session["StaffID"] = StaffID;

                    Session["BP"] = b.BoPhan != null ? b.BoPhan.ToString().Trim() : string.Empty;
                    Session["Group"] = b.Sub_Group.ToString();
                    Session["Level"] = (b.level == null ? 0 : b.level).ToString();
                    Session["GroupID"] = b.GroupID == null ? string.Empty : b.GroupID.ToString();
                    if (db.C222_Staff.Where(d => d.ngduyet == StaffID || d.ngduyet2 == StaffID || d.ngduyet3 == StaffID).ToList().Count() > 0)
                    {
                        Session["Level"] = db.C222_Staff.Where(d => d.StaffID == StaffID).ToList()[0].level.ToString();
                    }
                    else
                    {
                        Session["Level"] = 1.ToString();
                    }
                    Session["user"] = user;
                    Session["pass"] = pass;
                    Session["Department"] = string.IsNullOrEmpty(b.DeptCode) ? string.Empty : b.DeptCode;
                    Session.Timeout = 1000;

                    HttpCookie userInfo = new HttpCookie("Session");
                    userInfo["SecID"] = b.SecID.ToString();
                    userInfo["StaffID"] = StaffID;
                    userInfo["Department"] = b.DeptCode;
                    userInfo["Group"] = b.Sub_Group.ToString();
                    userInfo["Level"] = (b.level == null ? 0 : b.level).ToString();
                    userInfo["GroupID"] = b.GroupID == null ? string.Empty : b.GroupID.ToString();
                    userInfo["user"] = user;
                    //userInfo["pass"] = pass;
                    userInfo["BP"] = Session["BP"].ToString().Trim();
                    userInfo.Expires = DateTime.Now.AddHours(10);
                    Response.Cookies.Add(userInfo);
                    return RedirectToAction("Index", "Index");
                }
            }
            catch(Exception ex)
            {
                return View("Login");
            }

            
        }

        //
        // GET: /Login/

        public ActionResult Logout()
        {
            HttpCookie userInfo = new HttpCookie("Session");
            userInfo.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(userInfo);

            System.Web.HttpContext.Current.Session.Clear();
            return RedirectToAction("Index", "Login");
        }

        //
        // GET: /Change Pass/

        public ActionResult ChangePass()
        {
            return View("ChanPass");
        }

        //
        // POST: /Change Pass/
        [HttpPost]
        public JsonResult ChangePass(FormCollection fc)
        {
            string data = "";
            string OldPass = fc["OldPass"].ToString().Trim();
            OldPass = EncryptPassword(OldPass, "Ktd@");
            string NewPass = fc["NewPass"].ToString().Trim();
            string user = Session["user"].ToString().Trim();
            var check = db.C222_Users.Where(a => a.UserName == user).Where(a => a.Password == OldPass).ToList();
            if (check.Count > 0)
            {
                var c222_Users = db.C222_Users.Find(user);
                c222_Users.Password = EncryptPassword(NewPass, "Ktd@");
                //db.C222_Users.Add(c222_Users);
                db.Entry(c222_Users).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                data = "đã sửa xong";
            }
            else
            {
                data = "Mật khẩu hoặc tài khoản bị sai";
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #region hàm mã hóa mật khẩu dùng chung trong ERP
        public string EncryptPassword(string strEnCrypt, string key)
        {
            try
            {
                byte[] keyArr;
                byte[] EnCryptArr = Encoding.UTF8.GetBytes(strEnCrypt);
                MD5CryptoServiceProvider MD5Hash = new MD5CryptoServiceProvider();
                keyArr = MD5Hash.ComputeHash(Encoding.UTF8.GetBytes(key));
                TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider();
                tripDes.Key = keyArr;
                tripDes.Mode = CipherMode.ECB;
                tripDes.Padding = PaddingMode.PKCS7;
                ICryptoTransform transform = tripDes.CreateEncryptor();
                byte[] arrResult = transform.TransformFinalBlock(EnCryptArr, 0, EnCryptArr.Length);
                return Convert.ToBase64String(arrResult, 0, arrResult.Length);
            }
            catch (Exception ex) { }
            return "";
        }
        #endregion
    }
}
