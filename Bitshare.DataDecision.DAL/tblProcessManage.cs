using Bitshare.DataDecision.DBUtility;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace Bitshare.DataDecision.DAL
{
    //tblProcessManage
    public partial class tblProcessManage
    {

        public bool Exists(int Rid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tblProcessManage");
            strSql.Append(" where ");
            strSql.Append(" Rid = @Rid  ");
            SqlParameter[] parameters = {
					new SqlParameter("@Rid", SqlDbType.Int,4)
			};
            parameters[0].Value = Rid;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }



        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Bitshare.DataDecision.Model.tblProcessManage model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tblProcessManage(");
            strSql.Append("ProcessNum,ProcessName,NodeNum,NodeName,NodeNameNext,NodeType,ExecutorRole,ReMark");
            strSql.Append(") values (");
            strSql.Append("@ProcessNum,@ProcessName,@NodeNum,@NodeName,@NodeNameNext,@NodeType,@ExecutorRole,@ReMark");
            strSql.Append(") ");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
			            new SqlParameter("@ProcessNum", SqlDbType.Int,4) ,            
                        new SqlParameter("@ProcessName", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@NodeNum", SqlDbType.Int,4) ,            
                        new SqlParameter("@NodeName", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@NodeNameNext", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@NodeType", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@ExecutorRole", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@ReMark", SqlDbType.VarChar,100)             
              
            };

            parameters[0].Value = model.ProcessNum;
            parameters[1].Value = model.ProcessName;
            parameters[2].Value = model.NodeNum;
            parameters[3].Value = model.NodeName;
            parameters[4].Value = model.NodeNameNext;
            parameters[5].Value = model.NodeType;
            parameters[6].Value = model.ExecutorRole;
            parameters[7].Value = model.ReMark;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {

                return Convert.ToInt32(obj);

            }

        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Bitshare.DataDecision.Model.tblProcessManage model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tblProcessManage set ");

            strSql.Append(" ProcessNum = @ProcessNum , ");
            strSql.Append(" ProcessName = @ProcessName , ");
            strSql.Append(" NodeNum = @NodeNum , ");
            strSql.Append(" NodeName = @NodeName , ");
            strSql.Append(" NodeNameNext = @NodeNameNext , ");
            strSql.Append(" NodeType = @NodeType , ");
            strSql.Append(" ExecutorRole = @ExecutorRole , ");
            strSql.Append(" ReMark = @ReMark  ");
            strSql.Append(" where Rid=@Rid ");

            SqlParameter[] parameters = {
			            new SqlParameter("@Rid", SqlDbType.Int,4) ,            
                        new SqlParameter("@ProcessNum", SqlDbType.Int,4) ,            
                        new SqlParameter("@ProcessName", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@NodeNum", SqlDbType.Int,4) ,            
                        new SqlParameter("@NodeName", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@NodeNameNext", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@NodeType", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@ExecutorRole", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@ReMark", SqlDbType.VarChar,100)             
              
            };

            parameters[0].Value = model.Rid;
            parameters[1].Value = model.ProcessNum;
            parameters[2].Value = model.ProcessName;
            parameters[3].Value = model.NodeNum;
            parameters[4].Value = model.NodeName;
            parameters[5].Value = model.NodeNameNext;
            parameters[6].Value = model.NodeType;
            parameters[7].Value = model.ExecutorRole;
            parameters[8].Value = model.ReMark;
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int Rid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tblProcessManage ");
            strSql.Append(" where Rid=@Rid");
            SqlParameter[] parameters = {
					new SqlParameter("@Rid", SqlDbType.Int,4)
			};
            parameters[0].Value = Rid;


            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 批量删除一批数据
        /// </summary>
        public bool DeleteList(string Ridlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tblProcessManage ");
            strSql.Append(" where ID in (" + Ridlist + ")  ");
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Bitshare.DataDecision.Model.tblProcessManage GetModel(int Rid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Rid, ProcessNum, ProcessName, NodeNum, NodeName, NodeNameNext, NodeType, ExecutorRole, ReMark  ");
            strSql.Append("  from tblProcessManage ");
            strSql.Append(" where Rid=@Rid");
            SqlParameter[] parameters = {
					new SqlParameter("@Rid", SqlDbType.Int,4)
			};
            parameters[0].Value = Rid;


            Bitshare.DataDecision.Model.tblProcessManage model = new Bitshare.DataDecision.Model.tblProcessManage();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Rid"].ToString() != "")
                {
                    model.Rid = int.Parse(ds.Tables[0].Rows[0]["Rid"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ProcessNum"].ToString() != "")
                {
                    model.ProcessNum = int.Parse(ds.Tables[0].Rows[0]["ProcessNum"].ToString());
                }
                model.ProcessName = ds.Tables[0].Rows[0]["ProcessName"].ToString();
                if (ds.Tables[0].Rows[0]["NodeNum"].ToString() != "")
                {
                    model.NodeNum = int.Parse(ds.Tables[0].Rows[0]["NodeNum"].ToString());
                }
                model.NodeName = ds.Tables[0].Rows[0]["NodeName"].ToString();
                model.NodeNameNext = ds.Tables[0].Rows[0]["NodeNameNext"].ToString();
                model.NodeType = ds.Tables[0].Rows[0]["NodeType"].ToString();
                model.ExecutorRole = ds.Tables[0].Rows[0]["ExecutorRole"].ToString();
                model.ReMark = ds.Tables[0].Rows[0]["ReMark"].ToString();

                return model;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM tblProcessManage ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" * ");
            strSql.Append(" FROM tblProcessManage ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }


    }
}

