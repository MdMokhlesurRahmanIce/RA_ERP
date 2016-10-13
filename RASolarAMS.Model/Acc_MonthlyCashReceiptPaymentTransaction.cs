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
    
    public partial class Acc_MonthlyCashReceiptPaymentTransaction
    {
        public string YearMonth { get; set; }
        public string LocationCode { get; set; }
        public decimal Received_SHSMainProject { get; set; }
        public Nullable<decimal> Received_SHSIDCOLProject { get; set; }
        public Nullable<decimal> Received_SparePartsSalesForSHS { get; set; }
        public Nullable<decimal> Received_BioGas { get; set; }
        public Nullable<decimal> Received_TechnicalFeeForBioGas { get; set; }
        public Nullable<decimal> Received_SparePartsSalesForBioGas { get; set; }
        public Nullable<decimal> Received_SecurityDepositFromJTC { get; set; }
        public Nullable<decimal> Received_BandhuBatti { get; set; }
        public Nullable<decimal> Received_FromSubletRent { get; set; }
        public Nullable<decimal> Received_DRFAgreement { get; set; }
        public Nullable<decimal> Received_InstantRelease { get; set; }
        public Nullable<decimal> Received_AuditClaim { get; set; }
        public string Received_AuditClaimRemarks { get; set; }
        public Nullable<decimal> Received_FromOtherUnit { get; set; }
        public Nullable<decimal> Payment_BankDeposit { get; set; }
        public Nullable<decimal> Payment_OnlineTransfer { get; set; }
        public Nullable<decimal> Payment_MonthlyExpenses { get; set; }
        public Nullable<decimal> Payment_ToCustomerForSystemReturn { get; set; }
        public Nullable<decimal> Payment_ExpensesForBioGasConstruction { get; set; }
        public Nullable<decimal> Payment_AdvanceForBioGasConstruction { get; set; }
        public Nullable<decimal> Payment_ToOtherUnit { get; set; }
        public Nullable<decimal> Payment_CashShortageAndAuditClaimReceivable { get; set; }
        public decimal Payment_AdvanceOfficeRent { get; set; }
        public Nullable<System.DateTime> CheckedNFinalizedByUM { get; set; }
        public string FinalizedByUM_UserName { get; set; }
        public Nullable<System.DateTime> CheckedNFinalizedByAM { get; set; }
        public string FinalizedByAM_UserName { get; set; }
        public Nullable<System.DateTime> CheckedNFinalizedByHO { get; set; }
        public string FinalizedByHO_UserName { get; set; }
    }
}
