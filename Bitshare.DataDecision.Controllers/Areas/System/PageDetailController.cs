using Bitshare.Common;
using Bitshare.DataDecision.Common;
using Bitshare.DataDecision.Model;
using Bitshare.DataDecision.Service.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace Bitshare.DataDecision.Controllers.Areas.System
{
    public class PageDetailController : Controller
    {
        //
        // GET: /System/PageDetail/
        [UrlAuthorize("明细权限")]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index(int page = 1, int PageSize = 50, string keyword = null)
        {
            //取得按钮权限
            List<string> BtnList = FlowHelper.GetBtnAuthorityForPage("明细权限");
            ViewBag.BtnList = BtnList;
            #region 通过存储过程获得数据
            //PageInfo pager = new PageInfo();
            //pager.PageSize = 50;
            //pager.CurrentPageIndex = (page != null ? (int)page : 1);

            //StringBuilder strSql = new StringBuilder(" 1=1");
            //if (!string.IsNullOrWhiteSpace(keyword))
            //{
            //    strSql.Append(" and (ModelName like '%" + keyword + "%' or PageName like '%" + keyword + "%')");
            //}
            //string tableName = "tblPageDetail";
            //string orderBy = "ModelName,PageName,DetailName";
            //int totalCount = 0;
            //string queryFields = "Rid, ModelName,PageName,DetailName,Remark";
            //string where = strSql.ToString();
            //DataTable ds = DBContext.PTSM.QueryPageByProc(tableName, orderBy, out totalCount, queryFields, where, pager.CurrentPageIndex, pager.PageSize);

            //List<tblPageDetail> result = new List<tblPageDetail>();
            //result = ds.ToList<tblPageDetail>();
            ////添加序号
            //CommExtension.AddXuHao(result, pager.PageSize, pager.CurrentPageIndex);

            //pager.RecordCount = totalCount;
            //IEnumerable<tblPageDetail> info2 = result;
            //PagerQuery<PageInfo, IEnumerable<tblPageDetail>> query = new PagerQuery<PageInfo, IEnumerable<tblPageDetail>>(pager, info2);

            #endregion
            return View();
        }
        public ActionResult GetPageDetailList(string keyword = null, string Depart = null, int page = 1, string sidx = "", string sord = "asc")
        {
            //取得按钮权限
            List<string> BtnList = FlowHelper.GetBtnAuthorityForPage("明细权限");
            ViewBag.BtnList = BtnList;
            #region 通过存储过程获得数据
            PageInfo pager = new PageInfo();
            pager.PageSize = 50;
            pager.CurrentPageIndex = Math.Max(1, page);

            StringBuilder strSql = new StringBuilder(" 1=1");
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                strSql.Append(" and (ModelName like '%" + keyword + "%' or PageName like '%" + keyword + "%')");
            }
            string tableName = "tblPageDetail";
            string orderBy = "ModelName,PageName,DetailName";
            if (!string.IsNullOrWhiteSpace(sidx))
            {
                orderBy = sidx + " " + sord;
            }
            int totalCount = 0;
            string queryFields = "Rid, ModelName,PageName,DetailName,Remark";
            string where = strSql.ToString();
            DataTable ds = DBContext.DataDecision.QueryPageByProc(tableName, orderBy, out totalCount, queryFields, where, pager.CurrentPageIndex, pager.PageSize);

            List<tblPageDetail> result = new List<tblPageDetail>();
            result = ds.ToList<tblPageDetail>();
            #endregion
            jqGridData RM = new jqGridData();
            RM.page = pager.CurrentPageIndex;
            RM.rows = result;
            RM.total = (totalCount % pager.PageSize == 0 ? totalCount / pager.PageSize : totalCount / pager.PageSize + 1);
            RM.records = totalCount;
            return Json(RM, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /System/PageDetail/Create

        [HttpPost]
        public ActionResult Create(tblPageDetail obj, string IsContinue = "0")
        {
            ReturnMessage RM = new ReturnMessage(false);
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(obj.ModelName) || obj.ModelName == "" || obj.ModelName == "null" || obj.ModelName == null)
                    {
                        RM.IsSuccess = false;
                        throw new Exception("模块名称不能为空");
                    }
                    var list = BusinessContext.tblPageDetail.GetModelList("ModelName='" + obj.ModelName + "'");
                    if (list != null && list.Count() > 0)
                    {
                        var right = list.FirstOrDefault(p => p.PageName == obj.PageName && p.DetailName == obj.DetailName);
                        if (right != null)
                        {
                            throw new Exception("明细名称重复");
                        }
                    }
                    int Rid = BusinessContext.tblPageDetail.Add(obj);
                    RM.IsSuccess = Rid > 0;
                    if (RM.IsSuccess)
                    {

                        obj.Rid = Rid;
                        OperateLogHelper.Create<tblPageDetail>(obj);
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
                catch (Exception ex)
                {
                    RM.Message = ex.Message;
                }
            }
            return Json(RM);
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {

            tblPageDetail dpt = BusinessContext.tblPageDetail.GetModel(id);
            if (dpt == null)
            {
                return HttpNotFound();
            }
            return View(dpt);

        }
        [HttpPost]
        public ActionResult Edit(tblPageDetail collection)
        {
            ReturnMessage RM = new ReturnMessage(false);
            if (ModelState.IsValid)
            {
                try
                {
                    tblPageDetail old = BusinessContext.tblPageDetail.GetModel(collection.Rid);
                    RM.IsSuccess = BusinessContext.tblPageDetail.Update(collection);
                    if (RM.IsSuccess)
                    {
                        OperateLogHelper.Edit<tblPageDetail>(collection, old);

                    }
                }
                catch (Exception ex)
                {
                    RM.Message = ex.Message;
                }
            }
            return Json(RM);
        }
        [HttpPost]
        public ActionResult DeriveData()
        {
            ReturnMessage RM = new ReturnMessage();
            try
            {
                string paramData = Request.Form["paramData"];
                string[] RidArr = paramData.Split('*');
                StringBuilder strSql = new StringBuilder("select ModelName as '模块名称', PageName as '页面名称',DetailName as '明细名称', Remark as '备注' from tblPageDetail where 1=1 " );
                if (string.IsNullOrEmpty(paramData))
                {
                    string keyword = Request["keyword"];
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        strSql.Append(" and (ModelName like '%" + keyword + "%' or PageName like '%" + keyword + "%')");
                    }
                }
                else
                {
                    
                    strSql.Append(" and  Rid in " + DBContext.DataDecision.AssemblyInCondition(RidArr.ToList()));
                }
                strSql.Append(" order by ModelName,PageName,DetailName");
                DataTable dt = DBContext.DataDecision.GetDataTable(strSql.ToString());


                string sheetName = "明细权限管理";
                //返回路径
                string absoluFilePath = DoExport.ExportDataTableToExcel(dt, sheetName, "明细权限管理");

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
        [HttpPost]
        public ActionResult DataDel()
        {
            ReturnMessage RM = new ReturnMessage();
            try
            {
                string paramData = Request.Form["paramData"];
                string[] RidArr = paramData.Split('*');
                List<string> lsSql = new List<string>();
                // 
                StringBuilder strSql = new StringBuilder(" delete from tblPageDetail where Rid in " + DBContext.DataDecision.AssemblyInCondition(RidArr.ToList()));
                lsSql.Add(strSql.ToString());
                List<tblPageDetail> P_modelList = BusinessContext.tblPageDetail.GetModelList("Rid in " + DBContext.AssemblyInCondition(RidArr.ToList())).ToList();

                strSql = new StringBuilder(" delete from tblDetailButton where Detail_NameId in " + DBContext.DataDecision.AssemblyInCondition(RidArr.ToList()));
                lsSql.Add(strSql.ToString());
                List<tblDetailButton> B_modelList = BusinessContext.tblDetailButton.GetModelList("Detail_NameId in " + DBContext.DataDecision.AssemblyInCondition(RidArr.ToList())).ToList();
                if (DBContext.DataDecision.ExecTrans(lsSql.ToArray()))
                {

                    OperateLogHelper.Delete<tblPageDetail>(P_modelList);
                    OperateLogHelper.Delete<tblDetailButton>(B_modelList);

                    RM.IsSuccess = true;
                    RM.Message = "删除成功！";
                }
                else
                {
                    RM.IsSuccess = false;
                    RM.Message = "删除失败！";
                }
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
