using System.Web.Mvc;

namespace RASolarERP.Web.Areas.Financial
{
    public class FinancialAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Financial";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Financial_default",
                "Financial/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
