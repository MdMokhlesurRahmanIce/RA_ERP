using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarHelper.SalesHelperModel
{
    public class UnitWiseCashMemoInformation
    {
        public DateTime TransDate { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CashMemoNo { get; set; }
        public string CashMemoBookNo { get; set; }
        public string Particulars { get; set; }
        public decimal Amount { get; set; }
    }


}
