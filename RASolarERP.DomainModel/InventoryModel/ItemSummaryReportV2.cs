using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarERP.DomainModel.InventoryModel
{
    public class ItemSummaryReportV2
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemCategory { get; set; }
        public string ItemModel { get; set; }
        public double? A_OpeningBalance { get; set; }
        public double? B_ReceivedByAuditAdjustment { get; set; }
        public double? C_IssuedByAuditAdjustment { get; set; }
        public double? D_TotalReceived { get; set; }
        public double? E_TotalIssued { get; set; }
        public double? F_ClosingBalance_A_B_C_D_E_ { get; set; }
    }
}
