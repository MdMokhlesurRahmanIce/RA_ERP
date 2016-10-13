using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarERP.DomainModel.InventoryModel
{
    public class AssetAssign
    {
        public byte StoreLocation { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string ItemCode { get; set; }
        public string LocationCode { get; set; }
        public double? UnallocatedQuantity { get; set; }
        public int? AlreadyAllocated { get; set; }
        public DateTime? AllocationDate { get; set; }
        public string Remarks { get; set; }
        public Int16? AllocatedQuantity { get; set; }
        public double? AvailableQuantity { get; set; }

        public double AvailableToAssignQuantity { get; set; }
        public Int16 AssignedQuantity { get; set; }
        public string Option { get; set; }
    }
}
