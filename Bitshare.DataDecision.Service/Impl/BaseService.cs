using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Bitshare.DataDecision.Service.DTO;
using System.Data;
using Bitshare.DataDecision.Common;
using Bitshare.Common;
namespace Bitshare.DataDecision.Service.Impl
{
    #region 工厂定义
    /// <summary>
    /// 工厂
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseServiceFactory<T> where T : new()
    {
        static BaseService<T> Instance;
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <returns></returns>
        public IBaseService<T> GetInstance()
        {
            if (Instance == null)
            {
                Instance = new BaseService<T>();
            }
            return Instance;
        }
    }
    #endregion
    #region 实现
    /// <summary>
    /// 实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseService<T> : IBaseService<T>
    {
#pragma warning  disable 0693
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pager"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public SubPageResult<T> ListTableByPage<T>(PageInfo pager, Expression<Func<T, bool>> expression) where T : new()
        {
            SubPageResult<T> result = new SubPageResult<T>();
            try
            {
                List<T> list = new List<T>();
                int totalCount = 0;
                if (String.IsNullOrWhiteSpace(pager.Orderby))
                {
                    if (!String.IsNullOrWhiteSpace(pager.Sidx))
                    {
                        pager.Orderby = pager.Sidx + " " + pager.Sord;
                    }

                    else
                    {
                        pager.Orderby = "Rid desc";
                    }
                }
                //获取数据
                DataTable ds = DBContext.DataDecision.QueryPageByProc(typeof(T).Name, pager.Orderby, out totalCount, "*", pager.Where, pager.CurrentPageIndex, pager.PageSize);
                //对数据进行封装
                result.page = pager.CurrentPageIndex;
                result.rows = ds.ToList<T>();
                result.total = (totalCount + pager.PageSize - 1) / pager.PageSize;
                result.records = totalCount;
            }
            catch (Exception ex)
            {
                LogManager.Error("ListPageResult<" + typeof(T).Name + ">", ex);
            }
            return result;
        }
    }
    #endregion
}
