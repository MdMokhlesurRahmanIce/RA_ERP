using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarHelper.InventoryHelperModel
{
    public class StockInTransitAtGlanceDetails
    {
        public string ChallanFromLocationName { get; set; }
        public string ChallanFromLocationCode { get; set; }
        public string ChallanToLocationCode { get; set; }
        public string ChallanToLocationName { get; set; }
        public DateTime? ChallanDate { get; set; }
        public string ChallanSeqNo { get; set; }
        public string ItemTransaction { get; set; }
        public byte StoreLocationCode { get; set; }
        public string StoreLocationName { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemModel { get; set; }
        public double? DeliveryQuantity { get; set; }
        public double? ReceiveQuantity { get; set; }
        public double? InTransitQuantity { get; set; }
        public double? DeliveryValue { get; set; }
        public double? ReceiveValue { get; set; }
        public double? InTransitValue { get; set; }
        public string ChallanNo { get; set; }
        public string RefChallanNo { get; set; }
    }
}
