using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.InventoryModel
{
    public class CashMemoBookPagesStatus
    {
        public string CashMemoBookNo { get; set; }
        public string CashMemoNo { get; set; }
        public string Status { get; set; }
        public string CustomerCode { get; set; }
        public string Remarks { get; set; }
       //
        public decimal? Amount { get; set; }
    }
}
