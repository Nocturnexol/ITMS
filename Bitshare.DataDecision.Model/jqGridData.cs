using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bitshare.DataDecision.Model
{
    public class jqGridData
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
