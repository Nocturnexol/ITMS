using System;
using System.Collections.Generic;
using System.Data;

namespace Bitshare.DataDecision.BLL
{
    //tblProcessManage
    public partial class tblProcessManage
    {

        private readonly Bitshare.DataDecision.DAL.tblProcessManage dal = new Bitshare.DataDecision.DAL.tblProcessManage();
        public tblProcessManage()
        { }

        #region  Method
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
        public int Add(Bitshare.DataDecision.Model.tblProcessManage model)
        {
            return dal.Add(model);

        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Bitshare.DataDecision.Model.tblProcessManage model)
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
        public bool DeleteList(string Ridlist)
        {
            return dal.DeleteList(Ridlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Bitshare.DataDecision.Model.tblProcessManage GetModel(int Rid)
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
        public List<Bitshare.DataDecision.Model.tblProcessManage> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Bitshare.DataDecision.Model.tblProcessManage> DataTableToList(DataTable dt)
        {
            List<Bitshare.DataDecision.Model.tblProcessManage> modelList = new List<Bitshare.DataDecision.Model.tblProcessManage>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                Bitshare.DataDecision.Model.tblProcessManage model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new Bitshare.DataDecision.Model.tblProcessManage();
                    if (dt.Rows[n]["Rid"].ToString() != "")
                    {
                        model.Rid = int.Parse(dt.Rows[n]["Rid"].ToString());
                    }
                    if (dt.Rows[n]["ProcessNum"].ToString() != "")
                    {
                        model.ProcessNum = int.Parse(dt.Rows[n]["ProcessNum"].ToString());
                    }
                    model.ProcessName = dt.Rows[n]["ProcessName"].ToString();
                    if (dt.Rows[n]["NodeNum"].ToString() != "")
                    {
                        model.NodeNum = int.Parse(dt.Rows[n]["NodeNum"].ToString());
                    }
                    model.NodeName = dt.Rows[n]["NodeName"].ToString();
                    model.NodeNameNext = dt.Rows[n]["NodeNameNext"].ToString();
                    model.NodeType = dt.Rows[n]["NodeType"].ToString();
                    model.ExecutorRole = dt.Rows[n]["ExecutorRole"].ToString();
                    model.ReMark = dt.Rows[n]["ReMark"].ToString();


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