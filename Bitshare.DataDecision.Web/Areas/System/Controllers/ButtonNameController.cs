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


            PageInfo pager = new PageInfo();
            pager.PageSize = rows;
            pager.CurrentPageIndex = (page != 0 ? (int)page : 1);

            #region 通过存储过程获得数据

            string strSql = "1=1";
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                strSql += " and ButtonName like '%" + keyword + "%'";
            }
          
            string tableName = "tblButtonName";
            string orderBy = "ButtonName";
            if (!string.IsNullOrWhiteSpace(sidx))
            {
                orderBy = sidx + " " + sord;
            }
            int totalCount = 0;
            string queryFields = "*";
            string where = strSql.ToString();
            whe = where;
            DataTable ds = DBContext.DataDecision.QueryPageByProc(tableName, orderBy, out totalCount, queryFields, where, pager.CurrentPageIndex, pager.PageSize);
            List<tblButtonName> result = new List<tblButtonName>();
            result = ds.ToList<tblButtonName>();

            //添加序号
            CommExtension.AddXuHao(result, pager.PageSize, pager.CurrentPageIndex);
            #endregion
            jqGridData RM = new jqGridData();
            RM.page = pager.CurrentPageIndex;
            RM.rows = result;
            RM.total = (totalCount % pager.PageSize == 0 ? totalCount / pager.PageSize : totalCount / pager.PageSize + 1);
            RM.records = totalCount;
            return Json(RM, JsonRequestBehavior.AllowGet);
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
                        var query = BusinessContext.tblButtonName.GetModelList(String.Format("ButtonName='{0}'", collection.ButtonName));
                        if (query.Count > 0)
                        {
                            RM.Message = "按钮名称已被占用";
                        }
                        else
                        {
                            int tblRcdid = BusinessContext.tblButtonName.Add(collection);
                            RM.IsSuccess = tblRcdid > 0;
                            if (RM.IsSuccess)
                            {

                                collection.TblRcdId = tblRcdid;
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

            tblButtonName dpt = BusinessContext.tblButtonName.GetModel(id);
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
                    var query = BusinessContext.tblButtonName.GetModelList(String.Format("ButtonName='{0}' and TblRcdId<>{1}", collection.ButtonName,collection.TblRcdId));
                    if (query.Count > 0)
                    {
                        RM.Message = "按钮名称已被占用";
                    }
                    else
                    {
                        tblButtonName old = BusinessContext.tblButtonName.GetModel(collection.TblRcdId);
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
                    string[] tblRcdidArr = paramData.Split('*');
                    StringBuilder strSql = new StringBuilder(" select ButtonNameId from tblGroupButton where ButtonNameId in(select TblRcdId from tblButtonName where TblRcdId in " + DBContext.DataDecision.AssemblyInCondition(tblRcdidArr.ToList()) + ")");
                    DataTable dt = DBContext.DataDecision.GetDataTable(strSql.ToString());

                    strSql = new StringBuilder(" select ButtonNameId from tblDetailButton where ButtonNameId in(select TblRcdId from tblButtonName where TblRcdId in " + DBContext.DataDecision.AssemblyInCondition(tblRcdidArr.ToList()) + ")");
                    DataTable Detail_dt = DBContext.DataDecision.GetDataTable(strSql.ToString());

                    List<tblButtonName> modelList = BusinessContext.tblButtonName.GetModelList("TblRcdId in " + DBContext.AssemblyInCondition(tblRcdidArr.ToList())).ToList();
                    if ((dt != null && dt.Rows.Count != 0) || (Detail_dt != null && Detail_dt.Rows.Count != 0))
                    {
                        RM.IsSuccess = false;
                        RM.Message = "当前按钮已被使用！";
                    }
                    else
                    {
                        strSql = new StringBuilder(" delete from tblButtonName where TblRcdId in " + DBContext.DataDecision.AssemblyInCondition(tblRcdidArr.ToList()));
                        //DataTable dt = DBContext.BusDataSys.GetDataTable(strSql.ToString());
                        if (DBContext.DataDecision.ExecSql(strSql.ToString()) > 0)
                        {
                            OperateLogHelper.Delete<tblButtonName>(modelList);
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
                    string[] tblRcdidArr = paramData.Split('*');
                    strSql.Append(" and  TblRcdId in " + DBContext.DataDecision.AssemblyInCondition(tblRcdidArr.ToList()));
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
