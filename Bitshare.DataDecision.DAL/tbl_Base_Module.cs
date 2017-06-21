using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Bitshare.DataDecision.DBUtility;
using System.Data.SqlClient;
using Bitshare.Common;
namespace Bitshare.DataDecision.DAL
{
    //tbl_Base_Module
    public partial class tbl_Base_Module : BaseDAL, IBaseDAL<Bitshare.DataDecision.Model.tbl_Base_Module>
    {

        public bool Exists(int Rid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tbl_Base_Module");
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
        public int Add(Bitshare.DataDecision.Model.tbl_Base_Module model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tbl_Base_Module(");
            strSql.Append("UpdateTime,UpdateUser,ModuleId,ParentId,ModuleName,Icon,Url,Remark,CreateUser");
            strSql.Append(") values (");
            strSql.Append("@UpdateTime,@UpdateUser,@ModuleId,@ParentId,@ModuleName,@Icon,@Url,@Remark,@CreateUser");
            strSql.Append(") ");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
			            new SqlParameter("@UpdateTime", SqlDbType.SmallDateTime) ,            
                        new SqlParameter("@UpdateUser", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@ModuleId", SqlDbType.Int,4) ,            
                        new SqlParameter("@ParentId", SqlDbType.Int,4) ,            
                        new SqlParameter("@ModuleName", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@Icon", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@Url", SqlDbType.VarChar,1000) ,            
                        new SqlParameter("@Remark", SqlDbType.NChar,10) ,                  
                        new SqlParameter("@CreateUser", SqlDbType.VarChar,50)             
              
            };

            parameters[0].Value = model.UpdateTime;
            parameters[1].Value = model.UpdateUser;
            parameters[2].Value = model.ModuleId;
            parameters[3].Value = model.ParentId;
            parameters[4].Value = model.ModuleName;
            parameters[5].Value = model.Icon;
            parameters[6].Value = model.Url;
            parameters[7].Value = model.Remark;
            parameters[8].Value = model.CreateTime;
            parameters[9].Value = model.CreateUser;

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
        public bool Update(Bitshare.DataDecision.Model.tbl_Base_Module model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tbl_Base_Module set ");

            strSql.Append(" UpdateTime = @UpdateTime , ");
            strSql.Append(" UpdateUser = @UpdateUser , ");
            strSql.Append(" ModuleId = @ModuleId , ");
            strSql.Append(" ParentId = @ParentId , ");
            strSql.Append(" ModuleName = @ModuleName , ");
            strSql.Append(" Icon = @Icon , ");
            strSql.Append(" Url = @Url , ");
            strSql.Append(" Remark = @Remark , ");
            strSql.Append(" CreateTime = @CreateTime , ");
            strSql.Append(" CreateUser = @CreateUser  ");
            strSql.Append(" where Rid=@Rid ");

            SqlParameter[] parameters = {
			            new SqlParameter("@Rid", SqlDbType.Int,4) ,            
                        new SqlParameter("@UpdateTime", SqlDbType.SmallDateTime) ,            
                        new SqlParameter("@UpdateUser", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@ModuleId", SqlDbType.Int,4) ,            
                        new SqlParameter("@ParentId", SqlDbType.Int,4) ,            
                        new SqlParameter("@ModuleName", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@Icon", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@Url", SqlDbType.VarChar,1000) ,            
                        new SqlParameter("@Remark", SqlDbType.NChar,10) ,            
                        new SqlParameter("@CreateTime", SqlDbType.SmallDateTime) ,            
                        new SqlParameter("@CreateUser", SqlDbType.VarChar,50)             
              
            };

            parameters[0].Value = model.Rid;
            parameters[1].Value = model.UpdateTime;
            parameters[2].Value = model.UpdateUser;
            parameters[3].Value = model.ModuleId;
            parameters[4].Value = model.ParentId;
            parameters[5].Value = model.ModuleName;
            parameters[6].Value = model.Icon;
            parameters[7].Value = model.Url;
            parameters[8].Value = model.Remark;
            parameters[9].Value = model.CreateTime;
            parameters[10].Value = model.CreateUser;
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
            strSql.Append("delete from tbl_Base_Module ");
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
            strSql.Append("delete from tbl_Base_Module ");
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
        public Bitshare.DataDecision.Model.tbl_Base_Module GetModel(int Rid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Rid, UpdateTime, UpdateUser, ModuleId, ParentId, ModuleName, Icon, Url, Remark, CreateTime, CreateUser  ");
            strSql.Append("  from tbl_Base_Module ");
            strSql.Append(" where Rid=@Rid");
            SqlParameter[] parameters = {
					new SqlParameter("@Rid", SqlDbType.Int,4)
			};
            parameters[0].Value = Rid;


            Bitshare.DataDecision.Model.tbl_Base_Module model = new Bitshare.DataDecision.Model.tbl_Base_Module();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Rid"].ToString() != "")
                {
                    model.Rid = int.Parse(ds.Tables[0].Rows[0]["Rid"].ToString());
                }
                if (ds.Tables[0].Rows[0]["UpdateTime"].ToString() != "")
                {
                    model.UpdateTime = DateTime.Parse(ds.Tables[0].Rows[0]["UpdateTime"].ToString());
                }
                model.UpdateUser = ds.Tables[0].Rows[0]["UpdateUser"].ToString();
                if (ds.Tables[0].Rows[0]["ModuleId"].ToString() != "")
                {
                    model.ModuleId = int.Parse(ds.Tables[0].Rows[0]["ModuleId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ParentId"].ToString() != "")
                {
                    model.ParentId = int.Parse(ds.Tables[0].Rows[0]["ParentId"].ToString());
                }
                model.ModuleName = ds.Tables[0].Rows[0]["ModuleName"].ToString();
                model.Icon = ds.Tables[0].Rows[0]["Icon"].ToString();
                model.Url = ds.Tables[0].Rows[0]["Url"].ToString();
                model.Remark = ds.Tables[0].Rows[0]["Remark"].ToString();
                if (ds.Tables[0].Rows[0]["CreateTime"].ToString() != "")
                {
                    model.CreateTime = DateTime.Parse(ds.Tables[0].Rows[0]["CreateTime"].ToString());
                }
                model.CreateUser = ds.Tables[0].Rows[0]["CreateUser"].ToString();

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
            strSql.Append(" FROM tbl_Base_Module ");
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
            strSql.Append(" FROM tbl_Base_Module ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }




        public bool DeleteList(string Ridlist, string userName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update  tbl_Base_Module set isDel=1,UpdateUser='" + userName + "',");
            strSql.Append(" UpdateDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'");
            strSql.Append(" where Rid in " + Ridlist + "  ");
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

        public bool Delete(int Rid, Bitshare.DBUtility.TransactionInfo trans = null)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tbl_Base_Module set isDel=1");
            strSql.Append(" where Rid=@Rid");
            SqlParameter[] parameters = {
					new SqlParameter("@Rid", SqlDbType.Int,4)
			};
            parameters[0].Value = Rid;
            if (trans != null)
            {
                trans.AddCmd(strSql.ToString(), parameters);
                return false;
            }
            else
            {
                return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters) > 0;
            }
        }

        public bool Delete(string where, Bitshare.DBUtility.TransactionInfo trans = null)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tbl_Base_Module set isDel=1");
            strSql.Append(" where ");
            strSql.Append(where);
            if (trans != null)
            {
                trans.AddCmd(strSql.ToString(), null);
                return false;
            }
            else
            {
                return DbHelperSQL.ExecuteSql(strSql.ToString()) > 0;
            }
        }

        public List<Model.tbl_Base_Module> GetModelList(string strWhere)
        {
            DataSet ds = GetList(strWhere);
            return ds.Tables[0].ToList<Model.tbl_Base_Module>();
        }
    }
}
