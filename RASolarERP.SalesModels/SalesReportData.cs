using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RASolarERP.Model;
using RASolarHelper;
using RASolarERP.DomainModel.SalesModel;

namespace RASolarERP.Web.Areas.Sales.Models
{
    public class SalesReportData
    {
        private RASolarERPService salesService = new RASolarERPService();

        public List<OverdueCollectionTargetVsAchievementByUnitOffice> ReadOverdueCollectionTargetVsAchievementByUnitOffice()
        {
            List<OverdueCollectionTargetVsAchievementByUnitOffice> lstOverdueCollectionAchievement = new List<OverdueCollectionTargetVsAchievementByUnitOffice>();
            lstOverdueCollectionAchievement = salesService.ReadOverdueCollectionTargetVsAchievementByUnitOffice("201205", "", "RSFSUMMARY");

            return lstOverdueCollectionAchievement;
        }

        public List<CollectionEfficiencyByUnitOfficeSummary> ReadCollectionEfficiencyByUnitOfficeSummary(string yearMonth, string locationCode, string reportType)
        {
            return salesService.ReadCollectionEfficiencyByUnitOfficeSummary(yearMonth, locationCode, reportType);
        }

        public List<SalesSummaryToDetailView> ReadSalesSummaryToDetailView(string yearMonth)
        {
            return salesService.ReadSalesSummaryToDetailView(yearMonth);
        }

        public List<ProgressReviewDataEntryStatusDaily> ReadProgressReviewDataEntryStatusDaily(string reportType, string locationCode, string yearMonth, string respectiveAreaUserID)
        {
            return salesService.ReadProgressReviewDataEntryStatusDaily(reportType, locationCode, yearMonth, respectiveAreaUserID);
        }

        public List<UnitCollectionVsHeadOfficePhysicalCashMovement> ReadUnitCollectionVsHeadOfficePhysicalCashMovement(string reportType, string locationCode, string yearMonth)
        {
            return salesService.ReadUnitCollectionVsHeadOfficePhysicalCashMovement(reportType, locationCode, yearMonth);
        }

        public List<CollectionEfficiencyByCustomer> ReadCollectionEfficiencyByCustomer(string locationCode, string yearMonth)
        {
            return salesService.ReadCollectionEfficiencyByCustomer(locationCode, yearMonth);
        }

        public List<SummarySheetForRegionalSalesPosting> ReadSummarySheetForRegionalSalesPosting(DateTime dateFrom, DateTime dateTo, string regionCode)
        {
            return salesService.ReadSummarySheetForRegionalSalesPosting(dateFrom, dateTo, regionCode);
        }

        public List<UnitWiseCustomerLedger> ReadUnitWiseCustomerLedger(string reportOption, string locationCode, string dateFrom, string dateTo)
        {
            return salesService.ReadUnitWiseCustomerLedger(reportOption, locationCode, dateFrom, dateTo);
        }

        public List<CollectionSheetForCustomerFPR> ReadCollectionSheetForCustomerFPR(string customerFPR, string locationCode)
        {
            return salesService.ReadCollectionSheetForCustomerFPR(customerFPR, locationCode);
        }

    }
}