using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RASolarERP.Model;
using RASolarHelper;
using RASolarERP.DomainModel.InventoryModel;

namespace RASolarERP.Web.Areas.Inventory.Models
{
    public class InventoryReportData
    {
        RASolarERPService erpService = new RASolarERPService();

        public List<InventorySummaryToDetailViewReport> ReadInventorySummaryToDetailViewReport(string yearMonth, string itemCode)
        {
            return erpService.ReadInventorySummaryToDetailViewReport(yearMonth, itemCode);
        }
        //public List<InvItemInTransit> ReadInvItemInTransit(string itemType, string locationCode, string itemCode, DateTime fromDate, DateTime toDate)
        //{
        //    return erpService.ReadInvItemInTransit( itemType,  locationCode,  itemCode,  fromDate,  toDate);
        //}

        public List<StockInTransitAtGlanceDetails> ReadInvItemInTransit(string itemType, string locationCode, string itemCode, DateTime fromDate, DateTime toDate)
        {
            return erpService.ReadInvItemInTransit(itemType, locationCode, itemCode, fromDate, toDate);
        }
    }
}