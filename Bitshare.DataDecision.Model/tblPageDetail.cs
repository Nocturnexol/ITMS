using System;
using MongoDB.Bson;

namespace Bitshare.DataDecision.Model
{
	/// <summary>
	/// tblPageDetail:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class tblPageDetail
    {
        public ObjectId _id { get; set; }
		public tblPageDetail()
		{}
		#region Model
		private string _modelname;
		private string _pagename;
		private string _detailname;
		private string _remark;
		private int rid;
		/// <summary>
		/// 
		/// </summary>
		public string ModelName
		{
			set{ _modelname=value;}
			get{return _modelname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PageName
		{
			set{ _pagename=value;}
			get{return _pagename;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string DetailName
		{
			set{ _detailname=value;}
			get{return _detailname;}
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
		public int Rid
		{
			set{ rid=value;}
			get{return rid;}
		}
		#endregion Model

	}
}

