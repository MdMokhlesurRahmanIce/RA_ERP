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
    
    public partial class Common_CashMemoBookMaster
    {
        public string CashMemoBookNo { get; set; }
        public string LocationCode { get; set; }
        public bool IsAssignedForUse { get; set; }
        public Nullable<System.DateTime> AssignedDate { get; set; }
        public string AssignedToEmployee { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public byte Status { get; set; }
    
        public virtual Hrm_LocationWiseEmployee Hrm_LocationWiseEmployee { get; set; }
    }
}
