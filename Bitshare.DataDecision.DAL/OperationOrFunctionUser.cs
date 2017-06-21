using Bitshare.DataDecision.DBUtility;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace Bitshare.DataDecision.DAL
{
    /// <summary>
    /// 数据访问类:OperationOrFunctionUser
    /// </summary>
    public partial class OperationOrFunctionUser
	{
		public OperationOrFunctionUser()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public  Bitshare.DataDecision.Model.OperationOrFunctionUser GetModel(string UserName,string rf_Type,string OperRational_Name,string rf_Right_Authority)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 UserName,rf_Type,OperRational_Name,rf_Right_Authority from OperationOrFunctionUser ");
			strSql.Append(" where UserName=@UserName and rf_Type=@rf_Type and OperRational_Name=@OperRational_Name and rf_Right_Authority=@rf_Right_Authority ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserName", SqlDbType.VarChar,50),
					new SqlParameter("@rf_Type", SqlDbType.VarChar,50),
					new SqlParameter("@OperRational_Name", SqlDbType.VarChar,50),
					new SqlParameter("@rf_Right_Authority", SqlDbType.VarChar,50)			};
			parameters[0].Value = UserName;
			parameters[1].Value = rf_Type;
			parameters[2].Value = OperRational_Name;
			parameters[3].Value = rf_Right_Authority;

			 Bitshare.DataDecision.Model.OperationOrFunctionUser model=new  Bitshare.DataDecision.Model.OperationOrFunctionUser();
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
		public  Bitshare.DataDecision.Model.OperationOrFunctionUser DataRowToModel(DataRow row)
		{
			 Bitshare.DataDecision.Model.OperationOrFunctionUser model=new  Bitshare.DataDecision.Model.OperationOrFunctionUser();
			if (row != null)
			{
				if(row["UserName"]!=null)
				{
					model.UserName=row["UserName"].ToString();
				}
				if(row["rf_Type"]!=null)
				{
					model.rf_Type=row["rf_Type"].ToString();
				}
				if(row["OperRational_Name"]!=null)
				{
					model.OperRational_Name=row["OperRational_Name"].ToString();
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
			strSql.Append("select UserName,rf_Type,OperRational_Name,rf_Right_Authority ");
			strSql.Append(" FROM OperationOrFunctionUser ");
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
			strSql.Append(" UserName,rf_Type,OperRational_Name,rf_Right_Authority ");
			strSql.Append(" FROM OperationOrFunctionUser ");
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
			strSql.Append("select count(1) FROM OperationOrFunctionUser ");
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
				strSql.Append("order by T.rf_Right_Authority desc");
			}
			strSql.Append(")AS Row, T.*  from OperationOrFunctionUser T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			return DbHelperSQL.Query(strSql.ToString());
		}
		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

