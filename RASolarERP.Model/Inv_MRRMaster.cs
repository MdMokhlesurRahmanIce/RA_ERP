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
    
    public partial class Inv_MRRMaster
    {
        public Inv_MRRMaster()
        {
            this.Aud_AuditAdjustmentRelatedChallanOrMRRForReference = new HashSet<Aud_AuditAdjustmentRelatedChallanOrMRRForReference>();
            this.Inv_MRRDetails = new HashSet<Inv_MRRDetails>();
        }
    
        public string LocationCode { get; set; }
        public string MRRSeqNo { get; set; }
        public System.DateTime MRRDate { get; set; }
        public byte ForStoreLocation { get; set; }
        public string RefMRRNo { get; set; }
        public string RefExternalChallanNo { get; set; }
        public Nullable<System.DateTime> RefExternalChallanDate { get; set; }
        public string ChallanLocationCode { get; set; }
        public string ChallanSeqNo { get; set; }
        public string RefCustomerCode { get; set; }
        public string RefVendorID { get; set; }
        public string ItemType { get; set; }
        public string ItemTransTypeID { get; set; }
        public Nullable<byte> RefLocationType { get; set; }
        public string RefUserRoleOrGroupID { get; set; }
        public string RefAELocationCode { get; set; }
        public Nullable<System.DateTime> RefAETransDate { get; set; }
        public string RefAEProjectCode { get; set; }
        public string RefAETransNo { get; set; }
        public string Particulars { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public byte Status { get; set; }
    
        public virtual ICollection<Aud_AuditAdjustmentRelatedChallanOrMRRForReference> Aud_AuditAdjustmentRelatedChallanOrMRRForReference { get; set; }
        public virtual Common_LocationInfo Common_LocationInfo { get; set; }
        public virtual Inv_ChallanMaster Inv_ChallanMaster { get; set; }
        public virtual Inv_ItemTransactionTypes Inv_ItemTransactionTypes { get; set; }
        public virtual ICollection<Inv_MRRDetails> Inv_MRRDetails { get; set; }
        public virtual Inv_Sys_ItemType Inv_Sys_ItemType { get; set; }
        public virtual Inv_VendorInfo Inv_VendorInfo { get; set; }
        public virtual Sal_Customer Sal_Customer { get; set; }
    }
}
