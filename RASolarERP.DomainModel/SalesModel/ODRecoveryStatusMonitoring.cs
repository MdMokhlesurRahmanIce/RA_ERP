using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.SalesModel
{
    public class ODRecoveryStatusMonitoring
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public byte? MonthlyCollectionDay { get; set; }
        public decimal? OverdueBalance { get; set; }
        public string ODGradeThisMonth { get; set; }
        public decimal? ThisMonthRecoveryFromOD { get; set; }
        public decimal? RemainingODBalance { get; set; }
        public string EmployeeAsFPR { get; set; }
        public string EmployeeAsFPRName { get; set; }
        public string EmployeeAsFPRMobileNo { get; set; }

        public DateTime? UMNextRecoveryDateIfRemainingODBalance { get; set; }
        public string UMRemarks { get; set; }
        public DateTime? UMLastRemarksDate { get; set; }

        public string OdCustomerGrading { get; set; }
        public string CustomerFPR { get; set; }
        public bool IsOnlyForCollectionDatePassed { get; set; }
        public byte? CollectionDayScheduledDay { get; set; }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }

        public bool? IsCustomerPoliticallyInfluential { get; set; }
        public bool? IsCustomerALocalMuscleMan { get; set; }
        public bool? IsCustomerNotWillingToPay { get; set; }

        public string CricCustCategory { get; set; }
    }
}
