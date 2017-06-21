using Bitshare.DataDecision.DBUtility;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace Bitshare.DataDecision.DAL
{
    /// <summary>
    /// 数据访问类:View_DetailButtonNew
    /// </summary>
    public partial class View_DetailButtonNew
    {
        public View_DetailButtonNew()
        { }
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Detail_NameId, string ButtonName, string ModelName, string PageName, string DetailName, int ButtonNameId, int Rid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from View_DetailButtonNew");
            strSql.Append(" where Detail_NameId=@Detail_NameId and ButtonName=@ButtonName and ModelName=@ModelName and PageName=@PageName and DetailName=@DetailName and ButtonNameId=@ButtonNameId and Rid=@Rid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Detail_NameId", SqlDbType.Int,4),
					new SqlParameter("@ButtonName", SqlDbType.VarChar,50),
					new SqlParameter("@ModelName", SqlDbType.VarChar,50),
					new SqlParameter("@PageName", SqlDbType.VarChar,50),
					new SqlParameter("@DetailName", SqlDbType.VarChar,50),
					new SqlParameter("@ButtonNameId", SqlDbType.Int,4),
					new SqlParameter("@Rid", SqlDbType.Int,4)			};
            parameters[0].Value = Detail_NameId;
            parameters[1].Value = ButtonName;
            parameters[2].Value = ModelName;
            parameters[3].Value = PageName;
            parameters[4].Value = DetailName;
            parameters[5].Value = ButtonNameId;
            parameters[6].Value = Rid;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


  
  


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Bitshare.DataDecision.Model.View_DetailButtonNew GetModel(int Detail_NameId, string ButtonName, string ModelName, string PageName, string DetailName, int ButtonNameId, int Rid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Detail_NameId,ButtonName,ModelName,PageName,DetailName,ButtonNameId,Rid from View_DetailButtonNew ");
            strSql.Append(" where Detail_NameId=@Detail_NameId and ButtonName=@ButtonName and ModelName=@ModelName and PageName=@PageName and DetailName=@DetailName and ButtonNameId=@ButtonNameId and Rid=@Rid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Detail_NameId", SqlDbType.Int,4),
					new SqlParameter("@ButtonName", SqlDbType.VarChar,50),
					new SqlParameter("@ModelName", SqlDbType.VarChar,50),
					new SqlParameter("@PageName", SqlDbType.VarChar,50),
					new SqlParameter("@DetailName", SqlDbType.VarChar,50),
					new SqlParameter("@ButtonNameId", SqlDbType.Int,4),
					new SqlParameter("@Rid", SqlDbType.Int,4)			};
            parameters[0].Value = Detail_NameId;
            parameters[1].Value = ButtonName;
            parameters[2].Value = ModelName;
            parameters[3].Value = PageName;
            parameters[4].Value = DetailName;
            parameters[5].Value = ButtonNameId;
            parameters[6].Value = Rid;

            Bitshare.DataDecision.Model.View_DetailButtonNew model = new Bitshare.DataDecision.Model.View_DetailButtonNew();
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
        public Bitshare.DataDecision.Model.View_DetailButtonNew DataRowToModel(DataRow row)
        {
            Bitshare.DataDecision.Model.View_DetailButtonNew model = new Bitshare.DataDecision.Model.View_DetailButtonNew();
            if (row != null)
            {
                if (row["Detail_NameId"] != null && row["Detail_NameId"].ToString() != "")
                {
                    model.Detail_NameId = int.Parse(row["Detail_NameId"].ToString());
                }
                if (row["ButtonName"] != null)
                {
                    model.ButtonName = row["ButtonName"].ToString();
                }
                if (row["ModelName"] != null)
                {
                    model.ModelName = row["ModelName"].ToString();
                }
                if (row["PageName"] != null)
                {
                    model.PageName = row["PageName"].ToString();
                }
                if (row["DetailName"] != null)
                {
                    model.DetailName = row["DetailName"].ToString();
                }
                if (row["ButtonNameId"] != null && row["ButtonNameId"].ToString() != "")
                {
                    model.ButtonNameId = int.Parse(row["ButtonNameId"].ToString());
                }
                if (row["Rid"] != null && row["Rid"].ToString() != "")
                {
                    model.Rid = int.Parse(row["Rid"].ToString());
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Detail_NameId,ButtonName,ModelName,PageName,DetailName,ButtonNameId,Rid ");
            strSql.Append(" FROM View_DetailButtonNew ");
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
            strSql.Append(" Detail_NameId,ButtonName,ModelName,PageName,DetailName,ButtonNameId,Rid ");
            strSql.Append(" FROM View_DetailButtonNew ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM View_DetailButtonNew ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.Rid desc");
            }
            strSql.Append(")AS Row, T.*  from View_DetailButtonNew T ");
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

