using NNworking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NNworking.Controllers
{
    [RoutePrefix("quan-ly-kho")]
    public class StockerController : BaseController
    {
        public StockerController()
        {
            StockerMenu();
        }

        [Route("trang-chu.html")]
        // GET: Stocker
        public ActionResult Index()
        {
            CheckPermissAndRedirect();
            return View();
        }

        //[Route("{catergory?}.html")]
        //public ActionResult ItemList(string catergory)
        //{
        //    CheckPermissAndRedirect();
        //    ViewBag.Title = "Danh sách dụng cụ";
        //    ViewBag.Catergory = catergory;
        //    return View("~/Views/Stocker/StockerItemList.cshtml");
        //}

        [Route("{catergory?}.html")]
        public ActionResult StockerItem(string catergory)
        {
            CheckPermissAndRedirect();
            ViewBag.Title = "Danh sách dụng cụ";
            ViewBag.Catergory = catergory;
            return View("~/Views/Stocker/Manager/StockerItem.cshtml");
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

        [Route("kieu-danh-muc.html")]
        public ActionResult StokerCatergoryType()
        {
            CheckPermissAndRedirect();
            return View("~/Views/Stocker/Manager/StokerCatergoryType.cshtml");
        }

        [Route("kieu-san-pham.html")]
        public ActionResult StockerItemType()
        {
            CheckPermissAndRedirect();
            return View("~/Views/Stocker/Manager/StockerItemType.cshtml");
        }

        [Route("danh-muc-san-pham.html")]
        public ActionResult StokerCatergory()
        {
            CheckPermissAndRedirect();
            return View("~/Views/Stocker/Manager/StokerCatergory.cshtml");
        }

        [Route("don-vi-tinh.html")]
        public ActionResult StokerUnit()
        {
            CheckPermissAndRedirect();
            return View("~/Views/Stocker/Manager/StokerUnit.cshtml");
        }

        [Route("cau-hinh-danh-muc-san-pham.html")]
        public ActionResult CatergoryConfiguration()
        {
            CheckPermissAndRedirect();
            return View("~/Views/Stocker/Manager/CatergoryConfiguration.cshtml");
        }

        [HttpPost]
        public JsonResult History(string shift, string MachineID)
        {
            try
            {
                NN_DatabaseEntities _context = new NN_DatabaseEntities();
                var c222_stokeroutput = _context.sp_222_StockerOutput_OutputPreparation().ToList();
                return Json(new { Status = "OK", Values = c222_stokeroutput }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}