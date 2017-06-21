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
    /// 2016/6/21
    /// </summary>
    public class AdOrderServiceFactory
    {
        static IAdOrderService Instance;
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <returns></returns>
        public IAdOrderService GetInstance()
        {
            if (Instance == null)
            {
                Instance = new AdOrderService();
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
    internal class AdOrderService : IAdOrderService
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
        private PageInfo GetPageInfo(string loginName, string  authority, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "",
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
            if (!string.IsNullOrWhiteSpace(StartDate))
            {
                where.Append(" and InsureIssueDate>='" + StartDate + "'");
            }
            if (!string.IsNullOrWhiteSpace(EndDate))
            {
                where.Append(" and InsureEndDate<='" + EndDate + "'");
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
        private void AppendExtraWhere(ref PageInfo page, AdOrderTypeEnum type, string quickType, string loginName = null, string RadioAdorder="")
        {
            StringBuilder where = new StringBuilder(" ");
            if (type == AdOrderTypeEnum.MediaOrder) //媒体预定
            {
                where.Append(" AND (ContractSureFlag = 0 OR ContractSureFlag IS NULL) And ContractType!='自备车制作' and ContractType!='媒体制作' and ContractType!='媒体代理' and AdOrderId not in (SELECT DISTINCT processid FROM tblProcesslist where ProcessName='合同确认')");
            }
            else if (type == AdOrderTypeEnum.AdOrderConfirmation) //合同确认
            {
                string EndAdorderId = " AND (AdOrderId  in (SELECT DISTINCT AdOrderId from tblAdFixinglist where InsureEndDate>CONVERT(VARCHAR(10),GETDATE(),120)) OR AdOrderId NOT IN (SELECT DISTINCT AdOrderId from tblAdFixinglist)) ";
                where.Append(" and ContractType = '媒体合同' AND (ContractSureFlag = 0 OR ContractSureFlag IS NULL) and ContractType!='自备车制作' and ContractType!='媒体制作' and ContractType!='媒体代理' AND AdOrderId in (SELECT DISTINCT processid FROM tblProcesslist where ProcessName='媒体预定' and  NodeName='媒体确认' and PassFlag=1)" + EndAdorderId);

            }
            else if (type == AdOrderTypeEnum.ContractManagement)  //合同管理
            {
                if (RadioAdorder=="0")
                {
                    where.Append(" and ContractType = '媒体合同' AND ContractSureFlag = 1 AND " +
                        " (AdOrderId in (SELECT distinct AdOrderID from tblAdFixinglist  group by AdOrderId having  max(InsureEndDate)>=CONVERT(VARCHAR(10),GETDATE(),120)) " +
                        " OR AdOrderId in (SELECT distinct AdOrderID from tblOutdoorAdOrderMedia  group by AdOrderId having  max(endDate)>=CONVERT(VARCHAR(10),GETDATE(),120)) OR AdOrderId in (SELECT AdOrderID from tblAdFixinglist_Zi  group by AdOrderId having  max(InsureEndDate)>=CONVERT(VARCHAR(10),GETDATE(),120)) OR AdOrderId in (select distinct AdOrderId from tblAdFixinglist_Zi where AdorderId like 'B%')" +
                        " OR AdOrderId in (SELECT distinct AdOrderID from tblOutdoorAdOrderMedia_Zi  group by AdOrderId having  max(endDate)>=CONVERT(VARCHAR(10),GETDATE(),120))  OR AdOrderId in (SELECT distinct AdOrderID from tblBicycleMadialist  group by AdOrderId having  max(endDate)>=CONVERT(VARCHAR(10),GETDATE(),120)))");
                }
                else
                {
                    where.Append(" and ContractType = '媒体合同' AND ContractSureFlag = 1 AND " +
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
        public string AuthWhere(string loginName, string authority)
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


        /// <summary>
        /// 分页获取合同确认数据列表(查看所有)
        /// ChenPl
        /// 2016/06/21
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
        /// <param name="quickType">快捷查询.eg:到期预订单、待办预订单</param>
        /// <returns>返回对象</returns>
        public SubPageResult<tblAdOrder> GetAdOrderConfirmationPageResult(string loginName, string Look, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "",
            string keyword = null, string seller = null, string contractType = null, string dept = null, string trade = null, string quickType = null, string AdOrderId = null, string MediaTypeName = null, string AdContent = null, string StartDate = null, string EndDate = null)
        {
            SubPageResult<tblAdOrder> result = new SubPageResult<tblAdOrder>();
            try
            {
                PageInfo page = GetPageInfo(loginName, Look, pageIndex, pageSize, queryFields, orderBy, keyword, seller, contractType, dept, trade, AdOrderId, MediaTypeName, AdContent, StartDate, EndDate);
                AppendExtraWhere(ref page, AdOrderTypeEnum.AdOrderConfirmation, quickType, loginName);
                result = CommonHelper.ListSubPageResult<tblAdOrder>(page);
            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("GetAdOrderConfirmationPageResult", ex);
            }
            return result;
        }

        /// <summary>
        /// 分页获取合同管理数据列表(查看所有)
        /// Chenpl
        /// 2016/06/22
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
        public SubPageResult<tblAdOrder> GetContractManagementPageResult(string loginName, string Look, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "",
            string keyword = null, string seller = null, string contractType = null, string dept = null, string trade = null, string quickType = null, string AdOrderId = null, string MediaTypeName = null, string AdContent = null, string StartDate = null, string EndDate = null, string RadioAdorder = "")
        {
            SubPageResult<tblAdOrder> result = new SubPageResult<tblAdOrder>();
            try
            {
                PageInfo page = GetPageInfo(loginName, Look, pageIndex, pageSize, queryFields, orderBy, keyword, seller, contractType, dept, trade, AdOrderId, MediaTypeName, AdContent, StartDate, EndDate);
                AppendExtraWhere(ref page, AdOrderTypeEnum.ContractManagement, quickType, loginName,RadioAdorder);
                result = CommonHelper.ListSubPageResult<tblAdOrder>(page);
            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("GetContractManagementPageResult", ex);
            }
            return result;
        }

        ///// <summary>
        ///// ChenPl
        ///// 2016/06/21
        ///// 根据登录名获取其可以查看(且有订单)的业务员名称列表
        ///// </summary>
        ///// <param name="loginName">登录名</param>
        ///// <returns>业务员列表.eg:张三、李四</returns>
        //public List<string> ListSellerByLoginName(string loginName)
        //{
        //    List<string> list = new List<string>();
        //    if (!string.IsNullOrWhiteSpace(loginName))
        //    {
        //        string auth = Convert.ToString( service.GetBusinessAuthority(loginName, "订单查询"));
        //        list = CommonHelper.ListDistinctField("tblAdOrder", "Seller", AuthWhere(loginName, auth));
        //    }
        //    return list ?? new List<string>();
        //}
        /// <summary>
        /// 用来产生订单号的方法(订单号)
        /// </summary>
        /// <returns></returns>
        public string AutoGenerationOrderID()
        {
            int Year = DateTime.Now.Year;
            int Month = DateTime.Now.Month;
            string YearStr = Year.ToString();
            string MonthStr = Month.ToString();
            string AdOrderId = "";
            if (MonthStr.Length == 1)
            {
                MonthStr = "0" + MonthStr;
            }
            AdOrderId = YearStr + MonthStr;
            string StrQuery = "SELECT TOP 1 AdOrderId FROM tblAdorder WHERE AdOrderId LIKE '" + YearStr + MonthStr + "%' AND OldData<>1 ORDER BY AdOrderId DESC";
            DataTable dt = DBContext.PTMMHZ.GetDataTable(StrQuery);
            if (dt.Rows.Count > 0)
            {
                string DtAdOrderId = dt.Rows[0]["AdOrderId"].ToString();
                string MAdOrderId = DtAdOrderId.Remove(0, 6);
                int num = Int32.Parse(MAdOrderId);
                string Number = "";
                switch (MAdOrderId.Length)
                {

                    case 1:
                        num += 1;
                        if (num > 10)
                        {
                            Number = "00" + num.ToString();
                            if (Number.Length == 1)
                            {
                                Number = "00" + Number;
                            }
                            else if (Number.Length == 2)
                            {
                                Number = "0" + Number;
                            }
                        }
                        else
                        {
                            Number = "0" + num.ToString();
                            if (Number.Length == 1)
                            {
                                Number = "00" + Number;
                            }
                            else if (Number.Length == 2)
                            {
                                Number = "0" + Number;
                            }
                        }
                        break;
                    case 2:
                        num += 1;
                        if (num > 100)
                        {
                            Number = "0" + num.ToString();
                            if (Number.Length == 1)
                            {
                                Number = "00" + Number;
                            }
                            else if (Number.Length == 2)
                            {
                                Number = "0" + Number;
                            }
                        }
                        else
                        {
                            Number = num.ToString();
                            if (Number.Length == 1)
                            {
                                Number = "00" + Number;
                            }
                            else if (Number.Length == 2)
                            {
                                Number = "0" + Number;
                            }
                        }
                        break;

                    default:
                        num += 1;
                        Number = num.ToString();
                        if (Number.Length == 1)
                        {
                            Number = "00" + Number;
                        }
                        else if (Number.Length == 2)
                        {
                            Number = "0" + Number;
                        }
                        break;
                }
                AdOrderId += Number;
            }
            else
            {
                AdOrderId += "001";
            }

            return AdOrderId;
        }
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
        /// <param name="EndDate">结束日期</param>
        /// <param name="StartDate">开始日期</param>
        /// <param name="Look"></param>
        /// <returns>返回对象</returns>
        public SubPageResult<tblAdOrder> GetMediaOrderPageResult(string loginName, string Look, int pageIndex = 1, int pageSize = 50, string queryFields = "*", string orderBy = "",
            string keyword = null, string seller = null, string contractType = null, string dept = null, string trade = null, string quickType = null, string AdOrderId = null, string MediaTypeName = null, string AdContent = null, string StartDate = null, string EndDate = null)
        {
            SubPageResult<tblAdOrder> result = new SubPageResult<tblAdOrder>();
            try
            {
                PageInfo page = GetPageInfo(loginName, Look, pageIndex, pageSize, queryFields, orderBy, keyword, seller, contractType, dept, trade, AdOrderId, MediaTypeName, AdContent, StartDate, EndDate);
                AppendExtraWhere(ref page, AdOrderTypeEnum.MediaOrder, quickType, loginName);
                result = CommonHelper.ListSubPageResult<tblAdOrder>(page);
            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("GetMediaOrderPageResult", ex);
            }
            return result;
        }

        #region 合同确认

        #region 媒体明细

        /// <summary>
        /// 根据订单单号获取媒体明细
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="page">基础页对象</param>
        /// <returns></returns>
        public SubPageResult<tblAdOrderMedialist> GettblAdOrderMedialistPageResult(string orderId, PageInfo page)
        {
            SubPageResult<tblAdOrderMedialist> result = new SubPageResult<tblAdOrderMedialist>();
            try
            {
                page.Where = " AdorderId='" + orderId + "'";
                result = CommonHelper.ListSubPageResult<tblAdOrderMedialist>(page);
            }
            catch (Exception ex)
            {

                LogManager.Error("GettblAdOrderMedialistPageResult", ex);
            }
            return result;

        }

        #endregion

        #region 车位明细

        /// <summary>
        /// 根据订单单号获取车位明细
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="page">基础页对象</param>
        /// <returns></returns>
        public SubPageResult<tblAdFixingList> GettblAdFixinglistPageResult(string orderId, PageInfo page)
        {
            SubPageResult<tblAdFixingList> result = new SubPageResult<tblAdFixingList>();
            try
            {
                page.Where = " AdorderId='" + orderId + "'";
                result = CommonHelper.ListSubPageResult<tblAdFixingList>(page);
            }
            catch (Exception ex)
            {

                LogManager.Error("GettblAdFixinglistPageResult", ex);
            }
            return result;

        }

        #endregion

        #region 户外媒体明细

        /// <summary>
        /// 根据订单单号获取户外媒体明细
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="page">基础页对象</param>
        /// <returns></returns>
        public SubPageResult<tblOutdoorAdOrderMedia> GettblOutdoorAdOrderMediaPageResult(string orderId, PageInfo page)
        {
            SubPageResult<tblOutdoorAdOrderMedia> result = new SubPageResult<tblOutdoorAdOrderMedia>();
            try
            {
                page.Where = " AdorderId='" + orderId + "'";
                result = CommonHelper.ListSubPageResult<tblOutdoorAdOrderMedia>(page);
            }
            catch (Exception ex)
            {

                LogManager.Error("GettblOutdoorAdOrderMediaPageResult", ex);
            }
            return result;

        }

        #endregion

        #region 自行车媒体明细

        /// <summary>
        /// 根据订单单号获取自行车媒体明细
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="page">基础页对象</param>
        /// <returns></returns>
        public SubPageResult<tblBicycleMadialist> GettblBicycleMadialistPageResult(string orderId, PageInfo page)
        {
            SubPageResult<tblBicycleMadialist> result = new SubPageResult<tblBicycleMadialist>();
            try
            {
                page.Where = " AdorderId='" + orderId + "'";
                result = CommonHelper.ListSubPageResult<tblBicycleMadialist>(page);
            }
            catch (Exception ex)
            {

                LogManager.Error("GettblBicycleMadialistPageResult", ex);
            }
            return result;

        }

        #endregion

        #region 获取款项进度明细

        /// <summary>
        /// 根据订单单号获取款项进度明细
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="page">基础页对象</param>
        /// <returns></returns>
        public SubPageResult<tblAdOrderFundSet> GettblAdOrderFundSetPageResult(string orderId, PageInfo page)
        {
            SubPageResult<tblAdOrderFundSet> result = new SubPageResult<tblAdOrderFundSet>();
            try
            {
                page.Where = " AdOrderId='" + orderId + "'";
                page.Orderby = "OughtDate,OughtNum";
                result = CommonHelper.ListSubPageResult<tblAdOrderFundSet>(page);
            }
            catch (Exception ex)
            {

                LogManager.Error("GettblAdOrderFundSetPageResult", ex);
            }
            return result;

        }

        #endregion

        #region 获取发票明细

        /// <summary>
        /// 根据订单单号获取发票明细
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="page">基础页对象</param>
        /// <returns></returns>
        public SubPageResult<tblAdInvoice> GettblAdInvoicePageResult(string orderId, PageInfo page)
        {
            SubPageResult<tblAdInvoice> result = new SubPageResult<tblAdInvoice>();
            try
            {
                page.Where = " AdOrderid='" + orderId + "'";
                page.Orderby = "MakeDate";
                result = CommonHelper.ListSubPageResult<tblAdInvoice>(page);
            }
            catch (Exception ex)
            {

                LogManager.Error("GettblAdInvoicePageResult", ex);
            }
            return result;

        }

        #endregion

        #region 获取到款明细

        /// <summary>
        /// 根据订单单号获取到款明细
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="page">基础页对象</param>
        /// <returns></returns>
        public SubPageResult<tblAdFinance> GettblAdFinancePageResult(string orderId, PageInfo page)
        {
            SubPageResult<tblAdFinance> result = new SubPageResult<tblAdFinance>();
            try
            {
                page.Where = " AdorderId='" + orderId + "'";
                page.Orderby = "ArriveDate";
                result = CommonHelper.ListSubPageResult<tblAdFinance>(page);
            }
            catch (Exception ex)
            {

                LogManager.Error("GettblAdFinancePageResult", ex);
            }
            return result;

        }

        #endregion

        #region 获取媒体媒体调整明细

        /// <summary>
        /// 根据订单单号获取媒体调整明细
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="page">基础页对象</param>
        /// <returns></returns>
        public SubPageResult<tblMessage> GettblMessagePageResult(string orderId, PageInfo page)
        {
            SubPageResult<tblMessage> result = new SubPageResult<tblMessage>();
            try
            {
                page.Where = " AdorderId='" + orderId + "' and MsgTitle='媒体调整通知'";
                result = CommonHelper.ListSubPageResult<tblMessage>(page);
            }
            catch (Exception ex)
            {

                LogManager.Error("GettblMessagePageResult", ex);
            }
            return result;

        }

        #endregion

        #endregion




        

    }
    #endregion  
}
