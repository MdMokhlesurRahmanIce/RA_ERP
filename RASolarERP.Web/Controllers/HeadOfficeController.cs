using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using RASolarERP.Model;
using RASolarHelper;
using RASolarERP.Web.Models;

namespace RASolarERP.Web.Controllers
{
    public class HeadOfficeController : Controller
    {
        LoginHelper objLoginHelper = new LoginHelper();
        RASolarERPData erpDal = new RASolarERPData();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HOSales()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            //ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForSales.ToString("MMMM-yy");
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            return View();

        }

        public ActionResult HOInventory()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForInventory.ToString("MMMM-yy");
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.ModuleName = objLoginHelper.ModluleTitle;

            return View();
        }

        public ActionResult HOAccountFinance()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForAccounting.ToString("MMMM-yy");
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.ModuleName = objLoginHelper.ModluleTitle;


            return View();
        }

        public ActionResult HOHumanResourceAdmin()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            //ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForHRMS.ToString("MMMM-yy");
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.ModuleName = objLoginHelper.ModluleTitle;

            return View();
        }

        public ActionResult Auditor()
        {
            LoginHelper objLoginHelper = new LoginHelper();
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;

            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.ModuleName = objLoginHelper.ModluleTitle;

            if (objLoginHelper.UerRoleOrGroupID == UserGroup.RegionManager)
            {
                ViewBag.ShowHideMenu = true;
            }
            else
                ViewBag.ShowHideMenu = false;

            return View();
        }

        public ActionResult RSFExecutiveCommittee()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            return View();
        }

        public ActionResult HOACCAdvanceUsers()
        {

            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            return View();
        }

        public ActionResult HOACCEntryUsers()
        {

            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            return View();
        }

        public ActionResult INVAdvanceUsers()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            return View();
        }

        public ActionResult INVEntryUsers()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            return View();
        }


    }
}
