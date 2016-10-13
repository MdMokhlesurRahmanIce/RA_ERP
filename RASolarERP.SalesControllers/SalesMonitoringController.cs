using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using RASolarERP.Model;
using RASolarHelper;
using RASolarSecurity.Model;
using RASolarERP.Web.Areas.Sales.Models;
using RASolarERP.Web.Models;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.UI;
using System.Collections;
using RASolarERP.DomainModel.InventoryModel;
using RASolarERP.Web.Areas.Inventory.Models;
using RASolarERP.Web.Areas.HRMS.Models;
using RASolarHRMS.Model;
using RASolarERP.DomainModel.SalesModel;
using RASolarERP.DomainModel.HRMSModel;


namespace RASolarERP.Web.Areas.Sales.Controllers
{
    public class SalesMonitoringController : Controller
    {
        LoginHelper objLoginHelper = new LoginHelper();
        private SalesData salesDal = new SalesData();
        private SalesReportData salesReportDal = new SalesReportData();
        InventoryData inventoryDal = new InventoryData();
        private HRMSData hrmsData = new HRMSData();
        private HRMSData hrmsDal = new HRMSData();
        RASolarSecurityData securityDal = new RASolarSecurityData();
        string message = string.Empty;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SalesMonitoringEntry()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "SalesMonitoringEntry", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            string unitCode = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(Request.QueryString["uc"]))
                {
                    unitCode = objLoginHelper.LocationCode;
                    Session.Remove("unitCode");

                    ViewBag.LocationTitle = objLoginHelper.LocationTitle;
                    ViewBag.Location = objLoginHelper.Location;
                    ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
                    ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
                    ViewBag.RegionTitle = objLoginHelper.RegionTitle;
                    ViewBag.RegionName = objLoginHelper.LogInForRegionName;
                    ViewBag.UnitTitle = objLoginHelper.UnitTitle;
                    ViewBag.UnitName = objLoginHelper.LogInForUnitName;

                    ViewBag.TopMenu = objLoginHelper.TopMenu;
                    ViewBag.EnableDisable = "";
                }
                else
                {
                    unitCode = Request.QueryString["uc"];
                    Session["unitCode"] = unitCode;

                    LoginHelper objNewLoginHelper = new LoginHelper();
                    objNewLoginHelper = new HelperData().SearchLocation(unitCode);

                    ViewBag.LocationTitle = objNewLoginHelper.LocationTitle;
                    ViewBag.Location = objNewLoginHelper.Location;
                    ViewBag.ZoneTitle = objNewLoginHelper.ZoneTitle;
                    ViewBag.ZoneName = objNewLoginHelper.LogInForZoneName;
                    ViewBag.RegionTitle = objNewLoginHelper.RegionTitle;
                    ViewBag.RegionName = objNewLoginHelper.LogInForRegionName;
                    ViewBag.UnitTitle = objNewLoginHelper.UnitTitle;
                    ViewBag.UnitName = objNewLoginHelper.LogInForUnitName;

                    ViewBag.TopMenu = objNewLoginHelper.TopMenu;

                    ViewBag.EnableDisable = "disabled=disabled";
                }

                ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");

                Common_PeriodOpenClose objPeriodOpenClose = new Common_PeriodOpenClose();
                objPeriodOpenClose = salesDal.ReadPeriodOpenClose(unitCode);

                if (objPeriodOpenClose == null)
                {
                    Session["messageInformation"] = "Day Is Not Open For Daily Entry";
                    return RedirectToAction("ErrorMessage", "../ErrorHnadle");
                }

                TempData["PeriodOpenClose"] = Convert.ToDateTime(objPeriodOpenClose.CalenderDate).ToString("dddd,  MMMM  dd,  yyyy");
                //string yearMonth = Convert.ToDateTime(objPeriodOpenClose.CalenderDate).ToString("yyyyMM");
                //objLoginHelper.YearMonthCurrent = Convert.ToDateTime(objPeriodOpenClose.CalenderDate).ToString("yyyyMM");

                List<GetLocationWiseEmployee> lstGetLocationWiseEmployee = new List<GetLocationWiseEmployee>();
                lstGetLocationWiseEmployee = salesDal.ReadGetLocationWiseEmployee(unitCode);
                ViewBag.GetLocationWiseEmployee = lstGetLocationWiseEmployee;

                TempData["LocationWiseEmployeeSelected"] = "0";

                GetLocationWiseEmployee objGetLocationWiseEmployee = new GetLocationWiseEmployee();
                objGetLocationWiseEmployee = lstGetLocationWiseEmployee.Where(l => l.IsItUnitManager.Trim() == "U").FirstOrDefault();

                if (objGetLocationWiseEmployee != null)
                {
                    ViewBag.UnitManager = objGetLocationWiseEmployee.EmployeeID;
                    Session["Manager"] = objGetLocationWiseEmployee.EmployeeID;
                }
                else
                {
                    ViewBag.UnitManager = "0";
                    Session["Manager"] = "0";
                }

                Sal_LocationWiseSalesNCollectionTarget objLocationWiseSalesNCollectionTarget = new Sal_LocationWiseSalesNCollectionTarget();
                objLocationWiseSalesNCollectionTarget = salesDal.ReadLocationWiseSalesNCollectionTarget(unitCode, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForDailyProgressReview));

                bool IsEmployeeWiseTargetSettingCompleted = false;
                if (objLocationWiseSalesNCollectionTarget != null)
                {
                    IsEmployeeWiseTargetSettingCompleted = objLocationWiseSalesNCollectionTarget.IsEmployeeWiseTargetSettingCompleted;
                }


                ViewBag.IsEmployeeWiseTargetSettingCompleted = IsEmployeeWiseTargetSettingCompleted == false ? "Employee Wise Tagret Allocation Not Yet Completed For This Month, Please Check" : "";


            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult EmployeeWiseMonthlyTargetView()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "EmployeeWiseMonthlyTargetView", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            Common_PeriodOpenClose objPeriodOpenClose = new Common_PeriodOpenClose();
            objPeriodOpenClose = salesDal.ReadPeriodOpenClose(objLoginHelper.LogInForUnitCode);

            //ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            TempData["YearMonth"] = objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM, yyyy");
            //start original
            List<GetLocationWiseEmployeeTarget> lstLocationWiseEmployeeTarget = new List<GetLocationWiseEmployeeTarget>();
            lstLocationWiseEmployeeTarget = salesDal.ReadGetLocationWiseEmployeeTarget(objLoginHelper.LogInForUnitCode, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForDailyProgressReview));
            //end original

            //List<EmployeeWiseSalesNColcTargetNAchievementForAUnit> lstLocationWiseEmployeeTarget = new List<GetLocationWiseEmployeeTarget>();
            //lstLocationWiseEmployeeTarget = salesDal.ReadGetLocationWiseEmployeeTarget(objLoginHelper.LogInForUnitCode, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForDailyProgressReview));
            TempData["LocationWiseEmployeeTarget"] = lstLocationWiseEmployeeTarget;
            ViewBag.CountEmployee = lstLocationWiseEmployeeTarget.Count;

            Sal_LocationWiseSalesNCollectionTarget objLocationWiseSalesNCollectionTarget = new Sal_LocationWiseSalesNCollectionTarget();

            objLocationWiseSalesNCollectionTarget = salesDal.ReadLocationWiseSalesNCollectionTarget(objLoginHelper.LogInForUnitCode, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForDailyProgressReview));

            if (objLocationWiseSalesNCollectionTarget != null)
            {
                TempData["LocationWiseSalesNCollectionTarget"] = objLocationWiseSalesNCollectionTarget;
            }
            else
            {
                objLocationWiseSalesNCollectionTarget = new Sal_LocationWiseSalesNCollectionTarget();
                objLocationWiseSalesNCollectionTarget.SalesTarget = 0;
                objLocationWiseSalesNCollectionTarget.RegularCollectionTarget = 0;
                objLocationWiseSalesNCollectionTarget.OverdueCollectionTarget = 0;
                TempData["LocationWiseSalesNCollectionTarget"] = objLocationWiseSalesNCollectionTarget;
            }

            if (objLocationWiseSalesNCollectionTarget != null && objLocationWiseSalesNCollectionTarget.IsEmployeeWiseTargetSettingCompleted == true)
            {
                ViewBag.IsEmployeeWiseTargetSettingCompleted = true;
            }

            return View();
        }

        public ActionResult EmployeeWiseDailySalesTargetSetup()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "EmployeeWiseDailySalesTargetSetup", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            Common_PeriodOpenClose objPeriodOpenClose = new Common_PeriodOpenClose();
            objPeriodOpenClose = salesDal.ReadPeriodOpenClose(objLoginHelper.LogInForUnitCode);

            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            if (DateTime.IsLeapYear(objLoginHelper.MonthOpenForDailyProgressReview.Year) && objLoginHelper.MonthOpenForDailyProgressReview.Month == 2)
            {
                ViewBag.IsLeapYear = true;
            }
            else if (objLoginHelper.MonthOpenForDailyProgressReview.Month == 2)
            {
                ViewBag.IsLeapYear = false;
            }
            else
            {
                ViewBag.IsLeapYear = true;
            }

            TempData["YearMonth"] = objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM, yyyy");

            Sal_LocationWiseSalesNCollectionTarget objLocationWiseSalesNCollectionTarget = new Sal_LocationWiseSalesNCollectionTarget();
            objLocationWiseSalesNCollectionTarget = salesDal.ReadLocationWiseSalesNCollectionTarget(objLoginHelper.LogInForUnitCode, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForDailyProgressReview));

            if (objLocationWiseSalesNCollectionTarget != null)
            {
                TempData["LocationWiseSalesNCollectionTarget"] = objLocationWiseSalesNCollectionTarget;
            }
            else
            {
                objLocationWiseSalesNCollectionTarget = new Sal_LocationWiseSalesNCollectionTarget();
                objLocationWiseSalesNCollectionTarget.SalesTarget = 0;
                TempData["LocationWiseSalesNCollectionTarget"] = objLocationWiseSalesNCollectionTarget;
            }

            List<EmployeeTransferInfo> lstEmployeeTransferredButNotYetAccepted = new List<EmployeeTransferInfo>();
            lstEmployeeTransferredButNotYetAccepted = hrmsData.ReadGetEmployeeTransferredButNotYetAccepted(objLoginHelper.LocationCode);

            if (lstEmployeeTransferredButNotYetAccepted.Count > 0)
            {
                ViewBag.EmployeeTransferredButNotYetAccepted = 1;
            }
            else
            {
                ViewBag.EmployeeTransferredButNotYetAccepted = 0;
            }

            List<LocationNEmployeeWiseDailySalesNCollectionTarget> lstEmployeeWiseDailyTarget = new List<LocationNEmployeeWiseDailySalesNCollectionTarget>();
            lstEmployeeWiseDailyTarget = salesDal.ReadLocationNEmployeeWiseDailySalesNCollectionTarget(objLoginHelper.LocationCode);

            ViewBag.TotalAllocatedTarget = lstEmployeeWiseDailyTarget.Sum(s => s.TotalForTheMonth);
            ViewBag.CountEmployee = lstEmployeeWiseDailyTarget.Count;

            if (objLocationWiseSalesNCollectionTarget != null && objLocationWiseSalesNCollectionTarget.IsEmployeeWiseTargetSettingCompleted == true)
            {
                ViewBag.IsEmployeeWiseTargetSettingCompleted = 1;
            }
            else
            {
                ViewBag.IsEmployeeWiseTargetSettingCompleted = 0;
            }

            return View(lstEmployeeWiseDailyTarget);
        }

        public ActionResult DailyProgressReviewDataEntryStatus()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "DailyProgressReviewDataEntryStatus", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            string unitCode = string.Empty, userId = string.Empty;

            ViewBag.ShowHide = false;
            ViewBag.IsAuthenticForSave = objLoginHelper.IsAuthenticApproverForThisLocation;

            if (objLoginHelper.ReportType == Helper.HOSales)
            {
                unitCode = string.Empty;
                userId = string.Empty;
            }
            else if (objLoginHelper.ReportType == Helper.ZonalOffice)
            {
                unitCode = objLoginHelper.LogInForZoneCode;
                userId = string.Empty;
            }
            else if (objLoginHelper.ReportType == Helper.RegionalOffice)
            {
                unitCode = objLoginHelper.LogInForRegionCode;
                userId = string.Empty;
            }
            else if (objLoginHelper.ReportType == Helper.UnitOffice)
            {
                unitCode = objLoginHelper.LogInForUnitCode;
                userId = string.Empty;

                ViewBag.ShowHide = true;
            }

            //start code for ddEmployee

                                //List<GetLocationWiseEmployee> lstGetLocationWiseEmployee = new List<GetLocationWiseEmployee>();
                                //lstGetLocationWiseEmployee = salesDal.ReadGetLocationWiseEmployee(unitCode);
                                //ViewBag.GetLocationWiseEmployee = lstGetLocationWiseEmployee;

                                //TempData["LocationWiseEmployeeSelected"] = "0";

            //end code for ddEmployee



            // DateTime openMonthForDailyProgressReview = salesDal.ReadLastMonth().MonthOpenForDailyProgressReview.Date;
            // string openYearMonth = Helper.ConvertDateToYearMonth(openMonthForDailyProgressReview);

            TempData["PeriodOpenClose"] = Convert.ToDateTime(objLoginHelper.MonthOpenForDailyProgressReview).ToString("MMMM-yyyy");

            List<ProgressReviewDataEntryStatusDaily> lstProgressReview = new List<ProgressReviewDataEntryStatusDaily>();
            lstProgressReview = salesReportDal.ReadProgressReviewDataEntryStatusDaily(objLoginHelper.ReportType, objLoginHelper.LocationCode, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForDailyProgressReview), userId);

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            DateTime dtYesterday = DateTime.Now.Date.AddDays(-1);

            ViewBag.TotalNumberofUnits = lstProgressReview.Count();
            ViewBag.TargetSettingsCompleted = lstProgressReview.Count(p => p.IsEmployeeWiseTargetSettingCompleted.ToLower() == "yes");
            ViewBag.EntryUpToDate = lstProgressReview.Count(p => p.LastOpenDateByUO != null && (Convert.ToDateTime(p.LastOpenDateByUO).Date == dtYesterday || Convert.ToDateTime(p.LastOpenDateByUO).Date == dtYesterday.AddDays(1)));

            return View(lstProgressReview);
        }

        public ActionResult LocationNEmployeeWiseSalesNCollectionEntry()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "LocationNEmployeeWiseSalesNCollectionEntry", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            List<LocationAndEmployeeWiseWeeklySalesAndCollectionEntry> lstLocationAndEmployeeWiseWeeklySalesAndCollectionEntry = new List<LocationAndEmployeeWiseWeeklySalesAndCollectionEntry>();
            //lstLocationAndEmployeeWiseWeeklySalesAndCollectionEntry = salesDal.ReadLocationAndEmployeeWiseWeeklySalesAndCollectionEntry(objLoginHelper.LogInForUnitCode, objLoginHelper.CurrentYearWeek);

            ViewBag.YearWeek = objLoginHelper.CurrentYearWeek;
            ViewBag.WeekStartDate = objLoginHelper.FirstDateOfCurrentWeek.ToString("dd-MMM-yyyy");
            ViewBag.WeekEndDate = objLoginHelper.LastDateOfCurrentWeek.ToString("dd-MMM-yyyy");

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.Initial = "1";

            Session["To"] = 1;

            return View(lstLocationAndEmployeeWiseWeeklySalesAndCollectionEntry);
        }

        public ActionResult LocationNEmployeeWiseWeeklySalesNCollectionReport()
        {
            string openYearWeek = string.Empty;

            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "LocationNEmployeeWiseWeeklySalesNCollectionReport", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            openYearWeek = objLoginHelper.WeekOpenForWeeklyOverdueCollection;
            ViewBag.IsAuthenticForSave = objLoginHelper.IsAuthenticApproverForThisLocation;

            List<LocationAndEmployeeWiseWeeklySalesAndCollectionReport> lstLocationAndEmployeeWiseWeeklySalesAndCollectionReport = new List<LocationAndEmployeeWiseWeeklySalesAndCollectionReport>();

           // Sal_LocationWiseWeeklySalesNCollectionSummary objCollectionSummary = new Sal_LocationWiseWeeklySalesNCollectionSummary();
           // objCollectionSummary = salesDal.LocationWiseWeeklySalesNCollectionSummary(objLoginHelper.LogInForZoneCode, openYearWeek);

            if (objLoginHelper.ReportType == Helper.HOSales)
            {
                ViewBag.RemarksColumnShowHide = false;
                ViewBag.RemarksEntryShowHide = false;
            }
            else if (objLoginHelper.ReportType == Helper.ZonalOffice)
            {
                ViewBag.RemarksColumnShowHide = true;
                ViewBag.RemarksEntryShowHide = true;
            }
            else if (objLoginHelper.ReportType == Helper.RegionalOffice)
            {
                ViewBag.RemarksColumnShowHide = true;
                ViewBag.RemarksEntryShowHide = false;
            }

            //lstLocationAndEmployeeWiseWeeklySalesAndCollectionReport = salesDal.ReadLocationAndEmployeeWiseWeeklySalesAndCollectionReport(objLoginHelper.ReportType, objLoginHelper.LocationCode, openYearWeek);

            ViewBag.YearWeek = objLoginHelper.CurrentYearWeek;
            ViewBag.WeekStartDate = objLoginHelper.FirstDateOfCurrentWeek.ToString("dd-MMM-yyyy");
            ViewBag.WeekEndDate = objLoginHelper.LastDateOfCurrentWeek.ToString("dd-MMM-yyyy");

            ViewBag.OpenYearWeek = openYearWeek;

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.YearWeekFormat = new YearWeekFormat().WeekFormat();
            TempData["SelectYearWeek"] = openYearWeek;

            //if (objCollectionSummary != null)
            //    TempData["Reamrks"] = objCollectionSummary.Remarks != null ? objCollectionSummary.Remarks : "";
            //else
            //    TempData["Reamrks"] = "";

            return View(lstLocationAndEmployeeWiseWeeklySalesAndCollectionReport);
        }
        
        public ActionResult WeeklySalesNCollectionEntryStatus()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "WeeklySalesNCollectionEntryStatus", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            List<WeeklySalesNCollectionStatus> lstSalesCollectionStatus = new List<WeeklySalesNCollectionStatus>();

            ViewBag.IsAuthenticForSave = objLoginHelper.IsAuthenticApproverForThisLocation;

            List<LocationAndEmployeeWiseWeeklySalesAndCollectionReport> lstLocationAndEmployeeWiseWeeklySalesAndCollectionReport = new List<LocationAndEmployeeWiseWeeklySalesAndCollectionReport>();

            if (objLoginHelper.ReportType == Helper.HOSales)
            {
                ViewBag.RemarksColumnShowHide = false;
                ViewBag.RemarksEntryShowHide = false;
            }
            else if (objLoginHelper.ReportType == Helper.ZonalOffice)
            {
                ViewBag.RemarksColumnShowHide = true;
                ViewBag.RemarksEntryShowHide = true;
            }
            else if (objLoginHelper.ReportType == Helper.RegionalOffice)
            {
                ViewBag.RemarksColumnShowHide = true;
                ViewBag.RemarksEntryShowHide = false;
            }

            lstSalesCollectionStatus = salesDal.ReadWeeklySalesNCollectionStatus(objLoginHelper.ReportType, objLoginHelper.LocationCode, objLoginHelper.CurrentYearWeek);

            ViewBag.YearWeek = objLoginHelper.CurrentYearWeek;
            ViewBag.WeekStartDate = objLoginHelper.FirstDateOfCurrentWeek.ToString("dd-MMM-yyyy");
            ViewBag.WeekEndDate = objLoginHelper.LastDateOfCurrentWeek.ToString("dd-MMM-yyyy");

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            return View(lstSalesCollectionStatus);
        }

        //WeeklySalesNCollectionEntryStatusForUnitUser
        public ActionResult WeeklySalesNCollectionEntryStatusForUnitUser() 
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "WeeklySalesNCollectionEntryStatusForUnitUser", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            List<TrackerLocationWiseWeeklyODCollectionTargetVsAchievement> lstSalesCollectionStatus = new List<TrackerLocationWiseWeeklyODCollectionTargetVsAchievement>();

            ViewBag.IsAuthenticForSave = objLoginHelper.IsAuthenticApproverForThisLocation;

            List<LocationAndEmployeeWiseWeeklySalesAndCollectionReport> lstLocationAndEmployeeWiseWeeklySalesAndCollectionReport = new List<LocationAndEmployeeWiseWeeklySalesAndCollectionReport>();

            if (objLoginHelper.ReportType == Helper.HOSales)
            {
                ViewBag.RemarksColumnShowHide = false;
                ViewBag.RemarksEntryShowHide = false;
            }
            else if (objLoginHelper.ReportType == Helper.ZonalOffice)
            {
                ViewBag.RemarksColumnShowHide = true;
                ViewBag.RemarksEntryShowHide = true;
            }
            else if (objLoginHelper.ReportType == Helper.RegionalOffice)
            {
                ViewBag.RemarksColumnShowHide = true;
                ViewBag.RemarksEntryShowHide = false;
            }

            lstSalesCollectionStatus = salesDal.ReadTrackerLocationWiseWeeklyODCollectionTargetVsAchievement(objLoginHelper.ReportType, objLoginHelper.LocationCode, objLoginHelper.CurrentYearWeek);

            ViewBag.YearWeek = objLoginHelper.CurrentYearWeek;
            ViewBag.WeekStartDate = objLoginHelper.FirstDateOfCurrentWeek.ToString("dd-MMM-yyyy");
            ViewBag.WeekEndDate = objLoginHelper.LastDateOfCurrentWeek.ToString("dd-MMM-yyyy");

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            return View(lstSalesCollectionStatus);
        }
        public ActionResult LoadRequestEntry()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "LoadRequestEntry", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }
            
             List<CheckLoadRequestEntryForLocationCode> lstCheckLoadRequestEntryForLocationCode = new List<CheckLoadRequestEntryForLocationCode>();
             lstCheckLoadRequestEntryForLocationCode = salesDal.LocationCheckLoadRequestEntry(objLoginHelper.LocationCode);
             //string checkLocationCode = lstCheckLoadRequestEntryForLocationCode.Select(m => m.LocationCode).FirstOrDefault().ToString();

             if (lstCheckLoadRequestEntryForLocationCode.Count==0)
            {
                Session["messageInformation"] = "Load Request Entry is not Valid for your unit! Please contact with HO if necessary.";
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            List<LoadRequestEntryGet> lstLoadRequestEntryGet = new List<LoadRequestEntryGet>();
            List<LoadRequestEntryGet> lstLoadRequestEntryGetForInsert = new List<LoadRequestEntryGet>(); 
            List<CheckLoadRequestEntry> lstCheckLoadRequestEntry = new List<CheckLoadRequestEntry>();
            string yearMonth = objLoginHelper.MonthOpenForDailyProgressReview.ToString("yyyyMM");
            lstCheckLoadRequestEntry = salesDal.firstCheckLoadRequestEntry(yearMonth, objLoginHelper.LocationCode, DateTime.Now.ToString("yyyy-MM-dd"));
            ViewBag.TargetBtn = lstCheckLoadRequestEntry.Select(m => m.IsLoadRequestCompleted).FirstOrDefault().ToString()=="True"?1:0;
            ViewBag.IsAuthenticForSave = objLoginHelper.IsAuthenticApproverForThisLocation;

            //EXEC [SP_SalLocationNEmployeeNDateWiseTransaction] 'AMT001','','01-Dec-2014',0,0,'','Admin','INSERT'

            lstLoadRequestEntryGetForInsert = salesDal.InsertLoadRequest(objLoginHelper.LocationCode, DateTime.Now.ToString("dd-MMM-yyyy"));
            lstLoadRequestEntryGet = salesDal.ReadLoadRequestEntryGet(objLoginHelper.LocationCode, DateTime.Now.ToString("yyyy-MM-dd"));

            TempData["LstLoadRequestEntryGet"] = lstLoadRequestEntryGet;

            ViewBag.YearWeek = objLoginHelper.CurrentYearWeek;
            ViewBag.WeekStartDate = objLoginHelper.FirstDateOfCurrentWeek.ToString("dd-MMM-yyyy");
            ViewBag.WeekEndDate = objLoginHelper.LastDateOfCurrentWeek.ToString("dd-MMM-yyyy");

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");
            ViewBag.OpenDay = objLoginHelper.MonthOpenForDailyProgressReview.ToString("dd-MMM-yyyy");
            ViewBag.LocationCode = objLoginHelper.LocationCode;
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            return View(lstLoadRequestEntryGet);
        }


      


        public ActionResult ProgressReviewGraph()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "ProgressReviewGraph", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            List<SalSalesNCollectionTargetVsAchievementForGraph> lstSalSalesNCollectionTargetVsAchievementForGraph = new List<SalSalesNCollectionTargetVsAchievementForGraph>();
            lstSalSalesNCollectionTargetVsAchievementForGraph = salesDal.ReadSalSalesNCollectionTargetVsAchievementForGraph("RSFSUMMARY", "", "").ToList();
            TempData["lstSalSalesNCollectionTargetVsAchievementForGraph"] = lstSalSalesNCollectionTargetVsAchievementForGraph;

            ViewBag.Stack = 1;
            ViewBag.SeriesType = "line";
            ViewBag.ShowTitle = "Daily Progress";
            ViewBag.ShowLegend = "";
            ViewBag.LegendPosition = "";

            return View();
        }

        [HttpPost]
        public ActionResult LocationNEmployeeWiseWeeklySalesNCollectionReport(FormCollection fCollection)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            string openYearWeek = string.Empty;
            openYearWeek = fCollection["hfOpenYearWeek"];

            ViewBag.IsAuthenticForSave = objLoginHelper.IsAuthenticApproverForThisLocation;

            List<LocationAndEmployeeWiseWeeklySalesAndCollectionReport> lstLocationAndEmployeeWiseWeeklySalesAndCollectionReport = new List<LocationAndEmployeeWiseWeeklySalesAndCollectionReport>();

            if (objLoginHelper.ReportType == Helper.HOSales)
            {
                ViewBag.RemarksColumnShowHide = false;
                ViewBag.RemarksEntryShowHide = false;
            }
            else if (objLoginHelper.ReportType == Helper.ZonalOffice)
            {
                ViewBag.RemarksColumnShowHide = true;
                ViewBag.RemarksEntryShowHide = true;
            }
            else if (objLoginHelper.ReportType == Helper.RegionalOffice)
            {
                ViewBag.RemarksColumnShowHide = true;
                ViewBag.RemarksEntryShowHide = false;
            }

            if (fCollection["ddlYearWeek"] != "")
            {
               // lstLocationAndEmployeeWiseWeeklySalesAndCollectionReport = salesDal.ReadLocationAndEmployeeWiseWeeklySalesAndCollectionReport(objLoginHelper.ReportType, objLoginHelper.LocationCode, fCollection["ddlYearWeek"]);
                //TempData["SelectYearWeek"] = fCollection["ddlYearWeek"];
            }
            else
            {
                //lstLocationAndEmployeeWiseWeeklySalesAndCollectionReport = salesDal.ReadLocationAndEmployeeWiseWeeklySalesAndCollectionReport(objLoginHelper.ReportType, objLoginHelper.LocationCode, openYearWeek);
            }

            ViewBag.YearWeek = objLoginHelper.CurrentYearWeek;
            ViewBag.WeekStartDate = objLoginHelper.FirstDateOfCurrentWeek.ToString("dd-MMM-yyyy");
            ViewBag.WeekEndDate = objLoginHelper.LastDateOfCurrentWeek.ToString("dd-MMM-yyyy");

            ViewBag.OpenYearWeek = openYearWeek;

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.YearWeekFormat = new YearWeekFormat().WeekFormat();

            TempData["Reamrks"] = "";

            return View(lstLocationAndEmployeeWiseWeeklySalesAndCollectionReport);
        }

        public JsonResult SaveLocationNEmployeeWiseSalesNCollectionEntry(string customerCode, string odRecoveredCurrentWeek, string weeklyAcievement, string remarksRSF)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            //Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement objLocationWiseWeeklyTarget;
            //List<Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement> lstLocationWiseWeeklyTarge = new List<Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement>();

           // lstLocationWiseWeeklyTarge = salesDal.ReadWeeklySalesNCollectionTargetNAchievement(objLoginHelper.LogInForUnitCode, objLoginHelper.CurrentYearWeek, objLoginHelper.LastDateOfCurrentWeek);

            int countEmployee = 0;

            try
            {
                //if (weeklyAcievement != "" && lstLocationWiseWeeklyTarge.Count > 0)
                //{
                //    foreach (Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement LWE in lstLocationWiseWeeklyTarge)
                //    {
                //        objLocationWiseWeeklyTarget = new Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement();

                //        objLocationWiseWeeklyTarget = LWE;

                //        objLocationWiseWeeklyTarget.OverdueColcAchievement = Convert.ToDecimal(weeklyAcievement);
                //        objLocationWiseWeeklyTarget.Remarks = remarksRSF;
                //        objLocationWiseWeeklyTarget.EntryTime = DateTime.Now;

                //        salesDal.UpdateLocationAndEmployeeWiseWeeklySalesAndCollectionEntry(objLocationWiseWeeklyTarget);

                //        countEmployee++;
                //    }
                //}

               // bool objCustomerOverdue = salesDal.UpdateCustomerWiseOverdueCollection(objLoginHelper.LogInForUnitCode, customerCode, Convert.ToDecimal(odRecoveredCurrentWeek), objLoginHelper.CurrentYearWeek.Substring(4, 2));

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty) };

            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        [HttpPost]
        public ActionResult DailyProgressReviewDataEntryStatus(FormCollection fCollection)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            string userId = string.Empty;

            ViewBag.ShowHide = false;
            ViewBag.IsAuthenticForSave = objLoginHelper.IsAuthenticApproverForThisLocation;

            //DateTime openMonthForDailyProgressReview = salesDal.ReadLastMonth().MonthOpenForDailyProgressReview.Date;
            //string openYearMonth = Helper.ConvertDateToYearMonth(openMonthForDailyProgressReview);

            TempData["PeriodOpenClose"] = Convert.ToDateTime(objLoginHelper.MonthOpenForDailyProgressReview).ToString("MMMM-yyyy");

            if (objLoginHelper.ReportType == Helper.HOSales)
            {
                if (Convert.ToBoolean(fCollection["hfFilteredByUser"]) == true)
                {
                    userId = objLoginHelper.LogInID;
                }
                else
                    userId = string.Empty;
            }
            else if (objLoginHelper.ReportType == Helper.ZonalOffice)
            {
                userId = string.Empty;
            }
            else if (objLoginHelper.ReportType == Helper.RegionalOffice)
            {
                userId = string.Empty;
            }
            else if (objLoginHelper.ReportType == Helper.UnitOffice)
            {
                userId = string.Empty;
                ViewBag.ShowHide = true;
            }

            List<ProgressReviewDataEntryStatusDaily> lstProgressReview = new List<ProgressReviewDataEntryStatusDaily>();
            lstProgressReview = salesReportDal.ReadProgressReviewDataEntryStatusDaily(objLoginHelper.ReportType, objLoginHelper.LocationCode, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForDailyProgressReview), userId);

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            DateTime dtYesterday = DateTime.Now.Date.AddDays(-1);

            ViewBag.TotalNumberofUnits = lstProgressReview.Count();
            ViewBag.TargetSettingsCompleted = lstProgressReview.Count(p => p.IsEmployeeWiseTargetSettingCompleted.ToLower() == "yes");
            ViewBag.EntryUpToDate = lstProgressReview.Count(p => p.LastOpenDateByUO != null && Convert.ToDateTime(p.LastOpenDateByUO).Date == dtYesterday);

            return View(lstProgressReview);
        }

        [HttpPost]
        public ActionResult SalesMonitoringEntry(FormCollection fCollection)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            string unitCode = string.Empty;

            if (Session["unitCode"] != null)
            {
                unitCode = (string)Session["unitCode"];

                LoginHelper objNewLoginHelper = new LoginHelper();
                objNewLoginHelper = new HelperData().SearchLocation(unitCode);

                ViewBag.LocationTitle = objNewLoginHelper.LocationTitle;
                ViewBag.Location = objNewLoginHelper.Location;
                ViewBag.ZoneTitle = objNewLoginHelper.ZoneTitle;
                ViewBag.ZoneName = objNewLoginHelper.LogInForZoneName;
                ViewBag.RegionTitle = objNewLoginHelper.RegionTitle;
                ViewBag.RegionName = objNewLoginHelper.LogInForRegionName;
                ViewBag.UnitTitle = objNewLoginHelper.UnitTitle;
                ViewBag.UnitName = objNewLoginHelper.LogInForUnitName;

                ViewBag.TopMenu = objNewLoginHelper.TopMenu;
                ViewBag.EnableDisable = "disabled=disabled";
            }
            else
            {
                unitCode = objLoginHelper.LogInForUnitCode;

                ViewBag.LocationTitle = objLoginHelper.LocationTitle;
                ViewBag.Location = objLoginHelper.Location;
                ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
                ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
                ViewBag.RegionTitle = objLoginHelper.RegionTitle;
                ViewBag.RegionName = objLoginHelper.LogInForRegionName;
                ViewBag.UnitTitle = objLoginHelper.UnitTitle;
                ViewBag.UnitName = objLoginHelper.LogInForUnitName;

                ViewBag.TopMenu = objLoginHelper.TopMenu;
                ViewBag.EnableDisable = "";
            }

            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");

            Common_PeriodOpenClose objPeriodOpenClose = new Common_PeriodOpenClose();
            objPeriodOpenClose = salesDal.ReadPeriodOpenClose(unitCode);
            //TempData["PeriodOpenClose"] = Convert.ToDateTime(objPeriodOpenClose.CalenderDate).ToString("dddd,  MMMM  dd,  yyyy");

            objLoginHelper.YearMonthCurrent = Convert.ToDateTime(objPeriodOpenClose.CalenderDate).ToString("yyyyMM");

            // objLoginHelper.YearMonthCurrent = Convert.ToDateTime(objPeriodOpenClose.CalenderDate).ToString("yyyyMM");

            List<GetLocationWiseEmployee> lstGetLocationWiseEmployee = new List<GetLocationWiseEmployee>();
            lstGetLocationWiseEmployee = salesDal.ReadGetLocationWiseEmployee(unitCode);
            ViewBag.GetLocationWiseEmployee = lstGetLocationWiseEmployee;

            ViewBag.CountEmployee = lstGetLocationWiseEmployee.Count;

            var uManager = lstGetLocationWiseEmployee.Where(l => l.IsItUnitManager.Trim() == "U").FirstOrDefault();

            if (uManager != null)
                ViewBag.UnitManager = uManager.EmployeeID;
            else
                ViewBag.UnitManager = "0";

            TempData["LocationWiseEmployeeSelected"] = fCollection["ddlEmployee"].ToString();

            TempData["PeriodOpenClose"] = Convert.ToDateTime(objPeriodOpenClose.CalenderDate).ToString("dddd,  MMMM  dd,  yyyy");

            GetLocationNEmployeeWiseDailyEntry objGetLocationNEmployeeWiseDailyEntry = new GetLocationNEmployeeWiseDailyEntry();
            objGetLocationNEmployeeWiseDailyEntry = salesDal.ReadLocationNEmployeeWiseActivity(unitCode, fCollection["ddlEmployee"].ToString(), objLoginHelper.YearMonthCurrent, Convert.ToDateTime(objPeriodOpenClose.CalenderDate).Date);

            if (objGetLocationNEmployeeWiseDailyEntry != null)
            {
                ViewBag.EmployeeId = objGetLocationNEmployeeWiseDailyEntry.EmployeeID;
            }
            else
            {
                ViewBag.EmployeeId = "0";
            }

            TempData["LocationNEmployeeWiseDailyEntry"] = objGetLocationNEmployeeWiseDailyEntry;
            return View();
        }

        public JsonResult SaveSalesMonitoringEntry(string EmployeeID, string AchievementTodaySales, string AchievementTodayCollection, string AchievementTodayOverdue, string SystemInstallation, string CustomerVisit, string Maintenance, string BankOperation, string OfficialVisit, string Meeting, string TAAmount, string Leave, string DeskWork, string Remarks, string DataEntryinERP, string SystemReturn, string SystemReSale, string CustomerTraining, string BioGasSale, string RSFBatterySales, string TransactionDate, string TAReason, string InstallmentCollection, string hfCountEmployee, string SystemInstallationNextDay, string CustomerVisitNextDay, string MaintenanceNextDay, string BankOperationNextDay, string OfficialVisitNextDay, string MeetingNextDay, string InstallmentCollectionNextDay, string LeaveNextDay, string DeskWorkNextDay, string RemarksNextDay, string taAmountNextDay, string taReasonNextDay)
        {
            string successMessage = string.Empty;
            try
            {

                objLoginHelper = (LoginHelper)Session["LogInInformation"];

                Common_PeriodOpenClose objPeriodOpenClose = new Common_PeriodOpenClose();
                objPeriodOpenClose = salesDal.ReadPeriodOpenClose(objLoginHelper.LogInForUnitCode);
                objLoginHelper.YearMonthCurrent = Convert.ToDateTime(objPeriodOpenClose.CalenderDate).ToString("yyyyMM");

                Sal_LocationNEmployeeWiseActivityDaily objLocationNEmployeeWiseActivityDaily = new Sal_LocationNEmployeeWiseActivityDaily();
                //GetLocationNEmployeeWiseDailyEntry objGetLocationNEmployeeWiseDailyEntry = new GetLocationNEmployeeWiseDailyEntry();
                //objGetLocationNEmployeeWiseDailyEntry = salesDal.ReadLocationNEmployeeWiseActivity(objLoginHelper.LogInForUnitCode, EmployeeID, objLoginHelper.YearMonthCurrent, Convert.ToDateTime(TransactionDate).Date);
                objLocationNEmployeeWiseActivityDaily = salesDal.ReadLocationNEmployeeWiseActivityDaily(objLoginHelper.LogInForUnitCode, EmployeeID, objLoginHelper.YearMonthCurrent, Convert.ToDateTime(TransactionDate).Date);

                if (objLocationNEmployeeWiseActivityDaily != null)
                {
                    objLocationNEmployeeWiseActivityDaily.SalesAchievementToday = Convert.ToInt32(AchievementTodaySales != "0" ? AchievementTodaySales : "0");
                    objLocationNEmployeeWiseActivityDaily.RegColcAchievementToday = Convert.ToInt32(AchievementTodayCollection != "0" ? AchievementTodayCollection : "0");
                    objLocationNEmployeeWiseActivityDaily.OverdueColcAchievementToday = Convert.ToInt32(AchievementTodayOverdue != "0" ? AchievementTodayOverdue : "0");
                    objLocationNEmployeeWiseActivityDaily.TAToday = Convert.ToDecimal(TAAmount != "" ? TAAmount : "0");
                    objLocationNEmployeeWiseActivityDaily.RemarksToday = Remarks;

                    if (EmployeeID.Trim() == Session["Manager"].ToString())
                    {
                        objLocationNEmployeeWiseActivityDaily.SystemReturnToday = Convert.ToInt32(SystemReturn != "" ? SystemReturn : "0");
                        objLocationNEmployeeWiseActivityDaily.SystemReSaleToday = Convert.ToInt32(SystemReSale != "" ? SystemReSale : "0");
                        objLocationNEmployeeWiseActivityDaily.CustomerTrainingToday = Convert.ToInt32(CustomerTraining != "" ? CustomerTraining : "0");
                        objLocationNEmployeeWiseActivityDaily.BioGasSalesToday = Convert.ToInt32(BioGasSale != "" ? BioGasSale : "0");
                        objLocationNEmployeeWiseActivityDaily.RSFBatterySalesToday = Convert.ToInt32(RSFBatterySales != "" ? RSFBatterySales : "0");

                        //if (CloseCheckBox == true)
                        //{
                        List<Sal_LocationNEmployeeWiseActivityDaily> lstEntryCount = new List<Sal_LocationNEmployeeWiseActivityDaily>();

                        lstEntryCount = salesDal.ReadLocationNEmployeeWiseActivityDaily(objLoginHelper.LogInForUnitCode, objLoginHelper.YearMonthCurrent, Convert.ToDateTime(TransactionDate).Date);

                        if (lstEntryCount.Count == Convert.ToInt32(hfCountEmployee))
                        {
                            successMessage = salesDal.ExecuteDayEndProcess(objLoginHelper.LogInForUnitCode, Convert.ToDateTime(TransactionDate).ToString("dd-MMM-yyyy"));
                        }
                        else
                        {
                            return new JsonResult { Data = "Entry Not Completed" };
                        }

                        //}

                        if (successMessage == "Failed")
                        {
                            successMessage = "Failed To Day Close";
                        }
                        else
                        {
                            successMessage = "Saved Successfully";
                        }
                    }

                    if (SystemInstallation != "")
                        objLocationNEmployeeWiseActivityDaily.SystemInstallationToday = SystemInstallation == "0" ? false : true;
                    else
                        objLocationNEmployeeWiseActivityDaily.SystemInstallationToday = null;

                    if (SystemInstallationNextDay != "")
                        objLocationNEmployeeWiseActivityDaily.SystemInstallationTomorrow = SystemInstallationNextDay == "0" ? false : true;
                    else
                        objLocationNEmployeeWiseActivityDaily.SystemInstallationTomorrow = null;

                    if (InstallmentCollection != "")
                        objLocationNEmployeeWiseActivityDaily.InstallmentCollectionToday = InstallmentCollection == "0" ? false : true;
                    else
                        objLocationNEmployeeWiseActivityDaily.InstallmentCollectionToday = null;

                    if (InstallmentCollectionNextDay != "")
                    {
                        objLocationNEmployeeWiseActivityDaily.InstallmentCollectionTomorrow = InstallmentCollectionNextDay == "0" ? false : true;
                    }
                    else
                    {
                        objLocationNEmployeeWiseActivityDaily.InstallmentCollectionTomorrow = null;
                    }

                    if (CustomerVisit != "")
                        objLocationNEmployeeWiseActivityDaily.CustomerVisitForMarketingToday = CustomerVisit == "0" ? false : true;
                    else
                        objLocationNEmployeeWiseActivityDaily.CustomerVisitForMarketingToday = null;

                    if (CustomerVisitNextDay != "")
                    {
                        objLocationNEmployeeWiseActivityDaily.CustomerVisitForMarketingTomorrow = CustomerVisitNextDay == "0" ? false : true;
                    }
                    else
                    {
                        objLocationNEmployeeWiseActivityDaily.CustomerVisitForMarketingTomorrow = null;
                    }

                    if (Maintenance != "")
                        objLocationNEmployeeWiseActivityDaily.MaintenanceServicingToday = Maintenance == "0" ? false : true;
                    else
                        objLocationNEmployeeWiseActivityDaily.MaintenanceServicingToday = null;

                    if (MaintenanceNextDay != "")
                    {
                        objLocationNEmployeeWiseActivityDaily.MaintenanceServicingTomorrow = MaintenanceNextDay == "0" ? false : true;
                    }
                    else
                    {
                        objLocationNEmployeeWiseActivityDaily.MaintenanceServicingTomorrow = null;
                    }
                    if (BankOperation != "")
                        objLocationNEmployeeWiseActivityDaily.BankOperationToday = BankOperation == "0" ? false : true;
                    else
                        objLocationNEmployeeWiseActivityDaily.BankOperationToday = null;

                    if (BankOperationNextDay != "")
                    {
                        objLocationNEmployeeWiseActivityDaily.BankOperationTomorrow = BankOperationNextDay == "0" ? false : true;
                    }
                    else
                    {
                        objLocationNEmployeeWiseActivityDaily.BankOperationTomorrow = null;
                    }

                    if (OfficialVisit != "")
                        objLocationNEmployeeWiseActivityDaily.OfficialVisitToOtherUnitToday = OfficialVisit == "0" ? false : true;
                    else
                        objLocationNEmployeeWiseActivityDaily.OfficialVisitToOtherUnitToday = null;

                    if (OfficialVisitNextDay != "")
                    {
                        objLocationNEmployeeWiseActivityDaily.OfficialVisitToOtherUnitTomorrow = OfficialVisitNextDay == "0" ? false : true;
                    }
                    else
                    {
                        objLocationNEmployeeWiseActivityDaily.OfficialVisitToOtherUnitTomorrow = null;
                    }
                    if (Meeting != "")
                        objLocationNEmployeeWiseActivityDaily.MeetingToday = Meeting == "0" ? false : true;
                    else
                        objLocationNEmployeeWiseActivityDaily.MeetingToday = null;

                    if (MeetingNextDay != "")
                    {
                        objLocationNEmployeeWiseActivityDaily.MeetingTomorrow = MeetingNextDay == "0" ? false : true;
                    }
                    else
                    {
                        objLocationNEmployeeWiseActivityDaily.MeetingTomorrow = null;
                    }

                    objLocationNEmployeeWiseActivityDaily.TAToday = Convert.ToDecimal(TAAmount != "" ? TAAmount : "0");
                    objLocationNEmployeeWiseActivityDaily.TATomorrow = Convert.ToDecimal(LeaveNextDay != "" ? LeaveNextDay : "0");

                    objLocationNEmployeeWiseActivityDaily.ReasonForTAToday = TAReason;

                    if (Leave != "")
                        objLocationNEmployeeWiseActivityDaily.WasInLeaveToday = Leave == "0" ? false : true;
                    else
                        objLocationNEmployeeWiseActivityDaily.WasInLeaveToday = null;

                    if (DeskWork != "")
                        objLocationNEmployeeWiseActivityDaily.DeskWorksToday = DeskWork == "0" ? false : true;
                    else
                        objLocationNEmployeeWiseActivityDaily.DeskWorksToday = null;

                    objLocationNEmployeeWiseActivityDaily.RemarksToday = Remarks;

                    //objLocationNEmployeeWiseActivityDaily.DataEntryInERP = DataEntryinERP == "0" ? false : true;
                    //objLocationNEmployeeWiseActivityDaily.EntryTime = DateTime.Now;

                    objLocationNEmployeeWiseActivityDaily = salesDal.UpdateLocationNEmployeeWiseActivityDaily(objLocationNEmployeeWiseActivityDaily);

                    if (objLocationNEmployeeWiseActivityDaily != null)
                    {
                        successMessage = "Saved Successfully";
                    }
                    else
                    {
                        successMessage = "Failed To Save";
                    }
                }
                else
                {
                    Sal_LocationNEmployeeWiseActivityDaily objNewLocationNEmployeeWiseActivityDaily = new Sal_LocationNEmployeeWiseActivityDaily();

                    objNewLocationNEmployeeWiseActivityDaily.LocationCode = objLoginHelper.LogInForUnitCode;
                    objNewLocationNEmployeeWiseActivityDaily.EmployeeID = EmployeeID;
                    objNewLocationNEmployeeWiseActivityDaily.YearMonth = objLoginHelper.YearMonthCurrent;
                    objNewLocationNEmployeeWiseActivityDaily.TransDate = Convert.ToDateTime(TransactionDate).Date;
                    objNewLocationNEmployeeWiseActivityDaily.SalesAchievementToday = Convert.ToInt32(AchievementTodaySales != "" ? AchievementTodaySales : "0");
                    objNewLocationNEmployeeWiseActivityDaily.RegColcAchievementToday = Convert.ToInt32(AchievementTodayCollection != "" ? AchievementTodayCollection : "0");
                    objNewLocationNEmployeeWiseActivityDaily.OverdueColcAchievementToday = Convert.ToInt32(AchievementTodayOverdue != "" ? AchievementTodayOverdue : "0");

                    if (SystemInstallation != "")
                        objNewLocationNEmployeeWiseActivityDaily.SystemInstallationToday = SystemInstallation == "0" ? false : true;
                    if (SystemInstallationNextDay != "")
                    {
                        objNewLocationNEmployeeWiseActivityDaily.SystemInstallationTomorrow = SystemInstallationNextDay == "0" ? false : true;
                    }

                    if (InstallmentCollection != "")
                        objNewLocationNEmployeeWiseActivityDaily.InstallmentCollectionToday = InstallmentCollection == "0" ? false : true;

                    if (InstallmentCollectionNextDay != "")
                    {
                        objNewLocationNEmployeeWiseActivityDaily.InstallmentCollectionTomorrow = InstallmentCollectionNextDay == "0" ? false : true;
                    }

                    if (CustomerVisit != "")
                        objNewLocationNEmployeeWiseActivityDaily.CustomerVisitForMarketingToday = CustomerVisit == "0" ? false : true;

                    if (CustomerVisitNextDay != "")
                    {
                        objNewLocationNEmployeeWiseActivityDaily.CustomerVisitForMarketingTomorrow = CustomerVisitNextDay == "0" ? false : true;
                    }


                    if (Maintenance != "")
                        objNewLocationNEmployeeWiseActivityDaily.MaintenanceServicingToday = Maintenance == "0" ? false : true;

                    if (MaintenanceNextDay != "")
                    {
                        objNewLocationNEmployeeWiseActivityDaily.MaintenanceServicingTomorrow = MaintenanceNextDay == "0" ? false : true;
                    }
                    if (BankOperation != "")
                        objNewLocationNEmployeeWiseActivityDaily.BankOperationToday = BankOperation == "0" ? false : true;
                    if (BankOperationNextDay != "")
                    {
                        objNewLocationNEmployeeWiseActivityDaily.BankOperationTomorrow = BankOperationNextDay == "0" ? false : true;
                    }
                    if (OfficialVisit != "")
                        objNewLocationNEmployeeWiseActivityDaily.OfficialVisitToOtherUnitToday = OfficialVisit == "0" ? false : true;
                    if (OfficialVisitNextDay != "")
                    {
                        objNewLocationNEmployeeWiseActivityDaily.OfficialVisitToOtherUnitTomorrow = OfficialVisitNextDay == "0" ? false : true;
                    }

                    if (Meeting != "")
                        objNewLocationNEmployeeWiseActivityDaily.MeetingToday = Meeting == "0" ? false : true;
                    if (MeetingNextDay != "")
                    {
                        objNewLocationNEmployeeWiseActivityDaily.MeetingTomorrow = MeetingNextDay == "0" ? false : true;
                    }

                    objNewLocationNEmployeeWiseActivityDaily.TAToday = Convert.ToDecimal(TAAmount != "" ? TAAmount : "0");
                    objNewLocationNEmployeeWiseActivityDaily.TATomorrow = Convert.ToDecimal(taAmountNextDay != "" ? taAmountNextDay : "0");

                    objNewLocationNEmployeeWiseActivityDaily.ReasonForTAToday = TAReason != string.Empty ? TAReason.ToString() : "";
                    objNewLocationNEmployeeWiseActivityDaily.ReasonForTATomorrow = taReasonNextDay != string.Empty ? taAmountNextDay.ToString() : "";

                    if (Leave != "")
                        objNewLocationNEmployeeWiseActivityDaily.WasInLeaveToday = Leave == "0" ? false : true;

                    if (LeaveNextDay != "")
                    {
                        objNewLocationNEmployeeWiseActivityDaily.WillBeInLeaveTomorrow = LeaveNextDay == "0" ? false : true;
                    }

                    if (DeskWork != "")
                        objNewLocationNEmployeeWiseActivityDaily.DeskWorksToday = DeskWork == "0" ? false : true;
                    if (DeskWorkNextDay != "")
                    {
                        objNewLocationNEmployeeWiseActivityDaily.DeskWorksTomorrow = DeskWorkNextDay == "0" ? false : true;
                    }

                    if (EmployeeID.Trim() == Session["Manager"].ToString())
                    {
                        List<Sal_LocationNEmployeeWiseActivityDaily> lstEntryCount = new List<Sal_LocationNEmployeeWiseActivityDaily>();

                        lstEntryCount = salesDal.ReadLocationNEmployeeWiseActivityDaily(objLoginHelper.LocationCode, objLoginHelper.YearMonthCurrent, Convert.ToDateTime(TransactionDate).Date);

                        if (lstEntryCount.Count == Convert.ToInt32(hfCountEmployee))
                        {
                            successMessage = salesDal.ExecuteDayEndProcess(objLoginHelper.LogInForUnitCode, Convert.ToDateTime(TransactionDate).ToString("dd-MMM-yyyy"));
                        }
                        //else if (CloseCheckBox == true)
                        //{
                        //    successMessage = salesDal.ExecuteDayEndProcess(objLoginHelper.LogInForUnitCode, Convert.ToDateTime(TransactionDate).ToString("dd-MMM-yyyy"));
                        //}

                        objNewLocationNEmployeeWiseActivityDaily.SystemReturnToday = Convert.ToInt32(SystemReturn != "" ? SystemReturn : "0");
                        objNewLocationNEmployeeWiseActivityDaily.SystemReSaleToday = Convert.ToInt32(SystemReSale != "" ? SystemReSale : "0");
                        objNewLocationNEmployeeWiseActivityDaily.CustomerTrainingToday = Convert.ToInt32(CustomerTraining != "" ? CustomerTraining : "0");
                        objNewLocationNEmployeeWiseActivityDaily.BioGasSalesToday = Convert.ToInt32(BioGasSale != "" ? BioGasSale : "0");
                        objNewLocationNEmployeeWiseActivityDaily.RSFBatterySalesToday = Convert.ToInt32(RSFBatterySales != "" ? RSFBatterySales : "0");
                    }


                    objNewLocationNEmployeeWiseActivityDaily.RemarksToday = Remarks;
                    objNewLocationNEmployeeWiseActivityDaily.RemarksTomorrow = RemarksNextDay;

                    //objNewLocationNEmployeeWiseActivityDaily.DataEntryInERP = DataEntryinERP == "0" ? false : true;
                    //objNewLocationNEmployeeWiseActivityDaily.EntryTime = DateTime.Now;

                    objNewLocationNEmployeeWiseActivityDaily = salesDal.CreateLocationNEmployeeWiseActivityDaily(objNewLocationNEmployeeWiseActivityDaily);

                    if (objNewLocationNEmployeeWiseActivityDaily != null)
                    {
                        successMessage = "Saved Successfully";
                    }
                    else
                    {
                        successMessage = "Failed To Save";
                    }
                }

                return new JsonResult { Data = successMessage };

            }
            catch (Exception ex) { throw ex; }

            return new JsonResult { Data = successMessage };
        }

        public JsonResult EmployeeTargetSave(string employeeID, string salTarget, string collectionTarget, string overdueTarget)
        {

            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            string message = string.Empty;
            Common_PeriodOpenClose objPeriodOpenClose = new Common_PeriodOpenClose();
            objPeriodOpenClose = salesDal.ReadPeriodOpenClose(objLoginHelper.LogInForUnitCode);
            //objLoginHelper.YearMonthCurrent = Convert.ToDateTime(objPeriodOpenClose.CalenderDate).ToString("yyyyMM");

            Sal_LocationWiseSalesNCollectionTarget objLocationWiseSalesNCollectionTarget = new Sal_LocationWiseSalesNCollectionTarget();
            objLocationWiseSalesNCollectionTarget = salesDal.ReadLocationWiseSalesNCollectionTarget(objLoginHelper.LogInForUnitCode, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForDailyProgressReview));

            bool IsEmployeeWiseTargetSettingCompleted = false;
            IsEmployeeWiseTargetSettingCompleted = objLocationWiseSalesNCollectionTarget.IsEmployeeWiseTargetSettingCompleted;

            if (IsEmployeeWiseTargetSettingCompleted == true)
            {
                message = "Change Is Not Allowed After Final Save";
            }
            else
            {
                //ViewBag.IsEmployeeWiseTargetSettingCompleted = IsEmployeeWiseTargetSettingCompleted == tr ? "Employee wise tagret set not yet completed for this month please check" : "";


                Sal_LocationNEmployeeWiseActivityMonthly objLocationNEmployeeWiseActivityMonthly = new Sal_LocationNEmployeeWiseActivityMonthly();
                objLocationNEmployeeWiseActivityMonthly = salesDal.ReadLocationNEmployeeWiseActivityMonthly(objLoginHelper.LogInForUnitCode, employeeID, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForDailyProgressReview));

                if (objLocationNEmployeeWiseActivityMonthly != null)
                {
                    objLocationNEmployeeWiseActivityMonthly.LocationCode = objLoginHelper.LogInForUnitCode;
                    objLocationNEmployeeWiseActivityMonthly.EmployeeID = employeeID;
                    objLocationNEmployeeWiseActivityMonthly.YearMonth = Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForDailyProgressReview);
                    objLocationNEmployeeWiseActivityMonthly.SalesCurrentMonthTarget = Convert.ToInt32(salTarget);
                    objLocationNEmployeeWiseActivityMonthly.RegColcCurrentMonthTarget = Convert.ToInt32(collectionTarget);
                    objLocationNEmployeeWiseActivityMonthly.OverdueColcCurrentMonthTarget = Convert.ToInt32(overdueTarget);

                    salesDal.UpdateLocationNEmployeeWiseActivityMonthly(objLocationNEmployeeWiseActivityMonthly);
                    message = "Saved Successfully";
                }
                else
                {
                    Sal_LocationNEmployeeWiseActivityMonthly objNewLocationNEmployeeWiseActivityMonthly = new Sal_LocationNEmployeeWiseActivityMonthly();
                    objNewLocationNEmployeeWiseActivityMonthly.LocationCode = objLoginHelper.LogInForUnitCode;
                    objNewLocationNEmployeeWiseActivityMonthly.EmployeeID = employeeID;
                    objNewLocationNEmployeeWiseActivityMonthly.YearMonth = Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForDailyProgressReview);
                    objNewLocationNEmployeeWiseActivityMonthly.SalesCurrentMonthTarget = Convert.ToInt32(salTarget);
                    objNewLocationNEmployeeWiseActivityMonthly.RegColcCurrentMonthTarget = Convert.ToInt32(collectionTarget);
                    objNewLocationNEmployeeWiseActivityMonthly.OverdueColcCurrentMonthTarget = Convert.ToInt32(overdueTarget);

                    salesDal.CreateLocationNEmployeeWiseActivityMonthly(objNewLocationNEmployeeWiseActivityMonthly);
                    message = "Saved Successfully";
                }

            }
            return new JsonResult { Data = message };
        }

        public JsonResult EmployeeWiseTargetSettingCompleted(string hfTotalEmployee)
        {
            //string salTarget, string collectionTarget, string overdueTarget
            bool targetFakeInputCheck = true;
            string message = string.Empty;
            Sal_LocationWiseSalesNCollectionTarget objLocationWiseSalesNCollectionTarget = new Sal_LocationWiseSalesNCollectionTarget();
            List<Sal_LocationNEmployeeWiseActivityMonthly> lstLocationNEmployeeWiseActivityMonthly = new List<Sal_LocationNEmployeeWiseActivityMonthly>();

            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            Common_PeriodOpenClose objPeriodOpenClose = new Common_PeriodOpenClose();
            objPeriodOpenClose = salesDal.ReadPeriodOpenClose(objLoginHelper.LogInForUnitCode);

            objLocationWiseSalesNCollectionTarget = salesDal.ReadLocationWiseSalesNCollectionTarget(objLoginHelper.LogInForUnitCode, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForDailyProgressReview));

            bool IsEmployeeWiseTargetSettingCompleted = false;
            IsEmployeeWiseTargetSettingCompleted = objLocationWiseSalesNCollectionTarget.IsEmployeeWiseTargetSettingCompleted;

            lstLocationNEmployeeWiseActivityMonthly = salesDal.ReadLocationNEmployeeWiseActivityMonthly(objLoginHelper.LogInForUnitCode, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForDailyProgressReview));

            List<LocationWiseEmployeeTargetEntryCheck> lstLocationWiseEmployeeTargetEntryCheck = new List<LocationWiseEmployeeTargetEntryCheck>();

            lstLocationWiseEmployeeTargetEntryCheck = salesDal.ReadLocationWiseEmployeeTargetEntryCheck(objLoginHelper.LogInForUnitCode, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForDailyProgressReview));

            if (IsEmployeeWiseTargetSettingCompleted == true)
            {
                message = "Change Is Not Allowed After Final Save";
            }
            else
            {
                List<GetLocationWiseEmployeeTarget> lstLocationWiseEmployeeTarget = new List<GetLocationWiseEmployeeTarget>();
                lstLocationWiseEmployeeTarget = salesDal.ReadGetLocationWiseEmployeeTarget(objLoginHelper.LogInForUnitCode, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForDailyProgressReview));


                objLocationWiseSalesNCollectionTarget = salesDal.ReadLocationWiseSalesNCollectionTarget(objLoginHelper.LogInForUnitCode, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForDailyProgressReview));

                int salesTragetDb = 0, collectionTargetDb = 0, overdueTargetDb = 0;

                salesTragetDb = (int)lstLocationWiseEmployeeTarget.Sum(t => t.SalesCurrentMonthTarget);
                collectionTargetDb = 0; //(int)lstLocationWiseEmployeeTarget.Sum(t => t.RegColcCurrentMonthTarget);
                overdueTargetDb = 0;// (int)lstLocationWiseEmployeeTarget.Sum(t => t.OverdueColcCurrentMonthTarget);

                int salTarget = 0, collectionTarget = 0, overdueTarget = 0;

                salTarget = Convert.ToInt32(objLocationWiseSalesNCollectionTarget.SalesTarget);
                collectionTarget = Convert.ToInt32(objLocationWiseSalesNCollectionTarget.RegularCollectionTarget);
                overdueTarget = Convert.ToInt32(objLocationWiseSalesNCollectionTarget.OverdueCollectionTarget);

                if (salesTragetDb < Convert.ToInt32(salTarget))
                {
                    targetFakeInputCheck = false;
                }
                else if ((collectionTargetDb > Convert.ToInt32(collectionTarget)) || (collectionTargetDb < Convert.ToInt32(collectionTarget)))
                {
                    targetFakeInputCheck = false;
                }
                else if ((overdueTargetDb > Convert.ToInt32(overdueTarget)) || (overdueTargetDb < Convert.ToInt32(overdueTarget)))
                {
                    targetFakeInputCheck = false;
                }
                else if (lstLocationWiseEmployeeTargetEntryCheck.Count > 0)
                {
                    targetFakeInputCheck = false;
                }

                if (targetFakeInputCheck == true)
                {
                    Sal_LocationWiseSalesNCollectionTarget objLocationWiseSalNCollectionTarget = new Sal_LocationWiseSalesNCollectionTarget();
                    objLocationWiseSalNCollectionTarget = salesDal.ReadLocationWiseSalesNCollectionTarget(objLoginHelper.LogInForUnitCode, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForDailyProgressReview));
                    objLocationWiseSalNCollectionTarget.IsEmployeeWiseTargetSettingCompleted = true;
                    salesDal.UpdateLocationWiseSalesNCollectionTarget(objLocationWiseSalNCollectionTarget);

                    message = "Saved Successfully";
                }
                else
                {
                    message = "Please Save All The Changes And Try Again";
                }
            }
            return new JsonResult { Data = message };
        }

        public JsonResult DailyProgressReviewDataEntryStatusSave(string unitCode)
        {
            string saveSuccessMessage = string.Empty;
            //objLoginHelper = (LoginHelper)Session["LogInInformation"];

            //tbl_UnitWiseEntryStatus objUnitWiseEntryStatus = new tbl_UnitWiseEntryStatus();
            //objUnitWiseEntryStatus = salesDal.UnitWiseEntryStatus(unitCode, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForDailyProgressReview));

            //if (objLoginHelper.Location == Helper.Zone)
            //{
            //    objUnitWiseEntryStatus.DPRFinalizedByZM_UserName = objLoginHelper.LogInID;
            //    objUnitWiseEntryStatus.DPRCheckedNFinalizedByZM = DateTime.Now;
            //}
            //else if (objLoginHelper.Location == Helper.Region)
            //{
            //    objUnitWiseEntryStatus.DPRFinalizedByRM_UserName = objLoginHelper.LogInID;
            //    objUnitWiseEntryStatus.DPRCheckedNFinalizedByRM = DateTime.Now;
            //}
            //else if (objLoginHelper.Location == Helper.HeadOffice)
            //{
            //    objUnitWiseEntryStatus.DPRFinalizedByHO_UserName = objLoginHelper.LogInID;
            //    objUnitWiseEntryStatus.DPRCheckedNFinalizedByHO = DateTime.Now;
            //}

            //objUnitWiseEntryStatus.UserName = objLoginHelper.LogInUserName;
            //objUnitWiseEntryStatus = salesDal.UpdateUnitWiseEntryStatus(objUnitWiseEntryStatus);

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

        public JsonResult SaveZoneWiseLocationNEmployeeWiseWeeklySalesNCollectionReportReamrks(string remarks, string openYearWeek)
        {
            //objLoginHelper = (LoginHelper)Session["LogInInformation"];

            string successMessage = string.Empty;

            //Sal_LocationWiseWeeklySalesNCollectionSummary objCollectionSummary = new Sal_LocationWiseWeeklySalesNCollectionSummary();
            //objCollectionSummary = salesDal.LocationWiseWeeklySalesNCollectionSummary(objLoginHelper.LogInForZoneCode, openYearWeek);

            //Sal_LocationWiseWeeklySalesNCollectionSummary objCollectionSummaryCreateUpdate = new Sal_LocationWiseWeeklySalesNCollectionSummary();

            //if (objCollectionSummary == null)
            //{
            //    objCollectionSummaryCreateUpdate = new Sal_LocationWiseWeeklySalesNCollectionSummary();

            //    objCollectionSummaryCreateUpdate.LocationCode = objLoginHelper.LogInForZoneCode;
            //    objCollectionSummaryCreateUpdate.YearWeek = openYearWeek;
            //    objCollectionSummaryCreateUpdate.Remarks = remarks;
            //    objCollectionSummaryCreateUpdate.EntryTime = DateTime.Now;

            //    objCollectionSummaryCreateUpdate = salesDal.CreateLocationWiseWeeklySalesNCollectionSummary(objCollectionSummaryCreateUpdate);

            //}
            //else
            //{
            //    objCollectionSummaryCreateUpdate = objCollectionSummary;

            //    objCollectionSummaryCreateUpdate.Remarks = remarks;
            //    objCollectionSummaryCreateUpdate.EntryTime = DateTime.Now;

            //    objCollectionSummaryCreateUpdate = salesDal.UpdateLocationWiseWeeklySalesNCollectionSummary(objCollectionSummaryCreateUpdate);
            //}

            //if (objCollectionSummaryCreateUpdate != null)
            //{
            //    successMessage = "Save Is Succeed";
            //}

            return new JsonResult { Data = successMessage };
        }

        [GridAction]
        public ActionResult _CustomerWiseOverdueBalance(string unitCode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (unitCode != null)
            {
                Session["To"] = 2;
            }

            if (Session["To"].ToString() == "2")
            {
                return View(new GridModel<CustomerWiseOverdueBalanceTracker>
                {
                    Data = salesDal.ReadCustomerWiseOverdueBalanceForTracker(objLoginHelper.LogInForUnitCode, objLoginHelper.CurrentYearWeek)
                });
            }
            else
            {
                return View(new GridModel<CustomerWiseOverdueBalanceTracker>
                {
                    Data = new List<CustomerWiseOverdueBalanceTracker>()
                });
            }
        }


        public ActionResult EmployeeVisitPlan()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

           

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
           // ViewBag.RouteCategory = hrmsDal.ReadPersonLocationWiseEmployee(objLoginHelper.LogInForUnitCode).Select(m => new { m.EmployeeID, m.EmployeeName }).Distinct().ToList();

            ViewBag.RouteCategory = salesDal.ReadPersonLocationWiseEmployee(objLoginHelper.LogInForUnitCode).Select(m => new { m.EmployeeID, m.EmployeeName }).Distinct().ToList(); 
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

        [GridAction]
        public ActionResult __EmployeeDetailsVisit(string option, string empId, string ddlLocationPart1, string ddlLocationPart4)
        {

            //try
            //{
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            return View(new GridModel<EmployeeVisit> { Data = salesDal.ReadEmployeeDetailsVisit(option, empId, objLoginHelper.LogInForUnitCode, ddlLocationPart1, ddlLocationPart4) });
            //}
            //catch (Exception ex)
            //{

            //return new GridModel<EmployeeVisit> { Data = ExceptionHelper.ExceptionMessage(ex) };
            //} 
        }
       
        public JsonResult LocationPart1Load(string empWisePart1,string empID) 
        {
            JsonResult jsR = new JsonResult();
            try
            {
                objLoginHelper = (LoginHelper)Session["LogInInformation"];
                string locationCode = objLoginHelper.LogInForUnitCode;
                ArrayList arr = new ArrayList();

                List<LocationPart1District> lstLocationPart1District = new List<LocationPart1District>();
                lstLocationPart1District = salesDal.ReadLocationPart1District(empWisePart1, empID, locationCode);

                foreach (LocationPart1District objlstLocationPart1District in lstLocationPart1District)
                {
                    arr.Add(new { Display = objlstLocationPart1District.LocationPart1_District, Value = objlstLocationPart1District.LocationPart1_District });
                }

                jsR.Data = arr;
                //return new JsonResult { Data = arr };
            }
            catch (Exception ex)
            {

                //return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
                jsR.Data = ExceptionHelper.ExceptionMessage(ex);
            }

            return jsR; //new JsonResult { Data = arr };
        }

        public JsonResult LocationPart2Load(string empWisePart2, string empID, string locationPart1)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            string locationCode = objLoginHelper.LogInForUnitCode;
            ArrayList arr = new ArrayList();

            List<LocationPart2Upo> lstLocationPart2District = new List<LocationPart2Upo>();
            lstLocationPart2District = salesDal.ReadLocationPart2District(empWisePart2, empID, locationCode, locationPart1);

            foreach (LocationPart2Upo objlstLocationPart2District in lstLocationPart2District)
            {
                arr.Add(new { Display = objlstLocationPart2District.LocationPart2, Value = objlstLocationPart2District.LocationPart2 });
            }

            return new JsonResult { Data = arr };
        }

        //[GridAction]
        //public ActionResult __EmployeeDetailsVisit(string empId)
        //{
        //    objLoginHelper = (LoginHelper)Session["LogInInformation"];

        //    return View(new GridModel<EmployeeVisit> { Data = inventoryDal.ReadEmployeeDetailsVisit(empId, objLoginHelper.LogInForUnitCode) });

        //}



     

        public ActionResult SummaryForTheDayClosing()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "SummaryForTheDayClosing", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            //ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            //ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            //ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");
            //ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            //ViewBag.UnitCode = objLoginHelper.LocationCode;

            //ViewBag.TopMenu = objLoginHelper.TopMenu;
            ViewBag.StoreLocation = inventoryDal.ReadStoreLocation();

            Common_PeriodOpenClose objPeriodOpenClose = new Common_PeriodOpenClose();
            objPeriodOpenClose = salesDal.ReadPeriodOpenClose(objLoginHelper.LocationCode.ToString());

            if (objPeriodOpenClose != null)
                TempData["PeriodOpenClose"] = Convert.ToDateTime(objPeriodOpenClose.CalenderDate).ToString("dddd,  MMMM  dd,  yyyy");
            else
                TempData["PeriodOpenClose"] = string.Empty;

            ViewBag.EnableDisableDayClose = "disabled=disabled";

            List<SummaryForTheDayClosingForInventoryInfo> lstInventorySummaryforTheDayClosing = new List<SummaryForTheDayClosingForInventoryInfo>();
            lstInventorySummaryforTheDayClosing = salesDal.ReadSummaryForTheDayClosingForInventoryInfo("INDIVIDUALUNIT", objLoginHelper.LocationCode.ToString(), 1);
            TempData["SummaryforTheDayClosingInventory"] = lstInventorySummaryforTheDayClosing;

            if (objLoginHelper.Location == Helper.Unit)
            {
                List<REPSummaryForTheDayClosingForSales> lstSummaryForTheDayClosingForSales = new List<REPSummaryForTheDayClosingForSales>();
                lstSummaryForTheDayClosingForSales = salesDal.ReadREPSummaryForTheDayClosingForSalesData("INDIVIDUALUNIT", objLoginHelper.LocationCode.ToString());
                TempData["DayClosingForSales"] = lstSummaryForTheDayClosingForSales;
                ViewBag.AchievementUpToYesterday = lstSummaryForTheDayClosingForSales.Sum(i => i.NumberOfSalesUptoYesterday).ToString();
                ViewBag.AchievementToday = lstSummaryForTheDayClosingForSales.Sum(i => i.NumberOfSalesToday).ToString();

                decimal AchievementUpToYesterDay = 0, AchievementToday = 0, CurrentMonthTarget = 0, AchievementStatus = 0;

                AchievementUpToYesterDay = lstSummaryForTheDayClosingForSales.Sum(i => i.NumberOfSalesUptoYesterday);
                AchievementToday = lstSummaryForTheDayClosingForSales.Sum(i => i.NumberOfSalesToday);
                CurrentMonthTarget = lstSummaryForTheDayClosingForSales[0].SalesTargetThisMonth;

                if (CurrentMonthTarget <= 0)
                {
                    CurrentMonthTarget = 1;
                }
                AchievementStatus = ((AchievementUpToYesterDay + AchievementToday) * 100) / CurrentMonthTarget;

                ViewBag.AchievementStatus = AchievementStatus.ToString("0.");

                List<SummaryForTheDayClosingForCollectionInfo> lstSummaryForTheDayClosingForCollectionInfo = new List<SummaryForTheDayClosingForCollectionInfo>();
                lstSummaryForTheDayClosingForCollectionInfo = salesDal.ReadSummaryForTheDayClosingForCollectionInfo("INDIVIDUALUNIT", objLoginHelper.LocationCode.ToString());
                TempData["DayClosingForCollection"] = lstSummaryForTheDayClosingForCollectionInfo;

                decimal CurrentCollectionAchievementStatus = 0, OverdueCollectionAchievementStatus = 0;

                if (lstSummaryForTheDayClosingForCollectionInfo[0].CurrentCollectionTargetThisMonth > 0)
                {
                    CurrentCollectionAchievementStatus = (Convert.ToDecimal(lstSummaryForTheDayClosingForCollectionInfo[0].CurrentCollectionAchievementThisMonthUptoToday) * 100) / (Convert.ToDecimal(lstSummaryForTheDayClosingForCollectionInfo[0].CurrentCollectionTargetThisMonth));
                    ViewBag.CurrentCollectionAchievementStatus = CurrentCollectionAchievementStatus.ToString("0.");
                }
                else
                {
                    ViewBag.CurrentCollectionAchievementStatus = 0;
                }
                if (lstSummaryForTheDayClosingForCollectionInfo[0].OverdueCollectionTargetThisMonth > 0)
                {
                    OverdueCollectionAchievementStatus = (Convert.ToDecimal(lstSummaryForTheDayClosingForCollectionInfo[0].OverdueCollectionAchievementThisMonthUptoToday) * 100) / (Convert.ToDecimal(lstSummaryForTheDayClosingForCollectionInfo[0].OverdueCollectionTargetThisMonth));
                    ViewBag.OverdueCollectionAchievementStatus = OverdueCollectionAchievementStatus.ToString("0.");
                }
                else
                {
                    ViewBag.OverdueCollectionAchievementStatus = 0;
                }

            }
            else
            {
                ViewBag.AchievementUpToYesterday = "";
                ViewBag.AchievementToday = "";
                ViewBag.AchievementStatus = "";
                ViewBag.CurrentCollectionAchievementStatus = 0;
                ViewBag.OverdueCollectionAchievementStatus = 0;
            }
            List<SummaryForTheDayClosingForAccountingInfo> lstSummaryDayClosingAccounting = new List<SummaryForTheDayClosingForAccountingInfo>();
            lstSummaryDayClosingAccounting = salesDal.ReadSummaryForTheDayClosingForAccountingInfo("INDIVIDUALUNIT", objLoginHelper.LocationCode.ToString());
            TempData["DayClosingForAccounting"] = lstSummaryDayClosingAccounting;

            decimal OpeningBalance = Convert.ToDecimal(lstSummaryDayClosingAccounting[0].CashInHandOpeningBalance) + Convert.ToDecimal(lstSummaryDayClosingAccounting[0].CashAtBankOpeningBalance);
            ViewBag.OpeningBalance = OpeningBalance.ToString("0.");

            decimal Inflow = Convert.ToDecimal(lstSummaryDayClosingAccounting[0].CashInHandInFlow) + Convert.ToDecimal(lstSummaryDayClosingAccounting[0].CashAtBankInFlow);
            ViewBag.Inflow = Inflow.ToString("0.");

            decimal OutFlow = Convert.ToDecimal(lstSummaryDayClosingAccounting[0].CashInHandOutFlow) + Convert.ToDecimal(lstSummaryDayClosingAccounting[0].CashAtBankOutFlow);
            ViewBag.OutFlow = OutFlow.ToString("0.");

            decimal ClosingBalance = Convert.ToDecimal(lstSummaryDayClosingAccounting[0].CashInHandClosingBalance) + Convert.ToDecimal(lstSummaryDayClosingAccounting[0].CashAtBankClosingBalance);
            ViewBag.ClosingBalance = ClosingBalance.ToString("0.");

            return View();
        }

        [GridAction]
        public ActionResult __LoadInventoryItemLocationWise(string storeLocation)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<SummaryForTheDayClosingForInventoryInfo> lstInventoryInfo = new List<SummaryForTheDayClosingForInventoryInfo>();
            lstInventoryInfo = salesDal.ReadSummaryForTheDayClosingForInventoryInfo("INDIVIDUALUNIT", objLoginHelper.LocationCode, Convert.ToByte(storeLocation));
            return View(new GridModel<SummaryForTheDayClosingForInventoryInfo>
            {
                Data = lstInventoryInfo
            });
        }

        public JsonResult SaveSummaryForTheDayClose(bool salesEntryStatus, bool collectionStatus, bool inventoryStatus, bool accountingStatus, string TransactionDate)
        {
            string successMessage = string.Empty;
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            try
            {

                if (objLoginHelper.Location == Helper.Zone || objLoginHelper.Location == Helper.HeadOffice)
                {
                    successMessage = salesDal.ExecuteDayEndProcess(objLoginHelper.LocationCode, Convert.ToDateTime(TransactionDate).ToString("dd-MMM-yyyy"));
                }
                else if (salesEntryStatus == true && collectionStatus == true && inventoryStatus == true && accountingStatus == true)
                {
                    successMessage = salesDal.ExecuteDayEndProcess(objLoginHelper.LogInForUnitCode, Convert.ToDateTime(TransactionDate).ToString("dd-MMM-yyyy"));
                }

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Day is Closed Successfully") };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public JsonResult UpdateLoadRequestEntry(string locationCode, string employeeID, string dayOpenDay, string rsfServiceQunt, string prmCorporatePhoneNo, string paywellServiceQunt, Int32 gridLength, string cashMemo)  
        {


            string successMessage = string.Empty;
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            string finalSave = "0";

            Int32 countLengt = 0;

            string dayOpenDayTocurrentdate = DateTime.Now.ToString("dd-MMM-yyyy");

            try
            {
                countLengt = (Int32)Session["Count"];
            }

            catch
            {
                countLengt = 0;
            }


            try
            {
               
                //if (objLoginHelper.Location == Helper.Zone || objLoginHelper.Location == Helper.HeadOffice)
                //{
                successMessage = salesDal.UpdateLoadRequestEntry(locationCode, employeeID, dayOpenDayTocurrentdate, prmCorporatePhoneNo, rsfServiceQunt, paywellServiceQunt, cashMemo);

               
                Session["Count"] = countLengt + 1;

                countLengt = (Int32)Session["Count"];

                if (countLengt==gridLength)
                {
                    finalSave = "1";
                    Session["Count"] = 0; 
                    return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty) };
                    
                }

                if (finalSave == "1")
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

        public JsonResult UpdateLoadRequestEntryFinalSave(string txtRsfServiceTotal, string txtRsfPayWellTotal)
        {


            string successMessage = string.Empty;
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            string locationCode = objLoginHelper.LocationCode.ToString();

            string dayOpenDayTocurrentdate = DateTime.Now.ToString("dd-MMM-yyyy");

            try
            {
                successMessage = salesDal.UpdateLoadRequestEntryFinalSave(locationCode, dayOpenDayTocurrentdate, txtRsfServiceTotal, txtRsfPayWellTotal);

               return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty) };

            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }


        public JsonResult EntryEmployeeVisitPlan(string prmEmployeeID, string prmMovementDate, string prmTTLocationCode, string prmNoFcustomerVisit, string prmPurposeOfVisit, string prmDurationOfVisitInHours, Int32 gridLength)
        {


            string successMessage = string.Empty;
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            string finalSave = "0";

            Int32 countLengt = 0;
            Int32 countLengthEx = 0;
            Int32 lastEx = 0;

            //string dayOpenDayTocurrentdate = prmMovementDate.ToString("dd-MMM-yyyy");
            string prmLocationCode = objLoginHelper.LogInForUnitCode.ToString();
            string prmUserName = objLoginHelper.LogInID.ToString();
            string prmDBTransType = "INSERT";

            try
            {
                countLengt = (Int32)Session["Count"];
               
            }

            catch
            {
                countLengt = 0;
               
            }


            try
            {
                
                //if (objLoginHelper.Location == Helper.Zone || objLoginHelper.Location == Helper.HeadOffice)
                //{
                successMessage = salesDal.EntryEmployeeVisitPlan(prmLocationCode, prmEmployeeID, prmMovementDate, prmTTLocationCode, prmNoFcustomerVisit, prmPurposeOfVisit, prmDurationOfVisitInHours, prmUserName, prmDBTransType);


                Session["Count"] = countLengt + 1;
                 
                countLengt = (Int32)Session["Count"];

                if (countLengt == gridLength)
                {
                    finalSave = "1";
                    Session["Count"] = 0;
                    return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty) };

                }

                if (finalSave == "1")
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

                ////Session["lastEx"] = 1;
                ////lastEx = (Int32)Session["lastEx"];
                //if (countLengthEx == gridLength)
                //{
                //    Session["CountEx"] = 0;
                //   // Session["lastEx"] = 0;
                //    return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
                //}
                //else
                //    return new JsonResult { Data = ExceptionHelper.ExceptionMessageAdd(string.Empty) };

                return new JsonResult
                {

                    Data = ExceptionHelper.ExceptionMessage(ex)
                };
                    
            }
        }

        public JsonResult SaveEmployeeDailySalesTarget(string employeeWiseSalesTarget)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<Sal_LocationNEmployeeWiseDailySalesNCollectionTarget> lstEmployeeDailyTarget = new List<Sal_LocationNEmployeeWiseDailySalesNCollectionTarget>();
            Sal_LocationNEmployeeWiseDailySalesNCollectionTarget objEmployeeDailyTarget;

            Sal_LocationWiseSalesNCollectionTarget objLocationWiseSalesNCollectionTarget = new Sal_LocationWiseSalesNCollectionTarget();
            objLocationWiseSalesNCollectionTarget = salesDal.ReadLocationWiseSalesNCollectionTarget(objLoginHelper.LogInForUnitCode, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForDailyProgressReview));

            try
            {
                int columnCount = 0, totalDays = 0;

                string[] employeeDailyTarget = employeeWiseSalesTarget.Trim().Split(',');
                totalDays = employeeDailyTarget.Count();

                if (DateTime.IsLeapYear(objLoginHelper.MonthOpenForDailyProgressReview.Year) && objLoginHelper.MonthOpenForDailyProgressReview.Month == 2)
                {
                    totalDays = 30;
                }
                else if (objLoginHelper.MonthOpenForDailyProgressReview.Month == 2)
                {
                    totalDays = 29;
                }

                if (objLocationWiseSalesNCollectionTarget != null)
                {

                    for (columnCount = 1; columnCount < totalDays; columnCount++)
                    {
                        objEmployeeDailyTarget = new Sal_LocationNEmployeeWiseDailySalesNCollectionTarget();

                        objEmployeeDailyTarget.LocationCode = objLoginHelper.LocationCode;
                        objEmployeeDailyTarget.YearMonth = Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForDailyProgressReview);
                        objEmployeeDailyTarget.EmployeeID = employeeDailyTarget[0].Trim();
                        objEmployeeDailyTarget.TargetDate = Helper.DateFromDayAndYearMonth(columnCount, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForDailyProgressReview));
                        objEmployeeDailyTarget.SalesTarget = Convert.ToInt32(employeeDailyTarget[columnCount]);
                        objEmployeeDailyTarget.ModifiedBy = objLoginHelper.LogInID;
                        objEmployeeDailyTarget.ModifiedOn = DateTime.Now;
                        objEmployeeDailyTarget.EntryTime = DateTime.Now;

                        lstEmployeeDailyTarget.Add(objEmployeeDailyTarget);
                    }

                    salesDal.UpdateEmployeeWiseDailyTarget(lstEmployeeDailyTarget);

                    return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty) };
                }
                else
                {
                    return new JsonResult { Data = ExceptionHelper.ExceptionCustomErrorMessage("No Datat Found In Location Wise Sales And Collection Target Table.") };
                }

            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }

        }

        public JsonResult FinishedEmployeeDailySalesTarget()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            string message = string.Empty;
            Sal_LocationWiseSalesNCollectionTarget objLocationWiseSalesNCollectionTarget = new Sal_LocationWiseSalesNCollectionTarget();
            objLocationWiseSalesNCollectionTarget = salesDal.ReadLocationWiseSalesNCollectionTarget(objLoginHelper.LogInForUnitCode, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForDailyProgressReview));

            bool IsEmployeeWiseTargetSettingCompleted = false;
            if (objLocationWiseSalesNCollectionTarget != null)
            {
                IsEmployeeWiseTargetSettingCompleted = objLocationWiseSalesNCollectionTarget.IsEmployeeWiseTargetSettingCompleted;
            }
            try
            {
                if (IsEmployeeWiseTargetSettingCompleted == true)
                {
                    return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Daily Target Setting Is Already Locked") };
                }
                else
                {
                    if (objLocationWiseSalesNCollectionTarget != null)
                    {
                        Sal_LocationWiseSalesNCollectionTarget objLocationWiseSalNCollectionTarget = new Sal_LocationWiseSalesNCollectionTarget();
                        objLocationWiseSalNCollectionTarget = objLocationWiseSalesNCollectionTarget;
                        objLocationWiseSalNCollectionTarget.IsEmployeeWiseTargetSettingCompleted = true;
                        salesDal.UpdateLocationWiseSalesNCollectionTarget(objLocationWiseSalNCollectionTarget);

                        return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Save Is Succeed. Daily Target Setting Is Now Locked") };
                    }
                    else
                    {
                        return new JsonResult { Data = ExceptionHelper.ExceptionCustomErrorMessage("No Datat Found In Location Wise Sales And Collection Target Table.") };
                    }
                }
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public JsonResult LoadEmployee(string unitCode)
        {
            ArrayList arr = new ArrayList();

            List<GetLocationWiseEmployee> lstGetLocationWiseEmployee = new List<GetLocationWiseEmployee>();
            lstGetLocationWiseEmployee = salesDal.ReadGetLocationWiseEmployee(unitCode);

            foreach (GetLocationWiseEmployee emp in lstGetLocationWiseEmployee)
            {
                arr.Add(new { Display = emp.EmployeeName, Value = emp.EmployeeID });
            }

            return new JsonResult { Data = arr };
        }

        public ActionResult ProgressReviewGraphRelod(string reportType, string locationCode, string employeeId)
        {
            string graphReportType = string.Empty, graphLocationCode = string.Empty, graphEmployeeId = string.Empty;

            if (reportType == "RSFSUMMARY")
            {
                graphReportType = "RSFSUMMARY";
            }
            else if (reportType == "ZONESUMMARY")
            {
                graphReportType = "ZONESUMMARY";
                graphLocationCode = locationCode.Trim();
            }
            else if (reportType == "UNITSUMMARY")
            {
                graphReportType = "UNITSUMMARY";
                graphLocationCode = locationCode.Trim();
            }
            else if (reportType == "FORANEMPLOYEE")
            {
                graphReportType = "FORANEMPLOYEE";
                graphLocationCode = locationCode.Trim();
                graphEmployeeId = employeeId.Trim();
            }

            List<SalSalesNCollectionTargetVsAchievementForGraph> lstSalSalesNCollectionTargetVsAchievementForGraph = new List<SalSalesNCollectionTargetVsAchievementForGraph>();
            lstSalSalesNCollectionTargetVsAchievementForGraph = salesDal.ReadSalSalesNCollectionTargetVsAchievementForGraph(graphReportType, graphLocationCode, graphEmployeeId).ToList();

            ViewBag.Stack = 1;
            ViewBag.SeriesType = "line";
            ViewBag.ShowTitle = "Daily Progress";
            ViewBag.ShowLegend = "";
            ViewBag.LegendPosition = "";

            return Json(lstSalSalesNCollectionTargetVsAchievementForGraph);
        }

        public ActionResult SalesRecoveryCommitmentReview()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "ODRecoveryCommitmentReview", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            SalesRecoveryCommitmentByReviewViewModel objSalesRecoveryCommitmentByReviewViewModel = new SalesRecoveryCommitmentByReviewViewModel();
            objSalesRecoveryCommitmentByReviewViewModel = salesDal.ReadSalesRecoveryCommitmentByReview("", "");

            return View(objSalesRecoveryCommitmentByReviewViewModel);
        }

        [HttpPost]
        public ActionResult SalesRecoveryCommitmentReview(SalesRecoveryCommitmentByReviewViewModel objSalesRecoveryCommitment)
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
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            SalesRecoveryCommitmentByReviewViewModel objSalesRecoveryCommitmentByReviewViewModel = new SalesRecoveryCommitmentByReviewViewModel();

            string reportType = string.Empty, locationCode = string.Empty;

            if (objLoginHelper.Location == Helper.HeadOffice)
            {
                reportType = string.IsNullOrEmpty(objSalesRecoveryCommitment.ZoneCode) ? Helper.HO : Helper.ZonalOffice;
                //locationCode = string.IsNullOrEmpty(objSalesRecoveryCommitment.ZoneCode) ? objLoginHelper.LocationCode : objSalesRecoveryCommitment.ZoneCode;
                locationCode = "";
            }
            else if (objLoginHelper.Location == Helper.Zone)
            {
                reportType = Helper.ZonalOffice;
                locationCode = objLoginHelper.LocationCode;
            }
            else if (objLoginHelper.Location == Helper.Region)
            {
                reportType = Helper.AreaOffice;
                locationCode = objLoginHelper.LocationCode;

                //reportType = "RSFSUMMARY";
                //locationCode = "";
            }

            Session["ReportTypeZoneCode"] = reportType + "," + locationCode;

            objSalesRecoveryCommitmentByReviewViewModel = salesDal.ReadSalesRecoveryCommitmentByReview(reportType, locationCode);

            objSalesRecoveryCommitmentByReviewViewModel.ODCustomerGrading = objSalesRecoveryCommitment.ODCustomerGrading;
            objSalesRecoveryCommitmentByReviewViewModel.ZoneCode = objSalesRecoveryCommitment.ZoneCode;

            return View(objSalesRecoveryCommitmentByReviewViewModel);
        }


        [GridAction(GridName = "gvSalesRecoveryCommitmentReview")]
        public ActionResult SalesRecoveryCommitmentReviewGridPaging(SalesRecoveryCommitmentByReviewViewModel objSalesRecoveryCommitment)
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
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            SalesRecoveryCommitmentByReviewViewModel objSalesRecoveryCommitmentByReviewViewModel = new SalesRecoveryCommitmentByReviewViewModel();

            string reportType = string.Empty, locationCode = string.Empty;

            if (objLoginHelper.Location == Helper.HeadOffice)
            {
                reportType = string.IsNullOrEmpty(objSalesRecoveryCommitment.ZoneCode) ? Helper.HO : Helper.ZonalOffice;
                //locationCode = string.IsNullOrEmpty(objSalesRecoveryCommitment.ZoneCode) ? objLoginHelper.LocationCode : objSalesRecoveryCommitment.ZoneCode;
                locationCode = "";
            }
            else if (objLoginHelper.Location == Helper.Zone)
            {
                reportType = Helper.ZonalOffice;
                locationCode = objLoginHelper.LocationCode;
            }
            else if (objLoginHelper.Location == Helper.Region)
            {
                reportType = Helper.AreaOffice;
                locationCode = objLoginHelper.LocationCode;
            }


            objSalesRecoveryCommitmentByReviewViewModel = salesDal.ReadSalesRecoveryCommitmentByReview(reportType, locationCode);

            objSalesRecoveryCommitmentByReviewViewModel.ODCustomerGrading = objSalesRecoveryCommitment.ODCustomerGrading;
            objSalesRecoveryCommitmentByReviewViewModel.ZoneCode = objSalesRecoveryCommitment.ZoneCode;

            return View("SalesRecoveryCommitmentReview", objSalesRecoveryCommitmentByReviewViewModel);
        }

      
        public JsonResult SaveSalesRecoveryCommitment(string option, string unitCode, string amComment, string zmComment)
        {
            try
            {
                objLoginHelper = (LoginHelper)Session["LogInInformation"];

                //Sal_ODRecoveryCommitmentByRMnZM objOdCommitment = new Sal_ODRecoveryCommitmentByRMnZM();
                //objOdCommitment.UnitCode = unitCode;
                //objOdCommitment.YearMonth = Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForSales);

                //if (objLoginHelper.Location == Helper.Zone)
                //{
                //    objOdCommitment.LastZMCommitmentToRecoverRemainingOD = zmComment;
                //    objOdCommitment.LastZMRemarksOn = DateTime.Now;
                //}

                //else if (objLoginHelper.Location == Helper.Region)
                //{
                //    objOdCommitment.LastRMCommitmentToRecoverRemainingOD = rmComment;
                //    objOdCommitment.LastRMRemarksOn = DateTime.Now;
                //}

                //salesDal.UpdateODRecoveryCommitmentByRMnZM(objOdCommitment);

                SalesRecoveryStatusEntryMonitoring objSalesRecoveryStatusEntryMonitoring = new SalesRecoveryStatusEntryMonitoring();

                List<SalesRecoveryStatusEntryMonitoring> lstSalesRecoveryStatusEntryMonitoring = new List<SalesRecoveryStatusEntryMonitoring>();

                string locationCode = objLoginHelper.LocationCode;


                lstSalesRecoveryStatusEntryMonitoring = salesDal.UpdateSalesRecoveryStatusEntryMonitoring(option, unitCode, objSalesRecoveryStatusEntryMonitoring.CustomerCode, objSalesRecoveryStatusEntryMonitoring.UMNextRecoveryDateIfRemainingODBalance, objSalesRecoveryStatusEntryMonitoring.UMLastRemarks, " ", amComment);

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("") };
            }
            catch (Exception ex)
            {

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }  
    }

        public ActionResult ShowUnitWiseOdStatus(string unitCode, string unitName)
        {
            ODRecoveryStatusMonitoring objODRecoveryStatusMonitoring = new ODRecoveryStatusMonitoring();
            objODRecoveryStatusMonitoring.UnitCode = unitCode;
            objODRecoveryStatusMonitoring.UnitName = unitName;
            objODRecoveryStatusMonitoring.CustomerFPR = string.Empty;
            objODRecoveryStatusMonitoring.IsOnlyForCollectionDatePassed = true;
            objODRecoveryStatusMonitoring.OdCustomerGrading = string.Empty;

            return RedirectToAction("ODRecoveryStatusMonitoringGridPaging", objODRecoveryStatusMonitoring);
        }

        public ActionResult ODRecoveryStatus()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "ODRecoveryStatus", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.CollectionDay = new SalesDay().SalesCollectionDay();
            ViewBag.CustomerFPR = hrmsData.ReadLocationWiseEmployee(objLoginHelper.LocationCode);

            return View();
        }

        [HttpPost]
        public ActionResult ODRecoveryStatus(ODRecoveryStatusMonitoring objODRecoveryStatusMonitoring)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            string locationCode = string.Empty;

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            if (!string.IsNullOrEmpty(objODRecoveryStatusMonitoring.UnitCode))
            {
                ViewBag.UnitTitle = "Unit:";
                ViewBag.UnitName = objODRecoveryStatusMonitoring.UnitName + " [" + objODRecoveryStatusMonitoring.UnitCode + "]";
            }

            if (string.IsNullOrEmpty(objODRecoveryStatusMonitoring.CustomerFPR))
                objODRecoveryStatusMonitoring.CustomerFPR = string.Empty;

            if (string.IsNullOrEmpty(objODRecoveryStatusMonitoring.OdCustomerGrading))
                objODRecoveryStatusMonitoring.OdCustomerGrading = string.Empty;

            if (string.IsNullOrEmpty(objODRecoveryStatusMonitoring.UnitCode))
                locationCode = objLoginHelper.LocationCode;
            else
                locationCode = objODRecoveryStatusMonitoring.UnitCode;

            List<ODRecoveryStatusMonitoring> lstODRecoveryStatusMonitoring = new List<ODRecoveryStatusMonitoring>();
            lstODRecoveryStatusMonitoring = salesDal.ReadODRecoveryStatusMonitoring(locationCode, objODRecoveryStatusMonitoring.IsOnlyForCollectionDatePassed, objODRecoveryStatusMonitoring.OdCustomerGrading, objODRecoveryStatusMonitoring.CustomerFPR, objODRecoveryStatusMonitoring.CollectionDayScheduledDay);

            if (lstODRecoveryStatusMonitoring.Count() > 0)
            {
                lstODRecoveryStatusMonitoring[0].UnitCode = objODRecoveryStatusMonitoring.UnitCode;
                lstODRecoveryStatusMonitoring[0].UnitName = objODRecoveryStatusMonitoring.UnitName;
                lstODRecoveryStatusMonitoring[0].OdCustomerGrading = objODRecoveryStatusMonitoring.OdCustomerGrading;
                lstODRecoveryStatusMonitoring[0].CustomerFPR = objODRecoveryStatusMonitoring.CustomerFPR;
                lstODRecoveryStatusMonitoring[0].IsOnlyForCollectionDatePassed = objODRecoveryStatusMonitoring.IsOnlyForCollectionDatePassed;
                lstODRecoveryStatusMonitoring[0].CollectionDayScheduledDay = objODRecoveryStatusMonitoring.CollectionDayScheduledDay;
            }
            else
            {
                lstODRecoveryStatusMonitoring.Add(objODRecoveryStatusMonitoring);
            }

            ViewBag.CollectionDay = new SalesDay().SalesCollectionDay();
            ViewBag.CustomerFPR = hrmsData.ReadLocationWiseEmployee(locationCode);

            return View(lstODRecoveryStatusMonitoring);
        }

        [GridAction(GridName = "gvODRecoveryStatusMonitoring")]
        public ActionResult ODRecoveryStatusMonitoringGridPaging(ODRecoveryStatusMonitoring objODRecoveryStatusMonitoring)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            string locationCode = string.Empty;

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            if (!string.IsNullOrEmpty(objODRecoveryStatusMonitoring.UnitCode))
            {
                ViewBag.UnitTitle = "Unit:";
                ViewBag.UnitName = objODRecoveryStatusMonitoring.UnitName + " [" + objODRecoveryStatusMonitoring.UnitCode + "]";
            }

            if (string.IsNullOrEmpty(objODRecoveryStatusMonitoring.CustomerFPR))
                objODRecoveryStatusMonitoring.CustomerFPR = string.Empty;

            if (string.IsNullOrEmpty(objODRecoveryStatusMonitoring.OdCustomerGrading))
                objODRecoveryStatusMonitoring.OdCustomerGrading = string.Empty;

            if (string.IsNullOrEmpty(objODRecoveryStatusMonitoring.UnitCode))
                locationCode = objLoginHelper.LocationCode;
            else
                locationCode = objODRecoveryStatusMonitoring.UnitCode;

            List<ODRecoveryStatusMonitoring> lstODRecoveryStatusMonitoring = new List<ODRecoveryStatusMonitoring>();
            lstODRecoveryStatusMonitoring = salesDal.ReadODRecoveryStatusMonitoring(locationCode, objODRecoveryStatusMonitoring.IsOnlyForCollectionDatePassed, objODRecoveryStatusMonitoring.OdCustomerGrading, objODRecoveryStatusMonitoring.CustomerFPR, objODRecoveryStatusMonitoring.CollectionDayScheduledDay);

            if (lstODRecoveryStatusMonitoring.Count() > 0)
            {
                lstODRecoveryStatusMonitoring[0].UnitCode = objODRecoveryStatusMonitoring.UnitCode;
                lstODRecoveryStatusMonitoring[0].UnitName = objODRecoveryStatusMonitoring.UnitName;
                lstODRecoveryStatusMonitoring[0].OdCustomerGrading = objODRecoveryStatusMonitoring.OdCustomerGrading;
                lstODRecoveryStatusMonitoring[0].CustomerFPR = objODRecoveryStatusMonitoring.CustomerFPR;
                lstODRecoveryStatusMonitoring[0].IsOnlyForCollectionDatePassed = objODRecoveryStatusMonitoring.IsOnlyForCollectionDatePassed;
                lstODRecoveryStatusMonitoring[0].CollectionDayScheduledDay = objODRecoveryStatusMonitoring.CollectionDayScheduledDay;
            }

            ViewBag.CollectionDay = new SalesDay().SalesCollectionDay();
            ViewBag.CustomerFPR = hrmsData.ReadLocationWiseEmployee(locationCode);

            return View("ODRecoveryStatus", lstODRecoveryStatusMonitoring);
        }

        public ActionResult SaveODRecoveryStatus(ODRecoveryStatusMonitoring objODRecoveryStatusMonitoring)
        {
            try
            {
                objLoginHelper = (LoginHelper)Session["LogInInformation"];

                Sal_ODCustomerGrading objODCustomerGrading = new Sal_ODCustomerGrading();
                objODCustomerGrading.CustomerCode = objODRecoveryStatusMonitoring.CustomerCode;
                objODCustomerGrading.YearMonth = Helper.YearMonthPrevious(Helper.ConvertDateToYearMonth(objLoginHelper.TransactionOpenDate), 1);

                objODCustomerGrading.LastUMNextRecoveryDateIfRemainingODBalance = Convert.ToDateTime(objODRecoveryStatusMonitoring.UMNextRecoveryDateIfRemainingODBalance).Date;
                objODCustomerGrading.LastUMRemarks = objODRecoveryStatusMonitoring.UMRemarks;
                objODCustomerGrading.ODGradeThisMonth = objODRecoveryStatusMonitoring.ODGradeThisMonth;

                objODCustomerGrading.LastUMRemarksOn = DateTime.Now;

                objODCustomerGrading = salesDal.UpdateODRecoveryStatusMonitoring(objODCustomerGrading);

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex, string.Empty) };
            }

        }

        public ActionResult ShowUnitWiseSalesStatus(string unitCode, string unitName, string defaultEmployeeAsFPR)
        {
            SalesRecoveryStatusEntryMonitoring objSalesRecoveryStatusEntryMonitoring= new SalesRecoveryStatusEntryMonitoring();
            objSalesRecoveryStatusEntryMonitoring.UnitCode = unitCode;
            objSalesRecoveryStatusEntryMonitoring.UnitName = unitName;
            objSalesRecoveryStatusEntryMonitoring.CustomerFPR = defaultEmployeeAsFPR;
            objSalesRecoveryStatusEntryMonitoring.IsOnlyForCollectionDatePassed = true;
            objSalesRecoveryStatusEntryMonitoring.OdCustomerGrading = string.Empty;

            return RedirectToAction("SalesRecoveryStatusMonitoringGridPaging", objSalesRecoveryStatusEntryMonitoring);
        }

        public ActionResult SalesRecoveryStatusEntry()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "SalesRecoveryStatusEntry", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.CollectionDay = new SalesDay().SalesCollectionDay();
            ViewBag.CustomerFPR = hrmsData.ReadLocationWiseEmployee(objLoginHelper.LocationCode);

            return View();
        }

        [HttpPost]
        public ActionResult SalesRecoveryStatusEntry(SalesRecoveryStatusEntryMonitoring objSalesRecoveryStatusEntryMonitoring)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            string locationCode = string.Empty;

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");
            ViewBag.OpenMonthYearForValidation = objLoginHelper.MonthOpenForDailyProgressReview.ToString("dd-MMM-yyyy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            if (!string.IsNullOrEmpty(objSalesRecoveryStatusEntryMonitoring.UnitCode))
            {
                ViewBag.UnitTitle = "Unit:";
                ViewBag.UnitName = objSalesRecoveryStatusEntryMonitoring.UnitName + " [" + objSalesRecoveryStatusEntryMonitoring.UnitCode + "]";
            }

            if (string.IsNullOrEmpty(objSalesRecoveryStatusEntryMonitoring.CustomerFPR))
                objSalesRecoveryStatusEntryMonitoring.CustomerFPR = string.Empty;

            if (string.IsNullOrEmpty(objSalesRecoveryStatusEntryMonitoring.OdCustomerGrading))
                objSalesRecoveryStatusEntryMonitoring.OdCustomerGrading = string.Empty;

            if (string.IsNullOrEmpty(objSalesRecoveryStatusEntryMonitoring.UnitCode))
                locationCode = objLoginHelper.LocationCode;
            else
                locationCode = objSalesRecoveryStatusEntryMonitoring.UnitCode;

            List<SalesRecoveryStatusEntryMonitoring> lstSalesRecoveryStatusEntryMonitoring = new List<SalesRecoveryStatusEntryMonitoring>();
            lstSalesRecoveryStatusEntryMonitoring = salesDal.ReadSalesRecoveryStatusEntryMonitoring(locationCode, objSalesRecoveryStatusEntryMonitoring.IsOnlyForCollectionDatePassed, objSalesRecoveryStatusEntryMonitoring.OdCustomerGrading, objSalesRecoveryStatusEntryMonitoring.CustomerFPR, objSalesRecoveryStatusEntryMonitoring.CollectionDayScheduledDay);

            if (lstSalesRecoveryStatusEntryMonitoring.Count() > 0)
            {
                lstSalesRecoveryStatusEntryMonitoring[0].UnitCode = objSalesRecoveryStatusEntryMonitoring.UnitCode;
                lstSalesRecoveryStatusEntryMonitoring[0].UnitName = objSalesRecoveryStatusEntryMonitoring.UnitName;
                lstSalesRecoveryStatusEntryMonitoring[0].OdCustomerGrading = objSalesRecoveryStatusEntryMonitoring.OdCustomerGrading;
                lstSalesRecoveryStatusEntryMonitoring[0].CustomerFPR = objSalesRecoveryStatusEntryMonitoring.CustomerFPR;
                lstSalesRecoveryStatusEntryMonitoring[0].IsOnlyForCollectionDatePassed = objSalesRecoveryStatusEntryMonitoring.IsOnlyForCollectionDatePassed;
                lstSalesRecoveryStatusEntryMonitoring[0].CollectionDayScheduledDay = objSalesRecoveryStatusEntryMonitoring.CollectionDayScheduledDay;
            }
            else
            {
                lstSalesRecoveryStatusEntryMonitoring.Add(objSalesRecoveryStatusEntryMonitoring);
            }

            ViewBag.CollectionDay = new SalesDay().SalesCollectionDay();
            ViewBag.CustomerFPR = hrmsData.ReadLocationWiseEmployee(locationCode);

            Sal_DailyBusinessPerformanceMonitoring_SalesAndRecoveryStatusByUnit objSal_DailyBusinessPerformanceMonitoring_SalesAndRecoveryStatusByUnit = salesDal.ReadDailyBusinessPerformanceMonitoringRemarks(Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForDailyProgressReview), locationCode);

            if (objSal_DailyBusinessPerformanceMonitoring_SalesAndRecoveryStatusByUnit !=null)
            ViewBag.UMLastOverallRemarks = objSal_DailyBusinessPerformanceMonitoring_SalesAndRecoveryStatusByUnit.UMLastOverallRemarks;

            return View(lstSalesRecoveryStatusEntryMonitoring);
        }

        [GridAction(GridName = "gvSalesRecoveryStatusMonitoring")]
        public ActionResult SalesRecoveryStatusMonitoringGridPaging(SalesRecoveryStatusEntryMonitoring objSalesRecoveryStatusEntryMonitoring)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            string locationCode = string.Empty;

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");
            ViewBag.OpenMonthYearForValidation = objLoginHelper.MonthOpenForDailyProgressReview.ToString("dd-MMM-yyyy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            if (!string.IsNullOrEmpty(objSalesRecoveryStatusEntryMonitoring.UnitCode))
            {
                ViewBag.UnitTitle = "Unit:";
                ViewBag.UnitName = objSalesRecoveryStatusEntryMonitoring.UnitName + " [" + objSalesRecoveryStatusEntryMonitoring.UnitCode + "]";
            }

            if (string.IsNullOrEmpty(objSalesRecoveryStatusEntryMonitoring.CustomerFPR))
                objSalesRecoveryStatusEntryMonitoring.CustomerFPR = string.Empty;

            if (string.IsNullOrEmpty(objSalesRecoveryStatusEntryMonitoring.OdCustomerGrading))
                objSalesRecoveryStatusEntryMonitoring.OdCustomerGrading = string.Empty;

            if (string.IsNullOrEmpty(objSalesRecoveryStatusEntryMonitoring.UnitCode))
                locationCode = objLoginHelper.LocationCode;
            else
                locationCode = objSalesRecoveryStatusEntryMonitoring.UnitCode;

            List<SalesRecoveryStatusEntryMonitoring> lstSalesRecoveryStatusEntryMonitoring = new List<SalesRecoveryStatusEntryMonitoring>();
            lstSalesRecoveryStatusEntryMonitoring = salesDal.ReadSalesRecoveryStatusEntryMonitoring(locationCode, objSalesRecoveryStatusEntryMonitoring.IsOnlyForCollectionDatePassed, objSalesRecoveryStatusEntryMonitoring.OdCustomerGrading, objSalesRecoveryStatusEntryMonitoring.CustomerFPR, objSalesRecoveryStatusEntryMonitoring.CollectionDayScheduledDay);

            if (lstSalesRecoveryStatusEntryMonitoring.Count() > 0)
            {
                lstSalesRecoveryStatusEntryMonitoring[0].UnitCode = objSalesRecoveryStatusEntryMonitoring.UnitCode;
                lstSalesRecoveryStatusEntryMonitoring[0].UnitName = objSalesRecoveryStatusEntryMonitoring.UnitName;
                lstSalesRecoveryStatusEntryMonitoring[0].OdCustomerGrading = objSalesRecoveryStatusEntryMonitoring.OdCustomerGrading;
                lstSalesRecoveryStatusEntryMonitoring[0].CustomerFPR = objSalesRecoveryStatusEntryMonitoring.CustomerFPR;
                lstSalesRecoveryStatusEntryMonitoring[0].IsOnlyForCollectionDatePassed = objSalesRecoveryStatusEntryMonitoring.IsOnlyForCollectionDatePassed;
                lstSalesRecoveryStatusEntryMonitoring[0].CollectionDayScheduledDay = objSalesRecoveryStatusEntryMonitoring.CollectionDayScheduledDay;
            }
            else
            {
                lstSalesRecoveryStatusEntryMonitoring.Add(objSalesRecoveryStatusEntryMonitoring);
            }

            ViewBag.CollectionDay = new SalesDay().SalesCollectionDay();
            ViewBag.CustomerFPR = hrmsData.ReadLocationWiseEmployee(locationCode);

            Sal_DailyBusinessPerformanceMonitoring_SalesAndRecoveryStatusByUnit objSal_DailyBusinessPerformanceMonitoring_SalesAndRecoveryStatusByUnit = salesDal.ReadDailyBusinessPerformanceMonitoringRemarks(Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForDailyProgressReview), locationCode);

            if (objSal_DailyBusinessPerformanceMonitoring_SalesAndRecoveryStatusByUnit != null)
                ViewBag.UMLastOverallRemarks = objSal_DailyBusinessPerformanceMonitoring_SalesAndRecoveryStatusByUnit.UMLastOverallRemarks;

            //return View(lstSalesRecoveryStatusEntryMonitoring);

            return View("SalesRecoveryStatusEntry", lstSalesRecoveryStatusEntryMonitoring);
        }

        public ActionResult SaveSalesRecoveryStatusEntry(string option, SalesRecoveryStatusEntryMonitoring objSalesRecoveryStatusEntryMonitoring, string umOverallRemarks)
        {
            try
            {
                objLoginHelper = (LoginHelper)Session["LogInInformation"];

                //Sal_ODCustomerGrading objODCustomerGrading = new Sal_ODCustomerGrading();
                //objODCustomerGrading.CustomerCode = objODRecoveryStatusMonitoring.CustomerCode;
                //objODCustomerGrading.YearMonth = Helper.YearMonthPrevious(Helper.ConvertDateToYearMonth(objLoginHelper.TransactionOpenDate), 1);

                //objODCustomerGrading.LastUMNextRecoveryDateIfRemainingODBalance = Convert.ToDateTime(objODRecoveryStatusMonitoring.UMNextRecoveryDateIfRemainingODBalance).Date;
                //objODCustomerGrading.LastUMRemarks = objODRecoveryStatusMonitoring.UMRemarks;
                //objODCustomerGrading.ODGradeThisMonth = objODRecoveryStatusMonitoring.ODGradeThisMonth;

                //objODCustomerGrading.LastUMRemarksOn = DateTime.Now;

                List<SalesRecoveryStatusEntryMonitoring> lstSalesRecoveryStatusEntryMonitoring = new List<SalesRecoveryStatusEntryMonitoring>();

                string locationCode = objLoginHelper.LocationCode;


                lstSalesRecoveryStatusEntryMonitoring = salesDal.UpdateSalesRecoveryStatusEntryMonitoring(option, locationCode, objSalesRecoveryStatusEntryMonitoring.CustomerCode, objSalesRecoveryStatusEntryMonitoring.UMNextRecoveryDateIfRemainingODBalance, objSalesRecoveryStatusEntryMonitoring.UMLastRemarks, umOverallRemarks, " ");

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex, string.Empty) };
            }

        }
        
        public JsonResult SaveCustomerStatusForSalesMonitoring(string customerCode, string isCustomerPoliticallyInfluential, string isCustomerALocalMuscleMan, string isCustomerNotWillingToPay)
        {
            try
            {
                Sal_CustomerStatus objCustomerStatus = new Sal_CustomerStatus();
                objCustomerStatus.CustomerCode = customerCode;
                objCustomerStatus.IsCustomerPoliticallyInfluential = isCustomerPoliticallyInfluential == "1" ? true : false;
                objCustomerStatus.IsCustomerALocalMuscleMan = isCustomerALocalMuscleMan == "1" ? true : false;
                objCustomerStatus.IsCustomerNotWillingToPay = isCustomerNotWillingToPay == "1" ? true : false;


                salesDal.UpdateCustomerStatusForSalesMonitoring(objCustomerStatus);

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public ActionResult DailyPerformanceMonitoring()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "DailyPerformanceMonitoring", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            return View();
        }

        [GridAction]
        public ActionResult _DailyPerformanceMonitoringForSales(string reportOption, string locationCode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            List<DailyPerformanceMonitoringForSales> lstDailyPerformanceMonitoringForSales = new List<DailyPerformanceMonitoringForSales>();

            if (reportOption == "RSFSUMMARY")
            {
                locationCode = string.Empty;
            }
            else
            {
                if (string.IsNullOrEmpty(locationCode))
                    locationCode = objLoginHelper.LocationCode;
            }

            lstDailyPerformanceMonitoringForSales = salesDal.ReadDailyPerformanceMonitoringForSales(reportOption, locationCode);

            DailyPerformanceMonitoringForSales objMonitoringForSalesColumnTotal = new DailyPerformanceMonitoringForSales();
            objMonitoringForSalesColumnTotal.LocationName = "Total: ";
            objMonitoringForSalesColumnTotal.SalesTarget_CurrentMonthTotal = lstDailyPerformanceMonitoringForSales.Sum(s => s.SalesTarget_CurrentMonthTotal);
            objMonitoringForSalesColumnTotal.SalesTarget_Yesterday = lstDailyPerformanceMonitoringForSales.Sum(s => s.SalesTarget_Yesterday);
            objMonitoringForSalesColumnTotal.SalesAchievement_Yesterday = lstDailyPerformanceMonitoringForSales.Sum(s => s.SalesAchievement_Yesterday);
            objMonitoringForSalesColumnTotal.SalesTarget_UpToDate = lstDailyPerformanceMonitoringForSales.Sum(s => s.SalesTarget_UpToDate);
            objMonitoringForSalesColumnTotal.SalesAchievement_UpToDate = lstDailyPerformanceMonitoringForSales.Sum(s => s.SalesAchievement_UpToDate);
            objMonitoringForSalesColumnTotal.SalesVarianceWithMonthlyTarget = lstDailyPerformanceMonitoringForSales.Sum(s => s.SalesVarianceWithMonthlyTarget);
            objMonitoringForSalesColumnTotal.RequiredTargetPerDay = lstDailyPerformanceMonitoringForSales.Sum(s => s.RequiredTargetPerDay);

            lstDailyPerformanceMonitoringForSales.Add(objMonitoringForSalesColumnTotal);

            return View(new GridModel<DailyPerformanceMonitoringForSales>
            {
                Data = lstDailyPerformanceMonitoringForSales
            });
        }

        [GridAction]
        public ActionResult _DailyBusinessPerformanceMonitoringCollection(string reportOption, string locationCode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            List<DailyBusinessPerformanceMonitoringCollection> lstDailyBusinessPerformanceMonitoringCollection = new List<DailyBusinessPerformanceMonitoringCollection>();

            if (reportOption == "RSFSUMMARY")
            {
                locationCode = string.Empty;
            }
            else
            {
                if (string.IsNullOrEmpty(locationCode))
                    locationCode = objLoginHelper.LocationCode;
            }

            lstDailyBusinessPerformanceMonitoringCollection = salesDal.ReadDailyBusinessPerformanceMonitoringCollection(reportOption, locationCode);

            DailyBusinessPerformanceMonitoringCollection objMonitoringCollectionColumnTotal = new DailyBusinessPerformanceMonitoringCollection();
            objMonitoringCollectionColumnTotal.LocationName = "Total: ";
            objMonitoringCollectionColumnTotal.RegularCollectionTarget_CurrentMonthTotal = lstDailyBusinessPerformanceMonitoringCollection.Sum(s => s.RegularCollectionTarget_CurrentMonthTotal);
            objMonitoringCollectionColumnTotal.RegularCollectionTarget_UpToDate = lstDailyBusinessPerformanceMonitoringCollection.Sum(s => s.RegularCollectionTarget_UpToDate);
            objMonitoringCollectionColumnTotal.RegularCollectionAchievement_UpToDate = lstDailyBusinessPerformanceMonitoringCollection.Sum(s => s.RegularCollectionAchievement_UpToDate);
            objMonitoringCollectionColumnTotal.OverdueBalance_LastMonth = lstDailyBusinessPerformanceMonitoringCollection.Sum(s => s.OverdueBalance_LastMonth);
            objMonitoringCollectionColumnTotal.OverdueCollectionTarget_UpToDate = lstDailyBusinessPerformanceMonitoringCollection.Sum(s => s.OverdueCollectionTarget_UpToDate);
            objMonitoringCollectionColumnTotal.OverdueCollectionAchievement_UpToDate = lstDailyBusinessPerformanceMonitoringCollection.Sum(s => s.OverdueCollectionAchievement_UpToDate);

            lstDailyBusinessPerformanceMonitoringCollection.Add(objMonitoringCollectionColumnTotal);

            return View(new GridModel<DailyBusinessPerformanceMonitoringCollection>
            {
                Data = lstDailyBusinessPerformanceMonitoringCollection
            });
        }

        [GridAction]
        public ActionResult _DailyBusinessPerformanceMonitoringIncreaseOrDecreaseComperison(string reportOption, string locationCode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            List<DailyBusinessPerformanceMonitoringNetODIncreaseOrDecreaseComperison> lstDailyBusinessPerformanceMonitoringIncreaseOrDecreaseComperison = new List<DailyBusinessPerformanceMonitoringNetODIncreaseOrDecreaseComperison>();

            if (reportOption == "RSFSUMMARY")
            {
                locationCode = string.Empty;
            }
            else
            {
                if (string.IsNullOrEmpty(locationCode))
                    locationCode = objLoginHelper.LocationCode;
            }

            lstDailyBusinessPerformanceMonitoringIncreaseOrDecreaseComperison = salesDal.ReadDailyBusinessPerformanceMonitoringIncreaseOrDecreaseComperison(reportOption, locationCode);

            DailyBusinessPerformanceMonitoringNetODIncreaseOrDecreaseComperison objIncreaseOrDecreaseComperisonColumnTotal = new DailyBusinessPerformanceMonitoringNetODIncreaseOrDecreaseComperison();
            objIncreaseOrDecreaseComperisonColumnTotal.LocationCode = "Total: ";
            objIncreaseOrDecreaseComperisonColumnTotal.ODRecoveryInLastMonth = lstDailyBusinessPerformanceMonitoringIncreaseOrDecreaseComperison.Sum(s => s.ODRecoveryInLastMonth);
            objIncreaseOrDecreaseComperisonColumnTotal.OverdueInLastMonth = lstDailyBusinessPerformanceMonitoringIncreaseOrDecreaseComperison.Sum(s => s.OverdueInLastMonth);
            objIncreaseOrDecreaseComperisonColumnTotal.NetODInLastMonth = lstDailyBusinessPerformanceMonitoringIncreaseOrDecreaseComperison.Sum(s => s.NetODInLastMonth);
            objIncreaseOrDecreaseComperisonColumnTotal.ODRecoveryInCurrentMonth = lstDailyBusinessPerformanceMonitoringIncreaseOrDecreaseComperison.Sum(s => s.ODRecoveryInCurrentMonth);
            objIncreaseOrDecreaseComperisonColumnTotal.OverdueInCurrentMonth = lstDailyBusinessPerformanceMonitoringIncreaseOrDecreaseComperison.Sum(s => s.OverdueInCurrentMonth);
            objIncreaseOrDecreaseComperisonColumnTotal.NetODInCurrentMonth = lstDailyBusinessPerformanceMonitoringIncreaseOrDecreaseComperison.Sum(s => s.NetODInCurrentMonth);
            objIncreaseOrDecreaseComperisonColumnTotal.NetODIncreasesOrDecreasesFromLastMonth = lstDailyBusinessPerformanceMonitoringIncreaseOrDecreaseComperison.Sum(s => s.NetODIncreasesOrDecreasesFromLastMonth);

            lstDailyBusinessPerformanceMonitoringIncreaseOrDecreaseComperison.Add(objIncreaseOrDecreaseComperisonColumnTotal);

            return View(new GridModel<DailyBusinessPerformanceMonitoringNetODIncreaseOrDecreaseComperison>
            {
                Data = lstDailyBusinessPerformanceMonitoringIncreaseOrDecreaseComperison
            });
        }

        [GridAction]
        public ActionResult _DailyBusinessPerformanceMonitoringSalesNCollectionTargetVsAchievement(string reportOption, string locationCode, string dateForTheStatus)
        {
            //string reportOption, string locationCode, DateTime dateForTheStatus

            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            List<DailyBusinessPerformanceMonitoringSalesNCollectionTargetVsAchievement> lstDailyBusinessPerformanceMonitoringSalesNCollectionTargetVsAchievement = new List<DailyBusinessPerformanceMonitoringSalesNCollectionTargetVsAchievement>();

            if (reportOption == "RSFSUMMARY")
            {
                locationCode = string.Empty;
            }
            else
            {
                if (string.IsNullOrEmpty(locationCode))
                    locationCode = objLoginHelper.LocationCode;
            }

            lstDailyBusinessPerformanceMonitoringSalesNCollectionTargetVsAchievement = salesDal.ReadDailyBusinessPerformanceMonitoringSalesNCollectionTargetVsAchievement(reportOption, locationCode, Convert.ToDateTime(dateForTheStatus));

            DailyBusinessPerformanceMonitoringSalesNCollectionTargetVsAchievement objCollectionTargetVsAchievementColumnTotal = new DailyBusinessPerformanceMonitoringSalesNCollectionTargetVsAchievement();
            objCollectionTargetVsAchievementColumnTotal.LocationName = "Total: ";
            objCollectionTargetVsAchievementColumnTotal.SalesAchievement_ForTheDay = lstDailyBusinessPerformanceMonitoringSalesNCollectionTargetVsAchievement.Sum(s => s.SalesAchievement_ForTheDay);
            objCollectionTargetVsAchievementColumnTotal.RegularCollectionAchievement_ForTheDay = lstDailyBusinessPerformanceMonitoringSalesNCollectionTargetVsAchievement.Sum(s => s.RegularCollectionAchievement_ForTheDay);
            objCollectionTargetVsAchievementColumnTotal.OverdueCollectionAchievement_ForTheDay = lstDailyBusinessPerformanceMonitoringSalesNCollectionTargetVsAchievement.Sum(s => s.OverdueCollectionAchievement_ForTheDay);

            lstDailyBusinessPerformanceMonitoringSalesNCollectionTargetVsAchievement.Add(objCollectionTargetVsAchievementColumnTotal);

            return View(new GridModel<DailyBusinessPerformanceMonitoringSalesNCollectionTargetVsAchievement>
            {
                Data = lstDailyBusinessPerformanceMonitoringSalesNCollectionTargetVsAchievement
            });
        }

        [GridAction]
        public ActionResult _DailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus(string reportOption, string locationCode)
        {
            //string reportOption, string locationCode, DateTime dateForTheStatus

            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            List<DailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus> lstDailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus = new List<DailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus>();

            if (reportOption == "RSFSUMMARY")
            {
                locationCode = string.Empty;
            }
            else
            {
                if (string.IsNullOrEmpty(locationCode))
                    locationCode = objLoginHelper.LocationCode;
            }

            lstDailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus = salesDal.ReadDailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus(reportOption, locationCode);

            DailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus objCollectionStatusColumnTotal = new DailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus();
            objCollectionStatusColumnTotal.LocationName = "Total: ";
            objCollectionStatusColumnTotal.Last3MonthsSystemReturn = lstDailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus.Sum(s => s.Last3MonthsSystemReturn);
            objCollectionStatusColumnTotal.Last3MonthsResale = lstDailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus.Sum(s => s.Last3MonthsResale);
            objCollectionStatusColumnTotal.ThisMonthOPBalance_NetSealable = lstDailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus.Sum(s => s.ThisMonthOPBalance_NetSealable);
            objCollectionStatusColumnTotal.ThisMonthSystemReturn = lstDailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus.Sum(s => s.ThisMonthSystemReturn);
            objCollectionStatusColumnTotal.ThisMonthResale = lstDailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus.Sum(s => s.ThisMonthResale);
            objCollectionStatusColumnTotal.ClosingBalance = lstDailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus.Sum(s => s.ClosingBalance);
            objCollectionStatusColumnTotal.SalesMay2013ToOnward_Qty = lstDailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus.Sum(s => s.SalesMay2013ToOnward_Qty);
            objCollectionStatusColumnTotal.DRFMay2013ToOnward_Qty = lstDailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus.Sum(s => s.DRFMay2013ToOnward_Qty);
            objCollectionStatusColumnTotal.DRFBacklog_Qty = lstDailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus.Sum(s => s.DRFBacklog_Qty);
            objCollectionStatusColumnTotal.ThisMonthSales_Qty = lstDailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus.Sum(s => s.ThisMonthSales_Qty);
            objCollectionStatusColumnTotal.ThisMonthDRFReceived_Qty = lstDailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus.Sum(s => s.ThisMonthDRFReceived_Qty);

            lstDailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus.Add(objCollectionStatusColumnTotal);

            return View(new GridModel<DailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus>
            {
                Data = lstDailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus
            });
        }

        [GridAction]
        public ActionResult _DailyBusinessPerformanceMonitoringOtherStatus(string reportOption, string locationCode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            List<DailyBusinessPerformanceMonitoringOtherStatus> lstDailyBusinessPerformanceMonitoringOtherStatus = new List<DailyBusinessPerformanceMonitoringOtherStatus>();

            //reportOption = "ZONEWISESTATUS";
            lstDailyBusinessPerformanceMonitoringOtherStatus = salesDal.ReadDailyBusinessPerformanceMonitoringOtherStatus(reportOption,locationCode);

            DailyBusinessPerformanceMonitoringOtherStatus objMonitoringOtherStatus = new DailyBusinessPerformanceMonitoringOtherStatus();
            objMonitoringOtherStatus.ZoneName = "Total: ";

            objMonitoringOtherStatus.NoOfActiveCustomer = lstDailyBusinessPerformanceMonitoringOtherStatus.Sum(s => s.NoOfActiveCustomer);
            objMonitoringOtherStatus.NoOfCustomerWithZeroCollectionThisMonth = lstDailyBusinessPerformanceMonitoringOtherStatus.Sum(s => s.NoOfCustomerWithZeroCollectionThisMonth);
            objMonitoringOtherStatus.MissingCollectionThisMonth_InPercentage = lstDailyBusinessPerformanceMonitoringOtherStatus.Sum(s => s.MissingCollectionThisMonth_InPercentage);
            objMonitoringOtherStatus.NoOfOngoingCustomerTotal = lstDailyBusinessPerformanceMonitoringOtherStatus.Sum(s => s.NoOfOngoingCustomerTotal);
            objMonitoringOtherStatus.OngoingCustomerInOD_Qty = lstDailyBusinessPerformanceMonitoringOtherStatus.Sum(s => s.OngoingCustomerInOD_Qty);
            objMonitoringOtherStatus.OngoingInOD_InPercentage = lstDailyBusinessPerformanceMonitoringOtherStatus.Sum(s => s.OngoingInOD_InPercentage);

            objMonitoringOtherStatus.OngoingCustomerInOD_ODAmount = lstDailyBusinessPerformanceMonitoringOtherStatus.Sum(s => s.OngoingCustomerInOD_ODAmount);
            objMonitoringOtherStatus.OngoingCustomerInODSettled_Qty = lstDailyBusinessPerformanceMonitoringOtherStatus.Sum(s => s.OngoingCustomerInODSettled_Qty);
            objMonitoringOtherStatus.NoOfLPOC = lstDailyBusinessPerformanceMonitoringOtherStatus.Sum(s => s.NoOfLPOC);
            objMonitoringOtherStatus.LPOC_InPercentage = lstDailyBusinessPerformanceMonitoringOtherStatus.Sum(s => s.LPOC_InPercentage);
            objMonitoringOtherStatus.LPOC_ODAmount = lstDailyBusinessPerformanceMonitoringOtherStatus.Sum(s => s.LPOC_ODAmount);
            objMonitoringOtherStatus.LPOCSettled_Qty = lstDailyBusinessPerformanceMonitoringOtherStatus.Sum(s => s.LPOCSettled_Qty);

            objMonitoringOtherStatus.LastMonthAdvanceCollection = lstDailyBusinessPerformanceMonitoringOtherStatus.Sum(s => s.LastMonthAdvanceCollection);
            objMonitoringOtherStatus.LastMonthAdvanceAdjustment = lstDailyBusinessPerformanceMonitoringOtherStatus.Sum(s => s.LastMonthAdvanceAdjustment);
            objMonitoringOtherStatus.ThisMonthAdvanceCollection = lstDailyBusinessPerformanceMonitoringOtherStatus.Sum(s => s.ThisMonthAdvanceCollection);
           
            lstDailyBusinessPerformanceMonitoringOtherStatus.Add(objMonitoringOtherStatus);

            return View(new GridModel<DailyBusinessPerformanceMonitoringOtherStatus>
            {
                Data = lstDailyBusinessPerformanceMonitoringOtherStatus
            });
        }

    }
}
