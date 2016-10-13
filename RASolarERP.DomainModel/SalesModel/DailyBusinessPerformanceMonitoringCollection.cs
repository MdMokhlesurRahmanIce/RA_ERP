using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.SalesModel
{
    public class DailyBusinessPerformanceMonitoringCollection
    {
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public decimal RegularCollectionTarget_CurrentMonthTotal { get; set; }
        public decimal RegularCollectionTarget_UpToDate { get; set; }
        public decimal RegularCollectionAchievement_UpToDate { get; set; }
        public string CurrentCollectionEfficiency_UpToDate { get; set; }
        public string CurrentCollectionEfficiency_LastMonth { get; set; }
        public decimal OverdueBalance_LastMonth { get; set; }
        public decimal OverdueCollectionTarget_UpToDate { get; set; }
        public decimal OverdueCollectionAchievement_UpToDate { get; set; }
        public string OverdueCollectionEfficiency_UpToDate { get; set; }
        public string OverdueCollectionEfficiency_LastMonth { get; set; }
        public string OverallCollectionEfficiency_UpToDate { get; set; }
        public string OverallCollectionEfficiency_LastMonth { get; set; }
    }
}
