using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.InventoryModel
{
    public class PackageInformation
    {
        public string PackageCode { get; set; }
        public string PackageDescription { get; set; }
        public decimal PerUnitSalesPrice { get; set; }
    }
}
