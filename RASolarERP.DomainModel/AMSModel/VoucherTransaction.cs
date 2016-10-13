using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarERP.DomainModel.AMSModel
{
    public class VoucherTransaction
    {
        public string LocationCode { get; set; }
        public string TransactionNo { get; set; }
        public string AccountNo { get; set; }
        public string Particulars { get; set; }
        public decimal Amount { get; set; }
        public string ProjectCode { get; set; }
        public string AccountName { get; set; }
        public DateTime TransactionDate { get; set; }
        public string GeneralParticulars { get; set; }
        public string TransactionType { get; set; }
        public string DimensionCode { get; set; }
    }
}
