using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.SalesModel
{
    public class PackagePricingDetailsForSalesAgreement
    {
        public decimal PackagePrice { get; set; }
        public string DiscountID { get; set; }
        public double DiscountPercentage { get; set; }
        public bool IsDiscountAFixedAmount { get; set; }
        public decimal DiscountFixedAmount { get; set; }
        public double DiscountAmount { get; set; }
        public double AmountAfterDiscount { get; set; }
        public string DownPaymentID { get; set; }
        public double DownPaymentPercentage { get; set; }
        public double DownPaymentAmount { get; set; }
        public double TotalPrincipalReceivable { get; set; }
        public string ServiceChargeID { get; set; }
        public byte ServiceChargePercentage { get; set; }
        public double TotalServiceChargeReceivable { get; set; }
        public double TotalPrincipalPlusServiceChargeReceivable { get; set; }
        public byte NumberOfInstallment { get; set; }
        public double InstallmentSize { get; set; }
        public double InstallmentSizePrincipal { get; set; }
        public double InstallmentSizeServiceCharge { get; set; }
        public bool IsPackagePriceFixed { get; set; }

        public decimal OriginalPackagePrice { get; set; }
        public byte? PanelStoreLocation { get; set; }
        public byte? BatteryStoreLocation { get; set; }


    }
}
