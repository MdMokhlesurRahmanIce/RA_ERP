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
    
    public partial class Sal_Validation_CapacityVsLight
    {
        public Sal_Validation_CapacityVsLight()
        {
            this.Aud_AuditAdjustmentObservationOnSalesAgreement = new HashSet<Aud_AuditAdjustmentObservationOnSalesAgreement>();
            this.Sal_PackageMaster = new HashSet<Sal_PackageMaster>();
        }
    
        public string CapacityID { get; set; }
        public string LightID { get; set; }
        public byte Status { get; set; }
        public Nullable<bool> IsValidForDistribution { get; set; }
    
        public virtual ICollection<Aud_AuditAdjustmentObservationOnSalesAgreement> Aud_AuditAdjustmentObservationOnSalesAgreement { get; set; }
        public virtual Sal_Light Sal_Light { get; set; }
        public virtual ICollection<Sal_PackageMaster> Sal_PackageMaster { get; set; }
        public virtual Sal_PackageOrItemCapacity Sal_PackageOrItemCapacity { get; set; }
    }
}
