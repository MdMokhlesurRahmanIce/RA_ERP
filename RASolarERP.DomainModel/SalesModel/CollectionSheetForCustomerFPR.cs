using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.SalesModel
{
    public class CollectionSheetForCustomerFPR
    {
        public Int64 SerialNo { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Village { get; set; }
        public string SystemSize { get; set; }
        public string ODCustomerGrade { get; set; }
        public byte ScheduledCollectionDay { get; set; }
        public decimal CurrentReceivable { get; set; }
        public decimal OverdueReceivable { get; set; }
        public decimal TotalReceivable { get; set; }
        
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }

        public Int32 NoOfMonthPassed { get; set; }

        public decimal OutstandingBalance { get; set; }
        public DateTime LastPaidOn { get; set; }
        
    }
}
