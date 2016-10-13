using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using RASolarHelper;

namespace RASolarERP.Web.Controllers
{
    public class ErrorHnadleController : Controller
    {
        LoginHelper objLoginHelper = new LoginHelper();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ErrorMessage()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.MessageInformation = Session["messageInformation"].ToString();
            //Session.Remove("messageInformation");

            return View();
        }

    }
}
