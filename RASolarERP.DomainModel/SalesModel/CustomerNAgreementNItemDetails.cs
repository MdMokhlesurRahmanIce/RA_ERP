using RASolarERP.DomainModel.AMSModel;
using RASolarERP.DomainModel.InventoryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarERP.DomainModel.SalesModel
{
    public class CustomerNAgreementNItemDetails
    {

        // [LocationCode]
        //,[YearMonth]
        //,[CustomerCode]
        //,[DataStatusFlag]
        //,[AuditSeqNo]
        //,[CustomerName]
        //,[AgreementDate]
        //,[PackageCapacity]
        //,[PackageLight]
        //,[PackageCode]
        //,[ModeOfPaymentID]
        //,[CustomerType]
        //,[PackagePrice]
        //,[DownPaymentID]
        //,[DownPaymentAmount]
        //,[ServiceChargeID]
        //,[STDServiceChargePercentage]
        //,[CashMemoNo]
        //,[CashMemoUsesID]
        //,[RefMemoNo]
        //,[IsReSales]
        //,[StoreLocationForPanel]
        //,[PanelItemCode]
        //,[PanelSerialNo]
        //,[StoreLocationForBattery]
        //,[BatteryItemCode]
        //,[BatterySerialNo]
        //,[PanelStructureItemCode]
        //,[HolderItemCode]
        //,[Remarks]
        //,[Status]

        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public DateTime? AgreementDate { get; set; }
        public string CustomerType { get; set; }
        public string CustomerTypeName { get; set; }
        public string PackageCode { get; set; }
        public string PackageName { get; set; }
        public string Capacity { get; set; }
        public string PackageCapacity { get; set; }
        public string Light { get; set; }
        public string PackageLight { get; set; }
        
        public string ModeOfPaymentID { get; set; }
        public string ModeOfPaymentName { get; set; }

        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }

        public string DownPaymentID { get; set; }
        public decimal DownPaymentAmount { get; set; }
        public byte STDDownPaymentPercentage { get; set; }

        public string ServiceChargeID { get; set; }
        public byte STDServiceChargePercentage { get; set; }

        public string RefMemoNo { get; set; }
        public string CashMemoNo { get; set; }
        public string CashMemoUsesID { get; set; }
        public decimal? PackagePrice { get; set; }
        public bool IsResales { get; set; }

        public string PanelItemCode { get; set; }
        public string PanelName { get; set; }
        public byte StoreLocationForPanel { get; set; }        
        public string PanelSerialNo { get; set; }

        public string BatteryItemCode { get; set; }
        public string BatteryName { get; set; }
        public byte StoreLocationForBattery { get; set; }       
        public string BatterySerialNo { get; set; }

        public string PanelStructureItemCode { get; set; }
        public string PanelStructureName { get; set; }
        
        public string HolderItemCode { get; set; }
        public string HolderName { get; set; }


        public double? HolderQty { get; set; }
        public double? TubeLightQty { get; set; }
        public double? LEDTubeQty { get; set; }
        public double? LED2Qty { get; set; }
        public double? LED3Qty { get; set; }
        public double? LED5Qty { get; set; }
        public double? CFLQty { get; set; }
        public string LampShade { get; set; }
        public double? LampShadeQty { get; set; }
        public string CircuitOrLampCkt { get; set; }
        public double? CircuitOrLampCktQty { get; set; }
        public double? ChargeController10AmpsQty { get; set; }
        public double? ChargeController6AmpsQty { get; set; }
        public double? Cable70r76Qty { get; set; }
        public double? Cable40r76Qty { get; set; }
        public double? Cable23r76Qty { get; set; }
        public double? Cable14r76Qty { get; set; }
        public double? SwitchQty { get; set; }
        public double? SwitchBoardBoxQty { get; set; }

        public string Remarks { get; set; }

        public List<ProjectInfo> ProjectInfo { get; set; }
        public List<ItemInfo> PanelStructure { get; set; }
        public List<ItemInfo> Holder { get; set; }
        public List<ItemInfo> BatteryInfo { get; set; }


        public List<SalesItemDetails> CustomerItems { get; set; }
    }

}
