using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarHelper
{
    public static class Helper
    {
        #region Constant Value

        public const string HO = "RSFSUMMARY";
        public const string HOSales = "RSFSUMMARY";
        public const string HOInventory = "RSFSUMMARY";
        public const string HOAccounts = "RSFSUMMARY";
        public const string ZonalOffice = "ZONESUMMARY";
        public const string RegionalOffice = "REGIONSUMMARY";
        public const string AreaOffice = "AREASUMMARY";
        public const string UnitOffice = "INDIVIDUALUNIT";
        public const string UnitAudit = "UNTAUDITOR";
        public const string RacoOffice = "RSFSUMMARY";
        public const string ZonalAuditor = "ZOAUDITOR1";
        public const string RegionalAuditor = "REGAUDITOR";
        public const string HeadOfficeAuditor = "HOAUDITOR1";

        public const string HeadOffice = "HO";
        public const string Zone = "Zone";
        public const string Region = "Region";
        public const string Unit = "Unit";
        public const string HeadOfficeLocationCode = "9000";

        public const byte NewItem = 1;
        public const byte CustomerSupport = 2;
        public const byte SystemReturn = 3;

        public const byte NewSalesAgreement = 1;
        public const byte ResaleAgreementWithNewPackagePrice = 2;
        public const byte ResaleAgreementWithDepreciatedPackagePrice = 3;
        public const byte BondhuBati = 4;
        public const byte ResaleWithSpecialPackage = 5;

        public const string ReasonForUserOrAuditor = "AUDITOR";

        public const byte Active = 0;
        public const byte Inactive = 1;

        public const byte Open = 15;
        public const byte Completed = 16;

        public const byte ActiveCustomer = 0;
        public const byte FullPaiedCustomer = 5;
        public const byte NotMyCustomer = 7;
        public const byte CashSalesCustomer = 6;
        public const byte SystemReturnCustomer = 8;
        public const byte InTransferCustomer = 4;

        public const byte RegularCustomer = 11;
        public const byte AdvanceCustomer = 12;
        public const byte OverdueCustomer = 13;

        public const int OneMonthPrevious = 1;
        public const int TwoMonthPrevious = 2;
        public const int ThreeMonthPrevious = 3;

        public const string ForSales = "SAL";
        public const string ForInventory = "INV";
        public const string ForAccounting = "ACC";
        public const string ForHRMS = "HRM";
        public const string ForAuditor = "AUD";

        public const string PaymentModeCash = "Cash";
        public const string PaymentModeCredit = "Credit";

        public const string IsCapacityOnlyForPackagesAndItems = "PACKAGE-ITEM";
        public const string IsCapacityOnlyForItems = "ITEM";

        public const string CustomerIndividual = "PERSNL";
        public const string CustomerInstitute = "INSTIT";
        public const string CustomerReligiousInstitute = "MOSQUE";

        public const string Insert = "INSERT";
        public const string Update = "UPDATE";
        public const string Delete = "DELETE";
        public const string Select = "SELECT";

        public const byte SaleableOrGood = 20;
        public const byte NonSaleableDamageRepairable = 21;
        public const byte InsideRSF = 40;
        public const byte OutsideRSF = 41;
        public const byte FixedAssetItem = 14;

        public const string ReceiveOrIssue_Receive = "R";
        public const string ReceiveOrIssue_Issue = "I";
        public const string ReceiveEntrySource = "ITEMRECEIV";
        public const string ReceiveRefDocTypeWithChallan = "MRRWITHCHALLAN";
        public const string ReceiveRefDocTypeWithoutChallan = "MRRWITHOCHALLAN";
        public const string SparsePartsChallan = "ISSFSPSALE";
        public const string SparsePartsChallanForDisasterRecovery = "ISSFSPSWDS";
        public const string IssueForSales = "ISSINVSALE";
        public const string ReasonPurposeForIssue = "INVENTORYISSUE";
        public const string ReasonPurposeForReceive = "INVENTORYRECEIV";
        public const string ReasonPurposeForSales = "OTHERS";
        //public const string ReasonPurposeForCollection = "CUSTOMERCOLLECT";
        public static string[] ReasonPurposeForCollection = new string[3] { "CUSTOMERCOLLECT", "AUDITCLAIMCOLEC", "CUSLEDCORECTION" };
        public const string ReasonPurposeForAuditVoucher = "ACCOUNTINGADJ";

        public const string ContraVoucherForAuditor = "ACCADJFCONVOU";
        public const string PaymentVoucherForAuditor = "ACCADJFPAYVOU";
        public const string ReceiveVoucherForAuditor = "ACCADJFRECVOU";
        public const string NonCashVoucherForAuditor = "ACCADJFNCAVOU";

        public const string FixedAssets = "FIXEDASSET";
        public const string InventoryItem = "INVTORYITM";
        public const string StationaryItem = "CONSUMITEM";

        public const int UnitOfficeModule = 1;
        public const int RegionOfficeModule = 6;
        public const int ZonalOfficeModule = 7;
        public const int HeadOfficeModule = 9;

        public const string SuccessMessage = " Save Is Succeed";
        public const string UpdateMessage = " Update Is Succeed";
        public const string DeleteMessage = " Delete Is Succeed";

        public const string Yes = "YES";
        public const string No = "NO";
        public const string DBNullValue = "NULL";

        public const string CollectionTypeIOC = "IOC";
        public const string CollectionTypeAAA = "AAA";
        public const string CollectionTypeDRF = "DRF";
        public const string CollectionTypeSAC = "SAC";

        public const string RSFAddress = "Rural Services Foundation (PO ID: 12)\n 116 Arjotpara, Mohakhali, Dhaka \n";
        public const string RSFCompanyName = "Rural Services Foundation \n";
        public const string RSFAddressWithoutPOID = "Rural Services Foundation \n 116 Arjotpara, Mohakhali, Dhaka \n";
        public const string DefaultErrorMessage = "পেজটি বর্তমানে বন্ধ আছে। সাময়িক অসুবিধার জন্য আমরা আন্তরিক ভাবে দুঃখিত...কর্তৃপক্ষ";
        public const string CompanyName = "Rural Services Foundation";


        public const byte CashInHand = 1;
        public const byte CashAtBank = 2;
        public const byte CurrentACForUnitTransfer = 3;
        public const byte LoanACForProjectTransfer = 4;
        public const byte CollectionReconciliationAccount = 9;
        public const byte CurrentYearProfitLoss = 10;

        public const string ParentAccount = "P";
        public const string SubsidytAccount = "S";
        public const string CapitalAccount = "C";

        public const string ImagePath = "Content/EmployeeImages";
        public const string NoEmployeeImage = ImagePath + "/NoEmployeeImage.png";
        public const string NoEmployeeSignature = ImagePath + "/NoSignature.png";

        public const int ProbationPeriodDefault = 12;  // In Month
        public const int ContractPeriodDefault = 12; // In Month
        public const string GenderDefault = "M";

        public const byte TempApprovalForSalesAgreementStatus = 60;

        public const string CashMemuUsesIdFirst = "1STUSE";
        public const string NotApplicable = "NA";

        public const string ContraVoucherForAuditorEntrySource = "AUDNONCASH";
        public const string NonCashVoucherForAuditorEntrySource = "AUDCONTRAV";
        public const string PaymentVoucherForAuditorEntrySource = "AUDPAYMENT";
        public const string ReceiveVoucherForAuditorEntrySource = "AUDRECEIVE";

        public const string ContraVoucherForAccountsEntrySource = "CONTRAVCHR";
        public const string NonCashVoucherForAccountsEntrySource = "MANUALVCHR";
        public const string PaymentVoucherForAccountsEntrySource = "CASPAYMENT";
        public const string ReceiveVoucherForAccountsEntrySource = "CASRECEIVE";
        public const string BankPaymentVoucherForAccountsEntrySource = "PMNTBYBANK";

        public enum VoucherType
        {
            All = -1,

            [Description("Cash/Bank Voucher")]
            CashBankVoucher = 1,

            [Description("Non Cash/Bank Voucher")]
            NoneCashBankVoucher = 0
        }

        public enum CashBankVoucherType
        {
            All = 'A',

            [Description("Payment Voucher")]
            PaymentVoucher = 'P',

            [Description("Receive Voucher")]
            ReceiveVoucher = 'R',

            [Description("Contra Voucher")]
            ContraVoucher = 'C'
        }

        #endregion

        public static string Location(LoginHelper objLoginHelper)
        {
            string locationType = string.Empty;

            if (objLoginHelper.UerRoleOrGroupID == UserGroup.Administrator)
            {

            }
            else if (objLoginHelper.UerRoleOrGroupID == UserGroup.HeadOfficeAudit)
            {
                locationType = "";
            }
            else if (objLoginHelper.UerRoleOrGroupID == UserGroup.HeadOfficeFinanceAccount)
            {
                locationType = "";
            }
            else if (objLoginHelper.UerRoleOrGroupID == UserGroup.HeadOfficeInventory)
            {
                locationType = "";
            }
            else if (objLoginHelper.UerRoleOrGroupID == UserGroup.HeadOfficeSales)
            {
                locationType = "";
            }
            else if (objLoginHelper.UerRoleOrGroupID == UserGroup.HeadOfficeIT)
            {
                locationType = "";
            }
            else if (objLoginHelper.UerRoleOrGroupID == UserGroup.ZonalManager)
            {
                locationType = objLoginHelper.LogInForZoneCode;
            }
            else if (objLoginHelper.UerRoleOrGroupID == UserGroup.ZonalITUser)
            {
                locationType = objLoginHelper.LogInForZoneCode;
            }
            else if (objLoginHelper.UerRoleOrGroupID == UserGroup.RegionManager)
            {
                locationType = objLoginHelper.LogInForRegionCode;
            }
            else if (objLoginHelper.UerRoleOrGroupID == UserGroup.UnitManager)
            {
                locationType = objLoginHelper.LogInForUnitCode;
            }
            else if (objLoginHelper.UerRoleOrGroupID == UserGroup.UnitAuditor)
            {
                locationType = objLoginHelper.LogInForUnitCode;
            }

            return locationType;
        }

        public static string LocationName(LoginHelper objLoginHelper)
        {
            string locationName = string.Empty;

            if (objLoginHelper.UerRoleOrGroupID == UserGroup.Administrator)
            {
                locationName = "Head Office";
            }
            else if (objLoginHelper.UerRoleOrGroupID == UserGroup.HeadOfficeAudit)
            {
                locationName = "Head Office";
            }
            else if (objLoginHelper.UerRoleOrGroupID == UserGroup.HeadOfficeFinanceAccount)
            {
                locationName = "Head Office";
            }
            else if (objLoginHelper.UerRoleOrGroupID == UserGroup.HeadOfficeInventory)
            {
                locationName = "Head Office";
            }
            else if (objLoginHelper.UerRoleOrGroupID == UserGroup.HeadOfficeSales)
            {
                locationName = "Head Office";
            }
            else if (objLoginHelper.UerRoleOrGroupID == UserGroup.HeadOfficeIT)
            {
                locationName = "Head Office";
            }
            else if (objLoginHelper.UerRoleOrGroupID == UserGroup.RacoReviewer)
            {
                locationName = "Head Office";
            }
            else if (objLoginHelper.UerRoleOrGroupID == UserGroup.ZonalManager)
            {
                locationName = objLoginHelper.LogInForZoneName;
            }
            else if (objLoginHelper.UerRoleOrGroupID == UserGroup.ZonalITUser)
            {
                locationName = objLoginHelper.LogInForZoneCode;
            }
            else if (objLoginHelper.UerRoleOrGroupID == UserGroup.RegionManager)
            {
                locationName = objLoginHelper.LogInForRegionName;
            }
            else if (objLoginHelper.UerRoleOrGroupID == UserGroup.UnitManager)
            {
                locationName = objLoginHelper.LogInForUnitName;
            }
            else if (objLoginHelper.UerRoleOrGroupID == UserGroup.UnitAuditor)
            {
                locationName = objLoginHelper.LogInForUnitName;
            }

            return locationName;
        }

        public static string LocationCode(LoginHelper objLoginHelper)
        {
            string locationCodes = string.Empty;

            if (objLoginHelper.ReportType == Helper.HOSales)
            {
                locationCodes = "9000";
            }
            else if (objLoginHelper.ReportType == Helper.ZonalOffice)
            {
                locationCodes = objLoginHelper.LogInForZoneCode;
            }
            else if (objLoginHelper.ReportType == Helper.RegionalOffice)
            {
                locationCodes = objLoginHelper.LogInForRegionCode;
            }
            else if (objLoginHelper.ReportType == Helper.UnitOffice)
            {
                locationCodes = objLoginHelper.LogInForUnitCode;
            }

            return locationCodes;
        }

        public static string URL(LoginHelper objLoginHelper)
        {
            string pageUrl = string.Empty;

            if (!string.IsNullOrEmpty(objLoginHelper.URLSelectionLocation))
            {
                pageUrl = objLoginHelper.URLSelectionLocation;
            }
            else
            {
                pageUrl = objLoginHelper.URL;
            }

            return pageUrl;
        }

        public static string DateFormat(DateTime dates)
        {
            return Convert.ToDateTime(dates).ToString("dd-MMM-yy");
        }

        public static string DateFormatYYMMDD(DateTime date)
        {
            return Convert.ToDateTime(date).ToString("yyMMdd");
        }

        public static string DateFormatMMDDYYYY(string dates)
        {
            if (dates.Contains('-'))
            {
                dates = dates.Replace("-", "/");
            }

            string[] dateSplit = dates.Split('/');

            string dateMMDDYYYY = dateSplit[1] + "/" + dateSplit[0] + "/" + dateSplit[2];

            return dateMMDDYYYY;
        }

        public static string DateFormatMMDDYYYY(DateTime dates)
        {
            return Convert.ToDateTime(dates).ToString("MM-dd-yyyy");
        }

        public static DateTime DateFormatMMDDYYYYToDate(string dates)
        {
            string st = DateFormatMMDDYYYY(dates);
            return Convert.ToDateTime(st);
        }

        public static string DateFormatDDMMMYYYY(string dates)
        {
            string dateToMMDDYYYY = DateFormatMMDDYYYY(dates);

            dateToMMDDYYYY = Convert.ToDateTime(dateToMMDDYYYY).ToString("dd-MMM-yyyy");

            return dateToMMDDYYYY;
        }

        public static string YearMonthCurrent()
        {
            string currentYearMonth = string.Empty;
            string currentDate = DateTime.Now.Date.ToString("dd/MM/yyyy");

            string[] dateSplit = currentDate.ToString().Split('/');

            currentYearMonth = dateSplit[2] + dateSplit[1];  // Year + Month

            return currentYearMonth;
        }

        public static string YearMonth(string yearMonth)
        {
            string currentYearMonth = string.Empty;
            string currentDate = yearMonth;

            string[] dateSplit = currentDate.ToString().Split('/');

            string month = "00";

            switch (dateSplit[0])
            {
                case "January":
                    month = "01";
                    break;
                case "February":
                    month = "02";
                    break;
                case "March":
                    month = "03";
                    break;
                case "April":
                    month = "04";
                    break;
                case "May":
                    month = "05";
                    break;
                case "June":
                    month = "06";
                    break;
                case "July":
                    month = "07";
                    break;
                case "August":
                    month = "08";
                    break;
                case "September":
                    month = "09";
                    break;
                case "October":
                    month = "10";
                    break;
                case "November":
                    month = "11";
                    break;
                case "December":
                    month = "12";
                    break;
            }

            currentYearMonth = dateSplit[1] + month;  // Year + Month



            return currentYearMonth;
        }

        public static string YearMonthNumberToYearMonthText(string yearMonth)
        {
            string years = string.Empty, months = string.Empty, yearMonthText = string.Empty;

            years = yearMonth.Substring(0, 4);
            months = yearMonth.Substring(4);

            yearMonthText = MonthDigitToName(Convert.ToInt32(months)) + "/" + years;

            return yearMonthText;
        }

        public static string YearMonthPrevious(int howMuchPrevious)
        {
            string currentYearMonth = string.Empty;
            string currentDate = DateTime.Now.Date.AddMonths(howMuchPrevious * -1).ToString("dd/MM/yyyy");

            string[] dateSplit = currentDate.ToString().Split('/');

            currentYearMonth = dateSplit[2] + dateSplit[1];  // Year + Month

            return currentYearMonth;
        }

        public static string YearMonthPrevious(string yearMonth, int howMuchPrevious)
        {
            string previousYearmonth;

            string years = yearMonth.Substring(0, 4);
            string months = yearMonth.Substring(4);

            months = (int.Parse(months) - howMuchPrevious).ToString();

            if (int.Parse(months) > 0 && int.Parse(months) < 10)
            {
                months = "0" + months;
            }

            previousYearmonth = years + months;

            return previousYearmonth;
        }

        public static string ConvertDateToYearMonth(DateTime date)
        {
            string currentYearMonth = date.ToString("dd/MM/yyyy");

            string[] dateSplit = currentYearMonth.ToString().Split('/');

            currentYearMonth = dateSplit[2] + dateSplit[1];  // Year + Month

            return currentYearMonth;
        }

        public static DateTime DateFormat(string dateText)
        {
            DateTime dt = DateTime.Now;
            string[] dateSplit;
            string dateFormat = string.Empty;

            if (dateText.Contains('/'))
            {
                dateSplit = dateText.Split('/');
                dateFormat = dateSplit[1] + "/" + dateSplit[0] + "/" + dateSplit[2];
                dt = Convert.ToDateTime(dateFormat);
            }
            else if (dateText.Contains('-'))
            {
                dateSplit = dateText.Split('-');
                dateFormat = dateSplit[1] + "/" + dateSplit[0] + "/" + dateSplit[2];
                dt = Convert.ToDateTime(dateFormat);
            }
            else if (dateText.Contains('.'))
            {
                dateSplit = dateText.Split('.');
                dateFormat = dateSplit[1] + "/" + dateSplit[0] + "/" + dateSplit[2];
                dt = Convert.ToDateTime(dateFormat);
            }

            return dt;
        }

        public static DateTime DateFrom(string yearMonth)
        {
            DateTime dFrom = DateTime.Now;
            string dateFormat = string.Empty;

            dateFormat = yearMonth.Substring(0, 4) + "/" + yearMonth.Substring(4) + "/" + "01";
            dFrom = Convert.ToDateTime(dateFormat);

            return dFrom;
        }

        public static DateTime DateTo(string yearMonth)
        {
            DateTime dTo = DateTime.Now;
            string dateFormat = string.Empty;

            dateFormat = yearMonth.Substring(0, 4) + "/" + yearMonth.Substring(4);
            yearMonth = yearMonth.Substring(4);

            switch (yearMonth)
            {
                case "01":
                    dateFormat += "/" + "31";
                    dTo = Convert.ToDateTime(dateFormat);
                    break;
                case "02":
                    dateFormat += "/" + "28";
                    dTo = Convert.ToDateTime(dateFormat);
                    break;
                case "03":
                    dateFormat += "/" + "31";
                    dTo = Convert.ToDateTime(dateFormat);
                    break;
                case "04":
                    dateFormat += "/" + "30";
                    dTo = Convert.ToDateTime(dateFormat);
                    break;
                case "05":
                    dateFormat += "/" + "31";
                    dTo = Convert.ToDateTime(dateFormat);
                    break;
                case "06":
                    dateFormat += "/" + "30";
                    dTo = Convert.ToDateTime(dateFormat);
                    break;
                case "07":
                    dateFormat += "/" + "31";
                    dTo = Convert.ToDateTime(dateFormat);
                    break;
                case "08":
                    dateFormat += "/" + "31";
                    dTo = Convert.ToDateTime(dateFormat);
                    break;
                case "09":
                    dateFormat += "/" + "30";
                    dTo = Convert.ToDateTime(dateFormat);
                    break;
                case "10":
                    dateFormat += "/" + "31";
                    dTo = Convert.ToDateTime(dateFormat);
                    break;
                case "11":
                    dateFormat += "/" + "30";
                    dTo = Convert.ToDateTime(dateFormat);
                    break;
                case "12":
                    dateFormat += "/" + "31";
                    dTo = Convert.ToDateTime(dateFormat);
                    break;
            }

            return dTo;
        }

        public static string OpenEntryMonth(string yearMonth)
        {
            string CurrentMonth = yearMonth.Substring(4);
            string CurrentYear = yearMonth.Substring(0, 4);
            string OpenMonth = string.Empty;

            if (int.Parse(CurrentMonth) == 12)
            {
                OpenMonth = (int.Parse(CurrentYear) + 1).ToString() + "01";
            }
            else if (int.Parse(CurrentMonth) > 0 && int.Parse(CurrentMonth) < 9)
            {
                OpenMonth = CurrentYear + "0" + (int.Parse(CurrentMonth) + 1).ToString();
            }
            else if (int.Parse(CurrentMonth) > 9 && int.Parse(CurrentMonth) < 13)
            {
                OpenMonth = CurrentYear + (int.Parse(CurrentMonth) + 1).ToString();
            }

            return OpenMonth;
        }

        public static DateTime MonthStartDate(string yearMonth)
        {
            DateTime dateFormat;

            string years = yearMonth.Substring(0, 4);
            string months = yearMonth.Substring(4);

            dateFormat = Convert.ToDateTime(years + "-" + months + "-" + "01");

            return dateFormat;
        }

        public static DateTime MonthEndDate(string yearMonth)
        {
            DateTime dateFormat;

            string years = yearMonth.Substring(0, 4);
            string months = yearMonth.Substring(4);
            string days = "30";

            if (DateTime.IsLeapYear(Convert.ToInt32(years)))
            {
                days = "29";
            }
            else if (months == "02")
            {
                days = "28";
            }

            dateFormat = Convert.ToDateTime(years + "-" + months + "-" + days);

            return dateFormat;
        }

        public static DateTime MonthStartDate(DateTime date)
        {
            DateTime dateFormat;

            string years = date.Year.ToString();
            string months = date.Month.ToString();

            dateFormat = Convert.ToDateTime(years + "-" + months + "-" + "01");

            return dateFormat;
        }

        public static DateTime MonthStartDateFromMonthNumber(int month, DateTime date)
        {
            DateTime dateFormat;
            dateFormat = Convert.ToDateTime(date.Year.ToString() + "-" + month.ToString() + "-" + "01");

            return dateFormat;
        }

        public static DateTime MonthEndDateFromMonthNumber(int month, DateTime date)
        {
            DateTime dateFormat;
            dateFormat = Convert.ToDateTime(date.Year.ToString() + "-" + month.ToString() + "-" + MonthNumberToEndDays(month).ToString());

            return dateFormat;
        }

        public static DateTime MonthEndDate(DateTime date)
        {
            DateTime dateFormat;

            string years = date.Year.ToString();
            string months = date.Month.ToString();
            string days = "30";

            if (DateTime.IsLeapYear(Convert.ToInt32(years)))
            {
                days = "29";
            }
            else if (months == "02")
            {
                days = "28";
            }

            dateFormat = Convert.ToDateTime(years + "-" + months + "-" + days);

            return dateFormat;
        }

        public static int NumberOfCharacter(string rsfString)
        {
            int totalCharacter = 0;

            foreach (char ch in rsfString)
            {
                totalCharacter++;
            }

            return totalCharacter;
        }

        public static int CurrentWeekOfYear(DateTime dates)
        {
            CultureInfo myCI = new CultureInfo("en-US");
            System.Globalization.Calendar myCal = myCI.Calendar;

            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;

            int currentWeekOfTheYear = myCal.GetWeekOfYear(dates, myCWR, myFirstDOW);

            string dd = dates.DayOfWeek.ToString();

            if (dd == "Sunday")
            {
                currentWeekOfTheYear -= 1;
            }

            return currentWeekOfTheYear;
        }

        public static DateTime FirstDateOfCurrentWeek(DateTime date)
        {
            CultureInfo myCI = new CultureInfo("en-US");

            int year = date.Year;
            int weekNumber = CurrentWeekOfYear(date);

            DateTime currentWeekStartDate = date.Date;

            DateTime jan1 = new DateTime(year, 1, 1);

            int daysOffset = DayOfWeek.Sunday - jan1.DayOfWeek;
            DateTime firstMonday = jan1.AddDays(daysOffset);

            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstMonday, myCI.DateTimeFormat.CalendarWeekRule, DayOfWeek.Sunday);

            if (firstWeek < 1)
            {
                weekNumber -= 1;
            }

            currentWeekStartDate = firstMonday.AddDays(weekNumber * 7).AddDays(-6);

            return currentWeekStartDate.Date;
        }

        public static DateTime LastDateOfCurrentWeek(DateTime date)
        {
            DateTime lastWeekStartDate = date.Date;

            DateTime firstDateOfCurrentWeek = FirstDateOfCurrentWeek(date).AddDays(-1);

            lastWeekStartDate = firstDateOfCurrentWeek.AddDays(7);

            return lastWeekStartDate.Date;
        }

        public static string YearWeek(DateTime date)
        {
            string yearWeekCurrent = string.Empty;
            int weekNumber = CurrentWeekOfYear(date);

            if (weekNumber < 10)
            {
                yearWeekCurrent = date.Year.ToString() + weekNumber.ToString().PadLeft(2, '0');
            }
            else
            {
                yearWeekCurrent = date.Year.ToString() + weekNumber.ToString();
            }

            return yearWeekCurrent;
        }

        public static DateTime YearWeekToStartDate(string yearWeek)
        {
            int year = Convert.ToInt32(yearWeek.Substring(0, 4));
            int weekNumber = Convert.ToInt32(yearWeek.Substring(4));

            CultureInfo myCI = new CultureInfo("en-US");
            DateTime currentWeekStartDate = DateTime.Now.Date;

            DateTime jan1 = new DateTime(year, 1, 1);

            int daysOffset = DayOfWeek.Sunday - jan1.DayOfWeek;
            DateTime firstMonday = jan1.AddDays(daysOffset);

            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstMonday, myCI.DateTimeFormat.CalendarWeekRule, DayOfWeek.Sunday);

            if (firstWeek < 1)
            {
                weekNumber -= 1;
            }

            currentWeekStartDate = firstMonday.AddDays(weekNumber * 7).AddDays(-6);

            return currentWeekStartDate.Date;
        }

        public static DateTime YearWeekToEndDate(string yearWeek)
        {
            int year = Convert.ToInt32(yearWeek.Substring(0, 4));
            int weekNumber = Convert.ToInt32(yearWeek.Substring(4));

            CultureInfo myCI = new CultureInfo("en-US");
            DateTime lastWeekStartDate = DateTime.Now.Date;

            DateTime jan1 = new DateTime(year, 1, 1);

            int daysOffset = DayOfWeek.Sunday - jan1.DayOfWeek;
            DateTime firstMonday = jan1.AddDays(daysOffset);

            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstMonday, myCI.DateTimeFormat.CalendarWeekRule, DayOfWeek.Sunday);

            if (firstWeek < 1)
            {
                weekNumber -= 1;
            }

            lastWeekStartDate = firstMonday.AddDays(weekNumber * 7);

            return lastWeekStartDate.Date;
        }

        public static int PaymentMode(string paymentModeId)
        {
            int payment = 0;

            if (paymentModeId == "03YEAR")
            {
                payment = 36;
            }
            else if (paymentModeId == "02YEAR")
            {
                payment = 24;
            }
            else if (paymentModeId == "01YEAR")
            {
                payment = 12;
            }

            return payment;
        }

        public static byte StockLocationSelection(byte stockLocation)
        {
            byte stockLocationStatus = 0;

            if (stockLocation == 1)
            { stockLocationStatus = SaleableOrGood; }
            else if (stockLocation == 2)
            { stockLocationStatus = SaleableOrGood; }
            else if (stockLocation == 3)
            { stockLocationStatus = SaleableOrGood; }
            else if (stockLocation == 4)
            { stockLocationStatus = NonSaleableDamageRepairable; }
            else if (stockLocation == 5)
            { stockLocationStatus = NonSaleableDamageRepairable; }
            else if (stockLocation == 6)
            { stockLocationStatus = NonSaleableDamageRepairable; }
            else if (stockLocation == 7)
            { stockLocationStatus = NonSaleableDamageRepairable; }

            return stockLocationStatus;
        }

        public static byte UserLocationType(string locationType)
        {
            byte locationTypeID = 0;

            if (locationType == Helper.Unit)
            {
                locationTypeID = 1;
            }
            else if (locationType == Helper.Region)
            {
                locationTypeID = 6;
            }
            else if (locationType == Helper.Zone)
            {
                locationTypeID = 7;
            }
            else if (locationType == Helper.HeadOffice)
            {
                locationTypeID = 9;
            }

            return locationTypeID;
        }

        public static string UserLocationTypeName(byte locationTypeId)
        {
            string locationType = string.Empty;

            if (locationTypeId == 1)
            {
                locationType = Helper.Unit;
            }
            else if (locationTypeId == 6)
            {
                locationType = Helper.Region;
            }
            else if (locationTypeId == 7)
            {
                locationType = Helper.Zone;
            }
            else if (locationTypeId == 9)
            {
                locationType = HeadOffice;
            }

            return locationType;
        }

        /// <summary>
        /// Get MRR Type By Issue Type
        /// </summary>
        /// <param name="issueType">Issue Type ID</param>
        /// <returns>Valid MRR Type</returns>
        public static string MrrType(string issueType)
        {
            string receiveType = string.Empty;

            if (issueType == "ISSTOOTRUT")
            {
                receiveType = "RCVFRMORUT";
            }
            else if (issueType == "ISSTOHEADO")
            {
                receiveType = "RCVEFROMHO";
            }
            else if (issueType == "ISSTOZNLOF")
            {
                receiveType = "RCVFRMZNLO";
            }

            return receiveType;
        }

        /// <summary>
        /// Get MRR Type By Issue Type and Location
        /// </summary>
        /// <param name="issueType">Issue Type ID</param>
        /// <param name="locationType">Location Type (ie. HO, Unit, Zone) (reference objLoginHelper.Location)</param>
        /// <returns>Valid MRR Type</returns>
        public static string MrrType(string issueType, string locationType)
        {
            string receiveType = string.Empty;

            if (locationType == Helper.HeadOffice)
            {
                if (issueType == "ISSTOOTRUT")
                {
                    receiveType = "RCVEFROMHO";
                }
                else if (issueType == "ISSTOHEADO")
                {
                    receiveType = "RCVEFROMHO";
                }
                else if (issueType == "ISSTOZNLOF")
                {
                    receiveType = "RCVEFROMHO";
                }
            }
            else if (locationType == Helper.Unit)
            {
                if (issueType == "ISSTOOTRUT")
                {
                    receiveType = "RCVFRMORUT";
                }
                else if (issueType == "ISSTOHEADO")
                {
                    receiveType = "RCVFRMORUT";
                }
                else if (issueType == "ISSTOZNLOF")
                {
                    receiveType = "RCVFRMORUT";
                }
            }
            else if (locationType == Helper.Zone)
            {
                if (issueType == "ISSTOOTRUT")
                {
                    receiveType = "RCVFRMZNLO";
                }
                else if (issueType == "ISSTOHEADO")
                {
                    receiveType = "RCVFRMZNLO";
                }
            }

            return receiveType;
        }

        public static DateTime DateFromDayAndYearMonth(int date, string yearMonth)
        {
            string dateFormat = yearMonth.Substring(4) + "/" + date.ToString() + "/" + yearMonth.Substring(0, 4);
            DateTime dt = Convert.ToDateTime(dateFormat);
            return dt;
        }

        public static byte MRRItemsCondition(byte mrrType, byte itemCondition)
        {
            byte mrrItemOriginalStockLocation = 0;

            if (mrrType == 1)
            {
                if (itemCondition == 20)
                {
                    mrrItemOriginalStockLocation = 1;
                }
                else if (itemCondition == 21)
                {
                    mrrItemOriginalStockLocation = 11;
                }
            }
            else if (mrrType == 2)
            {
                if (itemCondition == 20)
                {
                    mrrItemOriginalStockLocation = 2;
                }
                else if (itemCondition == 21)
                {
                    mrrItemOriginalStockLocation = 12;
                }
            }
            else if (mrrType == 3)
            {
                if (itemCondition == 20)
                {
                    mrrItemOriginalStockLocation = 3;
                }
                else if (itemCondition == 21)
                {
                    mrrItemOriginalStockLocation = 13;
                }
            }
            else
            {
                mrrItemOriginalStockLocation = mrrType;
            }

            return mrrItemOriginalStockLocation;
        }

        //public static string AccessRightAllowOrNot(string controllerMethodName, LoginHelper objLoginHelper)
        //{
        //    string message = string.Empty;
        //    List<ListOfPagesWithAccessRightsForThisUser> lstPagesWithAccessRightsForThisUser = new List<ListOfPagesWithAccessRightsForThisUser>();
        //    //lstPagesWithAccessRightsForThisUser = new RASolarSecurity.Model.RASolarSecurityReposritory().ReadListOfPagesWithAccessRightsForUser(objLoginHelper.LogInID, "LISTOFALLPAGES", string.Empty);

        //    ListOfPagesWithAccessRightsForThisUser objPagesWithAccessRightsForThisUser = new ListOfPagesWithAccessRightsForThisUser();
        //    PageControllerMethodVSPageIDs objPageControllerMethodVSPageID = new PageControllerMethodVSPageIDs();

        //    if (lstPagesWithAccessRightsForThisUser != null || lstPagesWithAccessRightsForThisUser.Count > 0)
        //    {
        //        objPageControllerMethodVSPageID = new PageControllerMethodVSPageIDs().PageControllerMethodVSPageId().Where(p => p.PageControllerMethod == controllerMethodName).FirstOrDefault();

        //        if (objPageControllerMethodVSPageID != null)
        //        {
        //            objPagesWithAccessRightsForThisUser = lstPagesWithAccessRightsForThisUser.Where(p => p.PageID == objPageControllerMethodVSPageID.PageID).FirstOrDefault();

        //            if (objPagesWithAccessRightsForThisUser.IsAccessibleForThisUser == Helper.No)
        //            {
        //                if (objPagesWithAccessRightsForThisUser.MessageToShow != null)
        //                {
        //                    message = objPagesWithAccessRightsForThisUser.PageName + " " + objPagesWithAccessRightsForThisUser.MessageToShow;
        //                }
        //                else
        //                {
        //                    message = objPagesWithAccessRightsForThisUser.PageName + " Is Currently Not Available";
        //                }
        //            }
        //        }
        //    }

        //    return message;
        //}

        public static DateTime InvalidDate()
        {
            return new DateTime(1900, 1, 1);
        }

        public static string ChallanCequenceNumberGeneration(string challanNumberMax, LoginHelper objLoginHelper)
        {
            int challSeqNo = 0;
            string challnSequenceNumberNew = string.Empty;

            if (!string.IsNullOrEmpty(challanNumberMax))
            {
                challSeqNo = Convert.ToInt32(challanNumberMax.Substring(6));
                challSeqNo += 1;

                challnSequenceNumberNew = objLoginHelper.TransactionOpenDate.ToString("yyMMdd");
                challnSequenceNumberNew += Convert.ToString(challSeqNo).PadLeft(3, '0');
            }
            else
            {
                challnSequenceNumberNew = objLoginHelper.TransactionOpenDate.ToString("yyMMdd") + "001";
            }

            return challnSequenceNumberNew;
        }

        public static string ChallanCequenceNumberGeneration(string challanNumberMax)
        {
            int challSeqNo = 0;
            string challnSequenceNumberNew = string.Empty;

            if (!string.IsNullOrEmpty(challanNumberMax))
            {
                challSeqNo = Convert.ToInt32(challanNumberMax.Substring(6));
                challSeqNo += 1;

                challnSequenceNumberNew = challanNumberMax.Substring(0, 6);
                challnSequenceNumberNew += Convert.ToString(challSeqNo).PadLeft(3, '0');
            }
            else
            {
                challnSequenceNumberNew = challanNumberMax.Substring(0, 6) + "001";
            }

            return challnSequenceNumberNew;
        }

        public static string AccountSequenceNumberGeneration(ArrayList accountTransactionNumberMax, LoginHelper objLoginHelper)
        {
            int transNo = 0, transNo1 = 0, transNo2 = 0;
            string transSequenceNumberNew = string.Empty;

            if (accountTransactionNumberMax.Count > 0)
            {
                if (accountTransactionNumberMax.Count == 1)
                {
                    transNo1 = Convert.ToInt32(accountTransactionNumberMax[0].ToString().Substring(6));
                    transNo2 = 1;
                }
                else if (accountTransactionNumberMax.Count == 2)
                {
                    transNo1 = Convert.ToInt32(accountTransactionNumberMax[0].ToString().Substring(6));
                    transNo2 = Convert.ToInt32(accountTransactionNumberMax[1].ToString().Substring(6));
                }

                if (transNo1 > transNo2)
                {
                    transNo = ++transNo1;
                }
                else if (transNo2 > transNo1)
                {
                    transNo = ++transNo2;
                }
                else
                {
                    transNo = ++transNo1;
                }

                transSequenceNumberNew = objLoginHelper.CurrentDate.ToString("yyMMdd");
                transSequenceNumberNew += Convert.ToString(transNo).PadLeft(4, '0');
            }
            else
            {
                transSequenceNumberNew = objLoginHelper.CurrentDate.ToString("yyMMdd") + "0001";
            }

            return transSequenceNumberNew;
        }

        public static string AccountSequenceNumberGeneration(string accountTransactionnumberMax)
        {
            int transNo = 0;
            string transSequenceNumberNew = string.Empty;

            if (!string.IsNullOrEmpty(accountTransactionnumberMax))
            {
                transNo = Convert.ToInt32(accountTransactionnumberMax.Substring(6));
                transNo += 1;

                transSequenceNumberNew = accountTransactionnumberMax.Substring(0, 6);
                transSequenceNumberNew += Convert.ToString(transNo).PadLeft(4, '0');
            }
            else
            {
                transSequenceNumberNew = accountTransactionnumberMax.Substring(0, 6) + "0001";
            }

            return transSequenceNumberNew;
        }

        public static bool ChallanSequenceNumberDuplicationCheck(string currentSequenceNumber, string dbSequenceNumber)
        {
            bool validityOfSequenceNumber = true;
            string cSequence = currentSequenceNumber.Substring(6), dbSequence = dbSequenceNumber.Substring(6);

            if (Convert.ToInt32(cSequence) <= Convert.ToInt32(dbSequence))
            {
                validityOfSequenceNumber = false;
            }

            return validityOfSequenceNumber;
        }

        public static string CalculateDateDifferanceYearMonthDay(DateTime dateForm, DateTime dateTo)
        {
            int days = dateTo.Day - dateForm.Day;
            if (days < 0)
            {
                dateTo = dateTo.AddMonths(-1);
                days += DateTime.DaysInMonth(dateTo.Year, dateTo.Month);
            }

            int months = dateTo.Month - dateForm.Month;
            if (months < 0)
            {
                dateTo = dateTo.AddYears(-1);
                months += 12;
            }

            int years = dateTo.Year - dateForm.Year;

            return string.Format("{0} Year{1}, {2} Month{3} and {4} Day{5}",
                                 years, (years == 1 || years == 0) ? "" : "s",
                                 months, (months == 1 || months == 0) ? "" : "s",
                                 days, (days == 1 || days == 0) ? "" : "s");

        }

        public static string VoucherTypeSelection(string voucherType)
        {
            string CBVType = string.Empty;

            if (voucherType == Convert.ToChar(Helper.CashBankVoucherType.PaymentVoucher).ToString())
            {
                CBVType = Convert.ToChar(Helper.CashBankVoucherType.PaymentVoucher).ToString();
            }
            else if (voucherType == Convert.ToChar(Helper.CashBankVoucherType.ReceiveVoucher).ToString())
            {
                CBVType = Convert.ToChar(Helper.CashBankVoucherType.ReceiveVoucher).ToString();
            }
            else if (voucherType == Convert.ToChar(Helper.CashBankVoucherType.ContraVoucher).ToString())
            {
                CBVType = Convert.ToChar(Helper.CashBankVoucherType.ContraVoucher).ToString();
            }
            else if (voucherType == Convert.ToChar(Helper.CashBankVoucherType.ContraVoucher).ToString())
            {
                CBVType = Convert.ToChar(Helper.CashBankVoucherType.ContraVoucher).ToString();
            }
            else
            {
                CBVType = Convert.ToChar(Helper.CashBankVoucherType.All).ToString();
            }

            return CBVType;
        }

        public static bool VoucherSelection(bool voucher)
        {
            bool CBVoucher = true;

            if (voucher == true)
            {
                CBVoucher = Convert.ToBoolean(Helper.VoucherType.CashBankVoucher);
            }
            else if (voucher == false)
            {
                CBVoucher = Convert.ToBoolean(Helper.VoucherType.NoneCashBankVoucher);
            }

            return CBVoucher;
        }

        public static bool VoucherSelection(string voucherType)
        {
            bool CBVoucher = true;

            if (voucherType == Convert.ToChar(Helper.CashBankVoucherType.PaymentVoucher).ToString())
            {
                CBVoucher = Convert.ToBoolean(Helper.VoucherType.CashBankVoucher);
            }
            else if (voucherType == Convert.ToChar(Helper.CashBankVoucherType.ReceiveVoucher).ToString())
            {
                CBVoucher = Convert.ToBoolean(Helper.VoucherType.CashBankVoucher);
            }
            else if (voucherType == Convert.ToChar(Helper.CashBankVoucherType.ContraVoucher).ToString())
            {
                CBVoucher = Convert.ToBoolean(Helper.VoucherType.CashBankVoucher);
            }
            else if (voucherType == Convert.ToChar(Helper.CashBankVoucherType.ContraVoucher).ToString())
            {
                CBVoucher = Convert.ToBoolean(Helper.VoucherType.CashBankVoucher);
            }
            else
            {
                CBVoucher = Convert.ToBoolean(Helper.VoucherType.NoneCashBankVoucher);
            }

            return CBVoucher;
        }

        public static string EmployeeIdNew(string employeeIdMax)
        {
            string employeeId = string.Empty;

            if (string.IsNullOrEmpty(employeeIdMax))
            {
                employeeId = "RSF" + "00001";
            }
            else
            {
                if (employeeIdMax.Trim().Contains("RSF-"))
                {

                    string employeeExistingId = employeeIdMax.Substring(4);
                    employeeId = "RSF0" + (Convert.ToInt32(employeeExistingId) + 1);
                }
                else if (employeeIdMax.Trim().Contains("RSF"))
                {
                    string employeeExistingId = employeeIdMax.Substring(3);
                    int checkEmployeeExistingId = (Convert.ToInt32(employeeExistingId) + 1);
                    
                    if (checkEmployeeExistingId.ToString().Length <5)
                       employeeId = "RSF0" + checkEmployeeExistingId.ToString();
                    else
                        employeeId = "RSF" + checkEmployeeExistingId.ToString();
                }
            }

            return employeeId;
        }

        public static string EmployeeIdProcess(string employeeIdInNumberOnly)
        {
            string employeeId = string.Empty;

            if (string.IsNullOrEmpty(employeeIdInNumberOnly))
            {
                employeeId = "RSF" + "00001";
            }
            else if (!(employeeIdInNumberOnly.Contains("RSF") || employeeIdInNumberOnly.Contains("rsf")))
            {
                if (employeeIdInNumberOnly.Length == 4)
                    employeeId = "RSF-" + employeeIdInNumberOnly.PadLeft(4, '0');
                else if (employeeIdInNumberOnly.Length == 5)
                    employeeId = "RSF" + employeeIdInNumberOnly.PadLeft(5,'0');
            }
            else
            {
                if (employeeIdInNumberOnly.Trim().Contains("RSF-"))
                {
                    employeeIdInNumberOnly = employeeIdInNumberOnly.Remove(0, 4);
                    employeeId = "RSF-" + employeeIdInNumberOnly.PadLeft(4, '0');
                }
                else if (employeeIdInNumberOnly.Trim().Contains("RSF"))
                {
                    employeeIdInNumberOnly = employeeIdInNumberOnly.Remove(0, 3);
                    employeeId = "RSF" + employeeIdInNumberOnly.PadLeft(3, '0');
                }
            }

            return employeeId;
        }

        public static string EmployeeImagePath(string employeeId)
        {
            return Helper.ImagePath + "/" + employeeId + "-Img.jpg";
        }

        public static string EmployeeSignaturePath(string employeeId)
        {
            return Helper.ImagePath + "/" + employeeId + "-Sin.jpg";
        }

        public static byte EmployeeWiseSalaryStructureSeqenceNumberGeneration(byte sequenceNumber)
        {
            return (++sequenceNumber);
        }

        public static bool EmployeeIdValidation(string employeeId)
        {
            bool employeeIdValidOrNot = true;

            try
            {
                if (string.IsNullOrEmpty(employeeId))
                {
                    employeeIdValidOrNot = false;
                }
                else if (!employeeId.Trim().Contains("RSF"))
                {
                    employeeIdValidOrNot = false;
                }
                else if (employeeId.Trim().Contains("RSF-"))
                {
                    employeeIdValidOrNot = false;
                }
                else if (employeeId.Trim().Length < 8)
                {
                    employeeIdValidOrNot = false;
                }
                else if (employeeId.Trim().Length > 8)
                {
                    employeeIdValidOrNot = false;
                }

               
                string employeeIdDigitPart = employeeId.Trim().Substring(3);
                int digitCheck = Convert.ToInt32(employeeIdDigitPart);

                if (employeeIdDigitPart.Length > 5)
                {
                    employeeIdValidOrNot = false;
                }
                else if (employeeIdDigitPart.Length < 5)
                {
                    employeeIdValidOrNot = false;
                }
                
            }
            catch (Exception ex)
            {
                employeeIdValidOrNot = false;
            }

            return employeeIdValidOrNot;
        }

        public static string ClientIPAddress()
        {
            String ipAddress = string.Empty;
            ipAddress = System.Web.HttpContext.Current.Request.UserHostAddress;

            return ipAddress;
        }

        private static int MonthNumberToEndDays(int monthNumber)
        {
            int endDays = 1;

            if (monthNumber == 1)
            {
                endDays = 31;
            }
            else if (monthNumber == 2)
            {
                endDays = 28;
            }
            else if (monthNumber == 3)
            {
                endDays = 31;
            }
            else if (monthNumber == 4)
            {
                endDays = 30;
            }
            else if (monthNumber == 5)
            {
                endDays = 31;
            }
            else if (monthNumber == 6)
            {
                endDays = 30;
            }
            else if (monthNumber == 7)
            {
                endDays = 31;
            }
            else if (monthNumber == 8)
            {
                endDays = 31;
            }
            else if (monthNumber == 9)
            {
                endDays = 30;
            }
            else if (monthNumber == 10)
            {
                endDays = 31;
            }
            else if (monthNumber == 11)
            {
                endDays = 30;
            }
            else if (monthNumber == 12)
            {
                endDays = 31;
            }
            return endDays;
        }

        private static string MonthDigitToName(int month)
        {
            string monthText = string.Empty;

            if (month == 1)
            {
                monthText = "January";
            }
            else if (month == 2)
            {
                monthText = "February";
            }
            else if (month == 3)
            {
                monthText = "March";
            }
            else if (month == 4)
            {
                monthText = "April";
            }
            else if (month == 5)
            {
                monthText = "May";
            }
            else if (month == 6)
            {
                monthText = "June";
            }
            else if (month == 7)
            {
                monthText = "July";
            }
            else if (month == 8)
            {
                monthText = "August";
            }
            else if (month == 9)
            {
                monthText = "September";
            }
            else if (month == 10)
            {
                monthText = "October";
            }
            else if (month == 11)
            {
                monthText = "November";
            }
            else if (month == 12)
            {
                monthText = "December";
            }

            return monthText;
        }
    }
}
