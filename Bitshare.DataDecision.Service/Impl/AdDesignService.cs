using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bitshare.PTMM.Service.DTO;
using Bitshare.PTMM.Model;
using System.Data;
using Bitshare.PTMM.Service.Helper;
using Bitshare.PTMM.Common;

namespace Bitshare.PTMM.Service.Impl
{
    #region 工厂定义
    /// <summary>
    /// lq
    /// 2016/1/22
    /// </summary>
    public class AdDesignServiceFactory
    {
        static   IAdDesignService Instance;
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <returns></returns>
        public IAdDesignService GetInstance()
        {
            if (Instance == null)
            {
                Instance = new AdDesignService();
            }
            return Instance;
        }
    }

    #endregion

    #region 实现方法
    internal class AdDesignService : IAdDesignService
    {
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
                page.Where = sWhere;
                page.Orderby = orderBy;
                //查询数据
                if (type == "媒体明细")
                {
                    
                    result = CommonHelper.ListPageResult<tblAdOrderMediaList_Design>(page);
                }
                else
                if (type=="车位明细")
                {
                   
                    result = CommonHelper.ListPageResult<tblAdFixinglist_Design>(page);
                }else
                if (type == "户外媒体")
                {
                    
                    result = CommonHelper.ListPageResult<tblOutdoorAdOrderMedia_Design>(page);
                }else
                if (type == "自行车媒体")
                {
                    
                    result = CommonHelper.ListPageResult<tblBicycleMadialist_Design>(page);
                }else if (type == "设计稿明细")
                {
                  
                    result = CommonHelper.ListPageResult<tblDesignPhotos>(page);
                }
                
            }
            catch (Exception ex)
            {
                Bitshare.PTMM.Common.LogManager.Error("GettblAdFixinglistPageResult()", ex);
            }
            return result;
        }
        /// <summary>
        /// 获取订单下拉
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="authority"></param>
        /// <returns></returns>
        public List<string> GetAdDesignList(string loginName,string authority)
        {
            List<string> list = new List<string>();

            CommonServiceFactory factory = new CommonServiceFactory();
            ICommonService service = factory.GetInstance();
            StringBuilder where = new StringBuilder(" 1=1 ");
            tblUser_Sys user = CommonHelper.GetUserByLoginName(loginName);
            if (authority == "查看本人")
            {
                where.Append(" and (Seller= '" + user.UserName + "')");
            }
            else if (authority == "查看下级")
            {
                List<string> SellerXia = CommonHelper.ListUnderling(user.LoginName);
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
            where.AppendFormat(@"and (adorderid in (select distinct  adorderid from tblAdOrderMedialist where InsureEndDate>='{0}') 
                                or adorderid in (select distinct  adorderid from tblAdFixingList where InsureEndDate>='{0}')
                                or adorderid in (select distinct  adorderid from tblOutdoorAdOrderMedia where EndDate>='{0}')
                                or adorderid in (select distinct  adorderid from tblBicycleMadialist where EndDate>='{0}'))", DateTime.Now);
            list = CommonHelper.ListDistinctField("tblAdOrder", "AdOrderId", where.ToString(), " adorderid desc");

            return list;
        }


        public tblAdOrder GetTblAdOrder(string adOrderId)
        {
            tblAdOrder adOrder = new tblAdOrder();
            try
            {
                StringBuilder strbSql = new StringBuilder();
                strbSql.AppendFormat("  adorderId='{0}'", adOrderId);
                List<tblAdOrder> td = BusinessContext.tblAdOrder.GetModelList(strbSql.ToString());
                if (td.Count > 0)
                {
                    adOrder = td[0];
                }
                //DataTable dt = NBYJ.Web.Common.DBContext.PTMM.GetDataTable(strbSql.ToString());
                //if (dt != null && dt.Rows.Count > 0)
                //    adOrder = dt.ToList<tblAdOrder>()[0];
            }
            catch (Exception)
            {
            }
            return adOrder;
        }
    }
    #endregion
}
