using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarHelper
{
    public class LoginHelper
    {
        private string __LogInID = string.Empty;
        private string __LogInUserName = string.Empty;
        private string __LogInPassword = string.Empty;
        private string __LogInForZoneCode = string.Empty;
        private string __LogInForRegionCode = string.Empty;
        private string __LogInForUnitCode = string.Empty;
        private string __LogInForZoneName = string.Empty;
        private string __LogInForRegionName = string.Empty;
        private string __LogInForUnitName = string.Empty;
        private string __HomeLocation = string.Empty;
        private string __HomeURL = string.Empty;
        private string __UserRoleOrGroupID = string.Empty;
        private string __UserOperationalRoleOrGroupID = string.Empty;
        private DateTime __CurrentDate = DateTime.Now;
        private string __YearMonthCurrent = string.Empty;
        private string __AuditorYearMonth = string.Empty;
        private string __Location = string.Empty;
        private string __LocationCode = string.Empty;
        private string __LocationName = string.Empty;
        private string __LocationTitle = string.Empty;
        private string __ZoneTitle = string.Empty;
        private string __RegionTitle = string.Empty;
        private string __UnitTitle = string.Empty;
        private string __ReportType = string.Empty;
        private string __URL = string.Empty;
        private string __URLSelectionLocation = string.Empty;
        private string __TopMenu = string.Empty;

        private string __CurrentYearWeek = string.Empty;
        private int __CurrentWeekNumber = 0;
        private DateTime __FirstDateOfCurrentWeek = DateTime.Now;
        private DateTime __LastDateOfCurrentWeek = DateTime.Now;

        private string __OpenYearWeek = string.Empty;
        private int __OpenWeekNumber = 0;
        private DateTime __FirstDateOfOpenWeek = DateTime.Now;
        private DateTime __LastDateOfOpenWeek = DateTime.Now;

        private string __CustomerPrefix = string.Empty;
        private string __ModluleTitle = string.Empty;
        private int __OfficeModule = 0;

        private bool __IsAuthenticApproverForThisLocation = false;

        private string __OpenYearMonth = string.Empty;
        private DateTime __MonthOpenForSales = DateTime.Now;
        private DateTime __MonthOpenForInventory = DateTime.Now;
        private DateTime __MonthOpenForAccounting = DateTime.Now;
        private DateTime __MonthOpenForHRMS = DateTime.Now;
        private string __WeekOpenForWeeklyOverdueCollection = string.Empty;
        private DateTime __MonthOpenForDailyProgressReview = DateTime.Now;

        private DateTime __TransactionOpenDate = DateTime.Now;
        private DateTime __TransactionBackDate = DateTime.Now;

        private byte __IsInventoryImplemented = 1;

        public string LogInID
        {
            set { __LogInID = value; }
            get { return __LogInID; }
        }

        public string LogInUserName
        {
            set { __LogInUserName = value; }
            get { return __LogInUserName; }
        }

        public string LogInPassword
        {
            set { __LogInPassword = value; }
            get { return __LogInPassword; }
        }

        public string LogInForZoneCode
        {
            set { __LogInForZoneCode = value; }
            get { return __LogInForZoneCode; }
        }

        public string LogInForRegionCode
        {
            set { __LogInForRegionCode = value; }
            get { return __LogInForRegionCode; }
        }

        public string LogInForUnitCode
        {
            set { __LogInForUnitCode = value; }
            get { return __LogInForUnitCode; }
        }

        public string LogInForZoneName
        {
            set { __LogInForZoneName = value; }
            get { return __LogInForZoneName; }
        }

        public string LogInForRegionName
        {
            set { __LogInForRegionName = value; }
            get { return __LogInForRegionName; }
        }

        public string LogInForUnitName
        {
            set { __LogInForUnitName = value; }
            get { return __LogInForUnitName; }
        }

        public string HomeLocation
        {
            set { __HomeLocation = value; }
            get { return __HomeLocation; }
        }

        public string UerRoleOrGroupID
        {
            set { __UserRoleOrGroupID = value; }
            get { return __UserRoleOrGroupID; }
        }

        public string UserOperationalRoleOrGroupID
        {
            set { __UserOperationalRoleOrGroupID = value; }
            get { return __UserOperationalRoleOrGroupID; }
        }

        public DateTime CurrentDate
        {
            protected set { __CurrentDate = DateTime.Now; }
            get { return __CurrentDate; }
        }

        public string YearMonthCurrent
        {
            set { __YearMonthCurrent = value; }
            get { return __YearMonthCurrent; }
        }

        public string AuditorYearMonth
        {
            set { __AuditorYearMonth = value; }
            get { return __AuditorYearMonth; }
        }

        public string Location
        {
            set { __Location = value; }
            get { return __Location; }
        }

        public string LocationCode
        {
            set { __LocationCode = value; }
            get { return __LocationCode; }
        }

        public string LocationName
        {
            set { __LocationName = value; }
            get { return __LocationName; }
        }

        public string ReportType
        {
            get { return __ReportType; }
            set { __ReportType = value; }
        }

        public string URL
        {
            set { __URL = value; }
            get { return __URL; }
        }

        public string URLSelectionLocation
        {
            set { __URLSelectionLocation = value; }
            get { return __URLSelectionLocation; }
        }

        public string HomeURL
        {
            set { __HomeURL = value; }
            get { return __HomeURL; }
        }

        public string TopMenu
        {
            set { __TopMenu = value; }
            get { return __TopMenu; }
        }

        public string LocationTitle
        {
            set { __LocationTitle = value; }
            get { return __LocationTitle; }
        }

        public string ZoneTitle
        {
            set { __ZoneTitle = value; }
            get { return __ZoneTitle; }
        }

        public string RegionTitle
        {
            set { __RegionTitle = value; }
            get { return __RegionTitle; }
        }

        public string UnitTitle
        {
            set { __UnitTitle = value; }
            get { return __UnitTitle; }
        }

        public string CustomerPrefix
        {
            set { __CustomerPrefix = value; }
            get { return __CustomerPrefix; }
        }

        public int CurrentWeekNumber
        {
            set { __CurrentWeekNumber = value; }
            get { return __CurrentWeekNumber; }
        }

        public DateTime FirstDateOfCurrentWeek
        {
            set { __FirstDateOfCurrentWeek = value; }
            get { return __FirstDateOfCurrentWeek; }
        }

        public DateTime LastDateOfCurrentWeek
        {
            set { __LastDateOfCurrentWeek = value; }
            get { return __LastDateOfCurrentWeek; }
        }

        public string CurrentYearWeek
        {
            set { __CurrentYearWeek = value; }
            get { return __CurrentYearWeek; }
        }

        public int OpenWeekNumber
        {
            set { __OpenWeekNumber = value; }
            get { return __OpenWeekNumber; }
        }

        public DateTime FirstDateOfOpenWeek
        {
            set { __FirstDateOfOpenWeek = value; }
            get { return __FirstDateOfOpenWeek; }
        }

        public DateTime LastDateOfOpenWeek
        {
            set { __LastDateOfOpenWeek = value; }
            get { return __LastDateOfOpenWeek; }
        }

        public string OpenYearWeek
        {
            set { __OpenYearWeek = value; }
            get { return __OpenYearWeek; }
        }

        public string ModluleTitle
        {
            set { __ModluleTitle = value; }
            get { return __ModluleTitle; }
        }

        public int OfficeModule
        {
            set { __OfficeModule = value; }
            get { return __OfficeModule; }
        }

        public bool IsAuthenticApproverForThisLocation
        {
            set { __IsAuthenticApproverForThisLocation = value; }
            get { return __IsAuthenticApproverForThisLocation; }
        }

        public string OpenYearMonth { set { __OpenYearMonth = value; } get { return __OpenYearMonth; } }
        
        public DateTime MonthOpenForSales { set { __MonthOpenForSales = value; } get { return __MonthOpenForSales; } }
        public DateTime MonthOpenForInventory { set { __MonthOpenForInventory = value; } get { return __MonthOpenForInventory; } }
        public DateTime MonthOpenForAccounting { set { __MonthOpenForAccounting = value; } get { return __MonthOpenForAccounting; } }
        public DateTime MonthOpenForHRMS { set { __MonthOpenForHRMS = value; } get { return __MonthOpenForHRMS; } }
        public string WeekOpenForWeeklyOverdueCollection { set { __WeekOpenForWeeklyOverdueCollection = value; } get { return __WeekOpenForWeeklyOverdueCollection; } }
        public DateTime MonthOpenForDailyProgressReview { set { __MonthOpenForDailyProgressReview = value; } get { return __MonthOpenForDailyProgressReview; } }

        public DateTime TransactionOpenDate { set { __TransactionOpenDate = value; } get { return __TransactionOpenDate; } }

        public DateTime TransactionBackDate { set { __TransactionBackDate = value; } get { return __TransactionBackDate; } }

        public byte IsInventoryImplemented { set { __IsInventoryImplemented = value; } get { return __IsInventoryImplemented; } }
    }
}
