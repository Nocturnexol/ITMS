using System;
using MongoDB.Bson;

namespace Bitshare.DataDecision.Model
{
	/// <summary>
	/// sys_role:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class sys_role:BaseModel
	{
		public sys_role()
		{}
		#region Model
		
		private string _role_name;
		private string _role_desc;
		private int? _role_flag;
		
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
		public string role_desc
		{
			set{ _role_desc=value;}
			get{return _role_desc;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? role_flag
		{
			set{ _role_flag=value;}
			get{return _role_flag;}
		}
		#endregion Model

	}
}

