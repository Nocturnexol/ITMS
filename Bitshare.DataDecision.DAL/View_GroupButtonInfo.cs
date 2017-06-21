using Bitshare.DataDecision.DBUtility;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace Bitshare.DataDecision.DAL
{
    /// <summary>
    /// 数据访问类:View_GroupButtonInfo
    /// </summary>
    public partial class View_GroupButtonInfo
    {
        public View_GroupButtonInfo()
        { }
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Group_NameId, string ButtonName, string Module_Name, string Group_Name, int ButtonNameId, int Rid, int Module_Id, string Right_Name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from View_GroupButtonInfo");
            strSql.Append(" where Group_NameId=@Group_NameId and ButtonName=@ButtonName and Module_Name=@Module_Name and Group_Name=@Group_Name and ButtonNameId=@ButtonNameId and Rid=@Rid and Module_Id=@Module_Id and Right_Name=@Right_Name ");
            SqlParameter[] parameters = {
					new SqlParameter("@Group_NameId", SqlDbType.Int,4),
					new SqlParameter("@ButtonName", SqlDbType.VarChar,50),
					new SqlParameter("@Module_Name", SqlDbType.VarChar,50),
					new SqlParameter("@Group_Name", SqlDbType.VarChar,50),
					new SqlParameter("@ButtonNameId", SqlDbType.Int,4),
					new SqlParameter("@Rid", SqlDbType.Int,4),
					new SqlParameter("@Module_Id", SqlDbType.Int,4),
					new SqlParameter("@Right_Name", SqlDbType.VarChar,50)			};
            parameters[0].Value = Group_NameId;
            parameters[1].Value = ButtonName;
            parameters[2].Value = Module_Name;
            parameters[3].Value = Group_Name;
            parameters[4].Value = ButtonNameId;
            parameters[5].Value = Rid;
            parameters[6].Value = Module_Id;
            parameters[7].Value = Right_Name;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Bitshare.DataDecision.Model.View_GroupButtonInfo GetModel(int Group_NameId, string ButtonName, string Module_Name, string Group_Name, int ButtonNameId, int Rid, int Module_Id, string Right_Name)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Group_NameId,ButtonName,Module_Name,Group_Name,ButtonNameId,Rid,Module_Id,Right_Name from View_GroupButtonInfo ");
            strSql.Append(" where Group_NameId=@Group_NameId and ButtonName=@ButtonName and Module_Name=@Module_Name and Group_Name=@Group_Name and ButtonNameId=@ButtonNameId and Rid=@Rid and Module_Id=@Module_Id and Right_Name=@Right_Name ");
            SqlParameter[] parameters = {
					new SqlParameter("@Group_NameId", SqlDbType.Int,4),
					new SqlParameter("@ButtonName", SqlDbType.VarChar,50),
					new SqlParameter("@Module_Name", SqlDbType.VarChar,50),
					new SqlParameter("@Group_Name", SqlDbType.VarChar,50),
					new SqlParameter("@ButtonNameId", SqlDbType.Int,4),
					new SqlParameter("@Rid", SqlDbType.Int,4),
					new SqlParameter("@Module_Id", SqlDbType.Int,4),
					new SqlParameter("@Right_Name", SqlDbType.VarChar,50)			};
            parameters[0].Value = Group_NameId;
            parameters[1].Value = ButtonName;
            parameters[2].Value = Module_Name;
            parameters[3].Value = Group_Name;
            parameters[4].Value = ButtonNameId;
            parameters[5].Value = Rid;
            parameters[6].Value = Module_Id;
            parameters[7].Value = Right_Name;

            Bitshare.DataDecision.Model.View_GroupButtonInfo model = new Bitshare.DataDecision.Model.View_GroupButtonInfo();
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
        public Bitshare.DataDecision.Model.View_GroupButtonInfo DataRowToModel(DataRow row)
        {
            Bitshare.DataDecision.Model.View_GroupButtonInfo model = new Bitshare.DataDecision.Model.View_GroupButtonInfo();
            if (row != null)
            {
                if (row["Group_NameId"] != null && row["Group_NameId"].ToString() != "")
                {
                    model.Group_NameId = int.Parse(row["Group_NameId"].ToString());
                }
                if (row["ButtonName"] != null)
                {
                    model.ButtonName = row["ButtonName"].ToString();
                }
                if (row["Module_Name"] != null)
                {
                    model.Module_Name = row["Module_Name"].ToString();
                }
                if (row["Group_Name"] != null)
                {
                    model.Group_Name = row["Group_Name"].ToString();
                }
                if (row["ButtonNameId"] != null && row["ButtonNameId"].ToString() != "")
                {
                    model.ButtonNameId = int.Parse(row["ButtonNameId"].ToString());
                }
                if (row["Rid"] != null && row["Rid"].ToString() != "")
                {
                    model.Rid = int.Parse(row["Rid"].ToString());
                }
                if (row["Module_Id"] != null && row["Module_Id"].ToString() != "")
                {
                    model.Module_Id = int.Parse(row["Module_Id"].ToString());
                }
                if (row["Right_Name"] != null)
                {
                    model.Right_Name = row["Right_Name"].ToString();
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
            strSql.Append("select Group_NameId,ButtonName,Module_Name,Group_Name,ButtonNameId,Rid,Module_Id,Right_Name ");
            strSql.Append(" FROM View_GroupButtonInfo ");
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
            strSql.Append(" Group_NameId,ButtonName,Module_Name,Group_Name,ButtonNameId,Rid,Module_Id,Right_Name ");
            strSql.Append(" FROM View_GroupButtonInfo ");
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
            strSql.Append("select count(1) FROM View_GroupButtonInfo ");
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
                strSql.Append("order by T.Right_Name desc");
            }
            strSql.Append(")AS Row, T.*  from View_GroupButtonInfo T ");
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
            parameters[0].Value = "View_GroupButtonInfo";
            parameters[1].Value = "Right_Name";
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


