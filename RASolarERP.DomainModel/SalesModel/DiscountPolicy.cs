using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.SalesModel
{
    public class DiscountPolicy
    {
        public string DiscountID { get; set; }
        public string DiscountDescription { get; set; }
        public byte DiscountPercentage { get; set; }
        public bool IsDiscountAFixedAmount { get; set; }
        public decimal DiscountFixedAmount { get; set; }
    }
}
