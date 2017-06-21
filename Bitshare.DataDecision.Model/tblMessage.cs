using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;

namespace Bitshare.DataDecision.Model
{
    /// <summary>
    /// tblMessage:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class tblMessage
    {
        public ObjectId _id { get; set; }
        public tblMessage()
        { }
        #region Model
        private int rid;
        private string _sender;
        private string _accepter;
        private string _msgtype;
        private string _msgtitle;
        private string _msgcontent;
        private DateTime _senddate = DateTime.Now;
        private DateTime? _acceptdate;
        private bool _state = false;
        private string _processid;
        private string _adorderid;
        private int? _processserianum;
        private int? _deletestate = 0;
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
        public string Sender
        {
            set { _sender = value; }
            get { return _sender; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Accepter
        {
            set { _accepter = value; }
            get { return _accepter; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MsgType
        {
            set { _msgtype = value; }
            get { return _msgtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MsgTitle
        {
            set { _msgtitle = value; }
            get { return _msgtitle; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MsgContent
        {
            set { _msgcontent = value; }
            get { return _msgcontent; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime SendDate
        {
            set { _senddate = value; }
            get { return _senddate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? AcceptDate
        {
            set { _acceptdate = value; }
            get { return _acceptdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool State
        {
            set { _state = value; }
            get { return _state; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProcessId
        {
            set { _processid = value; }
            get { return _processid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AdOrderId
        {
            set { _adorderid = value; }
            get { return _adorderid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ProcessSeriaNum
        {
            set { _processserianum = value; }
            get { return _processserianum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DeleteState
        {
            set { _deletestate = value; }
            get { return _deletestate; }
        }
        #endregion Model

    }
}

