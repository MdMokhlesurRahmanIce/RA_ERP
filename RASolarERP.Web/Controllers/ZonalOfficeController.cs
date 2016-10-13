using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using RASolarERP.Model;
using RASolarHelper;
using RASolarERP.Web.Models;
using Telerik.Web.Mvc;

namespace RASolarERP.Web.Controllers
{
    public class ZonalOfficeController : Controller
    {
        private RASolarERPData erpDal = new RASolarERPData();
        LoginHelper objLoginHelper = new RASolarHelper.LoginHelper();

        public ActionResult Index(string userLocation, string zoneCode)
        {
            return View();
        }

        public ActionResult ZoneManager()
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

        public ActionResult ZOSales()
        {
            return View();
        }

        public ActionResult ZOInventory()
        {
            return View();
        }


        [GridAction]
        public ActionResult _DailyPerformanceMonitoringForSales(string reportOption, string locationCode)
        {
            /*objLoginHelper = (LoginHelper)Session["LogInInformation"];
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

            lstDailyPerformanceMonitoringForSales.Add(objMonitoringForSalesColumnTotal);*/

            List<DailyZonalPerformanceMonitoring> lstDailyZonalPerformanceReport = new List<DailyZonalPerformanceMonitoring>();

            lstDailyZonalPerformanceReport = erpDal.DailyPerformanceMonitoringZonalReport();

            DailyZonalPerformanceMonitoring objDailyZonalPerformanceMonitoring = new DailyZonalPerformanceMonitoring();
            objDailyZonalPerformanceMonitoring.LocationName = "Total";
            objDailyZonalPerformanceMonitoring.Sales_Monthly_Target_Qty = lstDailyZonalPerformanceReport.Sum(s => s.Sales_Monthly_Target_Qty);
            objDailyZonalPerformanceMonitoring.Sales_Monthly_Achievement_Percent = lstDailyZonalPerformanceReport.Sum(s => s.Sales_Monthly_Achievement_Percent);
            objDailyZonalPerformanceMonitoring.Sales_UpToDate_Target_Qty = lstDailyZonalPerformanceReport.Sum(s => s.Sales_UpToDate_Target_Qty);
            objDailyZonalPerformanceMonitoring.Sales_UpToDate_Achievement_Qty = lstDailyZonalPerformanceReport.Sum(s => s.Sales_UpToDate_Achievement_Qty);
            objDailyZonalPerformanceMonitoring.Sales_UpToDate_Achievement_Percent = lstDailyZonalPerformanceReport.Sum(s => s.Sales_UpToDate_Achievement_Percent);
            objDailyZonalPerformanceMonitoring.Sales_OnTheDate_Target_Qty = lstDailyZonalPerformanceReport.Sum(s => s.Sales_OnTheDate_Target_Qty);
            objDailyZonalPerformanceMonitoring.Sales_OnTheDate_Achievement_Qty = lstDailyZonalPerformanceReport.Sum(s => s.Sales_OnTheDate_Achievement_Qty);
            objDailyZonalPerformanceMonitoring.Collection_Monthly_CC_Receivable_InMillion = lstDailyZonalPerformanceReport.Sum(s => s.Collection_Monthly_CC_Receivable_InMillion);
            objDailyZonalPerformanceMonitoring.Collection_Monthly_Total_Overdue_InMillion = lstDailyZonalPerformanceReport.Sum(s => s.Collection_Monthly_Total_Overdue_InMillion);
            objDailyZonalPerformanceMonitoring.Collection_UpToDate_Target_CC_Receivable_InMillion = lstDailyZonalPerformanceReport.Sum(s => s.Collection_UpToDate_Target_CC_Receivable_InMillion);
            objDailyZonalPerformanceMonitoring.Collection_UpToDate_Target_Overdue_Receivable_InMillion = lstDailyZonalPerformanceReport.Sum(s => s.Collection_UpToDate_Target_Overdue_Receivable_InMillion);
            objDailyZonalPerformanceMonitoring.Collection_UpToDate_Achievement_CC_Recovered_InMillion = lstDailyZonalPerformanceReport.Sum(s => s.Collection_UpToDate_Achievement_CC_Recovered_InMillion);
            objDailyZonalPerformanceMonitoring.Collection_UpToDate_Achievement_Overdue_Recovered_InMillion = lstDailyZonalPerformanceReport.Sum(s => s.Collection_UpToDate_Achievement_Overdue_Recovered_InMillion);
            objDailyZonalPerformanceMonitoring.Collection_UpToDate_Achievement_CC_Plus_OD_Percent = lstDailyZonalPerformanceReport.Sum(s => s.Collection_UpToDate_Achievement_CC_Plus_OD_Percent);
            objDailyZonalPerformanceMonitoring.DRF_New_Qty = lstDailyZonalPerformanceReport.Sum(s => s.DRF_New_Qty);
            objDailyZonalPerformanceMonitoring.DRF_Old_Qty = lstDailyZonalPerformanceReport.Sum(s => s.DRF_Old_Qty);
            objDailyZonalPerformanceMonitoring.System_Return_UpToDate = lstDailyZonalPerformanceReport.Sum(s => s.System_Return_UpToDate);
            objDailyZonalPerformanceMonitoring.Resales_UpToDate = lstDailyZonalPerformanceReport.Sum(s => s.Resales_UpToDate);

            lstDailyZonalPerformanceReport.Add(objDailyZonalPerformanceMonitoring);


            return View(new GridModel<DailyZonalPerformanceMonitoring>
            {
                Data = lstDailyZonalPerformanceReport
            });
        }
    }
}
