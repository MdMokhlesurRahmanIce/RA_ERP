using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.SalesModel
{
    public class LocationAndEmployeeWiseWeeklySalesAndCollectionReport
    {
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public decimal? WeeklyTarget { get; set; }
        public decimal? WeeklyAchievement { get; set; }
        public decimal? VarianceInAmount { get; set; }
        public decimal? VarianceInPercentage { get; set; }
        public decimal? CumulativeTarget { get; set; }
        public decimal? CumulativeAchievement { get; set; }
        public decimal? CumVarianceInAmount { get; set; }
        public decimal? CumVarianceInPercentage { get; set; }
        public string Remarks { get; set; }
    }
}
