using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.SalesModel
{
   public class DailyBusinessPerformanceMonitoringOtherStatus
    {
        //public string LocationCode { get; set; }
        public string ZoneName { get; set; }
        public Int32? NoOfActiveCustomer { get; set; }
        public Int32? NoOfCustomerWithZeroCollectionThisMonth { get; set; }
        public Int32? MissingCollectionThisMonth_InPercentage { get; set; }

        public int? NoOfOngoingCustomerTotal { get; set; }
        public int? OngoingCustomerInOD_Qty { get; set; }
        public int? OngoingInOD_InPercentage { get; set; }

        public Decimal? OngoingCustomerInOD_ODAmount { get; set; }
        public int? OngoingCustomerInODSettled_Qty { get; set; }
        public int? NoOfLPOC{ get; set; }

         public int? LPOC_InPercentage { get; set; }
         public Decimal? LPOC_ODAmount { get; set; }
         public int? LPOCSettled_Qty { get; set; }

         public Decimal? LastMonthAdvanceCollection { get; set; }
         public Decimal? LastMonthAdvanceAdjustment { get; set; }
         public Decimal? ThisMonthAdvanceCollection { get; set; }
    }
}
