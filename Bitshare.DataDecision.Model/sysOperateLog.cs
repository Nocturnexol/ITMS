using System;
using MongoDB.Bson;

namespace Bitshare.DataDecision.Model
{
    //sysOperateLog
    public class sysOperateLog
    {
        public ObjectId _id { get; set; }
        /// <summary>
        /// Rid
        /// </summary>		
        private int rid;
        public int Rid
        {
            get { return rid; }
            set { rid = value; }
        }
        /// <summary>
        /// LoginName
        /// </summary>		
        private string _loginname;
        public string LoginName
        {
            get { return _loginname; }
            set { _loginname = value; }
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
        /// Content
        /// </summary>		
        private string _content;
        public string Content
        {
            get { return _content; }
            set { _content = value; }
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

    }
}

