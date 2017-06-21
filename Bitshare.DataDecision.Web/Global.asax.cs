using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Collections.Specialized;
using Bitshare.DataDecision.Common;

namespace Bitshare.DataDecision.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            FlowHelper.InitBasicType();
            AreaRegistration.RegisterAllAreas();
            this.BeginRequest += new EventHandler(Application_BeginRequest);
            this.EndRequest += new EventHandler(Application_EndRequest);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BundleTable.EnableOptimizations = true;
            AuthConfig.RegisterAuth();
        }
        public DateTime dt { set; get; }
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            //记录页面开始加载时间
            dt = DateTime.Now;
        }
        protected void Application_EndRequest(Object sender, EventArgs e)
        {
            DateTime dt2 = DateTime.Now;//页面加载结束时间
            TimeSpan ts = dt2 - dt;//获得页面加载花费时间
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;
            if (context.Response.StatusCode != 302 && context.Response.StatusCode != 301)
            {
                //context.Response.AddHeader("Time", ts.Milliseconds.ToString());
            }

        }
    }
}