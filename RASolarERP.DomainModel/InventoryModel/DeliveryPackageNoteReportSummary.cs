using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarERP.DomainModel.InventoryModel
{
    public class DeliveryPackageNoteReportSummary  
    {
        public string SLNo { get; set; }
        public string PackageCode { get; set; }
        public string PackageName { get; set; }
        public string Capacity { get; set; }
        public string Rate { get; set; }
        public double QtyPcs { get; set; }
        public string Amount { get; set; }
    }
}
