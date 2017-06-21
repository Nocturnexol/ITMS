using System;
using System.Collections.Generic;
using System.Data;
namespace Bitshare.DataDecision.BLL
{
    /// <summary>
    /// OperationOrFunctionUser
    /// </summary>
    public partial class OperationOrFunctionUser
    {
        private readonly Bitshare.DataDecision.DAL.OperationOrFunctionUser dal = new Bitshare.DataDecision.DAL.OperationOrFunctionUser();
        public OperationOrFunctionUser()
        { }
        #region  BasicMethod
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Bitshare.DataDecision.Model.OperationOrFunctionUser GetModel(string UserName, string rf_Type, string OperRational_Name, string rf_Right_Authority)
        {

            return dal.GetModel(UserName, rf_Type, OperRational_Name, rf_Right_Authority);
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
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public List<Bitshare.DataDecision.Model.OperationOrFunctionUser> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public List<Bitshare.DataDecision.Model.OperationOrFunctionUser> DataTableToList(DataTable dt)
        {
            List<Bitshare.DataDecision.Model.OperationOrFunctionUser> modelList = new List<Bitshare.DataDecision.Model.OperationOrFunctionUser>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                Bitshare.DataDecision.Model.OperationOrFunctionUser model;
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
            return dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
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

