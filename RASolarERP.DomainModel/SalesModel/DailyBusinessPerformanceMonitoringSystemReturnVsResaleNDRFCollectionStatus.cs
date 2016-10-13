using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.SalesModel
{
    public class DailyBusinessPerformanceMonitoringSystemReturnVsResaleNDRFCollectionStatus
    {
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public int Last3MonthsSystemReturn { get; set; }
        public int Last3MonthsResale { get; set; }
        public double ThisMonthOPBalance_NetSealable { get; set; }
        public int ThisMonthSystemReturn { get; set; }
        public int ThisMonthResale { get; set; }
        public double ClosingBalance { get; set; }
        public int SalesMay2013ToOnward_Qty { get; set; }
        public int DRFMay2013ToOnward_Qty { get; set; }
        public int DRFBacklog_Qty { get; set; }        
        public int ThisMonthSales_Qty { get; set; }
        public int ThisMonthDRFReceived_Qty { get; set; }
    }
}
