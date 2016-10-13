using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarERP.DomainModel.InventoryModel
{
    public class DeliveryNoteSummary 
    {

        public string DistribScheduleNo { get; set; }
        public DateTime DeliveryChallanDate { get; set; }
        public string RouteNo { get; set; }
        public string RouteName { get; set; }
        public string VendorChallanNoForPackage { get; set; }
        public string VendorChallanNoForSpareParts { get; set; }
    }
}
