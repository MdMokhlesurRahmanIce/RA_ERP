using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarERP.DomainModel.SalesModel
{
    public class CustomerDisasterRecoveryDetails
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public DateTime AgreementDate { get; set; }
        public int? AgreementDuration { get; set; }
        public string CustomerFPREmployeeCode { get; set; }
        public string CustomerFPREmployeeName { get; set; }
    }
}
