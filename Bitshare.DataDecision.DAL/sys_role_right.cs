using Bitshare.DataDecision.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace Bitshare.DataDecision.DAL
{
    /// <summary>
    /// 数据访问类:sys_role_right
    /// </summary>
    public partial class sys_role_right
	{
		public sys_role_right()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("Rid", "sys_role_right"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Rid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from sys_role_right");
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
		public int Add(Bitshare.DataDecision.Model.sys_role_right model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into sys_role_right(");
			strSql.Append("rf_Type,rf_Role_Id,rf_Right_Code,rf_Right_Authority)");
			strSql.Append(" values (");
			strSql.Append("@rf_Type,@rf_Role_Id,@rf_Right_Code,@rf_Right_Authority)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@rf_Type", SqlDbType.VarChar,50),
					new SqlParameter("@rf_Role_Id", SqlDbType.Int,4),
					new SqlParameter("@rf_Right_Code", SqlDbType.Int,4),
					new SqlParameter("@rf_Right_Authority", SqlDbType.VarChar,50)};
			parameters[0].Value = model.rf_Type;
			parameters[1].Value = model.rf_Role_Id;
			parameters[2].Value = model.rf_Right_Code;
			parameters[3].Value = model.rf_Right_Authority;

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
        /// 增加多条数据
        /// </summary>
        public Boolean Addlist(List<Bitshare.DataDecision.Model.sys_role_right> addlist,List<Bitshare.DataDecision.Model.sys_role_right> dellist, int RoleId)
        {
            List<string> lsSql = new List<string>();
            //先删除
            if (dellist.Count > 0)
            {
                List<string> tbls = new List<string>();
                foreach (var t in dellist)
                {
                    tbls.Add(Convert.ToString(t.Rid));
                }
                string str = string.Join(",", tbls);
                lsSql.Add("delete from  sys_role_right where  rf_Role_Id=" + RoleId + " and Rid in(" + str + ")");
            }
            foreach (Bitshare.DataDecision.Model.sys_role_right model in addlist)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into sys_role_right(");
                strSql.Append("rf_Type,rf_Role_Id,rf_Right_Code,rf_Right_Authority)");
                strSql.Append(" values (");
                strSql.Append("'"+model.rf_Type+"',"+model.rf_Role_Id+","+model.rf_Right_Code+",'"+model.rf_Right_Authority+"')");
                lsSql.Add(strSql.ToString());
            }
            return DbHelperSQL.ExecuteSqlTran(lsSql) > 0;
        }

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Bitshare.DataDecision.Model.sys_role_right model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update sys_role_right set ");
			strSql.Append("rf_Type=@rf_Type,");
			strSql.Append("rf_Role_Id=@rf_Role_Id,");
			strSql.Append("rf_Right_Code=@rf_Right_Code,");
			strSql.Append("rf_Right_Authority=@rf_Right_Authority");
			strSql.Append(" where Rid=@Rid");
			SqlParameter[] parameters = {
					new SqlParameter("@rf_Type", SqlDbType.VarChar,50),
					new SqlParameter("@rf_Role_Id", SqlDbType.Int,4),
					new SqlParameter("@rf_Right_Code", SqlDbType.Int,4),
					new SqlParameter("@rf_Right_Authority", SqlDbType.VarChar,50),
					new SqlParameter("@Rid", SqlDbType.Int,4)};
			parameters[0].Value = model.rf_Type;
			parameters[1].Value = model.rf_Role_Id;
			parameters[2].Value = model.rf_Right_Code;
			parameters[3].Value = model.rf_Right_Authority;
			parameters[4].Value = model.Rid;

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
			strSql.Append("delete from sys_role_right ");
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
			strSql.Append("delete from sys_role_right ");
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
		public Bitshare.DataDecision.Model.sys_role_right GetModel(int Rid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Rid,rf_Type,rf_Role_Id,rf_Right_Code,rf_Right_Authority from sys_role_right ");
			strSql.Append(" where Rid=@Rid");
			SqlParameter[] parameters = {
					new SqlParameter("@Rid", SqlDbType.Int,4)
			};
			parameters[0].Value = Rid;

			Bitshare.DataDecision.Model.sys_role_right model=new Bitshare.DataDecision.Model.sys_role_right();
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
		public Bitshare.DataDecision.Model.sys_role_right DataRowToModel(DataRow row)
		{
			Bitshare.DataDecision.Model.sys_role_right model=new Bitshare.DataDecision.Model.sys_role_right();
			if (row != null)
			{
				if(row["Rid"]!=null && row["Rid"].ToString()!="")
				{
					model.Rid=int.Parse(row["Rid"].ToString());
				}
				if(row["rf_Type"]!=null)
				{
					model.rf_Type=row["rf_Type"].ToString();
				}
				if(row["rf_Role_Id"]!=null && row["rf_Role_Id"].ToString()!="")
				{
					model.rf_Role_Id=int.Parse(row["rf_Role_Id"].ToString());
				}
				if(row["rf_Right_Code"]!=null && row["rf_Right_Code"].ToString()!="")
				{
					model.rf_Right_Code=int.Parse(row["rf_Right_Code"].ToString());
				}
				if(row["rf_Right_Authority"]!=null)
				{
					model.rf_Right_Authority=row["rf_Right_Authority"].ToString();
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
			strSql.Append("select Rid,rf_Type,rf_Role_Id,rf_Right_Code,rf_Right_Authority ");
			strSql.Append(" FROM sys_role_right ");
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
			strSql.Append(" Rid,rf_Type,rf_Role_Id,rf_Right_Code,rf_Right_Authority ");
			strSql.Append(" FROM sys_role_right ");
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
			strSql.Append("select count(1) FROM sys_role_right ");
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
			strSql.Append(")AS Row, T.*  from sys_role_right T ");
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
			parameters[0].Value = "sys_role_right";
			parameters[1].Value = "Rid";
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

