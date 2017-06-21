using Bitshare.DataDecision.DBUtility;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace Bitshare.DataDecision.DAL
{
    /// <summary>
    /// 数据访问类:tblUser_Sys
    /// </summary>
    public partial class tblUser_Sys
	{
		public tblUser_Sys()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("Rid", "tblUser_Sys"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Rid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tblUser_Sys");
			strSql.Append(" where Rid=@Rid");
			SqlParameter[] parameters = {
					new SqlParameter("@Rid", SqlDbType.Int,4)
			};
			parameters[0].Value = Rid;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(Bitshare.DataDecision.Model.tblUser_Sys model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tblUser_Sys(");
            strSql.Append("LoginName,UserName,EnglishName,UserMark,dept,dept_New,UserPwd,PassWord,Remark,RoleFlag,DefaultRoleId,FailTimes)");
			strSql.Append(" values (");
            strSql.Append("@LoginName,@UserName,@EnglishName,@UserMark,@dept,@dept_New,@UserPwd,@PassWord,@Remark,@RoleFlag,@DefaultRoleId,@FailTimes)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@LoginName", SqlDbType.VarChar,50),
					new SqlParameter("@UserName", SqlDbType.VarChar,50),
					new SqlParameter("@EnglishName", SqlDbType.VarChar,50),
					new SqlParameter("@UserMark", SqlDbType.VarChar,50),
					new SqlParameter("@dept", SqlDbType.VarChar,50),
					new SqlParameter("@dept_New", SqlDbType.VarChar,50),
					new SqlParameter("@UserPwd", SqlDbType.VarChar,50),
					new SqlParameter("@PassWord", SqlDbType.VarChar,50),
					new SqlParameter("@Remark", SqlDbType.VarChar,100),
					new SqlParameter("@RoleFlag", SqlDbType.Int,4),
                    new SqlParameter("@DefaultRoleId", SqlDbType.Int,4),
                    new SqlParameter("@FailTimes", SqlDbType.Int,4)
                                        };
			parameters[0].Value = model.LoginName;
			parameters[1].Value = model.UserName;
			parameters[2].Value = model.EnglishName;
			parameters[3].Value = model.UserMark;
			parameters[4].Value = model.dept;
			parameters[5].Value = model.dept_New;
			parameters[6].Value = model.UserPwd;
			parameters[7].Value = model.PassWord;
			parameters[8].Value = model.Remark;
			parameters[9].Value = model.RoleFlag;
            parameters[10].Value = model.DefaultRoleId;
            parameters[11].Value = model.FailTimes;
			object obj = DbHelperSQL.GetSingle(strSql.ToString(),parameters);
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
		public bool Update(Bitshare.DataDecision.Model.tblUser_Sys model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tblUser_Sys set ");
			strSql.Append("LoginName=@LoginName,");
			strSql.Append("UserName=@UserName,");
			strSql.Append("EnglishName=@EnglishName,");
			strSql.Append("UserMark=@UserMark,");
			strSql.Append("dept=@dept,");
			strSql.Append("dept_New=@dept_New,");
			strSql.Append("UserPwd=@UserPwd,");
			strSql.Append("PassWord=@PassWord,");
			strSql.Append("Remark=@Remark,");
			strSql.Append("RoleFlag=@RoleFlag,");
            strSql.Append("DefaultRoleId=@DefaultRoleId,");
            strSql.Append("FailTimes=@FailTimes");
			strSql.Append(" where Rid=@Rid");
			SqlParameter[] parameters = {
					new SqlParameter("@LoginName", SqlDbType.VarChar,50),
					new SqlParameter("@UserName", SqlDbType.VarChar,50),
					new SqlParameter("@EnglishName", SqlDbType.VarChar,50),
					new SqlParameter("@UserMark", SqlDbType.VarChar,50),
					new SqlParameter("@dept", SqlDbType.VarChar,50),
					new SqlParameter("@dept_New", SqlDbType.VarChar,50),
					new SqlParameter("@UserPwd", SqlDbType.VarChar,50),
					new SqlParameter("@PassWord", SqlDbType.VarChar,50),
					new SqlParameter("@Remark", SqlDbType.VarChar,100),
					new SqlParameter("@RoleFlag", SqlDbType.Int,4),
                    new SqlParameter("@DefaultRoleId", SqlDbType.Int,4),
                    new SqlParameter("@FailTimes", SqlDbType.Int,4),
					new SqlParameter("@Rid", SqlDbType.Int,4)
                                        };
			parameters[0].Value = model.LoginName;
			parameters[1].Value = model.UserName;
			parameters[2].Value = model.EnglishName;
			parameters[3].Value = model.UserMark;
			parameters[4].Value = model.dept;
			parameters[5].Value = model.dept_New;
			parameters[6].Value = model.UserPwd;
			parameters[7].Value = model.PassWord;
			parameters[8].Value = model.Remark;
			parameters[9].Value = model.RoleFlag;
            parameters[10].Value = model.DefaultRoleId;
            parameters[11].Value = model.FailTimes;
			parameters[12].Value = model.Rid;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
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
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tblUser_Sys ");
			strSql.Append(" where Rid=@Rid");
			SqlParameter[] parameters = {
					new SqlParameter("@Rid", SqlDbType.Int,4)
			};
			parameters[0].Value = Rid;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
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
		/// 批量删除数据
		/// </summary>
		public bool DeleteList(string Ridlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tblUser_Sys ");
			strSql.Append(" where Rid in ("+Ridlist + ")  ");
			int rows=DbHelperSQL.ExecuteSql(strSql.ToString());
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
		public Bitshare.DataDecision.Model.tblUser_Sys GetModel(int Rid)
		{
			
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select  top 1 Rid,LoginName,UserName,EnglishName,UserMark,dept,dept_New,UserPwd,PassWord,Remark,RoleFlag,DefaultRoleId,FailTimes from tblUser_Sys ");
			strSql.Append(" where Rid=@Rid");
			SqlParameter[] parameters = {
					new SqlParameter("@Rid", SqlDbType.Int,4)
			};
			parameters[0].Value = Rid;

			Bitshare.DataDecision.Model.tblUser_Sys model=new Bitshare.DataDecision.Model.tblUser_Sys();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				return DataRowToModel(ds.Tables[0].Rows[0]);
			}
			else
			{
				return null;
			}
		}
        /// <summary>
        /// 根据登录名称得到一个对象实体
        /// </summary>
        public Bitshare.DataDecision.Model.tblUser_Sys GetModelByLoginName(string loginName)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Rid,LoginName,UserName,EnglishName,UserMark,dept,dept_New,UserPwd,PassWord,Remark,RoleFlag,DefaultRoleId,FailTimes from tblUser_Sys ");
            strSql.Append(" where loginName=@loginName");
            SqlParameter[] parameters = {
					new SqlParameter("@loginName", SqlDbType.VarChar,50)
			};
            parameters[0].Value = loginName;

            Bitshare.DataDecision.Model.tblUser_Sys model = new Bitshare.DataDecision.Model.tblUser_Sys();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Bitshare.DataDecision.Model.tblUser_Sys DataRowToModel(DataRow row)
		{
			Bitshare.DataDecision.Model.tblUser_Sys model=new Bitshare.DataDecision.Model.tblUser_Sys();
			if (row != null)
			{
				if(row["Rid"]!=null && row["Rid"].ToString()!="")
				{
					model.Rid=int.Parse(row["Rid"].ToString());
				}
				if(row["LoginName"]!=null)
				{
					model.LoginName=row["LoginName"].ToString();
				}
				if(row["UserName"]!=null)
				{
					model.UserName=row["UserName"].ToString();
				}
				if(row["EnglishName"]!=null)
				{
					model.EnglishName=row["EnglishName"].ToString();
				}
				if(row["UserMark"]!=null)
				{
					model.UserMark=row["UserMark"].ToString();
				}
				if(row["dept"]!=null)
				{
					model.dept=row["dept"].ToString();
				}
				if(row["dept_New"]!=null)
				{
					model.dept_New=row["dept_New"].ToString();
				}
				if(row["UserPwd"]!=null)
				{
					model.UserPwd=row["UserPwd"].ToString();
				}
				if(row["PassWord"]!=null)
				{
					model.PassWord=row["PassWord"].ToString();
				}
				if(row["Remark"]!=null)
				{
					model.Remark=row["Remark"].ToString();
				}
				if(row["RoleFlag"]!=null && row["RoleFlag"].ToString()!="")
				{
					model.RoleFlag=int.Parse(row["RoleFlag"].ToString());
				}
                if (row["DefaultRoleId"] != null && row["DefaultRoleId"].ToString() != "")
				{
                    model.DefaultRoleId = int.Parse(row["DefaultRoleId"].ToString());
				}
                if (row["FailTimes"] != null && row["FailTimes"].ToString() != "")
				{
                    model.FailTimes = int.Parse(row["FailTimes"].ToString());
				}
			}
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select Rid,LoginName,UserName,EnglishName,UserMark,dept,dept_New,UserPwd,PassWord,Remark,RoleFlag,DefaultRoleId,FailTimes ");
			strSql.Append(" FROM tblUser_Sys ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
            strSql.Append(" Rid,LoginName,UserName,EnglishName,UserMark,dept,dept_New,UserPwd,PassWord,Remark,RoleFlag,DefaultRoleId,FailTimes ");
			strSql.Append(" FROM tblUser_Sys ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM tblUser_Sys ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			object obj = DbHelperSQL.GetSingle(strSql.ToString());
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
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT * FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby );
			}
			else
			{
				strSql.Append("order by T.Rid desc");
			}
			strSql.Append(")AS Row, T.*  from tblUser_Sys T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			return DbHelperSQL.Query(strSql.ToString());
		}
		#endregion  BasicMethod
	}
}

