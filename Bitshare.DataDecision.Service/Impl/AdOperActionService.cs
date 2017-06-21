using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bitshare.PTMM.Service.DTO;
using Bitshare.PTMM.Service.Impl;
using Bitshare.PTMM.Service;
using System.Data;
using System.Reflection;
using Bitshare.PTMM.Common;
using Bitshare.PTMM.Service.Enum;
using Bitshare.PTMM.Model;


namespace Bitshare.PTMM.Service.Impl
{
    #region 工厂定义
    /// <summary>
    /// lq
    /// 2016/1/22
    /// </summary>
    public class AdOperActionServiceFactory
    {
        static IAdOperActionService Instance;
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <returns></returns>
        public IAdOperActionService GetInstance()
        {
            if (Instance == null)
            {
                Instance = new AdOperActionService();
            }
            return Instance;
        }
    }

    #endregion

    #region 实现方法
    /// <summary>
    /// 广告合同模块服务
    /// ChenPl
    /// 2016/6/21
    /// </summary>
    internal class AdOperActionService : IAdOperActionService
    {
        static CommonServiceFactory factory = new CommonServiceFactory();
        ICommonService service = factory.GetInstance();

        /// <summary>
        ///拼装分页参数对象
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="authority">业务权限.eg:查看所有、查看下级、查看本人</param>
        /// <param name="pageIndex">查询页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="queryFields">查询字段,默认查询全部</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="keyword">关键字</param>
        /// <param name="Trade">行业</param>
        /// <param name="Seller">业务员</param>
        /// <param name="AdClientele">客户名称</param>
        /// <param name="AdContent">广告品牌</param>
        /// <returns></returns>
        private PageInfo GetPageInfo(string loginName, string authority, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", 
            string Trade = null, string Seller = null, string AdClientele = null, string AdContent = null)
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
            StringBuilder where = new StringBuilder(" 1=1");
            if (!string.IsNullOrWhiteSpace(Seller))
            {
                where.Append(" and Seller= '" + Seller + "'");
            }
            else
            {
                where.Append(AuthWhere(loginName, authority));
            }

            if (!string.IsNullOrWhiteSpace(Trade))
            {
                //行业
                where.Append(" and (INDUSTRYTYPE= '" + Trade + "')");
            }
            if (!string.IsNullOrWhiteSpace(AdClientele))
            {
                //客户名称
                where.Append(" and (SUBSCRIBER like '%" + AdClientele + "%')");
            }
            if (!string.IsNullOrWhiteSpace(AdContent))
            {
                //广告品牌
                where.Append(" and (BRAND= '" + AdContent + "')");
            }
            
            page.Where = where.ToString();
            return page;
        }



        /// <summary>
        /// 根据登录名与查看权限获取筛选
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="authority">查看权限</param>
        /// <returns></returns>
        public string AuthWhere(string loginName, string authority)
        {
            StringBuilder where = new StringBuilder("1=1");
            tblUser_Sys user = CommonHelper.GetUserByLoginName(loginName);
            if (authority == "查看本人")
            {
                where.Append(" and (Seller= '" + user.UserName + "')");
            }
            else if (authority == "查看下级")
            {
                List<string> SellerXia = CommonHelper.ListUnderling(loginName);
                string CdtSeller = " (";
                CdtSeller += String.Join("", SellerXia.Select(p => "'" + p + "',").ToList());
                CdtSeller += "'" + user.UserName + "')";
                where.Append(" and (Seller in " + CdtSeller + ")");
            }
            else if (authority == "查看本部门")
            {
                string Sql = " select UserName from tblUser_Sys where dept_New in (select dept_New from tblUser_Sys where loginName='" + loginName + "')";
                DataTable DT = DBContext.PTMMHZ.GetDataTable(Sql);
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    string UserName = Convert.ToString(DT.Rows[i]["UserName"]);
                    where.Append(" and (Seller= '" + UserName + "')");
                }
            }
            return where.ToString();
        }


        /// <summary>
        ///
        /// 
        /// 
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
        /// <returns>返回对象</returns>
        public PageResult GetCustomerPageResult(string loginName, string Look, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string Trade = null, string Seller = null, string AdClientele = null, string AdContent = null)
        {
            PageResult result = new PageResult();
            try
            {
                //PageInfo page = GetPageInfo(loginName, Look, pageIndex, pageSize, queryFields, orderBy,Trade, Seller, AdClientele, AdContent);
                //result = CommonHelper.l<tblCustomer>(page);
            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("GetCustomerPageResult", ex);
            }
            return result;
        }

        #region 日常工作记录
        /// <summary>
        /// 分页获取 日常工作记录
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="pageIndex">查询页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="queryFields">查询字段,默认查询全部</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="Seller">业务员</param>
        /// <param name="Actor">客户名称</param>
        /// <param name="Authority">查看权限的限制</param>
        /// <returns>返回分页结果对象</returns>
        public PageResult GetCalendarPageResult(int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string Seller = null, string Actor = null, string loginName = "", string Authority = "",string Year=null,string Month=null)
        {
            PageResult result = new PageResult();
            try
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
                StringBuilder where = new StringBuilder(" 1=1");
                if (!string.IsNullOrWhiteSpace(Seller))
                {
                    where.Append(" and Seller = '" + Seller + "'");
                }
                if (!string.IsNullOrWhiteSpace(Actor))
                {
                    where.Append(" and Actor like '%" + Actor + "%'");
                }
                if (!string.IsNullOrWhiteSpace(Year))
                {
                    where.Append(" and year(AffairTime) = '" + Year + "'");
                }
                if (!string.IsNullOrWhiteSpace(Month))
                {
                    where.Append(" and month(AffairTime) = '" + Month + "'");
                }
                if (Authority == AuthEnum.查看所有.ToString())
                {
                    string authority = "查看所有";
                    where.AppendFormat(" and {0}", AuthWhere(loginName, authority));

                }
                else if (Authority == AuthEnum.查看下级.ToString())
                {
                    string authority = "查看下级";
                    where.AppendFormat(" and {0}", AuthWhere(loginName, authority));
                }
                else if (Authority == AuthEnum.查看本人.ToString())
                {
                    string authority = "查看本人";
                    where.AppendFormat(" and {0}", AuthWhere(loginName, authority));
                }


                page.Where = where.ToString();
                //查询数据
                result = CommonHelper.ListPageResult<Bitshare.PTMM.Model.tblCalendar>(page);
            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("GetCalendarPageResult()", ex);
            }
            return result;
        }

        /// <summary>
        /// 根据条件获取日常工作记录数据下载地址
        /// </summary>
        /// <param name="ids">id列表,id列表为空则根据其他条件查询</param>
        /// <param name="Seller">业务员</param>
        /// <param name="Actor">客户名称</param>
        /// <param name="queryFields">查询字段,默认查询全部</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns>返回下载文件地址</returns>
        public string GetDownloadPathCalendar(List<int> ids, string Seller = null, string Actor = null, string queryFields = "*", string orderBy = "")
        {
            string path = string.Empty;
            try
            {
                queryFields = @"Seller as '业务员',Convert(varchar(100),AffairTime,23) as '拜访日期'
                            ,case IsRegister when 1 then '是' else  '否' end as '已登记',Actor as '客户名称',AdContent as '广告品牌'
                            ,Place as '地点'
                            ,Subject as '主题'
                            ,Content as '事件描述'
                            ,ContractName1 as '联系人'
                            ,Convert(varchar(100),RegDate,23) as '登记日期'
                            ,Remark as '备注' ";
                if (String.IsNullOrWhiteSpace(orderBy))
                {
                    orderBy = " TblRcdId Desc";
                }
                StringBuilder where = new StringBuilder(" 1=1");
                if (ids == null || ids.Count == 0)
                {
                    if (!string.IsNullOrWhiteSpace(Seller))
                    {
                        where.Append(" and Seller = '" + Seller + "'");
                    }
                    if (!string.IsNullOrWhiteSpace(Actor))
                    {
                        where.Append(" and Actor like '%" + Actor + "%'");
                    }

                }
                else
                {
                    where.Append(" and TblRcdId in " + DBContext.PTMMHZ.AssemblyInCondition(ids));
                }
                string sql = string.Format("select {0} from tblCalendar where {1} order by {2}", queryFields, where.ToString(), orderBy);
                DataTable dt = DBContext.PTMMHZ.GetDataTable(sql);
                string sheetName = "日常工作记录";
                path = DoExport.ExportDataTableToExcel(dt, sheetName, sheetName);
            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("GetDownloadPathCalendar()", ex);
            }
            return path;
        }
        #endregion
        /// <summary>
        /// 获取客户登记列表
        /// </summary>
        /// <param name="pager">分页</param>
        /// <param name="loginName">登录名</param>
        /// <returns></returns>
        public SubPageResult<tblAdOperationRegist> GetAdOperationRegistList(PageInfo pager, string loginName)
        {
            SubPageResult<tblAdOperationRegist> result = new SubPageResult<tblAdOperationRegist>();
            result = CommonHelper.ListSubPageResult<tblAdOperationRegist>(pager);
            return result;
        }
    }
    #endregion  


 
}
