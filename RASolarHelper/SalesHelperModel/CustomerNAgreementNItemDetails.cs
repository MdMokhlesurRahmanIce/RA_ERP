using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarHelper.SalesHelperModel
{
    public class CustomerNAgreementNItemDetails
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public DateTime? AgreementDate { get; set; }
        public string CustomerType { get; set; }
        public string CustomerTypeName { get; set; }
        public string PackageCode { get; set; }
        public string PackageName { get; set; }
        public string Capacity { get; set; }
        public string PackageCapacity { get; set; }
        public string Light { get; set; }
        public string PackageLight { get; set; }
        public string PanelSerialNo { get; set; }
        public string BatterySerialNo { get; set; }
        public string ModeOfPaymentID { get; set; }
        public string ModeOfPaymentName { get; set; }
        public decimal? DownPaymentAmount { get; set; }
        public string CashMemoNo { get; set; }
        public string CashMemoUsesID { get; set; }
        public decimal? PackagePrice { get; set; }
        public bool IsResales { get; set; }

        public List<SalesItemDetails> CustomerItems { get; set; }
    }

}
