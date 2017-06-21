using System.Web.Mvc;

namespace Bitshare.DataDecision.Web.Areas.System
{
    public class SystemAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "System";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "System_default",
                url: "System/{controller}/{action}/{id}",
                defaults: new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Bitshare.DataDecision.Controllers.Areas.System" }
            );
        }
    }
}
