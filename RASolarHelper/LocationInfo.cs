using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarHelper
{
    public class LocationInfo
    {
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public string ParentLocationCode { get; set; }
        public string CustomerCodePrefix { get; set; }
        public byte? BackDayAllowedForTransaction { get; set; }
        public byte LocationType { get; set; }
    }
}
