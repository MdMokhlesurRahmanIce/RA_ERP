using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RASolarERP.Model;
using RASolarHelper;
using RASolarERP.DomainModel.SalesModel;
using RASolarHRMS.Model;
using RASolarERP.DomainModel.HRMSModel;
using RASolarERP.DomainModel.AMSModel;
using RASolarERP.DomainModel.InventoryModel;
using System.Collections;

namespace RASolarERP.Web.Areas.Sales.Models
{
    public class SalesData
    {
        private RASolarERPService erpService = new RASolarERPService();

        public List<SalesDataEntryStatus> SalesEntryStatus(string reportType, string locationType, string yearMonth)
        {
            return erpService.SalesEntryStatus(reportType, locationType, yearMonth);
        }
        public List<REPSummaryForTheDayClosingForSales> ReadREPSummaryForTheDayClosingForSalesData(string reportType, string locationCode)
        {
            return erpService.ReadREPSummaryForTheDayClosingForSalesData(reportType, locationCode);
        }

        public List<SummaryForTheDayClosingForCollectionInfo> ReadSummaryForTheDayClosingForCollectionInfo(string reportType, string locationCode)
        {
            return erpService.ReadSummaryForTheDayClosingForCollectionInfo(reportType, locationCode).ToList();
        }


        public List<SummaryForTheDayClosingForInventoryInfo> ReadSummaryForTheDayClosingForInventoryInfo(string reportType, string locationCode, byte storeLocation)
        {
            return erpService.ReadSummaryForTheDayClosingForInventoryInfo(reportType, locationCode, storeLocation).ToList();
        }
        //public List<SummaryForTheDayClosingForAccountingInfo> ReadSummaryForTheDayClosingForCollectionInfo(string reportType, string locationCode, byte storeLocation)
        //{
        //    return erpService.ReadSummaryForTheDayClosingForCollectionInfo(reportType, locationCode, storeLocation).ToList();
        //}
        public List<SummaryForTheDayClosingForAccountingInfo> ReadSummaryForTheDayClosingForAccountingInfo(string reportType, string locationCode)
        {
            return erpService.ReadSummaryForTheDayClosingForAccountingInfo(reportType, locationCode).ToList();
        }
        //public tbl_UnitWiseEntryStatus UnitWiseEntryStatus(string unitCode, string yearMonth)
        //{
        //    return erpService.UnitWiseEntryStatus(unitCode, yearMonth);
        //}

        //public string AuditAdjustmentOpenMonth(string unitCode)
        //{
        //    return erpService.AuditAdjustmentOpenMonth(unitCode);
        //}

        //public tbl_UnitWiseEntryStatus UpdateUnitWiseEntryStatus(tbl_UnitWiseEntryStatus objUnitWiseEntryStatus)
        //{
        //    return erpService.Update(objUnitWiseEntryStatus);
        //}

        public List<Common_Sys_StatusInfo> CustomerStatus()
        {
            return erpService.CustomerStatus();
        }

        //public List<CustomerStatusListV2> ReadCustomerStatusListV2(string unitCode, byte customerStatus, byte paymentStatus, DateTime currentCollectionDate)
        //{
        //    return erpService.ReadCustomerStatusListV2(unitCode, customerStatus, paymentStatus, currentCollectionDate);
        //}

        //public List<GetCustomerListWithRecoveryStatus> ReadCustomerListWithRecoveryStatus(string unitCode, byte customerStatus, DateTime currentCollectionDate)
        //{
        //    return erpService.ReadCustomerListWithRecoveryStatus(unitCode, customerStatus, currentCollectionDate);
        //}

        //public tbl_UnitWiseCustomerStatus ReadUnitWiseCustomerStatus(string unitCode, string customerId)
        //{
        //    return erpService.ReadUnitWiseCustomerStatus(unitCode, customerId);
        //}

        //public tbl_UnitWiseCustomerStatus ReadUnitWiseCustomerStatus(string customerId)
        //{
        //    return erpService.ReadUnitWiseCustomerStatus(customerId);
        //}

        //public List<AdjustmentReasonCodes> SalesAuditResonCodes()
        //{
        //    return erpService.AuditAdjustMentReasons(Helper.ForInventory, 0, Helper.ForSales);
        //}

        //public tbl_UnitWiseCustomerStatus UpdateUnitWiseCustomerStatus(tbl_UnitWiseCustomerStatus objUnitWiseCustomerStatus)
        //{
        //    return erpService.Update(objUnitWiseCustomerStatus);
        //}

        //public Aud_AuditingMaster AuditingMasterByLocationCode(string locationCode)
        //{
        //    return erpService.AuditingMasterByLocationCode(locationCode);
        //}

        //public Aud_AuditingMaster UpdateAuditingMaster(Aud_AuditingMaster objAuditMaster)
        //{
        //    return erpService.Update(objAuditMaster);
        //}

        //public Aud_AuditingMaster SaveAuditingMaster(Aud_AuditingMaster objAuditMaster)
        //{
        //    return erpService.Create(objAuditMaster);
        //}
        public Sal_LocationNEmployeeWiseActivityDaily UpdateLocationNEmployeeWiseActivityDaily(Sal_LocationNEmployeeWiseActivityDaily objLocationNEmployeeWiseActivityDailyUpdate)
        {
            return erpService.Update(objLocationNEmployeeWiseActivityDailyUpdate);
        }

        public Sal_LocationNEmployeeWiseActivityMonthly UpdateLocationNEmployeeWiseActivityMonthly(Sal_LocationNEmployeeWiseActivityMonthly objLocationNEmployeeWiseActivityMonthlyUpdate)
        {
            return erpService.Update(objLocationNEmployeeWiseActivityMonthlyUpdate);
        }

        public Sal_LocationNEmployeeWiseActivityMonthly CreateLocationNEmployeeWiseActivityMonthly(Sal_LocationNEmployeeWiseActivityMonthly objLocationNEmployeeWiseActivityMonthlyCreate)
        {
            return erpService.Create(objLocationNEmployeeWiseActivityMonthlyCreate);
        }

        public Sal_LocationNEmployeeWiseActivityDaily CreateLocationNEmployeeWiseActivityDaily(Sal_LocationNEmployeeWiseActivityDaily objLocationNEmployeeWiseActivityDailyCreate)
        {
            return erpService.Create(objLocationNEmployeeWiseActivityDailyCreate);
        }

        public GetLocationNEmployeeWiseDailyEntry ReadLocationNEmployeeWiseActivity(string locationCode, string employeeID, string yearMonth, DateTime transDate)
        {
            return erpService.ReadLocationNEmployeeWiseActivity(locationCode, employeeID, yearMonth, transDate);
        }

        public Sal_LocationNEmployeeWiseActivityMonthly ReadLocationNEmployeeWiseActivityMonthly(string locationCode, string employeeID, string yearMonth)
        {
            return erpService.ReadLocationNEmployeeWiseActivityMonthly(locationCode, employeeID, yearMonth);
        }

        public Sal_LocationNEmployeeWiseActivityDaily ReadLocationNEmployeeWiseActivityDaily(string locationCode, string employeeID, string yearMonth, DateTime transDate)
        {
            return erpService.ReadLocationNEmployeeWiseActivityDaily(locationCode, employeeID, yearMonth, transDate);
        }

        public Common_PeriodOpenClose ReadPeriodOpenClose(string locationCode)
        {
            return erpService.ReadPeriodOpenClose(locationCode);
        }
        
        public List<EmployeeVisit> ReadEmployeeDetailsVisit(string option, string empId, string locationCode, string ddlLocationPart1, string ddlLocationPart4)
        {
            return erpService.ReadEmployeeDetailsVisit(option, empId, locationCode, ddlLocationPart1,ddlLocationPart4);
        }

        public List<Common_ProjectInfo> ReadProjectByProgramCode(string programCode)
        {
            return erpService.ReadProject(programCode);
        }

        public List<Common_ProgramInfo> ReadProgram()
        {
            return erpService.ReadProgram();
        }

        //public List<tbl_Package> ReadPackage(string programCode, string projectCode)
        //{
        //    return erpService.ReadPackage(programCode, projectCode);
        //}

        //public tbl_Package ReadPackage(string packageCode, string programCode, string projectCode)
        //{
        //    return erpService.ReadPackage(packageCode, programCode, projectCode);
        //}

        public List<PackageDetails> ReadPackageDetails(string packageCode, string modeOfPaymentID, string customerType)
        {
            return erpService.ReadPackageDetails(packageCode, modeOfPaymentID, customerType);
        }

        //public List<Hrm_LocationWiseEmployee> ReadLocationWiseEmployee(string locationCode)
        //{
        //    return erpService.ReadLocationWiseEmployee(locationCode);
        //}

        public List<PackageDetails> ReadPackageDetailsExtra()
        {
            return erpService.ReadPackageDetailsExtra(); 
        }

        public List<GetLocationWiseEmployee> ReadGetLocationWiseEmployee(string locationCode)
        {
            return erpService.ReadGetLocationWiseEmployee(locationCode);
        }

        public Sal_LocationWiseSalesNCollectionTarget ReadLocationWiseSalesNCollectionTarget(string locationCode, string yearMonth)
        {
            return erpService.ReadLocationWiseSalesNCollectionTarget(locationCode, yearMonth);
        }

        public List<GetLocationWiseEmployeeTarget> ReadGetLocationWiseEmployeeTarget(string locationCode, string yearMonth)
        {
            return erpService.ReadGetLocationWiseEmployeeTarget(locationCode, yearMonth);
        }
        public Sal_LocationWiseSalesNCollectionTarget UpdateLocationWiseSalesNCollectionTarget(Sal_LocationWiseSalesNCollectionTarget objSalesNCollectionTarget)
        {
            return erpService.Update(objSalesNCollectionTarget);
        }

        public string ExecuteDayEndProcess(string Location, string CloseableDate)
        {
            return erpService.ExecuteDayEndProcess(Location, CloseableDate);
        }

        //UpdateLoadRequestEntry
        public string UpdateLoadRequestEntry(string locationCode, string employeeID, string dayOpenDay, string prmCorporatePhoneNo, string rsfServiceQunt, string paywellServiceQunt,string cashMemo) 
        {
            return erpService.UpdateLoadRequestEntry(locationCode, employeeID, dayOpenDay, prmCorporatePhoneNo, rsfServiceQunt, paywellServiceQunt, cashMemo); 
        }

        
        public string UpdateLoadRequestEntryFinalSave(string locationCode,string  dayOpenDayTocurrentdate, string txtRsfServiceTotal, string txtRsfPayWellTotal)
        {
            return erpService.UpdateLoadRequestEntryFinalSave(locationCode, dayOpenDayTocurrentdate, txtRsfServiceTotal, txtRsfPayWellTotal);  
        }
        //
        public string EntryEmployeeVisitPlan(string prmLocationCode, string prmEmployeeID, string prmMovementDate, string prmTTLocationCode, string prmNoFcustomerVisit, string prmPurposeOfVisit, string  prmDurationOfVisitInHours, string  prmUserName, string prmDBTransType)
        {
            return erpService.EntryEmployeeVisitPlan(prmLocationCode, prmEmployeeID, prmMovementDate, prmTTLocationCode, prmNoFcustomerVisit, prmPurposeOfVisit, prmDurationOfVisitInHours, prmUserName, prmDBTransType);
        }
        public void CustomerTransferV2(string customerID, string LocationCode, string CustomerTransferOrReceive, string TransferToUnit, string TransDate, string NoteForTransfer, string UserID, string DBTransType)
        {
            erpService.CustomerTransferV2(customerID, LocationCode, CustomerTransferOrReceive, TransferToUnit, TransDate, NoteForTransfer, UserID, DBTransType);
        }

        public Sal_Customer ReadCustomer(string customerCode)
        {
            return erpService.ReadCustomer(customerCode);
        }

        public Sal_Customer ReadCustomer(string unitCode, string customerCode)
        {
            return erpService.ReadCustomer(unitCode, customerCode);
        }

        public Sal_SalesAgreementPrePost CreateSalesAgreement(Sal_CustomerPrePost objCustomer, Sal_SalesAgreementPrePost objSalesAgreement, List<Sal_SalesItemsPrePost> lstSalesItem, List<Sal_SalesItemsWithSerialNoPrePost> lstItemSalesWithSerialNo)
        {
            return erpService.CreateSalesAgreement(objCustomer, objSalesAgreement, lstSalesItem, lstItemSalesWithSerialNo);
        }

        public Sal_SalesAgreement SaveSalesAgreement(Sal_Customer objCustomer, Sal_SalesAgreement objSalesAgreement, Sal_CustomerStatus objCustomerStatus, List<Sal_SalesItems> lstSalesItem, List<Sal_SalesItemsWithSerialNo> lstItemSalesWithSerialNo, int customerCurrentSerial, string serialTempTableRows)
        {
            return erpService.Create(objCustomer, objSalesAgreement, objCustomerStatus, lstSalesItem, lstItemSalesWithSerialNo, customerCurrentSerial, serialTempTableRows);
        }

        public List<Sal_LocationNEmployeeWiseActivityDaily> ReadLocationNEmployeeWiseActivityDaily(string locationCode, string yearMonth, DateTime transDate)
        {
            return erpService.ReadLocationNEmployeeWiseActivityDaily(locationCode, yearMonth, transDate.Date);
        }

        public List<Sal_LocationNEmployeeWiseActivityMonthly> ReadLocationNEmployeeWiseActivityMonthly(string locationCode, string yearMonth)
        {
            return erpService.ReadLocationNEmployeeWiseActivityMonthly(locationCode, yearMonth);
        }

        //public List<LocationAndEmployeeWiseWeeklySalesAndCollectionEntry> ReadLocationAndEmployeeWiseWeeklySalesAndCollectionEntry(string locationCode, string yearWeek)
        //{
        //    return erpService.ReadLocationAndEmployeeWiseWeeklySalesAndCollectionEntry(locationCode, yearWeek);
        //}

        //public Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement CreateLocationAndEmployeeWiseWeeklySalesAndCollectionEntry(List<Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement> objLocationNEmployeeWiseWeeklySalesNCollectionAchievement)
        //{
        //    return erpService.Create(objLocationNEmployeeWiseWeeklySalesNCollectionAchievement);
        //}

        //public Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement UpdateLocationAndEmployeeWiseWeeklySalesAndCollectionEntry(Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement objLocationNEmployeeWiseWeeklySalesNCollectionAchievement)
        //{
        //    return erpService.Update(objLocationNEmployeeWiseWeeklySalesNCollectionAchievement);
        //}

        //public List<Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement> ReadWeeklySalesNCollectionTargetNAchievement(string locationCode, string yearWeek, DateTime transDate)
        //{
        //    return erpService.ReadWeeklySalesNCollectionTargetNAchievement(locationCode, yearWeek, transDate);
        //}

        //public List<LocationAndEmployeeWiseWeeklySalesAndCollectionReport> ReadLocationAndEmployeeWiseWeeklySalesAndCollectionReport(string reportType, string locationCode, string yearWeek)
        //{
        //    return erpService.ReadLocationAndEmployeeWiseWeeklySalesAndCollectionReport(reportType, locationCode, yearWeek);
        //}

        //public Sal_LocationWiseWeeklySalesNCollectionSummary LocationWiseWeeklySalesNCollectionSummary(string locationCode, string yearWeek)
        //{
        //    return erpService.LocationWiseWeeklySalesNCollectionSummary(locationCode, yearWeek);
        //}

        //public Sal_LocationWiseWeeklySalesNCollectionSummary CreateLocationWiseWeeklySalesNCollectionSummary(Sal_LocationWiseWeeklySalesNCollectionSummary objSal_LocationWiseWeeklySalesNCollectionSummary)
        //{
        //    return erpService.Create(objSal_LocationWiseWeeklySalesNCollectionSummary);
        //}

        //public Sal_LocationWiseWeeklySalesNCollectionSummary UpdateLocationWiseWeeklySalesNCollectionSummary(Sal_LocationWiseWeeklySalesNCollectionSummary objSal_LocationWiseWeeklySalesNCollectionSummary)
        //{
        //    return erpService.Update(objSal_LocationWiseWeeklySalesNCollectionSummary);
        //}

        public List<Sal_CustomerWiseOverdueBalanceForTracker> ReadCustomerWiseOverdueBalanceForTracker(String unitCode)
        {
            List<Sal_CustomerWiseOverdueBalanceForTracker> lstCustomerWiseOverdueBalanceUpto31July2012 = new List<Sal_CustomerWiseOverdueBalanceForTracker>();
            lstCustomerWiseOverdueBalanceUpto31July2012 = erpService.ReadCustomerWiseOverdueBalanceForTracker(unitCode);

            return lstCustomerWiseOverdueBalanceUpto31July2012;
        }

        public List<CustomerWiseOverdueBalanceTracker> ReadCustomerWiseOverdueBalanceForTracker(String unitCode, string yearWeek)
        {
            List<CustomerWiseOverdueBalanceTracker> lstCustomerWiseIverDueCollection = new List<CustomerWiseOverdueBalanceTracker>();

            lstCustomerWiseIverDueCollection = erpService.ReadCustomerWiseOverdueBalanceForTracker(unitCode, yearWeek.Substring(4, 2));

            return lstCustomerWiseIverDueCollection;
        }

        public bool UpdateCustomerWiseOverdueCollection(string unitCode, string customerCode, decimal collectedOverdue, string weekNumber)
        {
            return erpService.Update(unitCode, customerCode, collectedOverdue, weekNumber);
        }

        public List<WeeklySalesNCollectionStatus> ReadWeeklySalesNCollectionStatus(string reportType, string locationCode, string yearWeek)
        {
            return erpService.ReadWeeklySalesNCollectionStatus(reportType, locationCode, yearWeek);
        }

       

        public Common_CurrentYearMonthNWeek ReadCurrentYearMonthNWeek()
        {
            return erpService.ReadCurrentYearMonthNWeek();
        }

        public List<Sal_Light> ReadLightByCapacityId(string capacityID)
        {
            return erpService.ReadLight(capacityID);
        }

        public List<Sal_PackageMaster> ReadPopUpPackageList(string packageCode)
        {
            return erpService.ReadPopUpPackageList(packageCode); 
        }


        public List<PackageLightCapacityPop> ReadPopUpPackageCapacity(string packageCode)
        {
            return erpService.ReadPopUpPackageCapacity(packageCode);   
        }

        public List<Sal_PackageOrItemCapacity> ReadCapacityByProjectCode(string projectCode, string isForItemOrPackage)
        {
            return erpService.ReadPackageOrItemCapacity(projectCode, isForItemOrPackage);
        }

        public List<Sal_PackageMaster> ReadPackages(string capacityId, string lightId, byte salesType)
        {
            return erpService.ReadPackages(capacityId, lightId, salesType);
        }

        public List<Sal_PackageMaster> ReadPackagesPackgeListFrmPopUpTddlPckg(string capacityId, string lightId, byte salesType)
        {
            return erpService.ReadPackagesPackgeListFrmPopUpTddlPckg(capacityId, lightId, salesType); 
        }

        public List<Sal_Validation_CapacityVsLight> ReadPackageCapacity()
        {
            return erpService.ReadPackageCapacity();
        }

        public List<PersonEmployeeDetailsInfo> ReadPersonLocationWiseEmployee(string locationCode)
        {
            return erpService.ReadPersonLocationWiseEmployee(locationCode);
        }
        //

        public List<LocationPart1District> ReadLocationPart1District(string empWisePart1, string empID, string locationCode)
        {
            return erpService.ReadLocationPart1District(empWisePart1,empID,locationCode);
        }

        public List<LocationPart2Upo> ReadLocationPart2District(string empWisePart1, string empID, string locationCode, string locationPart1)
        {
            return erpService.ReadLocationPart2District(empWisePart1, empID, locationCode,locationPart1);
        }

        public List<Sal_Validation_CapacityVsLight> ReadLightByPackageCapacity(string capacityID)
        {
            return erpService.ReadLightByPCapacityID(capacityID);
        }

        public List<Sal_PackageMaster> ReadPackagesForSHSDP(string capacityId, string lightId)
        {
            return erpService.ReadPackagesForSHSDP(capacityId, lightId);
        }

        //public Sal_Validation_CustomerTypeNModeOfPaymentWiseServiceChargePolicy ReadServiceChargePolicy(string modeOfPaymentId, string customerType)
        //{
        //    return erpService.ReadServiceChargePolicy(modeOfPaymentId, customerType);
        //}

        //public Sal_Validation_ModeOfPaymentWiseDiscountPolicy ReadDiscountPolicy(string modeOfPaymentId, string discountId)
        //{
        //    return erpService.ReadDiscountPolicy(modeOfPaymentId, discountId);
        //}

        //public Sal_Validation_ModeOfPaymentNPackageWiseDownpaymentPolicy ReadPackageVsDownpayment(string packageCode, string modeOfPaymentId)
        //{
        //    return erpService.ReadPackageVsDownpayment(modeOfPaymentId, packageCode);
        //}

        public ServiceChargeInformation ReadServiceChargePolicy(string programCode, string customerType, string modeOfPayment)
        {
            return erpService.ReadServiceChargePolicy(programCode, customerType, modeOfPayment);
        }

        public DownPaymentPolicy ReadDownPaymentPolicy(string modeOfPayment, string packageCode)
        {
            return erpService.ReadDownPaymentPolicy(modeOfPayment, packageCode);
        }

        public DiscountPolicy ReadDiscountPolicyByModeofPaymentNDiscountId(string modeOfPayment, string discountId)
        {
            return erpService.ReadDiscountPolicyByModeofPaymentNDiscountId(modeOfPayment, discountId);
        }

        public DiscountPolicy ReadDiscountPolicyByModeofPaymentNPackageId(string modeOfPayment, string packageCode)
        {
            return erpService.ReadDiscountPolicyByModeofPaymentNPackageId(modeOfPayment, packageCode);
        }

        public List<RASolarERP.Model.Common_UpazillaInfo> ReadUpazillaByUnitCode(string unitCode)
        {
            return erpService.ReadUpazilla(unitCode);
        }

        public List<Common_UnionInfo> ReadUnionInfo(string upazilaCode)
        {
            return erpService.ReadUnionInfo(upazilaCode);
        }

        public List<Common_PostOfficeInfo> ReadPostOfficeInfo(string upazilaCode)
        {
            return erpService.ReadPostOfficeInfo(upazilaCode); 
        }
        //ReadCustomerPostOfficeInfo



        public List<Common_PostOfficeInfo> PostOfficeInfoLoadForUnitCode(string unitCode)
        {
            return erpService.PostOfficeInfoLoadForUnitCode(unitCode);  
        }

        public List<Common_PostOfficeInfo> ReadCustomerPostOfficeInfo(string CustomerCode, string PostOffice)
        {
            return erpService.ReadCustomerPostOfficeInfo(CustomerCode,PostOffice); 
        }

        public List<RASolarERP.Model.Common_DistrictInfo> ReadDistrictInfo(string upazilaCode)
        {
            return erpService.ReadDistrictInfo(upazilaCode);
        }

        public List<Inv_ItemModel> ReadItemModelByCategoryId(string itemCatagory, string itemCapacity, string itemCategoryIdForNull)
        {
            return erpService.ReadItemModel(itemCatagory, itemCapacity, itemCategoryIdForNull);
        }

        public List<Inv_ItemModel> ReadItemModelByCategoryId(string itemCatagory)
        {
            return erpService.ReadItemModel(itemCatagory);
        }

        public List<ItemCapacity> ReadItemCapacityByCategoryId(string itemCategory)
        {
            return erpService.ReadItemCapacity(itemCategory);
        }
        
        public List<RASolarERP.DomainModel.SalesModel.SystemReturnOrFullPaidCustomers> ReadSystemReturnOrFullPaidCustomers(string CustomerOrViewType, string locationCode, string fromDate, string toDate)
        {
            return erpService.ReadSystemReturnOrFullPaidCustomers(CustomerOrViewType, locationCode, fromDate, toDate);
        }
       // ReadTrackerLocationWiseWeeklyODCollectionTargetVsAchievement
        public List<TrackerLocationWiseWeeklyODCollectionTargetVsAchievement> ReadTrackerLocationWiseWeeklyODCollectionTargetVsAchievement(string prmReportType, string prmYearMonth, string prmYearWeek)
        {
            return erpService.ReadTrackerLocationWiseWeeklyODCollectionTargetVsAchievement(prmReportType, prmYearMonth, prmYearWeek);
        }
        //LoadRequestEntryGet
        public List<LoadRequestEntryGet> ReadLoadRequestEntryGet(string locationCode, string getDate)
        {
            return erpService.ReadLoadRequestEntryGet(locationCode,getDate);
        }

        public List<CheckLoadRequestEntry> firstCheckLoadRequestEntry(string yearMonth, string locationCode, string getDate)
        {
            return erpService.firstCheckLoadRequestEntry(yearMonth, locationCode, getDate); 
        }
        public List<CheckLoadRequestEntryForLocationCode> LocationCheckLoadRequestEntry(string locationCode)
        {
            return erpService.LocationCheckLoadRequestEntry(locationCode); 
        }

        public List<LoadRequestEntryGet> InsertLoadRequest(string locationCode, string getDate)
        {
            return erpService.InsertLoadRequest(locationCode, getDate);
        }


        public List<UnitWiseCustomerRegisterReport> ReadUnitWiseCustomerRegisterReport(string unitCode)
        {
            return erpService.ReadUnitWiseCustomerRegisterReport(unitCode);
        }

        public List<CustomerCollectionDetails> ReadCollectionFromCustomers(string customerCode, string yearMonth)
        {
            return erpService.ReadCollectionFromCustomers(customerCode, yearMonth);
        }

        public Sal_CollectionFromCustomers ReadCollectionFromCustomers(string customerCode, string yearMonth, byte collectionSerial)
        {
            return erpService.ReadCollectionFromCustomers(customerCode, yearMonth, collectionSerial);
        }

        public Sal_CollectionFromCustomersPrePost ReadCollectionFromCustomersPrepost(string customerCode, string yearMonth, byte collectionSerial)
        {
            return erpService.ReadCollectionFromCustomersPrepost(customerCode, yearMonth, collectionSerial);
        }

        public Sal_CollectionFromCustomersPrePost UpdateCustomerCollection(Sal_CollectionFromCustomersPrePost objCollectionFromCustomers, Sal_CollectionFromCustomers objCustomerCol)
        {
            return erpService.Update(objCollectionFromCustomers, objCustomerCol);
        }

        public Sal_CollectionFromCustomersPrePost SaveCustomerCollection(Sal_CollectionFromCustomersPrePost objCollectionFromCustomers, Sal_Customer objSalCustomerInfo)
        {
            return erpService.Create(objCollectionFromCustomers, objSalCustomerInfo);
        }

        public int CustomerCollectionSerial(string customerCode, string yearMonth)
        {
            return erpService.CustomerCollectionSerial(customerCode, yearMonth);
        }

        public int CustomerCollectionSerial(string customerCode, string yearMonth, string collectionType)
        {
            return erpService.CustomerCollectionSerial(customerCode, yearMonth, collectionType);
        }

        public List<Sal_ModeOfPayment> ReadModeOfPayment(string locationCode, string programCode, string packageCode, string customerType, string agreementDatePicker)
        {
            return erpService.ReadModeOfPayment(locationCode, programCode, packageCode, customerType, agreementDatePicker);
        }


        public List<ItemCatNDescription> ReadItemCatNDescription()
        {
            return erpService.ReadItemCatNDescription();
        }


        public Sal_SalesItems SaveSalesItems(Sal_SalesItems objSalesItems)
        {
            return erpService.Create(objSalesItems);
        }

        public Sal_SalesItemsWithSerialNo SaveSalesItemWithSerials(Sal_SalesItemsWithSerialNo objItemSalesWithSerialNo)
        {
            return erpService.Create(objItemSalesWithSerialNo);
        }

        public List<Inv_ItemMaster> ReadItemMaster()
        {
            return erpService.ReadItemMaster();
        }

        public RASolarERP.Model.Common_DistrictInfo ReadDistrict(string districtCode)
        {
            return erpService.ReadDistrict(districtCode);
        }

        public RASolarERP.Model.Common_UpazillaInfo ReadUpazillaByID(string upazillaCode)
        {
            return erpService.ReadUpazillaByID(upazillaCode);
        }

        public CustomerInfoNPackageDetailsForSystemReturn ReadSystemReturnInfo(string customerCode, DateTime returnDate)
        {
            return erpService.ReadSystemReturnInfo(customerCode, returnDate);
        }

        public List<Sal_CustomerType> ReadCustomerTypes()
        {
            return erpService.ReadCustomerTypes();
        }

        public int LastUsedCustomerSerial(string unitCode, string programCode)
        {
            return erpService.LastUsedCustomerSerial(unitCode, programCode);
        }

        public List<CustomerLedgerReport> ReadCustomerLedgerReport(string customerCode)
        {
            return erpService.ReadCustomerLedgerReport(customerCode);
        }

        public Sal_Customer UpdateCustomer(Sal_Customer objCustomer)
        {
            return erpService.Update(objCustomer);
        }
        public Sal_Customer UpdateCustomer(Sal_Customer objCustomer, string remarksNotes)
        {
            return erpService.Update(objCustomer, remarksNotes);
        }
        public List<GetCustomerTransferredButNotYetAccepted> ReadCustomerTransferredButNotYetAccepted(string locationCode)
        {
            return erpService.ReadCustomerTransferredButNotYetAccepted(locationCode).ToList();
        }

        public List<LocationWiseEmployeeTargetEntryCheck> ReadLocationWiseEmployeeTargetEntryCheck(string locatioCode, string YearMonth)
        {
            return erpService.ReadLocationWiseEmployeeTargetEntryCheck(locatioCode, YearMonth).ToList();
        }

        public Sal_SalesAgreement ReadSalesAgreement(string customerCode)
        {
            return erpService.ReadSalesAgreement(customerCode);
        }

        public List<PackageDetailsForSystemReturn> ReadPackageDetailsForSystemReturn(string customerCode, DateTime returnDate)
        {
            return erpService.ReadPackageDetailsForSystemReturn(customerCode, returnDate);
        }

        public Sal_SystemReturn SaveSystemReturn(Sal_SystemReturn objSystemReturn, List<Sal_SystemReturnItems> lstSystemReturnItems, List<Sal_SystemReturnItemsWithSerialNo> lstSystemReturnItemsWithSerialNo, string locationCode, string serialTempTableRows)
        {
            return erpService.Create(objSystemReturn, lstSystemReturnItems, lstSystemReturnItemsWithSerialNo, locationCode, serialTempTableRows);
        }

        public List<PanelSerialList> PanelSerialByLocationAndStock(string locationCode, byte storeLocation, string itemCategory, string itemCapacity, byte agreementType, string packageCode)
        {
            return erpService.PanelSerialByLocationAndStock(locationCode, storeLocation, itemCategory, itemCapacity, agreementType, packageCode);
        }

        public DepretiatedPackagePriceBySRPanelSerial GetDepretiatedPackagePriceBySRPanelSerial(string panelSerial, string packageCode)
        {
            return erpService.GetDepretiatedPackagePriceBySRPanelSerial(panelSerial, packageCode);
        }

        public List<ProgressReview> ReadProgressReview()
        {
            return erpService.ReadProgressReview();
        }

        public Sal_CustomerStatus UpdateFPR(List<CustomerFPRAndScheduledCollectionEntry> lstCustomerFPREntry, string unitCode)
        {
            return erpService.UpdateFPR(lstCustomerFPREntry, unitCode);
        }

        public CustomerWiseFPR GetCustomerFPRAndScheduledCollectionEntry(string unitCode, string optionForMissingFPROrDay, string prmEMP_ID, string scheduledCollectionDay, bool firstTimeCallOrNot)
        {
            List<CustomerFPRAndScheduledCollectionEntry> lstCustomerWiseFPREntry = new List<CustomerFPRAndScheduledCollectionEntry>();
            List<CustomerFPRNDayWiseRegularOrODTarget> lstCustomerReglarODTarget = new List<CustomerFPRNDayWiseRegularOrODTarget>();

            if (firstTimeCallOrNot)
            {
                lstCustomerWiseFPREntry = erpService.GetCustomerFPRAndScheduledCollectionEntry(unitCode, optionForMissingFPROrDay, prmEMP_ID, scheduledCollectionDay);
                lstCustomerReglarODTarget = this.GetCustomerFPRNDayWiseRegularOrODTarget(unitCode, prmEMP_ID);
            }

            List<EmployeeDetailsInfo> locationWiseEmployee = new List<EmployeeDetailsInfo>();
            locationWiseEmployee = new RASolarHRMSService().ReadLocationWiseEmployeeWithUMAcountManager(unitCode);

            CustomerWiseFPR objCustomerFPREntry = new CustomerWiseFPR();
            objCustomerFPREntry.CustomerFPRAndScheduledCollection = lstCustomerWiseFPREntry;
            objCustomerFPREntry.CustomerFPRNDayWiseRegularRODTarget = lstCustomerReglarODTarget;
            objCustomerFPREntry.LocationWiseEmployee = locationWiseEmployee;
            objCustomerFPREntry.CollectionDays = new SalesDay().SalesCollectionDay();

            return objCustomerFPREntry;
        }

        //public List<CustomerFPRAndScheduledCollectionEntry> GetCustomerFPRAndScheduledCollectionEntry(string unitCode, string optionForMissingFPROrDay, string prmEMP_ID, string scheduledCollectionDay)
        //{
        //    return erpService.GetCustomerFPRAndScheduledCollectionEntry(unitCode, optionForMissingFPROrDay, prmEMP_ID, scheduledCollectionDay);
        //}

        public List<CustomerFPRNDayWiseRegularOrODTarget> GetCustomerFPRNDayWiseRegularOrODTarget(string unitCode, string employeeID)
        {
            return erpService.GetCustomerFPRNDayWiseRegularOrODTarget(unitCode, employeeID);
        }

        public List<LocationNEmployeeWiseDailySalesNCollectionTarget> ReadLocationNEmployeeWiseDailySalesNCollectionTarget(string locationCode)
        {
            return erpService.ReadLocationNEmployeeWiseDailySalesNCollectionTarget(locationCode);
        }

        public bool UpdateEmployeeWiseDailyTarget(List<Sal_LocationNEmployeeWiseDailySalesNCollectionTarget> lstEmployeeDailyTarget)
        {
            return erpService.Update(lstEmployeeDailyTarget);
        }
        public List<SalSalesNCollectionTargetVsAchievementForGraph> ReadSalSalesNCollectionTargetVsAchievementForGraph(string reportType, string locationCode, string employeeID)
        {
            return erpService.ReadSalSalesNCollectionTargetVsAchievementForGraph(reportType, locationCode, employeeID);
        }

        public List<Sal_CustomerFuelUsed> ReadCustomerFuelUsed()
        {
            return erpService.ReadCustomerFuelUsed();
        }

        public List<Sal_CustomerIncomeRange> ReadCustomerIncomeRange()
        {
            return erpService.ReadCustomerIncomeRange();
        }

        public List<Sal_CustomerOccupations> ReadCustomerOccupations()
        {
            return erpService.ReadCustomerOccupations();
        }

        public List<Sal_CustomerRelations> ReadCustomerRelations()
        {
            return erpService.ReadCustomerRelations();
        }

        public string SparseChallanSequenceNumberMax(string locationCode, string yearMonthDate)
        {
            return erpService.SparseChallanSequenceNumberMax(locationCode, yearMonthDate);
        }

        public Sal_SparePartsSalesMaster SaveSparePartsChallan(Inv_ChallanMaster objChallanMaster, List<Inv_ChallanDetails> lstChallanDetails, List<Inv_ChallanDetailsWithSerialNo> lstChallanDetailsWithSerialNo, Sal_SparePartsSalesMaster objSparePartsSalesMaster, List<Sal_SparePartsSalesItems> lstSparePartsSalesItems, List<Sal_SparePartsSalesItemsWithSerialNo> lstSparePartsSalesItemsWithSerialNo)
        {
            return erpService.Create(objChallanMaster, lstChallanDetails, lstChallanDetailsWithSerialNo, objSparePartsSalesMaster, lstSparePartsSalesItems, lstSparePartsSalesItemsWithSerialNo);
        }

        public List<Sal_ListedUnitPriceForSparePartsSales> ReadListedUnitPriceForSparePartsSales()
        {
            return erpService.ReadListedUnitPriceForSparePartsSales();
        }

        public Sal_ListedUnitPriceForSparePartsSales ReadListedUnitPriceForSparePartsSales(string yearMonth, string itemCode, byte stockLocation)
        {
            return erpService.ReadListedUnitPriceForSparePartsSales(yearMonth, itemCode, stockLocation);
        }

        //Code By T.Alam

        public List<Sal_SparePartsSales_DataFromExternalSources> ReadSalesExternalSourceByLCode(string locationCode)
        {
            return erpService.ReadSalesExternalSourceByLCode(locationCode);
        }

        public List<Sal_SalesAgreementPrePost_DataFromExternalSources> ReadSalesResalesAgrExternalSourceByLCode(string locationCode)
        {
            return erpService.ReadSalesResalesAgrExternalSourceByLCode(locationCode); 
        }
        public string ReadCustomerTraineeName(string trainerId)
        {
            return erpService.ReadCustomerTraineeName(trainerId);
        }

        public List<CustomerTrainingInfo> ReadCustomerTrainingInfo(string unitCode, bool trainingStatus, byte trainingBatchNo)
        {
            return erpService.ReadCustomerTrainingInfo(unitCode, trainingStatus, trainingBatchNo);
        }

        public List<GetUnitWiseCustomerTrainingSchedule> ReadCustomerTrainingSchedule(string unitCode, DateTime? trainingDate, byte? trainingBatchNumber)
        {
            return erpService.ReadGetUnitWiseCustomerTrainingSchedule(unitCode, trainingDate, trainingBatchNumber);
        }

        public Sal_CustomerTrainingTransMaster Create(Sal_CustomerTrainingTransMaster objSalCustomerTrainingMaster, List<Sal_CustomerTrainingTransDetails> lstCustomerTrainingTransDetails)
        {
            return erpService.Create(objSalCustomerTrainingMaster, lstCustomerTrainingTransDetails);
        }

        public List<Sal_ItemSetMaster> ReadItemSetMaster()
        {
            return erpService.ReadItemSetMaster();
        }
        public List<Sal_ItemSetDetail> ReadItemSetDetails(string itemSetCode)
        {
            return erpService.ReadItemSetDetails(itemSetCode);
        }

        public List<SparePartSetDetils> ReadSparePartSetDetils(string itemSetCode)
        {
            return erpService.ReadSparePartSetDetils(itemSetCode);
        }

        public Sal_ItemSetMaster ReadItemSetMaster(string itemSetCode)
        {
            return erpService.ReadItemSetMaster(itemSetCode);
        }

        public List<CustomerDisasterRecoveryDetails> ReadCustomerDisasterRecoveryList(string collectionType, byte customerStatus, DateTime collectionDate, string locationCode)
        {
            RASolarHRMSService hrmService = new RASolarHRMSService();

            List<CustomerDisasterRecoveryDetails> lstCustomerDisasterRecoveryDetails = new List<CustomerDisasterRecoveryDetails>();
            lstCustomerDisasterRecoveryDetails = erpService.ReadCustomerDisasterRecoveryList(collectionType, customerStatus, collectionDate, locationCode);

            string[] aa = lstCustomerDisasterRecoveryDetails.Select(s => s.CustomerFPREmployeeCode).Distinct().ToArray();
            List<string> uids = new List<string>(aa);

            List<EmployeeDetailsInfo> employeeInfo = new List<EmployeeDetailsInfo>();
            employeeInfo = hrmService.ReadEmployeeInfo(uids);

            if (employeeInfo.Count() > 0)
            {
                lstCustomerDisasterRecoveryDetails = (from cdrd in lstCustomerDisasterRecoveryDetails
                                                      join emi in employeeInfo
                                                      on cdrd.CustomerFPREmployeeCode equals emi.EmployeeID
                                                      select new CustomerDisasterRecoveryDetails
                                                      {
                                                          CustomerCode = cdrd.CustomerCode,
                                                          CustomerName = cdrd.CustomerName,
                                                          AgreementDate = cdrd.AgreementDate,
                                                          AgreementDuration = cdrd.AgreementDuration,
                                                          CustomerFPREmployeeCode = cdrd.CustomerFPREmployeeCode,
                                                          CustomerFPREmployeeName = emi.EmployeeName
                                                      }).ToList();

            }

            return lstCustomerDisasterRecoveryDetails;
        }

        public decimal ReadCustomerDetailsForDRFACollection(string collectionType, string customerCode, string locationCode, DateTime collectionDate, byte customerStatus)
        {
            return erpService.ReadCustomerDetailsForDRFACollection(collectionType, customerCode, locationCode, collectionDate, customerStatus);
        }

        public List<UnitWiseCashMemoInformation> ReadCashMemoInformation(string locationCode, string fromDate, string toDate)
        {
            return erpService.ReadCashMemoInformation(locationCode, fromDate, toDate);
        }

        public string GetAuditSequenceNumber(string locationCode)
        {
            RASolarHRMSService hrmService = new RASolarHRMSService();
            return hrmService.GetAuditSequenceNumber(locationCode);
        }

        public Aud_AuditAdjustmentRelatedCollectionFromCustomers SaveCustomerCollectionForAudit(Sal_CollectionFromCustomers objCustomerCollection, Aud_AuditAdjustmentRelatedCollectionFromCustomers objAuditAdjustmentCustomerCollection, string cashMemoUsesId)
        {
            return erpService.SaveCustomerCollectionForAudit(objCustomerCollection, objAuditAdjustmentCustomerCollection, cashMemoUsesId);
        }

        public Sal_CollectionFromCustomers UpdateCustomerCollectionAuditAdjustment(Sal_CollectionFromCustomers objCollectionFromCustomers, Aud_AuditAdjustmentRelatedCollectionFromCustomers objCollectionAuditAdjustnment)
        {
            return erpService.Update(objCollectionFromCustomers, objCollectionAuditAdjustnment);
        }

        public List<CustomerCollectionAdjustmentForAudit> ReadCustomerCollectionAdjustmentForAudit(string customerCode, string yearMonth, string auditSeqNo)
        {
            return erpService.ReadCustomerCollectionAdjustmentForAudit(customerCode, yearMonth, auditSeqNo);
        }

        public bool IsAuditAdjustmentObservationOnSalesAgreementExistOrNot(string locationCode, string yearMonth, string customerCode, string auditSeqNo)
        {
            return erpService.IsAuditAdjustmentObservationOnSalesAgreementExistOrNot(locationCode, yearMonth, customerCode, auditSeqNo);
        }

        public Aud_AuditAdjustmentObservationOnSalesAgreement Create(Aud_AuditAdjustmentObservationOnSalesAgreement objAuditAdjustmentObservationOnSalesAgreement, Aud_AuditAdjustmentObservationOnSalesAgreement objPreviousDataAuditAdjustmentObservationOnSalesAgreement)
        {
            return erpService.Create(objAuditAdjustmentObservationOnSalesAgreement, objPreviousDataAuditAdjustmentObservationOnSalesAgreement);
        }

        public Aud_AuditAdjustmentObservationOnSalesAgreement Update(Aud_AuditAdjustmentObservationOnSalesAgreement objAuditOnSalesAgreement)
        {
            return erpService.Update(objAuditOnSalesAgreement);
        }

        public CustomerDataToCloseWithFullPaidOrWaive getCustomerDataToCloseWithFullPaidOrWaive(string select, string customerCode, string currentMonth, string locationCode)
        {
            return erpService.getCustomerDataToCloseWithFullPaidOrWaive(select, customerCode, currentMonth, locationCode);
        }

        public CustomerDataToPersonalInformation GetCustomerDataPersonalInformation(string customerCode)
        {

            return erpService.GetCustomerDataPersonalInformation(customerCode);

        }

        public Sal_SalesAgreementClosedWithFullPaidOrWaive SaveFullPaiedCustomer(Sal_SalesAgreementClosedWithFullPaidOrWaive objSalesAgreementClosedWithFullPaidOrWaive, string closedIn, bool? approvalRequiredForFullPayed)
        {
            return erpService.SaveFullPaiedCustomer(objSalesAgreementClosedWithFullPaidOrWaive, closedIn, approvalRequiredForFullPayed);
        }

        // Auditor's Section For Sales Correction------------------------------

        public CustomerNAgreementNItemDetails ReadCustomerNAgreementItemDetails(string customerCode, string programCode)
        {
            CustomerNAgreementNItemDetails objCustomerNAgreementNItemDetails = new CustomerNAgreementNItemDetails();
            objCustomerNAgreementNItemDetails = erpService.ReadCustomerNAgreementItemDetails(customerCode);

            objCustomerNAgreementNItemDetails.ProjectInfo = this.ReadProjectForAuditor(programCode);
            objCustomerNAgreementNItemDetails.PanelStructure = this.ReadInvItemsForAuditors(Helper.InventoryItem, "PNLSR1");
            objCustomerNAgreementNItemDetails.Holder = this.ReadInvItemsForAuditors(Helper.InventoryItem, "HOL001");
            objCustomerNAgreementNItemDetails.BatteryInfo = this.ReadInvItemsForAuditors(Helper.InventoryItem, "BAT001");

            objCustomerNAgreementNItemDetails.ProjectName = objCustomerNAgreementNItemDetails.ProjectInfo.Where(p => p.ProjectCode == objCustomerNAgreementNItemDetails.ProjectCode).Select(s => s.ProjectName).FirstOrDefault();

            return objCustomerNAgreementNItemDetails;
        }

        public List<ProjectInfo> ReadProjectForAuditor(string programCode)
        {
            return erpService.ReadProjectForAuditor(programCode);
        }

        public List<ItemCapacity> ReadPackageOrItemCapacityForAuditor(string projectCode, string isForItemOrPackage)
        {
            return erpService.ReadPackageOrItemCapacityForAuditor(projectCode, isForItemOrPackage);
        }

        public List<LightInfo> ReadLightForAuditor(string capacityID)
        {
            return erpService.ReadLightForAuditor(capacityID);
        }

        public List<PackageInformation> ReadPackagesForAuditor(string capacityId, string lightId)
        {
            return erpService.ReadPackagesForAuditor(capacityId, lightId);
        }

        public List<ItemInfo> ReadInvItemsForAuditors(string itemType, string itemCategory)
        {
            return erpService.ReadInvItemsForAuditors(itemType, itemCategory);
        }

        public List<ItemInfo> ReadInvItemsForAuditors(string itemType, string itemCategory, string itemCapacity)
        {
            return erpService.ReadInvItemsForAuditors(itemType, itemCategory, itemCapacity);
        }

        public List<ItemSerialInfo> ReadItemStockWithSerialNoByLocationForAuditor(byte storeLocation, string locationCode, string itemCode)
        {
            return erpService.ReadItemStockWithSerialNoByLocationForAuditor(storeLocation, locationCode, itemCode);
        }

        public List<ServiceChargeInformation> ReadServiceChargePolicyforAuditor(string customerType, string modeOfPayment)
        {
            return erpService.ReadServiceChargePolicyForAuditor(customerType, modeOfPayment);
        }

        public List<DownPaymentPolicy> ReadDownPaymentPolicyForAuditor(string modeOfPayment, string packageCode)
        {
            return erpService.ReadDownPaymentPolicyForAuditor(modeOfPayment, packageCode);
        }

        public DiscountPolicy ReadDiscountPolicyByModeofPaymentNDiscountIdForAuditor(string modeOfPayment, string discountId)
        {
            return erpService.ReadDiscountPolicyByModeofPaymentNDiscountIdForAuditor(modeOfPayment, discountId);
        }

        public DiscountPolicy ReadDiscountPolicyByModeofPaymentNPackageIdForAuditor(string modeOfPayment, string packageCode)
        {
            return erpService.ReadDiscountPolicyByModeofPaymentNPackageIdForAuditor(modeOfPayment, packageCode);
        }

        public List<ODRecoveryStatusMonitoring> ReadODRecoveryStatusMonitoring(string locationCode, bool IsOnlyForCollectionDatePassed, string customerGrading, string CustomerFPR, byte? scheduledCollectionDay)
        {
            return erpService.ReadODRecoveryStatusMonitoring(locationCode, IsOnlyForCollectionDatePassed, customerGrading, CustomerFPR, scheduledCollectionDay);
        }

        public SalesRecoveryCommitmentByReviewViewModel ReadSalesRecoveryCommitmentByReview(string reportType, string locationCode)
        {
            SalesRecoveryCommitmentByReviewViewModel objODRecoveryCommitmentByUnitViewModel = new SalesRecoveryCommitmentByReviewViewModel();
            objODRecoveryCommitmentByUnitViewModel.SalesRecoveryCommitmentByReview = erpService.ReadSalesRecoveryCommitmentByReview(reportType, locationCode);
            objODRecoveryCommitmentByUnitViewModel.Zone = erpService.Location();

            return objODRecoveryCommitmentByUnitViewModel;
        }

        public List<SalesRecoveryStatusEntryMonitoring> ReadSalesRecoveryStatusEntryMonitoring(string locationCode, bool IsOnlyForCollectionDatePassed, string customerGrading, string CustomerFPR, byte? scheduledCollectionDay)
        {
            return erpService.ReadSalesRecoveryStatusEntryMonitoring(locationCode, IsOnlyForCollectionDatePassed, customerGrading, CustomerFPR, scheduledCollectionDay);
        }

        public Sal_DailyBusinessPerformanceMonitoring_SalesAndRecoveryStatusByUnit ReadDailyBusinessPerformanceMonitoringRemarks(string yearMonth, string locationCode)
        {
            return erpService.ReadDailyBusinessPerformanceMonitoringRemarks(yearMonth, locationCode);
        }
         

        public List<SalesRecoveryStatusEntryMonitoring> UpdateSalesRecoveryStatusEntryMonitoring(string option, string locationCode, string customerCode, DateTime? umLastNextRecoveryDate, string umLastRemarks, string umLastOverallRemarks, string amLastRemarks)
        {
            return erpService.UpdateSalesRecoveryStatusEntryMonitoring(option, locationCode, customerCode, umLastNextRecoveryDate, umLastRemarks, umLastOverallRemarks, amLastRemarks);
        }

        public Sal_ODCustomerGrading UpdateODRecoveryStatusMonitoring(Sal_ODCustomerGrading objODCustomerGrading)
        {
            return erpService.UpdateODRecoveryStatusMonitoring(objODCustomerGrading);
        }

        public Sal_ODRecoveryCommitmentByRMnZM UpdateODRecoveryCommitmentByRMnZM(Sal_ODRecoveryCommitmentByRMnZM objODRecoveryCommitmentByRMnZM)
        {
            return erpService.UpdateODRecoveryCommitmentByRMnZM(objODRecoveryCommitmentByRMnZM);
        }

        public void CustomerCollectionSaveForUpdateOrDelete(string updateDeleteOptions, Sal_CollectionFromCustomers objCollectionFromCustomers)
        {
            erpService.CustomerCollectionSaveForUpdateOrDelete(updateDeleteOptions, objCollectionFromCustomers);
        }

        public bool IsAuditAdjustmentObservationOnSalesAgreementExistOrNot(string locationCode, string customerCode)
        {
            return erpService.IsAuditAdjustmentObservationOnSalesAgreementExistOrNot(locationCode, customerCode);
        }

        public byte getCustomerCollectionEntrySerialNumber(string customerCode, string collectionDate)
        {
            return erpService.getCustomerCollectionEntrySerialNumber(customerCode, collectionDate);
        }

        public void CustomerCollectionDelete(Sal_CollectionFromCustomersPrePost objCollectionFromCustomers)
        {
            erpService.CustomerCollectionDelete(objCollectionFromCustomers);
        }
        
        public ArrayList ReadSpecialPackageListForSales(string packageCapacityId, string lightId, string programCode, string salesReSalesOrBoth)
        {
            return erpService.ReadSpecialPackageListForSales(packageCapacityId, lightId, programCode, salesReSalesOrBoth);
        }

        public PackagePricingDetailsForSalesAgreement ReadPackagePricingDetailsForSalesAgreement(string locationCode, string programCode, string salesReSalesOrBoth, string customerType, string packageCapacity, string lightID, string packageCode, string modeOfPaymentID, string changedDownPaymentAmount) 
        {
            return erpService.ReadPackagePricingDetailsForSalesAgreement(locationCode, programCode, salesReSalesOrBoth, customerType, packageCapacity, lightID, packageCode, modeOfPaymentID, changedDownPaymentAmount);
        }

        public ArrayList ReadModeOfPaymentForSpecialPackageSales(string salesReSalesOrBoth)
        {
            return erpService.ReadModeOfPaymentForSpecialPackageSales(salesReSalesOrBoth);
        }

        public Sal_CustomerStatus UpdateCustomerStatusForSalesMonitoring(Sal_CustomerStatus objCustomerStatus)
        {
            return erpService.UpdateCustomerStatusForSalesMonitoring(objCustomerStatus);
        }

        public List<DailyPerformanceMonitoringForSales> ReadDailyPerformanceMonitoringForSales(string reportOption, string locationCode)
        {
            return erpService.ReadDailyPerformanceMonitoringForSales(reportOption, locationCode);
        }

        public List<DailyBusinessPerformanceMonitoringCollection> ReadDailyBusinessPerformanceMonitoringCollection(string reportOption, string locationCode)
        {
           return erpService.ReadDailyBusinessPerformanceMonitoringCollection(reportOption, locationCode);
        }

        public List<DailyBusinessPerformanceMonitoringNetODIncreaseOrDecreaseComperison> ReadDailyBusinessPerformanceMonitoringIncreaseOrDecreaseComperison(string reportOption, string locationCode)
        {
            return erpService.ReadDailyBusinessPerformanceMonitoringIncreaseOrDecreaseComperison(reportOption, locationCode);
        }

        public List<DailyBusinessPerformanceMonitoringSalesNCollectionTargetVsAchievement> ReadDailyBusinessPerformanceMonitoringSalesNCollectionTargetVsAchievement(string reportOption, string locationCode, DateTime dateForTheStatus)
        {
            return erpService.ReadDailyBusinessPerformanceMonitoringSalesNCollectionTargetVsAchievement(reportOption, locationCode, dateForTheStatus);
        }

        public List<DailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus> ReadDailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus(string reportOption, string locationCode)
        {
            return erpService.ReadDailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus(reportOption, locationCode);
        }

        public List<DailyBusinessPerformanceMonitoringOtherStatus> ReadDailyBusinessPerformanceMonitoringOtherStatus(string reportOption, string locationCode)
        {
            return erpService.ReadDailyBusinessPerformanceMonitoringOtherStatus(reportOption, locationCode);
        }
    }
}