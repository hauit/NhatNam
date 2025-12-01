using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NNworking.Controllers
{
    public class PurchaseController : BaseController
    {
        public static string Purchase
        {
            get { return "PurchaseController"; }
        }

        // GET: Purchase
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SettingModule()
        {
            return View();
        }
    }
}