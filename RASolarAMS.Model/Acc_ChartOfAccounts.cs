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
    
    public partial class Acc_ChartOfAccounts
    {
        public Acc_ChartOfAccounts()
        {
            this.Acc_FinalTransDetail = new HashSet<Acc_FinalTransDetail>();
            this.Acc_LocationWiseDailySummary = new HashSet<Acc_LocationWiseDailySummary>();
            this.Acc_PrePostTransDetail = new HashSet<Acc_PrePostTransDetail>();
            this.Acc_SpecialAccount = new HashSet<Acc_SpecialAccount>();
            this.Common_LocationWiseBankAccountInfo = new HashSet<Common_LocationWiseBankAccountInfo>();
            this.Acc_GLAccountVsDimension = new HashSet<Acc_GLAccountVsDimension>();
        }
    
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public string ParentAccountNo { get; set; }
        public byte AccountLevel { get; set; }
        public string OnlyForLocation { get; set; }
        public string OnlyForProject { get; set; }
        public Nullable<bool> OnlyForAutoEntry { get; set; }
        public Nullable<bool> IsValidForUnitOffice { get; set; }
        public Nullable<bool> IsValidForZonalOffice { get; set; }
        public Nullable<bool> IsValidForHeadOffice { get; set; }
        public string ParCapSub { get; set; }
        public byte Status { get; set; }
        public string UserID { get; set; }
        public System.DateTime EntryTime { get; set; }
    
        public virtual ICollection<Acc_FinalTransDetail> Acc_FinalTransDetail { get; set; }
        public virtual ICollection<Acc_LocationWiseDailySummary> Acc_LocationWiseDailySummary { get; set; }
        public virtual ICollection<Acc_PrePostTransDetail> Acc_PrePostTransDetail { get; set; }
        public virtual ICollection<Acc_SpecialAccount> Acc_SpecialAccount { get; set; }
        public virtual ICollection<Common_LocationWiseBankAccountInfo> Common_LocationWiseBankAccountInfo { get; set; }
        public virtual ICollection<Acc_GLAccountVsDimension> Acc_GLAccountVsDimension { get; set; }
    }
}