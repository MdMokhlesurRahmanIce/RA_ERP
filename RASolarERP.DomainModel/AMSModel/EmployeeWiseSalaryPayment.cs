using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.AMSModel
{
    public class EmployeeWiseSalaryPayment
    {
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string YearMonth { get; set; }
        public string AccountNo { get; set; }
        public decimal SalaryOtherThanTADAAmount { get; set; }
        public decimal TADAAmount { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal TotalSalaryAmount { get; set; }
        public string LocationCode { set; get; }
        public DateTime TransactionDate { set; get; }
        public int ProjectCode { set; get; }
        public string TransactionNo { set; get; }
    }
}
