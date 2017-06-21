using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bitshare.DataDecision.Model
{
    /// <summary>
    /// View_User_Role:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class View_User_Role
    {
        public View_User_Role()
        { }
        #region Model
        private int rid;
        private string _loginname;
        private string _username;
        private string _englishname;
        private string _usermark;
        private string _dept;
        private string _dept_new;
        private string _userpwd;
        private int? _defaultroleid;
        private string _password;
        private string _remark;
        private int? _roleflag;
        private string _role_name;
        /// <summary>
        /// 
        /// </summary>
        public int Rid
        {
            set { rid = value; }
            get { return rid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LoginName
        {
            set { _loginname = value; }
            get { return _loginname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EnglishName
        {
            set { _englishname = value; }
            get { return _englishname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserMark
        {
            set { _usermark = value; }
            get { return _usermark; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string dept
        {
            set { _dept = value; }
            get { return _dept; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string dept_New
        {
            set { _dept_new = value; }
            get { return _dept_new; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserPwd
        {
            set { _userpwd = value; }
            get { return _userpwd; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DefaultRoleId
        {
            set { _defaultroleid = value; }
            get { return _defaultroleid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PassWord
        {
            set { _password = value; }
            get { return _password; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? RoleFlag
        {
            set { _roleflag = value; }
            get { return _roleflag; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string role_name
        {
            set { _role_name = value; }
            get { return _role_name; }
        }
        #endregion Model

    }
}

