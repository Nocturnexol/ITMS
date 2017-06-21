using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bitshare.PTMM.Web.Common;
using Bitshare.PTMM.Common;
using Bitshare.PTMM.Model;
using System.Data;
using Bitshare.PTMM.Service.Impl;
using Bitshare.PTMM.Service;
using Bitshare.PTMM.Service.DTO;

namespace Bitshare.PTMM.Web.Areas.System.Controllers
{
    public class ScheduleControllerController : Controller
    {
        //
        // GET: /System/ScheduleController/

        public ActionResult Index()
        {

            List<string> BtnList = new List<string> { "搜索" };
            ViewBag.BtnList = BtnList;
            return View();
        }

        public ActionResult GetPageResult(string keyword = null, int page = 1, int rows = 50, string sidx = "", string sord = "")
        {
            SystemServiceFactory factory = new SystemServiceFactory();
            ISystemService service = factory.GetInstance();
            SubPageResult<tblAdOrder> result = new SubPageResult<tblAdOrder>();
            string LoginName = CurrentHelper.CurrentUser.User.LoginName;
            string Authority = FlowHelper.GetBusinessAuthority("订单查询");
            switch (Authority)
            {
                case "查看所有":
                    result = service.GetSchedulePageResult(LoginName, keyword, page, rows, sidx, sord); break;
                case "查看下级":
                    result = service.GetSchedulePageResultForMeUnderling(LoginName, keyword, page, rows, sidx, sord); break;
                default:
                    result = service.GetSchedulePageResultForMe(LoginName, keyword, page, rows, sidx, sord); ; break;

            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //
        // GET: /System/Schedule/Details/5
        public ActionResult Details(int id = 0)
        {
            
            return View();
        }
        //
        // GET: /System/Schedule/Create

        public ActionResult Create()
        {
            return View();
        }
        //
        // POST: /System/Schedule/Create

        [HttpPost]
        public ActionResult Create(tblAdOrder tbladorder)
        {
            if (ModelState.IsValid)
            {
                //db.tblAdOrder.AddObject(tbladorder);
                //db.SaveChanges();
                //return RedirectToAction("Index");
            }

            return View(tbladorder);
        }

        //
        // GET: /System/Schedule/Edit/5

        public ActionResult Edit(int id = 0)
        {
            tblAdOrder tbladorder = BusinessContext.tblAdOrder.GetModel(id);
            //tblAdOrder tbladorder = db.tblAdOrder.Single(t => t.TblRcdID == id);
            if (tbladorder == null)
            {
                return HttpNotFound();
            }
            return View(tbladorder);
        }

        //
        // POST: /System/Schedule/Edit/5

        [HttpPost]
        public ActionResult Edit(tblAdOrder tbladorder)
        {
            if (ModelState.IsValid)
            {
                //db.tblAdOrder.Attach(tbladorder);
                //db.ObjectStateManager.ChangeObjectState(tbladorder, EntityState.Modified);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbladorder);
        }

        //
        // GET: /System/Schedule/Delete/5

        public ActionResult Delete(int id = 0)
        {
            tblAdOrder tbladorder = BusinessContext.tblAdOrder.GetModel(id);
            //tblAdOrder tbladorder = db.tblAdOrder.Single(t => t.TblRcdID == id);
            if (tbladorder == null)
            {
                return HttpNotFound();
            }
            return View(tbladorder);
        }

        //
        // POST: /System/Schedule/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            //tblAdOrder tbladorder = db.tblAdOrder.Single(t => t.TblRcdID == id);
            //db.tblAdOrder.DeleteObject(tbladorder);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            //db.Dispose();
            base.Dispose(disposing);
        }
    }
}
