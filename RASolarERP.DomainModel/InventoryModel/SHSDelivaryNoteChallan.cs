using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.InventoryModel
{
    public class SHSDelivaryNoteChallan
    {
        public string RefScheduleNo { set; get; }
        public DateTime ScheduleDate { set; get; }
        public string ScheduleNotes { set; get; }
        public string RouteNo { set; get; }

        public string RootName { set; get; }
        public string LocationCode { set; get; }
        public string LocationName { set; get; }
        public string ChallanNumber { set; get; }

        public string ItemCode { set; get; }
        public string ItemName { set; get; }
        public double ItemQuantity { set; get; }
    }
}
