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
    public class RoleController : Controller
    {
        static string whe = string.Empty;
        //
        // GET: /System/Role/

        
        /// <summary>
        /// 角色管理
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [UrlAuthorize("角色管理")]
        public ActionResult Index(int page = 1, int PageSize = 50, string roleName = null)
        {
            ViewData["roleName"] = roleName;

            //取得按钮权限
            List<string> BtnList = FlowHelper.GetBtnAuthorityForPage("角色管理");
            ViewBag.BtnList = BtnList;
            return View();
        }

        public ActionResult GetRoleDataList(int page = 1, int rows = 20, string keyword = null, string sidx = "", string sord = "asc")
        {

            StringBuilder strSql = new StringBuilder(" 1=1");
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                strSql.Append(" and (role_name like '%" + keyword + "%')");
            }
            whe = strSql.ToString();
            string orderBy = " role_name";
            string where = strSql.ToString();
            jqGridData RM = FlowHelper.GetJqGridDataList<sys_role>(where, orderBy, page, rows, sidx, sord);
            return Json(RM, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id = 0)
        {
            sys_role sys_role = BusinessContext.sys_role.GetModel(id); //db.sys_role.Single(s => s.TblRcdId == id);
            if (sys_role == null)
            {
                return HttpNotFound();
            }
            return View(sys_role);
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
        public ActionResult Create(sys_role collection, string IsContinue = "0")
        {
            ReturnMessage RM = new ReturnMessage(false);
            if (ModelState.IsValid)
            {
                try
                {
                    var query = BusinessContext.sys_role.GetModelList(String.Format("role_name='{0}'", collection.role_name));
                     if (query.Count > 0)
                     {
                         RM.Message = "角色名称已被占用";
                     }
                     else
                     {
                         int tblRcdid = BusinessContext.sys_role.Add(collection);
                         RM.IsSuccess = tblRcdid > 0;
                         if (RM.IsSuccess)
                         {

                             collection.TblRcdId = tblRcdid;
                             OperateLogHelper.Create<sys_role>(collection);
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

            sys_role dpt = BusinessContext.sys_role.GetModel(id);
            if (dpt == null)
            {
                return HttpNotFound();
            }
            return View(dpt);
        }
        [HttpPost]
        public ActionResult Edit(sys_role collection)
        {
            ReturnMessage RM = new ReturnMessage(false);
            if (ModelState.IsValid)
            {
                try
                {
                    var query = BusinessContext.sys_role.GetModelList(String.Format("role_name='{0}' and TblRcdId<>{1}", collection.role_name, collection.TblRcdId));
                    if (query.Count > 0)
                    {
                        RM.Message = "角色名称已被占用";
                    }
                    else
                    {
                        sys_role old = BusinessContext.sys_role.GetModel(collection.TblRcdId);
                        RM.IsSuccess = BusinessContext.sys_role.Update(collection);
                        if (RM.IsSuccess)
                        {
                            OperateLogHelper.Edit<sys_role>(collection,old);
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
        [HttpPost]
        public ActionResult DataDel()
        {
            ReturnMessage RM = new ReturnMessage();
            try
            {
                string paramData = Request.Form["paramData"];

                string[] tblRcdidArr = paramData.Split('*');
                StringBuilder strSql = new StringBuilder(" select Role_Id from tblUser_Roles where Role_Id in " + DBContext.AssemblyInCondition(tblRcdidArr.ToList()));
                DataTable dt = DBContext.DataDecision.GetDataTable(strSql.ToString());
                if (dt != null && dt.Rows.Count != 0)
                {
                    RM.IsSuccess = false;
                    RM.Message = "存在用户拥有此角色，无法删除！";
                }
                else
                {
                    strSql = new StringBuilder(" delete from sys_role where TblRcdId in " + DBContext.AssemblyInCondition(tblRcdidArr.ToList()));
                    List<sys_role> modelList = BusinessContext.sys_role.GetModelList("TblRcdId in " + DBContext.AssemblyInCondition(tblRcdidArr.ToList())).ToList();
                    if (DBContext.DataDecision.ExecSql(strSql.ToString()) > 0)
                    {
                        OperateLogHelper.Delete<sys_role>(modelList);

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
                StringBuilder strSql = new StringBuilder("select  role_name '角色名', role_desc as '备注'  from sys_role  where  1=1");
                if (string.IsNullOrEmpty(paramData))
                {
                    string roleName = Request.QueryString["roleName"];
                    if (!string.IsNullOrWhiteSpace(roleName))
                    {
                        strSql.Append(" and (role_name like '%" + roleName + "%')");
                    }
                    strSql.Append(" and "+whe);
                }
                else
                {
                    string[] tblRcdidArr = paramData.Split('*');
                    strSql.Append(" and  TblRcdId in " + DBContext.DataDecision.AssemblyInCondition(tblRcdidArr.ToList()));
                }
                strSql.Append(" order by role_name");

                DataTable dt = DBContext.DataDecision.GetDataTable(strSql.ToString());
                string sheetName = "角色表";
                //返回路径
                string absoluFilePath = DoExport.ExportDataTableToExcel(dt, sheetName, "角色表");

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
