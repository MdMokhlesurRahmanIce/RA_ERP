//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RASolarERP.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo
    {
        public byte StoreLocation { get; set; }
        public string LocationCode { get; set; }
        public string ItemCode { get; set; }
        public string EmployeeID { get; set; }
        public string AllocationSeqNo { get; set; }
        public string ItemSerialNo { get; set; }
        public Nullable<bool> IsItAllocated { get; set; }
        public byte Status { get; set; }
    
        public virtual Fix_EmployeeWiseFixedAssetsAllocation Fix_EmployeeWiseFixedAssetsAllocation { get; set; }
        public virtual Inv_ItemNItemCategoryWithSerialNoMaster Inv_ItemNItemCategoryWithSerialNoMaster { get; set; }
        public virtual Inv_ItemStockWithSerialNoByLocation Inv_ItemStockWithSerialNoByLocation { get; set; }
    }
}