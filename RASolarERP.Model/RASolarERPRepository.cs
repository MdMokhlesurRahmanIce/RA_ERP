using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

using RASolarERP.Model;
using RASolarHelper;
using RASolarERP.DomainModel.SalesModel;
using RASolarERP.DomainModel.InventoryModel;
using System.Data.Objects;
using System.Collections;
using System.Data.SqlClient;
using System.Data.Objects.SqlClient;
using System.Data;
using System.Data.Entity.Validation;
using RASolarERP.DomainModel.AMSModel;
using RASolarERP.DomainModel.HRMSModel;

namespace RASolarERP.Model
{
    public class RASolarERPRepository : IRASolarERPRepository, IDisposable
    {
        #region Properties And Constructor

        private RASolarMISEntities context { get; set; }

        public RASolarERPRepository(RASolarMISEntities _context)
        {
            context = _context;
            ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext.CommandTimeout = 180;
        }
        #endregion

        #region Read Methods

        public IQueryable<Common_ZoneInfo> Zone()
        {
            return context.Common_ZoneInfo.Where(z => z.Status == Helper.Active);
        }

        public Common_ZoneInfo Zone(string zoneCode)
        {
            return context.Common_ZoneInfo.Where(z => z.Zone_Code == zoneCode && z.Status == Helper.Active).FirstOrDefault();
        }
        public IQueryable<Common_RegionInfo> Region()
        {
            return context.Common_RegionInfo.Where(r => r.Status == Helper.Active);
        }

        public Common_RegionInfo Region(string regionCode)
        {
            return context.Common_RegionInfo.Where(r => r.Reg_Code == regionCode && r.Status == Helper.Active).FirstOrDefault();
        }
        public IQueryable<Common_RegionInfo> RegionByZoneCode(string zoneCode)
        {
            return context.Common_RegionInfo.Where(r => r.Zone_Code == zoneCode && r.Status == Helper.Active);
        }

        public IQueryable<Common_UnitInfo> Unit()
        {
            return context.Common_UnitInfo.Where(u => u.Status == Helper.Active);
        }
        public Common_UnitInfo Unit(string unitCode)
        {
            return context.Common_UnitInfo.Where(u => u.Unit_Code == unitCode && u.Status == Helper.Active).FirstOrDefault();
        }
        public IQueryable<Common_UnitInfo> UnitByRegionCode(string regionCode)
        {
            return context.Common_UnitInfo.Where(u => u.Reg_Code == regionCode && u.Status == Helper.Active);
        }

        public int LastUsedCustomerSerial(string unitCode, string programCode)
        {
            string customerSerialQuery = "Select css.LastUsedCustomerSerial From Sal_LocationNProgramWiseLastUsedCustomerSerial css " +
                                         "Where css.LocationCode = '" + unitCode + "' And css.ProgramCode = '" + programCode + "'";

            int lastUsedCustomerSerial = 0;
            var v = context.Database.SqlQuery<int>(customerSerialQuery);

            if (v != null)
            {
                lastUsedCustomerSerial = Convert.ToInt32(v.FirstOrDefault());
            }

            return lastUsedCustomerSerial;

            //return context.Common_UnitInfo.Where(u => u.Unit_Code == unitCode && u.Status == Helper.Active).FirstOrDefault().LastUsedCustomerSerial;
        }

        public List<Common_UnionInfo> ReadUnionInfo(string upazilaCode)
        {
            return context.Common_UnionInfo.Where(u => u.UpazilaCode == upazilaCode && u.Status == Helper.Active).ToList();
        }


        public List<Common_PostOfficeInfo> ReadPostOfficeInfo(string upazilaCode) 
        {
            string postOfficeInfoQuery = " SELECT DISTINCT po.PostOfficeID,po.PostOfficeName "
                                         +" FROM Sal_Validation_UnitVsUpazilla uv "
                                         +" INNER JOIN Common_PostOfficeInfo po ON po.UpazilaCode = uv.UpazillaCode "
                                         +" WHERE UnitCode = '" + upazilaCode + "'";

            var postOfficeInfo = context.Database.SqlQuery<Common_PostOfficeInfo>(postOfficeInfoQuery);

            return postOfficeInfo.ToList();
        }


        public List<Common_PostOfficeInfo> PostOfficeInfoLoadForUnitCode(string unitCode)
        {
            string PostOfficeInfoLoadForUnitCodeQuery = "  SELECT DISTINCT po.PostOfficeID,po.PostOfficeName  FROM Sal_Validation_UnitVsUpazilla uv  "
                                                        + "  INNER JOIN Common_PostOfficeInfo po ON po.UpazilaCode = uv.UpazillaCode  WHERE UnitCode = '" + unitCode + "'";

            return context.Database.SqlQuery<Common_PostOfficeInfo>(PostOfficeInfoLoadForUnitCodeQuery).ToList(); 
        }




        public List<Common_PostOfficeInfo> ReadCustomerPostOfficeInfo(string CustomerCode, string PostOffice)
        {
            string postOfficeInfoQuery = "SELECT PostOfficeID,PostOfficeName FROM Sal_Customer c "
                                          + " INNER JOIN Common_PostOfficeInfo po ON po.PostOfficeID = c.PostOffice  "
                                          + " WHERE c.PostOffice = '" + PostOffice + "' AND c.CustomerCode = '" + CustomerCode + "' ";
                                   

            var postOfficeInfo = context.Database.SqlQuery<Common_PostOfficeInfo>(postOfficeInfoQuery);

            return postOfficeInfo.ToList(); 
        }

        //Code By T.Alam
        public List<Sal_SparePartsSales_DataFromExternalSources> ReadSalesExternalSourceByLCode(string locationCode)
        {
            //return context.Sal_SparePartsSales_DataFromExternalSources.Where(t => t.LocationCode == locationCode).ToList();
            return context.Sal_SparePartsSales_DataFromExternalSources.Where(t => t.LocationCode == locationCode && t.IsTransferredToFinalTable==null).ToList();

        }

        public List<Sal_SalesAgreementPrePost_DataFromExternalSources> ReadSalesResalesAgrExternalSourceByLCode(string locationCode)
        {
            return context.Sal_SalesAgreementPrePost_DataFromExternalSources.Where( t => t.UnitCode == locationCode && t.IsTransferredToFinalTable == null ).ToList();
        }



        public List<Common_DistrictInfo> ReadDistrictInfo(string upazilaCode)
        {
            //var districtQuery = from di in context.Common_DistrictInfo
            //                   join up in context.Common_UpazillaInfo on di.DIST_CODE equals up.DIST_CODE
            //                   where up.UPAZ_CODE == upazilaCode
            //                   select new Common_DistrictInfo
            //                   {
            //                       DIST_CODE = di.DIST_CODE,
            //                       DIST_NAME=di.DIST_NAME
            //                   };
            //return districtQuery.ToList();


            string districtQuery = " SELECT * FROM Common_DistrictInfo di  " +
                                   " INNER JOIN Common_UpazillaInfo up ON up.DIST_CODE = di.DIST_CODE " +
                                   " WHERE up.UPAZ_CODE = '" + upazilaCode + "' ";

            var district = context.Database.SqlQuery<Common_DistrictInfo>(districtQuery);

            return district.ToList();
        }

        public List<ClosingInventoryValuation> ClosingInventoryReport(string yearMonth)
        {
            return context.ClosingInventoryValuation(yearMonth).ToList();
        }

        public int CheckDuplicateDistributionPlan(string distribScheduleNo, DateTime scheduleDate, string refScheduleNo)
        {
            return context.SHSDistributionPlan_Master.Where(i => i.DistribScheduleNo==distribScheduleNo  && i.ScheduleDate == scheduleDate && i.RefScheduleNo == refScheduleNo).Count();
        }

        public List<SHSDistributionPlanPackageORItem> RootWiseLocationNPackage(string distributionScheduleNo)
        {
            // return context.SHSDistributionPlan_RootWiseLocationNPackage.Where(i => i.DistribScheduleNo == distributionScheduleNo).ToList();
            //orginal
            string sqlQuery = "(SELECT RNP.DistribScheduleNo,RNP.RouteNo, RM.RouteName,RM.RouteCategory, RNP.LocationCode,CLI.LocationName,'Package' Category, RNP.PackageCode PackageOrItemCode,PM.[Description] PackageName, RNP.PanelModel,PIM.[Description] PanelModelName, RNP.BatteryModel,ItemIM.[Description] BatteryModelName, RNP.PackageQuantity PackageOrItemQuantity  " +
                            " FROM SHSDistributionPlan_RootWiseLocationNPackage RNP INNER JOIN Sal_PackageMaster PM " +
                            " ON RNP.PackageCode=PM.PackageCode INNER JOIN Inv_ItemModel PIM ON RNP.PanelModel=PIM.ItemModelID and PIM.ItemCategory= 'PANEL1' " +
                            " INNER JOIN Inv_ItemModel ItemIM ON RNP.BatteryModel=ItemIM.ItemModelID and ItemIM.ItemCategory='BAT001' " +
                            " INNER JOIN Inv_RouteMaster RM ON RNP.RouteNo=RM.RouteNo " +
                            " INNER JOIN Common_LocationInfo CLI ON RNP.LocationCode=CLI.LocationCode " +
                            " WHERE DistribScheduleNo = '" + distributionScheduleNo + "') " +
                            " UNION ALL " +
                            " (SELECT DistribScheduleNo,InItem.RouteNo,RM.RouteName,RM.RouteCategory,InItem.LocationCode,ClI.LocationName, 'Item' Category, IM.ItemCode PackageOrItemCode, IM.ItemName PackageName,  NULL PanelModel, NULL PanelModelName,  NULL BatteryModel,  NULL BatteryModelName, ItemQuantity PackageOrItemQuantity " +
                            " FROM SHSDistributionPlan_IndividualItem InItem INNER JOIN Inv_ItemMaster IM " +
                            " ON InItem.ItemCode=IM.ItemCode " +
                            " INNER JOIN Inv_RouteMaster RM ON InItem.RouteNo=RM.RouteNo " +
                            " INNER JOIN Common_LocationInfo CLI ON InItem.LocationCode=CLI.LocationCode " +
                            " WHERE DistribScheduleNo = '" + distributionScheduleNo + "') ";  

            //change..
            //string sqlQuery = "( SELECT [RefScheduleNo],[ScheduleDate],[ScheduleNotes],[RouteNo],RouteName as RootName,[LocationCode],[LocationName],[ChallanNumber],[ItemCode] "
            //                   +" ,[ItemName],SUM([ItemQuantity]) [ItemQuantity] FROM  (SELECT dpm.[RefScheduleNo],dpm.ScheduleDate,dpm.ScheduleNotes,pwi.[RouteNo],rm.RouteName,pwi.[LocationCode],li.[LocationName] "
            //                   + " ,'RRE-'+rwl.[VendorChallanNoForPackage] [ChallanNumber],pwi.[ItemCode],im.ItemName,pwi.[ItemQuantity] FROM [SHSDistributionPlan_PackageWiseItem] pwi INNER JOIN [Common_LocationInfo] li ON pwi.LocationCode = li.LocationCode  "
            //                   + "  INNER JOIN SHSDistributionPlan_Master dpm ON dpm.DistribScheduleNo = pwi.DistribScheduleNo  INNER JOIN SHSDistributionPlan_RootWiseLocation rwl ON rwl.DistribScheduleNo = pwi.DistribScheduleNo "
            //                  +"  AND rwl.RouteNo = pwi.RouteNo AND rwl.LocationCode = pwi.LocationCode  INNER JOIN Inv_RouteMaster rm ON rm.RouteNo = pwi.RouteNo  "  
            //                  +"  INNER JOIN Inv_ItemMaster im ON im.ItemCode = pwi.ItemCode  WHERE pwi.[DistribScheduleNo] = '"+distributionScheduleNo+"' AND pwi.[ItemQuantity] <> 0  "
            //                  +" UNION ALL SELECT dpm.[RefScheduleNo],dpm.ScheduleDate,dpm.ScheduleNotes,ii.RouteNo,rm.RouteName,ii.LocationCode,li.LocationName "
            //                  +"  ,'RRE-'+rwl.[VendorChallanNoForSpareParts] [ChallanNumber],ii.ItemCode,im.ItemName,ii.ItemQuantity   " 
            //                  +"  FROM SHSDistributionPlan_IndividualItem ii  INNER JOIN [Common_LocationInfo] li ON ii.LocationCode = li.LocationCode   "
            //                  +" INNER JOIN SHSDistributionPlan_Master dpm ON dpm.DistribScheduleNo = ii.DistribScheduleNo   "
            //                 +" INNER JOIN SHSDistributionPlan_RootWiseLocation rwl ON rwl.DistribScheduleNo = ii.DistribScheduleNo AND rwl.RouteNo = ii.RouteNo "
            //                 +" AND rwl.LocationCode = ii.LocationCode INNER JOIN Inv_RouteMaster rm ON rm.RouteNo = ii.RouteNo INNER JOIN Inv_ItemMaster im ON im.ItemCode = ii.ItemCode    "
            //                 + "  WHERE ii.DistribScheduleNo = '" + distributionScheduleNo + "' AND ii.[ItemQuantity] <> 0) a   "
            //                 +" GROUP BY [RefScheduleNo],[ScheduleDate],[ScheduleNotes],[RouteNo],[RouteName],[LocationCode],[LocationName],[ChallanNumber],[ItemCode],[ItemName]   "
            //                 +"  ORDER BY [RefScheduleNo],ScheduleDate,[RouteNo],[LocationCode],[ItemCode]  )";

            var results = context.Database.SqlQuery<SHSDistributionPlanPackageORItem>(sqlQuery);

            return results.ToList();

        }


        public List<SHSDelivaryNoteChallan> ViewDeliveryNoteChallanSHSReport(string distribScheduleNo)
        {
            string sqlQuery = " SELECT [RefScheduleNo],[ScheduleDate],[ScheduleNotes],[RouteNo],RouteName as RootName,[LocationCode],[LocationName],[ChallanNumber],[ItemCode],[ItemName],SUM([ItemQuantity]) [ItemQuantity] FROM " +
                            "  (SELECT dpm.[RefScheduleNo],dpm.ScheduleDate,dpm.ScheduleNotes,pwi.[RouteNo],rm.RouteName,pwi.[LocationCode],li.[LocationName],'RRE-'+rwl.[VendorChallanNoForPackage] [ChallanNumber],pwi.[ItemCode],im.ItemName,pwi.[ItemQuantity]" +
                            "  FROM [SHSDistributionPlan_PackageWiseItem] pwi INNER JOIN [Common_LocationInfo] li ON pwi.LocationCode = li.LocationCode" +
                            "  INNER JOIN SHSDistributionPlan_Master dpm ON dpm.DistribScheduleNo = pwi.DistribScheduleNo " +
                            "  INNER JOIN SHSDistributionPlan_RootWiseLocation rwl ON rwl.DistribScheduleNo = pwi.DistribScheduleNo AND rwl.RouteNo = pwi.RouteNo AND rwl.LocationCode = pwi.LocationCode " +
                            "  INNER JOIN Inv_RouteMaster rm ON rm.RouteNo = pwi.RouteNo   " +
                            "  INNER JOIN Inv_ItemMaster im ON im.ItemCode = pwi.ItemCode   " +
                            "  WHERE pwi.[DistribScheduleNo] = '" + distribScheduleNo + "' "  +
                            " AND pwi.[ItemQuantity] <> 0  UNION ALL  " +
                            "  SELECT dpm.[RefScheduleNo],dpm.ScheduleDate,dpm.ScheduleNotes,ii.RouteNo,rm.RouteName,ii.LocationCode,li.LocationName,'RRE-'+rwl.[VendorChallanNoForSpareParts] [ChallanNumber],ii.ItemCode,im.ItemName,ii.ItemQuantity " +
                            "  FROM SHSDistributionPlan_IndividualItem ii" +
                            "  INNER JOIN [Common_LocationInfo] li ON ii.LocationCode = li.LocationCode " +
                            "  INNER JOIN SHSDistributionPlan_Master dpm ON dpm.DistribScheduleNo = ii.DistribScheduleNo " +
                            "  INNER JOIN SHSDistributionPlan_RootWiseLocation rwl ON rwl.DistribScheduleNo = ii.DistribScheduleNo AND rwl.RouteNo = ii.RouteNo AND rwl.LocationCode = ii.LocationCode         " +
                            "  INNER JOIN Inv_RouteMaster rm ON rm.RouteNo = ii.RouteNo " +
                            "  INNER JOIN Inv_ItemMaster im ON im.ItemCode = ii.ItemCode  " +

                            "  WHERE ii.DistribScheduleNo = '" + distribScheduleNo + "' AND ii.[ItemQuantity] <> 0 ) a " +
                            "  GROUP BY [RefScheduleNo],[ScheduleDate],[ScheduleNotes],[RouteNo],[RouteName],[LocationCode],[LocationName],[ChallanNumber],[ItemCode],[ItemName] " +
                            "  ORDER BY [RefScheduleNo],ScheduleDate,[RouteNo],[LocationCode],[ItemCode] ";


            var results = context.Database.SqlQuery<SHSDelivaryNoteChallan>(sqlQuery);

            return results.ToList();

        }

        public List<DailyZonalPerformanceMonitoring> DailyPerformanceMonitoringZonalReport()
        {

            return context.DailyZonalPerformanceMonitoring("RSFSUMMARY").ToList();
        }

        public List<InventoryAtVendorValuationByStockLocation> InventoryAtVendorValuationByStockLocation(string yearMonth)
        {
            return context.InventoryAtVendorValuationByStockLocation(yearMonth).ToList();
        }

        public List<SalesDataEntryStatus> SalesEntryStatus(string reportType, string locationType, string yearMonth)
        {
            return context.SalesDataEntryStatus(reportType, locationType).ToList();
        }

        //public Common_UnitWiseEntryStatus UnitWiseEntryStatus(string unitCode, string YearMonth)
        //{
        //    return context.Common_UnitWiseEntryStatus.Where(i => i.YearMonth == YearMonth && i.Unit_Code == unitCode).OrderByDescending(i => i.YearMonth).FirstOrDefault();
        //}

        //public IQueryable<Common_UnitWiseCustomerStatus> UnitWiseCustomerStatus(string unitCode)
        //{
        //    return context.Common_UnitWiseCustomerStatus.Where(cs => cs.Unit_Code == unitCode);
        //}
        //public Common_UnitWiseCustomerStatus ReadUnitWiseCustomerStatus(string unitCode, string customerId)
        //{
        //    Common_UnitWiseCustomerStatus q = context.Common_UnitWiseCustomerStatus.Where(i => i.Unit_Code == unitCode && i.Cust_Code == customerId).FirstOrDefault();
        //    return q;
        //}

        //public Common_UnitWiseCustomerStatus ReadUnitWiseCustomerStatus(string customerId)
        //{
        //    Common_UnitWiseCustomerStatus q = context.Common_UnitWiseCustomerStatus.Where(i => i.Cust_Code == customerId).FirstOrDefault();
        //    return q;
        //}

        public List<InventoryDataEntryStatus> InventoryDataEntryState(string reportType, string locationType, string yearMonth)
        {
            return context.InventoryDataEntryStatus(reportType, locationType, yearMonth).ToList();
        }

        //public List<InventoryAuditAdjustment> InventoryAuditAdjustmentAdjust(string unitCode, string yearMonth, string yearMonthWrite, byte componentStatus, int stockLocation)
        //{
        //    return context.InventoryAuditAdjustment(unitCode, yearMonth, yearMonthWrite, componentStatus, stockLocation).ToList();
        //}

        public List<Aud_AdjustmentReasonCodes> AuditAdjustMentReasons(string reasonPurposeID, string reasonForUserOrAuditor, string reasonForModule)
        {
            return context.Aud_AdjustmentReasonCodes.Where(i => i.ReasonPurposeID == reasonPurposeID && i.ReasonForUserOrAuditor == Helper.ReasonForUserOrAuditor && i.ReasonForModule == reasonForModule && i.Status == Helper.Active).ToList();
        }

        public List<Aud_AdjustmentReasonCodes> CollectionAdjustMentReasons(string reasonPurposeID1, string reasonPurposeID2, string reasonForUserOrAuditor, string reasonForModule)
        {
            return context.Aud_AdjustmentReasonCodes.Where(i => i.ReasonPurposeID == reasonPurposeID1 || i.ReasonPurposeID == reasonPurposeID2 && i.ReasonForUserOrAuditor == Helper.ReasonForUserOrAuditor && i.ReasonForModule == reasonForModule && i.Status == Helper.Active).ToList();
        }


        //public List<Aud_AdjustmentReasonCodes> AuditAdjustMentReasonsForSales(string reasonPurposeID)
        //{
        //    return context.Aud_AdjustmentReasonCodes.Where(i => i.ReasonPurposeID == reasonPurposeID && i.ReasonForUserOrAuditor == Helper.ReasonForUserOrAuditor && i.ReasonForModule == Helper.ForSales && i.Status == Helper.Active).ToList();
        //}        

        //public IQueryable<Aud_AdjustmentReasonCodes> AuditAdjustMentReasons(string reasonForUserOrAuditor, byte reasonForInventoryStock, string reasonForModule)
        //{
        //    IQueryable<Aud_AdjustmentReasonCodes> reasonCodes = null;

        //    if (reasonForModule == Helper.ForInventory)
        //    {
        //        reasonCodes = context.Aud_AdjustmentReasonCodes.Where(i => i.ReasonPurposeID == "INVENTORYISSUE" && i.ReasonForUserOrAuditor == Helper.ReasonForUserOrAuditor && i.ReasonForModule == Helper.ForInventory && i.Status == Helper.Active);

        //        //if (reasonForInventoryStock == Helper.NewItem)
        //        //{
        //        //    reasonCodes = context.Aud_AdjustmentReasonCodes.Where(i => i.ReasonForNew == true && i.ReasonForUserOrAuditor == Helper.ReasonForUserOrAuditor && i.ReasonForModule == Helper.ForInventory && i.Status == Helper.Active);
        //        //}
        //        //else if (reasonForInventoryStock == Helper.NewItem)
        //        //{
        //        //    reasonCodes = context.Aud_AdjustmentReasonCodes.Where(i => i.ReasonForNew == true && i.ReasonForUserOrAuditor == Helper.ReasonForUserOrAuditor && i.ReasonForModule == Helper.ForInventory && i.Status == Helper.Active);
        //        //}
        //        //else if (reasonForInventoryStock == Helper.SystemReturn)
        //        //{
        //        //    reasonCodes = context.Aud_AdjustmentReasonCodes.Where(i => i.ReasonForSR == true && i.ReasonForUserOrAuditor == Helper.ReasonForUserOrAuditor && i.ReasonForModule == Helper.ForInventory && i.Status == Helper.Active);
        //        //}
        //        //else if (reasonForInventoryStock == Helper.CustomerSupport)
        //        //{
        //        //    reasonCodes = context.Aud_AdjustmentReasonCodes.Where(i => i.ReasonForCSI == true && i.ReasonForUserOrAuditor == Helper.ReasonForUserOrAuditor && i.ReasonForModule == Helper.ForInventory && i.Status == Helper.Active);
        //        //}
        //    }
        //    else if (reasonForModule == Helper.ForSales)
        //    {
        //        reasonCodes = context.Aud_AdjustmentReasonCodes.Where(i => i.ReasonPurposeID == "OTHERS" && i.ReasonForUserOrAuditor == Helper.ReasonForUserOrAuditor && i.ReasonForModule == Helper.ForSales && i.Status == Helper.Active);
        //    }

        //    return reasonCodes;
        //}

        //public string AuditAdjustmentOpenMonth(string unitCode)
        //{
        //    return context.Common_UnitWiseEntryStatus.Where(u => u.Unit_Code == unitCode && u.AuditEntryStatus == Helper.Open).FirstOrDefault().YearMonth;
        //}

        //public Aud_AuditingMaster AuditingMasterByLocationCode(string locationCode)
        //{
        //    return context.Aud_AuditingMaster.Where(a => a.LocationCode == locationCode).OrderByDescending(o => o.AuditSeqNo).FirstOrDefault();
        //}

        //public Common_InventoryTransaction ReadInventoryTransaction(string unitCode, string itemCode, string yearMonth, int stockLocation)
        //{
        //    return context.Common_InventoryTransaction.Where(i => i.YearMonth == yearMonth && i.UNIT_CODE == unitCode && i.COMP_CODE == itemCode && i.StockLocation == stockLocation).FirstOrDefault();
        //}

        public IQueryable<Common_Sys_StatusInfo> CustomerStatus()
        {
            return context.Common_Sys_StatusInfo.Where(sys => sys.StatusID == 0 || sys.StatusID == 5 || sys.StatusID == 6 || sys.StatusID == 7 || sys.StatusID == 8);
        }

        //public IQueryable<Common_DailyProgressReport> ReadDailyProgressReport()
        //{
        //    return context.Common_DailyProgressReport;
        //}

        //public List<CustomerStatusListV2> ReadCustomerStatusListV2(string unitCode, byte customerStatus, byte paymentStatus, DateTime currentCollectionDate)
        //{
        //    return context.CustomerStatusListV2(unitCode, customerStatus, paymentStatus, currentCollectionDate).ToList();
        //}

        //public List<GetCustomerListWithRecoveryStatus> ReadCustomerListWithRecoveryStatus(string unitCode, byte customerStatus, DateTime currentCollectionDate)
        //{
        //    return context.GetCustomerListWithRecoveryStatus(unitCode, customerStatus, currentCollectionDate).ToList();
        //}

        public List<OverdueCollectionTargetVsAchievementByUnitOffice> ReadOverdueCollectionTargetVsAchievementByUnitOffice(string spYearMonth, string spLocationCode, string reportType)
        {

            return new List<OverdueCollectionTargetVsAchievementByUnitOffice>();

            //((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext.CommandTimeout = 300;
            //return context.OverdueCollectionTargetVsAchievementByUnitOffice(spYearMonth, "").ToList();
        }

        public List<CollectionEfficiencyByUnitOfficeSummary> ReadCollectionEfficiencyByUnitOfficeSummary(string yearMonth, string locationCode, string reportType)
        {
            ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext.CommandTimeout = 300;
            return context.CollectionEfficiencyByUnitOfficeSummary(yearMonth, locationCode, reportType).ToList();
        }

        //public List<Common_InvStockInTransitByMonth> ReadStockInTransitValue(string YearMonth)
        //{
        //    return context.Common_InvStockInTransitByMonth.Where(a => a.YearMonth == YearMonth).ToList();
        //}

        public List<InventorySummaryToDetailViewReport> ReadInventorySummaryToDetailViewReport(string yearMonth, string itemCode)
        {
            return context.InventorySummaryToDetailViewReport(yearMonth, itemCode).ToList();
        }

        public List<SalesSummaryToDetailView> ReadSalesSummaryToDetailView(string yearMonth)
        {
            return new List<SalesSummaryToDetailView>(); //context.SalesSummaryToDetailView(yearMonth).ToList();
        }

        public List<CustomerTrainingSummary> ReadCustomerTrainingSummary(DateTime dtFromDate, DateTime dtToDate)
        {
            return context.CustomerTrainingSummary(dtFromDate, dtToDate).ToList();
        }

        public List<CustomerTrainingDetails> ReadCustomerTrainingDetails(string dtFromDate, string dtToDate)
        {
            return context.CustomerTrainingDetails(dtFromDate, dtToDate).ToList();
        }

        public Sal_LocationNEmployeeWiseActivityMonthly ReadLocationNEmployeeWiseActivityMonthly(string locationCode, string employeeID, string yearMonth)
        {
            return context.Sal_LocationNEmployeeWiseActivityMonthly.Where(i => i.LocationCode == locationCode && i.YearMonth == yearMonth && i.EmployeeID == employeeID).FirstOrDefault();
        }

        public List<Sal_LocationNEmployeeWiseActivityMonthly> ReadLocationNEmployeeWiseActivityMonthly(string locationCode, string yearMonth)
        {
            return context.Sal_LocationNEmployeeWiseActivityMonthly.Where(i => i.LocationCode == locationCode && i.YearMonth == yearMonth).ToList();
        }

        public GetLocationNEmployeeWiseDailyEntry ReadLocationNEmployeeWiseActivity(string locationCode, string employeeID, string yearMonth, DateTime transDate)
        {
            try
            {
                return context.GetLocationNEmployeeWiseDailyEntry(locationCode, employeeID, yearMonth, transDate).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Sal_LocationNEmployeeWiseActivityDaily ReadLocationNEmployeeWiseActivityDaily(string locationCode, string employeeID, string yearMonth, DateTime transDate)
        {
            return context.Sal_LocationNEmployeeWiseActivityDaily.Where(i => i.LocationCode == locationCode && i.EmployeeID == employeeID && i.YearMonth == yearMonth && i.TransDate == transDate.Date).FirstOrDefault();
        }

        public List<Sal_LocationNEmployeeWiseActivityDaily> ReadLocationNEmployeeWiseActivityDaily(string locationCode, string yearMonth, DateTime transDate)
        {
            return context.Sal_LocationNEmployeeWiseActivityDaily.Where(i => i.LocationCode == locationCode && i.YearMonth == yearMonth && i.TransDate == transDate.Date).ToList();
        }

        public List<Common_ProjectInfo> ReadProject(string programCode)
        {
            return context.Common_ProjectInfo.Where(p => p.Status == Helper.Active && p.Prog_Code == programCode).ToList();
        }

        public List<Common_ProgramInfo> ReadProgram()
        {
            return context.Common_ProgramInfo.Where(p => p.Status == Helper.Active).ToList();
        }

        //public List<Common_Package> ReadPackage(string programCode, string projectCode)
        //{
        //    return context.Common_Package.Where(p => p.PROG_CODE == programCode && p.Proj_Code == projectCode && p.Status == Helper.Active).ToList();
        //}

        //public Common_Package ReadPackage(string packageCode, string programCode, string projectCode)
        //{
        //    return context.Common_Package.Where(p => p.PKG_CODE == packageCode && p.PROG_CODE == programCode && p.Proj_Code == projectCode).FirstOrDefault();
        //}

        public List<PackageDetails> ReadPackageDetails(string packageCode, string modeOfPaymentID, string customerType)
        {
            return context.PackageDetails(packageCode, modeOfPaymentID, customerType).OrderByDescending(d => d.IsShowInSalesAgreementPage).ThenBy(o => o.PackageItemRank).ToList();
        }

        public Common_PeriodOpenClose ReadPeriodOpenClose(string locationCode)
        {
            return context.Common_PeriodOpenClose.Where(i => i.LocationCode == locationCode && i.IsDayOpenForDailyEntry == true).FirstOrDefault();
        }

        //public List<Hrm_LocationWiseEmployee> ReadLocationWiseEmployee(string locationCode)
        //{
        //    return context.Hrm_LocationWiseEmployee.Where(e => e.LocationCode == locationCode).ToList();
        //}
        public List<PackageDetails> ReadPackageDetailsExtra()
        {
            string sqlQuery = "SELECT  * from PackageDetails "; 
            var results = context.Database.SqlQuery<PackageDetails>(sqlQuery); 
            return results.ToList();
        }
       // List<EmployeeVisit> ReadEmployeeDetailsVisit(string option, string empID, string locationCode, string ddlLocationPart1, string ddlLocationPart4)
        public List<EmployeeVisit> ReadEmployeeDetailsVisit(string option, string empID, string locationCode, string ddlLocationPart1, string ddlLocationPart4)
        {
            //string sqlQuery = "SELECT lm.TTLocationAddress, lm.TTLocationCode, cvsm.CorporatePhoneNo FROM TeamTracking_LocationNVillageAreaSIMWiseTTLocation sm "
            //                  + "  INNER JOIN TeamTracking_TTLocationMaster lm ON lm.TTLocationCode = sm.TTLocationCode "
            //                  + " INNER JOIN TeamTracking_CorporateVoiceSIMMaster cvsm ON cvsm.CorporatePhoneNo = sm.VillageAreaSIMNumber "
            //                  + " WHERE LocationCode = '" + locationCode + "' AND cvsm.AssignedToEmployeeID = '" + empID + "'";




            var results = context.Database.SqlQuery<EmployeeVisit>("EXEC [SP_TeamTracking_GetLocationNEmployeeWiseTTLocation] '" + option + "', '" + locationCode + "', '" + empID + "', '" + ddlLocationPart1 + "', '" + ddlLocationPart4 + "'");

            return results.ToList();
        }

        public List<GetLocationWiseEmployee> ReadGetLocationWiseEmployee(string locationCode)
        {
            return context.GetLocationWiseEmployee(locationCode).ToList();
        }
        public List<GetLocationWiseEmployeeTarget> ReadGetLocationWiseEmployeeTarget(string locationCode, string yearMonth)
        {
            return context.GetLocationWiseEmployeeTarget(locationCode).ToList();
        }

        public Sal_LocationWiseSalesNCollectionTarget ReadLocationWiseSalesNCollectionTarget(string locationCode, string yearMonth)
        {
            return context.Sal_LocationWiseSalesNCollectionTarget.Where(i => i.LocationCode == locationCode && i.YearMonth == yearMonth).FirstOrDefault();
        }

        public List<ProgressReviewDataEntryStatusDaily> ReadProgressReviewDataEntryStatusDaily(string reportType, string locationCode, string yearMonth, string respectiveAreaUserID)
        {
            return context.ProgressReviewDataEntryStatusDaily(reportType, locationCode, yearMonth, respectiveAreaUserID).ToList();
        }

        //public List<LocationAndEmployeeWiseWeeklySalesAndCollectionEntry> ReadLocationAndEmployeeWiseWeeklySalesAndCollectionEntry(string locationCode, string yearWeek)
        //{
        //    return context.LocationAndEmployeeWiseWeeklySalesAndCollectionEntry(locationCode, yearWeek).ToList();
        //}

        //public Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement ReadWeeklySalesNCollectionTargetNAchievement(string locationCode, string yearWeek, string EmployeeID, DateTime transDate)
        //{
        //    return context.Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement.Where(p => p.LocationCode == locationCode && p.YearWeek == yearWeek && p.EmployeeID == EmployeeID && p.TransDate == transDate).FirstOrDefault();
        //}

        //public List<Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement> ReadWeeklySalesNCollectionTargetNAchievement(string locationCode, string yearWeek, DateTime transDate)
        //{
        //    return context.Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement.Where(p => p.LocationCode == locationCode && p.YearWeek == yearWeek).ToList();
        //}

        //public List<LocationAndEmployeeWiseWeeklySalesAndCollectionReport> ReadLocationAndEmployeeWiseWeeklySalesAndCollectionReport(string reportType, string locationCode, string yearWeek)
        //{

        //    SqlParameter[] storeParam = new SqlParameter[3];
        //    storeParam[0] = new SqlParameter("prmReportType", reportType);
        //    storeParam[1] = new SqlParameter("prmLocationCode", locationCode);
        //    storeParam[2] = new SqlParameter("prmYearWeek", yearWeek);

        //    ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext.CommandTimeout = 300;
        //    var resultList = context.Database.SqlQuery<LocationAndEmployeeWiseWeeklySalesAndCollectionReport>("Exec REP_TrackerZoneWiseWeeklyODCollectionTargetVsAchievement @prmReportType, @prmLocationCode, @prmYearWeek", storeParam);

        //    return resultList.ToList();


        //    //return context.LocationAndEmployeeWiseWeeklySalesAndCollectionReport(reportType, locationCode, yearWeek).ToList();
        //}

        //public Sal_LocationWiseWeeklySalesNCollectionSummary LocationWiseWeeklySalesNCollectionSummary(string locationCode, string yearWeek)
        //{
        //    return context.Sal_LocationWiseWeeklySalesNCollectionSummary.Where(s => s.LocationCode == locationCode && s.YearWeek == yearWeek).FirstOrDefault();
        //}

        public List<Sal_CustomerWiseOverdueBalanceForTracker> ReadCustomerWiseOverdueBalanceForTracker(String unitCode)
        {
            return context.Sal_CustomerWiseOverdueBalanceForTracker.Where(c => c.UnitCode == unitCode).ToList();
        }

        public List<CustomerWiseOverdueBalanceTracker> ReadCustomerWiseOverdueBalanceForTracker(String unitCode, string weekNumber)
        {
            string sqlQuery =
                "Select CWOB.CustomerCode CustomerCode, CWOB.CustomerName, CWOB.OverdueBalance,	" +
                "( " +
                      "( " +
                          "ISNULL(CWOB.RecoWeek01,0) + ISNULL(CWOB.RecoWeek02,0) + ISNULL(CWOB.RecoWeek03,0) + ISNULL(CWOB.RecoWeek04,0) + " +
                          "ISNULL(CWOB.RecoWeek05,0) + ISNULL(CWOB.RecoWeek06,0) + ISNULL(CWOB.RecoWeek07,0) + ISNULL(CWOB.RecoWeek08,0) + " +
                          "ISNULL(CWOB.RecoWeek09,0) + ISNULL(CWOB.RecoWeek10,0) + ISNULL(CWOB.RecoWeek11,0) + ISNULL(CWOB.RecoWeek12,0) + " +
                          "ISNULL(CWOB.RecoWeek13,0) + ISNULL(CWOB.RecoWeek14,0) + ISNULL(CWOB.RecoWeek15,0) + ISNULL(CWOB.RecoWeek16,0) + " +
                          "ISNULL(CWOB.RecoWeek17,0) + ISNULL(CWOB.RecoWeek18,0) + ISNULL(CWOB.RecoWeek19,0) + ISNULL(CWOB.RecoWeek20,0) + " +
                          "ISNULL(CWOB.RecoWeek21,0) + ISNULL(CWOB.RecoWeek22,0) + ISNULL(CWOB.RecoWeek23,0) + ISNULL(CWOB.RecoWeek24,0) + " +
                          "ISNULL(CWOB.RecoWeek25,0) + ISNULL(CWOB.RecoWeek26,0) + ISNULL(CWOB.RecoWeek27,0) + ISNULL(CWOB.RecoWeek28,0) + " +
                          "ISNULL(CWOB.RecoWeek29,0) + ISNULL(CWOB.RecoWeek30,0) + ISNULL(CWOB.RecoWeek31,0) + ISNULL(CWOB.RecoWeek32,0) + " +
                          "ISNULL(CWOB.RecoWeek33,0) + ISNULL(CWOB.RecoWeek34,0) + ISNULL(CWOB.RecoWeek35,0) + ISNULL(CWOB.RecoWeek36,0) + " +
                          "ISNULL(CWOB.RecoWeek37,0) + ISNULL(CWOB.RecoWeek38,0) + ISNULL(CWOB.RecoWeek39,0) + ISNULL(CWOB.RecoWeek40,0) + " +
                          "ISNULL(CWOB.RecoWeek41,0) + ISNULL(CWOB.RecoWeek42,0) + ISNULL(CWOB.RecoWeek43,0) + ISNULL(CWOB.RecoWeek44,0) + " +
                          "ISNULL(CWOB.RecoWeek45,0) + ISNULL(CWOB.RecoWeek46,0) + ISNULL(CWOB.RecoWeek47,0) + ISNULL(CWOB.RecoWeek48,0) + " +
                          "ISNULL(CWOB.RecoWeek49,0) + ISNULL(CWOB.RecoWeek50,0) + ISNULL(CWOB.RecoWeek51,0) + ISNULL(CWOB.RecoWeek52,0) " +

                       ") - ISNULL(CWOB.RecoWeek" + weekNumber + ",0) " +
                 ") " +
                 "ODRecoveredUpToLastWeek, (ISNULL(CWOB.OverdueBalance,0) - " +
                 "( " +
                      "( " +
                          "ISNULL(CWOB.RecoWeek01,0) + ISNULL(CWOB.RecoWeek02,0) + ISNULL(CWOB.RecoWeek03,0) + ISNULL(CWOB.RecoWeek04,0) + " +
                          "ISNULL(CWOB.RecoWeek05,0) + ISNULL(CWOB.RecoWeek06,0) + ISNULL(CWOB.RecoWeek07,0) + ISNULL(CWOB.RecoWeek08,0) + " +
                          "ISNULL(CWOB.RecoWeek09,0) + ISNULL(CWOB.RecoWeek10,0) + ISNULL(CWOB.RecoWeek11,0) + ISNULL(CWOB.RecoWeek12,0) + " +
                          "ISNULL(CWOB.RecoWeek13,0) + ISNULL(CWOB.RecoWeek14,0) + ISNULL(CWOB.RecoWeek15,0) + ISNULL(CWOB.RecoWeek16,0) + " +
                          "ISNULL(CWOB.RecoWeek17,0) + ISNULL(CWOB.RecoWeek18,0) + ISNULL(CWOB.RecoWeek19,0) + ISNULL(CWOB.RecoWeek20,0) + " +
                          "ISNULL(CWOB.RecoWeek21,0) + ISNULL(CWOB.RecoWeek22,0) + ISNULL(CWOB.RecoWeek23,0) + ISNULL(CWOB.RecoWeek24,0) + " +
                          "ISNULL(CWOB.RecoWeek25,0) + ISNULL(CWOB.RecoWeek26,0) + ISNULL(CWOB.RecoWeek27,0) + ISNULL(CWOB.RecoWeek28,0) + " +
                          "ISNULL(CWOB.RecoWeek29,0) + ISNULL(CWOB.RecoWeek30,0) + ISNULL(CWOB.RecoWeek31,0) + ISNULL(CWOB.RecoWeek32,0) + " +
                          "ISNULL(CWOB.RecoWeek33,0) + ISNULL(CWOB.RecoWeek34,0) + ISNULL(CWOB.RecoWeek35,0) + ISNULL(CWOB.RecoWeek36,0) + " +
                          "ISNULL(CWOB.RecoWeek37,0) + ISNULL(CWOB.RecoWeek38,0) + ISNULL(CWOB.RecoWeek39,0) + ISNULL(CWOB.RecoWeek40,0) + " +
                          "ISNULL(CWOB.RecoWeek41,0) + ISNULL(CWOB.RecoWeek42,0) + ISNULL(CWOB.RecoWeek43,0) + ISNULL(CWOB.RecoWeek44,0) + " +
                          "ISNULL(CWOB.RecoWeek45,0) + ISNULL(CWOB.RecoWeek46,0) + ISNULL(CWOB.RecoWeek47,0) + ISNULL(CWOB.RecoWeek48,0) + " +
                          "ISNULL(CWOB.RecoWeek49,0) + ISNULL(CWOB.RecoWeek50,0) + ISNULL(CWOB.RecoWeek51,0) + ISNULL(CWOB.RecoWeek52,0) " +

                        ") - ISNULL(CWOB.RecoWeek" + weekNumber + ",0) " +
                  ") " +

                 ")RemainingODToBeRecovered,	" +
                 " ISNULL(CWOB.RecoWeek" + weekNumber + ",0) ODRecoveredThisWeek " +
                 " From Sal_CustomerWiseOverdueBalanceForTracker CWOB " +
                 " Where CWOB.UnitCode = '" + unitCode + "' ";

            var queryResult = context.Database.SqlQuery<CustomerWiseOverdueBalanceTracker>(sqlQuery);

            return queryResult.ToList();
        }

        public List<WeeklySalesNCollectionStatus> ReadWeeklySalesNCollectionStatus(string reportType, string locationCode, string yearWeek)
        {
            return context.WeeklySalesNCollectionStatus(reportType, locationCode, yearWeek).ToList();
        }

        public Common_CurrentYearMonthNWeek ReadCurrentYearMonthNWeek()
        {
            return context.Common_CurrentYearMonthNWeek.FirstOrDefault();
        }

        public List<Sal_CustomerType> ReadCustomerTypes()
        {
            return context.Sal_CustomerType.Where(t => t.Status == Helper.Active).ToList();
        }

        public List<Inv_Sys_ItemType> ReadItemType()
        {
            return context.Inv_Sys_ItemType.OrderBy(i => i.SortingOrder).ToList();
        }

        public Inv_ItemCategorySubCategory ReadItemCategorySubCategoryByCategoryID(string ItemCategoryID)
        {
            return context.Inv_ItemCategorySubCategory.Where(i => i.ItemCategoryID == ItemCategoryID).FirstOrDefault();
        }

        public List<Inv_ItemCategorySubCategory> ReadItemCategorySubCatagory(string itemTypeID)
        {
            return context.Inv_ItemCategorySubCategory.Where(i => i.ItemType == itemTypeID).OrderBy(o => o.SortingOrder).ToList();
        }

        public List<Inv_ItemCategorySubCategory> ReadItemCategorySubCatagoryByTransactionType(string itemType, string transactionType, byte locationType)
        {
            string itemCategorySubCategoryQuery = "SELECT icsc.* FROM Inv_ItemCategorySubCategory icsc  " +
                                                  "INNER JOIN Inv_Validation_ItemTransactionTypeVsItemCategory itvic " +
                                                  "ON icsc.ItemCategoryID = itvic.ItemCategory  " +
                                                  "INNER JOIN Inv_Validation_LocationTypeVsItemTransactionType loctypvtrtyp " +
                                                  "ON itvic.ItemTransType = loctypvtrtyp.ItemTransType " +
                                                  "Where icsc.ItemType = '" + itemType + "' AND " +
                                                  "itvic.ItemTransType = '" + transactionType + "' AND " +
                                                  "loctypvtrtyp.LocationType = " + locationType + " AND " +
                                                  "icsc.[Status] = 0 AND " +
                                                  "itvic.[Status] = 0 AND " +
                                                  "loctypvtrtyp.[Status] = 0";

            var result = context.Database.SqlQuery<Inv_ItemCategorySubCategory>(itemCategorySubCategoryQuery).OrderBy(o => o.SortingOrder);

            return result.ToList();
        }

        public bool ReadIsItemCodeWiseValidationExist(string itemCategory, string transactionType)
        {
            string query = "SELECT IsItemCodeWiseValidationExist FROM Inv_Validation_ItemTransactionTypeVsItemCategory " +
                           "WHERE ItemTransType = '" + transactionType + "' AND ItemCategory = '" + itemCategory + "'";


            bool isItemCodeWiseValidationExist = false;
            var result = context.Database.SqlQuery<bool>(query);

            if (result != null)
                isItemCodeWiseValidationExist = Convert.ToBoolean(result.FirstOrDefault());

            return isItemCodeWiseValidationExist;
        }



        public List<Inv_RouteMaster> ReadRootCategory()
        {

            return context.Inv_RouteMaster.OrderBy(i => i.RouteNo).ToList();

            //var distinctRootCategories=context.Inv_RootMaster.Select(m => new { m.RootCategory }).Distinct().ToList();

            //return distinctRootCategories.ToList();      
        }

        public List<Inv_RouteMaster> ReadRouteMaster(string routeCategory)
        {
            return context.Inv_RouteMaster.Where(c => c.RouteCategory == routeCategory.Trim() && c.Status==0).ToList();
        }

        public List<LocationInfo> ReadUnitList(string routeId)
        {
            var v = from rd in context.Inv_RouteDetail
                    join li in context.Common_LocationInfo
                    on rd.LocationCode equals li.LocationCode
                    where rd.RouteNo == routeId.Trim()
                    select new LocationInfo
                    {
                        LocationCode = li.LocationCode,
                        LocationName = li.LocationName
                    };

            return v.ToList();

        }

        //public List<EmployeeVisit> ReadEmployeeDetailsVisit(string empId, string locationCode)
        //{


        //    string sqlQuery = "SELECT lm.TTLocationAddress,cvsm.AssignedToEmployeeID FROM FFT_UnitNVillageAreaSIMWiseTTLocation sm "
        //                       +" INNER JOIN FFT_TTLocationMaster lm ON lm.TTLocationCode = sm.TTLocationCode "
        //                       +" INNER JOIN TeamTracking_CorporateVoiceSIMMaster cvsm ON SUBSTRING(cvsm.CorporatePhoneNo,2,20) = sm.VillageAreaSIMNumber "
        //                       + " WHERE UnitCode = '" + locationCode + "' AND cvsm.AssignedToEmployeeID = '" + empId + "'";



        //    var results = context.Database.SqlQuery<EmployeeVisit>(sqlQuery);

        //    return results.ToList();

        //}


        public List<Sal_PackageMaster> ReadPackagesForSHSDP(string capacityId, string lightId)
        {
            string sqlQuery = "SELECT [Description],[PackageCode],[Capacity],[Light],[ProjectCode],[PerUnitSalesPrice],[IsPriceChangeable],[MinSalesPrice],[MaxSalesPrice],[FixedModeOfPayment],[IsValidFor_SalesWithNewPackagePrice],[IsValidFor_ReSalesWithNewPackagePrice],[IsValidFor_ReSalesWithDepretiatedPackagePrice],[IsValidForDistribution],[PerUnitSalesPriceForCashSales],[Status]FROM Sal_PackageMaster " +
                               " WHERE Capacity = '" + capacityId + "' AND Light = '" + lightId + "' AND IsValidForDistribution = 1 AND [Status] = 0 ";



            var results = context.Database.SqlQuery<Sal_PackageMaster>(sqlQuery);

            return results.ToList();

        }

        public List<Inv_ItemModel> ReadPanelModelListForSHSDP(string packageCode)
        {
            string sqlQuery = " SELECT DISTINCT im.[Description],im.ItemModelID,im.[ItemCategory],im.[Status],im.[IsValidForDistribution] FROM Sal_PackageDetail pd " +
                              " INNER JOIN Inv_ItemModel im ON im.ItemCategory = pd.ItemCategory  " +
                              " INNER JOIN Inv_ItemMaster im1 ON im.ItemCategory = im1.ItemCategory AND im.ItemModelID = im1.ItemModel AND im1.ItemCapacity = pd.ItemCapacity AND im1.ItemCategory = pd.ItemCategory "+
                               " WHERE pd.PackageCode = '"+packageCode+"' AND pd.ItemCategory = 'PANEL1' AND im.IsValidForDistribution = 1 AND im.[Status] = 0  ";

            var results = context.Database.SqlQuery<Inv_ItemModel>(sqlQuery);
            return results.ToList();
        }

        public List<Inv_ItemModel> ReadBatteryModelListForSHSDP(string packageCode)
        {
            string sqlQuery = " SELECT im.[Description],im.[ItemModelID],im.[ItemCategory],im.[Status],im.[IsValidForDistribution] FROM Sal_PackageDetail pd" +
                              " INNER JOIN Inv_ItemModel im ON im.ItemCategory = pd.ItemCategory  " +
                              " INNER JOIN Inv_ItemMaster im1 ON im1.ItemCategory = im.ItemCategory AND im.ItemModelID = im1.ItemModel AND im1.ItemCapacity = pd.ItemCapacity AND im1.ItemCategory = pd.ItemCategory " +
                              " WHERE pd.ItemCategory = 'BAT001' AND pd.PackageCode = '"+packageCode+"' AND im.IsValidForDistribution = 1 AND im.[Status] = 0 ";

            var results = context.Database.SqlQuery<Inv_ItemModel>(sqlQuery);
            return results.ToList();
        }



        public List<Sal_Validation_CapacityVsLight> ReadPackageCapacity()
        {
            //string sqlQuery = "SELECT RIGHT([CapacityID],5) [Package Capacity],[CapacityID],[LightID],[Status],[IsValidForDistribution]" +
            //                 " FROM [RASolarERP].[dbo].[Sal_Validation_CapacityVsLight] " +
            //                  " WHERE IsValidForDistribution = 1 AND [Status] = 0 ";

            string sqlQuery = "SELECT distinct [CapacityID],(CASE WHEN LEFT(SUBSTRING([CapacityID],2,3),1) = 0 THEN RIGHT(SUBSTRING([CapacityID],2,3),2)+' WP' " +
                             "  ELSE SUBSTRING([CapacityID],2,3)+' WP' END) [Package Capacity],NULL [LightID],CAST (0  AS tinyint) [Status],CAST(1 as bit) [IsValidForDistribution] " +
                              "  FROM [RASolarERP].[dbo].[Sal_Validation_CapacityVsLight]  WHERE IsValidForDistribution = 1 AND [Status] = 0 ";


            var results = context.Database.SqlQuery<Sal_Validation_CapacityVsLight>(sqlQuery);

            return results.ToList();
        }

        public List<PersonEmployeeDetailsInfo> ReadPersonLocationWiseEmployee(string locationCode)
        {
            //SqlParameter[] storeParam = new SqlParameter[3];
            //storeParam[0] = new SqlParameter("Option", "LOCATION_WISE_EMPLOYEES");
            //storeParam[1] = new SqlParameter("prmLocationCode", locationCode);
            //storeParam[2] = new SqlParameter("prmEmployeeID", "");


            var resultList = context.Database.SqlQuery<PersonEmployeeDetailsInfo>("EXEC [SP_TeamTracking_GetLocationNEmployeeWiseTTLocation] 'LOCATION_WISE_EMPLOYEES', '"+locationCode+"', '','','' ");

            return resultList.ToList(); 
        }

       

        public List<LocationPart1District> ReadLocationPart1District(string empWisePart1, string empID, string locationCode)
        {
            var resultList = context.Database.SqlQuery<LocationPart1District>("EXEC [SP_TeamTracking_GetLocationNEmployeeWiseTTLocation] '" + empWisePart1 + "', '" + locationCode + "', '" + empID + "', '', ''");

            return resultList.ToList(); 
        }

        public List<LocationPart2Upo> ReadLocationPart2District(string empWisePart1, string empID, string locationCode, string locationPart2) 
        {
            var resultList = context.Database.SqlQuery<LocationPart2Upo>("EXEC [SP_TeamTracking_GetLocationNEmployeeWiseTTLocation] '" + empWisePart1 + "', '" + locationCode + "', '" + empID + "', '" + locationPart2 + "', ''");

            return resultList.ToList();
        }


        public List<Sal_Validation_CapacityVsLight> ReadLightByPCapacityID(string capacityID)
        {
            string sqlQuery = "SELECT [LightID],[CapacityID],[Status],[IsValidForDistribution]" +
                                " FROM [RASolarERP].[dbo].[Sal_Validation_CapacityVsLight] " +
                                " WHERE CapacityID = '" + capacityID + "' AND IsValidForDistribution = 1 AND [Status] = 0";


            var results = context.Database.SqlQuery<Sal_Validation_CapacityVsLight>(sqlQuery);

            return results.ToList();
        }



       
        public List<Inv_StoreLocation> ReadStoreLocation()
        {
            return context.Inv_StoreLocation.OrderBy(o => o.SortingOrder).ToList();
        }

        public List<Inv_StoreLocation> ReadStoreLocation(string itemType)
        {
            return context.Inv_StoreLocation.Where(s => s.ItemType == itemType).OrderBy(o => o.SortingOrder).ToList();
        }

        public List<Inv_StoreLocation> ReadStoreLocation(int storeLocationID, string itemTypeId)
        {
            return context.Inv_StoreLocation.Where(s => s.StoreLocationID == storeLocationID && s.ItemType == itemTypeId).ToList();
        }

        public List<Inv_StoreLocation> ReadStoreLocationByItemTypeAndTransaction(string itemType, string itemTransType, byte locationType, string location)
        {
            string whereCondition = string.Empty;
            //IsValidForUnitOffice	IsValidForZonalOffice	IsValidForHeadOffice
            if (location == Helper.HeadOffice)
            {
                whereCondition += " AND IsValidForHeadOffice = 1";
            }
            else if (location == Helper.Unit)
            {
                whereCondition += " AND IsValidForUnitOffice = 1";
            }
            else if (location == Helper.Zone)
            {
                whereCondition += " AND IsValidForZonalOffice = 1";
            }

            string storeLocationQuery = "SELECT sl.* FROM Inv_StoreLocation sl " +
                                        "INNER JOIN Inv_Validation_ItemTransactionTypeVsStoreLocation itvsl " +
                                        "ON sl.StoreLocationID = itvsl.StoreLocation " +
                                        "INNER JOIN Inv_Validation_LocationTypeVsItemTransactionType loctypvtrtyp " +
                                        "ON itvsl.ItemTransType = loctypvtrtyp.ItemTransType " +
                                        "Where sl.ItemType = '" + itemType + "' AND " +
                                        "itvsl.ItemTransType = '" + itemTransType + "' AND " +
                                        "loctypvtrtyp.LocationType = " + locationType + " AND " +
                                        "sl.[Status] = 0 AND itvsl.[Status]=0 " + whereCondition;
            //"itvsl.[Status] = 0 AND " +
            //"loctypvtrtyp.[Status] = 0" + 

            var result = context.Database.SqlQuery<Inv_StoreLocation>(storeLocationQuery).OrderBy(o => o.SortingOrder);

            return result.ToList();
        }

        public List<Inv_ItemStockWithSerialNoByLocation> ReadItemStockWithSerialNoByLocation(byte storeLocation, string locationCode, string itemCode)
        {
            return context.Inv_ItemStockWithSerialNoByLocation.Where(i => i.StoreLocation == storeLocation && i.LocationCode == locationCode && i.ItemCode == itemCode && i.IsAvailableInStore == true).ToList();
        }

        public List<Sal_Light> ReadLight(string capacityID)
        {
            string sqlQuery = "SELECT LG.LightID, LG.[Description],LG.LightQuantity,LG.[Status] FROM Sal_PackageOrItemCapacity PIC " +
                              " INNER JOIN Sal_Validation_CapacityVsLight CVL ON " +
                              " PIC.CapacityID = CVL.CapacityID " +
                              " INNER JOIN Sal_Light LG ON " +
                              " LG.LightID = CVL.LightID " +
                              " WHERE PIC.CapacityID = '" + capacityID + "' AND " +
                              " PIC.[Status] = 0 AND CVL.Status= 0 AND LG.[Status] = 0";

            var results = context.Database.SqlQuery<Sal_Light>(sqlQuery);

            return results.ToList();
        }

        public List<Sal_PackageMaster> ReadPopUpPackageList(string packageCode)
        {
            string sqlQuery = "SELECT [Description],[PackageCode],[Capacity],[Light],[ProjectCode],[PerUnitSalesPrice],[IsPriceChangeable],[MinSalesPrice],[MaxSalesPrice],[FixedModeOfPayment],[IsValidFor_SalesWithNewPackagePrice],[IsValidFor_ReSalesWithNewPackagePrice],[IsValidFor_ReSalesWithDepretiatedPackagePrice],[IsValidForDistribution],[PerUnitSalesPriceForCashSales],[Status]FROM Sal_PackageMaster  where PackageCode='" + packageCode + "'";

            var results = context.Database.SqlQuery<Sal_PackageMaster>(sqlQuery);

            return results.ToList();
        }



        public List<PackageLightCapacityPop> ReadPopUpPackageCapacity(string packageCode)
        {

            string sqlQuery = "SELECT DISTINCT pm.PackageCode,pm.[Description],Capacity,LEFT(pm.[Description],6) [PackageCapacity],Light,l.[Description] [PackageLight] "
                               +"   FROM Sal_PackageMaster pm INNER JOIN Sal_SalesAgreementPrePost_DataFromExternalSources sap ON sap.PackageCode = pm.PackageCode "
                               + "   INNER JOIN Sal_Light l ON l.LightID = pm.Light WHERE pm.PackageCode = '" + packageCode + "'";

            var result= context.Database.SqlQuery<PackageLightCapacityPop>(sqlQuery);
            
            return result.ToList();
        }

        public Inv_MRRMaster ReadMRRMasterByChallanSeqNo(string challanSeqNumber, string challanLocationCode)
        {
            return context.Inv_MRRMaster.Where(c => c.ChallanSeqNo == challanSeqNumber && c.ChallanLocationCode == challanLocationCode).FirstOrDefault();
        }

        public List<Inv_MRRMaster> ReadMRRMaster()
        {
            return context.Inv_MRRMaster.ToList();
        }

        public List<Sal_PackageOrItemCapacity> ReadPackageOrItemCapacity(string projectCode, string isForItemOrPackage)
        {
            return context.Sal_PackageOrItemCapacity.Where(c => c.ProjectCode == projectCode && c.Status == Helper.Active && c.CapacityFor.Contains(isForItemOrPackage)).ToList();
        }

        public List<Sal_PackageMaster> ReadPackages(string capacityId, string lightId, byte salesType)
        {
            List<Sal_PackageMaster> lstPackageMaster = new List<Sal_PackageMaster>();

            if (salesType == Helper.NewSalesAgreement)
            {
                lstPackageMaster = context.Sal_PackageMaster.Where(p => p.IsValidFor_SalesWithNewPackagePrice == true && p.Capacity == capacityId && p.Light == lightId && p.Status == Helper.Active).ToList();
            }
            else if (salesType == Helper.ResaleAgreementWithNewPackagePrice)
            {
                lstPackageMaster = context.Sal_PackageMaster.Where(p => p.IsValidFor_ReSalesWithNewPackagePrice == true && p.Capacity == capacityId && p.Light == lightId && p.Status == Helper.Active).ToList();
            }
            else if (salesType == Helper.ResaleAgreementWithDepreciatedPackagePrice)
            {
                lstPackageMaster = context.Sal_PackageMaster.Where(p => p.IsValidFor_ReSalesWithDepretiatedPackagePrice == true && p.Capacity == capacityId && p.Light == lightId && p.Status == Helper.Active).ToList();
            }
            else if (salesType == Helper.BondhuBati)
            {
                lstPackageMaster = context.Sal_PackageMaster.Where(p => p.IsValidFor_ReSalesWithDepretiatedPackagePrice == true && p.Capacity == capacityId && p.Light == lightId && p.Status == Helper.Active).ToList();
            }
            else
            {
                lstPackageMaster = context.Sal_PackageMaster.Where(p => p.Capacity == capacityId && p.Light == lightId && p.Status == Helper.Active).ToList();
            }

            return lstPackageMaster;
        }




        public List<Sal_PackageMaster> ReadPackagesPackgeListFrmPopUpTddlPckg(string capacityId, string lightId, byte salesType)
        {

            List<Sal_PackageMaster> lstPackageMaster = new List<Sal_PackageMaster>();

            if (salesType == Helper.NewSalesAgreement)
            {
                lstPackageMaster = context.Sal_PackageMaster.Where(p => p.IsValidFor_SalesWithNewPackagePrice == true && p.Capacity == capacityId && p.Light == lightId && p.Status == Helper.Active).ToList();
            }
            else if (salesType == Helper.ResaleAgreementWithNewPackagePrice)
            {
                lstPackageMaster = context.Sal_PackageMaster.Where(p => p.IsValidFor_ReSalesWithNewPackagePrice == true && p.Capacity == capacityId && p.Light == lightId && p.Status == Helper.Active).ToList();
            }
            else if (salesType == Helper.ResaleAgreementWithDepreciatedPackagePrice)
            {
                lstPackageMaster = context.Sal_PackageMaster.Where(p => p.IsValidFor_ReSalesWithDepretiatedPackagePrice == true && p.Capacity == capacityId && p.Light == lightId && p.Status == Helper.Active).ToList();
            }
            else if (salesType == Helper.BondhuBati)
            {
                lstPackageMaster = context.Sal_PackageMaster.Where(p => p.IsValidFor_ReSalesWithDepretiatedPackagePrice == true && p.Capacity == capacityId && p.Light == lightId && p.Status == Helper.Active).ToList();
            }
            else
            {
                lstPackageMaster = context.Sal_PackageMaster.Where(p => p.Capacity == capacityId && p.Light == lightId && p.Status == Helper.Active).ToList();
            }

            return lstPackageMaster;
        }

        //public Sal_Validation_CustomerTypeNModeOfPaymentWiseServiceChargePolicy ReadServiceChargePolicy(string modeOfPaymentId, string customerType)
        //{
        //    return context.Sal_Validation_CustomerTypeNModeOfPaymentWiseServiceChargePolicy.Where(s => s.ModeOfPaymentID == modeOfPaymentId && s.CustomerType == customerType).FirstOrDefault();
        //}

        //public Sal_Validation_ModeOfPaymentWiseDiscountPolicy ReadDiscountPolicy(string modeOfPaymentId, string discountId)
        //{
        //    return context.Sal_Validation_ModeOfPaymentWiseDiscountPolicy.Where(d => d.ModeOfPaymentID == modeOfPaymentId && d.DiscountID == discountId).FirstOrDefault();
        //}

        //public Sal_Validation_ModeOfPaymentNPackageWiseDownpaymentPolicy ReadPackageVsDownpayment(string packageCode, string modeOfPaymentId)
        //{
        //    return context.Sal_Validation_ModeOfPaymentNPackageWiseDownpaymentPolicy.Where(p => p.ModeOfPaymentID == modeOfPaymentId && p.PackageCode == packageCode).FirstOrDefault();
        //}

        public ServiceChargeInformation ReadServiceChargePolicy(string programCode, string customerType, string modeOfPayment)
        {
            var v = (from srvchrg in context.Sal_Validation_ProgramVsCustomerTypeNModeOfPaymentWiseServiceChargePolicy
                     where srvchrg.CustomerType == customerType &&
                     srvchrg.ModeOfPaymentID == modeOfPayment &&
                     srvchrg.Status == Helper.Active
                     select new ServiceChargeInformation
                     {
                         ServiceChargeID = srvchrg.ServiceChargeID,
                         ServiceChargeDescription = srvchrg.Description,
                         ServiceChargePercentage = srvchrg.ServiceChargePercentage
                     }).FirstOrDefault();

            return v;
        }

        public DownPaymentPolicy ReadDownPaymentPolicy(string modeOfPayment, string packageCode)
        {
            var v = (from srvchrg in context.Sal_Validation_ModeOfPaymentNPackageWiseDownpaymentPolicy
                     where srvchrg.ModeOfPaymentID == modeOfPayment &&
                     srvchrg.PackageCode == packageCode &&
                     srvchrg.Status == Helper.Active
                     select new DownPaymentPolicy
                     {
                         DownPaymentID = srvchrg.DownPaymentID,
                         DownPaymentDescription = srvchrg.Description,
                         DownPaymentPercentage = srvchrg.DownPaymentPercentage,
                         IsDPAFixedAmount = srvchrg.IsDPAFixedAmount,
                         DownPaymentFixedAmount = srvchrg.DownPaymentFixedAmount

                     }).FirstOrDefault();

            return v;
        }

        public DiscountPolicy ReadDiscountPolicyByModeofPaymentNDiscountId(string modeOfPayment, string discountId)
        {
            var v = (from mopwdp in context.Sal_Validation_ModeOfPaymentWiseDiscountPolicy
                     where mopwdp.ModeOfPaymentID == modeOfPayment &&
                     mopwdp.DiscountID == discountId &&
                     mopwdp.Status == Helper.Active
                     select new DiscountPolicy
                     {
                         DiscountID = mopwdp.DiscountID,
                         DiscountDescription = mopwdp.Description,
                         DiscountPercentage = mopwdp.DiscountPercentage

                     }).FirstOrDefault();

            return v;
        }

        public DiscountPolicy ReadDiscountPolicyByModeofPaymentNPackageId(string modeOfPayment, string packageCode)
        {
           /* var vvv = (from mopwdp in context.Sal_Validation_ModeOfPaymentWiseDiscountPolicy
                       where mopwdp.ModeOfPaymentID == modeOfPayment &&
                             mopwdp.DiscountID == (string)((
                              from mopnpvd in context.Sal_Validation_ModeOfPaymentNPackageVsDiscount
                              where mopnpvd.ModeOfPaymentID == modeOfPayment &&
                                    mopnpvd.PackageCode == packageCode
                              select mopnpvd.DiscountID
                             ).FirstOrDefault())
                       select new DiscountPolicy
                       {

                           DiscountID = mopwdp.DiscountID,
                           DiscountDescription = mopwdp.Description,
                           DiscountPercentage = mopwdp.DiscountPercentage,
                           IsDiscountAFixedAmount = mopwdp.IsDiscountAFixedAmount,
                           DiscountFixedAmount = mopwdp.DiscountFixedAmount
                       }).FirstOrDefault();*/

            var vvv = (from dp in context.Sal_Validation_ModeOfPaymentWiseDiscountPolicy
                       join pvd in context.Sal_Validation_ModeOfPaymentNPackageVsDiscount
                       on new { dp.ModeOfPaymentID, dp.DiscountID } equals new { pvd.ModeOfPaymentID, pvd.DiscountID }
                       where pvd.ModeOfPaymentID == modeOfPayment && pvd.PackageCode == packageCode && pvd.Status==0 && dp.Status == 0
                       select new DiscountPolicy
                       {
                           DiscountID = dp.DiscountID,
                           DiscountDescription = dp.Description,
                           DiscountPercentage = dp.DiscountPercentage,
                           IsDiscountAFixedAmount = dp.IsDiscountAFixedAmount,
                           DiscountFixedAmount = dp.DiscountFixedAmount
                       }).FirstOrDefault();

            return vvv;

        }

        public Sal_Customer ReadCustomer(string customerCode)
        {
            return context.Sal_Customer.Where(c => c.CustomerCode == customerCode).FirstOrDefault();
        }

        public Sal_Customer ReadCustomer(string unitCode, string customerCode)
        {
            return context.Sal_Customer.Where(c => c.CustomerCode == customerCode && c.UnitCode == unitCode).FirstOrDefault();
        }

        public Common_UpazillaInfo ReadUpazillaByID(string upazillaCode)
        {
            return context.Common_UpazillaInfo.Where(u => u.UPAZ_CODE == upazillaCode).FirstOrDefault();
        }

        public List<Common_UpazillaInfo> ReadUpazilla(string unitCode)
        {
            string upazillaQuery = "SELECT * FROM Common_UpazillaInfo UPZ " +
                                    "INNER JOIN Sal_Validation_UnitVsUpazilla UNTVUPZ " +
                                    "ON UPZ.UPAZ_CODE =  UNTVUPZ.UpazillaCode " +
                                    "WHERE UNTVUPZ.UnitCode = '" + unitCode + "' AND UPZ.IDCOL_ThanaID IS NOT NULL";

            var upazilla = context.Database.SqlQuery<Common_UpazillaInfo>(upazillaQuery);

            return upazilla.ToList();
        }

        public List<Inv_ItemModel> ReadItemModel(string itemCatagory, string itemCapacity, string itemCategoryIdForNull)
        {
            string itemModelQuery = "SELECT ISNULL(ITM.ItemModelID,'none')ItemModelID,ISNULL(ITM.[Description],'')[Description], " +
                                    " ISNULL(ITM.ItemCategory,'none')ItemCategory,ISNULL(ITM.IsValidForDistribution,0)IsValidForDistribution,ISNULL(ITM.[Status],0)[Status] FROM" +
                                    " (Select * from Inv_ItemMaster WHERE ItemCategory IN(" + itemCatagory + ") AND " +
                                    " ItemCapacity IN (" + itemCapacity + "))ASP " +
                                    " LEFT JOIN Inv_ItemModel ITM " +
                                    " ON ASP.ItemModel = ITM.ItemModelID ";

            string itemModelQueryForNullCapacity = " UNION ALL " +
                                                   "SELECT ISNULL(ITM.ItemModelID,'none')ItemModelID,ISNULL(ITM.[Description],'')[Description],   " +
                                                   "ISNULL(ITM.ItemCategory,'none')ItemCategory,ISNULL(ITM.IsValidForDistribution,0)IsValidForDistribution,ISNULL(ITM.[Status],0)[Status] FROM  " +
                                                   "(Select * from Inv_ItemMaster WHERE  " +
                                                   "ItemCategory IN(" + itemCategoryIdForNull + ")  " +
                                                   "AND  ItemCapacity IS NULL)ASP " +
                                                   "LEFT JOIN Inv_ItemModel ITM  ON ASP.ItemModel = ITM.ItemModelID ";

            if (!string.IsNullOrEmpty(itemCategoryIdForNull))
            {
                itemModelQuery += itemModelQueryForNullCapacity;
            }

            return context.Database.SqlQuery<Inv_ItemModel>(itemModelQuery).ToList();

        }

        public List<Inv_ItemModel> ReadItemModel(string itemCatagory)
        {
            return context.Inv_ItemModel.Where(m => m.ItemCategory == itemCatagory && m.Status == Helper.Active).ToList();
        }

        public List<ItemCapacity> ReadItemCapacity(string itemCategory)
        {
            string itemCapacityQuery = "SELECT ISNULL(asp.ItemCategory,'none') ItemCategoryID, " +
                                        "ISNULL(itc.CapacityID,'none') ItemCapacityID,ISNULL(itc.[Description],'') CapacityDescription " +
                                        "FROM " +
                                        "(SELECT * FROM Inv_ItemMaster " +
                                        "WHERE ItemCategory " +
                                        "IN(" + itemCategory + "))asp " +
                                        "LEFT JOIN Sal_PackageOrItemCapacity itc " +
                                        "ON asp.ItemCapacity = itc.CapacityID WHERE itc.CapacityID !='none  ' ";

            return context.Database.SqlQuery<ItemCapacity>(itemCapacityQuery).ToList();
        }

        public List<RASolarERP.DomainModel.SalesModel.SystemReturnOrFullPaidCustomers> ReadSystemReturnOrFullPaidCustomers(string CustomerOrViewType, string locationCode, string fromDate, string toDate)
        {
            // return context.SystemReturnOrFullPaidCustomers(yearMonth, locationCode, customerStatus).ToList();              //  comment by mahin

            SqlParameter[] storeParam = new SqlParameter[4];
            storeParam[0] = new SqlParameter("prmCustomerOrViewType", CustomerOrViewType);
            storeParam[1] = new SqlParameter("prmLocationCode", locationCode);
            storeParam[2] = new SqlParameter("prmFromDate", fromDate);
            storeParam[3] = new SqlParameter("prmToDate", toDate);

            var resultList = context.Database.SqlQuery<RASolarERP.DomainModel.SalesModel.SystemReturnOrFullPaidCustomers>
              ("Exec REP_GetCustomersOtherView @prmCustomerOrViewType, @prmLocationCode, @prmFromDate, @prmToDate", storeParam);

            return resultList.ToList();
        }
        //ReadTrackerLocationWiseWeeklyODCollectionTargetVsAchievement

        public List<TrackerLocationWiseWeeklyODCollectionTargetVsAchievement> ReadTrackerLocationWiseWeeklyODCollectionTargetVsAchievement(string prmReportType, string prmLocationCode, string prmYearWeek) 
        {
            
            SqlParameter[] storeParam = new SqlParameter[4]; 

            if(prmReportType=="RSFSUMMARY")
            {
                //SqlParameter[] storeParam = new SqlParameter[4];
                storeParam[0] = new SqlParameter("prmReportType", "ALLUNITOFRSF");
                storeParam[1] = new SqlParameter("prmYearMonth", "");
                storeParam[2] = new SqlParameter("prmYearWeek", "");
                storeParam[3] = new SqlParameter("prmLocationCode","");
           

                 //resultList = context.Database.SqlQuery<TrackerLocationWiseWeeklyODCollectionTargetVsAchievement>("EXEC REP_TrackerLocationWiseWeeklyODCollectionTargetVsAchievement @prmReportType, @prmYearMonth, @prmYearWeek, @prmLocationCode", storeParam);
            }
            else if(prmReportType=="ZONESUMMARY")
            {
                    //SqlParameter[] storeParam = new SqlParameter[4];
                storeParam[0] = new SqlParameter("prmReportType", "ALLUNITOFZONE");
                storeParam[1] = new SqlParameter("prmYearMonth", "");
                storeParam[2] = new SqlParameter("prmYearWeek", "");
                storeParam[3] = new SqlParameter("prmLocationCode", prmLocationCode);
           

                //resultList = context.Database.SqlQuery<TrackerLocationWiseWeeklyODCollectionTargetVsAchievement>("EXEC REP_TrackerLocationWiseWeeklyODCollectionTargetVsAchievement @prmReportType, @prmYearMonth, @prmYearWeek, @prmLocationCode", storeParam);
            }
            else if (prmReportType=="REGIONSUMMARY")
            {
                //EXEC [REP_TrackerLocationWiseWeeklyODCollectionTargetVsAchievement] 'ZONEWISESUMMARY','201411','201447'
                //SqlParameter[] storeParam = new SqlParameter[4];
                storeParam[0] = new SqlParameter("prmReportType", "ZONEWISESUMMARY");
                storeParam[1] = new SqlParameter("prmYearMonth", "201411");
                storeParam[2] = new SqlParameter("prmYearWeek", "201447");
                storeParam[3] = new SqlParameter("prmLocationCode", "");
           

                 //resultList = context.Database.SqlQuery<TrackerLocationWiseWeeklyODCollectionTargetVsAchievement>("EXEC REP_TrackerLocationWiseWeeklyODCollectionTargetVsAchievement @prmReportType, @prmYearMonth, @prmYearWeek, @prmLocationCode", storeParam);
            }

             var  resultList = context.Database.SqlQuery<TrackerLocationWiseWeeklyODCollectionTargetVsAchievement>("EXEC REP_TrackerLocationWiseWeeklyODCollectionTargetVsAchievement @prmReportType, @prmYearMonth, @prmYearWeek, @prmLocationCode", storeParam); 

            return resultList.ToList();
        }
        //LoadRequestEntryGet

        public List<LoadRequestEntryGet> ReadLoadRequestEntryGet(string locationCode,string getDate)
        {

            //string queryString = " SELECT ei.[EmployeeID]  "
            //                     +"  ,CASE WHEN ei.LastOperationalRole IN ('UNITMANAGR','ACTUNTMNGR') THEN ei.[EmployeeName]+' ('+ei.[EmployeeID]+') (U)' ELSE ei.[EmployeeName]+' ('+ei.[EmployeeID]+')' END [EmployeeName] "
            //                     +"  ,cvs.CorporatePhoneNo,le.LoadRequestForRSFServices,LoadRequestForPayWellServices "
            //                     +"  FROM  [Hrm_LocationWiseEmployee] lwe INNER JOIN [Hrm_EmployeeInfo] ei ON lwe.EmployeeID = ei.EmployeeID "
            //                     +"  INNER JOIN Common_LocationInfo li ON lwe.LocationCode = li.LocationCode "
            //                     +"  INNER JOIN Sal_LocationNEmployeeNDateWiseTransaction le ON le.LocationCode = lwe.LocationCode AND le.EmployeeID = lwe.EmployeeID "
            //                     +"  LEFT OUTER JOIN Hrm_OperationalRole opr ON ei.LastOperationalRole = opr.OperationalRoleID "
            //                     +"  LEFT OUTER JOIN TeamTracking_CorporateVoiceSIMMaster cvs ON cvs.DefaultSIMLocationCode = lwe.LocationCode AND cvs.AssignedToEmployeeID = lwe.EmployeeID "
            //                     +"  WHERE lwe.LocationCode = '"+locationCode +"' "
            //                     +"  AND lwe.[Status] IN(0) AND li.[Status] IN(0) AND ei.[Status] IN(0)   "
            //                     +"  AND li.LocationType = 1  "
            //                     +"  AND (ei.LastOperationalRole IS NULL OR opr.IsValidAsAFieldForce = 1) "
            //                     +"  AND ISNULL(ei.LastDesignation,'') NOT IN('MESSEN') "
            //                     +"  AND (ei.ReleaseDate IS NULL OR ISNULL(ei.ReleaseDate,'31-Dec-2100') >= GETDATE()) "
            //                     +"  AND le.YearMonth = (SELECT LEFT(CONVERT(VARCHAR,[MonthOpenForSales],112),6) FROM [Common_CurrentYearMonthNWeek])   "   
            //                     +"  AND le.Calendardate = '"+getDate+"'  ";

            string queryString = "SELECT ei.[EmployeeID]  ,CASE WHEN ei.LastOperationalRole IN ('UNITMANAGR','ACTUNTMNGR') "
                                  +"  THEN ei.[EmployeeName]+' ('+ei.[EmployeeID]+') (U)' ELSE ei.[EmployeeName]+' ('+ei.[EmployeeID]+')' END [EmployeeName] "
                                  +" ,cvs.CorporatePhoneNo,ma.LoadBalanceForRSFServices,le.LoadRequestForRSFServices,LoadRequestForPayWellServices   "
                                  +"      FROM  [Hrm_LocationWiseEmployee] lwe "
                                  +"              INNER JOIN Hrm_EmployeeInfo ei ON lwe.EmployeeID = ei.EmployeeID "
                                  +"            INNER JOIN Common_LocationInfo li ON lwe.LocationCode = li.LocationCode   "
                                  +"            INNER JOIN Sal_LocationNEmployeeNDateWiseTransaction le ON le.LocationCode = lwe.LocationCode AND le.EmployeeID = lwe.EmployeeID  " 
                                  +"             INNER JOIN (SELECT DISTINCT loe.EmployeeID,loe.LoadBalanceForRSFServices FROM MobileApp_ListOfEmployeesAsMobileAppUser loe INNER JOIN MobileApp_EmployeeWiseDeviceAllocationMaster ewd ON ewd.EmployeeID = loe.EmployeeID WHERE loe.[Status] = 0 AND ewd.[Status] = 0) ma ON ma.EmployeeID = lwe.EmployeeID  " 
                                  +"             LEFT OUTER JOIN Hrm_OperationalRole opr ON ei.LastOperationalRole = opr.OperationalRoleID   "
                                  +"             LEFT OUTER JOIN TeamTracking_CorporateVoiceSIMMaster cvs ON cvs.DefaultSIMLocationCode = lwe.LocationCode AND cvs.AssignedToEmployeeID = lwe.EmployeeID "
                                  +"       WHERE lwe.LocationCode = '"+locationCode+"'   AND lwe.[Status] IN (0) AND li.[Status] IN (0) AND ei.[Status] IN (0) "
                                  +"       AND li.LocationType = 1    AND (ei.LastOperationalRole IS NULL OR opr.IsValidAsAFieldForce = 1) "
                                  +"       AND ISNULL(ei.LastDesignation,'') NOT IN('MESSEN') "
                                  +"       AND (ei.ReleaseDate IS NULL OR ISNULL(ei.ReleaseDate,'31-Dec-2100') >= GETDATE()) "
                                  +"      AND le.YearMonth = (SELECT LEFT(CONVERT(VARCHAR,[MonthOpenForSales],112),6) FROM [Common_CurrentYearMonthNWeek])  "
                                  +"       AND le.Calendardate = '"+getDate+"'";
                                    

            var resultList = context.Database.SqlQuery<LoadRequestEntryGet>(queryString);

            return resultList.ToList();


        }

       public List<CheckLoadRequestEntry> firstCheckLoadRequestEntry(string yearMonth, string locationCode, string getDate)
        {

            string queryString = "SELECT  [YearMonth] , [LocationCode], [IsLoadRequestCompleted] FROM [Sal_LocationNDateWiseTransaction] WHERE YearMonth = '" + yearMonth + "' AND IsLoadRequestCompleted = 1 AND LocationCode = '" + locationCode + "' AND CalendarDate ='"+getDate+"'";
                        

            var resultList = context.Database.SqlQuery<CheckLoadRequestEntry>(queryString);

            return resultList.ToList();
        }

       public List<CheckLoadRequestEntryForLocationCode> LocationCheckLoadRequestEntry(string locationCode)
       {

           //string queryString = "SELECT DISTINCT lwe.LocationCode FROM Integration_PayWell_EmployeeWiseDeviceAllocationMaster eda "
           //                     +"  INNER JOIN HRM_LocationWiseEmployee lwe ON lwe.EmployeeID = eda.EmployeeID "
           //                     +"  WHERE lwe.[Status] = 0 AND lwe.LocationCode = '"+locationCode+"'";

           string queryString = " SELECT lwe.LocationCode FROM MobileApp_EmployeeWiseDeviceAllocationMaster eda "
                                + " INNER JOIN HRM_LocationWiseEmployee lwe ON lwe.EmployeeID = eda.EmployeeID "
                                + " WHERE lwe.locationCode = '" + locationCode + "' AND lwe.[Status] = 0 AND eda.[Status] = 0 ";
                                

           var resultList = context.Database.SqlQuery<CheckLoadRequestEntryForLocationCode>(queryString);

           return resultList.ToList();
       
       }

        public List<LoadRequestEntryGet> InsertLoadRequest(string locationCode, string getDate)
        {

            string strLoadRequestEntry = string.Empty;
            //strLoadRequestEntry = "EXEC [SP_SalLocationNEmployeeNDateWiseTransaction] '', '"+locationCode+"','','"+getDate+"',0,0,'','Admin','INSERT'";
            strLoadRequestEntry = "EXEC [SP_SalLocationNEmployeeNDateWiseTransaction] '', '" + locationCode + "','','" + getDate + "',0,'',0,'','Admin','INSERT'";
            try
            {
                return context.Database.SqlQuery<LoadRequestEntryGet>(strLoadRequestEntry).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }

        }




        public List<UnitWiseCustomerRegisterReport> ReadUnitWiseCustomerRegisterReport(string unitCode)
        {
            return context.UnitWiseCustomerRegisterReport(unitCode).ToList();
        }

        public List<CustomerCollectionDetails> ReadCollectionFromCustomers(string customerCode, string yearMonth)
        {
            IEnumerable<Sal_CollectionFromCustomers> q = context.Sal_CollectionFromCustomers.Where(c => c.CustomerCode == customerCode && c.YearMonth == yearMonth && c.CollectionType == "IOC");
            IEnumerable<CustomerCollectionDetails> p = q.Select(x => new CustomerCollectionDetails(x.SerialNo, x.CollectionDate, x.RefMemoNo, x.CashMemoNo, x.CollectionAmount, x.CollectedByEmployee));

            return p.ToList();
        }

        public List<CustomerCollectionAdjustmentForAudit> ReadCustomerCollectionAdjustmentForAudit(string customerCode, string yearMonth, string auditSeqNo)
        {
            var v = (from cfc in context.Sal_CollectionFromCustomers
                     join acfc in context.Aud_AuditAdjustmentRelatedCollectionFromCustomers
                     on new { cfc.CustomerCode, cfc.YearMonth } equals new { acfc.CustomerCode, acfc.YearMonth }
                     where cfc.CustomerCode == customerCode && cfc.YearMonth == yearMonth && acfc.AuditSeqNo == auditSeqNo
                     select new CustomerCollectionAdjustmentForAudit
                     {
                         SerialNo = cfc.SerialNo,
                         CollectionDate = cfc.CollectionDate,
                         RefMemoNo = cfc.CashMemoNo,
                         CollectionAmount = cfc.CollectionAmount,
                         CollectedByEmployeeID = cfc.CollectedByEmployee,
                         CollectedByEmployeeName = cfc.CollectedByEmployee,
                         AuditReason = acfc.ReasonCode,
                         ResponsibleEmployeeID = acfc.ResponsibleEmployeeID,
                         ResponsibleEmployeeName = acfc.ResponsibleEmployeeID,
                         Remarks = acfc.Remarks

                     }).ToList();

            return v;
        }

        public Sal_CollectionFromCustomers ReadCollectionFromCustomers(string customerCode, string yearMonth, byte collectionSerial)
        {
            return context.Sal_CollectionFromCustomers.Where(c => c.CustomerCode == customerCode && c.YearMonth == yearMonth && c.SerialNo == collectionSerial).FirstOrDefault();
        }

        public Sal_CollectionFromCustomersPrePost ReadCollectionFromCustomersPrepost(string customerCode, string yearMonth, byte collectionSerial)
        {
            return context.Sal_CollectionFromCustomersPrePost.Where(c => c.CustomerCode == customerCode && c.YearMonth == yearMonth && c.SerialNo == collectionSerial).FirstOrDefault();
        }

        public int CustomerCollectionSerial(string customerCode, string yearMonth)
        {
            int customerSerial = 0;

            string customerCollectionSerialQuery = "SELECT ISNULL(MAX(CFC.SerialNo),0) CustomerSerial FROM Sal_CollectionFromCustomers CFC " +
            "WHERE CFC.CustomerCode = '" + customerCode + "' AND CFC.YearMonth='" + yearMonth + "'";

            List<byte> vv = context.Database.SqlQuery<byte>(customerCollectionSerialQuery).ToList();

            if (vv != null)
            {
                customerSerial = Convert.ToInt32(vv[0]) + 1;
            }
            else
            {
                customerSerial = 1;
            }

            return customerSerial;
        }

        public int CustomerCollectionSerial(string customerCode, string yearMonth, string collectionType)
        {
            int customerSerial = 0;

            string customerCollectionSerialQuery = "SELECT ISNULL(MAX(CFC.SerialNo),0) CustomerSerial FROM Sal_CollectionFromCustomers CFC " +
            "WHERE CFC.CustomerCode = '" + customerCode + "' AND CFC.YearMonth='" + yearMonth + "' AND CollectionType = '" + collectionType + "'";

            List<byte> vv = context.Database.SqlQuery<byte>(customerCollectionSerialQuery).ToList();

            if (vv != null)
            {
                customerSerial = Convert.ToInt32(vv[0]) + 1;
            }
            else
            {
                customerSerial = 1;
            }

            return customerSerial;
        }

        public List<UnitCollectionVsHeadOfficePhysicalCashMovement> ReadUnitCollectionVsHeadOfficePhysicalCashMovement(string reportType, string locationCode, string yearMonth)
        {
            return context.UnitCollectionVsHeadOfficePhysicalCashMovement(reportType, locationCode, yearMonth).ToList();
        }
         // change for salesresalesagreement page for ddlpackage change
        //public List<Sal_ModeOfPayment> ReadModeOfPayment(string programCode, string packageCode, string customerType)
        //{

        //    //SELECT MOP.* FROM Sal_PackageMaster PKGM 
        //    //                            INNER JOIN 
        //    //                            Sal_Validation_ModeOfPaymentVsPackage PKGVMOP 
        //    //                            ON PKGM.PackageCode = PKGVMOP.PackageCode
        //    //                            INNER JOIN Sal_Validation_CustomerTypeNModeOfPaymentVsProgram ctnmopvpr
        //    //                            ON PKGVMOP.ModeOfPaymentID = ctnmopvpr.ModeOfPaymentID
        //    //                            INNER JOIN Sal_ModeOfPayment MOP
        //    //                            ON MOP.ModeOfPaymentID=PKGVMOP.ModeOfPaymentID 
        //    //                            WHERE PKGM.PackageCode = 'bon001' AND PKGVMOP.[Status] = 0 
        //    //                            AND ctnmopvpr.ProgramCode = 'bonb01' AND
        //    //                            ctnmopvpr.CustomerType = 'HOUHLD'
        //    //                            AND ctnmopvpr.[Status] = 0 


        //    var v = from pkgm in context.Sal_PackageMaster
        //            join pkgvmop in context.Sal_Validation_ModeOfPaymentVsPackage
        //                on pkgm.PackageCode equals pkgvmop.PackageCode
        //            join ctnmopvpr in context.Sal_Validation_ProgramVsCustomerTypeNModeOfPayment
        //                on pkgvmop.ModeOfPaymentID equals ctnmopvpr.ModeOfPaymentID
        //            join mop in context.Sal_ModeOfPayment
        //                on pkgvmop.ModeOfPaymentID equals mop.ModeOfPaymentID
        //            where
        //                 pkgm.PackageCode == packageCode &&
        //                 pkgvmop.Status == Helper.Active &&
        //                 ctnmopvpr.CustomerType == customerType &&
        //                 ctnmopvpr.ProgramCode == programCode &&
        //                 ctnmopvpr.Status == Helper.Active
        //            orderby mop.ShowingOrder
        //            select mop;


        //    return v.ToList();
        //}



        public List<Sal_ModeOfPayment> ReadModeOfPayment(string locationCode, string programCode, string packageCode, string customerType, string agreementDatePicker)
        {
            //SELECT MOP.* FROM Sal_PackageMaster PKGM 
            //                            INNER JOIN 
            //                            Sal_Validation_ModeOfPaymentVsPackage PKGVMOP 
            //                            ON PKGM.PackageCode = PKGVMOP.PackageCode
            //                            INNER JOIN Sal_Validation_CustomerTypeNModeOfPaymentVsProgram ctnmopvpr
            //                            ON PKGVMOP.ModeOfPaymentID = ctnmopvpr.ModeOfPaymentID
            //                            INNER JOIN Sal_ModeOfPayment MOP
            //                            ON MOP.ModeOfPaymentID=PKGVMOP.ModeOfPaymentID 
            //                            WHERE PKGM.PackageCode = 'bon001' AND PKGVMOP.[Status] = 0 
            //                            AND ctnmopvpr.ProgramCode = 'bonb01' AND
            //                            ctnmopvpr.CustomerType = 'HOUHLD'
            //                            AND ctnmopvpr.[Status] = 0 
            var v = from pkgm in context.Sal_PackageMaster
                    join pkgvmop in context.Sal_Validation_ModeOfPaymentVsPackage
                        on pkgm.PackageCode equals pkgvmop.PackageCode
                    join ctnmopvpr in context.Sal_Validation_ProgramVsCustomerTypeNModeOfPayment
                        on pkgvmop.ModeOfPaymentID equals ctnmopvpr.ModeOfPaymentID
                    join mop in context.Sal_ModeOfPayment
                        on pkgvmop.ModeOfPaymentID equals mop.ModeOfPaymentID
                    where
                         pkgm.PackageCode == packageCode &&
                         pkgvmop.Status == Helper.Active &&
                         ctnmopvpr.CustomerType == customerType &&
                         ctnmopvpr.ProgramCode == programCode &&
                         ctnmopvpr.Status == Helper.Active
                    orderby mop.ShowingOrder
                    select mop;
            return v.ToList();
        }

        public List<Inv_ItemMaster> ReadItemMaster()
        {
            return context.Inv_ItemMaster.Where(i => i.Status == Helper.Active).ToList();
        }

        public List<ItemCatNDescription> ReadItemCatNDescription()
        {
            string readItemCND = "SELECT DISTINCT ItemCategory,ic.[Description] FROM Inv_ItemMaster im "
                                 + " INNER JOIN Inv_ItemCategorySubCategory ic ON ic.ItemCategoryID = im.ItemCategory "
                                 + " WHERE IsItAWarrantyItem = 1 AND im.[Status] = 0 AND ic.[Status] = 0 ";
                             

            return context.Database.SqlQuery<ItemCatNDescription>(readItemCND).ToList();

        }

        public List<Inv_ItemMaster> ReadItemMasterByItemCategory(string itemCategory)
        {
            string itemMasterQuery = "SELECT itm.ItemCode, (itm.ItemName + ' - ' + itml.[Description])ItemName, " +
                                      "itm.ItemCategory,itm.ItemCapacity,itm.ItemModel,itm.AverageUnitCost,itm.FastOrSlowMoving,itm.[Status] " +
                                      "FROM Inv_ItemMaster itm " +
                                      "INNER JOIN Inv_ItemModel itml ON " +
                                      "itm.ItemModel = itml.ItemModelID " +
                                      "WHERE itm.ItemCategory = '" + itemCategory + "' " +
                                      "AND itm.[Status]=0";

            return context.Database.SqlQuery<Inv_ItemMaster>(itemMasterQuery).ToList();
        }

        public Inv_ItemMaster ReadItemMaster(string itemCode)
        {
            return context.Inv_ItemMaster.Where(i => i.ItemCode == itemCode).FirstOrDefault();
        }


        public List<Inv_ItemMaster> ReadItemMasterForItemCode(string itemCode)
        {
            //return context.Inv_ItemMaster.Where(i => i.Status == 0 && i.ItemType==INVTORYITM && i.ItemCode==itemCode).ToList();

            string itemMasterQuery = string.Empty;

            if (String.IsNullOrEmpty(itemCode))
            {
                itemMasterQuery = "SELECT  [ItemCode],[ItemName],[ItemType],[ItemCategory],[ItemCapacity],[ItemModel],[UKSeqNoForFANStationary],[AverageUnitCost] " +
                                            ",[FastOrSlowMoving],[IDCOL_Model],[IDCOL_Capacity],[IsItAWarrantyItem],[GeneralWarrantyPeriodInMonth] " +
                                            ",[VendorWarrantyPeriodInMonth],[IsItAObsoleteItem],[Status] " +
                                                    " FROM Inv_ItemMaster WHERE ItemType = 'INVTORYITM' AND [Status] = 0 ";
            }

            else if (!String.IsNullOrEmpty(itemCode))
            {
                itemMasterQuery = "SELECT  [ItemCode],[ItemName],[ItemType],[ItemCategory],[ItemCapacity],[ItemModel],[UKSeqNoForFANStationary],[AverageUnitCost] " +
                                           ",[FastOrSlowMoving],[IDCOL_Model],[IDCOL_Capacity],[IsItAWarrantyItem],[GeneralWarrantyPeriodInMonth] " +
                                           ",[VendorWarrantyPeriodInMonth],[IsItAObsoleteItem],[Status] " +
                                                   " FROM Inv_ItemMaster WHERE ItemType = 'INVTORYITM' AND [Status] = 0  AND [ItemCode] = ' " + itemCode + "' ";
            }

            return context.Database.SqlQuery<Inv_ItemMaster>(itemMasterQuery).ToList();
        }



        public Common_DistrictInfo ReadDistrict(string districtCode)
        {
            return context.Common_DistrictInfo.Where(d => d.DIST_CODE == districtCode).FirstOrDefault();
        }

        public CustomerInfoNPackageDetailsForSystemReturn ReadSystemReturnInfo(string customerCode, DateTime returnDate)
        {
            return context.CustomerInfoNPackageDetailsForSystemReturn(customerCode, returnDate).FirstOrDefault();
        }

        public List<InvAvailableItemInALocation> ReadInvAvailableItemInALocation(string locationCode, byte storeLocation, string itemCategory)
        {
            return context.InvAvailableItemInALocation(storeLocation, locationCode, itemCategory).ToList();
        }

        //public Inv_ItemSerialNoMaster ReadItemSerialNoMaster(string itemCode, string itemSerial, string itemCategory)
        //{
        //    return context.Inv_ItemSerialNoMaster.Where(m => m.ItemCategory == itemCategory && m.ItemCode == itemCode && m.ItemSerialNo == itemSerial).FirstOrDefault();
        //}

        public Inv_ItemNItemCategoryWithSerialNoMaster ReadItemSerialNoMasterByItemCategoryWise(string itemCode, string itemSerial, string itemCategory)
        {
            return context.Inv_ItemNItemCategoryWithSerialNoMaster.Where(m => m.ItemCategory == itemCategory && m.ItemCode == itemCode && m.ItemSerialNo == itemSerial).FirstOrDefault();
        }

        public Inv_ItemStockWithSerialNoByLocation ReadItemStockWithSerialNoByLocation(byte storeLocation, string locationCode, string itemCode, string serialNumber)
        {
            return context.Inv_ItemStockWithSerialNoByLocation.Where(i => i.StoreLocation == storeLocation && i.LocationCode == locationCode && i.ItemCode == itemCode && i.ItemSerialNo == serialNumber).FirstOrDefault();
        }

        public List<Inv_ItemTransactionTypes> ReadItemTransactionReceivedTypes(string itemType, byte locationType, string roleOrGroupID)
        {
            List<Inv_ItemTransactionTypes> lstItemTransactionTypes = new List<Inv_ItemTransactionTypes>();

            string isValidForInventoryFixedStationaryItem = string.Empty;

            if (itemType == Helper.InventoryItem)
            {
                isValidForInventoryFixedStationaryItem = "IsValidForInventoryItem = 1 ";
            }
            else if (itemType == Helper.FixedAssets)
            {
                isValidForInventoryFixedStationaryItem = "IsValidForFixedAssets = 1 ";
            }
            else if (itemType == Helper.StationaryItem)
            {
                isValidForInventoryFixedStationaryItem = "IsValidForConsumableItem = 1 ";
            }


            string itemTransactionTypeQuery = "SELECT itmtrnstyp.* FROM Inv_ItemTransactionTypes itmtrnstyp " +
                                              " INNER JOIN  " +
                                              " Inv_Validation_LocationTypeVsItemTransactionType locvitmtrnstyp " +
                                                 " ON itmtrnstyp.ItemTransTypeID = locvitmtrnstyp.ItemTransType " +
                                                 "INNER JOIN Inv_Validation_UserRoleOrGroupVsItemTransactionType ugvit ON itmtrnstyp.ItemTransTypeID = ugvit.ItemTransType " +
                                                   " WHERE " +
                                                   "  (itmtrnstyp.ReceiveOrIssue = 'C' OR itmtrnstyp.ReceiveOrIssue = 'R')  " +
                                                   " AND  itmtrnstyp.IsOnlyForAutoEntry = 0 AND itmtrnstyp." + isValidForInventoryFixedStationaryItem +
                                                   "  AND itmtrnstyp.[Status] = 0 AND " +
                                                   "  locvitmtrnstyp.[Status]= 0 AND " +
                                                   "  ugvit.[Status]=0 AND " +
                                                   "  locvitmtrnstyp.LocationType = " + locationType + " AND ugvit.UserRoleOrGroupID = '" + roleOrGroupID + "'";

            lstItemTransactionTypes = context.Database.SqlQuery<Inv_ItemTransactionTypes>(itemTransactionTypeQuery).ToList();

            return lstItemTransactionTypes;
        }

        public List<Inv_ItemTransactionTypes> ReadItemTransactionIssuedTypes(string itemType, byte locationType, string roleOrGroupID)
        {
            List<Inv_ItemTransactionTypes> lstItemTransactionTypes = new List<Inv_ItemTransactionTypes>();
            string isValidForInventoryFixedStationaryItem = string.Empty;

            if (itemType == Helper.InventoryItem)
            {
                isValidForInventoryFixedStationaryItem = "IsValidForInventoryItem = 1";
            }
            else if (itemType == Helper.FixedAssets)
            {
                isValidForInventoryFixedStationaryItem = "IsValidForFixedAssets = 1";
            }
            else if (itemType == Helper.StationaryItem)
            {
                isValidForInventoryFixedStationaryItem = "IsValidForConsumableItem = 1";
            }


            string itemTransactionTypeQuery = "SELECT itmtrnstyp.* FROM Inv_ItemTransactionTypes itmtrnstyp " +
                                              " INNER JOIN  " +
                                              " Inv_Validation_LocationTypeVsItemTransactionType locvitmtrnstyp " +
                                                 " ON itmtrnstyp.ItemTransTypeID = locvitmtrnstyp.ItemTransType " +
                                                 "INNER JOIN Inv_Validation_UserRoleOrGroupVsItemTransactionType ugvit ON itmtrnstyp.ItemTransTypeID = ugvit.ItemTransType " +
                                                   " WHERE " +
                                                   "  (itmtrnstyp.ReceiveOrIssue = 'C' OR itmtrnstyp.ReceiveOrIssue = 'I')  " +
                                                   " AND  itmtrnstyp.IsOnlyForAutoEntry = 0 AND itmtrnstyp." + isValidForInventoryFixedStationaryItem +
                                                   "  AND itmtrnstyp.[Status] = 0 AND " +
                                                   "  locvitmtrnstyp.[Status]= 0 AND " +
                                                   "  ugvit.[Status]=0 AND " +
                                                   "  locvitmtrnstyp.LocationType = " + locationType + " AND ugvit.UserRoleOrGroupID = '" + roleOrGroupID + "'";

            lstItemTransactionTypes = context.Database.SqlQuery<Inv_ItemTransactionTypes>(itemTransactionTypeQuery).ToList();

            return lstItemTransactionTypes;
        }

        public List<Inv_ItemTransactionTypes> ReadItemTransactionIssuedReceivedTypes(string itemType, string issueOrReceivedType, string roleOrGroupID)
        {
            List<Inv_ItemTransactionTypes> lstItemTransactionTypes = new List<Inv_ItemTransactionTypes>();

            if (itemType == Helper.InventoryItem)
            {
                lstItemTransactionTypes = context.Inv_ItemTransactionTypes.Where(c => c.IsValidForInventoryItem == true && c.ReceiveOrIssue == issueOrReceivedType).ToList();
            }
            else if (itemType == Helper.FixedAssets)
            {
                lstItemTransactionTypes = context.Inv_ItemTransactionTypes.Where(c => c.IsValidForFixedAssets == true && c.ReceiveOrIssue == issueOrReceivedType).ToList();
            }
            else if (itemType == Helper.StationaryItem)
            {
                lstItemTransactionTypes = context.Inv_ItemTransactionTypes.Where(c => c.IsValidForConsumableItem == true && c.ReceiveOrIssue == issueOrReceivedType).ToList();
            }
            return lstItemTransactionTypes;
        }

        public List<Inv_ItemMaster> ReadInvItems(string itemCategory)
        {

            //string strItems = "SELECT ItemCode,'['+ItemCode+'] '+ItemName+' '+ ISNULL(imod.[Description],'') ItemName,im.ItemCategory,im.ItemCapacity,im.ItemModel,im.AverageUnitCost,im.FastOrSlowMoving,im.IDCOL_Model,im.IDCOL_Capacity,im.Status  FROM Inv_ItemMaster im " +
            //                  " LEFT OUTER JOIN Inv_ItemModel imod ON im.ItemModel = imod.ItemModelID" +
            //                  " WHERE im.ItemCategory = '" + itemCategory + "'";

            //string strItems = "SELECT ItemCode, ItemName+' '+ ISNULL(imod.[Description],'') +' ['+ItemCode+']' ItemName,im.ItemCategory,im.ItemCapacity,im.ItemModel,im.UKSeqNoForFANStationary,im.AverageUnitCost,im.FastOrSlowMoving,im.IDCOL_Model,im.IDCOL_Capacity,im.Status  FROM Inv_ItemMaster im " +
            //                          " LEFT OUTER JOIN Inv_ItemModel imod ON im.ItemModel = imod.ItemModelID" +
            //                          " WHERE im.ItemCategory = '" + itemCategory + "'";

            string itemQuery = "SELECT ItemCode, ItemName+' ['+ItemCode+']' ItemName, im.ItemType, im.ItemCategory,im.ItemCapacity,im.ItemModel,im.UKSeqNoForFANStationary,im.AverageUnitCost,im.FastOrSlowMoving,im.IDCOL_Model,im.IDCOL_Capacity, im.IsItAWarrantyItem, im.IsItAObsoleteItem, im.GeneralWarrantyPeriodInMonth, im.VendorWarrantyPeriodInMonth, im.Status  FROM Inv_ItemMaster im " +
                                      " LEFT OUTER JOIN Inv_ItemModel imod ON im.ItemModel = imod.ItemModelID" +
                                      " WHERE im.ItemCategory = '" + itemCategory + "'";




            var lstItems = context.Database.SqlQuery<Inv_ItemMaster>(itemQuery);
            return lstItems.ToList();
        }

        public List<Inv_ItemMaster> ReadInvItems(string itemCategory, string itemTransType)
        {

            //string strItems = "SELECT ItemCode,'['+ItemCode+'] '+ItemName+' '+ ISNULL(imod.[Description],'') ItemName,im.ItemCategory,im.ItemCapacity,im.ItemModel,im.AverageUnitCost,im.FastOrSlowMoving,im.IDCOL_Model,im.IDCOL_Capacity,im.Status  FROM Inv_ItemMaster im " +
            //                  " LEFT OUTER JOIN Inv_ItemModel imod ON im.ItemModel = imod.ItemModelID" +
            //                  " WHERE im.ItemCategory = '" + itemCategory + "'";

            //string strItems = "SELECT ItemCode, ItemName+' '+ ISNULL(imod.[Description],'') +' ['+ItemCode+']' ItemName,im.ItemCategory,im.ItemCapacity,im.ItemModel,im.UKSeqNoForFANStationary,im.AverageUnitCost,im.FastOrSlowMoving,im.IDCOL_Model,im.IDCOL_Capacity,im.Status  FROM Inv_ItemMaster im " +
            //                          " LEFT OUTER JOIN Inv_ItemModel imod ON im.ItemModel = imod.ItemModelID" +
            //                          " WHERE im.ItemCategory = '" + itemCategory + "'";

            //string strItems = "SELECT ItemCode, ItemName+' ['+ItemCode+']' ItemName, im.ItemType, im.ItemCategory,im.ItemCapacity,im.ItemModel,im.UKSeqNoForFANStationary,im.AverageUnitCost,im.FastOrSlowMoving,im.IDCOL_Model,im.IDCOL_Capacity,im.Status  FROM Inv_ItemMaster im " +
            //                          " LEFT OUTER JOIN Inv_ItemModel imod ON im.ItemModel = imod.ItemModelID" +
            //                          " WHERE im.ItemCategory = '" + itemCategory + "'";

            string itemQuery = " SELECT im.ItemCode, im.ItemName+' ['+im.ItemCode+']' ItemName, im.ItemType, " +
                             " im.ItemCategory,im.ItemCapacity,im.ItemModel,im.UKSeqNoForFANStationary,im.AverageUnitCost, im.IsItAWarrantyItem," +
                             " im.GeneralWarrantyPeriodInMonth,im.VendorWarrantyPeriodInMonth,im.IsItAObsoleteItem, im.FastOrSlowMoving,im.IDCOL_Model,im.IDCOL_Capacity,im.Status   " +
                             " FROM Inv_ItemMaster im   " +
                             " LEFT OUTER JOIN Inv_ItemModel imod ON im.ItemModel = imod.ItemModelID " +
                             " LEFT OUTER JOIN Inv_Validation_ItemTransactionTypeVsItemCategoryVsItemCode itmtrnvitmcatvitmc " +
                             " ON im.ItemCode = itmtrnvitmcatvitmc.ItemCode AND " +
                             " im.ItemCategory = itmtrnvitmcatvitmc.ItemCategory " +
                             " WHERE im.ItemCategory = '" + itemCategory + "' AND " +
                             " itmtrnvitmcatvitmc.ItemTransType = '" + itemTransType + "'";


            var lstItems = context.Database.SqlQuery<Inv_ItemMaster>(itemQuery);
            return lstItems.ToList();
        }

        public List<CustomerLedgerReport> ReadCustomerLedgerReport(string customerCode)
        {
            return context.CustomerLedgerReport(customerCode).ToList();
        }

        public Inv_ItemStockByLocation ReadItemStockByLocation(byte storeLocation, string locationCode, string itemCode)
        {
            return context.Inv_ItemStockByLocation.Where(i => i.StoreLocation == storeLocation && i.LocationCode == locationCode && i.ItemCode == itemCode).FirstOrDefault();
        }

        public Inv_ChallanMaster ReadChallanMasterByChallanSequence(string challanSequenceNumber, string locationCode)
        {
            return context.Inv_ChallanMaster.Where(c => c.ChallanSeqNo == challanSequenceNumber && c.LocationCode == locationCode).FirstOrDefault();
        }

        public Inv_ChallanMaster ReadChallanMaster(string locationCode, string challanSequenceNumber)
        {
            return context.Inv_ChallanMaster.Where(c => c.ChallanToLocationCode == locationCode && c.ChallanSeqNo == challanSequenceNumber).FirstOrDefault();
        }

        public List<Inv_ChallanMaster> ReadChallanMaster(string locationCode)
        {
            return context.Inv_ChallanMaster.Where(c => c.ChallanToLocationCode == locationCode && c.Status == Helper.Active).ToList();
        }

        public List<ChallanInboxForChallanWithMRR> ReadChallanInbox(string locationCode, string itemType)
        {
            return context.ChallanInboxForChallanWithMRR(locationCode, itemType).ToList();
        }

        public bool IsItemTransationRelatedToVendor(string itemTransactionType)
        {
            bool IsItRelatedToVendor = false;
            var v = context.Inv_ItemTransactionTypes.Where(w => w.ItemTransTypeID == itemTransactionType).Select(s => s.IsItRelatedToVendor).FirstOrDefault();

            if (v != null)
                IsItRelatedToVendor = Convert.ToBoolean(v);

            return IsItRelatedToVendor;
        }

        public string ChallanSequenceNumberMax(string locationCode, string yearMonthDate)
        {
            string challanSeqQuery = "SELECT TOP(1) ISNULL(chm.ChallanSeqNo,'') ChallanSeqNo FROM " +
                                     "Inv_ChallanMaster chm WHERE chm.LocationCode = '" + locationCode + "' AND " +
                                     "CONVERT(INT,SUBSTRING(chm.ChallanSeqNo,1, 6)) = '" + yearMonthDate + "' " +
                                     "ORDER BY  CONVERT(INT,SUBSTRING(chm.ChallanSeqNo,1, 6)) DESC, " +
                                     "CONVERT(INT,SUBSTRING(chm.ChallanSeqNo,7, LEN(chm.ChallanSeqNo))) DESC ";

            //"SELECT TOP(1) chm.ChallanSeqNo FROM Inv_ChallanMaster chm " +
            //"WHERE chm.LocationCode = '" + locationCode + "'  " +
            //"ORDER BY  CONVERT(INT,SUBSTRING(chm.ChallanSeqNo,7, LEN(chm.ChallanSeqNo))) DESC";

            var seqNumber = context.Database.SqlQuery<string>(challanSeqQuery).FirstOrDefault();

            string challanNumber = string.Empty;

            if (seqNumber != null)
            {
                challanNumber = seqNumber.ToString();
            }

            return challanNumber;
        }

        public string SparseChallanSequenceNumberMax(string locationCode, string yearMonthDate)
        {
            string challanSeqQuery = "SELECT TOP(1) ISNULL(spsm.SPSSeqNo,'') SPSSeqNo FROM " +
                                     "Sal_SparePartsSalesMaster spsm WHERE spsm.LocationCode = '" + locationCode + "' AND " +
                                     "CONVERT(INT,SUBSTRING(spsm.SPSSeqNo,1, 6)) = '" + yearMonthDate + "' " +
                                     "ORDER BY  CONVERT(INT,SUBSTRING(spsm.SPSSeqNo,1, 6)) DESC, " +
                                     "CONVERT(INT,SUBSTRING(spsm.SPSSeqNo,7, LEN(spsm.SPSSeqNo))) DESC ";

            //"SELECT TOP(1) spsm.SPSSeqNo FROM Sal_SparePartsSalesMaster spsm " +
            //                     "WHERE spsm.LocationCode = '" + locationCode + "'  " +
            //                     "ORDER BY  CONVERT(INT,SUBSTRING(spsm.SPSSeqNo,7, LEN(spsm.SPSSeqNo))) DESC";

            var seqNumber = context.Database.SqlQuery<string>(challanSeqQuery).FirstOrDefault();

            string challanNumber = string.Empty;

            if (seqNumber != null)
            {
                challanNumber = seqNumber.ToString();
            }

            return challanNumber;
        }

        public string MRRSequenceNumberMax(string locationCode, string yearMonthDate)
        {
            string mrrSeqQuery = "SELECT TOP(1) ISNULL(mrrm.MRRSeqNo,'') MRRSeqNo FROM " +
                                 "Inv_MRRMaster mrrm WHERE mrrm.LocationCode = '" + locationCode + "' AND " +
                                 "CONVERT(INT,SUBSTRING(mrrm.MRRSeqNo,1, 6)) = '" + yearMonthDate + "' " +
                                 "ORDER BY  CONVERT(INT,SUBSTRING(mrrm.MRRSeqNo,1, 6)) DESC, " +
                                 "CONVERT(INT,SUBSTRING(mrrm.MRRSeqNo,7, LEN(mrrm.MRRSeqNo))) DESC ";

            //"SELECT TOP(1) ISNULL(mrrm.MRRSeqNo,'')MRRSeqNo FROM Inv_MRRMaster mrrm " +
            //                     "WHERE mrrm.LocationCode = '" + locationCode + "' " +
            //                     "ORDER BY  CONVERT(INT,SUBSTRING(mrrm.MRRSeqNo,7, LEN(mrrm.MRRSeqNo))) DESC";

            var seqNumber = context.Database.SqlQuery<string>(mrrSeqQuery).FirstOrDefault();

            string mrrNumber = string.Empty;

            if (seqNumber != null)
            {
                mrrNumber = seqNumber.ToString();
            }

            return mrrNumber;
        }

        public List<REPSummaryForTheDayClosingForSales> ReadREPSummaryForTheDayClosingForSalesData(string reportType, string locationCode)
        {
            //string strExecute = "EXEC REP_SummaryForTheDayClosing_ForAccountingInfo";
            return context.REPSummaryForTheDayClosingForSales(reportType, locationCode).ToList();

        }

        public List<SummaryForTheDayClosingForCollectionInfo> ReadSummaryForTheDayClosingForCollectionInfo(string reportType, string locationCode)
        {
            return context.SummaryForTheDayClosingForCollectionInfo(reportType, locationCode).ToList();
        }

        public List<SummaryForTheDayClosingForInventoryInfo> ReadSummaryForTheDayClosingForInventoryInfo(string reportType, string locationCode, byte storeLocation)
        {
            return context.SummaryForTheDayClosingForInventoryInfo(reportType, locationCode, storeLocation).ToList();
        }
        public List<SummaryForTheDayClosingForAccountingInfo> ReadSummaryForTheDayClosingForAccountingInfo(string reportType, string locationCode)
        {
            return context.SummaryForTheDayClosingForAccountingInfo(reportType, locationCode).ToList();
        }

        public List<MRRDetails> ReadMRRDetails(string locationCode, string mrrLocationCode, string challanSequenceNumber)
        {
            return context.MRRDetails(locationCode, challanSequenceNumber, mrrLocationCode).ToList();
        }

        public Int16 ItemTransactionSequenceMAX(byte storeLocation, string yearMonth, string locationCode, string itemCode, DateTime transDate)
        {
            string transectionSeqQuery = "SELECT TOP(1) ISNULL(itt.TransSeqNo, 0)TransSeqNo FROM Inv_ItemTransaction itt  " +
                                         "WHERE itt.StoreLocation = " + storeLocation + " AND itt.YearMonth = '" + yearMonth + "' AND itt.LocationCode = '" + locationCode + "' AND " +
                                         "itt.ItemCode = '" + itemCode + "' AND itt.TransDate = '" + transDate + "' " +
                                         "ORDER BY  itt.TransSeqNo DESC";

            Int16 transSequenceNumber = 0;
            var seqMax = context.Database.SqlQuery<Int16>(transectionSeqQuery).FirstOrDefault();

            if (seqMax != null)
                transSequenceNumber = Convert.ToInt16(seqMax);

            return transSequenceNumber;
        }

        public List<SalGetAvailableSerialNoForMRR> ReadChallanDetailsWithSerialNo(string challanSequenceNumber, string itemCode, string refDocType, string challanLocationCode)
        {
            return context.SalGetAvailableSerialNoForMRR(refDocType, challanLocationCode, challanSequenceNumber, itemCode).ToList();
        }

        public Inv_ChallanDetails ReadChallanDetails(string locationCode, string challanSequenceNumber, string itemCode)
        {
            return context.Inv_ChallanDetails.Where(c => c.LocationCode == locationCode && c.ChallanSeqNo == challanSequenceNumber && c.ItemCode == itemCode).FirstOrDefault();
        }

        public List<GetCustomerTransferredButNotYetAccepted> ReadCustomerTransferredButNotYetAccepted(string locationCode)
        {
            return context.GetCustomerTransferredButNotYetAccepted(locationCode).ToList();
        }

        public List<LocationWiseEmployeeTargetEntryCheck> ReadLocationWiseEmployeeTargetEntryCheck(string locatioCode, string YearMonth)
        {
            return context.LocationWiseEmployeeTargetEntryCheck(locatioCode, YearMonth).ToList();
        }

        public bool IsInventoryStockUpdateFinish(string locationCode)
        {
            Common_LocationInfo objLocationInfo = new Common_LocationInfo();
            objLocationInfo = context.Common_LocationInfo.Where(l => l.LocationCode == locationCode).FirstOrDefault();
            bool inventoryStockUpdateFinishOrNot = false;

            if (objLocationInfo.TempIsInventoryStockUpdateFinish != null)
            {
                if (objLocationInfo.TempIsInventoryStockUpdateFinish != false)
                {
                    inventoryStockUpdateFinishOrNot = true;
                }
            }

            return inventoryStockUpdateFinishOrNot;
        }

        public Sal_SalesAgreement ReadSalesAgreement(string customerCode)
        {
            return context.Sal_SalesAgreement.Where(s => s.CustomerCode == customerCode).FirstOrDefault();
        }

        public List<PackageDetailsForSystemReturn> ReadPackageDetailsForSystemReturn(string customerCode, DateTime returnDate)
        {
            return context.PackageDetailsForSystemReturn(customerCode, returnDate).ToList();
        }

        public List<PanelSerialList> PanelSerialByLocationAndStock(string locationCode, byte storeLocation, string itemCategory, string itemCapacity, byte agreementType, string packageCode)
        {
            string panelSerialByLocationAndStockQuery = string.Empty;


            if (agreementType == 1 || agreementType == 2 || agreementType == 5)
            {
                panelSerialByLocationAndStockQuery =
                    "SELECT isws.ItemSerialNo, im.ItemModel " +
        "FROM Inv_ItemMaster im   " +
        "INNER JOIN  " +
        "Inv_ItemStockWithSerialNoByLocation isws  " +
        "   ON im.ItemCode = isws.ItemCode   " +
        "		WHERE isws.LocationCode = '" + locationCode + "' AND  " +
        "	isws.StoreLocation = " + storeLocation + " AND  " +
        "	isws.IsAvailableInStore = 1 AND " +
        "	im.ItemCapacity = '" + itemCapacity + "' AND " +
        "	isws.ItemSerialNo NOT IN ( " +
        "			Select sp.PanelSerialNo From Sal_Special_SpecialPackagePriceForResale sp  " +
        "			Where sp.PackageCode = '" + packageCode + "' )";


            }
            else
            {
                panelSerialByLocationAndStockQuery =

                    "DECLARE @PackageCode CHAR(6); " +

                    "Select @PackageCode = sp.PackageCode From Sal_Special_SpecialPackagePriceForResale sp  " +
                    "Where sp.PackageCode = '" + packageCode + "' " +

                    "IF(@PackageCode IS NULL) " +
                    "BEGIN " +
                    "		SELECT isws.ItemSerialNo, im.ItemModel  " +
                    "		FROM Inv_ItemMaster im   " +
                    "		INNER JOIN  " +
                    "		Inv_ItemStockWithSerialNoByLocation isws  " +
                    "		    ON im.ItemCode = isws.ItemCode  " +
                    "				WHERE isws.LocationCode = '" + locationCode + "' AND  " +
                    "			isws.StoreLocation = " + storeLocation + " AND  " +
                    "			isws.IsAvailableInStore = 1 AND    " +
                    "			im.ItemCapacity = '" + itemCapacity + "' AND " +
                    "			isws.ItemSerialNo NOT IN ( " +
                    "					Select sp.PanelSerialNo From Sal_Special_SpecialPackagePriceForResale sp  " +
                    "					Where sp.PackageCode = '" + packageCode + "' " +
                    "			) END " +
                    "ELSE		 " +
                    "BEGIN " +
                    "        SELECT isws.ItemSerialNo, im.ItemModel  " +
                    "		FROM Inv_ItemMaster im   " +
                    "		INNER JOIN  " +
                    "		Inv_ItemStockWithSerialNoByLocation isws  " +
                    "		    ON im.ItemCode = isws.ItemCode   " +
                    "		LEFT OUTER JOIN Sal_Special_SpecialPackagePriceForResale sp  " +
                    "		    ON isws.ItemSerialNo = sp.PanelSerialNo  " +
                    "			WHERE isws.LocationCode = '" + locationCode + "' AND  " +
                    "			isws.StoreLocation = " + storeLocation + " AND  " +
                    "			isws.IsAvailableInStore = 1 AND   " +
                    "			im.ItemCapacity = '" + itemCapacity + "' AND  " +
                    "			sp.PackageCode = '" + packageCode + "' " +
                    "END ";
            }

            return context.Database.SqlQuery<PanelSerialList>(panelSerialByLocationAndStockQuery).ToList();
        }

        public DepretiatedPackagePriceBySRPanelSerial GetDepretiatedPackagePriceBySRPanelSerial(string panelSerial, string packageCode)
        {
            try
            {
                return context.DepretiatedPackagePriceBySRPanelSerial(panelSerial, packageCode).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<ProgressReview> ReadProgressReviewGraph()
        {

            var progressReviewGraph = context.ProgressReview().ToList();
            return progressReviewGraph;

        }

        public List<CollectionEfficiencyByCustomer> ReadCollectionEfficiencyByCustomer(string locationCode, string yearMonth)
        {
            return context.CollectionEfficiencyByCustomer(yearMonth, locationCode).ToList();
        }

        public List<LocationInfo> Location()
        {
            var v = from lcinf in context.Common_LocationInfo
                    where lcinf.ParentLocation == "9000" && lcinf.Status == Helper.Active
                    select new LocationInfo
                    {
                        LocationCode = lcinf.LocationCode,
                        LocationName = lcinf.LocationName,
                        BackDayAllowedForTransaction = lcinf.BackDayAllowedForTransaction
                    };

            return v.ToList();
        }

        public List<LocationInfo> LocationWithHeadOffice()
        {
            var v = from lcinf in context.Common_LocationInfo
                    where (lcinf.ParentLocation == "9000" || lcinf.ParentLocation == null) && lcinf.Status == Helper.Active
                    select new LocationInfo
                    {
                        LocationCode = lcinf.LocationCode,
                        LocationName = lcinf.LocationName,
                        BackDayAllowedForTransaction = lcinf.BackDayAllowedForTransaction
                    };

            return v.ToList(); //context.Common_LocationInfo.Where(l => l.ParentLocation == "9000" || l.ParentLocation == null).ToList();
        }

        public LocationInfo Location(string LocationCode)
        {
            var v = from lcinf in context.Common_LocationInfo
                    where lcinf.LocationCode == LocationCode && lcinf.Status == Helper.Active
                    select new LocationInfo
                    {
                        LocationCode = lcinf.LocationCode,
                        LocationName = lcinf.LocationName,
                        ParentLocationCode = lcinf.ParentLocation,
                        BackDayAllowedForTransaction = lcinf.BackDayAllowedForTransaction,
                        CustomerCodePrefix = lcinf.NewLocationCode,
                        LocationType = lcinf.LocationType
                    };

            return v.FirstOrDefault();

            //return context.Common_LocationInfo.Where(li => li.LocationCode == LocationCode).FirstOrDefault();
        }

        public List<LocationInfo> LocationByLocationCode(string LocationCode)
        {
            var v = from lcinf in context.Common_LocationInfo
                    where lcinf.ParentLocation == LocationCode && lcinf.Status == Helper.Active
                    select new LocationInfo
                    {
                        LocationCode = lcinf.LocationCode,
                        LocationName = lcinf.LocationName,
                        BackDayAllowedForTransaction = lcinf.BackDayAllowedForTransaction
                    };

            return v.ToList();

            //return context.Common_LocationInfo.Where(li => li.LocationCode == LocationCode).FirstOrDefault();
        }

        public List<CustomerFPRAndScheduledCollectionEntry> GetCustomerFPRAndScheduledCollectionEntry(string unitCode, string optionForMissingFPROrDay, string prmEmployeeID, string prmScheduledCollectionDay)
        {
            string strCustomerFPRAndScheduledCollectionEntry = string.Empty;
            strCustomerFPRAndScheduledCollectionEntry = "EXEC REP_GetUnitWiseCustomerListToAssignCustomerFPRNScheduleColcDay '" + unitCode + "','" + optionForMissingFPROrDay + "','" + prmEmployeeID + "','" + prmScheduledCollectionDay + "'";

            try
            {
                return context.Database.SqlQuery<CustomerFPRAndScheduledCollectionEntry>(strCustomerFPRAndScheduledCollectionEntry).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<CustomerFPRNDayWiseRegularOrODTarget> GetCustomerFPRNDayWiseRegularOrODTarget(string unitCode, string employeeID)
        {
            string strCustomerFPRAndScheduledCollectionEntry = string.Empty;
            strCustomerFPRAndScheduledCollectionEntry = "EXEC REP_GetCustomerFPRnDayWiseRegularOrODTarget '" + unitCode + "', '" + employeeID + "'";

            try
            {
                return context.Database.SqlQuery<CustomerFPRNDayWiseRegularOrODTarget>(strCustomerFPRAndScheduledCollectionEntry).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<LocationNEmployeeWiseDailySalesNCollectionTarget> ReadLocationNEmployeeWiseDailySalesNCollectionTarget(string locationCode)
        {
            return context.LocationNEmployeeWiseDailySalesNCollectionTarget(locationCode).ToList();
        }

        public List<SalSalesNCollectionTargetVsAchievementForGraph> ReadSalSalesNCollectionTargetVsAchievementForGraph(string reportType, string locationCode, string employeeID)
        {
            ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext.CommandTimeout = 600;
            return context.SalSalesNCollectionTargetVsAchievementForGraph(reportType, locationCode, employeeID).ToList();
        }

        public List<ItemLedgerReport> ReadInvItemLedger(byte storeLocation, string locationCode, string itemCode, string fromDate, string toDate)
        {
            try
            {
                SqlParameter[] storeParam = new SqlParameter[5];
                storeParam[0] = new SqlParameter("prmStoreLocation", storeLocation);
                storeParam[1] = new SqlParameter("prmLocationCode", locationCode);
                storeParam[2] = new SqlParameter("prmItemCode", itemCode);
                storeParam[3] = new SqlParameter("prmFromDate", fromDate);
                storeParam[4] = new SqlParameter("prmToDate", toDate);

                var v = context.Database.SqlQuery<ItemLedgerReport>("Exec REP_InvItemLedger @prmStoreLocation, @prmLocationCode, @prmItemCode, @prmFromDate, @prmToDate", storeParam);

                return v.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Sal_CustomerFuelUsed> ReadCustomerFuelUsed()
        {
            return context.Sal_CustomerFuelUsed.OrderBy(o => o.SortingOrder).ToList();
        }

        public List<Sal_CustomerIncomeRange> ReadCustomerIncomeRange()
        {
            return context.Sal_CustomerIncomeRange.OrderBy(o => o.SortingOrder).ToList();
        }

        public List<Sal_CustomerOccupations> ReadCustomerOccupations()
        {
            return context.Sal_CustomerOccupations.OrderBy(o => o.SortingOrder).ToList();
        }

        public List<Sal_CustomerRelations> ReadCustomerRelations()
        {
            return context.Sal_CustomerRelations.OrderBy(o => o.SortingOrder).ToList();
        }

        public List<InvChallanOrMRRDetailsForItemLedger> ReadInvChallanDetails(string locationCode, string challanOrMRRSeqNo, string challanOrMRR)
        {
            return context.InvChallanOrMRRDetailsForItemLedger(locationCode.Trim(), challanOrMRRSeqNo.Trim(), challanOrMRR.Trim()).ToList();
        }

        public List<Sal_ListedUnitPriceForSparePartsSales> ReadListedUnitPriceForSparePartsSales()
        {
            return context.Sal_ListedUnitPriceForSparePartsSales.ToList();
        }

        public Sal_ListedUnitPriceForSparePartsSales ReadListedUnitPriceForSparePartsSales(string yearMonth, string itemCode, byte stockLocation)
        {
            return context.Sal_ListedUnitPriceForSparePartsSales.Where(s => s.YearMonth == yearMonth && s.ItemCode == itemCode && s.StoreLocation == stockLocation).FirstOrDefault();
        }

        public List<InventorySummaryReportV2> ReadInventorySummaryReportV2(string reportType, string itemType, byte storeLocation, string locationCode, DateTime startDate, DateTime endDate, string vendorType)
        {
            return context.InventorySummaryReportV2(reportType, itemType, storeLocation, locationCode, vendorType, startDate, endDate).ToList();  // comment by mahin
            //try
            //{
            //    SqlParameter[] storeParam = new SqlParameter[6];
            //    storeParam[0] = new SqlParameter("prmReportType", reportType);
            //    storeParam[1] = new SqlParameter("prmItemType", itemType);
            //    storeParam[2] = new SqlParameter("prmStoreLocation", storeLocation);
            //    storeParam[3] = new SqlParameter("prmLocationCode", locationCode);
            //    storeParam[4] = new SqlParameter("prmFromDate", startDate);
            //    storeParam[5] = new SqlParameter("prmToDate", endDate);

            //    //ObjectResult<ItemSummaryReportV2> resultList = context.Database.SqlQuery<ItemSummaryReportV2>
            //    //    ("Exec REP_ItemSummaryReportV2 @prmReportType, @prmItemType, @prmStoreLocation, @prmLocationCode,@prmFromDate,@prmToDate", storeParam);
            //    return resultList.ToList();
            //}catch(Exception ex)
            //{
            //    throw;
            //}

        }

        public List<InventoryERPVersusPhysicalBalance> ReadInventoryERPVersusPhysicalBalance(string reportType, string itemType, byte storeLocation, string locationCode, string yearMonth, string vendorType)
        {
            return context.InventoryERPVersusPhysicalBalance(storeLocation, yearMonth, locationCode).ToList();  // comment by Md.Sultan Mahmud

        }

        public string ReadCustomerTraineeName(string trainerId)
        {

            string traineeQuery = string.Format(" SELECT DISTINCT STD.TraineeName " +
                                  " FROM [RASolarERP_IDCOL_TrainingModule].[dbo].[Solar_TOT_TrainingDetail] STD "+
                                 " WHERE TraineeTypeNo = 2 AND IsPasses = 1 AND [Status] = 0 AND STD.RSF_EmployeeID='"+trainerId+"' ") ;


            var traineeNameQuery = context.Database.SqlQuery<string>(traineeQuery).FirstOrDefault();

            string traineeName = string.Empty;

            if (traineeNameQuery != null)
            {
                traineeName = traineeNameQuery.ToString();
            }

            return traineeName;
        }

        public List<CustomerTrainingInfo> ReadCustomerTrainingInfo(string unitCode, bool trainingStatus, byte trainingBatchNo)
        {
            return context.CustomerTrainingInfo(unitCode, trainingStatus).ToList();
        }

        public List<GetUnitWiseCustomerTrainingSchedule> ReadGetUnitWiseCustomerTrainingSchedule(string unitCode, DateTime? trainingDate, byte? trainingBatchNumber)
        {
            return context.GetUnitWiseCustomerTrainingSchedule(unitCode, trainingDate, trainingBatchNumber).ToList();
        }

        public List<Sal_ItemSetMaster> ReadItemSetMaster()
        {
            return context.Sal_ItemSetMaster.Where(s => s.Status == Helper.Active).ToList();
        }
        public List<Sal_ItemSetDetail> ReadItemSetDetails(string itemSetCode)
        {
            return context.Sal_ItemSetDetail.Where(s => s.ItemSetCode == itemSetCode && s.Status == Helper.Active).ToList();
        }

        public List<SparePartSetDetils> ReadSparePartSetDetils(string itemSetCode)
        {
            return context.SparePartSetDetils(itemSetCode).ToList();
        }

        public Sal_ItemSetMaster ReadItemSetMaster(string itemSetCode)
        {
            return context.Sal_ItemSetMaster.Where(s => s.ItemSetCode == itemSetCode).FirstOrDefault();
        }

        public string ChallanOrMRRForAuditAdjustmentSequenceMax(string locationCode, string yearMonthDate)
        {
            string challanSeqQuery = "SELECT TOP(1) ISNULL(chm.DocSeqNo,'') DocSeqNo FROM " +
                                     "Aud_AuditAdjustmentRelatedChallanOrMRRForReference chm WHERE chm.LocationCode = '" + locationCode + "' AND " +
                                     "CONVERT(INT,SUBSTRING(chm.DocSeqNo,1, 6)) = '" + yearMonthDate + "' " +
                                     "ORDER BY  CONVERT(INT,SUBSTRING(chm.DocSeqNo,1, 6)) DESC, " +
                                     "CONVERT(INT,SUBSTRING(chm.DocSeqNo,7, LEN(chm.DocSeqNo))) DESC ";

            //"SELECT TOP(1) chm.DocSeqNo FROM Inv_RefChallanOrMRRForAuditAdjustment chm " +
            //                     "WHERE chm.LocationCode = '" + locationCode + "' " +
            //                     "ORDER BY  CONVERT(INT,SUBSTRING(chm.DocSeqNo,7, LEN(chm.DocSeqNo))) DESC";

            var seqNumber = context.Database.SqlQuery<string>(challanSeqQuery).FirstOrDefault();

            string challanNumber = string.Empty;

            if (seqNumber != null)
            {
                challanNumber = seqNumber.ToString();
            }

            return challanNumber;
        }

        public List<SummarySheetForRegionalSalesPosting> ReadSummarySheetForRegionalSalesPosting(DateTime dateFrom, DateTime dateTo, string regionCode)
        {
            return context.SummarySheetForRegionalSalesPosting(dateFrom, dateTo, regionCode).ToList();
        }

        public List<FixedAssetSerialList> GetFixedAssetSerialList(byte storeLocation, string locationCode, string itemCode, string option)
        {

            string selectOption = string.Empty;

            if (string.IsNullOrEmpty(option))
            {
                selectOption = "FORUNASSIGN";
            }
            else
            {
                selectOption = "VIEWASSIGNANDUNASSIGN";
            }

            SqlParameter[] storeParam = new SqlParameter[6];
            storeParam[0] = new SqlParameter("Option", selectOption);
            storeParam[1] = new SqlParameter("prmStoreLocation", storeLocation);
            storeParam[2] = new SqlParameter("LocationCode", locationCode);
            storeParam[3] = new SqlParameter("prmItemCode", itemCode);
            storeParam[4] = new SqlParameter("EmployeeID", "");
            storeParam[5] = new SqlParameter("prmAvailableToAssignQuantity", "");

            var resultList = context.Database.SqlQuery<FixedAssetSerialList>("Exec SP_GetLocationWiseEmployeeListToAssignUnassignOrView @Option, @prmStoreLocation, @LocationCode, @prmItemCode, @EmployeeID, @prmAvailableToAssignQuantity", storeParam);

            return resultList.ToList();

            //var serialList = (from itmstcwsrlnbloc in context.Inv_ItemStockWithSerialNoByLocation
            //                  join o in
            //                      (from empwfastaloc in context.Fix_EmployeeWiseFixedAssetsAllocation
            //                       join empwfastalocwsrl in context.Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo
            //                       on new { empwfastaloc.StoreLocation, empwfastaloc.LocationCode, empwfastaloc.ItemCode, empwfastaloc.EmployeeID, empwfastaloc.AllocationSeqNo }
            //                       equals new { empwfastalocwsrl.StoreLocation, empwfastalocwsrl.LocationCode, empwfastalocwsrl.ItemCode, empwfastalocwsrl.EmployeeID, empwfastalocwsrl.AllocationSeqNo }
            //                       into FixedAsetSerials
            //                       from fastsrl in FixedAsetSerials.DefaultIfEmpty()
            //                       where fastsrl.StoreLocation == storeLocation &&
            //                           fastsrl.LocationCode == locationCode &&
            //                           fastsrl.ItemCode == itemCode &&
            //                           fastsrl.IsItAllocated == true
            //                       select new
            //                       {
            //                           empwfastaloc.AllocationSeqNo,
            //                           empwfastaloc.StoreLocation,
            //                           empwfastaloc.LocationCode,
            //                           empwfastaloc.ItemCode,
            //                           empwfastaloc.EmployeeID,
            //                           fastsrl.ItemSerialNo,
            //                           empwfastaloc.AllocationDate,
            //                           empwfastaloc.Remarks
            //                       })
            //                 on new { itmstcwsrlnbloc.StoreLocation, itmstcwsrlnbloc.LocationCode, itmstcwsrlnbloc.ItemCode, itmstcwsrlnbloc.ItemSerialNo }
            //                 equals new { o.StoreLocation, o.LocationCode, o.ItemCode, o.ItemSerialNo }
            //                 into seriallist
            //                  from srl in seriallist.DefaultIfEmpty()
            //                  where itmstcwsrlnbloc.StoreLocation == storeLocation &&
            //                         itmstcwsrlnbloc.LocationCode == locationCode &&
            //                         itmstcwsrlnbloc.ItemCode == itemCode
            //                  select new FixedAssetSerialList
            //                  {
            //                      AllocationSeqNo = srl.AllocationSeqNo,
            //                      ItemSerialNo = itmstcwsrlnbloc.ItemSerialNo,
            //                      EmployeeID = srl.EmployeeID,
            //                      AllocationDate = srl.AllocationDate,
            //                      Remarks = srl.Remarks
            //                  });

            //return serialList.ToList();
        }

        public List<AssetAssign> FixedAssetAssignUnassign(string locationCode, string itemCode, byte storeLocation, string employeeId, byte status, string unAssignedQuantity)
        {

            SqlParameter[] storeParam = new SqlParameter[6];
            storeParam[0] = new SqlParameter("Option", "FORASSIGN");
            storeParam[1] = new SqlParameter("prmStoreLocation", storeLocation);
            storeParam[2] = new SqlParameter("LocationCode", locationCode);
            storeParam[3] = new SqlParameter("prmItemCode", itemCode);
            storeParam[4] = new SqlParameter("EmployeeID", employeeId);
            storeParam[5] = new SqlParameter("prmAvailableToAssignQuantity", unAssignedQuantity);

            var resultList = context.Database.SqlQuery<AssetAssign>("Exec SP_GetLocationWiseEmployeeListToAssignUnassignOrView @Option, @prmStoreLocation, @LocationCode, @prmItemCode, @EmployeeID, @prmAvailableToAssignQuantity", storeParam);

            return resultList.ToList();

            //if (string.IsNullOrEmpty(employeeId))
            //{
            //    var assignAssetToEmployee = from itmempsrl in
            //                                    (from itmstcbloc in context.Inv_ItemStockByLocation
            //                                     join empast in
            //                                         (from empwfixastaoc in context.Fix_EmployeeWiseFixedAssetsAllocation
            //                                          where empwfixastaoc.StoreLocation == storeLocation && empwfixastaoc.LocationCode == locationCode && empwfixastaoc.ItemCode == itemCode && empwfixastaoc.Status == Helper.Active
            //                                          group empwfixastaoc by new { empwfixastaoc.StoreLocation, empwfixastaoc.LocationCode, empwfixastaoc.ItemCode }
            //                                              into empwfixastaocGroup
            //                                              select new
            //                                              {
            //                                                  empwfixastaocGroup.Key.StoreLocation,
            //                                                  empwfixastaocGroup.Key.LocationCode,
            //                                                  empwfixastaocGroup.Key.ItemCode,
            //                                                  TotalAllocated = empwfixastaocGroup.Sum(s => s.AllocatedQuantity)
            //                                              })
            //                                          on new { itmstcbloc.StoreLocation, itmstcbloc.LocationCode, itmstcbloc.ItemCode } equals new { empast.StoreLocation, empast.LocationCode, empast.ItemCode }
            //                                          into empastLefJoin
            //                                     from empastavl in empastLefJoin.DefaultIfEmpty()
            //                                     where itmstcbloc.StoreLocation == storeLocation && itmstcbloc.LocationCode == locationCode && itmstcbloc.ItemCode == itemCode
            //                                     select new
            //                                     {
            //                                         itmstcbloc.StoreLocation,
            //                                         itmstcbloc.LocationCode,
            //                                         itmstcbloc.ItemCode,
            //                                         itmstcbloc.AvailableQuantity,
            //                                         UnallocatedQuantity = (itmstcbloc.AvailableQuantity - (empastavl.TotalAllocated != null ? empastavl.TotalAllocated : Helper.Active))
            //                                     })

            //                                join locwsrlaloc in
            //                                    (from locwemp in context.Hrm_LocationWiseEmployee
            //                                     join emlastsrl in
            //                                         (from allassignsrl in
            //                                              (from empwiseast in
            //                                                   (
            //                                                       from emlwsrltotal in
            //                                                           (from empwfixastaoc in context.Fix_EmployeeWiseFixedAssetsAllocation
            //                                                            where empwfixastaoc.StoreLocation == storeLocation && empwfixastaoc.LocationCode == locationCode && empwfixastaoc.ItemCode == itemCode && empwfixastaoc.Status == Helper.Active
            //                                                            group empwfixastaoc by new { empwfixastaoc.StoreLocation, empwfixastaoc.LocationCode, empwfixastaoc.ItemCode, empwfixastaoc.EmployeeID }
            //                                                                into empwfixastaocGroup
            //                                                                select new
            //                                                                {
            //                                                                    empwfixastaocGroup.Key.StoreLocation,
            //                                                                    empwfixastaocGroup.Key.LocationCode,
            //                                                                    empwfixastaocGroup.Key.ItemCode,
            //                                                                    empwfixastaocGroup.Key.EmployeeID,
            //                                                                    AlreadyAllocated = empwfixastaocGroup.Sum(s => s.AllocatedQuantity)
            //                                                                })
            //                                                       join emlweqnmax in
            //                                                           (from empwfixastaoc in context.Fix_EmployeeWiseFixedAssetsAllocation
            //                                                            where empwfixastaoc.StoreLocation == storeLocation && empwfixastaoc.LocationCode == locationCode && empwfixastaoc.ItemCode == itemCode && empwfixastaoc.AllocatedQuantity > Helper.Active && empwfixastaoc.Status == Helper.Active
            //                                                            group empwfixastaoc by new { empwfixastaoc.StoreLocation, empwfixastaoc.LocationCode, empwfixastaoc.ItemCode, empwfixastaoc.EmployeeID }
            //                                                                into empwfixastaocGroup
            //                                                                select new
            //                                                                {
            //                                                                    empwfixastaocGroup.Key.StoreLocation,
            //                                                                    empwfixastaocGroup.Key.LocationCode,
            //                                                                    empwfixastaocGroup.Key.ItemCode,
            //                                                                    empwfixastaocGroup.Key.EmployeeID,
            //                                                                    AllocationSeqNo = empwfixastaocGroup.Max(m => m.AllocationSeqNo)
            //                                                                })
            //                                                       on new { emlwsrltotal.StoreLocation, emlwsrltotal.LocationCode, emlwsrltotal.ItemCode, emlwsrltotal.EmployeeID, }
            //                                                       equals new { emlweqnmax.StoreLocation, emlweqnmax.LocationCode, emlweqnmax.ItemCode, emlweqnmax.EmployeeID }
            //                                                       select new
            //                                                       {
            //                                                           emlwsrltotal.StoreLocation,
            //                                                           emlwsrltotal.LocationCode,
            //                                                           emlwsrltotal.ItemCode,
            //                                                           emlwsrltotal.EmployeeID,
            //                                                           emlweqnmax.AllocationSeqNo,
            //                                                           emlwsrltotal.AlreadyAllocated
            //                                                       }
            //                                                    )
            //                                               join empwfixastaoc in context.Fix_EmployeeWiseFixedAssetsAllocation
            //                                               on new { empwiseast.StoreLocation, empwiseast.LocationCode, empwiseast.ItemCode, empwiseast.EmployeeID, empwiseast.AllocationSeqNo }
            //                                               equals new { empwfixastaoc.StoreLocation, empwfixastaoc.LocationCode, empwfixastaoc.ItemCode, empwfixastaoc.EmployeeID, empwfixastaoc.AllocationSeqNo }
            //                                               where empwfixastaoc.StoreLocation == storeLocation && empwfixastaoc.LocationCode == locationCode && empwfixastaoc.ItemCode == itemCode && empwfixastaoc.Status == Helper.Active && empwiseast.AlreadyAllocated > Helper.Active
            //                                               select new
            //                                               {
            //                                                   empwiseast.StoreLocation,
            //                                                   empwiseast.LocationCode,
            //                                                   empwiseast.ItemCode,
            //                                                   empwiseast.EmployeeID,
            //                                                   empwiseast.AllocationSeqNo,
            //                                                   empwiseast.AlreadyAllocated,
            //                                                   empwfixastaoc.AllocationDate,
            //                                                   empwfixastaoc.Remarks
            //                                               })
            //                                          join emlwfixastsrl in context.Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo
            //                                          on new { allassignsrl.StoreLocation, allassignsrl.LocationCode, allassignsrl.ItemCode, allassignsrl.EmployeeID, allassignsrl.AllocationSeqNo }
            //                                          equals new { emlwfixastsrl.StoreLocation, emlwfixastsrl.LocationCode, emlwfixastsrl.ItemCode, emlwfixastsrl.EmployeeID, emlwfixastsrl.AllocationSeqNo }
            //                                          into emlwfixastsrlLeftJoin
            //                                          from emlwfixastsrls in emlwfixastsrlLeftJoin.DefaultIfEmpty()
            //                                          where
            //                                          emlwfixastsrls.StoreLocation == storeLocation && emlwfixastsrls.LocationCode == locationCode && emlwfixastsrls.ItemCode == itemCode && emlwfixastsrls.IsItAllocated == true && emlwfixastsrls.Status == Helper.Active
            //                                          select new
            //                                          {
            //                                              allassignsrl.StoreLocation,
            //                                              allassignsrl.LocationCode,
            //                                              allassignsrl.ItemCode,
            //                                              allassignsrl.EmployeeID,
            //                                              allassignsrl.AllocationSeqNo,
            //                                              allassignsrl.AlreadyAllocated,
            //                                              allassignsrl.AllocationDate,
            //                                              allassignsrl.Remarks
            //                                          })
            //                                         on locwemp.EmployeeID equals emlastsrl.EmployeeID
            //                                         into emlastsrlLeftJoin
            //                                     from totalempwithsrl in emlastsrlLeftJoin.DefaultIfEmpty()
            //                                     where locwemp.LocationCode == locationCode && locwemp.Status == Helper.Active
            //                                     select new
            //                                     {
            //                                         totalempwithsrl.StoreLocation,
            //                                         locwemp.LocationCode,
            //                                         totalempwithsrl.ItemCode,
            //                                         EmployeeID = locwemp.EmployeeID,
            //                                         totalempwithsrl.AlreadyAllocated,
            //                                         totalempwithsrl.AllocationDate,
            //                                         totalempwithsrl.Remarks
            //                                     })

            //                                    on itmempsrl.LocationCode equals locwsrlaloc.LocationCode
            //                                    into locwsrlalocLeftJoin
            //                                from locwemplsrlaloc in locwsrlalocLeftJoin.DefaultIfEmpty()
            //                                select new AssetAssign
            //                                {
            //                                    StoreLocation = itmempsrl.StoreLocation,
            //                                    LocationCode = itmempsrl.LocationCode,
            //                                    ItemCode = itmempsrl.ItemCode,
            //                                    EmployeeID = locwemplsrlaloc.EmployeeID,
            //                                    UnallocatedQuantity = itmempsrl.UnallocatedQuantity,
            //                                    AlreadyAllocated = locwemplsrlaloc.AlreadyAllocated,
            //                                    AllocationDate = locwemplsrlaloc.AllocationDate,
            //                                    Remarks = locwemplsrlaloc.Remarks
            //                                };

            //    return assignAssetToEmployee.ToList();
            //}
            //else
            //{
            //    var assignAssetToEmployee = from itmempsrl in
            //                                    (from itmstcbloc in context.Inv_ItemStockByLocation
            //                                     join empast in
            //                                         (from empwfixastaoc in context.Fix_EmployeeWiseFixedAssetsAllocation
            //                                          where empwfixastaoc.StoreLocation == storeLocation && empwfixastaoc.LocationCode == locationCode && empwfixastaoc.ItemCode == itemCode && empwfixastaoc.Status == Helper.Active
            //                                          group empwfixastaoc by new { empwfixastaoc.StoreLocation, empwfixastaoc.LocationCode, empwfixastaoc.ItemCode }
            //                                              into empwfixastaocGroup
            //                                              select new
            //                                              {
            //                                                  empwfixastaocGroup.Key.StoreLocation,
            //                                                  empwfixastaocGroup.Key.LocationCode,
            //                                                  empwfixastaocGroup.Key.ItemCode,
            //                                                  TotalAllocated = empwfixastaocGroup.Sum(s => s.AllocatedQuantity)
            //                                              })
            //                                          on new { itmstcbloc.StoreLocation, itmstcbloc.LocationCode, itmstcbloc.ItemCode } equals new { empast.StoreLocation, empast.LocationCode, empast.ItemCode }
            //                                          into empastLefJoin
            //                                     from empastavl in empastLefJoin.DefaultIfEmpty()
            //                                     where itmstcbloc.StoreLocation == storeLocation && itmstcbloc.LocationCode == locationCode && itmstcbloc.ItemCode == itemCode
            //                                     select new
            //                                     {
            //                                         itmstcbloc.StoreLocation,
            //                                         itmstcbloc.LocationCode,
            //                                         itmstcbloc.ItemCode,
            //                                         itmstcbloc.AvailableQuantity,
            //                                         UnallocatedQuantity = (itmstcbloc.AvailableQuantity - (empastavl.TotalAllocated != null ? empastavl.TotalAllocated : Helper.Active))
            //                                     })

            //                                join locwsrlaloc in
            //                                    (from locwemp in context.Hrm_LocationWiseEmployee
            //                                     join emlastsrl in
            //                                         (from allassignsrl in
            //                                              (from empwiseast in
            //                                                   (
            //                                                       from emlwsrltotal in
            //                                                           (from empwfixastaoc in context.Fix_EmployeeWiseFixedAssetsAllocation
            //                                                            where empwfixastaoc.StoreLocation == storeLocation && empwfixastaoc.LocationCode == locationCode && empwfixastaoc.ItemCode == itemCode && empwfixastaoc.EmployeeID == employeeId && empwfixastaoc.Status == Helper.Active
            //                                                            group empwfixastaoc by new { empwfixastaoc.StoreLocation, empwfixastaoc.LocationCode, empwfixastaoc.EmployeeID, empwfixastaoc.ItemCode }
            //                                                                into empwfixastaocGroup
            //                                                                select new
            //                                                                {
            //                                                                    empwfixastaocGroup.Key.StoreLocation,
            //                                                                    empwfixastaocGroup.Key.LocationCode,
            //                                                                    empwfixastaocGroup.Key.ItemCode,
            //                                                                    empwfixastaocGroup.Key.EmployeeID,
            //                                                                    AlreadyAllocated = empwfixastaocGroup.Sum(s => s.AllocatedQuantity)
            //                                                                })
            //                                                       join emlweqnmax in
            //                                                           (from empwfixastaoc in context.Fix_EmployeeWiseFixedAssetsAllocation
            //                                                            where empwfixastaoc.StoreLocation == storeLocation && empwfixastaoc.LocationCode == locationCode && empwfixastaoc.ItemCode == itemCode && empwfixastaoc.EmployeeID == employeeId && empwfixastaoc.AllocatedQuantity > Helper.Active && empwfixastaoc.Status == Helper.Active
            //                                                            group empwfixastaoc by new { empwfixastaoc.StoreLocation, empwfixastaoc.LocationCode, empwfixastaoc.EmployeeID, empwfixastaoc.ItemCode }
            //                                                                into empwfixastaocGroup
            //                                                                select new
            //                                                                {
            //                                                                    empwfixastaocGroup.Key.StoreLocation,
            //                                                                    empwfixastaocGroup.Key.LocationCode,
            //                                                                    empwfixastaocGroup.Key.ItemCode,
            //                                                                    empwfixastaocGroup.Key.EmployeeID,
            //                                                                    AllocationSeqNo = empwfixastaocGroup.Max(m => m.AllocationSeqNo)
            //                                                                })
            //                                                       on new { emlwsrltotal.StoreLocation, emlwsrltotal.LocationCode, emlwsrltotal.ItemCode, emlwsrltotal.EmployeeID, }
            //                                                       equals new { emlweqnmax.StoreLocation, emlweqnmax.LocationCode, emlweqnmax.ItemCode, emlweqnmax.EmployeeID }
            //                                                       select new
            //                                                       {
            //                                                           emlwsrltotal.StoreLocation,
            //                                                           emlwsrltotal.LocationCode,
            //                                                           emlwsrltotal.ItemCode,
            //                                                           emlwsrltotal.EmployeeID,
            //                                                           emlweqnmax.AllocationSeqNo,
            //                                                           emlwsrltotal.AlreadyAllocated
            //                                                       }
            //                                                    )

            //                                               join empwfixastaoc in context.Fix_EmployeeWiseFixedAssetsAllocation
            //                                               on new { empwiseast.StoreLocation, empwiseast.LocationCode, empwiseast.ItemCode, empwiseast.EmployeeID, empwiseast.AllocationSeqNo }
            //                                               equals new { empwfixastaoc.StoreLocation, empwfixastaoc.LocationCode, empwfixastaoc.ItemCode, empwfixastaoc.EmployeeID, empwfixastaoc.AllocationSeqNo }
            //                                               where empwfixastaoc.StoreLocation == storeLocation && empwfixastaoc.LocationCode == locationCode && empwfixastaoc.ItemCode == itemCode && empwfixastaoc.EmployeeID == employeeId && empwfixastaoc.Status == Helper.Active && empwiseast.AlreadyAllocated > Helper.Active
            //                                               select new
            //                                               {
            //                                                   empwiseast.StoreLocation,
            //                                                   empwiseast.LocationCode,
            //                                                   empwiseast.ItemCode,
            //                                                   empwiseast.EmployeeID,
            //                                                   empwiseast.AllocationSeqNo,
            //                                                   empwiseast.AlreadyAllocated,
            //                                                   empwfixastaoc.AllocationDate,
            //                                                   empwfixastaoc.Remarks
            //                                               })
            //                                          join emlwfixastsrl in context.Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo
            //                                          on new { allassignsrl.StoreLocation, allassignsrl.LocationCode, allassignsrl.ItemCode, allassignsrl.EmployeeID, allassignsrl.AllocationSeqNo }
            //                                          equals new { emlwfixastsrl.StoreLocation, emlwfixastsrl.LocationCode, emlwfixastsrl.ItemCode, emlwfixastsrl.EmployeeID, emlwfixastsrl.AllocationSeqNo }
            //                                          into emlwfixastsrlLeftJoin
            //                                          from emlwfixastsrls in emlwfixastsrlLeftJoin.DefaultIfEmpty()
            //                                          where
            //                                          emlwfixastsrls.StoreLocation == storeLocation && emlwfixastsrls.LocationCode == locationCode && emlwfixastsrls.ItemCode == itemCode && emlwfixastsrls.EmployeeID == employeeId && emlwfixastsrls.IsItAllocated == true && emlwfixastsrls.Status == Helper.Active
            //                                          select new
            //                                          {
            //                                              allassignsrl.StoreLocation,
            //                                              allassignsrl.LocationCode,
            //                                              allassignsrl.ItemCode,
            //                                              allassignsrl.EmployeeID,
            //                                              allassignsrl.AllocationSeqNo,
            //                                              allassignsrl.AlreadyAllocated,
            //                                              allassignsrl.AllocationDate,
            //                                              allassignsrl.Remarks
            //                                          })
            //                                         on locwemp.EmployeeID equals emlastsrl.EmployeeID
            //                                         into emlastsrlLeftJoin
            //                                     from totalempwithsrl in emlastsrlLeftJoin.DefaultIfEmpty()
            //                                     where locwemp.LocationCode == locationCode &&
            //                                     locwemp.EmployeeID == employeeId &&
            //                                     locwemp.Status == Helper.Active
            //                                     select new
            //                                     {
            //                                         totalempwithsrl.StoreLocation,
            //                                         locwemp.LocationCode,
            //                                         totalempwithsrl.ItemCode,
            //                                         EmployeeID = locwemp.EmployeeID,
            //                                         totalempwithsrl.AlreadyAllocated,
            //                                         totalempwithsrl.AllocationDate,
            //                                         totalempwithsrl.Remarks
            //                                     })

            //                                    on itmempsrl.LocationCode equals locwsrlaloc.LocationCode
            //                                    into locwsrlalocLeftJoin
            //                                from locwemplsrlaloc in locwsrlalocLeftJoin.DefaultIfEmpty()
            //                                select new AssetAssign
            //                                {
            //                                    StoreLocation = itmempsrl.StoreLocation,
            //                                    LocationCode = itmempsrl.LocationCode,
            //                                    ItemCode = itmempsrl.ItemCode,
            //                                    EmployeeID = locwemplsrlaloc.EmployeeID,
            //                                    UnallocatedQuantity = itmempsrl.UnallocatedQuantity,
            //                                    AlreadyAllocated = locwemplsrlaloc.AlreadyAllocated,
            //                                    AllocationDate = locwemplsrlaloc.AllocationDate,
            //                                    Remarks = locwemplsrlaloc.Remarks
            //                                };


            //    return assignAssetToEmployee.ToList();
            //}
        }

        public string EmployeeWiseFixedAssetsAllocationSequenceNumberMax(string locationCode, string employeeId, string itemCode, byte storeLocation)
        {
            string fixedAssetSeqQuery = string.Format("SELECT TOP(1) ISNULL(emlwfixastaloc.AllocationSeqNo,'')FixedAssetSeqNo " +
                                 "FROM Fix_EmployeeWiseFixedAssetsAllocation emlwfixastaloc " +
                                 "WHERE emlwfixastaloc.EmployeeID = '{0}' AND " +
                                 "emlwfixastaloc.StoreLocation = {1} AND " +
                                 "emlwfixastaloc.LocationCode = '{2}' AND " +
                                 "emlwfixastaloc.ItemCode = '{3}' " +
                                 "ORDER BY  CONVERT(INT,SUBSTRING(emlwfixastaloc.AllocationSeqNo,7, LEN(emlwfixastaloc.AllocationSeqNo))) DESC", employeeId, storeLocation, locationCode, itemCode);

            var seqNumber = context.Database.SqlQuery<string>(fixedAssetSeqQuery).FirstOrDefault();

            string fixedAssetSeqNumber = string.Empty;

            if (seqNumber != null)
            {
                fixedAssetSeqNumber = seqNumber.ToString();
            }

            return fixedAssetSeqNumber;
        }

        public ArrayList GetFixedAssetAssignedOrUnassignedSeialList(byte storeLocation, string locationCode, string itemCode, string employeeId, int assignQuantity)
        {
            List<Inv_ItemStockWithSerialNoByLocation> lstItemStockWithSerial = new List<Inv_ItemStockWithSerialNoByLocation>();
            List<Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo> lstAssignSerials = new List<Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo>();

            lstItemStockWithSerial = this.ReadItemStockWithSerialNoByLocation(storeLocation, locationCode, itemCode);

            ArrayList serialList = new ArrayList();

            if (assignQuantity > 0)
            {
                lstAssignSerials = context.Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo.Where(f => f.LocationCode == locationCode && f.ItemCode == itemCode && f.IsItAllocated == true).ToList();

                foreach (Inv_ItemStockWithSerialNoByLocation itemserial in lstItemStockWithSerial)
                {
                    //var v = (from ss in lstAssignSerials where ss.ItemSerialNo == itemserial.ItemSerialNo select ss.ItemSerialNo).FirstOrDefault();

                    //if (v == null)
                    //{
                        serialList.Add(new { Display = itemserial.ItemSerialNo, Value = itemserial.ItemSerialNo });
                    //}
                }
            }
            else
            {
                lstAssignSerials = context.Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo.Where(f => f.LocationCode == locationCode && f.EmployeeID == employeeId && f.ItemCode == itemCode && f.IsItAllocated == true).ToList();

                foreach (Inv_ItemStockWithSerialNoByLocation itemserial in lstItemStockWithSerial)
                {
                    var v = (from ss in lstAssignSerials where ss.ItemSerialNo == itemserial.ItemSerialNo select ss.ItemSerialNo).FirstOrDefault();

                    if (v != null)
                    {
                        serialList.Add(new { Display = itemserial.ItemSerialNo, Value = itemserial.ItemSerialNo });
                    }
                }
            }


            return serialList;
        }

        public List<AvailableNAssignFixedAsset> ReadAvailableNAssignFixedAsset(byte storeLocation, string locationCode, string itemCategory, string employeeSelectionOption, string employeeID)
        {
            SqlParameter[] storeParam = new SqlParameter[5];
            storeParam[0] = new SqlParameter("prmStoreLocation", storeLocation);
            storeParam[1] = new SqlParameter("prmLocationCode", locationCode);
            storeParam[2] = new SqlParameter("prmItemCategory", itemCategory);
            storeParam[3] = new SqlParameter("prmEmployeeSelectionOption", employeeSelectionOption);
            storeParam[4] = new SqlParameter("prmEmployeeID", employeeID);

            var resultList = context.Database.SqlQuery<AvailableNAssignFixedAsset>("Exec SP_InvGetAvailableItemInALocationWithAllocationToEmployee @prmStoreLocation, @prmLocationCode, @prmItemCategory, @prmEmployeeSelectionOption, @prmEmployeeID", storeParam);

            return resultList.ToList();
        }

        public List<ChallanOrMRRItemsSerial> ReadChallanOrMRRItemsSerial(string locationCode, string challanSequenceNumber, string mrrSequenceNumber, string itemCode)
        {
            if (!string.IsNullOrEmpty(challanSequenceNumber.Trim()))
            {
                var v = from c in context.Inv_ChallanDetailsWithSerialNo
                        join i in context.Inv_ItemMaster
                        on c.ItemCode equals i.ItemCode
                        where c.LocationCode == locationCode && c.ChallanSeqNo == challanSequenceNumber && c.ItemCode == itemCode
                        select new ChallanOrMRRItemsSerial
                        {
                            ItemCode = c.ItemCode,
                            ItemName = i.ItemName + " [" + c.ItemCode + "]",
                            ItemSerialNumber = c.ItemSerialNo
                        };

                return v.ToList();
            }
            else
            {
                var v = from c in context.Inv_MRRDetailsWithSerialNo
                        join i in context.Inv_ItemMaster
                        on c.ItemCode equals i.ItemCode
                        where c.LocationCode == locationCode && c.MRRSeqNo == mrrSequenceNumber && c.ItemCode == itemCode
                        select new ChallanOrMRRItemsSerial
                        {
                            ItemCode = c.ItemCode,
                            ItemName = i.ItemName + " [" + c.ItemCode + "]",
                            ItemSerialNumber = c.ItemSerialNo
                        };

                return v.ToList();
            }
        }

        public ArrayList ReadVendorChallanWithItemReferenceToCheck(string deliveryNoteNo)
        {
            List<Inv_VendorChallanWithItemReferenceToCheck> vendorLocationTotalList = context.Inv_VendorChallanWithItemReferenceToCheck.Where(s => s.VendorRefChallanNo == deliveryNoteNo).ToList();
            List<Inv_VendorChallanWithItemReferenceToCheck> vendorLocationTotalDistinctList = vendorLocationTotalList.GroupBy(g => g.LocationCode).Select(s => s.First()).ToList();

            var v = from vcwirtc in vendorLocationTotalDistinctList
                    join linf in context.Common_LocationInfo
                    on vcwirtc.LocationCode equals linf.LocationCode
                    select new
                    {
                        vcwirtc.LocationCode,
                        LocationName = linf.LocationName + " [" + vcwirtc.LocationCode + "]",
                        VendorRefChallanDate = vcwirtc.VendorRefChallanDate.ToString("dd-MMM-yyyy")
                    };

            ArrayList vendorChallanLocation = new ArrayList(v.ToList());

            return vendorChallanLocation;
        }

        public Array MrrSequenceListForMaterialReceiving(string rreDeliveryNote, string locationCode)
        {
            var v = from mrrm in context.Inv_MRRMaster
                    where mrrm.LocationCode == locationCode && mrrm.RefExternalChallanNo == rreDeliveryNote
                    select new
                    {
                        mrrm.MRRSeqNo
                    };

            return v.ToArray();
        }

        public List<MaterialRecevingReportMrrDetails> MaterialReceivingMrrDetails(string LocationCode, string VendorRefChallanNo, string mrrSequenceNumber, string billNo)
        {
            SqlParameter[] storeParam = new SqlParameter[3];
            storeParam[0] = new SqlParameter("prmVendorBillNo", billNo);
            storeParam[1] = new SqlParameter("prmVendorRefChallanNo", VendorRefChallanNo);
            storeParam[2] = new SqlParameter("prmLocationCode", LocationCode);

            var resultList = context.Database.SqlQuery<MaterialRecevingReportMrrDetails>
                ("Exec REP_InvGetVendorChallanVsMRRForVerification @prmVendorBillNo, @prmVendorRefChallanNo, @prmLocationCode", storeParam);

            return resultList.ToList();
        }

        public List<DeliveryNoteSummary> ReadDeliveryNoteSummary(string delevaryScheduleNo)
        {
           

            //var resultList = context.Database.SqlQuery<DeliveryNoteSummary>
            //    ("SELECT DISTINCT rwl.DistribScheduleNo,m.ScheduleDate [DeliveryChallanDate],rwl.RouteNo,rm.RouteName "
            //        +" ,rwl.VendorChallanNoForPackage,rwl.VendorChallanNoForSpareParts FROM SHSDistributionPlan_RootWiseLocation rwl "+
            //        " INNER JOIN SHSDistributionPlan_Master m ON m.DistribScheduleNo = rwl.DistribScheduleNo INNER JOIN Inv_RouteMaster rm ON rm.RouteNo = rwl.RouteNo "
            //        + " INNER JOIN Common_LocationInfo li ON li.LocationCode = rwl.LocationCode WHERE rwl.DistribScheduleNo = '"+delevaryScheduleNo+"'");

            //return resultList.ToList();

            var resultList = "SELECT DISTINCT rwl.DistribScheduleNo,m.ScheduleDate [DeliveryChallanDate],rwl.RouteNo,rm.RouteName "
                             +" ,rwl.VendorChallanNoForPackage,rwl.VendorChallanNoForSpareParts FROM SHSDistributionPlan_RootWiseLocation rwl "+
                  " INNER JOIN SHSDistributionPlan_Master m ON m.DistribScheduleNo = rwl.DistribScheduleNo INNER JOIN Inv_RouteMaster rm ON rm.RouteNo = rwl.RouteNo "
                   + " INNER JOIN Common_LocationInfo li ON li.LocationCode = rwl.LocationCode WHERE rwl.DistribScheduleNo = '" + delevaryScheduleNo + "'";
            
            return context.Database.SqlQuery<DeliveryNoteSummary>(resultList).ToList();
        }



        public List<DeliveryItemNoteReportSummary> ReadDeliveryItemNoteReportSummary(string delevaryScheduleNo)
        {
           

            var resultList = " SELECT '' [SLNo],vc.ItemCode,im.ItemName [SHSFullaccessoriesKit],'' [Rate],SUM(VendorDeliveryQuantity) [QtyPcs],'' [Amount] "
                              +" FROM Inv_VendorChallanWithItemReferenceToCheck vc INNER JOIN Inv_ItemMaster im ON im.ItemCode = vc.ItemCode "
                              +" LEFT OUTER JOIN Inv_TempUnitCostForReceivingItemFromRRE rre on rre.ItemCode = vc.ItemCode AND rre.StoreLocation = 1 AND "
                              + " rre.YearMonth = '201408' WHERE vc.VendorRefChallanDate = '" + delevaryScheduleNo + "'  GROUP BY vc.ItemCode,im.ItemName,rre.UnitCost "
                              +" ORDER BY vc.ItemCode ";

            return context.Database.SqlQuery<DeliveryItemNoteReportSummary>(resultList).ToList();
        }

        public List<DeliveryPackageNoteReportSummary> ReadDeliveryPackageNoteReportSummary(string delevaryScheduleNo)
        {


            var resultList = " SELECT '' [SLNo],rwl.PackageCode [PackageCode],pm.[Description] [PackageName],pm.Capacity,'' [Rate],SUM(PackageQuantity) [QtyPcs],'' [Amount] "
                              +" FROM SHSDistributionPlan_RootWiseLocationNPackage rwl INNER JOIN Sal_PackageMaster pm ON pm.PackageCode = rwl.PackageCode "
                              +" WHERE rwl.DistribScheduleNo = '" + delevaryScheduleNo + "'  GROUP BY rwl.PackageCode,pm.[Description],pm.Capacity ORDER BY pm.Capacity,rwl.PackageCode";

            return context.Database.SqlQuery<DeliveryPackageNoteReportSummary>(resultList).ToList();
        }

        //start monthly

        public List<DeliveryNoteSummary> ReadMonthlyDeliveryNoteSummary(string monthYear)
        {


            var resultList = " SELECT DISTINCT rwl.DistribScheduleNo,m.ScheduleDate [DeliveryChallanDate],rwl.RouteNo,rm.RouteName "
                               +" ,rwl.VendorChallanNoForPackage,rwl.VendorChallanNoForSpareParts "
                               +"  FROM SHSDistributionPlan_RootWiseLocation rwl "
                               +"        INNER JOIN SHSDistributionPlan_Master m ON m.DistribScheduleNo = rwl.DistribScheduleNo "
                               +"       INNER JOIN Inv_RouteMaster rm ON rm.RouteNo = rwl.RouteNo "
                               +"        INNER JOIN Common_LocationInfo li ON li.LocationCode = rwl.LocationCode "
                               +"   WHERE '20'+LEFT(rwl.DistribScheduleNo,4) = '" + monthYear + "'";

            return context.Database.SqlQuery<DeliveryNoteSummary>(resultList).ToList();
        }



        public List<DeliveryItemNoteReportSummary> ReadMonthlyDeliveryItemNoteReportSummary(string monthYear)
        {


            var resultList = "SELECT '' [SLNo],vc.ItemCode,im.ItemName [SHSFullaccessoriesKit],'' [Rate],im.ItemCapacity,SUM(VendorDeliveryQuantity) [QtyPcs],'' [Amount] "
                            +"  FROM Inv_VendorChallanWithItemReferenceToCheck vc "
                            +"  INNER JOIN Inv_ItemMaster im ON im.ItemCode = vc.ItemCode "
                            +"  WHERE LEFT(CONVERT(VARCHAR,vc.VendorRefChallanDate,112),6) ='" + monthYear +
                            "' GROUP BY vc.ItemCode,im.ItemName,im.ItemCapacity ORDER BY vc.ItemCode,im.ItemCapacity";

            return context.Database.SqlQuery<DeliveryItemNoteReportSummary>(resultList).ToList();
        }

        public List<DeliveryPackageNoteReportSummary> ReadMonthlyDeliveryPackageNoteReportSummary(string monthYear)
        {


            var resultList = "SELECT '' [SLNo],rwl.PackageCode [PackageCode],pm.[Description] [PackageName],pm.Capacity,'' [Rate],SUM(PackageQuantity) [QtyPcs],'' [Amount] "
                             +" FROM SHSDistributionPlan_RootWiseLocationNPackage rwl "
                             +" INNER JOIN Sal_PackageMaster pm ON pm.PackageCode = rwl.PackageCode "
                             +"  WHERE '20'+LEFT(rwl.DistribScheduleNo,4) = '" + monthYear + "' GROUP BY rwl.PackageCode,pm.[Description],pm.Capacity ORDER BY pm.Capacity,rwl.PackageCode";

            return context.Database.SqlQuery<DeliveryPackageNoteReportSummary>(resultList).ToList();
        }

        //end monthly



        //start date wise

        public List<DeliveryNoteSummary> ReadDateWiseDeliveryNoteSummary(string fromDate, string toDate)
        {


            var resultList = " SELECT DISTINCT rwl.DistribScheduleNo,m.ScheduleDate [DeliveryChallanDate],rwl.RouteNo,rm.RouteName "
                               +" ,rwl.VendorChallanNoForPackage,rwl.VendorChallanNoForSpareParts "
                               +"     FROM SHSDistributionPlan_RootWiseLocation rwl "
                               +"           INNER JOIN SHSDistributionPlan_Master m ON m.DistribScheduleNo = rwl.DistribScheduleNo "
                               +"            INNER JOIN Inv_RouteMaster rm ON rm.RouteNo = rwl.RouteNo "
                               +"           INNER JOIN Common_LocationInfo li ON li.LocationCode = rwl.LocationCode "
                               +"      WHERE m.ScheduleDate BETWEEN '"+fromDate+"' AND '" + toDate+ "'";  
                                

            return context.Database.SqlQuery<DeliveryNoteSummary>(resultList).ToList();
        }



        public List<DeliveryItemNoteReportSummary> ReadDateWiseDeliveryItemNoteReportSummary(string fromDate, string toDate)
        {

            var resultList = "SELECT '' [SLNo],vc.ItemCode,im.ItemName [SHSFullaccessoriesKit],'' [Rate],im.ItemCapacity,SUM(VendorDeliveryQuantity) [QtyPcs],'' [Amount] "
                             +" FROM Inv_VendorChallanWithItemReferenceToCheck vc "
                             +" INNER JOIN SHSDistributionPlan_Master m ON m.DistribScheduleNo = vc.RefDistribScheduleNo "
                             +" INNER JOIN Inv_ItemMaster im ON im.ItemCode = vc.ItemCode   "
                             +" WHERE m.ScheduleDate BETWEEN '"+fromDate+"' AND '"+toDate+"' "
                             +" GROUP BY vc.ItemCode,im.ItemName,im.ItemCapacity "
                             +" ORDER BY vc.ItemCode,im.ItemCapacity ";

            return context.Database.SqlQuery<DeliveryItemNoteReportSummary>(resultList).ToList();
        }

        public List<DeliveryPackageNoteReportSummary> ReadDateWiseDeliveryPackageNoteReportSummary(string fromDate, string toDate)
        {


            var resultList = "SELECT '' [SLNo],rwl.PackageCode [PackageCode],pm.[Description] [PackageName],pm.Capacity,'' [Rate],SUM(PackageQuantity) [QtyPcs],'' [Amount] "
                             +" FROM SHSDistributionPlan_RootWiseLocationNPackage rwl "
                             +" INNER JOIN SHSDistributionPlan_Master m ON m.DistribScheduleNo = rwl.DistribScheduleNo "
                             +" INNER JOIN Sal_PackageMaster pm ON pm.PackageCode = rwl.PackageCode "
                             +"  WHERE m.ScheduleDate BETWEEN '"+fromDate+"' AND '"+toDate+"' "
                             +" GROUP BY rwl.PackageCode,pm.[Description],pm.Capacity "
                             +" ORDER BY pm.Capacity,rwl.PackageCode ";


            return context.Database.SqlQuery<DeliveryPackageNoteReportSummary>(resultList).ToList();
        }

        //end date wise



        public List<CustomerDisasterRecoveryDetails> ReadCustomerDisasterRecoveryList(string collectionType, byte customerStatus, DateTime collectionDate, string locationCode)
        {

            if (collectionType == "DRF")
            {
                var v = from cs in context.Sal_Customer
                        join sag in context.Sal_SalesAgreement
                        on cs.CustomerCode equals sag.CustomerCode
                        join css in context.Sal_CustomerStatus
                        on cs.CustomerCode equals css.CustomerCode
                        where
                        !(
                           from cus in context.Sal_Customer
                           join
                           cfc in context.Sal_CollectionFromCustomers
                           on cus.CustomerCode equals cfc.CustomerCode
                           where cus.UnitCode == locationCode &&
                                 cus.Status == customerStatus &&
                                 cfc.CollectionType == collectionType
                           select cus.CustomerCode
                        ).Contains(cs.CustomerCode) &&
                        cs.UnitCode == locationCode &&
                        cs.Status == customerStatus &&
                        SqlFunctions.DateDiff("Month", sag.AgreementDate, collectionDate) <= 36
                        select new CustomerDisasterRecoveryDetails
                        {
                            CustomerCode = cs.CustomerCode,
                            CustomerName = cs.CustomerName,
                            AgreementDate = sag.AgreementDate,
                            AgreementDuration = SqlFunctions.DateDiff("Month", sag.AgreementDate, collectionDate),
                            CustomerFPREmployeeCode = css.EmployeeAsFPR
                        };
                return v.ToList();
            }
            else
            {
                var v = from cs in context.Sal_Customer
                        join sag in context.Sal_SalesAgreement
                        on cs.CustomerCode equals sag.CustomerCode
                        join css in context.Sal_CustomerStatus
                        on cs.CustomerCode equals css.CustomerCode
                        where
                        !(
                           from cus in context.Sal_Customer
                           join
                           cfc in context.Sal_CollectionFromCustomers
                           on cus.CustomerCode equals cfc.CustomerCode
                           where cus.UnitCode == locationCode &&
                                 cus.Status == customerStatus &&
                                 cfc.CollectionType == collectionType &&
                                 SqlFunctions.DateDiff("Month", cfc.CollectionDate, collectionDate) >= 12
                           select cus.CustomerCode
                        ).Contains(cs.CustomerCode) &&
                        cs.UnitCode == locationCode &&
                        cs.Status == customerStatus &&
                        SqlFunctions.DateDiff("Month", sag.AgreementDate, collectionDate) >= 36
                        select new CustomerDisasterRecoveryDetails
                        {
                            CustomerCode = cs.CustomerCode,
                            CustomerName = cs.CustomerName,
                            AgreementDate = sag.AgreementDate,
                            AgreementDuration = SqlFunctions.DateDiff("Month", sag.AgreementDate, collectionDate),
                            CustomerFPREmployeeCode = css.EmployeeAsFPR
                        };

                return v.ToList();

            }
        }

        public decimal ReadCustomerDetailsForDRFACollection(string collectionType, string customerCode, string locationCode, DateTime collectionDate, byte customerStatus)
        {
            SqlParameter[] storeParam = new SqlParameter[5];
            storeParam[0] = new SqlParameter("CollectionType", collectionType);
            storeParam[1] = new SqlParameter("CustomerCode", customerCode);
            storeParam[2] = new SqlParameter("LocationCode", locationCode);
            storeParam[3] = new SqlParameter("CollectionDate", collectionDate);
            storeParam[4] = new SqlParameter("CustomerStatus", customerStatus);

            var resultList = context.Database.SqlQuery<decimal>("Exec SP_SalGetCustomerDetailsForDRFACollection @CollectionType, @CustomerCode, @LocationCode, @CollectionDate, @CustomerStatus", storeParam);

            return resultList.FirstOrDefault();
        }

        public ArrayList ReadMRRNDeliveryNoteValue(string locationCode, string mrrSequenceNumber, string yearMonth, string rreDeliveryNote)
        {
            string whereCondition = string.Empty;

            if (locationCode != "ALL")
            {
                whereCondition = "mrm.MRRSeqNo = '" + mrrSequenceNumber + "' AND " +
                                 "mrm.LocationCode = '" + locationCode + "'  AND ";
            }

            if (rreDeliveryNote != "ALL")
            {
                whereCondition += "mrm.RefExternalChallanNo = '" + rreDeliveryNote + "' AND ";
            }

            string mrrvQuery = "SELECT SUM(mrrv.TotalCost) MRRValue " +
                                "FROM " +
                                "(SELECT mrm.LocationCode, mrm.MRRSeqNo,mrm.RefExternalChallanNo, " +
                                " mrrd.ItemCode, mrrd.ReceiveQuantity,ucfrcn.UnitCost, " +
                                "(ISNULL(mrrd.ReceiveQuantity,0) * ISNULL(ucfrcn.UnitCost,0)) TotalCost " +
                                "FROM Inv_MRRMaster mrm " +
                                "INNER JOIN Inv_MRRDetails mrrd " +
                                "ON mrm.LocationCode = mrrd.LocationCode AND " +
                                "mrm.MRRSeqNo = mrrd.MRRSeqNo " +
                                "INNER JOIN  Inv_TempUnitCostForReceivingItemFromRRE ucfrcn " +
                                "ON mrrd.ItemCode = ucfrcn.ItemCode " +
                                "WHERE " + whereCondition +
                // "mrm.RefExternalChallanNo = '" + rreDeliveryNote + "' AND " +
                                "ucfrcn.YearMonth = '" + yearMonth + "')mrrv ";
            //+ "GROUP BY mrrv.RefExternalChallanNo"

            whereCondition = string.Empty;
            if (locationCode != "ALL")
            {
                whereCondition = "vcwirtc.LocationCode = '" + locationCode + "'  AND ";
            }

            if (rreDeliveryNote != "ALL")
            {
                whereCondition += "VendorRefChallanNo = '" + rreDeliveryNote + "' AND ";
            }

            string deliveryNoteValueQuery = "SELECT SUM(deliveryrre.TotalCost)DeliveryNoteValue " +
                                            "FROM " +
                                            "(SELECT vcwirtc.LocationCode, vcwirtc.ItemCode, vcwirtc.VendorDeliveryQuantity, " +
                                            "ucfrcn.UnitCost, (vcwirtc.VendorDeliveryQuantity * ucfrcn.UnitCost) TotalCost " +
                                            "FROM Inv_VendorChallanWithItemReferenceToCheck vcwirtc " +
                                            "INNER JOIN Inv_TempUnitCostForReceivingItemFromRRE ucfrcn " +
                                            "ON vcwirtc.ItemCode = ucfrcn.ItemCode " +
                                            "WHERE " + whereCondition +
                // "VendorRefChallanNo = '" + rreDeliveryNote + "' AND " +
                                            "ucfrcn.YearMonth = '" + yearMonth + "')deliveryrre";

            double? mrrValue = context.Database.SqlQuery<double?>(mrrvQuery).FirstOrDefault();
            double? deliveryNoteValue = context.Database.SqlQuery<double?>(deliveryNoteValueQuery).FirstOrDefault();

            if (mrrValue == null)
                mrrValue = 0;

            if (deliveryNoteValue == null)
                deliveryNoteValue = 0;

            ArrayList arlst = new ArrayList();
            arlst.Add(new { mrrValue = mrrValue, deliveryNoteValue = deliveryNoteValue });

            return arlst;
        }

        public List<ChallanInformationGlanceDetails> ReadChallanInformationGlanceList(string itemType, string locationCode, DateTime dateFrom, DateTime dateTo)
        {
            var v = from cmd in
                        (from cm in context.Inv_ChallanMaster
                         join itt in context.Inv_ItemTransactionTypes
                         on cm.ItemTransTypeID equals itt.ItemTransTypeID
                         where cm.LocationCode == locationCode &&
                               cm.ItemType == itemType &&
                               (cm.ChallanDate >= dateFrom && cm.ChallanDate <= dateTo) &&
                               cm.Status == Helper.Active 
                         select new
                         {
                             cm.ChallanSeqNo,
                             cm.RefChallanNo,
                             itt.ItemTransTypeDesc,
                             cm.ItemType,
                             cm.ChallanToLocationCode,
                             cm.LocationCode,
                             cm.ChallanDate,
                             cm.RefCustomerCode,
                         })
                    join lcinf in context.Common_LocationInfo
                    on cmd.ChallanToLocationCode equals lcinf.LocationCode
                    into lcinfLeftJoin
                    from cmdlinf in lcinfLeftJoin.DefaultIfEmpty() orderby cmd.ChallanDate ascending
                    select new ChallanInformationGlanceDetails
                    {
                        ChallanSeqNo = cmd.ChallanSeqNo,
                        ChallanNo = cmd.RefChallanNo != null ? cmd.RefChallanNo : cmd.RefCustomerCode,
                        TransDate = cmd.ChallanDate,
                        ChallanType = cmd.ItemTransTypeDesc,
                        IssuedTo = cmdlinf.LocationName + " [ " + cmd.ChallanToLocationCode + " ] ",
                        ChallanLocationCode = cmd.LocationCode,
                        MrrLocationCode = cmd.ChallanToLocationCode
                    };

            return v.ToList();
        }

        public List<MrrInformationGlanceDetails> ReadMrrInformationGlanceList(string itemType, string locationCode, DateTime dateFrom, DateTime dateTo)
        {
            //var v = (from cmd in
            //             (from mm in context.Inv_MRRMaster
            //              join itt in context.Inv_ItemTransactionTypes
            //              on mm.ItemTransTypeID equals itt.ItemTransTypeID
            //              where mm.LocationCode == locationCode &&
            //              mm.ItemType == itemType &&
            //             (mm.MRRDate >= dateFrom && mm.MRRDate <= dateTo) &&
            //              mm.ChallanSeqNo != null &&
            //              mm.Status == Helper.Active
            //              select new
            //              {
            //                  mm.MRRSeqNo,
            //                  mm.RefMRRNo,
            //                  itt.ItemTransTypeDesc,
            //                  mm.ItemType,
            //                  mm.ChallanLocationCode,
            //                  mm.LocationCode,
            //                  mm.MRRDate,
            //                  mm.ChallanSeqNo

            //              }).Distinct()
            //         join lcinf in context.Common_LocationInfo
            //         on cmd.ChallanLocationCode equals lcinf.LocationCode
            //         into lcinfLeftJoin
            //         from cmdlinf in lcinfLeftJoin.DefaultIfEmpty()
            //         select new MrrInformationGlanceDetails
            //         {
            //             MRRSeqNo = cmd.MRRSeqNo,
            //             MRRNo = cmd.RefMRRNo,
            //             TransDate = cmd.MRRDate,
            //             MRRType = cmd.ItemTransTypeDesc,
            //             ReceiveFrom = cmdlinf.LocationName + " [" + cmd.ChallanLocationCode + "]",
            //             ChallanSeqNo = cmd.ChallanSeqNo,
            //             ChallanLocationCode = cmd.ChallanLocationCode,
            //             MrrLocationCode = cmd.LocationCode
            //         }).Distinct();

            //return v.ToList();


            string sqlQuery = "SELECT mm.MRRDate TransDate,mm.ItemTransTypeID,it.ItemTransTypeDesc " +
                             " MRRType,mm.RefMRRNo MRRNo,mm.MRRSeqNo,cm.LocationCode ChallanLocationCode, " +
                             " li.LocationName + ' [' + cm.LocationCode + ']' ReceiveFrom,cm.RefChallanNo, " +
                              " cm.ChallanSeqNo FROM Inv_MRRMaster mm " +
                              " INNER JOIN Inv_ItemTransactionTypes it " +
                              " ON mm.ItemTransTypeID = it.ItemTransTypeID " +
                              " LEFT OUTER JOIN Inv_ChallanMaster cm " +
                              " ON mm.ChallanLocationCode = cm.LocationCode AND mm.ChallanSeqNo = cm.ChallanSeqNo " +
                              " LEFT OUTER JOIN Common_LocationInfo li " +
                              " ON cm.LocationCode = li.LocationCode " +
                              " WHERE mm.LocationCode = '" + locationCode + "' AND mm.MRRDate BETWEEN '" + dateFrom + "' AND '" + dateTo + "' AND mm.ItemType = '" + itemType + "' " +
                              " Order by mm.MRRDate ASC";


            //var vv = from pp in
            //             (from mm in context.Inv_MRRMaster
            //              join it in context.Inv_ItemTransactionTypes
            //              on mm.ItemTransTypeID equals it.ItemTransTypeID
            //              select new
            //              {
            //                  LocationCode = mm.ChallanLocationCode,
            //                  mm.ChallanSeqNo
            //              })
            //         join cm in context.Inv_ChallanMaster
            //         on new { pp.LocationCode, pp.ChallanSeqNo } equals new { cm.LocationCode, cm.ChallanSeqNo }
            //         select new
            //         {

            //         }
            //;




            var vvv = from mm in context.Inv_MRRMaster
                      join it in context.Inv_ItemTransactionTypes
                      on mm.ItemTransTypeID equals it.ItemTransTypeID
                      join cm in context.Inv_ChallanMaster
                      on new { LocationCode = mm.ChallanLocationCode, mm.ChallanSeqNo } equals new { cm.LocationCode, cm.ChallanSeqNo }
                      into cmLeftJoin
                      from cmmm in cmLeftJoin.DefaultIfEmpty()
                      join li in context.Common_LocationInfo
                      on cmmm.LocationCode equals li.LocationCode
                      into liLeftJoin
                      from lili in liLeftJoin.DefaultIfEmpty()
                      where mm.LocationCode == locationCode &&
                     (mm.MRRDate >= dateFrom && mm.MRRDate <= dateTo) &&
                     mm.ItemType == itemType orderby mm.MRRDate ascending
                      select new MrrInformationGlanceDetails
                      {
                          TransDate = mm.MRRDate,
                          MRRType = it.ItemTransTypeDesc,
                          MRRNo = mm.RefMRRNo,
                          MRRSeqNo = mm.MRRSeqNo,
                          RefChallanNo = cmmm.RefChallanNo != null ? cmmm.RefChallanNo : mm.RefCustomerCode,
                          ChallanSeqNo = cmmm.ChallanSeqNo,
                          ChallanLocationCode = lili.LocationCode,
                          ReceiveFrom = lili.LocationName + " [" + lili.LocationCode + "]",
                          MrrLocationCode = mm.LocationCode
                      };

            // var v = context.Database.SqlQuery<MrrInformationGlanceDetails>(sqlQuery);
            return vvv.ToList();
        }

        public List<StockInTransitAtGlanceDetails> ReadStockInTransitAtGlanceList(string challanType, string locationCode, DateTime dateFrom, DateTime dateTo)
        {
            SqlParameter[] storeParam = new SqlParameter[6];
            storeParam[0] = new SqlParameter("prmOption", challanType);
            storeParam[1] = new SqlParameter("prmItemType", "INVTORYITM");
            storeParam[2] = new SqlParameter("prmLocationCode", locationCode);
            storeParam[3] = new SqlParameter("prmItemCode", string.Empty);
            storeParam[4] = new SqlParameter("prmFromDate", dateFrom);
            storeParam[5] = new SqlParameter("prmToDate", dateTo);

            var resultList = context.Database.SqlQuery<StockInTransitAtGlanceDetails>
                 ("Exec REP_InvItemInTransitV2 @prmOption, @prmItemType, @prmLocationCode, @prmItemCode, @prmFromDate, @prmToDate", storeParam);

            return resultList.ToList();
        }

        public ArrayList ReadVendorRefChallanNo(string billNO)
        {
            List<Inv_VendorChallanWithItemReferenceToCheck> VendorChallanWithItemList = context.Inv_VendorChallanWithItemReferenceToCheck.Where(s => s.VendorBillNo == billNO).ToList();
            VendorChallanWithItemList = VendorChallanWithItemList.GroupBy(g => g.VendorRefChallanNo).Select(s => s.First()).ToList();

            var v = from o in
                        (from vcwit in VendorChallanWithItemList
                         select new
                         {
                             vcwit.VendorRefChallanNo,
                             vcwit.VendorBillDate
                         })
                    select new
                    {
                        RREDeliverNoteNo = o.VendorRefChallanNo,
                        BillDate = Convert.ToDateTime(o.VendorBillDate).ToString("dd-MMM-yyyy")
                    };

            ArrayList VendorRefChallanNo = new ArrayList(v.ToList());
            return VendorRefChallanNo;
        }

        public List<StockInTransitAtGlanceDetails> ReadInvItemInTransit(string itemType, string locationCode, string itemCode, DateTime fromDate, DateTime toDate)
        {
            //return context.InvItemInTransit("INDV_LOCATION_INDV_ITEM", itemType, locationCode.Trim(), itemCode.Trim(), fromDate, toDate).ToList();
            string indvItem;
            indvItem = (itemCode == string.Empty) ? "INDV_LOCATION_ALL_ITEM" : "INDV_LOCATION_INDV_ITEM";

            SqlParameter[] storeParam = new SqlParameter[6];
            storeParam[0] = new SqlParameter("prmOption", indvItem);
            storeParam[1] = new SqlParameter("prmItemType", itemType);
            storeParam[2] = new SqlParameter("prmLocationCode", locationCode);
            storeParam[3] = new SqlParameter("prmItemCode", itemCode);
            storeParam[4] = new SqlParameter("prmFromDate", fromDate);
            storeParam[5] = new SqlParameter("prmToDate", toDate);

            var resultList = context.Database.SqlQuery<StockInTransitAtGlanceDetails>
                ("Exec REP_InvItemInTransitV2 @prmOption, @prmItemType, @prmLocationCode, @prmItemCode, @prmFromDate, @prmToDate", storeParam);

            return resultList.ToList();
        }

        public InventoryInTransitBalance ReadInventoryInTransitBalance(string options, string yearMonth)
        {
            SqlParameter[] storeParam = new SqlParameter[2];
            storeParam[0] = new SqlParameter("prmOption", options);
            storeParam[1] = new SqlParameter("prmAsOnYearMonth", yearMonth);

            var resultList = context.Database.SqlQuery<InventoryInTransitBalance>
                ("Exec REP_InvGetInventoryInTransitBalance @prmOption, @prmAsOnYearMonth", storeParam);

            return resultList.FirstOrDefault();
        }

        public List<UnitWiseCustomerLedger> ReadUnitWiseCustomerLedger(string reportOption, string locationCode, string dateFrom, string dateTo)
        {
            SqlParameter[] storeParam = new SqlParameter[4];
            storeParam[0] = new SqlParameter("prmOption", reportOption);
            storeParam[1] = new SqlParameter("prmLocationCode", locationCode);
            storeParam[2] = new SqlParameter("prmFromDate", dateFrom);
            storeParam[3] = new SqlParameter("prmToDate", dateTo);

            var resultList = context.Database.SqlQuery<UnitWiseCustomerLedger>
                ("Exec REP_GetUnitWiseCustomerLedger @prmOption, @prmLocationCode, @prmFromDate, @prmToDate", storeParam);

            return resultList.ToList();
        }

        public string AuditSequenceNumberMax(string locationCode, string yearMonthDate)
        {
            string auditSeqQuery = " SELECT TOP(1) ISNULL(adfa.AuditSeqNo,'') AuditSeqNo " +
                                   " FROM Aud_AuditingMaster adfa " +
                                   " WHERE adfa.LocationCode = '" + locationCode + "' AND " +
                                   " CONVERT(INT,SUBSTRING(adfa.AuditSeqNo,1, 6)) = '" + yearMonthDate + "' " +
                                   " ORDER BY  CONVERT(INT,SUBSTRING(adfa.AuditSeqNo,1, 6)) DESC, " +
                                   " CONVERT(INT,SUBSTRING(adfa.AuditSeqNo,7, LEN(adfa.AuditSeqNo))) DESC ";

            var seqNumber = context.Database.SqlQuery<string>(auditSeqQuery).FirstOrDefault();

            string auditNumber = string.Empty;

            if (seqNumber != null)
            {
                auditNumber = seqNumber.ToString();
            }

            return auditNumber;
        }

        public bool IsAuditAdjustmentObservationOnSalesAgreementExistOrNot(string locationCode, string yearMonth, string customerCode, string auditSeqNo)
        {
            bool isExistOrNot = true;

            Aud_AuditAdjustmentObservationOnSalesAgreement objObservationOnSalesAgreement = new Aud_AuditAdjustmentObservationOnSalesAgreement();
            objObservationOnSalesAgreement = context.Aud_AuditAdjustmentObservationOnSalesAgreement.Where(a => a.LocationCode == locationCode && a.YearMonth == yearMonth && a.CustomerCode == customerCode && a.AuditSeqNo == auditSeqNo).FirstOrDefault();

            if (objObservationOnSalesAgreement == null)
            {
                isExistOrNot = false;
            }

            return isExistOrNot;
        }

        public Common_Sys_SystemSettings ReadSystemSettings(string companyName)
        {
            return context.Common_Sys_SystemSettings.Where(s => s.CompanyName == companyName).FirstOrDefault();
        }

        public bool IsCashMemoManagementEnabled(string companyName)
        {
            var v = context.Common_Sys_SystemSettings.Where(s => s.CompanyName == companyName).FirstOrDefault();
            bool isCashMemoSettingDone = false;

            if (v != null)
            {
                isCashMemoSettingDone = v.IsCashMemoManagementEnabled;
            }

            return isCashMemoSettingDone;
        }

        public List<AvailableNAssignFixedAsset> ReadAvailableNAssignCashMemo(byte storeLocation, string locationCode, string itemCategory, string employeeSelectionOption, string employeeID)
        {
            SqlParameter[] storeParam = new SqlParameter[5];
            storeParam[0] = new SqlParameter("prmStoreLocation", storeLocation);
            storeParam[1] = new SqlParameter("prmLocationCode", locationCode);
            storeParam[2] = new SqlParameter("prmItemCategory", itemCategory);
            storeParam[3] = new SqlParameter("prmEmployeeSelectionOption", employeeSelectionOption);
            storeParam[4] = new SqlParameter("prmEmployeeID", employeeID);

            var resultList = context.Database.SqlQuery<AvailableNAssignFixedAsset>("Exec SP_InvGetAvailableItemInALocationWithAllocationToEmployee @prmStoreLocation, @prmLocationCode, @prmItemCategory, @prmEmployeeSelectionOption, @prmEmployeeID", storeParam);

            return resultList.ToList();
        }

        public List<AssignCashMemoBook> CashMemoAssignUnassign(string locationCode, string itemCode,  byte storeLocation, string employeeId, byte status, string unAssignedQuantity)
        {
            //string option = string.Empty;

            //if (string.IsNullOrEmpty(employeeId))
            //{
            //    option = "ALL";
            //    employeeId = string.Empty;
            //}
            //else
            //{
            //    option = "INDIVIDUAL";
            //}

            //SqlParameter[] storeParam = new SqlParameter[3];
            //storeParam[0] = new SqlParameter("@Option", option);
            //storeParam[1] = new SqlParameter("@LocationCode", locationCode);
            //storeParam[2] = new SqlParameter("@EmployeeID", employeeId);

            //var resultList = context.Database.SqlQuery<AssignCashMemoBook>("Exec SP_GetLocationWiseEmployeeListNToAssignCashMemoBook @Option, @LocationCode, @EmployeeID", storeParam);


            SqlParameter[] storeParam = new SqlParameter[6];
            storeParam[0] = new SqlParameter("Option", "FORASSIGN");
            storeParam[1] = new SqlParameter("prmStoreLocation", storeLocation);
            storeParam[2] = new SqlParameter("prmLocationCode", locationCode);
            storeParam[3] = new SqlParameter("prmItemCode", itemCode);
            storeParam[4] = new SqlParameter("prmEmployeeID", employeeId);
            storeParam[5] = new SqlParameter("prmAvailableToAssignQuantity", unAssignedQuantity);

            var resultList = context.Database.SqlQuery<AssignCashMemoBook>("Exec SP_GetLocationWiseEmployeeListToAssignUnassignOrView @Option, @prmStoreLocation, @prmLocationCode, @prmItemCode, @prmEmployeeID, @prmAvailableToAssignQuantity", storeParam);

            return resultList.ToList();

        }

        public ArrayList GetCashMemoAssignedOrUnassignedSeialList(byte storeLocation, string locationCode, string itemCode, string employeeId, int assignQuantity)
        {
            List<Inv_ItemStockWithSerialNoByLocation> lstItemStockWithSerial = new List<Inv_ItemStockWithSerialNoByLocation>();

            lstItemStockWithSerial = (from iswsnbl in context.Inv_ItemStockWithSerialNoByLocation
                                      where iswsnbl.StoreLocation == storeLocation &&
                                            iswsnbl.LocationCode == locationCode &&
                                            iswsnbl.ItemCode == itemCode &&
                                            iswsnbl.IsAvailableInStore == true
                                      select iswsnbl).ToList();

            ArrayList serialList = new ArrayList();

            foreach (Inv_ItemStockWithSerialNoByLocation itemserial in lstItemStockWithSerial)
            {
                serialList.Add(new { Display = itemserial.ItemSerialNo, Value = itemserial.ItemSerialNo });
            }

            //List<Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo> lstAssignSerials = new List<Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo>();
            //lstItemStockWithSerial = (from iswsnbl in context.Inv_ItemStockWithSerialNoByLocation
            //                          join cmbm in context.Common_CashMemoBookMaster
            //                          on iswsnbl.ItemSerialNo equals cmbm.CashMemoBookNo
            //                          where iswsnbl.StoreLocation == storeLocation &&
            //                                iswsnbl.LocationCode == locationCode &&
            //                                iswsnbl.ItemCode == itemCode &&
            //                                iswsnbl.IsAvailableInStore == true &&
            //                                cmbm.IsAssignedForUse == false &&
            //                                cmbm.Status == Helper.Active
            //                          select iswsnbl).ToList();

            //ArrayList serialList = new ArrayList();

            //if (assignQuantity > 0)
            //{
            //    lstAssignSerials = (from ewfasn in context.Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo
            //                        join cmbm in context.Common_CashMemoBookMaster
            //                        on ewfasn.ItemSerialNo equals cmbm.CashMemoBookNo
            //                        where ewfasn.LocationCode == locationCode &&
            //                             ewfasn.ItemCode == itemCode &&
            //                             ewfasn.IsItAllocated == true &&
            //                             cmbm.Status == Helper.Active
            //                        select ewfasn
            //                        ).ToList();


            //    foreach (Inv_ItemStockWithSerialNoByLocation itemserial in lstItemStockWithSerial)
            //    {
            //        var v = (from ss in lstAssignSerials where ss.ItemSerialNo == itemserial.ItemSerialNo select ss.ItemSerialNo).FirstOrDefault();

            //        if (v == null)
            //        {
            //            serialList.Add(new { Display = itemserial.ItemSerialNo, Value = itemserial.ItemSerialNo });
            //        }
            //    }
            //}
            //else
            //{
            //    lstAssignSerials = context.Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo.Where(f => f.LocationCode == locationCode && f.EmployeeID == employeeId && f.ItemCode == itemCode && f.IsItAllocated == true).ToList();

            //    foreach (Inv_ItemStockWithSerialNoByLocation itemserial in lstItemStockWithSerial)
            //    {
            //        var v = (from ss in lstAssignSerials where ss.ItemSerialNo == itemserial.ItemSerialNo select ss.ItemSerialNo).FirstOrDefault();

            //        if (v != null)
            //        {
            //            serialList.Add(new { Display = itemserial.ItemSerialNo, Value = itemserial.ItemSerialNo });
            //        }
            //    }
            //}

            return serialList;
        }

        public string GetCashMemoInUsesId(string entrySource, string cashMemoNo, string cashMemoUsesId, string locationCode, string refDocNo)
        {
            string cashMemoId = string.Empty;

            SqlParameter[] storeParamForCashmemo = new SqlParameter[6];
            storeParamForCashmemo[0] = new SqlParameter("prmEntrySource", entrySource);
            storeParamForCashmemo[1] = new SqlParameter("prmCashMemoNo", cashMemoNo);
            storeParamForCashmemo[2] = new SqlParameter("prmCashMemoUsesID", cashMemoUsesId);
            storeParamForCashmemo[3] = new SqlParameter("prmLocationCode", locationCode);
            storeParamForCashmemo[4] = new SqlParameter("prmRefDocNo", refDocNo);
            storeParamForCashmemo[5] = new SqlParameter("prmCashMemoUsedByEmployee", Helper.DBNullValue);

            var v = context.Database.SqlQuery<string>("SP_CheckNSaveCashMemoNoInCashMemoMasterNTransactionRegisterTables @prmEntrySource, @prmCashMemoNo, @prmCashMemoUsesID, @prmLocationCode, @prmRefDocNo, @prmCashMemoUsedByEmployee", storeParamForCashmemo);

            if (v != null)
                cashMemoId = v.ToString();

            return cashMemoId;
        }

        public bool IsMemoStillAvailableToUse(string employeeId)
        {
            bool IsAvailableToUse = false;

            string cashMemoQuery = "SELECT COUNT(cmm.IsAvailableToUse) MemoStillAvailableToUse FROM Common_CashMemoBookMaster cmb " +
                                   "INNER JOIN Common_CashMemoMaster cmm ON cmm.CashMemoBookNo = cmb.CashMemoBookNo " +
                                   "WHERE cmb.AssignedToEmployee = '" + employeeId + "' AND cmm.IsAvailableToUse = 1";

            var v = context.Database.SqlQuery<int?>(cashMemoQuery).FirstOrDefault();

            if (v != null)
            {
                if (Convert.ToInt32(v) == 0)
                    IsAvailableToUse = true;
            }

            return IsAvailableToUse;
        }

       

        public CustomerDataToCloseWithFullPaidOrWaive getCustomerDataToCloseWithFullPaidOrWaive(string select, string customerCode, string currentMonth, string locationCode)
        {
            SqlParameter[] storeParam = new SqlParameter[4];
            storeParam[0] = new SqlParameter("Option", select);
            storeParam[1] = new SqlParameter("CustomerCode", customerCode);
            storeParam[2] = new SqlParameter("CurrentOrLastMonth", currentMonth);
            storeParam[3] = new SqlParameter("LocationCode", locationCode);

            var resultList = context.Database.SqlQuery<CustomerDataToCloseWithFullPaidOrWaive>("Exec SP_SalGetCustomerDataToCloseWithFullPaidOrWaive @Option, @CustomerCode, @CurrentOrLastMonth, @LocationCode", storeParam);

            return resultList.FirstOrDefault();
        }

        public CustomerDataToPersonalInformation GetCustomerDataPersonalInformation(string customerCode)
        {
         

            //var customerInfo = from sc in context.Sal_Customer
            //                   join un in context.Common_UnionInfo on sc.UnionID equals un.UnionID
            //                   join up in context.Common_UpazillaInfo on sc.ThanaID equals up.UPAZ_CODE
            //                   join di in context.Common_DistrictInfo on sc.DistrictCode equals di.DIST_CODE
            //                   where sc.CustomerCode == customerCode
            //                   select new CustomerDataToPersonalInformation
            //                   {
            //                       CustomerCode=sc.CustomerCode,
            //                       CustomerName=sc.CustomerName,
            //                       CustomerFatherHusbandName=sc.FathersOrHusbandName,
            //                       Village=sc.Village,
            //                       PostOffice=sc.PostOffice,
            //                       UnionCode=sc.UnionID,
            //                       UnionName=un.UnionName,
            //                       UpzillCode=up.UPAZ_CODE,
            //                       UpzillName = up.UPAZ_NAME,
            //                       DistrictCode=di.DIST_CODE,
            //                       DistrictName = di.DIST_NAME,
            //                       MobileNumber=sc.PhoneNo,
            //                       ModifiedBy=sc.ModifiedBy,
            //                       ModifiedOn=sc.ModifiedOn
            //                   };

            // return customerInfo.FirstOrDefault();  
            var customerInfo = from sc in context.Sal_Customer
                               join un in context.Common_UnionInfo on sc.UnionID equals un.UnionID into gj
                               from subpet in gj.DefaultIfEmpty()
                               join up in context.Common_UpazillaInfo on sc.ThanaID equals up.UPAZ_CODE
                               join di in context.Common_DistrictInfo on sc.DistrictCode equals di.DIST_CODE
                               where sc.CustomerCode == customerCode
                               select new CustomerDataToPersonalInformation
                               {
                                   CustomerCode = sc.CustomerCode,
                                   CustomerName = sc.CustomerName,
                                   CustomerFatherHusbandName = sc.FathersOrHusbandName,
                                   Village = sc.Village,
                                   PostOffice = sc.PostOffice,
                                   UnionCode = sc.UnionID,
                                   UnionName = (subpet ==null?String.Empty: subpet.UnionName),
                                   UpzillCode = up.UPAZ_CODE,
                                   UpzillName = up.UPAZ_NAME,
                                   DistrictCode = di.DIST_CODE,
                                   DistrictName = di.DIST_NAME,
                                   MobileNumber = sc.PhoneNo,
                                   ModifiedBy = sc.ModifiedBy,
                                   ModifiedOn = sc.ModifiedOn
                               };

            return customerInfo.FirstOrDefault();    

                           
        }



        public List<UnitWiseCashMemoInformation> ReadCashMemoInformation(string locationCode, string dateFrom, string dateTo)
        {
            SqlParameter[] storeParam = new SqlParameter[3];
            storeParam[0] = new SqlParameter("prmLocationCode", locationCode);
            storeParam[1] = new SqlParameter("prmFromDate", dateFrom);
            storeParam[2] = new SqlParameter("prmToDate", dateTo);

            var resultList = context.Database.SqlQuery<UnitWiseCashMemoInformation>
                ("Exec REP_GetCashMemoWiseTransactions @prmLocationCode, @prmFromDate, @prmToDate", storeParam);

            return resultList.ToList();
        }

        // Auditor's Methods For Sales Adjustment ----------------------------------------

        public CustomerNAgreementNItemDetails ReadCustomerNAgreementItemDetails(string customerCode)
        {
            CustomerNAgreementNItemDetails objCustomerNAgreementNItemDetails = new CustomerNAgreementNItemDetails();

            objCustomerNAgreementNItemDetails = (from cs in context.Sal_Customer
                                                 join sag in context.Sal_SalesAgreement
                                                 on cs.CustomerCode equals sag.CustomerCode
                                                 join sm in context.Sal_PackageMaster
                                                 on sag.PackageCode equals sm.PackageCode
                                                 join mop in context.Sal_ModeOfPayment
                                                 on sag.ModeOfPaymentID equals mop.ModeOfPaymentID
                                                 join cst in context.Sal_CustomerType
                                                 on sag.CustomerType equals cst.CustomerTypeID
                                                 join lite in context.Sal_Light
                                                 on sm.Light equals lite.LightID
                                                 into liteLeftJoin
                                                 from litecap in liteLeftJoin.DefaultIfEmpty()
                                                 join pakcap in context.Sal_PackageOrItemCapacity
                                                 on sm.Capacity equals pakcap.CapacityID
                                                 into pakcapLeftJoin
                                                 from pakcaps in pakcapLeftJoin.DefaultIfEmpty()
                                                 where cs.CustomerCode == customerCode
                                                 select new CustomerNAgreementNItemDetails
                                                 {
                                                     CustomerCode = cs.CustomerCode,
                                                     CustomerName = cs.CustomerName,
                                                     AgreementDate = sag.AgreementDate,

                                                     CustomerType = sag.CustomerType,
                                                     CustomerTypeName = cst.Description,

                                                     PackageCode = sag.PackageCode,
                                                     PackageName = sm.Description,

                                                     ProjectCode = sm.ProjectCode,

                                                     Capacity = sm.Capacity,
                                                     PackageCapacity = pakcaps.Description,

                                                     Light = sm.Light,
                                                     PackageLight = litecap.Description,

                                                     PanelSerialNo = sag.PanelSerialNo,
                                                     BatterySerialNo = sag.BatterySerialNo,

                                                     ModeOfPaymentID = sag.ModeOfPaymentID,
                                                     ModeOfPaymentName = mop.Description,

                                                     DownPaymentID = sag.DownPaymentID,
                                                     STDDownPaymentPercentage = sag.STDDownPaymentPercentage,
                                                     DownPaymentAmount = sag.DownPaymentAmount,

                                                     ServiceChargeID = sag.ServiceChargeID,
                                                     STDServiceChargePercentage = sag.STDServiceChargePercentage,

                                                     RefMemoNo = sag.RefMemoNo,
                                                     CashMemoNo = sag.CashMemoNo,
                                                     CashMemoUsesID = sag.CashMemoUsesID,

                                                     PackagePrice = sag.PackagePrice,
                                                     IsResales = sag.IsReSales

                                                 }).FirstOrDefault();


            List<SalesItemDetails> lstSalesItemDetails = new List<SalesItemDetails>();

            lstSalesItemDetails = (from salitm in context.Sal_SalesItems
                                   join itm in context.Inv_ItemMaster
                                   on salitm.ItemCode equals itm.ItemCode
                                   join itmcsc in context.Inv_ItemCategorySubCategory
                                   on salitm.ItemCategory equals itmcsc.ItemCategoryID
                                   join im in context.Inv_ItemModel
                                   on salitm.ItemModel equals im.ItemModelID
                                   join pakcap in context.Sal_PackageOrItemCapacity
                                   on salitm.ItemCapacity equals pakcap.CapacityID
                                   into pakcapLeftJoin
                                   from pakcaps in pakcapLeftJoin.DefaultIfEmpty()
                                   where salitm.CustomerCode == customerCode &&
                                         (itmcsc.ItemCategoryID == "PANEL1" || itmcsc.ItemCategoryID == "BAT001" ||
                                          itmcsc.ItemCategoryID == "PNLSR1" || itmcsc.ItemCategoryID == "HOL001")
                                   select new SalesItemDetails
                                   {
                                       ItemCategory = salitm.ItemCategory,
                                       ItemCode = salitm.ItemCode,
                                       ItemName = itm.ItemName,
                                       ItemCapacity = salitm.ItemCapacity,
                                       ItemCapacityName = pakcaps.Description,
                                       ItemModel = salitm.ItemModel,
                                       ItemModelName = im.Description,
                                       FromStoreLocation = salitm.FromStoreLocation

                                   }).ToList();

            List<SalesItemDetails> lstSalesItemDetailQuantityDetails = new List<SalesItemDetails>();

            lstSalesItemDetailQuantityDetails = (from salitm in context.Sal_SalesItems
                                                 join itmcsc in context.Inv_ItemCategorySubCategory
                                                 on salitm.ItemCategory equals itmcsc.ItemCategoryID
                                                 join im in context.Inv_ItemMaster
                                                 on salitm.ItemCode equals im.ItemCode
                                                 where salitm.CustomerCode == customerCode
                                                 select new SalesItemDetails
                                                 {
                                                     ItemCategory = salitm.ItemCategory,
                                                     ItemCode = salitm.ItemCode,
                                                     ItemName = im.ItemName,
                                                     ItemCapacity = salitm.ItemCapacity,
                                                     ItemModel = salitm.ItemModel,
                                                     ItemQuantity = salitm.ItemQuantity,
                                                     FromStoreLocation = salitm.FromStoreLocation
                                                 }).ToList();


            objCustomerNAgreementNItemDetails.PanelName = lstSalesItemDetails.Where(p => p.ItemCategory == "PANEL1").Select(s => s.ItemName).FirstOrDefault();
            objCustomerNAgreementNItemDetails.PanelItemCode = lstSalesItemDetails.Where(p => p.ItemCategory == "PANEL1").Select(s => s.ItemCode).FirstOrDefault();
            objCustomerNAgreementNItemDetails.StoreLocationForPanel = lstSalesItemDetails.Where(p => p.ItemCategory == "PANEL1").Select(s => s.FromStoreLocation).FirstOrDefault();

            objCustomerNAgreementNItemDetails.BatteryName = lstSalesItemDetails.Where(p => p.ItemCategory == "BAT001").Select(s => s.ItemName).FirstOrDefault();
            objCustomerNAgreementNItemDetails.BatteryItemCode = lstSalesItemDetails.Where(p => p.ItemCategory == "BAT001").Select(s => s.ItemCode).FirstOrDefault();
            objCustomerNAgreementNItemDetails.StoreLocationForBattery = lstSalesItemDetails.Where(p => p.ItemCategory == "BAT001").Select(s => s.FromStoreLocation).FirstOrDefault();

            objCustomerNAgreementNItemDetails.PanelStructureName = lstSalesItemDetails.Where(p => p.ItemCategory == "PNLSR1").Select(s => s.ItemName).FirstOrDefault();
            objCustomerNAgreementNItemDetails.PanelStructureItemCode = lstSalesItemDetails.Where(p => p.ItemCategory == "PNLSR1").Select(s => s.ItemCode).FirstOrDefault();

            objCustomerNAgreementNItemDetails.HolderName = lstSalesItemDetails.Where(p => p.ItemCategory == "HOL001").Select(s => s.ItemName).FirstOrDefault();
            objCustomerNAgreementNItemDetails.HolderItemCode = lstSalesItemDetails.Where(p => p.ItemCategory == "HOL001").Select(s => s.ItemCode).FirstOrDefault();

            objCustomerNAgreementNItemDetails.HolderQty = lstSalesItemDetailQuantityDetails.Where(p => p.ItemCategory == "HOL001" && p.ItemCode == "1290").Select(s => s.ItemQuantity).FirstOrDefault();
            objCustomerNAgreementNItemDetails.TubeLightQty = lstSalesItemDetailQuantityDetails.Where(p => p.ItemCategory == "LTTUB1" && p.ItemCode == "1255").Select(s => s.ItemQuantity).FirstOrDefault();
            objCustomerNAgreementNItemDetails.LEDTubeQty = lstSalesItemDetailQuantityDetails.Where(p => p.ItemCategory == "LTTUB1" && p.ItemCode == "1256").Select(s => s.ItemQuantity).FirstOrDefault();
            objCustomerNAgreementNItemDetails.LED2Qty = lstSalesItemDetailQuantityDetails.Where(p => p.ItemCategory == "LTLED1" && p.ItemCode == "1270").Select(s => s.ItemQuantity).FirstOrDefault();
            objCustomerNAgreementNItemDetails.LED3Qty = lstSalesItemDetailQuantityDetails.Where(p => p.ItemCategory == "LTLED1" && p.ItemCode == "1260").Select(s => s.ItemQuantity).FirstOrDefault();
            objCustomerNAgreementNItemDetails.LED5Qty = lstSalesItemDetailQuantityDetails.Where(p => p.ItemCategory == "LTLED1" && p.ItemCode == "1265").Select(s => s.ItemQuantity).FirstOrDefault();
            objCustomerNAgreementNItemDetails.CFLQty = lstSalesItemDetailQuantityDetails.Where(p => p.ItemCategory == "HOL001" && p.ItemCode == "1291").Select(s => s.ItemQuantity).FirstOrDefault();
            objCustomerNAgreementNItemDetails.LampShade = lstSalesItemDetailQuantityDetails.Where(p => p.ItemCategory == "LMPS01" && p.ItemCode == "1205").Select(s => s.ItemName).FirstOrDefault();
            objCustomerNAgreementNItemDetails.LampShadeQty = lstSalesItemDetailQuantityDetails.Where(p => p.ItemCategory == "LMPS01" && p.ItemCode == "1205").Select(s => s.ItemQuantity).FirstOrDefault();
            objCustomerNAgreementNItemDetails.CircuitOrLampCkt = lstSalesItemDetailQuantityDetails.Where(p => p.ItemCategory == "CKT001" && p.ItemCode == "1190").Select(s => s.ItemName).FirstOrDefault();
            objCustomerNAgreementNItemDetails.CircuitOrLampCktQty = lstSalesItemDetailQuantityDetails.Where(p => p.ItemCategory == "CKT001" && p.ItemCode == "1190").Select(s => s.ItemQuantity).FirstOrDefault();
            objCustomerNAgreementNItemDetails.ChargeController10AmpsQty = lstSalesItemDetailQuantityDetails.Where(p => p.ItemCategory == "CCR001" && p.ItemCode == "1185").Select(s => s.ItemQuantity).FirstOrDefault();
            objCustomerNAgreementNItemDetails.ChargeController6AmpsQty = lstSalesItemDetailQuantityDetails.Where(p => p.ItemCategory == "CCR001" && p.ItemCode == "1195").Select(s => s.ItemQuantity).FirstOrDefault();
            objCustomerNAgreementNItemDetails.Cable70r76Qty = lstSalesItemDetailQuantityDetails.Where(p => p.ItemCategory == "CBL001" && p.ItemCode == "1295").Select(s => s.ItemQuantity).FirstOrDefault();
            objCustomerNAgreementNItemDetails.Cable40r76Qty = lstSalesItemDetailQuantityDetails.Where(p => p.ItemCategory == "CBL001" && p.ItemCode == "1300").Select(s => s.ItemQuantity).FirstOrDefault();
            objCustomerNAgreementNItemDetails.Cable23r76Qty = lstSalesItemDetailQuantityDetails.Where(p => p.ItemCategory == "CBL001" && p.ItemCode == "1310").Select(s => s.ItemQuantity).FirstOrDefault();
            objCustomerNAgreementNItemDetails.Cable14r76Qty = lstSalesItemDetailQuantityDetails.Where(p => p.ItemCategory == "CBL001" && p.ItemCode == "1301").Select(s => s.ItemQuantity).FirstOrDefault();
            objCustomerNAgreementNItemDetails.SwitchQty = lstSalesItemDetailQuantityDetails.Where(p => p.ItemCategory == "SWT001" && p.ItemCode == "1330").Select(s => s.ItemQuantity).FirstOrDefault();
            objCustomerNAgreementNItemDetails.SwitchBoardBoxQty = lstSalesItemDetailQuantityDetails.Where(p => p.ItemCategory == "SWBX01" && p.ItemCode == "1340").Select(s => s.ItemQuantity).FirstOrDefault();


            objCustomerNAgreementNItemDetails.CustomerItems = lstSalesItemDetails;

            return objCustomerNAgreementNItemDetails;
        }

        public List<ProjectInfo> ReadProjectForAuditor(string programCode)
        {
            var projects = from pi in context.Common_ProjectInfo
                           where pi.Status == Helper.Active
                           select new ProjectInfo
                           {
                               ProjectCode = pi.ProjectCode,
                               ProjectName = pi.ProjectCode + " - " + pi.ProjectName
                           };

            return projects.ToList();
        }

        public List<ItemCapacity> ReadPackageOrItemCapacityForAuditor(string projectCode, string isForItemOrPackage)
        {
            var v = from itmcap in context.Sal_PackageOrItemCapacity
                    where itmcap.ProjectCode == projectCode && itmcap.CapacityFor.Contains(isForItemOrPackage)
                    select new ItemCapacity
                    {
                        ItemCapacityID = itmcap.CapacityID,
                        CapacityDescription = itmcap.Description
                    };

            return v.ToList();
        }

        public List<LightInfo> ReadLightForAuditor(string capacityID)
        {
            string sqlQuery = "SELECT LG.LightID, LG.[Description] LightDescription FROM Sal_PackageOrItemCapacity PIC " +
                              " INNER JOIN Sal_Validation_CapacityVsLight CVL ON " +
                              " PIC.CapacityID = CVL.CapacityID " +
                              " INNER JOIN Sal_Light LG ON " +
                              " LG.LightID = CVL.LightID " +
                              " WHERE PIC.CapacityID = '" + capacityID + "'";

            var results = context.Database.SqlQuery<LightInfo>(sqlQuery);

            return results.ToList();
        }

        public List<PackageInformation> ReadPackagesForAuditor(string capacityId, string lightId)
        {
            var v = from pck in context.Sal_PackageMaster
                    where pck.Capacity == capacityId &&
                    pck.Light == lightId
                    select new PackageInformation
                    {
                        PackageCode = pck.PackageCode,
                        PackageDescription = pck.Description,
                        PerUnitSalesPrice = pck.PerUnitSalesPrice
                    };

            return v.ToList();
        }

        public List<ItemInfo> ReadInvItemsForAuditors(string itemType, string itemCategory)
        {
            var v = from itm in context.Inv_ItemMaster
                    where itm.ItemType == itemType &&
                    itm.ItemCategory == itemCategory
                    select new ItemInfo
                    {
                        ItemCode = itm.ItemCode,
                        ItemName = itm.ItemName,
                        AverageUnitCost = itm.AverageUnitCost
                    };

            return v.ToList();
        }

        public List<ItemInfo> ReadInvItemsForAuditors(string itemType, string itemCategory, string itemCapacity)
        {
            var v = from itm in context.Inv_ItemMaster
                    where itm.ItemType == itemType &&
                    itm.ItemCategory == itemCategory &&
                    itm.ItemCapacity == itemCapacity
                    select new ItemInfo
                    {
                        ItemCode = itm.ItemCode,
                        ItemName = itm.ItemName,
                        AverageUnitCost = itm.AverageUnitCost
                    };

            return v.ToList();
        }

        public List<ItemSerialInfo> ReadItemStockWithSerialNoByLocationForAuditor(byte storeLocation, string locationCode, string itemCode)
        {

            var v = from itmsrl in context.Inv_ItemStockWithSerialNoByLocation
                    where itmsrl.StoreLocation == storeLocation && itmsrl.LocationCode == locationCode &&
                    itmsrl.ItemCode == itemCode && itmsrl.IsAvailableInStore == true
                    select new ItemSerialInfo
                    {
                        ItemCode = itmsrl.ItemCode,
                        ItemSerialNo = itmsrl.ItemSerialNo
                    };


            return v.ToList();
        }

        public List<ServiceChargeInformation> ReadServiceChargePolicyForAuditor(string customerType, string modeOfPayment)
        {
            var v = from srvchrg in context.Sal_Validation_ProgramVsCustomerTypeNModeOfPaymentWiseServiceChargePolicy
                    where srvchrg.CustomerType == customerType &&
                    srvchrg.ModeOfPaymentID == modeOfPayment
                    select new ServiceChargeInformation
                    {
                        ServiceChargeID = srvchrg.ServiceChargeID,
                        ServiceChargePercentage = srvchrg.ServiceChargePercentage,
                        ServiceChargeDescription = srvchrg.Description + " [ " + System.Data.Objects.SqlClient.SqlFunctions.StringConvert((double)srvchrg.ServiceChargePercentage) + " %]"
                    };

            return v.ToList();
        }

        public List<DownPaymentPolicy> ReadDownPaymentPolicyForAuditor(string modeOfPayment, string packageCode)
        {
            var v = from srvchrg in context.Sal_Validation_ModeOfPaymentNPackageWiseDownpaymentPolicy
                    where srvchrg.ModeOfPaymentID == modeOfPayment &&
                    srvchrg.PackageCode == packageCode
                    select new DownPaymentPolicy
                    {
                        DownPaymentID = srvchrg.DownPaymentID,
                        DownPaymentDescription = srvchrg.Description,
                        DownPaymentPercentage = srvchrg.DownPaymentPercentage
                    };

            return v.ToList();
        }

        public DiscountPolicy ReadDiscountPolicyByModeofPaymentNDiscountIdForAuditor(string modeOfPayment, string discountId)
        {
            var v = (from mopwdp in context.Sal_Validation_ModeOfPaymentWiseDiscountPolicy
                     where mopwdp.ModeOfPaymentID == modeOfPayment &&
                     mopwdp.DiscountID == discountId
                     select new DiscountPolicy
                     {
                         DiscountID = mopwdp.DiscountID,
                         DiscountDescription = mopwdp.Description,
                         DiscountPercentage = mopwdp.DiscountPercentage

                     }).FirstOrDefault();

            return v;
        }

        public DiscountPolicy ReadDiscountPolicyByModeofPaymentNPackageIdForAuditor(string modeOfPayment, string packageCode)
        {
            //SELECT * FROM 
            //Sal_Validation_ModeOfPaymentWiseDiscountPolicy mopwdp
            //INNER JOIN Sal_Validation_ModeOfPaymentNPackageVsDiscount mopnpvd
            //ON mopwdp.ModeOfPaymentID = mopnpvd.ModeOfPaymentID
            //WHERE mopnpvd.ModeOfPaymentID = '03YEAR' AND 
            //mopnpvd.PackageCode = 'SHS010'

            var v = (from mopwdp in context.Sal_Validation_ModeOfPaymentWiseDiscountPolicy
                     join mopnpvd in context.Sal_Validation_ModeOfPaymentNPackageVsDiscount
                     on mopwdp.ModeOfPaymentID equals mopnpvd.ModeOfPaymentID
                     where mopnpvd.ModeOfPaymentID == modeOfPayment &&
                     mopnpvd.PackageCode == packageCode
                     select new DiscountPolicy
                     {
                         DiscountID = mopwdp.DiscountID,
                         DiscountDescription = mopwdp.Description,
                         DiscountPercentage = mopwdp.DiscountPercentage

                     }).FirstOrDefault();

            return v;
        }

        //Ended Auditor's Methods For Sales Adjustment ----------------------------------------

        public List<ODRecoveryStatusMonitoring> ReadODRecoveryStatusMonitoring(string locationCode, bool IsOnlyForCollectionDatePassed, string customerGrading, string CustomerFPR, byte? scheduledCollectionDay)
        {
            SqlParameter[] storeParam = new SqlParameter[5];
            storeParam[0] = new SqlParameter("prmLocationCode", locationCode);

            storeParam[1] = new SqlParameter("prmIsOnlyForCollectionDatePassed", IsOnlyForCollectionDatePassed);

            if (scheduledCollectionDay != null)
                storeParam[2] = new SqlParameter("prmScheduledCollectionDay", scheduledCollectionDay);
            else
                storeParam[2] = new SqlParameter("prmScheduledCollectionDay", DBNull.Value);

            storeParam[3] = new SqlParameter("prmODCustomerGrade", customerGrading);
            storeParam[4] = new SqlParameter("prmCustomerFPR", CustomerFPR);

            ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext.CommandTimeout = 3600;
            var resultList = context.Database.SqlQuery<ODRecoveryStatusMonitoring>("Exec REP_GetODRecoveryStatusByCustomer @prmLocationCode, @prmIsOnlyForCollectionDatePassed, @prmScheduledCollectionDay, @prmODCustomerGrade, @prmCustomerFPR", storeParam);

            return resultList.ToList();
        }

        public List<SalesRecoveryCommitmentByReview> ReadSalesRecoveryCommitmentByReview(string reportType, string locationCode)
        {
            SqlParameter[] storeParam = new SqlParameter[2];
            storeParam[0] = new SqlParameter("prmReportType", reportType);
            storeParam[1] = new SqlParameter("prmLocationCode", locationCode);

            ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext.CommandTimeout = 10800;
            var resultList = context.Database.SqlQuery<SalesRecoveryCommitmentByReview>("Exec REP_DailyBusinessPerformanceMonitoring_SalesAndRecoveryStatusReview @prmReportType, @prmLocationCode", storeParam);

            return resultList.ToList();
        }

        public List<SalesRecoveryStatusEntryMonitoring> ReadSalesRecoveryStatusEntryMonitoring(string locationCode, bool IsOnlyForCollectionDatePassed, string customerGrading, string CustomerFPR, byte? scheduledCollectionDay)
        {
            SqlParameter[] storeParam = new SqlParameter[5];
            storeParam[0] = new SqlParameter("prmLocationCode", locationCode);

            storeParam[1] = new SqlParameter("prmIsOnlyForCollectionDatePassed", IsOnlyForCollectionDatePassed);

            if (scheduledCollectionDay != null)
                storeParam[2] = new SqlParameter("prmScheduledCollectionDay", scheduledCollectionDay);
            else
                storeParam[2] = new SqlParameter("prmScheduledCollectionDay", DBNull.Value);

            storeParam[3] = new SqlParameter("prmODCustomerGrade", customerGrading);
            storeParam[4] = new SqlParameter("prmCustomerFPR", CustomerFPR);

            ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext.CommandTimeout = 3600;
            var resultList = context.Database.SqlQuery<SalesRecoveryStatusEntryMonitoring>("Exec REP_DailyBusinessPerformanceMonitoring_SalesAndRecoveryStatusInput @prmLocationCode, @prmIsOnlyForCollectionDatePassed, @prmScheduledCollectionDay, @prmODCustomerGrade, @prmCustomerFPR", storeParam);

            return resultList.ToList();
        }

        public Sal_DailyBusinessPerformanceMonitoring_SalesAndRecoveryStatusByUnit ReadDailyBusinessPerformanceMonitoringRemarks(string yearMonth,string locationCode)
        {
            return context.Sal_DailyBusinessPerformanceMonitoring_SalesAndRecoveryStatusByUnit.Where(s => s.YearMonth == yearMonth && s.LocationCode == locationCode).FirstOrDefault();
        }


        public bool IsAuditAdjustmentObservationOnSalesAgreementExistOrNot(string locationCode, string customerCode)
        {
            bool isExistOrNot = true;

            Aud_AuditAdjustmentObservationOnSalesAgreement objObservationOnSalesAgreement = new Aud_AuditAdjustmentObservationOnSalesAgreement();
            objObservationOnSalesAgreement = context.Aud_AuditAdjustmentObservationOnSalesAgreement.Where(a => a.LocationCode == locationCode && a.CustomerCode == customerCode).FirstOrDefault();

            if (objObservationOnSalesAgreement == null)
            {
                isExistOrNot = false;
            }

            return isExistOrNot;
        }

        public List<Inv_VendorInfo> GetVendorListForItemSummary()
        {
            return context.Inv_VendorInfo.ToList();
        }

        public ArrayList GetVendorList(string itemTransactionType)
        {
            string vendorQuery = string.Format("SELECT vi.* " +
                                               "FROM Inv_Validation_ItemTransactionTypeVsVendorID vit " +
                                               "INNER JOIN Inv_VendorInfo vi ON vi.vendorID = vit.vendorID " +
                                               "WHERE ItemTransType = '{0}' ", itemTransactionType);

            List<Inv_VendorInfo> lstVenodr = new List<Inv_VendorInfo>();
            lstVenodr = context.Database.SqlQuery<Inv_VendorInfo>(vendorQuery).ToList();

            ArrayList vendorList = new ArrayList();

            foreach (Inv_VendorInfo v in lstVenodr)
            {
                vendorList.Add(new { VendorID = v.VendorID, VendorName = v.VendorName });
            }

            return vendorList;
        }

        public List<FixedAssetSerialList> GetCashMemoSerialList(byte storeLocation, string locationCode, string itemCode, string option)
        {
            string selectOption = string.Empty;

            if (string.IsNullOrEmpty(option))
            {
                selectOption = "FORUNASSIGN";
            }
            else
            {
                selectOption = "VIEWASSIGNANDUNASSIGN";
            }

            SqlParameter[] storeParam = new SqlParameter[6];
            storeParam[0] = new SqlParameter("Option", selectOption);
            storeParam[1] = new SqlParameter("prmStoreLocation", storeLocation);
            storeParam[2] = new SqlParameter("LocationCode", locationCode);
            storeParam[3] = new SqlParameter("prmItemCode", itemCode);
            storeParam[4] = new SqlParameter("EmployeeID", "");
            storeParam[5] = new SqlParameter("prmAvailableToAssignQuantity", "");

            var resultList = context.Database.SqlQuery<FixedAssetSerialList>("Exec SP_GetLocationWiseEmployeeListToAssignUnassignOrView @Option, @prmStoreLocation, @LocationCode, @prmItemCode, @EmployeeID, @prmAvailableToAssignQuantity", storeParam);

            return resultList.ToList();

            //var assignCashMemoSerials = (from empwfastaloc in context.Fix_EmployeeWiseFixedAssetsAllocation
            //                             join empwfastalocwsrl in context.Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo
            //                             on new { empwfastaloc.StoreLocation, empwfastaloc.LocationCode, empwfastaloc.ItemCode, empwfastaloc.EmployeeID, empwfastaloc.AllocationSeqNo }
            //                             equals new { empwfastalocwsrl.StoreLocation, empwfastalocwsrl.LocationCode, empwfastalocwsrl.ItemCode, empwfastalocwsrl.EmployeeID, empwfastalocwsrl.AllocationSeqNo }
            //                             into FixedAsetSerials
            //                             from fastsrl in FixedAsetSerials.DefaultIfEmpty()
            //                             join cmbm in context.Common_CashMemoBookMaster
            //                             on fastsrl.ItemSerialNo equals cmbm.CashMemoBookNo
            //                             into cmbmLeftJoin
            //                             from cmbms in cmbmLeftJoin.DefaultIfEmpty()
            //                             where fastsrl.StoreLocation == storeLocation &&
            //                                 fastsrl.LocationCode == locationCode &&
            //                                 fastsrl.ItemCode == itemCode &&
            //                                 fastsrl.IsItAllocated == true &&
            //                                 fastsrl.Status == Helper.Active &&
            //                                 cmbms.IsAssignedForUse == true &&
            //                                 cmbms.Status == Helper.Active
            //                             select new FixedAssetSerialList
            //                             {
            //                                 AllocationSeqNo = empwfastaloc.AllocationSeqNo,
            //                                 ItemCode = fastsrl.ItemCode,
            //                                 ItemSerialNo = fastsrl.ItemSerialNo,
            //                                 AllocationDate = empwfastaloc.AllocationDate,
            //                                 EmployeeID = empwfastaloc.EmployeeID,
            //                                 Remarks = empwfastaloc.Remarks
            //                             }).ToList();

            //var unAssignedCashMemoSerials = (from iswsnbl in context.Inv_ItemStockWithSerialNoByLocation
            //                                 join cmbm in context.Common_CashMemoBookMaster
            //                                 on iswsnbl.ItemSerialNo equals cmbm.CashMemoBookNo
            //                                 where iswsnbl.StoreLocation == storeLocation &&
            //                                       iswsnbl.LocationCode == locationCode &&
            //                                       iswsnbl.ItemCode == itemCode &&
            //                                       iswsnbl.IsAvailableInStore == true &&
            //                                       cmbm.IsAssignedForUse == false &&
            //                                       cmbm.Status == Helper.Active
            //                                 select new FixedAssetSerialList
            //                                 {
            //                                     AllocationSeqNo = null,
            //                                     ItemCode = iswsnbl.ItemCode,
            //                                     ItemSerialNo = iswsnbl.ItemSerialNo,
            //                                     AllocationDate = null,
            //                                     EmployeeID = null,
            //                                     Remarks = null
            //                                 }
            //                                 ).ToList();


            //var serialList = (assignCashMemoSerials.Union(unAssignedCashMemoSerials)).ToList();

            //var serialList = (from itmstcwsrlnbloc in context.Inv_ItemStockWithSerialNoByLocation
            //                  join o in
            //                      (from empwfastaloc in context.Fix_EmployeeWiseFixedAssetsAllocation
            //                       join empwfastalocwsrl in context.Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo
            //                       on new { empwfastaloc.StoreLocation, empwfastaloc.LocationCode, empwfastaloc.ItemCode, empwfastaloc.EmployeeID, empwfastaloc.AllocationSeqNo }
            //                       equals new { empwfastalocwsrl.StoreLocation, empwfastalocwsrl.LocationCode, empwfastalocwsrl.ItemCode, empwfastalocwsrl.EmployeeID, empwfastalocwsrl.AllocationSeqNo }
            //                       into FixedAsetSerials
            //                       from fastsrl in FixedAsetSerials.DefaultIfEmpty()
            //                       join cmbm in context.Common_CashMemoBookMaster
            //                       on fastsrl.ItemSerialNo equals cmbm.CashMemoBookNo
            //                       into cmbmLeftJoin
            //                       from cmbms in cmbmLeftJoin.DefaultIfEmpty()
            //                       where fastsrl.StoreLocation == storeLocation &&
            //                           fastsrl.LocationCode == locationCode &&
            //                           fastsrl.ItemCode == itemCode &&
            //                           fastsrl.IsItAllocated == true &&
            //                           fastsrl.Status == Helper.Active &&
            //                           cmbms.IsAssignedForUse == true &&
            //                           cmbms.Status == Helper.Active
            //                       select new
            //                       {
            //                           empwfastaloc.AllocationSeqNo,
            //                           empwfastaloc.StoreLocation,
            //                           empwfastaloc.LocationCode,
            //                           fastsrl.ItemCode,
            //                           empwfastaloc.EmployeeID,
            //                           fastsrl.ItemSerialNo,
            //                           empwfastaloc.AllocationDate,
            //                           empwfastaloc.Remarks
            //                       })
            //                 on new { itmstcwsrlnbloc.StoreLocation, itmstcwsrlnbloc.LocationCode, itmstcwsrlnbloc.ItemCode, itmstcwsrlnbloc.ItemSerialNo }
            //                 equals new { o.StoreLocation, o.LocationCode, o.ItemCode, o.ItemSerialNo }
            //                 into seriallist
            //                  from srl in seriallist.DefaultIfEmpty()
            //                  where itmstcwsrlnbloc.StoreLocation == storeLocation &&
            //                         itmstcwsrlnbloc.LocationCode == locationCode &&
            //                         itmstcwsrlnbloc.ItemCode == itemCode
            //                  select new FixedAssetSerialList
            //                  {
            //                      AllocationSeqNo = srl.AllocationSeqNo,
            //                      ItemSerialNo = itmstcwsrlnbloc.ItemSerialNo,
            //                      EmployeeID = srl.EmployeeID,
            //                      AllocationDate = srl.AllocationDate,
            //                      ItemCode = srl.ItemCode,
            //                      Remarks = srl.Remarks
            //                  });

            //return serialList.ToList();
        }

        public List<CollectionSheetForCustomerFPR> ReadCollectionSheetForCustomerFPR(string customerFPR, string locationCode)
        {
            try
            {
                SqlParameter[] storeParam = new SqlParameter[2];
                storeParam[0] = new SqlParameter("prmUnitCode", locationCode);
                storeParam[1] = new SqlParameter("prmEmployeeID", customerFPR);

                var resultList = context.Database.SqlQuery<CollectionSheetForCustomerFPR>("Exec REP_GetUnitWiseMonthlyCollectionSheet @prmUnitCode, @prmEmployeeID", storeParam);

                return resultList.ToList();
            }
            catch
            {
                throw;
            }

        }

        public List<CashMemoBookPagesStatus> GetStatusForCashMemoBookAllocation(string itemSerialNo)
        {
            //string sqlQuery = "SELECT cmm.CashMemoBookNo CashMemoBookNo,cmm.CashMemoNo CashMemoNo " +
            //                   ",CASE " +
            //                   "WHEN cmm.[Status] = 1 THEN 'InActive' " +
            //                   "WHEN cmm.[Status] = 0 AND cmm.IsAvailableToUse = 1 THEN 'Available to  Use' " +
            //                   "WHEN cmm.[Status] = 0 AND cmm.IsAvailableToUse = 0 THEN 'Already Used' " +
            //                   "END [Status] " +
            //                   ",cfc.CustomerCode " +
            //                   ",CASE " +
            //                   "WHEN cfc.CollectionType = 'DPA' THEN 'DP Collection' " +
            //                   "WHEN cfc.CollectionType = 'CSA' THEN 'Cash Collection' " +
            //                   "WHEN cfc.CollectionType = 'IOC' THEN 'Installment Collection' " +
            //                   "WHEN cfc.CollectionType = 'DRF' THEN 'DRF Collection' " +
            //                   "WHEN cfc.CollectionType = 'SAC' THEN 'Service Agreement Collection' " +
            //                   "WHEN sps.ItemTransTypeID = 'ISSFSPSALE' THEN 'Spare Parts Sales' " +
            //                   "END [Remarks] " +
            //                   "FROM [RASolarERP].[dbo].[Common_CashMemoMaster] cmm " +
            //                   "LEFT OUTER JOIN Sal_CollectionFromCustomers cfc ON cmm.CashMemoNo = cfc.CashMemoNo " +
            //                   "LEFT OUTER JOIN Sal_SparePartsSalesMaster sps ON cmm.CashMemoNo = sps.CashMemoNo " +
            //                   "WHERE cmm.CashMemoBookNo = '" + itemSerialNo + "' " +
            //                   "ORDER BY cmm.CashMemoNo";

            string sqlQuery = "SELECT cmm.CashMemoBookNo CashMemoBookNo,cmm.CashMemoNo CashMemoNo , "
                              +"    CASE WHEN cmm.[Status] = 1 THEN 'InActive' WHEN cmm.[Status] = 0 AND cmm.IsAvailableToUse = 1 THEN 'Available to  Use' WHEN cmm.[Status] = 0 AND cmm.IsAvailableToUse = 0 THEN 'Already Used' END [Status] ,cfc.CustomerCode , "
                              +"    CASE WHEN cfc.CollectionType IN ('DPA','CSA','IOC','DRF','SAC') THEN cfc.CollectionAmount WHEN sps.ItemTransTypeID = 'ISSFSPSALE' THEN sps.SalesAmountAfterDiscount END [Amount], "
                              +"   CASE WHEN cfc.CollectionType = 'DPA' THEN 'DP Collection' WHEN cfc.CollectionType = 'CSA' THEN 'Cash Collection' WHEN cfc.CollectionType = 'IOC' THEN 'Installment Collection' WHEN cfc.CollectionType = 'DRF' THEN 'DRF Collection' WHEN cfc.CollectionType = 'SAC' THEN 'Service Agreement Collection' WHEN sps.ItemTransTypeID = 'ISSFSPSALE' THEN 'Spare Parts Sales' END [Remarks] "
                              +"         FROM [RASolarERP].[dbo].[Common_CashMemoMaster] cmm  "
                              +"         LEFT OUTER JOIN Sal_CollectionFromCustomers cfc ON cmm.CashMemoNo = cfc.CashMemoNo "
                              +"          LEFT OUTER JOIN Sal_SparePartsSalesMaster sps ON cmm.CashMemoNo = sps.CashMemoNo WHERE cmm.CashMemoBookNo = '"+itemSerialNo+"' "
                              +"  ORDER BY cmm.CashMemoNo ";

            var results = context.Database.SqlQuery<CashMemoBookPagesStatus>(sqlQuery);

            return results.ToList();
        }

        // --------------------------- Special Package Sales

        public PackagePricingDetailsForSalesAgreement ReadPackagePricingDetailsForSalesAgreement(string locationCode, string programCode, string salesReSalesOrBoth, string customerType, string packageCapacity, string lightID, string packageCode, string modeOfPaymentID, string changedDownPaymentAmount)
        {
            try
            {
                //            @prmLocationCode	NCHAR(6),
                //@prmProgramCode		NCHAR(6),
                //@prmSalesReSalesOrBoth NCHAR(1),
                //@prmCustomerType	NCHAR(6),
                //@prmPackageCapacity NCHAR(6),
                //@prmLightID			NCHAR(7),
                //@prmPackageCode		NCHAR(6),
                //@prmModeOfPaymentID	NCHAR(6)	

                SqlParameter[] storeParam = new SqlParameter[9];
                storeParam[0] = new SqlParameter("prmLocationCode", locationCode);
                storeParam[1] = new SqlParameter("prmProgramCode", programCode);
                storeParam[2] = new SqlParameter("prmSalesReSalesOrBoth", salesReSalesOrBoth);
                storeParam[3] = new SqlParameter("prmCustomerType", customerType);
                storeParam[4] = new SqlParameter("prmPackageCapacity", packageCapacity);
                storeParam[5] = new SqlParameter("prmLightID", lightID);
                storeParam[6] = new SqlParameter("prmPackageCode", packageCode);
                storeParam[7] = new SqlParameter("prmModeOfPaymentID", modeOfPaymentID);
                storeParam[8] = new SqlParameter("prmChangedDownPaymentAmount", changedDownPaymentAmount);

                var resultList = context.Database.SqlQuery<PackagePricingDetailsForSalesAgreement>("EXEC SP_SalGetPackagePricingDetailsForSalesAgreement @prmLocationCode, @prmProgramCode, @prmSalesReSalesOrBoth, @prmCustomerType, @prmPackageCapacity, @prmLightID, @prmPackageCode, @prmModeOfPaymentID, @prmChangedDownPaymentAmount", storeParam);

                return resultList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ArrayList ReadModeOfPaymentForSpecialPackageSales(string salesReSalesOrBoth)
        {
            string modeOfPaymentQuery = string.Format("SELECT * FROM Sal_ModeOfPayment " +
                                                      "WHERE ModeOfPaymentID IN ( SELECT DISTINCT ModeOfPaymentID FROM Sal_Validation_ForSpecialPackageSales WHERE SalesReSalesOrBoth IN({0})) ", salesReSalesOrBoth);


            var modeOfPayment = context.Database.SqlQuery<Sal_ModeOfPayment>(modeOfPaymentQuery);

            ArrayList arr = new ArrayList();
            foreach (Sal_ModeOfPayment ss in modeOfPayment)
            {
                arr.Add(new { Value = ss.ModeOfPaymentID, Display = ss.Description });
            }

            return arr;
        }

        public ArrayList ReadSpecialPackageListForSales(string packageCapacityId, string lightId, string programCode, string salesReSalesOrBoth)
        {
            //var v = (from spfs in context.Sal_Validation_ForSpecialPackageSales
            //         where spfs.PackageCapacity == packageCapacityId &&
            //         spfs.LightID == lightId &&
            //         spfs.SalesReSalesOrBoth == salesReSalesOrBoth &&
            //         spfs.ProgramCode == programCode
            //         select spfs).ToList();

            //List<string> ssp = new List<string>();
            //ssp = v.Select(s => s.PackageCode).ToList();

            //var pack = (from s in context.Sal_PackageMaster
            //            where
            //            s.Capacity == packageCapacityId &&
            //            s.Light == lightId
            //            select s);

            var packageSpecialQuery = string.Format("Select * From Sal_PackageMaster " +
                                                    "Where PackageCode In ( " +
                                                    "   Select PackageCode From Sal_Validation_ForSpecialPackageSales " +
                                                    "   Where PackageCapacity = '{0}' And LightID = '{1}' And SalesReSalesOrBoth = '{2}' And ProgramCode = '{3}' " +
                                                     ")" +
                                                     "And Capacity = '{0}' And Light = '{1}'", packageCapacityId, lightId, salesReSalesOrBoth, programCode);

            var packageMaster = context.Database.SqlQuery<Sal_PackageMaster>(packageSpecialQuery);

            ArrayList arr = new ArrayList();
            foreach (Sal_PackageMaster ss in packageMaster)
            {
                arr.Add(new { PackageCode = ss.PackageCode, PackageName = ss.Description });
            }

            return arr;
        }

        public CustomerDetails GetCustomerDetailsForWarrantyClaim(string customerCode, string unitCode)
        {
            var v = from c in context.Sal_Customer
                    join a in context.Sal_SalesAgreement
                    on c.CustomerCode equals a.CustomerCode
                    join p in context.Sal_PackageMaster
                    on a.PackageCode equals p.PackageCode
                    where c.CustomerCode == customerCode
                    && c.UnitCode == unitCode
                    select new CustomerDetails
                    {
                        CustomerCode = c.CustomerCode,
                        CustomerName = c.CustomerName,
                        AgreementDate = a.AgreementDate,
                        Package = p.Description
                    };

            return v.FirstOrDefault();
        }

        public List<WarrantyItemsDetails> GetWarrantyItemsList(string customerCode, DateTime dayOpenDate)
        {
            SqlParameter[] storeParam = new SqlParameter[2];
            storeParam[0] = new SqlParameter("prmCustomerCode", customerCode);
            storeParam[1] = new SqlParameter("prmDayOpenDate", dayOpenDate);

            var resultList = context.Database.SqlQuery<WarrantyItemsDetails>("EXEC REP_InvGetWarrantyItemAtCustomerEnd @prmCustomerCode, @prmDayOpenDate", storeParam);

            return resultList.ToList();
        }

        public List<DailyPerformanceMonitoringForSales> ReadDailyPerformanceMonitoringForSales(string reportOption, string locationCode)
        {
            SqlParameter[] storeParam = new SqlParameter[2];
            storeParam[0] = new SqlParameter("prmReportType", reportOption);
            storeParam[1] = new SqlParameter("prmLocationCode", locationCode);

            var resultList = context.Database.SqlQuery<DailyPerformanceMonitoringForSales>("EXEC REP_DailyBusinessPerformanceMonitoring_Sales @prmReportType, @prmLocationCode", storeParam);

            return resultList.ToList();
        }

        public List<DailyBusinessPerformanceMonitoringCollection> ReadDailyBusinessPerformanceMonitoringCollection(string reportOption, string locationCode)
        {
            SqlParameter[] storeParam = new SqlParameter[2];
            storeParam[0] = new SqlParameter("prmReportType", reportOption);
            storeParam[1] = new SqlParameter("prmLocationCode", locationCode);

            var resultList = context.Database.SqlQuery<DailyBusinessPerformanceMonitoringCollection>("EXEC REP_DailyBusinessPerformanceMonitoring_Collection @prmReportType, @prmLocationCode", storeParam);

            return resultList.ToList();
        }

        public List<ItemSerialCorrection> ReadItemSerialCorrectionInformation(string locationCode, string itemCode, string optionForCorrection, string wrongSerialNo, string correctSerialNo, string loginID)
        {
            SqlParameter[] storeParam = new SqlParameter[6];
            storeParam[0] = new SqlParameter("@prmOption", optionForCorrection);
            storeParam[1] = new SqlParameter("@prmLocationCode", locationCode);
            storeParam[2] = new SqlParameter("@prmItemCode", itemCode);
            storeParam[3] = new SqlParameter("@prmWrongSerialNoToReplace", wrongSerialNo);
            storeParam[4] = new SqlParameter("@prmCorrectSerialNo", correctSerialNo);
            storeParam[5] = new SqlParameter("@prmUserID", loginID);

            var lstItemSerialCorrection = context.Database.SqlQuery<ItemSerialCorrection>("Exec Support_CorrectWrongItemSerialNo @prmOption, @prmLocationCode, @prmItemCode, @prmWrongSerialNoToReplace, @prmCorrectSerialNo, @prmUserID", storeParam);
            return lstItemSerialCorrection.ToList();
        }

        public List<DailyBusinessPerformanceMonitoringNetODIncreaseOrDecreaseComperison> ReadDailyBusinessPerformanceMonitoringIncreaseOrDecreaseComperison(string reportOption, string locationCode)
        {
            SqlParameter[] storeParam = new SqlParameter[2];
            storeParam[0] = new SqlParameter("prmReportType", reportOption);
            storeParam[1] = new SqlParameter("prmLocationCode", locationCode);

            var resultList = context.Database.SqlQuery<DailyBusinessPerformanceMonitoringNetODIncreaseOrDecreaseComperison>("EXEC REP_DailyBusinessPerformanceMonitoring_NetODIncreaseOrDecreaseComperison @prmReportType, @prmLocationCode", storeParam);

            return resultList.ToList();
        }

        public List<DailyBusinessPerformanceMonitoringSalesNCollectionTargetVsAchievement> ReadDailyBusinessPerformanceMonitoringSalesNCollectionTargetVsAchievement(string reportOption, string locationCode, DateTime dateForTheStatus)
        {
            SqlParameter[] storeParam = new SqlParameter[3];
            storeParam[0] = new SqlParameter("prmReportType", reportOption);
            storeParam[1] = new SqlParameter("prmLocationCode", locationCode);
            storeParam[2] = new SqlParameter("prmDateForTheStatus", dateForTheStatus.Date);

            var resultList = context.Database.SqlQuery<DailyBusinessPerformanceMonitoringSalesNCollectionTargetVsAchievement>("EXEC REP_DailyBusinessPerformanceMonitoring_SalesNCollectionTargetVsAchievement @prmReportType, @prmLocationCode, @prmDateForTheStatus", storeParam);

            return resultList.ToList();
        }

        public List<DailyBusinessPerformanceMonitoringOtherStatus> ReadDailyBusinessPerformanceMonitoringOtherStatus(string reportOption, string locationCode)
        {
            SqlParameter[] storeParam = new SqlParameter[2];
            storeParam[0] = new SqlParameter("prmReportType", reportOption);
            storeParam[1] = new SqlParameter("prmLocationCode", locationCode);

            var resultList = context.Database.SqlQuery<DailyBusinessPerformanceMonitoringOtherStatus>("EXEC REP_DailyBusinessPerformanceMonitoring_OtherStatus @prmReportType, @prmLocationCode", storeParam);

            return resultList.ToList();
        }


        public List<DailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus> ReadDailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus(string reportOption, string locationCode)
        {
            SqlParameter[] storeParam = new SqlParameter[2];
            storeParam[0] = new SqlParameter("prmReportType", reportOption);
            storeParam[1] = new SqlParameter("prmLocationCode", locationCode);

            var resultList = context.Database.SqlQuery<DailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus>("EXEC REP_DailyBusinessPerformanceMonitoring_SystemReturnVsResaleNDRFCollectionStatus @prmReportType, @prmLocationCode", storeParam);

            return resultList.ToList();
        }


        public List<WarrantyClaimReason> GetWarrantyClaimReason()
        {
            string claimReasonList = "SELECT ReasonCode, ReasonDescription FROM Common_ReasonCodeMaster";

            return context.Database.SqlQuery<WarrantyClaimReason>(claimReasonList).ToList();
        }

        public string CheckingIssueItemSerailNO(string locationCode, string itemSerial, string itemCode, string storeLocationForIssue)
        {
            byte storeLocation = Convert.ToByte(storeLocationForIssue);

            Inv_ItemStockWithSerialNoByLocation objItemStockWithSerialNoByLocation = new Inv_ItemStockWithSerialNoByLocation();
            objItemStockWithSerialNoByLocation = context.Inv_ItemStockWithSerialNoByLocation.Where(s => s.LocationCode == locationCode && s.StoreLocation == storeLocation && s.ItemSerialNo == itemSerial && s.ItemCode == itemCode).FirstOrDefault();

            if (objItemStockWithSerialNoByLocation == null)
            {
                return "0";
            }
            else
            {
                return "1";
            }
        }

        public List<ItemInfo> GetSubstituteItemCode(string itemCode)
        {
            string sqlQuery = "SELECT iim.ItemCode,iim.ItemName FROM Inv_Validation_SubstituteItemNMapping ivs " +
                              "right join Inv_ItemMaster iim on ivs.SubstituteItemCode = iim.ItemCode " +
                              "where ivs.ItemCode = '" + itemCode + "' ";

            var results = context.Database.SqlQuery<ItemInfo>(sqlQuery);

            return results.ToList();
        }

        #endregion

        #region Create

        //public Common_InventoryTransaction Create(Common_InventoryTransaction objInventoryTranscation)
        //{
        //    context.Common_InventoryTransaction(objInventoryTranscation);
        //    context.SaveChanges();

        //    return objInventoryTranscation;
        //}

        //public Aud_AuditingMaster Create(Aud_AuditingMaster objAuditingMaster)
        //{
        //    context.Aud_AuditingMaster(objAuditingMaster);
        //    context.SaveChanges();

        //    return objAuditingMaster;
        //}

        //public Common_UnitWiseCustomerStatus Create(Common_UnitWiseCustomerStatus objUnitWiseCustomerStatus)
        //{
        //    context.Common_UnitWiseCustomerStatus(objUnitWiseCustomerStatus);
        //    context.SaveChanges();

        //    return objUnitWiseCustomerStatus;
        //}

        public Sal_LocationNEmployeeWiseActivityMonthly Create(Sal_LocationNEmployeeWiseActivityMonthly objLocationNEmployeeWiseActivityMonthlyCreate)
        {
            context.Sal_LocationNEmployeeWiseActivityMonthly.Add(objLocationNEmployeeWiseActivityMonthlyCreate);
            context.SaveChanges();

            return objLocationNEmployeeWiseActivityMonthlyCreate;
        }

        public Sal_LocationNEmployeeWiseActivityDaily Create(Sal_LocationNEmployeeWiseActivityDaily objLocationNEmployeeWiseActivityDailyCreate)
        {
            context.Sal_LocationNEmployeeWiseActivityDaily.Add(objLocationNEmployeeWiseActivityDailyCreate);
            context.SaveChanges();
            return objLocationNEmployeeWiseActivityDailyCreate;
        }

        //public Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement Create(List<Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement> objLocationNEmployeeWiseWeeklySalesNCollectionAchievement)
        //{
        //    try
        //    {
        //        foreach (Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement lwet in objLocationNEmployeeWiseWeeklySalesNCollectionAchievement)
        //        {
        //            context.Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement.Add(lwet);
        //        }

        //        context.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }

        //    return objLocationNEmployeeWiseWeeklySalesNCollectionAchievement.FirstOrDefault();
        //}

        //public Sal_LocationWiseWeeklySalesNCollectionSummary Create(Sal_LocationWiseWeeklySalesNCollectionSummary objSal_LocationWiseWeeklySalesNCollectionSummary)
        //{
        //    context.Sal_LocationWiseWeeklySalesNCollectionSummary.Add(objSal_LocationWiseWeeklySalesNCollectionSummary);
        //    context.SaveChanges();

        //    return objSal_LocationWiseWeeklySalesNCollectionSummary;
        //}

        //Test Basis

        public Sal_SalesAgreementPrePost Create(Sal_CustomerPrePost objCustomer, Sal_SalesAgreementPrePost objSalesAgreement, List<Sal_SalesItemsPrePost> lstSalesItem, List<Sal_SalesItemsWithSerialNoPrePost> lstItemSalesWithSerialNo)
        {
            using (context)
            {
                try
                {
                    string cashMemoId = string.Empty;
                    //if (string.IsNullOrEmpty(objSalesAgreement.RefMemoNo))
                    //{
                    //    SqlParameter[] storeParamForCashmemo = new SqlParameter[6];
                    //    storeParamForCashmemo[0] = new SqlParameter("prmEntrySource", "COLCRECEIV");
                    //    storeParamForCashmemo[1] = new SqlParameter("prmCashMemoNo", objSalesAgreement.CashMemoNo);
                    //    storeParamForCashmemo[2] = new SqlParameter("prmCashMemoUsesID", Helper.CashMemuUsesIdFirst);
                    //    storeParamForCashmemo[3] = new SqlParameter("prmLocationCode", objCustomer.UnitCode);
                    //    storeParamForCashmemo[4] = new SqlParameter("prmRefDocNo", objSalesAgreement.CustomerCode);
                    //    storeParamForCashmemo[5] = new SqlParameter("prmCashMemoUsedByEmployee", objSalesAgreement.SalespersonCode);

                    //    var v = context.Database.SqlQuery<string>("SP_CheckNSaveCashMemoNoInCashMemoMasterNTransactionRegisterTables @prmEntrySource, @prmCashMemoNo, @prmCashMemoUsesID, @prmLocationCode, @prmRefDocNo, @prmCashMemoUsedByEmployee", storeParamForCashmemo);

                    //    if (v != null)
                    //        cashMemoId = v.FirstOrDefault().ToString();

                    //}

                    objSalesAgreement.CashMemoUsesID = cashMemoId;

                    context.Sal_CustomerPrePost.Add(objCustomer);
                    context.Sal_SalesAgreementPrePost.Add(objSalesAgreement);

                    foreach (Sal_SalesItemsPrePost sit in lstSalesItem)
                    {
                        context.Sal_SalesItemsPrePost.Add(sit);
                    }

                    foreach (Sal_SalesItemsWithSerialNoPrePost sits in lstItemSalesWithSerialNo)
                    {
                        context.Sal_SalesItemsWithSerialNoPrePost.Add(sits);
                    }

                    context.SaveChanges();

                    



                    SqlParameter[] storeParam = new SqlParameter[2];
                    storeParam[0] = new SqlParameter("prmCustomerCode", objCustomer.CustomerCode);
                    storeParam[1] = new SqlParameter("prmDBTransType", Helper.Insert);

                    context.Database.ExecuteSqlCommand("Exec AE_Sal_SalesAgreementSubmitV3 @prmCustomerCode, @prmDBTransType", storeParam);

                    //

                    context.Database.ExecuteSqlCommand(" UPDATE Sal_SalesAgreementPrePost_DataFromExternalSources SET IsTransferredToFinalTable = 1 WHERE CashMemoNo = '" + objSalesAgreement.CashMemoNo + "' AND ISNULL(IsTransferredToFinalTable,0) = 0 ");//A2445758
                    //

                    // context.SalesAgreementSubmitV2(objCustomer.CustomerCode, Helper.Insert);

                }
                catch (Exception ex)
                {
                    throw;
                }

                return objSalesAgreement;
            }
        }

        public Sal_SalesAgreement Create(Sal_Customer objCustomer, Sal_SalesAgreement objSalesAgreement, Sal_CustomerStatus objCustomerStatus, List<Sal_SalesItems> lstSalesItem, List<Sal_SalesItemsWithSerialNo> lstItemSalesWithSerialNo, int customerCurrentSerial, string serialTempTableRows)
        {
            Common_UnitInfo objUnit = new Common_UnitInfo();

            using (context)
            {
                using (var tx = new TransactionScope())
                {
                    try
                    {
                        string tempTableInsertQuery = "IF OBJECT_ID('tempdb..#TempItemNItemCategoryWithSerialNo') IS NOT NULL " +
                                                    "BEGIN " +
                                                    "	DROP TABLE #TempItemNItemCategoryWithSerialNo " +
                                                    " END " +

                                                    " CREATE TABLE #TempItemNItemCategoryWithSerialNo( " +
                                                    " ItemCode NCHAR(4), ItemSerialNo NVARCHAR(25), ItemCategory NCHAR(6), " +
                                                    " StoreLocation TINYINT, LocationCode NCHAR(6), CustomerCode  NCHAR(9)) " +

                                                    " INSERT INTO  #TempItemNItemCategoryWithSerialNo (ItemCode, ItemSerialNo, ItemCategory,StoreLocation,LocationCode, CustomerCode) VALUES " + serialTempTableRows +

                                                    " EXEC SP_CheckNSaveSerialNoInItemNItemCategoryWithSerialNoMasterTable 'SALESAGREEMENT', 'ISSINVSALE', '" + lstItemSalesWithSerialNo[0].RefLocationCode + "'";


                        context.Sal_Customer.Add(objCustomer);
                        context.SaveChanges();

                        context.Database.ExecuteSqlCommand(tempTableInsertQuery);

                        context.Sal_SalesAgreement.Add(objSalesAgreement);
                        context.Sal_CustomerStatus.Add(objCustomerStatus);

                        objUnit = context.Common_UnitInfo.Where(u => u.Unit_Code == objCustomer.UnitCode).FirstOrDefault();
                        objUnit.LastUsedCustomerSerial = Convert.ToInt16(customerCurrentSerial);

                        foreach (Sal_SalesItems sit in lstSalesItem)
                        {
                            context.Sal_SalesItems.Add(sit);
                        }

                        foreach (Sal_SalesItemsWithSerialNo sits in lstItemSalesWithSerialNo)
                        {
                            context.Sal_SalesItemsWithSerialNo.Add(sits);
                        }

                        //ObjectParameter[] objParam = new ObjectParameter[2];
                        //objParam[0] = new ObjectParameter("prmCustomerCode", objCustomer.CustomerCode);
                        //objParam[1] = new ObjectParameter("prmDBTransType", "INSERT");

                        context.SaveChanges();

                        //((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext.CommandTimeout = 3600;
                        context.SalesAgreementSubmitV2(objCustomer.CustomerCode, Helper.Insert);

                        tx.Complete();

                        //context.ExecuteFunction("SalesAgreementSubmitV2", objParam);
                        //context.SalesAgreementSubmitV2(objCustomer.CustomerCode, Helper.Insert);
                    }
                    catch (Exception ex)
                    {
                        tx.Dispose();
                        throw;
                    }
                }
                return objSalesAgreement;
            }
        }

        public Sal_CollectionFromCustomersPrePost Create(Sal_CollectionFromCustomersPrePost objCollectionFromCustomers, Sal_Customer objSalCustomerInfo)
        {
            using (context)
            {

                //using (var tx = new TransactionScope())
                //{
                try
                {
                    //string cashMemoId = string.Empty;

                    //if (string.IsNullOrEmpty(objCollectionFromCustomers.RefMemoNo))
                    //{
                    //    SqlParameter[] storeParamForCashmemo = new SqlParameter[6];
                    //    storeParamForCashmemo[0] = new SqlParameter("prmEntrySource", "COLCRECEIV");
                    //    storeParamForCashmemo[1] = new SqlParameter("prmCashMemoNo", objCollectionFromCustomers.CashMemoNo);
                    //    storeParamForCashmemo[2] = new SqlParameter("prmCashMemoUsesID", Helper.CashMemuUsesIdFirst);
                    //    storeParamForCashmemo[3] = new SqlParameter("prmLocationCode", objCollectionFromCustomers.RefAELocationCode);
                    //    storeParamForCashmemo[4] = new SqlParameter("prmRefDocNo", objCollectionFromCustomers.CustomerCode);
                    //    storeParamForCashmemo[5] = new SqlParameter("prmCashMemoUsedByEmployee", objCollectionFromCustomers.CollectedByEmployee);

                    //    var v = context.Database.SqlQuery<string>("SP_CheckNSaveCashMemoNoInCashMemoMasterNTransactionRegisterTables @prmEntrySource, @prmCashMemoNo, @prmCashMemoUsesID, @prmLocationCode, @prmRefDocNo, @prmCashMemoUsedByEmployee", storeParamForCashmemo);

                    //    if (v != null)
                    //        cashMemoId = v.FirstOrDefault().ToString();

                    //    objCollectionFromCustomers.RefAELocationCode = null;
                    //}

                    //objCollectionFromCustomers.CashMemoUsesID = cashMemoId;

                    context.Sal_CollectionFromCustomersPrePost.Add(objCollectionFromCustomers);

                    if (!String.IsNullOrEmpty(objSalCustomerInfo.CustomerCode))
                    {
                        Sal_Customer objSalCustomer = new Sal_Customer();
                        objSalCustomer = context.Sal_Customer.Where(c => c.CustomerCode == objSalCustomerInfo.CustomerCode).FirstOrDefault();
                        objSalCustomer.PhoneNo = objSalCustomerInfo.PhoneNo;
                    }
                    
                    context.SaveChanges();

                   


                    if (objCollectionFromCustomers.CollectionType == Helper.CollectionTypeDRF || objCollectionFromCustomers.CollectionType == Helper.CollectionTypeSAC)
                    {

                        SqlParameter[] storeParam = new SqlParameter[4];
                        storeParam[0] = new SqlParameter("prmCustomerCode", objCollectionFromCustomers.CustomerCode);
                        storeParam[1] = new SqlParameter("prmYearMonth", objCollectionFromCustomers.YearMonth);
                        storeParam[2] = new SqlParameter("prmEntrySerialNo", objCollectionFromCustomers.SerialNo);
                        storeParam[3] = new SqlParameter("prmDBTransType", "INSERT");

                        context.Database.ExecuteSqlCommand("AE_Sal_CollectionReceiveForOtherAmountV2 @prmCustomerCode, @prmYearMonth, @prmEntrySerialNo, @prmDBTransType", storeParam);
                    }
                    else
                    {
                        SqlParameter[] storeParam = new SqlParameter[4];
                        storeParam[0] = new SqlParameter("prmCustomerCode", objCollectionFromCustomers.CustomerCode);
                        storeParam[1] = new SqlParameter("prmYearMonth", objCollectionFromCustomers.YearMonth);
                        storeParam[2] = new SqlParameter("prmEntrySerialNo", objCollectionFromCustomers.SerialNo);
                        storeParam[3] = new SqlParameter("prmDBTransType", "INSERT");

                        context.Database.ExecuteSqlCommand("AE_Sal_CollectionReceiveV3 @prmCustomerCode, @prmYearMonth, @prmEntrySerialNo, @prmDBTransType", storeParam);
                    }

                    //tx.Complete();
                }
                catch (Exception ex)
                {
                    // tx.Dispose();
                    throw;
                }

                return objCollectionFromCustomers;
            }
        }

        public Sal_SalesItems Create(Sal_SalesItems objSalesItems)
        {
            context.Sal_SalesItems.Add(objSalesItems);
            context.SaveChanges();

            return objSalesItems;
        }

        public Sal_SalesItemsWithSerialNo Create(Sal_SalesItemsWithSerialNo objItemSalesWithSerialNo)
        {
            try
            {
                context.Sal_SalesItemsWithSerialNo.Add(objItemSalesWithSerialNo);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }

            return objItemSalesWithSerialNo;
        }

        public int Create(string insertedRows, string locationCode)
        {
            using (context)
            {

                using (TransactionScope Tx = new TransactionScope())
                {
                    try
                    {
                        string insertQuery = "IF OBJECT_ID('tempdb..#InvUpdateStockPosition') IS NOT NULL " +
                                             "BEGIN " +
                                             "   DROP TABLE #InvUpdateStockPosition " +
                                             "END " +

                                             "CREATE TABLE #InvUpdateStockPosition( " +
                                             "StoreLocation TINYINT, LocationCode NCHAR(6), ItemCode NCHAR(4), " +
                                             "ItemSerialNo NVARCHAR(25), ItemConditionOrStatus TINYINT, ItemCategory NCHAR(6) " +
                                             ") " +

                                             "INSERT INTO  #InvUpdateStockPosition (StoreLocation, LocationCode, ItemCode,ItemSerialNo,ItemConditionOrStatus,ItemCategory) VALUES " +
                                             insertedRows +
                                             " EXEC SP_InvUpdateStockPosition '" + locationCode + "'";

                        context.Database.ExecuteSqlCommand(insertQuery);
                        //context.InvUpdateStockPosition(locationCode);

                        Tx.Complete();
                    }
                    catch (Exception ex)
                    {
                        Tx.Dispose();
                        throw;
                    }
                }
                return 1;
            }
        }

        public Inv_ChallanMaster Create(Inv_ChallanMaster objChallanMaster, List<Inv_ChallanDetails> lstChallanDetails, List<Inv_ChallanDetailsWithSerialNo> lstChallanDetailsWithSerialNo)
        {
            using (context)
            {

                using (var tx = new TransactionScope())
                {
                    try
                    {
                        //string dbSequenceNumber = ChallanSequenceNumberMax(objChallanMaster.LocationCode);
                        //string sequenceNumber = string.Empty;

                        //if (!Helper.ChallanSequenceNumberDuplicationCheck(objChallanMaster.ChallanSeqNo, dbSequenceNumber))
                        //{
                        //    sequenceNumber = Helper.ChallanCequenceNumberGeneration(dbSequenceNumber);
                        //    objChallanMaster.ChallanSeqNo = sequenceNumber;

                        //    for (int i = 0; i < lstChallanDetails.Count; i++)
                        //    {
                        //        lstChallanDetails[i].ChallanSeqNo = sequenceNumber;
                        //    }

                        //    if (lstChallanDetailsWithSerialNo.Count > 0)
                        //    {
                        //        for (int i = 0; i < lstChallanDetailsWithSerialNo.Count; i++)
                        //        {
                        //            lstChallanDetailsWithSerialNo[i].ChallanSeqNo = sequenceNumber;
                        //        }
                        //    }
                        //}

                        context.Inv_ChallanMaster.Add(objChallanMaster);

                        foreach (Inv_ChallanDetails incd in lstChallanDetails)
                        {
                            context.Inv_ChallanDetails.Add(incd);
                        }

                        if (lstChallanDetailsWithSerialNo.Count > 0)
                        {
                            foreach (Inv_ChallanDetailsWithSerialNo incdws in lstChallanDetailsWithSerialNo)
                            {
                                context.Inv_ChallanDetailsWithSerialNo.Add(incdws);
                            }
                        }

                        context.SaveChanges();

                        context.InvGenerateItemTransactionForIssueInGeneral("CHALLANINVNTORY", objChallanMaster.LocationCode, objChallanMaster.ChallanSeqNo);

                        tx.Complete();
                    }
                    catch (Exception ex)
                    {
                        tx.Dispose();
                        throw;
                    }
                }

                return objChallanMaster;
            }
        }

        public Sal_SparePartsSalesMaster Create(Inv_ChallanMaster objChallanMaster, List<Inv_ChallanDetails> lstChallanDetails, List<Inv_ChallanDetailsWithSerialNo> lstChallanDetailsWithSerialNo, Sal_SparePartsSalesMaster objSparePartsSalesMaster, List<Sal_SparePartsSalesItems> lstSparePartsSalesItems, List<Sal_SparePartsSalesItemsWithSerialNo> lstSparePartsSalesItemsWithSerialNo)
        {
            using (context)
            {

                using (var tx = new TransactionScope())
                {
                    try
                    {
                        string cashMemoId = string.Empty;

                        if (string.IsNullOrEmpty(objSparePartsSalesMaster.RefMemoNo))
                        {
                            SqlParameter[] storeParamForCashmemo = new SqlParameter[6];
                            storeParamForCashmemo[0] = new SqlParameter("prmEntrySource", "COLCRECEIV");
                            storeParamForCashmemo[1] = new SqlParameter("prmCashMemoNo", objSparePartsSalesMaster.CashMemoNo);
                            storeParamForCashmemo[2] = new SqlParameter("prmCashMemoUsesID", Helper.CashMemuUsesIdFirst);
                            storeParamForCashmemo[3] = new SqlParameter("prmLocationCode", objSparePartsSalesMaster.LocationCode);
                            storeParamForCashmemo[4] = new SqlParameter("prmRefDocNo", objSparePartsSalesMaster.SPSSeqNo);
                            storeParamForCashmemo[5] = new SqlParameter("prmCashMemoUsedByEmployee", objSparePartsSalesMaster.SalespersonCode);

                            var v = context.Database.SqlQuery<string>("SP_CheckNSaveCashMemoNoInCashMemoMasterNTransactionRegisterTables @prmEntrySource, @prmCashMemoNo, @prmCashMemoUsesID, @prmLocationCode, @prmRefDocNo, @prmCashMemoUsedByEmployee", storeParamForCashmemo);

                            if (v != null)
                                cashMemoId = v.FirstOrDefault().ToString();

                        }

                        objSparePartsSalesMaster.CashMemoUsesID = cashMemoId;

                        context.Sal_SparePartsSalesMaster.Add(objSparePartsSalesMaster);

                        foreach (Sal_SparePartsSalesItems spsitm in lstSparePartsSalesItems)
                        {
                            context.Sal_SparePartsSalesItems.Add(spsitm);
                        }

                        if (lstSparePartsSalesItemsWithSerialNo.Count > 0)
                        {
                            foreach (Sal_SparePartsSalesItemsWithSerialNo spswsn in lstSparePartsSalesItemsWithSerialNo)
                            {
                                context.Sal_SparePartsSalesItemsWithSerialNo.Add(spswsn);
                            }
                        }

                        context.SaveChanges();
                        context.AESAL_SparePartsSalesV2(objSparePartsSalesMaster.LocationCode, objSparePartsSalesMaster.SPSSeqNo, "INSERT");

                        context.Inv_ChallanMaster.Add(objChallanMaster);

                        foreach (Inv_ChallanDetails incd in lstChallanDetails)
                        {
                            context.Inv_ChallanDetails.Add(incd);
                        }

                        if (lstChallanDetailsWithSerialNo.Count > 0)
                        {
                            foreach (Inv_ChallanDetailsWithSerialNo incdws in lstChallanDetailsWithSerialNo)
                            {
                                context.Inv_ChallanDetailsWithSerialNo.Add(incdws);
                            }
                        }

                        objSparePartsSalesMaster.ChallanSeqNo = objChallanMaster.ChallanSeqNo;
                        context.SaveChanges();

                        //Sal_SparePartsSalesMaster objUpdateSparePartsSalesMaster = new Sal_SparePartsSalesMaster();
                        // objUpdateSparePartsSalesMaster = context.Sal_SparePartsSalesMaster.whe

                        context.InvGenerateItemTransactionForIssueInGeneral("CHALLANINVNTORY", objChallanMaster.LocationCode, objChallanMaster.ChallanSeqNo);

                        tx.Complete();
                    }
                    catch (Exception ex)
                    {
                        tx.Dispose();
                        throw;
                    }
                }

                return objSparePartsSalesMaster;
            }
        }

        public Inv_ChallanMaster Create(Inv_ChallanMaster objChallanMaster, List<Inv_ChallanDetails> lstChallanDetails, List<Inv_ChallanDetailsWithSerialNo> lstChallanDetailsWithSerialNo, List<Inv_ChallanWithMultipleLocations> lstChallanWithMultipleLocations, List<Inv_ChallanWithMultipleLocationsWithItemWiseDistribution> lstChallanItemDistribution)
        {
            using (context)
            {

                using (var tx = new TransactionScope())
                {
                    try
                    {
                        string dbSequenceNumber = ChallanSequenceNumberMax(objChallanMaster.LocationCode, objChallanMaster.ChallanSeqNo.Substring(0, 6));
                        string sequenceNumber = string.Empty;

                        if (!string.IsNullOrEmpty(dbSequenceNumber))
                        {
                            if (!Helper.ChallanSequenceNumberDuplicationCheck(objChallanMaster.ChallanSeqNo, dbSequenceNumber))
                            {
                                sequenceNumber = Helper.ChallanCequenceNumberGeneration(dbSequenceNumber);
                                objChallanMaster.ChallanSeqNo = sequenceNumber;

                                for (int i = 0; i < lstChallanDetails.Count; i++)
                                {
                                    lstChallanDetails[i].ChallanSeqNo = sequenceNumber;
                                }

                                if (lstChallanDetailsWithSerialNo.Count > 0)
                                {
                                    for (int i = 0; i < lstChallanDetailsWithSerialNo.Count; i++)
                                    {
                                        lstChallanDetailsWithSerialNo[i].ChallanSeqNo = sequenceNumber;
                                    }
                                }

                                if (lstChallanWithMultipleLocations.Count > 0)
                                {
                                    for (int i = 0; i < lstChallanWithMultipleLocations.Count; i++)
                                    {
                                        lstChallanWithMultipleLocations[i].ChallanSeqNo = sequenceNumber;
                                    }
                                }

                                for (int i = 0; i < lstChallanItemDistribution.Count; i++)
                                {
                                    lstChallanItemDistribution[i].ChallanSeqNo = sequenceNumber;
                                }
                            }
                        }

                        context.Inv_ChallanMaster.Add(objChallanMaster);

                        foreach (Inv_ChallanDetails incd in lstChallanDetails)
                        {
                            context.Inv_ChallanDetails.Add(incd);
                        }

                        if (lstChallanDetailsWithSerialNo.Count > 0)
                        {
                            foreach (Inv_ChallanDetailsWithSerialNo incdws in lstChallanDetailsWithSerialNo)
                            {
                                context.Inv_ChallanDetailsWithSerialNo.Add(incdws);
                            }
                        }

                        if (lstChallanWithMultipleLocations.Count > 0)
                        {
                            foreach (Inv_ChallanWithMultipleLocations incwml in lstChallanWithMultipleLocations)
                            {
                                context.Inv_ChallanWithMultipleLocations.Add(incwml);
                            }
                        }

                        foreach (Inv_ChallanWithMultipleLocationsWithItemWiseDistribution objChallanItems in lstChallanItemDistribution)
                        {
                            context.Inv_ChallanWithMultipleLocationsWithItemWiseDistribution.Add(objChallanItems);
                        }

                        context.SaveChanges();

                        context.InvGenerateItemTransactionForIssueInGeneral("CHALLANINVNTORY", objChallanMaster.LocationCode, objChallanMaster.ChallanSeqNo);

                        tx.Complete();
                    }
                    catch (Exception ex)
                    {
                        tx.Dispose();
                        throw;
                    }
                }

                return objChallanMaster;
            }
        }

        public Inv_MRRMaster Create(Inv_MRRMaster objMRRMaster, List<Inv_MRRDetails> lstMRRDetails, List<Inv_MRRDetailsWithSerialNo> lstMRRDetailsWithSerialNo)
        {
            using (context)
            {

                using (TransactionScope tx = new TransactionScope())
                {
                    try
                    {
                        context.Inv_MRRMaster.Add(objMRRMaster);

                        foreach (Inv_MRRDetails itt in lstMRRDetails)
                        {
                            context.Inv_MRRDetails.Add(itt);

                            if (objMRRMaster.ChallanLocationCode != null)
                            {
                                Inv_ChallanDetails dbRecord = context.Inv_ChallanDetails.Where(c => c.LocationCode == objMRRMaster.ChallanLocationCode && c.ChallanSeqNo == objMRRMaster.ChallanSeqNo && c.ItemCode == itt.ItemCode).FirstOrDefault();

                                if (dbRecord.ReceivedByMRRQuantity == null)
                                    dbRecord.ReceivedByMRRQuantity = itt.ReceiveQuantity;
                                else
                                    dbRecord.ReceivedByMRRQuantity += itt.ReceiveQuantity;
                            }
                        }

                        if (lstMRRDetailsWithSerialNo.Count > 0)
                        {
                            foreach (Inv_MRRDetailsWithSerialNo ittws in lstMRRDetailsWithSerialNo)
                            {
                                context.Inv_MRRDetailsWithSerialNo.Add(ittws);
                            }
                        }

                        context.SaveChanges();

                        if (objMRRMaster.ChallanLocationCode == null)
                        {
                            context.InvGenerateItemTransactionForReceiveInGeneral(Helper.ReceiveRefDocTypeWithoutChallan, objMRRMaster.LocationCode, objMRRMaster.MRRSeqNo);
                        }
                        else
                        {
                            context.InvGenerateItemTransactionForReceiveInGeneral(Helper.ReceiveRefDocTypeWithChallan, objMRRMaster.LocationCode, objMRRMaster.MRRSeqNo);
                        }

                        tx.Complete();
                    }
                    catch (Exception ex)
                    {
                        tx.Dispose();
                        throw;
                    }
                }

                return objMRRMaster;
            }
        }

        public Inv_MRRMaster Create(Inv_MRRMaster objMRRMaster, List<Inv_MRRDetails> lstMRRDetails, List<Inv_MRRDetailsWithSerialNo> lstMRRDetailsWithSerialNo, string serialTempTableRows)
        {
            using (context)
            {

                using (TransactionScope tx = new TransactionScope())
                {
                    ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext.CommandTimeout = 3600;
                    string tempTableInsertQuery = "IF OBJECT_ID('tempdb..#TempItemNItemCategoryWithSerialNo') IS NOT NULL " +
                                                 "BEGIN " +
                                                 "	DROP TABLE #TempItemNItemCategoryWithSerialNo " +
                                                 " END " +

                                                 " CREATE TABLE #TempItemNItemCategoryWithSerialNo( " +
                                                 " ItemCode NCHAR(4), ItemSerialNo NVARCHAR(25), ItemCategory NCHAR(6), " +
                                                 " StoreLocation TINYINT, LocationCode NCHAR(6), CustomerCode  NCHAR(9)) " +

                                                 " INSERT INTO  #TempItemNItemCategoryWithSerialNo (ItemCode, ItemSerialNo, ItemCategory,StoreLocation,LocationCode, CustomerCode) VALUES " + serialTempTableRows +

                                                 " EXEC SP_CheckNSaveSerialNoInItemNItemCategoryWithSerialNoMasterTable 'MRRWITHOCHALLAN', '" + objMRRMaster.ItemTransTypeID + "', '" + objMRRMaster.LocationCode + "'";

                    try
                    {
                        if (!string.IsNullOrEmpty(serialTempTableRows.Trim()))
                            context.Database.ExecuteSqlCommand(tempTableInsertQuery);

                        context.Inv_MRRMaster.Add(objMRRMaster);

                        foreach (Inv_MRRDetails itt in lstMRRDetails)
                        {
                            context.Inv_MRRDetails.Add(itt);

                            if (objMRRMaster.ChallanLocationCode != null)
                            {
                                Inv_ChallanDetails dbRecord = context.Inv_ChallanDetails.Where(c => c.LocationCode == objMRRMaster.ChallanLocationCode && c.ChallanSeqNo == objMRRMaster.ChallanSeqNo && c.ItemCode == itt.ItemCode).FirstOrDefault();

                                if (dbRecord.ReceivedByMRRQuantity == null)
                                    dbRecord.ReceivedByMRRQuantity = itt.ReceiveQuantity;
                                else
                                    dbRecord.ReceivedByMRRQuantity += itt.ReceiveQuantity;
                            }
                        }

                        if (lstMRRDetailsWithSerialNo.Count > 0)
                        {
                            foreach (Inv_MRRDetailsWithSerialNo ittws in lstMRRDetailsWithSerialNo)
                            {
                                context.Inv_MRRDetailsWithSerialNo.Add(ittws);
                            }
                        }

                        context.SaveChanges();

                        if (objMRRMaster.ChallanLocationCode == null)
                        {
                            context.InvGenerateItemTransactionForReceiveInGeneral(Helper.ReceiveRefDocTypeWithoutChallan, objMRRMaster.LocationCode, objMRRMaster.MRRSeqNo);
                        }
                        else
                        {
                            context.InvGenerateItemTransactionForReceiveInGeneral(Helper.ReceiveRefDocTypeWithChallan, objMRRMaster.LocationCode, objMRRMaster.MRRSeqNo);
                        }

                        tx.Complete();
                    }
                    catch (Exception ex)
                    {
                        tx.Dispose();
                        throw;
                    }
                }

                return objMRRMaster;
            }
        }

        public Inv_MRRMaster Create(Inv_MRRMaster objMRRMaster, List<Inv_MRRDetails> lstMRRDetails, List<Inv_MRRDetailsWithSerialNo> lstMRRDetailsWithSerialNo, string serialTempTableRows, Aud_AuditAdjustmentRelatedChallanOrMRRForReference objChallanOrMRRForAuditAdjustment)
        {
            using (context)
            {

                using (TransactionScope tx = new TransactionScope())
                {
                    string tempTableInsertQuery = "IF OBJECT_ID('tempdb..#TempItemNItemCategoryWithSerialNo') IS NOT NULL " +
                                                 "BEGIN " +
                                                 "	DROP TABLE #TempItemNItemCategoryWithSerialNo " +
                                                 " END " +

                                                 " CREATE TABLE #TempItemNItemCategoryWithSerialNo( " +
                                                 " ItemCode NCHAR(4), ItemSerialNo NVARCHAR(25), ItemCategory NCHAR(6), " +
                                                 " StoreLocation TINYINT, LocationCode NCHAR(6), CustomerCode  NCHAR(9)) " +

                                                 " INSERT INTO  #TempItemNItemCategoryWithSerialNo (ItemCode, ItemSerialNo, ItemCategory,StoreLocation,LocationCode, CustomerCode) VALUES " + serialTempTableRows +

                                                 " EXEC SP_CheckNSaveSerialNoInItemNItemCategoryWithSerialNoMasterTable 'MRRWITHOCHALLAN', '" + objMRRMaster.ItemTransTypeID + "', '" + objMRRMaster.LocationCode + "'";

                    try
                    {
                        if (!string.IsNullOrEmpty(serialTempTableRows.Trim()))
                            context.Database.ExecuteSqlCommand(tempTableInsertQuery);

                        context.Inv_MRRMaster.Add(objMRRMaster);

                        foreach (Inv_MRRDetails itt in lstMRRDetails)
                        {
                            context.Inv_MRRDetails.Add(itt);

                            if (objMRRMaster.ChallanLocationCode != null)
                            {
                                Inv_ChallanDetails dbRecord = context.Inv_ChallanDetails.Where(c => c.LocationCode == objMRRMaster.ChallanLocationCode && c.ChallanSeqNo == objMRRMaster.ChallanSeqNo && c.ItemCode == itt.ItemCode).FirstOrDefault();

                                if (dbRecord.ReceivedByMRRQuantity == null)
                                    dbRecord.ReceivedByMRRQuantity = itt.ReceiveQuantity;
                                else
                                    dbRecord.ReceivedByMRRQuantity += itt.ReceiveQuantity;
                            }
                        }

                        if (lstMRRDetailsWithSerialNo.Count > 0)
                        {
                            foreach (Inv_MRRDetailsWithSerialNo ittws in lstMRRDetailsWithSerialNo)
                            {
                                context.Inv_MRRDetailsWithSerialNo.Add(ittws);
                            }
                        }

                        context.Aud_AuditAdjustmentRelatedChallanOrMRRForReference.Add(objChallanOrMRRForAuditAdjustment);

                        context.SaveChanges();

                        if (objMRRMaster.ChallanLocationCode == null)
                        {
                            context.InvGenerateItemTransactionForReceiveInGeneral(Helper.ReceiveRefDocTypeWithoutChallan, objMRRMaster.LocationCode, objMRRMaster.MRRSeqNo);
                        }
                        else
                        {
                            context.InvGenerateItemTransactionForReceiveInGeneral(Helper.ReceiveRefDocTypeWithChallan, objMRRMaster.LocationCode, objMRRMaster.MRRSeqNo);
                        }

                        tx.Complete();
                    }
                    catch (Exception ex)
                    {
                        tx.Dispose();
                        throw;
                    }
                }

                return objMRRMaster;
            }
        }

        public Sal_SystemReturn Create(Sal_SystemReturn objSystemReturn, List<Sal_SystemReturnItems> lstSystemReturnItems, List<Sal_SystemReturnItemsWithSerialNo> lstSystemReturnItemsWithSerialNo, string locationCode, string serialTempTableRows)
        {
            using (context)
            {

                string tempTableInsertQuery = "IF OBJECT_ID('tempdb..#TempItemNItemCategoryWithSerialNo') IS NOT NULL " +
                                                "BEGIN " +
                                                "	DROP TABLE #TempItemNItemCategoryWithSerialNo " +
                                                " END " +

                                                " CREATE TABLE #TempItemNItemCategoryWithSerialNo( " +
                                                " ItemCode NCHAR(4), ItemSerialNo NVARCHAR(25), ItemCategory NCHAR(6), " +
                                                " StoreLocation TINYINT, LocationCode NCHAR(6), CustomerCode  NCHAR(9)) " +

                                                " INSERT INTO  #TempItemNItemCategoryWithSerialNo (ItemCode, ItemSerialNo, ItemCategory,StoreLocation,LocationCode, CustomerCode) VALUES " + serialTempTableRows +

                                                " EXEC SP_CheckNSaveSerialNoInItemNItemCategoryWithSerialNoMasterTable 'MRRSYSTEMRETURN', 'RCVSYSRTRN', '" + locationCode + "'";

                using (TransactionScope tx = new TransactionScope())
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(serialTempTableRows.Trim()))
                            context.Database.ExecuteSqlCommand(tempTableInsertQuery);

                        context.Sal_SystemReturn.Add(objSystemReturn);

                        foreach (Sal_SystemReturnItems SRI in lstSystemReturnItems)
                        {
                            context.Sal_SystemReturnItems.Add(SRI);
                        }

                        if (lstSystemReturnItemsWithSerialNo.Count > 0)
                        {
                            foreach (Sal_SystemReturnItemsWithSerialNo objItemSeial in lstSystemReturnItemsWithSerialNo)
                            {
                                context.Sal_SystemReturnItemsWithSerialNo.Add(objItemSeial);
                            }
                        }

                        context.SaveChanges();

                        context.AESAL_SalesReturnV2(objSystemReturn.CustomerCode, Helper.Insert);

                        tx.Complete();
                    }
                    catch (Exception ex)
                    {
                        tx.Dispose();
                        throw;
                    }
                }

                return objSystemReturn;
            }
        }

        public Sal_CustomerTrainingTransMaster Create(Sal_CustomerTrainingTransMaster objSalCustomerTrainingMaster, List<Sal_CustomerTrainingTransDetails> lstCustomerTrainingTransDetails)
        {
            using (context)
            {

                using (TransactionScope tx = new TransactionScope())
                {
                    try
                    {
                        Sal_CustomerStatus objCustomerStatus;
                        foreach (Sal_CustomerTrainingTransDetails ctrd in lstCustomerTrainingTransDetails)
                        {
                            objCustomerStatus = new Sal_CustomerStatus();
                            objCustomerStatus = context.Sal_CustomerStatus.Where(c => c.CustomerCode == ctrd.CustomerCode).FirstOrDefault();

                            objCustomerStatus.IsCustomerTrained = true;
                            objCustomerStatus.CustomerTrainingDate = ctrd.TrainingDate;

                            context.SaveChanges();
                        }

                        context.Sal_CustomerTrainingTransMaster.Add(objSalCustomerTrainingMaster);

                        foreach (Sal_CustomerTrainingTransDetails ctrd in lstCustomerTrainingTransDetails)
                        {
                            context.Sal_CustomerTrainingTransDetails.Add(ctrd);
                        }

                        context.SaveChanges();

                        context.AESAL_CustomerTrainingExpensesV2(objSalCustomerTrainingMaster.UnitCode, objSalCustomerTrainingMaster.TrainingDate.ToString("dd-MMM-yy"), objSalCustomerTrainingMaster.TrainingBatchNo, objSalCustomerTrainingMaster.CreatedBy, Helper.Insert);

                        ////start for check 
                        //context.SaveChanges();

                        //string ceckExistTrainerID = " SELECT * FROM (SELECT DISTINCT TrainerEmployeeID FROM [RASolarERP].[dbo].[Sal_CustomerTrainingTransMaster] "
                        //                             + " WHERE TrainerEmployeeID IS NOT NULL AND TrainingDate > '31-Jan-2014'  "
                        //                             + " AND RefIDCOLTrainingNo IS NULL AND UnitCode = '" + objSalCustomerTrainingMaster.UnitCode + "' ) rsfCTM LEFT OUTER JOIN  "
                        //                             + " (SELECT DISTINCT RSF_EmployeeID RSF_Staff_As_Trainer,TrainingNo FROM [RASolarERP_IDCOL_TrainingModule].[dbo].[Solar_TOT_TrainingDetail]  "
                        //                             + "  WHERE TraineeTypeNo = 2 AND IsPasses = 1 AND [Status] = 0 ) IDCOL_StaffAsTrainer "
                        //                             + " ON rsfCTM.TrainerEmployeeID = IDCOL_StaffAsTrainer.RSF_Staff_As_Trainer WHERE IDCOL_StaffAsTrainer.TrainingNo IS NULL ";



                        //var v = context.Database.SqlQuery<int>(ceckExistTrainerID);

                        //if (v != null || v.Count()>0)
                        //{
                        //    tx.Dispose();
                        //}
                        //else if (v.Count()==0)
                        //{
                        //    tx.Complete();
                        //}
                        ////end chek
                        tx.Complete();
                    }
                    catch (Exception ex)
                    {
                        tx.Dispose();
                        throw;
                    }
                }

                return objSalCustomerTrainingMaster;
            }
        }

        public Inv_ChallanMaster Create(Inv_ChallanMaster objChallanMaster, List<Inv_ChallanDetails> lstChallanDetails, List<Inv_ChallanDetailsWithSerialNo> lstChallanDetailsWithSerialNo, Aud_AuditAdjustmentRelatedChallanOrMRRForReference objChallanOrMRRForAuditAdjustment)
        {
            using (context)
            {

                using (var tx = new TransactionScope())
                {
                    try
                    {
                        context.Inv_ChallanMaster.Add(objChallanMaster);

                        foreach (Inv_ChallanDetails incd in lstChallanDetails)
                        {
                            context.Inv_ChallanDetails.Add(incd);
                        }

                        if (lstChallanDetailsWithSerialNo.Count > 0)
                        {
                            foreach (Inv_ChallanDetailsWithSerialNo incdws in lstChallanDetailsWithSerialNo)
                            {
                                context.Inv_ChallanDetailsWithSerialNo.Add(incdws);
                            }
                        }

                        context.Aud_AuditAdjustmentRelatedChallanOrMRRForReference.Add(objChallanOrMRRForAuditAdjustment);

                        context.SaveChanges();

                        context.InvGenerateItemTransactionForIssueInGeneral("CHALLANINVNTORY", objChallanMaster.LocationCode, objChallanMaster.ChallanSeqNo);

                        tx.Complete();
                    }
                    catch (Exception ex)
                    {
                        tx.Dispose();
                        throw;
                    }
                }

                return objChallanMaster;
            }
        }

        public Fix_EmployeeWiseFixedAssetsAllocation Create(Fix_EmployeeWiseFixedAssetsAllocation objAssetAssign, List<Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo> lstAssetsAllocationWithSerialNo)
        {
            using (context)
            {

                using (TransactionScope Tx = new TransactionScope())
                {
                    try
                    {
                        if (lstAssetsAllocationWithSerialNo.Count > 0)
                        {
                            if (objAssetAssign.AllocatedQuantity > 0)
                            {
                                foreach (Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo assignSerial in lstAssetsAllocationWithSerialNo)
                                {
                                    context.Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo.Add(assignSerial);
                                }
                            }
                            else
                            {
                                foreach (Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo assignSerial in lstAssetsAllocationWithSerialNo)
                                {
                                    Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo assignSerialDb = context.Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo.Where(f => f.StoreLocation == objAssetAssign.StoreLocation && f.LocationCode == objAssetAssign.LocationCode && f.ItemCode == objAssetAssign.ItemCode && f.EmployeeID == objAssetAssign.EmployeeID && f.ItemSerialNo == assignSerial.ItemSerialNo && f.IsItAllocated == true && f.Status == Helper.Active).FirstOrDefault();
                                    assignSerialDb.IsItAllocated = false;
                                    context.SaveChanges();
                                }
                            }
                        }

                        context.Fix_EmployeeWiseFixedAssetsAllocation.Add(objAssetAssign);

                        context.SaveChanges();
                        Tx.Complete();
                    }
                    catch (Exception ex)
                    {
                        Tx.Dispose();
                        throw;
                    }
                }

                return objAssetAssign;
            }
        }

        public Aud_AuditAdjustmentRelatedCollectionFromCustomers SaveCustomerCollectionForAudit(Sal_CollectionFromCustomers objCustomerCollection, Aud_AuditAdjustmentRelatedCollectionFromCustomers objAuditAdjustmentCustomerCollection, string cashMemoUsesId)
        {
            using (context)
            {

                using (var Ts = new TransactionScope())
                {
                    try
                    {
                        string cashMemoId = string.Empty;
                        string cashMemoNo = string.Empty;

                        if (string.IsNullOrEmpty(objCustomerCollection.RefMemoNo))
                        {
                            SqlParameter[] storeParamForCashmemo = new SqlParameter[6];
                            storeParamForCashmemo[0] = new SqlParameter("prmEntrySource", "COLCAUDADJ");
                            storeParamForCashmemo[1] = new SqlParameter("prmCashMemoNo", objCustomerCollection.CashMemoNo);
                            storeParamForCashmemo[2] = new SqlParameter("prmCashMemoUsesID", objCustomerCollection.CashMemoUsesID);
                            storeParamForCashmemo[3] = new SqlParameter("prmLocationCode", objCustomerCollection.RefAELocationCode);
                            storeParamForCashmemo[4] = new SqlParameter("prmRefDocNo", objCustomerCollection.CustomerCode);
                            storeParamForCashmemo[5] = new SqlParameter("prmCashMemoUsedByEmployee", objCustomerCollection.CollectedByEmployee);

                            var v = context.Database.SqlQuery<string>("SP_CheckNSaveCashMemoNoInCashMemoMasterNTransactionRegisterTables @prmEntrySource, @prmCashMemoNo, @prmCashMemoUsesID, @prmLocationCode, @prmRefDocNo, @prmCashMemoUsedByEmployee", storeParamForCashmemo);

                            if (v != null)
                                cashMemoId = v.FirstOrDefault().ToString();

                            objCustomerCollection.RefAELocationCode = null;
                        }

                        cashMemoNo = objCustomerCollection.CashMemoNo.Trim(); //Use for 'Blank' CashMemoNo----------------- 
                        if (cashMemoNo.ToUpper() == "BLANK")
                        {
                            objCustomerCollection.CashMemoUsesID = null;
                        }

                        else
                        {
                            objCustomerCollection.CashMemoUsesID = cashMemoId;
                        }

                        context.Sal_CollectionFromCustomers.Add(objCustomerCollection);
                        context.Aud_AuditAdjustmentRelatedCollectionFromCustomers.Add(objAuditAdjustmentCustomerCollection);

                        context.SaveChanges();

                        SqlParameter[] salCollectionParamForAuditAdjustmentV2 = new SqlParameter[5];
                        salCollectionParamForAuditAdjustmentV2[0] = new SqlParameter("prmCustomerCode", objCustomerCollection.CustomerCode);
                        salCollectionParamForAuditAdjustmentV2[1] = new SqlParameter("prmYearMonth", objCustomerCollection.YearMonth);
                        salCollectionParamForAuditAdjustmentV2[2] = new SqlParameter("prmEntrySerialNo", objCustomerCollection.SerialNo);
                        salCollectionParamForAuditAdjustmentV2[3] = new SqlParameter("prmChangedCashMemoNo", objCustomerCollection.CashMemoNo);
                        salCollectionParamForAuditAdjustmentV2[4] = new SqlParameter("prmDBTransType", "INSERT");

                        context.Database.ExecuteSqlCommand("Exec AE_Sal_CollectionReceiveForAuditAdjustmentV2  @prmCustomerCode, @prmYearMonth, @prmEntrySerialNo, @prmChangedCashMemoNo, @prmDBTransType", salCollectionParamForAuditAdjustmentV2);

                        Ts.Complete();
                    }
                    catch (Exception ex)
                    {
                        Ts.Dispose();
                        throw;
                    }
                }
                return objAuditAdjustmentCustomerCollection;
            }
        }

        public Aud_AuditAdjustmentObservationOnSalesAgreement Create(Aud_AuditAdjustmentObservationOnSalesAgreement objAuditAdjustmentObservationOnSalesAgreement, Aud_AuditAdjustmentObservationOnSalesAgreement objPreviousDataAuditAdjustmentObservationOnSalesAgreement)
        {
            using (context)
            {

                using (var Ts = new TransactionScope())
                {
                    try
                    {
                        string cashMemoId = string.Empty;

                        if (!string.IsNullOrEmpty(objAuditAdjustmentObservationOnSalesAgreement.CashMemoNo)) //string.IsNullOrEmpty(objAuditAdjustmentObservationOnSalesAgreement.RefMemoNo) && 
                        {
                            SqlParameter[] storeParamForCashmemo = new SqlParameter[6];
                            storeParamForCashmemo[0] = new SqlParameter("prmEntrySource", "COLCAUDADJ");
                            storeParamForCashmemo[1] = new SqlParameter("prmCashMemoNo", objAuditAdjustmentObservationOnSalesAgreement.CashMemoNo);
                            storeParamForCashmemo[2] = new SqlParameter("prmCashMemoUsesID", Helper.CashMemuUsesIdFirst);
                            storeParamForCashmemo[3] = new SqlParameter("prmLocationCode", objAuditAdjustmentObservationOnSalesAgreement.LocationCode);
                            storeParamForCashmemo[4] = new SqlParameter("prmRefDocNo", objAuditAdjustmentObservationOnSalesAgreement.CustomerCode);
                            storeParamForCashmemo[5] = new SqlParameter("prmCashMemoUsedByEmployee", Helper.DBNullValue);

                            var v = context.Database.SqlQuery<string>("SP_CheckNSaveCashMemoNoInCashMemoMasterNTransactionRegisterTables @prmEntrySource, @prmCashMemoNo, @prmCashMemoUsesID, @prmLocationCode, @prmRefDocNo, @prmCashMemoUsedByEmployee", storeParamForCashmemo);

                            if (v != null)
                                cashMemoId = v.FirstOrDefault().ToString();

                            objAuditAdjustmentObservationOnSalesAgreement.CashMemoUsesID = cashMemoId;
                        }

                        context.Aud_AuditAdjustmentObservationOnSalesAgreement.Add(objPreviousDataAuditAdjustmentObservationOnSalesAgreement);
                        context.Aud_AuditAdjustmentObservationOnSalesAgreement.Add(objAuditAdjustmentObservationOnSalesAgreement);

                        context.SaveChanges();

                        SqlParameter[] storeParamForSalesAgreement = new SqlParameter[5];
                        storeParamForSalesAgreement[0] = new SqlParameter("prmLocationCode", objAuditAdjustmentObservationOnSalesAgreement.LocationCode);
                        storeParamForSalesAgreement[1] = new SqlParameter("prmYearMonth", objAuditAdjustmentObservationOnSalesAgreement.YearMonth);
                        storeParamForSalesAgreement[2] = new SqlParameter("prmCustomerCode", objAuditAdjustmentObservationOnSalesAgreement.CustomerCode);
                        storeParamForSalesAgreement[3] = new SqlParameter("prmAuditSeqNo", objAuditAdjustmentObservationOnSalesAgreement.AuditSeqNo);
                        storeParamForSalesAgreement[4] = new SqlParameter("prmDBTransType", "INSERT");

                        context.Database.ExecuteSqlCommand("Exec SP_Aud_CorrectSalesAgreementBasedOnObservationFromAuditAdjustment @prmLocationCode, @prmYearMonth, @prmCustomerCode, @prmAuditSeqNo, @prmDBTransType", storeParamForSalesAgreement);

                        Ts.Complete();
                    }
                    catch (Exception ex)
                    {
                        Ts.Dispose();
                        throw;
                    }
                }

                return objAuditAdjustmentObservationOnSalesAgreement;
            }
        }

        public Fix_EmployeeWiseFixedAssetsAllocation CreateForCashMemo(Fix_EmployeeWiseFixedAssetsAllocation objAssetAssign, List<Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo> lstAssetsAllocationWithSerialNo)
        {
            string sqlQuery = string.Empty;

            using (context)
            {

                using (TransactionScope Tx = new TransactionScope())
                {
                    try
                    {
                        context.Fix_EmployeeWiseFixedAssetsAllocation.Add(objAssetAssign);

                        foreach (Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo assignSerial in lstAssetsAllocationWithSerialNo)
                        {
                            context.Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo.Add(assignSerial);
                        }

                        context.SaveChanges();

                        SqlParameter[] storeParam = new SqlParameter[7];
                        storeParam[0] = new SqlParameter("prmAssignOrUnassign", "ASSIGN");
                        storeParam[1] = new SqlParameter("prmEmployeeID", objAssetAssign.EmployeeID);
                        storeParam[2] = new SqlParameter("prmStoreLocation", objAssetAssign.StoreLocation);
                        storeParam[3] = new SqlParameter("prmLocationCode", objAssetAssign.LocationCode);
                        storeParam[4] = new SqlParameter("prmItemCode", objAssetAssign.ItemCode);
                        storeParam[5] = new SqlParameter("prmAllocationSeqNo", objAssetAssign.AllocationSeqNo);
                        storeParam[6] = new SqlParameter("prmDBTransType", Helper.Insert);

                        context.Database.ExecuteSqlCommand("SP_FixAssignOrUnassignItemWithEmployee @prmAssignOrUnassign, @prmEmployeeID, @prmStoreLocation, @prmLocationCode, @prmItemCode, @prmAllocationSeqNo, @prmDBTransType", storeParam);

                        Tx.Complete();
                    }
                    catch (Exception ex)
                    {
                        Tx.Dispose();
                        throw;
                    }
                }

                return objAssetAssign;
            }
        }

        public Sal_SalesAgreementClosedWithFullPaidOrWaive SaveFullPaiedCustomer(Sal_SalesAgreementClosedWithFullPaidOrWaive objSalesAgreementClosedWithFullPaidOrWaive, string closedIn, bool? approvalRequiredForFullPayed)
        {
            using (context)
            {

                using (var Ts = new TransactionScope())
                {
                    try
                    {
                        if (approvalRequiredForFullPayed == true)
                        {
                            Sal_TempApprovalForSalesAgreementClosed objTempApprovalForSalesAgreementClosed = new Sal_TempApprovalForSalesAgreementClosed();
                            objTempApprovalForSalesAgreementClosed.CustomerCode = objSalesAgreementClosedWithFullPaidOrWaive.CustomerCode;
                            objTempApprovalForSalesAgreementClosed.ApprovalNo = objSalesAgreementClosedWithFullPaidOrWaive.ApprovalNo;
                            objTempApprovalForSalesAgreementClosed.RequestedAgreementClosedDate = objSalesAgreementClosedWithFullPaidOrWaive.AgreementClosedDate;
                            objTempApprovalForSalesAgreementClosed.RemainingServiceChargeReceivableAfterAdjustmentFromAdvance = objSalesAgreementClosedWithFullPaidOrWaive.RemainingServiceChargeReceivableAtTheEndOfClosedMonth;
                            objTempApprovalForSalesAgreementClosed.RemainingPrincipalReceivableAfterAdjustmentFromAdvance = objSalesAgreementClosedWithFullPaidOrWaive.RemainingPrincipalReceivableAtTheEndOfClosedMonth;
                            objTempApprovalForSalesAgreementClosed.Status = Helper.TempApprovalForSalesAgreementStatus;
                            context.Sal_TempApprovalForSalesAgreementClosed.Add(objTempApprovalForSalesAgreementClosed);
                        }
                        else
                        {
                            context.Sal_SalesAgreementClosedWithFullPaidOrWaive.Add(objSalesAgreementClosedWithFullPaidOrWaive);
                        }

                        context.SaveChanges();

                        SqlParameter[] storeParam = new SqlParameter[3];
                        storeParam[0] = new SqlParameter("CustomerCode", objSalesAgreementClosedWithFullPaidOrWaive.CustomerCode);
                        storeParam[1] = new SqlParameter("CurrentOrLastMonth", closedIn);

                        if (approvalRequiredForFullPayed != null)
                            storeParam[2] = new SqlParameter("IsApprovalRequired", approvalRequiredForFullPayed);
                        else
                            storeParam[2] = new SqlParameter("IsApprovalRequired", Helper.DBNullValue);

                        context.Database.ExecuteSqlCommand("Exec SP_SalUpdateCustomerDataToCloseWithFullPaidOrWaive @CustomerCode, @CurrentOrLastMonth, @IsApprovalRequired", storeParam);

                        Ts.Complete();

                        return objSalesAgreementClosedWithFullPaidOrWaive;
                    }
                    //catch (DbEntityValidationException ex)
                    //{
                    //    StringBuilder sb = new StringBuilder();

                    //    foreach (var failure in ex.EntityValidationErrors)
                    //    {
                    //        sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    //        foreach (var error in failure.ValidationErrors)
                    //        {
                    //            sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                    //            sb.AppendLine();
                    //        }
                    //    }

                    //    Ts.Dispose();

                    //    throw new DbEntityValidationException(
                    //        "Entity Validation Failed - errors follow:\n" +
                    //        sb.ToString(), ex
                    //    ); // Add the original exception as the innerException
                    //}
                    catch (Exception ex)
                    {
                        Ts.Dispose();
                        throw;
                    }
                }
            }
        }

        public void CreateUserLog(string ipAddress, string macAddress, string locationCode, string userId, string referenceEntrySource)
        {
            try
            {
                //string sqlQuery = "INSERT INTO Common_UserLogRegister (LogTime, UserID, LocationCode, IPAddress, ReferenceEntrySource) " +
                //                  "VALUES ( " +

                //                            "'" + DateTime.Now + "', " +
                //                            "'" + userId + "', " +
                //                            "'" + locationCode + "', " +
                //                            "'" + ipAddress + "', " +
                //                            "'" + referenceEntrySource + "'" +
                //                        " )";

                //context.Database.ExecuteSqlCommand(sqlQuery);

                SqlParameter[] storeParam = new SqlParameter[4];
                storeParam[0] = new SqlParameter("@prmUserID", userId);
                storeParam[1] = new SqlParameter("@prmLocationCode", locationCode);
                storeParam[2] = new SqlParameter("@prmIPAddress", ipAddress);
                storeParam[3] = new SqlParameter("@prmReferenceEntrySource", referenceEntrySource);

                context.Database.ExecuteSqlCommand("Exec SP_UpdateUserLogRegister @prmUserID, @prmLocationCode, @prmIPAddress, @prmReferenceEntrySource", storeParam);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Inv_MRRMaster SaveWarrantyClaimNSettlement(Inv_MRRMaster objMRRMaster, List<Inv_MRRDetails> lstMRRDetails, List<Inv_MRRDetailsWithSerialNo> lstMRRDetailsWithSerialNo, Inv_ChallanMaster objChallanMaster, List<Inv_ChallanDetails> lstChallanDetails, List<Inv_ChallanDetailsWithSerialNo> lstChallanDetailsWithSerialNo)
        {

            using (context)
            {

                using (TransactionScope tx = new TransactionScope())
                {
                    try
                    {
                        // For Receieve/ MRR 

                        context.Inv_MRRMaster.Add(objMRRMaster);

                        foreach (Inv_MRRDetails itt in lstMRRDetails)
                        {
                            context.Inv_MRRDetails.Add(itt);
                        }

                        if (lstMRRDetailsWithSerialNo.Count > 0)
                        {
                            if (lstMRRDetailsWithSerialNo[0].ItemSerialNo != null)
                            {
                                foreach (Inv_MRRDetailsWithSerialNo ittws in lstMRRDetailsWithSerialNo)
                                {
                                    context.Inv_MRRDetailsWithSerialNo.Add(ittws);
                                }
                            }
                        }

                        context.SaveChanges();

                        if (objMRRMaster.ChallanLocationCode == null)
                        {
                            context.InvGenerateItemTransactionForReceiveInGeneral(Helper.ReceiveRefDocTypeWithoutChallan, objMRRMaster.LocationCode, objMRRMaster.MRRSeqNo);
                        }
                        else
                        {
                            context.InvGenerateItemTransactionForReceiveInGeneral(Helper.ReceiveRefDocTypeWithChallan, objMRRMaster.LocationCode, objMRRMaster.MRRSeqNo);
                        }

                        // For Challan 

                        context.Inv_ChallanMaster.Add(objChallanMaster);

                        foreach (Inv_ChallanDetails incd in lstChallanDetails)
                        {
                            context.Inv_ChallanDetails.Add(incd);
                        }

                        if (lstChallanDetailsWithSerialNo.Count > 0)
                        {
                            if (lstChallanDetailsWithSerialNo[0].ItemSerialNo != null)
                            {
                                foreach (Inv_ChallanDetailsWithSerialNo incdws in lstChallanDetailsWithSerialNo)
                                {
                                    context.Inv_ChallanDetailsWithSerialNo.Add(incdws);
                                }
                            }
                        }

                        context.SaveChanges();

                        context.InvGenerateItemTransactionForIssueInGeneral("CHALLANINVNTORY", objChallanMaster.LocationCode, objChallanMaster.ChallanSeqNo);

                        tx.Complete();
                    }
                    catch (Exception ex)
                    {
                        tx.Dispose();
                        throw;
                    }
                }

                return objMRRMaster;
            }
        }

        public string ItemSerialCorrectionChangeNClear(string locationCode, string itemCode, string optionForCorrection, string wrongSerialNo, string correctSerialNo, string loginID)
        {
            using (context)
            {

                using (TransactionScope tx = new TransactionScope())
                {
                    try
                    {
                        SqlParameter[] storeParam = new SqlParameter[6];
                        storeParam[0] = new SqlParameter("@prmOption", optionForCorrection);
                        storeParam[1] = new SqlParameter("@prmLocationCode", locationCode);
                        storeParam[2] = new SqlParameter("@prmItemCode", itemCode);
                        storeParam[3] = new SqlParameter("@prmWrongSerialNoToReplace", wrongSerialNo);
                        storeParam[4] = new SqlParameter("@prmCorrectSerialNo", correctSerialNo);
                        storeParam[5] = new SqlParameter("@prmUserID", loginID);

                        var result = context.Database.ExecuteSqlCommand("Exec Support_CorrectWrongItemSerialNo @prmOption, @prmLocationCode, @prmItemCode, @prmWrongSerialNoToReplace, @prmCorrectSerialNo, @prmUserID", storeParam);
                        tx.Complete();

                        return result.ToString();
                    }
                    catch (Exception ex)
                    {
                        tx.Dispose();
                        throw;
                    }


                }
            }

        }

        public SHSDistributionPlan_Master Create(SHSDistributionPlan_Master objSHSDistributionPlan_Master, List<SHSDistributionPlan_RootWiseLocation> lstSHSDistributionPlan_RootWiseLocation, List<SHSDistributionPlan_RootWiseLocationNPackage> lstSHSDistributionPlan_RootWiseLocationNPackage, List<SHSDistributionPlan_IndividualItem> lstSHSDistributionPlan_IndividualItem)
        {
            using (context)
            {

                using (TransactionScope tx = new TransactionScope())
                {

                    try
                    {
                        context.SHSDistributionPlan_Master.Add(objSHSDistributionPlan_Master);

                        foreach (SHSDistributionPlan_RootWiseLocation srwl in lstSHSDistributionPlan_RootWiseLocation)
                        {
                            context.SHSDistributionPlan_RootWiseLocation.Add(srwl);
                        }

                        foreach (SHSDistributionPlan_RootWiseLocationNPackage trd in lstSHSDistributionPlan_RootWiseLocationNPackage)
                        {
                            context.SHSDistributionPlan_RootWiseLocationNPackage.Add(trd);
                        }

                        foreach (SHSDistributionPlan_IndividualItem sdpItems in lstSHSDistributionPlan_IndividualItem)
                        {
                            context.SHSDistributionPlan_IndividualItem.Add(sdpItems);
                        }

                        context.SaveChanges();


                        SqlParameter[] storeParam = new SqlParameter[2];
                        storeParam[0] = new SqlParameter("DistribScheduleNo ", objSHSDistributionPlan_Master.DistribScheduleNo);
                        storeParam[1] = new SqlParameter("prmOption", "INSERT");

                        context.Database.ExecuteSqlCommand("Exec SP_SHSDistributionPlanUpdatePackageWiseItem  @DistribScheduleNo, @prmOption", storeParam);


                        tx.Complete();

                        return objSHSDistributionPlan_Master;
                    }

                    catch (Exception ex)
                    {
                        tx.Dispose();
                        throw;
                    }
                }
            }
        }

        #endregion

        #region Update

        //public Common_UnitWiseEntryStatus Update(Common_UnitWiseEntryStatus objUnitWiseEntryStatus)
        //{
        //    Common_UnitWiseEntryStatus dbRecord = UnitWiseEntryStatus(objUnitWiseEntryStatus.Unit_Code, objUnitWiseEntryStatus.YearMonth);
        //    context.ApplyCurrentValues(dbRecord.EntityKey.EntitySetName, objUnitWiseEntryStatus);
        //    context.SaveChanges();

        //    return objUnitWiseEntryStatus;
        //}

        //public Common_InventoryTransaction Update(Common_InventoryTransaction objInventoryTranscation)
        //{
        //    Common_InventoryTransaction dbRecord = ReadInventoryTransaction(objInventoryTranscation.UNIT_CODE, objInventoryTranscation.COMP_CODE, objInventoryTranscation.YearMonth, Convert.ToInt32(objInventoryTranscation.StockLocation));
        //    context.ApplyCurrentValues(dbRecord.EntityKey.EntitySetName, objInventoryTranscation);
        //    context.SaveChanges();

        //    return objInventoryTranscation;
        //}

        //public Aud_AuditingMaster Update(Aud_AuditingMaster objAuditingMaster)
        //{
        //    Aud_AuditingMaster dbRecord = AuditingMasterByLocationCode(objAuditingMaster.LocationCode);
        //    context.ApplyCurrentValues(dbRecord.EntityKey.EntitySetName, objAuditingMaster);
        //    context.SaveChanges();

        //    return objAuditingMaster;
        //}

        //public Common_UnitWiseCustomerStatus Update(Common_UnitWiseCustomerStatus objUnitWiseCustomerStatus)
        //{
        //    Common_UnitWiseCustomerStatus dbRecord = ReadUnitWiseCustomerStatus(objUnitWiseCustomerStatus.Unit_Code, objUnitWiseCustomerStatus.Cust_Code);
        //    context.ApplyCurrentValues(dbRecord.EntityKey.EntitySetName, objUnitWiseCustomerStatus);
        //    context.SaveChanges();

        //    return objUnitWiseCustomerStatus;
        //}

        public Sal_LocationNEmployeeWiseActivityDaily Update(Sal_LocationNEmployeeWiseActivityDaily objLocationNEmployeeWiseActivityDailyUpdate)
        {
            //Sal_LocationNEmployeeWiseActivityDaily dbRecord = ReadLocationNEmployeeWiseActivityDaily(objLocationNEmployeeWiseActivityDailyUpdate.LocationCode, objLocationNEmployeeWiseActivityDailyUpdate.EmployeeID, objLocationNEmployeeWiseActivityDailyUpdate.YearMonth, objLocationNEmployeeWiseActivityDailyUpdate.TransDate);
            //context.ApplyCurrentValues(dbRecord.EntityKey.EntitySetName, objLocationNEmployeeWiseActivityDailyUpdate);

            context.Sal_LocationNEmployeeWiseActivityDaily.Attach(objLocationNEmployeeWiseActivityDailyUpdate);
            var LocationNEmployeeWiseActivityDailyEntry = context.Entry(objLocationNEmployeeWiseActivityDailyUpdate);
            LocationNEmployeeWiseActivityDailyEntry.State = EntityState.Modified;
            context.SaveChanges();

            return objLocationNEmployeeWiseActivityDailyUpdate;
        }

        public Sal_LocationNEmployeeWiseActivityMonthly Update(Sal_LocationNEmployeeWiseActivityMonthly objLocationNEmployeeWiseActivityMonthlyUpdate)
        {
            //Sal_LocationNEmployeeWiseActivityMonthly dbRecord = ReadLocationNEmployeeWiseActivityMonthly(objLocationNEmployeeWiseActivityMonthlyUpdate.LocationCode, objLocationNEmployeeWiseActivityMonthlyUpdate.EmployeeID, objLocationNEmployeeWiseActivityMonthlyUpdate.YearMonth);
            //context.ApplyCurrentValues(dbRecord.EntityKey.EntitySetName, objLocationNEmployeeWiseActivityMonthlyUpdate);

            context.Sal_LocationNEmployeeWiseActivityMonthly.Attach(objLocationNEmployeeWiseActivityMonthlyUpdate);
            var LocationNEmployeeWiseActivityMonthlyEntry = context.Entry(objLocationNEmployeeWiseActivityMonthlyUpdate);
            LocationNEmployeeWiseActivityMonthlyEntry.State = EntityState.Modified;

            context.SaveChanges();

            return objLocationNEmployeeWiseActivityMonthlyUpdate;

        }

        public Sal_LocationWiseSalesNCollectionTarget Update(Sal_LocationWiseSalesNCollectionTarget objSalesNCollectionTarget)
        {
            //Sal_LocationWiseSalesNCollectionTarget dbRecord = ReadLocationWiseSalesNCollectionTarget(objSalesNCollectionTarget.LocationCode, objSalesNCollectionTarget.YearMonth);
            context.Sal_LocationWiseSalesNCollectionTarget.Attach(objSalesNCollectionTarget);
            var LocationWiseSalesNCollectionTargetEntry = context.Entry(objSalesNCollectionTarget);
            LocationWiseSalesNCollectionTargetEntry.State = EntityState.Modified;

            context.SaveChanges();
            return objSalesNCollectionTarget;
        }

        //public Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement Update(Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement objWeeklyCollection)
        //{
        //    //Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement dbRecord = ReadWeeklySalesNCollectionTargetNAchievement(objWeeklyCollection.LocationCode, objWeeklyCollection.YearWeek, objWeeklyCollection.EmployeeID, objWeeklyCollection.TransDate.Date);
        //    //ontext.ApplyCurrentValues(dbRecord.EntityKey.EntitySetName, objWeeklyCollection);

        //    context.Sal_LocationNEmployeeWiseWeeklySalesNCollectionTargetNAchievement.Attach(objWeeklyCollection);
        //    var weeklyCollectionEntry = context.Entry(objWeeklyCollection);
        //    weeklyCollectionEntry.State = EntityState.Modified;
        //    context.SaveChanges();

        //    return objWeeklyCollection;
        //}

        //public Sal_LocationWiseWeeklySalesNCollectionSummary Update(Sal_LocationWiseWeeklySalesNCollectionSummary objSal_LocationWiseWeeklySalesNCollectionSummary)
        //{
        //    //Sal_LocationWiseWeeklySalesNCollectionSummary dbRecoed = LocationWiseWeeklySalesNCollectionSummary(objSal_LocationWiseWeeklySalesNCollectionSummary.LocationCode, objSal_LocationWiseWeeklySalesNCollectionSummary.YearWeek);
        //    //context.ApplyCurrentValues(dbRecoed.EntityKey.EntitySetName, objSal_LocationWiseWeeklySalesNCollectionSummary);

        //    context.Sal_LocationWiseWeeklySalesNCollectionSummary.Attach(objSal_LocationWiseWeeklySalesNCollectionSummary);
        //    var LocationWiseWeeklySalesNCollectionSummaryEntry = context.Entry(objSal_LocationWiseWeeklySalesNCollectionSummary);
        //    LocationWiseWeeklySalesNCollectionSummaryEntry.State = EntityState.Modified;
        //    context.SaveChanges();

        //    return objSal_LocationWiseWeeklySalesNCollectionSummary;
        //}

        public bool Update(string unitCode, string customerCode, decimal collectedOverdue, string weekNumber)
        {
            string sqlQuery = "Update Sal_CustomerWiseOverdueBalanceForTracker " +
                               "Set RecoWeek" + weekNumber + " = " + collectedOverdue +
                               " Where UnitCode = '" + unitCode + "' And CustomerCode ='" + customerCode + "'";

            var queryResult = context.Database.ExecuteSqlCommand(sqlQuery);

            return Convert.ToBoolean(queryResult);
        }

        public Sal_CollectionFromCustomersPrePost Update(Sal_CollectionFromCustomersPrePost objCollectionFromCustomers, Sal_CollectionFromCustomers objCustomerCol)
        {
            using (context)
            {
                //using (TransactionScope tx = new TransactionScope())
                //{
                try
                {
                    //Sal_CollectionFromCustomers dbRecord = ReadCollectionFromCustomers(objCollectionFromCustomers.CustomerCode, objCollectionFromCustomers.YearMonth, objCollectionFromCustomers.SerialNo);
                    //context.ApplyCurrentValues(dbRecord.EntityKey.EntitySetName, objCollectionFromCustomers);

                    //context.Sal_CollectionFromCustomersPrePost.Attach(objCollectionFromCustomers);
                    //var CollectionFromCustomersEntry = context.Entry(objCollectionFromCustomers);
                    //CollectionFromCustomersEntry.State = EntityState.Modified;

                    //CollectionFromCustomersEntry.Property(p => p.IsTransferredToFinalTable).IsModified = false;

                    context.Sal_CollectionFromCustomers.Attach(objCustomerCol);
                    var collectionFromCustomers = context.Entry(objCustomerCol);

                    collectionFromCustomers.Property(p => p.CashMemoNo).IsModified = false;
                    collectionFromCustomers.Property(p => p.CashMemoUsesID).IsModified = false;

                    collectionFromCustomers.State = EntityState.Modified;

                    context.SaveChanges();

                    //context.CollectionReciveV2(objCollectionFromCustomers.CustomerCode, objCollectionFromCustomers.YearMonth, objCollectionFromCustomers.SerialNo, Helper.Update);

                    //tx.Complete();

                    SqlParameter[] storeParam = new SqlParameter[5];
                    storeParam[0] = new SqlParameter("prmCustomerCode", objCollectionFromCustomers.CustomerCode);
                    storeParam[1] = new SqlParameter("prmYearMonth", objCollectionFromCustomers.YearMonth);
                    storeParam[2] = new SqlParameter("prmEntrySerialNo", objCollectionFromCustomers.SerialNo);
                    storeParam[3] = new SqlParameter("prmChangedCashMemoNo", objCollectionFromCustomers.CashMemoNo);
                    storeParam[4] = new SqlParameter("prmDBTransType", "UPDATE");

                    context.Database.ExecuteSqlCommand("AE_Sal_CollectionReceiveWithoutTransactionV3 @prmCustomerCode, @prmYearMonth, @prmEntrySerialNo, @prmChangedCashMemoNo, @prmDBTransType", storeParam);

                }
                catch (Exception ex)
                {
                    //tx.Dispose();
                    throw;
                }
                //}
                return objCollectionFromCustomers;
            }
        }

        public Sal_Customer Update(Sal_Customer objCustomer)
        {
            //Sal_Customer dbRecord = ReadCustomer(objCustomer.CustomerCode);
            //context.ApplyCurrentValues(dbRecord.EntityKey.EntitySetName, objCustomer);

            context.Sal_Customer.Attach(objCustomer);
            var CustomerEntry = context.Entry(objCustomer);
            CustomerEntry.State = EntityState.Modified;
            context.SaveChanges();

            return objCustomer;
        }



        public Sal_Customer Update(Sal_Customer objCustomer,string remarksNotes)
        {
            try
            {

                Sal_Customer objCustomerUpdate = new Sal_Customer();
                Sal_CustomerInfoBackup objSal_CustomerInfoBackup = new Sal_CustomerInfoBackup();

                objCustomerUpdate = context.Sal_Customer.Where(c => c.CustomerCode == objCustomer.CustomerCode).FirstOrDefault();

                objSal_CustomerInfoBackup.CustomerCode = objCustomerUpdate.CustomerCode;
                objSal_CustomerInfoBackup.BackupEntryTime = DateTime.Now;
                objSal_CustomerInfoBackup.OldCustomerName = objCustomerUpdate.CustomerName;
                objSal_CustomerInfoBackup.OldFathersOrHusbandName = objCustomerUpdate.FathersOrHusbandName;
                objSal_CustomerInfoBackup.OldRelationWithGuardian = objCustomerUpdate.RelationWithGuardian;
                objSal_CustomerInfoBackup.OldMothersName = objCustomerUpdate.MothersName;
                objSal_CustomerInfoBackup.OldNationalIDCard = objCustomerUpdate.NationalIDCard;
                objSal_CustomerInfoBackup.OldGender = objCustomerUpdate.Gender;
                objSal_CustomerInfoBackup.OldPhoneNo = objCustomerUpdate.PhoneNo;
                objSal_CustomerInfoBackup.OldIsMobileNoMandatory = objCustomerUpdate.IsMobileNoMandatory;
                objSal_CustomerInfoBackup.OldVillage = objCustomerUpdate.Village;
                objSal_CustomerInfoBackup.OldPostOffice = objCustomerUpdate.PostOffice;
                objSal_CustomerInfoBackup.OldUnionID = objCustomerUpdate.UnionID;
                objSal_CustomerInfoBackup.OldThanaID = objCustomerUpdate.ThanaID;
                objSal_CustomerInfoBackup.OldDistrictCode = objCustomerUpdate.DistrictCode;
                objSal_CustomerInfoBackup.OldUnitCode = objCustomerUpdate.UnitCode;
                objSal_CustomerInfoBackup.OldCustomerType = objCustomerUpdate.CustomerType;
                objSal_CustomerInfoBackup.OldOccupation = objCustomerUpdate.Occupation;
                objSal_CustomerInfoBackup.OldIncomeRange = objCustomerUpdate.IncomeRange;
                objSal_CustomerInfoBackup.OldTotalFamilyMember = objCustomerUpdate.TotalFamilyMember;
                objSal_CustomerInfoBackup.OldWomenInTotalFamilyMember = objCustomerUpdate.WomenInTotalFamilyMember;
                objSal_CustomerInfoBackup.OldIsConsultedWithWomenMemberForInstallationOfLamps = objCustomerUpdate.IsConsultedWithWomenMemberForInstallationOfLamps;
                objSal_CustomerInfoBackup.OldFuelUsedBeforeSHS = objCustomerUpdate.FuelUsedBeforeSHS;
                objSal_CustomerInfoBackup.OldFuelConsumptionPerMonth = objCustomerUpdate.FuelConsumptionPerMonth;
                objSal_CustomerInfoBackup.OldCreatedBy = objCustomerUpdate.CreatedBy;
                objSal_CustomerInfoBackup.OldCreatedOn = objCustomerUpdate.CreatedOn;
                objSal_CustomerInfoBackup.OldModifiedBy = objCustomerUpdate.ModifiedBy;
                objSal_CustomerInfoBackup.OldModifiedOn = objCustomerUpdate.ModifiedOn;
                objSal_CustomerInfoBackup.OldStatus = objCustomerUpdate.Status;
                objSal_CustomerInfoBackup.NewCustomerName = objCustomer.CustomerName;
                objSal_CustomerInfoBackup.NewVillage = objCustomer.Village;
                objSal_CustomerInfoBackup.NewPostOffice = objCustomer.PostOffice;
                objSal_CustomerInfoBackup.NewUnionID = objCustomer.UnionID;
                objSal_CustomerInfoBackup.NewThanaID = objCustomer.ThanaID;
                objSal_CustomerInfoBackup.NewDistrictCode = objCustomer.DistrictCode;
                objSal_CustomerInfoBackup.ModifiedBy = objCustomer.ModifiedBy;
                context.Sal_CustomerInfoBackup.Add(objSal_CustomerInfoBackup);


                objCustomerUpdate.CustomerName = objCustomer.CustomerName;
                objCustomerUpdate.FathersOrHusbandName = objCustomer.FathersOrHusbandName;
                objCustomerUpdate.Village = objCustomer.Village;
                objCustomerUpdate.PostOffice = objCustomer.PostOffice;
                objCustomerUpdate.ThanaID = objCustomer.ThanaID;
                objCustomerUpdate.UnionID = objCustomer.UnionID;
                objCustomerUpdate.DistrictCode = objCustomer.DistrictCode;
                objCustomerUpdate.PhoneNo = objCustomer.PhoneNo;
                objCustomerUpdate.ModifiedBy = objCustomer.ModifiedBy;
                objCustomerUpdate.ModifiedOn = objCustomer.ModifiedOn;
                
                
                Sal_CustomerStatus objCustomerStatus = new Sal_CustomerStatus();
                objCustomerStatus = context.Sal_CustomerStatus.Where(c => c.CustomerCode == objCustomer.CustomerCode).FirstOrDefault();

                objCustomerStatus.Remarks = remarksNotes;
                objCustomerStatus.ModifiedBy = objCustomer.ModifiedBy;
                objCustomerStatus.ModifiedOn = objCustomer.ModifiedOn;
                //objCustomerStatus.IsCustomerALocalMuscleMan = objCustomerStatus.IsCustomerALocalMuscleMan;

                context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw;
            }

            return objCustomer;
        }



        public Sal_CustomerStatus UpdateFPR(List<CustomerFPRAndScheduledCollectionEntry> lstCustomerFPREntry, string unitCode)
        {
            //string customerCode, string employeeAsFPR, string unitCode, string scheduledCollectionDay, string agreementDate
            Sal_CustomerStatus objCustomerStatus = null;

            using (context)
            {

                using (TransactionScope tx = new TransactionScope())
                {
                    try
                    {
                        string sqlQuery = string.Empty;

                        //"UPDATE [Sal_CustomerStatus] SET [EmployeeAsFPR]='" + employeeAsFPR + "' where CustomerCode='" + customerCode + "'";
                        //sqlQuery += "UPDATE Sal_SalesAgreement SET ScheduledCollectionDay='" + scheduledCollectionDay + "' WHERE CustomerCode='" + customerCode + "' AND AgreementDate='" + Convert.ToDateTime(agreementDate).ToString("dd-MMM-yyyy") + "' AND UnitCode='" + unitCode + "'";

                        foreach (CustomerFPRAndScheduledCollectionEntry fprSetup in lstCustomerFPREntry)
                        {

                            sqlQuery += string.Format("UPDATE [Sal_CustomerStatus] SET [EmployeeAsFPR]='{0}' where CustomerCode='{1}' UPDATE Sal_SalesAgreement SET ScheduledCollectionDay='{2}' WHERE CustomerCode='{3}' AND AgreementDate='{4}' AND UnitCode='{5}'",
                                                     fprSetup.EmployeeAsFPR, fprSetup.CustomerCode, fprSetup.ScheduledCollectionDay, fprSetup.CustomerCode, fprSetup.AgreementDate, unitCode);
                        }

                        var queryResult = context.Database.ExecuteSqlCommand(sqlQuery);

                        tx.Complete();
                    }
                    catch (Exception ex)
                    {
                        tx.Dispose();
                        throw;
                    }

                    return objCustomerStatus;
                }
            }
        }

        public bool Update(List<Sal_LocationNEmployeeWiseDailySalesNCollectionTarget> lstEmployeeDailyTarget)
        {
            bool updateSucceeedOrNot = true;
            using (context)
            {
                using (TransactionScope Tx = new TransactionScope())
                {
                    try
                    {
                        foreach (Sal_LocationNEmployeeWiseDailySalesNCollectionTarget salTarget in lstEmployeeDailyTarget)
                        {
                            Sal_LocationNEmployeeWiseDailySalesNCollectionTarget dbRecord = context.Sal_LocationNEmployeeWiseDailySalesNCollectionTarget.Where(s => s.LocationCode == salTarget.LocationCode && s.YearMonth == salTarget.YearMonth && s.EmployeeID == salTarget.EmployeeID && s.TargetDate == salTarget.TargetDate).FirstOrDefault();

                            dbRecord.SalesTarget = salTarget.SalesTarget;
                            dbRecord.ModifiedBy = salTarget.ModifiedBy;
                            dbRecord.ModifiedOn = salTarget.ModifiedOn;
                            dbRecord.EntryTime = salTarget.EntryTime;

                            context.SaveChanges();
                        }

                        Tx.Complete();
                    }
                    catch (Exception ex)
                    {
                        Tx.Dispose();
                        updateSucceeedOrNot = false;
                        throw;
                    }
                }

                return updateSucceeedOrNot;
            }
        }

        public Sal_CollectionFromCustomers Update(Sal_CollectionFromCustomers objCollectionFromCustomers, Aud_AuditAdjustmentRelatedCollectionFromCustomers objCollectionAuditAdjustnment)
        {
            using (context)
            {

                using (TransactionScope tx = new TransactionScope())
                {
                    try
                    {
                        context.Sal_CollectionFromCustomers.Attach(objCollectionFromCustomers);
                        var CollectionFromCustomersEntry = context.Entry(objCollectionFromCustomers);
                        CollectionFromCustomersEntry.State = EntityState.Modified;

                        CollectionFromCustomersEntry.Property(p => p.CashMemoNo).IsModified = false;
                        CollectionFromCustomersEntry.Property(p => p.RefMemoNo).IsModified = false;
                        CollectionFromCustomersEntry.Property(p => p.CashMemoUsesID).IsModified = false;

                        context.Aud_AuditAdjustmentRelatedCollectionFromCustomers.Attach(objCollectionAuditAdjustnment);
                        var CollectionAuditAdjustnmentEntry = context.Entry(objCollectionAuditAdjustnment);
                        CollectionAuditAdjustnmentEntry.State = EntityState.Modified;

                        context.SaveChanges();

                        SqlParameter[] storeParam = new SqlParameter[7];
                        storeParam[0] = new SqlParameter("prmCustomerCode", objCollectionFromCustomers.CustomerCode);
                        storeParam[1] = new SqlParameter("prmYearMonth", objCollectionFromCustomers.YearMonth);
                        storeParam[2] = new SqlParameter("prmEntrySerialNo", objCollectionFromCustomers.SerialNo);
                        storeParam[3] = new SqlParameter("prmUserID", objCollectionFromCustomers.UserID);
                        storeParam[4] = new SqlParameter("prmChangedCashMemoNo", objCollectionFromCustomers.CashMemoNo);
                        storeParam[5] = new SqlParameter("prmDBTransType", "UPDATE");

                        context.Database.ExecuteSqlCommand("SP_Aud_InsertOrChangeCurrentOrBackdatedCollectionNUpdateCustomerStatus @prmCustomerCode, @prmYearMonth, @prmEntrySerialNo, @prmUserID, @prmChangedCashMemoNo, @prmDBTransType", storeParam);

                        tx.Complete();
                    }
                    catch (Exception ex)
                    {
                        tx.Dispose();
                        throw;
                    }
                }
                return objCollectionFromCustomers;
            }
        }

        public Aud_AuditAdjustmentObservationOnSalesAgreement Update(Aud_AuditAdjustmentObservationOnSalesAgreement objAuditOnSalesAgreement)
        {
            using (context)
            {

                using (var Ts = new TransactionScope())
                {
                    try
                    {
                        //Aud_AuditAdjustmentObservationOnSalesAgreement dbRecordAudit = context.Aud_AuditAdjustmentObservationOnSalesAgreement.Where(a => a.CustomerCode == objAuditOnSalesAgreement.CustomerCode && a.YearMonth == objAuditOnSalesAgreement.YearMonth && a.CustomerCode == objAuditOnSalesAgreement.CustomerCode && a.AuditSeqNo == objAuditOnSalesAgreement.AuditSeqNo).FirstOrDefault();
                        //context.ApplyCurrentValues(dbRecordAudit.EntityKey.EntitySetName, objAuditOnSalesAgreement);


                        context.Aud_AuditAdjustmentObservationOnSalesAgreement.Attach(objAuditOnSalesAgreement);
                        var AuditOnSalesAgreementEntry = context.Entry(objAuditOnSalesAgreement);
                        AuditOnSalesAgreementEntry.State = EntityState.Modified;

                        context.SaveChanges();
                        Ts.Complete();
                    }
                    catch (Exception ex)
                    {
                        Ts.Dispose();
                        throw;
                    }
                }

                return objAuditOnSalesAgreement;
            }
        }

        public List<Inv_ItemStockByLocation> Update(List<Inv_ItemStockByLocation> lstItemStockByLocation, List<Inv_ItemStockWithSerialNoByLocation> lstItemStockWithSerialNoByLocation)
        {
            using (context)
            {

                using (TransactionScope tx = new TransactionScope())
                {
                    try
                    {

                        foreach (Inv_ItemStockWithSerialNoByLocation ctrd in lstItemStockWithSerialNoByLocation)
                        {
                            Inv_ItemStockWithSerialNoByLocation objItemStockWithSerialNoByLocation;
                            objItemStockWithSerialNoByLocation = context.Inv_ItemStockWithSerialNoByLocation.Where(c => c.StoreLocation == ctrd.StoreLocation && c.LocationCode == ctrd.LocationCode && c.ItemSerialNo == ctrd.ItemSerialNo && c.ItemCode == ctrd.ItemCode).FirstOrDefault();

                            objItemStockWithSerialNoByLocation.IsItUnderAuditObservation = ctrd.IsItUnderAuditObservation;
                            context.SaveChanges();
                        }

                        /*context.Sal_CustomerTrainingTransMaster.Add(objSalCustomerTrainingMaster);

                        foreach (Sal_CustomerTrainingTransDetails ctrd in lstCustomerTrainingTransDetails)
                        {
                            context.Sal_CustomerTrainingTransDetails.Add(ctrd);
                        }*/


                        Inv_ItemStockByLocation objItemStockByLocationUpdate;
                        foreach (Inv_ItemStockByLocation isbl in lstItemStockByLocation)
                        {
                            objItemStockByLocationUpdate = context.Inv_ItemStockByLocation.Where(c => c.ItemCode == isbl.ItemCode && c.StoreLocation == isbl.StoreLocation && c.LocationCode == isbl.LocationCode).FirstOrDefault();
                            objItemStockByLocationUpdate.QuantityUnderAuditObservation = isbl.QuantityUnderAuditObservation;
                            context.SaveChanges();
                        }


                        //  context.AESAL_CustomerTrainingExpensesV2(objSalCustomerTrainingMaster.UnitCode, objSalCustomerTrainingMaster.TrainingDate.ToString("dd-MMM-yy"), objSalCustomerTrainingMaster.TrainingBatchNo, objSalCustomerTrainingMaster.CreatedBy, Helper.Insert);

                        tx.Complete();
                    }
                    catch (Exception ex)
                    {
                        tx.Dispose();
                        throw;
                    }
                }

                return lstItemStockByLocation;
            }
        }

        public Sal_ODCustomerGrading UpdateODRecoveryStatusMonitoring(Sal_ODCustomerGrading objODCustomerGrading)
        {
            using (context)
            {
                using (var Ts = new TransactionScope())
                {
                    try
                    {
                        context.Sal_ODCustomerGrading.Attach(objODCustomerGrading);
                        var ODCustomerGrading = context.Entry(objODCustomerGrading);

                        ODCustomerGrading.Property(p => p.LastUMNextRecoveryDateIfRemainingODBalance).IsModified = true;
                        ODCustomerGrading.Property(p => p.LastUMRemarks).IsModified = true;
                        ODCustomerGrading.Property(p => p.LastUMRemarksOn).IsModified = true;

                        ODCustomerGrading.Property(p => p.YearMonth).IsModified = false;
                        ODCustomerGrading.Property(p => p.CustomerCode).IsModified = false;
                        ODCustomerGrading.Property(p => p.NumberOfInstallment).IsModified = false;
                        ODCustomerGrading.Property(p => p.NoOfMonthPassed).IsModified = false;
                        ODCustomerGrading.Property(p => p.DueNoOfInstllment).IsModified = false;
                        ODCustomerGrading.Property(p => p.OverdueBalance).IsModified = false;
                        ODCustomerGrading.Property(p => p.InstallmentSize).IsModified = false;
                        ODCustomerGrading.Property(p => p.ApproximateNoOfODInstallmentBeforeRound).IsModified = false;
                        ODCustomerGrading.Property(p => p.ApproximateNoOfODInstallmentRounded).IsModified = false;
                        ODCustomerGrading.Property(p => p.ApproximateNoOfInstallmentPaid).IsModified = false;
                        ODCustomerGrading.Property(p => p.OutstandingBalance).IsModified = false;
                        ODCustomerGrading.Property(p => p.ODGradeThisMonth).IsModified = false;
                        ODCustomerGrading.Property(p => p.EmployeeAsFPR).IsModified = false;
                        ODCustomerGrading.Property(p => p.CustomerUnit).IsModified = false;

                        context.SaveChanges();
                        Ts.Complete();

                    }
                    //catch (DbEntityValidationException ex)
                    //{
                    //    StringBuilder sb = new StringBuilder();

                    //    foreach (var failure in ex.EntityValidationErrors)
                    //    {
                    //        sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    //        foreach (var error in failure.ValidationErrors)
                    //        {
                    //            sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                    //            sb.AppendLine();
                    //        }
                    //    }

                    //    Ts.Dispose();

                    //    throw new DbEntityValidationException(
                    //        "Entity Validation Failed - errors follow:\n" +
                    //        sb.ToString(), ex
                    //    ); // Add the original exception as the innerException
                    //}
                    catch (Exception ex)
                    {
                        Ts.Dispose();
                        throw;
                    }
                }
                return objODCustomerGrading;
            }
        }

        public Sal_ODRecoveryCommitmentByRMnZM UpdateODRecoveryCommitmentByRMnZM(Sal_ODRecoveryCommitmentByRMnZM objODRecoveryCommitmentByRMnZM)
        {
            using (context)
            {

                using (var Ts = new TransactionScope())
                {
                    try
                    {
                        context.Sal_ODRecoveryCommitmentByRMnZM.Attach(objODRecoveryCommitmentByRMnZM);
                        var ODRecoveryCommitmentEntry = context.Entry(objODRecoveryCommitmentByRMnZM);

                        if (!string.IsNullOrEmpty(objODRecoveryCommitmentByRMnZM.LastRMCommitmentToRecoverRemainingOD))
                        {
                            ODRecoveryCommitmentEntry.Property(p => p.LastRMCommitmentToRecoverRemainingOD).IsModified = true;
                            ODRecoveryCommitmentEntry.Property(p => p.LastRMRemarksOn).IsModified = true;
                        }

                        if (!string.IsNullOrEmpty(objODRecoveryCommitmentByRMnZM.LastZMCommitmentToRecoverRemainingOD))
                        {
                            ODRecoveryCommitmentEntry.Property(p => p.LastZMCommitmentToRecoverRemainingOD).IsModified = true;
                            ODRecoveryCommitmentEntry.Property(p => p.LastZMRemarksOn).IsModified = true;
                        }

                        context.SaveChanges();
                        Ts.Complete();

                    }
                    catch (Exception ex)
                    {
                        Ts.Dispose();
                        throw;
                    }
                }
                return objODRecoveryCommitmentByRMnZM;
            }
        }

        public List<SalesRecoveryStatusEntryMonitoring> UpdateSalesRecoveryStatusEntryMonitoring(string option, string locationCode, string customerCode, DateTime? umLastNextRecoveryDate, string umLastRemarks, string umLastOverallRemarks, string amLastRemarks)
        {
            SqlParameter[] storeParam = new SqlParameter[7];
            storeParam[0] = new SqlParameter("prmOptions", option);

            storeParam[1] = new SqlParameter("prmLocationCode", locationCode);

            if (locationCode != null)
            {
                if (customerCode != null)
                    storeParam[2] = new SqlParameter("prmCustomerCode", customerCode);
                else
                    storeParam[2] = new SqlParameter("prmCustomerCode", " ");
            }
            else
                storeParam[2] = new SqlParameter("prmCustomerCode", DBNull.Value);

            if (umLastNextRecoveryDate != null)
                storeParam[3] = new SqlParameter("prmUMLastNextRecoveryDate", umLastNextRecoveryDate);
            else
                storeParam[3] = new SqlParameter("prmUMLastNextRecoveryDate", " ");
            if (umLastRemarks != null)
                storeParam[4] = new SqlParameter("prmUMLastRemarks", umLastRemarks);
            else
                storeParam[4] = new SqlParameter("prmUMLastRemarks", " ");
            storeParam[5] = new SqlParameter("prmUMLastOverallRemarks", umLastOverallRemarks);
            storeParam[6] = new SqlParameter("prmAMLastRemarks", amLastRemarks);

            ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext.CommandTimeout = 3600;
            var resultList = context.Database.SqlQuery<SalesRecoveryStatusEntryMonitoring>("Exec SP_DailyBusinessPerformanceMonitoring_UpdateSalesAndRecoveryStatus @prmOptions, @prmLocationCode, @prmCustomerCode,@prmUMLastNextRecoveryDate, @prmUMLastRemarks, @prmUMLastOverallRemarks, @prmAMLastRemarks", storeParam);

            return resultList.ToList();
        }


        public void CustomerCollectionSaveForUpdateOrDelete(string updateDeleteOptions, Sal_CollectionFromCustomers objCollectionFromCustomers)
        {
            using (context)
            {
                using (var TS = new TransactionScope())
                {
                    try
                    {
                        if (updateDeleteOptions != Helper.Delete && updateDeleteOptions != Helper.Update)
                        {
                            string cashMemoId = string.Empty;

                            if (string.IsNullOrEmpty(objCollectionFromCustomers.RefMemoNo))
                            {
                                SqlParameter[] storeParamForCashmemo = new SqlParameter[6];
                                storeParamForCashmemo[0] = new SqlParameter("prmEntrySource", "COLCRECEIV");  //COLCAUDADJ
                                storeParamForCashmemo[1] = new SqlParameter("prmCashMemoNo", objCollectionFromCustomers.CashMemoNo);
                                storeParamForCashmemo[2] = new SqlParameter("prmCashMemoUsesID", Helper.CashMemuUsesIdFirst);
                                storeParamForCashmemo[3] = new SqlParameter("prmLocationCode", objCollectionFromCustomers.RefAELocationCode);
                                storeParamForCashmemo[4] = new SqlParameter("prmRefDocNo", objCollectionFromCustomers.CustomerCode);

                                if (string.IsNullOrEmpty(objCollectionFromCustomers.CollectedByEmployee))
                                    storeParamForCashmemo[5] = new SqlParameter("prmCashMemoUsedByEmployee", Helper.DBNullValue);
                                else
                                    storeParamForCashmemo[5] = new SqlParameter("prmCashMemoUsedByEmployee", objCollectionFromCustomers.CollectedByEmployee);

                                var v = context.Database.SqlQuery<string>("SP_CheckNSaveCashMemoNoInCashMemoMasterNTransactionRegisterTables @prmEntrySource, @prmCashMemoNo, @prmCashMemoUsesID, @prmLocationCode, @prmRefDocNo, @prmCashMemoUsedByEmployee", storeParamForCashmemo);

                                if (v != null)
                                    cashMemoId = v.FirstOrDefault().ToString();

                                objCollectionFromCustomers.CashMemoUsesID = cashMemoId;
                            }
                        }

                        context.Sal_CollectionFromCustomers.Attach(objCollectionFromCustomers);
                        var CustomerCollectionEntry = context.Entry(objCollectionFromCustomers);
                        CustomerCollectionEntry.State = EntityState.Modified;

                        CustomerCollectionEntry.Property(p => p.CashMemoNo).IsModified = false;
                        CustomerCollectionEntry.Property(p => p.RefMemoNo).IsModified = false;
                        CustomerCollectionEntry.Property(p => p.CashMemoUsesID).IsModified = false;

                        context.SaveChanges();

                        //SqlParameter[] storeParam = new SqlParameter[5];
                        //storeParam[0] = new SqlParameter("prmCustomerCode", objCollectionFromCustomers.CustomerCode);
                        //storeParam[1] = new SqlParameter("prmYearMonth", objCollectionFromCustomers.YearMonth);
                        //storeParam[2] = new SqlParameter("prmEntrySerialNo", objCollectionFromCustomers.SerialNo);
                        //storeParam[3] = new SqlParameter("prmUserID", objCollectionFromCustomers.UserID);
                        //storeParam[4] = new SqlParameter("prmDBTransType", updateDeleteOptions);

                        //((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext.CommandTimeout = 300;
                        //context.Database.ExecuteSqlCommand("Exec Sal_InsertOrChangeCurrentOrBackdatedCollectionNUpdateCustomerStatus @prmCustomerCode, @prmYearMonth, @prmEntrySerialNo, @prmUserID, @prmDBTransType", storeParam);

                        SqlParameter[] storeParam = new SqlParameter[6];
                        storeParam[0] = new SqlParameter("prmCustomerCode", objCollectionFromCustomers.CustomerCode);
                        storeParam[1] = new SqlParameter("prmYearMonth", objCollectionFromCustomers.YearMonth);
                        storeParam[2] = new SqlParameter("prmEntrySerialNo", objCollectionFromCustomers.SerialNo);
                        storeParam[3] = new SqlParameter("prmUserID", objCollectionFromCustomers.UserID);


                       if (objCollectionFromCustomers.CashMemoNo=="")
                            storeParam[4] = new SqlParameter("prmChangedCashMemoNo", objCollectionFromCustomers.CashMemoNo);
                       else if (String.IsNullOrEmpty(objCollectionFromCustomers.CashMemoNo))
                            storeParam[4] = new SqlParameter("prmChangedCashMemoNo", objCollectionFromCustomers.RefMemoNo);
                        else
                            storeParam[4] = new SqlParameter("prmChangedCashMemoNo", objCollectionFromCustomers.CashMemoNo);



                        if (updateDeleteOptions == Helper.Update)
                        {
                            storeParam[5] = new SqlParameter("prmDBTransType", "UPDATE");
                        }
                        else if (updateDeleteOptions == Helper.Delete)
                        {
                            storeParam[5] = new SqlParameter("prmDBTransType", "DELETE");
                        }

                        ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext.CommandTimeout = 300;
                        context.Database.ExecuteSqlCommand("SP_Aud_InsertOrChangeCurrentOrBackdatedCollectionNUpdateCustomerStatus @prmCustomerCode, @prmYearMonth, @prmEntrySerialNo, @prmUserID, @prmChangedCashMemoNo, @prmDBTransType", storeParam);

                        TS.Complete();
                    }
                    catch (Exception ex)
                    {
                        TS.Dispose();
                        throw;
                    }
                }
            }
        }

        public byte getCustomerCollectionEntrySerialNumber(string customerCode, string collectionDate)
        {
            return context.Sal_CollectionFromCustomers.Where(a => a.CustomerCode == customerCode && a.YearMonth == collectionDate).Select(s => s.SerialNo).FirstOrDefault();
        }

        public Fix_EmployeeWiseFixedAssetsAllocation UpdateCashmemoAssign(Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo objEmployeeWiseFixedAssetsAllocationWithSerialNo, Fix_EmployeeWiseFixedAssetsAllocation objAssetAssign)
        {
            using (context)
            {

                using (var Ts = new TransactionScope())
                {
                    try
                    {
                        context.Fix_EmployeeWiseFixedAssetsAllocation.Add(objAssetAssign);
                        context.Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo.Add(objEmployeeWiseFixedAssetsAllocationWithSerialNo);

                        context.SaveChanges();

                        SqlParameter[] storeParam = new SqlParameter[7];
                        storeParam[0] = new SqlParameter("prmAssignOrUnassign", "UNASSIGN");
                        storeParam[1] = new SqlParameter("prmEmployeeID", objEmployeeWiseFixedAssetsAllocationWithSerialNo.EmployeeID);
                        storeParam[2] = new SqlParameter("prmStoreLocation", objEmployeeWiseFixedAssetsAllocationWithSerialNo.StoreLocation);
                        storeParam[3] = new SqlParameter("prmLocationCode", objEmployeeWiseFixedAssetsAllocationWithSerialNo.LocationCode);
                        storeParam[4] = new SqlParameter("prmItemCode", objEmployeeWiseFixedAssetsAllocationWithSerialNo.ItemCode);
                        storeParam[5] = new SqlParameter("prmAllocationSeqNo", objEmployeeWiseFixedAssetsAllocationWithSerialNo.AllocationSeqNo);
                        storeParam[6] = new SqlParameter("prmDBTransType", Helper.Update);

                        context.Database.ExecuteSqlCommand("SP_FixAssignOrUnassignItemWithEmployee @prmAssignOrUnassign, @prmEmployeeID, @prmStoreLocation, @prmLocationCode, @prmItemCode, @prmAllocationSeqNo, @prmDBTransType", storeParam);

                        Ts.Complete();
                    }
                    catch (Exception ex)
                    {
                        Ts.Dispose();
                        throw;
                    }
                }
                return objAssetAssign;
            }
        }

        public Sal_CustomerStatus UpdateCustomerStatusForSalesMonitoring(Sal_CustomerStatus objCustomerStatus)
        {
          
            try
            {
                Sal_CustomerStatus objCustomer = new Sal_CustomerStatus();
                objCustomer = context.Sal_CustomerStatus.Where(c => c.CustomerCode == objCustomerStatus.CustomerCode).FirstOrDefault();

                objCustomer.IsCustomerPoliticallyInfluential = objCustomerStatus.IsCustomerPoliticallyInfluential;
                objCustomer.IsCustomerNotWillingToPay = objCustomerStatus.IsCustomerNotWillingToPay;
                objCustomer.IsCustomerALocalMuscleMan = objCustomerStatus.IsCustomerALocalMuscleMan;

                context.SaveChanges();

            }
            catch (Exception ex)
            {
                throw;
            }

            return objCustomerStatus;
        }

        public Inv_ERPVersusPhysicalBalance Update(Inv_ERPVersusPhysicalBalance objInvERPVersusPhysicalBalance, List<Inv_ERPVersusPhysicalBalance> lstInvERPVersusPhysicalBalance)
        {
            using (context)
            {

                using (TransactionScope tx = new TransactionScope())
                {
                    try
                    {

                        Inv_ERPVersusPhysicalBalance objInvERPVersusPhysicalBalanceUpdate;
                        foreach (Inv_ERPVersusPhysicalBalance ipb in lstInvERPVersusPhysicalBalance)
                        {
                            objInvERPVersusPhysicalBalanceUpdate = context.Inv_ERPVersusPhysicalBalance.Where(c => c.ItemCode == ipb.ItemCode && c.StoreLocation == ipb.StoreLocation && c.LocationCode == ipb.LocationCode && c.YearMonth == ipb.YearMonth).FirstOrDefault();
                            objInvERPVersusPhysicalBalanceUpdate.PhysicalBalance = ipb.PhysicalBalance;
                            objInvERPVersusPhysicalBalanceUpdate.ModifiedBy = objInvERPVersusPhysicalBalance.ModifiedBy;
                            objInvERPVersusPhysicalBalanceUpdate.ModifiedOn = objInvERPVersusPhysicalBalance.ModifiedOn;
                            context.SaveChanges();
                        }


                        tx.Complete();
                    }
                    catch (Exception ex)
                    {
                        tx.Dispose();
                        throw;
                    }
                }

                return objInvERPVersusPhysicalBalance;
            }
        }

        public SHSDistributionPlan_Master Update(SHSDistributionPlan_Master objSHSDistributionPlan_Master, List<SHSDistributionPlan_RootWiseLocation> lstSHSDistributionPlan_RootWiseLocation, List<SHSDistributionPlan_RootWiseLocationNPackage> lstSHSDistributionPlan_RootWiseLocationNPackage, List<SHSDistributionPlan_IndividualItem> lstSHSDistributionPlan_IndividualItem)
        {
            using (context)
            {

                using (TransactionScope tx = new TransactionScope())
                {

                    try
                    {


                        //Inv_VendorChallanWithItemReferenceToCheck VendorChallanWithItemReferenceToCheck = (from gm in context.Inv_VendorChallanWithItemReferenceToCheck
                        //                               where (gm.memberID == memberid) && (gm.groupID == groupid)
                        //                               select gm).SingleOrDefault();


                        //dataContext.GroupMembers.DeleteOnSubmit(removeFromGroup);
                        //dataContext.SubmitChanges();

                        string sqlQuery1 = "DELETE FROM Inv_VendorChallanWithItemReferenceToCheck WHERE RefDistribScheduleNo = '"+objSHSDistributionPlan_Master.DistribScheduleNo+"' ";
                        context.Database.ExecuteSqlCommand(sqlQuery1);

                        string sqlQuery2 = "DELETE FROM SHSDistributionPlan_PackageWiseItem WHERE DistribScheduleNo= '" + objSHSDistributionPlan_Master.DistribScheduleNo + "' ";
                        context.Database.ExecuteSqlCommand(sqlQuery2);

                        string sqlQuery3 = "DELETE FROM SHSDistributionPlan_IndividualItem WHERE DistribScheduleNo = '" + objSHSDistributionPlan_Master.DistribScheduleNo + "' ";
                        context.Database.ExecuteSqlCommand(sqlQuery3);

                        string sqlQuery4 = "DELETE FROM SHSDistributionPlan_RootWiseLocationNPackage WHERE DistribScheduleNo= '" + objSHSDistributionPlan_Master.DistribScheduleNo + "' ";
                        context.Database.ExecuteSqlCommand(sqlQuery4);

                        string sqlQuery5 = "DELETE FROM SHSDistributionPlan_RootWiseLocation WHERE DistribScheduleNo= '" + objSHSDistributionPlan_Master.DistribScheduleNo + "' ";
                        context.Database.ExecuteSqlCommand(sqlQuery5);

                        context.SaveChanges();
                        //context.SHSDistributionPlan_Master.Add(objSHSDistributionPlan_Master);

                        foreach (SHSDistributionPlan_RootWiseLocation srwl in lstSHSDistributionPlan_RootWiseLocation)
                        {
                            context.SHSDistributionPlan_RootWiseLocation.Add(srwl);
                        }

                        foreach (SHSDistributionPlan_RootWiseLocationNPackage trd in lstSHSDistributionPlan_RootWiseLocationNPackage)
                        {
                            context.SHSDistributionPlan_RootWiseLocationNPackage.Add(trd);
                        }

                        foreach (SHSDistributionPlan_IndividualItem sdpItems in lstSHSDistributionPlan_IndividualItem)
                        {
                            context.SHSDistributionPlan_IndividualItem.Add(sdpItems);
                        }

                        context.SaveChanges();


                        SqlParameter[] storeParam = new SqlParameter[2];
                        storeParam[0] = new SqlParameter("DistribScheduleNo ", objSHSDistributionPlan_Master.DistribScheduleNo);
                        storeParam[1] = new SqlParameter("prmOption", "UPDATE");
                        
                        ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext.CommandTimeout = 10000;

                        context.Database.ExecuteSqlCommand("Exec SP_SHSDistributionPlanUpdatePackageWiseItem  @DistribScheduleNo, @prmOption", storeParam);

                        tx.Complete();

                        return objSHSDistributionPlan_Master;
                    }

                    catch (Exception ex)
                    {
                        tx.Dispose();
                        throw;
                    }
                }
            }
        }


        public string SaveSHSDistributionRouteTransfer(string routeid, string txtDistributionScheduleNo, string txtDistributionScheduleNoNew, string dtpDelivaryDate, string  txtDeliveryScheduleNo)
        {
         
            string successMessage = string.Empty;
             
            using (context)
            {

                using (TransactionScope tx = new TransactionScope())
                {

                    try
                    {

                        string [] routID = routeid.Split('_');

                        string routeIDFormated = "";
                        for (int k = 0; k < routID.Length-1; k++ )
                        {
                            if (k != (routID.Length -2)) //&& k != (routID.Length - 1))
                            {
                              routeIDFormated = routeIDFormated + "'" + routID [k]+ "',";
                            }
                            else if (k == (routID.Length - 2))
                            {
                                routeIDFormated = routeIDFormated + "'" + routID[k] + "'";
                            }
                          
                        }

                        //string sqlQueryStrng =    " IF NOT EXISTS (SELECT * FROM SHSDistributionPlan_Master WHERE DistribScheduleNo = '"+txtDistributionScheduleNoNew+"') "
                        //                        + "   BEGIN "
                        //                        + "       INSERT INTO [SHSDistributionPlan_Master] "
                        //                        + "       SELECT '" + txtDistributionScheduleNoNew + "' DistribScheduleNo,'" + dtpDelivaryDate + "' ScheduleDate,ScheduleNotes,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,'" + txtDeliveryScheduleNo + "' RefScheduleNo,[Status]  "
                        //                        + "        FROM SHSDistributionPlan_Master WHERE DistribScheduleNo = '"+txtDistributionScheduleNo+"' END "


                        //                         + "           INSERT INTO SHSDistributionPlan_RootWiseLocation   "
                        //                         + "  SELECT '" + txtDistributionScheduleNoNew + "' DistribScheduleNo,RouteNo,LocationCode,VendorChallanNoForPackage,VendorChallanNoForSpareParts "
                        //                         + "   FROM SHSDistributionPlan_RootWiseLocation WHERE DistribScheduleNo = '" + txtDistributionScheduleNo + "' AND RouteNo IN ("+routeIDFormated+") "

                                           
                        //                         + " INSERT INTO SHSDistributionPlan_RootWiseLocationForTemporary "
                        //                         + " SELECT '" + txtDistributionScheduleNoNew + "' DistribScheduleNo,RouteNo,LocationCode,VendorChallanNoForPackage,VendorChallanNoForSpareParts "
                        //                         + "   FROM SHSDistributionPlan_RootWiseLocationForTemporary WHERE DistribScheduleNo = '" + txtDistributionScheduleNo + "' AND RouteNo IN (" + routeIDFormated + ") "

                        //                         + " INSERT INTO SHSDistributionPlan_RootWiseLocationNPackage "
                        //                         + " SELECT '" + txtDistributionScheduleNoNew + "' DistribScheduleNo,RouteNo,LocationCode,PackageCode,PkgSequenceNo,PanelModel,BatteryModel,PackageQuantity "
                        //                         + "   FROM SHSDistributionPlan_RootWiseLocationNPackage WHERE DistribScheduleNo = '" + txtDistributionScheduleNo + "' AND RouteNo IN (" + routeIDFormated + ") "

                        //                         + " INSERT INTO SHSDistributionPlan_IndividualItem  "
                        //                         + " SELECT '" + txtDistributionScheduleNoNew + "' DistribScheduleNo,RouteNo,LocationCode,ItemCode,ItemQuantity "
                        //                         + "    FROM SHSDistributionPlan_IndividualItem WHERE DistribScheduleNo = '" + txtDistributionScheduleNo + "' AND RouteNo IN (" + routeIDFormated + ") "

                        //                         + "    INSERT INTO SHSDistributionPlan_PackageWiseItem   "
                        //                         + "   SELECT '" + txtDistributionScheduleNoNew + "' DistribScheduleNo,RouteNo,LocationCode,PackageCode,PkgSequenceNo,ItemCode,ItemQuantity "
                        //                         + "          FROM SHSDistributionPlan_PackageWiseItem WHERE DistribScheduleNo = '" + txtDistributionScheduleNo + "' AND RouteNo IN (" + routeIDFormated + ")  "


                        //                         + "   UPDATE Inv_VendorChallanWithItemReferenceToCheck SET VendorRefChallanDate = '"+dtpDelivaryDate+"',RefDistribScheduleNo = '" + txtDistributionScheduleNoNew + "' "
                        //                         + "        WHERE RefDistribScheduleNo = '" + txtDistributionScheduleNo + "' AND VendorDeliveryRouteNo IN (" + routeIDFormated + ") "

                        //                         + "    DELETE FROM Inv_VendorChallanWithItemReferenceToCheck WHERE  RefDistribScheduleNo = '" + txtDistributionScheduleNo + "' AND VendorDeliveryRouteNo IN (" + routeIDFormated + ") "
                        //                         + "    DELETE FROM SHSDistributionPlan_PackageWiseItem WHERE DistribScheduleNo = '" + txtDistributionScheduleNo + "' AND RouteNo IN (" + routeIDFormated + ") "
                        //                         + "   DELETE FROM SHSDistributionPlan_IndividualItem WHERE DistribScheduleNo = '" + txtDistributionScheduleNo + "' AND RouteNo IN (" + routeIDFormated + ") "
                        //                         + "     DELETE FROM SHSDistributionPlan_RootWiseLocationNPackage WHERE DistribScheduleNo = '" + txtDistributionScheduleNo + "' AND RouteNo IN (" + routeIDFormated + ") "
                        //                         + "    DELETE FROM SHSDistributionPlan_RootWiseLocation WHERE DistribScheduleNo = '" + txtDistributionScheduleNo + "' AND RouteNo IN (" + routeIDFormated + ") "
                        //                         + "     DELETE FROM [SHSDistributionPlan_RootWiseLocationForTemporary] WHERE DistribScheduleNo = '" + txtDistributionScheduleNo + "' AND RouteNo IN (" + routeIDFormated + ") "

                        //                         + "    IF NOT EXISTS (SELECT * FROM SHSDistributionPlan_RootWiseLocation WHERE DistribScheduleNo = '" + txtDistributionScheduleNo + "') BEGIN   DELETE FROM SHSDistributionPlan_Master WHERE DistribScheduleNo = '" + txtDistributionScheduleNo + "'      END    ";



                        string sqlQueryStrng =    "   IF NOT EXISTS (SELECT * FROM SHSDistributionPlan_Master WHERE DistribScheduleNo = '" + txtDistributionScheduleNoNew + "') "
                                                + "   BEGIN "
                                                + "       INSERT INTO [SHSDistributionPlan_Master] "
                                                + "       SELECT '" + txtDistributionScheduleNoNew + "' DistribScheduleNo,'20'+LEFT('" + txtDistributionScheduleNoNew + "',2)+'-'+RIGHT(LEFT('" + txtDistributionScheduleNoNew + "',4),2)+'-'+RIGHT('" + txtDistributionScheduleNoNew + "',2)  ScheduleDate,ScheduleNotes,CreatedBy,CreatedOn,ModifiedBy, "
                                                + "   ModifiedOn,'RSF/SCH-'+'20'+LEFT('" + txtDistributionScheduleNoNew + "',2)+'-'+RIGHT(LEFT('" + txtDistributionScheduleNoNew + "',4),2)+RIGHT('" + txtDistributionScheduleNoNew + "',2) RefScheduleNo,[Status]  "
                                                + "        FROM SHSDistributionPlan_Master WHERE DistribScheduleNo = '" + txtDistributionScheduleNo + "' END "


                                                 + "           INSERT INTO SHSDistributionPlan_RootWiseLocation   "
                                                 + "  SELECT '" + txtDistributionScheduleNoNew + "' DistribScheduleNo,RouteNo,LocationCode,VendorChallanNoForPackage,VendorChallanNoForSpareParts "
                                                 + "   FROM SHSDistributionPlan_RootWiseLocation WHERE DistribScheduleNo = '" + txtDistributionScheduleNo + "' AND RouteNo IN (" + routeIDFormated + ") "


                                                 + " INSERT INTO SHSDistributionPlan_RootWiseLocationForTemporary "
                                                 + " SELECT '" + txtDistributionScheduleNoNew + "' DistribScheduleNo,RouteNo,LocationCode,VendorChallanNoForPackage,VendorChallanNoForSpareParts "
                                                 + "   FROM SHSDistributionPlan_RootWiseLocationForTemporary WHERE DistribScheduleNo = '" + txtDistributionScheduleNo + "' AND RouteNo IN (" + routeIDFormated + ") "

                                                 + " INSERT INTO SHSDistributionPlan_RootWiseLocationNPackage "
                                                 + " SELECT '" + txtDistributionScheduleNoNew + "' DistribScheduleNo,RouteNo,LocationCode,PackageCode,PkgSequenceNo,PanelModel,BatteryModel,PackageQuantity "
                                                 + "   FROM SHSDistributionPlan_RootWiseLocationNPackage WHERE DistribScheduleNo = '" + txtDistributionScheduleNo + "' AND RouteNo IN (" + routeIDFormated + ") "

                                                 + " INSERT INTO SHSDistributionPlan_IndividualItem  "
                                                 + " SELECT '" + txtDistributionScheduleNoNew + "' DistribScheduleNo,RouteNo,LocationCode,ItemCode,ItemQuantity "
                                                 + "    FROM SHSDistributionPlan_IndividualItem WHERE DistribScheduleNo = '" + txtDistributionScheduleNo + "' AND RouteNo IN (" + routeIDFormated + ") "

                                                 + "    INSERT INTO SHSDistributionPlan_PackageWiseItem   "
                                                 + "   SELECT '" + txtDistributionScheduleNoNew + "' DistribScheduleNo,RouteNo,LocationCode,PackageCode,PkgSequenceNo,ItemCode,ItemQuantity "
                                                 + "          FROM SHSDistributionPlan_PackageWiseItem WHERE DistribScheduleNo = '" + txtDistributionScheduleNo + "' AND RouteNo IN (" + routeIDFormated + ")  "


                                                 + "   UPDATE Inv_VendorChallanWithItemReferenceToCheck "
                                                 + "  SET VendorRefChallanDate = '20'+LEFT('" + txtDistributionScheduleNoNew + "',2)+'-'+RIGHT(LEFT('" + txtDistributionScheduleNoNew + "',4),2)+'-'+RIGHT('" + txtDistributionScheduleNoNew + "',2),RefDistribScheduleNo = '" + txtDistributionScheduleNoNew + "' "
                                                 + "   WHERE RefDistribScheduleNo = '" + txtDistributionScheduleNo + "' AND VendorDeliveryRouteNo IN (" + routeIDFormated + ") "

                                                 + "    DELETE FROM Inv_VendorChallanWithItemReferenceToCheck WHERE  RefDistribScheduleNo = '" + txtDistributionScheduleNo + "' AND VendorDeliveryRouteNo IN (" + routeIDFormated + ") "
                                                 + "    DELETE FROM SHSDistributionPlan_PackageWiseItem WHERE DistribScheduleNo = '" + txtDistributionScheduleNo + "' AND RouteNo IN (" + routeIDFormated + ") "
                                                 + "   DELETE FROM SHSDistributionPlan_IndividualItem WHERE DistribScheduleNo = '" + txtDistributionScheduleNo + "' AND RouteNo IN (" + routeIDFormated + ") "
                                                 + "     DELETE FROM SHSDistributionPlan_RootWiseLocationNPackage WHERE DistribScheduleNo = '" + txtDistributionScheduleNo + "' AND RouteNo IN (" + routeIDFormated + ") "
                                                 + "    DELETE FROM SHSDistributionPlan_RootWiseLocation WHERE DistribScheduleNo = '" + txtDistributionScheduleNo + "' AND RouteNo IN (" + routeIDFormated + ") "
                                                 + "     DELETE FROM [SHSDistributionPlan_RootWiseLocationForTemporary] WHERE DistribScheduleNo = '" + txtDistributionScheduleNo + "' AND RouteNo IN (" + routeIDFormated + ") "

                                                 + "    IF NOT EXISTS (SELECT * FROM SHSDistributionPlan_RootWiseLocation WHERE DistribScheduleNo = '" + txtDistributionScheduleNo + "') BEGIN   DELETE FROM SHSDistributionPlan_Master WHERE DistribScheduleNo = '" + txtDistributionScheduleNo + "'      END    ";

                        
                        ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext.CommandTimeout = 10000;

                        context.Database.ExecuteSqlCommand(sqlQueryStrng);

                        context.SaveChanges();

                        tx.Complete();

                        return successMessage;
                    }

                    catch (Exception ex)
                    {
                        tx.Dispose();
                        throw;
                    }
                }
            }
        
        
        }


        #endregion

        #region Delete

        public void Delete(object obj)
        {
            //context.DeleteObject(obj);
            //context.SaveChanges();
        }

        public void Delete(object obj1, object obj2)
        {
            try
            {
                //context.DeleteObject(obj2);
                //context.DeleteObject(obj1);
                //context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Delete(Inv_ItemNItemCategoryWithSerialNoMaster objItemSerialMaster, Inv_ItemStockWithSerialNoByLocation objItemSerials)
        {
            using (context)
            {

                using (TransactionScope Tx = new TransactionScope())
                {
                    try
                    {
                        Inv_ItemStockByLocation objItemStockByLocation = context.Inv_ItemStockByLocation.Where(c => c.StoreLocation == objItemSerials.StoreLocation && c.LocationCode == objItemSerials.LocationCode && c.ItemCode == objItemSerials.ItemCode).FirstOrDefault();
                        objItemStockByLocation.SerialQuantity -= 1;

                        Inv_ItemNItemCategoryWithSerialNoMaster objItemSerialMasterDb = context.Inv_ItemNItemCategoryWithSerialNoMaster.Where(c => c.ItemCode == objItemSerials.ItemCode && c.ItemSerialNo == objItemSerials.ItemSerialNo).FirstOrDefault();
                        objItemSerialMasterDb.RefLastStoreLocation = null;
                        objItemSerialMasterDb.RefLastLocationCode = null;
                        context.SaveChanges();

                        context.Inv_ItemStockWithSerialNoByLocation.Attach(objItemSerials);
                        context.Inv_ItemStockWithSerialNoByLocation.Remove(objItemSerials);

                        context.Inv_ItemNItemCategoryWithSerialNoMaster.Attach(objItemSerialMaster);
                        context.Inv_ItemNItemCategoryWithSerialNoMaster.Remove(objItemSerialMaster);

                        context.SaveChanges();

                        Tx.Complete();
                    }
                    catch (Exception ex)
                    {
                        Tx.Dispose();
                        throw;
                    }
                }
            }
        }

        public void CustomerCollectionDelete(Sal_CollectionFromCustomersPrePost objCollectionFromCustomers)
        {
            using (context)
            {
                using (TransactionScope tx = new TransactionScope())
                {
                    try
                    {
                        SqlParameter[] storeParam = new SqlParameter[5];
                        storeParam[0] = new SqlParameter("prmCustomerCode", objCollectionFromCustomers.CustomerCode);
                        storeParam[1] = new SqlParameter("prmYearMonth", objCollectionFromCustomers.YearMonth);
                        storeParam[2] = new SqlParameter("prmEntrySerialNo", objCollectionFromCustomers.SerialNo);
                        storeParam[3] = new SqlParameter("prmChangedCashMemoNo", string.Empty);
                        storeParam[4] = new SqlParameter("prmDBTransType", "DELETE");

                        context.Database.ExecuteSqlCommand("AE_Sal_CollectionReceiveWithoutTransactionV3 @prmCustomerCode, @prmYearMonth, @prmEntrySerialNo, @prmChangedCashMemoNo, @prmDBTransType", storeParam);

                        tx.Complete();
                    }
                    catch (Exception ex)
                    {
                        tx.Dispose();
                        throw;
                    }


                }


            }
        }


        #endregion


        #region executeSP

        public string ExecuteDayEndProcess(string Location, string CloseableDate)
        {
            string successMessage = string.Empty;

            try
            {
                successMessage = context.DayEndProcess(Location, CloseableDate, string.Empty).FirstOrDefault().ToString();
            }
            catch (Exception ex)
            {
                throw;
            }

            return successMessage;
        }


        //UpdateLoadRequestEntry
        public string UpdateLoadRequestEntry(string locationCode, string employeeID, string dayOpenDay, string prmCorporatePhoneNo, string rsfServiceQunt, string paywellServiceQunt, string cashMemo)
        {
            string successMessage = string.Empty;
            using (context)
            {

                using (TransactionScope tx = new TransactionScope())
                {

                    try
                    {
    
                        SqlParameter[] storeParam = new SqlParameter[10];
                        storeParam[0] = new SqlParameter("prmOption", "LOADREQUEST_ENTRY");
                        storeParam[1] = new SqlParameter("prmLocationCode",locationCode);
                        storeParam[2] = new SqlParameter("prmEmployeeID", employeeID);
                        storeParam[3] = new SqlParameter("prmTransDate", dayOpenDay);
                        storeParam[4] = new SqlParameter("prmLoadRequestForRSFServices", rsfServiceQunt);
                        storeParam[5] = new SqlParameter("prmCashMemoNoForRSFServices", cashMemo);
                        storeParam[6] = new SqlParameter("prmLoadRequestForPayWellServices", paywellServiceQunt);
                        storeParam[7] = new SqlParameter("prmCorporatePhoneNo", prmCorporatePhoneNo);
                        storeParam[8] = new SqlParameter("prmUserName", "Admin");
                        storeParam[9] = new SqlParameter("prmDBTransType", "UPDATE");

                        //((System.Data.Entity.Infrastructure.IObjectContextAdapter)HrmContext).ObjectContext.CommandTimeout = 180;
                        
                        ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext.CommandTimeout = 3080;

                        context.Database.ExecuteSqlCommand("Exec SP_SalLocationNEmployeeNDateWiseTransaction @prmOption, @prmLocationCode, @prmEmployeeID, @prmTransDate, @prmLoadRequestForRSFServices,@prmCashMemoNoForRSFServices, @prmLoadRequestForPayWellServices, @prmCorporatePhoneNo, @prmUserName,@prmDBTransType", storeParam);

                        context.SaveChanges();

                        tx.Complete();

                        return successMessage;
                    }

                    catch (Exception ex)
                    {
                        tx.Dispose();
                        throw;
                    }
                }
            }

        }

        public string UpdateLoadRequestEntryFinalSave(string locationCode, string dayOpenDayTocurrentdate, string txtRsfServiceTotal, string txtRsfPayWellTotal)
        {
            string successMessage = string.Empty;
            using (context)
            {

                using (TransactionScope tx = new TransactionScope())
                {

                    try
                    {

                        SqlParameter[] storeParam = new SqlParameter[10];
                        storeParam[0] = new SqlParameter("prmOption", "LOADREQUEST_FINALSAVE");
                        storeParam[1] = new SqlParameter("prmLocationCode", locationCode);
                        storeParam[2] = new SqlParameter("prmEmployeeID", ""); 
                        storeParam[3] = new SqlParameter("prmTransDate", dayOpenDayTocurrentdate);
                        storeParam[4] = new SqlParameter("prmLoadRequestForRSFServices", txtRsfServiceTotal);
                        storeParam[5] = new SqlParameter("prmCashMemoNoForRSFServices", "");
                        storeParam[6] = new SqlParameter("prmLoadRequestForPayWellServices", txtRsfPayWellTotal);
                        storeParam[7] = new SqlParameter("prmCorporatePhoneNo", "");
                        storeParam[8] = new SqlParameter("prmUserName", "Admin");
                        storeParam[9] = new SqlParameter("prmDBTransType", "UPDATE");

                        //((System.Data.Entity.Infrastructure.IObjectContextAdapter)HrmContext).ObjectContext.CommandTimeout = 180;

                        ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext.CommandTimeout = 3080;

                        context.Database.ExecuteSqlCommand("Exec SP_SalLocationNEmployeeNDateWiseTransaction @prmOption, @prmLocationCode, @prmEmployeeID, @prmTransDate, @prmLoadRequestForRSFServices, @prmCashMemoNoForRSFServices, @prmLoadRequestForPayWellServices, @prmCorporatePhoneNo, @prmUserName,@prmDBTransType", storeParam);

                        context.SaveChanges();

                        tx.Complete();

                        return successMessage;
                    }

                    catch (Exception ex)
                    {
                        tx.Dispose();
                        throw;
                    }
                }
            }
        }

   
        public string EntryEmployeeVisitPlan(string prmLocationCode, string prmEmployeeID, string prmMovementDate, string prmTTLocationCode, string prmNoFcustomerVisit, string prmPurposeOfVisit, string prmDurationOfVisitInHours, string prmUserName, string prmDBTransType)
        {
            string successMessage = string.Empty;

            using (context)
            {

                using (TransactionScope tx = new TransactionScope())
                {

                    try
                    {
                       
                        SqlParameter[] storeParam = new SqlParameter[9];
                        storeParam[0] = new SqlParameter("prmLocationCode", prmLocationCode);
                        storeParam[1] = new SqlParameter("prmEmployeeID", prmEmployeeID);
                        storeParam[2] = new SqlParameter("prmMovementDate", prmMovementDate);
                        storeParam[3] = new SqlParameter("prmTTLocationCode", prmTTLocationCode); 
                        storeParam[4] = new SqlParameter("prmNumberOfCustomerVisit", prmNoFcustomerVisit);
                        storeParam[5] = new SqlParameter("prmPurposeOfVisit", prmPurposeOfVisit);
                        storeParam[6] = new SqlParameter("prmDurationOfVisitInHours", prmDurationOfVisitInHours);
                        storeParam[7] = new SqlParameter("prmUserName", prmUserName);
                        storeParam[8] = new SqlParameter("prmDBTransType", prmDBTransType);

                        //((System.Data.Entity.Infrastructure.IObjectContextAdapter)HrmContext).ObjectContext.CommandTimeout = 180;

                        ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext.CommandTimeout = 3080;

                        context.Database.ExecuteSqlCommand("Exec SP_TeamTracking_LocationNEmployeeWiseMovementPlanEntry  @prmLocationCode, @prmEmployeeID, @prmMovementDate, @prmTTLocationCode, @prmNumberOfCustomerVisit, @prmPurposeOfVisit, @prmDurationOfVisitInHours, @prmUserName, @prmDBTransType", storeParam);

                        context.SaveChanges();

                        tx.Complete();

                        return successMessage;
                    }

                    catch (Exception ex)
                    {
                        tx.Dispose();
                        throw;
                    }
                }
            }
        }



        public void CustomerTransferV2(string customerID, string LocationCode, string CustomerTransferOrReceive, string TransferToUnit, string TransDate, string NoteForTransfer, string UserID, string DBTransType)
        {
            using (context)
            {
                using (TransactionScope tx = new TransactionScope())
                {
                    try
                    {
                        context.CustomerTransferV2(customerID.Trim(), LocationCode.Trim(), CustomerTransferOrReceive.Trim(), TransferToUnit.Trim(), TransDate.Trim(), NoteForTransfer.Trim(), UserID.Trim(), DBTransType.Trim());
                        tx.Complete();
                    }
                    catch (Exception ex)
                    {
                        tx.Dispose();
                        throw;
                    }
                }
            }
        }

        #endregion executeSP

        #region Dispose Repository

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
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

