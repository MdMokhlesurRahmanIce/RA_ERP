using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.SalesModel
{
    public class CustomerWiseOverdueBalanceTracker
    {
        private string __CustomerCode;
        private string __CustomerName;
        private decimal __OverdueBalance;
        private decimal __ODRecoveredUpToLastWeek;
        private decimal __RemainingODToBeRecovered;
        private decimal __ODRecoveredThisWeek;

        public string CustomerCode
        {
            get { return __CustomerCode; }
            set { __CustomerCode = value; }
        }

        public string CustomerName
        {
            get { return __CustomerName; }
            set { __CustomerName = value; }
        }

        public decimal OverdueBalance
        {
            get { return __OverdueBalance; }
            set { __OverdueBalance = value; }
        }

        public decimal ODRecoveredUpToLastWeek
        {
            get { return __ODRecoveredUpToLastWeek; }
            set { __ODRecoveredUpToLastWeek = value; }
        }

        public decimal RemainingODToBeRecovered
        {
            get { return __RemainingODToBeRecovered; }
            set { __RemainingODToBeRecovered = value; }
        }

        public decimal ODRecoveredThisWeek
        {
            get { return __ODRecoveredThisWeek; }
            set { __ODRecoveredThisWeek = value; }
        }
    }
}
