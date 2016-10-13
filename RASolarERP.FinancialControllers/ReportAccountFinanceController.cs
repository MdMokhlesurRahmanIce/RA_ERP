using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using RASolarHelper;
using RASolarERP.Web.Models;

namespace RASolarERP.Web.Areas.Financial.Controllers
{
    public class ReportAccountFinanceController : Controller
    {
        HelperData helperDal = new HelperData();
        RASolarSecurityData securityDal = new RASolarSecurityData();

        LoginHelper objLoginHelper = new LoginHelper();
        string message = string.Empty;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReportViewerAccountFinance()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAccounting, "ReportViewerAccountFinance", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            ViewData["reportUrl"] = "../Reports/View/local/StatementOfFinancilaPosition/";

            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForAccounting.ToString("MMMM-yy");
            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;

            return View();
        }

        public ActionResult ReportIncomeStatement()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAccounting, "ReportIncomeStatement", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            ViewData["reportUrl"] = "../Reports/View/local/IncomeStatement/";

            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForAccounting.ToString("MMMM-yy");
            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;

            return View();
        }
    }
}
