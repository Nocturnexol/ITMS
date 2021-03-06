﻿using Bitshare.DataDecision.Common;
using Bitshare.DataDecision.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Bitshare.DataDecision.BLL;
using Bitshare.DataDecision.Service.DTO;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using FunctionalAuthority = Bitshare.DataDecision.Model.FunctionalAuthority;
using sys_role_right = Bitshare.DataDecision.Model.sys_role_right;
using tblGroupButton = Bitshare.DataDecision.Model.tblGroupButton;


namespace Bitshare.DataDecision.Controllers.Areas.System
{
    public class FunctionalController : Controller
    {
        private readonly MongoBll<FunctionalAuthority> _bll = new MongoBll<FunctionalAuthority>();
        //
        // GET: /System/Functional/
        [UrlAuthorize("数据权限")]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index(int page = 1, int PageSize = 50, string keyword = null)
        {
            //取得按钮权限
            List<string> BtnList = FlowHelper.GetBtnAuthorityForPage("数据权限");
           
            ViewBag.BtnList = BtnList;
            return View();
        }
        public ActionResult GetFunctionalDataList(string Module_Name = "", string Group_Name = "", string Right_Name = "", int page = 1, int rows = 20, string sidx = "", string sord = "asc")
        {
            if (string.IsNullOrEmpty(sidx))
                sidx = "Rid";
            var pager = new PageInfo
            {
                PageSize = rows,
                CurrentPageIndex = page > 0 ? page : 1
            };

            List<IMongoQuery> queryList = new List<IMongoQuery>();

            if (!string.IsNullOrWhiteSpace(Module_Name))
            {
                queryList.Add(Query<FunctionalAuthority>.Matches(t => t.Module_Name,
                        new BsonRegularExpression(new Regex(Module_Name, RegexOptions.IgnoreCase))));
            }
            if (!string.IsNullOrWhiteSpace(Group_Name))
            {
                queryList.Add(Query<FunctionalAuthority>.Matches(t => t.Group_Name,
                         new BsonRegularExpression(new Regex(Group_Name, RegexOptions.IgnoreCase))));
            }
            if (!string.IsNullOrWhiteSpace(Right_Name))
            {
                queryList.Add(Query<FunctionalAuthority>.Matches(t => t.Right_Name,
                        new BsonRegularExpression(new Regex(Right_Name, RegexOptions.IgnoreCase))));
            }

            var where = queryList.Any() ? Query.And(queryList) : null;

            long totalCount;
            //string orderBy = "module_id,group_id,right_Id";
            var rm = new jqGridData
            {
                page = pager.CurrentPageIndex,
                rows = _bll.GetList(out totalCount, page, rows, where, sidx, sord),
                total = (int)(totalCount % pager.PageSize == 0 ? totalCount / pager.PageSize : totalCount / pager.PageSize + 1),
                records = (int)totalCount
            };
            return Json(rm, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /System/Functional/Create

        [HttpPost]
        public ActionResult Create(FunctionalAuthority obj, string IsContinue = "0")
        {
            ReturnMessage RM = new ReturnMessage(false);
            if (ModelState.IsValid)
            {
                try
                {
                    //var list = BusinessContext.FunctionalAuthority.GetList("Module_Name ='" + obj.Module_Name + "'");
                    List<FunctionalAuthority> list =
                        BusinessContext.FunctionalAuthority.GetList(Query<FunctionalAuthority>.EQ(t => t.Module_Name,
                            obj.Module_Name));
                    //var list = BusinessContext.FunctionalAuthority.Where(p => p.Module_Name == obj.Module_Name).ToList();
                    if (list != null && list.Count() > 0)
                    {
                        obj.Module_Id = list.First().Module_Id;
                        var group = list.FirstOrDefault(p => p.Group_Name == obj.Group_Name);
                        var right = list.FirstOrDefault(p => p.Right_Name == obj.Right_Name);
                        if (right != null)
                        {
                            RM.Message = "页面名称重复";
                            return Json(RM);

                        }
                        if (group == null)
                        {
                            obj.Group_Id = list.Max(p => p.Group_Id) + 1;
                            obj.Right_Id = 1;
                        }
                        else
                        {
                            obj.Group_Id = group.Group_Id;
                            obj.Right_Id = Convert.ToInt32(list.Where(p => p.Group_Id == obj.Group_Id).Max(p => p.Right_Id)) + 1;
                        }
                    }
                    else
                    {
                        int maxModule = Convert.ToInt32(BusinessContext.FunctionalAuthority.GetMaxId());
                        obj.Module_Id = maxModule + 1;
                        obj.Group_Id = 1;
                        obj.Right_Id = 1;
                    }
                    bool res= BusinessContext.FunctionalAuthority.Add(obj);
                    RM.IsSuccess = res ;
                    if (RM.IsSuccess)
                    {
                        //obj.Rid = Rid;
                        OperateLogHelper.Create(obj);
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

            FunctionalAuthority dpt =
                BusinessContext.FunctionalAuthority.Get(Query<FunctionalAuthority>.EQ(t => t.Rid, id));
            if (dpt == null)
            {
                return HttpNotFound();
            }
            return View(dpt);

        }
        [HttpPost]
        public ActionResult Edit(FunctionalAuthority collection)
        {
            ReturnMessage RM = new ReturnMessage(false);
            if (ModelState.IsValid)
            {
                try
                {
                    FunctionalAuthority old = BusinessContext.FunctionalAuthority.Get(Query<FunctionalAuthority>.EQ(t => t.Rid, collection.Rid));
                    RM.IsSuccess = BusinessContext.FunctionalAuthority.Update(collection);
                    if (RM.IsSuccess)
                    {
                        OperateLogHelper.Edit(collection, old);
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
                if (!string.IsNullOrWhiteSpace(paramData))
                {
                    var RidArr = paramData.Split('*').Select(int.Parse);
                    List<string> lsSql = new List<string>();
                    var ridArr = RidArr as IList<int> ?? RidArr.ToList();
                    var res=_bll.Delete(ridArr);
                    //StringBuilder strSql = new StringBuilder(" delete from FunctionalAuthority where Rid in " + DBContext.DataDecision.AssemblyInCondition(ridArr.ToList()));
                    //lsSql.Add(strSql.ToString());
                    //List<FunctionalAuthority> F_modelList = BusinessContext.FunctionalAuthority.GetList(Query<FunctionalAuthority>.In(t => t.Rid, ridArr)).ToList();


                    //strSql = new StringBuilder("delete from dbo.sys_role_right where rf_right_code in (select Rid  from dbo.tblGroupButton where Group_NameId in " + DBContext.DataDecision.AssemblyInCondition(ridArr.ToList()) + ")");
                    //lsSql.Add(strSql.ToString());
                    //List<sys_role_right> S_modelList = BusinessContext.sys_role_right.GetModelList("rf_right_code in (select Rid  from dbo.tblGroupButton where Group_NameId in " + DBContext.DataDecision.AssemblyInCondition(ridArr.ToList()) + ")").ToList();

                    //strSql = new StringBuilder("delete from dbo.tblGroupButton where Group_NameId in " + DBContext.DataDecision.AssemblyInCondition(ridArr.ToList()));
                    //lsSql.Add(strSql.ToString());
                    //List<tblGroupButton> G_modelList = BusinessContext.tblGroupButton.GetModelList("Group_NameId in " + DBContext.DataDecision.AssemblyInCondition(ridArr.ToList())).ToList();

                    if (DBContext.DataDecision.ExecTrans(lsSql.ToArray()))
                    {
                        //OperateLogHelper.Delete<FunctionalAuthority>(F_modelList);
                        //OperateLogHelper.Delete<sys_role_right>(S_modelList);
                        //OperateLogHelper.Delete<tblGroupButton>(G_modelList);

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
                //StringBuilder strSql = new StringBuilder("select  Module_Name '模块名称 ',Module_Id '模块ID ', Group_Name '分组名称 ',Group_Id '分组ID', Right_Name '页面名称 ',Right_Id '页面ID',Right_Url '页面路径',Remark as '备注'  from FunctionalAuthority  where  1=1");
                StringBuilder strSql = new StringBuilder("select  Module_Name '模块名称 ', Group_Name '分组名称 ', Right_Name '页面名称 ',Right_Url '页面路径',Remark as '备注'  from FunctionalAuthority  where  1=1");
                if (string.IsNullOrEmpty(paramData))
                {
                    string Module_Name = Request["Module_Name"];
                    string Group_Name = Request["Group_Name"];
                    string Right_Name = Request["Right_Name"];
                    StringBuilder where = new StringBuilder(" 1=1");
                    if (!string.IsNullOrWhiteSpace(Module_Name))
                    {
                        where.Append(" and Module_Name like '%" + Module_Name + "%'");
                    }
                    if (!string.IsNullOrWhiteSpace(Group_Name))
                    {
                        where.Append(" and Group_Name like '%" + Group_Name + "%'");
                    }
                    if (!string.IsNullOrWhiteSpace(Right_Name))
                    {
                        where.Append(" and Right_Name like '%" + Right_Name + "%'");
                    }
                    strSql.Append(" and " + where.ToString());
                }
                else
                {
                    string[] RidArr = paramData.Split('*');
                    strSql.Append(" and  Rid in " + DBContext.DataDecision.AssemblyInCondition(RidArr.ToList()));
                }
                strSql.Append(" order by Module_Id,Group_Id,Right_Id");

                DataTable dt = DBContext.DataDecision.GetDataTable(strSql.ToString());
                string sheetName = "模块信息";
                //返回路径
                string absoluFilePath = DoExport.ExportDataTableToExcel(dt, sheetName, "模块信息");

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
