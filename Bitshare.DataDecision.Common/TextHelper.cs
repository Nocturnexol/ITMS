using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Bitshare.DataDecision.Common
{
    /// <summary>
    /// 文本处理类
    /// </summary>
    public class TextHelper
    {
        #region 对指定的字符串,进行md5加密散列值计算
        /// <summary>
        /// 对指定的字符串,进行md5加密散列值计算
        /// </summary>
        /// <param name="source">需要md5加密的字符串值</param>
        /// <returns>md5值</returns>
        public static string MD5(string source)
        {
            byte[] result = Encoding.Default.GetBytes(source);
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            return ByteArrayToHexString(md5.ComputeHash(result));
        }
        #endregion

        #region 把对应的字节值转换为对应的字符串
        /// <summary>
        /// 把对应的字节值转换为对应的字符串
        /// </summary>
        /// <param name="values">字节值</param>
        /// <returns>字符串</returns>
        private static string ByteArrayToHexString(byte[] values)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte value in values)
            {
                sb.AppendFormat("{0:X2}", value);
            }
            return sb.ToString();
        }
        #endregion

        #region 字符串转成 column in ('','','','',)的条件字符串
        /// <summary>
        ///  字符串转成 column in ('','','','',)的条件字符串
        /// </summary>
        /// <param name="strings">字符串</param>
        /// <param name="split">拆分字符</param>
        /// <returns></returns>
        public static string ConvertStringArrayToWhere(string strings, string split)
        {
            string[] array = strings.Split(split.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            return ConvertStringArrayToWhere(array);
        }

        /// <summary>
        /// 组合数组为('','','','',)的条件字符串
        /// </summary>
        /// <param name="array">数组</param>
        /// <returns>query的条件</returns>
        public static string ConvertStringArrayToWhere(string[] array)
        {
            //组合guid为query的条件
            StringBuilder where = new StringBuilder();
            where.Append("('");
            where.Append(string.Join("','", array));
            where.Append("')");

            return where.ToString();
        }

        /// <summary>
        /// 组合数组为('','','','',)的条件字符串
        /// </summary>
        /// <param name="array">数组</param>
        /// <returns>query的条件</returns>
        public static string ConvertStringArrayToWhere(IEnumerable<string> array)
        {
            return ConvertStringArrayToWhere(array.ToArray());
        }

        /// <summary>
        /// 组合数组为('','','','',)的条件字符串
        /// </summary>
        /// <param name="array">数组</param>
        /// <returns>query的条件</returns>
        public static string ConvertIntArrayToWhere(IEnumerable<int> array)
        {
            //组合guid为query的条件
            StringBuilder where = new StringBuilder();
            where.Append("('");
            where.Append(string.Join("','", array));
            where.Append("')");

            return where.ToString();
        }
        /// <summary>
        /// 把字符串转换为ModID的条件字符串
        /// </summary>
        /// <param name="strings">品牌ID逗号分割字符串</param>
        /// <param name="split">分隔符</param>
        /// <param name="Mod">余数</param>
        /// <returns>把字符串转换为ModID的条件字符串</returns>
        public static string ConvertIntArrayToModIDWhere(string strings, string split, int Mod = 50)
        {
            string[] array = strings.Split(split.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            return ConvertIntArrayToModIDWhere(array, Mod);
        }

        /// <summary>
        /// 组合数组为('','','','',)的条件字符串
        /// </summary>
        /// <param name="array">数组</param>
        /// <param name="Mod">余数</param>
        /// <returns>query的条件</returns>
        public static string ConvertIntArrayToModIDWhere(IEnumerable<int> array, int Mod = 50)
        {
            //组合guid为query的条件
            List<int> temp = array.ToList();

            for (int i = 0; i < temp.Count; i++)
            {
                temp[i] = temp[i] % Mod;
            }

            StringBuilder where = new StringBuilder();
            where.Append("('");
            where.Append(string.Join("','", temp));
            where.Append("')");

            return where.ToString();
        }

        /// <summary>
        /// 组合数组为('','','','',)的条件字符串
        /// </summary>
        /// <param name="array">数组</param>
        /// <param name="Mod">余数</param>
        /// <returns>query的条件</returns>
        public static string ConvertIntArrayToModIDWhere(IEnumerable<string> array, int Mod = 50)
        {
            //组合guid为query的条件
            List<int> temp = array.Select(p => Convert.ToInt32(p)).ToList();

            for (int i = 0; i < temp.Count; i++)
            {
                temp[i] = temp[i] % Mod;
            }

            StringBuilder where = new StringBuilder();
            where.Append("('");
            where.Append(string.Join("','", temp.Distinct()));
            where.Append("')");

            return where.ToString();
        }
        #endregion

        #region 转化为PinYin匹配的条件
        /// <summary>
        /// 查询关键字
        /// </summary>
        /// <param name="searchTxt"></param>
        /// <returns></returns>
        public static string ConvertStringToPinYinWhereString(string searchTxt)
        {
            StringBuilder where = new StringBuilder();

            if (!string.IsNullOrEmpty(searchTxt))
            {
                char[] chararray = searchTxt.ToCharArray();

                where.Append("%");
                where.Append(string.Join("%", chararray));
                where.Append("%");
            }

            return where.ToString();
        }
        #endregion

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="encode">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string EncodeBase64(Encoding encode, string source)
        {
            byte[] bytes = encode.GetBytes(source);
            string encodeString = "";
            try
            {
                encodeString = Convert.ToBase64String(bytes);
            }
            catch
            {
                encodeString = source;
            }
            return encodeString;
        }

        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string EncodeBase64(string source)
        {
            return EncodeBase64(Encoding.UTF8, source);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="encode">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(Encoding encode, string result)
        {
            string decodeString = "";
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decodeString = encode.GetString(bytes);
            }
            catch
            {
                decodeString = result;
            }
            return decodeString;
        }

        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(string result)
        {
            return DecodeBase64(Encoding.UTF8, result);
        }

        /// <summary>
        /// 判断字符串是否是中文
        /// </summary>
        /// <param name="chars">字符串</param>
        /// <param name="RegType">true:全部是中文；false:包含有中文</param>
        /// <returns></returns>
        private static bool IsChinese(string chars, bool RegType)
        {
            if (RegType)
            {
                return System.Text.RegularExpressions.Regex.IsMatch(chars, @"^([\u4e00-\u9fa5]|[\uff01-\uff60]|\u3000){1,}$");
            }
            else
            {
                return System.Text.RegularExpressions.Regex.IsMatch(chars, @"([\u4e00-\u9fa5]|[\uff01-\uff60]|\u3000){1,}");
            }
        }

        /// <summary>
        /// 截取字符串(按字节)
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="length">截取的字节长度</param>
        /// <returns></returns>
        public static string SubStr(string s, int length)
        {
            if (string.IsNullOrWhiteSpace(s) || length < 1)
            {
                return "";
            }
            if (Encoding.GetEncoding("GB2312").GetBytes(s).Length <= length)
            {
                return s;
            }
            // 全部是中文
            if (IsChinese(s, true))
            {
                return s.Substring(0, length / 2);
            }
            // 如果不含有中文
            if (!IsChinese(s, false))
            {
                return s.Substring(0, length);
            }
            string str = "";
            int num = length / 2;
            int num2 = length;
            while (true)
            {
                str = str + s.Substring(str.Length, num);
                num2 = length - Encoding.GetEncoding("GB2312").GetBytes(str).Length;
                if (num2 <= 1)
                {
                    if ((num2 == 1) && (Encoding.GetEncoding("GB2312").GetBytes(s.Substring(str.Length, 1)).Length == 1))
                    {
                        str = str + s.Substring(str.Length, 1);
                    }
                    return str;
                }
                num = num2 / 2;
            }
        }

        /// <summary>
        /// 截取指定长度的字节数，并在末尾追加指定字符，比如“...”
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length"></param>
        /// <param name="tailString"></param>
        /// <returns></returns>
        public static string SubStr(string s, int length, string tailString)
        {
            if (string.IsNullOrWhiteSpace(s) || length < 1)
            {
                return string.Empty;
            }
            if (string.IsNullOrWhiteSpace(tailString))
            {
                tailString = "...";
            }
            if (Encoding.GetEncoding("GB2312").GetBytes(s).Length > length)
            {
                return SubStr(s, length) + tailString;
            }
            return s;
        }

        /// <summary>
        /// 对字符串进行编码，对应js的escape()方法
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Escape(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return string.Empty;
            }
            return Microsoft.JScript.GlobalObject.escape(str);
        }

        /// <summary>
        /// 对字符串进行解码，对应js的unescape()方法
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Unescape(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return string.Empty;
            }
            return Microsoft.JScript.GlobalObject.unescape(str);
        }

        #region 调试模式再输出
        /// <summary>
        /// 调试模式再输出
        /// </summary>
        /// <param name="message">信息</param>
        public static void DebugWriteLine(string message)
        {
            #if DEBUG
            Console.WriteLine(message);
            #endif
        }
        #endregion
    }
}