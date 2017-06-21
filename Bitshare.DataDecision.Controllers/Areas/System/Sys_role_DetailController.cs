using Bitshare.DataDecision.Common;
using Bitshare.DataDecision.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Bitshare.DataDecision.Controllers.Areas.System
{
    public class Sys_role_DetailController : Controller
    {
        //
        // GET: /System/Sys_role_Detail/

        public ActionResult Index(int roleId, string roleName)
        {
            List<View_DetailButtonNew> GBIList = BusinessContext.View_DetailButtonNew.GetModelList(" ButtonNameId is not null"); //查找所有模块页面按钮
   
            ViewBag.RoleId = roleId;
            ViewBag.roleName = roleName;
            List<sys_role_Detail> sys_role_rights = BusinessContext.sys_role_Detail.GetModelList("rf_Role_Id=" + roleId); //查找对应角色的所有权限
            List<sys_role_Detail> roleRights = sys_role_rights.Where(p => p.rf_Type == "数据管理").ToList();//查找数据权限
          
            List<string> RightInfos = new List<string>();//数据权限拼接
     
            foreach (sys_role_Detail RoleRight in roleRights)
            {
                string RightInfo = RoleRight.rf_Type + "|" + RoleRight.rf_Role_Id + "|" + RoleRight.rf_Right_Code;
                RightInfos.Add(RightInfo);
            }
           

            ViewBag.RoleRightList = RightInfos;
            return View(GBIList);
        }
        [HttpPost]
        public ActionResult SaveData()
        {
            ReturnMessage RM = new ReturnMessage();
           
            try
            {
                string paramData = Request.Form["paramData"];
                int RoleId = Convert.ToInt32(Request.Form["RoleId"]);
                List<sys_role_Detail> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<sys_role_Detail>>(paramData);

                RM.IsSuccess = BusinessContext.sys_role_Detail.Addlist(list, RoleId);
                if (RM.IsSuccess)
                {
                    foreach (sys_role_Detail item in list)
                    {
                        OperateLogHelper.Create<sys_role_Detail>(item);
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
