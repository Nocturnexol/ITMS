using System;
using MongoDB.Bson;

namespace Bitshare.DataDecision.Model
{
	/// <summary>
	/// tblProcessList:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class tblProcessList
    {
        public ObjectId _id { get; set; }
		public tblProcessList()
		{}
		#region Model
		private int rid;
		private string _processid;
		private string _adorderid;
		private string _processname;
		private string _nodename;
		private bool _approval_pass= false;
		private int? _approval_processnum=100;
		private string _sender;
		private DateTime _senddate= DateTime.Now;
		private string _accepter;
		private bool _passflag= false;
		private string _opinion;
		private string _executor;
		private DateTime? _executedate;
		private bool _state= false;
		private bool _deletestate= false;
		/// <summary>
		/// 
		/// </summary>
		public int Rid
		{
			set{ rid=value;}
			get{return rid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ProcessId
		{
			set{ _processid=value;}
			get{return _processid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string AdOrderId
		{
			set{ _adorderid=value;}
			get{return _adorderid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ProcessName
		{
			set{ _processname=value;}
			get{return _processname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string NodeName
		{
			set{ _nodename=value;}
			get{return _nodename;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Approval_Pass
		{
			set{ _approval_pass=value;}
			get{return _approval_pass;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Approval_ProcessNum
		{
			set{ _approval_processnum=value;}
			get{return _approval_processnum;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Sender
		{
			set{ _sender=value;}
			get{return _sender;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime SendDate
		{
			set{ _senddate=value;}
			get{return _senddate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Accepter
		{
			set{ _accepter=value;}
			get{return _accepter;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool PassFlag
		{
			set{ _passflag=value;}
			get{return _passflag;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Opinion
		{
			set{ _opinion=value;}
			get{return _opinion;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Executor
		{
			set{ _executor=value;}
			get{return _executor;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? ExecuteDate
		{
			set{ _executedate=value;}
			get{return _executedate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool State
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool DeleteState
		{
			set{ _deletestate=value;}
			get{return _deletestate;}
		}
		#endregion Model

	}
}

