using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarHelper
{
    public class PageAccessRightHelper
    {
        public string RoleOrGroupID { set; get; }
        public string ModuleID { set; get; }
        public string PageID { set; get; }
        public string PageNameToLink { set; get; }
        public bool? IsGrantedForSelect { set; get; }
        public bool? IsGrantedForInsert { set; get; }
        public bool? IsGrantedForUpdate { set; get; }
        public bool? IsGrantedForDelete { set; get; }
        public string MessageToShow { set; get; }
        public byte AccessStatus { get; set; }
    }
}
