using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bitshare.Common
{
    /// <summary>
    /// 通用方法工具类
    /// </summary>
    public class Utilitycs
    {
        /// <summary>
        /// 把时间戳转换成对应的时间
        /// </summary>
        /// <param name="timestamp">时间戳</param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(string timestamp)
        {
            var dtbase = new DateTime(1970, 1, 1, 8, 0, 0, 0); // UTC +8
            return dtbase.AddSeconds(double.Parse(timestamp));
        }

        /// <summary>
        /// Generate the UNIX style timestamp for DateTime.UtcNow        
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="isMiliseconds">是否返回毫秒</param>
        /// <returns></returns>
        public static string GenerateTimeStamp(DateTime dt, bool isMiliseconds)
        {
            if (isMiliseconds)
            {
                TimeSpan ts = dt - new DateTime(1970, 1, 1, 8, 0, 0, 0);
                return Convert.ToInt64(ts.TotalMilliseconds).ToString();
            }
            else
            {
                TimeSpan ts = dt - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                return Convert.ToInt64(ts.TotalSeconds).ToString();
            }
        }

        /// <summary>
        /// Generate the UNIX style timestamp for DateTime.UtcNow        
        /// </summary>
        /// <returns></returns>
        public static string GenerateTimeStamp()
        {
            return GenerateTimeStamp(DateTime.UtcNow, false);
        }
    }
}
