namespace Bitshare.DataDecision.Service.DTO
{
    /// <summary>
    /// th
    /// 2016.1.12
    /// </summary>
    public class PageData
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public int records { set; get; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int pageCount { set; get; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int page { set; get; }
        /// <summary>
        /// 每页记录数
        /// </summary>
        public int total { set; get; }

        /// <summary>
        /// 数据集
        /// </summary>
        public object rows { set; get; }
    }
}
