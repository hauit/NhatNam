using NNworking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Windows.Media.Animation;
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

        public ActionResult PurchaseList(int purchaseStatus)
        {
            ViewBag.PurchaseStatus = purchaseStatus;
            return View();
        }

        /// <summary>
        /// Purchase Order Detail
        /// </summary>
        /// id: Purchase Order ID
        /// <returns></returns>
        public ActionResult PurchaseOrderDetail(int id)
        {
            using (NN_DatabaseEntities _context = new NN_DatabaseEntities())
            {
                var purchase = _context.C222_PurChase.Where(x => x.ID == id).FirstOrDefault();
                if (purchase == null) {
                    return HttpNotFound();
                }

                ViewBag.Purchase = purchase;
                var details = _context.C222_PurChaseDetail.Where(x => x.PurchaseID == purchase.PurchaseID).ToList();
                ViewBag.Details = details;
                var instance = _context.C222_WorkFolowInstance.Where(x => x.ItemID == purchase.ID && x.ModuleName == ModuleName).FirstOrDefault();
                var history = _context.C222_WorkFolowInstanceHistory.Where(x => x.InstanceID == instance.ID).ToList();
                C222_WorkFolowInstanceHistory createObj = new C222_WorkFolowInstanceHistory
                {
                    Commment = "Đơn hàng được tạo bởi " + purchase.StaffID,
                    ActionBy = purchase.StaffID,
                    ActionDate = purchase.InputDate,
                    ModuleName = ModuleName
                };

                history.Insert(0, createObj);
                ViewBag.History = history;
            }
            return View();
        }

        [HttpPost]
        public JsonResult InsertPurchaseOrders([FromBody] PurchaseFullDto dto)
        {
            using (NN_DatabaseEntities _context = new NN_DatabaseEntities())
            using (var tran = _context.Database.BeginTransaction())
            {
                try
                {
                    dto.Purchase.PurchaseID = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    dto.Purchase.Complete = false;
                    _context.C222_PurChase.Add(dto.Purchase);
                    _context.SaveChanges();

                    foreach (var d in dto.Details)
                    {
                        d.PurchaseID = dto.Purchase.PurchaseID;
                        d.Amount = d.Price * d.Qty;
                        _context.C222_PurChaseDetail.Add(d);
                    }

                    BaseModel.InputWorkFolloIntance(_context, dto.Purchase.StaffID, dto.Purchase.ID, ModuleName);

                    _context.SaveChanges();
                    tran.Commit();
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    return Json(new { Status = "NG", Values = ex.Message });
                }
            }
            return Json(new { Status = "OK", Values = string.Empty });
        }

        public class PurchaseFullDto
        {
            public C222_PurChase Purchase { get; set; }
            public List<C222_PurChaseDetail> Details { get; set; }
        }
    }
}