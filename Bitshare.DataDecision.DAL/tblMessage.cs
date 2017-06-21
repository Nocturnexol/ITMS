using Bitshare.DataDecision.DBUtility;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Bitshare.DataDecision.DAL
{

    /// <summary>
    /// 数据访问类:tblMessage
    /// </summary>
    public partial class tblMessage
    {
        public tblMessage()
        { }
        #region  BasicMethod
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Rid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tblMessage");
            strSql.Append(" where Rid=@Rid");
            SqlParameter[] parameters = {
					new SqlParameter("@Rid", SqlDbType.Int,4)
			};
            parameters[0].Value = Rid;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Bitshare.DataDecision.Model.tblMessage model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tblMessage(");
            strSql.Append("Sender,Accepter,MsgType,MsgTitle,MsgContent,SendDate,AcceptDate,State,ProcessId,AdOrderId,ProcessSeriaNum,DeleteState)");
            strSql.Append(" values (");
            strSql.Append("@Sender,@Accepter,@MsgType,@MsgTitle,@MsgContent,@SendDate,@AcceptDate,@State,@ProcessId,@AdOrderId,@ProcessSeriaNum,@DeleteState)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Sender", SqlDbType.VarChar,50),
					new SqlParameter("@Accepter", SqlDbType.VarChar,50),
					new SqlParameter("@MsgType", SqlDbType.VarChar,50),
					new SqlParameter("@MsgTitle", SqlDbType.VarChar,100),
					new SqlParameter("@MsgContent", SqlDbType.VarChar,8000),
					new SqlParameter("@SendDate", SqlDbType.DateTime),
					new SqlParameter("@AcceptDate", SqlDbType.DateTime),
					new SqlParameter("@State", SqlDbType.Bit,1),
					new SqlParameter("@ProcessId", SqlDbType.VarChar,50),
					new SqlParameter("@AdOrderId", SqlDbType.VarChar,50),
					new SqlParameter("@ProcessSeriaNum", SqlDbType.Int,4),
					new SqlParameter("@DeleteState", SqlDbType.Int,4)};
            parameters[0].Value = model.Sender;
            parameters[1].Value = model.Accepter;
            parameters[2].Value = model.MsgType;
            parameters[3].Value = model.MsgTitle;
            parameters[4].Value = model.MsgContent;
            parameters[5].Value = model.SendDate;
            parameters[6].Value = model.AcceptDate;
            parameters[7].Value = model.State;
            parameters[8].Value = model.ProcessId;
            parameters[9].Value = model.AdOrderId;
            parameters[10].Value = model.ProcessSeriaNum;
            parameters[11].Value = model.DeleteState;

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
        public bool Update(Bitshare.DataDecision.Model.tblMessage model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tblMessage set ");
            strSql.Append("Sender=@Sender,");
            strSql.Append("Accepter=@Accepter,");
            strSql.Append("MsgType=@MsgType,");
            strSql.Append("MsgTitle=@MsgTitle,");
            strSql.Append("MsgContent=@MsgContent,");
            strSql.Append("SendDate=@SendDate,");
            strSql.Append("AcceptDate=@AcceptDate,");
            strSql.Append("State=@State,");
            strSql.Append("ProcessId=@ProcessId,");
            strSql.Append("AdOrderId=@AdOrderId,");
            strSql.Append("ProcessSeriaNum=@ProcessSeriaNum,");
            strSql.Append("DeleteState=@DeleteState");
            strSql.Append(" where Rid=@Rid");
            SqlParameter[] parameters = {
					new SqlParameter("@Sender", SqlDbType.VarChar,50),
					new SqlParameter("@Accepter", SqlDbType.VarChar,50),
					new SqlParameter("@MsgType", SqlDbType.VarChar,50),
					new SqlParameter("@MsgTitle", SqlDbType.VarChar,100),
					new SqlParameter("@MsgContent", SqlDbType.VarChar,8000),
					new SqlParameter("@SendDate", SqlDbType.DateTime),
					new SqlParameter("@AcceptDate", SqlDbType.DateTime),
					new SqlParameter("@State", SqlDbType.Bit,1),
					new SqlParameter("@ProcessId", SqlDbType.VarChar,50),
					new SqlParameter("@AdOrderId", SqlDbType.VarChar,50),
					new SqlParameter("@ProcessSeriaNum", SqlDbType.Int,4),
					new SqlParameter("@DeleteState", SqlDbType.Int,4),
					new SqlParameter("@Rid", SqlDbType.Int,4)};
            parameters[0].Value = model.Sender;
            parameters[1].Value = model.Accepter;
            parameters[2].Value = model.MsgType;
            parameters[3].Value = model.MsgTitle;
            parameters[4].Value = model.MsgContent;
            parameters[5].Value = model.SendDate;
            parameters[6].Value = model.AcceptDate;
            parameters[7].Value = model.State;
            parameters[8].Value = model.ProcessId;
            parameters[9].Value = model.AdOrderId;
            parameters[10].Value = model.ProcessSeriaNum;
            parameters[11].Value = model.DeleteState;
            parameters[12].Value = model.Rid;

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
            strSql.Append("delete from tblMessage ");
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
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string Ridlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tblMessage ");
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
        public Bitshare.DataDecision.Model.tblMessage GetModel(int Rid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Rid,Sender,Accepter,MsgType,MsgTitle,MsgContent,SendDate,AcceptDate,State,ProcessId,AdOrderId,ProcessSeriaNum,DeleteState from tblMessage ");
            strSql.Append(" where Rid=@Rid");
            SqlParameter[] parameters = {
					new SqlParameter("@Rid", SqlDbType.Int,4)
			};
            parameters[0].Value = Rid;

            Bitshare.DataDecision.Model.tblMessage model = new Bitshare.DataDecision.Model.tblMessage();
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
        public Bitshare.DataDecision.Model.tblMessage DataRowToModel(DataRow row)
        {
            Bitshare.DataDecision.Model.tblMessage model = new Bitshare.DataDecision.Model.tblMessage();
            if (row != null)
            {
                if (row["Rid"] != null && row["Rid"].ToString() != "")
                {
                    model.Rid = int.Parse(row["Rid"].ToString());
                }
                if (row["Sender"] != null)
                {
                    model.Sender = row["Sender"].ToString();
                }
                if (row["Accepter"] != null)
                {
                    model.Accepter = row["Accepter"].ToString();
                }
                if (row["MsgType"] != null)
                {
                    model.MsgType = row["MsgType"].ToString();
                }
                if (row["MsgTitle"] != null)
                {
                    model.MsgTitle = row["MsgTitle"].ToString();
                }
                if (row["MsgContent"] != null)
                {
                    model.MsgContent = row["MsgContent"].ToString();
                }
                if (row["SendDate"] != null && row["SendDate"].ToString() != "")
                {
                    model.SendDate = DateTime.Parse(row["SendDate"].ToString());
                }
                if (row["AcceptDate"] != null && row["AcceptDate"].ToString() != "")
                {
                    model.AcceptDate = DateTime.Parse(row["AcceptDate"].ToString());
                }
                if (row["State"] != null && row["State"].ToString() != "")
                {
                    if ((row["State"].ToString() == "1") || (row["State"].ToString().ToLower() == "true"))
                    {
                        model.State = true;
                    }
                    else
                    {
                        model.State = false;
                    }
                }
                if (row["ProcessId"] != null)
                {
                    model.ProcessId = row["ProcessId"].ToString();
                }
                if (row["AdOrderId"] != null)
                {
                    model.AdOrderId = row["AdOrderId"].ToString();
                }
                if (row["ProcessSeriaNum"] != null && row["ProcessSeriaNum"].ToString() != "")
                {
                    model.ProcessSeriaNum = int.Parse(row["ProcessSeriaNum"].ToString());
                }
                if (row["DeleteState"] != null && row["DeleteState"].ToString() != "")
                {
                    model.DeleteState = int.Parse(row["DeleteState"].ToString());
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
            strSql.Append("select Rid,Sender,Accepter,MsgType,MsgTitle,MsgContent,SendDate,AcceptDate,State,ProcessId,AdOrderId,ProcessSeriaNum,DeleteState ");
            strSql.Append(" FROM tblMessage ");
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
            strSql.Append(" Rid,Sender,Accepter,MsgType,MsgTitle,MsgContent,SendDate,AcceptDate,State,ProcessId,AdOrderId,ProcessSeriaNum,DeleteState ");
            strSql.Append(" FROM tblMessage ");
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
            strSql.Append("select count(1) FROM tblMessage ");
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
            strSql.Append(")AS Row, T.*  from tblMessage T ");
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
            parameters[0].Value = "tblMessage";
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

