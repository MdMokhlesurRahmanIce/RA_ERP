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
    
    public partial class Inv_RouteMaster
    {
        public Inv_RouteMaster()
        {
            this.Inv_RouteDetail = new HashSet<Inv_RouteDetail>();
        }
    
        public string RouteNo { get; set; }
        public string RouteCategory { get; set; }
        public string RouteName { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public byte Status { get; set; }
    
        public virtual ICollection<Inv_RouteDetail> Inv_RouteDetail { get; set; }
    }
}
