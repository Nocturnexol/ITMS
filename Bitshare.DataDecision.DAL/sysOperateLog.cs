using Bitshare.DataDecision.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace Bitshare.DataDecision.DAL
{
    //sysOperateLog
    public partial class sysOperateLog
    {

        public bool Exists(int Rid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from sysOperateLog");
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
        public int Add(Bitshare.DataDecision.Model.sysOperateLog model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into sysOperateLog(");
            strSql.Append("LoginName,UserName,Content,OperateTime,ChangeMode");
            strSql.Append(") values (");
            strSql.Append("@LoginName,@UserName,@Content,@OperateTime,@ChangeMode");
            strSql.Append(") ");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
			            new SqlParameter("@LoginName", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@UserName", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@Content", SqlDbType.Text) ,            
                        new SqlParameter("@OperateTime", SqlDbType.SmallDateTime) ,
                        new SqlParameter("@ChangeMode", SqlDbType.VarChar,50) 
              
            };

            parameters[0].Value = model.LoginName;
            parameters[1].Value = model.UserName;
            parameters[2].Value = model.Content;
            parameters[3].Value = model.OperateTime;
            parameters[4].Value = model.ChangeMode;
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
        /// 增加一条数据
        /// </summary>
        public bool Add(List<Bitshare.DataDecision.Model.sysOperateLog> modelList)
        {
            List<string> sqlList = new List<string>();
            foreach (var model in modelList)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into sysOperateLog(");
                strSql.Append("LoginName,UserName,Content,OperateTime,ChangeMode");
                strSql.Append(") values (");
                strSql.Append("'{0}','{1}','{2}','{3}','{4}'");
                strSql.Append(") ");
                sqlList.Add(string.Format(strSql.ToString(), model.LoginName, model.UserName, model.Content, model.OperateTime, model.ChangeMode));


            }
            return DbHelperSQL.ExecuteSqlTran(sqlList) > 0;
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Bitshare.DataDecision.Model.sysOperateLog model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update sysOperateLog set ");

            strSql.Append(" LoginName = @LoginName , ");
            strSql.Append(" UserName = @UserName , ");
            strSql.Append(" ChangeMode = @ChangeMode , ");
            strSql.Append(" Content = @Content , ");
            strSql.Append(" OperateTime = @OperateTime  ");
            strSql.Append(" where Rid=@Rid ");

            SqlParameter[] parameters = {
			            new SqlParameter("@Rid", SqlDbType.Int,4) ,            
                        new SqlParameter("@LoginName", SqlDbType.VarChar,50) ,   
                        new SqlParameter("@UserName", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@ChangeMode", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@Content", SqlDbType.Text) ,            
                        new SqlParameter("@OperateTime", SqlDbType.SmallDateTime)             
              
            };

            parameters[0].Value = model.Rid;
            parameters[1].Value = model.LoginName;
            parameters[2].Value = model.UserName;
            parameters[3].Value = model.ChangeMode;
            parameters[4].Value = model.Content;
            parameters[5].Value = model.OperateTime;
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
            strSql.Append("delete from sysOperateLog ");
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
            strSql.Append("delete from sysOperateLog ");
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
        public Bitshare.DataDecision.Model.sysOperateLog GetModel(int Rid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Rid, LoginName, UserName,ChangeMode, Content, OperateTime  ");
            strSql.Append("  from sysOperateLog ");
            strSql.Append(" where Rid=@Rid");
            SqlParameter[] parameters = {
					new SqlParameter("@Rid", SqlDbType.Int,4)
			};
            parameters[0].Value = Rid;


            Bitshare.DataDecision.Model.sysOperateLog model = new Bitshare.DataDecision.Model.sysOperateLog();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Rid"].ToString() != "")
                {
                    model.Rid = int.Parse(ds.Tables[0].Rows[0]["Rid"].ToString());
                }
                model.LoginName = ds.Tables[0].Rows[0]["LoginName"].ToString();
                model.UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                model.Content = ds.Tables[0].Rows[0]["Content"].ToString();
                model.ChangeMode = ds.Tables[0].Rows[0]["ChangeMode"].ToString();
                if (ds.Tables[0].Rows[0]["OperateTime"].ToString() != "")
                {
                    model.OperateTime = DateTime.Parse(ds.Tables[0].Rows[0]["OperateTime"].ToString());
                }

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
            strSql.Append(" FROM sysOperateLog ");
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
            strSql.Append(" FROM sysOperateLog ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }


    }
}

