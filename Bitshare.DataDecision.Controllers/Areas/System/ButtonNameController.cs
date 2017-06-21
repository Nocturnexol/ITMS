using Bitshare.Common;
using Bitshare.DataDecision.Common;
using Bitshare.DataDecision.Model;
using Bitshare.DataDecision.Service.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Bitshare.DataDecision.Controllers.Areas.System
{
    public class ButtonNameController : Controller
    {
        static string whe = string.Empty;
        //
        // GET: /System/ButtonName/
        [UrlAuthorize("按钮名称管理")]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index(string keyword = null, string Depart = null, int page = 1)
        {
            //按钮权限
            List<string> BtnList = FlowHelper.GetBtnAuthorityForPage("按钮名称管理");
            ViewBag.BtnList = BtnList;
            return View();
        }


        public ActionResult GetButtonNameDataList(int page = 1, int rows = 20, string keyword = null, string sidx = "ButtonName", string sord = "asc")
        {

            if (string.IsNullOrEmpty(sidx))
                sidx = "Rid";
            var pager = new PageInfo
            {
                PageSize = rows,
                CurrentPageIndex = page > 0 ? page : 1
            };

            List<IMongoQuery> queryList = null;



            if (!string.IsNullOrWhiteSpace(keyword))
            {
                queryList = new List<IMongoQuery>
                {
                    Query<tblButtonName>.Matches(t => t.ButtonName,
                        new BsonRegularExpression(new Regex(keyword, RegexOptions.IgnoreCase)))
                };
            }
            var where = queryList != null && queryList.Any() ? Query.And(queryList) : null;

            long totalCount;
            var rm = new jqGridData
            {
                page = pager.CurrentPageIndex,
                rows = BusinessContext.tblButtonName.GetList(out totalCount, page, rows, where, sidx, sord),
                total = (int)(totalCount % pager.PageSize == 0 ? totalCount / pager.PageSize : totalCount / pager.PageSize + 1),
                records = (int)totalCount
            };
            return Json(rm, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 新建
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(tblButtonName collection, string IsContinue = "0")
        {
            ReturnMessage RM = new ReturnMessage(false);
            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrEmpty(collection.ButtonName))
                    {
                        var query =
                            BusinessContext.tblButtonName.GetList(Query<tblButtonName>.EQ(t => t.ButtonName,
                                collection.ButtonName));
                        if (query.Count > 0)
                        {
                            RM.Message = "按钮名称已被占用";
                        }
                        else
                        {
                            var res= BusinessContext.tblButtonName.Add(collection);
                            RM.IsSuccess = res;
                            if (RM.IsSuccess)
                            {

                                //collection.Rid = Rid;
                                OperateLogHelper.Create<tblButtonName>(collection);
                                if (IsContinue == "1")
                                {
                                    RM.IsContinue = true;
                                }
                                else
                                {
                                    RM.IsContinue = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        RM.IsSuccess = false;
                        RM.Message = "按钮名称不能为空！";
                    }

                }
                catch (Exception ex)
                {
                    RM.Message = ex.Message;
                }
            }
            return Json(RM);
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {

            tblButtonName dpt = BusinessContext.tblButtonName.Get(Query<tblButtonName>.EQ(t=>t.Rid,id));
            if (dpt == null)
            {
                return HttpNotFound();
            }
            return View(dpt);

        }
        [HttpPost]
        public ActionResult Edit(tblButtonName collection)
        {
            ReturnMessage RM = new ReturnMessage(false);
            if (ModelState.IsValid)
            {
                try
                {
                    var q = Query.And(Query<tblButtonName>.EQ(t => t.ButtonName, collection.ButtonName),
                        Query<tblButtonName>.NE(t => t.Rid, collection.Rid));
                    var query = BusinessContext.tblButtonName.GetList(q);
                    if (query.Count > 0)
                    {
                        RM.Message = "按钮名称已被占用";
                    }
                    else
                    {
                        tblButtonName old = BusinessContext.tblButtonName.Get(Query<tblButtonName>.EQ(t => t.Rid, collection.Rid));
                        RM.IsSuccess = BusinessContext.tblButtonName.Update(collection);
                        if (RM.IsSuccess)
                        {
                            OperateLogHelper.Edit<tblButtonName>(collection, old);
                        }
                    }

                }
                catch (Exception ex)
                {
                    RM.Message = ex.Message;
                }
            }
            return Json(RM);
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
                string paramData = Request.Form["paramData"];
                if (!string.IsNullOrWhiteSpace(paramData))
                {
                    string[] RidArr = paramData.Split('*');
                    StringBuilder strSql = new StringBuilder(" select ButtonNameId from tblGroupButton where ButtonNameId in(select Rid from tblButtonName where Rid in " + DBContext.DataDecision.AssemblyInCondition(RidArr.ToList()) + ")");
                    DataTable dt = DBContext.DataDecision.GetDataTable(strSql.ToString());

                    strSql = new StringBuilder(" select ButtonNameId from tblDetailButton where ButtonNameId in(select Rid from tblButtonName where Rid in " + DBContext.DataDecision.AssemblyInCondition(RidArr.ToList()) + ")");
                    DataTable Detail_dt = DBContext.DataDecision.GetDataTable(strSql.ToString());

                    //List<tblButtonName> modelList = BusinessContext.tblButtonName.GetModelList("Rid in " + DBContext.AssemblyInCondition(RidArr.ToList())).ToList();
                    if ((dt != null && dt.Rows.Count != 0) || (Detail_dt != null && Detail_dt.Rows.Count != 0))
                    {
                        RM.IsSuccess = false;
                        RM.Message = "当前按钮已被使用！";
                    }
                    else
                    {
                        strSql = new StringBuilder(" delete from tblButtonName where Rid in " + DBContext.DataDecision.AssemblyInCondition(RidArr.ToList()));
                        //DataTable dt = DBContext.BusDataSys.GetDataTable(strSql.ToString());
                        if (DBContext.DataDecision.ExecSql(strSql.ToString()) > 0)
                        {
                            //OperateLogHelper.Delete<tblButtonName>(modelList);
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
                StringBuilder strSql = new StringBuilder("select  ButtonName '按钮名称 ', Remark as '备注'  from tblButtonName  where  1=1");
                if (string.IsNullOrEmpty(paramData))
                {
                    string keyword = Request.QueryString["keyword"];
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        strSql.Append(" and (ButtonName like '%" + keyword + "%')");
                    }
                    strSql.Append(" and "+whe);
                }
                else
                {
                    string[] RidArr = paramData.Split('*');
                    strSql.Append(" and  Rid in " + DBContext.DataDecision.AssemblyInCondition(RidArr.ToList()));
                }
                strSql.Append(" order by ButtonName");

                DataTable dt = DBContext.DataDecision.GetDataTable(strSql.ToString());
                string sheetName = "按钮名称";
                //返回路径
                string absoluFilePath = DoExport.ExportDataTableToExcel(dt, sheetName, "按钮名称");

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
