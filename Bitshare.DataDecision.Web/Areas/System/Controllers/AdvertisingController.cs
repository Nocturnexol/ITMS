using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bitshare.DataDecision.Web.Common;
using Bitshare.DataDecision.Common;
using Bitshare.DataDecision.Model;
using System.Data;
using Bitshare.DataDecision.Service.Impl;
using Bitshare.DataDecision.Service.DTO;
using Bitshare.DataDecision.Service;

namespace Bitshare.DataDecision.Web.Areas.System.Controllers
{
    public class AdvertisingController : Controller
    {
        //
        // GET: /System/Advertising/

        public ActionResult Index(string keyword = null, int page = 1, int PageSize = 50)
        {

            List<string> BtnList = new List<string> { "搜索" };
            ViewBag.BtnList = BtnList;
            return View();
        }

        

        //
        // GET: /System/Advertising/Details/5

        //public ActionResult Details(int id = 0)
        //{
        //    tblAdOrder tbladorder = db.tblAdOrder.Single(t => t.TblRcdID == id);
        //    if (tbladorder == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tbladorder);
        //}

        //
        // GET: /System/Advertising/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /System/Advertising/Create

        //[HttpPost]
        //public ActionResult Create(tblAdOrder tbladorder)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.tblAdOrder.AddObject(tbladorder);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(tbladorder);
        //}

        //
        // GET: /System/Advertising/Edit/5

        //public ActionResult Edit(int id = 0)
        //{
        //    tblAdOrder tbladorder = db.tblAdOrder.Single(t => t.TblRcdID == id);
        //    if (tbladorder == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tbladorder);
        //}

        //
        // POST: /System/Advertising/Edit/5

        //[HttpPost]
        //public ActionResult Edit(tblAdOrder tbladorder)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.tblAdOrder.Attach(tbladorder);
        //        db.ObjectStateManager.ChangeObjectState(tbladorder, EntityState.Modified);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(tbladorder);
        //}

        //
        // GET: /System/Advertising/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    tblAdOrder tbladorder = db.tblAdOrder.Single(t => t.TblRcdID == id);
        //    if (tbladorder == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tbladorder);
        //}

        //
        // POST: /System/Advertising/Delete/5

        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    tblAdOrder tbladorder = db.tblAdOrder.Single(t => t.TblRcdID == id);
        //    db.tblAdOrder.DeleteObject(tbladorder);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            //db.Dispose();
            base.Dispose(disposing);
        }
    }
}
