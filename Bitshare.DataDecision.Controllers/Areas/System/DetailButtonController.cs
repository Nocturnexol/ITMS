using Bitshare.DataDecision.Common;
using Bitshare.DataDecision.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;


namespace Bitshare.DataDecision.Controllers.Areas.System
{
    public class DetailButtonController : Controller
    {
        //
        // GET: /System/DetailButton/

        public ActionResult Index(int id)
        {
            tblPageDetail obj = BusinessContext.tblPageDetail.GetModel(id);
            ViewBag.PageDetail = obj;
            //获取已有的按钮
            List<int> HasButtonIdList = BusinessContext.tblDetailButton.GetModelList("Detail_NameId=" + id).Select(p => p.ButtonNameId.Value).ToList();
            ViewBag.HasButtonIdList = HasButtonIdList;
            List<tblButtonName> list = BusinessContext.tblButtonName.GetList();
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
                List<tblDetailButton> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<tblDetailButton>>(paramData);
                // 获取菜单原来关联的按钮
                List<tblDetailButton> old_list_grpBtn = BusinessContext.tblDetailButton.GetModelList("Detail_NameId = " + id + "");
                // 保存新增的按钮
                List<tblDetailButton> add_list_grpBtn = new List<tblDetailButton>();
                //循环页面设置的按钮
                foreach (tblDetailButton model in list)
                {
                    tblDetailButton obj = old_list_grpBtn.FirstOrDefault(p => p.Detail_NameId == model.Detail_NameId && p.ButtonNameId == model.ButtonNameId);
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
                    foreach (tblDetailButton item in add_list_grpBtn)
                    {
                        string a = "insert into tblDetailButton(Detail_NameId,ButtonNameId,Remark) values (" + item.Detail_NameId + "," + item.ButtonNameId + ",'" + item.Remark + "')";
                        listSql.Add(a);
                    }
                    // 取消的按钮
                    List<tblDetailButton> modelList = new List<tblDetailButton>();
                    List<int> delGrpbtnIdList = old_list_grpBtn.Select(p => p.Rid).ToList();
                    foreach (tblDetailButton item in old_list_grpBtn)
                    {
                        modelList.Add(item);
                        string a = "delete from tblDetailButton where Rid = " + item.Rid + "";
                        listSql.Add(a);
                    }
                    if (DBContext.DataDecision.ExecTrans(listSql.ToArray()))
                    {
                        foreach (tblDetailButton item in add_list_grpBtn)
                        {
                            OperateLogHelper.Create<tblDetailButton>(item);
                        }
                        OperateLogHelper.Delete<tblDetailButton>(modelList);
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
