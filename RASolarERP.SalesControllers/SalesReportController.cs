using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using RASolarERP.Model;
using RASolarHelper;

using RASolarERP.Web.Models;
using RASolarERP.Web.Areas.Sales.Models;
using System.Collections;
using Telerik.Web.Mvc;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using System.Text;
using RASolarHRMS.Model;
using RASolarERP.DomainModel.HRMSModel;
using RASolarERP.DomainModel.SalesModel;

namespace RASolarERP.Web.Areas.Sales.Controllers
{
    public class SalesReportController : Controller
    {
        private SalesReportData salesReportDal = new SalesReportData();
        private LoginHelper objLoginHelper = new LoginHelper();
        private RASolarERPData erpDal = new RASolarERPData();
        RASolarSecurityData securityDal = new RASolarSecurityData();
        string message = string.Empty;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OverdueCollectionTargetVsAchievementByUnitOfficeReport()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "OverdueCollectionTargetVsAchievementByUnitOfficeReport", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForSales.ToString("MMMM-yy");

            return View(salesReportDal.ReadOverdueCollectionTargetVsAchievementByUnitOffice());
        }

        public ActionResult CollectionEfficiencySummary()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "CollectionEfficiencySummary", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            List<CollectionEfficiencyByUnitOfficeSummary> collectionSalesSummary = new List<CollectionEfficiencyByUnitOfficeSummary>();

            collectionSalesSummary = salesReportDal.ReadCollectionEfficiencyByUnitOfficeSummary(objLoginHelper.AuditorYearMonth, Helper.Location(objLoginHelper), objLoginHelper.ReportType);

            ViewBag.IsAuthenticForSave = objLoginHelper.IsAuthenticApproverForThisLocation;

            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForSales.ToString("MMMM-yy");
            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;

            decimal CM_Receivable_One_Installment_4 = Convert.ToDecimal(collectionSalesSummary.Sum(s => s.CM_Receivable__One_Installment__4));
            decimal CM_TotalCollection_8_5_6_7 = Convert.ToDecimal(collectionSalesSummary.Sum(s => s.CM_TotalCollection_8_5_6_7));

            decimal OD_SystemReturn_14 = Convert.ToDecimal(collectionSalesSummary.Sum(s => s.OD_SystemReturn_14));
            decimal OD_RecoveryFromOverdueInCurrentMonth_13 = Convert.ToDecimal(collectionSalesSummary.Sum(s => s.OD_RecoveryFromOverdueInCurrentMonth_13));
            decimal OD_OverdueBalanceAtTheEndOfLastMonth_11 = Convert.ToDecimal(collectionSalesSummary.Sum(s => s.OD_OverdueBalanceAtTheEndOfLastMonth_11));

            decimal CM_CollectionEfficiency_9_8_4 = (CM_TotalCollection_8_5_6_7 / CM_Receivable_One_Installment_4) * 100;
            decimal OverallCollectionEfficiency_16_8_13_14_4_11 = ((CM_TotalCollection_8_5_6_7 + OD_RecoveryFromOverdueInCurrentMonth_13 + OD_SystemReturn_14) / (CM_Receivable_One_Installment_4 + OD_OverdueBalanceAtTheEndOfLastMonth_11)) * 100;

            ViewBag.CMCollectionEfficiency = CM_CollectionEfficiency_9_8_4.ToString("0.00");
            ViewBag.OverallCollectionEfficiency_16_8_13_14_4_11 = OverallCollectionEfficiency_16_8_13_14_4_11.ToString("0.00");

            return View(collectionSalesSummary);
        }

        public ActionResult SalesSummaryDetailView()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "SalesSummaryDetailView", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            ViewBag.YearMonth = new YearMonthFormat().MonthFormat();
            TempData["SelectValue"] = Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForSales);

            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForSales.ToString("MMMM-yy");
            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;

            return View();
        }

        public ActionResult CustomerCollectionEfficiencyReport()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "CustomerCollectionEfficiencyReport", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForSales.ToString("MMMM-yy");

            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            List<CollectionEfficiencyByCustomer> lstCollectionEfficiencyByCustomer = new List<CollectionEfficiencyByCustomer>();
            lstCollectionEfficiencyByCustomer = salesReportDal.ReadCollectionEfficiencyByCustomer(objLoginHelper.LocationCode, objLoginHelper.AuditorYearMonth);

            decimal CM_Receivable_One_Installment_4 = 0, CM_CashCollection_5 = 0, CM_SystemReturn_6 = 0, CM_AdvanceAdjustment_7 = 0;
            decimal CM_TotalCollection_8_5_6_7 = 0, CM_CollectionEfficiency_9_8_4 = 0, OD_OverdueInCurrentMonth_10_4_8 = 0;
            decimal OD_OverdueBalanceAtTheEndOfLastMonth_11 = 0, OD_TotalOverdueUpToCurrentMonth_12_10_11 = 0, OD_RecoveryFromOverdueInCurrentMonth_13 = 0;
            decimal OD_SystemReturn_14 = 0, OD_OverdueBalanceAtTheEndOfCurrentMonth_15_12_13_14 = 0, OverallCollectionEfficiency_16_8_13_14_4_11 = 0;
            decimal AD_AdvanceBalanceAtTheEndOfLastMonth_17 = 0, AD_AdvanceReceivedInCurrentMonth_18 = 0, AD_TotalAdvance_19_17_18 = 0;
            decimal AD_AdvanceAdjustmentInCurrentMonth_20 = 0, AD_SystemReturn_21 = 0, AD_AdvanceBalanceAfterAdjustment_22_19_20_21 = 0;

            foreach (CollectionEfficiencyByCustomer cebc in lstCollectionEfficiencyByCustomer)
            {
                CM_Receivable_One_Installment_4 += cebc.CM_Receivable__One_Installment__4;
                CM_CashCollection_5 += cebc.CM_CashCollection_5;
                CM_SystemReturn_6 += cebc.CM_SystemReturn_6;
                CM_AdvanceAdjustment_7 += Convert.ToDecimal(cebc.CM_AdvanceAdjustment_7);
                CM_TotalCollection_8_5_6_7 += Convert.ToDecimal(cebc.CM_TotalCollection_8_5_6_7);

                OD_OverdueInCurrentMonth_10_4_8 += Convert.ToDecimal(cebc.OD_OverdueInCurrentMonth_10_4_8);
                OD_OverdueBalanceAtTheEndOfLastMonth_11 += Convert.ToDecimal(cebc.OD_OverdueBalanceAtTheEndOfLastMonth_11);
                OD_TotalOverdueUpToCurrentMonth_12_10_11 += Convert.ToDecimal(cebc.OD_TotalOverdueUpToCurrentMonth_12_10_11);
                OD_RecoveryFromOverdueInCurrentMonth_13 += Convert.ToDecimal(cebc.OD_RecoveryFromOverdueInCurrentMonth_13);
                OD_SystemReturn_14 += Convert.ToDecimal(cebc.OD_SystemReturn_14);
                OD_OverdueBalanceAtTheEndOfCurrentMonth_15_12_13_14 += Convert.ToDecimal(cebc.OD_OverdueBalanceAtTheEndOfCurrentMonth_15_12_13_14);

                AD_AdvanceBalanceAtTheEndOfLastMonth_17 += Convert.ToDecimal(cebc.AD_AdvanceBalanceAtTheEndOfLastMonth_17);
                AD_AdvanceReceivedInCurrentMonth_18 += Convert.ToDecimal(cebc.AD_AdvanceReceivedInCurrentMonth_18);
                AD_TotalAdvance_19_17_18 += Convert.ToDecimal(cebc.AD_TotalAdvance_19_17_18);
                AD_AdvanceAdjustmentInCurrentMonth_20 += Convert.ToDecimal(cebc.AD_AdvanceAdjustmentInCurrentMonth_20);

                AD_SystemReturn_21 += Convert.ToDecimal(cebc.AD_SystemReturn_21);
                AD_AdvanceBalanceAfterAdjustment_22_19_20_21 += Convert.ToDecimal(cebc.AD_AdvanceBalanceAfterAdjustment_22_19_20_21);
            }

            CM_CollectionEfficiency_9_8_4 = (CM_TotalCollection_8_5_6_7 / CM_Receivable_One_Installment_4) * 100;
            OverallCollectionEfficiency_16_8_13_14_4_11 = ((CM_TotalCollection_8_5_6_7 + OD_RecoveryFromOverdueInCurrentMonth_13 + OD_SystemReturn_14) / (CM_Receivable_One_Installment_4 + OD_OverdueBalanceAtTheEndOfLastMonth_11)) * 100;

            List<decimal> arr = new List<decimal>();

            arr.Add(CM_Receivable_One_Installment_4);
            arr.Add(CM_CashCollection_5);
            arr.Add(CM_SystemReturn_6);
            arr.Add(CM_AdvanceAdjustment_7);
            arr.Add(CM_TotalCollection_8_5_6_7);
            arr.Add(CM_CollectionEfficiency_9_8_4);

            arr.Add(OD_OverdueInCurrentMonth_10_4_8);
            arr.Add(OD_OverdueBalanceAtTheEndOfLastMonth_11);
            arr.Add(OD_TotalOverdueUpToCurrentMonth_12_10_11);
            arr.Add(OD_RecoveryFromOverdueInCurrentMonth_13);
            arr.Add(OD_SystemReturn_14);
            arr.Add(OD_OverdueBalanceAtTheEndOfCurrentMonth_15_12_13_14);
            arr.Add(OverallCollectionEfficiency_16_8_13_14_4_11);

            arr.Add(AD_AdvanceBalanceAtTheEndOfLastMonth_17);
            arr.Add(AD_AdvanceReceivedInCurrentMonth_18);
            arr.Add(AD_TotalAdvance_19_17_18);
            arr.Add(AD_AdvanceAdjustmentInCurrentMonth_20);
            arr.Add(AD_SystemReturn_21);
            arr.Add(AD_AdvanceBalanceAfterAdjustment_22_19_20_21);

            TempData["TotalSums"] = arr;

            return View(lstCollectionEfficiencyByCustomer);
        }

        public ActionResult SummarySheetForRegionalSalesPosting()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "SummarySheetForRegionalSalesPosting", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForSales.ToString("MMMM-yy");

            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.YearMonth = new YearMonthFormat().MonthFormat();
            ViewBag.Zone = erpDal.Zone();

            return View();
        }

        [HttpPost]
        public ActionResult SalesSummaryDetailView(FormCollection fCollection)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            //string yearMonth = Helper.ConvertDateToYearMonth(salesReportDal.LastClosedMonthForReport());

            ViewBag.YearMonth = new YearMonthFormat().MonthFormat();
            TempData["SelectValue"] = Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForSales); //fCollection["ddlYearMonth"].ToString();

            List<SalesSummaryToDetailView> lstSalesDetailView = new List<SalesSummaryToDetailView>();
            lstSalesDetailView = salesReportDal.ReadSalesSummaryToDetailView(Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForSales));//fCollection["ddlYearMonth"].ToString()

            TempData["SalesDetailView"] = lstSalesDetailView;

            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForSales.ToString("MMMM-yy");
            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;

            return View();
        }

        [GridAction]
        public ActionResult __SummarySheetForRegionalSalesPostingGridBind(string yearMonth, string regionCode)
        {
            DateTime dFrom = Helper.DateFrom(yearMonth);
            DateTime dTo = Helper.DateTo(yearMonth);

            return View(new GridModel<SummarySheetForRegionalSalesPosting> { Data = salesReportDal.ReadSummarySheetForRegionalSalesPosting(dFrom, dTo, regionCode) });
        }

        public ActionResult CollectionSheetForCustomerFPR()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "CollectionSheetForCustomerFPR", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            ViewBag.TopMenu = objLoginHelper.TopMenu;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForSales.ToString("MMMM-yy");
            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;

            List<EmployeeDetailsInfo> locationWiseEmployee = new List<EmployeeDetailsInfo>();
            locationWiseEmployee = new RASolarHRMSService().ReadLocationWiseEmployeeWithUMAcountManager(objLoginHelper.LocationCode);
            ViewBag.CustomerFPR = locationWiseEmployee;

            return View();
        }

        [GridAction]
        public ActionResult GetCollectionSheetForCustomerFPR(string customerFPR)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (string.IsNullOrEmpty(customerFPR))
            {
                customerFPR = "ALL";
            }

            List<CollectionSheetForCustomerFPR> lstCollectionSheetForCustomerFPR = new List<CollectionSheetForCustomerFPR>();
            lstCollectionSheetForCustomerFPR = salesReportDal.ReadCollectionSheetForCustomerFPR(customerFPR, objLoginHelper.LocationCode);

            return View(new GridModel<CollectionSheetForCustomerFPR>
            {
                Data = lstCollectionSheetForCustomerFPR
            });


        }

        public ActionResult ExportToPdfCollectionSheet(int page, string groupby, string orderBy, string filter, string customerFPR)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (customerFPR == "~")
            {
                customerFPR = "ALL";
            }

            List<CollectionSheetForCustomerFPR> lstCollectionSheetForCustomerFPR = new List<CollectionSheetForCustomerFPR>();
            lstCollectionSheetForCustomerFPR = salesReportDal.ReadCollectionSheetForCustomerFPR(customerFPR, objLoginHelper.LocationCode);

            var customerFPRName = string.Empty;

            if (customerFPR != "ALL")
            {
                customerFPRName = (from lstCollectionSheet in lstCollectionSheetForCustomerFPR select lstCollectionSheet.EmployeeName).FirstOrDefault();
            }

            Document pdfDocumnet = new Document(PageSize.A4, 35, 20, 15, 10);
            MemoryStream memStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(pdfDocumnet, memStream);

            StringBuilder headerStringCompanyTitle = new StringBuilder();
            StringBuilder headerStringReportTitle = new StringBuilder();
            headerStringCompanyTitle.AppendFormat(Helper.RSFCompanyName);
            headerStringReportTitle.AppendFormat("Customer FPR Name:{0}                 Month:          Unit: {1}", customerFPRName, objLoginHelper.LogInForUnitName);

            Chunk chunkCompanyTitle = new Chunk(headerStringCompanyTitle.ToString(), FontFactory.GetFont(FontFactory.COURIER, 9, Font.BOLD, new Color(0, 0, 0)));
            Chunk chunkReportTitle = new Chunk(headerStringReportTitle.ToString(), FontFactory.GetFont(FontFactory.COURIER, 8, Font.NORMAL, new Color(0, 0, 0)));

            HeaderFooter header = new HeaderFooter(new Phrase(chunkCompanyTitle), false);
            header.After = new Phrase(chunkReportTitle);
            header.Alignment = Element.ALIGN_LEFT;
            header.BorderWidthTop = 0;
            header.BorderColorBottom = new Color(166, 165, 165);

            HeaderFooter footer = new HeaderFooter(new Phrase("-"), new Phrase("-"));
            footer.Alignment = Element.ALIGN_CENTER;
            footer.Border = 0;
            footer.BorderColor = new Color(230, 230, 230);
            footer.BorderWidthTop = .25F;

            pdfDocumnet.Header = header;
            pdfDocumnet.Footer = footer;

            pdfDocumnet.Open();

            int numOfColumns = 13;
            float[] widths = new float[] { 5f, 10f, 20f, 16f, 5f, 5f, 5f, 6f, 6f, 6f, 7f, 5f, 5f };

            PdfPTable pdfTable = new PdfPTable(numOfColumns);
            pdfTable.SetWidths(widths);
            pdfTable.WidthPercentage = 100;
            pdfTable.DefaultCell.Padding = 3;
            pdfTable.DefaultCell.BorderWidth = 0.5F;
            pdfTable.DefaultCell.BorderColor = new Color(221, 218, 218);
            pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfTable.DefaultCell.BackgroundColor = new Color(235, 235, 235);

            Font fontForTableHeader = new Font(Font.COURIER, 8, Font.BOLD, new Color(0, 0, 0));
            pdfTable.AddCell(new Phrase("No", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Cust ID", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Cust Name", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Village", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Cust GD", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Col Day", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Col Day", fontForTableHeader));
            pdfTable.AddCell(new Phrase("CM Rec", fontForTableHeader));
            pdfTable.AddCell(new Phrase("OD Rec", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Total Rec", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Col up to date", fontForTableHeader));
            pdfTable.AddCell(new Phrase(" ", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Remarks", fontForTableHeader));

            pdfTable.HeaderRows = 1;
            pdfTable.DefaultCell.BorderWidth = 0.5F;
            pdfTable.DefaultCell.BorderColor = new Color(195, 195, 195);
            pdfTable.DefaultCell.BackgroundColor = new Color(255, 255, 255);

            Font fontForContent = new Font(Font.COURIER, 7, Font.NORMAL, new Color(0, 0, 0));

            foreach (CollectionSheetForCustomerFPR cr in lstCollectionSheetForCustomerFPR)
            {
                pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.AddCell(new Phrase(((decimal)cr.SerialNo).ToString("#,##0"), fontForContent));
                pdfTable.AddCell(new Phrase(cr.CustomerCode, fontForContent));
                pdfTable.AddCell(new Phrase(cr.CustomerName, fontForContent));
                pdfTable.AddCell(new Phrase(cr.Village, fontForContent));
                pdfTable.AddCell(new Phrase(cr.ODCustomerGrade, fontForContent));
                pdfTable.AddCell(new Phrase(((byte)cr.ScheduledCollectionDay).ToString(), fontForContent));
                pdfTable.AddCell(new Phrase(string.Empty, fontForContent));

                pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                pdfTable.AddCell(new Phrase(((decimal)cr.CurrentReceivable).ToString("#,##0"), fontForContent));
                pdfTable.AddCell(new Phrase(((decimal)cr.OverdueReceivable).ToString("#,##0"), fontForContent));
                pdfTable.AddCell(new Phrase(((decimal)cr.TotalReceivable).ToString("#,##0"), fontForContent));
                pdfTable.AddCell(new Phrase(string.Empty, fontForContent));
                pdfTable.AddCell(new Phrase(string.Empty, fontForContent));
                pdfTable.AddCell(new Phrase(string.Empty, fontForContent));
            }

            pdfDocumnet.Add(pdfTable);
            pdfDocumnet.Close();

            return File(memStream.ToArray(), "application/pdf", "RSF-CollectionSheetForCustomerFPR.pdf");
        }

        public ActionResult ExportToPdfCollectionSheet1(int page, string groupby, string orderBy, string filter, string customerFPR)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (customerFPR == "~")
            {
                customerFPR = "ALL";
            }

            List<CollectionSheetForCustomerFPR> lstCollectionSheetForCustomerFPR = new List<CollectionSheetForCustomerFPR>();
            lstCollectionSheetForCustomerFPR = salesReportDal.ReadCollectionSheetForCustomerFPR(customerFPR, objLoginHelper.LocationCode);

            var customerFPRName = string.Empty;

            if (customerFPR != "ALL")
            {
                customerFPRName = (from lstCollectionSheet in lstCollectionSheetForCustomerFPR select lstCollectionSheet.EmployeeName).FirstOrDefault();
            }

            List<CollectionSheetForCustomerFPR> lstCustomerFpr = new List<CollectionSheetForCustomerFPR>();
            lstCustomerFpr = lstCollectionSheetForCustomerFPR.GroupBy(g => g.EmployeeID).Select(s => s.First()).ToList();

            List<CollectionSheetForCustomerFPR> lstCollectionFPR = new List<CollectionSheetForCustomerFPR>();

            Document pdfDocumnet = new Document(PageSize.A4, 35, 20, 15, 10);
            MemoryStream memStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(pdfDocumnet, memStream);

            //-----------          

            StringBuilder headerStringCompanyTitle = new StringBuilder();
            StringBuilder headerStringReportTitle = new StringBuilder();
            headerStringCompanyTitle.AppendFormat(Helper.RSFCompanyName);
            headerStringReportTitle.AppendFormat("Customer FPR Name:{0}                 Month:          Unit: {1}", lstCollectionSheetForCustomerFPR[0].EmployeeName, objLoginHelper.LogInForUnitName);

            Chunk chunkCompanyTitle = new Chunk(headerStringCompanyTitle.ToString(), FontFactory.GetFont(FontFactory.COURIER, 9, Font.BOLD, new Color(0, 0, 0)));
            Chunk chunkReportTitle = new Chunk(headerStringReportTitle.ToString(), FontFactory.GetFont(FontFactory.COURIER, 8, Font.NORMAL, new Color(0, 0, 0)));

            HeaderFooter header = new HeaderFooter(new Phrase(chunkCompanyTitle), false);
            header.After = new Phrase(chunkReportTitle);
            header.Alignment = Element.ALIGN_LEFT;
            header.BorderWidthTop = 0;
            header.BorderColorBottom = new Color(166, 165, 165);

            HeaderFooter footer = new HeaderFooter(new Phrase("-"), new Phrase("-"));
            footer.Alignment = Element.ALIGN_CENTER;
            footer.Border = 0;
            footer.BorderColor = new Color(230, 230, 230);
            footer.BorderWidthTop = .25F;

            pdfDocumnet.Header = header;
            pdfDocumnet.Footer = footer;

            //--------

            pdfDocumnet.Open();

            foreach (CollectionSheetForCustomerFPR lst in lstCustomerFpr)
            {
                int numOfColumns = 13;
                float[] widths = new float[] { 5f, 10f, 20f, 16f, 3f, 4f, 6f, 6f, 6f, 6f, 10f, 5f, 5f };

                PdfPTable pdfTable = new PdfPTable(numOfColumns);
                pdfTable.SetWidths(widths);
                pdfTable.WidthPercentage = 100;
                pdfTable.DefaultCell.Padding = 3;
                pdfTable.DefaultCell.BorderWidth = 0.5F;
                pdfTable.DefaultCell.BorderColor = new Color(221, 218, 218);
                pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.DefaultCell.BackgroundColor = new Color(235, 235, 235);

                Font fontForTableHeader = new Font(Font.COURIER, 8, Font.BOLD, new Color(0, 0, 0));
                pdfTable.AddCell(new Phrase("No", fontForTableHeader));
                pdfTable.AddCell(new Phrase("Cust ID", fontForTableHeader));
                pdfTable.AddCell(new Phrase("Cust Name", fontForTableHeader));
                pdfTable.AddCell(new Phrase("Village", fontForTableHeader));
                pdfTable.AddCell(new Phrase("Cust GD", fontForTableHeader));
                pdfTable.AddCell(new Phrase("Col Day", fontForTableHeader));
               // pdfTable.AddCell(new Phrase("Col Day", fontForTableHeader));
                pdfTable.AddCell(new Phrase("CM Rec", fontForTableHeader));
                pdfTable.AddCell(new Phrase("OD Rec", fontForTableHeader));
                pdfTable.AddCell(new Phrase("Total Rec", fontForTableHeader));
                //pdfTable.AddCell(new Phrase("Col up to date", fontForTableHeader));
                //
                pdfTable.AddCell(new Phrase("OutStanding Balance", fontForTableHeader));
                pdfTable.AddCell(new Phrase("Last Payment Date", fontForTableHeader));
                pdfTable.AddCell(new Phrase("Col Up to Date", fontForTableHeader));
                //
                //pdfTable.AddCell(new Phrase(" ", fontForTableHeader));
                pdfTable.AddCell(new Phrase("Remarks", fontForTableHeader));
                
                pdfTable.HeaderRows = 1;
                pdfTable.DefaultCell.BorderWidth = 0.5F;
                pdfTable.DefaultCell.BorderColor = new Color(195, 195, 195);
                pdfTable.DefaultCell.BackgroundColor = new Color(255, 255, 255);

                Font fontForContent = new Font(Font.COURIER, 7, Font.NORMAL, new Color(0, 0, 0));

                lstCollectionFPR = (from ss in lstCollectionSheetForCustomerFPR
                                    where ss.EmployeeID == lst.EmployeeID
                                    select ss).ToList();

                foreach (CollectionSheetForCustomerFPR cr in lstCollectionFPR)
                {
                    pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfTable.AddCell(new Phrase(((decimal)cr.SerialNo).ToString("#,##0"), fontForContent));
                    pdfTable.AddCell(new Phrase(cr.CustomerCode, fontForContent));
                    pdfTable.AddCell(new Phrase(cr.CustomerName, fontForContent));
                    pdfTable.AddCell(new Phrase(cr.Village, fontForContent));
                    pdfTable.AddCell(new Phrase(cr.ODCustomerGrade, fontForContent));
                    pdfTable.AddCell(new Phrase(((byte)cr.ScheduledCollectionDay).ToString(), fontForContent));
                    //pdfTable.AddCell(new Phrase(string.Empty, fontForContent));

                    pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdfTable.AddCell(new Phrase(((decimal)cr.CurrentReceivable).ToString("#,##0"), fontForContent));
                    pdfTable.AddCell(new Phrase(((decimal)cr.OverdueReceivable).ToString("#,##0"), fontForContent));
                    pdfTable.AddCell(new Phrase(((decimal)cr.TotalReceivable).ToString("#,##0"), fontForContent));
                    //
                    //pdfTable.AddCell(new Phrase(((decimal)cr.).ToString("#,##0"), fontForContent)); 
                    //
                    pdfTable.AddCell(new Phrase(((decimal)cr.OutstandingBalance).ToString("#,##0"), fontForContent));
                    pdfTable.AddCell(new Phrase(((DateTime)cr.LastPaidOn).ToString("dd/MM/yyyy"), fontForContent));
                    pdfTable.AddCell(new Phrase(string.Empty, fontForContent));
                    //pdfTable.AddCell(new Phrase(((Int32)cr.NoOfMonthPassed).ToString("#,##0"), fontForContent));
                    pdfTable.AddCell(new Phrase(string.Empty, fontForContent));
                }

                pdfDocumnet.ResetHeader();

                headerStringCompanyTitle = new StringBuilder();
                headerStringReportTitle = new StringBuilder();
                headerStringCompanyTitle.AppendFormat(Helper.RSFCompanyName);
                headerStringReportTitle.AppendFormat("Customer FPR Name:{0}                 Month:          Unit: {1}", lst.EmployeeName, objLoginHelper.LogInForUnitName);

                chunkCompanyTitle = new Chunk(headerStringCompanyTitle.ToString(), FontFactory.GetFont(FontFactory.COURIER, 9, Font.BOLD, new Color(0, 0, 0)));
                chunkReportTitle = new Chunk(headerStringReportTitle.ToString(), FontFactory.GetFont(FontFactory.COURIER, 8, Font.NORMAL, new Color(0, 0, 0)));

                header = new HeaderFooter(new Phrase(chunkCompanyTitle), false);
                header.After = new Phrase(chunkReportTitle);
                header.Alignment = Element.ALIGN_LEFT;
                header.BorderWidthTop = 0;
                header.BorderColorBottom = new Color(166, 165, 165);

                footer = new HeaderFooter(new Phrase("-"), new Phrase("-"));
                footer.Alignment = Element.ALIGN_CENTER;
                footer.Border = 0;
                footer.BorderColor = new Color(230, 230, 230);
                footer.BorderWidthTop = .25F;

                pdfDocumnet.Header = header;
                pdfDocumnet.Footer = footer;

                pdfDocumnet.NewPage();
                pdfDocumnet.Add(pdfTable);

                pdfDocumnet.ResetPageCount();

            }

            pdfDocumnet.Close();

            return File(memStream.ToArray(), "application/pdf", "RSF-CollectionSheetForCustomerFPR.pdf");
        }


    }
}
