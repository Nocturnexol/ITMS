using Bitshare.DataDecision.DBUtility;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace Bitshare.DataDecision.DAL
{
    /// <summary>
    /// 数据访问类:View_User_Roles
    /// </summary>
    public partial class View_User_Roles
	{
		public View_User_Roles()
		{}
		#region  BasicMethod

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string UserName,string role_name,string LoginName)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from View_User_Roles");
			strSql.Append(" where UserName=@UserName and role_name=@role_name and LoginName=@LoginName ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserName", SqlDbType.VarChar,50),
					new SqlParameter("@role_name", SqlDbType.VarChar,50),
					new SqlParameter("@LoginName", SqlDbType.VarChar,50)			};
			parameters[0].Value = UserName;
			parameters[1].Value = role_name;
			parameters[2].Value = LoginName;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Bitshare.DataDecision.Model.View_User_Roles model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into View_User_Roles(");
			strSql.Append("UserName,role_name,LoginName)");
			strSql.Append(" values (");
			strSql.Append("@UserName,@role_name,@LoginName)");
			SqlParameter[] parameters = {
					new SqlParameter("@UserName", SqlDbType.VarChar,50),
					new SqlParameter("@role_name", SqlDbType.VarChar,50),
					new SqlParameter("@LoginName", SqlDbType.VarChar,50)};
			parameters[0].Value = model.UserName;
			parameters[1].Value = model.role_name;
			parameters[2].Value = model.LoginName;

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
		/// 更新一条数据
		/// </summary>
		public bool Update(Bitshare.DataDecision.Model.View_User_Roles model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update View_User_Roles set ");
			strSql.Append("UserName=@UserName,");
			strSql.Append("role_name=@role_name,");
			strSql.Append("LoginName=@LoginName");
			strSql.Append(" where UserName=@UserName and role_name=@role_name and LoginName=@LoginName ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserName", SqlDbType.VarChar,50),
					new SqlParameter("@role_name", SqlDbType.VarChar,50),
					new SqlParameter("@LoginName", SqlDbType.VarChar,50)};
			parameters[0].Value = model.UserName;
			parameters[1].Value = model.role_name;
			parameters[2].Value = model.LoginName;

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
		public bool Delete(string UserName,string role_name,string LoginName)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from View_User_Roles ");
			strSql.Append(" where UserName=@UserName and role_name=@role_name and LoginName=@LoginName ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserName", SqlDbType.VarChar,50),
					new SqlParameter("@role_name", SqlDbType.VarChar,50),
					new SqlParameter("@LoginName", SqlDbType.VarChar,50)			};
			parameters[0].Value = UserName;
			parameters[1].Value = role_name;
			parameters[2].Value = LoginName;

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
		/// 得到一个对象实体
		/// </summary>
		public Bitshare.DataDecision.Model.View_User_Roles GetModel(string UserName,string role_name,string LoginName)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 UserName,role_name,LoginName from View_User_Roles ");
			strSql.Append(" where UserName=@UserName and role_name=@role_name and LoginName=@LoginName ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserName", SqlDbType.VarChar,50),
					new SqlParameter("@role_name", SqlDbType.VarChar,50),
					new SqlParameter("@LoginName", SqlDbType.VarChar,50)			};
			parameters[0].Value = UserName;
			parameters[1].Value = role_name;
			parameters[2].Value = LoginName;

			Bitshare.DataDecision.Model.View_User_Roles model=new Bitshare.DataDecision.Model.View_User_Roles();
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
		/// 得到一个对象实体
		/// </summary>
		public Bitshare.DataDecision.Model.View_User_Roles DataRowToModel(DataRow row)
		{
			Bitshare.DataDecision.Model.View_User_Roles model=new Bitshare.DataDecision.Model.View_User_Roles();
			if (row != null)
			{
				if(row["UserName"]!=null)
				{
					model.UserName=row["UserName"].ToString();
				}
				if(row["role_name"]!=null)
				{
					model.role_name=row["role_name"].ToString();
				}
				if(row["LoginName"]!=null)
				{
					model.LoginName=row["LoginName"].ToString();
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
			strSql.Append("select UserName,role_name,LoginName ");
			strSql.Append(" FROM View_User_Roles ");
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
			strSql.Append(" UserName,role_name,LoginName ");
			strSql.Append(" FROM View_User_Roles ");
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
			strSql.Append("select count(1) FROM View_User_Roles ");
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
				strSql.Append("order by T.LoginName desc");
			}
			strSql.Append(")AS Row, T.*  from View_User_Roles T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
			parameters[0].Value = "View_User_Roles";
			parameters[1].Value = "LoginName";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

