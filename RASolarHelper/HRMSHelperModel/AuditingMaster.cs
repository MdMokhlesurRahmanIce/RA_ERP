using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarHelper.HRMSHelperModel
{
    public class AuditingMaster
    { 
        public string LocationCode { get; set; }
        public string AuditSeqNo { get; set; }
        public DateTime? AuditStartDate { get; set; }
        public DateTime? AuditFinishDate { get; set; }
        public DateTime? AuditPeriodFromDate { get; set; }
        public DateTime? AuditPeriodToDate { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte Status { get; set; }

        public List<AuditingDetailsForAuditors> AuditingDetails { get; set; }
    }
}
