using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Bitshare.DataDecision.Common
{
    /// <summary>
    /// Url页面权限验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class UrlAuthorizeAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 页面名称
        /// </summary>
        private string PageTitle;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pageTitle">页面对应菜单名称</param>
        public UrlAuthorizeAttribute(string pageTitle)
        {
            PageTitle = pageTitle;
        }

        /// <summary>        
        /// 在执行操作方法之前由 ASP.NET MVC 框架调用。        
        /// </summary>        
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //FilterContextInfo fi = new FilterContextInfo(filterContext);
            //if (CurrentHelper.CurrentUser == null)
            //{
            //    RouteValueDictionary routeValues = new RouteValueDictionary(new { Area = "", Controller = "Home", Action = "MainPage" });
            //    filterContext.Result = new RedirectToRouteResult(routeValues);
            //    return;
            //}
            //if (!IsHasRight()) //判断是否满足条件         
            //{
            //    // 直接返回 return Content("抱歉,你不具有当前操作的权限！")
            //    //filterContext.Result = new ContentResult { Content = @"抱歉,你不具有当前操作的权限！" };

            //    // 逻辑代码                
            //    //filterContext.Result = new HttpUnauthorizedResult(); // 直接URL输入的页面地址跳转到登陆页                  
            //    //filterContext.Result = new RedirectResult("http://www.baidu.com");//也可以跳到别的站点

            //    RouteValueDictionary routeValues = new RouteValueDictionary(new { Area = "", Controller = "Home", Action = "NoRight" });
            //    filterContext.Result = new RedirectToRouteResult(routeValues);
            //}
        }

        /// <summary>
        /// 判断当前用户是否有页面权限
        /// </summary>
        /// <returns></returns>
        private bool IsHasRight()
        {
            List<string> list = new List<string>();
            list = FlowHelper.GetBtnAuthorityForPage(PageTitle);
            //return list.Contains("查询");
            return list != null && list.Count > 0;
        }
    }

    /// <summary>
    /// 过滤器信息类
    /// </summary>
    public class FilterContextInfo
    {
        public FilterContextInfo(ActionExecutingContext filterContext)
        {
            #region 获取链接信息
            // 获取域名            
            DomainName = filterContext.HttpContext.Request.Url.Authority;
            // 获取模块名称            
            Module = filterContext.HttpContext.Request.Url.Segments[1].Replace('/', ' ').Trim();
            // 获取controller名称            
            ControllerName = filterContext.RouteData.Values["controller"].ToString();
            // 获取action 名称            
            ActionName = filterContext.RouteData.Values["action"].ToString();
            #endregion
        }

        /// <summary>        
        /// 获取域名        
        /// </summary>        
        public string DomainName { get; set; }
        /// <summary>        
        /// 获取模块名称        
        /// </summary>        
        public string Module { get; set; }
        /// <summary>        
        /// 获取 ControllerName 名称        
        /// </summary>        
        public string ControllerName { get; set; }
        /// <summary>        
        /// 获取ACTION 名称        
        /// </summary>        
        public string ActionName { get; set; }
    }
}
