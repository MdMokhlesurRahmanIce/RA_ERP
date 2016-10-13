using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarHelper.SalesHelperModel
{
    public class UnitWiseCustomerLedger
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public DateTime AgreementDate { get; set; }
        public decimal PackagePrice { get; set; }
        public decimal InstallmentSize { get; set; }
        public decimal TotalPrincipalPlusServiceChargeReceivable { get; set; }
        public string Particulars { get; set; }
        public string MemoNo { get; set; }
        public DateTime TransDate { get; set; }
        public decimal Amount { get; set; }
        public decimal Outstanding { get; set; }
        public string BookNo { get; set; }
    }
}
