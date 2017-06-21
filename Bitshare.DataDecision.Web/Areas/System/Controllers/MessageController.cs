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
    public class MessageController : Controller
    {
       
		static string whe = string.Empty;
        //
       // GET: /System/Message/
		public ActionResult Index()
		{
			List<string> BtnList =  FlowHelper.GetBtnAuthorityForPage("个人消息");
			ViewBag.BtnList = BtnList;
            return View();
		}

		//获取个人消息数据列表
		[HttpPost]
        public ActionResult GetDataList(int page = 1, int rows = 50, string keyword = null, string sidx = "", string sord="asc")
		{
			  //StringBuilder strSql = new StringBuilder(" 1=1");
             // string loginName = CurrentHelper.CurrentUser.User.UserName.ToString();
              StringBuilder strSql = new StringBuilder(string.Format(" Accepter='{0}'", CurrentHelper.CurrentUser.User.UserName));
              //strSql.Append(" where  1=1 and Accepter='" + loginName + "'");
             
            whe = strSql.ToString();
            string orderBy = " SendDate desc";
            string where = strSql.ToString();
            jqGridData RM = FlowHelper.GetJqGridDataList<tblMessage>(where, orderBy, page, rows, sidx, sord);
            return Json(RM, JsonRequestBehavior.AllowGet);
		}


        // GET: /Message/Details/5
        public ActionResult Details(int id = 0)
        {
            Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
            dictionary = FlowHelper.GetBtnAuthorityForPageList("系统设置", "个人消息");
            tblMessage tblMessage = BusinessContext.tblMessage.GetModel(id);
            if (tblMessage == null)
            {
                return HttpNotFound();
            }

            ViewBag.MessageModel = tblMessage;

            return View(tblMessage);

        }  

        /// <summary>
        /// 消息置为已读
        /// </summary>
        /// <returns></returns>
        public JsonResult ChangeMessageState(string paramData)
        {
            ReturnMessage RM = new ReturnMessage();
            try
            {
                if (string.IsNullOrEmpty(paramData))
                {
                    RM.IsSuccess = false;
                    RM.Message = "请至少选中一项！";
                }
                string[] TblRcdIdArr = paramData.Split('*');
                List<string> lsSql = new List<string>();
                //修改消息状态
                StringBuilder strSql = new StringBuilder(" Update tblMessage set State=1 where TblRcdId in " + DBContext.DataDecision.AssemblyInCondition(TblRcdIdArr.ToList()));
                lsSql.Add(strSql.ToString());
                if (DBContext.DataDecision.ExecTrans(lsSql.ToArray()))
                {
                    RM.IsSuccess = true;
                    RM.Message = "修改成功！";
                }
                else
                {
                    RM.IsSuccess = false;
                    RM.Message = "修改失败！";
                }
            }

            catch (Exception ex)
            {
                RM.IsSuccess = false;
                RM.Message = ex.Message;
            }
            return Json(RM, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DataDel()
        {
            ReturnMessage RM = new ReturnMessage();
            try
            {
                string paramData = Request.Form["paramData"];
                if (!string.IsNullOrWhiteSpace(paramData))
                {
                    string[] TblRcdIdArr = paramData.Split('*');
                    List<string> lsSql = new List<string>();
                    //消息数据

                    StringBuilder strSql = new StringBuilder(" delete from tblMessage where TblRcdId in " + DBContext.AssemblyInCondition(TblRcdIdArr.ToList()));
                    List<tblMessage> modelList = BusinessContext.tblMessage.GetModelList("TblRcdId in " + DBContext.AssemblyInCondition(TblRcdIdArr.ToList())).ToList();
                    if (DBContext.DataDecision.ExecSql(strSql.ToString()) > 0)
                    {
                        OperateLogHelper.Delete<tblMessage>(modelList);

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

      
		
		//导出
        [HttpPost]
        public ActionResult DeriveData()
        {
            ReturnMessage RM = new ReturnMessage();
            try
            {
                string paramData = Request.Form["paramData"];
                StringBuilder strSql=new StringBuilder ("1=1");

                if (string.IsNullOrWhiteSpace(paramData))
                {
                    strSql = new StringBuilder("select Sender as '发送人', Accepter as '接收人', MsgType as '消息类型', MsgTitle as '消息标题', MsgContent as '消息内容', CONVERT(varchar, SendDate, 23 ) as '发送日期' from tblMessage  ");

                    string loginName = CurrentHelper.CurrentUser.User.UserName.ToString();

                    strSql.Append(" where  1=1 and Accepter='" + loginName + "'");
                    //string Accepter = Request.Form["Accepter"];//接收人
                    //if (!string.IsNullOrWhiteSpace(Accepter))
                    //{
                    //    strSql.Append(" and (Accepter like '%" + Accepter + "%')");
                    //}
                    strSql.Append(" order by  SendDate  desc");

                }
                else
                {
                    string[] TblRcdIdArr = paramData.Split('*');
                    strSql = new StringBuilder("select Sender as '发送人', Accepter as '接收人', MsgType as '消息类型', MsgTitle as '消息标题', MsgContent as '消息内容', CONVERT(varchar, SendDate, 23 ) as '发送日期' from tblMessage  where TblRcdId in " + DBContext.DataDecision.AssemblyInCondition(TblRcdIdArr.ToList()));

                    strSql.Append(" order by  SendDate  desc");
                }

                DataTable dt = DBContext.DataDecision.GetDataTable(strSql.ToString());


                string sheetName = "个人消息列表 ";
                //返回路径
                string absoluFilePath = DoExport.ExportDataTableToExcel(dt, sheetName, "个人消息列表");

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