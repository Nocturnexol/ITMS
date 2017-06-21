using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bitshare.PTMM.Service.DTO;
using System.Data;
using Bitshare.PTMM.BLL;
using Bitshare.PTMM.Model;
using Bitshare.PTMM.Common;

namespace Bitshare.PTMM.Service
{
    public interface IMediaDataMaintenanceService
    {
        /// <summary>
        /// 分页获取调线查询列表
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="pageIndex">查询页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="queryFields">查询字段,默认查询全部</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="sWhere">查询条件</param>
        /// <returns>返回对象</returns>
        PageResult GetAdjustLineQueryPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null);

        /// <summary>
        /// 分页获取户外媒体资源列表
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
        PageResult GetOutdoorMediaResourcesPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null, string adOrderId = null, string issueDate = null, string issueEndDate = null);  
        
        /// <summary>
        /// 分页获取户外媒体定义列表
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="pageIndex">查询页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="queryFields">查询字段,默认查询全部</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="sWhere">查询条件</param>
        /// <returns>返回对象</returns>
        PageResult GetOutdoorMediaDefinitionPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null);

        /// <summary>
        /// 分页获取自行车媒体资源列表
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
        PageResult GetBicycleMediaResourcesPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null, string adOrderId = null, string issueDate = null, string issueEndDate = null);

    }
}
