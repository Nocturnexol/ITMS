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
    public class BusinessRightsController : Controller
    {
        //
        // GET: /System/BusinessRights/

        public ActionResult Index()
        {
            List<string> BtnList = FlowHelper.GetBtnAuthorityForPage("业务权限");
            ViewBag.BtnList = BtnList;
            return View();
        }
        public ActionResult GetJqGridDataList(int page = 1, int rows = 20, string keyword = null, string sidx = null, string sord = "asc")
        {

            StringBuilder strSql = new StringBuilder("1=1");
            string OperRational_Name = Request["OperRational_Name"];
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                strSql.AppendFormat(" and OperRational_Name like '%{0}%'", keyword);
            }
            string orderBy = "OperRational_Name";
            string where = strSql.ToString();
            jqGridData RM = FlowHelper.GetJqGridDataList<OperationalAuthority>(where, orderBy, page, rows, sidx, sord);
            return Json(RM, JsonRequestBehavior.AllowGet);
        }
       
        //
        // GET: /System/BusinessRights/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /System/BusinessRights/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /System/BusinessRights/Create

        [HttpPost]
        public ActionResult Create(OperationalAuthority collection, string IsContinue = "0")
        {
            ReturnMessage RM = new ReturnMessage(false);

            try
            {
                collection.RightsOptions = Request["RightsOptions"];
                DateTime now = DateTime.Now;
                collection.CreateDataTime = now;
                collection.UpdateTime = now;
                collection.UpdateUser = CurrentHelper.CurrentUser.User.LoginName;
                collection.CreateDataUser = CurrentHelper.CurrentUser.User.LoginName;
                var query = BusinessContext.OperationalAuthority.GetModelList(String.Format("OperRational_Name='{0}'", collection.OperRational_Name));
                if (query.Count > 0)
                {
                    RM.Message = "业务名已被占用";
                }
                else
                {
                    int tblRcdid = BusinessContext.OperationalAuthority.Add(collection);
                    RM.IsSuccess = tblRcdid > 0;
                    collection.TblRcdId = tblRcdid;
                    if (RM.IsSuccess)
                    {
                        OperateLogHelper.Create<OperationalAuthority>(collection);
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

        //
        // GET: /System/BusinessRights/Edit/5

        public ActionResult Edit(int id)
        {

            OperationalAuthority model = BusinessContext.OperationalAuthority.GetModel(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            IList<SelectListItem> RightsOptionsList = new List<SelectListItem>();
            string[] RightsArray = model.RightsOptions.Split(',');
            RightsOptionsList.Add(new SelectListItem { Text = "查看本人", Value = "查看本人", Selected = RightsArray.Contains("查看本人") });
            RightsOptionsList.Add(new SelectListItem { Text = "查看下级", Value = "查看下级", Selected = RightsArray.Contains("查看下级") });
            RightsOptionsList.Add(new SelectListItem { Text = "查看所有", Value = "查看所有", Selected = RightsArray.Contains("查看所有") });
            RightsOptionsList.Add(new SelectListItem { Text = "查看本部门", Value = "查看本部门", Selected = RightsArray.Contains("查看本部门") });
            ViewData["RightsOptionsList"] = RightsOptionsList;

            IList<SelectListItem> OptionList = new List<SelectListItem>();
            OptionList = new List<SelectListItem>();
            OptionList.Add(new SelectListItem { Text = "单选", Value = "false", Selected = (model.Options == false ? true : false) });
            OptionList.Add(new SelectListItem { Text = "多选", Value = "true", Selected = (model.Options == true ? true : false) });
            ViewData["OptionList"] = OptionList;
            return View(model);
        }

        //
        // POST: /System/BusinessRights/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, OperationalAuthority collection, FormCollection form)
        {
            ReturnMessage RM = new ReturnMessage(false);

            try
            {
                OperationalAuthority old = BusinessContext.OperationalAuthority.GetModel(id);
                string Rights_select = Request["Rights_select"];
                collection.RightsOptions = Rights_select;
                collection.UpdateUser = CurrentHelper.CurrentUser.User.LoginName;
                collection.UpdateTime = DateTime.Now;
                RM.IsSuccess = BusinessContext.OperationalAuthority.Update(collection);
                if (RM.IsSuccess)
                {
                    OperateLogHelper.Edit<OperationalAuthority>(collection, old);
                }

            }
            catch (Exception ex)
            {
                RM.Message = ex.Message;
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
                List<string> lsSql = new List<string>();
                StringBuilder strSql = new StringBuilder(" delete from sys_role_right where  rf_Type='业务权限' and rf_right_code in  " + DBContext.DataDecision.AssemblyInCondition(tblRcdidArr.ToList()));
                lsSql.Add(strSql.ToString());
                strSql = new StringBuilder(" delete from OperationalAuthority where TblRcdId in " + DBContext.AssemblyInCondition(tblRcdidArr.ToList()));
                lsSql.Add(strSql.ToString());
                List<OperationalAuthority> modelList = BusinessContext.OperationalAuthority.GetModelList("TblRcdId in " + DBContext.AssemblyInCondition(tblRcdidArr.ToList())).ToList();
                if (DBContext.DataDecision.ExecTrans(lsSql.ToArray()))
                {
                    RM.IsSuccess = true;
                    RM.Message = "删除成功！";
                    OperateLogHelper.Delete<OperationalAuthority>(modelList);
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
        public ActionResult DeriveData()
        {
            ReturnMessage RM = new ReturnMessage();
            try
            {
                string paramData = Request.Form["paramData"];
                StringBuilder strSql = new StringBuilder("select OperRational_Name as '业务名 ', RightsOptions as '权限描述', Remark as '备注'  from OperationalAuthority  where  1=1");
                if (string.IsNullOrEmpty(paramData))
                {
                    string keyword = Request["keyword"];
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        strSql.Append(" and (OperRational_Name like '%" + keyword + "%')");
                    }
                }
                else
                {
                    string[] tblRcdidArr = paramData.Split('*');
                    strSql.Append(" and  TblRcdId in " + DBContext.DataDecision.AssemblyInCondition(tblRcdidArr.ToList()));
                }
                strSql.Append(" order by OperRational_Name");

                DataTable dt = DBContext.DataDecision.GetDataTable(strSql.ToString());
                string sheetName = "业务权限定义表";
                //返回路径
                string absoluFilePath = DoExport.ExportDataTableToExcel(dt, sheetName, "业务权限定义表");

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
        //
        // GET: /System/BusinessRights/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /System/BusinessRights/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
