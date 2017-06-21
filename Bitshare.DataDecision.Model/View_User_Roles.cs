using System;
namespace Bitshare.DataDecision.Model
{
	/// <summary>
	/// View_User_Roles:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class View_User_Roles
	{
		public View_User_Roles()
		{}
		#region Model
		private string _username;
		private string _role_name;
		private string _loginname;
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
		public string role_name
		{
			set{ _role_name=value;}
			get{return _role_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LoginName
		{
			set{ _loginname=value;}
			get{return _loginname;}
		}
		#endregion Model

	}
}

