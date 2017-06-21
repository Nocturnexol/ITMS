using System;
using System.Collections.Generic;
using System.Data;
namespace Bitshare.DataDecision.BLL
{
    /// <summary>
    /// View_GroupButtonInfo
    /// </summary>
    public partial class View_GroupButtonInfo
    {
        private readonly Bitshare.DataDecision.DAL.View_GroupButtonInfo dal = new Bitshare.DataDecision.DAL.View_GroupButtonInfo();
        public View_GroupButtonInfo()
        { }
        #region  BasicMethod
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Group_NameId, string ButtonName, string Module_Name, string Group_Name, int ButtonNameId, int Rid, int Module_Id, string Right_Name)
        {
            return dal.Exists(Group_NameId, ButtonName, Module_Name, Group_Name, ButtonNameId, Rid, Module_Id, Right_Name);
        }

  

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Bitshare.DataDecision.Model.View_GroupButtonInfo GetModel(int Group_NameId, string ButtonName, string Module_Name, string Group_Name, int ButtonNameId, int Rid, int Module_Id, string Right_Name)
        {

            return dal.GetModel(Group_NameId, ButtonName, Module_Name, Group_Name, ButtonNameId, Rid, Module_Id, Right_Name);
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
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Bitshare.DataDecision.Model.View_GroupButtonInfo> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Bitshare.DataDecision.Model.View_GroupButtonInfo> DataTableToList(DataTable dt)
        {
            List<Bitshare.DataDecision.Model.View_GroupButtonInfo> modelList = new List<Bitshare.DataDecision.Model.View_GroupButtonInfo>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                Bitshare.DataDecision.Model.View_GroupButtonInfo model;
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
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            return dal.GetRecordCount(strWhere);
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            return dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
        }
        /// <summary>
        /// 分页获取数据列表
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

