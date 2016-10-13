using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarHelper.InventoryHelperModel
{
    public class AvailableNAssignFixedAsset
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string CapacityID { get; set; }
        public string ItemCapacity { get; set; }
        public string ItemModelID { get; set; }
        public string ItemModel { get; set; }
        public bool IsItASerializableItem { get; set; }
        public double AvailableQuantity { get; set; }
        public Int16 SerialQuantity { get; set; }
    }
}
