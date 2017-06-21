using System;
using System.Collections.Generic;
using System.Data;
namespace Bitshare.DataDecision.BLL
{
    /// <summary>
    /// View_User_Roles
    /// </summary>
    public partial class View_User_Roles
	{
		private readonly Bitshare.DataDecision.DAL.View_User_Roles dal=new Bitshare.DataDecision.DAL.View_User_Roles();
		public View_User_Roles()
		{}
		#region  BasicMethod
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(string UserName,string role_name,string LoginName)
		{
			return dal.Exists(UserName,role_name,LoginName);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public bool Add(Bitshare.DataDecision.Model.View_User_Roles model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public bool Update(Bitshare.DataDecision.Model.View_User_Roles model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool Delete(string UserName,string role_name,string LoginName)
		{
			
			return dal.Delete(UserName,role_name,LoginName);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Bitshare.DataDecision.Model.View_User_Roles GetModel(string UserName,string role_name,string LoginName)
		{
			
			return dal.GetModel(UserName,role_name,LoginName);
		}


		/// <summary>
		/// ��������б�
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// ���ǰ��������
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<Bitshare.DataDecision.Model.View_User_Roles> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<Bitshare.DataDecision.Model.View_User_Roles> DataTableToList(DataTable dt)
		{
			List<Bitshare.DataDecision.Model.View_User_Roles> modelList = new List<Bitshare.DataDecision.Model.View_User_Roles>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				Bitshare.DataDecision.Model.View_User_Roles model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = dal.DataRowToModel(dt.Rows[n]);
					if (model != null)
					{
						modelList.Add(model);
					}
				}
			}
			return modelList;
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// ��ҳ��ȡ�����б�
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			return dal.GetRecordCount(strWhere);
		}
		/// <summary>
		/// ��ҳ��ȡ�����б�
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			return dal.GetListByPage( strWhere,  orderby,  startIndex,  endIndex);
		}
		/// <summary>
		/// ��ҳ��ȡ�����б�
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

