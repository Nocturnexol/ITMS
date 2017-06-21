using System;
namespace Bitshare.DataDecision.Model
{
    /// <summary>
    /// View_DetailButtonNew:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class View_DetailButtonNew
    {
        public View_DetailButtonNew()
        { }
        #region Model
        private int? _detail_nameid;
        private string _buttonname;
        private string _modelname;
        private string _pagename;
        private string _detailname;
        private int? _buttonnameid;
        private int rid;
        /// <summary>
        /// 
        /// </summary>
        public int? Detail_NameId
        {
            set { _detail_nameid = value; }
            get { return _detail_nameid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ButtonName
        {
            set { _buttonname = value; }
            get { return _buttonname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ModelName
        {
            set { _modelname = value; }
            get { return _modelname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PageName
        {
            set { _pagename = value; }
            get { return _pagename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DetailName
        {
            set { _detailname = value; }
            get { return _detailname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ButtonNameId
        {
            set { _buttonnameid = value; }
            get { return _buttonnameid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Rid
        {
            set { rid = value; }
            get { return rid; }
        }
        #endregion Model

    }
}

