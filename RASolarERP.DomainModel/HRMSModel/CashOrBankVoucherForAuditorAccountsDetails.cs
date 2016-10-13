using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarERP.DomainModel.HRMSModel
{
    public class CashOrBankVoucherForAuditorAccountsDetails
    {
        public int AccountNo { get; set; }
        public string AccountName { get; set; }
        public string Particulars { get; set; }
        public double? DrAmount { get; set; }
        public double? CrAmount { get; set; }
        public string DimensionCode { get; set; }
    }
}
