using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarERP.DomainModel.AMSModel
{
    public class AccountIncomeStatement
    {
        public string AccountNo { set; get; }
        public string AccountName { set; get; }
        public decimal PeriodicAmount { set; get; }
        public string PeriodicAmountPercentageOnRevenue { set; get; }
        public decimal YearToDateAmount { set; get; }
        public string YearToDateAmountPercentageOnRevenue { set; get; }
    }
}
