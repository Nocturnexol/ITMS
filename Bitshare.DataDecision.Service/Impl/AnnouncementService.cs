using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bitshare.PTMM.Service.DTO;
using Bitshare.PTMM.Model;
using Bitshare.PTMM.Common;
using System.Data;
namespace Bitshare.PTMM.Service.Impl
{
    #region 工厂定义
    public class AnnouncementServiceFactory
    {
        static IAnnouncementService Instance;
        public IAnnouncementService GetInstance()
        {
            if (Instance == null)
            {
                Instance = new AnnouncementService();
            }
            return Instance;

        }
    }
    #endregion


    #region  实现方法
    internal class AnnouncementService : IAnnouncementService
    {
        /// <summary>
        /// 获取公告管理-发布公告信息
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="loginName"></param>
        /// <returns></returns>

        public PageResult GetAnnouncementList(int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string loginName = "", string AnnouncementName = null, string ReleaseDepart = null, string AnnouncementType = null, string Content = null, string InsureIssueDate = null, string InsureEndDate = null, string RadioAdorder = null)
        {

            PageResult result = new PageResult();
            try
            {
                PageInfo page = new PageInfo();
                page.CurrentPageIndex = pageIndex;
                page.PageSize = pageSize;
                page.QueryFields = queryFields;
                page.Orderby = orderBy;
                StringBuilder strSql = new StringBuilder("1=1");
                if (!string.IsNullOrEmpty(AnnouncementName))
                {
                    strSql.AppendFormat(" and AnnouncementName like '%{0}%'", AnnouncementName);
                }
                if (!string.IsNullOrEmpty(Content))
                {
                    strSql.AppendFormat(" and Content like '%{0}%'", Content);
                }
                if (!string.IsNullOrEmpty(ReleaseDepart) && ReleaseDepart != "-请选择-")
                {
                    strSql.AppendFormat(" and ReleaseDepart= '{0}'", ReleaseDepart);
                }
                if (!string.IsNullOrEmpty(AnnouncementType) && AnnouncementType != "-请选择-")
                {
                    strSql.AppendFormat(" and AnnouncementType= '{0}'", AnnouncementType);
                }

                if ((!string.IsNullOrEmpty(InsureEndDate)))
                {
                    strSql.AppendFormat(" and InsureIssueDate <='{0}' ", InsureEndDate);
                }

                if ((!string.IsNullOrEmpty(InsureIssueDate)))
                {
                    strSql.AppendFormat(" and InsureEndDate >='{0}' ", InsureIssueDate);
                }

                strSql.AppendFormat(" and ReleaseUserName='{0}'", loginName);

                page.Where = strSql.ToString();
                //查询数据
                result = CommonHelper.ListPageResult<Bitshare.PTMM.Model.View_AnnouncementList>(page);
            }
            catch (Exception ex)
            {
                LogManager.Error("GetAnnouncementList()", ex);
            }
            return result;


        }

        /// <summary>
        /// 获取公告管理-接收公告信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryFields"></param>
        /// <param name="orderBy"></param>
        /// <param name="AnnouncementName"></param>
        /// <param name="ReleaseDepart"></param>
        /// <param name="loginName"></param>
        /// <param name="AnnouncementType"></param>
        /// <param name="Content"></param>
        /// <param name="InsureIssueDate"></param>
        /// <param name="InsureEndDate"></param>
        /// <param name="RadioAdorder"></param>
        /// <returns></returns>
        public PageResult GetAnnouncementList_view(int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string AnnouncementName = null, string ReleaseDepart = null, string loginName = "", string AnnouncementType = null, string Content = null, string InsureIssueDate = null, string InsureEndDate = null, string RadioAdorder = null)
        {

            PageResult result = new PageResult();
            try
            {
                PageInfo page = new PageInfo();
                page.CurrentPageIndex = pageIndex;
                page.PageSize = pageSize;
                page.QueryFields = queryFields;
                page.Orderby = orderBy;
                StringBuilder strSql = new StringBuilder("1=1");
                if (!string.IsNullOrEmpty(AnnouncementName))
                {
                    strSql.AppendFormat(" and AnnouncementName like '%{0}%'", AnnouncementName);
                }
                if (!string.IsNullOrEmpty(Content))
                {
                    strSql.AppendFormat(" and Content like '%{0}%'", Content);
                }
                if (!string.IsNullOrEmpty(ReleaseDepart) && ReleaseDepart != "-请选择-")
                {
                    strSql.AppendFormat(" and ReleaseDepart= '{0}'", ReleaseDepart);
                }
                if (!string.IsNullOrEmpty(AnnouncementType) && AnnouncementType != "-请选择-")
                {
                    strSql.AppendFormat(" and AnnouncementType= '{0}'", AnnouncementType);
                }

                if ((!string.IsNullOrEmpty(InsureEndDate)))
                {
                    strSql.AppendFormat(" and InsureIssueDate <='{0}' ",InsureEndDate );
                }

                if ((!string.IsNullOrEmpty(InsureIssueDate)))
                {
                    strSql.AppendFormat(" and InsureEndDate >='{0}' ", InsureIssueDate);
                }


                strSql.AppendFormat(" and AccessLoginname = '{0}'", loginName);
               
                page.Where = strSql.ToString();
                //查询数据
                result = CommonHelper.ListPageResult<Bitshare.PTMM.Model.View_Announcement>(page);
            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("GetAnnouncementList_view()", ex);
            }
            return result;

        }
        #region 工作汇报 
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
        public PageResult GetWorkReportReceivePageResult(string LoginName, string authority, int page, int rows, string orderBy, string keyword, string ReportDepart, string ReportType, string ReportDate, string ReportEndDate, string Type = "")
        {
            PageResult result = new PageResult();
            try
            {
                PageInfo pageInfo = new PageInfo();
                pageInfo.CurrentPageIndex = page;
                pageInfo.PageSize = rows;
                pageInfo.QueryFields = "*";
                pageInfo.Orderby = orderBy;
                pageInfo.TableName = "View_ReportPostil";
                StringBuilder strSql = new StringBuilder("1=1");

                strSql.AppendFormat(" and (Loginname ='{0}')", LoginName );
                if (!string.IsNullOrEmpty(ReportDepart))
                {
                    strSql.AppendFormat(" and ReportDepart ='{0}'", ReportDepart);
                }
                if (!string.IsNullOrEmpty(ReportType))
                {
                    strSql.AppendFormat(" and ReportType ='{0}'", ReportType);
                }
                if ((!string.IsNullOrEmpty(ReportDate)))
                {
                    strSql.AppendFormat(" and ReportDate >='{0}' ", ReportDate);
                }
                if ((!string.IsNullOrEmpty(ReportEndDate)))
                {
                    strSql.AppendFormat(" and ReportEndDate <='{0}' ", ReportEndDate);
                }
                pageInfo.Where = strSql.ToString();
                //查询数据
                result = CommonHelper.ListPageResult<Bitshare.PTMM.Model.View_ReportPostil>(pageInfo);
            }
            catch (Exception ex)
            {
                LogManager.Error("GetWorkReportPageResult()", ex);
            }
            return result;
        }
        /// <summary>
        /// 查询工作汇报
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
        public PageResult GetWorkReportPageResult(string LoginName, string authority, int page, int rows, string orderBy, string keyword, string ReportDepart, string ReportType, string ReportDate, string ReportEndDate, string Type = "")
        {
            PageResult result = new PageResult();
            try
            {
                PageInfo pageInfo = new PageInfo();
                pageInfo.CurrentPageIndex = page;
                pageInfo.PageSize = rows;
                pageInfo.QueryFields = "*";
                pageInfo.Orderby = orderBy;
                pageInfo.TableName = "TblReport";
                StringBuilder strSql = new StringBuilder("1=1");
                if (!string.IsNullOrEmpty(Type))
                {
                    if (Type == "0")
                    {
                        strSql.AppendFormat(" and (State ='{0}' or State is NULL) and CreateName='{1}'", Type, LoginName);
                    }
                    else if (Type == "1")
                    {
                        strSql.AppendFormat(" and State ='{0}' and (CreateName='{1}')", Type, LoginName);
                    }
                    else if (Type == "2")
                    {
                        strSql.AppendFormat(" and State ='1' and Tblradid in (select Mainkey from dbo.View_ReportPostil where loginname='{0}')", LoginName);
                    }
                }
                if (!string.IsNullOrEmpty(ReportDepart))
                {
                    strSql.AppendFormat(" and ReportDepart ='{0}'", ReportDepart);
                }
                if (!string.IsNullOrEmpty(ReportType))
                {
                    strSql.AppendFormat(" and ReportType ='{0}'", ReportType);
                }
                if ((!string.IsNullOrEmpty(ReportDate)))
                {
                    strSql.AppendFormat(" and ReportDate >='{0}' ", ReportDate);
                }
                if ((!string.IsNullOrEmpty(ReportEndDate)))
                {
                    strSql.AppendFormat(" and ReportEndDate <='{0}' ", ReportEndDate);
                }
                pageInfo.Where = strSql.ToString();
                //查询数据
                result = CommonHelper.ListPageResult<Bitshare.PTMM.Model.View_MainReport>(pageInfo);
            }
            catch (Exception ex)
            {
                LogManager.Error("GetWorkReportPageResult()", ex);
            }
            return result;
        }
        #region 工作汇报上传
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="orderBy"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public PageResult GetAttachmentPageResult(string loginName, int page, int rows, string orderBy, string Type = "",string Mainkey="")
        {
            PageResult result = new PageResult();
            try
            {
                PageInfo pageInfo = new PageInfo();
                pageInfo.CurrentPageIndex = page;
                pageInfo.PageSize = rows;
                pageInfo.QueryFields = "*";
                pageInfo.Orderby = orderBy;
                pageInfo.TableName = "TblAttachment";
                StringBuilder strSql = new StringBuilder("1=1");
                if (!string.IsNullOrEmpty(Type))
                {
                    strSql.AppendFormat(" and Grouping ='{0}'", Type);
                }
                if (!string.IsNullOrEmpty(Mainkey))
                {
                    strSql.AppendFormat(" and Mainkey ={0}", Mainkey);
                }
                pageInfo.Where = strSql.ToString();
                //查询数据
                result = CommonHelper.ListPageResult<Bitshare.PTMM.Model.TblAttachment>(pageInfo);
            }
            catch (Exception ex)
            {
                LogManager.Error("GetAttachmentPageResult()", ex);
            }
            return result;
        }
        #endregion
        #endregion

        #region 工作联系单获取附件列表
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="orderBy"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public PageResult GetAttachmentIdPageResult(string loginName, int page, int rows, string orderBy, string Type = "", string TblRcdId = "")
        {
            PageResult result = new PageResult();
            try
            {
                PageInfo pageInfo = new PageInfo();
                pageInfo.CurrentPageIndex = page;
                pageInfo.PageSize = rows;
                pageInfo.QueryFields = "*";
                pageInfo.Orderby = orderBy;
                pageInfo.TableName = "TblAttachment";
                StringBuilder strSql = new StringBuilder("1=1");
                if (!string.IsNullOrEmpty(Type))
                {
                    strSql.AppendFormat(" and Grouping ='{0}'", Type);
                }
                if (!string.IsNullOrEmpty(TblRcdId))
                {
                    strSql.AppendFormat(" and ");
                    string[] TblRcdIdList = TblRcdId.Split(',');
                    for (int i = 0; i < TblRcdIdList.Count()-1; i++)
                    {

                        strSql.AppendFormat(" TblRadId ={0}", TblRcdIdList[i]);
                        if (TblRcdIdList.Count()-2>i)
                        {
                            strSql.AppendFormat(" or ");
                        }
                        

                    }
                }

                
                pageInfo.Where = strSql.ToString();
                //查询数据
                result = CommonHelper.ListPageResult<Bitshare.PTMM.Model.TblAttachment>(pageInfo);
            }
            catch (Exception ex)
            {
                LogManager.Error("GetAttachmentPageResult()", ex);
            }
            return result;
        }
        #endregion


        #region 工作联系单

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
        public PageResult GetWorkContactPageResult(string loginName, string UserName, string authority, int page, int rows, string orderBy, string keyword, string ContactDeper, string ContactConten, string startDate, string EndDate, string Type = "")
        {
            PageResult result = new PageResult();
            try
            {
                PageInfo pageInfo = new PageInfo();
                pageInfo.CurrentPageIndex = page;
                pageInfo.PageSize = rows;
                pageInfo.QueryFields = "*";
                pageInfo.Orderby = orderBy;
                if (Type=="2")
                {
                    pageInfo.TableName = "View_ContactReply";
                }
                else
                {
                    pageInfo.TableName = "TblContact";
                }
                
                StringBuilder strSql = new StringBuilder("1=1");
                if (!string.IsNullOrEmpty(Type))
                {
                    if (Type == "0")
                    {
                        strSql.AppendFormat(" and (State ='{0}' or State is NULL) and CreateName='{1}'", Type, UserName);
                    }
                    else if (Type == "1")
                    {
                        strSql.AppendFormat(" and State ='{0}' and (CreateName='{1}')", Type, UserName);
                    }
                    else if (Type == "2")
                    {
                        strSql.AppendFormat(" and (LoginName = '{0}')", loginName);
                    }
                }
                //if (!string.IsNullOrEmpty(ContactDeper))
                //{
                //    strSql.AppendFormat(" and CreateNameDeper like '%" + ContactDeper + "%'");
                //}
                if (!string.IsNullOrEmpty(ContactConten))
                {
                    strSql.AppendFormat(" and ContactConten like '%" + ContactConten + "%'");
                }
                if ((!string.IsNullOrEmpty(startDate)))
                {
                    strSql.AppendFormat(" and ContactDate >='{0}' ", startDate);
                }
                if ((!string.IsNullOrEmpty(EndDate)))
                {
                    strSql.AppendFormat(" and ContactDate <='{0}' ", EndDate);
                }
                pageInfo.Where = strSql.ToString();
                //查询数据
                if (Type == "2")
                {
                    result = CommonHelper.ListPageResult<Bitshare.PTMM.Model.View_ContactReply>(pageInfo);
                }
                else
                {
                    result = CommonHelper.ListPageResult<Bitshare.PTMM.Model.TblContact>(pageInfo);
                }
                
            }
            catch (Exception ex)
            {
                LogManager.Error("GetWorkContactPageResult()", ex);
            }
            return result;
        }

        #endregion

        #region 获取查阅时间状态明细

        /// <summary>
        /// 获取明细
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">条数</param>
        /// <param name="queryFields">搜索项,默认全部</param>
        /// <param name="orderBy">排序</param>
        /// <param name="sWhere">条件</param>
        /// <returns></returns>
        public PageResult GetAllPageResult(int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null)
        {

            PageResult result = new PageResult();
            try
            {
                Bitshare.PTMM.Service.DTO.PageInfo page = new Service.DTO.PageInfo();
                page.CurrentPageIndex = pageIndex;
                page.PageSize = pageSize;
                page.QueryFields = queryFields;
                //page.Where = sWhere;
                page.Orderby = orderBy;
                //查询数据
                page.Where = sWhere + " ";
                result = CommonHelper.ListPageResult<View_Announcement>(page);


            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("GetAllPageResult()", ex);
            }
            return result;
        }

        #endregion

    }
    #endregion
}
