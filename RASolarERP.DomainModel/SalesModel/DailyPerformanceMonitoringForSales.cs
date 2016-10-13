using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.SalesModel
{
    public class DailyPerformanceMonitoringForSales
    {
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public int? SalesTarget_CurrentMonthTotal { get; set; }
        public int? SalesTarget_Yesterday { get; set; }
        public int? SalesAchievement_Yesterday { get; set; }
        public int? SalesTarget_UpToDate { get; set; }
        public int? SalesAchievement_UpToDate { get; set; }
        public string SalesEfficiency_UpToDate { get; set; }
        public string SalesEfficiency_LastMonth { get; set; }
        public int? SalesVarianceWithMonthlyTarget { get; set; }
        public int? RequiredTargetPerDay { get; set; }
    }
}
