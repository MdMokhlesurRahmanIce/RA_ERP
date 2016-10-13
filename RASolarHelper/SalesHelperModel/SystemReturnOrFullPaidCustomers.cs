using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarHelper.SalesHelperModel
{
    public class SystemReturnOrFullPaidCustomers
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public DateTime AgreementDate { get; set; }
        public int? NoOfMonthAtCustomerEnd { get; set; }
        public string Package { get; set; }
        public decimal? OverdueOrAdvanceBalanceUpToDate { get; set; }
        public decimal? OutstandingBalance { get; set; }
        public decimal? CollectionAmount { get; set; }
        public string ReasonForSystemReturn { get; set; }
    }
}
