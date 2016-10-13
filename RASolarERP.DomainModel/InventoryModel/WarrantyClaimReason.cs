using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.InventoryModel
{
    public class WarrantyClaimReason
    {
        public string ReasonCode { get; set; }
        public string ReasonDescription { get; set; }
        public byte OrderSerialNo { get; set; }
        public byte Status { get; set; }
    }
}
