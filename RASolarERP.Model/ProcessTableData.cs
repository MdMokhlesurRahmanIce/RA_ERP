using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarERP.Model
{
    public static class ProcessTableData
    {
        public static List<Common_ZoneInfo> ZoneProcess(List<Common_ZoneInfo> zone)
        {
            Common_ZoneInfo objZone;
            List<Common_ZoneInfo> zoneFinal = new List<Common_ZoneInfo>();

            var zz = from zo in zone
                     select new
                     {
                         zo.Zone_Code,
                         Zone_Name = ("[" + zo.Zone_Code + "] " + zo.Zone_Name),
                         Created = zo.Created != null ? zo.Created : DateTime.Now,
                         Modified = zo.Modified != null ? zo.Modified : DateTime.Now,
                         zo.Status
                     };

            foreach (object z in zz)
            {
                objZone = new Common_ZoneInfo();
                Type typeB = z.GetType();

                objZone.Zone_Code = (typeB.GetProperty("Zone_Code").GetValue(z, null)).ToString();
                objZone.Zone_Name = (typeB.GetProperty("Zone_Name").GetValue(z, null)).ToString();
                objZone.Created = Convert.ToDateTime(typeB.GetProperty("Created").GetValue(z, null));
                objZone.Modified = Convert.ToDateTime(typeB.GetProperty("Modified").GetValue(z, null));
                objZone.Status = Convert.ToByte(typeB.GetProperty("Status").GetValue(z, null));

                zoneFinal.Add(objZone);
            }

            return zoneFinal;
        }

        public static List<Common_RegionInfo> RegionProcess(List<Common_RegionInfo> region)
        {
            Common_RegionInfo objRegion;
            List<Common_RegionInfo> regionFinal = new List<Common_RegionInfo>();

            var rr = from ro in region
                     select new
                     {
                         ro.Reg_Code,
                         //ro.Reg_Code1,
                         Reg_Name = ("[" + ro.Reg_Code + "] " + ro.Reg_Name),
                         ro.Zone_Code,
                         Created = ro.Created != null ? ro.Created : DateTime.Now,
                         Modified = ro.Modified != null ? ro.Modified : DateTime.Now,
                     };

            foreach (object r in rr)
            {
                objRegion = new Common_RegionInfo();
                Type typeB = r.GetType();

                objRegion.Reg_Code = (typeB.GetProperty("Reg_Code").GetValue(r, null)).ToString();
                //objRegion.Reg_Code1 = (typeB.GetProperty("Reg_Code1").GetValue(r, null)).ToString();
                objRegion.Reg_Name = (typeB.GetProperty("Reg_Name").GetValue(r, null)).ToString();
                objRegion.Zone_Code = (typeB.GetProperty("Zone_Code").GetValue(r, null)).ToString();
                objRegion.Created = Convert.ToDateTime(typeB.GetProperty("Created").GetValue(r, null));
                objRegion.Modified = Convert.ToDateTime(typeB.GetProperty("Modified").GetValue(r, null));

                regionFinal.Add(objRegion);
            }

            return regionFinal;
        }

        public static List<Common_UnitInfo> UnitProcess(List<Common_UnitInfo> unit)
        { 
            Common_UnitInfo objUnit;
            List<Common_UnitInfo> unitFinal = new List<Common_UnitInfo>();

            var uu = from un in unit
                     select new
                     {
                         un.Unit_Code,
                         Unit_Name = ("[" + un.Unit_Code + "] " + un.Unit_Name),
                         un.Reg_Code,
                         CREATED = un.CREATED != null ? un.CREATED : DateTime.Now,
                         MODIFIED = un.MODIFIED != null ? un.MODIFIED : DateTime.Now,
                         CUSTCODE_PREFIX = un.CUSTCODE_PREFIX != null ? un.CUSTCODE_PREFIX : "",
                         IDCOL_BranchID = un.IDCOL_BranchID != null ? un.IDCOL_BranchID : 0,
                         IDCOL_BranchName = un.IDCOL_BranchName != null ? un.IDCOL_BranchName : "",
                         un.Status
                         //Emp_Bank = un.Emp_Bank != null ? un.Emp_Bank : "",
                         //Emp_Branch = un.Emp_Branch != null ? un.Emp_Branch : "",
                         //Emp_Acc_No = un.Emp_Acc_No != null ? un.Emp_Acc_No : ""

                     };

            foreach (object u in uu)
            {
                objUnit = new Common_UnitInfo();
                Type typeB = u.GetType();

                objUnit.Unit_Code = (typeB.GetProperty("Unit_Code").GetValue(u, null)).ToString();
                objUnit.Unit_Name = (typeB.GetProperty("Unit_Name").GetValue(u, null)).ToString();
                objUnit.Reg_Code = (typeB.GetProperty("Reg_Code").GetValue(u, null)).ToString();
                objUnit.CREATED = Convert.ToDateTime(typeB.GetProperty("CREATED").GetValue(u, null));
                objUnit.MODIFIED = Convert.ToDateTime(typeB.GetProperty("MODIFIED").GetValue(u, null));
                objUnit.CUSTCODE_PREFIX = (typeB.GetProperty("CUSTCODE_PREFIX").GetValue(u, null)).ToString();
                objUnit.IDCOL_BranchID = Convert.ToDecimal(typeB.GetProperty("IDCOL_BranchID").GetValue(u, null));
                objUnit.IDCOL_BranchName = (typeB.GetProperty("IDCOL_BranchName").GetValue(u, null)).ToString();
                objUnit.Status = Convert.ToByte(typeB.GetProperty("Status").GetValue(u, null));
                //objUnit.Emp_Bank = (typeB.GetProperty("Emp_Bank").GetValue(u, null)).ToString();
                //objUnit.Emp_Branch = (typeB.GetProperty("Emp_Branch").GetValue(u, null)).ToString();
                //objUnit.Emp_Acc_No = (typeB.GetProperty("Emp_Acc_No").GetValue(u, null)).ToString();

                unitFinal.Add(objUnit);
            }

            return unitFinal;

        }

    }
}
