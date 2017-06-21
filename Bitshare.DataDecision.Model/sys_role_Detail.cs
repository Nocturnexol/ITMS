using System;
using MongoDB.Bson;

namespace Bitshare.DataDecision.Model
{
	/// <summary>
	/// sys_role_Detail:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class sys_role_Detail
	{
		public sys_role_Detail()
		{}
        public ObjectId _id { get; set; }
		#region Model
		private int _rid;
		private string _rf_type;
		private int? _rf_role_id;
		private int? _rf_right_code;
		/// <summary>
		/// 
		/// </summary>
		public int Rid
		{
			set{ _rid=value;}
			get{return _rid;}
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
		public int? rf_Role_Id
		{
			set{ _rf_role_id=value;}
			get{return _rf_role_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? rf_Right_Code
		{
			set{ _rf_right_code=value;}
			get{return _rf_right_code;}
		}
		#endregion Model

	}
}

