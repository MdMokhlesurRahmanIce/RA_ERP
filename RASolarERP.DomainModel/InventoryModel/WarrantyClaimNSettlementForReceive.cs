using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.InventoryModel
{
    public class WarrantyClaimNSettlementForReceive
    {
        public string ItemType {get;set;}
        public string StoreLocation {get;set;}
        public string ItemCategory {get;set;}
        public string ItemName {get;set;}
        public DateTime TransactionDate {get;set;}
        public string MRRSequenceNo {get;set;}
        public string MRRNo {get;set;}
        public string ReasonForClaimNParticulars {get;set;}
        public double ReceivedSerialQuantity {get;set;}
        public string ItemSerialNo {get;set;}
        public string ItemCode {get;set;}
        public string ItemCapacity {get;set;}
        public string ItemModel {get;set;}
        public double ReceiveQuantity {get;set;}
    }
}
