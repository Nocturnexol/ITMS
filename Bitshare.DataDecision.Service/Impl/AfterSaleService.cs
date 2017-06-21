using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bitshare.PTMM.Service.DTO;
using Bitshare.PTMM.Service.Enum;
using Bitshare.PTMM.Model;
using Bitshare.PTMM.Common;
using System.Data;

namespace Bitshare.PTMM.Service.Impl
{
    #region 工厂定义
    /// <summary>
    /// ChenPl
    /// 2016/7/12
    /// </summary>
    public class AfterSaleServiceFactory
    {
        static IAfterSaleService Instance;
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <returns></returns>
        public IAfterSaleService GetInstance()
        {
            if (Instance == null)
            {
                Instance = new AfterSaleService();
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
    internal class AfterSaleService : IAfterSaleService
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
        /// <param name="seller">业务员</param>
        /// <param name="contractType">合同类型</param>
        /// <param name="dept">部门</param>
        /// <param name="trade">广告行业</param>
        /// <returns></returns>
        private PageInfo GetPageInfo(string loginName, string authority, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "",
            string keyword = null, string seller = null, string contractType = null, string dept = null, string trade = null, string AdOrderId = null, string MediaTypeName = null, string AdContent = null, string StartDate = null, string EndDate = null)
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
            //if (!string.IsNullOrWhiteSpace(keyword))
            //{
            //    where.Append(" and (AdOrderId like '%" + keyword + "%' or ContractId like '%" + keyword + "%' or AdClientele like '%" + keyword + "%' or AdContent like '%" + keyword + "%')");
            //}
            if (!string.IsNullOrWhiteSpace(seller))
            {
                where.Append(" and Seller= '" + seller + "'");
            }
            else
            {
                where.Append(AuthWhere(loginName, authority));
            }
            if (!string.IsNullOrWhiteSpace(StartDate) && !string.IsNullOrWhiteSpace(EndDate))
            {
                where.Append(" and InsureEndDate<='" + EndDate + "' and InsureIssueDate>='" + StartDate + "'");
            }
            if (!string.IsNullOrWhiteSpace(contractType))
            {
                where.Append(" and (ContractType= '" + contractType + "')");
            }
            //if (!string.IsNullOrWhiteSpace(dept))
            //{
            //    where.Append(" and (Dept= '" + dept + "')");
            //}
            //if (!string.IsNullOrWhiteSpace(trade))
            //{
            //    where.Append(" and (Trade= '" + trade + "')");
            //}
            if (!string.IsNullOrWhiteSpace(AdOrderId))
            {
                where.Append(" and (AdOrderId like '%" + AdOrderId + "%')");
            }
            if (!string.IsNullOrWhiteSpace(MediaTypeName))
            {
                where.Append(" and (MediaTypeName= '" + MediaTypeName + "')");
            }
            if (!string.IsNullOrWhiteSpace(AdContent))
            {
                where.Append(" and (AdContent like '%" + AdContent + "%')");
            }
            page.Where = where.ToString();
            return page;
        }


        /// <summary>
        /// 根据页面补充分页参数对象
        /// </summary>
        /// <param name="page">分页参数对象</param>
        /// <param name="type">页面枚举</param>
        /// <param name="quickType">快捷查询.eg:到期预订单、待办预订单、过期合同</param>
        /// <param name="loginName">登录名</param>
        private void AppendExtraWhere(ref PageInfo page, AdOrderTypeEnum type, string quickType, string loginName = null, string RadioAdorder = "")
        {
            StringBuilder where = new StringBuilder(" ");
            if (type == AdOrderTypeEnum.MediaOrder) //媒体预定
            {
                where.Append(" AND (ContractSureFlag = 0 OR ContractSureFlag IS NULL) And ContractType!='自备车制作' and ContractType!='媒体制作' and ContractType!='媒体代理'");
            }
            else if (type == AdOrderTypeEnum.AdOrderConfirmation) //合同确认
            {
                string EndAdorderId = " AND (AdOrderId  in (SELECT DISTINCT AdOrderId from tblAdFixinglist where InsureEndDate>CONVERT(VARCHAR(10),GETDATE(),120)) OR AdOrderId NOT IN (SELECT DISTINCT AdOrderId from tblAdFixinglist)) ";
                where.Append(" AND (ContractSureFlag = 0 OR ContractSureFlag IS NULL) and ContractType!='自备车制作' and ContractType!='媒体制作' and ContractType!='媒体代理' AND AdOrderId in (SELECT DISTINCT processid FROM tblProcesslist where ProcessName='媒体预定' and  NodeName='媒体确认' and PassFlag=1)" + EndAdorderId);

            }
            else if (type == AdOrderTypeEnum.ContractManagement)  //合同管理
            {
                if (RadioAdorder == "0")
                {
                    where.Append(" AND ContractSureFlag = 1 AND " +
                        " (AdOrderId in (SELECT distinct AdOrderID from tblAdFixinglist  group by AdOrderId having  max(InsureEndDate)>=CONVERT(VARCHAR(10),GETDATE(),120)) " +
                        " OR AdOrderId in (SELECT distinct AdOrderID from tblOutdoorAdOrderMedia  group by AdOrderId having  max(endDate)>=CONVERT(VARCHAR(10),GETDATE(),120)) OR AdOrderId in (SELECT AdOrderID from tblAdFixinglist_Zi  group by AdOrderId having  max(InsureEndDate)>=CONVERT(VARCHAR(10),GETDATE(),120)) OR AdOrderId in (select distinct AdOrderId from tblAdFixinglist_Zi where AdorderId like 'B%')" +
                        " OR AdOrderId in (SELECT distinct AdOrderID from tblOutdoorAdOrderMedia_Zi  group by AdOrderId having  max(endDate)>=CONVERT(VARCHAR(10),GETDATE(),120))  OR AdOrderId in (SELECT distinct AdOrderID from tblBicycleMadialist  group by AdOrderId having  max(endDate)>=CONVERT(VARCHAR(10),GETDATE(),120)))");
                }
                else
                {
                    where.Append(" AND ContractSureFlag = 1 AND " +
                      " (AdOrderId in (SELECT distinct AdOrderID from tblAdFixinglist  group by AdOrderId having  max(InsureEndDate)<=CONVERT(VARCHAR(10),GETDATE(),120)) " +
                      "  OR AdOrderId in (SELECT distinct AdOrderID from tblOutdoorAdOrderMedia  group by AdOrderId having  max(endDate)<=CONVERT(VARCHAR(10),GETDATE(),120)) or AdOrderId in (SELECT distinct AdOrderID from tblAdFixinglist_Zi  group by AdOrderId having  max(InsureEndDate)<=CONVERT(VARCHAR(10),GETDATE(),120)) OR AdOrderId in (SELECT AdOrderID from tblAdFixinglist_Zi  group by AdOrderId having  max(InsureEndDate)>=CONVERT(VARCHAR(10),GETDATE(),120)) OR AdOrderId in (select distinct AdOrderId from tblAdFixinglist_Zi where AdorderId like 'B%')" +
                      "  OR AdOrderId in (SELECT distinct AdOrderID from tblOutdoorAdOrderMedia_Zi  group by AdOrderId having  max(endDate)<=CONVERT(VARCHAR(10),GETDATE(),120)) OR AdOrderId in (SELECT distinct AdOrderID from tblBicycleMadialist  group by AdOrderId having  max(endDate)>=CONVERT(VARCHAR(10),GETDATE(),120)))");
                }

            }
            else if (type == AdOrderTypeEnum.CorrectingAdOrder)  //改稿
            {
                where.Append(" and (ContractType ='媒体改稿')");
            }
            page.Where += where.ToString();
        }

        /// <summary>
        /// 根据登录名与查看权限获取筛选
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="authority">查看权限</param>
        /// <returns></returns>
        private string AuthWhere(string loginName, string authority)
        {
            StringBuilder where = new StringBuilder();
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
                string Sql = " select UserName from tblUser_Sys where dept_New in (select dept_New from tblUser_Sys where loginName='" + user.LoginName + "')";
                DataTable DT = DBContext.PTMMHZ.GetDataTable(Sql);
                List<string> SellerXia = new List<string>();
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    string UserName = Convert.ToString(DT.Rows[i]["UserName"]);
                    SellerXia.Add(UserName);
                }
                where.Append(" and (Seller in " + DBContext.PTMMHZ.AssemblyInCondition(SellerXia) + ")");
            }
            return where.ToString();
        }

        #region 获取车位明细、户外明细、自行车明细

        /// <summary>
        /// 获取明细
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">条数</param>
        /// <param name="queryFields">搜索项,默认全部</param>
        /// <param name="orderBy">排序</param>
        /// <param name="sWhere">条件</param>
        /// <returns></returns>
        public PageResult GetAllPageResult(int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "", string sWhere = null, string type = null)
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
                if (type == "Vehicle")
                {
                    page.Where = sWhere + " and EndDate>= getdate()";
                    result = CommonHelper.ListPageResult<tblAdFixingList_Monitor>(page);
                }
                else if (type == "Out")
                {
                    page.Where = sWhere + " and EndDate>= getdate()";
                    result = CommonHelper.ListPageResult<tblOutdoorAdOrderMedia_Monitor>(page);
                }
                else if (type == "Bicycle")
                {
                    page.Where = sWhere + " and endDate>= getdate() ";
                    result = CommonHelper.ListPageResult<tblBicycleMadialist_Monitor>(page);
                }

            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("GetAllPageResult()", ex);
            }
            return result;
        }

        #endregion


        #region 车辆修补
        /// <summary>
        /// 车辆修补列表
        /// </summary>
        /// <param name="pager">分页</param>
        /// <param name="loginName">登录名</param>
        /// <returns></returns>
        public SubPageResult<tblAdVehicleRepair> GetAdVehicleRepairList(PageInfo pager, string loginName)
        {
            SubPageResult<tblAdVehicleRepair> result = new SubPageResult<tblAdVehicleRepair>();
            result = CommonHelper.ListSubPageResult<tblAdVehicleRepair>(pager);
            return result;

        }

        #endregion





    }
    #endregion



}
