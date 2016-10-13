using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using RASolarHelper;

namespace RASolarERP.Web.Controllers
{
    public class RREController : Controller
    {
        LoginHelper objLoginHelper = new LoginHelper();
       
        // GET: /RRE/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RREReview()
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

    }
}
