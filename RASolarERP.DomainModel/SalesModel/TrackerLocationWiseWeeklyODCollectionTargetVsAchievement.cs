using RASolarERP.DomainModel.HRMSModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.SalesModel
{
    public class TrackerLocationWiseWeeklyODCollectionTargetVsAchievement 
    {
        public string FPR { get; set; }
        public string Zone_Name { get; set; }
        public string Region_Name { get; set; }
        public string Unit_Code { get; set; }
        public string Unit_Name { get; set; }
        public decimal WeeklyTarget { get; set; }
        public decimal WeeklyAchievement { get; set; }
        public Nullable<decimal> WeeklyVarianceInAmount { get; set; }
        public Nullable<decimal> WeeklyVarianceInPercentage { get; set; }
        public decimal CumulativeTarget { get; set; }
        public decimal CumulativeAchievement { get; set; }
        public Nullable<decimal> CumVarianceInAmount { get; set; }
        public Nullable<decimal> CumVarianceInPercentage { get; set; }
        public string Remarks { get; set; }
    }
}
