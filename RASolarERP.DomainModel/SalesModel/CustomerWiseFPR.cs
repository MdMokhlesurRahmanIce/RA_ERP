using RASolarERP.DomainModel.HRMSModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.SalesModel
{
    public class CustomerWiseFPR
    {
        public string EmployeeID { get; set; }
        public byte ScheduledCollectionDay { get; set; }

        public int? CustomerFPRMissing { get; set; }
        public int? CollectionDayMissing { get; set; }
        public int? MissingAll { get; set; }

        public List<CustomerFPRAndScheduledCollectionEntry> CustomerFPRAndScheduledCollection { get; set; }
        public List<CustomerFPRNDayWiseRegularOrODTarget> CustomerFPRNDayWiseRegularRODTarget { get; set; }
        public List<EmployeeDetailsInfo> LocationWiseEmployee { get; set; }
        public List<SalesDay> CollectionDays { get; set; }
    }
}
