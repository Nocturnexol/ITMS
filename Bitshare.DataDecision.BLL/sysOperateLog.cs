using System;
using System.Collections.Generic;
using System.Data;
namespace Bitshare.DataDecision.BLL
{
    //sysOperateLog
    public partial class sysOperateLog
    {

        private readonly Bitshare.DataDecision.DAL.sysOperateLog dal = new Bitshare.DataDecision.DAL.sysOperateLog();
        public sysOperateLog()
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
        public int Add(Bitshare.DataDecision.Model.sysOperateLog model)
        {
            return dal.Add(model);

        }
        /// <summary>
        /// 
        /// </summary>
        public bool Add(List<Bitshare.DataDecision.Model.sysOperateLog> modelList)
        {
            return dal.Add(modelList);

        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Bitshare.DataDecision.Model.sysOperateLog model)
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
        public Bitshare.DataDecision.Model.sysOperateLog GetModel(int Rid)
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
        public List<Bitshare.DataDecision.Model.sysOperateLog> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Bitshare.DataDecision.Model.sysOperateLog> DataTableToList(DataTable dt)
        {
            List<Bitshare.DataDecision.Model.sysOperateLog> modelList = new List<Bitshare.DataDecision.Model.sysOperateLog>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                Bitshare.DataDecision.Model.sysOperateLog model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new Bitshare.DataDecision.Model.sysOperateLog();
                    if (dt.Rows[n]["Rid"].ToString() != "")
                    {
                        model.Rid = int.Parse(dt.Rows[n]["Rid"].ToString());
                    }
                    model.LoginName = dt.Rows[n]["LoginName"].ToString();
                    model.UserName = dt.Rows[n]["UserName"].ToString();
                    model.ChangeMode = dt.Rows[n]["ChangeMode"].ToString();
                    model.Content = dt.Rows[n]["Content"].ToString();
                    if (dt.Rows[n]["OperateTime"].ToString() != "")
                    {
                        model.OperateTime = DateTime.Parse(dt.Rows[n]["OperateTime"].ToString());
                    }


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