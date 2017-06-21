using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;

namespace Bitshare.DataDecision.Model
{
    /// <summary>
    /// tblProcessNotice:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class tblProcessNotice
    {
        public ObjectId _id { get; set; }
        public tblProcessNotice()
        { }
        #region Model
        private int rid;
        private string _processname;
        private string _nodename;
        private string _noticerole;
        private string _remark;
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
        public string ProcessName
        {
            set { _processname = value; }
            get { return _processname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string NodeName
        {
            set { _nodename = value; }
            get { return _nodename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string NoticeRole
        {
            set { _noticerole = value; }
            get { return _noticerole; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        #endregion Model

    }
}

