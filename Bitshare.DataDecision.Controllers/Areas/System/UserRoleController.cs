using Bitshare.DataDecision.Common;
using Bitshare.DataDecision.Model;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MongoDB.Driver.Builders;

namespace Bitshare.DataDecision.Controllers.Areas.System
{
    public class UserRoleController : Controller
    {
        //
        // GET: /System/UserRole/
        /// <summary>
        /// 人员角色设置
        /// </summary>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        public ActionResult Index(string LoginName)
        {
            ViewBag.LoginName = LoginName;

            List<sys_role> roleIList = BusinessContext.sys_role.GetList(); 
            //当前用户的所有角色
            List<tblUser_Roles> Roles_list =
                BusinessContext.tblUser_Roles.GetList(Query<tblUser_Roles>.EQ(t => t.LoginName, LoginName));
            List<string> RightInfos = new List<string>();
            foreach (tblUser_Roles RoleRight in Roles_list)
            {
                string RightInfo = RoleRight.LoginName + "|" + RoleRight.Role_Id + "|" + RoleRight.Remark;
                RightInfos.Add(RightInfo);
            }
            ViewBag.RoleList = RightInfos;
            return View(roleIList);
        }
        /// <summary>
        /// 保存设置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveData()
        {
            ReturnMessage RM = new ReturnMessage();
            try
            {
                string paramData = Request.Form["paramData"];
                string login = Request.Form["LoginName"];

                List<tblUser_Roles> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<tblUser_Roles>>(paramData);
                RM.IsSuccess = BusinessContext.tblUser_Roles.Add(list);
                if (RM.IsSuccess)
                {
                    foreach (tblUser_Roles item in list)
                    {
                        OperateLogHelper.Create<tblUser_Roles>(item);
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

    }
}
