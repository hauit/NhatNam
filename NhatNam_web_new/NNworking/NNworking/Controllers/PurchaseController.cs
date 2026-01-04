using NNworking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace NNworking.Controllers
{
    public class PurchaseController : BaseController
    {
        public static string ModuleName
        {
            get { return nameof(PurchaseController); }
        }

        // GET: Purchase
        public ActionResult Index()
        {
            return View();
        }

        // GET: Purchase
        public ActionResult PurChaseOrderAdd()
        {
            return View();
        }

        public ActionResult WorkFolowSetup()
        {
            ViewBag.ModuleName = ModuleName;
            return View("~/Views/Kaizen/WorkFolowSetup.cshtml");
        }

        /// <summary>
        /// Purchase Order Detail
        /// </summary>
        /// id: Purchase Order ID
        /// <returns></returns>
        public ActionResult PurchaseOrderDetail(int id)
        {
            return View();
        }

        [HttpPost]
        public JsonResult InsertPurchaseOrders([FromBody] PurchaseFullDto dto)
        {
            try
            {
                using (NN_DatabaseEntities _context = new NN_DatabaseEntities())
                using (var tran = _context.Database.BeginTransaction())
                {
                    dto.Purchase.PurchaseID = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    dto.Purchase.Complete = false;
                    _context.C222_PurChase.Add(dto.Purchase);
                    //_context.SaveChanges();

                    foreach (var d in dto.Details)
                    {
                        d.PurchaseID = dto.Purchase.PurchaseID;
                        d.Amount = d.Price * d.Qty;
                        _context.C222_PurChaseDetail.Add(d);
                    }

                    _context.SaveChanges();
                    tran.Commit();
                }
                return Json(new { Status = "OK", Values = string.Empty });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "NG", Values = ex.Message});
            }
        }

        public class PurchaseFullDto
        {
            public C222_PurChase Purchase { get; set; }
            public List<C222_PurChaseDetail> Details { get; set; }
        }
    }
}