using System;
using System.Data;
using System.Data.OracleClient;

namespace Bitshare.DataDecision.Common
{
    /// <summary>
    /// Oracle数据库访问操作类
    /// </summary>
    internal class OracleDBHelper
    {
#pragma warning disable CS0618 // 类型或成员已过时
        /// <summary>
        /// 执行无参SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns>影响的记录数</returns>
        public static int ExecSql(string strSql, string connectionString)
        {
            return ExecSql(strSql, connectionString, null);
        }
        /// <summary>
        /// 执行带参数的SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>影响的记录数</returns>
        public static int ExecSql(string sql, string connectionString, OracleParameter[] parameters)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                try
                {
                    OracleCommand cmd = new OracleCommand(sql, connection);
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (Exception ex)
                {
                    connection.Close();
                    LogManager.Error("Oracle-ExecSql", ex);
                    return 0;
                }
            }
        }


        /// <summary>
        /// 执行无参的存储过程，返回影响的记录数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns>影响的记录数</returns>
        public static int ExecProc(string sql, string connectionString)
        {
            return ExecProc(sql, connectionString, null);
        }
        /// <summary>
        /// 执行带参数的存储过程，返回影响的记录数
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>影响的记录数</returns>
        public static int ExecProc(string strSql, string connectionString, OracleParameter[] parameters)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                try
                {
                    OracleCommand cmd = new OracleCommand(strSql, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (Exception ex)
                {
                    connection.Close();
                    LogManager.Error("Oracle-ExecProc", ex);
                    return 0;
                }
            }
        }


        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="sqls">多条SQL语句</param>
        /// <returns></returns>
        public static Boolean ExecTrans(string[] sqls, string connectionString)
        {
            Boolean result = false;
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                OracleTransaction tx = null;
                try
                {
                    tx = conn.BeginTransaction();
                    OracleCommand cmd = conn.CreateCommand();
                    cmd.Transaction = tx;
                    conn.Open();
                    foreach (string sql in sqls)
                    {
                        if (string.IsNullOrWhiteSpace(sql))
                        {
                            continue;
                        }
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }
                    tx.Commit();
                    result = true;
                }
                catch (Exception ex)
                {
                    if (tx != null)
                    {
                        tx.Rollback();
                    }
                    LogManager.Error("Oracle-ExecTrans", ex);
                }
            }
            return result;
        }


        /// <summary>
        /// 执行一条查询语句，返回第一行第一列（object）。
        /// </summary>
        /// <param name="strSql">sql查询语句</param>
        /// <param name="connectionString">数据库连接字符串</param> 
        /// <returns>查询结果（object）</returns>
        public static object GetScalar(string strSql, string connectionString)
        {
            return GetScalar(strSql, connectionString, null);
        }
        /// <summary>
        /// 执行一条查询语句，返回第一行第一列（object）。
        /// </summary>
        /// <param name="strSql">sql查询语句</param>
        /// <param name="connectionString">数据库连接字符串</param> 
        /// <param name="values">参数数组</param>
        /// <returns>查询结果（object）</returns>
        public static object GetScalar(string strSql, string connectionString, OracleParameter[] parameters)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                try
                {
                    OracleCommand cmd = new OracleCommand(strSql, connection);
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
                    LogManager.Error("Oracle-GetScalar", ex);
                    return null;
                }
            }
        }


        /// <summary>
        /// 执行查询语句，返回OracleDataReader ( 注意：调用该方法后，一定要对OracleDataReader进行Close )
        /// </summary>
        /// <param name="strSql">查询语句</param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns>OracleDataReader</returns>
        public static OracleDataReader ExecuteReader(string strSql, string connectionString)
        {
            return ExecuteReader(strSql, connectionString, null);
        }
        /// <summary>
        /// 执行查询语句，返回OracleDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="strSql">查询语句</param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="values">参数数组</param>
        /// <returns>OracleDataReader</returns>
        public static OracleDataReader ExecuteReader(string strSql, string connectionString, OracleParameter[] parameters)
        {
            OracleConnection connection = new OracleConnection(connectionString);
            OracleCommand cmd = new OracleCommand(strSql, connection);
            try
            {
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                connection.Open();
                OracleDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (Exception ex)
            {
                connection.Close();
                LogManager.Error("Oracle-ExecuteReader", ex);
                return null;
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="strSql">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet GetDataSet(string strSql, string connectionString)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                try
                {
                    OracleDataAdapter dap = new OracleDataAdapter(strSql, connection);
                    DataSet ds = new DataSet();
                    connection.Open();
                    dap.Fill(ds);
                    return ds;
                }
                catch (Exception ex)
                {
                    connection.Close();
                    LogManager.Error("Oracle-GetDataSet", ex);
                    return null;
                }
            }
        }
        /// <summary>
        /// 执行查询语句，返回DataTable
        /// </summary>
        /// <param name="strSql">查询语句</param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataTable(string strSql, string connectionString)
        {
            return GetDataTable(strSql, connectionString, null);
        }
        /// <summary>
        /// 执行查询语句，返回DataTable
        /// </summary>
        /// <param name="strSql">查询语句</param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="values">参数数组</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataTable(string strSql, string connectionString, OracleParameter[] parameters)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    OracleCommand cmd = new OracleCommand(strSql, connection);
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    DataTable dt = new DataTable();
                    connection.Open();
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("Oracle-GetDataTable", ex);
                return null;
            }
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
        /// 执行带参数的查询存储过程,返回DataTable(存储过程需要一个游标类型的输出参数“P_Cursor”)
        /// </summary>
        /// <param name="strProcName">存储过程名称</param>
        /// <param name="connectionstring">连接字符串</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        public static DataTable GetTableByExecProc(string strProcName, string connectionstring, OracleParameter[] parameters)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(connectionstring))
                {
                    OracleCommand sqlCmd = connection.CreateCommand();
                    sqlCmd.CommandText = strProcName;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null && parameters.Length > 0)
                    {
                        sqlCmd.Parameters.AddRange(parameters);
                    }
                    //添加输出类型
                    OracleParameter outParam = new OracleParameter();
                    outParam.Direction = ParameterDirection.Output;
                    outParam.OracleType = OracleType.Cursor;
                    outParam.ParameterName = "P_Cursor";
                    sqlCmd.Parameters.Add(outParam);
                    DataSet ds = new DataSet();
                    connection.Open();
                    OracleDataAdapter da = new OracleDataAdapter(sqlCmd);
                    da.Fill(ds);
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("Oracle-GetTableByExecProc", ex);
                return null;
            }
        }
#pragma warning restore CS0618 // 类型或成员已过时
    }
}
