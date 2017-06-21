using System;
using MongoDB.Bson;

namespace Bitshare.DataDecision.Model
{
    public class OperationalAuthority:BaseModel
    {
        public ObjectId _id { get; set; }
		/// <summary>
		/// OperRational_Name
        /// </summary>		
		private string _operrational_name;
        public string OperRational_Name
        {
            get{ return _operrational_name; }
            set{ _operrational_name = value; }
        }        
		/// <summary>
		/// RightsOptions
        /// </summary>		
		private string _rightsoptions;
        public string RightsOptions
        {
            get{ return _rightsoptions; }
            set{ _rightsoptions = value; }
        }

        private bool _options;
        public bool Options {
            get { return _options; }
            set { _options = value; }
        }
      
		/// <summary>
		/// CreateDataTime
        /// </summary>		
		private DateTime _createdatatime;
        public DateTime CreateDataTime
        {
            get{ return _createdatatime; }
            set{ _createdatatime = value; }
        }        
		/// <summary>
		/// CreateDataUser
        /// </summary>		
		private string _createdatauser;
        public string CreateDataUser
        {
            get{ return _createdatauser; }
            set{ _createdatauser = value; }
        }        
		/// <summary>
		/// HandleCompany
        /// </summary>		
		private string _handlecompany;
        public string HandleCompany
        {
            get{ return _handlecompany; }
            set{ _handlecompany = value; }
        }        
		/// <summary>
		/// UpdateTime
        /// </summary>		
		private DateTime _updatetime;
        public DateTime UpdateTime
        {
            get{ return _updatetime; }
            set{ _updatetime = value; }
        }        
		/// <summary>
		/// UpdateUser
        /// </summary>		
		private string _updateuser;
        public string UpdateUser
        {
            get{ return _updateuser; }
            set{ _updateuser = value; }
        }
        /// <summary>
        /// UpdateUser
        /// </summary>		
        private string _Remark;
        public string Remark
        {
            get { return _Remark; }
            set { _Remark = value; }
        }     
	}
}

