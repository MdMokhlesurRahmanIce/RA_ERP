using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.SalesModel
{
    public class DownPaymentPolicy
    {
        public string DownPaymentID { get; set; }
        public string DownPaymentDescription { get; set; }
        public byte DownPaymentPercentage { get; set; }
        public bool IsDPAFixedAmount { get; set; }
        public decimal DownPaymentFixedAmount { get; set; }
    }
}
