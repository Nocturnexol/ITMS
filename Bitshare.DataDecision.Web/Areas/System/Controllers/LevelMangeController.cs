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

namespace Bitshare.DataDecision.Web.Areas.System.Controllers
{
    public class LevelMangeController : Controller
    {
        static string whe = string.Empty;

        public ActionResult Index()
        {
            List<string> BtnList = FlowHelper.GetBtnAuthorityForPage("上下级管理");
            ViewBag.BtnList = BtnList;
            return View();
        }



        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult GetJqGridDataList(int page = 1, int rows = 20, string keyword = null, string sidx = null, string sord = "asc")
        {

            StringBuilder strSql = new StringBuilder("1=1");
            string UserName = Request["UserName"];
            string Leader = Request["Leader"];
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                strSql.AppendFormat(" and (UserName like '%{0}%' or Leader like '%{0}%')", keyword);
            }
            string orderBy = " UserName";
            string where = strSql.ToString();
            whe = where;
            jqGridData RM = FlowHelper.GetJqGridDataList<tblLevelManange>(where, orderBy, page, rows, sidx, sord);
            return Json(RM, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Create()
        {
            SetDropList();
            return View();
        }

        public void SetDropList()
        {
            List<SelectListItem> UserList = new List<SelectListItem>();
            UserList = BusinessContext.tblUser_Sys.GetModelList("1=1").GroupBy(p => p.UserName).Select(p => p.Key).Select(p => new SelectListItem { Text = p, Value = p }).ToList();
            UserList.Insert(0, new SelectListItem { Text = "-请选择-", Value = "" });
            ViewData["UserList"] = UserList;
            ViewData["LeaderList"] = UserList;
        }


        [HttpPost]
        public ActionResult Create(tblLevelManange model, string IsContinue = "0")
        {
            ReturnMessage RM = new ReturnMessage(false);

            try
            {
                if (model.Leader == model.UserName)
                {
                    RM.Message = "自己不可设置自己为直隶上级";
                }
                else
                {
                    bool isHas = BusinessContext.tblLevelManange.Exists(model.UserName, model.Leader);
                    if (isHas)
                    {
                        RM.Message = "已存在的上下级";
                    }
                    else
                    {
                        int tblRcdid = BusinessContext.tblLevelManange.Add(model);
                        RM.IsSuccess = tblRcdid > 0;

                        if (RM.IsSuccess)
                        {
                            model.TblRcdId = tblRcdid;
                            OperateLogHelper.Create<tblLevelManange>(model);
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
            }
            catch (Exception ex)
            {
                RM.Message = ex.Message;
            }

            return Json(RM);
        }



        public ActionResult Edit(int id)
        {
            tblLevelManange model = BusinessContext.tblLevelManange.GetModel(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            SetDropList();
            return View(model);
        }



        [HttpPost]
        public ActionResult Edit(int id, tblLevelManange collection)
        {
            ReturnMessage RM = new ReturnMessage(false);
            if (ModelState.IsValid)
            {
                try
                {
                    tblLevelManange old = BusinessContext.tblLevelManange.GetModel(id);

                    RM.IsSuccess = BusinessContext.tblLevelManange.Update(collection);
                    if (RM.IsSuccess)
                    {
                        OperateLogHelper.Edit<tblLevelManange>(collection, old);
                    }

                }
                catch (Exception ex)
                {
                    RM.Message = ex.Message;
                }
            }
            return Json(RM);
        }
        public ActionResult DataDel()
        {
            ReturnMessage RM = new ReturnMessage();
            try
            {
                string paramData = Request.Form["paramData"];

                string[] tblRcdidArr = paramData.Split('*');
                StringBuilder strSql = new StringBuilder(" delete from tblLevelManange where TblRcdId in " + DBContext.AssemblyInCondition(tblRcdidArr.ToList()));

                List<tblLevelManange> modelList = BusinessContext.tblLevelManange.GetModelList("TblRcdId in " + DBContext.AssemblyInCondition(tblRcdidArr.ToList())).ToList();
                if (DBContext.DataDecision.ExecSql(strSql.ToString()) > 0)
                {
                    RM.IsSuccess = true;
                    RM.Message = "删除成功！";
                    OperateLogHelper.Delete<tblLevelManange>(modelList);
                }
                else
                {
                    RM.IsSuccess = false;
                    RM.Message = "删除失败！";
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
        public ActionResult DeriveData(string keyword = null)
        {
            ReturnMessage RM = new ReturnMessage();
            try
            {
                string paramData = Request.Form["paramData"];

                StringBuilder strSql = new StringBuilder("1=1");

                strSql = new StringBuilder("select UserName as '职员 ', Leader as '直隶上级', ReMark  as '备注'  from tblLevelManange  where  1=1");
                if (string.IsNullOrEmpty(paramData))
                {

                    string UserName = Request["UserName"];
                    string Leader = Request["Leader"];
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        strSql.AppendFormat(" and (UserName like '%{0}%' or Leader like '%{0}%')", keyword);
                    }


                }
                else
                {
                    string[] tblRcdidArr = paramData.Split('*');
                    strSql.Append(" and  TblRcdId in " + DBContext.DataDecision.AssemblyInCondition(tblRcdidArr.ToList()));
                }
                strSql.Append(" order by UserName");

                DataTable dt = DBContext.DataDecision.GetDataTable(strSql.ToString());
                string sheetName = "业务上下级信息表";
                //返回路径
                string absoluFilePath = DoExport.ExportDataTableToExcel(dt, sheetName, "业务上下级信息表");

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
