using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RASolarERP.Model;
using RASolarHelper;
using RASolarERP.DomainModel.InventoryModel;
using RASolarHRMS.Model;

namespace RASolarERP.Web.Models
{
    public class RASolarERPData
    {
        RASolarERPService erpService = new RASolarERPService();

        public List<Common_ZoneInfo> Zone()
        {
            return erpService.Zone();
        }

        public Common_ZoneInfo Zone(string zoneCode)
        {
            return erpService.Zone(zoneCode);
        }

        public Common_RegionInfo Region(string regionCode)
        {
            return erpService.Region(regionCode);
        }

        public Common_UnitInfo Unit(string unitCode)
        {
            return erpService.Unit(unitCode);
        }

        public List<Common_RegionInfo> RegionByZoneCode(string zoneCode)
        {
            return erpService.RegionByZoneCode(zoneCode);
        }

        public List<Common_UnitInfo> UnitByRegionCode(string regionCode)
        {
            return erpService.UnitByRegionCode(regionCode);
        }

        public List<SalesDataEntryStatus> SalesEntryStatus(string reportType, string locationType, string yearMonth)
        {
            return erpService.SalesEntryStatus(reportType, locationType, yearMonth);
        }

        //public Common_UnitInfoWiseEntryStatus UnitWiseEntryStatus(string unitCode, string yearMonth)
        //{
        //    return erpService.UnitWiseEntryStatus(unitCode, yearMonth);
        //}

        //public string AuditAdjustmentOpenMonth(string unitCode)
        //{
        //    return erpService.AuditAdjustmentOpenMonth(unitCode);
        //}

        ////public Common_UnitInfoWiseEntryStatus Update(Common_UnitInfoWiseEntryStatus objUnitWiseEntryStatus)
        //{
        //    return erpService.Update(objUnitWiseEntryStatus);
        //}

        //public List<InventoryDataEntryStatus> InventoryDataEntryState(string reportType, string locationType, string yearMonth)
        //{
        //    return erpService.InventoryDataEntryState(reportType, locationType, yearMonth);
        //}

        public List<ClosingInventoryValuation> ClosingInventoryReport(string yearMonth)
        {
            return erpService.ClosingInventoryReport(yearMonth);
        }

        public List<SHSDelivaryNoteChallan> ViewDeliveryNoteChallanSHSReport(string distribScheduleNo)
        {
            return erpService.ViewDeliveryNoteChallanSHSReport(distribScheduleNo);
        }

        public List<DailyZonalPerformanceMonitoring> DailyPerformanceMonitoringZonalReport()
        {
            return erpService.DailyPerformanceMonitoringZonalReport();
        }

        //public List<InventoryAtVendorValuationByStockLocation> InventoryAtVendorValuationByStockLocation(string yearMonth) 
        //{         
        //        return erpService.InventoryAtVendorValuationByStockLocation(yearMonth);           
        //}

        //public List<tbl_InvStockInTransitByMonth> ReadStockInTransitValue(string yearMonth) 
        //{
        //    return erpService.ReadStockInTransitValue(yearMonth);
        //}

        //public List<tbl_DailyProgressReport> ReadDailyProgressReport() 
        //{
        //    return erpService.ReadDailyProgressReport();
        //}

        public List<CustomerTrainingSummary> ReadCustomerTraining(DateTime dtFromDate, DateTime dtToDate)
        {
            return erpService.ReadCustomerTrainingSummary(dtFromDate, dtToDate);
        }

        public List<CustomerTrainingDetails> ReadCustomerTrainingDetails(string dtFromDate, string dtToDate)
        {
            return erpService.ReadCustomerTrainingDetails(dtFromDate, dtToDate);
        }

        public Common_CurrentYearMonthNWeek ReadCurrentYearMonthNWeek()
        {
            return erpService.ReadCurrentYearMonthNWeek();
        }

        public LocationInfo Location(string LocationCode)
        {
            return erpService.Location(LocationCode);
        }

        public List<LocationInfo> Location()
        {
            return erpService.Location();
        }

        public List<LocationInfo> LocationWithHeadOffice()
        {
            return erpService.LocationWithHeadOffice();
        }

        public List<LocationInfo> LocationByLocationCode(string LocationCode)
        {
            return erpService.LocationByLocationCode(LocationCode);
        }

        public InventoryInTransitBalance ReadInventoryInTransitBalance(string options, string yearMonth)
        {
            return erpService.ReadInventoryInTransitBalance(options, yearMonth);
        }

        public bool IsCashMemoManagementEnabled(string companyName)
        {
            return erpService.IsCashMemoManagementEnabled(companyName);
        }

        //public List<Hrm_LocationWiseEmployee> ReadLocationWiseEmployee(string locationCode)
        //{
        //    return erpService.ReadLocationWiseEmployee(locationCode);
        //}

        public string GetCashMemoInUsesId(string entrySource, string cashMemoNo, string cashMemoUsesId, string locationCode, string refDocNo)
        {
            return erpService.GetCashMemoInUsesId(entrySource, cashMemoNo, cashMemoUsesId, locationCode, refDocNo);
        }

        public bool IsMemoStillAvailableToUse(string employeeId)
        {
            return erpService.IsMemoStillAvailableToUse(employeeId);
        }

        public Common_PeriodOpenClose ReadPeriodOpenClose(string locationCode)
        {
            return erpService.ReadPeriodOpenClose(locationCode);
        }

        public string ReadAuditSeqNumberAfterCheckFinishedDate(string locationCode)
        {
            IRASolarHRMSService HRMSService = new RASolarHRMSService() { };
            return HRMSService.ReadAuditSeqNumberAfterCheckFinishedDate(locationCode);
        }

        public void CreateUserLog(string ipAddress, string macAddress, string locationCode, string userId, string referenceEntrySource)
        {
            erpService.CreateUserLog(ipAddress, macAddress, locationCode, userId, referenceEntrySource);
        }
    }
}