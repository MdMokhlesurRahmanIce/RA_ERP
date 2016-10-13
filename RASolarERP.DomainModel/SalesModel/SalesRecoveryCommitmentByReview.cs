using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.SalesModel
{
    public class SalesRecoveryCommitmentByReview
    {
        public string ProgramCode { get; set; }
        public string ProgramName { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public double? LastMonthODCollectionEfficiency { get; set; }
        public Decimal? CurrentCollectionTarget { get; set; }
        public Decimal? ThisMonthRecoveryFromCurrent { get; set; }
        public Decimal? PossibleODFromCurrent { get; set; }
        public Decimal? OverdueBalance { get; set; }
        public Decimal? ThisMonthRecoveryFromOD { get; set; }
        public Decimal? RemainingODBalance { get; set; }
        public double? ThisMonthODCollectionEfficiency { get; set; }

        public int? SalesTarget { get; set; }
        public int? SalesAchievement { get; set; }
        public string UMLastOverallRemarks { get; set; }
        public DateTime? UMLastOverallRemarksEntryDate { get; set; }
        public string AMLastRemarks { get; set; }
        public DateTime? AMLastRemarksEntryDate { get; set; }
        public string HOLastRemarks { get; set; }
        public DateTime? HOLastRemarksEntryDate { get; set; }
        public string DefaultEmployeeAsFPR { get; set; }
        public DateTime? LastOpenDateByUO { get; set; }

    }
}
