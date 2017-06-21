using System.Collections.Generic;

namespace Bitshare.DataDecision.Service.DTO
{
    /// <summary>
    ///分页数据 
    /// </summary>
    public class SubPageResult<T> where T : new()
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 分页数
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 记录总数
        /// </summary>
        public int records { get; set; }
        /// <summary>
        /// 结果集
        /// </summary>
        public List<T> rows { get; set; }
    }
}
