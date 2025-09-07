using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NNworking.Controllers
{
    [RoutePrefix("Thong-bao")]
    public class EmailConfigurationController : Controller
    {
        [Route("trang-chu.html")]
        // GET: EmailConfiguration
        public ActionResult Index()
        {
            return View();
        }
        
        [Route("danh-sach-email.html")]
        public ActionResult NotificationEmailList()
        {
            return View();
        }
        
        [Route("danh-sach-chuc-nang.html")]
        public ActionResult NotificationFunctionList()
        {
            return View();
        }
        
        [Route("chuc-nang-thong-bao.html")]
        public ActionResult NotificationSendingEmail()
        {
            return View();
        }
        
        [Route("kieu-thong-bao.html")]
        public ActionResult NotificationSendType()
        {
            return View();
        }

        [Route("danh-sach-bao-cao.html")]
        public ActionResult ReportList()
        {
            return View();
        }

        [Route("thoi-gian-lay-du-lieu-cua-bao-cao.html")]
        public ActionResult TimeSendEmail()
        {
            return View();
        }
    }
}