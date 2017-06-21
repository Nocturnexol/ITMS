using System;
using System.Collections.Generic;
using System.Data;
namespace Bitshare.DataDecision.BLL
{
    //tblLevelManange
    public partial class tblLevelManange
	{
   		     
		private readonly Bitshare.DataDecision.DAL.tblLevelManange dal=new Bitshare.DataDecision.DAL.tblLevelManange();
		public tblLevelManange()
		{}
		
		#region  Method
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Rid)
		{
			return dal.Exists(Rid);
		}
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string username, string leader)
        {
            return dal.Exists(username, leader);
        }
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(Bitshare.DataDecision.Model.tblLevelManange model)
		{
						return dal.Add(model);
						
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Bitshare.DataDecision.Model.tblLevelManange model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int Rid)
		{
			
			return dal.Delete(Rid);
		}
				/// <summary>
		/// 批量删除一批数据
		/// </summary>
		public bool DeleteList(string Ridlist )
		{
			return dal.DeleteList(Ridlist );
		}
		
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Bitshare.DataDecision.Model.tblLevelManange GetModel(int Rid)
		{
			
			return dal.GetModel(Rid);
		}



		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<Bitshare.DataDecision.Model.tblLevelManange> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<Bitshare.DataDecision.Model.tblLevelManange> DataTableToList(DataTable dt)
		{
			List<Bitshare.DataDecision.Model.tblLevelManange> modelList = new List<Bitshare.DataDecision.Model.tblLevelManange>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				Bitshare.DataDecision.Model.tblLevelManange model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new Bitshare.DataDecision.Model.tblLevelManange();					
													if(dt.Rows[n]["Rid"].ToString()!="")
				{
					model.Rid=int.Parse(dt.Rows[n]["Rid"].ToString());
				}
																																				model.UserName= dt.Rows[n]["UserName"].ToString();
																																model.Leader= dt.Rows[n]["Leader"].ToString();
																																model.Remark= dt.Rows[n]["Remark"].ToString();
																						
				
					modelList.Add(model);
				}
			}
			return modelList;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}
#endregion
   
	}
}