using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Bitshare.Common
{
    /// <summary>
    /// SQL Server 数据库访问操作类
    /// </summary>
    public class SQLDBVisitor
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        private string _connectionString;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public SQLDBVisitor(string connectionString)
        {
            this._connectionString = connectionString;
        }

        /// <summary>
        /// 执行无参数的SQL 语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public int ExecSql(string sql)
        {
            return ExecSql(sql, null);
        }
        /// <summary>
        /// 执行SQL语句,返回受影响行数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        public int ExecSql(string sql, SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    int s = cmd.ExecuteNonQuery();
                    return s;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("SQL-ExecSql", ex);
                return -1;
            }
        }


        /// <summary>
        /// 执行一个存储过程
        /// </summary>
        /// <param name="strProcName">存储过程名称</param>
        /// <returns></returns>
        public bool ExecProc(string strProcName)
        {
            return ExecProc(strProcName, null);
        }
        /// <summary>
        /// 带参数的存储过程
        /// </summary>
        /// <param name="strProcName">存储过程名称</param>
        /// <param name="parameters">参数数组</param>
        /// <rereturns></rereturns>
        public bool ExecProc(string strProcName, SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this._connectionString))
                {
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = strProcName;
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("SQL-ExecProc", ex);
                return false;
            }
        }
        /// <summary>
        /// 执行带事务处理的多条T-SQL 
        /// </summary>
        /// <param name="sqlList">sql数组</param>
        /// <returns></returns>
        public bool ExecTrans(List<string> sqlList)
        {
            return ExecTrans(sqlList.ToArray());
        }

        /// <summary>
        /// 执行带事务处理的多条T-SQL 
        /// </summary>
        /// <param name="sqls">sql数组</param>
        /// <returns></returns>
        public bool ExecTrans(string[] sqls)
        {
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                SqlTransaction trans = null;
                try
                {
                    conn.Open();
                    trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.Transaction = trans;
                    foreach (string sql in sqls)
                    {
                        if (string.IsNullOrWhiteSpace(sql))
                        {
                            continue;
                        }
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }
                    trans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    if (trans != null)
                    {
                        trans.Rollback();
                    }
                    LogManager.Error("SQL-ExecTrans", ex);
                    return false;
                }
            }
        }


        /// <summary>
        /// 执行插入操作，返回@@IDENTITY
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public int Insert(string sql)
        {
            return Insert(sql, null);
        }
        /// <summary>
        /// 执行插入操作，返回@@IDENTITY
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        public int Insert(string sql, SqlParameter[] parameters)
        {
            try
            {
                sql += "; SELECT @@IDENTITY";
                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    return result;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("SQL-Insert", ex);
                return -1;
            }
        }


        /// <summary>
        /// 执行查询，返回查询结果集中的第一行第一列
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public object GetScalar(string sql)
        {
            return GetScalar(sql, null);
        }
        /// <summary>
        /// 执行查询，返回查询结果集中的第一行第一列
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        public object GetScalar(string sql, SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(this._connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    connection.Open();
                    object obj = cmd.ExecuteScalar();
                    if (obj == null || Convert.IsDBNull(obj))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (Exception ex)
                {
                    connection.Close();
                    LogManager.Error("SQL-GetScalar", ex);
                    return null;
                }
            }
        }


        /// <summary>
        /// 执行Sql，返回SqlDataReader结果
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public SqlDataReader GetReader(string sql)
        {
            return GetReader(sql, null);
        }
        /// <summary>
        /// 执行Sql，返回SqlDataReader结果
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        public SqlDataReader GetReader(string sql, SqlParameter[] parameters)
        {
            SqlConnection connection = new SqlConnection(this._connectionString);
            try
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch (Exception ex)
            {
                connection.Close();
                LogManager.Error("SQL-GetReader", ex);
                return null;
            }
        }

        /// <summary>
        /// 执行查询 返回DataTable
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public DataSet GetDataSet(string sql)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    DataSet ds = new DataSet();
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("SQL-GetDataSet", ex);
                return null;
            }
        }

        /// <summary>
        /// 执行查询返回DataTable
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql)
        {
            return GetDataTable(sql, null);
        }
        /// <summary>
        /// 执行查询DataTable
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>DataTable 结果</returns>
        public DataTable GetDataTable(string sql, SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this._connectionString))
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    DataTable dt = new DataTable();
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("SQL-GetDataTable", ex);
                return null;
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="queryFields">要查询的字段,多个字段用逗号(",")分隔,默认为"*"</param>
        /// <param name="where">查询条件</param>
        /// <param name="pageIndex">分页页码,默认1</param>
        /// <param name="pageSize">分页大小,默认20,最小5</param>
        /// <returns></returns>
        public DataTable GetDataTableByPageIndex(string tableName, string orderBy, out int totalCount, string queryFields = "*", string where = null, int pageIndex = 1, int pageSize = 20)
        {
            DataTable dt = null;
            totalCount = 0;
            try
            {
                string strWhere = string.IsNullOrWhiteSpace(where) ? "" : " where " + where;
                pageIndex = Math.Max(1, pageIndex);
                pageSize = Math.Max(5, pageSize);

                StringBuilder sb = new StringBuilder();
                sb.Append("select t.* from (");
                sb.AppendFormat("select top {0} {1},row_number() over( order by {2}) as rowNo from {3} {4}", pageIndex * pageSize, queryFields, orderBy, tableName, strWhere);
                sb.AppendFormat(") t where t.rowNo>{0}", ((pageIndex - 1) * pageSize));
                sb.Append(";select count(1) as total from " + tableName + strWhere);

                using (SqlConnection conn = new SqlConnection(this._connectionString))
                {
                    SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
                    DataSet ds = new DataSet();
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds != null && ds.Tables.Count >= 2)
                    {
                        dt = ds.Tables[0];
                        totalCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("SQL-GetDataTableByPageIndex", ex);
            }
            return dt;
        }


        /// <summary>
        /// 存储过程分页查询
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="queryFields">要查询的字段,多个字段用逗号(",")分隔,默认为"*"</param>
        /// <param name="where">查询条件</param>
        /// <param name="pageIndex">分页页码,默认1</param>
        /// <param name="pageSize">分页大小,默认20,最小5</param>
        /// <returns></returns>
        public DataTable QueryPageByProc(string tableName, string orderBy, out int totalCount, string queryFields = "*", string where = null, int pageIndex = 1, int pageSize = 20)
        {
            DataTable dt = null;
            totalCount = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(this._connectionString))
                {
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SP_GetPageListByQueryString";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@formName", SqlDbType.VarChar, 50); //表名
                    cmd.Parameters["@formName"].Direction = ParameterDirection.Input;
                    cmd.Parameters["@formName"].Value = tableName;

                    cmd.Parameters.Add("@queryFields", SqlDbType.VarChar, 2000); //查询字段
                    cmd.Parameters["@queryFields"].Direction = ParameterDirection.Input;
                    cmd.Parameters["@queryFields"].Value = queryFields;

                    cmd.Parameters.Add("@where", SqlDbType.VarChar, 2000); //查询条件
                    cmd.Parameters["@where"].Direction = ParameterDirection.Input;
                    cmd.Parameters["@where"].Value = where ?? string.Empty;

                    cmd.Parameters.Add("@orderBy", SqlDbType.VarChar, 200); //排序
                    cmd.Parameters["@orderBy"].Direction = ParameterDirection.Input;
                    cmd.Parameters["@orderBy"].Value = orderBy;

                    cmd.Parameters.Add("@pageIndex", SqlDbType.Int); //当前页
                    cmd.Parameters["@pageIndex"].Direction = ParameterDirection.Input;
                    cmd.Parameters["@pageIndex"].Value = pageIndex;

                    cmd.Parameters.Add("@pageSize", SqlDbType.Int); //每页显示记录条数
                    cmd.Parameters["@pageSize"].Direction = ParameterDirection.Input;
                    cmd.Parameters["@pageSize"].Value = pageSize;

                    cmd.Parameters.Add("@totalCount", SqlDbType.Int); //总记录数
                    cmd.Parameters["@totalCount"].Direction = ParameterDirection.Output;
                    cmd.Parameters["@totalCount"].Value = 0;
                    conn.Open();
                    dt = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);

                    //总记录数
                    totalCount = int.Parse(cmd.Parameters["@totalCount"].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("SQL-QueryPageByProc", ex);
            }
            return dt;
        }
        /// <summary>
        /// 存储过程分页查询
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="queryFields">要查询的字段,多个字段用逗号(",")分隔,默认为"*"</param>
        /// <param name="where">查询条件</param>
        /// <param name="pageIndex">分页页码,默认1</param>
        /// <param name="pageSize">分页大小,默认20,最小5</param>
        /// <returns></returns>
        public DataTable QueryPageByProc2(string tableName, string orderBy, string groupby, out int totalCount, string queryFields = "*", string where = null, int pageIndex = 1, int pageSize = 20)
        {
            DataTable dt = null;
            totalCount = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(this._connectionString))
                {
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SP_GetPageListByQueryTest";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@formName", SqlDbType.VarChar, 50); //表名
                    cmd.Parameters["@formName"].Direction = ParameterDirection.Input;
                    cmd.Parameters["@formName"].Value = tableName;

                    cmd.Parameters.Add("@queryFields", SqlDbType.VarChar, 2000); //查询字段
                    cmd.Parameters["@queryFields"].Direction = ParameterDirection.Input;
                    cmd.Parameters["@queryFields"].Value = queryFields;

                    cmd.Parameters.Add("@groupby", SqlDbType.VarChar, 2000); //查询字段
                    cmd.Parameters["@groupby"].Direction = ParameterDirection.Input;
                    cmd.Parameters["@groupby"].Value = groupby;

                    cmd.Parameters.Add("@where", SqlDbType.VarChar, 2000); //查询条件
                    cmd.Parameters["@where"].Direction = ParameterDirection.Input;
                    cmd.Parameters["@where"].Value = where ?? string.Empty;

                    cmd.Parameters.Add("@orderBy", SqlDbType.VarChar, 200); //排序
                    cmd.Parameters["@orderBy"].Direction = ParameterDirection.Input;
                    cmd.Parameters["@orderBy"].Value = orderBy;

                    cmd.Parameters.Add("@pageIndex", SqlDbType.Int); //当前页
                    cmd.Parameters["@pageIndex"].Direction = ParameterDirection.Input;
                    cmd.Parameters["@pageIndex"].Value = pageIndex;

                    cmd.Parameters.Add("@pageSize", SqlDbType.Int); //每页显示记录条数
                    cmd.Parameters["@pageSize"].Direction = ParameterDirection.Input;
                    cmd.Parameters["@pageSize"].Value = pageSize;

                    cmd.Parameters.Add("@totalCount", SqlDbType.Int); //总记录数
                    cmd.Parameters["@totalCount"].Direction = ParameterDirection.Output;
                    cmd.Parameters["@totalCount"].Value = 0;
                    conn.Open();
                    dt = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);

                    //总记录数
                    totalCount = int.Parse(cmd.Parameters["@totalCount"].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("SQL-QueryPageByProc", ex);
            }
            return dt;
        }

        /// <summary>
        /// 存储过程分页查询
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="where">查询条件</param>
        public List<string> QueryOrderBy(string tableName, string columnName, string where = null, string orderBy = null)
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
                DataTable dt = GetDataTable(sql);
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
        /// 执行无参数的查询存储过程,返回DataTable
        /// </summary>
        /// <param name="strProcName">存储过程名称</param>
        /// <returns></returns>
        public DataTable GetTableByExecProc(string strProcName)
        {
            return GetTableByExecProc(strProcName, null);
        }

        /// <summary>
        /// 执行带参数的查询存储过程,返回DataTable
        /// </summary>
        /// <param name="strProcName">存储过程名称</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        public DataTable GetTableByExecProc(string strProcName, SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this._connectionString))
                {
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = strProcName;
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    DataTable dt = new DataTable();
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("SQL-GetTableByExecProc", ex);
                return null;
            }
        }

        /// <summary>
        /// 高性能数据批量插入
        /// </summary>
        /// <param name="destinationTableName">目标表明</param>
        /// <param name="table">要导入的数据表实例</param>
        /// <param name="columnMappings">列映射(key-源表列名；value-目标表列名)</param>
        /// <returns></returns>
        public int BatchInsert(string destinationTableName, DataTable table, Dictionary<string, string> columnMappings = null)
        {
            if (string.IsNullOrWhiteSpace(destinationTableName) || table == null || table.Rows.Count == 0)
            {
                return 0;
            }
            int resultCount = 0;
            SqlBulkCopy sbc = new SqlBulkCopy(this._connectionString);
            sbc.BulkCopyTimeout = 300;
            sbc.BatchSize = table.Rows.Count;
            sbc.DestinationTableName = destinationTableName;
            if (columnMappings != null && columnMappings.Count > 0)
            {
                foreach (KeyValuePair<string, string> kv in columnMappings)
                {
                    sbc.ColumnMappings.Add(new SqlBulkCopyColumnMapping(kv.Key, kv.Value));
                }
            }
            try
            {
                sbc.WriteToServer(table);
                resultCount = table.Rows.Count;
            }
            catch (Exception ex)
            {
                LogManager.Error("SQL-BatchInsert", ex);
                resultCount = 0;
            }
            finally
            {
                sbc.Close();
            }
            return resultCount;
        }

        /// <summary>
        /// 组合SQL语句用于 in 条件的查询
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public string AssemblyInCondition(List<string> values)
        {
            string str = string.Empty;
            if (values != null && values.Count > 0)
            {
                // 过滤空值和重复项
                var list = values.Where(p => !string.IsNullOrWhiteSpace(p)).Distinct().Select(p => "'" + p + "'").ToList();
                if (list.Count > 0)
                {
                    str = "(" + string.Join(",", list) + ")";
                }
            }
            return str;
        }

        /// <summary>
        /// 组合SQL语句用于 in 条件的查询
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public string AssemblyInCondition(List<int> values)
        {
            string str = string.Empty;
            if (values != null && values.Count > 0)
            {
                str = "(" + string.Join(",", values.Distinct()) + ")";
            }
            return str;
        }

        #region
        /// <summary>
        /// 将实体插入数据库
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tbl"></param>
        /// <returns></returns>
        public int AddObject<T>(T tbl) where T : new()
        {
            string sql = "";
            try
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                Type t = typeof(T);
                PropertyInfo[] ProList = t.GetProperties();
                foreach (var item in ProList)
                {
                    var value = tbl.GetType().GetProperty(item.Name).GetValue(tbl, null);
                    if (tbl.GetType().GetProperty(item.Name).PropertyType == typeof(DateTime))
                    {
                        if (Convert.ToDateTime(value) < Convert.ToDateTime("1900-1-1"))
                            value = null;
                    }
                    if (value != null)
                    {
                        if (item.Name.ToString().ToLower() != "Rid")
                            dic.Add(item.Name, value.ToString());
                    }
                }
                string sql_header = "insert into " + tbl.GetType().Name + "(";
                string sql_footer = ")values(";
                foreach (KeyValuePair<string, string> kvp in dic)
                {
                    sql_header += kvp.Key + ",";
                    sql_footer += String.Format("'{0}',", kvp.Value);
                }
                sql_header = sql_header.Remove(sql_header.Length - 1);
                sql_footer = sql_footer.Remove(sql_footer.Length - 1);
                sql = String.Format("{0}{1})", sql_header, sql_footer);
                return ExecSql(sql);
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("SQL-AddObject<{1}>[{0}]", sql, typeof(T).Name), ex);
                return 0;
            }
        }

        /// <summary>
        /// 组合insertSQL语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tbl"></param>
        /// <returns></returns>
        public string InsertSql<T>(T tbl) where T : new()
        {
            string sql = "";
            try
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                Type t = typeof(T);
                PropertyInfo[] ProList = t.GetProperties();
                foreach (var item in ProList)
                {
                    var value = tbl.GetType().GetProperty(item.Name).GetValue(tbl, null);
                    if (tbl.GetType().GetProperty(item.Name).PropertyType == typeof(DateTime))
                    {
                        if (Convert.ToDateTime(value) < Convert.ToDateTime("1900-1-1"))
                            value = null;
                    }
                    if (value != null)
                    {
                        if (item.Name.ToString().ToLower() != "Rid")
                            dic.Add(item.Name, value.ToString());
                    }
                }
                string sql_header = "insert into " + tbl.GetType().Name + "(";
                string sql_footer = ")values(";
                foreach (KeyValuePair<string, string> kvp in dic)
                {
                    sql_header += kvp.Key + ",";
                    sql_footer += String.Format("'{0}',", kvp.Value);
                }
                sql_header = sql_header.Remove(sql_header.Length - 1);
                sql_footer = sql_footer.Remove(sql_footer.Length - 1);
                sql = String.Format("{0}{1})", sql_header, sql_footer);
                return sql;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("组合SQL", sql, typeof(T).Name), ex);
                return "";
            }
        }

        public T GetEntiy<T>(int id) where T : new()
        {
            string sql = "";
            try
            {
                T t = new T();
                sql = "select * from " + typeof(T).Name + " where Rid=" + id;
                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        PropertyInfo[] ProList = typeof(T).GetProperties();
                        foreach (var item in ProList)
                        {

                            try { var r = reader[item.Name]; }
                            catch
                            {
                                continue;
                            }

                            if (reader[item.Name].GetType() == typeof(DateTime))
                            {
                                if (reader[item.Name] != DBNull.Value)
                                    typeof(T).GetProperty(item.Name).SetValue(t, Convert.ToDateTime(reader[item.Name]).ToString("yyyy-MM-dd"), null);
                            }
                            else if (reader[item.Name].GetType() == typeof(Int32))
                            {
                                if (reader[item.Name] != DBNull.Value)
                                {
                                    if (typeof(T).GetProperty(item.Name).PropertyType == typeof(Int32))
                                        typeof(T).GetProperty(item.Name).SetValue(t, Convert.ToInt32(reader[item.Name]), null);
                                    else
                                        typeof(T).GetProperty(item.Name).SetValue(t, Convert.ToString(reader[item.Name]), null);
                                }

                            }
                            else
                            {
                                if (reader[item.Name] != DBNull.Value)
                                    typeof(T).GetProperty(item.Name).SetValue(t, Convert.ToString(reader[item.Name]), null);

                                else
                                    typeof(T).GetProperty(item.Name).SetValue(t, "", null);
                            }
                        }
                    }
                    return t;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("SQL-GetEntiy<{1}>[{0}]", sql, typeof(T).Name), ex);
                return default(T);
            }
        }

        public List<T> GetEntiyList<T>() where T : new()
        {
            string sql = "";
            try
            {
                List<T> list = new List<T>();
                sql = "select * from " + typeof(T).Name;
                using (SqlConnection connection = new SqlConnection(this._connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        T t = new T();
                        PropertyInfo[] ProList = typeof(T).GetProperties();
                        List<string> str = new List<string>();
                        for (int ii = 0; ii < reader.FieldCount; ii++)
                        {
                            str.Add(reader.GetName(ii).ToLower());
                        }
                        foreach (var item in ProList)
                        {
                            if (str.Contains(item.Name.ToLower()))
                            {
                                if (reader[item.Name].GetType() == typeof(DateTime))
                                {
                                    if (reader[item.Name] != DBNull.Value)
                                        typeof(T).GetProperty(item.Name).SetValue(t, Convert.ToDateTime(reader[item.Name]).ToString("yyyy-MM-dd"), null);
                                }
                                else
                                {
                                    if (reader[item.Name] != DBNull.Value)
                                        typeof(T).GetProperty(item.Name).SetValue(t, Convert.ToString(reader[item.Name]), null);
                                    else
                                        typeof(T).GetProperty(item.Name).SetValue(t, "", null);
                                }
                            }

                        }
                        list.Add(t);
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("SQL-GetEntiyList<{1}>[{0}]", sql, typeof(T).Name), ex);
                return default(List<T>);
            }
        }
        public bool UpdateObject<T>(T tbl) where T : new()
        {
            string sql = "";
            try
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                Type t = typeof(T);
                PropertyInfo[] ProList = t.GetProperties();
                foreach (var item in ProList)
                {
                    var value = tbl.GetType().GetProperty(item.Name).GetValue(tbl, null);
                    if (tbl.GetType().GetProperty(item.Name).PropertyType == typeof(DateTime))
                    {
                        if (Convert.ToDateTime(value) < Convert.ToDateTime("1990-1-1"))
                            value = null;
                    }
                    if (value != null)
                    {
                        dic.Add(item.Name, value.ToString());
                    }
                }
                string sql_header = "update " + tbl.GetType().Name + " set ";
                string sql_footer = " where ";
                foreach (KeyValuePair<string, string> kvp in dic)
                {
                    if (kvp.Key.ToLower() != "Rid")
                    {
                        sql_header += String.Format("{0}='{1}',", kvp.Key, kvp.Value);
                    }
                    else
                    {

                        sql_footer += String.Format("{0}={1}", kvp.Key, kvp.Value);
                    }
                }
                sql_header = sql_header.Remove(sql_header.Length - 1);

                sql = String.Format("{0}{1}", sql_header, sql_footer);
                int flag = ExecSql(sql);
                return flag == 1 ? true : false;
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("SQL-AddObject<{1}>[{0}]", sql, typeof(T).Name), ex);
                return false;
            }
        }

        public int Add<T>(T model) where T : new()
        {
            string sql = "";
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                Type t = typeof(T);
                PropertyInfo[] ProList = t.GetProperties();
                List<SqlParameter> parameters = new List<SqlParameter>();
                string sql_header = "insert into " + model.GetType().Name + "(";
                string sql_footer = ")values(";
                foreach (var item in ProList)
                {
                    var value = model.GetType().GetProperty(item.Name).GetValue(model, null);
                    sql_header += item.Name + ",";
                    sql_footer += String.Format("@{0},", item.Name);
                    Type type = model.GetType().GetProperty(item.Name).PropertyType;
                    parameters.Add(new SqlParameter(String.Format("@{0},", item.Name), value));
                }

                sql_header = sql_header.Remove(sql_header.Length - 1);
                sql_footer = sql_footer.Remove(sql_footer.Length - 1);
                sql = String.Format("{0}{1})", sql_header, sql_footer);
                return ExecSql(sql, parameters.ToArray());
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("SQL-AddObject<{1}>[{0}]", sql, typeof(T).Name), ex);
                return 0;
            }
        }


        #endregion
    }
}
