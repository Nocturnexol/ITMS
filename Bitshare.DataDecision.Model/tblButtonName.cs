using System;
namespace Bitshare.DataDecision.Model
{
	/// <summary>
	/// tblButtonName:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class tblButtonName:BaseModel
	{
		public tblButtonName()
		{}
		#region Model
		
		private string _buttonname;
		private string _remark;
		
		/// <summary>
		/// 
		/// </summary>
		public string ButtonName
		{
			set{ _buttonname=value;}
			get{return _buttonname;}
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

