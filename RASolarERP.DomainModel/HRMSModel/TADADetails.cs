using RASolarHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.HRMSModel
{
    public class TADADetails
    {
        public string LocationCode { get; set; }
        public string YearMonth { get; set; }
        public string EmployeeID { get; set; }
        public DateTime DateOfTADA { get; set; }
        public decimal TAAmount { get; set; }
        public string ParticularsForTA { get; set; }
        public decimal DAAmount { get; set; }
        public string ParticularsForDA { get; set; }
        public byte? DaysOfPendingEntry { get; set; }
        public int? TADAEntryMonth { get; set; }
        public string EditEmployee { get; set; }
        public decimal TotalTADAAmount { get; set; }
        public string EmployeeName {get; set;}
        public byte TotalDaysForTA { get; set; }
        public byte TotalDaysForDA { get; set; }
        public string  ZoneCode { get; set; }
        public string  ZoneName { get; set; }
        public string RegionCode { get; set; }
        public string  RegionName { get; set; }
        public string ProgramName { get; set; }
        public string AreaName { get; set; }
        public string  UnitCode { get; set; }
        public string UnitName { get; set; }

        public decimal TotalCollection { get; set; }
        public int TotalSales { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public DateTime JoiningDate { get; set; }
        public string TADAMonth { get; set; }
        public string Reason { get; set; }


        public List<EmployeeDetailsInfo> Employee { get; set; }
        public List<YearMonthFormat> MonthList { get; set; }
    }
}
