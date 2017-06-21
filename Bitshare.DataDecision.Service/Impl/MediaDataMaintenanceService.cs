using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bitshare.PTMM.Service.DTO;
using System.Data;
using Bitshare.PTMM.BLL;
using Bitshare.PTMM.Model;
using Bitshare.PTMM.Common;

namespace Bitshare.PTMM.Service.Impl
{
    #region 工厂类定义
    /// <summary>
    /// 
    /// </summary>
    /// 
    public class MediaDataMaintenanceServiceFactory
    {
        static IMediaDataMaintenanceService MediaDataMaintenance;
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <returns></returns>
        public IMediaDataMaintenanceService GetMediaDataMaintenance()
        {
            if (MediaDataMaintenance == null)
            {

                MediaDataMaintenance = new MediaDataMaintenanceService();
            }
            return MediaDataMaintenance;
        }
    }
    #endregion

    #region 实现类
        public class MediaDataMaintenanceService : IMediaDataMaintenanceService
        {
            /// <summary>
            /// 拼装分页参数对象
            /// </summary>
            /// <param name="loginName">登录名</param>
            /// <param name="pageIndex">查询页码</param>
            /// <param name="pageSize">分页大小</param>
            /// <param name="queryFields">查询参数，默认全部</param>
            /// <param name="orderBy">排列书序</param>
            /// <param name="sWhere">查询条件</param>
            /// <returns></returns>
            private PageInfo GetPageInfo(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null)
            {
                PageInfo page = new PageInfo();
                page.CurrentPageIndex = pageIndex;
                page.PageSize = pageSize;
                page.QueryFields = queryFields;
                if (String.IsNullOrWhiteSpace(orderBy))
                {
                    orderBy = " TblRcdId Desc";
                }
                page.Orderby = orderBy;
                StringBuilder where = new StringBuilder(sWhere);
                page.Where = where.ToString();
                return page;
            }

            #region 调线查询    
            /// <summary>
            /// 分页获取调线查询数据列表(查看所有)
            /// </summary>
            /// <param name="loginName">登录名</param>
            /// <param name="pageIndex">查询页码</param>
            /// <param name="pageSize">分页大小</param>
            /// <param name="queryFields">查询字段,默认查询全部</param>
            /// <param name="orderBy">排序条件</param>
            /// <param name="sWhere">查询条件</param>
            /// <returns>返回对象</returns>
            public PageResult GetAdjustLineQueryPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null)
            {
                PageResult result = new PageResult();
                try
                {
                    PageInfo page = GetPageInfo(loginName, pageIndex, pageSize, queryFields, orderBy, sWhere);
                    result = CommonHelper.ListPageResult<Bitshare.PTMM.Model.tblVehicleChangeRoad>(page);
                }
                catch (Exception ex)
                {
                    Bitshare.PTMM.Common.LogManager.Error("GetAdjustLineQueryPageResult", ex);
                }
                return result;
            }

            #endregion

            #region 户外媒体资源
            /// <summary>
            /// 分页获取户外媒体资源数据列表(查看所有)
            /// </summary>
            /// <param name="loginName">登录名</param>
            /// <param name="pageIndex">查询页码</param>
            /// <param name="pageSize">分页大小</param>
            /// <param name="queryFields">查询字段,默认查询全部</param>
            /// <param name="orderBy">排序条件</param>
            /// <param name="sWhere">查询条件</param>
            /// <param name="adOrderId"></param>
            /// <param name="issueDate"></param>
            /// <param name="issueEndDate"></param>
            /// <returns>返回对象</returns>
            public PageResult GetOutdoorMediaResourcesPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null, string adOrderId = null, string issueDate = null, string issueEndDate = null)
            {
                PageResult result = new PageResult();
                try
                {
                    if (!string.IsNullOrWhiteSpace(adOrderId))
                    {

                        sWhere += " and OutDoorMadieNumbering not in( select OutDoorMediaNumbering from tblOutdoorAdOrderMedia where IssueDate<='" + issueEndDate + "' and Enddate>='" + issueDate + "' and contractflag=1 )";

                        sWhere += " and OutDoorMadieNumbering not in(select  OutDoorMediaNumbering from tblOutdoorAdOrderMedia where AdOrderId ='" + adOrderId + "' and IssueDate<='" + issueEndDate + "' and Enddate>='" + issueDate + "' )";
                    }
                    PageInfo page = GetPageInfo(loginName, pageIndex, pageSize, queryFields, orderBy, sWhere);
                    result = CommonHelper.ListPageResult<Bitshare.PTMM.Model.tblOutDoor_Media>(page);
                }
                catch (Exception ex)
                {
                    Bitshare.PTMM.Common.LogManager.Error("GetOutdoorMediaResourcesPageResult", ex);
                }
                return result;
            }

            #endregion

            #region 户外媒体定义
            /// <summary>
            /// 分页获取户外媒体定义数据列表(查看所有)
            /// </summary>
            /// <param name="loginName">登录名</param>
            /// <param name="pageIndex">查询页码</param>
            /// <param name="pageSize">分页大小</param>
            /// <param name="queryFields">查询字段,默认查询全部</param>
            /// <param name="orderBy">排序条件</param>
            /// <param name="sWhere">查询条件</param>
            /// <returns>返回对象</returns>
            public PageResult GetOutdoorMediaDefinitionPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null)
            {
                PageResult result = new PageResult();
                try
                {
                    PageInfo page = GetPageInfo(loginName, pageIndex, pageSize, queryFields, orderBy, sWhere);
                    result = CommonHelper.ListPageResult<Bitshare.PTMM.Model.tblOutDoorOrderBy>(page);
                }
                catch (Exception ex)
                {
                    Bitshare.PTMM.Common.LogManager.Error("GetOutdoorMediaDefinitionPageResult", ex);
                }
                return result;
            }

            #endregion

            #region 自行车媒体资源
            /// <summary>
            /// 分页获取户外媒体定义数据列表(查看所有)
            /// </summary>
            /// <param name="loginName">登录名</param>
            /// <param name="pageIndex">查询页码</param>
            /// <param name="pageSize">分页大小</param>
            /// <param name="queryFields">查询字段,默认查询全部</param>
            /// <param name="orderBy">排序条件</param>
            /// <param name="sWhere">查询条件</param>
            /// <returns>返回对象</returns>
            public PageResult GetBicycleMediaResourcesPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null, string adOrderId = null, string issueDate = null, string issueEndDate = null)
            {
                PageResult result = new PageResult();
                try
                {
                    if (!string.IsNullOrWhiteSpace(adOrderId))
                    {

                        sWhere += " and OUTDOORMADIENUMBERING not in( select OutDoorMediaNumbering from tblBicycleMadialist where IssueDate<='" + issueEndDate + "' and Enddate>='" + issueDate + "' and contractflag=1 )";

                        sWhere += " and OUTDOORMADIENUMBERING not in(select  OutDoorMediaNumbering from tblBicycleMadialist where AdOrderId ='" + adOrderId + "' and IssueDate<='" + issueEndDate + "' and Enddate>='" + issueDate + "' )";
                    }
                    PageInfo page = GetPageInfo(loginName, pageIndex, pageSize, queryFields, orderBy, sWhere);
                    result = CommonHelper.ListPageResult<Bitshare.PTMM.Model.tblBicycle_Madia>(page);
                }
                catch (Exception ex)
                {
                    Bitshare.PTMM.Common.LogManager.Error("GetBicycleMediaResourcesPageResult", ex);
                }
                return result;
            }

            #endregion
        }
    #endregion
    
}
