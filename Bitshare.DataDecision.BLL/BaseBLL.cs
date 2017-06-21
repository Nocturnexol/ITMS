using Bitshare.DataDecision.DAL;
using Bitshare.DBUtility;
using System.Collections.Generic;
using System.Data;

namespace Bitshare.DataDecision.BLL
{
    public class BaseBLL<T, D>
        where T : class
        where D : IBaseDAL<T>, new()
    {
        private D dal = new D();
        #region  成员方法
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
        public int Add(T model)
        {
            return dal.Add(model);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(T model)
        {
            return dal.Update(model);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        public bool Delete(int Rid)
        {
            return dal.Delete(Rid);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        public bool Delete(int Rid, TransactionInfo trans = null)
        {
            return dal.Delete(Rid, trans);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        public bool Delete(string where, TransactionInfo trans = null)
        {
            return dal.Delete(where, trans);
        }
        public bool DeleteList(string Ridlist,string userName)
        {
            return dal.DeleteList(Ridlist, userName);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public T GetModel(int Rid)
        {
            return dal.GetModel(Rid);
        }

        public List<T> GetModelList(string strWhere)
        {
            return dal.GetModelList(strWhere);
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
        /// 执行事务操作
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ExecTransaction(TransactionInfo model)
        {
            return dal.ExecTransaction(model);
        }
        #endregion  成员方法
    }
}
