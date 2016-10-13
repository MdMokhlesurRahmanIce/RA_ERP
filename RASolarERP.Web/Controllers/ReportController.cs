using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using RASolarHelper;
using RASolarERP.Model;
using RASolarERP.Web.Models;

namespace RASolarERP.Web.Controllers
{
    public class ReportController : Controller
    {
        RASolarERPData erpDal = new RASolarERPData();
        HelperData helperDal = new HelperData();
        LoginHelper objLoginHelper = new LoginHelper();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReportViewer()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            RASolarSecurityData dal = new RASolarSecurityData();

            ViewData["reportUrl"] = "../Reports/View/local/ClosingInventory/";
            ViewBag.YearMonthFormat = helperDal.ReadYearMonthFormat();
            ViewBag.TopMenu = objLoginHelper.TopMenu;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForInventory.ToString("MMMM-yy");
            ViewBag.OpenMonth = objLoginHelper.MonthOpenForInventory.ToString("yyyyMM");
            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;

            return View();
        }
    }
}
