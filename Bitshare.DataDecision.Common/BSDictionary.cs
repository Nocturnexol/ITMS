using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace Bitshare.PTMM.Common
{
    public class BSDictionary : ConcurrentDictionary<string, DateTime>
    {
        public BSDictionary() { }

        public BSDictionary(IDictionary<string, DateTime> dictionary)
            : base(dictionary)
        {

        }

        public  void Add(string key, DateTime val)
        {
            List<string> expireKeys = this.Where(p => p.Value < DateTime.Now).Select(p => p.Key).ToList();
            if (expireKeys.Count > 0)
            {
                expireKeys.ForEach((k) =>
                {
                    DateTime time;
                    this.TryRemove(k, out time);
                });
            }
            this.AddOrUpdate(key, val, (k, v) => val);
        }
        /// <summary>
        /// 0：不存在，-1：过期，1：正常
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public  int TokenExpireState(string key)
        {
            if (!this.ContainsKey(key))
            {
                return 0;
            }
            else
            {
                DateTime time;
                this.TryGetValue(key, out time);
                if (time < DateTime.Now)
                {
                    this.TryRemove(key, out time);
                    return -1;
                    
                }
                else
                {
                    return 1;
                }
            }
        }

       

    }
}
