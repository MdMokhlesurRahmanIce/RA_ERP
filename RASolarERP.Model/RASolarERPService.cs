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
    public class RASolarERPService
    {
        #region Properties And Constructor

        private IRASolarERPRepository raSolarERPRepository;

        public RASolarERPService()
        {
            this.raSolarERPRepository = new RASolarERPRepository(new RASolarMISEntities());
        }

        public RASolarERPService(IRASolarERPRepository raSolarRepository)
        {
            this.raSolarERPRepository = raSolarRepository;
        }
        #endregion

        #region Read Methods

        public List<Common_ZoneInfo> Zone()
        {
            return ProcessTableData.ZoneProcess(raSolarERPRepository.Zone().ToList());
        }
        public Common_ZoneInfo Zone(string zoneCode)
        {
            return raSolarERPRepository.Zone(zoneCode);
        }

        public List<Common_RegionInfo> Region()
        {
            return raSolarERPRepository.Region().ToList();
        }
        public Common_RegionInfo Region(string regionCode)
        {
            return raSolarERPRepository.Region(regionCode);
        }
        public List<Common_RegionInfo> RegionByZoneCode(string zoneCode)
        {
            return ProcessTableData.RegionProcess(raSolarERPRepository.RegionByZoneCode(zoneCode).ToList());
        }

        public List<Common_UnitInfo> Unit()
        {
            return raSolarERPRepository.Unit().ToList();
        }
        public Common_UnitInfo Unit(string unitCode)
        {
            return raSolarERPRepository.Unit(unitCode);
        }
        public List<Common_UnitInfo> UnitByRegionCode(string regionCode)
        {
            return ProcessTableData.UnitProcess(raSolarERPRepository.UnitByRegionCode(regionCode).ToList());
        }

        public List<Common_UnionInfo> ReadUnionInfo(string upazilaCode)
        {
            return raSolarERPRepository.ReadUnionInfo(upazilaCode);
        }


        public List<Common_PostOfficeInfo> ReadPostOfficeInfo(string upazilaCode)
        {
            return raSolarERPRepository.ReadPostOfficeInfo(upazilaCode); 
        }

        public List<Common_PostOfficeInfo> PostOfficeInfoLoadForUnitCode(string unitCode)
        {
            return raSolarERPRepository.PostOfficeInfoLoadForUnitCode(unitCode); 
        }
        //ReadCustomerPostOfficeInfo
        public List<Common_PostOfficeInfo> ReadCustomerPostOfficeInfo(string CustomerCode, string PostOffice)
        {
            return raSolarERPRepository.ReadCustomerPostOfficeInfo(CustomerCode, PostOffice); 
        }

        public List<Common_DistrictInfo> ReadDistrictInfo(string upazilaCode)
        {
            return raSolarERPRepository.ReadDistrictInfo(upazilaCode);
        }

        public int LastUsedCustomerSerial(string unitCode, string programCode)
        {
            return raSolarERPRepository.LastUsedCustomerSerial(unitCode, programCode);
        }

        public List<ClosingInventoryValuation> ClosingInventoryReport(string yearMonth)
        {
            return raSolarERPRepository.ClosingInventoryReport(yearMonth);
        }

        public int CheckDuplicateDistributionPlan(string distribScheduleNo, DateTime scheduleDate, string refScheduleNo)
        {
            return raSolarERPRepository.CheckDuplicateDistributionPlan(distribScheduleNo, scheduleDate, refScheduleNo);
        }

        public List<SHSDistributionPlanPackageORItem> RootWiseLocationNPackage(string distributionScheduleNo)
        {
            return raSolarERPRepository.RootWiseLocationNPackage(distributionScheduleNo);
        }

        public List<SHSDelivaryNoteChallan> ViewDeliveryNoteChallanSHSReport(string distribScheduleNo)
        {
            return raSolarERPRepository.ViewDeliveryNoteChallanSHSReport(distribScheduleNo);
        }


        public List<DailyZonalPerformanceMonitoring> DailyPerformanceMonitoringZonalReport()
        {
            return  raSolarERPRepository.DailyPerformanceMonitoringZonalReport();
        }

        public List<InventoryAtVendorValuationByStockLocation> InventoryAtVendorValuationByStockLocation(string yearMonth)
        {
            return raSolarERPRepository.InventoryAtVendorValuationByStockLocation(yearMonth);
        }

        public List<SalesDataEntryStatus> SalesEntryStatus(string reportType, string locationType, string yearMonth)
        {
            return raSolarERPRepository.SalesEntryStatus(reportType, locationType, yearMonth);
        }

        //public Common_UnitWiseEntryStatus UnitWiseEntryStatus(string unitCode, string YearMonth)
        //{
        //    return raSolarERPRepository.UnitWiseEntryStatus(unitCode, YearMonth);
        //}

        //public List<Common_UnitWiseCustomerStatus> UnitWiseCustomerStatus(string unitCode)
        //{
        //    return raSolarERPRepository.UnitWiseCustomerStatus(unitCode).ToList();
        //}
        //public Common_UnitWiseCustomerStatus ReadUnitWiseCustomerStatus(string unitCode, string customerId)
        //{
        //    return raSolarERPRepository.ReadUnitWiseCustomerStatus(unitCode, customerId);
        //}

        //public Common_UnitWiseCustomerStatus ReadUnitWiseCustomerStatus(string customerId)
        //{
        //    return raSolarERPRepository.ReadUnitWiseCustomerStatus(customerId);
        //}

        public List<InventoryDataEntryStatus> InventoryDataEntryState(string reportType, string locationType, string yearMonth)
        {
            return raSolarERPRepository.InventoryDataEntryState(reportType, locationType, yearMonth);
        }

        //public List<InventoryAuditAdjustment> InventoryAuditAdjustmentAdjust(string unitCode, string yearMonth, string yearMonthWrite, byte componentStatus, int stockLocation)
        //{
        //    return raSolarERPRepository.InventoryAuditAdjustmentAdjust(unitCode, yearMonth, yearMonthWrite, componentStatus, stockLocation).ToList();
        //}

        public List<Aud_AdjustmentReasonCodes> AuditAdjustMentReasons(string reasonPurposeID, string reasonForUserOrAuditor, string reasonForModule)
        {
            return raSolarERPRepository.AuditAdjustMentReasons(reasonPurposeID, reasonForUserOrAuditor, reasonForModule);
        }

        public List<Aud_AdjustmentReasonCodes> CollectionAdjustMentReasons(string reasonPurposeID1, string reasonPurposeID2, string reasonForUserOrAuditor, string reasonForModule)
        {
            return raSolarERPRepository.CollectionAdjustMentReasons(reasonPurposeID1, reasonPurposeID2, reasonForUserOrAuditor, reasonForModule);
        }

        //public List<Aud_AdjustmentReasonCodes> AuditAdjustMentReasonsForSales(string reasonPurposeID)
        //{
        //    return raSolarERPRepository.AuditAdjustMentReasonsForSales(reasonPurposeID);
        //}

        //public List<Aud_AdjustmentReasonCodes> AuditAdjustMentReasons(string reasonForUserOrAuditor, byte reasonForInventoryStock, string reasonForModule)
        //{
        //    return raSolarERPRepository.AuditAdjustMentReasons(reasonForUserOrAuditor, reasonForInventoryStock, reasonForModule).ToList();
        //}

        //public string AuditAdjustmentOpenMonth(string unitCode)
        //{
        //    return raSolarERPRepository.AuditAdjustmentOpenMonth(unitCode);
        //}

        //public Aud_AuditingMaster AuditingMasterByLocationCode(string locationCode)
        //{
        //    return raSolarERPRepository.AuditingMasterByLocationCode(locationCode);
        //}

        //public Common_InventoryTransaction ReadInventoryTransaction(string unitCode, string itemCode, string yearMonth, int stockLocation)
        //{
        //    return raSolarERPRepository.ReadInventoryTransaction(unitCode, itemCode, yearMonth, stockLocation);
        //}

        //public Common_InventoryTransaction Update(Common_InventoryTransaction objInventoryTranscation)
        //{
        //    return raSolarERPRepository.Update(objInventoryTranscation);
        //}

        public List<Common_Sys_StatusInfo> CustomerStatus()
        {
            return raSolarERPRepository.CustomerStatus().ToList();
        }

        //public List<Common_DailyProgressReport> ReadDailyProgressReport()
        //{
        //    return raSolarERPRepository.ReadDailyProgressReport().ToList();
        //}

        //public List<CustomerStatusListV2> ReadCustomerStatusListV2(string unitCode, byte customerStatus, byte paymentStatus, DateTime currentCollectionDate)
        //{
        //    return raSolarERPRepository.ReadCustomerStatusListV2(unitCode, customerStatus, paymentStatus, currentCollectionDate);
        //}

        //public List<GetCustomerListWithRecoveryStatus> ReadCustomerListWithRecoveryStatus(string unitCode, byte customerStatus, DateTime currentCollectionDate)
        //{
        //    return raSolarERPRepository.ReadCustomerListWithRecoveryStatus(unitCode, customerStatus, currentCollectionDate);
        //}

        //public List<Common_InvStockInTransitByMonth> ReadStockInTransitValue(string YearMonth)
        //{
        //    return raSolarERPRepository.ReadStockInTransitValue(YearMonth);
        //}

        public List<InventorySummaryToDetailViewReport> ReadInventorySummaryToDetailViewReport(string yearMonth, string itemCode)
        {
            return raSolarERPRepository.ReadInventorySummaryToDetailViewReport(yearMonth, itemCode);
        }

        public List<SalesSummaryToDetailView> ReadSalesSummaryToDetailView(string yearMonth)
        {
            return raSolarERPRepository.ReadSalesSummaryToDetailView(yearMonth);
        }

        public List<CustomerTrainingSummary> ReadCustomerTrainingSummary(DateTime dtFromDate, DateTime dtToDate)
        {
            return raSolarERPRepository.ReadCustomerTrainingSummary(dtFromDate, dtToDate);
        }

        public List<CustomerTrainingDetails> ReadCustomerTrainingDetails(string dtFromDate, string dtToDate)
        {

            return raSolarERPRepository.ReadCustomerTrainingDetails(dtFromDate, dtToDate);
        }

        public Sal_LocationNEmployeeWiseActivityMonthly ReadLocationNEmployeeWiseActivityMonthly(string locationCode, string employeeID, string yearMonth)
        {
            return raSolarERPRepository.ReadLocationNEmployeeWiseActivityMonthly(locationCode, employeeID, yearMonth);
        }

        public GetLocationNEmployeeWiseDailyEntry ReadLocationNEmployeeWiseActivity(string locationCode, string employeeID, string yearMonth, DateTime transDate)
        {
            return raSolarERPRepository.ReadLocationNEmployeeWiseActivity(locationCode, employeeID, yearMonth, transDate);
        }

        public Sal_LocationNEmployeeWiseActivityDaily ReadLocationNEmployeeWiseActivityDaily(string locationCode, string employeeID, string yearMonth, DateTime transDate)
        {
            return raSolarERPRepository.ReadLocationNEmployeeWiseActivityDaily(locationCode, employeeID, yearMonth, transDate);
        }

        public List<Common_ProjectInfo> ReadProject(string programCode)
        {
            return raSolarERPRepository.ReadProject(programCode);
        }

        public List<Common_ProgramInfo> ReadProgram()
        {
            return raSolarERPRepository.ReadProgram();
        }

        //public List<Common_Package> ReadPackage(string programCode, string projectCode)
        //{
        //    return raSolarERPRepository.ReadPackage(programCode, projectCode);
        //}

        //public Common_Package ReadPackage(string packageCode, string programCode, string projectCode)
        //{
        //    return raSolarERPRepository.ReadPackage(packageCode, programCode, projectCode);
        //}

        public Common_PeriodOpenClose ReadPeriodOpenClose(string locationCode)
        {
            return raSolarERPRepository.ReadPeriodOpenClose(locationCode);
        }
        
        public List<EmployeeVisit> ReadEmployeeDetailsVisit(string option, string empID, string locationCode, string ddlLocationPart1, string ddlLocationPart4)
        {
            return raSolarERPRepository.ReadEmployeeDetailsVisit(option, empID, locationCode, ddlLocationPart1, ddlLocationPart4); 
        }

        public List<PackageDetails> ReadPackageDetails(string packageCode, string modeOfPaymentID, string customerType)
        {
            return raSolarERPRepository.ReadPackageDetails(packageCode, modeOfPaymentID, customerType);
        }

        //public List<Hrm_LocationWiseEmployee> ReadLocationWiseEmployee(string locationCode)
        //{
        //    return raSolarERPRepository.ReadLocationWiseEmployee(locationCode);
        //}
        public List<PackageDetails> ReadPackageDetailsExtra()
        {
            return raSolarERPRepository.ReadPackageDetailsExtra(); 
        }
        public List<GetLocationWiseEmployee> ReadGetLocationWiseEmployee(string locationCode)
        {
            return raSolarERPRepository.ReadGetLocationWiseEmployee(locationCode);
        }

        public List<GetLocationWiseEmployeeTarget> ReadGetLocationWiseEmployeeTarget(string locationCode, string yearMonth)
        {
            return raSolarERPRepository.ReadGetLocationWiseEmployeeTarget(locationCode, yearMonth);
        }

        public Sal_LocationWiseSalesNCollectionTarget ReadLocationWiseSalesNCollectionTarget(string locationCode, string yearMonth)
        {
            return raSolarERPRepository.ReadLocationWiseSalesNCollectionTarget(locationCode, yearMonth);
        }

        public List<Sal_LocationNEmployeeWiseActivityDaily> ReadLocationNEmployeeWiseActivityDaily(string locationCode, string yearMonth, DateTime transDate)
        {
            return raSolarERPRepository.ReadLocationNEmployeeWiseActivityDaily(locationCode, yearMonth, transDate.Date);
        }

        public List<ProgressReviewDataEntryStatusDaily> ReadProgressReviewDataEntryStatusDaily(string reportType, string locationCode, string yearMonth, string respectiveAreaUserID)
        {
            return raSolarERPRepository.ReadProgressReviewDataEntryStatusDaily(reportType, locationCode, yearMonth, respectiveAreaUserID);
        }

        public List<Sal_LocationNEmployeeWiseActivityMonthly> ReadLocationNEmployeeWiseActivityMonthly(string locationCode, string yearMonth)
        {
            return raSolarERPRepository.ReadLocationNEmployeeWiseActivityMonthly(locationCode, yearMonth);
        }

        //public List<LocationAndEmployeeWiseWeeklySalesAndCollectionEntry> ReadLocationAndEmployeeWiseWeeklySalesAndCollectionEntry(string locationCode, string yearWeek)
        //{
        //    return raSolarERPRepository.ReadLocationAndEmployeeWiseWeeklySalesAndCollectionEntry(locationCode, yearWeek);
        //}

        //public Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement ReadWeeklySalesNCollectionTargetNAchievement(string locationCode, string yearWeek, string eMP_ID, DateTime transDate)
        //{
        //    return raSolarERPRepository.ReadWeeklySalesNCollectionTargetNAchievement(locationCode, yearWeek, eMP_ID, transDate);
        //}

        //public List<Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement> ReadWeeklySalesNCollectionTargetNAchievement(string locationCode, string yearWeek, DateTime transDate)
        //{
        //    return raSolarERPRepository.ReadWeeklySalesNCollectionTargetNAchievement(locationCode, yearWeek, transDate);
        //}

        //public List<LocationAndEmployeeWiseWeeklySalesAndCollectionReport> ReadLocationAndEmployeeWiseWeeklySalesAndCollectionReport(string reportType, string locationCode, string yearWeek)
        //{
        //    return raSolarERPRepository.ReadLocationAndEmployeeWiseWeeklySalesAndCollectionReport(reportType, locationCode, yearWeek);
        //}

        //public Sal_LocationWiseWeeklySalesNCollectionSummary LocationWiseWeeklySalesNCollectionSummary(string locationCode, string yearWeek)
        //{
        //    return raSolarERPRepository.LocationWiseWeeklySalesNCollectionSummary(locationCode, yearWeek);
        //}

        public List<Sal_CustomerWiseOverdueBalanceForTracker> ReadCustomerWiseOverdueBalanceForTracker(String unitCode)
        {
            return raSolarERPRepository.ReadCustomerWiseOverdueBalanceForTracker(unitCode);
        }

        public List<CustomerWiseOverdueBalanceTracker> ReadCustomerWiseOverdueBalanceForTracker(String unitCode, string weekNumber)
        {
            return raSolarERPRepository.ReadCustomerWiseOverdueBalanceForTracker(unitCode, weekNumber);
        }

        public List<WeeklySalesNCollectionStatus> ReadWeeklySalesNCollectionStatus(string reportType, string locationCode, string yearWeek)
        {
            return raSolarERPRepository.ReadWeeklySalesNCollectionStatus(reportType, locationCode, yearWeek);
        }

        

        public Common_CurrentYearMonthNWeek ReadCurrentYearMonthNWeek()
        {
            return raSolarERPRepository.ReadCurrentYearMonthNWeek();
        }

        public List<Sal_Light> ReadLight(string capacityID)
        {
            return raSolarERPRepository.ReadLight(capacityID);
        }


        public List<Sal_PackageMaster> ReadPopUpPackageList(string packageCode)
        {
            return raSolarERPRepository.ReadPopUpPackageList(packageCode); 
        }


        public List<PackageLightCapacityPop> ReadPopUpPackageCapacity(string packageCode)
        {
            return raSolarERPRepository.ReadPopUpPackageCapacity(packageCode);
        }


        public List<Inv_MRRMaster> ReadMRRMaster()
        {
            return raSolarERPRepository.ReadMRRMaster();
        }

        public Inv_MRRMaster ReadMRRMasterByChallanSeqNo(string challanSeqNumber, string challanLocationCode)
        {
            return raSolarERPRepository.ReadMRRMasterByChallanSeqNo(challanSeqNumber, challanLocationCode);
        }

        public List<Sal_PackageOrItemCapacity> ReadPackageOrItemCapacity(string projectCode, string isForItemOrPackage)
        {
            return raSolarERPRepository.ReadPackageOrItemCapacity(projectCode, isForItemOrPackage);
        }

        public List<Sal_PackageMaster> ReadPackages(string capacityId, string lightId, byte salesType)
        {
            return raSolarERPRepository.ReadPackages(capacityId, lightId, salesType);
        }

        public List<Sal_PackageMaster> ReadPackagesPackgeListFrmPopUpTddlPckg(string capacityId, string lightId, byte salesType)
        {
            return raSolarERPRepository.ReadPackagesPackgeListFrmPopUpTddlPckg(capacityId, lightId, salesType); 
        }

        //public Sal_Validation_CustomerTypeNModeOfPaymentWiseServiceChargePolicy ReadServiceChargePolicy(string modeOfPaymentId, string customerType)
        //{
        //    return raSolarERPRepository.ReadServiceChargePolicy(modeOfPaymentId, customerType);
        //}

        //public Sal_Validation_ModeOfPaymentWiseDiscountPolicy ReadDiscountPolicy(string modeOfPaymentId, string discountId)
        //{
        //    return raSolarERPRepository.ReadDiscountPolicy(modeOfPaymentId, discountId);
        //}

        //public Sal_Validation_ModeOfPaymentNPackageWiseDownpaymentPolicy ReadPackageVsDownpayment(string packageCode, string modeOfPaymentId)
        //{
        //    return raSolarERPRepository.ReadPackageVsDownpayment(modeOfPaymentId, packageCode);
        //}

        public ServiceChargeInformation ReadServiceChargePolicy(string programCode, string customerType, string modeOfPayment)
        {
            return raSolarERPRepository.ReadServiceChargePolicy(programCode, customerType, modeOfPayment);
        }

        public DownPaymentPolicy ReadDownPaymentPolicy(string modeOfPayment, string packageCode)
        {
            return raSolarERPRepository.ReadDownPaymentPolicy(modeOfPayment, packageCode);
        }

        public DiscountPolicy ReadDiscountPolicyByModeofPaymentNDiscountId(string modeOfPayment, string discountId)
        {
            return raSolarERPRepository.ReadDiscountPolicyByModeofPaymentNDiscountId(modeOfPayment, discountId);
        }

        public DiscountPolicy ReadDiscountPolicyByModeofPaymentNPackageId(string modeOfPayment, string packageCode)
        {
            return raSolarERPRepository.ReadDiscountPolicyByModeofPaymentNPackageId(modeOfPayment, packageCode);
        }

        public Sal_Customer ReadCustomer(string customerCode)
        {
            return raSolarERPRepository.ReadCustomer(customerCode);
        }

        public Sal_Customer ReadCustomer(string unitCode, string customerCode)
        {
            return raSolarERPRepository.ReadCustomer(unitCode, customerCode);
        }

        public Common_UpazillaInfo ReadUpazillaByID(string upazillaCode)
        {
            return raSolarERPRepository.ReadUpazillaByID(upazillaCode);
        }

        public List<Common_UpazillaInfo> ReadUpazilla(string unitCode)
        {
            return raSolarERPRepository.ReadUpazilla(unitCode);
        }

        public List<Inv_ItemModel> ReadItemModel(string itemCatagory, string itemCapacity, string itemCategoryIdForNull)
        {
            return raSolarERPRepository.ReadItemModel(itemCatagory, itemCapacity, itemCategoryIdForNull);
        }

        public List<Inv_ItemModel> ReadItemModel(string itemCatagory)
        {
            return raSolarERPRepository.ReadItemModel(itemCatagory);
        }

        public List<ItemCapacity> ReadItemCapacity(string itemCategory)
        {
            return raSolarERPRepository.ReadItemCapacity(itemCategory);
        }

        public List<RASolarERP.DomainModel.SalesModel.SystemReturnOrFullPaidCustomers> ReadSystemReturnOrFullPaidCustomers(string CustomerOrViewType, string locationCode, string fromDate, string toDate)
        {
            return raSolarERPRepository.ReadSystemReturnOrFullPaidCustomers(CustomerOrViewType, locationCode, fromDate, toDate);
        }
        //ReadTrackerLocationWiseWeeklyODCollectionTargetVsAchievement

        public List<TrackerLocationWiseWeeklyODCollectionTargetVsAchievement> ReadTrackerLocationWiseWeeklyODCollectionTargetVsAchievement(string prmReportType, string prmYearMonth, string prmYearWeek)
        {
            return raSolarERPRepository.ReadTrackerLocationWiseWeeklyODCollectionTargetVsAchievement(prmReportType, prmYearMonth, prmYearWeek);
        }
        //LoadRequestEntryGet

        public List<LoadRequestEntryGet> ReadLoadRequestEntryGet(string locationCode, string getDate)
        {
            return raSolarERPRepository.ReadLoadRequestEntryGet(locationCode,getDate);
        }

        public List<CheckLoadRequestEntry> firstCheckLoadRequestEntry(string yearMonth, string locationCode, string getDate)
        {
            return raSolarERPRepository.firstCheckLoadRequestEntry(yearMonth, locationCode, getDate); 
        }

        public List<CheckLoadRequestEntryForLocationCode> LocationCheckLoadRequestEntry(string locationCode)
        {
            return raSolarERPRepository.LocationCheckLoadRequestEntry(locationCode); 
        }

        public List<LoadRequestEntryGet> InsertLoadRequest(string locationCode, string getDate)
        {
            return raSolarERPRepository.InsertLoadRequest(locationCode, getDate); 
        }


        public List<UnitWiseCustomerRegisterReport> ReadUnitWiseCustomerRegisterReport(string unitCode)
        {
            return raSolarERPRepository.ReadUnitWiseCustomerRegisterReport(unitCode);
        }

        public List<CustomerCollectionDetails> ReadCollectionFromCustomers(string customerCode, string yearMonth)
        {
            return raSolarERPRepository.ReadCollectionFromCustomers(customerCode, yearMonth);
        }

        public Sal_CollectionFromCustomers ReadCollectionFromCustomers(string customerCode, string yearMonth, byte collectionSerial)
        {
            return raSolarERPRepository.ReadCollectionFromCustomers(customerCode, yearMonth, collectionSerial);
        }

        public Sal_CollectionFromCustomersPrePost ReadCollectionFromCustomersPrepost(string customerCode, string yearMonth, byte collectionSerial)
        {
            return raSolarERPRepository.ReadCollectionFromCustomersPrepost(customerCode, yearMonth, collectionSerial);
        }

        public int CustomerCollectionSerial(string customerCode, string yearMonth)
        {
            return raSolarERPRepository.CustomerCollectionSerial(customerCode, yearMonth);
        }

        public int CustomerCollectionSerial(string customerCode, string yearMonth, string collectionType)
        {
            return raSolarERPRepository.CustomerCollectionSerial(customerCode, yearMonth, collectionType);
        }

        public List<UnitCollectionVsHeadOfficePhysicalCashMovement> ReadUnitCollectionVsHeadOfficePhysicalCashMovement(string reportType, string locationCode, string yearMonth)
        {
            return raSolarERPRepository.ReadUnitCollectionVsHeadOfficePhysicalCashMovement(reportType, locationCode, yearMonth);
        }

        public List<Sal_ModeOfPayment> ReadModeOfPayment(string locationCode, string programCode, string packageCode, string customerType, string agreementDatePicker)
        {
            return raSolarERPRepository.ReadModeOfPayment(locationCode, programCode, packageCode, customerType, agreementDatePicker);
        }
        public List<ItemCatNDescription> ReadItemCatNDescription()
        {
            return raSolarERPRepository.ReadItemCatNDescription(); 
        }

        public List<Inv_ItemMaster> ReadItemMaster()
        {
            return raSolarERPRepository.ReadItemMaster();
        }

        public List<Inv_ItemMaster> ReadItemMasterByItemCategory(string itemCategory)
        {
            return raSolarERPRepository.ReadItemMasterByItemCategory(itemCategory);
        }

        public Inv_ItemMaster ReadItemMaster(string itemCode)
        {
            return raSolarERPRepository.ReadItemMaster(itemCode);
        }

        public List<Inv_ItemMaster> ReadItemMasterForItemCode(string itemCode)
        {
            return raSolarERPRepository.ReadItemMasterForItemCode(itemCode);
        }

        public Common_DistrictInfo ReadDistrict(string districtCode)
        {
            return raSolarERPRepository.ReadDistrict(districtCode);
        }

        public CustomerInfoNPackageDetailsForSystemReturn ReadSystemReturnInfo(string customerCode, DateTime returnDate)
        {
            return raSolarERPRepository.ReadSystemReturnInfo(customerCode, returnDate);
        }

        public List<Sal_CustomerType> ReadCustomerTypes()
        {
            return raSolarERPRepository.ReadCustomerTypes();
        }

        public List<Inv_Sys_ItemType> ReadItemType()
        {
            return raSolarERPRepository.ReadItemType();
        }

        public Inv_ItemCategorySubCategory ReadItemCategorySubCategoryByCategoryID(string ItemCategoryID)
        {
            return raSolarERPRepository.ReadItemCategorySubCategoryByCategoryID(ItemCategoryID);
        }

        public List<Inv_ItemCategorySubCategory> ReadItemCategorySubCatagory(string itemTypeID)
        {
            return raSolarERPRepository.ReadItemCategorySubCatagory(itemTypeID);
        }

        public List<Inv_ItemCategorySubCategory> ReadItemCategorySubCatagoryByTransactionType(string itemTypeID, string transactionType, byte locationType)
        {
            return raSolarERPRepository.ReadItemCategorySubCatagoryByTransactionType(itemTypeID, transactionType, locationType);
        }

        public bool ReadIsItemCodeWiseValidationExist(string itemCategory, string transactionType)
        {
            return raSolarERPRepository.ReadIsItemCodeWiseValidationExist(itemCategory, transactionType);
        }


        public List<Inv_RouteMaster> ReadRootCategory()
        {
            return raSolarERPRepository.ReadRootCategory();
        }
        public List<Inv_RouteMaster> ReadRouteMaster(string routeCategory)
        {
            return raSolarERPRepository.ReadRouteMaster(routeCategory);
        }

        public List<LocationInfo> ReadUnitList(string routeId)
        {
            return raSolarERPRepository.ReadUnitList(routeId);
        }

        //public List<EmployeeVisit> ReadEmployeeDetailsVisit(string empId, string locationCode)
        //{
        //    return raSolarERPRepository.ReadEmployeeDetailsVisit(empId,locationCode);
        //}


        public List<Sal_Validation_CapacityVsLight> ReadPackageCapacity()
        {
            return raSolarERPRepository.ReadPackageCapacity();
        }

        public List<PersonEmployeeDetailsInfo> ReadPersonLocationWiseEmployee(string locationCode)
        {
            return raSolarERPRepository.ReadPersonLocationWiseEmployee(locationCode);
        }

        public List<LocationPart1District> ReadLocationPart1District(string empWisePart1, string empID, string locationCode)
        {
            return raSolarERPRepository.ReadLocationPart1District(empWisePart1, empID, locationCode); 
        }

        public List<LocationPart2Upo> ReadLocationPart2District(string empWisePart1, string empID, string locationCode, string locationPart1)
        {
            return raSolarERPRepository.ReadLocationPart2District(empWisePart1, empID, locationCode, locationPart1);
        }

        public List<Sal_Validation_CapacityVsLight> ReadLightByPCapacityID(string capacityID)
        {
            return raSolarERPRepository.ReadLightByPCapacityID(capacityID);
        }
        public List<Sal_PackageMaster> ReadPackagesForSHSDP(string capacityId, string lightId)
        {
            return raSolarERPRepository.ReadPackagesForSHSDP(capacityId, lightId);
        }

        public List<Inv_ItemModel> ReadPanelModelListForSHSDP(string packageCode)
        {
            return raSolarERPRepository.ReadPanelModelListForSHSDP(packageCode);
        }

        public List<Inv_ItemModel> ReadBatteryModelListForSHSDP(string packageCode)
        {
            return raSolarERPRepository.ReadBatteryModelListForSHSDP(packageCode);
        }



        public List<Inv_StoreLocation> ReadStoreLocation()
        {
            return raSolarERPRepository.ReadStoreLocation();
        }

        public List<Inv_StoreLocation> ReadStoreLocation(string itemType)
        {
            return raSolarERPRepository.ReadStoreLocation(itemType);
        }

        public List<Inv_StoreLocation> ReadStoreLocation(int storeLocationID, string itemTypeId)
        {
            return raSolarERPRepository.ReadStoreLocation(storeLocationID, itemTypeId);
        }

        public List<Inv_StoreLocation> ReadStoreLocationByItemTypeAndTransaction(string itemType, string itemTransType, byte locationType, string location)
        {
            return raSolarERPRepository.ReadStoreLocationByItemTypeAndTransaction(itemType, itemTransType, locationType, location);
        }

        public List<InvAvailableItemInALocation> ReadInvAvailableItemInALocation(string locationCode, byte storeLocation, string itemCategory)
        {
            return raSolarERPRepository.ReadInvAvailableItemInALocation(locationCode, storeLocation, itemCategory);
        }

        //public Inv_ItemSerialNoMaster ReadItemSerialNoMaster(string itemCode, string itemSerial, string itemCategory)
        //{
        //    return raSolarERPRepository.ReadItemSerialNoMaster(itemCode, itemSerial, itemCategory);
        //}

        public Inv_ItemNItemCategoryWithSerialNoMaster ReadItemSerialNoMasterByItemCategoryWise(string itemCode, string itemSerial, string itemCategory)
        {
            return raSolarERPRepository.ReadItemSerialNoMasterByItemCategoryWise(itemCode, itemSerial, itemCategory);
        }

        public List<Inv_ItemStockWithSerialNoByLocation> ReadItemStockWithSerialNoByLocation(byte storeLocation, string locationCode, string itemCode)
        {
            return raSolarERPRepository.ReadItemStockWithSerialNoByLocation(storeLocation, locationCode, itemCode);
        }

        public Inv_ItemStockWithSerialNoByLocation ReadItemStockWithSerialNoByLocation(byte storeLocation, string locationCode, string itemCode, string serialNumber)
        {
            return raSolarERPRepository.ReadItemStockWithSerialNoByLocation(storeLocation, locationCode, itemCode, serialNumber);
        }

        public List<Inv_ItemStockByLocation> Update(List<Inv_ItemStockByLocation> lstItemStockByLocation, List<Inv_ItemStockWithSerialNoByLocation> lstItemStockWithSerialNoByLocation)
        {
            return raSolarERPRepository.Update(lstItemStockByLocation, lstItemStockWithSerialNoByLocation);
        }

        public List<Inv_ItemTransactionTypes> ReadItemTransactionReceivedTypes(string itemType, byte locationType, string roleOrGroupID)
        {
            return raSolarERPRepository.ReadItemTransactionReceivedTypes(itemType, locationType, roleOrGroupID);
        }

        public List<Inv_ItemTransactionTypes> ReadItemTransactionIssuedTypes(string itemType, byte locationType, string roleOrGroupID)
        {
            return raSolarERPRepository.ReadItemTransactionIssuedTypes(itemType, locationType, roleOrGroupID);
        }

        public List<Inv_ItemTransactionTypes> ReadItemTransactionIssuedReceivedTypes(string itemType, string issueOrReceivedType, string roleOrGroupID)
        {
            return raSolarERPRepository.ReadItemTransactionIssuedReceivedTypes(itemType, issueOrReceivedType, roleOrGroupID);
        }

        public List<Inv_ItemMaster> ReadInvItems(string itemCategory)
        {
            return raSolarERPRepository.ReadInvItems(itemCategory);
        }

        public List<Inv_ItemMaster> ReadInvItems(string itemCategory, string itemTransType)
        {
            return raSolarERPRepository.ReadInvItems(itemCategory, itemTransType);
        }

        public List<CustomerLedgerReport> ReadCustomerLedgerReport(string customerCode)
        {
            return raSolarERPRepository.ReadCustomerLedgerReport(customerCode);
        }

        public Inv_ItemStockByLocation ReadItemStockByLocation(byte storeLocation, string locationCode, string itemCode)
        {
            return raSolarERPRepository.ReadItemStockByLocation(storeLocation, locationCode, itemCode);
        }

        public Inv_ChallanMaster ReadChallanMaster(string locationCode, string challanSequenceNumber)
        {
            return raSolarERPRepository.ReadChallanMaster(locationCode, challanSequenceNumber);
        }

        public bool IsItemTransationRelatedToVendor(string itemTransactionType)
        {
            return raSolarERPRepository.IsItemTransationRelatedToVendor(itemTransactionType);
        }

        public List<ChallanInboxForChallanWithMRR> ReadChallanInbox(string locationCode, string itemType)
        {
            return raSolarERPRepository.ReadChallanInbox(locationCode, itemType);
        }

        public Inv_ChallanMaster ReadChallanMasterByChallanSequence(string challanSequenceNumber, string locationCode)
        {
            return raSolarERPRepository.ReadChallanMasterByChallanSequence(challanSequenceNumber, locationCode);
        }

        public List<Inv_ChallanMaster> ReadChallanMaster(string locationCode)
        {
            return raSolarERPRepository.ReadChallanMaster(locationCode);
        }

        public string ChallanSequenceNumberMax(string locationCode, string yearMonthDate)
        {
            return raSolarERPRepository.ChallanSequenceNumberMax(locationCode, yearMonthDate);
        }

        public string SparseChallanSequenceNumberMax(string locationCode, string yearMonthDate)
        {
            return raSolarERPRepository.SparseChallanSequenceNumberMax(locationCode, yearMonthDate);
        }

        public string MRRSequenceNumberMax(string locationCode, string yearMonthDate)
        {
            return raSolarERPRepository.MRRSequenceNumberMax(locationCode, yearMonthDate);
        }

        public List<MRRDetails> ReadMRRDetails(string locationCode, string mrrLocationCode, string challanSequenceNumber)
        {
            return raSolarERPRepository.ReadMRRDetails(locationCode, mrrLocationCode, challanSequenceNumber);
        }

        //public List<REPSummaryForTheDayClosingForSales> ReadREPSummaryForTheDayClosingForSalesData(string reportType, string locationCode)
        //{
        //    return raSolarERPRepository.ReadREPSummaryForTheDayClosingForSalesData(reportType, locationCode);
        //}
        //public List<SummaryForTheDayClosingForCollectionInfo> ReadSummaryForTheDayClosingForCollectionInfo(string reportType, string locationCode)
        //{
        //    return raSolarERPRepository.ReadSummaryForTheDayClosingForCollectionInfo(reportType, locationCode).ToList();
        //}

        //------------------
        public List<REPSummaryForTheDayClosingForSales> ReadREPSummaryForTheDayClosingForSalesData(string reportType, string locationCode)
        {
            return raSolarERPRepository.ReadREPSummaryForTheDayClosingForSalesData(reportType, locationCode).ToList();
        }

        public List<SummaryForTheDayClosingForCollectionInfo> ReadSummaryForTheDayClosingForCollectionInfo(string reportType, string locationCode)
        {
            return raSolarERPRepository.ReadSummaryForTheDayClosingForCollectionInfo(reportType, locationCode).ToList();
        }

        public List<SummaryForTheDayClosingForInventoryInfo> ReadSummaryForTheDayClosingForInventoryInfo(string reportType, string locationCode, byte storeLocation)
        {
            return raSolarERPRepository.ReadSummaryForTheDayClosingForInventoryInfo(reportType, locationCode, storeLocation).ToList();
        }
        public List<SummaryForTheDayClosingForAccountingInfo> ReadSummaryForTheDayClosingForAccountingInfo(string reportType, string locationCode)
        {
            return raSolarERPRepository.ReadSummaryForTheDayClosingForAccountingInfo(reportType, locationCode).ToList();
        }
        //---------------------

        public Int16 ItemTransactionSequenceMAX(byte storeLocation, string yearMonth, string locationCode, string itemCode, DateTime transDate)
        {
            return raSolarERPRepository.ItemTransactionSequenceMAX(storeLocation, yearMonth, locationCode, itemCode, transDate);
        }

        public List<SalGetAvailableSerialNoForMRR> ReadChallanDetailsWithSerialNo(string challanSequenceNumber, string itemCode, string refDocType, string challanLocationCode)
        {
            return raSolarERPRepository.ReadChallanDetailsWithSerialNo(challanSequenceNumber, itemCode, refDocType, challanLocationCode);
        }

        public List<GetCustomerTransferredButNotYetAccepted> ReadCustomerTransferredButNotYetAccepted(string locationCode)
        {
            return raSolarERPRepository.ReadCustomerTransferredButNotYetAccepted(locationCode).ToList();
        }

        public List<LocationWiseEmployeeTargetEntryCheck> ReadLocationWiseEmployeeTargetEntryCheck(string locatioCode, string YearMonth)
        {
            return raSolarERPRepository.ReadLocationWiseEmployeeTargetEntryCheck(locatioCode, YearMonth).ToList();
        }

        public bool IsInventoryStockUpdateFinish(string locationCode)
        {
            return raSolarERPRepository.IsInventoryStockUpdateFinish(locationCode);
        }

        public Sal_SalesAgreement ReadSalesAgreement(string customerCode)
        {
            return raSolarERPRepository.ReadSalesAgreement(customerCode);
        }

        public List<PackageDetailsForSystemReturn> ReadPackageDetailsForSystemReturn(string customerCode, DateTime returnDate)
        {
            return raSolarERPRepository.ReadPackageDetailsForSystemReturn(customerCode, returnDate);
        }

        public List<PanelSerialList> PanelSerialByLocationAndStock(string locationCode, byte storeLocation, string itemCategory, string itemCapacity, byte agreementType, string packageCode)
        {
            return raSolarERPRepository.PanelSerialByLocationAndStock(locationCode, storeLocation, itemCategory, itemCapacity, agreementType, packageCode);
        }

        public DepretiatedPackagePriceBySRPanelSerial GetDepretiatedPackagePriceBySRPanelSerial(string panelSerial, string packageCode)
        {
            return raSolarERPRepository.GetDepretiatedPackagePriceBySRPanelSerial(panelSerial, packageCode);
        }

        public List<CollectionEfficiencyByCustomer> ReadCollectionEfficiencyByCustomer(string locationCode, string yearMonth)
        {
            return raSolarERPRepository.ReadCollectionEfficiencyByCustomer(locationCode, yearMonth);
        }

        public List<ProgressReview> ReadProgressReview()
        {
            return raSolarERPRepository.ReadProgressReviewGraph();
        }

        public List<LocationInfo> Location()
        {
            return raSolarERPRepository.Location();
        }

        public List<LocationInfo> LocationWithHeadOffice()
        {
            return raSolarERPRepository.LocationWithHeadOffice();
        }

        public LocationInfo Location(string LocationCode)
        {
            return raSolarERPRepository.Location(LocationCode);
        }

        public List<LocationInfo> LocationByLocationCode(string LocationCode)
        {
            return raSolarERPRepository.LocationByLocationCode(LocationCode);
        }

        public List<CustomerFPRAndScheduledCollectionEntry> GetCustomerFPRAndScheduledCollectionEntry(string unitCode, string optionForMissingFPROrDay, string prmEMP_ID, string scheduledCollectionDay)
        {
            return raSolarERPRepository.GetCustomerFPRAndScheduledCollectionEntry(unitCode, optionForMissingFPROrDay, prmEMP_ID, scheduledCollectionDay);
        }

        public List<CustomerFPRNDayWiseRegularOrODTarget> GetCustomerFPRNDayWiseRegularOrODTarget(string unitCode, string employeeID)
        {
            return raSolarERPRepository.GetCustomerFPRNDayWiseRegularOrODTarget(unitCode, employeeID);
        }

        public List<LocationNEmployeeWiseDailySalesNCollectionTarget> ReadLocationNEmployeeWiseDailySalesNCollectionTarget(string locationCode)
        {
            return raSolarERPRepository.ReadLocationNEmployeeWiseDailySalesNCollectionTarget(locationCode);
        }
        public List<SalSalesNCollectionTargetVsAchievementForGraph> ReadSalSalesNCollectionTargetVsAchievementForGraph(string reportType, string locationCode, string employeeID)
        {
            return raSolarERPRepository.ReadSalSalesNCollectionTargetVsAchievementForGraph(reportType, locationCode, employeeID);
        }
        public List<ItemLedgerReport> ReadInvItemLedger(byte storeLocation, string locationCode, string itemCode, string fromDate, string toDate)
        {
            return raSolarERPRepository.ReadInvItemLedger(storeLocation, locationCode, itemCode, fromDate, toDate);
        }

        public List<Sal_CustomerFuelUsed> ReadCustomerFuelUsed()
        {
            return raSolarERPRepository.ReadCustomerFuelUsed();
        }

        public List<Sal_CustomerIncomeRange> ReadCustomerIncomeRange()
        {
            return raSolarERPRepository.ReadCustomerIncomeRange();
        }

        public List<Sal_CustomerOccupations> ReadCustomerOccupations()
        {
            return raSolarERPRepository.ReadCustomerOccupations();
        }

        public List<Sal_CustomerRelations> ReadCustomerRelations()
        {
            return raSolarERPRepository.ReadCustomerRelations();
        }

        public List<InvChallanOrMRRDetailsForItemLedger> ReadInvChallanDetails(string locationCode, string challanOrMRRSeqNo, string challanOrMRR)
        {
            return raSolarERPRepository.ReadInvChallanDetails(locationCode, challanOrMRRSeqNo, challanOrMRR);
        }

        public List<Sal_ListedUnitPriceForSparePartsSales> ReadListedUnitPriceForSparePartsSales()
        {
            return raSolarERPRepository.ReadListedUnitPriceForSparePartsSales();
        }

        public Sal_ListedUnitPriceForSparePartsSales ReadListedUnitPriceForSparePartsSales(string yearMonth, string itemCode, byte stockLocation)
        {
            return raSolarERPRepository.ReadListedUnitPriceForSparePartsSales(yearMonth, itemCode, stockLocation);
        }

        public List<InventorySummaryReportV2> ReadInventorySummaryReportV2(string reportType, string itemType, byte storeLocation, string locationCode, DateTime startDate, DateTime endDate, string vendorType)
        {
            return raSolarERPRepository.ReadInventorySummaryReportV2(reportType, itemType, storeLocation, locationCode, startDate, endDate, vendorType);
        }

        public List<InventoryERPVersusPhysicalBalance> ReadInventoryERPVersusPhysicalBalance(string reportType, string itemType, byte storeLocation, string locationCode, string yearMonth, string vendorType)
        {
            return raSolarERPRepository.ReadInventoryERPVersusPhysicalBalance(reportType, itemType, storeLocation, locationCode, yearMonth, vendorType);
        }

        public string ReadCustomerTraineeName(string trainerId)
        {
             return raSolarERPRepository.ReadCustomerTraineeName(trainerId);
        }

        public List<CustomerTrainingInfo> ReadCustomerTrainingInfo(string unitCode, bool trainingStatus, byte trainingBatchNo)
        {
            return raSolarERPRepository.ReadCustomerTrainingInfo(unitCode, trainingStatus, trainingBatchNo);
        }

        public List<GetUnitWiseCustomerTrainingSchedule> ReadGetUnitWiseCustomerTrainingSchedule(string unitCode, DateTime? trainingDate, byte? trainingBatchNumber)
        {
            return raSolarERPRepository.ReadGetUnitWiseCustomerTrainingSchedule(unitCode, trainingDate, trainingBatchNumber);
        }

        public List<Sal_ItemSetMaster> ReadItemSetMaster()
        {
            return raSolarERPRepository.ReadItemSetMaster();
        }
        public List<Sal_ItemSetDetail> ReadItemSetDetails(string itemSetCode)
        {
            return raSolarERPRepository.ReadItemSetDetails(itemSetCode);
        }

        // Code By T.Alam
        public List<Sal_SparePartsSales_DataFromExternalSources> ReadSalesExternalSourceByLCode(string locationCode)
        {
            return raSolarERPRepository.ReadSalesExternalSourceByLCode(locationCode);
        }

        public List<Sal_SalesAgreementPrePost_DataFromExternalSources> ReadSalesResalesAgrExternalSourceByLCode(string locationCode)
        {
            return raSolarERPRepository.ReadSalesResalesAgrExternalSourceByLCode(locationCode); 
        }
        public List<SparePartSetDetils> ReadSparePartSetDetils(string itemSetCode)
        {
            return raSolarERPRepository.ReadSparePartSetDetils(itemSetCode);
        }

        public Sal_ItemSetMaster ReadItemSetMaster(string itemSetCode)
        {
            return raSolarERPRepository.ReadItemSetMaster(itemSetCode);
        }

        public string ChallanOrMRRForAuditAdjustmentSequenceMax(string locationCode, string yearMonthDate)
        {
            return raSolarERPRepository.ChallanOrMRRForAuditAdjustmentSequenceMax(locationCode, yearMonthDate);
        }

        public List<SummarySheetForRegionalSalesPosting> ReadSummarySheetForRegionalSalesPosting(DateTime dateFrom, DateTime dateTo, string regionCode)
        {
            return raSolarERPRepository.ReadSummarySheetForRegionalSalesPosting(dateFrom, dateTo, regionCode);
        }

        public List<FixedAssetSerialList> GetFixedAssetSerialList(byte storeLocation, string locationCode, string itemCode, string option)
        {
            return raSolarERPRepository.GetFixedAssetSerialList(storeLocation, locationCode, itemCode, option);
        }

        public List<AssetAssign> FixedAssetAssignUnassign(string locationCode, string itemCode, byte storeLocation, string employeeId, byte status, string unAssignedQuantity)
        {
            return raSolarERPRepository.FixedAssetAssignUnassign(locationCode, itemCode, storeLocation, employeeId, status, unAssignedQuantity);
        }

        public string EmployeeWiseFixedAssetsAllocationSequenceNumberMax(string locationCode, string employeeId, string itemCode, byte storeLocation)
        {
            return raSolarERPRepository.EmployeeWiseFixedAssetsAllocationSequenceNumberMax(locationCode, employeeId, itemCode, storeLocation);
        }

        public List<ChallanOrMRRItemsSerial> ReadChallanOrMRRItemsSerial(string locationCode, string challanSequenceNumber, string mrrSequenceNumber, string itemCode)
        {
            return raSolarERPRepository.ReadChallanOrMRRItemsSerial(locationCode, challanSequenceNumber, mrrSequenceNumber, itemCode);
        }

        public ArrayList ReadVendorChallanWithItemReferenceToCheck(string deliveryNoteNo)
        {
            return raSolarERPRepository.ReadVendorChallanWithItemReferenceToCheck(deliveryNoteNo);
        }

        public Array MrrSequenceListForMaterialReceiving(string rreDeliveryNote, string locationCode)
        {
            return raSolarERPRepository.MrrSequenceListForMaterialReceiving(rreDeliveryNote, locationCode);
        }

        public List<MaterialRecevingReportMrrDetails> MaterialReceivingMrrDetails(string LocationCode, string VendorRefChallanNo, string mrrSequenceNumber, string billNo)
        {
            return raSolarERPRepository.MaterialReceivingMrrDetails(LocationCode, VendorRefChallanNo, mrrSequenceNumber, billNo);
        }
        
        public List<DeliveryNoteSummary> ReadDeliveryNoteSummary(string delivaryScheduleNo)
        {
            return raSolarERPRepository.ReadDeliveryNoteSummary(delivaryScheduleNo);
        }

        public List<DeliveryItemNoteReportSummary> ReadDeliveryItemNoteReportSummary(string delivaryScheduleNo)
        {
            return raSolarERPRepository.ReadDeliveryItemNoteReportSummary(delivaryScheduleNo); 
        }

        public List<DeliveryPackageNoteReportSummary> ReadDeliveryPackageNoteReportSummary(string delivaryScheduleNo)
        {
            return raSolarERPRepository.ReadDeliveryPackageNoteReportSummary(delivaryScheduleNo);
        }

        //start monthly

        public List<DeliveryNoteSummary> ReadMonthlyDeliveryNoteSummary(string monthYear)
        {
            return raSolarERPRepository.ReadMonthlyDeliveryNoteSummary(monthYear);
        }

        public List<DeliveryItemNoteReportSummary> ReadMonthlyDeliveryItemNoteReportSummary(string monthYear)
        {
            return raSolarERPRepository.ReadMonthlyDeliveryItemNoteReportSummary(monthYear);
        }

        public List<DeliveryPackageNoteReportSummary> ReadMonthlyDeliveryPackageNoteReportSummary(string monthYear)
        {
            return raSolarERPRepository.ReadMonthlyDeliveryPackageNoteReportSummary(monthYear);
        }
        //end monthly

        //start date wise

        public List<DeliveryNoteSummary> ReadDateWiseDeliveryNoteSummary(string fromDate, string toDate)
        {
            return raSolarERPRepository.ReadDateWiseDeliveryNoteSummary(fromDate,toDate);
        }

        public List<DeliveryItemNoteReportSummary> ReadDateWiseDeliveryItemNoteReportSummary(string fromDate, string toDate)
        {
            return raSolarERPRepository.ReadDateWiseDeliveryItemNoteReportSummary(fromDate,toDate);
        }

        public List<DeliveryPackageNoteReportSummary> ReadDateWiseDeliveryPackageNoteReportSummary(string fromDate, string toDate)
        {
            return raSolarERPRepository.ReadDateWiseDeliveryPackageNoteReportSummary(fromDate,toDate);
        }
        //end date wise

        public List<CustomerDisasterRecoveryDetails> ReadCustomerDisasterRecoveryList(string collectionType, byte customerStatus, DateTime collectionDate, string locationCode)
        {
            return raSolarERPRepository.ReadCustomerDisasterRecoveryList(collectionType, customerStatus, collectionDate, locationCode);
        }

        public decimal ReadCustomerDetailsForDRFACollection(string collectionType, string customerCode, string locationCode, DateTime collectionDate, byte customerStatus)
        {
            return raSolarERPRepository.ReadCustomerDetailsForDRFACollection(collectionType, customerCode, locationCode, collectionDate, customerStatus);
        }

        public ArrayList ReadMRRNDeliveryNoteValue(string locationCode, string mrrSequenceNumber, string yearMonth, string rreDeliveryNote)
        {
            return raSolarERPRepository.ReadMRRNDeliveryNoteValue(locationCode, mrrSequenceNumber, yearMonth, rreDeliveryNote);
        }
        public List<ChallanInformationGlanceDetails> ReadChallanInformationGlanceList(string itemType, string locationCode, DateTime dateFrom, DateTime dateTo)
        {
            return raSolarERPRepository.ReadChallanInformationGlanceList(itemType, locationCode, dateFrom, dateTo);
        }

        public List<MrrInformationGlanceDetails> ReadMrrInformationGlanceList(string itemType, string locationCode, DateTime dateFrom, DateTime dateTo)
        {
            return raSolarERPRepository.ReadMrrInformationGlanceList(itemType, locationCode, dateFrom, dateTo);
        }

        public List<StockInTransitAtGlanceDetails> ReadStockInTransitAtGlanceList(string challanType, string locationCode, DateTime dateFrom, DateTime dateTo)
        {
            return raSolarERPRepository.ReadStockInTransitAtGlanceList(challanType, locationCode, dateFrom, dateTo);
        }

        public ArrayList ReadVendorRefChallanNo(string billNO)
        {
            return raSolarERPRepository.ReadVendorRefChallanNo(billNO);
        }

        public InventoryInTransitBalance ReadInventoryInTransitBalance(string options, string yearMonth)
        {
            return raSolarERPRepository.ReadInventoryInTransitBalance(options, yearMonth);
        }

        public List<UnitWiseCustomerLedger> ReadUnitWiseCustomerLedger(string reportOption, string locationCode, string dateFrom, string dateTo)
        {
            return raSolarERPRepository.ReadUnitWiseCustomerLedger(reportOption, locationCode, dateFrom, dateTo);
        }

        public string AuditSequenceNumberMax(string locationCode, string yearMonthDate)
        {
            return raSolarERPRepository.AuditSequenceNumberMax(locationCode, yearMonthDate);
        }

        public CustomerNAgreementNItemDetails ReadCustomerNAgreementItemDetails(string customerCode)
        {
            return raSolarERPRepository.ReadCustomerNAgreementItemDetails(customerCode);
        }

        public Aud_AuditAdjustmentRelatedCollectionFromCustomers SaveCustomerCollectionForAudit(Sal_CollectionFromCustomers objCustomerCollection, Aud_AuditAdjustmentRelatedCollectionFromCustomers objAuditAdjustmentCustomerCollection, string cashMemoUsesId)
        {
            return raSolarERPRepository.SaveCustomerCollectionForAudit(objCustomerCollection, objAuditAdjustmentCustomerCollection, cashMemoUsesId);
        }

        public Sal_CollectionFromCustomers Update(Sal_CollectionFromCustomers objCollectionFromCustomers, Aud_AuditAdjustmentRelatedCollectionFromCustomers objCollectionAuditAdjustnment)
        {
            return raSolarERPRepository.Update(objCollectionFromCustomers, objCollectionAuditAdjustnment);
        }

        public List<CustomerCollectionAdjustmentForAudit> ReadCustomerCollectionAdjustmentForAudit(string customerCode, string yearMonth, string auditSeqNo)
        {
            return raSolarERPRepository.ReadCustomerCollectionAdjustmentForAudit(customerCode, yearMonth, auditSeqNo);
        }

        public Aud_AuditAdjustmentObservationOnSalesAgreement Create(Aud_AuditAdjustmentObservationOnSalesAgreement objAuditAdjustmentObservationOnSalesAgreement, Aud_AuditAdjustmentObservationOnSalesAgreement objPreviousDataAuditAdjustmentObservationOnSalesAgreement)
        {
            return raSolarERPRepository.Create(objAuditAdjustmentObservationOnSalesAgreement, objPreviousDataAuditAdjustmentObservationOnSalesAgreement);
        }

        public bool IsAuditAdjustmentObservationOnSalesAgreementExistOrNot(string locationCode, string yearMonth, string customerCode, string auditSeqNo)
        {
            return raSolarERPRepository.IsAuditAdjustmentObservationOnSalesAgreementExistOrNot(locationCode, yearMonth, customerCode, auditSeqNo);
        }

        public Common_Sys_SystemSettings ReadSystemSettings(string companyName)
        {
            return raSolarERPRepository.ReadSystemSettings(companyName);
        }

        public bool IsCashMemoManagementEnabled(string companyName)
        {
            return raSolarERPRepository.IsCashMemoManagementEnabled(companyName);
        }

        public string GetCashMemoInUsesId(string entrySource, string cashMemoNo, string cashMemoUsesId, string locationCode, string refDocNo)
        {
            return raSolarERPRepository.GetCashMemoInUsesId(entrySource, cashMemoNo, cashMemoUsesId, locationCode, refDocNo);
        }

        public bool IsMemoStillAvailableToUse(string employeeId)
        {
            return raSolarERPRepository.IsMemoStillAvailableToUse(employeeId);
        }

        // Auditor's Section For Sales Correction------------------------------

        public List<ProjectInfo> ReadProjectForAuditor(string programCode)
        {
            return raSolarERPRepository.ReadProjectForAuditor(programCode);
        }

        public List<ItemCapacity> ReadPackageOrItemCapacityForAuditor(string projectCode, string isForItemOrPackage)
        {
            return raSolarERPRepository.ReadPackageOrItemCapacityForAuditor(projectCode, isForItemOrPackage);
        }

        public List<LightInfo> ReadLightForAuditor(string capacityID)
        {
            return raSolarERPRepository.ReadLightForAuditor(capacityID);
        }

        public List<PackageInformation> ReadPackagesForAuditor(string capacityId, string lightId)
        {
            return raSolarERPRepository.ReadPackagesForAuditor(capacityId, lightId);
        }

        public List<ItemInfo> ReadInvItemsForAuditors(string itemType, string itemCategory)
        {
            return raSolarERPRepository.ReadInvItemsForAuditors(itemType, itemCategory);
        }

        public List<ItemInfo> ReadInvItemsForAuditors(string itemType, string itemCategory, string itemCapacity)
        {
            return raSolarERPRepository.ReadInvItemsForAuditors(itemType, itemCategory, itemCapacity);
        }

        public List<ItemSerialInfo> ReadItemStockWithSerialNoByLocationForAuditor(byte storeLocation, string locationCode, string itemCode)
        {
            return raSolarERPRepository.ReadItemStockWithSerialNoByLocationForAuditor(storeLocation, locationCode, itemCode);
        }

        public List<ServiceChargeInformation> ReadServiceChargePolicyForAuditor(string customerType, string modeOfPayment)
        {
            return raSolarERPRepository.ReadServiceChargePolicyForAuditor(customerType, modeOfPayment);
        }

        public List<DownPaymentPolicy> ReadDownPaymentPolicyForAuditor(string modeOfPayment, string packageCode)
        {
            return raSolarERPRepository.ReadDownPaymentPolicyForAuditor(modeOfPayment, packageCode);
        }

        public DiscountPolicy ReadDiscountPolicyByModeofPaymentNDiscountIdForAuditor(string modeOfPayment, string discountId)
        {
            return raSolarERPRepository.ReadDiscountPolicyByModeofPaymentNDiscountIdForAuditor(modeOfPayment, discountId);
        }

        public DiscountPolicy ReadDiscountPolicyByModeofPaymentNPackageIdForAuditor(string modeOfPayment, string packageCode)
        {
            return raSolarERPRepository.ReadDiscountPolicyByModeofPaymentNPackageIdForAuditor(modeOfPayment, packageCode);
        }

        public List<ODRecoveryStatusMonitoring> ReadODRecoveryStatusMonitoring(string locationCode, bool IsOnlyForCollectionDatePassed, string customerGrading, string CustomerFPR, byte? scheduledCollectionDay)
        {
            return raSolarERPRepository.ReadODRecoveryStatusMonitoring(locationCode, IsOnlyForCollectionDatePassed, customerGrading, CustomerFPR, scheduledCollectionDay);
        }

        public List<SalesRecoveryCommitmentByReview> ReadSalesRecoveryCommitmentByReview(string reportType, string locationCode)
        {
            return raSolarERPRepository.ReadSalesRecoveryCommitmentByReview(reportType, locationCode);
        }

        public List<SalesRecoveryStatusEntryMonitoring> ReadSalesRecoveryStatusEntryMonitoring(string locationCode, bool IsOnlyForCollectionDatePassed, string customerGrading, string CustomerFPR, byte? scheduledCollectionDay)
        {
            return raSolarERPRepository.ReadSalesRecoveryStatusEntryMonitoring(locationCode, IsOnlyForCollectionDatePassed, customerGrading, CustomerFPR, scheduledCollectionDay);
        }

        public Sal_DailyBusinessPerformanceMonitoring_SalesAndRecoveryStatusByUnit ReadDailyBusinessPerformanceMonitoringRemarks(string yearMonth, string locationCode)
        {
            return raSolarERPRepository.ReadDailyBusinessPerformanceMonitoringRemarks(yearMonth, locationCode);
        }
         
      

        public bool IsAuditAdjustmentObservationOnSalesAgreementExistOrNot(string locationCode, string customerCode)
        {
            return raSolarERPRepository.IsAuditAdjustmentObservationOnSalesAgreementExistOrNot(locationCode, customerCode);
        }

        public byte getCustomerCollectionEntrySerialNumber(string customerCode, string collectionDate)
        {
            return raSolarERPRepository.getCustomerCollectionEntrySerialNumber(customerCode, collectionDate);
        }

        public List<Inv_VendorInfo> GetVendorListForItemSummary()
        {
            return raSolarERPRepository.GetVendorListForItemSummary();
        }

        public ArrayList GetVendorList(string itemTransactionType)
        {
            return raSolarERPRepository.GetVendorList(itemTransactionType);
        }

        public ArrayList ReadSpecialPackageListForSales(string packageCapacityId, string lightId, string programCode, string salesReSalesOrBoth)
        {
            return raSolarERPRepository.ReadSpecialPackageListForSales(packageCapacityId, lightId, programCode, salesReSalesOrBoth);
        }

        public PackagePricingDetailsForSalesAgreement ReadPackagePricingDetailsForSalesAgreement(string locationCode, string programCode, string salesReSalesOrBoth, string customerType, string packageCapacity, string lightID, string packageCode, string modeOfPaymentID, string changedDownPaymentAmount)
        {
            return raSolarERPRepository.ReadPackagePricingDetailsForSalesAgreement(locationCode, programCode, salesReSalesOrBoth, customerType, packageCapacity, lightID, packageCode, modeOfPaymentID, changedDownPaymentAmount);
        }

        public ArrayList ReadModeOfPaymentForSpecialPackageSales(string salesReSalesOrBoth)
        {
            return raSolarERPRepository.ReadModeOfPaymentForSpecialPackageSales(salesReSalesOrBoth);
        }

        public List<DailyPerformanceMonitoringForSales> ReadDailyPerformanceMonitoringForSales(string reportOption, string locationCode)
        {
            return raSolarERPRepository.ReadDailyPerformanceMonitoringForSales(reportOption, locationCode);
        }

        public List<DailyBusinessPerformanceMonitoringCollection> ReadDailyBusinessPerformanceMonitoringCollection(string reportOption, string locationCode)
        {
            return raSolarERPRepository.ReadDailyBusinessPerformanceMonitoringCollection(reportOption, locationCode);
        }

        public List<ItemSerialCorrection> ReadItemSerialCorrectionInformation(string locationCode, string itemCode, string optionForCorrection, string wrongSerialNo, string correctSerialNo, string loginID)
        {
            return raSolarERPRepository.ReadItemSerialCorrectionInformation(locationCode, itemCode, optionForCorrection, wrongSerialNo, correctSerialNo, loginID);
        }

        public List<DailyBusinessPerformanceMonitoringNetODIncreaseOrDecreaseComperison> ReadDailyBusinessPerformanceMonitoringIncreaseOrDecreaseComperison(string reportOption, string locationCode)
        {
            return raSolarERPRepository.ReadDailyBusinessPerformanceMonitoringIncreaseOrDecreaseComperison(reportOption, locationCode);
        }

        public List<DailyBusinessPerformanceMonitoringSalesNCollectionTargetVsAchievement> ReadDailyBusinessPerformanceMonitoringSalesNCollectionTargetVsAchievement(string reportOption, string locationCode, DateTime dateForTheStatus)
        {
            return raSolarERPRepository.ReadDailyBusinessPerformanceMonitoringSalesNCollectionTargetVsAchievement(reportOption, locationCode, dateForTheStatus);
        }

        public List<DailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus> ReadDailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus(string reportOption, string locationCode)
        {
            return raSolarERPRepository.ReadDailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus(reportOption, locationCode);
        }

        public List<DailyBusinessPerformanceMonitoringOtherStatus> ReadDailyBusinessPerformanceMonitoringOtherStatus(string reportOption, string locationCode)
        {
            return raSolarERPRepository.ReadDailyBusinessPerformanceMonitoringOtherStatus(reportOption, locationCode);
        }

        public List<WarrantyClaimReason> GetWarrantyClaimReason()
        {
            return raSolarERPRepository.GetWarrantyClaimReason();
        }

        public string CheckingIssueItemSerailNO(string locationCode, string itemSerial, string itemCode, string storeLocationForIssue)
        {
            return raSolarERPRepository.CheckingIssueItemSerailNO(locationCode, itemSerial, itemCode, storeLocationForIssue);
        }

        public List<ItemInfo> GetSubstituteItemCode(string itemCode)
        {
            return raSolarERPRepository.GetSubstituteItemCode(itemCode);
        }

        #endregion

        #region Create

        //public Aud_AuditingMaster Create(Aud_AuditingMaster objAuditingMaster)
        //{
        //    return raSolarERPRepository.Create(objAuditingMaster);
        //}

        //public Common_UnitWiseCustomerStatus Create(Common_UnitWiseCustomerStatus objUnitWiseCustomerStatus)
        //{
        //    return raSolarERPRepository.Create(objUnitWiseCustomerStatus);
        //}

        public List<OverdueCollectionTargetVsAchievementByUnitOffice> ReadOverdueCollectionTargetVsAchievementByUnitOffice(string spYearMonth, string spLocationCode, string reportType)
        {
            return raSolarERPRepository.ReadOverdueCollectionTargetVsAchievementByUnitOffice(spYearMonth, spLocationCode, reportType);
        }

        public List<CollectionEfficiencyByUnitOfficeSummary> ReadCollectionEfficiencyByUnitOfficeSummary(string yearMonth, string locationCode, string reportType)
        {
            return raSolarERPRepository.ReadCollectionEfficiencyByUnitOfficeSummary(yearMonth, locationCode, reportType);
        }

        //public Common_InventoryTransaction Create(Common_InventoryTransaction objInventoryTranscation)
        //{
        //    return raSolarERPRepository.Create(objInventoryTranscation);
        //}

        public Sal_LocationNEmployeeWiseActivityDaily Create(Sal_LocationNEmployeeWiseActivityDaily objLocationNEmployeeWiseActivityDailyCreate)
        {
            return raSolarERPRepository.Create(objLocationNEmployeeWiseActivityDailyCreate);
        }

        public Sal_LocationNEmployeeWiseActivityMonthly Create(Sal_LocationNEmployeeWiseActivityMonthly objLocationNEmployeeWiseActivityMonthlyCreate)
        {
            return raSolarERPRepository.Create(objLocationNEmployeeWiseActivityMonthlyCreate);

        }

        //public Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement Create(List<Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement> objLocationNEmployeeWiseWeeklySalesNCollectionAchievement)
        //{
        //    return raSolarERPRepository.Create(objLocationNEmployeeWiseWeeklySalesNCollectionAchievement);
        //}

        //public Sal_LocationWiseWeeklySalesNCollectionSummary Create(Sal_LocationWiseWeeklySalesNCollectionSummary objSal_LocationWiseWeeklySalesNCollectionSummary)
        //{
        //    return raSolarERPRepository.Create(objSal_LocationWiseWeeklySalesNCollectionSummary);
        //}

        public Sal_SalesAgreementPrePost CreateSalesAgreement(Sal_CustomerPrePost objCustomer, Sal_SalesAgreementPrePost objSalesAgreement, List<Sal_SalesItemsPrePost> lstSalesItem, List<Sal_SalesItemsWithSerialNoPrePost> lstItemSalesWithSerialNo)
        {
            return raSolarERPRepository.Create(objCustomer, objSalesAgreement, lstSalesItem, lstItemSalesWithSerialNo);
        }

        public Sal_SalesAgreement Create(Sal_Customer objCustomer, Sal_SalesAgreement objSalesAgreement, Sal_CustomerStatus objCustomerStatus, List<Sal_SalesItems> lstSalesItem, List<Sal_SalesItemsWithSerialNo> lstItemSalesWithSerialNo, int customerCurrentSerial, string serialTempTableRows)
        {
            return raSolarERPRepository.Create(objCustomer, objSalesAgreement, objCustomerStatus, lstSalesItem, lstItemSalesWithSerialNo, customerCurrentSerial, serialTempTableRows);
        }

        public Sal_CollectionFromCustomersPrePost Create(Sal_CollectionFromCustomersPrePost objCollectionFromCustomers, Sal_Customer objSalCustomerInfo)
        {
            return raSolarERPRepository.Create(objCollectionFromCustomers, objSalCustomerInfo);
        }

        public Sal_SalesItems Create(Sal_SalesItems objSalesItems)
        {
            return raSolarERPRepository.Create(objSalesItems);
        }

        public Sal_SalesItemsWithSerialNo Create(Sal_SalesItemsWithSerialNo objItemSalesWithSerialNo)
        {
            return raSolarERPRepository.Create(objItemSalesWithSerialNo);
        }

        public int Create(string insertedRows, string locationCode)
        {
            return raSolarERPRepository.Create(insertedRows, locationCode);
        }

        public Inv_MRRMaster Create(Inv_MRRMaster objMRRMaster, List<Inv_MRRDetails> lstMRRDetails, List<Inv_MRRDetailsWithSerialNo> lstMRRDetailsWithSerialNo)
        {
            return raSolarERPRepository.Create(objMRRMaster, lstMRRDetails, lstMRRDetailsWithSerialNo);
        }

        public Inv_MRRMaster Create(Inv_MRRMaster objMRRMaster, List<Inv_MRRDetails> lstMRRDetails, List<Inv_MRRDetailsWithSerialNo> lstMRRDetailsWithSerialNo, string serialTempTableRows)
        {
            return raSolarERPRepository.Create(objMRRMaster, lstMRRDetails, lstMRRDetailsWithSerialNo, serialTempTableRows);
        }

        public Inv_MRRMaster Create(Inv_MRRMaster objMRRMaster, List<Inv_MRRDetails> lstMRRDetails, List<Inv_MRRDetailsWithSerialNo> lstMRRDetailsWithSerialNo, string serialTempTableRows, Aud_AuditAdjustmentRelatedChallanOrMRRForReference objChallanOrMRRForAuditAdjustment)
        {
            return raSolarERPRepository.Create(objMRRMaster, lstMRRDetails, lstMRRDetailsWithSerialNo, serialTempTableRows, objChallanOrMRRForAuditAdjustment);
        }

        public Inv_ChallanMaster Create(Inv_ChallanMaster objChallanMaster, List<Inv_ChallanDetails> lstChallanDetails, List<Inv_ChallanDetailsWithSerialNo> lstChallanDetailsWithSerialNo)
        {
            return raSolarERPRepository.Create(objChallanMaster, lstChallanDetails, lstChallanDetailsWithSerialNo);
        }

        public Sal_SparePartsSalesMaster Create(Inv_ChallanMaster objChallanMaster, List<Inv_ChallanDetails> lstChallanDetails, List<Inv_ChallanDetailsWithSerialNo> lstChallanDetailsWithSerialNo, Sal_SparePartsSalesMaster objSparePartsSalesMaster, List<Sal_SparePartsSalesItems> lstSparePartsSalesItems, List<Sal_SparePartsSalesItemsWithSerialNo> lstSparePartsSalesItemsWithSerialNo)
        {
            return raSolarERPRepository.Create(objChallanMaster, lstChallanDetails, lstChallanDetailsWithSerialNo, objSparePartsSalesMaster, lstSparePartsSalesItems, lstSparePartsSalesItemsWithSerialNo);
        }

        public Inv_ChallanMaster Create(Inv_ChallanMaster objChallanMaster, List<Inv_ChallanDetails> lstChallanDetails, List<Inv_ChallanDetailsWithSerialNo> lstChallanDetailsWithSerialNo, List<Inv_ChallanWithMultipleLocations> lstChallanWithMultipleLocations, List<Inv_ChallanWithMultipleLocationsWithItemWiseDistribution> lstChallanItemDistribution)
        {
            return raSolarERPRepository.Create(objChallanMaster, lstChallanDetails, lstChallanDetailsWithSerialNo, lstChallanWithMultipleLocations, lstChallanItemDistribution);
        }

        public Sal_SystemReturn Create(Sal_SystemReturn objSystemReturn, List<Sal_SystemReturnItems> lstSystemReturnItems, List<Sal_SystemReturnItemsWithSerialNo> lstSystemReturnItemsWithSerialNo, string locationCode, string serialTempTableRows)
        {
            return raSolarERPRepository.Create(objSystemReturn, lstSystemReturnItems, lstSystemReturnItemsWithSerialNo, locationCode, serialTempTableRows);
        }

        public Sal_CustomerTrainingTransMaster Create(Sal_CustomerTrainingTransMaster objSalCustomerTrainingMaster, List<Sal_CustomerTrainingTransDetails> lstCustomerTrainingTransDetails)
        {
            return raSolarERPRepository.Create(objSalCustomerTrainingMaster, lstCustomerTrainingTransDetails);
        }

        public Inv_ChallanMaster Create(Inv_ChallanMaster objChallanMaster, List<Inv_ChallanDetails> lstChallanDetails, List<Inv_ChallanDetailsWithSerialNo> lstChallanDetailsWithSerialNo, Aud_AuditAdjustmentRelatedChallanOrMRRForReference objChallanOrMRRForAuditAdjustment)
        {
            return raSolarERPRepository.Create(objChallanMaster, lstChallanDetails, lstChallanDetailsWithSerialNo, objChallanOrMRRForAuditAdjustment);
        }

        public ArrayList GetFixedAssetAssignedOrUnassignedSeialList(byte storeLocation, string locationCode, string itemCode, string employeeId, int assignQuantity)
        {
            return raSolarERPRepository.GetFixedAssetAssignedOrUnassignedSeialList(storeLocation, locationCode, itemCode, employeeId, assignQuantity);
        }

        public Fix_EmployeeWiseFixedAssetsAllocation Create(Fix_EmployeeWiseFixedAssetsAllocation objAssetAssign, List<Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo> lstAssetsAllocationWithSerialNo)
        {
            return raSolarERPRepository.Create(objAssetAssign, lstAssetsAllocationWithSerialNo);
        }

        public List<AvailableNAssignFixedAsset> ReadAvailableNAssignFixedAsset(byte storeLocation, string locationCode, string itemCategory, string employeeSelectionOption, string employeeID)
        {
            return raSolarERPRepository.ReadAvailableNAssignFixedAsset(storeLocation, locationCode, itemCategory, employeeSelectionOption, employeeID);
        }

        //public List<InvItemInTransit> ReadInvItemInTransit(string itemType, string locationCode, string itemCode, DateTime fromDate, DateTime toDate)
        //{
        //    return raSolarERPRepository.ReadInvItemInTransit(itemType, locationCode, itemCode, fromDate, toDate);
        //}

        public List<StockInTransitAtGlanceDetails> ReadInvItemInTransit(string itemType, string locationCode, string itemCode, DateTime fromDate, DateTime toDate)
        {
            return raSolarERPRepository.ReadInvItemInTransit(itemType, locationCode, itemCode, fromDate, toDate);
        }


        public List<UnitWiseCashMemoInformation> ReadCashMemoInformation(string locationCode, string fromDate, string toDate)
        {
            return raSolarERPRepository.ReadCashMemoInformation(locationCode, fromDate, toDate);
        }

        public List<AvailableNAssignFixedAsset> ReadAvailableNAssignCashMemo(byte storeLocation, string locationCode, string itemCategory, string employeeSelectionOption, string employeeID)
        {
            return raSolarERPRepository.ReadAvailableNAssignCashMemo(storeLocation, locationCode, itemCategory, employeeSelectionOption, employeeID);
        }

        public List<AssignCashMemoBook> CashMemoAssignUnassign(string locationCode, string itemCode, byte storeLocation, string employeeId, byte status, string unAssignedQuantity)
        {
            return raSolarERPRepository.CashMemoAssignUnassign(locationCode, itemCode, storeLocation, employeeId, status, unAssignedQuantity);
        }

        public ArrayList GetCashMemoAssignedOrUnassignedSeialList(byte storeLocation, string locationCode, string itemCode, string employeeId, int assignQuantity)
        {
            return raSolarERPRepository.GetCashMemoAssignedOrUnassignedSeialList(storeLocation, locationCode, itemCode, employeeId, assignQuantity);
        }

        public Fix_EmployeeWiseFixedAssetsAllocation CreateForCashMemo(Fix_EmployeeWiseFixedAssetsAllocation objAssetAssign, List<Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo> lstAssetsAllocationWithSerialNo)
        {
            return raSolarERPRepository.CreateForCashMemo(objAssetAssign, lstAssetsAllocationWithSerialNo);
        }

        public Fix_EmployeeWiseFixedAssetsAllocation UpdateCashmemoAssign(Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo objEmployeeWiseFixedAssetsAllocationWithSerialNo, Fix_EmployeeWiseFixedAssetsAllocation objAssetAssign)
        {
            return raSolarERPRepository.UpdateCashmemoAssign(objEmployeeWiseFixedAssetsAllocationWithSerialNo, objAssetAssign);
        }

        public CustomerDataToCloseWithFullPaidOrWaive getCustomerDataToCloseWithFullPaidOrWaive(string select, string customerCode, string currentMonth, string locationCode)
        {
            return raSolarERPRepository.getCustomerDataToCloseWithFullPaidOrWaive(select, customerCode, currentMonth, locationCode);
        }

        public CustomerDataToPersonalInformation GetCustomerDataPersonalInformation(string customerCode)
        {

            return raSolarERPRepository.GetCustomerDataPersonalInformation(customerCode);

        }

        public Sal_SalesAgreementClosedWithFullPaidOrWaive SaveFullPaiedCustomer(Sal_SalesAgreementClosedWithFullPaidOrWaive objSalesAgreementClosedWithFullPaidOrWaive, string closedIn, bool? approvalRequiredForFullPayed)
        {
            return raSolarERPRepository.SaveFullPaiedCustomer(objSalesAgreementClosedWithFullPaidOrWaive, closedIn, approvalRequiredForFullPayed);
        }

        public Sal_ODCustomerGrading UpdateODRecoveryStatusMonitoring(Sal_ODCustomerGrading objODCustomerGrading)
        {
            return raSolarERPRepository.UpdateODRecoveryStatusMonitoring(objODCustomerGrading);
        }

        public void CreateUserLog(string ipAddress, string macAddress, string locationCode, string userId, string referenceEntrySource)
        {
            raSolarERPRepository.CreateUserLog(ipAddress, macAddress, locationCode, userId, referenceEntrySource);
        }

        public List<FixedAssetSerialList> GetCashMemoSerialList(byte storeLocation, string locationCode, string itemCode, string option)
        {
            return raSolarERPRepository.GetCashMemoSerialList(storeLocation, locationCode, itemCode, option);
        }

        public List<CollectionSheetForCustomerFPR> ReadCollectionSheetForCustomerFPR(string customerFPR, string locationCode)
        {
            return raSolarERPRepository.ReadCollectionSheetForCustomerFPR(customerFPR, locationCode);
        }

        public List<CashMemoBookPagesStatus> GetStatusForCashMemoBookAllocation(string itemSerialNo)
        {
            return raSolarERPRepository.GetStatusForCashMemoBookAllocation(itemSerialNo);
        }

        public CustomerDetails GetCustomerDetailsForWarrantyClaim(string customerCode, string unitCode)
        {
            return raSolarERPRepository.GetCustomerDetailsForWarrantyClaim(customerCode, unitCode);
        }

        public List<WarrantyItemsDetails> GetWarrantyItemsList(string customerCode, DateTime dayOpenDate)
        {
            return raSolarERPRepository.GetWarrantyItemsList(customerCode, dayOpenDate);
        }

        public Inv_MRRMaster SaveWarrantyClaimNSettlement(Inv_MRRMaster objMRRMaster, List<Inv_MRRDetails> lstMRRDetails, List<Inv_MRRDetailsWithSerialNo> lstMRRDetailsWithSerialNo, Inv_ChallanMaster objChallanMaster, List<Inv_ChallanDetails> lstChallanDetails, List<Inv_ChallanDetailsWithSerialNo> lstChallanDetailsWithSerialNo)
        {
            return raSolarERPRepository.SaveWarrantyClaimNSettlement(objMRRMaster, lstMRRDetails, lstMRRDetailsWithSerialNo, objChallanMaster, lstChallanDetails, lstChallanDetailsWithSerialNo);
        }

        public string ItemSerialCorrectionChangeNClear(string locationCode, string itemCode, string optionForCorrection, string wrongSerialNo, string correctSerialNo, string loginID)
        {
            return raSolarERPRepository.ItemSerialCorrectionChangeNClear(locationCode, itemCode, optionForCorrection, wrongSerialNo, correctSerialNo, loginID);
        }

        public SHSDistributionPlan_Master Create(SHSDistributionPlan_Master objSHSDistributionPlan_Master, List<SHSDistributionPlan_RootWiseLocation> lstSHSDistributionPlan_RootWiseLocation, List<SHSDistributionPlan_RootWiseLocationNPackage> lstSHSDistributionPlan_RootWiseLocationNPackage, List<SHSDistributionPlan_IndividualItem> lstSHSDistributionPlan_IndividualItem)
        {
            return raSolarERPRepository.Create(objSHSDistributionPlan_Master, lstSHSDistributionPlan_RootWiseLocation, lstSHSDistributionPlan_RootWiseLocationNPackage, lstSHSDistributionPlan_IndividualItem);
        }

        #endregion

        #region Update

        //public Common_UnitWiseEntryStatus Update(Common_UnitWiseEntryStatus objUnitWiseEntryStatus)
        //{
        //    return raSolarERPRepository.Update(objUnitWiseEntryStatus);
        //}

        //public Aud_AuditingMaster Update(Aud_AuditingMaster objAuditingMaster)
        //{
        //    return raSolarERPRepository.Update(objAuditingMaster);
        //}

        //public Common_UnitWiseCustomerStatus Update(Common_UnitWiseCustomerStatus objUnitWiseCustomerStatus)
        //{
        //    return raSolarERPRepository.Update(objUnitWiseCustomerStatus);
        //}

        public Sal_LocationNEmployeeWiseActivityDaily Update(Sal_LocationNEmployeeWiseActivityDaily objLocationNEmployeeWiseActivityDailyUpdate)
        {
            return raSolarERPRepository.Update(objLocationNEmployeeWiseActivityDailyUpdate);
        }

        public Sal_LocationNEmployeeWiseActivityMonthly Update(Sal_LocationNEmployeeWiseActivityMonthly objLocationNEmployeeWiseActivityMonthlyUpdate)
        {
            return raSolarERPRepository.Update(objLocationNEmployeeWiseActivityMonthlyUpdate);
        }
        public Sal_LocationWiseSalesNCollectionTarget Update(Sal_LocationWiseSalesNCollectionTarget objSalesNCollectionTarget)
        {
            return raSolarERPRepository.Update(objSalesNCollectionTarget);
        }

        //public Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement Update(Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement objWeeklyCollection)
        //{
        //    return raSolarERPRepository.Update(objWeeklyCollection);
        //}

        //public Sal_LocationWiseWeeklySalesNCollectionSummary Update(Sal_LocationWiseWeeklySalesNCollectionSummary objSal_LocationWiseWeeklySalesNCollectionSummary)
        //{
        //    return raSolarERPRepository.Update(objSal_LocationWiseWeeklySalesNCollectionSummary);
        //}

        public bool Update(string unitCode, string customerCode, decimal collectedOverdue, string weekNumber)
        {
            return raSolarERPRepository.Update(unitCode, customerCode, collectedOverdue, weekNumber);
        }

        public Sal_CollectionFromCustomersPrePost Update(Sal_CollectionFromCustomersPrePost objCollectionFromCustomers, Sal_CollectionFromCustomers objCustomerCol)
        {
            return raSolarERPRepository.Update(objCollectionFromCustomers, objCustomerCol);
        }

        public Sal_Customer Update(Sal_Customer objCustomer)
        {
            return raSolarERPRepository.Update(objCustomer);
        }

        public Sal_Customer Update(Sal_Customer objCustomer, string remarksNotes)
        {
            return raSolarERPRepository.Update(objCustomer, remarksNotes);
        }

        public Sal_CustomerStatus UpdateFPR(List<CustomerFPRAndScheduledCollectionEntry> lstCustomerFPREntry, string unitCode)
        {
            return raSolarERPRepository.UpdateFPR(lstCustomerFPREntry, unitCode);
        }

        public bool Update(List<Sal_LocationNEmployeeWiseDailySalesNCollectionTarget> lstEmployeeDailyTarget)
        {
            return raSolarERPRepository.Update(lstEmployeeDailyTarget);
        }

        public Aud_AuditAdjustmentObservationOnSalesAgreement Update(Aud_AuditAdjustmentObservationOnSalesAgreement objAuditOnSalesAgreement)
        {
            return raSolarERPRepository.Update(objAuditOnSalesAgreement);
        }

        public Sal_ODRecoveryCommitmentByRMnZM UpdateODRecoveryCommitmentByRMnZM(Sal_ODRecoveryCommitmentByRMnZM objODRecoveryCommitmentByRMnZM)
        {
            return raSolarERPRepository.UpdateODRecoveryCommitmentByRMnZM(objODRecoveryCommitmentByRMnZM);
        }

        public List<SalesRecoveryStatusEntryMonitoring> UpdateSalesRecoveryStatusEntryMonitoring(string option, string locationCode, string customerCode, DateTime? umLastNextRecoveryDate, string umLastRemarks, string umLastOverallRemarks, string amLastRemarks)
        {
            return raSolarERPRepository.UpdateSalesRecoveryStatusEntryMonitoring(option, locationCode, customerCode, umLastNextRecoveryDate, umLastRemarks, umLastOverallRemarks, amLastRemarks);
        }

        public void CustomerCollectionSaveForUpdateOrDelete(string updateDeleteOptions, Sal_CollectionFromCustomers objCollectionFromCustomers)
        {
            raSolarERPRepository.CustomerCollectionSaveForUpdateOrDelete(updateDeleteOptions, objCollectionFromCustomers);
        }


        public Sal_CustomerStatus UpdateCustomerStatusForSalesMonitoring(Sal_CustomerStatus objCustomerStatus)
        {
            return raSolarERPRepository.UpdateCustomerStatusForSalesMonitoring(objCustomerStatus);
        }

        public Inv_ERPVersusPhysicalBalance Update(Inv_ERPVersusPhysicalBalance objInvERPVersusPhysicalBalance, List<Inv_ERPVersusPhysicalBalance> lstInvERPVersusPhysicalBalance)
        {
            return raSolarERPRepository.Update(objInvERPVersusPhysicalBalance, lstInvERPVersusPhysicalBalance);
        }

        public SHSDistributionPlan_Master Update(SHSDistributionPlan_Master objSHSDistributionPlan_Master, List<SHSDistributionPlan_RootWiseLocation> lstSHSDistributionPlan_RootWiseLocation, List<SHSDistributionPlan_RootWiseLocationNPackage> lstSHSDistributionPlan_RootWiseLocationNPackage, List<SHSDistributionPlan_IndividualItem> lstSHSDistributionPlan_IndividualItem)
        {
            return raSolarERPRepository.Update(objSHSDistributionPlan_Master, lstSHSDistributionPlan_RootWiseLocation, lstSHSDistributionPlan_RootWiseLocationNPackage, lstSHSDistributionPlan_IndividualItem);
        }


        public string SaveSHSDistributionRouteTransfer(string routeid, string txtDistributionScheduleNo, string txtDistributionScheduleNoNew, string dtpDelivaryDate, string txtDeliveryScheduleNo)
        {
            return raSolarERPRepository.SaveSHSDistributionRouteTransfer(routeid, txtDistributionScheduleNo, txtDistributionScheduleNoNew, dtpDelivaryDate, txtDeliveryScheduleNo);
        }


        #endregion

        #region Delete

        public void Delete(object obj)
        {
            raSolarERPRepository.Delete(obj);
        }

        public void Delete(object obj1, object obj2)
        {
            raSolarERPRepository.Delete(obj1, obj2);
        }

        public void Delete(Inv_ItemNItemCategoryWithSerialNoMaster objItemSerialMaster, Inv_ItemStockWithSerialNoByLocation objItemSerials)
        {
            raSolarERPRepository.Delete(objItemSerialMaster, objItemSerials);
        }

        public void CustomerCollectionDelete(Sal_CollectionFromCustomersPrePost objCollectionFromCustomers)
        {
            raSolarERPRepository.CustomerCollectionDelete(objCollectionFromCustomers);
        }

        #endregion

        #region ExecuteSP

        public string ExecuteDayEndProcess(string Location, string CloseableDate)
        {
            return raSolarERPRepository.ExecuteDayEndProcess(Location, CloseableDate);
        }


        //UpdateLoadRequestEntry
        public string UpdateLoadRequestEntry(string locationCode, string employeeID, string dayOpenDay, string prmCorporatePhoneNo, string rsfServiceQunt, string paywellServiceQunt, string cashMemo)
        {
            return raSolarERPRepository.UpdateLoadRequestEntry(locationCode, employeeID, dayOpenDay, prmCorporatePhoneNo , rsfServiceQunt, paywellServiceQunt, cashMemo); 
        }


        public string UpdateLoadRequestEntryFinalSave(string locationCode, string dayOpenDayTocurrentdate, string txtRsfServiceTotal, string txtRsfPayWellTotal)
        {
            return raSolarERPRepository.UpdateLoadRequestEntryFinalSave(locationCode, dayOpenDayTocurrentdate, txtRsfServiceTotal, txtRsfPayWellTotal); 
        }

        public string EntryEmployeeVisitPlan(string prmLocationCode, string prmEmployeeID, string prmMovementDate, string prmTTLocationCode, string prmNoFcustomerVisit, string prmPurposeOfVisit, string prmDurationOfVisitInHours, string prmUserName, string prmDBTransType)
        {
            return raSolarERPRepository.EntryEmployeeVisitPlan(prmLocationCode, prmEmployeeID, prmMovementDate, prmTTLocationCode, prmNoFcustomerVisit, prmPurposeOfVisit, prmDurationOfVisitInHours, prmUserName, prmDBTransType);
        }


        public void CustomerTransferV2(string customerID, string LocationCode, string CustomerTransferOrReceive, string TransferToUnit, string TransDate, string NoteForTransfer, string UserID, string DBTransType)
        {
            raSolarERPRepository.CustomerTransferV2(customerID, LocationCode, CustomerTransferOrReceive, TransferToUnit, TransDate, NoteForTransfer, UserID, DBTransType);
        }

        #endregion

        #region Dispose Repository

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    raSolarERPRepository.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
