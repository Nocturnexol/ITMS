﻿using System;
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
    public class GroupButtonController : Controller
    {
        //
        // GET: /System/GroupButton/

        public ActionResult Index(int id)
        {
            FunctionalAuthority obj = BusinessContext.FunctionalAuthority.GetModel(id);
            ViewBag.Functional = obj;
            //获取已有的按钮
            List<int> HasButtonIdList = BusinessContext.tblGroupButton.GetModelList("Group_NameId=" + id).Select(p => p.ButtonNameId).ToList();
            ViewBag.HasButtonIdList = HasButtonIdList;
            List<tblButtonName> list = BusinessContext.tblButtonName.GetModelList("1=1");
            return View(list);
        }

        [HttpPost]
        public ActionResult SaveData()
        {
            ReturnMessage RM = new ReturnMessage();

            try
            {
                string paramData = Request.Form["paramData"];
                int id = Convert.ToInt32(Request.Form["Id"]);
                // 获取设置的按钮列表
                List<tblGroupButton> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<tblGroupButton>>(paramData);
                // 获取菜单原来关联的按钮
                List<tblGroupButton> old_list_grpBtn = BusinessContext.tblGroupButton.GetModelList("Group_NameId = " + id + "");
                // 保存新增的按钮
                List<tblGroupButton> add_list_grpBtn = new List<tblGroupButton>();

                //循环页面设置的按钮
                foreach (tblGroupButton model in list)
                {
                    tblGroupButton obj = old_list_grpBtn.FirstOrDefault(p => p.Group_NameId == model.Group_NameId && p.ButtonNameId == model.ButtonNameId);
                    if (obj == null) // 新增的按钮
                    {
                        add_list_grpBtn.Add(model);
                    }
                    else // 已存在的
                    {
                        old_list_grpBtn.Remove(obj); //移除已存在的项,最终剩下的则是取消的项
                    }
                }

                // 判断是否有新增和删除
                if (add_list_grpBtn.Count == 0 && old_list_grpBtn.Count == 0)
                {
                    RM.IsSuccess = true; // 没有变化
                }
                else
                {
                    List<string> listSql = new List<string>();
                    // 新增的按钮
                    foreach (tblGroupButton item in add_list_grpBtn)
                    {
                        string a = "insert into tblGroupButton(Group_NameId,ButtonNameId,Remark) values (" + item.Group_NameId + "," + item.ButtonNameId + ",'" + item.Remark + "')";
                        listSql.Add(a);
                        //db.tblGroupButton.AddObject(item);
                    }
                    foreach (tblGroupButton item in old_list_grpBtn)
                    {
                        string a = "delete from tblGroupButton where tblrcdid = " + item.TblRcdId + "";
                        listSql.Add(a);
                    }
                    //// 取消的按钮
                    List<int> delGrpbtnIdList = old_list_grpBtn.Select(p => p.TblRcdId).ToList();
                    List<sys_role_right> old_list_roleRight = new List<sys_role_right>();
                    if (delGrpbtnIdList.Count > 0)
                    {
                        old_list_roleRight = BusinessContext.sys_role_right.GetModelList("rf_Type = '数据管理' and rf_Right_Code in " + DBContext.DataDecision.AssemblyInCondition(delGrpbtnIdList));//查找数据权限
                        foreach (sys_role_right item in old_list_roleRight)
                        {
                            string a = "delete from sys_role_right where tblrcdid = " + item.TblRcdId + "";
                            listSql.Add(a);
                        }

                    }
                    if (DBContext.DataDecision.ExecTrans(listSql.ToArray()))
                    {
                        foreach (tblGroupButton item in add_list_grpBtn)
                        {
                            OperateLogHelper.Create<tblGroupButton>(item);
                        }
                        OperateLogHelper.Delete<tblGroupButton>(old_list_grpBtn);
                        OperateLogHelper.Delete<sys_role_right>(old_list_roleRight);
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

    }
}
