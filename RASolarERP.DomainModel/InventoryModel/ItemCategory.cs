using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.InventoryModel
{
    public class ItemCategory
    {
        public string ItemCategoryID { get; set; }
        public string ItemCategoryName { get; set; }
        public string ItemType { get; set; }
        public string UnitOfMeasure { get; set; }
        public bool IsItASerializableItem { get; set; }
        public bool IsItemCapacityAvailable { get; set; }
        public bool IsItemModelAvailable { get; set; }
        public bool IsItemCodeWiseValidationExist { get; set; }

    }
}
