using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Data;
using System.Web.Script.Serialization;
using System.Collections;
using System.ComponentModel;
using Bitshare.Common;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;

namespace Bitshare.DataDecision.Common
{
    /// <summary>
    /// 扩展方法类
    /// </summary>
    public static class CommExtension
    {
        /// <summary>
        /// 数据类型对应转换方法字典
        /// </summary>
        static Dictionary<Type, Func<object, object>> dic_convert = new Dictionary<Type, Func<object, object>>();

        /// <summary>
        /// 构造函数
        /// </summary>
        static CommExtension()
        {
            Func<object, object> f = null;
            //string
            f = (val) => Convert.ToString(val);
            dic_convert.Add(typeof(string), f);
            //int
            f = (val) => Convert.ToInt32(val);
            dic_convert.Add(typeof(int), f);
            //long
            f = (val) => Convert.ToInt64(val);
            dic_convert.Add(typeof(long), f);
            //double
            f = (val) => Convert.ToDouble(val);
            dic_convert.Add(typeof(double), f);
            //float
            f = (val) => (float)(val);
            dic_convert.Add(typeof(float), f);
            //decimal
            f = (val) => Convert.ToDecimal(val);
            dic_convert.Add(typeof(decimal), f);
            //datetime
            f = (val) => Convert.ToDateTime(val);
            dic_convert.Add(typeof(DateTime), f);

            //datetime?
            f = (val) => Convert.ToDateTime(val);
            dic_convert.Add(typeof(DateTime?), f);

            //bool
            f = (val) => Convert.ToBoolean(val);
            dic_convert.Add(typeof(bool), f);
            //byte
            f = (val) => Convert.ToByte(val);
            dic_convert.Add(typeof(byte), f);
            //char
            f = (val) => Convert.ToChar(val);
            dic_convert.Add(typeof(char), f);
            //null
            f = (val) => (null);
            dic_convert.Add(typeof(DBNull), f);
        }

        /// <summary>
        /// 把类型转换为Hashtable
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static System.Collections.Hashtable ToHashtable<T>(this T obj) where T : new()
        {
            System.Collections.Hashtable hts = new System.Collections.Hashtable();
            if (obj == null)
            {
                return hts;
            }
            // 获得此模型的类型
            Type type = typeof(T);
            // 获得此模型的公共属性
            var plist = new List<PropertyInfo>(type.GetProperties());
            foreach (PropertyInfo p in plist)
            {
                var val = p.GetValue(obj, null);
                // 排除为null的数据
                if (val == null)
                    continue;
                hts.Add(p.Name, val);
            }

            return hts;
        }

        #region 对 DataTable 对象的扩展方法

        /// <summary>
        /// 获取DataTable的列名集合
        /// </summary>
        public static IList<string> GetColumnNames(this DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return new List<string>(0);
            }
            //由于集合中的元素是确定的，所以可以指定元素的个数，系统就不会分配多余的空间，效率会高点
            IList<string> list = new List<string>(dt.Columns.Count);
            foreach (DataColumn dc in dt.Columns)
            {
                list.Add(dc.ColumnName);
            }

            return list;
        }

        /// <summary>
        /// 将 DataTable 序列化成 json 字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToJson(this DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return "[]";
            }
            JavaScriptSerializer myJson = new JavaScriptSerializer();

            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    result.Add(dc.ColumnName, dr[dc].ToString());
                }
                list.Add(result);
            }
            return myJson.Serialize(list);
        }
        /// <summary>
        /// 将 DataTable 序列化成 ArrayList
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static ArrayList ToArrayList(this DataTable dt)
        {
            ArrayList arrayList = new ArrayList();
            if (dt != null)
            {
                foreach (DataRow dataRow in dt.Rows)
                {
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();  //实例化一个参数集合  
                    foreach (DataColumn dataColumn in dt.Columns)
                    {
                        dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName].ToString());
                    }
                    arrayList.Add(dictionary);
                }
            }
            return arrayList;
        }

        /// <summary>
        /// 把DataTable转换成泛型列表
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt) where T : new()
        {
            var list = new List<T>();
            if (dt == null || dt.Rows.Count == 0)
            {
                return list;
            }
            // 获得此模型的公共属性
            var plist = new List<PropertyInfo>(typeof(T).GetProperties());
            // 循环行
            foreach (DataRow row in dt.Rows)
            {
                var t = new T();
                // 循环列
                foreach (DataColumn dc in dt.Columns)
                {
                    var value = row[dc.ColumnName];
                    // 判断值是否有效
                    if (Convert.IsDBNull(value))
                        continue;

                    //var p = info.GetType().GetProperty(dc.ColumnName);
                    var p = plist.FirstOrDefault(c => c.Name.Equals(dc.ColumnName, StringComparison.CurrentCultureIgnoreCase));
                    // 判断此属性是否有Setter
                    if (p == null || !p.CanWrite)
                        continue;
                    var pType = p.PropertyType;
                    if (pType.IsGenericType && pType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    {
                        if (value != null)
                        {
                            NullableConverter nullableConverter = new NullableConverter(pType);
                            pType = nullableConverter.UnderlyingType;
                        }
                    }
                    if (dic_convert.ContainsKey(pType))
                    {
                        var val = dic_convert[pType].Invoke(value);
                        p.SetValue(t, val, null);
                    }
                }
                list.Add(t);
            }
            dt.Dispose();
            dt = null;

            return list;
        }

        public static List<T> ToEntityList<T>(this DataTable dt) where T : new()
        {
            // 定义集合
            List<T> list = new List<T>();
            if (dt == null || dt.Rows.Count == 0)
            {
                return list;
            }
            EmitEntityBuilder<T>.DynamicMethodDelegate<DataRow> handler = EmitEntityBuilder<T>.CreateHandler(dt.Rows[0]);
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(handler(dr));
            }
            return list;
        }




        /// <summary>
        /// 将DataTable实例转换成List<T>
        /// </summary>
        /// <typeparam name="T">Model类型</typeparam>列表
        /// <param name="dt">DataTable对象</param>
        /// <returns></returns>
        public static List<T> ToListSmart<T>(this DataTable dt) where T : new()
        {
            // 定义集合
            List<T> list = new List<T>();
            if (dt == null || dt.Rows.Count == 0)
            {
                return list;
            }
            // 获得此模型的类型
            Type type = typeof(T);
            // 获得此模型的公共属性
            var plist = new List<PropertyInfo>(type.GetProperties());
            // 获取列的个数
            int col_cnt = dt.Columns.Count;
            T t;
            if (plist.Count >= col_cnt)
            {
                #region 循环列给属性赋值方式
                foreach (DataRow row in dt.Rows)
                {
                    t = new T();
                    // 循环列
                    foreach (DataColumn dc in dt.Columns)
                    {
                        // 根据列名查找属性
                        PropertyInfo pi = plist.Find(p => p.Name.Equals(dc.ColumnName, StringComparison.CurrentCultureIgnoreCase));
                        if (pi != null)
                        {
                            // 判断此属性是否有Setter
                            if (!pi.CanWrite)
                                continue;
                            // 判断值是否有效
                            if (Convert.IsDBNull(row[pi.Name]))
                                continue;

                            if (dic_convert.ContainsKey(pi.PropertyType))
                            {
                                var val = dic_convert[pi.PropertyType].Invoke(row[pi.Name]);
                                pi.SetValue(t, val, null);
                            }
                        }
                    }
                    list.Add(t);
                }
                #endregion
            }
            else
            {
                #region 循环属性查找列对应的值
                foreach (DataRow row in dt.Rows)
                {
                    t = System.Activator.CreateInstance<T>();
                    // 循环属性列表
                    foreach (PropertyInfo pi in plist)
                    {
                        // 检查DataTable是否包含此列
                        if (dt.Columns.Contains(pi.Name))
                        {
                            // 判断此属性是否有Setter
                            if (!pi.CanWrite)
                                continue;
                            // 判断值是否有效
                            if (row[pi.Name] == DBNull.Value)
                                continue;

                            if (dic_convert.ContainsKey(pi.PropertyType))
                            {
                                var val = dic_convert[pi.PropertyType].Invoke(row[pi.Name]);
                                pi.SetValue(t, val, null);
                            }
                        }
                    }
                    list.Add(t);
                }
                #endregion
            }
            dt.Dispose();
            dt = null;

            return list;
        }
        #endregion

        #region 对Object的扩展方法
        /// <summary>
        /// 把对象转换成json字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJSON<T>(this T obj) where T : new()
        {
            JavaScriptSerializer myJson = new JavaScriptSerializer();
            string strJson = myJson.Serialize(obj);
            // 处理时间类型

            return strJson;
        }
        #endregion

        public static object ChangeType(this object value, Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value != null)
                {
                    NullableConverter nullableConverter = new NullableConverter(conversionType);
                    conversionType = nullableConverter.UnderlyingType;
                }

                return null;
            }
            return Convert.ChangeType(value, conversionType);
        }

        #region 添加序号 add by sunyi 2015年1月6日17:36:09

        public static void AddXuHao<T>(List<T> list, int pagesize, int curpage) where T : new()
        {
            int StartIndex = curpage == 1 ? 1 : ((curpage - 1) * pagesize + 1);
            foreach (var item in list)
            {
                var XuHao = item.GetType().GetProperty("XuHao");
                if (XuHao == null)
                    return;
                XuHao.SetValue(item, StartIndex++, null);
                //item.XuHao = StartIndex++;
            }
        }
        #endregion

        /// <summary>
        /// 去除空
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string TrimNull(this string val)
        {
            if (String.IsNullOrWhiteSpace(val))
                return "";
            else
                return val.Trim();
        }

        /// <summary>
        /// 获取实体类的属性名称
        /// </summary>
        /// <param name="source">实体类</param>
        /// <returns>属性名称列表</returns>
        public static List<string> GetPropertyNames(this object source)
        {
            if (source == null)
            {
                return new List<string>();
            }
            return GetPropertyNames(source.GetType());
        }
        /// <summary>
        /// 获取类类型的属性名称（按声明顺序）
        /// </summary>
        /// <param name="source">类类型</param>
        /// <returns>属性名称列表</returns>
        public static List<string> GetPropertyNames(this Type source)
        {
            return GetPropertyNames(source, true);
        }
        /// <summary>
        /// 获取类类型的属性名称
        /// </summary>
        /// <param name="source">类类型</param>
        /// <param name="declarationOrder">是否按声明顺序排序</param>
        /// <returns>属性名称列表</returns>
        public static List<string> GetPropertyNames(this Type source, bool declarationOrder)
        {
            if (source == null)
            {
                return new List<string>();
            }
            var list = source.GetProperties().AsQueryable();
            if (declarationOrder)
            {
                list = list.OrderBy(p => p.MetadataToken);
            }
            return list.Select(o => o.Name).ToList(); ;
        }

        /// <summary>
        /// 从源对象赋值到当前对象
        /// </summary>
        /// <param name="destination">当前对象</param>
        /// <param name="source">源对象</param>
        /// <returns>成功复制的值个数</returns>
        public static int CopyValueFrom(this object destination, object source)
        {
            return CopyValueFrom(destination, source, null);
        }

        /// <summary>
        /// 从源对象赋值到当前对象
        /// </summary>
        /// <param name="destination">当前对象</param>
        /// <param name="source">源对象</param>
        /// <param name="excludeName">排除下列名称的属性不要复制</param>
        /// <returns>成功复制的值个数</returns>
        public static int CopyValueFrom(this object destination, object source, IEnumerable<string> excludeName)
        {
            if (destination == null || source == null)
            {
                return 0;
            }
            return Copy(destination, source, source.GetType(), excludeName);
        }

        /// <summary>
        /// 从当前对象赋值到目标对象
        /// </summary>
        /// <param name="source">当前对象</param>
        /// <param name="destination">目标对象</param>
        /// <returns>成功复制的值个数</returns>
        public static int CopyValueTo(this object source, object destination)
        {
            return CopyValueTo(destination, source, null);
        }

        /// <summary>
        /// 从当前对象赋值到目标对象
        /// </summary>
        /// <param name="source">当前对象</param>
        /// <param name="destination">目标对象</param>
        /// <param name="excludeName">排除下列名称的属性不要复制</param>
        /// <returns>成功复制的值个数</returns>
        public static int CopyValueTo(this object source, object destination, IEnumerable<string> excludeName)
        {
            if (destination == null || source == null)
            {
                return 0;
            }
            return Copy(destination, source, source.GetType(), excludeName);
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="destination">目标</param>
        /// <param name="source">来源</param>
        /// <returns>成功复制的值个数</returns>
        public static int Copy(object destination, object source)
        {
            if (destination == null || source == null)
            {
                return 0;
            }
            return Copy(destination, source, source.GetType());
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="destination">目标</param>
        /// <param name="source">来源</param>
        /// <param name="type">复制的属性字段模板</param>
        /// <returns>成功复制的值个数</returns>
        public static int Copy(object destination, object source, Type type)
        {
            return Copy(destination, source, type, null);
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="destination">目标</param>
        /// <param name="source">来源</param>
        /// <param name="type">复制的属性字段模板</param>
        /// <param name="excludeName">排除下列名称的属性不要复制</param>
        /// <returns>成功复制的值个数</returns>
        public static int Copy(object destination, object source, Type type, IEnumerable<string> excludeName)
        {
            if (destination == null || source == null)
            {
                return 0;
            }
            if (excludeName == null)
            {
                excludeName = new List<string>();
            }
            int i = 0;
            Type desType = destination.GetType();
            foreach (FieldInfo mi in type.GetFields())
            {
                if (excludeName.Contains(mi.Name))
                {
                    continue;
                }
                try
                {
                    FieldInfo des = desType.GetField(mi.Name);
                    if (des != null && des.FieldType == mi.FieldType)
                    {
                        des.SetValue(destination, mi.GetValue(source));
                        i++;
                    }

                }
                catch
                {
                }
            }

            foreach (PropertyInfo pi in type.GetProperties())
            {
                if (excludeName.Contains(pi.Name))
                {
                    continue;
                }
                try
                {
                    PropertyInfo des = desType.GetProperty(pi.Name);
                    if (des != null && des.PropertyType == pi.PropertyType && des.CanWrite && pi.CanRead)
                    {
                        des.SetValue(destination, pi.GetValue(source, null), null);
                        i++;
                    }

                }
                catch
                {
                    //throw ex;
                }
            }
            return i;
        }


        public static object Clone(this object old)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                object CloneObject;
                BinaryFormatter bf = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
                bf.Serialize(ms, old);
                ms.Seek(0, SeekOrigin.Begin); // 反序列化至另一个对象(即创建了一个原对象的深表副本)  
                CloneObject = bf.Deserialize(ms);   // 关闭流  
                ms.Close();
                return CloneObject;
            }
        }
        /// <summary>
        /// 将日期转为以5结尾的日期
        /// </summary>
        /// <param name="dtime"></param>
        /// <returns></returns>
        public static DateTime ToFiveDate(this DateTime dtime)
        {
            DateTime stime = new DateTime();
            if (dtime.Day > 25 || dtime.Day <= 5)
            {
                stime = new DateTime(dtime.AddMonths(1).Year, dtime.AddMonths(1).Month, 5);
            }
            else if (dtime.Day > 15)
            {
                stime = new DateTime(dtime.Year, dtime.Month, 25);
            }
            else
            {
                stime = new DateTime(dtime.Year, dtime.Month, 15);
            }
            return stime;
        }
        /// <summary>
        /// 过滤sql值中的不安全字符
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string FilteSQL(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return source;
            }
            source = source.ToLower().Trim();
            source = source.Replace("'", "");
            source = source.Replace(";", "");
            source = source.Replace("\"", "");
            source = source.Replace("&", "&amp");
            source = source.Replace("<", "&lt");
            source = source.Replace(">", "&gt");
            source = source.Replace("delete", "");
            source = source.Replace("update", "");
            source = source.Replace("insert", "");
            source = source.Replace("exec", "");
            source = source.Replace("execute", "");
            return source;
        }

        public static string ToHexString(this byte[] input)
        {
            StringBuilder hexString = new StringBuilder(64);

            for (int i = 0; i < input.Length; i++)
            {
                hexString.Append(String.Format("{0:x2}", input[i]));
            }
            return hexString.ToString();
        }
        public static byte[] ToHexBytes(this string hex)
        {
            if (hex.Length == 0)
            {
                return new byte[] { 0 };
            }

            if (hex.Length % 2 == 1)
            {
                hex = "0" + hex;
            }

            byte[] result = new byte[hex.Length / 2];

            for (int i = 0; i < hex.Length / 2; i++)
            {
                result[i] = byte.Parse(hex.Substring(2 * i, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            }

            return result;
        }


        private static Dictionary<string, Dictionary<string, string>> enumCache;

        private static Dictionary<string, Dictionary<string, string>> EnumCache
        {
            get
            {
                if (enumCache == null)
                {
                    enumCache = new Dictionary<string, Dictionary<string, string>>();
                }
                return enumCache;
            }
            set { enumCache = value; }
        }

        public static string GetEnumText(this System.Enum en)
        {
            string enString = string.Empty;
            if (null == en) return enString;
            var type = en.GetType();
            enString = en.ToString();
            if (!EnumCache.ContainsKey(type.FullName))
            {
                var fields = type.GetFields();
                Dictionary<string, string> temp = new Dictionary<string, string>();
                foreach (var item in fields)
                {
                    var attrs = item.GetCustomAttributes(typeof(TextAttribute), false);
                    if (attrs.Length == 1)
                    {
                        var v = ((TextAttribute)attrs[0]).Value;
                        temp.Add(item.Name, v);
                    }
                }
                EnumCache.Add(type.FullName, temp);
            }
            if (EnumCache[type.FullName].ContainsKey(enString))
            {
                return EnumCache[type.FullName][enString];
            }
            return enString;
        }
    }

    public class TextAttribute : Attribute
    {
        public TextAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
    }
}
