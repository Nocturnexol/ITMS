using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bitshare.PTMM.Service.DTO;
using Bitshare.PTMM.Model;
using System.Collections;

namespace Bitshare.PTMM.Service
{
    /// <summary>
    /// 流程管理
    /// </summary>
    public interface IProcessService
    {
        /// <summary>
        /// 获取当前流程所需展示的数据集
        /// </summary>
        /// <param name="ProcessId">流程编号</param>
        /// <param name="AdOrderId">订单编号</param>
        /// <param name="ProcessName">流程名称</param>
        /// <param name="UserSys">用户信息</param>
        /// <param name="UserSys">节点名称</param>
        /// <returns>ProcessListModel</returns>
        ProcessListModel GetProcessList(string ProcessId,string AdOrderId, string ProcessName, tblUser_Sys UserSys, string NodeName);

        /// <summary>
        /// 获取流程在页面展示时所需的条件字段
        /// </summary>
        /// <param name="ProcessId">流程编号</param>
        /// <param name="ProcessName">流程名称</param>
        /// <param name="AdOrderId">订单号</param>
        /// <param name="UserSys">用户信息</param>
        /// <returns>ProcessListModel</returns>
        ProcessListModel GetProcessShowData(string ProcessId, string ProcessName, string AdOrderId, tblUser_Sys UserSys);

        /// <summary>
        /// 保存节点数据，并根据节点对相关表状态进行更新
        /// </summary>
        /// <param name="ProcessName">流程编号</param>
        /// <param name="NodeName">节点名称</param>
        /// <param name="ProcessId">流程id</param>
        /// <param name="AdOrderId">订单id</param>
        /// <returns></returns>
        ReturnMessageModel SaveNodeInfo(string ProcessName, string NodeName, string ProcessId, string AdOrderId, string Opinion, string[] NextNodeName, tblUser_Sys US, string UserName, string LoginName);


        /// <summary>
        /// 撤销流程操作
        /// </summary>
        /// <param name="ProcessName">流程名称</param>
        /// <param name="NodeName">节点名称</param>
        /// <param name="ProcessId">流程编号</param>
        /// <param name="AdOrderId">合同号</param>
        /// <param name="UserName">用户类</param>
        /// <param name="Opinion">意见</param>
        /// <returns></returns>
        Hashtable RevocationProcess(string ProcessName, string NodeName, string ProcessId, string AdOrderId, tblUser_Sys UserName, string Opinion);

        /// <summary>
        /// 在执行保存前，对数据进行判断操作。
        /// </summary>
        /// <param name="ProcessName">流程名称</param>
        /// <param name="NodeName">节点名称</param>
        /// <param name="ProcessId">流程id</param>
        /// <param name="AdOrderId">订单id</param>
        /// <returns></returns>
        ReturnMessageModel JudgeProcessData(string ProcessName, string NodeName, string ProcessId, string AdOrderId);


        /// <summary>
        /// 根据节点名称，获取下个节点的接收节点下拉数据
        /// </summary>
        /// <param name="NodeName">流程节点</param>
        /// <param name="ProcessName">流程名称</param>
        /// <param name="ProcessId">流程编号</param>
        /// <returns>eg:媒体预定-客户经理-zq</returns>
        List<string> GetNextRoelUserList(string NodeName, string ProcessName, string ProcessId,string AdOrderId);

        /// <summary>
        /// 根据订单和流程编号获取添加流程提示信息
        /// </summary>
        /// <param name="ProcessName">流程名称</param>
        /// <param name="NodeName">节点名称</param>
        /// <param name="ProcessId">流程id</param>
        /// <param name="AdOrderId">订单id</param>
        /// <returns>ture/false</returns>
        bool SaveMessage(string ProcessName, string NodeName, string ProcessId, string AdOrderId);
        /// <summary>
        /// 分页获取合同详细
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="pageIndex">查询页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="queryFields">查询字段,默认查询全部</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="SWhere">查询条件</param>
        /// <returns>返回对象</returns>
        SubPageResult<tblProcessList> GetProcessListPageResult(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "",
            string SWhere = null);
        /// <summary>
        /// 分页获取合同详细
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="pageIndex">查询页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="queryFields">查询字段,默认查询全部</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="SWhere">查询条件</param>
        /// <returns>返回对象</returns>
        SubPageResult<tblProcesslist_Temp> GetOldProcessListPageData(string loginName, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string SWhere = null);
    }
}
