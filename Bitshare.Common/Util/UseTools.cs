using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Bitshare.Common
{
    public class UseTools
    {
        public static string GetProjectTitle()
        {
            string CacheKey = "AppSettings-ProjectTitle";
            object objModel = DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = ConfigurationManager.AppSettings["ProjectTitle"];
                    if (objModel != null)
                    {
                        DataCache.SetCache(CacheKey, objModel);
                    }
                    else
                    {
                        objModel = "";
                    }
                }
                catch
                {
                    objModel = "";
                }
            }

            return objModel.ToString();
        }

        public static string GetProjectKey()
        {
            string CacheKey = "AppSettings-ProjectKey";
            object objModel = DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = ConfigurationManager.AppSettings["ProjectKey"];
                    if (objModel != null)
                    {
                        DataCache.SetCache(CacheKey, objModel);
                    }
                    else
                    {
                        objModel = "";
                    }
                }
                catch
                {
                    objModel = "";
                }
            }
            return objModel.ToString();
        }

        public static int GetSecurityType()
        {
            string CacheKey = "AppSettings-SecurityType";
            object objModel = DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = ConfigurationManager.AppSettings["SecurityType"];
                    if (objModel != null)
                    {
                        DataCache.SetCache(CacheKey, objModel);
                    }
                    else
                    {
                        objModel = "0";
                    }
                }
                catch
                {
                    objModel = "0";
                }
            }
            int type = 0;
            Int32.TryParse(Convert.ToString(objModel), out type);
            return type;
        }

    }
}
