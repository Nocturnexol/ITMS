using Bitshare.PTMM.Common;
using Bitshare.PTMM.Model;
using Bitshare.PTMM.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bitshare.PTMM.Service.Enum;
namespace Bitshare.PTMM.Service
{
    /// <summary>
    /// 销售过程管理
    /// 
    /// lq
    /// 2016-01-22
    /// </summary>
    public interface IAdOperActionService
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
        PageResult GetCustomerPageResult(string loginName, string look, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string Trade = null, string Seller = null, string AdClientele = null, string AdContent = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="authority"></param>
        /// <returns></returns>
        string AuthWhere(string loginName, string authority);




        #region 日常工作记录
        /// <summary>
        /// 分页获取日常工作记录
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="pageIndex">查询页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="queryFields">查询字段,默认查询全部</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="Seller">业务员</param>
        /// <param name="Actor">客户名称</param>
        /// <returns>返回分页结果对象</returns>
        PageResult GetCalendarPageResult(int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string Seller = null, string Actor = null, string loginName = "", string Authority = "",string Year=null,string Month=null);
        /// <summary>
        /// 根据条件获取日常工作记录数据下载地址
        /// </summary>
        /// <param name="ids">id列表,id列表为空则根据其他条件查询</param>
        /// <param name="ScreenAttributeName">上画属性</param>
        /// <param name="queryFields">查询字段,默认查询全部</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns>返回下载文件地址</returns>
        string GetDownloadPathCalendar(List<int> ids, string Seller = null, string Actor = null, string queryFields = "*", string orderBy = "");
        ///// <summary>
        ///// 根据客户名称列表
        ///// </summary>
        ///// <param name="loginName">用户名</param>
        ///// <param name="Seller">业务员</param>
        ///// <returns></returns>
        //List<string> ListActor(string loginName, string Seller);
        ///// <summary>
        ///// 根据联系人
        ///// </summary>
        ///// <param name="Actor">客户名称</param>
        ///// <param name="loginName">用户名</param>
        ///// <returns></returns>
        //List<string> ListContractName1(string Actor, string loginName);
        #endregion
        /// <summary>
        /// 获取客户登记列表
        /// </summary>
        /// <param name="pager">分页</param>
        /// <param name="loginName">登录名</param>
        /// <returns></returns>
        SubPageResult<tblAdOperationRegist> GetAdOperationRegistList(PageInfo pager, string loginName);

    }


}
