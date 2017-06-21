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
    /// <summary>
    /// 部分管理
    /// </summary>
    public class DepartController : Controller
    {
        //
        // GET: /System/Depart/
        static string whe = string.Empty;
        public ActionResult Index()
        {
            List<string> BtnList = FlowHelper.GetBtnAuthorityForPage("部门管理");
            ViewBag.BtnList = BtnList;
            return View();
        }
        [AntiSqlInject]
        public ActionResult GetGqDataList(int page = 1, int rows = 20, string keyword = null, string sidx = "dept", string sord = "asc")
        {
            string strSql = "1=1";
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                strSql += " and dept like '%" + keyword + "%'";
            }
            string orderBy = "dept";

            string where = strSql.ToString();
            whe = where;
            jqGridData RM = FlowHelper.GetJqGridDataList<tblDepart>(where, orderBy, page, rows, sidx, sord);
            return Json(RM, JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /System/Depart/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /System/Depart/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /System/Depart/Create

        [HttpPost]
        public ActionResult Create(tblDepart collection, string IsContinue = "0")
        {
            ReturnMessage RM = new ReturnMessage(false);
            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrEmpty(collection.dept))
                    {
                        var query = BusinessContext.tblDepart.GetModelList(String.Format("dept='{0}'", collection.dept));
                         if (query.Count > 0)
                         {
                             RM.Message = "部门名称被占用";
                         }
                         else
                         {
                             int tblRcdid = BusinessContext.tblDepart.Add(collection);
                             RM.IsSuccess = tblRcdid > 0;
                             if (IsContinue == "1")
                             {
                                 OperateLogHelper.Create<tblDepart>(collection);
                                 RM.IsContinue = true;
                             }
                             else
                             {
                                 RM.IsContinue = false;
                             }
                         }
                    }
                    else
                    {
                        RM.IsSuccess = false;
                        RM.Message = "部门名称不能为空！";
                    }

                }
                catch (Exception ex)
                {
                    RM.Message = ex.Message;
                }
            }
            return Json(RM);
        }

        //
        // GET: /System/Depart/Edit/5

        public ActionResult Edit(int id)
        {
            tblDepart model = BusinessContext.tblDepart.GetModel(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        //
        // POST: /System/Depart/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, tblDepart collection)
        {
            ReturnMessage RM = new ReturnMessage(false);// new ReturnMessage(false);
            try
            {
                if (!string.IsNullOrEmpty(collection.dept))
                {
                    var query = BusinessContext.tblDepart.GetModelList(String.Format("dept='{0}' and tblrcdid<>{1}", collection.dept, collection.Tblrcdid));
                    tblDepart old = BusinessContext.tblDepart.GetModel(id); 
                    if (query.Count > 0)
                    {
                        RM.Message = "部门名称被占用";
                    }
                    else
                    {
                        RM.IsSuccess = BusinessContext.tblDepart.Update(collection);
                        if (RM.IsSuccess)
                        {
                            OperateLogHelper.Edit<tblDepart>(collection, old);
                        }
                    }
                }
                else
                {
                    RM.IsSuccess = false;
                    RM.Message = "部门名称不能为空！";
                }
            }
            catch (Exception ex)
            {
                RM.Message = ex.Message;
            }
            return Json(RM);
        }

        //
        // GET: /System/Depart/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /System/Depart/Delete/5

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

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public ActionResult DataDel()
        {
            ReturnMessage RM = new ReturnMessage();
            try
            {
                string paramData = Request["paramData"];
                if (!string.IsNullOrWhiteSpace(paramData))
                {
                    string[] tblRcdidArr = paramData.Split('*');

                    StringBuilder strSql = new StringBuilder(" delete from tblDepart where TblRcdId in " + DBContext.DataDecision.AssemblyInCondition(tblRcdidArr.ToList()));
                    List<tblDepart> modelList = BusinessContext.tblDepart.GetModelList("TblRcdId in " + DBContext.AssemblyInCondition(tblRcdidArr.ToList())).ToList();
                    if (DBContext.DataDecision.ExecSql(strSql.ToString()) > 0)
                    {

                        OperateLogHelper.Delete<tblDepart>(modelList);
                        RM.IsSuccess = true;
                        RM.Message = "删除成功！";
                    }
                    else
                    {
                        RM.IsSuccess = false;
                        RM.Message = "删除失败！";
                    }
                }
            }
            catch (Exception ex)
            {
                RM.IsSuccess = false;
                RM.Message = ex.Message;
            }
            return Json(RM, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeriveData()
        {
            ReturnMessage RM = new ReturnMessage();
            try
            {
                string paramData = Request.Form["paramData"];
                StringBuilder strSql = new StringBuilder("select  dept '部门 ', Remark as '备注'  from tblDepart  where  1=1 ");
                if (string.IsNullOrEmpty(paramData))
                {
                    string keyword = Request.QueryString["keyword"];
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        strSql.Append(" and (dept like '%" + keyword + "%')");
                    }
                    strSql.Append(" and " + whe);
                }
                else
                {
                    string[] tblRcdidArr = paramData.Split('*');
                    strSql.Append(" and  TblRcdId in " + DBContext.DataDecision.AssemblyInCondition(tblRcdidArr.ToList()));
                }
                strSql.Append(" order by dept");

                DataTable dt = DBContext.DataDecision.GetDataTable(strSql.ToString());
                string sheetName = "部门名称";
                //返回路径
                string absoluFilePath = DoExport.ExportDataTableToExcel(dt, sheetName, "部门名称");

                RM.IsSuccess = true;
                RM.Text = HttpUtility.UrlEncode(absoluFilePath);

            }
            catch (Exception ex)
            {
                RM.IsSuccess = false;
                RM.Message = ex.Message;
            }
            return Json(RM, JsonRequestBehavior.AllowGet);
        }
    }
}
