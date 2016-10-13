using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using RASolarERP.Web.Models;

using RASolarHelper;
using RASolarSecurity.Model;
using System.Configuration;

namespace RASolarERP.Web.Controllers
{
    public class UnitOfficeUserController : Controller
    {
        private RASolarERPData erpDal = new RASolarERPData();
        LoginHelper objLoginHelper = new RASolarHelper.LoginHelper();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UnitOfficeUsers()
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
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            return View();
        }


        public ActionResult TrainingModule()
        {
            string zone = string.Empty, region = string.Empty, unitCode = string.Empty, unitName = string.Empty;
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            zone = objLoginHelper.LogInForZoneName;
            region = objLoginHelper.LogInForRegionName;
            unitCode = objLoginHelper.LogInForUnitCode;
            unitName = objLoginHelper.LogInForUnitName;

            string webAddress = ConfigurationManager.AppSettings["RASolarERP_CustomerTraining"];

            return Redirect(webAddress + "?z=" + zone + "&r=" + region + "&lc=" + unitCode + "&un=" + unitName);

        }
    }
}
