using Bitshare.DataDecision.BLL;
using System;
using System.Collections.Generic;

namespace Bitshare.DataDecision.Common
{
    /// <summary>
    /// 业务上下文类
    /// </summary>
    public class BusinessContext
    {



        /// <summary>
        /// OperationOrFunctionUser业务处理类
        /// </summary>
        public static OperationOrFunctionUser OperationOrFunctionUser
        {
            get
            {
                if (_OperationOrFunctionUserg == null)
                {
                    _OperationOrFunctionUserg = new OperationOrFunctionUser();
                }
                return _OperationOrFunctionUserg;
            }
        }
        private static OperationOrFunctionUser _OperationOrFunctionUserg;





        /// <summary>
        /// tbl_Base_Module业务处理类
        /// </summary>
        public static tbl_Base_Module tbl_Base_Module
        {
            get
            {
                if (_tbl_Base_Module == null)
                {
                    _tbl_Base_Module = new tbl_Base_Module();
                }
                return _tbl_Base_Module;
            }
        }
        private static tbl_Base_Module _tbl_Base_Module;




        /// <summary>
        /// FunctionalAuthority业务处理类
        /// </summary>
        public static MongoBll<Model.FunctionalAuthority> FunctionalAuthority
        {
            get { return _functionalAuthority ?? (_functionalAuthority = new MongoBll<Model.FunctionalAuthority>()); }
        }
        private static MongoBll<Model.FunctionalAuthority> _functionalAuthority;


        /// <summary>
        /// sys_role业务处理类
        /// </summary>
        public static MongoBll<Model.sys_role> sys_role
        {
            get { return _sys_role ?? (_sys_role = new MongoBll<Model.sys_role>()); }
        }
        private static MongoBll<Model.sys_role> _sys_role;

        /// <summary>
        /// sys_role_Detail业务处理类
        /// </summary>
        public static sys_role_Detail sys_role_Detail
        {
            get
            {
                if (_sys_role_Detail == null)
                {
                    _sys_role_Detail = new sys_role_Detail();
                }
                return _sys_role_Detail;
            }
        }
        private static sys_role_Detail _sys_role_Detail;

        /// <summary>
        /// sys_role_right业务处理类
        /// </summary>
        public static MongoBll<Model.sys_role_right> sys_role_right
        {
            get { return _sys_role_right ?? (_sys_role_right = new MongoBll<Model.sys_role_right>()); }
        }
        private static MongoBll<Model.sys_role_right> _sys_role_right;


        /// <summary>
        /// tblButtonName业务处理类
        /// </summary>
        public static MongoBll<Model.tblButtonName> tblButtonName
        {
            get { return _tblButtonName ?? (_tblButtonName = new MongoBll<Model.tblButtonName>()); }
        }
        private static MongoBll<Model.tblButtonName> _tblButtonName;


        /// <summary>
        /// tblDetailButton业务处理类
        /// </summary>
        public static tblDetailButton tblDetailButton
        {
            get
            {
                if (_tblDetailButton == null)
                {
                    _tblDetailButton = new tblDetailButton();
                }
                return _tblDetailButton;
            }
        }
        private static tblDetailButton _tblDetailButton;



        /// <summary>
        /// tblGroupButton业务处理类
        /// </summary>
        public static MongoBll<Model.tblGroupButton> tblGroupButton
        {
            get { return _tblGroupButton ?? (_tblGroupButton = new MongoBll<Model.tblGroupButton>()); }
        }
        private static MongoBll<Model.tblGroupButton> _tblGroupButton;



        /// <summary>
        /// tblPageDetail业务处理类
        /// </summary>
        public static tblPageDetail tblPageDetail
        {
            get
            {
                if (_tblPageDetail == null)
                {
                    _tblPageDetail = new tblPageDetail();
                }
                return _tblPageDetail;
            }
        }
        private static tblPageDetail _tblPageDetail;






        /// <summary>
        /// tblUser_Roles业务处理类
        /// </summary>
        public static MongoBll<Model.tblUser_Roles> tblUser_Roles
        {
            get
            {
                if (_tblUser_Roles == null)
                {
                    _tblUser_Roles = new MongoBll<Model.tblUser_Roles>();
                }
                return _tblUser_Roles;
            }
        }
        private static MongoBll<Model.tblUser_Roles> _tblUser_Roles;

        /// <summary>
        /// tblUser_Sys业务处理类
        /// </summary>
        public static MongoBll<Model.tblUser_Sys> tblUser_Sys
        {
            get { return _tblUser_Sys ?? (_tblUser_Sys = new MongoBll<Model.tblUser_Sys>()); }
        }
        private static MongoBll<Model.tblUser_Sys> _tblUser_Sys;


        public static View_GroupButtonInfo View_GroupButtonInfo
        {
            get
            {
                if (_View_GroupButtonInfo == null)
                {
                    _View_GroupButtonInfo = new View_GroupButtonInfo();
                }
                return _View_GroupButtonInfo;
            }
        }
        private static View_GroupButtonInfo _View_GroupButtonInfo;

        public static View_DetailButtonNew View_DetailButtonNew
        {
            get
            {
                if (_View_DetailButtonNew == null)
                {
                    _View_DetailButtonNew = new View_DetailButtonNew();
                }
                return _View_DetailButtonNew;
            }
        }
        private static View_DetailButtonNew _View_DetailButtonNew;



        public static View_User_Roles view_User_Roles
        {
            get
            {
                if (_view_User_Roles == null)
                {
                    _view_User_Roles = new View_User_Roles();
                }
                return _view_User_Roles;
            }
        }
        private static View_User_Roles _view_User_Roles;


        public static tblProcessNotice tblProcessNotice
        {
            get
            {
                if (_tblProcessNotice == null)
                {
                    _tblProcessNotice = new tblProcessNotice();
                }
                return _tblProcessNotice;
            }
        }
        private static tblProcessNotice _tblProcessNotice;

        public static tblProcessManage tblProcessManage
        {
            get
            {
                if (_tblProcessManage == null)
                {
                    _tblProcessManage = new tblProcessManage();
                }
                return _tblProcessManage;
            }
        }
        private static tblProcessManage _tblProcessManage;

        public static tblMessage tblMessage
        {
            get
            {
                if (_tblMessage == null)
                {
                    _tblMessage = new tblMessage();
                }
                return _tblMessage;
            }
        }
        private static tblMessage _tblMessage;

        public static MongoBll<Model.tblDepart> tblDepart
        {
            get { return _tblDepart ?? (_tblDepart = new MongoBll<Model.tblDepart>()); }
        }
        private static MongoBll<Model.tblDepart> _tblDepart;

        /// <summary>
        /// 操作日志业务处理类
        /// </summary>
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





        /// <summary>
        /// 业务权限处理类
        /// </summary>
        public static OperationalAuthority OperationalAuthority
        {
            get
            {
                if (_OperationalAuthority == null)
                {
                    _OperationalAuthority = new OperationalAuthority();
                }
                return _OperationalAuthority;
            }
        }
        private static OperationalAuthority _OperationalAuthority;



        /// <summary>
        /// 
        /// </summary>
        public static tblLevelManange tblLevelManange
        {
            get
            {
                if (_tblLevelManange == null)
                {
                    _tblLevelManange = new tblLevelManange();
                }
                return _tblLevelManange;
            }
        }
        private static tblLevelManange _tblLevelManange;



        /// <summary>
        /// 行车公司业务处理类
        /// </summary>
        public static tblPtCompany tblPtCompany
        {
            get
            {
                if (_tblPtCompany == null)
                {
                    _tblPtCompany = new tblPtCompany();
                }
                return _tblPtCompany;
            }
        }
        private static tblPtCompany _tblPtCompany;




        //存放
        private static Dictionary<Type, object> dic = new Dictionary<Type, object>();

        /// <summary>
        /// 通用方法
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <returns></returns>
        public static T BLLobject<T>()
        {
            T t = default(T);
            if (!dic.ContainsKey(typeof(T)))
            {
                t = Activator.CreateInstance<T>();
                dic.Add(typeof(T), t);
            }
            return t;
        }
    }
}
