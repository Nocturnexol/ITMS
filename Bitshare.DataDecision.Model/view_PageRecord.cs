using System;
using System.Collections.Generic;


namespace Bitshare.DataDecision.Model
{
    /// <summary>
    /// view_PageRecord:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>

    public class view_PageRecord
    {
        #region  Model
        private int _tblrecid;
        private string _pagetitle;
        private string _pageurl;
        private int _viewcount;

        public int Rid
        {
            set { _tblrecid = value; }
            get { return _tblrecid; }
        }
        /// <summary>
        /// UserName
        /// </summary>		
        private string _username;
        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }
        /// <summary>
        /// ChangeMode
        /// </summary>		
        private string _changemode;
        public string ChangeMode
        {
            get { return _changemode; }
            set { _changemode = value; }
        }

        /// <summary>
        /// OperateTime
        /// </summary>		
        private DateTime _operatetime;
        public DateTime OperateTime
        {
            get { return _operatetime; }
            set { _operatetime = value; }
        }
        /// <summary>
        /// RecordDate
        /// </summary>
        private DateTime _recoredate;
        public DateTime RecordDate
        {
            get { return _recoredate; }
            set { _recoredate = value; }
        }
        /// <summary>
        /// Content
        /// </summary>		
        private string _content;
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }
        /// <summary>
        /// PageTitle
        /// </summary>
        public string PageTitle
        {
            set { _pagetitle = value; }
            get { return _pagetitle; }
        }
        /// <summary>
        /// PageUrl
        /// </summary>
        public string PageUrl
        {
            set { _pageurl = value; }
            get { return _pageurl; }
        }


        /// <summary>
        /// CountSum
        /// </summary>
        /// 
        private int _countsum;
        public int CountSum
        {
            set { _countsum = value; }
            get { return _countsum; }
        }


        /// <summary>
        /// ViewCount
        /// </summary>
        public int ViewCount
        {
            set { _viewcount = value; }
            get { return _viewcount; }
        }


        #endregion Model

    }


}
