using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.SalesModel
{
    public class EmployeeWiseSalesNColcTargetNAchievementForAUnit   
    {
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public int SalesCurrentMonthTarget { get; set; }
        public int NoOfCustomerAssigned { get; set; }
        // public Nullable<decimal> RegColcCurrentMonthTarget { get; set; }
        // public Nullable<decimal> OverdueColcCurrentMonthTarget { get; set; }
        public Nullable<int> SalesCurrentMonthAchievement { get; set; }
        public Nullable<decimal> CollectionTarget { get; set; }
        public Nullable<decimal> CollectionAchievement { get; set; }   
       
    }
}
