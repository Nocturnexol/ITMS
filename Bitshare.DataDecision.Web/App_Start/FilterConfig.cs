using System.Web;
using System.Web.Mvc;
using Bitshare.DataDecision.Common;

namespace Bitshare.DataDecision.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}