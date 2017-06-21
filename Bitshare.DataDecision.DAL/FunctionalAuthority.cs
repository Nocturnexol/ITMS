using Bitshare.DataDecision.DBUtility;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace Bitshare.DataDecision.DAL
{
    /// <summary>
    /// 数据访问类:FunctionalAuthority
    /// </summary>
    public partial class FunctionalAuthority
	{
		public FunctionalAuthority()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("Rid", "FunctionalAuthority"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Rid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from FunctionalAuthority");
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
		public int Add(Bitshare.DataDecision.Model.FunctionalAuthority model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into FunctionalAuthority(");
			strSql.Append("Module_Name,Module_Id,Group_Name,Group_Id,Right_Name,Right_Id,Right_Url,Rigth_Tip,Remark)");
			strSql.Append(" values (");
			strSql.Append("@Module_Name,@Module_Id,@Group_Name,@Group_Id,@Right_Name,@Right_Id,@Right_Url,@Rigth_Tip,@Remark)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Module_Name", SqlDbType.VarChar,50),
					new SqlParameter("@Module_Id", SqlDbType.Int,4),
					new SqlParameter("@Group_Name", SqlDbType.VarChar,50),
					new SqlParameter("@Group_Id", SqlDbType.Int,4),
					new SqlParameter("@Right_Name", SqlDbType.VarChar,50),
					new SqlParameter("@Right_Id", SqlDbType.Decimal,9),
					new SqlParameter("@Right_Url", SqlDbType.VarChar,8000),
					new SqlParameter("@Rigth_Tip", SqlDbType.VarChar,50),
					new SqlParameter("@Remark", SqlDbType.VarChar,100)};
			parameters[0].Value = model.Module_Name;
			parameters[1].Value = model.Module_Id;
			parameters[2].Value = model.Group_Name;
			parameters[3].Value = model.Group_Id;
			parameters[4].Value = model.Right_Name;
			parameters[5].Value = model.Right_Id;
			parameters[6].Value = model.Right_Url;
			parameters[7].Value = model.Rigth_Tip;
			parameters[8].Value = model.Remark;

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
		public bool Update(Bitshare.DataDecision.Model.FunctionalAuthority model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update FunctionalAuthority set ");
			strSql.Append("Module_Name=@Module_Name,");
            //strSql.Append("Module_Id=@Module_Id,");
			strSql.Append("Group_Name=@Group_Name,");
            //strSql.Append("Group_Id=@Group_Id,");
			strSql.Append("Right_Name=@Right_Name,");
            //strSql.Append("Right_Id=@Right_Id,");
			strSql.Append("Right_Url=@Right_Url,");
			strSql.Append("Rigth_Tip=@Rigth_Tip,");
			strSql.Append("Remark=@Remark");
			strSql.Append(" where Rid=@Rid");
			SqlParameter[] parameters = {
					new SqlParameter("@Module_Name", SqlDbType.VarChar,50),
					//new SqlParameter("@Module_Id", SqlDbType.Int,4),
					new SqlParameter("@Group_Name", SqlDbType.VarChar,50),
					//new SqlParameter("@Group_Id", SqlDbType.Int,4),
					new SqlParameter("@Right_Name", SqlDbType.VarChar,50),
					//new SqlParameter("@Right_Id", SqlDbType.Decimal,9),
					new SqlParameter("@Right_Url", SqlDbType.VarChar,8000),
					new SqlParameter("@Rigth_Tip", SqlDbType.VarChar,50),
					new SqlParameter("@Remark", SqlDbType.VarChar,100),
					new SqlParameter("@Rid", SqlDbType.Int,4)};
			parameters[0].Value = model.Module_Name;
			//parameters[1].Value = model.Module_Id;
			parameters[1].Value = model.Group_Name;
			//parameters[3].Value = model.Group_Id;
			parameters[2].Value = model.Right_Name;
			//parameters[5].Value = model.Right_Id;
			parameters[3].Value = model.Right_Url;
			parameters[4].Value = model.Rigth_Tip;
			parameters[5].Value = model.Remark;
			parameters[6].Value = model.Rid;

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
			strSql.Append("delete from FunctionalAuthority ");
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
			strSql.Append("delete from FunctionalAuthority ");
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
		public Bitshare.DataDecision.Model.FunctionalAuthority GetModel(int Rid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Rid,Module_Name,Module_Id,Group_Name,Group_Id,Right_Name,Right_Id,Right_Url,Rigth_Tip,Remark from FunctionalAuthority ");
			strSql.Append(" where Rid=@Rid");
			SqlParameter[] parameters = {
					new SqlParameter("@Rid", SqlDbType.Int,4)
			};
			parameters[0].Value = Rid;

			Bitshare.DataDecision.Model.FunctionalAuthority model=new Bitshare.DataDecision.Model.FunctionalAuthority();
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
		public Bitshare.DataDecision.Model.FunctionalAuthority DataRowToModel(DataRow row)
		{
			Bitshare.DataDecision.Model.FunctionalAuthority model=new Bitshare.DataDecision.Model.FunctionalAuthority();
			if (row != null)
			{
				if(row["Rid"]!=null && row["Rid"].ToString()!="")
				{
					model.Rid=int.Parse(row["Rid"].ToString());
				}
				if(row["Module_Name"]!=null)
				{
					model.Module_Name=row["Module_Name"].ToString();
				}
				if(row["Module_Id"]!=null && row["Module_Id"].ToString()!="")
				{
					model.Module_Id=int.Parse(row["Module_Id"].ToString());
				}
				if(row["Group_Name"]!=null)
				{
					model.Group_Name=row["Group_Name"].ToString();
				}
				if(row["Group_Id"]!=null && row["Group_Id"].ToString()!="")
				{
					model.Group_Id=int.Parse(row["Group_Id"].ToString());
				}
				if(row["Right_Name"]!=null)
				{
					model.Right_Name=row["Right_Name"].ToString();
				}
				if(row["Right_Id"]!=null && row["Right_Id"].ToString()!="")
				{
					model.Right_Id=decimal.Parse(row["Right_Id"].ToString());
				}
				if(row["Right_Url"]!=null)
				{
					model.Right_Url=row["Right_Url"].ToString();
				}
				if(row["Rigth_Tip"]!=null)
				{
					model.Rigth_Tip=row["Rigth_Tip"].ToString();
				}
				if(row["Remark"]!=null)
				{
					model.Remark=row["Remark"].ToString();
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
			strSql.Append("select Rid,Module_Name,Module_Id,Group_Name,Group_Id,Right_Name,Right_Id,Right_Url,Rigth_Tip,Remark ");
			strSql.Append(" FROM FunctionalAuthority ");
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
			strSql.Append(" Rid,Module_Name,Module_Id,Group_Name,Group_Id,Right_Name,Right_Id,Right_Url,Rigth_Tip,Remark ");
			strSql.Append(" FROM FunctionalAuthority ");
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
			strSql.Append("select count(1) FROM FunctionalAuthority ");
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
			strSql.Append(")AS Row, T.*  from FunctionalAuthority T ");
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
			parameters[0].Value = "FunctionalAuthority";
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

