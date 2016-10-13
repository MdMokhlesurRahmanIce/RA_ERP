using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.InventoryModel
{
    public class ItemSerialCorrection
    {
        public string LocationCode { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public byte StoreLocation { get; set; }
        public string ItemSerialNo { get; set; }
        public string Availability { get; set; }
        public string Available { get; set; }
        public string CustomerCode { get; set; }
        public string SpareParts { get; set; }
        public string Status { get; set; }
    }
}
