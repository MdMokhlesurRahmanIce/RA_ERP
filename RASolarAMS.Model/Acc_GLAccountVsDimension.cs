//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RASolarAMS.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Acc_GLAccountVsDimension
    {
        public Acc_GLAccountVsDimension()
        {
            this.Acc_PrePostTransDetailByDimension = new HashSet<Acc_PrePostTransDetailByDimension>();
        }
    
        public string AccountNo { get; set; }
        public string DimensionCode { get; set; }
        public bool IsTheDimensionMandatory { get; set; }
        public bool IsItTheDefaultDimension { get; set; }
        public byte Status { get; set; }
    
        public virtual Acc_ChartOfAccounts Acc_ChartOfAccounts { get; set; }
        public virtual ICollection<Acc_PrePostTransDetailByDimension> Acc_PrePostTransDetailByDimension { get; set; }
    }
}
