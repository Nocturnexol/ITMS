using System;
using Bitshare.DataDecision.Service.DTO;
using System.Linq.Expressions;
#pragma warning  disable 0693
namespace Bitshare.DataDecision.Service
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseService<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pager"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        SubPageResult<T> ListTableByPage<T>(PageInfo pager, Expression<Func<T, bool>> expression) where T : new();
    }
}
