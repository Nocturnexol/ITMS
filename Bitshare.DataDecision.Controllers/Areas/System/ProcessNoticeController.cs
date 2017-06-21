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
    public class ProcessNoticeController : Controller
    {
        static string wheres = string.Empty;
        //
        // GET: /System/ProcessNotice/

        public ActionResult Index()
        {
            List<string> BtnList = FlowHelper.GetBtnAuthorityForPage("流程消息");
            ViewBag.BtnList = BtnList;
            return View();
        }

        //
        // GET: /System/ProcessManage/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult GetJqGridDataList(int page = 1, int rows = 20,string sidx = null, string sord = "asc")
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
            string tableName = " tblProcessNotice";
            string orderBy = "  ProcessName";
            if (!string.IsNullOrWhiteSpace(sidx))
            {
                orderBy = sidx + " " + sord;
            }
            int totalCount = 0;
            string queryFields = "*";
            string where = strSql.ToString();
            wheres = where;
            DataTable ds = DBContext.DataDecision.QueryPageByProc(tableName, orderBy, out totalCount, queryFields, where, pager.CurrentPageIndex, pager.PageSize);

            List<tblProcessNotice> result = new List<tblProcessNotice>();

            result = ds.ToList<tblProcessNotice>();

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
            GetDropList();
            return View();
        }

        public void GetDropList()
        {
            //流程列表
            List<SelectListItem> ProcessList = new List<SelectListItem>();
            ProcessList = BusinessContext.tblProcessManage.GetModelList("1=1").GroupBy(p => p.ProcessName).Select(p => p.Key).Select(p => new SelectListItem { Text = p, Value = p }).ToList();
            ProcessList.Insert(0, new SelectListItem());
            ViewData["ProcessList"] = ProcessList;
            //角色
            List<SelectListItem> RoleList = new List<SelectListItem>();
            RoleList = BusinessContext.sys_role.GetList(null).GroupBy(p => p.role_name).Select(p => p.Key).Select(p => new SelectListItem { Text = p, Value = p }).ToList();
            RoleList.Insert(0, new SelectListItem());
            ViewData["RoleList"] = RoleList;
            //节点名称
            List<SelectListItem> NodeList = new List<SelectListItem>();
            NodeList = BusinessContext.tblProcessManage.GetModelList("1=1").GroupBy(p => p.NodeName).Select(p => p.Key).Select(p => new SelectListItem { Text = p, Value = p }).ToList();
            NodeList.Insert(0, new SelectListItem());
            ViewData["NodeList"] = NodeList;
        }

        //
        // POST: /System/ProcessManage/Create

        [HttpPost]
        public ActionResult Create(tblProcessNotice tblprocessnotice, string IsContinue = "0")
        {
            ReturnMessage RM = new ReturnMessage(false);
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(Convert.ToString(tblprocessnotice.ProcessName)))
                    {
                        RM.IsSuccess = false;
                        RM.Message = "请填写流程名称";
                        return Json(RM);
                    }
                 
                    if (string.IsNullOrWhiteSpace(Convert.ToString(tblprocessnotice.NodeName)))
                    {
                        RM.IsSuccess = false;
                        RM.Message = "请填写节点名称";
                        return Json(RM);
                    }
                    if (string.IsNullOrWhiteSpace(tblprocessnotice.NoticeRole))
                    {
                        RM.IsSuccess = false;
                        RM.Message = "请填写通知角色";
                        return Json(RM);
                    }


                    int Rid = BusinessContext.tblProcessNotice.Add(tblprocessnotice);

                    RM.IsSuccess = Rid > 0;

                    if (RM.IsSuccess)
                    {
                        tblprocessnotice.Rid = Rid;
                        OperateLogHelper.Create<tblProcessNotice>(tblprocessnotice);
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
            tblProcessNotice model = BusinessContext.tblProcessNotice.GetModel(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            GetDropList();
            return View(model);
        }

        //
        // POST: /System/ProcessManage/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, tblProcessNotice collection)
        {
            ReturnMessage RM = new ReturnMessage(false);
            if (ModelState.IsValid)
            {
                try
                {
                    tblProcessNotice old = BusinessContext.tblProcessNotice.GetModel(id);

                    RM.IsSuccess = BusinessContext.tblProcessNotice.Update(collection);
                    if (RM.IsSuccess)
                    {
                        OperateLogHelper.Edit<tblProcessNotice>(collection, old);
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

                string[] RidArr = paramData.Split('*');
                StringBuilder strSql = new StringBuilder(" delete from tblProcessNotice where Rid in " + DBContext.AssemblyInCondition(RidArr.ToList()));

                List<tblProcessNotice> modelList = BusinessContext.tblProcessNotice.GetModelList("Rid in " + DBContext.AssemblyInCondition(RidArr.ToList())).ToList();
                if (DBContext.DataDecision.ExecSql(strSql.ToString()) > 0)
                {
                    RM.IsSuccess = true;
                    RM.Message = "删除成功！";
                    OperateLogHelper.Delete<tblProcessNotice>(modelList);
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
                StringBuilder strSql = new StringBuilder("select  ProcessName as '流程名称',  NodeName as '节点名称',NoticeRole as '通知角色', Remark  as '备注'  from tblProcessNotice  where  1=1");
                if (string.IsNullOrEmpty(paramData))
                {
                    string ProcessName = Request.Form["ProcessName"];
                    if (!string.IsNullOrWhiteSpace(ProcessName))
                    {
                        strSql.Append(" and (ProcessName like '%" + ProcessName + "%')");
                    }
                    strSql.Append(" and " + wheres);
                }
                else
                {
                    string[] RidArr = paramData.Split('*');
                    strSql.Append(" and  Rid in " + DBContext.DataDecision.AssemblyInCondition(RidArr.ToList()));
                }
                strSql.Append(" order by ProcessName");

                DataTable dt = DBContext.DataDecision.GetDataTable(strSql.ToString());
                string sheetName = "流程消息设置表";
                //返回路径
                string absoluFilePath = DoExport.ExportDataTableToExcel(dt, sheetName, "流程消息设置表");

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
        public ActionResult Delete(int id, tblProcessNotice collection)
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
