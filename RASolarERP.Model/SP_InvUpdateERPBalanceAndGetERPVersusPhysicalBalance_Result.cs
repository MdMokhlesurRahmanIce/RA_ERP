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
    
    public partial class SP_InvUpdateERPBalanceAndGetERPVersusPhysicalBalance_Result
    {
        public byte StoreLocation { get; set; }
        public string YearMonth { get; set; }
        public string LocationCode { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemCategory { get; set; }
        public string ItemModel { get; set; }
        public double ERPBalance { get; set; }
        public double PhysicalBalance { get; set; }
    }
}
