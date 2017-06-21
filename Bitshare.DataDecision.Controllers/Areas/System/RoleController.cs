using Bitshare.DataDecision.Common;
using Bitshare.DataDecision.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Bitshare.DataDecision.Service.DTO;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;


namespace Bitshare.DataDecision.Controllers.Areas.System
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
            if (string.IsNullOrEmpty(sidx))
                sidx = "Rid";
            var pager = new PageInfo
            {
                PageSize = rows,
                CurrentPageIndex = page > 0 ? page : 1
            };

            List<IMongoQuery> queryList = new List<IMongoQuery>();



            if (!string.IsNullOrWhiteSpace(keyword))
            {
                queryList.Add(Query<sys_role>.Matches(t => t.role_name,
                    new BsonRegularExpression(new Regex(keyword, RegexOptions.IgnoreCase))));
            }
            var where = queryList.Any() ? Query.And(queryList) : null;

            long totalCount;
            var rm = new jqGridData
            {
                page = pager.CurrentPageIndex,
                rows = BusinessContext.sys_role.GetList(out totalCount, page, rows, where, sidx, sord),
                total = (int)(totalCount % pager.PageSize == 0 ? totalCount / pager.PageSize : totalCount / pager.PageSize + 1),
                records = (int)totalCount
            };
            return Json(rm, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id = 0)
        {
            sys_role sys_role = BusinessContext.sys_role.Get(Query<sys_role>.EQ(t => t.Rid, id)); //db.sys_role.Single(s => s.Rid == id);
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
                    var query =
                        BusinessContext.sys_role.GetList(Query<sys_role>.EQ(t => t.role_name, collection.role_name));
                     if (query.Count > 0)
                     {
                         RM.Message = "角色名称已被占用";
                     }
                     else
                     {
                         var res = BusinessContext.sys_role.Add(collection);
                         RM.IsSuccess = res;
                         if (RM.IsSuccess)
                         {

                             //collection.Rid = Rid;
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

            sys_role dpt = BusinessContext.sys_role.Get(Query<sys_role>.EQ(t => t.Rid, id));
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
                    var q = Query.And(Query<sys_role>.EQ(t => t.role_name, collection.role_name),
                        Query<sys_role>.NE(t => t.Rid, collection.Rid));
                    var query = BusinessContext.sys_role.GetList(q);
                    if (query.Count > 0)
                    {
                        RM.Message = "角色名称已被占用";
                    }
                    else
                    {
                        sys_role old = BusinessContext.sys_role.Get(Query<sys_role>.EQ(t => t.Rid, collection.Rid));
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

                string[] RidArr = paramData.Split('*');
                StringBuilder strSql = new StringBuilder(" select Role_Id from tblUser_Roles where Role_Id in " + DBContext.AssemblyInCondition(RidArr.ToList()));
                DataTable dt = DBContext.DataDecision.GetDataTable(strSql.ToString());
                if (dt != null && dt.Rows.Count != 0)
                {
                    RM.IsSuccess = false;
                    RM.Message = "存在用户拥有此角色，无法删除！";
                }
                else
                {
                    strSql = new StringBuilder(" delete from sys_role where Rid in " + DBContext.AssemblyInCondition(RidArr.ToList()));
                    List<sys_role> modelList =
                        BusinessContext.sys_role.GetList(Query<sys_role>.In(t => t.Rid, RidArr.Select(int.Parse)));
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
                    string[] RidArr = paramData.Split('*');
                    strSql.Append(" and  Rid in " + DBContext.DataDecision.AssemblyInCondition(RidArr.ToList()));
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
