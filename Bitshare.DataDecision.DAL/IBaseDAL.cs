using Bitshare.DBUtility;
using System.Collections.Generic;
using System.Data;

namespace Bitshare.DataDecision.DAL
{
    public interface IBaseDAL<T> where T : class
    {
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        bool Exists(int Rid);
        /// <summary>
        /// 增加一条数据
        /// </summary>
        int Add(T model);
        /// <summary>
        /// 更新一条数据
        /// </summary>
        bool Update(T model);
        /// <summary>
        /// 删除数据
        /// </summary>
        bool Delete(int Rid);
        /// <summary>
        /// 按主键批量删除
        /// </summary>
        /// <param name="Ridlist"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool DeleteList(string Ridlist,string userName);
        /// <summary>
        /// 删除数据
        /// </summary>
        bool Delete(int Rid, TransactionInfo trans = null);
        /// <summary>
        /// 带条件的删除
        /// </summary>
        /// <param name="where"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        bool Delete(string where, TransactionInfo trans = null);


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        T GetModel(int Rid);

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        List<T> GetModelList(string strWhere);
        /// <summary>
        /// 获得数据列表
        /// </summary>
        DataSet GetList(string strWhere);
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        DataSet GetList(int Top, string strWhere, string filedOrder);

        /// <summary>
        /// 执行事务操作
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool ExecTransaction(TransactionInfo model);
        #endregion  成员方法
    }
}
