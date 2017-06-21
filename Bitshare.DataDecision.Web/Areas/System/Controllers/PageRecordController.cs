using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bitshare.DataDecision.Web.Common;
using System.Text;
using Bitshare.DataDecision.Model;
using System.Data;
using Bitshare.DataDecision.Common;
using Bitshare.DataDecision.Web.Models;

namespace Bitshare.DataDecision.Web.Areas.System.Controllers

{
    public class PageRecordController : Controller
    {

        public ActionResult Index()
        {
          List<string> BtnList = FlowHelper.GetBtnAuthorityForPage("页面访问统计");
          ViewBag.BtnList = BtnList;

          #region 暂时不用
          //if (!string.IsNullOrEmpty(startDate) && startDate != DateTime.Now.ToString())
          //{
          //    if (!string.IsNullOrEmpty(str_cdt))
          //    {
          //        str_cdt += "and EndDate>='" + startDate + "'";
          //    }
          //    else
          //    {
          //        str_cdt += "EndDate>='" + startDate + "'";
          //    }
          //}
          //if (!string.IsNullOrEmpty(endDate) && endDate != DateTime.Now.ToString())
          //{
          //    if (!string.IsNullOrEmpty(str_cdt))
          //    {
          //        str_cdt += "and EndDate<='" + endDate + "'";
          //    }
          //    else
          //    {
          //        str_cdt += "EndDate<='" + endDate + "'";
          //    }
          //}

          //StartTime = StartTime == null ? (DateTime.Now.AddMonths(-12).ToShortDateString()) : (StartTime);
          //EndTime = EndTime == null ? (DateTime.Now.ToShortDateString()) : (EndTime);
          //ViewData["StartTime"] = StartTime;
          //ViewData["EndTime"] = EndTime;
          #endregion
          return View();
        }


        public ActionResult GetDataList(string TblRcdId = null, int page = 1, int rows = 50, string startDate = null, string endDate=null, string sidx = "", string sord = "asc")
        {
            #region 通过存储过程获得数据
           
            PageInfo pager = new PageInfo();
            pager.PageSize = rows;
            pager.CurrentPageIndex = (page != 0 ? (int)page : 1);
           // string PageTitle = Request["PageTitle"];
           // string PageUrl = Request["PageUrl"];
            StringBuilder strSql = new StringBuilder(" 1=1");
            if (!string.IsNullOrEmpty(startDate))
            {
                strSql.Append(" and RecordDate>='" + startDate + "'");
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                strSql.Append(" and RecordDate<='" + endDate + "'");
            }  
            string tableName = "tblPageView";
            string orderBy = " PageTitle ";
            int totalCount = 0;
            string queryFields = " PageTitle, PageUrl, count(tblrcdid)  as CountSum ";
            string where = strSql.ToString() + " group by PageTitle, PageUrl ";
            // 获取DataTable 数据
            DataTable ds = DBContext.DataDecision.QueryPageByProc(tableName, orderBy, out totalCount, queryFields, where, pager.CurrentPageIndex, pager.PageSize);

            List<view_PageRecord> result = new List<view_PageRecord>();
            result = ds.ToList<view_PageRecord>();

            jqGridData RM = new jqGridData();
            RM.page = pager.CurrentPageIndex;
            RM.rows = result;
            RM.total = (totalCount % pager.PageSize == 0 ? totalCount / pager.PageSize : totalCount / pager.PageSize + 1);
            RM.records = totalCount;
            return Json(RM, JsonRequestBehavior.AllowGet);

        }
            #endregion

        #region 导出数据
        public ActionResult DeriveData()
        {
            ReturnMessage RM = new ReturnMessage();
            try
            {
                string paramData=Request.Form["paramData"];
                string PageTitle = Request["PageTitle"];
                string PageUrl = Request["PageUrl"];
                string ViewCount = Request["ViewCount"];
                string RecordDate = Request["RecordDate"];
                DateTime now = DateTime.Now;
                string stateDate=null;
                string endDate = null;
                string where = " 1=1 ";
                if (!string.IsNullOrWhiteSpace(stateDate))
                {
                    where += " and (RecordDate>='" + stateDate + "')";
                }
                if (!string.IsNullOrEmpty(endDate))
                {
                    where += " and (RecordDate<= '" + endDate + "')";
                }
               
                StringBuilder strSql=new StringBuilder();

                strSql.Append("select ");
                strSql.Append("PageTitle as '页面标题'");
                strSql.Append(",PageUrl as '页面路径'");
                strSql.Append(",count(tblrcdid) as '访问数量'");
                strSql.Append(" from tblPageView");
                strSql.Append(" where" + where);
                strSql.Append("group by PageTitle, PageUrl");
              
                DataTable dt = DBContext.DataDecision.GetDataTable(strSql.ToString());

                string sheetName = "页面访问统计";
                //返回路径
                string absoluFilePath = DoExport.ExportDataTableToExcel(dt, sheetName, "页面访问统计");
                RM.IsSuccess = true;
                RM.Text = HttpUtility.UrlEncode(absoluFilePath);
            }
            catch (Exception e)
            {

                RM.IsSuccess = false;
                RM.Message = e.Message;
            }
            return Json(RM, JsonRequestBehavior.AllowGet);
        }
        #endregion

        // GET: /System/PageRecord/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: /System/PageRecord/Create

        public ActionResult Create()
        {
            return View();
        }

        // POST: /System/PageRecord/Create

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

        // GET: /System/PageRecord/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }


        // POST: /System/PageRecord/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {


                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // GET: /System/PageRecord/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /System/PageRecord/Delete/5

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
