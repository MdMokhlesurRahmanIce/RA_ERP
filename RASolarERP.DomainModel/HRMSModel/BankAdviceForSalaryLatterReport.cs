using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarERP.DomainModel.HRMSModel
{
    public class BankAdviceForSalaryLatterReport  
    {
        public string LetterReferenceNo { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string UnitName { get; set; }
        public string AreaName { get; set; }
        public DateTime AgreementDate { get; set; }
        public string PackageCapacity { get; set; }
        public decimal TotalPriceWithServiceCharge { get; set; }
        public DateTime PaymentUpToDate { get; set; }
        public decimal TotalPaidDPPlusInstallment { get; set; }
        public decimal OverdueBalance { get; set; }
        public decimal OutstandingBalance { get; set; }
        public DateTime PayByDate { get; set; }
        public decimal PackagePrice { get; set; }
        public decimal ServiceCharge { get; set; }
        public string UnionName { get; set; }
        public string UM_CellNo { get; set; }
        public string AM_CellNo { get; set; }

        public string Village { get; set; }
     
        public string PostOffice { get; set; }
        public string UpazilaName { get; set; }
        public string DistrictName { get; set; }
        public string UnionID { get; set; }
        public string UpazilaCode { get; set; }

        public double? SlNo { get; set; }

        public string GuardianName { get; set; } 

        




             
    }
}
