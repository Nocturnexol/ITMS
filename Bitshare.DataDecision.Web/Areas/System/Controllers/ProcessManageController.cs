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
    public class ProcessManageController : Controller
    {
        static string whe = string.Empty;
        //
        // GET: /System/ProcessManage/

        public ActionResult Index()
        {
            List<string> BtnList = FlowHelper.GetBtnAuthorityForPage("流程定义");
            ViewBag.BtnList = BtnList;
            SetDropList();
            return View();
        }

        //
        // GET: /System/ProcessManage/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult GetJqGridDataList(int page = 1, int rows = 20, string keyword = null, string sidx = null, string sord = "asc")
        {
            PageInfo pager = new PageInfo();
            pager.PageSize = rows;
            pager.CurrentPageIndex = (page != 0 ? (int)page : 1);

            StringBuilder strSql = new StringBuilder("1=1");
            string ProcessName = Request.Form["ProcessName"];
            if (!string.IsNullOrWhiteSpace(ProcessName))
            {
                strSql.AppendFormat(" and ProcessName like '%{0}%'", ProcessName);
            }

            #region 通过存储过程获得数据
            string tableName = "tblProcessManage";
            string orderBy = " ProcessNum,NodeNum";
            if (!string.IsNullOrWhiteSpace(sidx))
            {
                orderBy = sidx + " " + sord;
            }
            int totalCount = 0;
            string queryFields = "*";
            string where = strSql.ToString();
            whe = where;
            DataTable ds = DBContext.DataDecision.QueryPageByProc(tableName, orderBy, out totalCount, queryFields, where, pager.CurrentPageIndex, pager.PageSize);

            List<tblProcessManage> result = new List<tblProcessManage>();

            result = ds.ToList<tblProcessManage>();

            #endregion
            jqGridData RM = new jqGridData();
            RM.page = pager.CurrentPageIndex;
            RM.rows = result;
            RM.total = (totalCount % pager.PageSize == 0 ? totalCount / pager.PageSize : totalCount / pager.PageSize + 1);
            RM.records = totalCount;
            return Json(RM, JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /System/ProcessManage/Create

        public ActionResult Create()
        {
            SetDropList();
            return View();
        }

        public void SetDropList()
        {
            //流程列表
            List<SelectListItem> ProcessList = new List<SelectListItem>();
            ProcessList = BusinessContext.tblProcessManage.GetModelList("1=1").GroupBy(p => p.ProcessName).Select(p => p.Key).Select(p => new SelectListItem { Text = p, Value = p }).ToList();
            ProcessList.Insert(0, new SelectListItem());
            ViewData["ProcessList"] = ProcessList;
            //角色
            List<SelectListItem> RoleList = new List<SelectListItem>();
            RoleList = BusinessContext.sys_role.GetModelList("1=1").GroupBy(p => p.role_name).Select(p => p.Key).Select(p => new SelectListItem { Text = p, Value = p }).ToList();
            RoleList.Insert(0, new SelectListItem());
            ViewData["RoleList"] = RoleList;
            //状态
            List<SelectListItem> StateList = new List<SelectListItem>();
            StateList.Insert(0, new SelectListItem { Text = "开始", Value = "开始", Selected = true });
            StateList.Insert(0, new SelectListItem { Text = "正常", Value = "正常" });
            StateList.Insert(0, new SelectListItem { Text = "结束", Value = "结束" });
            ViewData["StateList"] = StateList;

        }

        //
        // POST: /System/ProcessManage/Create

        [HttpPost]
        public ActionResult Create(tblProcessManage tblprocessmanage, string IsContinue = "0")
        {

            ReturnMessage RM = new ReturnMessage(false);
            if (tblprocessmanage.ProcessNum==0)
            {
                RM.IsSuccess = false;
                RM.Message = "请填写正确流程编号";
                return Json(RM);
            }
            if (tblprocessmanage.NodeNum==0)
            {
                RM.IsSuccess = false;
                RM.Message = "请填写正确节点编号";
                return Json(RM);
            }

           
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(Convert.ToString(tblprocessmanage.ProcessNum)))
                    {
                        RM.IsSuccess = false;
                        RM.Message = "请填写流程编号";
                        return Json(RM);
                    }
                    if (string.IsNullOrWhiteSpace(tblprocessmanage.ProcessName))
                    {
                        RM.IsSuccess = false;
                        RM.Message = "请填写流程名称";
                        return Json(RM);
                    }
                    if (string.IsNullOrWhiteSpace(Convert.ToString(tblprocessmanage.NodeNum)))
                    {
                        RM.IsSuccess = false;
                        RM.Message = "请填写节点编号";
                        return Json(RM);
                    }
                    if (string.IsNullOrWhiteSpace(tblprocessmanage.NodeName))
                    {
                        RM.IsSuccess = false;
                        RM.Message = "请填写节点名称";
                        return Json(RM);
                    }
                    if (string.IsNullOrWhiteSpace(tblprocessmanage.NodeNameNext))
                    {
                        RM.IsSuccess = false;
                        RM.Message = "请填写下一节点";
                        return Json(RM);
                    }
                    
                    int tblRcdid = BusinessContext.tblProcessManage.Add(tblprocessmanage);

                    RM.IsSuccess = tblRcdid > 0;
                    
                    if (RM.IsSuccess)
                    {
                        tblprocessmanage.TblRcdId = tblRcdid;
                        OperateLogHelper.Create<tblProcessManage>(tblprocessmanage);
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

        //
        // GET: /System/ProcessManage/Edit/5

        public ActionResult Edit(int id)
        {
            tblProcessManage model = BusinessContext.tblProcessManage.GetModel(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            SetDropList();
            return View(model);
        }

        //
        // POST: /System/ProcessManage/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, tblProcessManage collection)
        {
            ReturnMessage RM = new ReturnMessage(false);
            if (ModelState.IsValid)
            {
                try
                {
                    tblProcessManage old = BusinessContext.tblProcessManage.GetModel(id);

                    RM.IsSuccess = BusinessContext.tblProcessManage.Update(collection);
                    if (RM.IsSuccess)
                    {
                        OperateLogHelper.Edit<tblProcessManage>(collection, old);
                    }

                }
                catch (Exception ex)
                {
                    RM.Message = ex.Message;
                }
            }
            return Json(RM);
        }
        public ActionResult DataDel()
        {
            ReturnMessage RM = new ReturnMessage();
            try
            {
                string paramData = Request.Form["paramData"];

                string[] tblRcdidArr = paramData.Split('*');
                StringBuilder strSql = new StringBuilder(" delete from tblProcessManage where TblRcdId in " + DBContext.AssemblyInCondition(tblRcdidArr.ToList()));

                List<tblProcessManage> modelList = BusinessContext.tblProcessManage.GetModelList("TblRcdId in " + DBContext.AssemblyInCondition(tblRcdidArr.ToList())).ToList();
                if (DBContext.DataDecision.ExecSql(strSql.ToString()) > 0)
                {
                    RM.IsSuccess = true;
                    RM.Message = "删除成功！";
                    OperateLogHelper.Delete<tblProcessManage>(modelList);
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

        [HttpPost]
        public ActionResult DeriveData()
        {
            ReturnMessage RM = new ReturnMessage();
            try
            {
                string paramData = Request.Form["paramData"];
                StringBuilder strSql = new StringBuilder("select ProcessNum as '流程编号 ', ProcessName as '流程名称', NodeNum as '节点编号', NodeName as '节点名称', NodeNameNext as '下一节点', NodeType as '节点状态', ExecutorRole as '执行角色', ReMark  as '备注'  from tblProcessManage  where  1=1");
                if (string.IsNullOrEmpty(paramData))
                {
                    string keyword = Request.QueryString["keyword"];
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        strSql.Append(" and (ProcessName like '%" + keyword + "%')");
                    }
                    strSql.Append(" and " + whe);
                }
                else
                {
                    string[] tblRcdidArr = paramData.Split('*');
                    strSql.Append(" and  TblRcdId in " + DBContext.DataDecision.AssemblyInCondition(tblRcdidArr.ToList()));
                }
                strSql.Append(" order by ProcessNum,NodeNum");

                DataTable dt = DBContext.DataDecision.GetDataTable(strSql.ToString());
                string sheetName = "流程定义表";
                //返回路径
                string absoluFilePath = DoExport.ExportDataTableToExcel(dt, sheetName, "流程定义表");

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
        //
        // GET: /System/ProcessManage/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /System/ProcessManage/Delete/5

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
