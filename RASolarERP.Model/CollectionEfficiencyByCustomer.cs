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
    
    public partial class CollectionEfficiencyByCustomer
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public decimal CM_Receivable__One_Installment__4 { get; set; }
        public decimal CM_CashCollection_5 { get; set; }
        public decimal CM_SystemReturn_6 { get; set; }
        public Nullable<decimal> CM_AdvanceAdjustment_7 { get; set; }
        public Nullable<decimal> CM_TotalCollection_8_5_6_7 { get; set; }
        public Nullable<decimal> CM_CollectionEfficiency_9_8_4_ { get; set; }
        public Nullable<decimal> OD_OverdueInCurrentMonth_10_4_8 { get; set; }
        public Nullable<decimal> OD_OverdueBalanceAtTheEndOfLastMonth_11 { get; set; }
        public Nullable<decimal> OD_TotalOverdueUpToCurrentMonth_12_10_11 { get; set; }
        public Nullable<decimal> OD_RecoveryFromOverdueInCurrentMonth_13 { get; set; }
        public Nullable<decimal> OD_SystemReturn_14 { get; set; }
        public Nullable<decimal> OD_OverdueBalanceAtTheEndOfCurrentMonth_15_12_13_14 { get; set; }
        public Nullable<decimal> OverallCollectionEfficiency_16__8_13_14___4_11__ { get; set; }
        public Nullable<decimal> AD_AdvanceBalanceAtTheEndOfLastMonth_17 { get; set; }
        public Nullable<decimal> AD_AdvanceReceivedInCurrentMonth_18 { get; set; }
        public Nullable<decimal> AD_TotalAdvance_19_17_18 { get; set; }
        public Nullable<decimal> AD_AdvanceAdjustmentInCurrentMonth_20 { get; set; }
        public Nullable<decimal> AD_SystemReturn_21 { get; set; }
        public Nullable<decimal> AD_AdvanceBalanceAfterAdjustment_22_19_20_21 { get; set; }
    }
}
