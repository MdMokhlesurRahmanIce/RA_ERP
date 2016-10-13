using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.AMSModel
{
    public class SubLedgerHeadDetails
    {
        public string DimensionValueID { get; set; }
        public string DimensionValueDesc { get; set; }
        public decimal? DimensionAmount { get; set; }
        public string AccountNo { get; set; }
        public string DimensionCode { get; set; }
        public string TransactionType { get; set; }
    }
}
