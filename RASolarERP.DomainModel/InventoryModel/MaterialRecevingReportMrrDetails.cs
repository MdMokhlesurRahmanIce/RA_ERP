using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarERP.DomainModel.InventoryModel
{
    public class MaterialRecevingReportMrrDetails
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemCapacity { get; set; }
        public string ItemModel { get; set; }
        public double? VendorDeliveryQuantity { get; set; }
        public double? ReceiveQuantity { get; set; }
        public double? Difference { get; set; }
        public double? VendorDeliveryValue { get; set; }
        public double? ReceiveValue { get; set; }
    }
}
