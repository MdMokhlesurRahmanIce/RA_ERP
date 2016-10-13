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
    
    public partial class UserRoleOrGroupInformation
    {
        public UserRoleOrGroupInformation()
        {
            this.UserInformation = new HashSet<UserInformation>();
            this.UserRoleOrGroupWisePermission = new HashSet<UserRoleOrGroupWisePermission>();
        }
    
        public string RoleOrGroupID { get; set; }
        public string RoleOrGroupName { get; set; }
        public byte Status { get; set; }
        public System.DateTime EntryTime { get; set; }
    
        public virtual Sys_StatusInfo Sys_StatusInfo { get; set; }
        public virtual ICollection<UserInformation> UserInformation { get; set; }
        public virtual ICollection<UserRoleOrGroupWisePermission> UserRoleOrGroupWisePermission { get; set; }
    }
}
