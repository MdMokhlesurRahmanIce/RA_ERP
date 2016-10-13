using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarERP.DomainModel.SalesModel
{
    public class CustomerCollectionAdjustmentForAudit
    {
        public byte SerialNo { get; set; }
        public DateTime CollectionDate { get; set; }
        public string RefMemoNo { get; set; }
        public decimal CollectionAmount { get; set; }
        public string CollectedByEmployeeID { get; set; }
        public string CollectedByEmployeeName { get; set; }
        public string AuditReason { get; set; }
        public string ResponsibleEmployeeID { get; set; }
        public string ResponsibleEmployeeName { get; set; }
        public string Remarks { get; set; }
    }
}
