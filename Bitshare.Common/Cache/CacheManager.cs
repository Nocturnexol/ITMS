using System;
using System.Collections;
using System.Web;

namespace Bitshare.DataDecision.Common
{
    /// <summary>
    /// Cache管理类
    /// tanh
    /// 2016/01/14
    /// </summary>
    public class CacheManager
    {
        private static System.Web.Caching.Cache cache = HttpRuntime.Cache;

        /// <summary>
        /// 插入缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="obj">值</param>
        /// <returns>成功?</returns>
        public static bool Insert(string key, object obj)
        {
            if (obj == null) return false;
            cache.Insert(key, obj, null, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration);
            return true;
        }
        /// <summary>
        /// 插入缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="obj">值</param>
        /// <returns>成功?</returns>
        public static bool Insert(string key, object obj, DateTime expiry)
        {
            if (obj == null) return false;
            cache.Insert(key, obj, null, expiry, System.Web.Caching.Cache.NoSlidingExpiration,
                System.Web.Caching.CacheItemPriority.Normal, null);

            return true;
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static object Get(string key)
        {
            return cache.Get(key);
        }
        /// <summary>
        /// 移出缓存
        /// </summary>
        /// <param name="key">键</param>
        public static void Remove(string key)
        {
            cache.Remove(key);
        }

        /// <summary>
        /// 移出所有缓存
        /// </summary>
        public static void RemoveAll()
        {
            IDictionaryEnumerator iterator = cache.GetEnumerator();//遍历整个缓存
            while (iterator.MoveNext())
            {
                Remove(iterator.Key.ToString());
            }
        }

    }
}
