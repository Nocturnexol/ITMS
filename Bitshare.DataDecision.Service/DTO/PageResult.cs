﻿namespace Bitshare.DataDecision.Service.DTO
{
    /// <summary>
    ///分页数据 
    /// </summary>
    public  class PageResult
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
        public object rows { get; set; }


    }
}
