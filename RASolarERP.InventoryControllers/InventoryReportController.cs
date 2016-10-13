using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using RASolarERP.Model;
using RASolarHelper;

using RASolarERP.Web.Areas.Inventory.Models;
using RASolarERP.Web.Models;
using Telerik.Web.Mvc;
using RASolarERP.DomainModel.InventoryModel;

namespace RASolarERP.Web.Areas.Inventory.Controllers
{
    public class InventoryReportController : Controller
    {
        InventoryReportData reportDal = new InventoryReportData();
        InventoryData inventoryData = new InventoryData();
        RASolarSecurityData securityDal = new RASolarSecurityData();
        RASolarERPData raSolarErpDal = new RASolarERPData();
        LoginHelper objLoginHelper = new LoginHelper();
        HelperData helperDal = new HelperData();
        string message = string.Empty;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InventorySummaryToDetailView(string ym)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForInventory, "InventorySummaryToDetailView", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            string yearMonth = string.Empty;
            ViewBag.YearMonth = new YearMonthFormat().MonthFormat();

            if (Session["YearMonth"] != null)
            {
                yearMonth = Session["YearMonth"].ToString();

                List<InventorySummaryToDetailViewReport> lstReaportDetailView = new List<InventorySummaryToDetailViewReport>();
                List<ClosingInventoryValuation> lstClosingInventoryValuation = new List<ClosingInventoryValuation>();

                ym = ym.Replace(" ", "");
                ym = ym.Trim();

                if (ym != "")
                {
                    lstReaportDetailView = reportDal.ReadInventorySummaryToDetailViewReport(yearMonth, ym);//.Take(500).ToList();
                    TempData["detailsView"] = lstReaportDetailView;
                }

                ViewBag.YearMonth = new YearMonthFormat().MonthFormat();
                TempData["selectValue"] = yearMonth;

                //lstClosingInventoryValuation = raSolarErpDal.ClosingInventoryReport(yearMonth);
                //TempData["ClosingInventoryData"] = lstClosingInventoryValuation;

                //if (ym != "")
                //    ViewBag.ItemName = lstClosingInventoryValuation.Where(i => i.Item_Code.Replace(" ", "").Trim() == ym).Select(i => i.Item_Name).FirstOrDefault().ToString() + " [" + ym + "]";
            }
            else
            {
                Session.Remove("YearMonth");
                TempData["selectValue"] = "0";
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
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            return View();
        }

        [HttpPost]
        public ActionResult InventorySummaryToDetailView(FormCollection fCollection)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<ClosingInventoryValuation> lstClosingInventoryValuation = new List<ClosingInventoryValuation>();

            //ViewBag.YearMonth = new YearMonthFormat().MonthFormat();
            //TempData["selectValue"] = fCollection["ddlYearMonth"].ToString();

            //try
            //{
            //    lstClosingInventoryValuation = raSolarErpDal.ClosingInventoryReport(fCollection["ddlYearMonth"].ToString());
            //    TempData["ClosingInventoryData"] = lstClosingInventoryValuation;
            //}
            //catch(Exception ex)
            //{
            //    TempData["ClosingInventoryData"] = new List<ClosingInventoryValuation>(); 
            //}

            //Session["YearMonth"] = fCollection["ddlYearMonth"].ToString();

            //ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            //ViewBag.Location = objLoginHelper.Location;
            //ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            //ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            //ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            //ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            //ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            //ViewBag.UnitName = objLoginHelper.LogInForUnitName;

            //ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            //ViewBag.TopMenu = objLoginHelper.TopMenu;

            return View(lstClosingInventoryValuation);
        }

        public ActionResult InventoryInTransitReport()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForInventory, "InventoryInTransitReport", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            string yearMonth = string.Empty;
            ViewBag.ItemType = inventoryData.ReadItemType();
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

            //List<InvItemInTransit> lstInvInTransit = new List<InvItemInTransit>();
            //lstInvInTransit = reportDal.ReadInvItemInTransit(objLoginHelper.LocationCode, "1017", "01-Jan-2013", "31-Jan-2013");
            //TempData["lstInvInTransit"] = lstInvInTransit;

            return View();
        }

        [GridAction]
        public ActionResult __loadInvItemInTransit(string itemType, string location, string Item, string fromDate, string toDate)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            List<StockInTransitAtGlanceDetails> lstItemInTransit = new List<StockInTransitAtGlanceDetails>();

            String itemCode = string.Empty;

            if (!String.IsNullOrEmpty(Item))
            {
                itemCode = Item.Trim().Substring(0, 4);
            }

            if (location == null)
            {
                lstItemInTransit = null;
            }
            else
            {
                lstItemInTransit = reportDal.ReadInvItemInTransit(itemType, location.Trim(),itemCode, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate));
            }
            return View(new GridModel<StockInTransitAtGlanceDetails>
            {
                Data = lstItemInTransit
            });

        }

        public ActionResult DelevaryNoteChallanSHS()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            //if (!securityDal.IsPageAccessible(Helper.ForInventory, "InventoryInTransitReport", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            //{
            //    Session["messageInformation"] = message;
            //    return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            //}

            string yearMonth = string.Empty;
            ViewBag.ItemType = inventoryData.ReadItemType();
            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForInventory.ToString("MMMM-yy");
            //start for monthly report
            ViewBag.YearMonth = objLoginHelper.MonthOpenForInventory.ToString("yyyyMM");
            //end for monthly report
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            return View();
        }

        public ActionResult ReportViewer()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            RASolarSecurityData dal = new RASolarSecurityData();

            ViewData["reportUrl"] = "../Reports/View/local/ClosingInventory/";
            ViewBag.YearMonthFormat = helperDal.ReadYearMonthFormat();
            ViewBag.TopMenu = objLoginHelper.TopMenu;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForInventory.ToString("MMMM-yy");
            ViewBag.OpenMonth = objLoginHelper.MonthOpenForInventory.ToString("yyyyMM");
            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;

            return View();
        }
    
    }
}
