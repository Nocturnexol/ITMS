using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.OracleClient;

namespace Bitshare.DataDecision.Common
{
    /// <summary>
    /// Oracle数据库访问操作类
    /// </summary>
    public class OracleDBVisitor
    {
#pragma warning disable CS0618 // 类型或成员已过时
        /// <summary>
        /// 连接字符串
        /// </summary>
        private string _connectionString;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public OracleDBVisitor(string connectionString)
        {
            this._connectionString = connectionString;
        }

        /// <summary>
        /// 执行无参SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public int ExecSql(string strSql)
        {
            return ExecSql(strSql, null);
        }
        /// <summary>
        /// 执行带参数的SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>影响的记录数</returns>
        public int ExecSql(string sql, OracleParameter[] parameters)
        {

            using (OracleConnection connection = new OracleConnection(this._connectionString))

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
        /// <returns>影响的记录数</returns>
        public int ExecProc(string sql)
        {
            return ExecProc(sql, null);
        }
        /// <summary>
        /// 执行带参数的存储过程，返回影响的记录数
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>影响的记录数</returns>
        public int ExecProc(string strSql, OracleParameter[] parameters)
        {
            using (OracleConnection connection = new OracleConnection(this._connectionString))
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
        public Boolean ExecTrans(string[] sqls)
        {
            Boolean result = false;
            using (OracleConnection conn = new OracleConnection(this._connectionString))
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
        /// <returns>查询结果（object）</returns>
        public object GetScalar(string strSql)
        {
            return GetScalar(strSql, null);
        }
        /// <summary>
        /// 执行一条查询语句，返回第一行第一列（object）。
        /// </summary>
        /// <param name="strSql">sql查询语句</param>
        /// <param name="values">参数数组</param>
        /// <returns>查询结果（object）</returns>
        public object GetScalar(string strSql, OracleParameter[] parameters)
        {
            using (OracleConnection connection = new OracleConnection(this._connectionString))
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
        /// <returns>OracleDataReader</returns>
        public OracleDataReader ExecuteReader(string strSql)
        {
            return ExecuteReader(strSql, null);
        }
        /// <summary>
        /// 执行查询语句，返回OracleDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="strSql">查询语句</param>
        /// <param name="values">参数数组</param>
        /// <returns>OracleDataReader</returns>
        public OracleDataReader ExecuteReader(string strSql, OracleParameter[] parameters)
        {
            OracleConnection connection = new OracleConnection(this._connectionString);
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
        public DataSet GetDataSet(string strSql)
        {
            using (OracleConnection connection = new OracleConnection(this._connectionString))
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
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string strSql)
        {
            return GetDataTable(strSql, null);
        }
        /// <summary>
        /// 执行查询语句，返回DataTable
        /// </summary>
        /// <param name="strSql">查询语句</param>
        /// <param name="values">参数数组</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string strSql, OracleParameter[] parameters)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(this._connectionString))
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
        /// <returns></returns>
        public DataTable GetTableByExecProc(string strProcName)
        {
            return GetTableByExecProc(strProcName, null);
        }
        /// <summary>
        /// 执行带参数的查询存储过程,返回DataTable(存储过程需要一个游标类型的输出参数“P_Cursor”)
        /// </summary>
        /// <param name="strProcName">存储过程名称</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        public DataTable GetTableByExecProc(string strProcName, OracleParameter[] parameters)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(this._connectionString))
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

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="selectFields">查询字段</param>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="curPage">当前页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">总页数</param>
        /// <returns></returns>
        public DataTable QueryPaging(string tableName, string selectFields, string where, string orderBy, int curPage, int pageSize, out int recordCount, out int pageCount)
        {
            recordCount = 0;
            pageCount = 0;
            try
            {
                using (OracleConnection connection = new OracleConnection(this._connectionString))
                {
                    OracleCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "pd_query_paging";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_tableName", OracleType.VarChar, 50); //表名
                    cmd.Parameters["p_tableName"].Direction = ParameterDirection.Input;
                    cmd.Parameters["p_tableName"].Value = tableName;

                    cmd.Parameters.Add("p_selectFields", OracleType.VarChar, 3000); //查询字段
                    cmd.Parameters["p_selectFields"].Direction = ParameterDirection.Input;
                    cmd.Parameters["p_selectFields"].Value = selectFields;

                    cmd.Parameters.Add("p_where", OracleType.VarChar, 3000); //查询条件
                    cmd.Parameters["p_where"].Direction = ParameterDirection.Input;
                    cmd.Parameters["p_where"].Value = where;

                    cmd.Parameters.Add("p_orderBy", OracleType.VarChar, 3000); //排序
                    cmd.Parameters["p_orderBy"].Direction = ParameterDirection.Input;
                    cmd.Parameters["p_orderBy"].Value = orderBy;

                    cmd.Parameters.Add("p_curPage", OracleType.Number); //当前页
                    cmd.Parameters["p_curPage"].Direction = ParameterDirection.Input;
                    cmd.Parameters["p_curPage"].Value = curPage;

                    cmd.Parameters.Add("p_pageSize", OracleType.Number); //每页显示记录条数
                    cmd.Parameters["p_pageSize"].Direction = ParameterDirection.Input;
                    cmd.Parameters["p_pageSize"].Value = pageSize;

                    cmd.Parameters.Add("p_RecordCount", OracleType.Number); //总记录数
                    cmd.Parameters["p_RecordCount"].Direction = ParameterDirection.Output;
                    cmd.Parameters["p_RecordCount"].Value = 0;

                    cmd.Parameters.Add("p_pageCount", OracleType.Number); //总页数
                    cmd.Parameters["p_pageCount"].Direction = ParameterDirection.Output;
                    cmd.Parameters["p_pageCount"].Value = 0;

                    cmd.Parameters.Add("v_cur", OracleType.Cursor); //返回的游标
                    cmd.Parameters["v_cur"].Direction = ParameterDirection.Output;

                    DataTable dt = new DataTable();
                    OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                    adapter.Fill(dt);
                    connection.Close();

                    //总记录数
                    recordCount = int.Parse(cmd.Parameters["p_RecordCount"].Value.ToString());
                    //总页数
                    pageCount = Convert.ToInt32(Math.Floor(Convert.ToDouble(cmd.Parameters["p_pageCount"].Value.ToString())));

                    return dt;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error("Oracle-QueryPaging", ex);
                return null;
            }
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
#pragma warning restore CS0618 // 类型或成员已过时
    }
}
