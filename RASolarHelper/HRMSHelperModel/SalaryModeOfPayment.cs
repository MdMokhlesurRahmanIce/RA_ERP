using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarHelper.HRMSHelperModel
{
    public class SalaryModeOfPayment
    {
        private string _PaymentTypeName;
        private string _PaymentTypeValue;

        public string PaymentTypeName
        {
            get { return this._PaymentTypeName; }
            set { _PaymentTypeName = value; }
        }

        public string PaymentTypeValue
        {
            get { return this._PaymentTypeValue; }
            set { _PaymentTypeValue = value; }
        }

        public List<SalaryModeOfPayment> SalaryModeOfPaymentList()
        {
            SalaryModeOfPayment objSalaryModeOfPayment;
            List<SalaryModeOfPayment> lstSalaryModeOfPayment = new List<SalaryModeOfPayment>();

            objSalaryModeOfPayment = new SalaryModeOfPayment();
            objSalaryModeOfPayment.PaymentTypeName = "Cash";
            objSalaryModeOfPayment.PaymentTypeValue = "CASH";
            lstSalaryModeOfPayment.Add(objSalaryModeOfPayment);

            objSalaryModeOfPayment = new SalaryModeOfPayment();
            objSalaryModeOfPayment.PaymentTypeName = "Bank";
            objSalaryModeOfPayment.PaymentTypeValue = "BANK";
            lstSalaryModeOfPayment.Add(objSalaryModeOfPayment);

            return lstSalaryModeOfPayment;
        }
    }
}
