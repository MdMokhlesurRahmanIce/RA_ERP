//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RASolarSecurity.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserInformation
    {
        public UserInformation()
        {
            this.UserLocationMapping = new HashSet<UserLocationMapping>();
            this.UserLocationMapping1 = new HashSet<UserLocationMapping>();
        }
    
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string UserRoleOrGroupID { get; set; }
        public Nullable<bool> IsLocationDependent { get; set; }
        public string OnlyForLocation { get; set; }
        public Nullable<bool> IsAuthenticApproverForThisLocation { get; set; }
        public byte Status { get; set; }
        public System.DateTime EntryTime { get; set; }
    
        public virtual Sys_StatusInfo Sys_StatusInfo { get; set; }
        public virtual UserRoleOrGroupInformation UserRoleOrGroupInformation { get; set; }
        public virtual ICollection<UserLocationMapping> UserLocationMapping { get; set; }
        public virtual ICollection<UserLocationMapping> UserLocationMapping1 { get; set; }
    }
}
