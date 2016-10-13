using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarHelper.SalesHelperModel
{
    public class CustomerDataToCloseWithFullPaidOrWaive
    {   
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public DateTime AgreementDate { get; set; }
        public byte NumberOfInstallment { get; set; }
        public double? MonthUsage { get; set; }
        public decimal TotalPrincipalReceivable{ get; set; }
        public decimal TotalServiceChargeReceivableUpToDate {get;set;}
        public decimal TotalPrincipalRecovered { get; set; }
        public decimal OverdueOrAdvanceBalance {get;set;}
        public decimal TotalServiceChargeRecovered { get; set; }
        public decimal RemainingPrincipalReceivableAfterAdjustmentFromAdvance { get; set; }
        public decimal RemainingServiceChargeReceivableAfterAdjustmentFromAdvance { get; set; }
        public decimal OverdueOrAdvanceBalanceAfterAdjustment { get; set; }      

        public string AgreementClosedDate { get; set; }
        public string TransDate { get; set; }
        public string AgreementDateStringFormat { get; set; }
        public bool IsApprovalRequired {get; set;}
        public string ApprovalNo {get; set;}
        public string ApprovalStatusInDetails { get; set; }
        public byte? ApprovalStatus { get; set; }
    }
}