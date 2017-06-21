using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using Bitshare.DataDecision.Common;
using Bitshare.DataDecision.Service.DTO;
using System.Data;
using Bitshare.DataDecision.DBUtility;
using Bitshare.DataDecision.Model;
using System.Reflection;
using System.Text;
using Bitshare.Common;
namespace Bitshare.DataDecision.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class CommonHelper
    {
        /// <summary>
        /// 获取表中字段Distinct(单字段)放在帮助类中实现.
        /// tanhu
        /// 2016/01/13
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">字段</param>
        /// <param name="where">过滤条件</param>
        /// <param name="orderBy">排序</param> 
        /// <returns></returns>
        public static List<string> ListDistinctField(string tableName, string columnName, string where = null, string orderBy = null)
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
                SqlDataReader reader = DBContext.DataDecision.GetReader(sql);
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

        /// <summary>
        /// 存储过程分页查询
        /// </summary>
        /// <param name="tableName">表名</param> 
        /// <param name="columnName">排序字段</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="where">查询条件</param>
        public static List<string> QueryOrderBy(string tableName, string columnName, string where = null, string orderBy = null)
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
                DataTable dt = DBContext.DataDecision.GetDataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    list = dt.AsEnumerable().Select(p => Convert.ToString(p[0])).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("SQL-QueryOrderBy", ex);
            }
            return list;
        }
       /// <summary>
       /// 
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="pageIndex"></param>
       /// <param name="pageSize"></param>
       /// <param name="queryFields"></param>
       /// <param name="where"></param>
       /// <param name="orderBy"></param>
       /// <returns></returns>
        public static PageData ListByPage<T>(int pageIndex = 1, int pageSize = 50, string queryFields = "*", string where = "", string orderBy = "") where T : new()
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
            //RM.total = totalCount;
            RM.rows = list;
           // RM.pageCount = (totalCount + pageSize - 1) / pageSize;

            return RM;
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="T">Model中名称必须和数据库表名一致</typeparam>
        /// <param name="page">数据参数</param>
        /// <returns></returns>
        public static PageResult  ListPageResult<T>(PageInfo page) where T : new()
        {
            PageResult result = new PageResult();
            try
            {
                List<T> list = new List<T>();
                int totalCount = 0;
                //获取数据
                DataTable ds = DBContext.DataDecision.QueryPageByProc(typeof(T).Name, page.Orderby, out totalCount, page.QueryFields, page.Where, page.CurrentPageIndex, page.PageSize);
                //对数据进行封装
                result.page = page.CurrentPageIndex;
                result.rows = ds.ToList<T>();
                result.total = (totalCount + page.PageSize - 1) / page.PageSize;
                result.records = totalCount;
            }
            catch (Exception ex)
            {
                LogManager.Error("ListPageResult<" + typeof(T).Name + ">", ex);
            }
            return result;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="T">Model中名称必须和数据库表名一致</typeparam>
        /// <param name="page">数据参数</param>
        /// <returns></returns>
        public static SubPageResult<T> ListSubPageResult<T>(PageInfo page) where T : new()
        {
            SubPageResult<T> result = new SubPageResult<T>();
            try
            {
                List<T> list = new List<T>();
                int totalCount = 0;
                if (String.IsNullOrWhiteSpace(page.Orderby))
                {
                    if (!String.IsNullOrWhiteSpace(page.Sidx))
                    {
                        page.Orderby = page.Sidx + " " + page.Sord;
                    }

                    else
                    {
                        page.Orderby = "Rid desc";
                    }
                }
                //获取数据
                DataTable ds = DBContext.DataDecision.QueryPageByProc(typeof(T).Name, page.Orderby, out totalCount, "*", page.Where, page.CurrentPageIndex, page.PageSize);
                //对数据进行封装
                result.page = page.CurrentPageIndex;
                result.rows = ds.ToList<T>();
                result.total = (totalCount + page.PageSize - 1) / page.PageSize;
                result.records = totalCount;
            }
            catch (Exception ex)
            {
                LogManager.Error("ListPageResult<" + typeof(T).Name + ">", ex);
            }
            return result;
        }

        /// <summary>
        /// 根据登录名获取用户信息
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <returns></returns>
        public static tblUser_Sys GetUserByLoginName(string loginName)
        {

            tblUser_Sys user = CacheManager.Get("tblUser_Sys-" + loginName) as tblUser_Sys;
            if (user == null)
            {
                string SQL = "select * from tblUser_Sys where LoginName='" + loginName + "'";
                DataTable DT = DBContext.DataDecision.GetDataTable(SQL);
                List<tblUser_Sys> UserList = DT.ToList<tblUser_Sys>();
                if (UserList != null && UserList.Count > 0)
                {
                    user = UserList[0];
                }
                CacheManager.Insert("tblUser_Sys-" + loginName, user);
            }
            return user ?? new tblUser_Sys();
        }

        /// <summary>
        /// 根据登录名获取用户所有角色集合
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <returns>角色集合</returns>
        public static sys_role ListRolesByLoginName(string loginName)
        {
            sys_role user = CacheManager.Get("sys_role-" + loginName) as sys_role;
            if (user==null)
            {
                string sql = "select * from sys_role where LoginName='" + loginName + "'";
                DataTable Dt = DBContext.DataDecision.GetDataTable(sql);
                List<sys_role> UserList = Dt.ToList<sys_role>();
                if (UserList !=null && UserList.Count > 0)
                {
		            user=UserList[0];
                }
                CacheManager.Insert("sys_role-" + loginName, user);
            }
            
            return user ?? new sys_role();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string BuildExportSql<T>()
        {
            #region 根据Model类中属性构造导出Sql语句，默认全字段导出,sql存入Cache中
            Type type = typeof(T);
            int hashcode = type.GetHashCode();
            string sql = String.Empty;

            sql = DataCache.GetCache("exportsql_" + hashcode) as string;
            if (string.IsNullOrWhiteSpace(sql))
           
            {
                PropertyInfo[] propertys = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                //存在是否导出属性则导出，默认false
               // StringBuilder exportsql = new StringBuilder();

                List<Tuple<string, int>> exportsql = new List<Tuple<string, int>>();
                #region 循环属性
                foreach (PropertyInfo p in propertys)
                {
                    object[] objs = p.GetCustomAttributes(false);
                    foreach (object obj in objs)
                    {
                        if (obj is FieldAttribute)
                        {
                            FieldAttribute field = (FieldAttribute)obj;

                            int orderIndex = 9999;
                            if (field.Order > 0)
                            {
                                orderIndex = field.Order;
                            }
                            
                            if (field.IsExport)
                            {
                                if (p.PropertyType == typeof(bool) || p.PropertyType == typeof(bool?))
                                {
                                   // exportsql.AppendFormat(",(case when {0}=1 then '{1}' else '{2}' end) as '{3}'", p.Name, String.IsNullOrWhiteSpace(field.TrueShow) ? "true" : field.TrueShow, String.IsNullOrWhiteSpace(field.FalseShow) ? "false" : field.FalseShow, String.IsNullOrWhiteSpace(field.Description) ? p.Name : field.Description);
                                    exportsql.Add(new Tuple<string, int>(string.Format("(case when {0}=1 then '{1}' else '{2}' end) as '{3}'", p.Name, String.IsNullOrWhiteSpace(field.TrueShow) ? "true" : field.TrueShow, String.IsNullOrWhiteSpace(field.FalseShow) ? "false" : field.FalseShow, String.IsNullOrWhiteSpace(field.Description) ? p.Name : field.Description), orderIndex));
                                
                                
                                }
                                else if (p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?))
                                {

                                    if (field.DataFormat.ToLower() == "date")
                                    {
                                       // exportsql.AppendFormat(",CONVERT(varchar(10), {0}, 121) as '{1}'", p.Name, String.IsNullOrWhiteSpace(field.Description) ? p.Name : field.Description);

                                        exportsql.Add(new Tuple<string, int>(string.Format("CONVERT(varchar(10), {0}, 121) as '{1}'", p.Name, String.IsNullOrWhiteSpace(field.Description) ? p.Name : field.Description), orderIndex));
                                
                                    }
                                    else if (field.DataFormat.ToLower() == "time")
                                    {
                                        //exportsql.AppendFormat(",CONVERT(varchar(10), {0}, 108) as '{1}'", p.Name, String.IsNullOrWhiteSpace(field.Description) ? p.Name : field.Description);
                                        exportsql.Add(new Tuple<string, int>(string.Format("CONVERT(varchar(10), {0}, 108) as '{1}'", p.Name, String.IsNullOrWhiteSpace(field.Description) ? p.Name : field.Description), orderIndex));
                                
                                    }
                                    else
                                    {
                                       // exportsql.AppendFormat(",CONVERT(varchar(19), {0}, 121) as '{1}'", p.Name, String.IsNullOrWhiteSpace(field.Description) ? p.Name : field.Description);
                                        exportsql.Add(new Tuple<string, int>(string.Format("CONVERT(varchar(19), {0}, 121) as '{1}'", p.Name, String.IsNullOrWhiteSpace(field.Description) ? p.Name : field.Description), orderIndex));
                                
                                    }
                                }
                                else
                                {
                                  //  exportsql.AppendFormat(",{0} as '{1}'", p.Name, String.IsNullOrWhiteSpace(field.Description) ? p.Name : field.Description);
                                    exportsql.Add(new Tuple<string, int>(string.Format("{0} as '{1}'", p.Name, String.IsNullOrWhiteSpace(field.Description) ? p.Name : field.Description), orderIndex));
                                
                                }
                            }
                            break;
                        }
                    }
                }
                #endregion
                string _sql = "select ";
                if (exportsql.Count > 0)
                {
                    var orderSql = exportsql.OrderBy(p => p.Item2).ToList();
                    _sql += string.Join(",", orderSql.Select(p => p.Item1).ToList());
                }
                else
                {
                    _sql += "* ";
                }
                _sql += String.Format(" from {0} where 1=1 ", type.Name);
                sql = _sql;
                DataCache.SetCache("exportsql_" + hashcode, sql);
            }
            #endregion
            return sql;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Newtonsoft.Json.Linq.JArray InitGridConfig<T>()
        {
            List<GridConfig> configList = null;


            #region 根据Model类中属性构造导出Sql语句，默认全字段导出,sql存入Cache中
            Type type = typeof(T);
            int hashcode = type.GetHashCode();
            string sql = String.Empty;


            configList = DataCache.GetCache("exportsql_" + hashcode) as List<GridConfig>;
            if (configList == null || configList.Count() == 0)
            {
                PropertyInfo[] propertys = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                configList = new List<GridConfig>();
                #region 循环属性
                foreach (PropertyInfo p in propertys)
                {
                    object[] objs = p.GetCustomAttributes(false);
                    foreach (object obj in objs)
                    {
                        if (obj is FieldAttribute)
                        {
                            FieldAttribute field = (FieldAttribute)obj;
                            #region 导出字段以及主键
                            if (field.IsExport || field.IsPrimaryKey)
                            {
                                int orderIndex = 9999;
                                if (field.Order >= 0)
                                {
                                    orderIndex = field.Order;
                                }
                                GridConfig config = new GridConfig();
                                config.OrderNum = orderIndex;
                                config.Name = field.Description;
                                config.Index = p.Name;
                                if (field.IsPrimaryKey)
                                {
                                    config.Hidden = true;
                                    config.IsPrimaryKey = true;
                                }
                                config.Width = field.Width > 0 ? field.Width : 60;
                                if (field.IsFixed)
                                {
                                    config.Fixed = true;
                                }
                                if (field.IsFrozen)
                                {
                                    config.Frozen = true;
                                }
                                #region 格式
                                if (p.PropertyType == typeof(bool))
                                {
                                    config.Formatter = "bool";
                                }
                                else if (p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?))
                                {

                                    if (field.DataFormat.ToLower() == "date")
                                    {
                                        config.Formatter = "date";
                                    }
                                    else if (field.DataFormat.ToLower() == "time")
                                    {
                                        config.Formatter = "time";
                                    }
                                    else
                                    {
                                        config.Formatter = "datetime";
                                    }
                                }
                                else
                                {

                                }
                                #endregion

                                configList.Add(config);
                            }
                            #endregion
                        }
                    }
                }
                #endregion

            }
            configList = configList.OrderBy(p => p.OrderNum).ToList();
            Newtonsoft.Json.Linq.JArray jarray = new Newtonsoft.Json.Linq.JArray();
            foreach (var item in configList)
            {
                Newtonsoft.Json.Linq.JObject obj = new Newtonsoft.Json.Linq.JObject();
                obj["name"] = item.Name;
                obj["index"] = item.Index;
                obj["width"] = item.Width;
                if (!string.IsNullOrWhiteSpace(item.Formatter))
                {
                    obj["formatter"] = item.Formatter;
                }
                if (item.Hidden)
                {
                    obj["hidden"] = item.Hidden;
                }
                if (item.Fixed)
                {
                    obj["fixed"] = item.Fixed;
                }
                if (item.Frozen)
                {
                    obj["frozen"] = item.Frozen;
                }
                if (item.IsPrimaryKey)
                {
                    obj["key"] = item.IsPrimaryKey;
                }
                jarray.Add(obj);
            }
            #endregion

            return jarray;
        }

        /// <summary>
        /// 查询所有list组合信息
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string GetAllInfoByList(List<string> values)
        {
            string str = string.Empty;
            if (values != null && values.Count > 0)
            {
                // 过滤空值和重复项
                var list = values.Where(p => !string.IsNullOrWhiteSpace(p)).Distinct().Select(p => p).ToList();
                if (list.Count > 0)
                {
                    str = string.Join(",", list);
                }
            }
            return str;
        }


        /// <summary>
        /// 根据登录名获取其所有下属
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <returns>返回所有下属</returns>
        public static List<string> ListUnderling(string loginName)
        {
            List<string> list = new List<string>();
            try
            {
                Bitshare.DataDecision.Model.tblUser_Sys user = GetUserByLoginName(loginName);
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
    }
}
