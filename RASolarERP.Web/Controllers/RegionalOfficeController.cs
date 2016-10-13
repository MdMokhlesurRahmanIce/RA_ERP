using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using RASolarERP.Web.Models;
using RASolarHelper;
using RASolarSecurity.Model;
using RASolarERP.Model;
using Telerik.Web.Mvc;

namespace RASolarERP.Web.Controllers
{
    public class RegionalOfficeController : Controller
    {
        private RASolarERPData erpDal = new RASolarERPData();
        LoginHelper objLoginHelper = new RASolarHelper.LoginHelper();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RegionManager()
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

        [GridAction]
        public ActionResult _DailyPerformanceMonitoringForSales(string reportOption, string locationCode)
        {
          

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
