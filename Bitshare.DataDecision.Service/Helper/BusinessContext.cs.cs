using Bitshare.DataDecision.BLL;
namespace Bitshare.DataDecision.Service.Helper
{
    public class BusinessContext
    {
        /// <summary>
        /// tblUser_Roles业务处理类
        /// </summary>
        public static tblUser_Roles tblUser_Roles
        {
            get
            {
                if (_tblUser_Roles == null)
                {
                    _tblUser_Roles = new tblUser_Roles();
                }
                return _tblUser_Roles;
            }
        }
        private static tblUser_Roles _tblUser_Roles;
        /// <summary>
        /// sys_role业务处理类
        /// </summary>
        public static sys_role sys_role
        {
            get
            {
                if (_sys_role == null)
                {
                    _sys_role = new sys_role();
                }
                return _sys_role;
            }
        }
        private static sys_role _sys_role;

        /// <summary>
        /// tblUser_Sys业务处理类
        /// </summary>
        public static tblUser_Sys tblUser_Sys
        {
            get
            {
                if (_tblUser_Sys == null)
                {
                    _tblUser_Sys = new tblUser_Sys();
                }
                return _tblUser_Sys;
            }
        }
        private static tblUser_Sys _tblUser_Sys;


        public static sysOperateLog sysOperateLog
        {
            get
            {
                if (_sysOperateLog == null)
                {
                    _sysOperateLog = new sysOperateLog();
                }
                return _sysOperateLog;
            }
        }
        private static sysOperateLog _sysOperateLog;
    }
}
