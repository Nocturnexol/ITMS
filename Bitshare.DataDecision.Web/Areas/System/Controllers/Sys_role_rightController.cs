using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bitshare.DataDecision.Model;
using Bitshare.DataDecision.Web.Common;
using Bitshare.DataDecision.Web.Models;
using System.Data.Common;
using Bitshare.DataDecision.Common;

namespace Bitshare.DataDecision.Web.Areas.System.Controllers
{
    public class Sys_role_rightController : Controller
    {
        //
        // GET: /System/Sys_role_right/

        public ActionResult Index(int roleId, string roleName)
        {
            List<View_GroupButtonInfo> GBIList = BusinessContext.View_GroupButtonInfo.GetModelList(" ButtonNameId is not null"); //db.View_GroupButtonInfo.Where(p => p.ButtonNameId != null).ToList();//查找所有模块页面按钮
            List<OperationalAuthority> OpAList = BusinessContext.OperationalAuthority.GetModelList(" OperRational_Name is not null").ToList();//查找所有的业务权限信息
           
            ViewBag.RoleId = roleId;
            ViewBag.roleName = roleName;
            List<sys_role_right> sys_role_rights = BusinessContext.sys_role_right.GetModelList("rf_Role_Id =" + roleId); //db.sys_role_right.Where(p => p.rf_Role_Id == roleId).ToList();//查找对应角色的所有权限
            List<sys_role_right> roleRights = sys_role_rights.Where(p => p.rf_Type == "数据管理").ToList();//查找数据权限
            List<sys_role_right> roleBussiness = sys_role_rights.Where(p => p.rf_Type == "业务权限").ToList();//查找业务权限
          

            List<string> RightInfos = new List<string>();//数据权限拼接
            List<string> Rightussinesss = new List<string>();//数据权限拼接
         
            foreach (sys_role_right RoleRight in roleRights)
            {
                string RightInfo = RoleRight.rf_Type + "|" + RoleRight.rf_Role_Id + "|" + RoleRight.rf_Right_Code + "|" + RoleRight.rf_Right_Authority;
                RightInfos.Add(RightInfo);
            }
            foreach (sys_role_right RoleRight in roleBussiness)
            {
                string RightInfo = RoleRight.rf_Type + "|" + RoleRight.rf_Role_Id + "|" + RoleRight.rf_Right_Code + "|" + RoleRight.rf_Right_Authority;
                Rightussinesss.Add(RightInfo);
            }
           
            ViewBag.OpAList = OpAList;
      
            ViewBag.RoleRightList = RightInfos;
            ViewBag.RoleBussinessList = Rightussinesss;
  
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
                List<sys_role_right> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<sys_role_right>>(paramData);
                RM.IsSuccess = BusinessContext.sys_role_right.Addlist(list, RoleId);
                if (RM.IsSuccess)
                {
                    foreach (sys_role_right item in list)
                    {
                        OperateLogHelper.Create<sys_role_right>(item);
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
