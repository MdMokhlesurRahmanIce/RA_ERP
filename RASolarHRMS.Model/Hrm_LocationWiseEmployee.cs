//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RASolarHRMS.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Hrm_LocationWiseEmployee
    {
        public Hrm_LocationWiseEmployee()
        {
            this.Hrm_EmployeeInfo = new HashSet<Hrm_EmployeeInfo>();
            this.Hrm_EmployeeTransfer = new HashSet<Hrm_EmployeeTransfer>();
            this.Hrm_EmployeeNDateWiseTADAEntry = new HashSet<Hrm_EmployeeNDateWiseTADAEntry>();
        }
    
        public string LocationCode { get; set; }
        public string EmployeeID { get; set; }
        public byte Status { get; set; }
    
        public virtual Common_LocationInfo Common_LocationInfo { get; set; }
        public virtual ICollection<Hrm_EmployeeInfo> Hrm_EmployeeInfo { get; set; }
        public virtual Hrm_EmployeeInfo Hrm_EmployeeInfo1 { get; set; }
        public virtual ICollection<Hrm_EmployeeTransfer> Hrm_EmployeeTransfer { get; set; }
        public virtual ICollection<Hrm_EmployeeNDateWiseTADAEntry> Hrm_EmployeeNDateWiseTADAEntry { get; set; }
    }
}
