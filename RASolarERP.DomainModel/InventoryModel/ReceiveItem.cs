using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.InventoryModel
{
    public class ReceiveItem
    {
        public string ItemCode { get; set; }
        public string ItemType { get; set; }
        public byte StoreLocation { get; set; }
        public string ItemCategory { get; set; }
        public string ItemCapacity { get; set; }
        public string ItemModel { get; set; }
        public int ReceivedQuantity { get; set; }
        public DateTime MrrDate { get; set; }
        public string MrrSequenceNo { get; set; }
        public string MrrNumber { get; set; }
        public string Particulars { get; set; }
        public string TransactionType { get; set; }
        public string ItemSerialNo { get; set; }
        public string PhysicalSerialNumber { get; set; }
    }
}
