using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using Bitshare.DataDecision.DBUtility;
using Bitshare.DataDecision.Common;
using Bitshare.DataDecision.Service.DTO;
using System.Data;
using System.Text;
using Bitshare.DataDecision.Model;
using Bitshare.Common;
namespace Bitshare.DataDecision.Service.Impl
{
    #region 工厂定义
    /// <summary>
    /// th
    /// 2016/1/12
    /// </summary>
    public class CommonServiceFactory
    {
        static ICommonService Instance;
        private string serviceName;
        /// <summary>
        /// 
        /// </summary>
        public CommonServiceFactory()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceName"></param>
        public CommonServiceFactory(string serviceName)
        {
            this.serviceName = serviceName;
        }
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <returns></returns>
        internal ICommonService GetInstance()
        {
            if (Instance == null)
            {
                Instance = new CommonService();
            }
            return Instance;
        }
    }

    #endregion

    #region 实现类
    /// <summary>
    /// th
    /// 2016/1/12
    /// </summary>
    internal class CommonService : ICommonService
    {
        /// <summary>
        /// 根据登录名获取其所有下属
        /// hejh
        /// 2016/01/13
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <returns>返回所有下属</returns>
        public List<string> ListUnderling(string loginName)
        {
            List<string> list = new List<string>();
            try
            {
                Bitshare.DataDecision.Model.tblUser_Sys user = CommonHelper.GetUserByLoginName(loginName);
                if (user != null && !String.IsNullOrWhiteSpace(user.UserName))
                {
                    SqlParameter[] paras = new SqlParameter[]{
                         new SqlParameter("@leader",user.UserName)
                     };
                    DataTable dt = DBContext.DataDecision.GetTableByExecProc("pd_GetUnderling", paras);
                    list = dt.AsEnumerable().Select(p => p.Field<string>("sell")).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("GetUnderling", ex);
            }
            return list;
        }



        public List<string> ListDistinctField(string tableName, string columnName, string where = null, string orderBy = null)
        {
            List<string> list = new List<string>();
            try
            {
                string sql = "select distinct " + columnName + " from " + tableName;
                if (!string.IsNullOrWhiteSpace(where))
                {
                    sql += " where " + where;
                }
                if (string.IsNullOrWhiteSpace(orderBy))
                {
                    orderBy = columnName;
                }
                sql += " order by " + orderBy;
                SqlDataReader reader = DbHelperSQL.ExecuteReader(sql);
                while (reader.Read())
                {
                    string fieldValue = Convert.ToString(reader[columnName]);
                    list.Add(fieldValue);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                LogManager.Error("ListDistinctField", ex);
            }
            return list;
        }

        public PageData ListByPage<T>(int pageIndex = 1, int pageSize = 50, string queryFields = "*", string where = "", string orderBy = "") where T : new()
        {
            if (string.IsNullOrWhiteSpace(queryFields))
            {
                queryFields = "*";
            }
            PageData RM = new PageData();
            RM.page = Math.Max(1, pageIndex);
            RM.total = Math.Max(10, pageSize);
            int totalCount = 0;
            List<T> list = new List<T>();
            try
            {
                DataTable dt = DbHelperSQL.QueryPageByProc(typeof(T).Name, orderBy, out totalCount, queryFields, where, RM.page, RM.total);
                list = dt.ToList<T>();
            }
            catch (Exception ex)
            {
                LogManager.Error("ListByPage<" + typeof(T).Name + ">", ex);
            }
            RM.total = (totalCount + pageSize - 1) / pageSize;
            RM.rows = list;
            RM.records = totalCount;

            return RM;
        }

    
        /// <summary>
        /// 根据登录名与查看权限获取筛选
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="authority">查看权限</param>
        /// <returns></returns>
        public  string AuthWhere(string loginName, string authority)
        {
            StringBuilder where = new StringBuilder();
            tblUser_Sys user = CommonHelper.GetUserByLoginName(loginName);
            if (authority == "查看本人")
            {
                where.Append(" and (Seller= '" + user.UserName + "')");
            }
            else if (authority == "查看下级")
            {
                List<string> SellerXia =CommonHelper.ListUnderling(loginName);
                string CdtSeller = " (";
                CdtSeller += String.Join("", SellerXia.Select(p => "'" + p + "',").ToList());
                CdtSeller += "'" + user.UserName + "')";
                where.Append(" and (Seller in " + CdtSeller + ")");
            }
            else if (authority == "查看本部门")
            {
                string Sql = " select UserName from tblUser_Sys where dept_New in (select dept_New from tblUser_Sys where loginName='" + user.LoginName + "')";
                DataTable DT = DBContext.DataDecision.GetDataTable(Sql);
                List<string> SellerXia = new List<string>();
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    string UserName = Convert.ToString(DT.Rows[i]["UserName"]);
                    SellerXia.Add(UserName);
                }
                where.Append(" and (Seller in " + DBContext.DataDecision.AssemblyInCondition(SellerXia) + ")");
            }
            return where.ToString();
        }

    }
    #endregion
}
