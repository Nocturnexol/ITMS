using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Bitshare.DataDecision.Controllers.Filter;

namespace Bitshare.DataDecision.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Filters.Add(new ApiSecurityFilter());
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "Api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
               
            );
        }
    }
}
