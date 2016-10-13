using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.SalesModel
{
    public class CustomerFPRNDayWiseRegularOrODTarget
    {
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int? DayOfTheMonth { get; set; }
        public decimal? DayWiseOverdueRecoveryTargetLimit { get; set; }
        public decimal? DayWiseOverdueRecoveryTargetAmount { get; set; }
        public decimal? DayWiseRegularRecoveryTargetAmount { get; set; }
        public int? NoOfCustomerAssigned { get; set; }
    }
}
