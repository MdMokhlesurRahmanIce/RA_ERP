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
    
    public partial class Sal_PackageOrItemCapacity
    {
        public Sal_PackageOrItemCapacity()
        {
            this.Inv_ItemMaster = new HashSet<Inv_ItemMaster>();
            this.Sal_PackageMaster = new HashSet<Sal_PackageMaster>();
            this.Sal_Validation_CapacityVsLight = new HashSet<Sal_Validation_CapacityVsLight>();
        }
    
        public string CapacityID { get; set; }
        public string Description { get; set; }
        public Nullable<byte> CapacityInValue { get; set; }
        public string ProjectCode { get; set; }
        public string CapacityFor { get; set; }
        public byte Status { get; set; }
    
        public virtual Common_ProjectInfo Common_ProjectInfo { get; set; }
        public virtual ICollection<Inv_ItemMaster> Inv_ItemMaster { get; set; }
        public virtual ICollection<Sal_PackageMaster> Sal_PackageMaster { get; set; }
        public virtual ICollection<Sal_Validation_CapacityVsLight> Sal_Validation_CapacityVsLight { get; set; }
    }
}