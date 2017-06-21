using Bitshare.DataDecision.DBUtility;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace Bitshare.DataDecision.DAL
{
    /// <summary>
    /// 数据访问类:tblPageDetail
    /// </summary>
    public partial class tblPageDetail
	{
		public tblPageDetail()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("Rid", "tblPageDetail"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Rid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tblPageDetail");
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
		public int Add(Bitshare.DataDecision.Model.tblPageDetail model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tblPageDetail(");
			strSql.Append("ModelName,PageName,DetailName,Remark)");
			strSql.Append(" values (");
			strSql.Append("@ModelName,@PageName,@DetailName,@Remark)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@ModelName", SqlDbType.VarChar,50),
					new SqlParameter("@PageName", SqlDbType.VarChar,50),
					new SqlParameter("@DetailName", SqlDbType.VarChar,50),
					new SqlParameter("@Remark", SqlDbType.VarChar,500)};
			parameters[0].Value = model.ModelName;
			parameters[1].Value = model.PageName;
			parameters[2].Value = model.DetailName;
			parameters[3].Value = model.Remark;

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
		public bool Update(Bitshare.DataDecision.Model.tblPageDetail model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tblPageDetail set ");
			strSql.Append("ModelName=@ModelName,");
			strSql.Append("PageName=@PageName,");
			strSql.Append("DetailName=@DetailName,");
			strSql.Append("Remark=@Remark");
			strSql.Append(" where Rid=@Rid");
			SqlParameter[] parameters = {
					new SqlParameter("@ModelName", SqlDbType.VarChar,50),
					new SqlParameter("@PageName", SqlDbType.VarChar,50),
					new SqlParameter("@DetailName", SqlDbType.VarChar,50),
					new SqlParameter("@Remark", SqlDbType.VarChar,500),
					new SqlParameter("@Rid", SqlDbType.Int,4)};
			parameters[0].Value = model.ModelName;
			parameters[1].Value = model.PageName;
			parameters[2].Value = model.DetailName;
			parameters[3].Value = model.Remark;
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
			strSql.Append("delete from tblPageDetail ");
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
			strSql.Append("delete from tblPageDetail ");
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
		public Bitshare.DataDecision.Model.tblPageDetail GetModel(int Rid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ModelName,PageName,DetailName,Remark,Rid from tblPageDetail ");
			strSql.Append(" where Rid=@Rid");
			SqlParameter[] parameters = {
					new SqlParameter("@Rid", SqlDbType.Int,4)
			};
			parameters[0].Value = Rid;

			Bitshare.DataDecision.Model.tblPageDetail model=new Bitshare.DataDecision.Model.tblPageDetail();
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
		public Bitshare.DataDecision.Model.tblPageDetail DataRowToModel(DataRow row)
		{
			Bitshare.DataDecision.Model.tblPageDetail model=new Bitshare.DataDecision.Model.tblPageDetail();
			if (row != null)
			{
				if(row["ModelName"]!=null)
				{
					model.ModelName=row["ModelName"].ToString();
				}
				if(row["PageName"]!=null)
				{
					model.PageName=row["PageName"].ToString();
				}
				if(row["DetailName"]!=null)
				{
					model.DetailName=row["DetailName"].ToString();
				}
				if(row["Remark"]!=null)
				{
					model.Remark=row["Remark"].ToString();
				}
				if(row["Rid"]!=null && row["Rid"].ToString()!="")
				{
					model.Rid=int.Parse(row["Rid"].ToString());
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
			strSql.Append("select ModelName,PageName,DetailName,Remark,Rid ");
			strSql.Append(" FROM tblPageDetail ");
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
			strSql.Append(" ModelName,PageName,DetailName,Remark,Rid ");
			strSql.Append(" FROM tblPageDetail ");
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
			strSql.Append("select count(1) FROM tblPageDetail ");
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
			strSql.Append(")AS Row, T.*  from tblPageDetail T ");
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
			parameters[0].Value = "tblPageDetail";
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

