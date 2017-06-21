using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Bitshare.DataDecision.BLL
{
    /// <summary>
    /// tblUser_Sys
    /// </summary>
    public partial class tblUser_Sys
    {
        private readonly Bitshare.DataDecision.DAL.tblUser_Sys dal = new Bitshare.DataDecision.DAL.tblUser_Sys();
        public tblUser_Sys()
        { }
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return dal.GetMaxId();
        }

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
        public int Add(Bitshare.DataDecision.Model.tblUser_Sys model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Bitshare.DataDecision.Model.tblUser_Sys model)
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
            return dal.DeleteList(Bitshare.Common.PageValidate.SafeLongFilter(Ridlist, 0));
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Bitshare.DataDecision.Model.tblUser_Sys GetModel(int Rid)
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
        public List<Bitshare.DataDecision.Model.tblUser_Sys> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Bitshare.DataDecision.Model.tblUser_Sys> DataTableToList(DataTable dt)
        {
            List<Bitshare.DataDecision.Model.tblUser_Sys> modelList = new List<Bitshare.DataDecision.Model.tblUser_Sys>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                Bitshare.DataDecision.Model.tblUser_Sys model;
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
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Bitshare.DataDecision.Model.tblUser_Sys GetModel(string strWhere)
        {
            Bitshare.DataDecision.Model.tblUser_Sys obj = null;
            try
            {
                DataSet ds = dal.GetList(strWhere);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<Bitshare.DataDecision.Model.tblUser_Sys> list = DataTableToList(ds.Tables[0]);
                    obj = list.FirstOrDefault();
                }
            }
            catch
            { }
            return obj;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Bitshare.DataDecision.Model.tblUser_Sys GetModelByLoginName(string loginName)
        {

            return dal.GetModelByLoginName(loginName);
        }
        #endregion  ExtensionMethod
    }
}

