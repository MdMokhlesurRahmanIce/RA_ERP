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
    
    public partial class Common_ZoneInfo
    {
        public Common_ZoneInfo()
        {
            this.Common_RegionInfo = new HashSet<Common_RegionInfo>();
        }
    
        public string Zone_Code { get; set; }
        public string Zone_Name { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<System.DateTime> Modified { get; set; }
        public string ZM_CellNo { get; set; }
        public Nullable<byte> Status { get; set; }
        public string RespectiveAreaUser { get; set; }
    
        public virtual Common_LocationInfo Common_LocationInfo { get; set; }
        public virtual ICollection<Common_RegionInfo> Common_RegionInfo { get; set; }
    }
}