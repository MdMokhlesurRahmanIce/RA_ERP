using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.SalesModel
{
    public class CheckLoadRequestEntry 
    {
        public string YearMonth { get; set; } 
        public string LocayionCode { get; set; }
        public Nullable<bool> IsLoadRequestCompleted { get; set; }
        
    }
}
