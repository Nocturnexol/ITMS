using Bitshare.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Bitshare.DataDecision.Common
{
    /// <summary>
    /// SQL Server 数据库访问操作类
    /// </summary>
    internal class SQLDBHelper
    {
        /// <summary>
        /// 执行无参数的SQL 语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="connectionstring">连接字符串</param>
        /// <returns></returns>
        public static int ExecSql(string sql, string connectionstring)
        {
            return ExecSql(sql, connectionstring, null);
        }
        /// <summary>
        /// 执行SQL语句,返回受影响行数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="connectionstring">连接字符串</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        public static int ExecSql(string sql, string connectionstring, SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("SQL-ExecSql", ex);
                return 0;
            }
        }


        /// <summary>
        /// 执行一个存储过程
        /// </summary>
        /// <param name="strProcName">存储过程名称</param>
        /// <param name="connectionstring">连接字符串</param>
        /// <returns></returns>
        public static int ExecProc(string strProcName, string connectionstring)
        {
            return ExecProc(strProcName, connectionstring, null);
        }
        /// <summary>
        /// 带参数的存储过程
        /// </summary>
        /// <param name="strProcName">存储过程名称</param>
        /// <param name="connectionstring">连接字符串</param>
        /// <param name="parameters">参数数组</param>
        /// <rereturns></rereturns>
        public static int ExecProc(string strProcName, string connectionstring, SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = strProcName;
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("SQL-ExecProc", ex);
                return 0;
            }
        }


        /// <summary>
        /// 执行带事务处理的多条T-SQL 
        /// </summary>
        /// <param name="sqls">sql数组</param>
        /// <param name="connectionstring">连接字符串</param>
        /// <returns></returns>
        public static bool ExecTrans(string[] sqls, string connectionstring)
        {
            using (SqlConnection conn = new SqlConnection(connectionstring))
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
        /// <param name="connectionstring">连接字符串</param>
        /// <returns></returns>
        public static int Insert(string sql, string connectionstring)
        {
            return Insert(sql, connectionstring);
        }
        /// <summary>
        /// 执行插入操作，返回@@IDENTITY
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="connectionstring">连接字符串</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        public static int Insert(string sql, string connectionstring, SqlParameter[] parameters)
        {
            try
            {
                sql += "; SELECT @@IDENTITY";
                using (SqlConnection connection = new SqlConnection(connectionstring))
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
        /// <param name="connectionstring">连接字符串</param>
        /// <returns></returns>
        public static object GetScalar(string sql, string connectionstring)
        {
            return GetScalar(sql, connectionstring, null);
        }
        /// <summary>
        /// 执行查询，返回查询结果集中的第一行第一列
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="connectionstring">连接字符串</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        public static object GetScalar(string sql, string connectionstring, SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
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
        /// <param name="connectionstring">连接字符串</param>
        /// <returns></returns>
        public static SqlDataReader GetReader(string sql, string connectionstring)
        {
            return GetReader(sql, connectionstring, null);
        }
        /// <summary>
        /// 执行Sql，返回SqlDataReader结果
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="connectionstring">连接字符串</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        public static SqlDataReader GetReader(string sql, string connectionstring, SqlParameter[] parameters)
        {
            SqlConnection connection = new SqlConnection(connectionstring);
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
        /// <param name="connectionstring">连接字符串</param>
        /// <returns></returns>
        public static DataSet GetDataSet(string sql, string connectionstring)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionstring))
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
        /// 组合SQL语句用于 in 条件的查询
        /// </summary>
        /// <param name="strsql"></param>
        /// <param name="Condition"></param>
        /// <returns></returns>
        public static string Assembly(string conditon, string[] values)
        {
            try
            {
                conditon = "(";
                for (int j = 0; j < values.Length; j++)
                {
                    conditon = conditon + "'" + values[j].Trim() + "'" + ",";
                }
                conditon = conditon.Substring(0, conditon.Length - 1);
                conditon = conditon + ")";
                return conditon;
            }
            catch (Exception ex)
            {
                LogManager.Error("SQL-GetDataSet", ex);
                throw;
            }

        }
        /// <summary>
        /// 执行查询返回DataTable
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="connectionstring">连接字符串</param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql, string connectionstring)
        {
            return GetDataTable(sql, connectionstring, null);
        }
        /// <summary>
        /// 执行查询DataTable
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="connectionstring">连接字符串</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>DataTable 结果</returns>
        public static DataTable GetDataTable(string sql, string connectionstring, SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionstring))
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
        /// <param name="connectionstring">连接字符串</param>
        /// <param name="tableName">表名</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="queryFields">要查询的字段,多个字段用逗号(",")分隔,默认为"*"</param>
        /// <param name="where">查询条件</param>
        /// <param name="pageIndex">分页页码,默认1</param>
        /// <param name="pageSize">分页大小,默认20,最小5</param>
        /// <returns></returns>
        public static DataTable GetDataTableByPageIndex(string connectionstring, string tableName, string orderBy, out int totalCount, string queryFields = "*", string where = null, int pageIndex = 1, int pageSize = 20)
        {
            DataTable dt = null;
            totalCount = 0;
            try
            {
                string strWhere = string.IsNullOrWhiteSpace(where) ? "" : " where " + where;
                pageIndex = Math.Max(1, pageIndex);
                pageSize = Math.Max(5, pageSize);

                StringBuilder sb = new StringBuilder();
                sb.Append("select t.* from（");
                sb.AppendFormat("select top {0} {1},row_number() over({2}) as rowNo from {3} {4}", pageIndex * pageSize, queryFields, orderBy, tableName, strWhere);
                sb.AppendFormat(") t where t.rowNo>{0}", ((pageIndex - 1) * pageSize));
                sb.Append(";select count(1) as total from " + tableName + strWhere);

                using (SqlConnection conn = new SqlConnection(connectionstring))
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
        /// 执行无参数的查询存储过程,返回DataTable
        /// </summary>
        /// <param name="strProcName">存储过程名称</param>
        /// <param name="connectionstring">连接字符串</param>
        /// <returns></returns>
        public static DataTable GetTableByExecProc(string strProcName, string connectionstring)
        {
            return GetTableByExecProc(strProcName, connectionstring, null);
        }
        /// <summary>
        /// 执行带参数的查询存储过程,返回DataTable
        /// </summary>
        /// <param name="strProcName">存储过程名称</param>
        /// <param name="connectionstring">连接字符串</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        public static DataTable GetTableByExecProc(string strProcName, string connectionstring, SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionstring))
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
        /// <param name="connectionstring">连接字符串</param>
        /// <param name="destinationTableName">目标表明</param>
        /// <param name="table">要导入的数据表实例</param>
        /// <param name="columnMappings">列映射(key-源表列名；value-目标表列名)</param>
        /// <returns></returns>
        public static int BatchInsert(string connectionstring, string destinationTableName, DataTable table, Dictionary<string, string> columnMappings = null)
        {
            if (string.IsNullOrWhiteSpace(destinationTableName) || table == null || table.Rows.Count == 0)
            {
                return 0;
            }
            int resultCount = 0;
            SqlBulkCopy sbc = new SqlBulkCopy(connectionstring);
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
    }
}
