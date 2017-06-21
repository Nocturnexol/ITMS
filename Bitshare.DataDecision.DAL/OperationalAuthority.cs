using Bitshare.DataDecision.DBUtility;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace Bitshare.DataDecision.DAL
{

    public partial class OperationalAuthority
    {

        public bool Exists(int Rid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from OperationalAuthority");
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
        public int Add(Bitshare.DataDecision.Model.OperationalAuthority model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into OperationalAuthority(");
            strSql.Append("UpdateUser,OperRational_Name,RightsOptions,Options,CreateDataTime,CreateDataUser,HandleCompany,UpdateTime");
            strSql.Append(") values (");
            strSql.Append("@UpdateUser,@OperRational_Name,@RightsOptions,@Options,@CreateDataTime,@CreateDataUser,@HandleCompany,@UpdateTime");
            strSql.Append(") ");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
			            new SqlParameter("@UpdateUser", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@OperRational_Name", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@RightsOptions", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@Options", SqlDbType.Bit,1) ,            
                        new SqlParameter("@CreateDataTime", SqlDbType.SmallDateTime) ,            
                        new SqlParameter("@CreateDataUser", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@HandleCompany", SqlDbType.VarChar,200) ,            
                        new SqlParameter("@UpdateTime", SqlDbType.SmallDateTime)             
              
            };

            parameters[0].Value = model.UpdateUser;
            parameters[1].Value = model.OperRational_Name;
            parameters[2].Value = model.RightsOptions;
            parameters[3].Value = model.Options;
            parameters[4].Value = model.CreateDataTime;
            parameters[5].Value = model.CreateDataUser;
            parameters[6].Value = model.HandleCompany;
            parameters[7].Value = model.UpdateTime;

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
        public bool Update(Bitshare.DataDecision.Model.OperationalAuthority model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update OperationalAuthority set ");

            strSql.Append(" UpdateUser = @UpdateUser , ");
            strSql.Append(" OperRational_Name = @OperRational_Name , ");
            strSql.Append(" RightsOptions = @RightsOptions , ");
            strSql.Append(" Options = @Options , ");
            strSql.Append(" CreateDataTime = @CreateDataTime , ");
            strSql.Append(" CreateDataUser = @CreateDataUser , ");
            strSql.Append(" HandleCompany = @HandleCompany , ");
            strSql.Append(" UpdateTime = @UpdateTime  ");
            strSql.Append(" where Rid=@Rid ");

            SqlParameter[] parameters = {
			            new SqlParameter("@Rid", SqlDbType.Int,4) ,            
                        new SqlParameter("@UpdateUser", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@OperRational_Name", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@RightsOptions", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@Options", SqlDbType.Bit,1) ,            
                        new SqlParameter("@CreateDataTime", SqlDbType.SmallDateTime) ,            
                        new SqlParameter("@CreateDataUser", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@HandleCompany", SqlDbType.VarChar,200) ,            
                        new SqlParameter("@UpdateTime", SqlDbType.SmallDateTime)             
              
            };

            parameters[0].Value = model.Rid;
            parameters[1].Value = model.UpdateUser;
            parameters[2].Value = model.OperRational_Name;
            parameters[3].Value = model.RightsOptions;
            parameters[4].Value = model.Options;
            parameters[5].Value = model.CreateDataTime;
            parameters[6].Value = model.CreateDataUser;
            parameters[7].Value = model.HandleCompany;
            parameters[8].Value = model.UpdateTime;
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
            strSql.Append("delete from OperationalAuthority ");
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
            strSql.Append("delete from OperationalAuthority ");
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
        public Bitshare.DataDecision.Model.OperationalAuthority GetModel(int Rid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Rid, UpdateUser, OperRational_Name, RightsOptions, Options, CreateDataTime, CreateDataUser, HandleCompany, UpdateTime  ");
            strSql.Append("  from OperationalAuthority ");
            strSql.Append(" where Rid=@Rid");
            SqlParameter[] parameters = {
					new SqlParameter("@Rid", SqlDbType.Int,4)
			};
            parameters[0].Value = Rid;


            Bitshare.DataDecision.Model.OperationalAuthority model = new Bitshare.DataDecision.Model.OperationalAuthority();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Rid"].ToString() != "")
                {
                    model.Rid = int.Parse(ds.Tables[0].Rows[0]["Rid"].ToString());
                }
                model.UpdateUser = ds.Tables[0].Rows[0]["UpdateUser"].ToString();
                model.OperRational_Name = ds.Tables[0].Rows[0]["OperRational_Name"].ToString();
                model.RightsOptions = ds.Tables[0].Rows[0]["RightsOptions"].ToString();
                if (ds.Tables[0].Rows[0]["Options"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Options"].ToString() == "1") || (ds.Tables[0].Rows[0]["Options"].ToString().ToLower() == "true"))
                    {
                        model.Options = true;
                    }
                    else
                    {
                        model.Options = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["CreateDataTime"].ToString() != "")
                {
                    model.CreateDataTime = DateTime.Parse(ds.Tables[0].Rows[0]["CreateDataTime"].ToString());
                }
                model.CreateDataUser = ds.Tables[0].Rows[0]["CreateDataUser"].ToString();
                model.HandleCompany = ds.Tables[0].Rows[0]["HandleCompany"].ToString();
                if (ds.Tables[0].Rows[0]["UpdateTime"].ToString() != "")
                {
                    model.UpdateTime = DateTime.Parse(ds.Tables[0].Rows[0]["UpdateTime"].ToString());
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
            strSql.Append(" FROM OperationalAuthority ");
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
            strSql.Append(" FROM OperationalAuthority ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }


    }
}

