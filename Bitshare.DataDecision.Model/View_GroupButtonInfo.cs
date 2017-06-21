using System;
namespace Bitshare.DataDecision.Model
{
    /// <summary>
    /// View_GroupButtonInfo:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class View_GroupButtonInfo
    {
        public View_GroupButtonInfo()
        { }
        #region Model
        private int _group_nameid;
        private string _buttonname;
        private string _module_name;
        private string _group_name;
        private int? _buttonnameid;
        private int rid;
        private int? _module_id;
        private string _right_name;
        /// <summary>
        /// 
        /// </summary>
        public int Group_NameId
        {
            set { _group_nameid = value; }
            get { return _group_nameid; }
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
        public string Module_Name
        {
            set { _module_name = value; }
            get { return _module_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Group_Name
        {
            set { _group_name = value; }
            get { return _group_name; }
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
        /// <summary>
        /// 
        /// </summary>
        public int? Module_Id
        {
            set { _module_id = value; }
            get { return _module_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Right_Name
        {
            set { _right_name = value; }
            get { return _right_name; }
        }
        #endregion Model

    }
}

