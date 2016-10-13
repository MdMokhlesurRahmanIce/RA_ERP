using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RASolarERP.Model;
using RASolarHelper;
using RASolarERP.DomainModel.InventoryModel;
using System.Collections;
using RASolarERP.DomainModel.SalesModel;
using RASolarERP.DomainModel.HRMSModel;
using RASolarHRMS.Model;


namespace RASolarERP.Web.Areas.Inventory.Models
{
    public class InventoryData
    {
        RASolarERPService erpService = new RASolarERPService();

        public List<InventoryDataEntryStatus> InventoryDataEntryState(string reportType, string locationType, string yearMonth)
        {
            return erpService.InventoryDataEntryState(reportType, locationType, yearMonth);
        }

        //public tbl_UnitWiseEntryStatus UnitWiseEntryStatus(string unitCode, string yearMonth)
        //{
        //    return erpService.UnitWiseEntryStatus(unitCode, yearMonth);
        //}

        //public List<InventoryAuditAdjustment> InventoryAdjustmentForItem(string unitCode, string yearMonth, string yearMonthWrite, byte componentStatus, int stockLocation)
        //{
        //    return erpService.InventoryAuditAdjustmentAdjust(unitCode, yearMonth, yearMonthWrite, componentStatus, stockLocation);
        //}

        public List<Aud_AdjustmentReasonCodes> AuditAdjustMentReasons(string reasonPurposeID, string reasonForUserOrAuditor, string reasonForModule)
        {
            return erpService.AuditAdjustMentReasons(reasonPurposeID, reasonForUserOrAuditor, reasonForModule);
        }

        public List<Aud_AdjustmentReasonCodes> CollectionAdjustMentReasons(string reasonPurposeID1, string reasonPurposeID2, string reasonForUserOrAuditor, string reasonForModule)
        {
            return erpService.CollectionAdjustMentReasons(reasonPurposeID1, reasonPurposeID2, reasonForUserOrAuditor, reasonForModule);
        }

        //public List<Aud_AdjustmentReasonCodes> SalesAdjustMentReasons(string reasonPurposeID)
        //{
        //    return erpService.AuditAdjustMentReasonsForInventory(reasonPurposeID);
        //}

        //public List<Aud_AdjustmentReasonCodes> AuditAdjustMentReasonForNew()
        //{
        //    return erpService.AuditAdjustMentReasons(Helper.ReasonForUserOrAuditor, Helper.NewItem, Helper.ForInventory);
        //}

        //public Aud_AuditingMaster AuditingMasterByLocationCode(string locationCode)
        //{
        //    return erpService.AuditingMasterByLocationCode(locationCode);
        //}

        //public tbl_InventoryTransaction ReadInventoryTransaction(string unitCode, string itemCode, string yearMonth, int stockLocation)
        //{
        //    return erpService.ReadInventoryTransaction(unitCode, itemCode, yearMonth, stockLocation);
        //}

        //public tbl_InventoryTransaction UpdateInventoryTransaction(tbl_InventoryTransaction objInventoryTranscation)
        //{
        //    return erpService.Update(objInventoryTranscation);
        //}

        //public Aud_AuditingMaster SaveAuditingMaster(Aud_AuditingMaster objAuditMaster)
        //{
        //    return erpService.Create(objAuditMaster);
        //}

        //public Aud_AuditingMaster UpdateAuditingMaster(Aud_AuditingMaster objAuditMaster)
        //{
        //    return erpService.Update(objAuditMaster);
        //}

        //public tbl_InventoryTransaction InsertInventoryTransaction(tbl_InventoryTransaction objInventoryTransaction)
        //{
        //    return erpService.Create(objInventoryTransaction);
        //}

        //public tbl_UnitWiseEntryStatus UpdateInvetoryEntryStatus(tbl_UnitWiseEntryStatus objUnitWiseEntryStatus)
        //{
        //    return erpService.Update(objUnitWiseEntryStatus);
        //}

        public List<Inv_Sys_ItemType> ReadItemType()
        {
            return erpService.ReadItemType();
        }

        public Inv_ItemCategorySubCategory ReadItemCategorySubCategoryByCategoryID(string ItemCategoryID)
        {
            return erpService.ReadItemCategorySubCategoryByCategoryID(ItemCategoryID);
        }

        public List<Inv_ItemCategorySubCategory> ReadItemCategorySubCatagory(string itemTypeID)
        {
            return erpService.ReadItemCategorySubCatagory(itemTypeID);
        }

        public List<Inv_ItemCategorySubCategory> ReadItemCategorySubCatagoryByTransactionType(string itemTypeID, string transactionType, byte locationType)
        {
            return erpService.ReadItemCategorySubCatagoryByTransactionType(itemTypeID, transactionType, locationType);
        }

        public bool ReadIsItemCodeWiseValidationExist(string itemCategory, string transactionType)
        {
            return erpService.ReadIsItemCodeWiseValidationExist(itemCategory, transactionType);
        }


        public List<Inv_RouteMaster> ReadRootCategory()
        {
            return erpService.ReadRootCategory();
        }
        public List<Inv_RouteMaster> ReadRouteMaster(string routeCategory)
        {
            return erpService.ReadRouteMaster(routeCategory);
        }
        public List<LocationInfo> ReadUnitList(string routeId)
        {
            return erpService.ReadUnitList(routeId);
        }

        //public List<EmployeeVisit> ReadEmployeeDetailsVisit(string empId, string locationCode)
        //{
        //    return erpService.ReadEmployeeDetailsVisit(empId, locationCode);
        //}


        public List<Inv_ItemModel> ReadPanelModelListForSHSDP(string packageCode)
        {
            return erpService.ReadPanelModelListForSHSDP(packageCode);
        }
        public List<Inv_ItemModel> ReadBatteryModelListForSHSDP(string packageCode)
        {
            return erpService.ReadBatteryModelListForSHSDP(packageCode);
        }
        public int CheckDuplicateDistributionPlan(string distribScheduleNo, DateTime scheduleDate, string refScheduleNo)
        {
            return erpService.CheckDuplicateDistributionPlan(distribScheduleNo, scheduleDate, refScheduleNo);
        }
        public SHSDistributionPlan_Master CreateSHSDistributionPlan(SHSDistributionPlan_Master objSHSDistributionPlan_Master, List<SHSDistributionPlan_RootWiseLocation> lstSHSDistributionPlan_RootWiseLocation, List<SHSDistributionPlan_RootWiseLocationNPackage> lstSHSDistributionPlan_RootWiseLocationNPackage, List<SHSDistributionPlan_IndividualItem> lstSHSDistributionPlan_IndividualItem)
        {
            return erpService.Create(objSHSDistributionPlan_Master, lstSHSDistributionPlan_RootWiseLocation, lstSHSDistributionPlan_RootWiseLocationNPackage, lstSHSDistributionPlan_IndividualItem);
        }
        public SHSDistributionPlan_Master UpdateSHSDistributionPlan(SHSDistributionPlan_Master objSHSDistributionPlan_Master, List<SHSDistributionPlan_RootWiseLocation> lstSHSDistributionPlan_RootWiseLocation, List<SHSDistributionPlan_RootWiseLocationNPackage> lstSHSDistributionPlan_RootWiseLocationNPackage, List<SHSDistributionPlan_IndividualItem> lstSHSDistributionPlan_IndividualItem)
        {
            return erpService.Update(objSHSDistributionPlan_Master, lstSHSDistributionPlan_RootWiseLocation, lstSHSDistributionPlan_RootWiseLocationNPackage, lstSHSDistributionPlan_IndividualItem);
        }

        public string SaveSHSDistributionRouteTransfer(string routeid, string txtDistributionScheduleNo, string txtDistributionScheduleNoNew,string dtpDelivaryDate,string txtDeliveryScheduleNo)
        {
            return erpService.SaveSHSDistributionRouteTransfer(routeid, txtDistributionScheduleNo, txtDistributionScheduleNoNew, dtpDelivaryDate, txtDeliveryScheduleNo);
        }

        public List<SHSDistributionPlanPackageORItem> RootWiseLocationNPackage(string distributionScheduleNo)
        {
            return erpService.RootWiseLocationNPackage(distributionScheduleNo);
        }

        public List<Inv_StoreLocation> ReadStoreLocation()
        {
            return erpService.ReadStoreLocation();
        }

        public List<Inv_StoreLocation> ReadStoreLocation(string itemType)
        {
            return erpService.ReadStoreLocation(itemType);
        }

        public List<Inv_StoreLocation> ReadStoreLocation(int storeLocationID, string itemTypeId)
        {
            return erpService.ReadStoreLocation(storeLocationID, itemTypeId);
        }

        public List<Inv_StoreLocation> ReadStoreLocationByItemTypeAndTransaction(string itemType, string itemTransType, byte locationType, string location)
        {
            return erpService.ReadStoreLocationByItemTypeAndTransaction(itemType, itemTransType, locationType, location);
        }

        public List<InvAvailableItemInALocation> ReadInvAvailableItemInALocation(string locationCode, byte storeLocation, string itemCategory)
        {
            return erpService.ReadInvAvailableItemInALocation(locationCode, storeLocation, itemCategory);
        }

        //public Inv_ItemSerialNoMaster ReadItemSerialNoMaster(string itemCode, string itemSerial, string itemCategory)
        //{
        //    return erpService.ReadItemSerialNoMaster(itemCode, itemSerial, itemCategory);
        //}

        public Inv_ItemNItemCategoryWithSerialNoMaster ReadItemSerialNoMasterByItemCategoryWise(string itemCode, string itemSerial, string itemCategory)
        {
            return erpService.ReadItemSerialNoMasterByItemCategoryWise(itemCode, itemSerial, itemCategory);
        }

        public int SaveItemStockWithSerialNoByLocation(string insertedRows, string locationCode)
        {
            return erpService.Create(insertedRows, locationCode);
        }

        public List<Inv_ItemStockWithSerialNoByLocation> ReadItemStockWithSerialNoByLocation(byte storeLocation, string locationCode, string itemCode)
        {
            return erpService.ReadItemStockWithSerialNoByLocation(storeLocation, locationCode, itemCode);
        }

        public Inv_ItemStockWithSerialNoByLocation ReadItemStockWithSerialNoByLocation(byte storeLocation, string locationCode, string itemCode, string serialNumber)
        {
            return erpService.ReadItemStockWithSerialNoByLocation(storeLocation, locationCode, itemCode, serialNumber);
        }

        public List<Inv_ItemStockByLocation> Update(List<Inv_ItemStockByLocation> lstItemStockByLocation, List<Inv_ItemStockWithSerialNoByLocation> lstItemStockWithSerialNoByLocation)
        {
            return erpService.Update(lstItemStockByLocation, lstItemStockWithSerialNoByLocation);
        }


        public void Delete(object obj)
        {
            erpService.Delete(obj);
        }

        public void Delete(object obj1, object obj2)
        {
            erpService.Delete(obj1, obj2);
        }

        public void Delete(Inv_ItemNItemCategoryWithSerialNoMaster objItemSerialMaster, Inv_ItemStockWithSerialNoByLocation objItemSerials)
        {
            erpService.Delete(objItemSerialMaster, objItemSerials);
        }

        public List<Inv_ItemTransactionTypes> ReadItemTransactionIssuedTypes(string itemType, byte locationType, string roleOrGroupID)
        {
            return erpService.ReadItemTransactionIssuedTypes(itemType, locationType, roleOrGroupID);
        }

        public List<Inv_ItemTransactionTypes> ReadItemTransactionReceivedTypes(string itemType, byte locationType, string roleOrGroupID)
        {
            return erpService.ReadItemTransactionReceivedTypes(itemType, locationType, roleOrGroupID);
        }

        public List<Inv_ItemTransactionTypes> ReadItemTransactionIssuedReceivedTypes(string itemType, string issueOrReceivedType, string roleOrGroupID)
        {
            return erpService.ReadItemTransactionIssuedReceivedTypes(itemType, issueOrReceivedType, roleOrGroupID);
        }

        public List<Inv_MRRMaster> ReadMRRMaster()
        {
            return erpService.ReadMRRMaster();
        }

        public Inv_MRRMaster ReadMRRMasterByChallanSeqNo(string challanSeqNumber, string challanLocationCode)
        {
            return erpService.ReadMRRMasterByChallanSeqNo(challanSeqNumber, challanLocationCode);
        }

        public Inv_MRRMaster SaveMRR(Inv_MRRMaster objMRRMaster, List<Inv_MRRDetails> lstMRRDetails, List<Inv_MRRDetailsWithSerialNo> lstMRRDetailsWithSerialNo)
        {
            return erpService.Create(objMRRMaster, lstMRRDetails, lstMRRDetailsWithSerialNo);
        }

        public Inv_MRRMaster SaveMRR(Inv_MRRMaster objMRRMaster, List<Inv_MRRDetails> lstMRRDetails, List<Inv_MRRDetailsWithSerialNo> lstMRRDetailsWithSerialNo, string serialTempTableRows)
        {
            return erpService.Create(objMRRMaster, lstMRRDetails, lstMRRDetailsWithSerialNo, serialTempTableRows);
        }

        public Inv_MRRMaster SaveMRR(Inv_MRRMaster objMRRMaster, List<Inv_MRRDetails> lstMRRDetails, List<Inv_MRRDetailsWithSerialNo> lstMRRDetailsWithSerialNo, string serialTempTableRows, Aud_AuditAdjustmentRelatedChallanOrMRRForReference objChallanOrMRRForAuditAdjustment)
        {
            return erpService.Create(objMRRMaster, lstMRRDetails, lstMRRDetailsWithSerialNo, serialTempTableRows, objChallanOrMRRForAuditAdjustment);
        }

        public List<Inv_ItemMaster> ReadInvItems(string itemCategory)
        {
            return erpService.ReadInvItems(itemCategory);
        }

        public List<Inv_ItemMaster> ReadInvItems(string itemCategory, string itemTransType)
        {
            return erpService.ReadInvItems(itemCategory, itemTransType);
        }

        public List<Inv_ItemMaster> ReadItemMasterByItemCategory(string itemCategory)
        {
            return erpService.ReadItemMasterByItemCategory(itemCategory);
        }

        public Inv_ItemMaster ReadItemMaster(string itemCode)
        {
            return erpService.ReadItemMaster(itemCode);
        }
        public List<Inv_ItemMaster> ReadItemMasterForItemCode(string itemCode)
        {
            return erpService.ReadItemMasterForItemCode(itemCode);
        }


        public Inv_ItemStockByLocation ReadItemStockByLocation(byte storeLocation, string locationCode, string itemCode)
        {
            return erpService.ReadItemStockByLocation(storeLocation, locationCode, itemCode);
        }

        public Inv_ChallanMaster ReadChallanMaster(string locationCode, string challanSequenceNumber)
        {
            return erpService.ReadChallanMaster(locationCode, challanSequenceNumber);
        }

        public bool IsItemTransationRelatedToVendor(string itemTransactionType)
        {
            return erpService.IsItemTransationRelatedToVendor(itemTransactionType);
        }

        public List<ChallanInboxForChallanWithMRR> ReadChallanInbox(string locationCode, string itemType)
        {
            return erpService.ReadChallanInbox(locationCode, itemType);
        }

        public Inv_ChallanMaster ReadChallanMasterByChallanSequence(string challanSequenceNumber, string locationCode)
        {
            return erpService.ReadChallanMasterByChallanSequence(challanSequenceNumber, locationCode);
        }

        public List<Inv_ChallanMaster> ReadChallanMaster(string locationCode)
        {
            return erpService.ReadChallanMaster(locationCode);
        }

        public string ChallanSequenceNumberMax(string locationCode, string yearMonthDate)
        {
            return erpService.ChallanSequenceNumberMax(locationCode, yearMonthDate);
        }

        public Inv_ChallanMaster SaveIssueChallan(Inv_ChallanMaster objChallanMaster, List<Inv_ChallanDetails> lstChallanDetails, List<Inv_ChallanDetailsWithSerialNo> lstChallanDetailsWithSerialNo)
        {
            return erpService.Create(objChallanMaster, lstChallanDetails, lstChallanDetailsWithSerialNo);
        }

        public Inv_ChallanMaster SaveIssueChallan(Inv_ChallanMaster objChallanMaster, List<Inv_ChallanDetails> lstChallanDetails, List<Inv_ChallanDetailsWithSerialNo> lstChallanDetailsWithSerialNo, List<Inv_ChallanWithMultipleLocations> lstChallanWithMultipleLocations, List<Inv_ChallanWithMultipleLocationsWithItemWiseDistribution> lstChallanItemDistribution)
        {
            return erpService.Create(objChallanMaster, lstChallanDetails, lstChallanDetailsWithSerialNo, lstChallanWithMultipleLocations, lstChallanItemDistribution);
        }

        public string MRRSequenceNumberMax(string locationCode, string yearMonthDate)
        {
            return erpService.MRRSequenceNumberMax(locationCode, yearMonthDate);
        }

        public List<MRRDetails> ReadMRRDetails(string locationCode, string mrrLocationCode, string challanSequenceNumber)
        {
            return erpService.ReadMRRDetails(locationCode, mrrLocationCode, challanSequenceNumber);
        }

        public Int16 ItemTransactionSequenceMAX(byte storeLocation, string yearMonth, string locationCode, string itemCode, DateTime transDate)
        {
            return erpService.ItemTransactionSequenceMAX(storeLocation, yearMonth, locationCode, itemCode, transDate);
        }

        public List<SalGetAvailableSerialNoForMRR> ReadChallanDetailsWithSerialNo(string challanSequenceNumber, string itemCode, string refDocType, string challanLocationCode)
        {
            return erpService.ReadChallanDetailsWithSerialNo(challanSequenceNumber, itemCode, refDocType, challanLocationCode);
        }

        public bool IsInventoryStockUpdateFinish(string locationCode)
        {
            return erpService.IsInventoryStockUpdateFinish(locationCode);
        }

        public LocationInfo Location(string LocationCode)
        {
            return erpService.Location(LocationCode);
        }

        public List<ItemLedgerReport> ReadInvItemLedger(byte storeLocation, string locationCode, string itemCode, string fromDate, string toDate)
        {
            return erpService.ReadInvItemLedger(storeLocation, locationCode, itemCode, fromDate, toDate);
        }

        public List<InvChallanOrMRRDetailsForItemLedger> ReadInvChallanDetails(string locationCode, string challanOrMRRSeqNo, string challanOrMRR)
        {
            return erpService.ReadInvChallanDetails(locationCode, challanOrMRRSeqNo, challanOrMRR);
        }

        public List<InventorySummaryReportV2> ReadInventorySummaryReportV2(string reportType, string itemType, byte storeLocation, string locationCode, DateTime startDate, DateTime endDate, string vendorType)
        {
            return erpService.ReadInventorySummaryReportV2(reportType, itemType, storeLocation, locationCode, startDate, endDate, vendorType);
        }

        public List<InventoryERPVersusPhysicalBalance> ReadInventoryERPVersusPhysicalBalance(string reportType, string itemType, byte storeLocation, string locationCode, string yearMonth, string vendorType)
        {
            return erpService.ReadInventoryERPVersusPhysicalBalance(reportType, itemType, storeLocation, locationCode, yearMonth, vendorType);
        }

        public Inv_ERPVersusPhysicalBalance Update(Inv_ERPVersusPhysicalBalance objInvERPVersusPhysicalBalance, List<Inv_ERPVersusPhysicalBalance> lstInvERPVersusPhysicalBalance)
        {
            return erpService.Update(objInvERPVersusPhysicalBalance, lstInvERPVersusPhysicalBalance);
        }

        public string ChallanOrMRRForAuditAdjustmentSequenceMax(string locationCode, string yearMonthDate)
        {
            return erpService.ChallanOrMRRForAuditAdjustmentSequenceMax(locationCode, yearMonthDate);
        }

        public Inv_ChallanMaster SaveChallanOrMRRForAuditAdjustment(Inv_ChallanMaster objChallanMaster, List<Inv_ChallanDetails> lstChallanDetails, List<Inv_ChallanDetailsWithSerialNo> lstChallanDetailsWithSerialNo, Aud_AuditAdjustmentRelatedChallanOrMRRForReference objChallanOrMRRForAuditAdjustment)
        {
            return erpService.Create(objChallanMaster, lstChallanDetails, lstChallanDetailsWithSerialNo, objChallanOrMRRForAuditAdjustment);
        }

        public List<FixedAssetSerialList> GetFixedAssetSerialList(byte storeLocation, string locationCode, string itemCode, string option)
        {
            RASolarHRMSService hrmService = new RASolarHRMSService();

            List<FixedAssetSerialList> lstFixedAssetSerial = new List<FixedAssetSerialList>();
            lstFixedAssetSerial = erpService.GetFixedAssetSerialList(storeLocation, locationCode, itemCode, option);

            //string[] aa = lstFixedAssetSerial.Select(s => s.EmployeeID).Distinct().ToArray();
            //List<string> uids = new List<string>(aa);

            //List<EmployeeDetailsInfo> employeeInfo = new List<EmployeeDetailsInfo>();
            //employeeInfo = hrmService.ReadEmployeeInfo(uids);

            //if (employeeInfo.Count() > 0)
            //{
            //    List<FixedAssetSerialList> lstFixedAssetSerial1 = new List<FixedAssetSerialList>();
            //    lstFixedAssetSerial1 = (from cdrd in lstFixedAssetSerial
            //                            join emi in employeeInfo
            //                            on cdrd.EmployeeID equals emi.EmployeeID
            //                            select new FixedAssetSerialList
            //                            {
            //                                AllocationSeqNo = cdrd.AllocationSeqNo,
            //                                ItemSerialNo = cdrd.ItemSerialNo,
            //                                EmployeeID = cdrd.EmployeeID,
            //                                EmployeeName = emi.EmployeeName,
            //                                AllocationDate = cdrd.AllocationDate,
            //                                Remarks = cdrd.Remarks,

            //                            }).ToList();

            //    lstFixedAssetSerial = (from ss in lstFixedAssetSerial
            //                           where !(from aaa in lstFixedAssetSerial1 select aaa.EmployeeID).Contains(ss.EmployeeID)
            //                           select ss
            //                           ).ToList();


            //    lstFixedAssetSerial = lstFixedAssetSerial.Union(lstFixedAssetSerial1).ToList();

            //    lstFixedAssetSerial = lstFixedAssetSerial.Union(lstFixedAssetSerial1).ToList();
            //}

            return lstFixedAssetSerial;
        }

        public List<AssetAssign> FixedAssetAssignUnassign(string locationCode, string itemCode, byte storeLocation, string employeeId, byte status, string unAssignedQuantity)
        {
            return erpService.FixedAssetAssignUnassign(locationCode, itemCode, storeLocation, employeeId, status, unAssignedQuantity);
        }

        public string EmployeeWiseFixedAssetsAllocationSequenceNumberMax(string locationCode, string employeeId, string itemCode, byte storeLocation)
        {
            return erpService.EmployeeWiseFixedAssetsAllocationSequenceNumberMax(locationCode, employeeId, itemCode, storeLocation);
        }

        public ArrayList GetFixedAssetAssignedOrUnassignedSeialList(byte storeLocation, string locationCode, string itemCode, string employeeId, int assignQuantity)
        {
            return erpService.GetFixedAssetAssignedOrUnassignedSeialList(storeLocation, locationCode, itemCode, employeeId, assignQuantity);
        }

        public Fix_EmployeeWiseFixedAssetsAllocation SaveFisedAssetFroEmployee(Fix_EmployeeWiseFixedAssetsAllocation objAssetAssign, List<Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo> lstAssetsAllocationWithSerialNo)
        {
            return erpService.CreateForCashMemo(objAssetAssign, lstAssetsAllocationWithSerialNo);
        }

        public List<AvailableNAssignFixedAsset> ReadAvailableNAssignFixedAsset(byte storeLocation, string locationCode, string itemCategory, string employeeSelectionOption, string employeeID)
        {
            return erpService.ReadAvailableNAssignFixedAsset(storeLocation, locationCode, itemCategory, employeeSelectionOption, employeeID);
        }

        public List<ChallanOrMRRItemsSerial> ReadChallanOrMRRItemsSerial(string locationCode, string challanSequenceNumber, string mrrSequenceNumber, string itemCode)
        {
            return erpService.ReadChallanOrMRRItemsSerial(locationCode, challanSequenceNumber, mrrSequenceNumber, itemCode);
        }

        public ArrayList ReadVendorChallanWithItemReferenceToCheck(string deliveryNoteNo)
        {
            return erpService.ReadVendorChallanWithItemReferenceToCheck(deliveryNoteNo);
        }

        public Array MrrSequenceListForMaterialReceiving(string rreDeliveryNote, string locationCode)
        {
            return erpService.MrrSequenceListForMaterialReceiving(rreDeliveryNote, locationCode);
        } 

        public List<MaterialRecevingReportMrrDetails> MaterialReceivingMrrDetails(string LocationCode, string VendorRefChallanNo, string mrrSequenceNumber, string billNo)
        {
            return erpService.MaterialReceivingMrrDetails(LocationCode, VendorRefChallanNo, mrrSequenceNumber, billNo);
        }
        
        public List<DeliveryNoteSummary> ReadDeliveryNoteSummary(string delivarySchduleNo)
        {
            return erpService.ReadDeliveryNoteSummary(delivarySchduleNo);
        }

        public List<DeliveryItemNoteReportSummary> ReadDeliveryItemNoteReportSummary(string delivarySchduleNo)
        {
            return erpService.ReadDeliveryItemNoteReportSummary(delivarySchduleNo);
        }

        public List<DeliveryPackageNoteReportSummary> ReadDeliveryPackageNoteReportSummary(string delivarySchduleNo)
        {
            return erpService.ReadDeliveryPackageNoteReportSummary(delivarySchduleNo); 
        }
        //start for monthly
        public List<DeliveryNoteSummary> ReadMonthlyDeliveryNoteSummary(string monthYear) 
        {
            return erpService.ReadMonthlyDeliveryNoteSummary(monthYear);
        }

        public List<DeliveryItemNoteReportSummary> ReadMonthlyDeliveryItemNoteReportSummary(string monthYear)
        {
            return erpService.ReadMonthlyDeliveryItemNoteReportSummary(monthYear);
        }

        public List<DeliveryPackageNoteReportSummary> ReadMonthlyDeliveryPackageNoteReportSummary(string monthlYear)
        {
            return erpService.ReadMonthlyDeliveryPackageNoteReportSummary(monthlYear);
        }
        //end for monthly

        //start Date Wise
        public List<DeliveryNoteSummary> ReadDateWiseDeliveryNoteSummary(string fromDate, string toDate)
        {
            return erpService.ReadDateWiseDeliveryNoteSummary(fromDate,toDate);
        }

        public List<DeliveryItemNoteReportSummary> ReadDateWiseDeliveryItemNoteReportSummary(string fromDate, string toDate)
        {
            return erpService.ReadDateWiseDeliveryItemNoteReportSummary(fromDate, toDate);
        }

        public List<DeliveryPackageNoteReportSummary> ReadDateWiseDeliveryPackageNoteReportSummary(string fromDate, string toDate)
        {
            return erpService.ReadDateWiseDeliveryPackageNoteReportSummary(fromDate, toDate);
        }
        //end Date Wise

        public ArrayList ReadMRRNDeliveryNoteValue(string locationCode, string mrrSequenceNumber, string yearMonth, string rreDeliveryNote)
        {
            return erpService.ReadMRRNDeliveryNoteValue(locationCode, mrrSequenceNumber, yearMonth, rreDeliveryNote);
        }
        public List<ChallanInformationGlanceDetails> ReadChallanInformationGlanceList(string itemType, string locationCode, DateTime dateFrom, DateTime dateTo)
        {
            return erpService.ReadChallanInformationGlanceList(itemType, locationCode, dateFrom, dateTo);
        }
        public List<MrrInformationGlanceDetails> ReadMrrInformationGlanceList(string itemType, string locationCode, DateTime dateFrom, DateTime dateTo)
        {
            return erpService.ReadMrrInformationGlanceList(itemType, locationCode, dateFrom, dateTo);
        }

        public List<StockInTransitAtGlanceDetails> ReadStockInTransitAtGlanceList(string challanType, string locationCode, DateTime dateFrom, DateTime dateTo)
        {
            return erpService.ReadStockInTransitAtGlanceList(challanType, locationCode, dateFrom, dateTo);
        }

        public ArrayList ReadVendorRefChallanNo(string billNO)
        {
            return erpService.ReadVendorRefChallanNo(billNO);
        }

        public string AuditSequenceNumberMax(string locationCode, string yearMonthDate)
        {
            return erpService.AuditSequenceNumberMax(locationCode, yearMonthDate);
        }

        public List<AvailableNAssignFixedAsset> ReadAvailableNAssignCashMemo(byte storeLocation, string locationCode, string itemCategory, string employeeSelectionOption, string employeeID)
        {
            return erpService.ReadAvailableNAssignCashMemo(storeLocation, locationCode, itemCategory, employeeSelectionOption, employeeID);
        }

        public List<AssignCashMemoBook> CashMemoAssignUnassign(string locationCode, string itemCode, byte storeLocation, string employeeId, byte status, string unAssignedQuantity)
        {
            return erpService.CashMemoAssignUnassign(locationCode, itemCode, storeLocation, employeeId, status, unAssignedQuantity);
        }

        public ArrayList GetCashMemoAssignedOrUnassignedSeialList(byte storeLocation, string locationCode, string itemCode, string employeeId, int assignQuantity)
        {
            return erpService.GetCashMemoAssignedOrUnassignedSeialList(storeLocation, locationCode, itemCode, employeeId, assignQuantity);
        }

        public Fix_EmployeeWiseFixedAssetsAllocation SaveForCashMemoAllocation(Fix_EmployeeWiseFixedAssetsAllocation objAssetAssign, List<Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo> lstAssetsAllocationWithSerialNo)
        {
            return erpService.CreateForCashMemo(objAssetAssign, lstAssetsAllocationWithSerialNo);
        }

        public Fix_EmployeeWiseFixedAssetsAllocation UpdateCashmemoAssign(Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo objEmployeeWiseFixedAssetsAllocationWithSerialNo, Fix_EmployeeWiseFixedAssetsAllocation objAssetAssign)
        {
            return erpService.UpdateCashmemoAssign(objEmployeeWiseFixedAssetsAllocationWithSerialNo, objAssetAssign);
        }

        public List<Inv_VendorInfo> GetVendorListForItemSummary()
        {
            return erpService.GetVendorListForItemSummary();
        }

        public ArrayList GetVendorList(string itemTransactionType)
        {
            return erpService.GetVendorList(itemTransactionType);
        }

        public List<FixedAssetSerialList> GetCashMemoSerialList(byte storeLocation, string locationCode, string itemCode, string option)
        {
            RASolarHRMSService hrmService = new RASolarHRMSService();

            List<FixedAssetSerialList> lstFixedAssetSerial = new List<FixedAssetSerialList>();
            lstFixedAssetSerial = erpService.GetCashMemoSerialList(storeLocation, locationCode, itemCode, option);

            //string[] aa = lstFixedAssetSerial.Select(s => s.EmployeeID).Distinct().ToArray();
            //List<string> uids = new List<string>(aa);

            //List<EmployeeDetailsInfo> employeeInfo = new List<EmployeeDetailsInfo>();
            //employeeInfo = hrmService.ReadEmployeeInfo(uids);

            //if (employeeInfo.Count() > 0)
            //{
            //    List<FixedAssetSerialList> lstFixedAssetSerial1 = new List<FixedAssetSerialList>();
            //    lstFixedAssetSerial1 = (from cdrd in lstFixedAssetSerial
            //                            join emi in employeeInfo
            //                            on cdrd.EmployeeID equals emi.EmployeeID
            //                            select new FixedAssetSerialList
            //                            {
            //                                AllocationSeqNo = cdrd.AllocationSeqNo,
            //                                ItemSerialNo = cdrd.ItemSerialNo,
            //                                EmployeeID = cdrd.EmployeeID,
            //                                EmployeeName = emi.EmployeeName,
            //                                AllocationDate = cdrd.AllocationDate,
            //                                ItemCode = cdrd.ItemCode,
            //                                Remarks = cdrd.Remarks,

            //                            }).ToList();

            //    lstFixedAssetSerial = (from ss in lstFixedAssetSerial
            //                           where !(from aaa in lstFixedAssetSerial1 select aaa.EmployeeID).Contains(ss.EmployeeID)
            //                           select ss
            //                           ).ToList();


            //    lstFixedAssetSerial = lstFixedAssetSerial.Union(lstFixedAssetSerial1).ToList();

            //    lstFixedAssetSerial = lstFixedAssetSerial.Union(lstFixedAssetSerial1).ToList();
            //}

            return lstFixedAssetSerial;
        }

        public List<CashMemoBookPagesStatus> GetStatusForCashMemoBookAllocation(string itemSerialNo)
        {
            return erpService.GetStatusForCashMemoBookAllocation(itemSerialNo);
        }

        public CustomerDetails GetCustomerDetailsForWarrantyClaim(string customerCode, string unitCode)
        {
            return erpService.GetCustomerDetailsForWarrantyClaim(customerCode, unitCode);
        }

        public List<WarrantyItemsDetails> GetWarrantyItemsList(string customerCode, DateTime dayOpenDate)
        {
            return erpService.GetWarrantyItemsList(customerCode, dayOpenDate);
        }

        public Inv_MRRMaster SaveWarrantyClaimNSettlement(Inv_MRRMaster objMRRMaster, List<Inv_MRRDetails> lstMRRDetails, List<Inv_MRRDetailsWithSerialNo> lstMRRDetailsWithSerialNo, Inv_ChallanMaster objChallanMaster, List<Inv_ChallanDetails> lstChallanDetails, List<Inv_ChallanDetailsWithSerialNo> lstChallanDetailsWithSerialNo)
        {
            return erpService.SaveWarrantyClaimNSettlement(objMRRMaster, lstMRRDetails, lstMRRDetailsWithSerialNo, objChallanMaster, lstChallanDetails, lstChallanDetailsWithSerialNo);
        }

        public string ItemSerialCorrectionChangeNClear(string locationCode, string itemCode, string optionForCorrection, string wrongSerialNo, string correctSerialNo, string loginID)
        {
            return erpService.ItemSerialCorrectionChangeNClear(locationCode, itemCode, optionForCorrection, wrongSerialNo, correctSerialNo, loginID);
        }

        public List<ItemSerialCorrection> ReadItemSerialCorrectionInformation(string locationCode, string itemCode, string optionForCorrection, string wrongSerialNo, string correctSerialNo, string loginID)
        {
            return erpService.ReadItemSerialCorrectionInformation(locationCode, itemCode, optionForCorrection, wrongSerialNo, correctSerialNo, loginID);
        }

        public List<WarrantyClaimReason> GetWarrantyClaimReason() 
        {    
            return erpService.GetWarrantyClaimReason();
        }

        public string CheckingIssueItemSerailNO(string locationCode, string itemSerial, string itemCode, string storeLocationForIssue)
        {
            return erpService.CheckingIssueItemSerailNO(locationCode, itemSerial, itemCode, storeLocationForIssue);
        }

        public List<ItemInfo> GetSubstituteItemCode(string itemCode)
        {
            return erpService.GetSubstituteItemCode(itemCode);
        }
    }
}