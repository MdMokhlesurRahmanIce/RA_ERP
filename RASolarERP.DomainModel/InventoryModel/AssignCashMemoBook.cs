using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.InventoryModel
{
    public class AssignCashMemoBook
    {
        public string  EmployeeID  { get; set; }
        public string  EmployeeName  { get; set; }
        public double?  AvailableBookQuantity  { get; set; }
        public Int32  AlreadyAssignBookQuantity  { get; set; }
        public double?  AvailableBookQuantityToAssign  { get; set; }
        public Int32 AssignBookQuantityInUse { get; set; }
        public byte StoreLocation { get; set; }
        public string ItemCode { get; set; }
        public double? RemainingUnallocatedQuantity { get; set; }
        public double AvailableToAssignQuantity { get; set; }
        public Int16 AssignedQuantity { get; set; }
    }
}
