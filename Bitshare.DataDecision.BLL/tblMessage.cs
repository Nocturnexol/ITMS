using Bitshare.Common;
using System;
using System.Collections.Generic;
using System.Data;

namespace Bitshare.DataDecision.BLL
{

    /// <summary>
    /// tblMessage
    /// </summary>
    public partial class tblMessage
    {
        private readonly Bitshare.DataDecision.DAL.tblMessage dal = new Bitshare.DataDecision.DAL.tblMessage();
        public tblMessage()
        { }
        #region  BasicMethod
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Rid)
        {
            return dal.Exists(Rid);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Bitshare.DataDecision.Model.tblMessage model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Bitshare.DataDecision.Model.tblMessage model)
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
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string Ridlist)
        {
            return dal.DeleteList(PageValidate.SafeLongFilter(Ridlist, 0));
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Bitshare.DataDecision.Model.tblMessage GetModel(int Rid)
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
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Bitshare.DataDecision.Model.tblMessage> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Bitshare.DataDecision.Model.tblMessage> DataTableToList(DataTable dt)
        {
            List<Bitshare.DataDecision.Model.tblMessage> modelList = new List<Bitshare.DataDecision.Model.tblMessage>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                Bitshare.DataDecision.Model.tblMessage model;
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

