using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarERP.DomainModel.InventoryModel
{
    public class MrrInformationGlanceDetails
    {
        public string MRRNo { get; set; }
        public string MRRSeqNo { get; set; }
        public string MRRType { get; set; }
        public DateTime TransDate { get; set; }
        public string ReceiveFrom { get; set; }
        public string RefChallanNo { get; set; }
        public string ChallanSeqNo { get; set; }
        public string ChallanLocationCode { get; set; }
        public string MrrLocationCode { get; set; }
    }
}
