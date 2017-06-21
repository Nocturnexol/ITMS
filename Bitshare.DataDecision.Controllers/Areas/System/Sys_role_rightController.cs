using Bitshare.DataDecision.Common;
using Bitshare.DataDecision.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MongoDB.Driver.Builders;

namespace Bitshare.DataDecision.Controllers.Areas.System
{
    public class Sys_role_rightController : Controller
    {
        //
        // GET: /System/Sys_role_right/

        public ActionResult Index(int roleId, string roleName)
        {
            var model = (from gb in BusinessContext.tblGroupButton.GetList()
                join bn in BusinessContext.tblButtonName.GetList() on gb.ButtonNameId equals bn.Rid into g
                from a in g.DefaultIfEmpty()
                join f in BusinessContext.FunctionalAuthority.GetList() on gb.Group_NameId equals f.Rid into gg
                from aa in gg.DefaultIfEmpty()
                where gb.ButtonNameId!=null
                select new View_GroupButtonInfo
                {
                    ButtonNameId = gb.ButtonNameId,
                    ButtonName = a.ButtonName,
                    Group_NameId = gb.Group_NameId,
                    Group_Name = aa.Group_Name,
                    Rid = gb.Rid,
                    Module_Id = aa.Module_Id,
                    Module_Name = aa.Module_Name,
                    Right_Name = aa.Right_Name
                }).ToList();
            //List<View_GroupButtonInfo> GBIList = BusinessContext.View_GroupButtonInfo.GetModelList(" ButtonNameId is not null"); //db.View_GroupButtonInfo.Where(p => p.ButtonNameId != null).ToList();//查找所有模块页面按钮
            //List<OperationalAuthority> OpAList = BusinessContext.OperationalAuthority.GetModelList(" OperRational_Name is not null").ToList();//查找所有的业务权限信息
           
            ViewBag.RoleId = roleId;
            ViewBag.roleName = roleName;
            List<sys_role_right> sys_role_rights = BusinessContext.sys_role_right.GetList(Query<sys_role_right>.EQ(t=>t.rf_Role_Id,roleId)); //db.sys_role_right.Where(p => p.rf_Role_Id == roleId).ToList();//查找对应角色的所有权限
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
           
            //ViewBag.OpAList = OpAList;
      
            ViewBag.RoleRightList = RightInfos;
            ViewBag.RoleBussinessList = Rightussinesss;

            return View(model);
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

                List<sys_role_right> oldlist =
                    BusinessContext.sys_role_right.GetList(Query<sys_role_right>.EQ(t => t.rf_Role_Id, RoleId));

                var newList = list.Except(oldlist).ToList();//新增的权限
                var delList = oldlist.Except(list).ToList();//删除的权限
                if (newList.Count == 0 && delList.Count == 0)
                {
                    RM.IsSuccess = true;
                    RM.Message = "无权限变化";
                }
                else
                {
                    RM.IsSuccess=BusinessContext.sys_role_right.Delete(delList.Select(t=>t.Rid).ToList());
                    RM.IsSuccess =RM.IsSuccess&& BusinessContext.sys_role_right.Add(newList);
                    if (RM.IsSuccess)
                    {
                        foreach (sys_role_right item in list)
                        {
                            OperateLogHelper.Create<sys_role_right>(item);
                        }

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
