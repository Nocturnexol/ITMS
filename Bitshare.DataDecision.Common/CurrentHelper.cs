using Bitshare.Common;
using Bitshare.DataDecision.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Security;
using MongoDB.Driver.Builders;

namespace Bitshare.DataDecision.Common
{
    /// <summary>
    /// 当前登录信息帮助类
    /// </summary>
    public class CurrentHelper
    {
        #region 私有属性
        /// <summary>
        /// 记录用户帐号密码的cookie名称
        /// </summary>
        /// 

        public static string ProjectKey = UseTools.GetProjectKey();
        public static string LOGIN_INFO_COOKIE = ProjectKey + "userlogininfo";
        /// <summary>
        /// 记录帐号登陆名的cookie名称
        /// </summary>
        public static string LOGIN_NAME_COOKIE = "login";
        /// <summary>
        /// 记录帐号登陆密码的cookie名称
        /// </summary>
        public static string LOGIN_PWD_COOKIE = "pwd";
        #endregion

        private static string m_WebSiteUrl = string.Empty;


     

        /// <summary>
        /// 当前登陆的用户
        /// </summary>
        public static UserModel CurrentUser
        {
            get
            {
                //用于记录当前用户帐号信息
                UserModel m_CurrentUser = null;

                if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session["User"] != null)
                {
                    m_CurrentUser = (UserModel)System.Web.HttpContext.Current.Session["User"];
                }
                else if (System.Web.HttpContext.Current.Request.Cookies[CurrentHelper.LOGIN_INFO_COOKIE] != null)
                {
                    string loginName = System.Web.HttpContext.Current.Request.Cookies[CurrentHelper.LOGIN_INFO_COOKIE].Values[CurrentHelper.LOGIN_NAME_COOKIE];
                    string pwd = System.Web.HttpContext.Current.Request.Cookies[CurrentHelper.LOGIN_INFO_COOKIE].Values[CurrentHelper.LOGIN_PWD_COOKIE];

                    if (!string.IsNullOrEmpty(loginName))
                    {
                        tblUser_Sys user =
                            BusinessContext.tblUser_Sys.Get(Query<tblUser_Sys>.EQ(t => t.LoginName, loginName));

                        //根据cookie当中的记录,如果验证通过则直接登陆成功
                        if (user != null && pwd == user.UserPwd)
                        {
                            FormsAuthentication.SetAuthCookie(user.LoginName, false);

                            m_CurrentUser = new UserModel();
                            m_CurrentUser.User = user;
                            m_CurrentUser.Roles = new List<sys_role>();
                            List<tblUser_Roles> roleList = BusinessContext.tblUser_Roles.GetList(Query<tblUser_Roles>.EQ(t => t.LoginName, loginName));
                            List<int> roleIds = roleList.Select(p => p.Role_Id).ToList();
                            if (roleIds != null && roleIds.Count > 0)
                            {
                                //strWhere = string.Format("rid in {0}", DBContext.AssemblyInCondition(roleIds));
                                m_CurrentUser.Roles =
                                    BusinessContext.sys_role.GetList(Query<sys_role>.In(t => t.Rid, roleIds));
                            }
                            System.Web.HttpContext.Current.Session["User"] = m_CurrentUser;
                        }
                    }
                }
                else if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Request.IsAuthenticated && !string.IsNullOrEmpty(System.Web.HttpContext.Current.User.Identity.Name))
                {
                    m_CurrentUser = new UserModel();
                    //string strWhere = "LoginName='" + System.Web.HttpContext.Current.User.Identity.Name + "'";
                    m_CurrentUser.User =
                        BusinessContext.tblUser_Sys.Get(Query<tblUser_Sys>.EQ(t => t.LoginName,
                            System.Web.HttpContext.Current.User.Identity.Name));
                    m_CurrentUser.Roles = new List<sys_role>();
                    List<tblUser_Roles> roleList =
                        BusinessContext.tblUser_Roles.GetList(Query<tblUser_Roles>.EQ(t => t.LoginName,
                            System.Web.HttpContext.Current.User.Identity.Name));
                    List<int> roleIds = roleList.Select(p => p.Role_Id).ToList();
                    if (roleIds != null && roleIds.Count > 0)
                    {
                        //strWhere = string.Format("rid in {0}", DBContext.AssemblyInCondition(roleIds));
                        m_CurrentUser.Roles = BusinessContext.sys_role.GetList(Query<sys_role>.In(t => t.Rid, roleIds));
                    }
                    System.Web.HttpContext.Current.Session["User"] = m_CurrentUser;
                }

                return m_CurrentUser;
            }
            set
            {
                if (System.Web.HttpContext.Current != null)
                {
                    System.Web.HttpContext.Current.Session["User"] = value;
                }
            }
        }

        /// <summary>
        /// 是否需要出现验证码
        /// </summary>
        public static bool IsNeedCheckCode
        {
            get
            {
                bool m_IsNeedCheckCode = false;

                if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["IsNeedCheckCode"] != null)
                {
                    m_IsNeedCheckCode = (bool)System.Web.HttpContext.Current.Session["IsNeedCheckCode"];
                }
                return m_IsNeedCheckCode;
            }
            set
            {

                if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null)
                {
                    System.Web.HttpContext.Current.Session["IsNeedCheckCode"] = value;
                }
            }
        }

        /// <summary>
        /// 出现验证码的页面
        /// </summary>
        public static string CurrentCheckCodeUrl
        {
            get
            {
                string m_CurrentCheckCodeUrl = "";
                if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["CurrentCheckCodeUrl"] != null)
                {
                    m_CurrentCheckCodeUrl = System.Web.HttpContext.Current.Session["CurrentCheckCodeUrl"] as string;
                }
                return m_CurrentCheckCodeUrl;
            }
            set
            {
                if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null)
                {
                    System.Web.HttpContext.Current.Session["CurrentCheckCodeUrl"] = value;
                }
            }
        }

        /// <summary>
        /// 当前website的请求根地址
        /// </summary>
        public static string WebSiteUrl
        {
            get
            {
                if (string.IsNullOrEmpty(m_WebSiteUrl))
                {
                    m_WebSiteUrl = "http://" + System.Web.HttpContext.Current.Request.Url.Authority;
                }
                return m_WebSiteUrl;
            }
            set
            {
                m_WebSiteUrl = value;
            }
        }

        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            string IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrEmpty(IP))
            {
                IP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }

            return IP;
        }

        /// <summary>
        /// 根据登录名获取用户信息
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <returns></returns>
        public static tblUser_Sys GetUserByLoginName(string loginName)
        {

            tblUser_Sys user = CacheManager.Get("tblUser_Sys-" + loginName) as tblUser_Sys;
            if (user == null)
            {
                string SQL = "select * from tblUser_Sys where LoginName='" + loginName + "'";
                DataTable DT = DBContext.DataDecision.GetDataTable(SQL);
                List<tblUser_Sys> UserList = DT.ToList<tblUser_Sys>();
                if (UserList != null && UserList.Count > 0)
                {
                    user = UserList[0];
                }
                CacheManager.Insert("tblUser_Sys-" + loginName, user);
            }
            return user ?? new tblUser_Sys();
        }
    }

}
