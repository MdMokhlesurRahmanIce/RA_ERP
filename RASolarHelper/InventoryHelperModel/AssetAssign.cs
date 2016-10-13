using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarHelper.InventoryHelperModel
{
    public class AssetAssign
    {
        public byte StoreLocation { get; set; }
        public string LocationCode { get; set; }
        public string EmployeeID { get; set; }
        public string ItemCode { get; set; }
        public double? UnallocatedQuantity { get; set; }
        public int? AlreadyAllocated { get; set; }
        public DateTime? AllocationDate { get; set; }
        public string Remarks { get; set; }
    }
}
