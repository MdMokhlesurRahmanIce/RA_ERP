using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RASolarERP.DomainModel.AMSModel;

//using RASolarERP.DomainModel.AMSModel;

namespace RASolarHelper
{
    public static class VoucherWiseChartOfAccount
    {
        public static List<ChartOfAccounts> CashInHand()
        {
            List<ChartOfAccounts> lstChartOfAccounts = new List<ChartOfAccounts>();
            ChartOfAccounts objChartOfAccounts;

            objChartOfAccounts = new ChartOfAccounts();
            objChartOfAccounts.AccountNo = "105063010";
            objChartOfAccounts.AccountName = "Cash in hand –  Unit Office";

            lstChartOfAccounts.Add(objChartOfAccounts);

            return lstChartOfAccounts;
        }
    }
}
