using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bitshare.DataDecision.Model
{
    public class tbl_Base_Module:BaseModel
    {
        /// <summary>
        /// ModuleId
        /// </summary>		
        private int _moduleid;
        public int ModuleId
        {
            get { return _moduleid; }
            set { _moduleid = value; }
        }
        /// <summary>
        /// ParentId
        /// </summary>		
        private int _parentid;
        public int ParentId
        {
            get { return _parentid; }
            set { _parentid = value; }
        }
        /// <summary>
        /// ModuleName
        /// </summary>		
        private string _modulename;
        public string ModuleName
        {
            get { return _modulename; }
            set { _modulename = value; }
        }
        /// <summary>
        /// Icon
        /// </summary>		
        private string _icon;
        public string Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }
        /// <summary>
        /// Url
        /// </summary>		
        private string _url;
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }
        /// <summary>
        /// Remark
        /// </summary>		
        private string _remark;
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        /// <summary>
        /// CreateTime
        /// </summary>		
        private DateTime? _createtime;
        public DateTime? CreateTime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
        /// <summary>
        /// CreateUser
        /// </summary>		
        private string _createuser;
        public string CreateUser
        {
            get { return _createuser; }
            set { _createuser = value; }
        }
        /// <summary>
        /// UpdateTime
        /// </summary>		
        private DateTime? _updatetime;
        public DateTime? UpdateTime
        {
            get { return _updatetime; }
            set { _updatetime = value; }
        }
        /// <summary>
        /// UpdateUser
        /// </summary>		
        private string _updateuser;
        public string UpdateUser
        {
            get { return _updateuser; }
            set { _updateuser = value; }
        }

    }
}
