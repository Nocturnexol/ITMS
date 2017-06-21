using System;
using MongoDB.Bson;

namespace Bitshare.DataDecision.Model
{
	/// <summary>
	/// OperationOrFunctionUser:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class OperationOrFunctionUser
	{
		public OperationOrFunctionUser()
        { } 
        public ObjectId _id { get; set; }
		#region Model
		private string _username;
		private string _rf_type;
		private string _operrational_name;
		private string _rf_right_authority;
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
		public string rf_Type
		{
			set{ _rf_type=value;}
			get{return _rf_type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OperRational_Name
		{
			set{ _operrational_name=value;}
			get{return _operrational_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string rf_Right_Authority
		{
			set{ _rf_right_authority=value;}
			get{return _rf_right_authority;}
		}
		#endregion Model

	}
}

