using System;
using MongoDB.Bson;

namespace Bitshare.DataDecision.Model
{
	/// <summary>
	/// tblUser_Sys:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class tblUser_Sys
	{
		public tblUser_Sys()
		{}
		#region Model
		private int rid;
		private string _loginname;
		private string _username;
		private string _englishname;
		private string _usermark;
		private string _dept;
		private string _dept_new;
		private string _userpwd;
		private string _password;
		private string _remark;
		private int? _roleflag;
		/// <summary>
		/// 
		/// </summary>
		public int Rid
		{
			set{ rid=value;}
			get{return rid;}
		}
        public ObjectId _id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string LoginName
		{
			set{ _loginname=value;}
			get{return _loginname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UserName
		{
			set{ _username=value;}
			get{return _username;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string EnglishName
		{
			set{ _englishname=value;}
			get{return _englishname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UserMark
		{
			set{ _usermark=value;}
			get{return _usermark;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string dept
		{
			set{ _dept=value;}
			get{return _dept;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string dept_New
		{
			set{ _dept_new=value;}
			get{return _dept_new;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UserPwd
		{
			set{ _userpwd=value;}
			get{return _userpwd;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PassWord
		{
			set{ _password=value;}
			get{return _password;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? RoleFlag
		{
			set{ _roleflag=value;}
			get{return _roleflag;}
		}
        /// <summary>
        /// 默认角色Id
        /// </summary>
        private int _DefaultRoleId;
        public int DefaultRoleId
        {
            set { _DefaultRoleId = value; }
            get { return _DefaultRoleId; }
        }
        /// <summary>
        /// 失败次数
        /// </summary>
        public int FailTimes { set; get; }
		#endregion Model

	}
    public class UserPassWord
    {
        public string LoginName { get; set; }
        public string OldPassWord { get; set; }
        public string NewPassWord { get; set; }
        public string SureNewPassWord { get; set; }
    }
}

