using System;
using MongoDB.Bson;

namespace Bitshare.DataDecision.Model
{
	/// <summary>
	/// FunctionalAuthority:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class FunctionalAuthority
	{
		public FunctionalAuthority()
		{}
		#region Model
		private int rid;
		private string _module_name;
		private int _module_id=1;
		private string _group_name;
		private int _group_id=1;
		private string _right_name;
		private decimal _right_id;
		private string _right_url;
		private string _right_tip;
		private string _remark;
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
		public string Module_Name
		{
			set{ _module_name=value;}
			get{return _module_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Module_Id
		{
			set{ _module_id=value;}
			get{return _module_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Group_Name
		{
			set{ _group_name=value;}
			get{return _group_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Group_Id
		{
			set{ _group_id=value;}
			get{return _group_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Right_Name
		{
			set{ _right_name=value;}
			get{return _right_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal Right_Id
		{
			set{ _right_id=value;}
			get{return _right_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Right_Url
		{
			set{ _right_url=value;}
			get{return _right_url;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Rigth_Tip
		{
			set{ _right_tip=value;}
			get{return _right_tip;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		#endregion Model

	}
}

