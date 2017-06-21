using Bitshare.Common;
using Bitshare.DataDecision.Common;
using Bitshare.DataDecision.Model;
using Bitshare.DataDecision.Service.DTO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Bitshare.DataDecision.Controllers.Areas.System
{
    public class OperateLogController : Controller
    {
        //
        // GET: /System/OperateLog/

        public ActionResult Index()
        {
            List<string> BtnList = FlowHelper.GetBtnAuthorityForPage("操作日志");
            ViewBag.BtnList = BtnList;
            return View();
        }
        public ActionResult GetDataList(int page = 1, int rows = 20, string keyword = null, string sidx = "", string sord = "asc")
        {
            #region 通过存储过程获得数据

            PageInfo pager = new PageInfo();
            pager.PageSize = rows;
            pager.CurrentPageIndex = (page != 0 ? (int)page : 1);
            StringBuilder strSql = new StringBuilder(" 1=1");
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                strSql.Append(" and (Content like '%" + keyword + "%' or LoginName like'%"+keyword+"%' or UserName like'%"+keyword+"%')");
            }

            string tableName = "sysOperateLog";
            string orderBy = "OperateTime desc";
            if (!string.IsNullOrWhiteSpace(sidx))
            {
                orderBy = sidx + " " + sord;
            }
            int totalCount = 0;
            string queryFields = "*";
            string where = strSql.ToString();
            DataTable ds = DBContext.DataDecision.QueryPageByProc(tableName, orderBy, out totalCount, queryFields, where, pager.CurrentPageIndex, pager.PageSize);

            List<sysOperateLog> result = new List<sysOperateLog>();

            result = ds.ToList<sysOperateLog>();
        
            #endregion
            jqGridData RM = new jqGridData();
            RM.page = pager.CurrentPageIndex;
            RM.rows = result;
            RM.total = (totalCount % pager.PageSize == 0 ? totalCount / pager.PageSize : totalCount / pager.PageSize + 1);
            RM.records = totalCount;
            return Json(RM, JsonRequestBehavior.AllowGet);
        }


        //
        // GET: /System/OperateLog/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /System/OperateLog/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /System/OperateLog/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /System/OperateLog/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /System/OperateLog/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /System/OperateLog/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /System/OperateLog/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
