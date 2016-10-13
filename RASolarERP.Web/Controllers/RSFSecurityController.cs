using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using RASolarSecurity.Model;
using RASolarERP.Model;
using RASolarHelper;
using RASolarERP.Web.Models;
using System.Collections;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Collections.Specialized;
using System.Management;

namespace RASolarERP.Web.Controllers
{
    public class RSFSecurityController : Controller
    {
        LoginHelper objLoginHelper = new LoginHelper();
        private RASolarERPData erpDal = new RASolarERPData();
        private HelperData helperDal = new HelperData();
        private RASolarSecurityData securityDal = new RASolarSecurityData();

        public ActionResult Index()
        {
            ViewBag.RSFModule = helperDal.GetRASolarERPModule();
            return View();
        }

        public ActionResult Login()
        {
            if (Session["LogInInformation"] != null)
            {
                LoginHelper objLoginHelper = new LoginHelper();
                objLoginHelper = (LoginHelper)Session["LogInInformation"];

                string[] actionController = objLoginHelper.HomeURL.Split('/');

                return RedirectToAction(actionController[1], actionController[0]);
            }

            return View();
        }

        [HttpPost]
        public JsonResult Login(FormCollection fCollection)
        {

            try
            {
                ArrayList logInMessage = new ArrayList();

                UserInformation userInfo = new UserInformation();
                userInfo = securityDal.ReadUserInformation(fCollection["txtUserName"], fCollection["txtPassword"], Helper.Active);

                if (userInfo != null)
                {
                    objLoginHelper = securityDal.LoginInformation(fCollection, userInfo);

                    String ipAddress = string.Empty, sMacAddress = string.Empty;
                    
                    ipAddress = IPNetworking.GetIP4Address();
                    // ipAddress = System.Web.HttpContext.Current.Request.UserHostAddress; // Use it Or the below one for get Visitors IP address

                    if (String.IsNullOrEmpty(ipAddress))
                    {

                        ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                        if (ipAddress == null)
                        {
                            ipAddress = Request.ServerVariables["REMOTE_ADDR"];
                        }
                    }
                   

                    //sMacAddress = NetworkUtility.GetMacAddress(NetworkUtility.GetMacAddress(ipAddress));

                    erpDal.CreateUserLog(ipAddress, sMacAddress, objLoginHelper.LocationCode, objLoginHelper.LogInID, "Login");

                    Session["LogInInformation"] = objLoginHelper;

                    return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty, "Valid", Helper.URL(objLoginHelper)) };
                }
                else
                {
                    return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Sorry, UserId/Password Is Not Valid.", "error", string.Empty) };
                }

                //RASolarERPDAL objModuleDal = new RASolarERPDAL();

                //ViewBag.RSFModule = objModuleDal.RSFERPModuleDataForCombo();
                //ViewBag.UserValidation = false;

                //return View();

                //aa = fCollection["ddlZone"];
                //aa = fCollection["ddlRegion"];
                //aa = fCollection["ddlUnit"];
                //aa = fCollection["txtUserName"];
                //aa = fCollection["txtPassword"];
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public ActionResult LogOut()
        {
            Session.RemoveAll();
            Session.Abandon();
            GC.Collect();

            return RedirectToAction("Login", "RSFSecurity");
        }

        public ActionResult LocationSelection()
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

            if (objLoginHelper.UerRoleOrGroupID == UserGroup.RegionManager)
            {
                ViewBag.URL = "HeadOffice/Auditor";
                ViewBag.RoleOrGroup = UserGroup.HeadOfficeAudit;
            }
            else
            {
                ViewBag.URL = objLoginHelper.URL;
                ViewBag.RoleOrGroup = objLoginHelper.UerRoleOrGroupID;
            }

            ViewBag.LocationType = new UserLocationType().UserLocationTypeFormat().Where(l => l.LocationTypeID != "region").ToList();

            ViewBag.Zone = erpDal.Zone();
            ViewBag.ModuleName = "Auditor Module";
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            return View();
        }

        public JsonResult LocationSet(string hoCode, string zoneCode, string regionCode, string unitCode)
        {
            LoginHelper objLoginHelper = new LoginHelper();

            Common_ZoneInfo objZone = new Common_ZoneInfo();
            Common_RegionInfo objRegion = new Common_RegionInfo();
            Common_UnitInfo objUnit = new Common_UnitInfo();

            string message = string.Empty, auditPrefix = "Audit- ";

            try
            {
                objLoginHelper = (LoginHelper)Session["LogInInformation"];


                if (!string.IsNullOrEmpty(zoneCode))
                {
                    objZone = erpDal.Zone(zoneCode);
                    objRegion = null;
                    objUnit = null;
                }

                if (!string.IsNullOrEmpty(regionCode))
                    objRegion = erpDal.Region(regionCode);

                if (!string.IsNullOrEmpty(unitCode))
                    objUnit = erpDal.Unit(unitCode);

                objLoginHelper.LogInForZoneCode = string.Empty;
                objLoginHelper.LogInForZoneName = string.Empty;
                objLoginHelper.LogInForRegionCode = string.Empty;
                objLoginHelper.LogInForRegionName = string.Empty;
                objLoginHelper.LogInForUnitCode = string.Empty;
                objLoginHelper.LogInForUnitName = string.Empty;

                objLoginHelper.ZoneTitle = string.Empty;
                objLoginHelper.RegionTitle = string.Empty;
                objLoginHelper.UnitTitle = string.Empty;


                if (!string.IsNullOrEmpty(hoCode))
                {
                    objLoginHelper.Location = "HO";
                    objLoginHelper.LocationCode = "9000";

                    objZone = null;
                    objRegion = null;
                    objUnit = null;

                    objLoginHelper.ModluleTitle = auditPrefix + "Head Office";
                    objLoginHelper.OfficeModule = Helper.HeadOfficeModule;

                    objLoginHelper.UserOperationalRoleOrGroupID = UserGroup.HeadOfficeAudit;
                }

                if (objZone != null)
                {
                    objLoginHelper.LogInForZoneCode = zoneCode;
                    objLoginHelper.LogInForZoneName = objZone.Zone_Name;
                    objLoginHelper.LocationCode = zoneCode;
                    objLoginHelper.Location = "Zone";

                    objLoginHelper.ZoneTitle = "Zone: ";
                    objLoginHelper.RegionTitle = string.Empty;
                    objLoginHelper.UnitTitle = string.Empty;

                    objLoginHelper.ModluleTitle = auditPrefix + "Zonal Module";
                    objLoginHelper.OfficeModule = Helper.ZonalOfficeModule;

                    objLoginHelper.UserOperationalRoleOrGroupID = UserGroup.ZonalManager;
                }

                if (objRegion != null)
                {
                    objLoginHelper.LogInForRegionCode = regionCode;
                    objLoginHelper.LogInForRegionName = objRegion.Reg_Name;
                    objLoginHelper.LocationCode = regionCode;
                    objLoginHelper.Location = "Region";

                    objLoginHelper.RegionTitle = "Region: ";
                }

                if (objUnit != null)
                {
                    objLoginHelper.LogInForUnitCode = unitCode;
                    objLoginHelper.LogInForUnitName = objUnit.Unit_Name + " [" + unitCode + "]";
                    objLoginHelper.LocationCode = unitCode;
                    objLoginHelper.Location = "Unit";

                    objLoginHelper.UnitTitle = "Unit: ";

                    objLoginHelper.CustomerPrefix = objUnit.CUSTCODE_PREFIX + "-";

                    objLoginHelper.ModluleTitle = auditPrefix + "Unit Module";
                    objLoginHelper.OfficeModule = Helper.UnitOfficeModule;

                    objLoginHelper.UserOperationalRoleOrGroupID = UserGroup.UnitOfficeUser;
                }

                objLoginHelper = securityDal.LoginInformation(objLoginHelper);

                String ipAddress = string.Empty, sMacAddress = string.Empty;

                 ipAddress = IPNetworking.GetIP4Address();

                if (String.IsNullOrEmpty(ipAddress))
                {

                    ipAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
                }

                erpDal.CreateUserLog(ipAddress, sMacAddress, objLoginHelper.LocationCode, objLoginHelper.LogInID, "Auditor- Visit Location After Login");

                //string auditOpenMonth = objLoginHelper.AuditorYearMonth;
                //objLoginHelper.YearMonthCurrent = auditOpenMonth;
                //objLoginHelper.AuditorYearMonth = Helper.YearMonthPrevious(auditOpenMonth, Helper.OneMonthPrevious);

                if (objLoginHelper.UerRoleOrGroupID == UserGroup.RegionManager)
                {
                    objLoginHelper.URL = "HeadOffice/Auditor";
                    objLoginHelper.TopMenu = "SiteMenu/UnitAuditorMenu";
                    objLoginHelper.URLSelectionLocation = "RSFSecurity/LocationSelection";
                }

                Session.Remove("LogInInformation");
                Session["LogInInformation"] = objLoginHelper;

                message = "succeed";
            }
            catch (Exception ex)
            {
                message = "notSucceed";
            }

            return new JsonResult { Data = message };
        }

        public ActionResult HomePageSelection()
        {
            LoginHelper objLoginHelper = new LoginHelper();

            string homeURL = string.Empty;

            try
            {
                objLoginHelper = (LoginHelper)Session["LogInInformation"];

                if (objLoginHelper.UerRoleOrGroupID == UserGroup.RegionManager)
                {
                    Common_RegionInfo objRegion = new Common_RegionInfo();
                    objRegion = erpDal.Region(objLoginHelper.HomeLocation);

                    Common_ZoneInfo objZone = erpDal.Zone(objRegion.Zone_Code);

                    objLoginHelper.LogInForRegionCode = objRegion.Reg_Code;
                    objLoginHelper.LogInForRegionName = objRegion.Reg_Name + " [" + objLoginHelper.HomeLocation + "]";

                    objLoginHelper.LogInForZoneCode = objRegion.Zone_Code;
                    objLoginHelper.LogInForZoneName = objZone.Zone_Name;

                    objLoginHelper.LogInForUnitCode = "";
                    objLoginHelper.LogInForUnitName = "";

                    objLoginHelper.Location = Helper.Region;
                    objLoginHelper.TopMenu = "SiteMenu/RegionManagerMenu";

                    objLoginHelper.URL = "RegionalOffice/RegionManager";
                    objLoginHelper.URLSelectionLocation = "RSFSecurity/LocationSelection";

                }

                Session.Remove("LogInInformation");
                Session["LogInInformation"] = objLoginHelper;

                homeURL = objLoginHelper.HomeURL;
            }
            catch (Exception ex)
            {
                homeURL = "notSucceed";
            }

            string[] controllerAction = homeURL.Split('/');

            return RedirectToAction(controllerAction[1], controllerAction[0]);
        }

        public JsonResult LoadZone()
        {
            ArrayList arl = new ArrayList();
            //List<Common_ZoneInfo> zone = new List<Common_ZoneInfo>();

            List<LocationInfo> zone = new List<LocationInfo>();
            zone = erpDal.Location();

            //zone = erpDal.Zone();

            foreach (LocationInfo zz in zone)
            {
                arl.Add(new { Value = zz.LocationCode, Display = zz.LocationName });
            }

            return new JsonResult { Data = arl };
        }

        public JsonResult LoadParticularZone()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            ArrayList arl = new ArrayList();

            RASolarERPData objDal = new RASolarERPData();
            //Common_ZoneInfo zone = new Common_ZoneInfo();
            //zone = erpDal.Zone(objLoginHelper.LocationCode);

            LocationInfo zone = new LocationInfo();
            zone = erpDal.Location(objLoginHelper.LocationCode);

            arl.Add(new { Value = zone.LocationCode.ToString(), Display = zone.LocationName });

            return new JsonResult { Data = arl };
        }

        public JsonResult LoadRegion(string zoneCode)
        {
            ArrayList arl = new ArrayList();

            RASolarERPData objDal = new RASolarERPData();
            //List<Common_RegionInfo> region = new List<Common_RegionInfo>();
            //region = erpDal.RegionByZoneCode(zoneCode);

            List<LocationInfo> region = new List<LocationInfo>();
            region = erpDal.LocationByLocationCode(zoneCode);

            foreach (LocationInfo rg in region)
            {
                arl.Add(new { Value = rg.LocationCode, Display = rg.LocationName });
            }

            return new JsonResult { Data = arl };
        }

        public JsonResult LoadUnit(string regionCode)
        {
            ArrayList arl = new ArrayList();

            //List<Common_UnitInfo> unit = new List<Common_UnitInfo>();
            List<LocationInfo> units = new List<LocationInfo>();
            units = erpDal.LocationByLocationCode(regionCode);

            //unit = erpDal.UnitByRegionCode(regionCode);

            foreach (LocationInfo un in units)
            {
                arl.Add(new { Value = un.LocationCode.ToString(), Display = un.LocationName });
            }

            return new JsonResult { Data = arl };
        }

        public ActionResult LocationSelectionPartial()
        {
            ViewBag.LocationType = new UserLocationType().UserLocationTypeFormat();
            return PartialView("UnitSelection");
        }
    }
}
