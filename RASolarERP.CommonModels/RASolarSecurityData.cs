using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using RASolarERP.Model;
using RASolarSecurity.Model;
using RASolarHelper;
using System.Collections;

namespace RASolarERP.Web.Models
{
    public class RASolarSecurityData
    {
        private RASolarSecurityService securityService = new RASolarSecurityService();
        private RASolarERPData erpService = new RASolarERPData();

        string URL = string.Empty, reportType = string.Empty, URLSelectionLocation = string.Empty, TopNav = string.Empty;
        string HomeURL = string.Empty, customerPrefix = string.Empty, modluleTitle = string.Empty;
        int officeModule = 0;
        byte isInventoryImplemented = 0;

        public UserInformation ReadUserInformation(string userId, string userPassword, byte userStatus)
        {
            return securityService.GetUserInformation(userId, userPassword, userStatus);
        }

        public LoginHelper LoginInformation(FormCollection fCollection, UserInformation objUserInfo)
        {
            try
            {
                LoginHelper userLoginInfo = new LoginHelper();

                userLoginInfo.LogInID = fCollection["txtUserName"].ToString().ToLower();

                userLoginInfo.LogInUserName = objUserInfo.UserName != null ? objUserInfo.UserName : "";

                if (userLoginInfo.LogInUserName != null)
                {
                    if (Helper.NumberOfCharacter(userLoginInfo.LogInUserName) > 20)
                    {
                        userLoginInfo.LogInUserName = objUserInfo.UserName.Substring(0, 19);
                    }
                }

                userLoginInfo.LogInPassword = fCollection["txtPassword"].ToString().ToLower();

                userLoginInfo.UerRoleOrGroupID = objUserInfo.UserRoleOrGroupID;

                userLoginInfo.YearMonthCurrent = Helper.YearMonthCurrent();
                userLoginInfo.AuditorYearMonth = Helper.YearMonthPrevious(Helper.OneMonthPrevious);

                ArrayList userLocationList = new ArrayList();
                userLocationList = RoleWiseLocation(objUserInfo);

                userLoginInfo.LogInForZoneCode = userLocationList[0].ToString();
                userLoginInfo.LogInForZoneName = userLocationList[1].ToString();
                userLoginInfo.LogInForRegionCode = userLocationList[2].ToString();
                userLoginInfo.LogInForRegionName = userLocationList[3].ToString();
                userLoginInfo.LogInForUnitCode = userLocationList[4].ToString();
                userLoginInfo.LogInForUnitName = userLocationList[5].ToString();
                userLoginInfo.Location = userLocationList[6].ToString();

                userLoginInfo.CustomerPrefix = customerPrefix;

                userLoginInfo.HomeLocation = objUserInfo.OnlyForLocation;
                userLoginInfo.HomeURL = HomeURL;

                userLoginInfo.LocationTitle = userLocationList[7].ToString();
                userLoginInfo.ZoneTitle = userLocationList[8].ToString();
                userLoginInfo.RegionTitle = userLocationList[9].ToString();
                userLoginInfo.UnitTitle = userLocationList[10].ToString();

                userLoginInfo.ReportType = reportType;
                userLoginInfo.URL = URL;
                userLoginInfo.URLSelectionLocation = URLSelectionLocation;
                userLoginInfo.TopMenu = TopNav;

                userLoginInfo.LocationCode = Helper.LocationCode(userLoginInfo);
                userLoginInfo.LocationName = Helper.LocationName(userLoginInfo);

                Common_PeriodOpenClose objPeriodOpenClose = new Common_PeriodOpenClose();
                objPeriodOpenClose = erpService.ReadPeriodOpenClose(userLoginInfo.LocationCode);

                if (objPeriodOpenClose != null)
                {
                    userLoginInfo.TransactionOpenDate = objPeriodOpenClose.CalenderDate;
                }
                else
                {
                    userLoginInfo.TransactionOpenDate = new DateTime(1900, 1, 1);
                }

                LocationInfo objLocationInfo = new LocationInfo();
                objLocationInfo = erpService.Location(userLoginInfo.LocationCode);

                if (objLocationInfo != null)
                {
                    if (objLocationInfo.BackDayAllowedForTransaction != null)
                    {
                        if (objLocationInfo.BackDayAllowedForTransaction != 0)
                        {
                            DateTime transactionBackDate = objPeriodOpenClose.CalenderDate.AddDays((Convert.ToDouble(objLocationInfo.BackDayAllowedForTransaction) * -1)).Date;
                            userLoginInfo.TransactionBackDate = transactionBackDate;
                        }
                        else
                        {
                            userLoginInfo.TransactionBackDate = objPeriodOpenClose != null ? objPeriodOpenClose.CalenderDate : new DateTime(1900, 1, 1);
                        }
                    }
                    else
                    {
                        userLoginInfo.TransactionBackDate = objPeriodOpenClose != null ? objPeriodOpenClose.CalenderDate : new DateTime(1900, 1, 1);
                    }
                }
                else
                {
                    userLoginInfo.TransactionBackDate = objPeriodOpenClose != null ? objPeriodOpenClose.CalenderDate : new DateTime(1900, 1, 1);
                }

                Common_CurrentYearMonthNWeek objCurrenOpenYearMonthNWeek = erpService.ReadCurrentYearMonthNWeek();

                userLoginInfo.MonthOpenForSales = objCurrenOpenYearMonthNWeek.MonthOpenForSales;
                userLoginInfo.MonthOpenForInventory = objCurrenOpenYearMonthNWeek.MonthOpenForInventory;
                userLoginInfo.MonthOpenForAccounting = objCurrenOpenYearMonthNWeek.MonthOpenForAccounting;
                userLoginInfo.MonthOpenForHRMS = objCurrenOpenYearMonthNWeek.MonthOpenForHRMS;
                userLoginInfo.WeekOpenForWeeklyOverdueCollection = objCurrenOpenYearMonthNWeek.WeekOpenForWeeklyOverdueCollection;
                userLoginInfo.MonthOpenForDailyProgressReview = objCurrenOpenYearMonthNWeek.MonthOpenForDailyProgressReview;

                DateTime openDateForWeek = Helper.YearWeekToStartDate(objCurrenOpenYearMonthNWeek.WeekOpenForWeeklyOverdueCollection);
                userLoginInfo.OpenYearWeek = Helper.YearWeek(openDateForWeek);
                userLoginInfo.OpenWeekNumber = Helper.CurrentWeekOfYear(openDateForWeek);
                userLoginInfo.FirstDateOfOpenWeek = Helper.FirstDateOfCurrentWeek(openDateForWeek);
                userLoginInfo.LastDateOfOpenWeek = Helper.LastDateOfCurrentWeek(openDateForWeek);

                DateTime currentDate = DateTime.Now.Date;
                userLoginInfo.CurrentYearWeek = Helper.YearWeek(currentDate);
                userLoginInfo.CurrentWeekNumber = Helper.CurrentWeekOfYear(currentDate);
                userLoginInfo.FirstDateOfCurrentWeek = Helper.FirstDateOfCurrentWeek(currentDate);
                userLoginInfo.LastDateOfCurrentWeek = Helper.LastDateOfCurrentWeek(currentDate);

                userLoginInfo.ModluleTitle = modluleTitle;
                userLoginInfo.OfficeModule = officeModule;

                userLoginInfo.IsAuthenticApproverForThisLocation = objUserInfo.IsAuthenticApproverForThisLocation != null ? Convert.ToBoolean(objUserInfo.IsAuthenticApproverForThisLocation) : false;
                userLoginInfo.IsInventoryImplemented = isInventoryImplemented;

                return userLoginInfo;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ArrayList RoleWiseLocation(UserInformation objUserInfo)
        {
            try
            {

                string zoneName = string.Empty, regionName = string.Empty, unitName = string.Empty, zoneCode = string.Empty, regionCode = string.Empty, unitCode = string.Empty;
                string location = string.Empty, locationTitle = string.Empty, zoneTitle = string.Empty, regionTitle = string.Empty, unitTitle = string.Empty;
                string root = ""; // root = "../"

                ArrayList userLocation = new ArrayList();

                if (objUserInfo.UserRoleOrGroupID == UserGroup.Administrator)
                {
                    if (objUserInfo.IsLocationDependent == false)
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = Helper.HeadOffice;
                    }
                    else
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = Helper.HeadOffice;
                    }

                    officeModule = Helper.HeadOfficeModule;
                }
                else if (objUserInfo.UserRoleOrGroupID == UserGroup.HeadOfficeAudit)
                {
                    if (objUserInfo.IsLocationDependent == false)
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = Helper.HeadOffice;
                    }
                    else
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = Helper.HeadOffice;
                    }

                    URL = root + "HeadOffice/Auditor";
                    URLSelectionLocation = root + "RSFSecurity/LocationSelection";

                    locationTitle = "Location Type: ";
                    zoneTitle = "Location: ";
                    regionTitle = "";
                    unitTitle = "";
                    officeModule = Helper.HeadOfficeModule;
                }
                else if (objUserInfo.UserRoleOrGroupID == UserGroup.HeadOfficeFinanceAccount)
                {
                    if (objUserInfo.IsLocationDependent == false)
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = Helper.HeadOffice;
                    }
                    else
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = Helper.HeadOffice;
                    }

                    URL = root + "HeadOffice/HOAccountFinance";
                    reportType = Helper.HOAccounts;
                    TopNav = "SiteMenu/AccountFinanceMenuHO";
                    HomeURL = "HeadOffice/HOAccountFinance";

                    locationTitle = "Location Type: ";
                    zoneTitle = "";
                    regionTitle = "";
                    unitTitle = "";
                    modluleTitle = "HO Accounting";
                    officeModule = Helper.HeadOfficeModule;
                }
                else if (objUserInfo.UserRoleOrGroupID == UserGroup.HeadOfficeInventory)
                {
                    if (objUserInfo.IsLocationDependent == false)
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = Helper.HeadOffice;
                    }
                    else
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = Helper.HeadOffice;
                    }

                    URL = root + "HeadOffice/HOInventory";
                    reportType = Helper.HOInventory;
                    TopNav = "SiteMenu/InventoryMenuHO";
                    HomeURL = "HeadOffice/HOInventory";

                    locationTitle = "Location Type: ";
                    zoneTitle = "";
                    regionTitle = "";
                    unitTitle = "";
                    modluleTitle = "HO Inventory";
                    officeModule = Helper.HeadOfficeModule;
                }
                else if (objUserInfo.UserRoleOrGroupID == UserGroup.HeadOfficeSales)
                {
                    if (objUserInfo.IsLocationDependent == false)
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = Helper.HeadOffice;
                    }
                    else
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = Helper.HeadOffice;
                    }

                    URL = root + "HeadOffice/HOSales";
                    reportType = Helper.HOSales;
                    TopNav = "SiteMenu/SalesMenuHO";
                    HomeURL = "HeadOffice/HOSales";

                    locationTitle = "Location Type: ";
                    zoneTitle = "";
                    regionTitle = "";
                    unitTitle = "";
                    modluleTitle = "HO Sales";
                    officeModule = Helper.HeadOfficeModule;
                }
                else if (objUserInfo.UserRoleOrGroupID == UserGroup.HeadOfficeHRNAdmin)
                {
                    if (objUserInfo.IsLocationDependent == false)
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = Helper.HeadOffice;
                    }
                    else
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = Helper.HeadOffice;
                    }

                    URL = root + "HeadOffice/HOHumanResourceAdmin";
                    reportType = Helper.HOSales;
                    TopNav = "SiteMenu/HRAdminMenuHO";
                    HomeURL = "HeadOffice/HOHumanResourceAdmin";

                    locationTitle = "Location Type: ";
                    zoneTitle = "";
                    regionTitle = "";
                    unitTitle = "";
                    modluleTitle = "HO HR Admin";
                    officeModule = Helper.HeadOfficeModule;
                }
                else if (objUserInfo.UserRoleOrGroupID == UserGroup.HeadOfficeIT)
                {
                    if (objUserInfo.IsLocationDependent == false)
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = Helper.HeadOffice;
                    }
                    else
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = Helper.HeadOffice;
                    }

                    URL = root + "HeadOffice/HOInventory";
                    officeModule = Helper.HeadOfficeModule;
                }
                else if (objUserInfo.UserRoleOrGroupID == UserGroup.ZonalManager || objUserInfo.UserRoleOrGroupID == UserGroup.ZonalOperationUser)
                {
                    if (objUserInfo.IsLocationDependent == false)
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = Helper.Zone;
                    }
                    else
                    {
                        //zoneCode = objUserInfo.OnlyForLocation;
                        //zoneName = erpService.Zone(objUserInfo.OnlyForLocation).Zone_Name + " [" + objUserInfo.OnlyForLocation + "]";                   

                        zoneCode = objUserInfo.OnlyForLocation;
                        zoneName = erpService.Location(objUserInfo.OnlyForLocation).LocationName + " [" + objUserInfo.OnlyForLocation + "]";

                        regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = Helper.Zone;
                    }

                    URL = root + "ZonalOffice/ZoneManager";
                    reportType = Helper.ZonalOffice;
                    TopNav = "SiteMenu/ZoneManagerMenu";
                    HomeURL = "ZonalOffice/ZoneManager";

                    locationTitle = "Location Type: ";
                    zoneTitle = "Zone : ";
                    regionTitle = "";
                    unitTitle = "";

                    modluleTitle = "Zonal Module";
                    officeModule = Helper.ZonalOfficeModule;
                }
                else if (objUserInfo.UserRoleOrGroupID == UserGroup.ZonalITUser)
                {
                    if (objUserInfo.IsLocationDependent == false)
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = Helper.Zone;
                    }
                    else
                    {
                        //zoneName = erpService.Zone(objUserInfo.OnlyForLocation).Zone_Name;

                        zoneCode = objUserInfo.OnlyForLocation;
                        zoneName = erpService.Location(objUserInfo.OnlyForLocation).LocationName;

                        regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = Helper.Zone;
                    }

                    URL = root + "ZonalOffice/ZoneManager";
                    reportType = Helper.ZonalOffice;
                    TopNav = "SiteMenu/ZoneManagerMenu";
                    officeModule = Helper.ZonalOfficeModule;
                }
                else if (objUserInfo.UserRoleOrGroupID == UserGroup.RegionManager)
                {
                    if (objUserInfo.IsLocationDependent == false)
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = Helper.Region;
                    }
                    else
                    {
                        //Common_RegionInfo objRegion = new Common_RegionInfo();
                        //objRegion = erpService.Region(objUserInfo.OnlyForLocation);

                        LocationInfo objRegion = new LocationInfo();
                        objRegion = erpService.Location(objUserInfo.OnlyForLocation);

                        regionCode = objUserInfo.OnlyForLocation;
                        regionName = objRegion.LocationName + " [" + objUserInfo.OnlyForLocation + "]";

                        LocationInfo objZone = new LocationInfo();
                        objZone = erpService.Location(objRegion.ParentLocationCode);

                        zoneCode = objZone.LocationCode;
                        zoneName = objZone.LocationName;

                        unitCode = ""; unitName = ""; location = Helper.Region;
                    }

                    URL = root + "RegionalOffice/RegionManager";
                    reportType = Helper.RegionalOffice;
                    TopNav = "SiteMenu/RegionManagerMenu";
                    HomeURL = "RegionalOffice/RegionManager";

                    locationTitle = "Location Type: ";
                    zoneTitle = "Zone: ";
                    regionTitle = "Region: ";
                    unitTitle = "";

                    modluleTitle = "Regional Module";
                    officeModule = Helper.RegionOfficeModule;
                }
                else if (objUserInfo.UserRoleOrGroupID == UserGroup.UnitManager)
                {
                    if (objUserInfo.IsLocationDependent == false)
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = Helper.Unit;
                    }
                    else
                    {

                        LocationInfo objUnit = new LocationInfo();
                        objUnit = erpService.Location(objUserInfo.OnlyForLocation);

                        unitCode = objUnit.LocationCode;
                        unitName = objUnit.LocationName;

                        LocationInfo objRegion = new LocationInfo();
                        objRegion = erpService.Location(objUnit.ParentLocationCode);

                        regionCode = objRegion.LocationCode;
                        regionName = objRegion.LocationName;

                        LocationInfo objZone = new LocationInfo();
                        objZone = erpService.Location(objRegion.ParentLocationCode);

                        zoneCode = objRegion.LocationCode;
                        zoneName = objZone.LocationName + " [" + objZone.LocationCode + "]";

                        location = Helper.Unit;
                    }

                    URL = root + "HeadOffice/HOInventory";
                    reportType = Helper.UnitOffice;

                    modluleTitle = "Unit Module";
                    officeModule = Helper.UnitOfficeModule;
                }
                else if (objUserInfo.UserRoleOrGroupID == UserGroup.UnitAuditor)
                {
                    if (objUserInfo.IsLocationDependent == false)
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = "Auditor";
                    }
                    else
                    {
                        LocationInfo objUnit = new LocationInfo();
                        objUnit = erpService.Location(objUserInfo.OnlyForLocation);

                        unitCode = objUnit.LocationCode;
                        unitName = objUnit.LocationName + " [" + objUnit.LocationCode + "]";

                        LocationInfo objRegion = new LocationInfo();
                        objRegion = erpService.Location(objUnit.ParentLocationCode);

                        regionCode = objRegion.LocationCode;
                        regionName = objRegion.LocationName;

                        LocationInfo objZone = new LocationInfo();
                        objZone = erpService.Location(objRegion.ParentLocationCode);

                        zoneCode = objZone.LocationCode;
                        zoneName = objZone.LocationName;

                        location = "Auditor";
                    }

                    URL = root + "HeadOffice/Auditor";
                    URLSelectionLocation = root + "RSFSecurity/LocationSelection";
                    TopNav = "SiteMenu/UnitAuditorMenu";
                    HomeURL = "HeadOffice/Auditor";

                    locationTitle = "Type: ";
                    zoneTitle = "Zone: ";
                    regionTitle = "Region: ";
                    unitTitle = "Unit: ";

                    modluleTitle = "Unit Auditor Module";

                    reportType = Helper.UnitAudit;
                    officeModule = Helper.UnitOfficeModule;
                }
                else if (objUserInfo.UserRoleOrGroupID == UserGroup.UnitOfficeUser)
                {
                    if (objUserInfo.IsLocationDependent == false)
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = Helper.Unit;
                    }
                    else
                    {

                        LocationInfo objUnit = new LocationInfo();
                        objUnit = erpService.Location(objUserInfo.OnlyForLocation);

                        //Common_UnitInfo objUnit = new Common_UnitInfo();  // Need to change using upper comment
                        //objUnit = erpService.Unit(objUserInfo.OnlyForLocation);

                        LocationInfo objRegion = new LocationInfo();
                        objRegion = erpService.Location(objUnit.ParentLocationCode);

                        LocationInfo objZone = new LocationInfo();
                        objZone = erpService.Location(objRegion.ParentLocationCode);

                        //Common_RegionInfo objRegion = new Common_RegionInfo();
                        //objRegion = erpService.Region(objUnit.Reg_Code);

                        //Common_ZoneInfo objZone = new Common_ZoneInfo();
                        //objZone = erpService.Zone(objRegion.Zone_Code);

                        zoneCode = objZone.LocationCode;
                        zoneName = objZone.LocationName;

                        regionCode = objRegion.LocationCode;
                        regionName = objRegion.LocationName;

                        unitCode = objUnit.LocationCode;
                        unitName = objUnit.LocationName + " [" + objUserInfo.OnlyForLocation + "]";

                        customerPrefix = objUnit.CustomerCodePrefix;

                        location = Helper.Unit;

                        modluleTitle = "Unit Module";
                        officeModule = Helper.UnitOfficeModule;

                        isInventoryImplemented = 1;//objUnit.IsInventoryImplemented != null ? Convert.ToByte(objUnit.IsInventoryImplemented) : Convert.ToByte(0);
                    }

                    URL = root + "UnitOfficeUser/UnitOfficeUsers";
                    reportType = Helper.UnitOffice;
                    TopNav = "SiteMenu/UnitUserMenu";
                    HomeURL = "UnitOfficeUser/UnitOfficeUsers";

                    locationTitle = "Location Type: ";
                    zoneTitle = "Zone: ";
                    regionTitle = "Region: ";
                    unitTitle = "Unit: ";
                }
                else if (objUserInfo.UserRoleOrGroupID == UserGroup.RSFExecutiveCommittee)
                {
                    if (objUserInfo.IsLocationDependent == false)
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = "";
                    }
                    else
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = "";
                    }

                    URL = root + "HeadOffice/RSFExecutiveCommittee";

                    reportType = Helper.RacoOffice;
                    TopNav = "SiteMenu/RSFExecutiveCommitteeMenu";
                    HomeURL = "HeadOffice/RSFExecutiveCommittee";

                    modluleTitle = "RSF ECM Module";
                    officeModule = Helper.HeadOfficeModule;
                }
                else if (objUserInfo.UserRoleOrGroupID == UserGroup.RacoReviewer)
                {
                    if (objUserInfo.IsLocationDependent == false)
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = "";
                    }
                    else
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = "";
                    }

                    URL = root + "RACO/Reviewer";

                    reportType = Helper.RacoOffice;
                    TopNav = "SiteMenu/RACOReviewInventoryMenu";
                    HomeURL = "RACO/Reviewer";

                    modluleTitle = "RACO Module";
                    officeModule = Helper.HeadOfficeModule;
                }
                else if (objUserInfo.UserRoleOrGroupID == UserGroup.RREReviewer)
                {
                    if (objUserInfo.IsLocationDependent == false)
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = "";
                    }
                    else
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = "";
                    }

                    URL = root + "RRE/RREReview";

                    reportType = Helper.RacoOffice;
                    TopNav = "SiteMenu/RREReviewMenu";
                    HomeURL = "RRE/RREReview";

                    modluleTitle = "RRE Module";
                    officeModule = Helper.HeadOfficeModule;
                }
                else if (objUserInfo.UserRoleOrGroupID == UserGroup.ITReviewer)
                {
                    if (objUserInfo.IsLocationDependent == false)
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = "";
                    }
                    else
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = "";
                    }

                    URL = root + "IT/ITReview";

                    reportType = Helper.RacoOffice;
                    TopNav = "SiteMenu/ITReviewMenu";
                    HomeURL = "IT/ITReview";

                    modluleTitle = "IT Reviewer Menu";
                    officeModule = Helper.HeadOfficeModule;
                }

                else if (objUserInfo.UserRoleOrGroupID == UserGroup.HeadOfficeAccountAdvanceUsers)
                {
                    if (objUserInfo.IsLocationDependent == false)
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = Helper.HeadOffice;
                    }
                    else
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = Helper.HeadOffice;
                    }

                    URL = root + "HeadOffice/HOACCAdvanceUsers";
                    reportType = Helper.HOAccounts;
                    TopNav = "SiteMenu/AccountAdvenceUserMenuHO";
                    HomeURL = "HeadOffice/HOACCAdvanceUsers";

                    locationTitle = "Location Type: ";
                    zoneTitle = "";
                    regionTitle = "";
                    unitTitle = "";
                    modluleTitle = "Accounting Advance User";
                    officeModule = Helper.HeadOfficeModule;
                }
                else if (objUserInfo.UserRoleOrGroupID == UserGroup.HeadOfficeAccountEntryUsers)
                {
                    if (objUserInfo.IsLocationDependent == false)
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = Helper.HeadOffice;
                    }
                    else
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = Helper.HeadOffice;
                    }

                    URL = root + "HeadOffice/HOACCEntryUsers";
                    reportType = Helper.HOAccounts;
                    TopNav = "SiteMenu/AccountEntryUserMenuHO";
                    HomeURL = "HeadOffice/HOACCEntryUsers";

                    locationTitle = "Location Type: ";
                    zoneTitle = "";
                    regionTitle = "";
                    unitTitle = "";
                    modluleTitle = "Accounting Entry Users";
                    officeModule = Helper.HeadOfficeModule;
                }
                else if (objUserInfo.UserRoleOrGroupID == UserGroup.InventroyAdvanceUsers)
                {
                    if (objUserInfo.IsLocationDependent == false)
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = Helper.HeadOffice;
                    }
                    else
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = Helper.HeadOffice;
                    }

                    URL = root + "HeadOffice/INVAdvanceUsers";
                    reportType = Helper.HOAccounts;
                    TopNav = "SiteMenu/InventroyAdvanceUsersMenuHO";
                    HomeURL = "HeadOffice/INVAdvanceUsers";

                    locationTitle = "Location Type: ";
                    zoneTitle = "";
                    regionTitle = "";
                    unitTitle = "";
                    modluleTitle = "Inventory Advance Users";
                    officeModule = Helper.HeadOfficeModule;
                }
                else if (objUserInfo.UserRoleOrGroupID == UserGroup.InventroyAccountEntryUsers)
                {
                    if (objUserInfo.IsLocationDependent == false)
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = Helper.HeadOffice;
                    }
                    else
                    {
                        zoneCode = ""; zoneName = ""; regionCode = ""; regionName = "";
                        unitCode = ""; unitName = ""; location = Helper.HeadOffice;
                    }

                    URL = root + "HeadOffice/INVEntryUsers";
                    reportType = Helper.HOAccounts;
                    TopNav = "SiteMenu/InventroyEntryUsersMenuHO";
                    HomeURL = "HeadOffice/INVEntryUsers";

                    locationTitle = "Location Type: ";
                    zoneTitle = "";
                    regionTitle = "";
                    unitTitle = "";
                    modluleTitle = "Inventory Entry Users";
                    officeModule = Helper.HeadOfficeModule;
                }
                userLocation.Add(zoneCode);
                userLocation.Add(zoneName);
                userLocation.Add(regionCode);
                userLocation.Add(regionName);
                userLocation.Add(unitCode);
                userLocation.Add(unitName);
                userLocation.Add(location);

                userLocation.Add(locationTitle);
                userLocation.Add(zoneTitle);
                userLocation.Add(regionTitle);
                userLocation.Add(unitTitle);

                return userLocation;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public LoginHelper LoginInformation(LoginHelper objLoginHelper)
        {
            try
            {
                Common_PeriodOpenClose objPeriodOpenClose = new Common_PeriodOpenClose();
                objPeriodOpenClose = erpService.ReadPeriodOpenClose(objLoginHelper.LocationCode);

                if (objPeriodOpenClose != null)
                {
                    objLoginHelper.TransactionOpenDate = objPeriodOpenClose.CalenderDate;
                }
                else
                {
                    objLoginHelper.TransactionOpenDate = new DateTime(1900, 1, 1);
                }

                LocationInfo objLocationInfo = new LocationInfo();
                objLocationInfo = erpService.Location(objLoginHelper.LocationCode);

                if (objLocationInfo != null)
                {
                    if (objLocationInfo.BackDayAllowedForTransaction != null)
                    {
                        if (objLocationInfo.BackDayAllowedForTransaction != 0)
                        {
                            DateTime transactionBackDate = objPeriodOpenClose.CalenderDate.AddDays((Convert.ToDouble(objLocationInfo.BackDayAllowedForTransaction) * -1)).Date;
                            objLoginHelper.TransactionBackDate = transactionBackDate;
                        }
                        else
                        {
                            objLoginHelper.TransactionBackDate = objPeriodOpenClose != null ? objPeriodOpenClose.CalenderDate : new DateTime(1900, 1, 1);
                        }
                    }
                    else
                    {
                        objLoginHelper.TransactionBackDate = objPeriodOpenClose != null ? objPeriodOpenClose.CalenderDate : new DateTime(1900, 1, 1);
                    }
                }
                else
                {
                    objLoginHelper.TransactionBackDate = objPeriodOpenClose != null ? objPeriodOpenClose.CalenderDate : new DateTime(1900, 1, 1);
                }

                Common_CurrentYearMonthNWeek objCurrenOpenYearMonthNWeek = erpService.ReadCurrentYearMonthNWeek();

                objLoginHelper.MonthOpenForSales = objCurrenOpenYearMonthNWeek.MonthOpenForSales;
                objLoginHelper.MonthOpenForInventory = objCurrenOpenYearMonthNWeek.MonthOpenForInventory;
                objLoginHelper.MonthOpenForAccounting = objCurrenOpenYearMonthNWeek.MonthOpenForAccounting;
                objLoginHelper.MonthOpenForHRMS = objCurrenOpenYearMonthNWeek.MonthOpenForHRMS;
                objLoginHelper.WeekOpenForWeeklyOverdueCollection = objCurrenOpenYearMonthNWeek.WeekOpenForWeeklyOverdueCollection;
                objLoginHelper.MonthOpenForDailyProgressReview = objCurrenOpenYearMonthNWeek.MonthOpenForDailyProgressReview;

                DateTime openDateForWeek = Helper.YearWeekToStartDate(objCurrenOpenYearMonthNWeek.WeekOpenForWeeklyOverdueCollection);
                objLoginHelper.OpenYearWeek = Helper.YearWeek(openDateForWeek);
                objLoginHelper.OpenWeekNumber = Helper.CurrentWeekOfYear(openDateForWeek);
                objLoginHelper.FirstDateOfOpenWeek = Helper.FirstDateOfCurrentWeek(openDateForWeek);
                objLoginHelper.LastDateOfOpenWeek = Helper.LastDateOfCurrentWeek(openDateForWeek);

                DateTime currentDate = DateTime.Now.Date;
                objLoginHelper.CurrentYearWeek = Helper.YearWeek(currentDate);
                objLoginHelper.CurrentWeekNumber = Helper.CurrentWeekOfYear(currentDate);
                objLoginHelper.FirstDateOfCurrentWeek = Helper.FirstDateOfCurrentWeek(currentDate);
                objLoginHelper.LastDateOfCurrentWeek = Helper.LastDateOfCurrentWeek(currentDate);

                objLoginHelper.IsAuthenticApproverForThisLocation = true; //objUserInfo.IsAuthenticApproverForThisLocation != null ? Convert.ToBoolean(objUserInfo.IsAuthenticApproverForThisLocation) : false;
                objLoginHelper.IsInventoryImplemented = 1;

                return objLoginHelper;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ListOfPagesWithAccessRightsForThisUser ReadListOfPagesWithAccessRightsForAnUser(string userID, string pageOption, string pageID)
        {
            return securityService.ReadListOfPagesWithAccessRightsForAnUser(userID, pageOption, pageID);
        }

        public List<ListOfPagesWithAccessRightsForThisUser> ReadListOfPagesWithAccessRightsForUser(string userID, string pageOption, string pageID)
        {
            return securityService.ReadListOfPagesWithAccessRightsForUser(userID, pageOption, pageID);
        }

        public PageAccessRightHelper ReadPageAccessRight(string moduleId, string pageNameToLink, string roleOrGroupID, byte accessRightStatus)
        {
            return securityService.ReadPageAccessRight(moduleId, pageNameToLink, roleOrGroupID, accessRightStatus);
        }

        /// <summary>
        /// Check if the page is accessible for the current user.
        /// </summary>
        /// <param name="moduleId">The page is for what module. ie. SAL for Sales, AUD for audit, INV for Inventory</param>
        /// <param name="pageNameToLink">WHich Action is responsible for this page. ie. Action method Name</param>
        /// <param name="roleOrGroupId">User Role/Group ID</param>
        /// <param name="accessRightStatus">Ture for Access & False For UnAccess</param>
        /// <param name="message">The Message you want to show. It is Default. If page is not accessible the a error message is throw</param>
        /// <returns>Ture for Access & False For UnAccess</returns>
        public bool IsPageAccessible(string moduleId, string pageNameToLink, string roleOrGroupId, byte accessRightStatus, out string message)
        {
            bool accessibleOrNot = true;
            message = string.Empty;

            PageAccessRightHelper objPageAccessRightHelper = new PageAccessRightHelper();
            objPageAccessRightHelper = securityService.ReadPageAccessRight(moduleId, pageNameToLink, roleOrGroupId, accessRightStatus);
            if (objPageAccessRightHelper != null)
            {
                if (objPageAccessRightHelper.AccessStatus == Helper.Inactive)
                {
                    accessibleOrNot = false;
                    message = objPageAccessRightHelper.MessageToShow;
                }
            }

            return accessibleOrNot;
        }
    }
}