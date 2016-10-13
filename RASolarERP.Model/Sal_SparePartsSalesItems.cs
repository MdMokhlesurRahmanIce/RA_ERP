//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RASolarERP.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sal_SparePartsSalesItems
    {
        public Sal_SparePartsSalesItems()
        {
            this.Sal_SparePartsSalesItemsWithSerialNo = new HashSet<Sal_SparePartsSalesItemsWithSerialNo>();
        }
    
        public string LocationCode { get; set; }
        public string SPSSeqNo { get; set; }
        public byte CompSeqNo { get; set; }
        public string ItemCode { get; set; }
        public double ItemQuantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal UnitPrice { get; set; }
        public Nullable<byte> FromStoreLocation { get; set; }
        public string ItemCategory { get; set; }
        public string ItemCapacity { get; set; }
        public string ItemModel { get; set; }
        public string UnitOfMeasure { get; set; }
        public Nullable<decimal> DiscountForDisasterRecovery { get; set; }
    
        public virtual Inv_ItemMaster Inv_ItemMaster { get; set; }
        public virtual ICollection<Sal_SparePartsSalesItemsWithSerialNo> Sal_SparePartsSalesItemsWithSerialNo { get; set; }
        public virtual Sal_SparePartsSalesMaster Sal_SparePartsSalesMaster { get; set; }
    }
}
