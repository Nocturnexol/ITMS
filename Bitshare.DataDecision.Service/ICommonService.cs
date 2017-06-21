using System.Collections.Generic;
using Bitshare.DataDecision.Service.DTO;
using Bitshare.DataDecision.Service.Enum;

namespace Bitshare.DataDecision.Service
{
    /// <summary>
    /// 公共服务
    /// 
    /// wangl
    /// 2016/1/8
    /// </summary>
    internal interface ICommonService
    {
        /// <summary>
        /// 获取表中字段Distinct(单字段)放在帮助类中实现.Serice不暴露数据库
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">字段</param>
        /// <param name="where">过滤条件</param>
        /// <param name="orderBy">过滤条件</param> 
        /// <returns></returns>
        List<string> ListDistinctField(string tableName, string columnName, string where = null, string orderBy = null);

        /// <summary>
        /// 获取分页数据对象
        /// </summary>
        /// <typeparam name="T">返回实体类型</typeparam>
        /// <param name="pageIndex">查询页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="queryFields">查询字段,默认查询全部</param>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        PageData ListByPage<T>(int pageIndex = 1, int pageSize = 50, string queryFields = "*", string where = "", string orderBy = "") where T : new();

       

        ///// <summary>
        ///// 得到用户的业务权限
        ///// tanh
        ///// 2016/01/14
        ///// </summary>
        ///// <param name="loginName">登录名</param>
        ///// <param name="Func">业务名称</param>
        ///// <returns>查看权限枚举.eg:查看所有;查看下级;</returns>
        //AuthEnum GetBusinessAuthority(string loginName, string Func);
       /// <summary>
        /// 根据登录名与查看权限获取筛选
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="authority">查看权限</param>
        /// <returns></returns>
        string AuthWhere(string loginName, string authority);


        /// <summary>
        /// 根据登录名获取其所有下属
        /// hejh
        /// 2016/01/13
        /// </summary>
        /// <param name="loginName">用户名</param>
        /// <returns></returns>
        List<string> ListUnderling(string loginName);

    }
}
