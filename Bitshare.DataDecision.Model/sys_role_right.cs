using System;
using MongoDB.Bson;

namespace Bitshare.DataDecision.Model
{
	/// <summary>
	/// sys_role_right:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class sys_role_right
	{
		public sys_role_right()
		{}
        public ObjectId _id { get; set; }
		#region Model
		private int rid;
		private string _rf_type;
		private int _rf_role_id;
		private int _rf_right_code;
		private string _rf_right_authority;
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
		public string rf_Type
		{
			set{ _rf_type=value;}
			get{return _rf_type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int rf_Role_Id
		{
			set{ _rf_role_id=value;}
			get{return _rf_role_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int rf_Right_Code
		{
			set{ _rf_right_code=value;}
			get{return _rf_right_code;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string rf_Right_Authority
		{
			set{ _rf_right_authority=value;}
			get{return _rf_right_authority;}
		}

        public override string ToString()
        {
            return string.Format("{0}|{1}|{2}|{3}",this._rf_role_id,this._rf_type,this._rf_right_code,this._rf_right_authority);
        }
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
        public override bool Equals(object obj)
        {
                return this.GetHashCode() == obj.GetHashCode();
        }

		#endregion Model

	}
}

