using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarERP.DomainModel.InventoryModel
{
    public class InventoryInTransitBalance
    {
        public decimal TransitBalanceNEW { get; set; }
        public decimal TransitBalanceOLD { get; set; }
        public decimal TransitBalanceNEWRD { get; set; }
        public decimal TransitBalanceOLDRD { get; set; }
        public decimal TotalTransitBalance { get; set; }
    }
}
