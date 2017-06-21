using System.Web.Mvc;

namespace Bitshare.DataDecision.Web.Areas.ResourceManagement
{
    public class ResourceManagementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ResourceManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "ResourceManagement_default",
                url: "ResourceManagement/{controller}/{action}/{id}",
                defaults: new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Bitshare.DataDecision.Controllers.Areas.ResourceManagement" }
            );
        }
    }
}
