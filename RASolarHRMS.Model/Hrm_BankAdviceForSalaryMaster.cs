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
    
    public partial class Hrm_BankAdviceForSalaryMaster
    {
        public string YearMonth { get; set; }
        public string BankID { get; set; }
        public string BankName { get; set; }
        public string BankBranchID { get; set; }
        public string BankBranchName { get; set; }
        public string SalaryLocationCode { get; set; }
        public string SalaryDisbursementBankAccountNumber { get; set; }
        public short AdviceLetterReferenceSeqNo { get; set; }
        public string AdviceLetterReferenceNo { get; set; }
        public string Signatory1Designation { get; set; }
        public string Signatory1Name { get; set; }
        public string Signatory2Designation { get; set; }
        public string Signatory2Name { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
    }
}
