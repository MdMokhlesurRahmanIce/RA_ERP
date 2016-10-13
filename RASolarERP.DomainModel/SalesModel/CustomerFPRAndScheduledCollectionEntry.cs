using RASolarERP.DomainModel.HRMSModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarERP.DomainModel.SalesModel
{
    public class CustomerFPRAndScheduledCollectionEntry
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public DateTime AgreementDate { get; set; }
        public decimal CollectionInCurrentMonthWithoutDP { get; set; }
        public decimal OverdueOrAdvanceBalance { get; set; }
        public decimal OutstandingBalance { get; set; }
        public string EmployeeAsFPR { get; set; }
        public string Village { get; set; }
        public byte ScheduledCollectionDay { get; set; }        
    }
}
