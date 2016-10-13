using System.Web.Mvc;

namespace RASolarERP.Web.Areas.HRMS
{
    public class HRMSAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "HRMS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "HRMS_default",
                "HRMS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
