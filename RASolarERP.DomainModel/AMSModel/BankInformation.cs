using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarERP.DomainModel.AMSModel
{
    public class BankInformation
    {
        public string BankID { get; set; }
        public string BankName { get; set; }

        public string BankBranchID { get; set; }

        public string BankAccountNumber { get; set; }
        public string BankAccountTypeID { get; set; }

        public string Address { get; set; }
        public DateTime AccountOpeningDate { get; set; }

        public string SalaryLocationCode { get; set; }
        public string SalaryDisbursementBankAccountNumber { get; set; }
        public string SalaryDisbursementBranchName { get; set; }
    }
}
