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
using Bitshare.Common.DEncrypt;

namespace Bitshare.DataDecision.Web.Areas.System.Controllers
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
            var query = BusinessContext.tblDepart.GetModelList("1=1");
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
            int CurrentPageIndex = (page != 0 ? (int)page : 1);
            string strSql = "1=1";
            if (!string.IsNullOrWhiteSpace(LoginName))
            {
                strSql += " and (LoginName like '%" + LoginName + "%' or UserName like '%"+LoginName+"%')";
            }
            if (!string.IsNullOrWhiteSpace(Depart))
            {
                strSql += " and (dept_New ='" + Depart + "')";
            }
            string orderBy = "LoginName,UserName";
            string where = strSql.ToString();
            whe = where;
            if (!string.IsNullOrWhiteSpace(sidx))
            {
                orderBy = sidx + " " + sord;
            }
            jqGridData RM = FlowHelper.GetJqGridDataList<View_User_Role>(where, orderBy, CurrentPageIndex, rows, sidx, sord);
            return Json(RM, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 新建
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            List<SelectListItem> deptList = BusinessContext.tblDepart.GetModelList("1=1").Select(p => new SelectListItem { Text = p.dept, Value = p.dept }).ToList();
            deptList.Insert(0, new SelectListItem { Text = "-请选择-", Value = "", Selected = true });
            ViewData["deptList"] = deptList;

            List<SelectListItem> RoleList = BusinessContext.sys_role.GetModelList("1=1").Select(p => new SelectListItem { Text = p.role_name, Value = p.TblRcdId.ToString() }).ToList();
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
                    ///默认密码MD5加密
                    collection.UserPwd = Md5.Encode("123456");
                }
                ///根据登录名称查询是否已经存在,
                var query = BusinessContext.tblUser_Sys.GetModelByLoginName(collection.LoginName);
                if (query != null)
                {
                    RM.Message = "登录名已被占用";
                }
                else
                {
                    ///添加用户,并返回数据库ID,保存操作日志
                    int tblRcdid = BusinessContext.tblUser_Sys.Add(collection);
                    RM.IsSuccess = tblRcdid > 0;
                    if (RM.IsSuccess)
                    {
                        collection.TblRcdId = tblRcdid;
                        tblUser_Roles tblUser_Roles = new Model.tblUser_Roles();
                        tblUser_Roles.Role_Id = collection.DefaultRoleId;
                        tblUser_Roles.LoginName = collection.LoginName;
                        tblUser_Roles.IsDefault = true;
                        BusinessContext.tblUser_Roles.Add(tblUser_Roles);
                        OperateLogHelper.Create<tblUser_Roles>(tblUser_Roles);
                        OperateLogHelper.Create<tblUser_Sys>(collection);
                        ///IsContinue 为1时保存并继续
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
            
            return Json(RM);
        }


        public ActionResult Details(int id = 0)
        {
            tblUser_Sys tbluser_sys = BusinessContext.tblUser_Sys.GetModel(id);
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

            tblUser_Sys dpt = BusinessContext.tblUser_Sys.GetModel(id);
            if (dpt == null)
            {
                return HttpNotFound();
            }
           
           
            if (dpt.DefaultRoleId == 0)
            {
                var query=BusinessContext.tblUser_Roles.GetModelList("LoginName='"+dpt.LoginName+"' and IsDefault=1").OrderBy(p=>p.TblRcdId).Select(p=>p.Role_Id).ToList();
                if(query!=null&&query.Count>0)
                {
                    dpt.DefaultRoleId = query[0];
                }
            }
            List<SelectListItem> deptList = BusinessContext.tblDepart.GetModelList("1=1").Select(p => new SelectListItem { Text = p.dept, Value = p.dept, Selected = dpt.dept_New == p.dept }).ToList();
            deptList.Insert(0, new SelectListItem { Text = "-请选择-", Value = "" });
            ViewData["deptList"] = deptList;
            List<SelectListItem> RoleList = BusinessContext.sys_role.GetModelList("1=1").Select(p => new SelectListItem { Text = p.role_name, Value = p.TblRcdId.ToString(), Selected = dpt.DefaultRoleId == p.TblRcdId }).ToList();
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
                    var query = BusinessContext.tblUser_Sys.GetModelList(String.Format("LoginName='{0}' and TblRcdId<>{1}", collection.LoginName, collection.TblRcdId));
                    if (query.Count > 0)
                    {
                        RM.Message = "登录名已被占用";
                    }
                    else
                    {
                        tblUser_Sys old = BusinessContext.tblUser_Sys.GetModel(collection.TblRcdId);
                        
                        RM.IsSuccess = BusinessContext.tblUser_Sys.Update(collection);
                        if (RM.IsSuccess)
                        {
                            OperateLogHelper.Edit<tblUser_Sys>(collection, old);
                            var list = BusinessContext.tblUser_Roles.GetModelList("LoginName='" + collection.LoginName + "'").Where(p => p.Role_Id == collection.DefaultRoleId).ToList();
                            if (list != null && list.Count > 0)
                            {
                                tblUser_Roles role = list[0];
                                role.IsDefault = true;
                                role.Role_Id = collection.DefaultRoleId;

                                tblUser_Roles old_Roles = BusinessContext.tblUser_Roles.GetModel(role.TblRcdId);
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
            ReturnMessage RM = new ReturnMessage();
            try
            {
                string paramData = Request.Form["paramData"];
                if (!string.IsNullOrWhiteSpace(paramData))
                {
                    string[] tblRcdidArr = paramData.Split('*');
                    List<string> lsSql = new List<string>();
                    //删除用户角色
                    StringBuilder strSql = new StringBuilder(" delete from tblUser_Roles where LoginName in ( select LoginName from tblUser_Sys where TblRcdId in " + DBContext.AssemblyInCondition(tblRcdidArr.ToList()) + ")");
                    lsSql.Add(strSql.ToString());
                    //删除用户管理数据
                    strSql = new StringBuilder(" delete from tblUser_Sys where TblRcdId in " + DBContext.AssemblyInCondition(tblRcdidArr.ToList()));

                    lsSql.Add(strSql.ToString());

                    List<tblUser_Sys> obj = BusinessContext.tblUser_Sys.GetModelList(" TblRcdId in " + DBContext.DataDecision.AssemblyInCondition(tblRcdidArr.ToList()));
                    List<tblUser_Roles> obj_Roles = BusinessContext.tblUser_Roles.GetModelList(" LoginName in ( select LoginName from tblUser_Sys where TblRcdId in " + DBContext.AssemblyInCondition(tblRcdidArr.ToList()) + ")");
                    if (DBContext.DataDecision.ExecTrans(lsSql.ToArray()))
                    {
                        OperateLogHelper.Delete<tblUser_Sys>(obj);
                        OperateLogHelper.Delete<tblUser_Roles>(obj_Roles);
                        RM.IsSuccess = true;
                        RM.Message = "删除成功！";
                    }
                    else
                    {
                        RM.IsSuccess = false;
                        RM.Message = "删除失败！";
                    }
                }
                else
                {
                    RM.Message = "未选择需要删除的！";
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
                    string[] tblRcdidArr = paramData.Split('*');
                    strSql.Append(" and tblrcdid in " + DBContext.DataDecision.AssemblyInCondition(tblRcdidArr.ToList()));
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
                if (user == null || user.TblRcdId <= 0)
                {
                    RM.Message = "数据异常,请重新登录";
                }
                else
                {
                    tblUser_Sys model = new tblUser_Sys();

                    model.UserPwd = Md5.Encode(newPassword);
                    model.TblRcdId = user.TblRcdId;
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
