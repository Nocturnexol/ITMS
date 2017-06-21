using Bitshare.DataDecision.DBUtility;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Bitshare.DataDecision.DAL
{
    /// <summary>
    /// 数据访问类:View_User_Role
    /// </summary>
    public partial class View_User_Role
    {
        public View_User_Role()
        { }
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Rid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from View_User_Role");
            strSql.Append(" where Rid=@Rid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Rid", SqlDbType.Int,4)			};
            parameters[0].Value = Rid;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Bitshare.DataDecision.Model.View_User_Role model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into View_User_Role(");
            strSql.Append("Rid,LoginName,UserName,EnglishName,UserMark,dept,dept_New,UserPwd,DefaultRoleId,PassWord,Remark,RoleFlag,role_name)");
            strSql.Append(" values (");
            strSql.Append("@Rid,@LoginName,@UserName,@EnglishName,@UserMark,@dept,@dept_New,@UserPwd,@DefaultRoleId,@PassWord,@Remark,@RoleFlag,@role_name)");
            SqlParameter[] parameters = {
					new SqlParameter("@Rid", SqlDbType.Int,4),
					new SqlParameter("@LoginName", SqlDbType.VarChar,50),
					new SqlParameter("@UserName", SqlDbType.VarChar,50),
					new SqlParameter("@EnglishName", SqlDbType.VarChar,50),
					new SqlParameter("@UserMark", SqlDbType.VarChar,50),
					new SqlParameter("@dept", SqlDbType.VarChar,50),
					new SqlParameter("@dept_New", SqlDbType.VarChar,50),
					new SqlParameter("@UserPwd", SqlDbType.VarChar,50),
					new SqlParameter("@DefaultRoleId", SqlDbType.Int,4),
					new SqlParameter("@PassWord", SqlDbType.VarChar,50),
					new SqlParameter("@Remark", SqlDbType.VarChar,100),
					new SqlParameter("@RoleFlag", SqlDbType.Int,4),
					new SqlParameter("@role_name", SqlDbType.VarChar,50)};
            parameters[0].Value = model.Rid;
            parameters[1].Value = model.LoginName;
            parameters[2].Value = model.UserName;
            parameters[3].Value = model.EnglishName;
            parameters[4].Value = model.UserMark;
            parameters[5].Value = model.dept;
            parameters[6].Value = model.dept_New;
            parameters[7].Value = model.UserPwd;
            parameters[8].Value = model.DefaultRoleId;
            parameters[9].Value = model.PassWord;
            parameters[10].Value = model.Remark;
            parameters[11].Value = model.RoleFlag;
            parameters[12].Value = model.role_name;

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
        /// 更新一条数据
        /// </summary>
        public bool Update(Bitshare.DataDecision.Model.View_User_Role model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update View_User_Role set ");
            strSql.Append("Rid=@Rid,");
            strSql.Append("LoginName=@LoginName,");
            strSql.Append("UserName=@UserName,");
            strSql.Append("EnglishName=@EnglishName,");
            strSql.Append("UserMark=@UserMark,");
            strSql.Append("dept=@dept,");
            strSql.Append("dept_New=@dept_New,");
            strSql.Append("UserPwd=@UserPwd,");
            strSql.Append("DefaultRoleId=@DefaultRoleId,");
            strSql.Append("PassWord=@PassWord,");
            strSql.Append("Remark=@Remark,");
            strSql.Append("RoleFlag=@RoleFlag,");
            strSql.Append("role_name=@role_name");
            strSql.Append(" where Rid=@Rid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Rid", SqlDbType.Int,4),
					new SqlParameter("@LoginName", SqlDbType.VarChar,50),
					new SqlParameter("@UserName", SqlDbType.VarChar,50),
					new SqlParameter("@EnglishName", SqlDbType.VarChar,50),
					new SqlParameter("@UserMark", SqlDbType.VarChar,50),
					new SqlParameter("@dept", SqlDbType.VarChar,50),
					new SqlParameter("@dept_New", SqlDbType.VarChar,50),
					new SqlParameter("@UserPwd", SqlDbType.VarChar,50),
					new SqlParameter("@DefaultRoleId", SqlDbType.Int,4),
					new SqlParameter("@PassWord", SqlDbType.VarChar,50),
					new SqlParameter("@Remark", SqlDbType.VarChar,100),
					new SqlParameter("@RoleFlag", SqlDbType.Int,4),
					new SqlParameter("@role_name", SqlDbType.VarChar,50)};
            parameters[0].Value = model.Rid;
            parameters[1].Value = model.LoginName;
            parameters[2].Value = model.UserName;
            parameters[3].Value = model.EnglishName;
            parameters[4].Value = model.UserMark;
            parameters[5].Value = model.dept;
            parameters[6].Value = model.dept_New;
            parameters[7].Value = model.UserPwd;
            parameters[8].Value = model.DefaultRoleId;
            parameters[9].Value = model.PassWord;
            parameters[10].Value = model.Remark;
            parameters[11].Value = model.RoleFlag;
            parameters[12].Value = model.role_name;

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
            strSql.Append("delete from View_User_Role ");
            strSql.Append(" where Rid=@Rid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Rid", SqlDbType.Int,4)			};
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
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string Ridlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from View_User_Role ");
            strSql.Append(" where Rid in (" + Ridlist + ")  ");
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
        public Bitshare.DataDecision.Model.View_User_Role GetModel(int Rid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Rid,LoginName,UserName,EnglishName,UserMark,dept,dept_New,UserPwd,DefaultRoleId,PassWord,Remark,RoleFlag,role_name from View_User_Role ");
            strSql.Append(" where Rid=@Rid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Rid", SqlDbType.Int,4)			};
            parameters[0].Value = Rid;

            Bitshare.DataDecision.Model.View_User_Role model = new Bitshare.DataDecision.Model.View_User_Role();
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
        public Bitshare.DataDecision.Model.View_User_Role DataRowToModel(DataRow row)
        {
            Bitshare.DataDecision.Model.View_User_Role model = new Bitshare.DataDecision.Model.View_User_Role();
            if (row != null)
            {
                if (row["Rid"] != null && row["Rid"].ToString() != "")
                {
                    model.Rid = int.Parse(row["Rid"].ToString());
                }
                if (row["LoginName"] != null)
                {
                    model.LoginName = row["LoginName"].ToString();
                }
                if (row["UserName"] != null)
                {
                    model.UserName = row["UserName"].ToString();
                }
                if (row["EnglishName"] != null)
                {
                    model.EnglishName = row["EnglishName"].ToString();
                }
                if (row["UserMark"] != null)
                {
                    model.UserMark = row["UserMark"].ToString();
                }
                if (row["dept"] != null)
                {
                    model.dept = row["dept"].ToString();
                }
                if (row["dept_New"] != null)
                {
                    model.dept_New = row["dept_New"].ToString();
                }
                if (row["UserPwd"] != null)
                {
                    model.UserPwd = row["UserPwd"].ToString();
                }
                if (row["DefaultRoleId"] != null && row["DefaultRoleId"].ToString() != "")
                {
                    model.DefaultRoleId = int.Parse(row["DefaultRoleId"].ToString());
                }
                if (row["PassWord"] != null)
                {
                    model.PassWord = row["PassWord"].ToString();
                }
                if (row["Remark"] != null)
                {
                    model.Remark = row["Remark"].ToString();
                }
                if (row["RoleFlag"] != null && row["RoleFlag"].ToString() != "")
                {
                    model.RoleFlag = int.Parse(row["RoleFlag"].ToString());
                }
                if (row["role_name"] != null)
                {
                    model.role_name = row["role_name"].ToString();
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
            strSql.Append("select Rid,LoginName,UserName,EnglishName,UserMark,dept,dept_New,UserPwd,DefaultRoleId,PassWord,Remark,RoleFlag,role_name ");
            strSql.Append(" FROM View_User_Role ");
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
            strSql.Append(" Rid,LoginName,UserName,EnglishName,UserMark,dept,dept_New,UserPwd,DefaultRoleId,PassWord,Remark,RoleFlag,role_name ");
            strSql.Append(" FROM View_User_Role ");
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
            strSql.Append("select count(1) FROM View_User_Role ");
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
            strSql.Append(")AS Row, T.*  from View_User_Role T ");
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
            parameters[0].Value = "View_User_Role";
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

