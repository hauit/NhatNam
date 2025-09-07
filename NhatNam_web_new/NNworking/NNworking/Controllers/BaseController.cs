using NNworking.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NNworking.Controllers
{
    public class BaseController : Controller
    {
        clsBase check = new clsBase();
        NN_DatabaseEntities db = new NN_DatabaseEntities();

        /// <summary>
        /// Thu muc file ban ve
        /// </summary>
        protected string RoodPathDraw
        {
            get
            {
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var data = db.C222_RootPath.Where(x => x.Code.ToUpper() == "DRAW").FirstOrDefault();
                if (data == null)
                {
                    return string.Empty;
                }

                return data.RootPath;
            }
        }

        /// <summary>
        /// Thu muc file huong dan gia cong
        /// </summary>
        protected string RoodPathHDGC
        {
            get
            {
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var data = db.C222_RootPath.Where(x => x.Code.ToUpper() == "HDGC").FirstOrDefault();
                if (data == null)
                {
                    return string.Empty;
                }

                return data.RootPath;
            }
        }

        /// <summary>
        /// Thu muc file chuong trinh gia cong
        /// </summary>
        protected string RoodPathCTGC
        {
            get
            {
                NN_DatabaseEntities db = new NN_DatabaseEntities();
                var data = db.C222_RootPath.Where(x => x.Code.ToUpper() == "CT").FirstOrDefault();
                if (data == null)
                {
                    return string.Empty;
                }

                return data.RootPath;
            }
        }

        public BaseController()
        {
            check.ReCreateSession();
            //Response.Redirect(Url.Action("Index", "Index"));
        }

        protected void CheckPermissAndRedirect()
        {
            CheckAndUpdateDAtabaseUrl();

            if (!CheckWebPermiss())
            {
                System.Web.HttpContext.Current.Response.Redirect(Url.Action("Index", "Index"));
            }
        }
        /// <summary>
        /// Thu muc file chuong trinh gia cong
        /// </summary>
        protected void StockerMenu()
        {
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var data = db.View_222_StokerMenu.OrderBy(x=>x.ParentID).OrderBy(x => x.CatergoryID).OrderBy(x=>x.ParentID).ToList();
            ViewBag.Menu = data;
        }

        protected List<string> UploadFiles(HttpFileCollectionBase files)
        {
            List<string> listFiles = new List<string>();
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

                fname = Path.Combine(Server.MapPath($@"~/Files/Stored/"), fname);
                file.SaveAs(fname);
                string name = ChangeName(fname, true);
                System.IO.File.Move(fname, name);
                name = $@"Files/Stored/{Path.GetFileName(name)}";
                    listFiles.Add(name);
                }

            return listFiles;
        }
        
        protected string UploadImportFiles(HttpFileCollectionBase files)
        {
            string name = string.Empty;
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
                name = ChangeName(fname);
                System.IO.File.Move(fname, name);
            }

            return name;
        }

        private string ChangeName(string fname, bool keepName)
        {
            string path = Path.GetDirectoryName(fname);
            string extention = Path.GetExtension(fname);
            string name = Path.GetFileNameWithoutExtension(fname);
            string newName = $"{path}/{name}_{DateTime.Now.ToString("yyyyMMddHHmmss")}{extention}";
            return newName;
        }

        private string ChangeName(string fname)
        {
            string path = Path.GetDirectoryName(fname);
            string extention = Path.GetExtension(fname);
            string newName = $"{path}/{DateTime.Now.ToString("yyyyMMddHHmmss")}{extention}";
            return newName;
        }
        
        private void CheckAndUpdateDAtabaseUrl()
        {
            string controllerName = RouteData.Values["controller"].ToString();
            string actionName = RouteData.Values["action"].ToString();
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var data = db.C222_WebUrl.Where(x => x.Action.ToUpper() == actionName.ToUpper() && x.Controller.ToUpper() == controllerName.ToUpper()).FirstOrDefault();
            if (data != null)
            {
                var url = Url.Action(actionName, controllerName);
                if(data.LocalPath.ToUpper() != url.ToUpper())
                {
                    data.LocalPath = url;
                    db.SaveChanges();
                }
                return;
            }

            C222_WebUrl obj = new C222_WebUrl();
            obj.Action = actionName;
            obj.Controller = controllerName;
            obj.LocalPath = Url.Action(actionName, controllerName);
            obj.Updated = true;
            db.C222_WebUrl.Add(obj);
            db.SaveChanges();
        }

        private bool CheckWebPermiss()
        {
            bool result = true;
            NN_DatabaseEntities db = new NN_DatabaseEntities();
            var controller = Request.RequestContext.RouteData.Values["Controller"].ToString();
            var requestUrl = Request.Url.LocalPath.ToUpper();
            var data = db.View_222_WebPermission.Where(x => x.Controller.ToUpper() == controller.ToUpper() && x.LocalPath.ToUpper() == requestUrl).ToList();
            if(data.Count > 0)
            {
                var staffID = Session["StaffID"].ToString();
                var per = data.Where(x => x.StaffID == staffID).Any();
                result = per;
            }
            return result;
        }
    }
}