using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.InventoryModel
{
   public class SHSDistributionPlanPackageORItem
    {

        public string DistribScheduleNo { get; set; }
        public string RouteNo { get; set; }
        public string RouteName { get; set; }
        public string RouteCategory { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public string Category { get; set; }
        public string PackageOrItemCode { get; set; }
        public string PackageName { get; set; }
        public string PanelModel { get; set; }
        public string PanelModelName { get; set; }
        public string BatteryModel { get; set; }
        public string BatteryModelName { get; set; }

        public double PackageOrItemQuantity { get; set; }
     }
}
