using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarERP.DomainModel.InventoryModel
{
    public class FixedAssetSerialList
    {
        public string AllocationSeqNo { get; set; }
        public string ItemSerialNo { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public DateTime? AllocationDate { get; set; }
        public string ItemCode { get; set; }
        public string Remarks { get; set; }
        public DateTime? AssignedDate { get; set; }
        public string Option { get; set; }
    }
}
