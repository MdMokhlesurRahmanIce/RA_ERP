using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RASolarHelper;
using RASolarERP.Model;

using RASolarERP.Web.Models;
using System.Collections;
using Telerik.Web.Mvc;
using RASolarERP.DomainModel.InventoryModel;
using RASolarERP.Web.Areas.Sales.Models;
using RASolarERP.Web.Areas.Inventory.Models;
using RASolarERP.Web.Areas.HRMS.Models;

namespace RASolarERP.Web.Areas.Inventory.Controllers
{
    public class InventoryDeptController : Controller
    {
        LoginHelper objLoginHelper = new LoginHelper();

        RASolarSecurityData securityDal = new RASolarSecurityData();
        InventoryData inventoryDal = new InventoryData();
        SalesData salesDal = new SalesData();
        SHSDistributionProcess shsDistributionProcess = new SHSDistributionProcess();
        HelperData helperDal = new HelperData();
        private HRMSData hrmsDal = new HRMSData();

        private RASolarERPData erpDal = new RASolarERPData();
        string message = string.Empty;

        List<SHSDistributionPlan> lstSHSDistributionPlanAdd = new List<SHSDistributionPlan>();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InventoryDataEntryStatusReport(string reportType, string locationType)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForInventory, "InventoryDataEntryStatusReport", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            List<InventoryDataEntryStatus> inventoryDataEntrySummary = new List<InventoryDataEntryStatus>();

            inventoryDataEntrySummary = inventoryDal.InventoryDataEntryState(objLoginHelper.ReportType, objLoginHelper.LocationCode, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForInventory));

            ViewBag.IsAuthenticForSave = objLoginHelper.IsAuthenticApproverForThisLocation;

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForInventory.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.TotalNumberofUnits = inventoryDataEntrySummary.Count();
            ViewBag.EntryFinalizedbyUM = inventoryDataEntrySummary.Count(s => s.FinalizedByUM != null);
            ViewBag.EntryfinalizedbyRM = inventoryDataEntrySummary.Count(s => s.FinalizedByRM != null);
            ViewBag.EntryFinalizedbyZM = inventoryDataEntrySummary.Count(s => s.FinalizedByZM != null);
            ViewBag.EntryfinalizedbyHO = inventoryDataEntrySummary.Count(s => s.FinalizedByHO != null);



            TempData["InventoryOpenMonth"] = objLoginHelper.MonthOpenForInventory.ToString("MMM-yyy");

            return View(inventoryDataEntrySummary);
        }

        public ActionResult ItemSummary()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForInventory, "ItemSummary", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            ViewBag.ItemType = inventoryDal.ReadItemType();

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForInventory.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.MonthYear = new YearMonthFormat().MonthFormat();
            ViewBag.ReportType = new RASolarERPModule().RSFERPReportSelection();
            ViewBag.StoreLocation = inventoryDal.ReadStoreLocation();
            ViewBag.VendorList = inventoryDal.GetVendorListForItemSummary();

            return View();
        }

        public ActionResult ERPVersusPhysicalBalance()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForInventory, "ERPVersusPhysicalBalance", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            ViewBag.ItemType = inventoryDal.ReadItemType();
            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.ZoneCode = objLoginHelper.LogInForZoneCode;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.RegionCode = objLoginHelper.LogInForRegionCode;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.UnitCode = objLoginHelper.LogInForUnitCode;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForInventory.ToString("MMMM-yy");
            ViewBag.OpenMonthYearForDDl = Convert.ToInt32(objLoginHelper.MonthOpenForInventory.ToString("yyyyMM"));
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;
            ViewBag.YearMonthFormat = helperDal.ReadYearMonthFormat().Where(mn => Convert.ToInt32(mn.YearMonthValue) <= ViewBag.OpenMonthYearForDDl).Select(mn => new { mn.YearMonthValue, mn.YearMonthName });
            ViewBag.MonthYear = new YearMonthFormat().MonthFormat();
            ViewBag.ReportType = new RASolarERPModule().RSFERPReportSelection().Select(b => new { b.ModuleTypeValue, b.ModuleTypeName }).Distinct().ToList();

            if (ViewBag.ZoneCode == "" || ViewBag.ZoneCode == null)
            {
                ViewBag.Zone = new RASolarERPData().Location().Select(z => new { z.LocationCode, z.LocationName }).Distinct().ToList();
            }
            else if (ViewBag.ZoneCode != "" || ViewBag.ZoneCode != null)
            {
                ViewBag.Zone = new RASolarERPData().Location().Where(z => z.LocationCode == ViewBag.ZoneCode).Select(z => new { z.LocationCode, z.LocationName }).Distinct().ToList();
            }
            if (ViewBag.RegionCode == "" || ViewBag.RegionCode == null)
            {
                ViewBag.Region = new RASolarERPData().LocationByLocationCode(objLoginHelper.LogInForZoneCode).Select(r => new { r.LocationCode, r.LocationName }).Distinct().ToList();
            }
            else if (ViewBag.RegionCode != "" || ViewBag.RegionCode != null)
            {
                ViewBag.Region = new RASolarERPData().LocationByLocationCode(objLoginHelper.LogInForZoneCode).Where(r => r.LocationCode == ViewBag.RegionCode).Select(r => new { r.LocationCode, r.LocationName }).Distinct().ToList();
            }

            //ViewBag.Region = new RASolarERPData().LocationByLocationCode(objLoginHelper.LogInForZoneCode).Where(r=>r.LocationCode==ViewBag.RegionCode).Select(r => new {r.LocationCode,r.LocationName}).Distinct().ToList();
            if (ViewBag.UnitCode == "" || ViewBag.UnitCode == null)
            {
                ViewBag.Unit = new RASolarERPData().LocationByLocationCode(objLoginHelper.LogInForRegionCode).Select(u => new { u.LocationCode, u.LocationName }).Distinct().ToList();
            }
            else if (ViewBag.UnitCode != "" || ViewBag.UnitCode != null)
            {
                ViewBag.Unit = new RASolarERPData().LocationByLocationCode(objLoginHelper.LogInForRegionCode).Where(u => u.LocationCode == ViewBag.UnitCode).Select(u => new { u.LocationCode, u.LocationName }).Distinct().ToList();
            }
            //ViewBag.Unit = new RASolarERPData().LocationByLocationCode(objLoginHelper.LogInForRegionCode).Where(u=>u.LocationCode==ViewBag.UnitCode).Select(u => new { u.LocationCode, u.LocationName }).Distinct().ToList();
            ViewBag.StoreLocation = inventoryDal.ReadStoreLocation();
            ViewBag.VendorList = inventoryDal.GetVendorListForItemSummary();

            return View();
        }

        public JsonResult SaveInventoryStatus(string unitCode)
        {
            string saveSuccessMessage = string.Empty;
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            //tbl_UnitWiseEntryStatus objUnitWiseEntryStatus = new tbl_UnitWiseEntryStatus();

            //objUnitWiseEntryStatus = inventoryDal.UnitWiseEntryStatus(unitCode, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForInventory));

            //if (objLoginHelper.Location == Helper.Zone)
            //{
            //    objUnitWiseEntryStatus.InvFinalizedByZM_UserName = objLoginHelper.LogInID;
            //    objUnitWiseEntryStatus.InvCheckedNFinalizedByZM = DateTime.Now;
            //}
            //else if (objLoginHelper.Location == Helper.Region)
            //{
            //    objUnitWiseEntryStatus.InvFinalizedByRM_UserName = objLoginHelper.LogInID;
            //    objUnitWiseEntryStatus.InvCheckedNFinalizedByRM = DateTime.Now;
            //}
            //else if (objLoginHelper.Location == Helper.HeadOffice)
            //{
            //    objUnitWiseEntryStatus.InvFinalizedByHO_UserName = objLoginHelper.LogInID;
            //    objUnitWiseEntryStatus.InvCheckedNFinalizedByHO = DateTime.Now;
            //}

            //objUnitWiseEntryStatus.UserName = objLoginHelper.LogInUserName;
            //objUnitWiseEntryStatus = inventoryDal.UpdateInvetoryEntryStatus(objUnitWiseEntryStatus);

            //if (objUnitWiseEntryStatus != null)
            //{
            //    saveSuccessMessage = "Succeed";
            //}
            //else
            //{
            //    saveSuccessMessage = "NotSucceed";
            //}

            return new JsonResult { Data = saveSuccessMessage };
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

        public JsonResult LoadLocationInfoWithHo()
        {
            ArrayList arl = new ArrayList();

            List<LocationInfo> lstLocationInfo = new List<LocationInfo>();
            lstLocationInfo = erpDal.LocationWithHeadOffice();

            foreach (LocationInfo li in lstLocationInfo)
            {
                arl.Add(new { Value = li.LocationCode, Display = li.LocationName });
            }

            return new JsonResult { Data = arl };
        }

        [GridAction]
        public ActionResult __InventorySummaryReport(string reportType, string itemType, string storeLocation, string locationCode, string startDate, string endDate, string vendorType)
        {
            byte strLocation = 0;

            if (!string.IsNullOrEmpty(storeLocation))
                strLocation = Convert.ToByte(storeLocation);

            return View(new GridModel<InventorySummaryReportV2> { Data = inventoryDal.ReadInventorySummaryReportV2(reportType.Trim(), itemType.Trim(), strLocation, locationCode, Convert.ToDateTime(Helper.DateFormatMMDDYYYY(startDate)).Date, Convert.ToDateTime(Helper.DateFormatMMDDYYYY(endDate)).Date, vendorType.Trim()) });
        }

        [GridAction]
        public ActionResult __InventoryERPVersusPhysicalBalance(string reportType, string itemType, string storeLocation, string locationCode, string yearMonth, string vendorType)
        {
            byte strLocation = 0;

            if (!string.IsNullOrEmpty(storeLocation))
                strLocation = Convert.ToByte(storeLocation);

            return View(new GridModel<InventoryERPVersusPhysicalBalance> { Data = inventoryDal.ReadInventoryERPVersusPhysicalBalance(reportType.Trim(), itemType.Trim(), strLocation, locationCode, yearMonth.Trim(), vendorType.Trim()) });
        }

        public ActionResult VendorChallanVMrrVerification()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForInventory, "VendorChallanVMrrVerification", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForInventory.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            return View();
        }

        public ActionResult SHSDistributionPlan()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            Session["lstSHSDistributionPlanAdd"] = lstSHSDistributionPlanAdd;

            if (!securityDal.IsPageAccessible(Helper.ForInventory, "SHSDistributionPlan", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForInventory.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.RouteCategory = inventoryDal.ReadRootCategory().Select(m => new { m.RouteCategory }).Distinct().ToList();
            ViewBag.PackageCapacity = salesDal.ReadPackageCapacity();
            ViewBag.PackageLight = salesDal.ReadLightByPackageCapacity("0020WP");
            ViewBag.Package = salesDal.ReadPackagesForSHSDP("0020WP", "02LIGHT");
            ViewBag.PanelModel = inventoryDal.ReadPanelModelListForSHSDP("SHS221").Select(p => new { p.Description, p.ItemModelID }).Distinct().ToList();
            ViewBag.BatteryModel = inventoryDal.ReadBatteryModelListForSHSDP("SHS221").Select(b => new { b.Description, b.ItemModelID }).Distinct().ToList();
            ViewBag.SHSDelivaryScheduleNo = "RSF/SCH-2015-" + objLoginHelper.CurrentDate.ToString("MMdd");

            // start  for SHSDelivaryScheduleNo with save button
            //distribScheduleNo,scheduleDate,refScheduleNo
            ViewBag.prmScheduleDate = objLoginHelper.CurrentDate.ToString("yyyy-MM-dd");
            ViewBag.prmRefScheduleNo = ViewBag.SHSDelivaryScheduleNo;
            ViewBag.ChkDistribScheduleNo = inventoryDal.CheckDuplicateDistributionPlan(Helper.DateFormatYYMMDD(Convert.ToDateTime(ViewBag.prmScheduleDate)), Convert.ToDateTime(ViewBag.prmScheduleDate), ViewBag.prmRefScheduleNo);
            // end  for SHSDelivaryScheduleNo with save button

            return View();
        }


        public ActionResult EmployeeVisitPlan()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            Session["lstSHSDistributionPlanAdd"] = lstSHSDistributionPlanAdd;

            if (!securityDal.IsPageAccessible(Helper.ForInventory, "EmployeeVisitPlan", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForInventory.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            //ViewBag.RouteCategory = inventoryDal.ReadRootCategory().Select(m => new { m.RouteCategory }).Distinct().ToList();
            ViewBag.RouteCategory = hrmsDal.ReadPersonLocationWiseEmployee(objLoginHelper.LogInForUnitCode).Select(m => new { m.EmployeeID, m.EmployeeName}).Distinct().ToList(); 
            ViewBag.PackageCapacity = salesDal.ReadPackageCapacity();
            ViewBag.PackageLight = salesDal.ReadLightByPackageCapacity("0020WP");
            ViewBag.Package = salesDal.ReadPackagesForSHSDP("0020WP", "02LIGHT");
            ViewBag.PanelModel = inventoryDal.ReadPanelModelListForSHSDP("SHS221").Select(p => new { p.Description, p.ItemModelID }).Distinct().ToList();
            ViewBag.BatteryModel = inventoryDal.ReadBatteryModelListForSHSDP("SHS221").Select(b => new { b.Description, b.ItemModelID }).Distinct().ToList();
            ViewBag.SHSDelivaryScheduleNo = "RSF/SCH-2014-" + objLoginHelper.CurrentDate.ToString("MMdd");

            // start  for SHSDelivaryScheduleNo with save button
            //distribScheduleNo,scheduleDate,refScheduleNo
            ViewBag.prmScheduleDate = objLoginHelper.CurrentDate.ToString("yyyy-MM-dd");
            ViewBag.prmRefScheduleNo = ViewBag.SHSDelivaryScheduleNo;
            ViewBag.ChkDistribScheduleNo = inventoryDal.CheckDuplicateDistributionPlan(Helper.DateFormatYYMMDD(Convert.ToDateTime(ViewBag.prmScheduleDate)), Convert.ToDateTime(ViewBag.prmScheduleDate), ViewBag.prmRefScheduleNo);
            // end  for SHSDelivaryScheduleNo with save button

            return View();
        }

        public ActionResult EditSHSDistributionPlan()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForInventory, "EditSHSDistributionPlan", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForInventory.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.RouteCategory = inventoryDal.ReadRootCategory().Select(m => new { m.RouteCategory }).Distinct().ToList();
            ViewBag.PackageCapacity = salesDal.ReadPackageCapacity();
            ViewBag.PackageLight = salesDal.ReadLightByPackageCapacity("0020WP");
            ViewBag.Package = salesDal.ReadPackagesForSHSDP("0020WP", "02LIGHT");
            ViewBag.PanelModel = inventoryDal.ReadPanelModelListForSHSDP("SHS221").Select(p => new { p.Description, p.ItemModelID }).Distinct().ToList();
            ViewBag.BatteryModel = inventoryDal.ReadBatteryModelListForSHSDP("SHS221").Select(b => new { b.Description, b.ItemModelID }).Distinct().ToList();
            ViewBag.SHSDelivaryScheduleNo = "RSF/SCH-2015-" + objLoginHelper.CurrentDate.ToString("MMdd");
            ViewBag.StartDate = Convert.ToDateTime(objLoginHelper.CurrentDate.ToString("dd-MMM-yyyy"));
            // start  for SHSDelivaryScheduleNo with save button
            //distribScheduleNo,scheduleDate,refScheduleNo
            ViewBag.prmScheduleDate = objLoginHelper.CurrentDate.ToString("yyyy-MM-dd");
            ViewBag.prmRefScheduleNo = ViewBag.SHSDelivaryScheduleNo;
            ViewBag.ChkDistribScheduleNo = inventoryDal.CheckDuplicateDistributionPlan(Helper.DateFormatYYMMDD(Convert.ToDateTime(ViewBag.prmScheduleDate)), Convert.ToDateTime(ViewBag.prmScheduleDate), ViewBag.prmRefScheduleNo);
            // end  for SHSDelivaryScheduleNo with save button
            
            ViewBag.dataNotFound = "FirstLoadPage";

            List<SHSDistributionPlanPackageORItem> lstSHSDistributionPlan_RootWiseLocationNPackage = new List<SHSDistributionPlanPackageORItem>();
            lstSHSDistributionPlan_RootWiseLocationNPackage = inventoryDal.RootWiseLocationNPackage("");

            List<SHSDistributionPlanPackageORItem> lstLocationSHSDistributionPlanPackageORItem = new List<SHSDistributionPlanPackageORItem>();
            lstLocationSHSDistributionPlanPackageORItem = (from t in lstSHSDistributionPlan_RootWiseLocationNPackage
                                                           group t by new { t.LocationCode }
                                                               into grp
                                                               select new SHSDistributionPlanPackageORItem
                                                               {
                                                                   RouteNo = grp.Min(K => K.RouteNo),
                                                                   RouteName = grp.Min(i => i.RouteName),
                                                                   RouteCategory = grp.Min(j => j.RouteCategory),
                                                                   LocationCode = grp.Key.LocationCode,
                                                                   LocationName = grp.Min(m => m.LocationName)

                                                               }).ToList();

            TempData["ListLocationSHSDistributionPlanPackageORItem"] = lstLocationSHSDistributionPlanPackageORItem;


            List<SHSDistributionPlanPackageORItem> lstRouteSHSDistributionPlanPackageORItem = new List<SHSDistributionPlanPackageORItem>();
            lstRouteSHSDistributionPlanPackageORItem = (from t in lstSHSDistributionPlan_RootWiseLocationNPackage
                                                        group t by new { t.RouteNo }
                                                            into grp
                                                            select new SHSDistributionPlanPackageORItem
                                                            {
                                                                RouteNo = grp.Key.RouteNo,
                                                                RouteName = grp.Min(i => i.RouteName),
                                                                RouteCategory = grp.Min(j => j.RouteCategory)

                                                            }).ToList();

            TempData["ListRouteSHSDistributionPlanPackageORItem"] = lstRouteSHSDistributionPlanPackageORItem;


            List<SHSDistributionPlanPackageORItem> lstSHSDistributionPlan_RootWiseLocationNPackage1 = new List<SHSDistributionPlanPackageORItem>();
            lstSHSDistributionPlan_RootWiseLocationNPackage1 = (from t in lstSHSDistributionPlan_RootWiseLocationNPackage
                                                                group t by new { t.RouteNo, t.PackageOrItemCode, t.PanelModel, t.BatteryModel }
                                                                    into grp
                                                                    select new SHSDistributionPlanPackageORItem
                                                                    {


                                                                        RouteNo = grp.Key.RouteNo,
                                                                        Category = grp.Min(i => i.Category),
                                                                        PackageOrItemCode = grp.Key.PackageOrItemCode,
                                                                        PackageName = grp.Min(t => t.PackageName),
                                                                        PanelModel = grp.Key.PanelModel,
                                                                        PanelModelName = grp.Min(t => t.PanelModelName),
                                                                        BatteryModel = grp.Key.BatteryModel,
                                                                        BatteryModelName = grp.Min(j => j.BatteryModelName),
                                                                        PackageOrItemQuantity = grp.Sum(t => t.PackageOrItemQuantity)


                                                                    }).ToList();


            TempData["ListSHSDistributionPlanPackageORItem"] = lstSHSDistributionPlan_RootWiseLocationNPackage1;


            List<SHSDistributionPlanPackageORItem> lstdistributionSheetGridForPackageOrItemCode = new List<SHSDistributionPlanPackageORItem>();
            lstdistributionSheetGridForPackageOrItemCode = (from t in lstSHSDistributionPlan_RootWiseLocationNPackage
                                                            group t by new { t.PackageOrItemCode, t.PanelModel, t.BatteryModel }
                                                                into grp
                                                                select new SHSDistributionPlanPackageORItem
                                                                {
                                                                    Category = grp.Min(i => i.Category),
                                                                    PackageOrItemCode = grp.Key.PackageOrItemCode,
                                                                    PackageName = grp.Min(t => t.PackageName),
                                                                    PanelModel = grp.Key.PanelModel,
                                                                    PanelModelName = grp.Min(t => t.PanelModelName),
                                                                    BatteryModel = grp.Key.BatteryModel,
                                                                    BatteryModelName = grp.Min(j => j.BatteryModelName),
                                                                    PackageOrItemQuantity = grp.Sum(t => t.PackageOrItemQuantity)


                                                                }).ToList();


            TempData["ListDistributionSheetGridForPackageOrItemCode"] = lstdistributionSheetGridForPackageOrItemCode;

            return View(lstSHSDistributionPlan_RootWiseLocationNPackage);
        }

        [HttpPost]
        public ActionResult EditSHSDistributionPlan(FormCollection fCollection)
        {



            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForInventory, "EditSHSDistributionPlan", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForInventory.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.RouteCategory = inventoryDal.ReadRootCategory().Select(m => new { m.RouteCategory }).Distinct().ToList();
            ViewBag.PackageCapacity = salesDal.ReadPackageCapacity();
            ViewBag.PackageLight = salesDal.ReadLightByPackageCapacity("0020WP");
            ViewBag.Package = salesDal.ReadPackagesForSHSDP("0020WP", "02LIGHT");
            ViewBag.PanelModel = inventoryDal.ReadPanelModelListForSHSDP("SHS221").Select(p => new { p.Description, p.ItemModelID }).Distinct().ToList();
            ViewBag.BatteryModel = inventoryDal.ReadBatteryModelListForSHSDP("SHS221").Select(b => new { b.Description, b.ItemModelID }).Distinct().ToList();
            ViewBag.SHSDelivaryScheduleNo = "RSF/SCH-2015-" + objLoginHelper.CurrentDate.ToString("MMdd");
            // start  for SHSDelivaryScheduleNo with save button
            //distribScheduleNo,scheduleDate,refScheduleNo
            ViewBag.prmScheduleDate = objLoginHelper.CurrentDate.ToString("yyyy-MM-dd");
            ViewBag.prmRefScheduleNo = ViewBag.SHSDelivaryScheduleNo;
            ViewBag.ChkDistribScheduleNo = inventoryDal.CheckDuplicateDistributionPlan(Helper.DateFormatYYMMDD(Convert.ToDateTime(ViewBag.prmScheduleDate)), Convert.ToDateTime(ViewBag.prmScheduleDate), ViewBag.prmRefScheduleNo);



            // SHSDistributionPlan_Master objSHSDistributionPlan_Master = new SHSDistributionPlan_Master();



            string distributionScheduleNo = fCollection["txtDistributionScheduleNo"].ToString();

            string distributionScheduleNoYear = distributionScheduleNo.Substring(0, 2);
            string distributionScheduleNoMonth = distributionScheduleNo.Substring(2, 2);
            string distributionScheduleNoDay = distributionScheduleNo.Substring(4, 2);
            string monthSet = "";

            switch (distributionScheduleNoMonth)
            {

                case "01":
                    monthSet = "Jan";
                    break;
                case "02":
                    monthSet = "Feb";
                    break;
                case "03":
                    monthSet = "Mar";
                    break;
                case "04":
                    monthSet = "Apr";
                    break;
                case "05":
                    monthSet = "May";
                    break;
                case "06":
                    monthSet = "Jun";
                    break;
                case "07":
                    monthSet = "Jul";
                    break;
                case "08":
                    monthSet = "Aug";
                    break;
                case "09":
                    monthSet = "Sep";
                    break;
                case "10":
                    monthSet = "Oct";
                    break;
                case "11":
                    monthSet = "Nov";
                    break;
                case "12":
                    monthSet = "Dec";
                    break;
                default:
                    break;
            }

            ViewBag.SHSDelivaryScheduleNo = "RSF/SCH-2015-" + distributionScheduleNoMonth + distributionScheduleNoDay; //RSF/SCH-2014-0914
            ViewBag.StartDate = Convert.ToDateTime(distributionScheduleNoDay + "-" + monthSet + "-" + "2015"); //14-Sep-2014

            ViewBag.DistributionScheduleNo = distributionScheduleNo;

            int countDistributionPlan = 0; //inventoryDal.CheckDuplicateDistributionPlan(distribScheduleNo, scheduleDate, refScheduleNo);

            List<SHSDistributionPlanPackageORItem> lstSHSDistributionPlan_RootWiseLocationNPackage = new List<SHSDistributionPlanPackageORItem>();
            lstSHSDistributionPlan_RootWiseLocationNPackage = inventoryDal.RootWiseLocationNPackage(distributionScheduleNo);

            if (lstSHSDistributionPlan_RootWiseLocationNPackage.Count == 0)
            {
                ViewBag.dataNotFound = "0";
            }

            else if (lstSHSDistributionPlan_RootWiseLocationNPackage.Count>0)
            {
                ViewBag.dataNotFound = "1";
            }


            List<SHSDistributionPlanPackageORItem> lstLocationSHSDistributionPlanPackageORItem = new List<SHSDistributionPlanPackageORItem>();
            lstLocationSHSDistributionPlanPackageORItem = (from t in lstSHSDistributionPlan_RootWiseLocationNPackage
                                                           group t by new { t.LocationCode }
                                                               into grp
                                                               select new SHSDistributionPlanPackageORItem
                                                               {
                                                                   RouteNo = grp.Min(K => K.RouteNo),
                                                                   RouteName = grp.Min(i => i.RouteName),
                                                                   RouteCategory = grp.Min(j => j.RouteCategory),
                                                                   LocationCode = grp.Key.LocationCode,
                                                                   LocationName = grp.Min(m => m.LocationName)

                                                               }).ToList();

            TempData["ListLocationSHSDistributionPlanPackageORItem"] = lstLocationSHSDistributionPlanPackageORItem;



            List<SHSDistributionPlanPackageORItem> lstRouteSHSDistributionPlanPackageORItem = new List<SHSDistributionPlanPackageORItem>();
            lstRouteSHSDistributionPlanPackageORItem = (from t in lstSHSDistributionPlan_RootWiseLocationNPackage
                                                        group t by new { t.RouteNo }
                                                            into grp
                                                            select new SHSDistributionPlanPackageORItem
                                                            {
                                                                RouteNo = grp.Key.RouteNo,
                                                                RouteName = grp.Min(i => i.RouteName),
                                                                RouteCategory = grp.Min(j => j.RouteCategory)

                                                            }).ToList();

            TempData["ListRouteSHSDistributionPlanPackageORItem"] = lstRouteSHSDistributionPlanPackageORItem;

            //var grouped =
            //from it in lstSHSDistributionPlan_RootWiseLocationNPackage
            //group it by new { it.RouteNo, it.LocationCode };


            List<SHSDistributionPlanPackageORItem> lstSHSDistributionPlan_RootWiseLocationNPackage1 = new List<SHSDistributionPlanPackageORItem>();

            // lstSHSDistributionPlan_RootWiseLocationNPackage1 = grouped.ToList();

            //lstSHSDistributionPlan_RootWiseLocationNPackage1 =
            //lstSHSDistributionPlan_RootWiseLocationNPackage.GroupBy(x => new { RouteNo = x.RouteNo, PackageOrItemCode = x.PackageOrItemCode, PanelModel = x.PanelModel, BatteryModel=x.BatteryModel })
            //.Select(y => new SHSDistributionPlanPackageORItem
            //{
            //    RouteNo = y.Key.RouteNo,
            //    PackageOrItemCode = y.Key.PackageOrItemCode,
            //    PanelModel=y.Key.PanelModel,
            //    BatteryModel=y.Key.BatteryModel
            //}).ToList();


            lstSHSDistributionPlan_RootWiseLocationNPackage1 = (from t in lstSHSDistributionPlan_RootWiseLocationNPackage
                                                                group t by new { t.RouteNo, t.PackageOrItemCode, t.PanelModel, t.BatteryModel }
                                                                    into grp
                                                                    select new SHSDistributionPlanPackageORItem
                                                                    {


                                                                        RouteNo = grp.Key.RouteNo,
                                                                        Category = grp.Min(i => i.Category),
                                                                        PackageOrItemCode = grp.Key.PackageOrItemCode,
                                                                        PackageName = grp.Min(t => t.PackageName),
                                                                        PanelModel = grp.Key.PanelModel,
                                                                        PanelModelName = grp.Min(t => t.PanelModelName),
                                                                        BatteryModel = grp.Key.BatteryModel,
                                                                        BatteryModelName = grp.Min(j => j.BatteryModelName),
                                                                        PackageOrItemQuantity = grp.Sum(t => t.PackageOrItemQuantity)


                                                                    }).ToList();


            TempData["ListSHSDistributionPlanPackageORItem"] = lstSHSDistributionPlan_RootWiseLocationNPackage1;


            //var query = (from t in Transactions
            //             group t by new { t.MaterialID, t.ProductID }
            //                 into grp
            //                 select new
            //                 {
            //                     grp.Key.MaterialID,
            //                     grp.Key.ProductID,
            //                     Quantity = grp.Sum(t => t.Quantity)
            //                 }).ToList();


            //.Select(r => new { RouteNo = r.Key.RouteNo, LocationCode = r.Key.LocationCode }).ToList();

            //.Select(y => new SHSDistributionPlanPackageORItem
            //{

            //    RouteNo = y.Key
            //    , LocationCode = y.SelectMany(x => x.LocationCode).ToString()

            //}).ToList();


            // .GroupBy(r => new { Problem = r.Problem, Year = r.CreatedDate.Year, Quarter = ((r.CreatedDate.Month) / 3) })
            //.Select(r => new { Problem = r.Key.Problem, Year = r.Key.Year, Quarter = r.Key.Quarter, Count = r.Count() })

            /* List<Address> listOfFilteredAddresses =
                 listOfAddresses
                 .GroupBy(x => x.AddressId)
                 .Select(y => new Address
                 {
                     AddressId = y.Key
                     ,
                     NodeIds = y.SelectMany(x => x.NodeIds).ToList()
                 });
             return View(booksGrouped.ToList());*/

            List<SHSDistributionPlanPackageORItem> lstdistributionSheetGridForPackageOrItemCode = new List<SHSDistributionPlanPackageORItem>();

            lstdistributionSheetGridForPackageOrItemCode = (from t in lstSHSDistributionPlan_RootWiseLocationNPackage
                                                            group t by new { t.PackageOrItemCode, t.PanelModel, t.BatteryModel }
                                                                into grp
                                                                select new SHSDistributionPlanPackageORItem
                                                                {
                                                                    Category = grp.Min(i => i.Category),
                                                                    PackageOrItemCode = grp.Key.PackageOrItemCode,
                                                                    PackageName = grp.Min(t => t.PackageName),
                                                                    PanelModel = grp.Key.PanelModel,
                                                                    PanelModelName = grp.Min(t => t.PanelModelName),
                                                                    BatteryModel = grp.Key.BatteryModel,
                                                                    BatteryModelName = grp.Min(j => j.BatteryModelName),
                                                                    PackageOrItemQuantity = grp.Sum(t => t.PackageOrItemQuantity)


                                                                }).ToList();


            TempData["ListDistributionSheetGridForPackageOrItemCode"] = lstdistributionSheetGridForPackageOrItemCode;


            return View(lstSHSDistributionPlan_RootWiseLocationNPackage);

        }

        public ActionResult SHSDistributionScheduleOrRouteTransfer() 
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForInventory, "SHSDistributionScheduleOrRouteTransfer", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForInventory.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.RouteCategory = inventoryDal.ReadRootCategory().Select(m => new { m.RouteCategory }).Distinct().ToList();
            ViewBag.PackageCapacity = salesDal.ReadPackageCapacity();
            ViewBag.PackageLight = salesDal.ReadLightByPackageCapacity("0020WP");
            ViewBag.Package = salesDal.ReadPackagesForSHSDP("0020WP", "02LIGHT");
            ViewBag.PanelModel = inventoryDal.ReadPanelModelListForSHSDP("SHS221").Select(p => new { p.Description, p.ItemModelID }).Distinct().ToList();
            ViewBag.BatteryModel = inventoryDal.ReadBatteryModelListForSHSDP("SHS221").Select(b => new { b.Description, b.ItemModelID }).Distinct().ToList();
            ViewBag.SHSDelivaryScheduleNo = "RSF/SCH-2015-" + objLoginHelper.CurrentDate.ToString("MMdd");
            ViewBag.StartDate = Convert.ToDateTime(objLoginHelper.CurrentDate.ToString("dd-MMM-yyyy"));
            // start  for SHSDelivaryScheduleNo with save button
            //distribScheduleNo,scheduleDate,refScheduleNo
            ViewBag.prmScheduleDate = objLoginHelper.CurrentDate.ToString("yyyy-MM-dd");
            ViewBag.prmRefScheduleNo = ViewBag.SHSDelivaryScheduleNo;
            ViewBag.ChkDistribScheduleNo = inventoryDal.CheckDuplicateDistributionPlan(Helper.DateFormatYYMMDD(Convert.ToDateTime(ViewBag.prmScheduleDate)), Convert.ToDateTime(ViewBag.prmScheduleDate), ViewBag.prmRefScheduleNo);
            // end  for SHSDelivaryScheduleNo with save button

            ViewBag.dataNotFound = "FirstLoadPage";

            List<SHSDistributionPlanPackageORItem> lstSHSDistributionPlan_RootWiseLocationNPackage = new List<SHSDistributionPlanPackageORItem>();
            lstSHSDistributionPlan_RootWiseLocationNPackage = inventoryDal.RootWiseLocationNPackage("");

            List<SHSDistributionPlanPackageORItem> lstLocationSHSDistributionPlanPackageORItem = new List<SHSDistributionPlanPackageORItem>();
            lstLocationSHSDistributionPlanPackageORItem = (from t in lstSHSDistributionPlan_RootWiseLocationNPackage
                                                           group t by new { t.LocationCode }
                                                               into grp
                                                               select new SHSDistributionPlanPackageORItem
                                                               {
                                                                   RouteNo = grp.Min(K => K.RouteNo),
                                                                   RouteName = grp.Min(i => i.RouteName),
                                                                   RouteCategory = grp.Min(j => j.RouteCategory),
                                                                   LocationCode = grp.Key.LocationCode,
                                                                   LocationName = grp.Min(m => m.LocationName)

                                                               }).ToList();

            TempData["ListLocationSHSDistributionPlanPackageORItem"] = lstLocationSHSDistributionPlanPackageORItem;


            List<SHSDistributionPlanPackageORItem> lstRouteSHSDistributionPlanPackageORItem = new List<SHSDistributionPlanPackageORItem>();
            lstRouteSHSDistributionPlanPackageORItem = (from t in lstSHSDistributionPlan_RootWiseLocationNPackage
                                                        group t by new { t.RouteNo }
                                                            into grp
                                                            select new SHSDistributionPlanPackageORItem
                                                            {
                                                                RouteNo = grp.Key.RouteNo,
                                                                RouteName = grp.Min(i => i.RouteName),
                                                                RouteCategory = grp.Min(j => j.RouteCategory)

                                                            }).ToList();

            TempData["ListRouteSHSDistributionPlanPackageORItem"] = lstRouteSHSDistributionPlanPackageORItem;


            List<SHSDistributionPlanPackageORItem> lstSHSDistributionPlan_RootWiseLocationNPackage1 = new List<SHSDistributionPlanPackageORItem>();
            lstSHSDistributionPlan_RootWiseLocationNPackage1 = (from t in lstSHSDistributionPlan_RootWiseLocationNPackage
                                                                group t by new { t.RouteNo, t.PackageOrItemCode, t.PanelModel, t.BatteryModel }
                                                                    into grp
                                                                    select new SHSDistributionPlanPackageORItem
                                                                    {


                                                                        RouteNo = grp.Key.RouteNo,
                                                                        Category = grp.Min(i => i.Category),
                                                                        PackageOrItemCode = grp.Key.PackageOrItemCode,
                                                                        PackageName = grp.Min(t => t.PackageName),
                                                                        PanelModel = grp.Key.PanelModel,
                                                                        PanelModelName = grp.Min(t => t.PanelModelName),
                                                                        BatteryModel = grp.Key.BatteryModel,
                                                                        BatteryModelName = grp.Min(j => j.BatteryModelName),
                                                                        PackageOrItemQuantity = grp.Sum(t => t.PackageOrItemQuantity)


                                                                    }).ToList();


            TempData["ListSHSDistributionPlanPackageORItem"] = lstSHSDistributionPlan_RootWiseLocationNPackage1;


            List<SHSDistributionPlanPackageORItem> lstdistributionSheetGridForPackageOrItemCode = new List<SHSDistributionPlanPackageORItem>();
            lstdistributionSheetGridForPackageOrItemCode = (from t in lstSHSDistributionPlan_RootWiseLocationNPackage
                                                            group t by new { t.PackageOrItemCode, t.PanelModel, t.BatteryModel }
                                                                into grp
                                                                select new SHSDistributionPlanPackageORItem
                                                                {
                                                                    Category = grp.Min(i => i.Category),
                                                                    PackageOrItemCode = grp.Key.PackageOrItemCode,
                                                                    PackageName = grp.Min(t => t.PackageName),
                                                                    PanelModel = grp.Key.PanelModel,
                                                                    PanelModelName = grp.Min(t => t.PanelModelName),
                                                                    BatteryModel = grp.Key.BatteryModel,
                                                                    BatteryModelName = grp.Min(j => j.BatteryModelName),
                                                                    PackageOrItemQuantity = grp.Sum(t => t.PackageOrItemQuantity)


                                                                }).ToList();


            TempData["ListDistributionSheetGridForPackageOrItemCode"] = lstdistributionSheetGridForPackageOrItemCode;

            return View(lstSHSDistributionPlan_RootWiseLocationNPackage);
        }



        [HttpPost]
        public ActionResult SHSDistributionScheduleOrRouteTransfer(FormCollection fCollection)
        {



            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForInventory, "SHSDistributionScheduleOrRouteTransfer", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForInventory.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.RouteCategory = inventoryDal.ReadRootCategory().Select(m => new { m.RouteCategory }).Distinct().ToList();
            ViewBag.PackageCapacity = salesDal.ReadPackageCapacity();
            ViewBag.PackageLight = salesDal.ReadLightByPackageCapacity("0020WP");
            ViewBag.Package = salesDal.ReadPackagesForSHSDP("0020WP", "02LIGHT");
            ViewBag.PanelModel = inventoryDal.ReadPanelModelListForSHSDP("SHS221").Select(p => new { p.Description, p.ItemModelID }).Distinct().ToList();
            ViewBag.BatteryModel = inventoryDal.ReadBatteryModelListForSHSDP("SHS221").Select(b => new { b.Description, b.ItemModelID }).Distinct().ToList();
            ViewBag.SHSDelivaryScheduleNo = "RSF/SCH-2015-" + objLoginHelper.CurrentDate.ToString("MMdd");
            // start  for SHSDelivaryScheduleNo with save button
            //distribScheduleNo,scheduleDate,refScheduleNo
            ViewBag.prmScheduleDate = objLoginHelper.CurrentDate.ToString("yyyy-MM-dd");
            ViewBag.prmRefScheduleNo = ViewBag.SHSDelivaryScheduleNo;
            ViewBag.ChkDistribScheduleNo = inventoryDal.CheckDuplicateDistributionPlan(Helper.DateFormatYYMMDD(Convert.ToDateTime(ViewBag.prmScheduleDate)), Convert.ToDateTime(ViewBag.prmScheduleDate), ViewBag.prmRefScheduleNo);



            // SHSDistributionPlan_Master objSHSDistributionPlan_Master = new SHSDistributionPlan_Master();



            string distributionScheduleNo = fCollection["txtDistributionScheduleNo"].ToString();

            string distributionScheduleNoYear = distributionScheduleNo.Substring(0, 2);
            string distributionScheduleNoMonth = distributionScheduleNo.Substring(2, 2);
            string distributionScheduleNoDay = distributionScheduleNo.Substring(4, 2);
            string monthSet = "";

            switch (distributionScheduleNoMonth)
            {

                case "01":
                    monthSet = "Jan";
                    break;
                case "02":
                    monthSet = "Feb";
                    break;
                case "03":
                    monthSet = "Mar";
                    break;
                case "04":
                    monthSet = "Apr";
                    break;
                case "05":
                    monthSet = "May";
                    break;
                case "06":
                    monthSet = "Jun";
                    break;
                case "07":
                    monthSet = "Jul";
                    break;
                case "08":
                    monthSet = "Aug";
                    break;
                case "09":
                    monthSet = "Sep";
                    break;
                case "10":
                    monthSet = "Oct";
                    break;
                case "11":
                    monthSet = "Nov";
                    break;
                case "12":
                    monthSet = "Dec";
                    break;
                default:
                    break;
            }

            ViewBag.SHSDelivaryScheduleNo = "RSF/SCH-2015-" + distributionScheduleNoMonth + distributionScheduleNoDay; //RSF/SCH-2014-0914
            ViewBag.StartDate = Convert.ToDateTime(distributionScheduleNoDay + "-" + monthSet + "-" + "2015"); //14-Sep-2014

            ViewBag.DistributionScheduleNo = distributionScheduleNo;

            int countDistributionPlan = 0; //inventoryDal.CheckDuplicateDistributionPlan(distribScheduleNo, scheduleDate, refScheduleNo);

            List<SHSDistributionPlanPackageORItem> lstSHSDistributionPlan_RootWiseLocationNPackage = new List<SHSDistributionPlanPackageORItem>();
            lstSHSDistributionPlan_RootWiseLocationNPackage = inventoryDal.RootWiseLocationNPackage(distributionScheduleNo);

            if (lstSHSDistributionPlan_RootWiseLocationNPackage.Count == 0)
            {
                ViewBag.dataNotFound = "0";
            }

            else if (lstSHSDistributionPlan_RootWiseLocationNPackage.Count > 0)
            {
                ViewBag.dataNotFound = "1";
            }


            List<SHSDistributionPlanPackageORItem> lstLocationSHSDistributionPlanPackageORItem = new List<SHSDistributionPlanPackageORItem>();
            lstLocationSHSDistributionPlanPackageORItem = (from t in lstSHSDistributionPlan_RootWiseLocationNPackage
                                                           group t by new { t.LocationCode }
                                                               into grp
                                                               select new SHSDistributionPlanPackageORItem
                                                               {
                                                                   RouteNo = grp.Min(K => K.RouteNo),
                                                                   RouteName = grp.Min(i => i.RouteName),
                                                                   RouteCategory = grp.Min(j => j.RouteCategory),
                                                                   LocationCode = grp.Key.LocationCode,
                                                                   LocationName = grp.Min(m => m.LocationName)

                                                               }).ToList();

            TempData["ListLocationSHSDistributionPlanPackageORItem"] = lstLocationSHSDistributionPlanPackageORItem;



            List<SHSDistributionPlanPackageORItem> lstRouteSHSDistributionPlanPackageORItem = new List<SHSDistributionPlanPackageORItem>();
            lstRouteSHSDistributionPlanPackageORItem = (from t in lstSHSDistributionPlan_RootWiseLocationNPackage
                                                        group t by new { t.RouteNo }
                                                            into grp
                                                            select new SHSDistributionPlanPackageORItem
                                                            {
                                                                RouteNo = grp.Key.RouteNo,
                                                                RouteName = grp.Min(i => i.RouteName),
                                                                RouteCategory = grp.Min(j => j.RouteCategory)

                                                            }).ToList();

            TempData["ListRouteSHSDistributionPlanPackageORItem"] = lstRouteSHSDistributionPlanPackageORItem;

            //var grouped =
            //from it in lstSHSDistributionPlan_RootWiseLocationNPackage
            //group it by new { it.RouteNo, it.LocationCode };


            List<SHSDistributionPlanPackageORItem> lstSHSDistributionPlan_RootWiseLocationNPackage1 = new List<SHSDistributionPlanPackageORItem>();

            // lstSHSDistributionPlan_RootWiseLocationNPackage1 = grouped.ToList();

            //lstSHSDistributionPlan_RootWiseLocationNPackage1 =
            //lstSHSDistributionPlan_RootWiseLocationNPackage.GroupBy(x => new { RouteNo = x.RouteNo, PackageOrItemCode = x.PackageOrItemCode, PanelModel = x.PanelModel, BatteryModel=x.BatteryModel })
            //.Select(y => new SHSDistributionPlanPackageORItem
            //{
            //    RouteNo = y.Key.RouteNo,
            //    PackageOrItemCode = y.Key.PackageOrItemCode,
            //    PanelModel=y.Key.PanelModel,
            //    BatteryModel=y.Key.BatteryModel
            //}).ToList();


            lstSHSDistributionPlan_RootWiseLocationNPackage1 = (from t in lstSHSDistributionPlan_RootWiseLocationNPackage
                                                                group t by new { t.RouteNo, t.PackageOrItemCode, t.PanelModel, t.BatteryModel }
                                                                    into grp
                                                                    select new SHSDistributionPlanPackageORItem
                                                                    {


                                                                        RouteNo = grp.Key.RouteNo,
                                                                        Category = grp.Min(i => i.Category),
                                                                        PackageOrItemCode = grp.Key.PackageOrItemCode,
                                                                        PackageName = grp.Min(t => t.PackageName),
                                                                        PanelModel = grp.Key.PanelModel,
                                                                        PanelModelName = grp.Min(t => t.PanelModelName),
                                                                        BatteryModel = grp.Key.BatteryModel,
                                                                        BatteryModelName = grp.Min(j => j.BatteryModelName),
                                                                        PackageOrItemQuantity = grp.Sum(t => t.PackageOrItemQuantity)


                                                                    }).ToList();


            TempData["ListSHSDistributionPlanPackageORItem"] = lstSHSDistributionPlan_RootWiseLocationNPackage1;


            //var query = (from t in Transactions
            //             group t by new { t.MaterialID, t.ProductID }
            //                 into grp
            //                 select new
            //                 {
            //                     grp.Key.MaterialID,
            //                     grp.Key.ProductID,
            //                     Quantity = grp.Sum(t => t.Quantity)
            //                 }).ToList();


            //.Select(r => new { RouteNo = r.Key.RouteNo, LocationCode = r.Key.LocationCode }).ToList();

            //.Select(y => new SHSDistributionPlanPackageORItem
            //{

            //    RouteNo = y.Key
            //    , LocationCode = y.SelectMany(x => x.LocationCode).ToString()

            //}).ToList();


            // .GroupBy(r => new { Problem = r.Problem, Year = r.CreatedDate.Year, Quarter = ((r.CreatedDate.Month) / 3) })
            //.Select(r => new { Problem = r.Key.Problem, Year = r.Key.Year, Quarter = r.Key.Quarter, Count = r.Count() })

            /* List<Address> listOfFilteredAddresses =
                 listOfAddresses
                 .GroupBy(x => x.AddressId)
                 .Select(y => new Address
                 {
                     AddressId = y.Key
                     ,
                     NodeIds = y.SelectMany(x => x.NodeIds).ToList()
                 });
             return View(booksGrouped.ToList());*/

            List<SHSDistributionPlanPackageORItem> lstdistributionSheetGridForPackageOrItemCode = new List<SHSDistributionPlanPackageORItem>();

            lstdistributionSheetGridForPackageOrItemCode = (from t in lstSHSDistributionPlan_RootWiseLocationNPackage
                                                            group t by new { t.PackageOrItemCode, t.PanelModel, t.BatteryModel }
                                                                into grp
                                                                select new SHSDistributionPlanPackageORItem
                                                                {
                                                                    Category = grp.Min(i => i.Category),
                                                                    PackageOrItemCode = grp.Key.PackageOrItemCode,
                                                                    PackageName = grp.Min(t => t.PackageName),
                                                                    PanelModel = grp.Key.PanelModel,
                                                                    PanelModelName = grp.Min(t => t.PanelModelName),
                                                                    BatteryModel = grp.Key.BatteryModel,
                                                                    BatteryModelName = grp.Min(j => j.BatteryModelName),
                                                                    PackageOrItemQuantity = grp.Sum(t => t.PackageOrItemQuantity)


                                                                }).ToList();


            TempData["ListDistributionSheetGridForPackageOrItemCode"] = lstdistributionSheetGridForPackageOrItemCode;


            return View(lstSHSDistributionPlan_RootWiseLocationNPackage);

        }

        public JsonResult RouteList(string routeCategory)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            ArrayList arr = new ArrayList();

            List<Inv_RouteMaster> lstRootMaster = new List<Inv_RouteMaster>();
            lstRootMaster = inventoryDal.ReadRouteMaster(routeCategory);

            foreach (Inv_RouteMaster objRootMaster in lstRootMaster)
            {
                arr.Add(new { Display = (objRootMaster.RouteNo + "-" + objRootMaster.RouteName), Value = objRootMaster.RouteNo });
            }

            return new JsonResult { Data = arr };
        }

        public JsonResult PanelModelList(string packageCode)
        {
            List<Inv_ItemModel> lstPanelModelList = new List<Inv_ItemModel>();
            lstPanelModelList = inventoryDal.ReadPanelModelListForSHSDP(packageCode);

            ArrayList arl = new ArrayList();

            foreach (Inv_ItemModel pml in lstPanelModelList)
            {
                arl.Add(new { Value = pml.ItemModelID.ToString(), Display = pml.Description });
            }

            return new JsonResult { Data = arl };
        }

        public JsonResult BatteryModelList(string packageCode)
        {
            List<Inv_ItemModel> lstPanelBatteryList = new List<Inv_ItemModel>();
            lstPanelBatteryList = inventoryDal.ReadBatteryModelListForSHSDP(packageCode);

            ArrayList arl = new ArrayList();

            foreach (Inv_ItemModel pml in lstPanelBatteryList)
            {
                arl.Add(new { Value = pml.ItemModelID.ToString(), Display = pml.Description });
            }

            return new JsonResult { Data = arl };
        }

        public ActionResult LocationSelectionPartial(string issueTypeId)
        {
            ViewBag.LocationType = new UserLocationType().UserLocationTypeFormatChallan(issueTypeId);
            return PartialView("UserLocationSelection");
        }


        public ActionResult ItemCodeSelectionPartial(string itemCode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<Inv_ItemMaster> lstItemsDetails = new List<Inv_ItemMaster>();
            lstItemsDetails = inventoryDal.ReadItemMasterForItemCode(itemCode);

            ViewBag.ItemCode = lstItemsDetails;

            return PartialView("ItemDetailSearch", lstItemsDetails);
        }

        public JsonResult LightList(string capacityId)
        {
            List<Sal_Validation_CapacityVsLight> lstLight = new List<Sal_Validation_CapacityVsLight>();
            lstLight = salesDal.ReadLightByPackageCapacity(capacityId);

            return new JsonResult { Data = lstLight };
        }

        public JsonResult PackgeList(string capacityId, string lightId, int check)
        {

            if (check == 0)
            {
                List<Sal_Validation_CapacityVsLight> lstLight = new List<Sal_Validation_CapacityVsLight>();
                lstLight = salesDal.ReadLightByPackageCapacity(capacityId);

                foreach (Sal_Validation_CapacityVsLight objCapacityVsLight in lstLight)
                {
                  
                        lightId = objCapacityVsLight.LightID;
                        break;
                }
            }


            List<Sal_PackageMaster> lstPackage = new List<Sal_PackageMaster>();
            lstPackage = salesDal.ReadPackagesForSHSDP(capacityId, lightId);

            ArrayList arl = new ArrayList();

            foreach (Sal_PackageMaster pkg in lstPackage)
            {
                arl.Add(new { Value = pkg.PackageCode.ToString(), Display = pkg.Description });
            }

            return new JsonResult { Data = arl };
        }



        public JsonResult MaterialReceivingMrrLocationList(string deliveryNoteNo)
        {
            ArrayList arr = new ArrayList();
            arr = inventoryDal.ReadVendorChallanWithItemReferenceToCheck(deliveryNoteNo);
            return new JsonResult { Data = arr };
        }

        public JsonResult MrrNoList(string deliveryNote, string locationCode)
        {
            Array arr = inventoryDal.MrrSequenceListForMaterialReceiving(deliveryNote, locationCode);
            return new JsonResult { Data = arr };
        }

        [GridAction]
        public ActionResult __LocationDetailsList(string routeId)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];


            return View(new GridModel<LocationInfo> { Data = inventoryDal.ReadUnitList(routeId) });

        }



        //[GridAction]
        //public ActionResult __EmployeeDetailsVisit(string empId)
        //{
        //    objLoginHelper = (LoginHelper)Session["LogInInformation"];

        //    return View(new GridModel<EmployeeVisit> { Data = inventoryDal.ReadEmployeeDetailsVisit(empId, objLoginHelper.LogInForUnitCode) });

        //}

        [GridAction]
        public ActionResult MaterialReceivingReportGrid(string locationCode, string deliveryNoteNo, string mrrSequenceNumber, string billNo)
        {
            List<MaterialRecevingReportMrrDetails> lstMaterialReceiving = new List<MaterialRecevingReportMrrDetails>();
            lstMaterialReceiving = inventoryDal.MaterialReceivingMrrDetails(locationCode, deliveryNoteNo, mrrSequenceNumber, billNo);

            return View(new GridModel<MaterialRecevingReportMrrDetails>
            {
                Data = lstMaterialReceiving
            });
        }
        
        [GridAction]
        public ActionResult DeliveryNoteSummaryGrid(string distributionScheduleNo) 
        {
            List<DeliveryNoteSummary> lstDeliveryNoteSummaryGrid = new List<DeliveryNoteSummary>();
            lstDeliveryNoteSummaryGrid = inventoryDal.ReadDeliveryNoteSummary(distributionScheduleNo);

            return View(new GridModel<DeliveryNoteSummary>
            {
                Data = lstDeliveryNoteSummaryGrid
            });
        }

        [GridAction]
        public ActionResult DeliveryItemNoteReportSummaryGrid(string distributionScheduleNo) 
        {
            List<DeliveryItemNoteReportSummary> lstDeliveryItemNoteReportSummary = new List<DeliveryItemNoteReportSummary>();
            lstDeliveryItemNoteReportSummary = inventoryDal.ReadDeliveryItemNoteReportSummary(distributionScheduleNo);

            return View(new GridModel<DeliveryItemNoteReportSummary>
            {
                Data = lstDeliveryItemNoteReportSummary
            });
        }

        [GridAction]
        public ActionResult DeliveryPackageNoteReportSummaryGrid(string distributionScheduleNo)
        {
            List<DeliveryPackageNoteReportSummary> lstDeliveryPackageNoteReportSummary = new List<DeliveryPackageNoteReportSummary>();
            lstDeliveryPackageNoteReportSummary = inventoryDal.ReadDeliveryPackageNoteReportSummary(distributionScheduleNo);

            return View(new GridModel<DeliveryPackageNoteReportSummary>
            {
                Data = lstDeliveryPackageNoteReportSummary
            });
        }


        //start monthly
        [GridAction]
        public ActionResult MonthlyDeliveryNoteSummaryGrid(string monthYear)
        {
            List<DeliveryNoteSummary> lstMonthlyDeliveryNoteSummaryGrid = new List<DeliveryNoteSummary>();
            lstMonthlyDeliveryNoteSummaryGrid = inventoryDal.ReadMonthlyDeliveryNoteSummary(monthYear);

            return View(new GridModel<DeliveryNoteSummary>
            {
                Data = lstMonthlyDeliveryNoteSummaryGrid
            });
        }

        [GridAction]
        public ActionResult MonthlyDeliveryItemNoteReportSummaryGrid(string monthYear)
        {
            List<DeliveryItemNoteReportSummary> lstMonthlyDeliveryItemNoteReportSummary = new List<DeliveryItemNoteReportSummary>();
            lstMonthlyDeliveryItemNoteReportSummary = inventoryDal.ReadMonthlyDeliveryItemNoteReportSummary(monthYear);

            return View(new GridModel<DeliveryItemNoteReportSummary>
            {
                Data = lstMonthlyDeliveryItemNoteReportSummary
            });
        }

        [GridAction]
        public ActionResult MonthlyDeliveryPackageNoteReportSummaryGrid(string monthYear)
        {
            List<DeliveryPackageNoteReportSummary> lstMonthlyDeliveryPackageNoteReportSummary = new List<DeliveryPackageNoteReportSummary>();
            lstMonthlyDeliveryPackageNoteReportSummary = inventoryDal.ReadMonthlyDeliveryPackageNoteReportSummary(monthYear);

            return View(new GridModel<DeliveryPackageNoteReportSummary>
            {
                Data = lstMonthlyDeliveryPackageNoteReportSummary
            });
        }
        //end monthly

        //start Date wise
        [GridAction]
        public ActionResult DateWiseDeliveryNoteSummaryGrid(string  fromDate, string toDate)
            
        {
            List<DeliveryNoteSummary> lstDateWiseDeliveryNoteSummaryGrid = new List<DeliveryNoteSummary>();
            lstDateWiseDeliveryNoteSummaryGrid = inventoryDal.ReadDateWiseDeliveryNoteSummary(fromDate, toDate);

            return View(new GridModel<DeliveryNoteSummary>
            {
                Data = lstDateWiseDeliveryNoteSummaryGrid
            });
        }

        [GridAction]
        public ActionResult DateWiseDeliveryItemNoteReportSummaryGrid(string fromDate, string toDate)
        {
            List<DeliveryItemNoteReportSummary> lstDateWiseDeliveryItemNoteReportSummary = new List<DeliveryItemNoteReportSummary>();
            lstDateWiseDeliveryItemNoteReportSummary = inventoryDal.ReadDateWiseDeliveryItemNoteReportSummary(fromDate, toDate);

            return View(new GridModel<DeliveryItemNoteReportSummary>
            {
                Data = lstDateWiseDeliveryItemNoteReportSummary
            });
        }

        [GridAction]
        public ActionResult DateWiseDeliveryPackageNoteReportSummaryGrid(string fromDate, string toDate)
        {
            List<DeliveryPackageNoteReportSummary> lstDateWiseDeliveryPackageNoteReportSummary = new List<DeliveryPackageNoteReportSummary>();
            lstDateWiseDeliveryPackageNoteReportSummary = inventoryDal.ReadDateWiseDeliveryPackageNoteReportSummary(fromDate, toDate);

            return View(new GridModel<DeliveryPackageNoteReportSummary>
            {
                Data = lstDateWiseDeliveryPackageNoteReportSummary
            });
        }
        //end Date wise

        public JsonResult MRRNDeliveryNoteValue(string locationCode, string mrrSequenceNumber, string rreDeliveryNote)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            ArrayList arr = new ArrayList();
            arr = inventoryDal.ReadMRRNDeliveryNoteValue(locationCode, mrrSequenceNumber, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForInventory), rreDeliveryNote);
            return new JsonResult { Data = arr };
        }

        public JsonResult MaterialReceivingRREDeliveryNoteList(string billNO)
        {
            ArrayList arr = new ArrayList();
            arr = inventoryDal.ReadVendorRefChallanNo(billNO);
            return new JsonResult { Data = arr };
        }

        public JsonResult CheckDistributionSchedule(DateTime scheduleDate, string refScheduleNo)
        {


            string distribScheduleNo = Helper.DateFormatYYMMDD(Convert.ToDateTime(scheduleDate.ToString()));

            int countDistributionPlan = inventoryDal.CheckDuplicateDistributionPlan(distribScheduleNo, scheduleDate, refScheduleNo);



            return Json(new
            {
                Success = true, //error
                Count = countDistributionPlan  //return exception
            });
        }


        public JsonResult SaveSHSDistributionPlan(List<SHSDistributionPlan> lstUnitWisePackageAmount, Int32 gridLength, string distribScheduleNo, string scheduleDate, string scheduleNotes)
        {
            try
            {
               
                //var lstSHSDistributionPlanAddCheck = from D in lstUnitWisePackageAmount
                //                        where D.LocationCode == null && D.PackageCode==null
                //                       select D;



                int finalSave = 0;

                Int32 countLengt=0;

                try
                {
                    countLengt = (Int32)Session["Count"];
                }

                catch
                {
                    countLengt = 0;
                }


               

                    SHSDistributionPlan objSHSDistributionPlanAdd;
                    foreach (SHSDistributionPlan objSHSAdd in lstUnitWisePackageAmount)
                    {

                        lstSHSDistributionPlanAdd = (List<SHSDistributionPlan>)Session["lstSHSDistributionPlanAdd"];

                        if (lstSHSDistributionPlanAdd == null)
                        {
                            lstSHSDistributionPlanAdd = new List<SHSDistributionPlan>();
                        }

                        objSHSDistributionPlanAdd = new SHSDistributionPlan();

                        objSHSDistributionPlanAdd.RootNo = objSHSAdd.RootNo;
                        objSHSDistributionPlanAdd.LocationCode = objSHSAdd.LocationCode;
                        objSHSDistributionPlanAdd.PackageCode = objSHSAdd.PackageCode;
                        objSHSDistributionPlanAdd.PanelCode = objSHSAdd.PanelCode;
                        objSHSDistributionPlanAdd.BatteryCode = objSHSAdd.BatteryCode;
                        objSHSDistributionPlanAdd.PackageOrItemSelection = objSHSAdd.PackageOrItemSelection;
                        objSHSDistributionPlanAdd.PackageQuantity = objSHSAdd.PackageQuantity;

                        lstSHSDistributionPlanAdd.Add(objSHSDistributionPlanAdd);

                        Session["lstSHSDistributionPlanAdd"] = lstSHSDistributionPlanAdd;

                        Session["Count"] = countLengt + 1;

                        countLengt = (Int32)Session["Count"];

                        if (gridLength == countLengt)
                        {
                
                            objLoginHelper = (LoginHelper)Session["LogInInformation"];

                            lstSHSDistributionPlanAdd = (List<SHSDistributionPlan>)Session["lstSHSDistributionPlanAdd"];

                            SHSDistributionPlan_Master objSHSDistributionPlan_Master = new SHSDistributionPlan_Master();
                            objSHSDistributionPlan_Master.DistribScheduleNo = Helper.DateFormatYYMMDD(Convert.ToDateTime(scheduleDate.ToString()));
                            objSHSDistributionPlan_Master.ScheduleDate = Convert.ToDateTime(scheduleDate.ToString());
                            objSHSDistributionPlan_Master.ScheduleNotes = scheduleNotes.ToString();
                            objSHSDistributionPlan_Master.CreatedBy = objLoginHelper.LogInID;
                            objSHSDistributionPlan_Master.CreatedOn = objLoginHelper.CurrentDate;
                            objSHSDistributionPlan_Master.RefScheduleNo = distribScheduleNo;
                            objSHSDistributionPlan_Master.Status = 0;

                            List<SHSDistributionPlan_RootWiseLocation> lstSHSDistributionPlan_RootWiseLocation = new List<SHSDistributionPlan_RootWiseLocation>();
                            lstSHSDistributionPlan_RootWiseLocation = shsDistributionProcess.SHSDistributionRootWiseLocation(lstSHSDistributionPlanAdd, scheduleDate);
                
                            //SHSDistributionPlan_RootWiseLocationNPackage objSHSDistributionPlan_RootLocationPackage = new SHSDistributionPlan_RootWiseLocationNPackage();

                            List<SHSDistributionPlan_RootWiseLocationNPackage> lstSHSDistributionPlan_RootWiseLocationNPackage = new List<SHSDistributionPlan_RootWiseLocationNPackage>();
                            //lstSHSDistributionPlan_RootLocationPackage = shsDistributionProcess.UnitWisePackageAmountSHSDistributionProcess(lstUnitWisePackageAmount, objLoginHelper);
                            lstSHSDistributionPlan_RootWiseLocationNPackage = shsDistributionProcess.UnitWisePackageAmountSHSDistributionProcess(lstSHSDistributionPlanAdd, scheduleDate);


                            List<SHSDistributionPlan_IndividualItem> lstSHSDistributionPlan_IndividualItem = new List<SHSDistributionPlan_IndividualItem>();
                            lstSHSDistributionPlan_IndividualItem = shsDistributionProcess.UnitWiseItemAmountSHSDistributionProcess(lstSHSDistributionPlanAdd, scheduleDate); 
              
                            // return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty, Helper.AccountSequenceNumberGeneration(objPrePostTransMaster.TransNo)) };

                            Session["Count"] = 0;
                            Session["lstSHSDistributionPlanAdd"] = null;

                            objSHSDistributionPlan_Master = inventoryDal.CreateSHSDistributionPlan(objSHSDistributionPlan_Master, lstSHSDistributionPlan_RootWiseLocation, lstSHSDistributionPlan_RootWiseLocationNPackage, lstSHSDistributionPlan_IndividualItem);

                            finalSave = 1;

                         //   return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty) };


                       }
                   
                }

                if (finalSave == 1)
                {

                    return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty) };
                }

                else
                {
                    return new JsonResult { Data = ExceptionHelper.ExceptionMessageAdd(string.Empty) };
                }
                
            }


            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }

        }

        public JsonResult UpdateSHSDistributionPlan(List<SHSDistributionPlan> lstUnitWisePackageAmount, Int32 gridLength, string distribScheduleNo, string scheduleDate, string scheduleNotes)
        {
            try
            {

                //string sub = distribScheduleNo.Substring(0, 2);
                //string sub1 = distribScheduleNo.Substring(2, 2);
                //string sub2 = distribScheduleNo.Substring(4, 2);
                //string check = "20" + sub + "-" + sub1 + "-" + sub2;
                //DateTime dt = Convert.ToDateTime(check);

                //scheduleDate = dt.ToString("yyyy-MM-dd");



                int finalSave = 0;

                Int32 countLengt = 0;

                try
                {
                    countLengt = (Int32)Session["Count"];
                }

                catch
                {
                    countLengt = 0;
                }




                SHSDistributionPlan objSHSDistributionPlanAdd;
                foreach (SHSDistributionPlan objSHSAdd in lstUnitWisePackageAmount)
                {

                    lstSHSDistributionPlanAdd = (List<SHSDistributionPlan>)Session["lstSHSDistributionPlanAdd"];

                    if (lstSHSDistributionPlanAdd == null)
                    {
                        lstSHSDistributionPlanAdd = new List<SHSDistributionPlan>();
                    }

                    objSHSDistributionPlanAdd = new SHSDistributionPlan();

                    objSHSDistributionPlanAdd.RootNo = objSHSAdd.RootNo;
                    objSHSDistributionPlanAdd.LocationCode = objSHSAdd.LocationCode;
                    objSHSDistributionPlanAdd.PackageCode = objSHSAdd.PackageCode;
                    objSHSDistributionPlanAdd.PanelCode = objSHSAdd.PanelCode;
                    objSHSDistributionPlanAdd.BatteryCode = objSHSAdd.BatteryCode;
                    objSHSDistributionPlanAdd.PackageOrItemSelection = objSHSAdd.PackageOrItemSelection;
                    objSHSDistributionPlanAdd.PackageQuantity = objSHSAdd.PackageQuantity;

                    lstSHSDistributionPlanAdd.Add(objSHSDistributionPlanAdd);

                    Session["lstSHSDistributionPlanAdd"] = lstSHSDistributionPlanAdd;

                    Session["Count"] = countLengt + 1;

                    countLengt = (Int32)Session["Count"];

                    if (gridLength == countLengt)
                    {

                        objLoginHelper = (LoginHelper)Session["LogInInformation"];

                        lstSHSDistributionPlanAdd = (List<SHSDistributionPlan>)Session["lstSHSDistributionPlanAdd"];

                        SHSDistributionPlan_Master objSHSDistributionPlan_Master = new SHSDistributionPlan_Master();
                        objSHSDistributionPlan_Master.DistribScheduleNo = Helper.DateFormatYYMMDD(Convert.ToDateTime(scheduleDate.ToString()));
                        objSHSDistributionPlan_Master.ScheduleDate = Convert.ToDateTime(scheduleDate.ToString());
                        objSHSDistributionPlan_Master.ScheduleNotes = scheduleNotes.ToString();
                        objSHSDistributionPlan_Master.CreatedBy = objLoginHelper.LogInID;
                        objSHSDistributionPlan_Master.CreatedOn = objLoginHelper.CurrentDate;
                        objSHSDistributionPlan_Master.RefScheduleNo = distribScheduleNo;
                        objSHSDistributionPlan_Master.Status = 0;

                        List<SHSDistributionPlan_RootWiseLocation> lstSHSDistributionPlan_RootWiseLocation = new List<SHSDistributionPlan_RootWiseLocation>();
                        lstSHSDistributionPlan_RootWiseLocation = shsDistributionProcess.SHSDistributionRootWiseLocation(lstSHSDistributionPlanAdd, scheduleDate);

                        //SHSDistributionPlan_RootWiseLocationNPackage objSHSDistributionPlan_RootLocationPackage = new SHSDistributionPlan_RootWiseLocationNPackage();

                        List<SHSDistributionPlan_RootWiseLocationNPackage> lstSHSDistributionPlan_RootWiseLocationNPackage = new List<SHSDistributionPlan_RootWiseLocationNPackage>();
                        //lstSHSDistributionPlan_RootLocationPackage = shsDistributionProcess.UnitWisePackageAmountSHSDistributionProcess(lstUnitWisePackageAmount, objLoginHelper);
                        lstSHSDistributionPlan_RootWiseLocationNPackage = shsDistributionProcess.UnitWisePackageAmountSHSDistributionProcess(lstSHSDistributionPlanAdd, scheduleDate);


                        List<SHSDistributionPlan_IndividualItem> lstSHSDistributionPlan_IndividualItem = new List<SHSDistributionPlan_IndividualItem>();
                        lstSHSDistributionPlan_IndividualItem = shsDistributionProcess.UnitWiseItemAmountSHSDistributionProcess(lstSHSDistributionPlanAdd, scheduleDate);

                        // return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty, Helper.AccountSequenceNumberGeneration(objPrePostTransMaster.TransNo)) };

                        Session["Count"] = 0;
                        Session["lstSHSDistributionPlanAdd"] = null;

                        objSHSDistributionPlan_Master = inventoryDal.UpdateSHSDistributionPlan(objSHSDistributionPlan_Master, lstSHSDistributionPlan_RootWiseLocation, lstSHSDistributionPlan_RootWiseLocationNPackage, lstSHSDistributionPlan_IndividualItem);

                        finalSave = 1;

                        //   return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty) };


                    }

                }

                if (finalSave == 1)
                {

                    return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty) };
                }

                else
                {
                    return new JsonResult { Data = ExceptionHelper.ExceptionMessageAdd(string.Empty) };
                }

            }


            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }

        }


        public JsonResult SaveSHSDistributionRouteTransfer(string routeid, string txtDistributionScheduleNo, string  txtDistributionScheduleNoNew, string dtpDelivaryDate, string txtDeliveryScheduleNo)
        { 
            string successMessage = string.Empty;
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            DateTime scheduleDate1=Convert.ToDateTime(dtpDelivaryDate.ToString());

            string scheduleDate = scheduleDate1.ToString("yyyy-MM-dd"); 


            try
            {
                successMessage = inventoryDal.SaveSHSDistributionRouteTransfer(routeid, txtDistributionScheduleNo, txtDistributionScheduleNoNew, scheduleDate, txtDeliveryScheduleNo);

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty) };
            }

            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        
        }
        public JsonResult SavePhysicalBalance(List<Inv_ERPVersusPhysicalBalance> lstInvERPVersusPhysicalBalance)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            
            Inv_ERPVersusPhysicalBalance objInvERPVersusPhysicalBalance = new Inv_ERPVersusPhysicalBalance();
            objInvERPVersusPhysicalBalance.ModifiedBy = objLoginHelper.LogInID;
            objInvERPVersusPhysicalBalance.ModifiedOn = objLoginHelper.CurrentDate;

            objInvERPVersusPhysicalBalance = inventoryDal.Update(objInvERPVersusPhysicalBalance, lstInvERPVersusPhysicalBalance);
      
            return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty) };
           
        }

    }
}
