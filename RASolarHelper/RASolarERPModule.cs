using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarHelper
{
    public class RASolarERPModule
    {
        private string __ModuleTypeName = string.Empty;
        private string __ModuleTypeValue = string.Empty;


        public string ModuleTypeName
        {
            set { __ModuleTypeName = value; }
            get { return __ModuleTypeName; }
        }

        public string ModuleTypeValue
        {
            set { __ModuleTypeValue = value; }
            get { return __ModuleTypeValue; }
        }

        public List<RASolarERPModule> RSFERPModuleSelection()
        {
            List<RASolarERPModule> rsfModule = new List<RASolarERPModule>();
            RASolarERPModule objRSFModule;

            objRSFModule = new RASolarERPModule();
            objRSFModule.__ModuleTypeName = "--Select--";
            objRSFModule.__ModuleTypeValue = "0";
            rsfModule.Add(objRSFModule);

            objRSFModule = new RASolarERPModule();
            objRSFModule.__ModuleTypeName = "HO Sales";
            objRSFModule.__ModuleTypeValue = "RSFSUMMARYSales";
            rsfModule.Add(objRSFModule);

            objRSFModule = new RASolarERPModule();
            objRSFModule.__ModuleTypeName = "HO Inventory";
            objRSFModule.__ModuleTypeValue = "RSFSUMMARYInventory";
            rsfModule.Add(objRSFModule);

            objRSFModule = new RASolarERPModule();
            objRSFModule.__ModuleTypeName = "HO Accounts";
            objRSFModule.__ModuleTypeValue = "RSFSUMMARYAccount";
            rsfModule.Add(objRSFModule);

            objRSFModule = new RASolarERPModule();
            objRSFModule.__ModuleTypeName = "Zonal Office";
            objRSFModule.__ModuleTypeValue = "ZONESUMMARY";
            rsfModule.Add(objRSFModule);

            objRSFModule = new RASolarERPModule();
            objRSFModule.__ModuleTypeName = "Regional Office";
            objRSFModule.__ModuleTypeValue = "REGIONSUMMARY";
            rsfModule.Add(objRSFModule);

            objRSFModule = new RASolarERPModule();
            objRSFModule.__ModuleTypeName = "Unit Office";
            objRSFModule.__ModuleTypeValue = "INDIVIDUALUNIT";
            rsfModule.Add(objRSFModule);

            objRSFModule = new RASolarERPModule();
            objRSFModule.__ModuleTypeName = "Unit Office Audit";
            objRSFModule.__ModuleTypeValue = "UNITAUDITR";
            rsfModule.Add(objRSFModule);

            return rsfModule;
        }

        public List<RASolarERPModule> RSFERPReportSelection()
        {
            List<RASolarERPModule> rsfModule = new List<RASolarERPModule>();
            RASolarERPModule objRSFModule;

            objRSFModule = new RASolarERPModule();
            objRSFModule.__ModuleTypeName = "RSF Summary";
            objRSFModule.__ModuleTypeValue = "RSFSUMMARY";
            rsfModule.Add(objRSFModule);

            objRSFModule = new RASolarERPModule();
            objRSFModule.__ModuleTypeName = "Zone Summary";
            objRSFModule.__ModuleTypeValue = "ZONESUMMARY";
            rsfModule.Add(objRSFModule);

            objRSFModule = new RASolarERPModule();
            objRSFModule.__ModuleTypeName = "Region Summary";
            objRSFModule.__ModuleTypeValue = "REGIONSUMMARY";
            rsfModule.Add(objRSFModule);

            objRSFModule = new RASolarERPModule();
            objRSFModule.__ModuleTypeName = "Unit Summary";
            objRSFModule.__ModuleTypeValue = "INDIVIDUALUNIT";
            rsfModule.Add(objRSFModule);

            objRSFModule = new RASolarERPModule();
            objRSFModule.__ModuleTypeName = "HO/ZO";
            objRSFModule.__ModuleTypeValue = "INDIVIDUALUNIT1";
            rsfModule.Add(objRSFModule);

            objRSFModule = new RASolarERPModule();
            objRSFModule.__ModuleTypeName = "Inventory At Vendor";
            objRSFModule.__ModuleTypeValue = "INVTORYATVENDR";
            rsfModule.Add(objRSFModule);

            return rsfModule;
        }
    }
}
