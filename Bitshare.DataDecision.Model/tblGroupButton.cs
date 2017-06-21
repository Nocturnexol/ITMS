using System;
using MongoDB.Bson;

namespace Bitshare.DataDecision.Model
{
	/// <summary>
	/// tblGroupButton:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class tblGroupButton
    {
        public ObjectId _id { get; set; }
		public tblGroupButton()
		{}
		#region Model
		private int rid;
		private int _group_nameid;
		private int? _buttonnameid;
		private string _remark;
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
		public int Group_NameId
		{
			set{ _group_nameid=value;}
			get{return _group_nameid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ButtonNameId
		{
			set{ _buttonnameid=value;}
			get{return _buttonnameid;}
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

