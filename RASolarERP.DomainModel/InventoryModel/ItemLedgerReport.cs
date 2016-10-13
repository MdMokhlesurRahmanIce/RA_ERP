using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.InventoryModel
{
    public class ItemLedgerReport
    {
        public DateTime TransDate { get; set; }
        public string Particulars { get; set; }
        public string MRRSeqNo { get; set; }
        public string ChallanSeqNo { get; set; }
        public double ReceiveQuantity { get; set; }
        public double DeliveryQuantity { get; set; }
        public double ClosingBalance { get; set; }
    }
}
