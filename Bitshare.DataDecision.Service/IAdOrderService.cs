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
    /// 2016/06/21
    /// </summary>
    public interface IAdOrderService
    {
        /// <summary>
        /// 分页获取媒体预定数据列表(查看所有)
        /// </summary>
        /// <param name="loginName">登录名</param>
        ///  <param name="AuthEnum">权限，例如：查看本人</param>
        /// <param name="pageIndex">查询页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="queryFields">查询字段,默认查询全部</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="keyword">关键字</param>
        /// <param name="seller">业务员</param>
        /// <param name="contractType">合同类型</param>
        /// <param name="dept">部门</param>
        /// <param name="trade">广告行业</param>
        /// <param name="quickType">快捷查询.eg:到期预订单、待办预订单</param>
        /// <returns>返回对象</returns>
        SubPageResult<tblAdOrder> GetAdOrderConfirmationPageResult(string loginName, string look, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string keyword = null,
            string seller = null, string contractType = null, string dept = null, string trade = null, string quickType = null, string AdOrderId=null, string MediaTypeName=null, string AdContent=null, string StartDate = null, string EndDate = null);

        /// <summary>
        /// 分页获取合同管理数据列表(查看所有)
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="pageIndex">查询页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="queryFields">查询字段,默认查询全部</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="keyword">关键字</param>
        /// <param name="seller">业务员</param>
        /// <param name="contractType">合同类型</param>
        /// <param name="dept">部门</param>
        /// <param name="trade">广告行业</param>
        /// <param name="quickType">快捷查询.eg:到期合同</param>
        /// <returns>返回对象</returns>
        SubPageResult<tblAdOrder> GetContractManagementPageResult(string loginName, string look, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string keyword = null,
            string seller = null, string contractType = null, string dept = null, string trade = null, string quickType = null, string AdOrderId = null, string MediaTypeName = null, string AdContent = null, string StartDate = null, string EndDate = null, string RadioAdorder = "");


        ///// <summary> 
        ///// ChenPl
        ///// 2016/06/21
        ///// 根据登录名获取其可以查看的业务员列表
        ///// </summary>
        ///// <param name="loginName">登录名</param>
        ///// <returns>业务员列表</returns>
        //List<string> ListSellerByLoginName(string loginName);


         /// <summary>
        /// 用来产生订单号的方法(订单号)
        /// </summary>
        /// <returns></returns>
        string AutoGenerationOrderID();
        /// <summary>
        /// 分页获取媒体预定数据列表
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="pageIndex">查询页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="queryFields">查询字段,默认查询全部</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="keyword">关键字</param>
        /// <param name="seller">业务员</param>
        /// <param name="contractType">合同类型</param>
        /// <param name="dept">部门</param>
        /// <param name="trade">广告行业</param>
        /// <param name="quickType">快捷查询.eg:到期合同</param>
        /// <returns>返回对象</returns>
        SubPageResult<tblAdOrder> GetMediaOrderPageResult(string loginName, string Look, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "",
            string keyword = null, string seller = null, string contractType = null, string dept = null, string trade = null, string quickType = null, string AdOrderId = null, string MediaTypeName = null, string AdContent = null, string StartDate = null, string EndDate = null);

        #region 合同确认

        #region 媒体明细
        /// <summary>
        /// 根据订单号获取媒体明细
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="page">基础页对象</param>
        /// <returns></returns>
        SubPageResult<tblAdOrderMedialist> GettblAdOrderMedialistPageResult(string orderId, PageInfo page);
        #endregion

        #region 车位明细
        /// <summary>
        /// 根据订单号获取车位明细
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="page">基础页对象</param>
        /// <returns></returns>
        SubPageResult<tblAdFixingList> GettblAdFixinglistPageResult(string orderId, PageInfo page);
        #endregion

        #region 户外媒体明细
        /// <summary>
        /// 根据订单号获取户外媒体明细
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="page">基础页对象</param>
        /// <returns></returns>
        SubPageResult<tblOutdoorAdOrderMedia> GettblOutdoorAdOrderMediaPageResult(string orderId, PageInfo page);
        #endregion

        #region 自行车媒体明细
        /// <summary>
        /// 根据订单号获取自行车媒体明细
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="page">基础页对象</param>
        /// <returns></returns>
        SubPageResult<tblBicycleMadialist> GettblBicycleMadialistPageResult(string orderId, PageInfo page);
        #endregion

        #region 获取款项进度明细
        /// <summary>
        /// 根据订单号获取款项进度明细
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="page">基础页对象</param>
        /// <returns></returns>
        SubPageResult<tblAdOrderFundSet> GettblAdOrderFundSetPageResult(string orderId, PageInfo page);
        #endregion


        #region 获取发票明细
        /// <summary>
        /// 根据订单号获取发票明细
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="page">基础页对象</param>
        /// <returns></returns>
        SubPageResult<tblAdInvoice> GettblAdInvoicePageResult(string orderId, PageInfo page);
        #endregion

        #region 获取到款明细
        /// <summary>
        /// 根据订单号获取到款明细
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="page">基础页对象</param>
        /// <returns></returns>
        SubPageResult<tblAdFinance> GettblAdFinancePageResult(string orderId, PageInfo page);
        #endregion

        #region 获取媒体调整明细
        /// <summary>
        /// 根据订单号获取媒体调整明细
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="page">基础页对象</param>
        /// <returns></returns>
        SubPageResult<tblMessage> GettblMessagePageResult(string orderId, PageInfo page);
        #endregion
        #endregion

        

         /// <summary>
        /// 根据登录名与查看权限获取筛选
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="authority">查看权限</param>
        /// <returns></returns>
        string AuthWhere(string loginName, string authority);

    }
}
