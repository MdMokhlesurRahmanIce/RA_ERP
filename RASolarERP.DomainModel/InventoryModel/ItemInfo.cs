using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.InventoryModel
{
    public class ItemInfo
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public decimal AverageUnitCost { get; set; }
    }
}
