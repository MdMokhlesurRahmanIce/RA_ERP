using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.InventoryModel
{
    public class Warrant_ClaimNSettlementForIssue
    {
        public string ItemName { get; set; }
        public string StoreLocation { get; set; }
        public string ItemSerialNo { get; set; }
        public string ChallanSequenceNo { get; set; }
        public string ChallanNo { get; set; }
        public double AvailableQuantity { get; set; }
        public double IssuedQuantity  { get; set; }
        public double IssuedSerialQuantity  { get; set; }
        public string ItemCode  { get; set; }
        public string ItemCapacity { get; set; }
        public string ItemModel { get; set; }
    }
}
