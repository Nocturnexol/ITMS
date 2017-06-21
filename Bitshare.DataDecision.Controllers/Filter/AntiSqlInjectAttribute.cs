using Bitshare.Common;
using System.Web.Mvc;
namespace Bitshare.DataDecision.Controllers.Filter
{
    /// <summary>
    /// SQL注入过滤
    /// </summary>
    public class AntiSqlInjectAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var actionParameters = filterContext.ActionDescriptor.GetParameters();
            foreach (var p in actionParameters)
            {
                if (p.ParameterType == typeof(string))
                {

                }
            }
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var actionParameters = filterContext.ActionDescriptor.GetParameters();
            foreach (var p in actionParameters)
            {
                if (p.ParameterType == typeof(string))
                {
                    if (filterContext.ActionParameters[p.ParameterName] != null)
                    {
                        filterContext.ActionParameters[p.ParameterName] = filterContext.ActionParameters[p.ParameterName].ToString().FilteSQL();
                    }
                }
            }
        }
    }

}
