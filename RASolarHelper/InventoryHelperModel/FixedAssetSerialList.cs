using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarHelper.InventoryHelperModel
{
    public class FixedAssetSerialList
    {
        public string ItemSerialNo { get; set; }
        public string EmployeeID { get; set; }
        public DateTime? AllocationDate { get; set; }
        public string Remarks { get; set; }
    }
}
