using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RASolarHelper;
using RASolarERP.DomainModel.SalesModel;
using RASolarERP.DomainModel.InventoryModel;
using System.Collections;
using RASolarERP.DomainModel.AMSModel;
using RASolarERP.DomainModel.HRMSModel;


namespace RASolarERP.Model
{
    public interface IRASolarERPRepository : IDisposable
    {
        IQueryable<Common_ZoneInfo> Zone();
        Common_ZoneInfo Zone(string zoneCode);

        IQueryable<Common_RegionInfo> Region();
        Common_RegionInfo Region(string regionCode);
        IQueryable<Common_RegionInfo> RegionByZoneCode(string zoneCode);

        IQueryable<Common_UnitInfo> Unit();
        Common_UnitInfo Unit(string unitCode);
        IQueryable<Common_UnitInfo> UnitByRegionCode(string regionCode);
        int LastUsedCustomerSerial(string unitCode, string programCode);

        List<Common_UnionInfo> ReadUnionInfo(string upazilaCode);

        List<Common_PostOfficeInfo> ReadPostOfficeInfo(string upazilaCode);


        List<Common_PostOfficeInfo> PostOfficeInfoLoadForUnitCode(string unitCode);
      
        //ReadCustomerPostOfficeInfo
        List<Common_PostOfficeInfo> ReadCustomerPostOfficeInfo(string CustomerCode, string PostOffice);  

        List<Common_DistrictInfo> ReadDistrictInfo(string upazilaCode);

        List<REPSummaryForTheDayClosingForSales> ReadREPSummaryForTheDayClosingForSalesData(string reportType, string locationCode);

        List<SummaryForTheDayClosingForCollectionInfo> ReadSummaryForTheDayClosingForCollectionInfo(string reportType, string locationCode);

        List<SummaryForTheDayClosingForInventoryInfo> ReadSummaryForTheDayClosingForInventoryInfo(string reportType, string locationCode, byte storeLocation);

        List<SummaryForTheDayClosingForAccountingInfo> ReadSummaryForTheDayClosingForAccountingInfo(string reportType, string locationCode);

        //Common_UnitInfoWiseEntryStatus UnitWiseEntryStatus(string unitCode, string YearMonth);

        //Common_UnitInfoWiseCustomerStatus Create(Common_UnitInfoWiseCustomerStatus objUnitWiseCustomerStatus);
        //Common_UnitInfoWiseCustomerStatus Update(Common_UnitInfoWiseCustomerStatus objUnitWiseCustomerStatus);
        //IQueryable<Common_UnitInfoWiseCustomerStatus> UnitWiseCustomerStatus(string unitCode);
        //Common_UnitInfoWiseCustomerStatus ReadUnitWiseCustomerStatus(string unitCode, string customerId);
        //Common_UnitInfoWiseCustomerStatus ReadUnitWiseCustomerStatus(string customerId);

        List<InventoryDataEntryStatus> InventoryDataEntryState(string reportType, string locationType, string yearMonth);

        List<ClosingInventoryValuation> ClosingInventoryReport(string yearMonth);

        List<SHSDelivaryNoteChallan> ViewDeliveryNoteChallanSHSReport(string distribScheduleNo);

        List<DailyZonalPerformanceMonitoring> DailyPerformanceMonitoringZonalReport();

        List<InventoryAtVendorValuationByStockLocation> InventoryAtVendorValuationByStockLocation(string yearMonth);

        List<SalesDataEntryStatus> SalesEntryStatus(string reportType, string locationType, string yearMonth);

        // Common_UnitInfoWiseEntryStatus Update(Common_UnitInfoWiseEntryStatus unitWiseEntryStatus);

        // List<InventoryAuditAdjustment> InventoryAuditAdjustmentAdjust(string unitCode, string yearMonth, string yearMonthWrite, byte componentStatus, int stockLocation);

        List<Aud_AdjustmentReasonCodes> AuditAdjustMentReasons(string reasonPurposeID, string reasonForUserOrAuditor, string reasonForModule);

        List<Aud_AdjustmentReasonCodes> CollectionAdjustMentReasons(string reasonPurposeID1, string reasonPurposeID2, string reasonForUserOrAuditor, string reasonForModule);

        //List<Aud_AdjustmentReasonCodes> AuditAdjustMentReasonsForSales(string reasonPurposeID);

        //IQueryable<Aud_AdjustmentReasonCodes> AuditAdjustMentReasons(string reasonForUserOrAuditor, byte reasonForInventoryStock, string reasonForModule);

        //string AuditAdjustmentOpenMonth(string unitCode);

        //Aud_AuditingMaster Create(Aud_AuditingMaster objAud_AuditingMaster);
        //Aud_AuditingMaster Update(Aud_AuditingMaster objAud_AuditingMaster);
        //Aud_AuditingMaster AuditingMasterByLocationCode(string locationCode);

        //Common_InventoryTransaction ReadInventoryTransaction(string unitCode, string itemCode, string yearMonth, int stockLocation);
        //Common_InventoryTransaction Create(Common_InventoryTransaction objInventoryTranscation);
        //Common_InventoryTransaction Update(Common_InventoryTransaction objInventoryTranscation);


        Sal_LocationNEmployeeWiseActivityMonthly ReadLocationNEmployeeWiseActivityMonthly(string locationCode, string employeeID, string yearMonth);
        List<Sal_LocationNEmployeeWiseActivityMonthly> ReadLocationNEmployeeWiseActivityMonthly(string locationCode, string yearMonth);

        Sal_LocationNEmployeeWiseActivityMonthly Update(Sal_LocationNEmployeeWiseActivityMonthly objLocationNEmployeeWiseActivityMonthlyUpdate);

        Sal_LocationNEmployeeWiseActivityMonthly Create(Sal_LocationNEmployeeWiseActivityMonthly objLocationNEmployeeWiseActivityMonthlyCreate);

        GetLocationNEmployeeWiseDailyEntry ReadLocationNEmployeeWiseActivity(string locationCode, string employeeID, string yearMonth, DateTime transDate);
        Sal_LocationNEmployeeWiseActivityDaily Create(Sal_LocationNEmployeeWiseActivityDaily objLocationNEmployeeWiseActivityDailyCreate);
        Sal_LocationNEmployeeWiseActivityDaily ReadLocationNEmployeeWiseActivityDaily(string locationCode, string employeeID, string yearMonth, DateTime transDate);

        List<Sal_LocationNEmployeeWiseActivityDaily> ReadLocationNEmployeeWiseActivityDaily(string locationCode, string yearMonth, DateTime transDate);

        Sal_LocationNEmployeeWiseActivityDaily Update(Sal_LocationNEmployeeWiseActivityDaily objLocationNEmployeeWiseActivityDailyUpdate);

        IQueryable<Common_Sys_StatusInfo> CustomerStatus();

        //IQueryable<Common_DailyProgressReport> ReadDailyProgressReport();

        //List<CustomerStatusListV2> ReadCustomerStatusListV2(string unitCode, byte customerStatus, byte paymentStatus, DateTime currentCollectionDate);

        //List<GetCustomerListWithRecoveryStatus> ReadCustomerListWithRecoveryStatus(string unitCode, byte customerStatus, DateTime currentCollectionDate);

        List<OverdueCollectionTargetVsAchievementByUnitOffice> ReadOverdueCollectionTargetVsAchievementByUnitOffice(string spYearMonth, string spLocationCode, string reportType);

        List<CollectionEfficiencyByUnitOfficeSummary> ReadCollectionEfficiencyByUnitOfficeSummary(string yearMonth, string locationCode, string reportType);

        //List<Common_InvStockInTransitByMonth> ReadStockInTransitValue(string YearMonth);

        List<InventorySummaryToDetailViewReport> ReadInventorySummaryToDetailViewReport(string yearMonth, string itemCode);

        List<SalesSummaryToDetailView> ReadSalesSummaryToDetailView(string yearMonth);

        List<CustomerTrainingSummary> ReadCustomerTrainingSummary(DateTime dtFromDate, DateTime dtToDate);

        List<CustomerTrainingDetails> ReadCustomerTrainingDetails(string dtFromDate, string dtToDate);

        List<Common_ProjectInfo> ReadProject(string programCode);
        List<Common_ProgramInfo> ReadProgram();
        //List<Common_Package> ReadPackage(string programCode, string projectCode);
        //Common_Package ReadPackage(string packageCode, string programCode, string projectCode);
        List<PackageDetails> ReadPackageDetails(string packageCode, string modeOfPaymentID, string customerType);
        List<PackageDetails> ReadPackageDetailsExtra(); 
        Common_PeriodOpenClose ReadPeriodOpenClose(string locationCode);
        //ReadEmployeeDetailsVisit(option, empId, objLoginHelper.LogInForUnitCode, ddlLocationPart1, ddlLocationPart4)
        List<EmployeeVisit> ReadEmployeeDetailsVisit(string option, string empID, string locationCode, string ddlLocationPart1, string ddlLocationPart4);

        //List<Hrm_LocationWiseEmployee> ReadLocationWiseEmployee(string locationCode);

        List<GetLocationWiseEmployee> ReadGetLocationWiseEmployee(string locationCode);

        Sal_LocationWiseSalesNCollectionTarget ReadLocationWiseSalesNCollectionTarget(string locationCode, string yearMonth);

        Sal_LocationWiseSalesNCollectionTarget Update(Sal_LocationWiseSalesNCollectionTarget objSalesNCollectionTarget);

        List<GetLocationWiseEmployeeTarget> ReadGetLocationWiseEmployeeTarget(string locationCode, string yearMonth);

        string ExecuteDayEndProcess(string Location, string CloseableDate);

        //UpdateLoadRequestEntry
        string UpdateLoadRequestEntry(string locationCode, string employeeID, string dayOpenDay, string prmCorporatePhoneNo, string rsfServiceQunt, string paywellServiceQunt, string cashMemo);

        string UpdateLoadRequestEntryFinalSave(string locationCode, string dayOpenDayTocurrentdate, string txtRsfServiceTotal, string txtRsfPayWellTotal);
        
        string EntryEmployeeVisitPlan(string prmLocationCode, string prmEmployeeID, string prmMovementDate, string prmTTLocationCode, string prmNoFcustomerVisit, string prmPurposeOfVisit, string prmDurationOfVisitInHours, string prmUserName, string prmDBTransType);
       


        void CustomerTransferV2(string customerID, string LocationCode, string CustomerTransferOrReceive, string TransferToUnit, string TransDate, string NoteForTransfer, string UserID, string DBTransType);

        List<ProgressReviewDataEntryStatusDaily> ReadProgressReviewDataEntryStatusDaily(string reportType, string locationCode, string yearMonth, string respectiveAreaUserID);

        //Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement Create(List<Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement> objLocationNEmployeeWiseWeeklySalesNCollectionAchievement);
        //List<LocationAndEmployeeWiseWeeklySalesAndCollectionEntry> ReadLocationAndEmployeeWiseWeeklySalesAndCollectionEntry(string locationCode, string yearWeek);

        //Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement ReadWeeklySalesNCollectionTargetNAchievement(string locationCode, string yearWeek, string eMP_ID, DateTime transDate);
        //List<Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement> ReadWeeklySalesNCollectionTargetNAchievement(string locationCode, string yearWeek, DateTime transDate);
        //Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement Update(Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement objLocationNEmployeeWiseWeeklySalesNCollectionAchievement);

        //List<LocationAndEmployeeWiseWeeklySalesAndCollectionReport> ReadLocationAndEmployeeWiseWeeklySalesAndCollectionReport(string reportType, string locationCode, string yearWeek);

       // Sal_LocationWiseWeeklySalesNCollectionSummary LocationWiseWeeklySalesNCollectionSummary(string locationCode, string yearWeek);
        //Sal_LocationWiseWeeklySalesNCollectionSummary Create(Sal_LocationWiseWeeklySalesNCollectionSummary objSal_LocationWiseWeeklySalesNCollectionSummary);
       // Sal_LocationWiseWeeklySalesNCollectionSummary Update(Sal_LocationWiseWeeklySalesNCollectionSummary objSal_LocationWiseWeeklySalesNCollectionSummary);

        List<Sal_CustomerWiseOverdueBalanceForTracker> ReadCustomerWiseOverdueBalanceForTracker(String unitCode);
        List<CustomerWiseOverdueBalanceTracker> ReadCustomerWiseOverdueBalanceForTracker(String unitCode, string weekNumber);
        bool Update(string unitCode, string customerCode, decimal collectedOverdue, string weekNumber);
        List<WeeklySalesNCollectionStatus> ReadWeeklySalesNCollectionStatus(string reportType, string locationCode, string yearWeek);
        //ReadTrackerLocationWiseWeeklyODCollectionTargetVsAchievement
        Common_CurrentYearMonthNWeek ReadCurrentYearMonthNWeek();

        List<Sal_Light> ReadLight(string capacityID);

        List<Sal_PackageMaster> ReadPopUpPackageList(string packageCode);
        List<PackageLightCapacityPop> ReadPopUpPackageCapacity(string packageCode);
        

        List<Sal_PackageOrItemCapacity> ReadPackageOrItemCapacity(string projectCode, string isForItemOrPackage);

        List<Sal_PackageMaster> ReadPackages(string capacityId, string lightId, byte salesType);

        List<Sal_PackageMaster> ReadPackagesPackgeListFrmPopUpTddlPckg(string capacityId, string lightId, byte salesType);
        

        //Sal_Validation_CustomerTypeNModeOfPaymentWiseServiceChargePolicy ReadServiceChargePolicy(string modeOfPaymentId, string customerType);
        //Sal_Validation_ModeOfPaymentWiseDiscountPolicy ReadDiscountPolicy(string modeOfPaymentId, string discountId);
        //Sal_Validation_ModeOfPaymentNPackageWiseDownpaymentPolicy ReadPackageVsDownpayment(string packageCode, string modeOfPaymentId);

        ServiceChargeInformation ReadServiceChargePolicy(string programCode, string customerType, string modeOfPayment);
        DownPaymentPolicy ReadDownPaymentPolicy(string modeOfPayment, string packageCode);
        DiscountPolicy ReadDiscountPolicyByModeofPaymentNDiscountId(string modeOfPayment, string discountId);
        DiscountPolicy ReadDiscountPolicyByModeofPaymentNPackageId(string modeOfPayment, string packageCode);

        Sal_Customer ReadCustomer(string customerCode);
        Sal_Customer Update(Sal_Customer objCustomer);
        Sal_Customer Update(Sal_Customer objCustomer, string remarksNotes);


        Sal_Customer ReadCustomer(string unitCode, string customerCode);

        Sal_SalesAgreement ReadSalesAgreement(string customerCode);
        Sal_SalesAgreementPrePost Create(Sal_CustomerPrePost objCustomer, Sal_SalesAgreementPrePost objSalesAgreement, List<Sal_SalesItemsPrePost> lstSalesItem, List<Sal_SalesItemsWithSerialNoPrePost> lstItemSalesWithSerialNo);
        Sal_SalesAgreement Create(Sal_Customer objCustomer, Sal_SalesAgreement objSalesAgreement, Sal_CustomerStatus objCustomerStatus, List<Sal_SalesItems> lstSalesItem, List<Sal_SalesItemsWithSerialNo> lstItemSalesWithSerialNo, int customerCurrentSerial, string serialTempTableRows);

        Common_UpazillaInfo ReadUpazillaByID(string upazillaCode);
        List<Common_UpazillaInfo> ReadUpazilla(string unitCode);
        List<Inv_ItemModel> ReadItemModel(string itemCatagory, string itemCapacity, string itemCategoryIdForNull);
        List<Inv_ItemModel> ReadItemModel(string itemCatagory);
        List<ItemCapacity> ReadItemCapacity(string itemCategory);

        List<RASolarERP.DomainModel.SalesModel.SystemReturnOrFullPaidCustomers> ReadSystemReturnOrFullPaidCustomers(string CustomerOrViewType, string locationCode, string fromDate, string toDate);
        //ReadTrackerLocationWiseWeeklyODCollectionTargetVsAchievement
        List<TrackerLocationWiseWeeklyODCollectionTargetVsAchievement> ReadTrackerLocationWiseWeeklyODCollectionTargetVsAchievement(string prmReportType, string prmYearMonth, string prmYearWeek);
        //LoadRequestEntryGet
        List<LoadRequestEntryGet> ReadLoadRequestEntryGet(string locationCode, string getDate);
        List<CheckLoadRequestEntry> firstCheckLoadRequestEntry(string yearMonth, string locationCode, string getDate);
        List<CheckLoadRequestEntryForLocationCode> LocationCheckLoadRequestEntry(string locationCode);
       
       

        List<LoadRequestEntryGet> InsertLoadRequest(string locationCode, string getDate); 

        List<UnitWiseCustomerRegisterReport> ReadUnitWiseCustomerRegisterReport(string unitCode);

        List<CustomerCollectionDetails> ReadCollectionFromCustomers(string customerCode, string yearMonth);
        Sal_CollectionFromCustomers ReadCollectionFromCustomers(string customerCode, string yearMonth, byte collectionSerial);
        Sal_CollectionFromCustomersPrePost ReadCollectionFromCustomersPrepost(string customerCode, string yearMonth, byte collectionSerial);

        int CustomerCollectionSerial(string customerCode, string yearMonth);
        int CustomerCollectionSerial(string customerCode, string yearMonth, string collectionType);

        Sal_CollectionFromCustomersPrePost Create(Sal_CollectionFromCustomersPrePost objCollectionFromCustomers, Sal_Customer objSalCustomerInfo);
        Sal_CollectionFromCustomersPrePost Update(Sal_CollectionFromCustomersPrePost objCollectionFromCustomers, Sal_CollectionFromCustomers objCustomerCol);

        List<UnitCollectionVsHeadOfficePhysicalCashMovement> ReadUnitCollectionVsHeadOfficePhysicalCashMovement(string reportType, string locationCode, string yearMonth);

        Sal_SalesItems Create(Sal_SalesItems objSalesItems);
        Sal_SalesItemsWithSerialNo Create(Sal_SalesItemsWithSerialNo objItemSalesWithSerialNo);

        List<Sal_ModeOfPayment> ReadModeOfPayment(string locationCode, string programCode, string packageCode, string customerType, string agreementDatePicker);

        List<ItemCatNDescription> ReadItemCatNDescription();
       
        List<Inv_ItemMaster> ReadItemMaster();
        List<Inv_ItemMaster> ReadItemMasterByItemCategory(string itemCategory);
        Inv_ItemMaster ReadItemMaster(string itemCode);
        List<Inv_ItemMaster> ReadItemMasterForItemCode(string itemCode);

        Common_DistrictInfo ReadDistrict(string districtCode);

        CustomerInfoNPackageDetailsForSystemReturn ReadSystemReturnInfo(string customerCode, DateTime returnDate);

        List<Sal_CustomerType> ReadCustomerTypes();

        List<Inv_Sys_ItemType> ReadItemType();
        Inv_ItemCategorySubCategory ReadItemCategorySubCategoryByCategoryID(string ItemCategoryID);
        List<Inv_ItemCategorySubCategory> ReadItemCategorySubCatagory(string itemTypeID);
        List<Inv_ItemCategorySubCategory> ReadItemCategorySubCatagoryByTransactionType(string itemTypeID, string transactionType, byte locationType);
        bool ReadIsItemCodeWiseValidationExist(string itemCategory, string transactionType);

        List<Inv_RouteMaster> ReadRootCategory();
        List<Inv_RouteMaster> ReadRouteMaster(string routeCategory);
        List<LocationInfo> ReadUnitList(string routeId);

       // List<EmployeeVisit> ReadEmployeeDetailsVisit(string empId, string locationCode);

        List<Sal_Validation_CapacityVsLight> ReadPackageCapacity();

        List<PersonEmployeeDetailsInfo> ReadPersonLocationWiseEmployee(string locationCode);
        List<LocationPart1District> ReadLocationPart1District(string empWisePart1, string empID, string locationCode);
        List<LocationPart2Upo> ReadLocationPart2District(string empWisePart1, string empID, string locationCode, string locationPart1);
       
      
       
        List<Sal_Validation_CapacityVsLight> ReadLightByPCapacityID(string capacityID);
        List<Sal_PackageMaster> ReadPackagesForSHSDP(string capacityId, string lightId);
        List<Inv_ItemModel> ReadPanelModelListForSHSDP(string packageCode);
        List<Inv_ItemModel> ReadBatteryModelListForSHSDP(string packageCode);
        int CheckDuplicateDistributionPlan(string distribScheduleNo, DateTime scheduleDate, string refScheduleNo);
        List<SHSDistributionPlanPackageORItem> RootWiseLocationNPackage(string distributionScheduleNo);
        SHSDistributionPlan_Master Create(SHSDistributionPlan_Master objSHSDistributionPlan_Master, List<SHSDistributionPlan_RootWiseLocation> lstSHSDistributionPlan_RootWiseLocation, List<SHSDistributionPlan_RootWiseLocationNPackage> lstSHSDistributionPlan_RootWiseLocationNPackage, List<SHSDistributionPlan_IndividualItem> lstSHSDistributionPlan_IndividualItem);
        SHSDistributionPlan_Master Update(SHSDistributionPlan_Master objSHSDistributionPlan_Master, List<SHSDistributionPlan_RootWiseLocation> lstSHSDistributionPlan_RootWiseLocation, List<SHSDistributionPlan_RootWiseLocationNPackage> lstSHSDistributionPlan_RootWiseLocationNPackage, List<SHSDistributionPlan_IndividualItem> lstSHSDistributionPlan_IndividualItem);

        string SaveSHSDistributionRouteTransfer(string routeid, string txtDistributionScheduleNo, string txtDistributionScheduleNoNew, string  dtpDelivaryDate, string txtDeliveryScheduleNo);

        List<Inv_StoreLocation> ReadStoreLocation();
        List<Inv_StoreLocation> ReadStoreLocation(string itemType);
        List<Inv_StoreLocation> ReadStoreLocation(int storeLocationID, string itemTypeId);
        List<Inv_StoreLocation> ReadStoreLocationByItemTypeAndTransaction(string itemType, string itemTransType, byte locationType, string location);

        List<InvAvailableItemInALocation> ReadInvAvailableItemInALocation(string locationCode, byte storeLocation, string itemCategory);

        //Inv_ItemSerialNoMaster ReadItemSerialNoMaster(string itemCode, string itemSerial, string itemCategory);
        Inv_ItemNItemCategoryWithSerialNoMaster ReadItemSerialNoMasterByItemCategoryWise(string itemCode, string itemSerial, string itemCategory);

        Inv_ItemStockWithSerialNoByLocation ReadItemStockWithSerialNoByLocation(byte storeLocation, string locationCode, string itemCode, string serialNumber);
        List<Inv_ItemStockWithSerialNoByLocation> ReadItemStockWithSerialNoByLocation(byte storeLocation, string locationCode, string itemCode);
        int Create(string insertedRows, string locationCode);
        List<Inv_ItemStockByLocation> Update(List<Inv_ItemStockByLocation> lstItemStockByLocation, List<Inv_ItemStockWithSerialNoByLocation> lstItemStockWithSerialNoByLocation);

        void Delete(object obj);
        void Delete(object obj1, object obj2);
        void Delete(Inv_ItemNItemCategoryWithSerialNoMaster objItemSerialMaster, Inv_ItemStockWithSerialNoByLocation objItemSerials);

        List<Inv_ItemTransactionTypes> ReadItemTransactionReceivedTypes(string itemType, byte locationType, string roleOrGroupID);
        List<Inv_ItemTransactionTypes> ReadItemTransactionIssuedTypes(string itemType, byte locationType, string roleOrGroupID);
        List<Inv_ItemTransactionTypes> ReadItemTransactionIssuedReceivedTypes(string itemType, string issueOrReceivedType, string roleOrGroupID);

        List<Inv_MRRMaster> ReadMRRMaster();
        Inv_MRRMaster ReadMRRMasterByChallanSeqNo(string challanSeqNumber, string challanLocationCode);

        Inv_MRRMaster Create(Inv_MRRMaster objMRRMaster, List<Inv_MRRDetails> lstMRRDetails, List<Inv_MRRDetailsWithSerialNo> lstMRRDetailsWithSerialNo);
        Inv_MRRMaster Create(Inv_MRRMaster objMRRMaster, List<Inv_MRRDetails> lstMRRDetails, List<Inv_MRRDetailsWithSerialNo> lstMRRDetailsWithSerialNo, string serialTempTableRows);
        Inv_MRRMaster Create(Inv_MRRMaster objMRRMaster, List<Inv_MRRDetails> lstMRRDetails, List<Inv_MRRDetailsWithSerialNo> lstMRRDetailsWithSerialNo, string serialTempTableRows, Aud_AuditAdjustmentRelatedChallanOrMRRForReference objChallanOrMRRForAuditAdjustment);

        List<Inv_ItemMaster> ReadInvItems(string itemCategory);
        List<Inv_ItemMaster> ReadInvItems(string itemCategory, string itemTransType);

        List<CustomerLedgerReport> ReadCustomerLedgerReport(string customerCode);

        Inv_ItemStockByLocation ReadItemStockByLocation(byte storeLocation, string locationCode, string itemCode);

        Inv_ChallanMaster ReadChallanMasterByChallanSequence(string challanSequenceNumber, string locationCode);
        Inv_ChallanMaster ReadChallanMaster(string locationCode, string challanSequenceNumber);
        List<Inv_ChallanMaster> ReadChallanMaster(string locationCode);
        List<ChallanInboxForChallanWithMRR> ReadChallanInbox(string locationCode, string itemType);

        bool IsItemTransationRelatedToVendor(string itemTransactionType);

        string ChallanSequenceNumberMax(string locationCode, string yearMonthDate);
        string SparseChallanSequenceNumberMax(string locationCode, string yearMonthDate);
        Inv_ChallanMaster Create(Inv_ChallanMaster objChallanMaster, List<Inv_ChallanDetails> lstChallanDetails, List<Inv_ChallanDetailsWithSerialNo> lstChallanDetailsWithSerialNo);
        Sal_SparePartsSalesMaster Create(Inv_ChallanMaster objChallanMaster, List<Inv_ChallanDetails> lstChallanDetails, List<Inv_ChallanDetailsWithSerialNo> lstChallanDetailsWithSerialNo, Sal_SparePartsSalesMaster objSparePartsSalesMaster, List<Sal_SparePartsSalesItems> lstSparePartsSalesItems, List<Sal_SparePartsSalesItemsWithSerialNo> lstSparePartsSalesItemsWithSerialNo);
        Inv_ChallanMaster Create(Inv_ChallanMaster objChallanMaster, List<Inv_ChallanDetails> lstChallanDetails, List<Inv_ChallanDetailsWithSerialNo> lstChallanDetailsWithSerialNo, List<Inv_ChallanWithMultipleLocations> lstChallanWithMultipleLocations, List<Inv_ChallanWithMultipleLocationsWithItemWiseDistribution> lstChallanItemDistribution);

        string MRRSequenceNumberMax(string locationCode, string yearMonthDate);
        List<MRRDetails> ReadMRRDetails(string locationCode, string mrrLocationCode, string challanSequenceNumber);

        Int16 ItemTransactionSequenceMAX(byte storeLocation, string yearMonth, string locationCode, string itemCode, DateTime transDate);

        List<SalGetAvailableSerialNoForMRR> ReadChallanDetailsWithSerialNo(string challanSequenceNumber, string itemCode, string refDocType, string challanLocationCode);
        Inv_ChallanDetails ReadChallanDetails(string locationCode, string challanSequenceNumber, string itemCode);

        List<GetCustomerTransferredButNotYetAccepted> ReadCustomerTransferredButNotYetAccepted(string locationCode);

        List<LocationWiseEmployeeTargetEntryCheck> ReadLocationWiseEmployeeTargetEntryCheck(string locatioCode, string YearMonth);

        bool IsInventoryStockUpdateFinish(string locationCode);

        List<PackageDetailsForSystemReturn> ReadPackageDetailsForSystemReturn(string customerCode, DateTime returnDate);

        Sal_SystemReturn Create(Sal_SystemReturn objSystemReturn, List<Sal_SystemReturnItems> lstSystemReturnItems, List<Sal_SystemReturnItemsWithSerialNo> lstSystemReturnItemsWithSerialNo, string locationCode, string serialTempTableRows);
        List<PanelSerialList> PanelSerialByLocationAndStock(string locationCode, byte storeLocation, string itemCategory, string itemCapacity, byte agreementType, string packageCode);

        DepretiatedPackagePriceBySRPanelSerial GetDepretiatedPackagePriceBySRPanelSerial(string panelSerial, string packageCode);

        List<CollectionEfficiencyByCustomer> ReadCollectionEfficiencyByCustomer(string locationCode, string yearMonth);
        List<ProgressReview> ReadProgressReviewGraph();

        List<LocationInfo> Location();
        List<LocationInfo> LocationWithHeadOffice();
        LocationInfo Location(string LocationCode);
        List<LocationInfo> LocationByLocationCode(string LocationCode);

        Sal_CustomerStatus UpdateFPR(List<CustomerFPRAndScheduledCollectionEntry> lstCustomerFPREntry, string unitCode);
        List<CustomerFPRAndScheduledCollectionEntry> GetCustomerFPRAndScheduledCollectionEntry(string unitCode, string optionForMissingFPROrDay, string prmEMP_ID, string scheduledCollectionDay);
        List<CustomerFPRNDayWiseRegularOrODTarget> GetCustomerFPRNDayWiseRegularOrODTarget(string unitCode, string employeeID);

        List<LocationNEmployeeWiseDailySalesNCollectionTarget> ReadLocationNEmployeeWiseDailySalesNCollectionTarget(string locationCode);

        bool Update(List<Sal_LocationNEmployeeWiseDailySalesNCollectionTarget> lstEmployeeDailyTarget);

        List<SalSalesNCollectionTargetVsAchievementForGraph> ReadSalSalesNCollectionTargetVsAchievementForGraph(string reportType, string locationCode, string employeeID);

        List<ItemLedgerReport> ReadInvItemLedger(byte storeLocation, string locationCode, string itemCode, string fromDate, string toDate);

        List<Sal_CustomerFuelUsed> ReadCustomerFuelUsed();
        List<Sal_CustomerIncomeRange> ReadCustomerIncomeRange();
        List<Sal_CustomerOccupations> ReadCustomerOccupations();
        List<Sal_CustomerRelations> ReadCustomerRelations();
        List<InvChallanOrMRRDetailsForItemLedger> ReadInvChallanDetails(string locationCode, string challanOrMRRSeqNo, string challanOrMRR);

        List<Sal_ListedUnitPriceForSparePartsSales> ReadListedUnitPriceForSparePartsSales();
        Sal_ListedUnitPriceForSparePartsSales ReadListedUnitPriceForSparePartsSales(string yearMonth, string itemCode, byte stockLocation);

        List<InventorySummaryReportV2> ReadInventorySummaryReportV2(string reportType, string itemType, byte storeLocation, string locationCode, DateTime startDate, DateTime endDate, string vendorType);
        List<InventoryERPVersusPhysicalBalance> ReadInventoryERPVersusPhysicalBalance(string reportType, string itemType, byte storeLocation, string locationCode, string yearMonth, string vendorType);
        Inv_ERPVersusPhysicalBalance Update(Inv_ERPVersusPhysicalBalance objInvERPVersusPhysicalBalance, List<Inv_ERPVersusPhysicalBalance> lstInvERPVersusPhysicalBalance);

        string ReadCustomerTraineeName(string trainerId);
        List<CustomerTrainingInfo> ReadCustomerTrainingInfo(string unitCode, bool trainingStatus, byte trainingBatchNo);
        List<GetUnitWiseCustomerTrainingSchedule> ReadGetUnitWiseCustomerTrainingSchedule(string unitCode, DateTime? trainingDate, byte? trainingBatchNumber);
        Sal_CustomerTrainingTransMaster Create(Sal_CustomerTrainingTransMaster objSalCustomerTrainingMaster, List<Sal_CustomerTrainingTransDetails> lstCustomerTrainingTransDetails);

        List<Sal_ItemSetMaster> ReadItemSetMaster();
        Sal_ItemSetMaster ReadItemSetMaster(string itemSetCode);
        List<Sal_ItemSetDetail> ReadItemSetDetails(string itemSetCode);
        List<SparePartSetDetils> ReadSparePartSetDetils(string itemSetCode);

        string ChallanOrMRRForAuditAdjustmentSequenceMax(string locationCode, string yearMonthDate);
        Inv_ChallanMaster Create(Inv_ChallanMaster objChallanMaster, List<Inv_ChallanDetails> lstChallanDetails, List<Inv_ChallanDetailsWithSerialNo> lstChallanDetailsWithSerialNo, Aud_AuditAdjustmentRelatedChallanOrMRRForReference objChallanOrMRRForAuditAdjustment);

        List<SummarySheetForRegionalSalesPosting> ReadSummarySheetForRegionalSalesPosting(DateTime dateFrom, DateTime dateTo, string regionCode);

        List<FixedAssetSerialList> GetFixedAssetSerialList(byte storeLocation, string locationCode, string itemCode, string option);
        List<AssetAssign> FixedAssetAssignUnassign(string locationCode, string itemCode, byte storeLocation, string employeeId, byte status, string unAssignedQuantity);
        string EmployeeWiseFixedAssetsAllocationSequenceNumberMax(string locationCode, string employeeId, string itemCode, byte storeLocation);
        ArrayList GetFixedAssetAssignedOrUnassignedSeialList(byte storeLocation, string locationCode, string itemCode, string employeeId, int assignQuantity);
        Fix_EmployeeWiseFixedAssetsAllocation Create(Fix_EmployeeWiseFixedAssetsAllocation objAssetAssign, List<Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo> lstAssetsAllocationWithSerialNo);
        List<AvailableNAssignFixedAsset> ReadAvailableNAssignFixedAsset(byte storeLocation, string locationCode, string itemCategory, string employeeSelectionOption, string employeeID);

        List<ChallanOrMRRItemsSerial> ReadChallanOrMRRItemsSerial(string locationCode, string challanSequenceNumber, string mrrSequenceNumber, string itemCode);
        ArrayList ReadVendorChallanWithItemReferenceToCheck(string deliveryNoteNo);
        Array MrrSequenceListForMaterialReceiving(string rreDeliveryNote, string locationCode);
        List<MaterialRecevingReportMrrDetails> MaterialReceivingMrrDetails(string LocationCode, string VendorRefChallanNo, string mrrSequenceNumber, string billNo);
        
        List<DeliveryNoteSummary> ReadDeliveryNoteSummary(string scheduleNo);

        List<DeliveryItemNoteReportSummary> ReadDeliveryItemNoteReportSummary(string scheduleNo);

        List<DeliveryPackageNoteReportSummary> ReadDeliveryPackageNoteReportSummary(string scheduleNo);
        //start monthly
        List<DeliveryNoteSummary> ReadMonthlyDeliveryNoteSummary(string monthYear);

        List<DeliveryItemNoteReportSummary> ReadMonthlyDeliveryItemNoteReportSummary(string monthYear);

        List<DeliveryPackageNoteReportSummary> ReadMonthlyDeliveryPackageNoteReportSummary(string monthYear); 
        //end monthly

        //start date wise
        List<DeliveryNoteSummary> ReadDateWiseDeliveryNoteSummary(string fromDate, string toDate);

        List<DeliveryItemNoteReportSummary> ReadDateWiseDeliveryItemNoteReportSummary(string fromDate, string toDate);

        List<DeliveryPackageNoteReportSummary> ReadDateWiseDeliveryPackageNoteReportSummary(string fromDate, string toDate); 
        //end date wise


        List<CustomerDisasterRecoveryDetails> ReadCustomerDisasterRecoveryList(string collectionType, byte customerStatus, DateTime collectionDate, string locationCode);
        decimal ReadCustomerDetailsForDRFACollection(string collectionType, string customerCode, string locationCode, DateTime collectionDate, byte customerStatus);

        ArrayList ReadMRRNDeliveryNoteValue(string locationCode, string mrrSequenceNumber, string yearMonth, string rreDeliveryNote);

        List<ChallanInformationGlanceDetails> ReadChallanInformationGlanceList(string itemType, string locationCode, DateTime dateFrom, DateTime dateTo);
        List<MrrInformationGlanceDetails> ReadMrrInformationGlanceList(string itemType, string locationCode, DateTime dateFrom, DateTime dateTo);
        List<StockInTransitAtGlanceDetails> ReadStockInTransitAtGlanceList(string challanType, string locationCode, DateTime dateFrom, DateTime dateTo);
        ArrayList ReadVendorRefChallanNo(string billNO);

        //List<InvItemInTransit> ReadInvItemInTransit(string itemType, string locationCode, string itemCode, DateTime fromDate, DateTime toDate);
        List<StockInTransitAtGlanceDetails> ReadInvItemInTransit(string itemType, string locationCode, string itemCode, DateTime fromDate, DateTime toDate);

        InventoryInTransitBalance ReadInventoryInTransitBalance(string options, string yearMonth);

        List<UnitWiseCustomerLedger> ReadUnitWiseCustomerLedger(string reportOption, string locationCode, string dateFrom, string dateTo);
        string AuditSequenceNumberMax(string locationCode, string yearMonthDate);

        CustomerNAgreementNItemDetails ReadCustomerNAgreementItemDetails(string customerCode);
        Aud_AuditAdjustmentRelatedCollectionFromCustomers SaveCustomerCollectionForAudit(Sal_CollectionFromCustomers objCustomerCollection, Aud_AuditAdjustmentRelatedCollectionFromCustomers objAuditAdjustmentCustomerCollection, string cashMemoUsesId);
        Sal_CollectionFromCustomers Update(Sal_CollectionFromCustomers objCollectionFromCustomers, Aud_AuditAdjustmentRelatedCollectionFromCustomers objCollectionAuditAdjustnment);
        List<CustomerCollectionAdjustmentForAudit> ReadCustomerCollectionAdjustmentForAudit(string customerCode, string yearMonth, string auditSeqNo);
        Aud_AuditAdjustmentObservationOnSalesAgreement Create(Aud_AuditAdjustmentObservationOnSalesAgreement objAuditAdjustmentObservationOnSalesAgreement, Aud_AuditAdjustmentObservationOnSalesAgreement objPreviousDataAuditAdjustmentObservationOnSalesAgreement);
        bool IsAuditAdjustmentObservationOnSalesAgreementExistOrNot(string locationCode, string yearMonth, string customerCode, string auditSeqNo);
        bool IsAuditAdjustmentObservationOnSalesAgreementExistOrNot(string locationCode , string customerCode);
        Aud_AuditAdjustmentObservationOnSalesAgreement Update(Aud_AuditAdjustmentObservationOnSalesAgreement objAuditOnSalesAgreement);

        Common_Sys_SystemSettings ReadSystemSettings(string companyName);
        bool IsCashMemoManagementEnabled(string companyName);

        List<AvailableNAssignFixedAsset> ReadAvailableNAssignCashMemo(byte storeLocation, string locationCode, string itemCategory, string employeeSelectionOption, string employeeID);
        List<AssignCashMemoBook> CashMemoAssignUnassign(string locationCode, string itemCode, byte storeLocation, string employeeId, byte status, string unAssignedQuantity);
        ArrayList GetCashMemoAssignedOrUnassignedSeialList(byte storeLocation, string locationCode, string itemCode, string employeeId, int assignQuantity);
        
        Fix_EmployeeWiseFixedAssetsAllocation CreateForCashMemo(Fix_EmployeeWiseFixedAssetsAllocation objAssetAssign, List<Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo> lstAssetsAllocationWithSerialNo);
        Fix_EmployeeWiseFixedAssetsAllocation UpdateCashmemoAssign(Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo objEmployeeWiseFixedAssetsAllocationWithSerialNo, Fix_EmployeeWiseFixedAssetsAllocation objAssetAssign);

        string GetCashMemoInUsesId(string entrySource, string cashMemoNo, string cashMemoUsesId, string locationCode, string refDocNo);
        bool IsMemoStillAvailableToUse(string employeeId);

        CustomerDataToCloseWithFullPaidOrWaive getCustomerDataToCloseWithFullPaidOrWaive(string select, string customerCode, string currentMonth, string locationCode);
        CustomerDataToPersonalInformation GetCustomerDataPersonalInformation(string customerCode);
        Sal_SalesAgreementClosedWithFullPaidOrWaive SaveFullPaiedCustomer(Sal_SalesAgreementClosedWithFullPaidOrWaive objSalesAgreementClosedWithFullPaidOrWaive, string closedIn, bool? approvalRequiredForFullPayed);

        List<UnitWiseCashMemoInformation> ReadCashMemoInformation(string locationCode, string dateFrom, string dateTo);

        List<ProjectInfo> ReadProjectForAuditor(string programCode);
        List<ItemCapacity> ReadPackageOrItemCapacityForAuditor(string projectCode, string isForItemOrPackage);
        List<LightInfo> ReadLightForAuditor(string capacityID);
        List<PackageInformation> ReadPackagesForAuditor(string capacityId, string lightId);
        List<ItemInfo> ReadInvItemsForAuditors(string itemType, string itemCategory);
        List<ItemInfo> ReadInvItemsForAuditors(string itemType, string itemCategory, string itemCapacity);
        List<ItemSerialInfo> ReadItemStockWithSerialNoByLocationForAuditor(byte storeLocation, string locationCode, string itemCode);
        List<ServiceChargeInformation> ReadServiceChargePolicyForAuditor(string customerType, string modeOfPayment);
        List<DownPaymentPolicy> ReadDownPaymentPolicyForAuditor(string modeOfPayment, string packageCode);
        DiscountPolicy ReadDiscountPolicyByModeofPaymentNDiscountIdForAuditor(string modeOfPayment, string discountId);
        DiscountPolicy ReadDiscountPolicyByModeofPaymentNPackageIdForAuditor(string modeOfPayment, string packageCode);
        List<ODRecoveryStatusMonitoring> ReadODRecoveryStatusMonitoring(string locationCode, bool IsOnlyForCollectionDatePassed, string customerGrading, string CustomerFPR, byte? scheduledCollectionDay);

        Sal_ODCustomerGrading UpdateODRecoveryStatusMonitoring(Sal_ODCustomerGrading objODCustomerGrading);
        List<SalesRecoveryCommitmentByReview> ReadSalesRecoveryCommitmentByReview(string reportType, string locationCode);

        Sal_ODRecoveryCommitmentByRMnZM UpdateODRecoveryCommitmentByRMnZM(Sal_ODRecoveryCommitmentByRMnZM objODRecoveryCommitmentByRMnZM);

        List<SalesRecoveryStatusEntryMonitoring> ReadSalesRecoveryStatusEntryMonitoring(string locationCode, bool IsOnlyForCollectionDatePassed, string customerGrading, string CustomerFPR, byte? scheduledCollectionDay);
        Sal_DailyBusinessPerformanceMonitoring_SalesAndRecoveryStatusByUnit ReadDailyBusinessPerformanceMonitoringRemarks(string yearMonth, string locationCode);
        List<SalesRecoveryStatusEntryMonitoring> UpdateSalesRecoveryStatusEntryMonitoring(string option, string locationCode, string customerCode, DateTime? umLastNextRecoveryDate, string umLastRemarks, string umLastOverallRemarks, string amLastRemarks);

        void CustomerCollectionSaveForUpdateOrDelete(string updateDeleteOptions, Sal_CollectionFromCustomers objCollectionFromCustomers);

        void CreateUserLog(string ipAddress, string macAddress, string locationCode, string userId, string referenceEntrySource);

        byte getCustomerCollectionEntrySerialNumber(string customerCode, string collectionDate);
        List<Inv_VendorInfo> GetVendorListForItemSummary();
        ArrayList GetVendorList(string itemTransactionType);

        List<FixedAssetSerialList> GetCashMemoSerialList(byte storeLocation, string locationCode, string itemCode, string option);

        void CustomerCollectionDelete(Sal_CollectionFromCustomersPrePost objCollectionFromCustomers);

        List<CollectionSheetForCustomerFPR> ReadCollectionSheetForCustomerFPR(string customerFPR, string locationCode);
        List<CashMemoBookPagesStatus> GetStatusForCashMemoBookAllocation(string itemSerialNo);

        ArrayList ReadSpecialPackageListForSales(string packageCapacityId, string lightId, string programCode, string salesReSalesOrBoth);
        PackagePricingDetailsForSalesAgreement ReadPackagePricingDetailsForSalesAgreement(string locationCode, string programCode, string salesReSalesOrBoth, string customerType, string packageCapacity, string lightID, string packageCode, string modeOfPaymentID, string changedDownPaymentAmount);
        ArrayList ReadModeOfPaymentForSpecialPackageSales(string salesReSalesOrBoth);
        Sal_CustomerStatus UpdateCustomerStatusForSalesMonitoring(Sal_CustomerStatus objCustomerStatus);
        CustomerDetails GetCustomerDetailsForWarrantyClaim(string customerCode, string unitCode);
        List<WarrantyItemsDetails> GetWarrantyItemsList(string customerCode, DateTime dayOpenDate);
        Inv_MRRMaster SaveWarrantyClaimNSettlement(Inv_MRRMaster objMRRMaster, List<Inv_MRRDetails> lstMRRDetails, List<Inv_MRRDetailsWithSerialNo> lstMRRDetailsWithSerialNo, Inv_ChallanMaster objChallanMaster, List<Inv_ChallanDetails> lstChallanDetails, List<Inv_ChallanDetailsWithSerialNo> lstChallanDetailsWithSerialNo);
        string ItemSerialCorrectionChangeNClear(string locationCode, string itemCode, string optionForCorrection, string wrongSerialNo, string correctSerialNo, string loginID);
        List<DailyPerformanceMonitoringForSales> ReadDailyPerformanceMonitoringForSales(string reportOption, string locationCode);
        List<DailyBusinessPerformanceMonitoringCollection> ReadDailyBusinessPerformanceMonitoringCollection(string reportOption, string locationCode);
        List<ItemSerialCorrection> ReadItemSerialCorrectionInformation(string locationCode, string itemCode, string optionForCorrection, string wrongSerialNo, string correctSerialNo, string loginID);
        List<DailyBusinessPerformanceMonitoringNetODIncreaseOrDecreaseComperison> ReadDailyBusinessPerformanceMonitoringIncreaseOrDecreaseComperison(string reportOption, string locationCode);
        List<DailyBusinessPerformanceMonitoringSalesNCollectionTargetVsAchievement> ReadDailyBusinessPerformanceMonitoringSalesNCollectionTargetVsAchievement(string reportOption, string locationCode, DateTime dateForTheStatus);
        List<DailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus> ReadDailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus(string reportOption, string locationCode);
        List<DailyBusinessPerformanceMonitoringOtherStatus> ReadDailyBusinessPerformanceMonitoringOtherStatus(string reportOption, string locationCode);
        List<WarrantyClaimReason> GetWarrantyClaimReason();
        string CheckingIssueItemSerailNO(string locationCode, string itemSerial, string itemCode, string storeLocationForIssue);
        List<ItemInfo> GetSubstituteItemCode(string itemCode);

        List<Sal_SparePartsSales_DataFromExternalSources> ReadSalesExternalSourceByLCode(string locationCode);

        List<Sal_SalesAgreementPrePost_DataFromExternalSources> ReadSalesResalesAgrExternalSourceByLCode(string locationCode);
        
    }
}
