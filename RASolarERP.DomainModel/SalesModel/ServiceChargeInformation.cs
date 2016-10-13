using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.SalesModel
{
    public class ServiceChargeInformation
    {
        public string ServiceChargeID { get; set; }
        public string ServiceChargeDescription { get; set; }
        public byte ServiceChargePercentage { get; set; }
    }
}
