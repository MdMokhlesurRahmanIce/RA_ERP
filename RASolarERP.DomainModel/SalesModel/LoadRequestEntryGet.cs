using RASolarERP.DomainModel.HRMSModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.SalesModel
{
    public class LoadRequestEntryGet  
    {
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string CorporatePhoneNo { get; set; }
        public decimal? LoadBalanceForRSFServices { get; set; }
        public decimal? LoadRequestForRSFServices {get; set;}
        public decimal? LoadRequestForPayWellServices { get; set; }
      
    }
}
