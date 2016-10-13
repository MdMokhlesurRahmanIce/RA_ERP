using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.InventoryModel
{
    public class SHSDistributionPlan
    {
        public string RootNo { set; get; }
        public string LocationCode { set; get; }
        public string PackageCode { set; get; }
        public string PanelCode { set; get; }
        public string BatteryCode { set; get; }
        public string PackageOrItemSelection { set; get; }
        public float PackageQuantity { set; get; }
    }
}
