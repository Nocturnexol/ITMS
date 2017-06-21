using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bitshare.PTMM.Service.DTO;
using Bitshare.PTMM.Model;
namespace Bitshare.PTMM.Service
{
    public interface IAdOrder_ZiService
    {
         /// <summary>
        /// 保存之前生成新订单号和生成新媒体明细
        /// </summary>
        /// <param name="title"></param>
        /// <param name="AdOrderId"></param>
        /// <param name="MediaTypeName"></param>
        /// <returns></returns>
        string MainGridBeforeSave(string title, string AdOrderId, string MediaTypeName);
        /// <summary>
        /// 获取媒体制作列表数据
        /// </summary>
        /// <param name="pager">分页对象</param>
        /// <param name="loginName">登陆名</param>
        /// <returns></returns>
        SubPageResult<tblAdOrder> GetZAdOrderList(PageInfo pager, string loginName, string title);
        /// <summary>
        /// 根据不同页面条件不同
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        string GetAdOrderCondtion(string title);
        
        /// <summary>
        /// 根据订单单号获取媒体明细
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="page">基础页对象</param>
        /// <returns></returns>
        SubPageResult<tblAdFixingList_Zi> GettblAdFixinglist_ZiPageResult(string orderId, PageInfo page);
         /// <summary>
        /// 根据订单单号获取媒体明细
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="page">基础页对象</param>
        /// <returns></returns>
        SubPageResult<tblAdOrderMedialist_Zi> GettblAdOrderMedia_ZilistPageResult(string orderId, PageInfo page);
        /// <summary>
        /// 根据订单单号获取媒体明细
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="page">基础页对象</param>
        /// <returns></returns>
        SubPageResult<tblOutdoorAdOrderMedia_Zi> GettblOutdoorAdOrderMedia_ZiPageDate(string orderId, PageInfo page);
    }
}
