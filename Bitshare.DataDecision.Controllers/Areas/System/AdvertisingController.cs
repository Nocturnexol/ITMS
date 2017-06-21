using System.Collections.Generic;
using System.Web.Mvc;

namespace Bitshare.DataDecision.Controllers.Areas.System
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
        // GET: /System/Advertising/Create

        public ActionResult Create()
        {
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
