using System;
using System.Collections.Generic;
using System.Text;
using Bitshare.DataDecision.Model;
using System.Reflection;

namespace Bitshare.DataDecision.Service.Helper
{
    /// <summary>
    /// 操作日志帮助类
    /// </summary>
    public class OperateLogHelper
    {
        private static List<string> ExcludePropertyList = new List<string> { "EntityState", "EntityKey" };

        /// <summary>
        /// 记录添加操作日志
        /// </summary>
        public static void Create<T>(T obj, tblUser_Sys user) where T : new()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0},添加数据：", typeof(T).Name);
            sb.Append(GetPropertyInfo(obj));
            SaveOperateLog(user.LoginName, user.UserName, sb.ToString(), "添加数据");
        }
        /// <summary>
        /// 记录批量删除操作日志
        /// </summary>
        public static void Create<T>(List<T> objList, tblUser_Sys user) where T : new()
        {
            if (objList.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (T obj in objList)
                {

                    sb.AppendFormat("{0},添加数据：", typeof(T).Name);
                    sb.Append(GetPropertyInfo(obj));
                }
               
                SaveOperateLog(user.LoginName, user.UserName, sb.ToString(), "添加数据");
            }
        }


        /// <summary>
        /// 记录编辑操作日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newObj">更新后的对象</param>
        /// <param name="oldObj">更新前的对象</param>
        /// <param name="user">更新前的对象</param>
        public static void Edit<T>(T newObj, T oldObj, tblUser_Sys user) where T : new()
        {
            StringBuilder sb = new StringBuilder();
            Type type = typeof(T);
            sb.AppendFormat("{0},更新数据：", type.Name);
            sb.Append("{");
            // 获得此模型的公共属性
            var plist = new List<PropertyInfo>(type.GetProperties());
            foreach (PropertyInfo p in plist)
            {
                if (ExcludePropertyList.Contains(p.Name))
                    continue;
                var oldVal = p.GetValue(oldObj, null);
                var newVal = p.GetValue(newObj, null);

                if (p.PropertyType == typeof(DateTime))
                {
                    sb.AppendFormat("{0}:{1:yyyy-MM-dd}=>{2:yyyy-MM-dd},", p.Name, Convert.ToDateTime(oldVal), Convert.ToDateTime(newVal));
                }
                else
                {
                    sb.AppendFormat("{0}:{1}=>{2},", p.Name, oldVal, newVal);
                }
            }
            sb.Append("}");

  
            SaveOperateLog(user.LoginName, user.UserName, sb.ToString(), "更新数据");
        }

        public static void Edit<T>(List<T> oldList, List<T> newList, tblUser_Sys user) where T : new()
        {
            List<sysOperateLog> list = new List<sysOperateLog>();
            for (int i = 0; i < oldList.Count; i++)
            {
                list.Add(EditList<T>(newList[i], oldList[i],user));
            }
            SaveOperateLog(list[0]);
        }

        /// <summary>
        /// 记录编辑操作日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newObj">更新后的对象</param>
        /// <param name="oldObj">更新前的对象</param>
        public static sysOperateLog EditList<T>(T newObj, T oldObj, tblUser_Sys user) where T : new()
        {
            StringBuilder sb = new StringBuilder();
            Type type = typeof(T);
            sb.AppendFormat("{0},更新数据：", type.Name);
            sb.Append("{");
            // 获得此模型的公共属性
            var plist = new List<PropertyInfo>(type.GetProperties());
            foreach (PropertyInfo p in plist)
            {
                if (ExcludePropertyList.Contains(p.Name))
                    continue;
                var oldVal = p.GetValue(oldObj, null);
                var newVal = p.GetValue(newObj, null);

                if (p.PropertyType == typeof(DateTime))
                {
                    sb.AppendFormat("{0}:{1:yyyy-MM-dd}=>{2:yyyy-MM-dd},", p.Name, Convert.ToDateTime(oldVal), Convert.ToDateTime(newVal));
                }
                else
                {
                    sb.AppendFormat("{0}:{1}=>{2},", p.Name, oldVal, newVal);
                }
            }
            sb.Append("}");

            
            sysOperateLog model = new sysOperateLog();
            model.LoginName = user.LoginName;
            model.UserName = user.UserName;
            model.Content = sb.ToString();
            model.OperateTime = DateTime.Now;
            model.ChangeMode = "更新数据";
            return model;
        }
        /// <summary>
        /// 记录删除操作日志
        /// </summary>
        public static void Delete<T>(T obj, tblUser_Sys user) where T : new()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0},删除数据：", typeof(T).Name);
            sb.Append(GetPropertyInfo(obj));


            SaveOperateLog(user.LoginName, user.UserName, sb.ToString(), "删除数据");
        }
        /// <summary>
        /// 记录批量删除操作日志
        /// </summary>
        public static void Delete<T>(List<T> objList, tblUser_Sys user) where T : new()
        {
            if (objList.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (T obj in objList)
                {

                    sb.AppendFormat("{0},删除数据：", typeof(T).Name);
                    sb.Append(GetPropertyInfo(obj));
                }
 
                SaveOperateLog(user.LoginName, user.UserName, sb.ToString(), "删除数据");
            }
        }
        /// <summary>
        /// 返回对象的属性信息:{属性名1:值1,属性名2:值2,...}(时间类型精确到:yyyy-MM-dd)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static string GetPropertyInfo<T>(T obj) where T : new()
        {
            StringBuilder sb = new StringBuilder();
            // 获得此模型的类型
            Type type = typeof(T);
            // 获得此模型的公共属性
            var plist = new List<PropertyInfo>(type.GetProperties());
            sb.Append("{");
            foreach (PropertyInfo p in plist)
            {
                if (ExcludePropertyList.Contains(p.Name))
                    continue;
                var val = p.GetValue(obj, null);
                //// 排除为null的数据
                //if (val == null)
                //    continue;
                if (p.PropertyType == typeof(DateTime))
                {
                    sb.AppendFormat("{0}:{1:yyyy-MM-dd},", p.Name, Convert.ToDateTime(val));
                }
                else
                {
                    sb.AppendFormat("{0}:{1},", p.Name, val);
                }
            }
            if (plist.Count > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// 保存操作日志
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="userName">用户名</param>
        /// <param name="content">内容</param>
        public static void SaveOperateLog(string loginName, string userName, string content, string ChangeMode)
        {
            sysOperateLog oper = new sysOperateLog();
            oper.LoginName = loginName;
            oper.UserName = userName;
            oper.Content = content;
            oper.OperateTime = DateTime.Now;
            oper.ChangeMode = ChangeMode;
            SaveOperateLog(oper);
        }

        /// <summary>
        /// 保存操作日志
        /// </summary>
        /// <param name="oper">操作日志对象</param>
        public static void SaveOperateLog(sysOperateLog oper)
        {
            try
            {
                BusinessContext.sysOperateLog.Add(oper);
            }
            catch
            { }
        }
        public static void SaveOperateLog(List<sysOperateLog> oper)
        {
            try
            {
                BusinessContext.sysOperateLog.Add(oper);
            }
            catch
            { }
        }
        
    }

}
