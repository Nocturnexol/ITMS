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
using Bitshare.Common;
using Bitshare.DataDecision.Service.DTO;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Bitshare.DataDecision.Controllers.Areas.System
{
    public class SysUserController : Controller
    {
        static string whe = string.Empty;
        //
        // GET: /System/SysUser/
        [UrlAuthorize("用户管理")]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index(string keyword = null, string Depart = null, int page = 1)
        {
            //按钮权限
            List<string> BtnList = FlowHelper.GetBtnAuthorityForPage("用户管理");
            ViewBag.BtnList = BtnList;
            var query = BusinessContext.tblDepart.GetList();
            List<SelectListItem> DepartList = query.GroupBy(p => p.dept).Select(p => new SelectListItem { Text = p.Key, Value = p.Key }).ToList();
            DepartList.Insert(0, new SelectListItem {Text="-请选择-",Value="",Selected=true });
            ViewBag.DepartList = DepartList;

            return View();
        }

        /// <summary>
        /// 获取用户列表，用于jqGrid展示
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AntiSqlInject]
        public ActionResult GetSysUserDataList(string LoginName = null, string Depart = null, int page = 1, int rows = 20, string sidx = "", string sord = "asc")
        {
            if (string.IsNullOrEmpty(sidx))
                sidx = "Rid";
            var pager = new PageInfo
            {
                PageSize = rows,
                CurrentPageIndex = page > 0 ? page : 1
            };

            List<IMongoQuery> queryList = new List<IMongoQuery>();

            if (!string.IsNullOrWhiteSpace(LoginName))
            {
                queryList.Add(
                    Query.Or(
                        Query<tblUser_Sys>.Matches(t => t.LoginName,
                            new BsonRegularExpression(new Regex(LoginName, RegexOptions.IgnoreCase))),
                        Query<tblUser_Sys>.Matches(t => t.UserName,
                            new BsonRegularExpression(new Regex(LoginName, RegexOptions.IgnoreCase)))));
            }
            if (!string.IsNullOrWhiteSpace(Depart))
            {
                queryList.Add(Query<tblUser_Sys>.EQ(t => t.dept_New, Depart));
            }
            var where = queryList.Any() ? Query.And(queryList) : null;

            long totalCount;
            var rm = new jqGridData
            {
                page = pager.CurrentPageIndex,
                rows = BusinessContext.tblUser_Sys.GetList(out totalCount, page, rows, where, sidx, sord),
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
            List<SelectListItem> deptList = BusinessContext.tblDepart.GetList().Select(p => new SelectListItem { Text = p.dept, Value = p.dept }).ToList();
            deptList.Insert(0, new SelectListItem { Text = "-请选择-", Value = "", Selected = true });
            ViewData["deptList"] = deptList;

            List<SelectListItem> RoleList = BusinessContext.sys_role.GetList().Select(p => new SelectListItem { Text = p.role_name, Value = p.Rid.ToString() }).ToList();
            RoleList.Insert(0, new SelectListItem { Text = "-请选择-", Value = "", Selected = true });
            
            ViewData["RoleList"] = RoleList;
            return View();
        }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="collection">用户表单</param>
        /// <param name="IsContinue">是否保存并继续，0：否，1：是</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(tblUser_Sys collection, string IsContinue = "0")
        {
            ReturnMessage RM = new ReturnMessage(false);

            try
            {
                if (collection.UserPwd == "" || string.IsNullOrEmpty(collection.UserPwd))
                {
                    //默认密码MD5加密
                    collection.UserPwd = Md5.Encode("123456");
                }
                //根据登录名称查询是否已经存在,
                var query =
                    BusinessContext.tblUser_Sys.Get(Query<tblUser_Sys>.EQ(t => t.LoginName, collection.LoginName));
                if (query != null)
                {
                    RM.Message = "登录名已被占用";
                }
                else
                {
                    //添加用户,并返回数据库ID,保存操作日志
                   var res = BusinessContext.tblUser_Sys.Add(collection);
                   RM.IsSuccess = res;
                    if (RM.IsSuccess)
                    {
                        //collection.Rid = Rid;
                        tblUser_Roles tblUser_Roles = new tblUser_Roles();
                        tblUser_Roles.Role_Id = collection.DefaultRoleId;
                        tblUser_Roles.LoginName = collection.LoginName;
                        tblUser_Roles.IsDefault = true;
                        BusinessContext.tblUser_Roles.Add(tblUser_Roles);
                        OperateLogHelper.Create<tblUser_Roles>(tblUser_Roles);
                        OperateLogHelper.Create<tblUser_Sys>(collection);
                        //IsContinue 为1时保存并继续
                        RM.IsContinue = IsContinue == "1";
                    }
                }
            }
            catch (Exception ex)
            {
                RM.Message = ex.Message;
            }
            
            return Json(RM);
        }


        public ActionResult Details(int id = 0)
        {
            tblUser_Sys tbluser_sys = BusinessContext.tblUser_Sys.Get(Query<tblUser_Sys>.EQ(t => t.Rid, id));
            if (tbluser_sys == null)
            {
                return HttpNotFound();
            }
            return View(tbluser_sys);
        }


        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {

            tblUser_Sys dpt = BusinessContext.tblUser_Sys.Get(Query<tblUser_Sys>.EQ(t => t.Rid, id));
            if (dpt == null)
            {
                return HttpNotFound();
            }
           
           
            if (dpt.DefaultRoleId == 0)
            {
                var q = Query.And(Query<tblUser_Roles>.EQ(t => t.LoginName, dpt.LoginName),
                    Query<tblUser_Roles>.EQ(t => t.IsDefault, true));
                var query=BusinessContext.tblUser_Roles.GetList(q).OrderBy(p=>p.Rid).Select(p=>p.Role_Id).ToList();
                if(query!=null&&query.Count>0)
                {
                    dpt.DefaultRoleId = query[0];
                }
            }
            List<SelectListItem> deptList = BusinessContext.tblDepart.GetList().Select(p => new SelectListItem { Text = p.dept, Value = p.dept, Selected = dpt.dept_New == p.dept }).ToList();
            deptList.Insert(0, new SelectListItem { Text = "-请选择-", Value = "" });
            ViewData["deptList"] = deptList;
            List<SelectListItem> RoleList = BusinessContext.sys_role.GetList().Select(p => new SelectListItem { Text = p.role_name, Value = p.Rid.ToString(), Selected = dpt.DefaultRoleId == p.Rid }).ToList();
            RoleList.Insert(0, new SelectListItem { Text = "-请选择-", Value = "" });
            ViewData["RoleList"] = RoleList;

            return View(dpt);
       
        }
        [HttpPost]
        public ActionResult Edit(tblUser_Sys collection)
        {
            ReturnMessage RM = new ReturnMessage(false);
            if (ModelState.IsValid)
            {
                try
                {
                    var query =
                        BusinessContext.tblUser_Sys.GetList(
                            Query.And(Query<tblUser_Sys>.EQ(t => t.LoginName, collection.LoginName),
                                Query<tblUser_Sys>.NE(t => t.Rid, collection.Rid)));
                    if (query.Count > 0)
                    {
                        RM.Message = "登录名已被占用";
                    }
                    else
                    {
                        tblUser_Sys old =
                            BusinessContext.tblUser_Sys.Get(Query<tblUser_Sys>.EQ(t => t.Rid, collection.Rid));
                        
                        RM.IsSuccess = BusinessContext.tblUser_Sys.Update(collection);
                        if (RM.IsSuccess)
                        {
                            OperateLogHelper.Edit<tblUser_Sys>(collection, old);
                            var list =
                                BusinessContext.tblUser_Roles.GetList(Query<tblUser_Roles>.EQ(t => t.LoginName,
                                    collection.LoginName)).Where(p => p.Role_Id == collection.DefaultRoleId).ToList();
                            if (list != null && list.Count > 0)
                            {
                                tblUser_Roles role = list[0];
                                role.IsDefault = true;
                                role.Role_Id = collection.DefaultRoleId;

                                tblUser_Roles old_Roles = BusinessContext.tblUser_Roles.Get(Query<tblUser_Roles>.EQ(t => t.Rid, role.Rid));
                                BusinessContext.tblUser_Roles.Update(role);
                                OperateLogHelper.Edit<tblUser_Roles>(role, old_Roles);
                            }
                            else
                            {
                                tblUser_Roles role = new tblUser_Roles();
                                role.IsDefault = true;
                                role.LoginName = collection.LoginName;
                                role.Role_Id = collection.DefaultRoleId;
                                BusinessContext.tblUser_Roles.Add(role);
                                OperateLogHelper.Create<tblUser_Roles>(role);
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
        /// 删除
        /// </summary>
        /// <returns></returns>
        public ActionResult DataDel()
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                string paramData = Request.Form["paramData"];
                if (!string.IsNullOrWhiteSpace(paramData))
                {
                    string[] ridArr = paramData.Split('*');
                    //List<string> lsSql = new List<string>();
                    //删除用户角色
                    //StringBuilder strSql = new StringBuilder(" delete from tblUser_Roles where LoginName in ( select LoginName from tblUser_Sys where Rid in " + DBContext.AssemblyInCondition(RidArr.ToList()) + ")");
                    //lsSql.Add(strSql.ToString());
                    var loginNames =
                        BusinessContext.tblUser_Sys.GetList(Query<tblUser_Sys>.In(t => t.Rid, ridArr.Select(int.Parse)))
                            .Select(t => t.LoginName);
                    var roleRids =
                        BusinessContext.tblUser_Roles.GetList(Query<tblUser_Roles>.In(t => t.LoginName, loginNames))
                            .Select(t => t.Rid)
                            .ToList();
                   var flag =  BusinessContext.tblUser_Roles.Delete(roleRids);

                    //删除用户管理数据
                    //strSql = new StringBuilder(" delete from tblUser_Sys where Rid in " + DBContext.AssemblyInCondition(RidArr.ToList()));
                    //lsSql.Add(strSql.ToString());

                    flag = flag && BusinessContext.tblUser_Sys.Delete(ridArr.Select(int.Parse).ToList());

                    //List<tblUser_Sys> obj =
                    //    BusinessContext.tblUser_Sys.GetList(Query<tblUser_Sys>.In(t => t.Rid, RidArr.Select(int.Parse)));
                    //List<tblUser_Roles> obj_Roles = BusinessContext.tblUser_Roles.GetModelList(" LoginName in ( select LoginName from tblUser_Sys where Rid in " + DBContext.AssemblyInCondition(RidArr.ToList()) + ")");
                    if (flag)
                    {
                        //OperateLogHelper.Delete<tblUser_Sys>(obj);
                        //OperateLogHelper.Delete<tblUser_Roles>(obj_Roles);
                        rm.IsSuccess = true;
                        rm.Message = "删除成功！";
                    }
                    else
                    {
                        rm.IsSuccess = false;
                        rm.Message = "删除失败！";
                    }
                }
                else
                {
                    rm.Message = "未选择需要删除的！";
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
        [AntiSqlInject]
        public ActionResult DeriveData()
        {
            ReturnMessage RM = new ReturnMessage();
            try
            {
                string paramData = Request.Form["paramData"];
                StringBuilder strSql = new StringBuilder("select LoginName as  '登录名',UserName  as '用户名',dept_New  as '所属部门', 'role_name' as '角色',Remark '备注' from View_User_Role where 1=1");
                if (string.IsNullOrEmpty(paramData))
                {
                    string keyword = Request["keyword"];
                    string Depart = Request["Depart"];
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        strSql.Append(" and (LoginName like '%" + keyword + "%' or UserName like '%" + keyword + "%')");
                    }
                    if (!string.IsNullOrWhiteSpace(Depart))
                    {
                        strSql.Append(" and (dept_New ='" + Depart + "')");
                    }
                    strSql.Append(" and " + whe);
                }
                else
                {
                    string[] RidArr = paramData.Split('*');
                    strSql.Append(" and Rid in " + DBContext.DataDecision.AssemblyInCondition(RidArr.ToList()));
                }
                strSql.Append(" order by LoginName,UserName  ");
                DataTable dt = DBContext.DataDecision.GetDataTable(strSql.ToString());


                DataTable User_Roles_dt = DBContext.DataDecision.GetDataTable("select distinct LoginName,Role_Name from dbo.View_User_Roles where LoginName is not null and LoginName<>'' and  Role_Name is not null and Role_Name<>'' order by LoginName,Role_Name");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string LoginName = Convert.ToString(dt.Rows[i]["登录名"]);
                    string Role_NameList = "";
                    DataRow[] datarow = User_Roles_dt.Select("LoginName='" + LoginName + "'");
                    for (int j = 0; j < datarow.Length; j++)
                    {
                        Role_NameList += Convert.ToString(datarow[j]["Role_Name"]) + ",";
                    }
                    dt.Rows[i]["角色"] = Role_NameList;
                }
                string sheetName = "用户表";
                //返回路径
                string absoluFilePath = DoExport.ExportDataTableToExcel(dt, sheetName, "用户表");
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

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string newPassword)
        {
            ReturnMessage RM = new ReturnMessage(false);
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                RM.Message = "新密码不能为空";
            }
            else
            {
                tblUser_Sys user = CurrentHelper.CurrentUser.User;
                if (user == null || user.Rid <= 0)
                {
                    RM.Message = "数据异常,请重新登录";
                }
                else
                {
                    tblUser_Sys model = new tblUser_Sys();

                    model.UserPwd = Md5.Encode(newPassword);
                    model.Rid = user.Rid;
                    model.LoginName = user.LoginName;
                    model.UserName = user.UserName;
                    model.Remark = user.Remark;
                    model.RoleFlag = user.RoleFlag;
                    model.dept = user.dept;
                    model.dept_New = user.dept_New;
                    model.DefaultRoleId = user.DefaultRoleId;
                    model.EnglishName = user.EnglishName;
                    model.PassWord = Md5.Encode(newPassword);
                    model.UserMark = user.UserMark;

                    BusinessContext.tblUser_Sys.Update(model);
                    OperateLogHelper.Edit<tblUser_Sys>(model, user);
                    RM.IsSuccess = true;
                }
            }

            return Json(RM);
        }


    }
}
