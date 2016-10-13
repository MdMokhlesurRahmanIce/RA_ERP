using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarHelper.SalesHelperModel
{
    public class CustomerFPRAndScheduledCollectionEntry
    {
        private string _customerCode;
        private string _customerName;
        private DateTime _agreementDate;
        private decimal _collectionInCurrentMonthWithoutDP;
        private decimal _overdueOrAdvanceBalance;
        private decimal _outstandingBalance;
        private string _employeeAsFPR;
        private byte _scheduledCollectionDay;

        public string CustomerCode
        {
            get{return _customerCode;}
            set{_customerCode=value;}           
        }
        public string CustomerName
        {
            get{return _customerName;}
            set{_customerName=value;}
        }
        public DateTime AgreementDate
        {
            get{return _agreementDate;}
            set{_agreementDate=value;}
        }
        public decimal CollectionInCurrentMonthWithoutDP
        {
            get{return _collectionInCurrentMonthWithoutDP;}
            set{_collectionInCurrentMonthWithoutDP=value;}
        }
        public decimal OverdueOrAdvanceBalance
        {
            get{return _overdueOrAdvanceBalance;}
            set{_overdueOrAdvanceBalance=value;}
        }
        public decimal OutstandingBalance
        {
            get{return _outstandingBalance;}
            set{_outstandingBalance=value;}
        }
        public string EmployeeAsFPR
        {
            get{return _employeeAsFPR;}
            set{_employeeAsFPR=value;}
        }
        public byte ScheduledCollectionDay
        {
            get{return _scheduledCollectionDay;}
            set{_scheduledCollectionDay=value;}
        }

    }
}
