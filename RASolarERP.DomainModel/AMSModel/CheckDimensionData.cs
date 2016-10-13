using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarERP.DomainModel.AMSModel
{
    public class CheckDimensionData 
    {
        public string LocationCode { get; set; }
        public DateTime? TransDate { get; set; }
        public string ProjectCode { get; set; }
        public string TransNo { get; set; }
        public string SerialNo { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Amounts { get; set; }
        
    }
}
