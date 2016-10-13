using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using RASolarHelper;
using RASolarERP.Model;
using RASolarSecurity.Model;

using RASolarERP.Web.Areas.Sales.Models;
using Telerik.Web.Mvc;
using System.Collections;
using System.Text.RegularExpressions;
using System.Text;
using RASolarERP.DomainModel.SalesModel;
using RASolarERP.Web.Models;
using RASolarERP.Web.Areas.Inventory.Models;
using System.Data.SqlClient;
using RASolarERP.Web.Areas.HRMS.Models;
using RASolarERP.Web.Areas.Sales.Controllers;

namespace RASolarERP.Web.Areas.Sales.Controllers
{
    public class SalesDeptController : Controller
    {
        LoginHelper objLoginHelper = new LoginHelper();
        private SalesData salesDal = new SalesData();
        private SalesReportData salesReportDal = new SalesReportData();
        private RASolarERPData erpDal = new RASolarERPData();
        InventoryData inventoryDal = new InventoryData();
        RASolarSecurityData securityDal = new RASolarSecurityData();
        private HRMSData hrmsDal = new HRMSData();
        string message = string.Empty;

        List<Sal_SalesItems> lstSalesItem = new List<Sal_SalesItems>();
        List<Sal_SalesItemsWithSerialNo> lstItemWithSerials = new List<Sal_SalesItemsWithSerialNo>();

        string serialTempTableRows = string.Empty;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SalesSummary()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "SalesSummary", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            List<SalesDataEntryStatus> salesDataEntrySummary = new List<SalesDataEntryStatus>();

            salesDataEntrySummary = salesDal.SalesEntryStatus(objLoginHelper.ReportType, objLoginHelper.LocationCode, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForSales));

            ViewBag.IsAuthenticForSave = objLoginHelper.IsAuthenticApproverForThisLocation;

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForSales.ToString("MMMM-yy");
            ViewBag.TotalNumberofUnits = salesDataEntrySummary.Count();
            ViewBag.EntryfinalizedbyRM = salesDataEntrySummary.Count(s => s.FinalizedByRM != null);
            ViewBag.EntryFinalizedbyZM = salesDataEntrySummary.Count(s => s.FinalizedByZM != null);
            ViewBag.EntryfinalizedbyHO = salesDataEntrySummary.Count(s => s.FinalizedByHO != null);

            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            TempData["SalesOpenMonth"] = Helper.DateTo(Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForSales)).ToString("MMM-yyy");

            return View(salesDataEntrySummary);
        }

        public ActionResult SalesReSalesAgreement()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "SalesReSalesAgreement", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            if (objLoginHelper.TransactionOpenDate == Helper.InvalidDate())
            {
                Session["messageInformation"] = "Day Is Not Open For Sales";
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            //start for popup

            
            List<Sal_SalesAgreementPrePost_DataFromExternalSources> ReadSalesResalesAgrExternalSourceByLCode = salesDal.ReadSalesResalesAgrExternalSourceByLCode(objLoginHelper.LocationCode);
            if (ReadSalesResalesAgrExternalSourceByLCode.Count > 0)
            {
                ViewBag.ShowDialogue = 1;
            }
            else
                ViewBag.ShowDialogue = 0;

            //end for popup


            //
            ViewBag.LocationCode = objLoginHelper.LocationCode;
            //
            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenDayForTransaction = "Day Open: " + objLoginHelper.TransactionOpenDate.ToString("MMMM dd, yyyy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;
            
            //for warrenty check  item category
            List<ItemCatNDescription> lstItemCateNDescription = new List<ItemCatNDescription>();

            lstItemCateNDescription = salesDal.ReadItemCatNDescription();
            TempData["lstItemCateNDescription"] = lstItemCateNDescription;

            List<PackageDetails> lstPackageDetails = new List<PackageDetails>();
           // lstPackageDetails = salesDal.ReadPackageDetailsExtra(); 
            TempData["lstPackageDetails"] = lstPackageDetails;

            //

            ViewBag.Program = salesDal.ReadProgram().Where(p => p.Prog_Code == "SHS001");
            ViewBag.Project = salesDal.ReadProjectByProgramCode("SHS001").Where(p => p.ProjectCode == "100200");
            ViewBag.PackageCapacity = salesDal.ReadCapacityByProjectCode("100200", Helper.IsCapacityOnlyForPackagesAndItems);
            ViewBag.PackageLight = salesDal.ReadLightByCapacityId("0020WP");
            ViewBag.Package = salesDal.ReadPackages("0020WP", "02LIGHT", Helper.NewSalesAgreement);

            //ViewBag.ModeOfPayment = salesDal.ReadModeOfPayment("SHS001", "SHS010", "HOUHLD"); real before 101614 modified for saleresalesagreement page

            ViewBag.ModeOfPayment = salesDal.ReadModeOfPayment("","SHS001", "SHS010", "HOUHLD","");

            ViewBag.CustomerType = salesDal.ReadCustomerTypes();
            //start change for ddl
            ViewBag.Employee = hrmsDal.ReadPersonLocationWiseEmployee(objLoginHelper.LogInForUnitCode);
            //end  change for ddl
             //ViewBag.Employee = hrmsDal.ReadLocationWiseEmployee(objLoginHelper.LogInForUnitCode);

            ViewBag.Upazilla = salesDal.ReadUpazillaByUnitCode(objLoginHelper.LogInForUnitCode);
            ViewBag.PostOffice = salesDal.ReadPostOfficeInfo(objLoginHelper.LogInForUnitCode);
            ViewBag.CollectionDay = new SalesDay().SalesCollectionDay();

            ViewBag.GurdianRelation = salesDal.ReadCustomerRelations();
            ViewBag.CustomerOccupation = salesDal.ReadCustomerOccupations();
            ViewBag.CustomerIncomeRange = salesDal.ReadCustomerIncomeRange();
            ViewBag.FuelUsedBeforeSHS = salesDal.ReadCustomerFuelUsed();

            ViewBag.CustomerId = objLoginHelper.CustomerPrefix;

            ViewBag.OpenDay = objLoginHelper.TransactionOpenDate.Date.ToString("dd-MMM-yyyy");
            ViewBag.OpenBackDay = objLoginHelper.TransactionBackDate.Date.ToString("dd-MMM-yyyy");

            ViewBag.DayOpen = objLoginHelper.MonthOpenForSales.Date.ToString("dd-MMM-yyyy");
            ViewBag.CurrentCustomerSerial = (salesDal.LastUsedCustomerSerial(objLoginHelper.LogInForUnitCode, "SHS001") + 1).ToString();
            ViewBag.IsInventoryImplemented = objLoginHelper.IsInventoryImplemented;

            return View();
        }

        //[HttpGet]
        //public ActionResult SpareSales()
        //{
        //    objLoginHelper = (LoginHelper)Session["LogInInformation"];

        //    if (!securityDal.IsPageAccessible(Helper.ForSales, "SpareSales", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
        //    {
        //        Session["messageInformation"] = message;
        //        return RedirectToAction("ErrorMessage", "../ErrorHnadle");
        //    }

        //    List<Sal_SparePartsSales_DataFromExternalSources> ReadSalesExternalSourceByLCode = salesDal.ReadSalesExternalSourceByLCode(objLoginHelper.LocationCode);

        //    return View();
        //}

        public ActionResult GetSpareSalesItems(string itemCode)
        {
          
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<Sal_SparePartsSales_DataFromExternalSources> ReadSalesExternalSourceByLCode = salesDal.ReadSalesExternalSourceByLCode(objLoginHelper.LocationCode);

            List<Sal_SparePartsSales_DataFromExternalSources> lstPopUP = new List<Sal_SparePartsSales_DataFromExternalSources>();
            lstPopUP = (from t in ReadSalesExternalSourceByLCode select new Sal_SparePartsSales_DataFromExternalSources { ItemDescription = t.ItemDescription, ItemQuantity = t.ItemQuantity, CashMemoNo = t.CashMemoNo, SalesAmountAfterDiscount = t.SalesAmountAfterDiscount, SalespersonCode = t.SalespersonCode }).ToList();
           // ViewBag.ItemCode = itemCode;

            return PartialView("ItemDetailSearch", lstPopUP);
       }

        public ActionResult GetSalseReSalesAgreItems(string itemCode) 
        {

            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<Sal_SalesAgreementPrePost_DataFromExternalSources> ReadSalesResalesAgrExternalSourceByLCode = salesDal.ReadSalesResalesAgrExternalSourceByLCode(objLoginHelper.LocationCode);

            List<Sal_SalesAgreementPrePost_DataFromExternalSources> lstPopUP = new List<Sal_SalesAgreementPrePost_DataFromExternalSources>();
            lstPopUP = (from t in ReadSalesResalesAgrExternalSourceByLCode select new Sal_SalesAgreementPrePost_DataFromExternalSources 
                          { CustomerName=t.CustomerName,PhoneNo=t.PhoneNo,AgreementType=t.AgreementType,CustomerType=t.CustomerType,PackageCode=t.PackageCode
                           ,ModeOfPaymentID=t.ModeOfPaymentID,YearMonth=t.YearMonth,AgreementDate=t.AgreementDate,NumberOfInstallment=t.NumberOfInstallment,PackagePrice=t.PackagePrice
                           ,DiscountID=t.DiscountID,DiscountAmount=t.DiscountAmount,AmountAfterDiscount=t.AmountAfterDiscount,DownPaymentID=t.DownPaymentID,STDDownPaymentPercentage=t.STDDownPaymentPercentage
                           ,DownPaymentAmount=t.DownPaymentAmount,TotalPrincipalReceivable=t.TotalPrincipalReceivable,ServiceChargeID=t.ServiceChargeID,STDServiceChargePercentage=t.STDServiceChargePercentage
                           ,TotalServiceChargeReceivable=t.TotalServiceChargeReceivable,TotalPrincipalPlusServiceChargeReceivable=t.TotalPrincipalPlusServiceChargeReceivable,InstallmentSize=t.InstallmentSize
                           ,InstallmentSizePrincipal=t.InstallmentSizePrincipal,InstallmentSizeServiceCharge=t.InstallmentSizeServiceCharge,ScheduledCollectionDay=t.ScheduledCollectionDay,UnitCode=t.UnitCode
                           ,ProjectCode=t.ProjectCode,ProgramCode=t.ProgramCode,CashMemoNo=t.CashMemoNo,CashMemoUsesID=t.CashMemoUsesID,RefMemoNo=t.RefMemoNo,SourceOrUserName=t.SourceOrUserName,RefDeviceUserEmployeeID=t.RefDeviceUserEmployeeID
                           ,RefExternalDeviceID=t.RefExternalDeviceID,RefExternalTransactionNo=t.RefExternalTransactionNo,TechnicalFees=t.TechnicalFees,Subsidies=t.Subsidies,DisbursementNo=t.DisbursementNo,SalespersonCode=t.SalespersonCode,IsReSales=t.IsReSales
                           ,PanelItemCode=t.PanelItemCode,PanelSerialNo=t.PanelSerialNo,BatteryItemCode=t.BatteryItemCode,BatterySerialNo=t.BatterySerialNo,CreatedBy=t.CreatedBy,CreatedOn=t.CreatedOn,IsTransferredToFinalTable=t.IsTransferredToFinalTable}).ToList();
            // ViewBag.ItemCode = itemCode;

            return PartialView("SalesReSalesAgreementForCashMemoPopUP", lstPopUP);
        }


        public ActionResult SpareSales()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "SpareSales", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            List<Sal_SparePartsSales_DataFromExternalSources> ReadSalesExternalSourceByLCode = salesDal.ReadSalesExternalSourceByLCode(objLoginHelper.LocationCode);

            if (ReadSalesExternalSourceByLCode.Count > 0)
            {
                ViewBag.ShowDialogue = 1;
            }
            else
                ViewBag.ShowDialogue = 0;

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenDayForTransaction = "Day Open: " + objLoginHelper.TransactionOpenDate.ToString("MMMM dd, yyyy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForSales.ToString("MMMM-yy");

            ViewBag.OpenDay = objLoginHelper.TransactionOpenDate.Date.ToString("dd-MMM-yyyy");
            ViewBag.OpenBackDay = objLoginHelper.TransactionBackDate.Date.ToString("dd-MMM-yyyy");

            ViewBag.ItemType = inventoryDal.ReadItemType().Where(t => t.ItemTypeID == "INVTORYITM").ToList();
            ViewBag.StoreLocation = inventoryDal.ReadStoreLocation();

            string sparseChallanNumberMax = salesDal.SparseChallanSequenceNumberMax(objLoginHelper.LocationCode, objLoginHelper.TransactionOpenDate.ToString("yyMMdd"));
            string spareSalesSequenceNumberNew = Helper.ChallanCequenceNumberGeneration(sparseChallanNumberMax, objLoginHelper);
            ViewBag.spareSalesSequenceNumber = spareSalesSequenceNumberNew;

            ViewBag.Zone = erpDal.Zone();
            ViewBag.LocationType = new UserLocationType().UserLocationTypeFormat();

            if (objLoginHelper.LocationCode == Helper.HeadOfficeLocationCode)
            {
                ViewBag.IsInventoryImplemented = 1;
                ViewBag.Employee = hrmsDal.ReadLocationWiseEmployeeForHeadOffice(objLoginHelper.LocationCode);
            }
            else
            {
                ViewBag.IsInventoryImplemented = objLoginHelper.IsInventoryImplemented;
                ViewBag.Employee = hrmsDal.ReadLocationWiseEmployee(objLoginHelper.LogInForUnitCode);
            }

            ViewBag.InventoryStockUpdateFinishClosed = true; //inventoryDal.IsInventoryStockUpdateFinish(objLoginHelper.LocationCode);

            return View();
        }

        public ActionResult SpareSalesByItemSet()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "SpareSalesByItemSet", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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
            ViewBag.OpenDayForTransaction = "Day Open: " + objLoginHelper.TransactionOpenDate.ToString("MMMM dd, yyyy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForSales.ToString("MMMM-yy");

            ViewBag.OpenDay = objLoginHelper.TransactionOpenDate.Date.ToString("dd-MMM-yyyy");
            ViewBag.OpenBackDay = objLoginHelper.TransactionBackDate.Date.ToString("dd-MMM-yyyy");

            ViewBag.ItemType = inventoryDal.ReadItemType().Where(t => t.ItemTypeID == "INVTORYITM").ToList();
            ViewBag.StoreLocation = inventoryDal.ReadStoreLocation();

            string sparseChallanNumberMax = salesDal.SparseChallanSequenceNumberMax(objLoginHelper.LocationCode, objLoginHelper.TransactionOpenDate.ToString("yyMMdd"));
            string spareSalesSequenceNumberNew = Helper.ChallanCequenceNumberGeneration(sparseChallanNumberMax, objLoginHelper);
            ViewBag.spareSalesSequenceNumber = spareSalesSequenceNumberNew;

            ViewBag.Zone = erpDal.Zone();
            ViewBag.LocationType = new UserLocationType().UserLocationTypeFormat();
            ViewBag.Employee = hrmsDal.ReadLocationWiseEmployee(objLoginHelper.LogInForUnitCode);

            if (objLoginHelper.LocationCode == "9000")
            { ViewBag.IsInventoryImplemented = 1; }
            else
            { ViewBag.IsInventoryImplemented = objLoginHelper.IsInventoryImplemented; }

            ViewBag.ItemSets = salesDal.ReadItemSetMaster();

            ViewBag.InventoryStockUpdateFinishClosed = true; //inventoryDal.IsInventoryStockUpdateFinish(objLoginHelper.LocationCode);

            return View();
        }

        [HttpPost]
        public ActionResult SalesReSalesAgreement(FormCollection fCollection)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            SalesDataProcess salesdataProcess = new SalesDataProcess();
            Sal_SalesAgreementPrePost objAgreement = new Sal_SalesAgreementPrePost();
            List<PackageDetails> lstPackageDetails = new List<PackageDetails>();

            try
            {
                lstPackageDetails = (List<PackageDetails>)Session["PackageDetails"];

                if (fCollection["ddlProgram"].ToString() == "SHS001")
                {
                    salesdataProcess.AssignSalesItemAndSave(fCollection, lstPackageDetails, objLoginHelper);

                    if (objAgreement != null)
                    {
                        erpDal.CreateUserLog(Helper.ClientIPAddress(), string.Empty, objLoginHelper.LocationCode, objLoginHelper.LogInID, "Sales Agreement, Customer Code: " + fCollection["hfCustomerCode"].Trim());
                        return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Agreement" + Helper.SuccessMessage) };
                    }
                }
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }

            return new JsonResult { Data = "" };
        }

        //[HttpPost]
        //public ActionResult SalesReSalesAgreement(FormCollection fCollection)
        //{
        //    objLoginHelper = (LoginHelper)Session["LogInInformation"];

        //    Sal_Customer objCustomer = new Sal_Customer();
        //    Sal_SalesAgreement objAgreement = new Sal_SalesAgreement();
        //    Sal_CustomerStatus objCustomerStatus = new Sal_CustomerStatus();
        //    List<Inv_ItemMaster> lstItemMaster = new List<Inv_ItemMaster>();

        //    try
        //    {
        //        lstItemMaster = salesDal.ReadItemMaster();

        //        if (fCollection["ddlProgram"].ToString() == "SHS001")
        //        {
        //            objCustomer = AssignCustomerInfo(fCollection);
        //            objAgreement = AssignSalesAgreement(fCollection);
        //            AssignSalesItem(fCollection, lstItemMaster);

        //            objCustomerStatus.CustomerCode = objCustomer.CustomerCode;

        //            objAgreement = salesDal.SaveSalesAgreement(objCustomer, objAgreement, objCustomerStatus, lstSalesItem, lstItemWithSerials, Convert.ToInt16(fCollection["txtCustomerSerial"]), serialTempTableRows);

        //            if (objAgreement != null)
        //            {
        //                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Agreement" + Helper.SuccessMessage) };
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
        //    }

        //    return new JsonResult { Data = "" };
        //}

        public ActionResult UOCollectionVsHOPhysicalCashMovementReport()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "UOCollectionVsHOPhysicalCashMovementReport", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            List<UnitCollectionVsHeadOfficePhysicalCashMovement> lstUnitCollectionVSHeadHfficeHardcopy = new List<Model.UnitCollectionVsHeadOfficePhysicalCashMovement>();
            lstUnitCollectionVSHeadHfficeHardcopy = salesReportDal.ReadUnitCollectionVsHeadOfficePhysicalCashMovement(objLoginHelper.ReportType, objLoginHelper.LocationCode, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForDailyProgressReview));

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

            return View(lstUnitCollectionVSHeadHfficeHardcopy);
        }

        [GridAction]
        public ActionResult _PackageDetailsLoad(string packageCode, string modeOfPaymentID, string customerType)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            //if (customerType != null)
            //{
            //    if (customerType == "RELINS")
            //    {
            //        customerType = "INSTIT";
            //    }
            //    else
            //    {
            //        customerType = "PERSNL";
            //    }
            //}

            List<PackageDetails> lstPackageDetails = new List<PackageDetails>();
            lstPackageDetails = salesDal.ReadPackageDetails(packageCode, modeOfPaymentID, customerType);
            TempData["lstPackageDetails"] = lstPackageDetails;

            Session["PackageDetails"] = lstPackageDetails;

            Session["PackageDetailsOne"] = lstPackageDetails;

            return View(new GridModel<PackageDetails>
            {
                Data = lstPackageDetails.Where(p => p.IsShowInSalesAgreementPage == true)
            });
        }

        public JsonResult SaveSalesStatus(string unitCode)
        {
            string saveSuccessMessage = string.Empty;
            //objLoginHelper = (LoginHelper)Session["LogInInformation"];

            //tbl_UnitWiseEntryStatus objUnitWiseEntryStatus = new tbl_UnitWiseEntryStatus();

            //objUnitWiseEntryStatus = salesDal.UnitWiseEntryStatus(unitCode, Helper.YearMonthPrevious(1));

            //if (objLoginHelper.Location == Helper.Zone)
            //{
            //    objUnitWiseEntryStatus.SalFinalizedByZM_UserName = objLoginHelper.LogInID;
            //    objUnitWiseEntryStatus.SalCheckedNFinalizedByZM = DateTime.Now;
            //}
            //else if (objLoginHelper.Location == Helper.Region)
            //{
            //    objUnitWiseEntryStatus.EntryStatusForSales = Helper.Completed;
            //    objUnitWiseEntryStatus.ClosingDateForSales = DateTime.Now;

            //    objUnitWiseEntryStatus.SalFinalizedByRM_UserName = objLoginHelper.LogInID;
            //    objUnitWiseEntryStatus.SalCheckedNFinalizedByRM = DateTime.Now;
            //}
            //else if (objLoginHelper.Location == Helper.HeadOffice)
            //{
            //    objUnitWiseEntryStatus.SalFinalizedByHO_UserName = objLoginHelper.LogInID;
            //    objUnitWiseEntryStatus.SalCheckedNFinalizedByHO = DateTime.Now;
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

        public JsonResult ProjectList(string programCode)
        {
            List<Common_ProjectInfo> lstProject = new List<Common_ProjectInfo>();
            lstProject = salesDal.ReadProjectByProgramCode(programCode);

            ArrayList arl = new ArrayList();

            foreach (Common_ProjectInfo prj in lstProject)
            {
                arl.Add(new { Value = prj.ProjectCode, Display = prj.ProjectName });
            }

            return new JsonResult { Data = arl };
        }

        public JsonResult ProjectListWithoutSHS(string programCode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<Common_ProjectInfo> lstProject = new List<Common_ProjectInfo>();
            lstProject = salesDal.ReadProjectByProgramCode(programCode).Where(p => p.ProjectCode != "100200").ToList();

            int lastUsedCustomerSerial = (salesDal.LastUsedCustomerSerial(objLoginHelper.LogInForUnitCode, programCode) + 1);

            ArrayList arl = new ArrayList();

            foreach (Common_ProjectInfo prj in lstProject)
            {
                arl.Add(new { Value = prj.ProjectCode, Display = prj.ProjectName });
            }
            arl.Add(new { Value = lastUsedCustomerSerial, Display = lastUsedCustomerSerial });

            return new JsonResult { Data = arl };
        }

        public JsonResult PackgeList(string capacityId, string lightId, string salesType)
        {
            List<Sal_PackageMaster> lstPackage = new List<Sal_PackageMaster>();
            lstPackage = salesDal.ReadPackages(capacityId, lightId, Convert.ToByte(salesType));

            ArrayList arl = new ArrayList();

            foreach (Sal_PackageMaster pkg in lstPackage)
            {
                arl.Add(new { Value = pkg.PackageCode.ToString(), Display = pkg.Description });
            }

            return new JsonResult { Data = arl };
        }


        public JsonResult PopUpPackageList(string packageCode) 
        {
            List<Sal_PackageMaster> lstPackage = new List<Sal_PackageMaster>();
            lstPackage = salesDal.ReadPopUpPackageList(packageCode);

            ArrayList arl = new ArrayList();

            foreach (Sal_PackageMaster pkg in lstPackage)
            {
                arl.Add(new { Value = pkg.PackageCode.ToString(), Display = pkg.Description });
            }

            return new JsonResult { Data = arl };
        }

        public JsonResult PopUpPackageCapacity(string packageCode)
        {
            List<PackageLightCapacityPop> lstPackage = new List<PackageLightCapacityPop>();
            lstPackage = salesDal.ReadPopUpPackageCapacity(packageCode);

            ArrayList arl = new ArrayList();

            foreach (PackageLightCapacityPop pkg in lstPackage)
            {
                arl.Add(new { Value = pkg.Capacity.ToString(), Display = pkg.PackageCapacity });

               // Session["CapacityIDForPckg"] = pkg.Capacity.ToString();
            }

            return new JsonResult { Data = arl };
        }

        //...for light.....//
        public JsonResult PopUpPackageLight(string packageCode) 
        {
            List<PackageLightCapacityPop> lstPackage = new List<PackageLightCapacityPop>();
            lstPackage = salesDal.ReadPopUpPackageCapacity(packageCode);

            ArrayList arl = new ArrayList();

            foreach (PackageLightCapacityPop pkg in lstPackage)
            {
                arl.Add(new { Value = pkg.Light.ToString(), Display = pkg.PackageLight });

                //Session["LightIDForPckg"] = pkg.Light.ToString();
            }

            return new JsonResult { Data = arl };
        }


        public JsonResult PackgeListFrmPopUpTddlPckg(string salesType, string packageCode) 
        {
            string capacityId = "";//= Session["CapacityIDForPckg"].ToString();

            List<PackageLightCapacityPop> lstPackage = new List<PackageLightCapacityPop>();
            lstPackage = salesDal.ReadPopUpPackageCapacity(packageCode);

            //ArrayList arl = new ArrayList();

            foreach (PackageLightCapacityPop pkg in lstPackage)
            {
                //arl.Add(new { Value = pkg.Capacity.ToString(), Display = pkg.PackageCapacity });
                capacityId = pkg.Capacity.ToString();
                //Session["CapacityIDForPckg"] = pkg.Capacity.ToString();
            }





            string lightId = "";// Session["LightIDForPckg"].ToString();
            List<PackageLightCapacityPop> lstPackageLight = new List<PackageLightCapacityPop>();
            lstPackageLight = salesDal.ReadPopUpPackageCapacity(packageCode);

            //ArrayList arl = new ArrayList();

            foreach (PackageLightCapacityPop pkg in lstPackageLight)
            {
                //arl.Add(new { Value = pkg.Light.ToString(), Display = pkg.PackageLight });

                //Session["LightIDForPckg"] = pkg.Light.ToString();
                lightId = pkg.Light.ToString();
            }



            List<Sal_PackageMaster> lstPackageddl = new List<Sal_PackageMaster>();
            lstPackageddl = salesDal.ReadPackagesPackgeListFrmPopUpTddlPckg(capacityId, lightId, Convert.ToByte(salesType)); 

            ArrayList arl = new ArrayList();

            foreach (Sal_PackageMaster pkg in lstPackageddl)
            {
                arl.Add(new { Value = pkg.PackageCode.ToString(), Display = pkg.Description });
            }
            //Session["CapacityIDForPckg"] = null;
            //Session["LightIDForPckg"] = null;
            return new JsonResult { Data = arl };
        }


        public JsonResult SalesAgreementDetails(string packageCode, string programCode, string projectCode, string paymentMode, string customerType, string dpAmountClient, string isResaleOrNewSales, string panelSerial, string resalePackagePrice)
        {
            List<PackageDetails> lstPackageDetails = new List<PackageDetails>();

            DiscountPolicy objDiscountPolicy = new DiscountPolicy();
            ServiceChargeInformation objServiceChargePolicy = new ServiceChargeInformation();
            DownPaymentPolicy objPackageVDownpaymnet = new DownPaymentPolicy();
            DepretiatedPackagePriceBySRPanelSerial objDepreciatedPackagePrice = new DepretiatedPackagePriceBySRPanelSerial();

            //decimal? packagePrice = 0;decimal discountPercentage = 0, actualPayableAmount = 0, dPAmount = 0, restAmount = 0, totalServiceCharge = 0, installmentSizeAmount = 0;
            //decimal monthlyPrincipalPayable = 0, discountAmount = 0, dPParcentage = 0, lowerLimitPackagePrice = 0, originalPackagePrice = 0;
            //decimal serviceCharge = 0, monthlyServiceChargePayable = 0, totalPayablewithServiceCharge = 0, totalYears = 0, installmentInMonth = 0;
            //int modeOfPayment = 0;

            decimal packagePrice = 0; decimal discountPercentage = 0, actualPayableAmount = 0, dPAmount = 0, restAmount = 0, totalServiceCharge = 0, installmentSizeAmount = 0;
            decimal monthlyPrincipalPayable = 0, discountAmount = 0, dPParcentage = 0;
            decimal originalPackagePrice = 0;
            decimal lowerLimitPackagePrice = 0;
            decimal serviceCharge = 0, monthlyServiceChargePayable = 0, totalPayablewithServiceCharge = 0, totalYears = 0, installmentInMonth = 0;
            int modeOfPayment = 0;

            string calculationMesage = string.Empty;
            ArrayList alSalAgr = new ArrayList();

            try
            {
                lstPackageDetails = (List<PackageDetails>)Session["PackageDetails"];

                modeOfPayment = Helper.PaymentMode(paymentMode);

                decimal depreciatedPackagePrice = 0;

                if (Convert.ToInt32(isResaleOrNewSales.Trim()) == 1)
                {
                    if (!string.IsNullOrEmpty(panelSerial.Trim()))
                    {
                        objDepreciatedPackagePrice = salesDal.GetDepretiatedPackagePriceBySRPanelSerial(panelSerial.Trim(), packageCode);
                        depreciatedPackagePrice = objDepreciatedPackagePrice != null ? Convert.ToDecimal(objDepreciatedPackagePrice.DepretiatedPackagePrice) : 0;
                        lowerLimitPackagePrice = depreciatedPackagePrice;

                        if (Convert.ToDecimal(resalePackagePrice.Trim()) > 0)
                        {
                            depreciatedPackagePrice = Convert.ToDecimal(resalePackagePrice.Trim());
                        }
                    }
                }

                if (lstPackageDetails != null)
                {
                    if (lstPackageDetails.Count > 0)
                    {
                        if (Convert.ToInt32(isResaleOrNewSales.Trim()) != 0)
                        {
                            packagePrice = depreciatedPackagePrice;  // Depreciated Package Value
                        }
                        else
                        {
                            packagePrice = lstPackageDetails[0].PerUnitSalesPrice;
                            lowerLimitPackagePrice = packagePrice;
                        }

                        originalPackagePrice = lstPackageDetails[0].PerUnitSalesPrice;

                        objDiscountPolicy = salesDal.ReadDiscountPolicyByModeofPaymentNPackageId(paymentMode, packageCode);
                        objServiceChargePolicy = salesDal.ReadServiceChargePolicy(programCode, lstPackageDetails[0].CustomerType, paymentMode);
                        objPackageVDownpaymnet = salesDal.ReadDownPaymentPolicy(paymentMode, packageCode);

                        installmentInMonth = modeOfPayment;
                        totalYears = (decimal)(installmentInMonth / 12);

                        if (modeOfPayment == 0)
                        {
                            if (objDiscountPolicy != null)
                            {
                                discountPercentage = objDiscountPolicy.IsDiscountAFixedAmount == false ? objDiscountPolicy.DiscountPercentage : Convert.ToDecimal(((objDiscountPolicy.DiscountFixedAmount / packagePrice) * 100).ToString("0.00"));
                                discountAmount = objDiscountPolicy.IsDiscountAFixedAmount == false ? Math.Round((packagePrice * objDiscountPolicy.DiscountPercentage) / 100) : objDiscountPolicy.DiscountFixedAmount;
                            }

                            actualPayableAmount = packagePrice - discountAmount;
                            restAmount = actualPayableAmount;
                        }
                        else
                        {
                            actualPayableAmount = packagePrice;

                            if (Convert.ToDecimal(dpAmountClient) == 0)
                            {
                                if (objPackageVDownpaymnet != null)
                                {
                                    dPParcentage = objPackageVDownpaymnet.IsDPAFixedAmount == false ? objPackageVDownpaymnet.DownPaymentPercentage : Convert.ToDecimal(((objPackageVDownpaymnet.DownPaymentFixedAmount / actualPayableAmount) * 100).ToString("0.00"));
                                    dPAmount = objPackageVDownpaymnet.IsDPAFixedAmount == false ? Math.Round((actualPayableAmount * dPParcentage) / 100, 0, MidpointRounding.AwayFromZero) : objPackageVDownpaymnet.DownPaymentFixedAmount;
                                }
                            }
                            else
                            {
                                dPAmount = Convert.ToDecimal(dpAmountClient);
                                dPParcentage = ((dPAmount / actualPayableAmount) * 100);

                                dPParcentage = Convert.ToDecimal(dPParcentage.ToString("0.00"));

                                if (dPParcentage < objPackageVDownpaymnet.DownPaymentPercentage)
                                {
                                    dPParcentage = objPackageVDownpaymnet.DownPaymentPercentage;
                                    dPAmount = Math.Round((actualPayableAmount * dPParcentage) / 100, 0, MidpointRounding.AwayFromZero);

                                    calculationMesage = "DP Should Not Be Less Than " + objPackageVDownpaymnet.DownPaymentPercentage.ToString("0") + "(%)";
                                }
                            }

                            restAmount = actualPayableAmount - dPAmount;

                            //if (customerType != null || customerType.Trim() == "")
                            //{
                            //    if (customerType == "RELINS")
                            //    {
                            //        customerType = "INSTIT";
                            //    }
                            //    else
                            //    {
                            //        customerType = "PERSNL";
                            //    }
                            //}

                            if (customerType == Helper.CustomerReligiousInstitute && totalYears == 1)
                            {
                                serviceCharge = 0;
                                totalServiceCharge = 0;

                                monthlyPrincipalPayable = Math.Round(restAmount / installmentInMonth, 0, MidpointRounding.AwayFromZero);
                            }
                            else
                            {
                                if (objServiceChargePolicy != null)
                                {
                                    serviceCharge = objServiceChargePolicy.ServiceChargePercentage;
                                }

                                totalServiceCharge = Math.Round(((restAmount * serviceCharge) / 100) * totalYears);

                                monthlyPrincipalPayable = Math.Round(restAmount / installmentInMonth, 0, MidpointRounding.AwayFromZero);
                                monthlyServiceChargePayable = Math.Round(totalServiceCharge / installmentInMonth, 0, MidpointRounding.AwayFromZero);
                            }

                            installmentSizeAmount = monthlyPrincipalPayable + monthlyServiceChargePayable;
                            totalPayablewithServiceCharge = restAmount + totalServiceCharge;
                        }
                    }
                }

                alSalAgr.Add(new
                {
                    packagePrice = packagePrice,
                    discountPercentage = discountPercentage,
                    discountAmount = discountAmount,
                    actualPayableAmount = actualPayableAmount,
                    dPParcentage = dPParcentage,
                    dPAmount = dPAmount,
                    restAmount = restAmount,
                    serviceCharge = serviceCharge,
                    totalServiceCharge = totalServiceCharge,
                    installmentInMonth = installmentInMonth,
                    totalYears = totalYears,
                    installmentSizeAmount = installmentSizeAmount,
                    totalPayablewithServiceCharge = totalPayablewithServiceCharge,
                    monthlyPrincipalPayable = monthlyPrincipalPayable,
                    monthlyServiceChargePayable = monthlyServiceChargePayable,
                    calculationMesage = calculationMesage,
                    originalPackagePrice = originalPackagePrice,
                    lowerLimitPackagePrice = lowerLimitPackagePrice,
                    isPackagePriceFixed = objDepreciatedPackagePrice.IsPackagePriceFixed,
                    panelError = ""
                });
            }
            catch (Exception ex)
            {
                calculationMesage = ExceptionHelper.ExceptionMessageOnly(ex);

                alSalAgr.Add(new
                {
                    packagePrice = packagePrice,
                    discountPercentage = discountPercentage,
                    discountAmount = discountAmount,
                    actualPayableAmount = actualPayableAmount,
                    dPParcentage = dPParcentage,
                    dPAmount = dPAmount,
                    restAmount = restAmount,
                    serviceCharge = serviceCharge,
                    totalServiceCharge = totalServiceCharge,
                    installmentInMonth = installmentInMonth,
                    totalYears = totalYears,
                    installmentSizeAmount = installmentSizeAmount,
                    totalPayablewithServiceCharge = totalPayablewithServiceCharge,
                    monthlyPrincipalPayable = monthlyPrincipalPayable,
                    monthlyServiceChargePayable = monthlyServiceChargePayable,
                    calculationMesage = calculationMesage,
                    originalPackagePrice = originalPackagePrice,
                    lowerLimitPackagePrice = lowerLimitPackagePrice,
                    isPackagePriceFixed = objDepreciatedPackagePrice.IsPackagePriceFixed,
                    panelError = calculationMesage
                });
            }

            return new JsonResult { Data = alSalAgr };
        }

        public JsonResult GenerateCustomerID(string customerSerials)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            string message = string.Empty, customerCode = string.Empty;

            Sal_Customer objCustomer = new Sal_Customer();

            //string prefixCode = objLoginHelper.CustomerPrefix;
            //string newPrefixCode = customerCode.Substring(0, customerCode.IndexOf('-')) + "-";
            //string customerCodeWithoutPrefix = customerCode.Substring(customerCode.IndexOf('-') + 1).Replace("-", "");

            int customerCodeLength = customerSerials.Length;

            if (customerCodeLength < 9)
            {
                StringBuilder sb = new StringBuilder();
                int remainLength = 9 - customerCodeLength - objLoginHelper.CustomerPrefix.Length;

                sb.Append(objLoginHelper.CustomerPrefix);
                sb.Append('0', remainLength);
                sb.Append(customerSerials);

                customerCode = sb.ToString();
            }

            if (customerCodeLength > 9)
            {
                return new JsonResult { Data = "Code" };
            }

            objCustomer = salesDal.ReadCustomer(objLoginHelper.LogInForUnitCode, customerCode);

            if (objCustomer != null)
            {
                return new JsonResult { Data = "Already" };
            }

            //bool idContainsNumberOrNot = Regex.IsMatch(customerCode, @"^\d+$");

            //if (idContainsNumberOrNot == true)
            //{
            //    return new JsonResult { Data = "Number" };
            //}
            //else 

            if (customerCode.Length > 9)
            {
                return new JsonResult { Data = "Code" };
            }

            return new JsonResult { Data = customerCode };
        }

        public JsonResult CapacityList(string projectCode)
        {
            ArrayList arl = new ArrayList();

            List<Sal_PackageOrItemCapacity> lstCapacity = new List<Sal_PackageOrItemCapacity>();
            lstCapacity = salesDal.ReadCapacityByProjectCode(projectCode, Helper.IsCapacityOnlyForPackagesAndItems);

            foreach (Sal_PackageOrItemCapacity PIC in lstCapacity)
            {
                arl.Add(new { Value = PIC.CapacityID, Display = PIC.Description });
            }

            return new JsonResult { Data = arl };
        }

        public JsonResult LightList(string capacityId)
        {
            List<Sal_Light> lstLight = new List<Sal_Light>();
            lstLight = salesDal.ReadLightByCapacityId(capacityId);

            return new JsonResult { Data = lstLight };
        }

        public JsonResult ItemModelList(string itemCategoryId, string itemCapacityId)
        {
            List<Inv_ItemModel> lstemModel = new List<Inv_ItemModel>();

            string[] itemCategory = itemCategoryId.Split('#');
            string[] itemCapacity = itemCapacityId.Split('#');

            int itemCategoryLength = itemCategory.Length, itemCapacityLength = itemCapacity.Length, rowCount = 0;
            string itemCategoryIds = string.Empty, itemCapacityIds = string.Empty, itemCategoryIdForNull = string.Empty;

            for (rowCount = 0; rowCount < itemCapacityLength; rowCount++)
            {
                if (itemCapacity[rowCount].Trim() != "NULL" && itemCapacity[rowCount].Trim() != "" && itemCapacity[rowCount].Trim() != "0")
                {
                    itemCapacityIds += "'" + itemCapacity[rowCount].Trim() + "',";
                    itemCategoryIds += "'" + itemCategory[rowCount].Trim() + "',";
                }
                else
                {
                    itemCategoryIdForNull += "'" + itemCategory[rowCount].Trim() + "',";
                }
            }

            itemCapacityIds = itemCapacityIds.Remove(itemCapacityIds.Length - 1);
            itemCategoryIds = itemCategoryIds.Remove(itemCategoryIds.Length - 1);

            if (!string.IsNullOrEmpty(itemCategoryIdForNull))
            {
                itemCategoryIdForNull = itemCategoryIdForNull.Remove(itemCategoryIdForNull.Length - 1);
            }

            lstemModel = salesDal.ReadItemModelByCategoryId(itemCategoryIds, itemCapacityIds, itemCategoryIdForNull);

            return new JsonResult { Data = lstemModel };
        }

        public JsonResult ItemCapacityList(string itemCategoryId)
        {
            List<ItemCapacity> lsItemCapacity = new List<ItemCapacity>();

            string[] itemCategory = itemCategoryId.Split('#');

            int itemCategoryLength = itemCategory.Length, rowCount = 0;
            string itemCategoryIds = string.Empty;

            for (rowCount = 0; rowCount < itemCategoryLength; rowCount++)
            {
                itemCategoryIds += "'" + itemCategory[rowCount].Trim() + "',";
            }

            itemCategoryIds = itemCategoryIds.Remove(itemCategoryIds.Length - 1);

            lsItemCapacity = salesDal.ReadItemCapacityByCategoryId(itemCategoryIds);

            return new JsonResult { Data = lsItemCapacity };
        }

        public JsonResult ModeOfPaymentList(string locationCode, string programCode, string packageCode, string customerType, string agreementDatePicker)
        {
            List<Sal_ModeOfPayment> lstModeOfPayment = new List<Sal_ModeOfPayment>();
            lstModeOfPayment = salesDal.ReadModeOfPayment(locationCode, programCode, packageCode, customerType, agreementDatePicker);

            ArrayList arr = new ArrayList();

            foreach (Sal_ModeOfPayment mop in lstModeOfPayment)
            {
                arr.Add(new { Value = mop.ModeOfPaymentID, Display = mop.Description });
            }

            return new JsonResult { Data = arr };
        }

        public JsonResult LoadPanleSerialsList(string storeLocation, string itemCapacity, string agreementType, string packageCode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            List<PanelSerialList> lstPanel = new List<PanelSerialList>();
            ArrayList arr = new ArrayList();

            //List<PackageDetails> lstPackageDetails = new List<PackageDetails>();
            //lstPackageDetails = (List<PackageDetails>)Session["PackageDetails"];

            string itemCategory = "";// lstPackageDetails[0].ItemCategoryID;

            lstPanel = salesDal.PanelSerialByLocationAndStock(objLoginHelper.LocationCode, Convert.ToByte(storeLocation), itemCategory, itemCapacity, Convert.ToByte(agreementType), packageCode.Trim());

            foreach (PanelSerialList panelSerials in lstPanel)
            {
                arr.Add(new { PanelSerial = panelSerials.ItemSerialNo, PanelModel = panelSerials.ItemModel });
            }

            return new JsonResult { Data = arr };
        }

        private Sal_Customer AssignCustomerInfo(FormCollection fCollection)
        {
            Sal_Customer objCustomer = new Sal_Customer();

            objCustomer.CustomerCode = fCollection["hfCustomerCode"];
            objCustomer.CustomerName = fCollection["txtCustomerName"];
            objCustomer.FathersOrHusbandName = fCollection["txtFatherHusbandName"];
            objCustomer.MothersName = fCollection["txtMotherName"];
            objCustomer.Gender = fCollection["ddlGender"];
            objCustomer.NationalIDCard = fCollection["txtNationalIdNumber"];
            objCustomer.PhoneNo = fCollection["txtMobileNumber"];
            objCustomer.Village = fCollection["txtVillage"];
            objCustomer.PostOffice = fCollection["txtPostOffice"];
            objCustomer.ThanaID = fCollection["ddlUpazillaThana"];
            objCustomer.DistrictCode = salesDal.ReadUpazillaByID(fCollection["ddlUpazillaThana"]).DIST_CODE;
            objCustomer.UnitCode = objLoginHelper.LogInForUnitCode;
            objCustomer.CustomerType = fCollection["ddlCustomerType"];

            objCustomer.Occupation = fCollection["ddlOccupation"].Trim();
            objCustomer.IncomeRange = fCollection["ddlIncomeRange"].Trim();
            objCustomer.TotalFamilyMember = Convert.ToByte(fCollection["txtFamilyMember"].Trim());

            if (!string.IsNullOrEmpty(fCollection["txtWomenMember"].Trim()))
            {
                objCustomer.WomenInTotalFamilyMember = Convert.ToByte(fCollection["txtWomenMember"].Trim());
            }
            else
            {
                objCustomer.WomenInTotalFamilyMember = 0;
            }

            if (fCollection["ddlIsConsultedWithWomenMemberForInstallationOfLamps"].Trim() == "0")
                objCustomer.IsConsultedWithWomenMemberForInstallationOfLamps = false;
            else
                objCustomer.IsConsultedWithWomenMemberForInstallationOfLamps = true;

            objCustomer.FuelUsedBeforeSHS = fCollection["ddlFuelUsedBeforeSHS"].Trim();
            objCustomer.FuelConsumptionPerMonth = Convert.ToByte(fCollection["txtFuelConsumptionPerMonthLitre"].Trim());
            objCustomer.RelationWithGuardian = fCollection["ddlGurdianRelation"].Trim();
            objCustomer.UnionID = fCollection["ddlUnion"].Trim();

            objCustomer.CreatedBy = objLoginHelper.LogInID;
            objCustomer.CreatedOn = DateTime.Now;

            if (fCollection["ddlModeOfPayment"].ToLower().Contains("cash"))
                objCustomer.Status = Helper.CashSalesCustomer;
            else
                objCustomer.Status = Helper.Active;

            //objCustomer.ModifiedBy = "";
            //objCustomer.ModifiedOn = DateTime.Now;
            //objCustomer.StatusChangedDate = DateTime.Now;

            return objCustomer;
        }

        private Sal_SalesAgreement AssignSalesAgreement(FormCollection fCollection)
        {
            Sal_SalesAgreement objSalesAgreement = new Sal_SalesAgreement();
            List<PackageDetails> lstPackageDetails = new List<PackageDetails>();

            // DiscountPolicy objDiscountPolicy = new DiscountPolicy();
            ServiceChargeInformation objServiceChargePolicy = new ServiceChargeInformation();
            DownPaymentPolicy objPackageVDownpaymnet = new DownPaymentPolicy();

            lstPackageDetails = (List<PackageDetails>)Session["PackageDetails"];

            bool reSaleStatus = false;

            if (Convert.ToInt32(fCollection["hfIsReSalesOrSales"].Trim()) == 1)  // If The Sales Agreement Is Resale
            {
                reSaleStatus = true;
            }

            if (fCollection["ddlProgram"] != "BIO001")
            {
                //objDiscountPolicy = salesDal.ReadDiscountPolicyByModeofPaymentNPackageId(fCollection["ddlModeOfPayment"], fCollection["ddlPackage"]);
                objServiceChargePolicy = salesDal.ReadServiceChargePolicy(fCollection["ddlProgram"], fCollection["ddlCustomerType"], fCollection["ddlModeOfPayment"]);
                objPackageVDownpaymnet = salesDal.ReadDownPaymentPolicy(fCollection["ddlModeOfPayment"], fCollection["ddlPackage"]);

                objSalesAgreement.CustomerCode = fCollection["hfCustomerCode"];
                objSalesAgreement.AgreementDate = Convert.ToDateTime(Helper.DateFormatMMDDYYYY(fCollection["dtpAgreementDate"]));
                objSalesAgreement.PackageCode = fCollection["ddlPackage"];
                objSalesAgreement.ModeOfPaymentID = fCollection["ddlModeOfPayment"];
                objSalesAgreement.CustomerType = fCollection["ddlCustomerType"];
                objSalesAgreement.NumberOfInstallment = Convert.ToByte(fCollection["txtInstallments"]);

                objSalesAgreement.PackagePrice = Convert.ToDecimal(fCollection["txtPackagePrice"].Trim());

                objSalesAgreement.DiscountID = lstPackageDetails[0].DiscountID;
                objSalesAgreement.DiscountAmount = Convert.ToDecimal(fCollection["txtDiscountAmount"]);

                objSalesAgreement.AmountAfterDiscount = Convert.ToDecimal(fCollection["txActualPayableAmount"]);

                objSalesAgreement.DownPaymentID = objPackageVDownpaymnet.DownPaymentID;
                objSalesAgreement.STDDownPaymentPercentage = objPackageVDownpaymnet.DownPaymentPercentage;
                objSalesAgreement.DownPaymentAmount = Convert.ToDecimal(fCollection["txtDPAmount"]);

                objSalesAgreement.ServiceChargeID = objServiceChargePolicy.ServiceChargeID;
                objSalesAgreement.STDServiceChargePercentage = objServiceChargePolicy.ServiceChargePercentage;

                objSalesAgreement.TotalPrincipalReceivable = Convert.ToDecimal(fCollection["txtRestAmount"]);
                objSalesAgreement.TotalServiceChargeReceivable = Convert.ToDecimal(fCollection["txtTotalServiceCharge"]); ;
                objSalesAgreement.TotalPrincipalPlusServiceChargeReceivable = Convert.ToDecimal(fCollection["txtTotalPayableWithServiceCharge"]);
                objSalesAgreement.InstallmentSize = Convert.ToDecimal(fCollection["txtInstallmentAmount"]);
                objSalesAgreement.InstallmentSizePrincipal = Convert.ToDecimal(fCollection["txtMonthlyPrincipalPayable"]);
                objSalesAgreement.InstallmentSizeServiceCharge = Convert.ToDecimal(fCollection["txtMonthlyServiceChargePayable"]);
                objSalesAgreement.ScheduledCollectionDay = Convert.ToByte(fCollection["ddlCollectionDay"].Trim());
                objSalesAgreement.ProjectCode = fCollection["ddlProject"];
                objSalesAgreement.ProgramCode = fCollection["ddlProgram"];

                if (fCollection["txtMemoNumber"].Trim() != "")
                    objSalesAgreement.RefMemoNo = fCollection["txtMemoNumber"];

                objSalesAgreement.TechnicalFees = 0;
                objSalesAgreement.Subsidies = 0;
                objSalesAgreement.DisbursementNo = 0;
                objSalesAgreement.SalespersonCode = fCollection["ddlEmployee"];
                objSalesAgreement.IsReSales = reSaleStatus;

                //objSalesAgreement.ModifiedBy = "";
                //objSalesAgreement.ModifiedOn = DateTime.Now;
            }
            else if (fCollection["ddlProgram"] == "BIO001")
            {

            }

            return objSalesAgreement;
        }

        private void AssignSalesItem(FormCollection fCollection, List<Inv_ItemMaster> lstItemMaster)
        {
            int modelCounter = 0, serialCounter = 0;
            string itemCode = string.Empty, modelCode = string.Empty;
            byte storeLocation = 1;

            Sal_SalesItems objSalesItem;
            Sal_SalesItemsWithSerialNo objItemSalesWithSerialNo;

            List<PackageDetails> lstPackageDetails = new List<PackageDetails>();
            serialTempTableRows = string.Empty;

            try
            {
                lstPackageDetails = (List<PackageDetails>)Session["PackageDetails"];

                string[] packageComponents = fCollection["hfStockLocationWiseComponenet"].Split('#');

                foreach (PackageDetails pkgd in lstPackageDetails)
                {
                    objSalesItem = new Sal_SalesItems();
                    objSalesItem.CustomerCode = fCollection["hfCustomerCode"].Trim();
                    objSalesItem.CompSeqNo = pkgd.CompSeqNo;
                    objSalesItem.ItemQuantity = Convert.ToDouble(pkgd.FixedQuantity);
                    objSalesItem.ItemCategory = pkgd.ItemCategoryID;
                    objSalesItem.ItemCapacity = pkgd.ItemCapacity;
                    objSalesItem.UnitOfMeasure = pkgd.UnitOfMeasure;

                    if (pkgd.IsShowInSalesAgreementPage == true)
                    {
                        string[] componentStoreLocation = packageComponents[serialCounter].Split('_');
                        storeLocation = Convert.ToByte(componentStoreLocation[3].Trim());  // Item Store Location
                        objSalesItem.FromStoreLocation = storeLocation;
                    }

                    if (pkgd.IsShowInSalesAgreementPage == true && pkgd.ItemModel.Trim() == "")
                    {
                        string[] componentsDeatils = packageComponents[modelCounter].Split('_');

                        if (componentsDeatils[0] == pkgd.ItemCategoryID)
                            objSalesItem.ItemModel = componentsDeatils[1];  // Item Model

                        modelCode = objSalesItem.ItemModel;

                        modelCounter++;
                    }
                    else
                    {
                        if (pkgd.ItemModel.Trim() != "")
                        {
                            objSalesItem.ItemModel = pkgd.ItemModel;
                            modelCode = objSalesItem.ItemModel;
                        }
                        else
                        {
                            objSalesItem.ItemModel = null;
                            modelCode = string.Empty;
                        }
                        modelCounter++;
                    }

                    if (modelCode != string.Empty)
                    {
                        if (pkgd.ItemCapacity.Trim() == "NULL" || pkgd.ItemCapacity.Trim() == "")
                        {
                            var vIt = (from itc in lstItemMaster where itc.ItemCategory == pkgd.ItemCategoryID && itc.ItemCapacity == null && itc.ItemModel == objSalesItem.ItemModel select itc.ItemCode).FirstOrDefault();
                            objSalesItem.ItemCode = vIt.ToString();
                        }
                        else
                        {
                            var vIt = (from itc in lstItemMaster where itc.ItemCategory == pkgd.ItemCategoryID && itc.ItemCapacity == pkgd.ItemCapacity && itc.ItemModel == objSalesItem.ItemModel select itc.ItemCode).FirstOrDefault();
                            objSalesItem.ItemCode = vIt.ToString();
                        }
                    }
                    else
                    {
                        if (pkgd.ItemCapacity.Trim() == "NULL" || pkgd.ItemCapacity.Trim() == "")
                        {
                            var vIt = (from itc in lstItemMaster where itc.ItemCategory == pkgd.ItemCategoryID && itc.ItemCapacity == null && itc.ItemModel == null select itc.ItemCode).FirstOrDefault();
                            objSalesItem.ItemCode = vIt.ToString();
                        }
                        else
                        {
                            var vIt = (from itc in lstItemMaster where itc.ItemCategory == pkgd.ItemCategoryID && itc.ItemCapacity == pkgd.ItemCapacity && itc.ItemModel == null select itc.ItemCode).FirstOrDefault();
                            objSalesItem.ItemCode = vIt.ToString();
                        }
                    }

                    objSalesItem.UnitCost = (from itc in lstItemMaster where itc.ItemCode == objSalesItem.ItemCode select itc.AverageUnitCost).FirstOrDefault();

                    if (pkgd.IsShowInSalesAgreementPage == true && pkgd.IsSerialNoMandatory == true)
                    {
                        string[] componentsDeatils = packageComponents[serialCounter].Split('_');

                        objItemSalesWithSerialNo = new Sal_SalesItemsWithSerialNo();

                        objItemSalesWithSerialNo.CustomerCode = fCollection["hfCustomerCode"];
                        objItemSalesWithSerialNo.CompSeqNo = pkgd.CompSeqNo;
                        objItemSalesWithSerialNo.ItemSerialNo = componentsDeatils[2].Trim();
                        objItemSalesWithSerialNo.Status = Helper.Active;
                        objItemSalesWithSerialNo.ItemCode = objSalesItem.ItemCode;
                        objItemSalesWithSerialNo.RefStoreLocation = storeLocation;  // Item Store Location
                        objItemSalesWithSerialNo.RefLocationCode = objLoginHelper.LocationCode;

                        lstItemWithSerials.Add(objItemSalesWithSerialNo);

                        if (!string.IsNullOrEmpty(serialTempTableRows))
                            serialTempTableRows += "," + "(" + objSalesItem.ItemCode + ",'" + objItemSalesWithSerialNo.ItemSerialNo + "','" + pkgd.ItemCategoryID + "'," + Convert.ToByte(objItemSalesWithSerialNo.RefStoreLocation) + ",'" + objLoginHelper.LocationCode + "','" + fCollection["hfCustomerCode"].Trim() + "')";
                        else
                            serialTempTableRows = "(" + objSalesItem.ItemCode + ",'" + objItemSalesWithSerialNo.ItemSerialNo + "','" + pkgd.ItemCategoryID + "'," + Convert.ToByte(objItemSalesWithSerialNo.RefStoreLocation) + ",'" + objLoginHelper.LocationCode + "','" + fCollection["hfCustomerCode"].Trim() + "')";

                    }

                    serialCounter++;
                    lstSalesItem.Add(objSalesItem);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult UnionInfoLoad(string upazillaCode)
        {
            List<Common_UnionInfo> lstUnion = new List<Common_UnionInfo>();
            lstUnion = salesDal.ReadUnionInfo(upazillaCode);

            ArrayList arr = new ArrayList();

            foreach (Common_UnionInfo ui in lstUnion)
            {
                arr.Add(new { Display = ui.UnionName, Value = ui.UnionID });
            }

            return new JsonResult { Data = arr };
        }

        public JsonResult PostOfficeInfoLoad(string upazillaCode)
        {
            List<Common_PostOfficeInfo> lstUnion = new List<Common_PostOfficeInfo>();
            lstUnion = salesDal.ReadPostOfficeInfo(upazillaCode);

            ArrayList arr = new ArrayList();

            foreach (Common_PostOfficeInfo ui in lstUnion)
            {
                arr.Add(new { Display = ui.PostOfficeName, Value = ui.PostOfficeID });
            }

            return new JsonResult { Data = arr };
        }

        public JsonResult PostOfficeInfoLoadForUnitCode(string upazillaCode) 
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            string unitCode = objLoginHelper.LogInForUnitCode;
            List<Common_PostOfficeInfo> lstUnion = new List<Common_PostOfficeInfo>();
            lstUnion = salesDal.PostOfficeInfoLoadForUnitCode(unitCode); 

            ArrayList arr = new ArrayList();

            foreach (Common_PostOfficeInfo ui in lstUnion)
            {
                arr.Add(new { Display = ui.PostOfficeName, Value = ui.PostOfficeID });
            }

            return new JsonResult { Data = arr };
        }
        //CustomerPostOfficeInfoLoad

        public JsonResult CustomerPostOfficeInfoLoad(string CustomerCode, string PostOffice) 
        {
            List<Common_PostOfficeInfo> lstUnion = new List<Common_PostOfficeInfo>();
            lstUnion = salesDal.ReadCustomerPostOfficeInfo(CustomerCode, PostOffice);

            ArrayList arr = new ArrayList();

            foreach (Common_PostOfficeInfo ui in lstUnion)
            {
                arr.Add(new { Display = ui.PostOfficeName, Value = ui.PostOfficeID });
            }

            return new JsonResult { Data = arr };
        }



        public JsonResult DistrictInfoLoad(string upazillaCode)
        {
            List<Common_DistrictInfo> lstDistrict = new List<Common_DistrictInfo>();
            lstDistrict = salesDal.ReadDistrictInfo(upazillaCode);

            ArrayList arr = new ArrayList();

            foreach (Common_DistrictInfo ui in lstDistrict)
            {
                arr.Add(new { Display = ui.DIST_NAME, Value = ui.DIST_CODE });
            }

            return new JsonResult { Data = arr };
        }

        public JsonResult SaveSparePartsIssue(string sparseMaster, string sparseWithSerials, string challanDate, string sparseSequenceNumber, string memoNumber, string itemType, string salesPersons)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            Inv_ChallanMaster objChallanMaster; Inv_ChallanDetails objChallanDetails;
            Inv_ChallanDetailsWithSerialNo objChallanDetailsWithSerialNo;

            List<Inv_ChallanDetails> lstChallanDetails = new List<Inv_ChallanDetails>();
            List<Inv_ChallanDetailsWithSerialNo> lstChallanDetailsWithSerialNo = new List<Inv_ChallanDetailsWithSerialNo>();

            string validItemTransTypeForMRR = string.Empty, sparePartsChallanType = string.Empty;

            string challanNumberMax = inventoryDal.ChallanSequenceNumberMax(objLoginHelper.LocationCode, objLoginHelper.TransactionOpenDate.ToString("yyMMdd"));
            string challanSequenceNumber = Helper.ChallanCequenceNumberGeneration(challanNumberMax, objLoginHelper);

            try
            {
                string[] sparesMasterIssue = sparseMaster.Split('#');
                string[] sparseMasterWithSerialsIssue = sparseWithSerials.Split('#');

                int challanLength = sparesMasterIssue.Length, challanCounter = 0, serialLength = sparseMasterWithSerialsIssue.Length, serialCounter = 0;
                decimal salesAmount = 0;
                string[] challanMasterItem = sparesMasterIssue[0].Split(',');

                validItemTransTypeForMRR = Helper.MrrType(challanMasterItem[1].Trim(), objLoginHelper.Location);
                sparePartsChallanType = challanMasterItem[1].Trim();

                objChallanMaster = new Inv_ChallanMaster();
                objChallanMaster.LocationCode = objLoginHelper.LocationCode;
                objChallanMaster.ChallanSeqNo = challanSequenceNumber.Trim();
                objChallanMaster.ChallanDate = Convert.ToDateTime(Helper.DateFormatMMDDYYYY(challanDate.Trim()));
                objChallanMaster.FromStoreLocation = Convert.ToByte(challanMasterItem[2].Trim());        // Store Location Code 
                objChallanMaster.RefChallanNo = null;

                objChallanMaster.ValidItemTransTypeForMRR = !string.IsNullOrEmpty(validItemTransTypeForMRR) ? validItemTransTypeForMRR : null;

                if (challanMasterItem[3].Trim() != "")
                {
                    objChallanMaster.ChallanToLocationCode = challanMasterItem[3].Trim();                   // Challan To Location
                    objChallanMaster.IsItForMultipleLocations = false;
                }
                else
                {
                    objChallanMaster.ChallanToLocationCode = null;
                    objChallanMaster.IsItForMultipleLocations = false;
                }

                objChallanMaster.RefCustomerCode = null;
                objChallanMaster.ItemType = itemType;
                objChallanMaster.ItemTransTypeID = challanMasterItem[1].Trim();                         // Issue Type
                objChallanMaster.CreatedBy = objLoginHelper.LogInID;
                objChallanMaster.CreatedOn = DateTime.Now;
                objChallanMaster.Status = Helper.Active;

                for (challanCounter = 0; challanCounter < challanLength; challanCounter++)
                {
                    string[] challanIssueItem = sparesMasterIssue[challanCounter].Split(',');

                    objChallanDetails = new Inv_ChallanDetails();
                    objChallanDetails.LocationCode = objLoginHelper.LocationCode;
                    objChallanDetails.ChallanSeqNo = challanSequenceNumber;
                    objChallanDetails.CompSeqNo = Convert.ToByte(challanIssueItem[5].Trim());              // Component Sequence Number
                    objChallanDetails.ItemCode = challanIssueItem[0].Trim();                               // Item Code
                    objChallanDetails.DeliveryQuantity = Convert.ToDouble(challanIssueItem[4].Trim());     // Delivery Quantity
                    objChallanDetails.UnitCost = 0;
                    objChallanDetails.FromStoreLocation = Convert.ToByte(challanIssueItem[2].Trim());      // Store Location
                    lstChallanDetails.Add(objChallanDetails);

                    if (serialLength > 0)
                    {
                        for (serialCounter = 0; serialCounter < serialLength; serialCounter++)
                        {
                            string[] challanIssueSerialItem = sparseMasterWithSerialsIssue[serialCounter].Split(',');

                            if (challanIssueSerialItem[0] == challanIssueItem[0])
                            {
                                objChallanDetailsWithSerialNo = new Inv_ChallanDetailsWithSerialNo();
                                objChallanDetailsWithSerialNo.LocationCode = objLoginHelper.LocationCode;
                                objChallanDetailsWithSerialNo.ChallanSeqNo = challanSequenceNumber.Trim();
                                objChallanDetailsWithSerialNo.CompSeqNo = Convert.ToByte(challanIssueItem[5].Trim());          // Component Sequence Number
                                objChallanDetailsWithSerialNo.ItemCode = challanIssueSerialItem[0].Trim();                     // Item Code
                                objChallanDetailsWithSerialNo.ItemSerialNo = challanIssueSerialItem[1].Trim();                 // Item Serial
                                objChallanDetailsWithSerialNo.RefStoreLocation = Convert.ToByte(challanIssueItem[2].Trim());   // Store Location Code
                                objChallanDetailsWithSerialNo.Status = Helper.Active;
                                lstChallanDetailsWithSerialNo.Add(objChallanDetailsWithSerialNo);
                            }
                        }
                    }
                }

                Sal_SparePartsSalesMaster objSparePartsSalesMaster = new Sal_SparePartsSalesMaster();
                Sal_SparePartsSalesItems objSparePartsSalesItems;
                Sal_SparePartsSalesItemsWithSerialNo objSparePartsSalesItemsWithSerialNo;

                List<Sal_SparePartsSalesItems> lstSparePartsSalesItems = new List<Sal_SparePartsSalesItems>();
                List<Sal_SparePartsSalesItemsWithSerialNo> lstSparePartsSalesItemsWithSerialNo = new List<Sal_SparePartsSalesItemsWithSerialNo>();

                if (sparePartsChallanType == Helper.SparsePartsChallan || sparePartsChallanType == Helper.SparsePartsChallanForDisasterRecovery)
                {
                    objSparePartsSalesMaster.LocationCode = objLoginHelper.LocationCode;
                    objSparePartsSalesMaster.SPSSeqNo = sparseSequenceNumber.Trim();
                    objSparePartsSalesMaster.SPSDate = Convert.ToDateTime(Helper.DateFormatMMDDYYYY(challanDate.Trim()));
                    objSparePartsSalesMaster.ProjectCode = null;
                    objSparePartsSalesMaster.ProgramCode = null;
                    objSparePartsSalesMaster.SalesAmountAfterDiscount = 0;
                    //objSparePartsSalesMaster.RefCustomerCode = challanMasterItem[10].ToString();
                    //
                    objSparePartsSalesMaster.RefCustomerCode = challanMasterItem[12].ToString();
                    //
                    if (!erpDal.IsCashMemoManagementEnabled(Helper.CompanyName))
                    {
                        objSparePartsSalesMaster.RefMemoNo = memoNumber.Trim();
                    }
                    else
                    {
                        objSparePartsSalesMaster.CashMemoNo = memoNumber.Trim();
                        objSparePartsSalesMaster.CashMemoUsesID = Helper.CashMemuUsesIdFirst;
                    }

                    objSparePartsSalesMaster.RefAETransNo = null;
                    objSparePartsSalesMaster.FromStoreLocation = Convert.ToByte(challanMasterItem[2].Trim());        // Store Location Code;
                    objSparePartsSalesMaster.ChallanLocationCode = objLoginHelper.LocationCode;
                    objSparePartsSalesMaster.ChallanSeqNo = null;
                    //objSparePartsSalesMaster.ChallanSeqNo = challanSequenceNumber.Trim();
                    objSparePartsSalesMaster.ItemTransTypeID = challanMasterItem[1].Trim();                         // Issue Type
                    objSparePartsSalesMaster.SalespersonCode = salesPersons.Trim();
                    objSparePartsSalesMaster.CreatedBy = objLoginHelper.LogInID;
                    objSparePartsSalesMaster.CreatedOn = DateTime.Now;
                    objSparePartsSalesMaster.Status = Helper.Active;


                    for (challanCounter = 0; challanCounter < challanLength; challanCounter++)
                    {
                        string[] sparseItem = sparesMasterIssue[challanCounter].Split(',');
                        objSparePartsSalesItems = new Sal_SparePartsSalesItems();

                        objSparePartsSalesItems.LocationCode = objLoginHelper.LocationCode;
                        objSparePartsSalesItems.SPSSeqNo = sparseSequenceNumber.Trim();
                        objSparePartsSalesItems.CompSeqNo = Convert.ToByte(sparseItem[5].Trim());              // Component Sequence Number
                        objSparePartsSalesItems.ItemCode = sparseItem[0].Trim();                              // Item Code
                        objSparePartsSalesItems.ItemQuantity = Convert.ToDouble(sparseItem[4].Trim());       // Delivery Quantity
                        objSparePartsSalesItems.UnitCost = 0;
                        if (Convert.ToString(sparseItem[1].Trim()) == "ISSFSPSWDS")
                            //objSparePartsSalesItems.UnitPrice = (Convert.ToDecimal(sparseItem[6].Trim())) - (Convert.ToDecimal(sparseItem[11].Trim())); // Unit Price
                             //
                            objSparePartsSalesItems.UnitPrice = (Convert.ToDecimal(sparseItem[6].Trim())) - (Convert.ToDecimal(sparseItem[13].Trim())); // Unit Price
                            //
                        else
                            objSparePartsSalesItems.UnitPrice = Convert.ToDecimal(sparseItem[6].Trim()); // Unit Price
                        //objSparePartsSalesItems.DiscountForDisasterRecovery = Convert.ToDecimal(sparseItem[11].Trim());
                        //objSparePartsSalesItems.ItemCategory = sparseItem[7].Trim(); // Item Category 
                        //
                        objSparePartsSalesItems.DiscountForDisasterRecovery = Convert.ToDecimal(sparseItem[13].Trim());
                        objSparePartsSalesItems.ItemCategory = sparseItem[9].Trim(); // Item Category 
                        //
                        salesAmount += Convert.ToInt32(objSparePartsSalesItems.ItemQuantity) * objSparePartsSalesItems.UnitPrice;

                        //if (sparseItem[8].Trim() != string.Empty)
                        //    objSparePartsSalesItems.ItemCapacity = sparseItem[8].Trim(); // Item Capacity 
                        //else
                        //    objSparePartsSalesItems.ItemCapacity = null;

                        //if (sparseItem[9].Trim() != string.Empty)
                        //    objSparePartsSalesItems.ItemModel = sparseItem[9].Trim(); // Item Model 

                        //
                        if (sparseItem[10].Trim() != string.Empty)
                            objSparePartsSalesItems.ItemCapacity = sparseItem[10].Trim(); // Item Capacity 
                        else
                            objSparePartsSalesItems.ItemCapacity = null;

                        if (sparseItem[11].Trim() != string.Empty)
                            objSparePartsSalesItems.ItemModel = sparseItem[11].Trim(); // Item Model 
                        //
                        else
                            objSparePartsSalesItems.ItemModel = null;

                        objSparePartsSalesItems.UnitOfMeasure = string.Empty;

                        lstSparePartsSalesItems.Add(objSparePartsSalesItems);

                        if (serialLength > 0)
                        {
                            for (serialCounter = 0; serialCounter < serialLength; serialCounter++)
                            {
                                string[] sparseSerialItem = sparseMasterWithSerialsIssue[serialCounter].Split(',');

                                if (sparseSerialItem[0] == sparseItem[0])
                                {
                                    objSparePartsSalesItemsWithSerialNo = new Sal_SparePartsSalesItemsWithSerialNo();

                                    objSparePartsSalesItemsWithSerialNo.LocationCode = objLoginHelper.LocationCode;
                                    objSparePartsSalesItemsWithSerialNo.SPSSeqNo = sparseSequenceNumber.Trim();
                                    objSparePartsSalesItemsWithSerialNo.CompSeqNo = Convert.ToByte(sparseItem[5].Trim());          // Component Sequence Number
                                    objSparePartsSalesItemsWithSerialNo.ItemCode = sparseSerialItem[0].Trim();                     // Item Code
                                    objSparePartsSalesItemsWithSerialNo.ItemSerialNo = sparseSerialItem[1].Trim();                 // Item Serial
                                    objSparePartsSalesItemsWithSerialNo.RefStoreLocation = Convert.ToByte(sparseItem[2].Trim());   // Store Location Code
                                    objSparePartsSalesItemsWithSerialNo.Status = Helper.Active;

                                    lstSparePartsSalesItemsWithSerialNo.Add(objSparePartsSalesItemsWithSerialNo);
                                }
                            }
                        }
                    }

                    objSparePartsSalesMaster.SalesAmountAfterDiscount = salesAmount;
                }

                objSparePartsSalesMaster = salesDal.SaveSparePartsChallan(objChallanMaster, lstChallanDetails, lstChallanDetailsWithSerialNo, objSparePartsSalesMaster, lstSparePartsSalesItems, lstSparePartsSalesItemsWithSerialNo);

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Spare Parts Sales" + Helper.SuccessMessage, Helper.ChallanCequenceNumberGeneration(objSparePartsSalesMaster.SPSSeqNo, objLoginHelper)) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        [HttpPost]
        public ActionResult SpareSalesByItemSet(FormCollection fCollection)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<Inv_ItemMaster> lstItemMaster = new List<Inv_ItemMaster>();
            lstItemMaster = salesDal.ReadItemMaster();

            List<Sal_ListedUnitPriceForSparePartsSales> lstListedUnitPriceForSparePartsSales = new List<Sal_ListedUnitPriceForSparePartsSales>();
            lstListedUnitPriceForSparePartsSales = salesDal.ReadListedUnitPriceForSparePartsSales();

            string itemModel = string.Empty, itemCapacity = string.Empty, itemCode = string.Empty, itemsType = string.Empty, itemCategory = string.Empty;
            string validItemTransTypeForMRR = string.Empty;
            string[] sparePartSetItem = fCollection["hfSpareSetItems"].Split('#');
            int spareItemSetLength = sparePartSetItem.Length, itemSetCounter = 0;
            decimal salesCost = 0;

            Inv_ChallanMaster objChallanMaster; Inv_ChallanDetails objChallanDetails;

            List<Inv_ChallanDetails> lstChallanDetails = new List<Inv_ChallanDetails>();
            List<Inv_ChallanDetailsWithSerialNo> lstChallanDetailsWithSerialNo = new List<Inv_ChallanDetailsWithSerialNo>();

            Sal_SparePartsSalesMaster objSparePartsSalesMaster = new Sal_SparePartsSalesMaster();
            Sal_SparePartsSalesItems objSparePartsSalesItems;

            List<Sal_SparePartsSalesItems> lstSparePartsSalesItems = new List<Sal_SparePartsSalesItems>();
            List<Sal_SparePartsSalesItemsWithSerialNo> lstSparePartsSalesItemsWithSerialNo = new List<Sal_SparePartsSalesItemsWithSerialNo>();

            Inv_ItemStockByLocation objItemStockByLocation;

            validItemTransTypeForMRR = Helper.MrrType(fCollection["ddlIssueType"].Trim(), objLoginHelper.Location);
            string challanNumberMax = inventoryDal.ChallanSequenceNumberMax(objLoginHelper.LocationCode, objLoginHelper.TransactionOpenDate.ToString("yyMMdd")), storeLocation = string.Empty;
            string challanSequenceNumber = Helper.ChallanCequenceNumberGeneration(challanNumberMax, objLoginHelper);

            try
            {
                for (itemSetCounter = 0; itemSetCounter < spareItemSetLength; itemSetCounter++)
                {
                    string[] spareSetItem = sparePartSetItem[itemSetCounter].Split(',');

                    itemCategory = spareSetItem[0].Trim();   // Item Category
                    itemCapacity = spareSetItem[1].Trim();   // Item Capacity
                    itemModel = spareSetItem[2].Trim();      // Item Model
                    storeLocation = spareSetItem[4].Trim();  // Store Location

                    if (itemModel == "0")
                    {
                        if (itemCapacity == "0")
                        {
                            var vIt = (from itc in lstItemMaster where itc.ItemCategory == itemCategory && itc.ItemCapacity == null && itc.ItemModel == null select itc.ItemCode).FirstOrDefault();
                            itemCode = vIt.ToString();
                        }
                        else
                        {
                            var vIt = (from itc in lstItemMaster where itc.ItemCategory == itemCategory && itc.ItemCapacity == itemCapacity && itc.ItemModel == null select itc.ItemCode).FirstOrDefault();
                            itemCode = vIt.ToString();
                        }
                    }
                    else
                    {
                        if (itemCapacity == "0")
                        {
                            var vIt = (from itc in lstItemMaster where itc.ItemCategory == itemCategory && itc.ItemCapacity == null && itc.ItemModel == itemModel select itc.ItemCode).FirstOrDefault();
                            itemCode = vIt.ToString();
                        }
                        else
                        {
                            var vIt = (from itc in lstItemMaster where itc.ItemCategory == itemCategory && itc.ItemCapacity == itemCapacity && itc.ItemModel == itemModel select itc.ItemCode).FirstOrDefault();
                            itemCode = vIt.ToString();
                        }
                    }

                    var scost = (from itmcost in lstListedUnitPriceForSparePartsSales where itmcost.ItemCode == itemCode && itmcost.StoreLocation == Convert.ToByte(storeLocation) select itmcost.UnitPrice).FirstOrDefault();
                    salesCost += Convert.ToDecimal(scost) * Convert.ToInt32(spareSetItem[3].Trim());

                    objItemStockByLocation = new Inv_ItemStockByLocation();
                    objItemStockByLocation = inventoryDal.ReadItemStockByLocation(Convert.ToByte(storeLocation), objLoginHelper.LocationCode, itemCode);

                    if (objItemStockByLocation == null)
                    {
                        break;
                    }
                    else if (objItemStockByLocation.AvailableQuantity == 0)
                    {
                        break;
                    }

                    //Challan Details
                    objChallanDetails = new Inv_ChallanDetails();
                    objChallanDetails.LocationCode = objLoginHelper.LocationCode;
                    objChallanDetails.ChallanSeqNo = challanSequenceNumber;
                    objChallanDetails.CompSeqNo = Convert.ToByte(itemSetCounter + 1);                      // Component Sequence Number
                    objChallanDetails.ItemCode = itemCode.Trim();
                    objChallanDetails.DeliveryQuantity = Convert.ToDouble(spareSetItem[3].Trim());     // Delivery Quantity
                    objChallanDetails.UnitCost = 0;
                    objChallanDetails.FromStoreLocation = Convert.ToByte(storeLocation);
                    lstChallanDetails.Add(objChallanDetails);

                    //Spare Parts Item
                    objSparePartsSalesItems = new Sal_SparePartsSalesItems();
                    objSparePartsSalesItems.LocationCode = objLoginHelper.LocationCode;
                    objSparePartsSalesItems.SPSSeqNo = fCollection["txtSequenceNumber"].Trim();
                    objSparePartsSalesItems.CompSeqNo = Convert.ToByte(itemSetCounter + 1);              // Component Sequence Number
                    objSparePartsSalesItems.ItemCode = itemCode.Trim();
                    objSparePartsSalesItems.ItemQuantity = Convert.ToDouble(spareSetItem[3].Trim());       // Delivery Quantity
                    objSparePartsSalesItems.UnitCost = 0;
                    objSparePartsSalesItems.UnitPrice = (Convert.ToDecimal(scost) * Convert.ToDecimal(spareSetItem[3].Trim()));        // Item Wise Total Unit Price
                    objSparePartsSalesItems.FromStoreLocation = Convert.ToByte(storeLocation);
                    objSparePartsSalesItems.ItemCategory = itemCategory;

                    if (itemCapacity != "0")
                        objSparePartsSalesItems.ItemCapacity = itemCapacity;
                    else
                        objSparePartsSalesItems.ItemCapacity = null;

                    if (itemModel != "0")
                        objSparePartsSalesItems.ItemModel = itemModel;
                    else
                        objSparePartsSalesItems.ItemModel = null;

                    objSparePartsSalesItems.UnitOfMeasure = string.Empty;

                    lstSparePartsSalesItems.Add(objSparePartsSalesItems);
                }

                if (itemSetCounter < spareItemSetLength)
                {
                    var itemName = (from itc in lstItemMaster where itc.ItemCode == itemCode && itc.ItemCategory == itemCategory select itc.ItemName).FirstOrDefault();
                    return new JsonResult { Data = ExceptionHelper.ExceptionCustomErrorMessage("Stock Is Not Available For " + itemName + "(" + itemCode + ")") };
                }

                decimal unitCostRatio = Convert.ToDecimal(fCollection["txtTotalPrice"]) / salesCost;
                foreach (Sal_SparePartsSalesItems spsitm in lstSparePartsSalesItems)
                {
                    spsitm.UnitPrice = Math.Round((spsitm.UnitPrice * unitCostRatio) / Convert.ToDecimal(spsitm.ItemQuantity), 2);
                }

                //Challan Master
                objChallanMaster = new Inv_ChallanMaster();
                objChallanMaster.LocationCode = objLoginHelper.LocationCode;
                objChallanMaster.ChallanSeqNo = challanSequenceNumber;
                objChallanMaster.ChallanDate = Convert.ToDateTime(Helper.DateFormatMMDDYYYY(fCollection["dtpChallanDate"].Trim()));
                objChallanMaster.FromStoreLocation = Helper.NewItem; // Convert.ToByte(fCollection["ddlStoreLocation"].Trim());        // Store Location Code 
                objChallanMaster.RefChallanNo = null;
                objChallanMaster.ValidItemTransTypeForMRR = !string.IsNullOrEmpty(validItemTransTypeForMRR) ? validItemTransTypeForMRR : null;
                objChallanMaster.ChallanToLocationCode = null;
                objChallanMaster.IsItForMultipleLocations = false;
                objChallanMaster.RefCustomerCode = null;
                objChallanMaster.ItemType = fCollection["ddlItemType"];
                objChallanMaster.ItemTransTypeID = fCollection["ddlIssueType"];                         // Issue Type
                objChallanMaster.CreatedBy = objLoginHelper.LogInID;
                objChallanMaster.CreatedOn = DateTime.Now;
                objChallanMaster.Status = Helper.Active;

                //Spare Master
                objSparePartsSalesMaster.LocationCode = objLoginHelper.LocationCode;
                objSparePartsSalesMaster.SPSSeqNo = fCollection["txtSequenceNumber"].Trim();
                objSparePartsSalesMaster.SPSDate = Convert.ToDateTime(Helper.DateFormatMMDDYYYY(fCollection["dtpChallanDate"].Trim()));
                objSparePartsSalesMaster.ProjectCode = null;
                objSparePartsSalesMaster.ProgramCode = null;
                objSparePartsSalesMaster.SalesAmountAfterDiscount = 0;

                //objSparePartsSalesMaster.RefMemoNo = fCollection["txtMemoNumber"].Trim();

                if (!erpDal.IsCashMemoManagementEnabled(Helper.CompanyName))
                {
                    objSparePartsSalesMaster.RefMemoNo = fCollection["txtMemoNumber"].Trim();
                }
                else
                {
                    objSparePartsSalesMaster.CashMemoNo = fCollection["txtMemoNumber"].Trim();
                    objSparePartsSalesMaster.CashMemoUsesID = Helper.CashMemuUsesIdFirst;
                }

                objSparePartsSalesMaster.RefAETransNo = null;
                objSparePartsSalesMaster.FromStoreLocation = Helper.NewItem; //Convert.ToByte(fCollection["ddlStoreLocation"].Trim());        // Store Location Code
                objSparePartsSalesMaster.ChallanLocationCode = objLoginHelper.LocationCode;
                objSparePartsSalesMaster.ChallanSeqNo = null;
                objSparePartsSalesMaster.ItemTransTypeID = fCollection["ddlIssueType"];                       // Issue Type
                objSparePartsSalesMaster.SalespersonCode = fCollection["ddlEmployee"];
                objSparePartsSalesMaster.SalesAmountAfterDiscount = Convert.ToDecimal(fCollection["txtTotalPrice"]);
                objSparePartsSalesMaster.CreatedBy = objLoginHelper.LogInID;
                objSparePartsSalesMaster.CreatedOn = DateTime.Now;
                objSparePartsSalesMaster.Status = Helper.Active;

                objSparePartsSalesMaster = salesDal.SaveSparePartsChallan(objChallanMaster, lstChallanDetails, lstChallanDetailsWithSerialNo, objSparePartsSalesMaster, lstSparePartsSalesItems, lstSparePartsSalesItemsWithSerialNo);

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Spare Parts Sets Sales: " + Helper.SuccessMessage, Helper.ChallanCequenceNumberGeneration(objSparePartsSalesMaster.SPSSeqNo, objLoginHelper)) };

            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public JsonResult IssueTypeList(string itemTypeId)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<Inv_ItemTransactionTypes> lstItemTransactionTypes = new List<Inv_ItemTransactionTypes>();
            lstItemTransactionTypes = inventoryDal.ReadItemTransactionIssuedTypes(itemTypeId, Helper.UserLocationType(objLoginHelper.Location), objLoginHelper.UerRoleOrGroupID).Where(s => s.ItemTransTypeID == "ISSFSPSALE" || s.ItemTransTypeID == "ISSFSPSWDS").ToList();

            ArrayList arr = new ArrayList();

            foreach (Inv_ItemTransactionTypes itrnstyp in lstItemTransactionTypes)
            {
                arr.Add(new { Value = itrnstyp.ItemTransTypeID, Display = itrnstyp.ItemTransTypeDesc });
            }

            return new JsonResult { Data = arr };
        }


        public JsonResult IssueTypeListForItemSet(string itemTypeId)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<Inv_ItemTransactionTypes> lstItemTransactionTypes = new List<Inv_ItemTransactionTypes>();
            lstItemTransactionTypes = inventoryDal.ReadItemTransactionIssuedTypes(itemTypeId, Helper.UserLocationType(objLoginHelper.Location), objLoginHelper.UerRoleOrGroupID).Where(s => s.ItemTransTypeID == "ISSFSPSALE").ToList();

            ArrayList arr = new ArrayList();

            foreach (Inv_ItemTransactionTypes itrnstyp in lstItemTransactionTypes)
            {
                arr.Add(new { Value = itrnstyp.ItemTransTypeID, Display = itrnstyp.ItemTransTypeDesc });
            }

            return new JsonResult { Data = arr };
        }

        



        public JsonResult AvailAbleStoreItemQuantityAndUnitPrice(string itemCode, string storeLocation)
        {
            ArrayList arr = new ArrayList();

            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            string quantity = string.Empty, unitPrice = string.Empty;

            Inv_ItemStockByLocation objStockByLocation = new Inv_ItemStockByLocation();
            objStockByLocation = inventoryDal.ReadItemStockByLocation(Convert.ToByte(storeLocation), objLoginHelper.LocationCode, itemCode);

            Inv_ItemMaster objItemMaster = new Inv_ItemMaster();
            objItemMaster = inventoryDal.ReadItemMaster(itemCode);

            Sal_ListedUnitPriceForSparePartsSales objListedUnitPriceForSparePartsSales = new Sal_ListedUnitPriceForSparePartsSales();
            objListedUnitPriceForSparePartsSales = salesDal.ReadListedUnitPriceForSparePartsSales(Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForSales), itemCode, Convert.ToByte(storeLocation));

            string itemName = objItemMaster.ItemName;

            if (objItemMaster.ItemName.Contains('-'))
            {
                itemName = objItemMaster.ItemName.Substring(0, (objItemMaster.ItemName.IndexOf('-') - 1));
            }

            string itemCapacityName = objItemMaster.Sal_PackageOrItemCapacity != null ? objItemMaster.Sal_PackageOrItemCapacity.Description : "";
            string itemModelName = objItemMaster.Inv_ItemModel != null ? objItemMaster.Inv_ItemModel.Description : "";

            if (objStockByLocation != null)
                quantity = objStockByLocation.AvailableQuantity.ToString("0");
            else
                quantity = "0";

            if (objListedUnitPriceForSparePartsSales != null)
                unitPrice = objListedUnitPriceForSparePartsSales.UnitPrice.ToString("0");
            else
                unitPrice = "0";

            arr.Add(new { ItemName = itemName, ItemCapacityName = itemCapacityName, ItemModelName = itemModelName, AvailableQuantity = quantity, UnitPrice = unitPrice });

            return new JsonResult { Data = arr };
        }

        [GridAction]
        public ActionResult __SparePartsSalesSets(string itemSetCode)
        {
            return View(new GridModel<SparePartSetDetils>
            {
                Data = salesDal.ReadSparePartSetDetils(itemSetCode)
            });
        }

        public JsonResult GetItemSetPrice(string itemSetCode)
        {
            return new JsonResult { Data = salesDal.ReadItemSetMaster(itemSetCode).PerUnitSalesPrice };
        }

        public ActionResult OtherSalesReSalesAgreement()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "OtherSalesReSalesAgreement", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            if (objLoginHelper.TransactionOpenDate == Helper.InvalidDate())
            {
                Session["messageInformation"] = "Day Is Not Open For Sales";
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
            ViewBag.OpenDayForTransaction = "Day Open: " + objLoginHelper.TransactionOpenDate.ToString("MMMM dd, yyyy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.Program = salesDal.ReadProgram();   //.Where(p => p.Prog_Code != "SHS001");
            ViewBag.Project = salesDal.ReadProjectByProgramCode("BONB01").Where(p => p.ProjectCode != "100200");
            ViewBag.PackageCapacity = salesDal.ReadCapacityByProjectCode("101400", Helper.IsCapacityOnlyForPackagesAndItems);
            ViewBag.PackageLight = salesDal.ReadLightByCapacityId("0004WP");
            ViewBag.Package = salesDal.ReadPackages("0004WP", "01LIGHT", Helper.NewSalesAgreement);
            //ViewBag.ModeOfPayment = salesDal.ReadModeOfPayment("BONB01", "BON001", "HOUHLD"); real before 101614 modified for saleresalesagreement page

            ViewBag.ModeOfPayment = salesDal.ReadModeOfPayment("","BONB01", "BON001", "HOUHLD","");

            ViewBag.CustomerType = salesDal.ReadCustomerTypes();

            //start change for ddl
            ViewBag.Employee = hrmsDal.ReadPersonLocationWiseEmployee(objLoginHelper.LogInForUnitCode);
            //end  change for ddl
            
            //ViewBag.Employee = hrmsDal.ReadLocationWiseEmployee(objLoginHelper.LogInForUnitCode);
            ViewBag.Upazilla = salesDal.ReadUpazillaByUnitCode(objLoginHelper.LogInForUnitCode);
           
            ViewBag.PostOffice = salesDal.ReadPostOfficeInfo(objLoginHelper.LogInForUnitCode);

            ViewBag.CollectionDay = new SalesDay().SalesCollectionDay();

            ViewBag.GurdianRelation = salesDal.ReadCustomerRelations();
            ViewBag.CustomerOccupation = salesDal.ReadCustomerOccupations();
            ViewBag.CustomerIncomeRange = salesDal.ReadCustomerIncomeRange();
            ViewBag.FuelUsedBeforeSHS = salesDal.ReadCustomerFuelUsed();

            ViewBag.CustomerId = objLoginHelper.CustomerPrefix;

            ViewBag.OpenDay = objLoginHelper.TransactionOpenDate.Date.ToString("dd-MMM-yyyy");
            ViewBag.OpenBackDay = objLoginHelper.TransactionBackDate.Date.ToString("dd-MMM-yyyy");

            ViewBag.DayOpen = objLoginHelper.MonthOpenForSales.Date.ToString("dd-MMM-yyyy");
            ViewBag.CurrentCustomerSerial = (salesDal.LastUsedCustomerSerial(objLoginHelper.LogInForUnitCode, "BONB01") + 1).ToString();
            ViewBag.IsInventoryImplemented = objLoginHelper.IsInventoryImplemented;

            return View();
        }

        [HttpPost]
        public ActionResult OtherSalesReSalesAgreement(FormCollection fCollection)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            SalesDataProcess salesdataProcess = new SalesDataProcess();
            Sal_SalesAgreementPrePost objAgreement = new Sal_SalesAgreementPrePost();
            List<PackageDetails> lstPackageDetails = new List<PackageDetails>();

            try
            {
                lstPackageDetails = (List<PackageDetails>)Session["PackageDetails"];

                if (fCollection["ddlProgram"].ToString() == "SHS001" || fCollection["ddlProgram"].ToString() == "BONB01")
                {
                    salesdataProcess.AssignSalesItemAndSave(fCollection, lstPackageDetails, objLoginHelper);
                    erpDal.CreateUserLog(Helper.ClientIPAddress(), string.Empty, objLoginHelper.LocationCode, objLoginHelper.LogInID, "Other Sales Agreement, Customer Code: " + fCollection["hfCustomerCode"].Trim());

                    if (objAgreement != null)
                    {
                        return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Agreement" + Helper.SuccessMessage) };
                    }
                }
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }

            return new JsonResult { Data = "" };
        }


        //--------------------------Sales Agreement For SpecialPackageSales ----Sal_Validation_ForSpecialPackageSales-----------------------------

        public ActionResult SalesAgreementForSpecialPackageSales()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "SalesAgreementForSpecialPackageSales", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            if (objLoginHelper.TransactionOpenDate == Helper.InvalidDate())
            {
                Session["messageInformation"] = "Day Is Not Open For Sales";
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
            ViewBag.OpenDayForTransaction = "Day Open: " + objLoginHelper.TransactionOpenDate.ToString("MMMM dd, yyyy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.Program = salesDal.ReadProgram();   //.Where(p => p.Prog_Code != "SHS001");
            ViewBag.Project = salesDal.ReadProjectByProgramCode("SHS001");    //.Where(p => p.ProjectCode != "100100");
            ViewBag.PackageCapacity = salesDal.ReadCapacityByProjectCode("100200", Helper.IsCapacityOnlyForPackagesAndItems).Where(p => p.CapacityID != "0010WP");
            ViewBag.PackageLight = salesDal.ReadLightByCapacityId("0020WP");
            ViewBag.Package = salesDal.ReadPackages("0020WP", "01LIGHT", Helper.ResaleWithSpecialPackage);
            ViewBag.ModeOfPayment = salesDal.ReadModeOfPaymentForSpecialPackageSales("'R','B'");

            ViewBag.CustomerType = salesDal.ReadCustomerTypes();
            //start change for ddl
            ViewBag.Employee = hrmsDal.ReadPersonLocationWiseEmployee(objLoginHelper.LogInForUnitCode);
            //end  change for ddl
            //ViewBag.Employee = hrmsDal.ReadLocationWiseEmployee(objLoginHelper.LogInForUnitCode);
            ViewBag.Upazilla = salesDal.ReadUpazillaByUnitCode(objLoginHelper.LogInForUnitCode);
            ViewBag.PostOffice = salesDal.ReadPostOfficeInfo(objLoginHelper.LogInForUnitCode);

            ViewBag.CollectionDay = new SalesDay().SalesCollectionDay();

            ViewBag.GurdianRelation = salesDal.ReadCustomerRelations();
            ViewBag.CustomerOccupation = salesDal.ReadCustomerOccupations();
            ViewBag.CustomerIncomeRange = salesDal.ReadCustomerIncomeRange();
            ViewBag.FuelUsedBeforeSHS = salesDal.ReadCustomerFuelUsed();

            ViewBag.CustomerId = objLoginHelper.CustomerPrefix;

            ViewBag.OpenDay = objLoginHelper.TransactionOpenDate.Date.ToString("dd-MMM-yyyy");
            ViewBag.OpenBackDay = objLoginHelper.TransactionBackDate.Date.ToString("dd-MMM-yyyy");

            ViewBag.DayOpen = objLoginHelper.MonthOpenForSales.Date.ToString("dd-MMM-yyyy");
            ViewBag.CurrentCustomerSerial = (salesDal.LastUsedCustomerSerial(objLoginHelper.LogInForUnitCode, "SHS001") + 1).ToString();
            ViewBag.IsInventoryImplemented = objLoginHelper.IsInventoryImplemented;

            return View();
        }

        public JsonResult SalesAgreementDetailsForSpecialPackage(string packageCode, string programCode, string projectCode, string paymentMode, string customerType, string dpAmountClient, string isResaleOrNewSales, string panelSerial, string resalePackagePrice, string capacityId, string lightId)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            List<PackageDetails> lstPackageDetails = new List<PackageDetails>();

            DiscountPolicy objDiscountPolicy = new DiscountPolicy();
            ServiceChargeInformation objServiceChargePolicy = new ServiceChargeInformation();
            DownPaymentPolicy objPackageVDownpaymnet = new DownPaymentPolicy();
            DepretiatedPackagePriceBySRPanelSerial objDepreciatedPackagePrice = new DepretiatedPackagePriceBySRPanelSerial();

            decimal packagePrice = 0, discountPercentage = 0, actualPayableAmount = 0, dPAmount = 0, restAmount = 0, totalServiceCharge = 0, installmentSizeAmount = 0;
            decimal monthlyPrincipalPayable = 0, discountAmount = 0, dPParcentage = 0, lowerLimitPackagePrice = 0, originalPackagePrice = 0;
            decimal serviceCharge = 0, monthlyServiceChargePayable = 0, totalPayablewithServiceCharge = 0, totalYears = 0, installmentInMonth = 0;
           
            string calculationMesage = string.Empty;
            ArrayList alSalAgr = new ArrayList();
                       
            try
            {
                isResaleOrNewSales = "R";

                PackagePricingDetailsForSalesAgreement objAgreementDetails = new PackagePricingDetailsForSalesAgreement();
                objAgreementDetails = salesDal.ReadPackagePricingDetailsForSalesAgreement(objLoginHelper.LocationCode, programCode, isResaleOrNewSales, customerType, capacityId, lightId, packageCode, paymentMode, dpAmountClient);

                if (objAgreementDetails.NumberOfInstallment != 0)
                    totalYears = (decimal)(objAgreementDetails.NumberOfInstallment / 12);

                alSalAgr.Add(new
                {
                    packagePrice = objAgreementDetails.PackagePrice,
                    discountPercentage = objAgreementDetails.DiscountPercentage,
                    discountAmount = objAgreementDetails.DiscountAmount,
                    actualPayableAmount = objAgreementDetails.AmountAfterDiscount,
                    dPParcentage = objAgreementDetails.DownPaymentPercentage,
                    dPAmount = objAgreementDetails.DownPaymentAmount,
                    restAmount = objAgreementDetails.TotalPrincipalReceivable,
                    serviceCharge = objAgreementDetails.ServiceChargePercentage,
                    totalServiceCharge = objAgreementDetails.TotalServiceChargeReceivable,
                    installmentInMonth = objAgreementDetails.NumberOfInstallment,
                    totalYears = totalYears,
                    installmentSizeAmount = objAgreementDetails.InstallmentSize,
                    totalPayablewithServiceCharge = objAgreementDetails.TotalPrincipalPlusServiceChargeReceivable,
                    monthlyPrincipalPayable = objAgreementDetails.InstallmentSizePrincipal,
                    monthlyServiceChargePayable = objAgreementDetails.InstallmentSizeServiceCharge,
                    calculationMesage = calculationMesage,
                    originalPackagePrice = objAgreementDetails.OriginalPackagePrice,
                    lowerLimitPackagePrice = lowerLimitPackagePrice,
                    isPackagePriceFixed = objAgreementDetails.IsPackagePriceFixed == true ? 1 : 0,
                    panelError = "",
                    panelStoreLocation = objAgreementDetails.PanelStoreLocation,
                    batteryStoreLocation = objAgreementDetails.BatteryStoreLocation
                });
            }
            catch (Exception ex)
            {
                calculationMesage = ExceptionHelper.ExceptionMessageOnly(ex);

                alSalAgr.Add(new
                {
                    packagePrice = packagePrice,
                    discountPercentage = discountPercentage,
                    discountAmount = discountAmount,
                    actualPayableAmount = actualPayableAmount,
                    dPParcentage = dPParcentage,
                    dPAmount = dPAmount,
                    restAmount = restAmount,
                    serviceCharge = serviceCharge,
                    totalServiceCharge = totalServiceCharge,
                    installmentInMonth = installmentInMonth,
                    totalYears = totalYears,
                    installmentSizeAmount = installmentSizeAmount,
                    totalPayablewithServiceCharge = totalPayablewithServiceCharge,
                    monthlyPrincipalPayable = monthlyPrincipalPayable,
                    monthlyServiceChargePayable = monthlyServiceChargePayable,
                    calculationMesage = calculationMesage,
                    originalPackagePrice = originalPackagePrice,
                    lowerLimitPackagePrice = lowerLimitPackagePrice,
                    isPackagePriceFixed = 1,
                    panelError = calculationMesage,
                    panelStoreLocation = 0,
                    batteryStoreLocation = 0
                });
            }

            return new JsonResult { Data = alSalAgr };
        }

        public JsonResult PackgeListForSpecialPackageSales(string capacityId, string lightId, string salesType, string programCode)
        {
            ArrayList arl = new ArrayList();
            arl = salesDal.ReadSpecialPackageListForSales(capacityId, lightId, programCode, "R");

            return new JsonResult { Data = arl };
        }

        public JsonResult ModeOfPaymentListForSpecialPackageSales(string programCode, string packageCode, string customerType, string salesReSalesOrBoth)
        {
            salesReSalesOrBoth = "'R','B'";

            ArrayList arr = new ArrayList();
            arr = salesDal.ReadModeOfPaymentForSpecialPackageSales(salesReSalesOrBoth);

            return new JsonResult { Data = arr };
        }

        public JsonResult CapacityListForSpecialPackageSales(string projectCode)
        {
            ArrayList arl = new ArrayList();

            List<Sal_PackageOrItemCapacity> lstCapacity = new List<Sal_PackageOrItemCapacity>();
            lstCapacity = salesDal.ReadCapacityByProjectCode(projectCode, Helper.IsCapacityOnlyForPackagesAndItems);

            foreach (Sal_PackageOrItemCapacity PIC in lstCapacity.Where(p => p.CapacityID != "0010WP"))
            {
                arl.Add(new { Value = PIC.CapacityID, Display = PIC.Description });
            }

            return new JsonResult { Data = arl };
        }

        public JsonResult ProjectListForSpecialPackageSales(string programCode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<Common_ProjectInfo> lstProject = new List<Common_ProjectInfo>();
            lstProject = salesDal.ReadProjectByProgramCode(programCode);   //.Where(p => p.ProjectCode != "100200").ToList();

            int lastUsedCustomerSerial = (salesDal.LastUsedCustomerSerial(objLoginHelper.LogInForUnitCode, programCode) + 1);

            ArrayList arl = new ArrayList();

            foreach (Common_ProjectInfo prj in lstProject)
            {
                arl.Add(new { Value = prj.ProjectCode, Display = prj.ProjectName });
            }
            arl.Add(new { Value = lastUsedCustomerSerial, Display = lastUsedCustomerSerial });

            return new JsonResult { Data = arl };
        }

    }  
}
