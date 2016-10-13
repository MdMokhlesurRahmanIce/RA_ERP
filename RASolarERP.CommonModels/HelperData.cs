using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RASolarHelper;
using RASolarERP.Model;
//using RASolarERP.Web.Models;

namespace RASolarERP.Web.Models
{
    public class HelperData
    {
        public List<YearMonthFormat> ReadYearMonthFormat()
        {
            return new YearMonthFormat().MonthFormat();
        }

        public List<RASolarERPModule> GetRASolarERPModule()
        {
            return new RASolarERPModule().RSFERPModuleSelection();
        }

        public LoginHelper SearchLocation(string unitCode)
        {
            RASolarERPData erpDal = new RASolarERPData();

            LoginHelper objLoginHelper = new LoginHelper();

            //Common_UnitInfo objUnit = new Common_UnitInfo();
            //objUnit = erpDal.Unit(unitCode);

            LocationInfo objUnit = new LocationInfo();
            objUnit = erpDal.Location(unitCode);

            //Common_RegionInfo objRegion = new Common_RegionInfo();
            //objRegion = erpDal.Region(objUnit.Reg_Code);

            LocationInfo objRegion = new LocationInfo();
            objRegion = erpDal.Location(objUnit.ParentLocationCode);

            //Common_ZoneInfo objZone = new Common_ZoneInfo();
            //objZone = erpDal.Zone(objRegion.Zone_Code);

            LocationInfo objZone = new LocationInfo();
            objZone = erpDal.Location(objRegion.ParentLocationCode);

            objLoginHelper.TopMenu = "SiteMenu/UnitUserMenu";
            objLoginHelper.LocationTitle = "Location Type: ";
            objLoginHelper.ZoneTitle = "Zone: ";
            objLoginHelper.RegionTitle = "Region: ";
            objLoginHelper.UnitTitle = "Unit: ";

            objLoginHelper.LogInForZoneCode = objZone.LocationCode;
            objLoginHelper.LogInForZoneName = objZone.LocationName;
            objLoginHelper.LogInForRegionCode = objRegion.LocationCode;
            objLoginHelper.LogInForRegionName = objRegion.LocationName;
            objLoginHelper.LogInForUnitCode = objUnit.LocationCode;
            objLoginHelper.LogInForUnitName = objUnit.LocationName + " [" + objUnit.LocationCode + "]";
            objLoginHelper.Location = Helper.Unit;

            return objLoginHelper;
        }
    }
}