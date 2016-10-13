using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.SalesModel
{
    public class DailyBusinessPerformanceMonitoringNetODIncreaseOrDecreaseComperison
    {
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public decimal ODRecoveryInLastMonth { get; set; }
        public decimal OverdueInLastMonth { get; set; }
        public decimal NetODInLastMonth { get; set; }
        public decimal ODRecoveryInCurrentMonth { get; set; }
        public decimal OverdueInCurrentMonth { get; set; }
        public decimal NetODInCurrentMonth { get; set; }
        public decimal NetODIncreasesOrDecreasesFromLastMonth { get; set; }
    }
}
