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
    
    public partial class Inv_ItemModel
    {
        public Inv_ItemModel()
        {
            this.Inv_ItemMaster = new HashSet<Inv_ItemMaster>();
            this.Sal_PackageDetail = new HashSet<Sal_PackageDetail>();
        }
    
        public string ItemModelID { get; set; }
        public string Description { get; set; }
        public string ItemCategory { get; set; }
        public Nullable<byte> Status { get; set; }
        public Nullable<bool> IsValidForDistribution { get; set; }
    
        public virtual Inv_ItemCategorySubCategory Inv_ItemCategorySubCategory { get; set; }
        public virtual ICollection<Inv_ItemMaster> Inv_ItemMaster { get; set; }
        public virtual ICollection<Sal_PackageDetail> Sal_PackageDetail { get; set; }
    }
}
