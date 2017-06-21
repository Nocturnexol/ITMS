using System;
using MongoDB.Bson;

namespace Bitshare.DataDecision.Model
{
	/// <summary>
	/// tblDetailButton:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class tblDetailButton
	{
        public ObjectId _id { get; set; }
		public tblDetailButton()
		{}
		#region Model
		private int rid;
		private int? _detail_nameid;
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
		public int? Detail_NameId
		{
			set{ _detail_nameid=value;}
			get{return _detail_nameid;}
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

