using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NNworking.Controllers
{
    [RoutePrefix("bao-tri")]
    public class BaoTriController : BaseController
    {
        // GET: BaoTri
        public ActionResult Index()
        {
            return RedirectToAction("MachineAndPlan");
        }

        [Route("cong-viec")]
        public ActionResult DailyCheck()
        {
            return View();
        }

        [Route("thuc-hien")]
        public ActionResult DailyWork(string machineId, string assignDate, bool? readOnly)
        {
            ViewBag.MachineId = machineId;
            ViewBag.AssignDate = assignDate;
            ViewBag.ReadOnly = readOnly ?? false;
            return View();
        }

        [Route("su-co")]
        public ActionResult DailyBug()
        {
            return View();
        }

        [Route("quan-ly")]
        public ActionResult MachineAndPlan()
        {
            return View();
        }
    }
}