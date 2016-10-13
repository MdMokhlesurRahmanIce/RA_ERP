using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarERP.DomainModel.SalesModel
{
    public class SalesItemDetails
    {
        public string ItemCategory { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemCapacity { get; set; }
        public string ItemCapacityName { get; set; }
        public string ItemModel { get; set; }
        public string ItemModelName { get; set; }
        public byte FromStoreLocation { get; set; }
        public double? ItemQuantity { get; set; }
    }
}
