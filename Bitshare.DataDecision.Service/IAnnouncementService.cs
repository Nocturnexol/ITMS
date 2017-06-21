using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bitshare.PTMM.Model;
using Bitshare.PTMM.Service.DTO;

namespace Bitshare.PTMM.Service
{
   public  interface IAnnouncementService
    {

        /// <summary>
        /// 获取公告管理-发布公告信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryFields"></param>
        /// <param name="orderBy"></param>
        /// <param name="loginName"></param>
        /// <param name="AnnouncementName"></param>
        /// <param name="ReleaseDepart"></param>
        /// <param name="AnnouncementType"></param>
        /// <param name="Content"></param>
        /// <param name="InsureIssueDate"></param>
        /// <param name="InsureEndDate"></param>
        /// <returns></returns>

       PageResult GetAnnouncementList(int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string loginName = "", string AnnouncementName = null, string ReleaseDepart = null, string AnnouncementType = null, string Content = null, string InsureIssueDate = null, string InsureEndDate = null, string RadioAdorder = null);

       /// <summary>
       /// 获取公告管理-接收公告信息
       /// </summary>
       /// <param name="pageIndex"></param>
       /// <param name="pageSize"></param>
       /// <param name="queryFields"></param>
       /// <param name="orderBy"></param>
       /// <param name="loginName"></param>
       /// <param name="AnnouncementName"></param>
       /// <param name="ReleaseDepart"></param>
       /// <param name="AnnouncementType"></param>
       /// <param name="Content"></param>
       /// <param name="InsureIssueDate"></param>
       /// <param name="InsureEndDate"></param>
       /// <returns></returns>
       PageResult GetAnnouncementList_view(int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string AnnouncementName = null, string ReleaseDepart = null, string loginName = "", string AnnouncementType = null, string Content = null, string InsureIssueDate = null, string InsureEndDate = null, string RadioAdorder = null);
       /// <summary>
        /// 查询工作汇报
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="authority"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="orderBy"></param>
        /// <param name="keyword"></param>
        /// <param name="ReportDepart"></param>
        /// <param name="ReportType"></param>
        /// <param name="ReportDate"></param>
        /// <param name="ReportEndDate"></param>
        /// <returns></returns>
       PageResult GetWorkReportPageResult(string loginName, string authority, int page, int rows, string orderBy, string keyword, string ReportDepart, string ReportType, string ReportDate, string ReportEndDate, string Type);
       
       /// <summary>
       /// 查询工作联系单
       /// </summary>
       /// <param name="loginName"></param>
       /// <param name="authority"></param>
       /// <param name="page"></param>
       /// <param name="rows"></param>
       /// <param name="orderBy"></param>
       /// <param name="keyword"></param>
       /// <param name="ContactDeper"></param>
       /// <param name="ContactName"></param>
       /// <param name="ContactDate"></param>
       /// <returns></returns>
       PageResult GetWorkContactPageResult(string loginName, string UserName, string authority, int page, int rows, string orderBy, string keyword, string ContactDeper, string ContactConten, string startDate, string EndDate, string Type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="orderBy"></param>
        /// <param name="Type"></param>
        /// <param name="MainKey"></param>
        /// <returns></returns>
       PageResult GetAttachmentPageResult(string loginName, int page, int rows, string orderBy, string Type = "", string MainKey="");

       /// <summary>
       /// 
       /// </summary>
       /// <param name="loginName"></param>
       /// <param name="page"></param>
       /// <param name="rows"></param>
       /// <param name="orderBy"></param>
       /// <param name="Type"></param>
       /// <param name="MainKey"></param>
       /// <returns></returns>
       PageResult GetAttachmentIdPageResult(string loginName, int page, int rows, string orderBy, string Type = "", string TblRcdId = "");


        /// <summary>
        /// 
        /// </summary>
        /// <param name="LoginName"></param>
        /// <param name="authority"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="orderBy"></param>
        /// <param name="keyword"></param>
        /// <param name="ReportDepart"></param>
        /// <param name="ReportType"></param>
        /// <param name="ReportDate"></param>
        /// <param name="ReportEndDate"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
       PageResult GetWorkReportReceivePageResult(string LoginName, string authority, int page, int rows, string orderBy, string keyword, string ReportDepart, string ReportType, string ReportDate, string ReportEndDate, string Type = "");

       /// <summary>
       /// 详细页面明细数据获取
       /// </summary>
       /// <param name="pageIndex">页码</param>
       /// <param name="pageSize">条数</param>
       /// <param name="queryFields">查询字段,默认查询全部</param>
       /// <param name="orderBy">排序</param>
       /// <param name="sWhere">条件</param>
       /// <param name="type">判断是哪一个明细</param>
       /// <returns></returns>
       PageResult GetAllPageResult(int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null);

    }
}
