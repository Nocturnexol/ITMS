using Bitshare.DataDecision.DBUtility;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace Bitshare.DataDecision.DAL
{
    //tblLevelManange
    public partial class tblLevelManange
	{
   		     
		public bool Exists(int Rid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tblLevelManange");
			strSql.Append(" where ");
			                                       strSql.Append(" Rid = @Rid  ");
                            			SqlParameter[] parameters = {
					new SqlParameter("@Rid", SqlDbType.Int,4)
			};
			parameters[0].Value = Rid;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}

        public bool Exists(string username, string leader)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tblLevelManange");
            strSql.Append(" where ");
            strSql.Append(" username = @username  ");
            strSql.Append(" and leader = @leader  ");
            SqlParameter[] parameters = {
					new SqlParameter("@username", SqlDbType.VarChar,50),
                    new SqlParameter("@leader", SqlDbType.VarChar,50)
			};
            parameters[0].Value = username;
            parameters[1].Value = leader;
            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }		
		
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(Bitshare.DataDecision.Model.tblLevelManange model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tblLevelManange(");			
            strSql.Append("UserName,Leader,Remark");
			strSql.Append(") values (");
            strSql.Append("@UserName,@Leader,@Remark");            
            strSql.Append(") ");            
            strSql.Append(";select @@IDENTITY");		
			SqlParameter[] parameters = {
			            new SqlParameter("@UserName", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@Leader", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@Remark", SqlDbType.VarChar,100)             
              
            };
			            
            parameters[0].Value = model.UserName;                        
            parameters[1].Value = model.Leader;                        
            parameters[2].Value = model.Remark;                        
			   
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
		public bool Update(Bitshare.DataDecision.Model.tblLevelManange model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tblLevelManange set ");
			                                                
            strSql.Append(" UserName = @UserName , ");                                    
            strSql.Append(" Leader = @Leader , ");                                    
            strSql.Append(" Remark = @Remark  ");            			
			strSql.Append(" where Rid=@Rid ");
						
SqlParameter[] parameters = {
			            new SqlParameter("@Rid", SqlDbType.Int,4) ,            
                        new SqlParameter("@UserName", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@Leader", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@Remark", SqlDbType.VarChar,100)             
              
            };
						            
            parameters[0].Value = model.Rid;                        
            parameters[1].Value = model.UserName;                        
            parameters[2].Value = model.Leader;                        
            parameters[3].Value = model.Remark;                        
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
			strSql.Append("delete from tblLevelManange ");
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
		/// 批量删除一批数据
		/// </summary>
		public bool DeleteList(string Ridlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tblLevelManange ");
			strSql.Append(" where ID in ("+Ridlist + ")  ");
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
		public Bitshare.DataDecision.Model.tblLevelManange GetModel(int Rid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Rid, UserName, Leader, Remark  ");			
			strSql.Append("  from tblLevelManange ");
			strSql.Append(" where Rid=@Rid");
						SqlParameter[] parameters = {
					new SqlParameter("@Rid", SqlDbType.Int,4)
			};
			parameters[0].Value = Rid;

			
			Bitshare.DataDecision.Model.tblLevelManange model=new Bitshare.DataDecision.Model.tblLevelManange();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			
			if(ds.Tables[0].Rows.Count>0)
			{
												if(ds.Tables[0].Rows[0]["Rid"].ToString()!="")
				{
					model.Rid=int.Parse(ds.Tables[0].Rows[0]["Rid"].ToString());
				}
																																				model.UserName= ds.Tables[0].Rows[0]["UserName"].ToString();
																																model.Leader= ds.Tables[0].Rows[0]["Leader"].ToString();
																																model.Remark= ds.Tables[0].Rows[0]["Remark"].ToString();
																										
				return model;
			}
			else
			{
				return null;
			}
		}
		
		
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select * ");
			strSql.Append(" FROM tblLevelManange ");
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
			strSql.Append(" * ");
			strSql.Append(" FROM tblLevelManange ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

   
	}
}

