using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarERP.DomainModel.AMSModel
{
    public class VoucherTransactionAudit
    {
        public string LocationCode { get; set; }
        public DateTime TransDate { get; set; }
        public string ProjectCode { get; set; }
        public string TransNo { get; set; }
        public string AuditSeqNo { get; set; }
        public string ReasonCode { get; set; }
        public DateTime? RefVoucherDate { get; set; }
        public string ResponsibleEmployeeID { get; set; }
        public string Remarks { get; set; }
    }
}
