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
    
    public partial class InventoryAtVendorValuationByStockLocation
    {
        public string ItemPosition { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Item_Model { get; set; }
        public double QntyInNew { get; set; }
        public double QntyInSR { get; set; }
        public double QntyInRDNew { get; set; }
        public double QntyInRDSR { get; set; }
        public double QntyInDMG { get; set; }
        public double QntyAtVendor { get; set; }
        public double TotalQnty { get; set; }
        public double UnitPriceForNew { get; set; }
        public double UnitPriceForSR { get; set; }
        public decimal ValueForNew { get; set; }
        public decimal ValueForSR { get; set; }
        public decimal ValueForRDNew { get; set; }
        public decimal ValueForRDSR { get; set; }
        public decimal ValueForDMG { get; set; }
        public decimal ValueForInvAtVendor { get; set; }
        public Nullable<decimal> TotalValue { get; set; }
        public double QntyInCSI { get; set; }
    }
}
