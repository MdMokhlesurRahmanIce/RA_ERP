using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.InventoryModel
{
    public class WarrantyItemsDetails
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemType { get; set; }       
        public string ItemCategory { get; set; }
        public string ItemCapacity { get; set; }
        public string ItemModel { get; set; }
        public double AvailableQuantity { get; set; }
        public byte ExchangeStoreLocation { get; set; }
        public string ItemSerialNo { get; set; }
        public DateTime? LastChallanDate { get; set; }
        public int TotalWarrantyPeriodInDays { get; set; }
        public int TotalWarrantyPeriodInYear { get; set; }
        public int TotalRemainingWarrantyPeriodInDays { get; set; }
        public int RemainingWarrantyPeriodInYear { get; set; }
        public int RemainingWarrantyPeriodInMonth { get; set; }
        public int RemainingWarrantyPeriodInDays { get; set; }
        public string RemainingWarrantyPeriod { get; set; }
        public string WarrantyStatusBasedOnPeriod { get; set; }
        public string ItemCategoryDesc { get; set; }
        public string ItemCapacityDesc { get; set; }
        public string ItemModelDesc { get; set; }
        public string ItemTypeDesc { get; set; }

        public DateTime? DeliveryDate { get; set; }
        public string WarrantyPeriod { get; set; }
         
    }
}
