using Bitshare.DataDecision.Common;
using Bitshare.DataDecision.Controllers.Filter;
using Bitshare.DataDecision.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Bitshare.DataDecision.Service.DTO;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Bitshare.DataDecision.Controllers.Areas.System
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
            if (string.IsNullOrEmpty(sidx))
                sidx = "dept";
            var pager = new PageInfo
            {
                PageSize = rows,
                CurrentPageIndex = page > 0 ? page : 1
            };

            List<IMongoQuery> queryList = new List<IMongoQuery>();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                queryList.Add(Query<tblDepart>.Matches(t => t.dept,
                    new BsonRegularExpression(new Regex(keyword, RegexOptions.IgnoreCase))));
            }
            var where = queryList.Any() ? Query.And(queryList) : null;

            long totalCount;
            var rm = new jqGridData
            {
                page = pager.CurrentPageIndex,
                rows = BusinessContext.tblDepart.GetList(out totalCount, page, rows, where, sidx, sord),
                total = (int)(totalCount % pager.PageSize == 0 ? totalCount / pager.PageSize : totalCount / pager.PageSize + 1),
                records = (int)totalCount
            };
            return Json(rm, JsonRequestBehavior.AllowGet);
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
                        var query = BusinessContext.tblDepart.GetList(Query<tblDepart>.EQ(t => t.dept, collection.dept));
                         if (query.Count > 0)
                         {
                             RM.Message = "部门名称被占用";
                         }
                         else
                         {
                             var res = BusinessContext.tblDepart.Add(collection);
                             RM.IsSuccess = res;
                             RM.IsContinue = IsContinue == "1";
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
            tblDepart model = BusinessContext.tblDepart.Get(Query<tblDepart>.EQ(t => t.Rid, id));
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
                    var q = Query.And(Query<tblDepart>.EQ(t => t.dept, collection.dept),
                        Query<tblDepart>.NE(t => t.Rid, collection.Rid));
                    var query = BusinessContext.tblDepart.GetList(q);
                    //tblDepart old = BusinessContext.tblDepart.Get(Query<tblDepart>.EQ(t => t.Rid, id));
                    if (query.Count > 0)
                    {
                        RM.Message = "部门名称被占用";
                    }
                    else
                    {
                        RM.IsSuccess = BusinessContext.tblDepart.Update(collection);
                        if (RM.IsSuccess)
                        {
                            //OperateLogHelper.Edit<tblDepart>(collection, old);
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
            ReturnMessage rm = new ReturnMessage();
            try
            {
                string paramData = Request["paramData"];
                if (!string.IsNullOrWhiteSpace(paramData))
                {
                    string[] ridArr = paramData.Split('*');

                    //StringBuilder strSql = new StringBuilder(" delete from tblDepart where Rid in " + DBContext.DataDecision.AssemblyInCondition(RidArr.ToList()));
                    //List<tblDepart> modelList = BusinessContext.tblDepart.GetList(Query<tblDepart>.In(t=>t.Rid,RidArr.Select(int.Parse))).ToList();
                    if (BusinessContext.tblDepart.Delete(ridArr.Select(int.Parse).ToList()))
                    {

                        //OperateLogHelper.Delete<tblDepart>(modelList);
                        rm.IsSuccess = true;
                        rm.Message = "删除成功！";
                    }
                    else
                    {
                        rm.IsSuccess = false;
                        rm.Message = "删除失败！";
                    }
                }
            }
            catch (Exception ex)
            {
                rm.IsSuccess = false;
                rm.Message = ex.Message;
            }
            return Json(rm, JsonRequestBehavior.AllowGet);
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
                    string[] RidArr = paramData.Split('*');
                    strSql.Append(" and  Rid in " + DBContext.DataDecision.AssemblyInCondition(RidArr.ToList()));
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
