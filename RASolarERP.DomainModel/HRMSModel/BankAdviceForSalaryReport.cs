using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarERP.DomainModel.HRMSModel
{
    public class BankAdviceForSalaryReport
    {
        public string SalaryZoneCode { get; set; }
        public string SalaryZoneName { get; set; }
        public string SalaryRegionCode { get; set; }
        public string SalaryRegionName { get; set; }
        public string SalaryLocationCode1 { get; set; }
        public string SalaryLocationName { get; set; }

        public string BankID { get; set; }
        public string BankName { get; set; }
        public string BankBranchID { get; set; }
        public string LetterAddressTo { get; set; }
        public string BankAddress1 { get; set; }        
        public string BankBranchName { get; set; }
        public string SalaryLocationCode { get; set; }
        public string CompanyBankAccountNumber { get; set; }
        public string BATypeDescription { get; set; }
        public Int16 AdviceLetterReferenceSeqNo { get; set; }
        public string AdviceLetterReferenceNo { get; set; }
        public string Signatory1Designation { get; set; }
        public string Signatory1Name { get; set; }
        public string Signatory2Designation { get; set; }
        public string Signatory2Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EmployeeLocationCode { get; set; }
        public Int64 SerialNo { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeBankAccountNumber { get; set; }
        public string SalaryDisbursementBranchName { get; set; }
        public decimal SalaryOtherThanTADAAmount { get; set; }
        public decimal TADAAmount { get; set; }
        public decimal? BonusAmount { get; set; }
        public decimal TotalSalaryAmount { get; set; }      
    }
}
