namespace Bitshare.DataDecision.Model
{
    //tblLevelManange
    public class tblLevelManange:BaseModel
	{
   		     
      		
		
		/// <summary>
		/// UserName
        /// </summary>		
		private string _username;
        public string UserName
        {
            get{ return _username; }
            set{ _username = value; }
        }        
		/// <summary>
		/// Leader
        /// </summary>		
		private string _leader;
        public string Leader
        {
            get{ return _leader; }
            set{ _leader = value; }
        }        
		/// <summary>
		/// Remark
        /// </summary>		
		private string _remark;
        public string Remark
        {
            get{ return _remark; }
            set{ _remark = value; }
        }        
		   
	}
}

