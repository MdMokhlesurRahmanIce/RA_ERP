using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.SalesModel
{
   public class CustomerDataToPersonalInformation
    {
       public string CustomerCode { set; get; }
       public string CustomerName { set; get; }
       public string CustomerFatherHusbandName { set; get; }
       public string Village { set; get; }
       public string PostOffice { set; get; }
       public string UnionCode { set; get; }
       public string UnionName { set; get; }
       public string UpzillCode { set; get; }
       public string UpzillName { set; get; }
       public string DistrictCode { set; get; }
       public string DistrictName { set; get; }
       public string MobileNumber { set; get; }
       public string ModifiedBy { set; get; }
       public DateTime? ModifiedOn { set; get; }

    }
}
