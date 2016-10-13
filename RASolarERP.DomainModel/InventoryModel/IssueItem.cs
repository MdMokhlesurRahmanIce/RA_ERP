using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.InventoryModel
{
    public class IssueItem
    {
        public string ItemCode { get; set; }
        public byte StoreLocation { get; set; }
        public double ReceivedQuantity { get; set; }
        public string ItemCategory { get; set; }
        public string ItemCapacity { get; set; }
        public string ItemModel { get; set; }
        public string ChallanSequenceNo { get; set; }
        public string ChallanNumber { get; set; }
        public string ItemSerialNO { get; set; }
    }
}
