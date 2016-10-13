using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarERP.DomainModel.InventoryModel
{
    public class DeliveryItemNoteReportSummary 
    {
        public string SLNo { get; set; }
        public string ItemCode { get; set; }
        public string SHSFullaccessoriesKit  { get; set; }
        public string Rate { get; set; }
        public double QtyPcs { get; set; }
        public string Amount { get; set; }
    }
}
