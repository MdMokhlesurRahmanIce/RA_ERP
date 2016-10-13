using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.SalesModel
{
    public class DailyBusinessPerformanceMonitoringSalesNCollectionTargetVsAchievement
    {
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public int? SalesTarget_ForTheDay { get; set; }
        public int? SalesAchievement_ForTheDay { get; set; }
        public string SalesEfficiency_ForTheDay { get; set; }
        public decimal? RegularCollectionTarget_ForTheDay { get; set; }
        public decimal? RegularCollectionAchievement_ForTheDay { get; set; }
        public string RegularCollectionEfficiency_ForTheDay { get; set; }
        public decimal? OverdueCollectionTarget_ForTheDay { get; set; }
        public decimal? OverdueCollectionAchievement_ForTheDay { get; set; }
        public string OverdueCollectionEfficiency_ForTheDay { get; set; }
    }
}
