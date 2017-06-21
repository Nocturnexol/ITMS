using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bitshare.PTMM.Service.DTO;
using Bitshare.PTMM.Model;
using Bitshare.PTMM.Service.Enum;

namespace Bitshare.PTMM.Service
{
    /// <summary>
    /// 广告合同模块服务
    /// ChenPl
    /// 2016/07/12
    /// </summary>
    public interface IAfterSaleService
    {
        #region 售后服务
        #region 获取车位明细、户外明细、自行车明细
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
        PageResult GetAllPageResult(int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null, string type = null);

       
        #endregion

        #region 车辆修补

        /// <summary>
        /// 车辆修补列表
        /// </summary>
        /// <param name="pager">分页</param>
        /// <param name="loginName">登录名</param>
        /// <returns></returns>
        SubPageResult<tblAdVehicleRepair> GetAdVehicleRepairList(PageInfo pager, string loginName);
        #endregion

        #endregion
    }
}
