using RASolarERP.DomainModel.SalesModel;
using RASolarERP.Model;
using RASolarHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RASolarERP.Web.Areas.Sales.Models
{
    public class SalesDataProcess
    {
        private RASolarERPService erpService = new RASolarERPService();

        public Sal_CustomerPrePost AssignCustomerInfo(FormCollection fCollection, LoginHelper objLoginHelper)
        {
            try
            {
                Sal_CustomerPrePost objCustomer = new Sal_CustomerPrePost();

                objCustomer.CustomerCode = fCollection["hfCustomerCode"];
                objCustomer.CustomerName = fCollection["txtCustomerName"];
                objCustomer.FathersOrHusbandName = fCollection["txtFatherHusbandName"];
                objCustomer.MothersName = fCollection["txtMotherName"];
                objCustomer.Gender = fCollection["ddlGender"];
                objCustomer.NationalIDCard = fCollection["txtNationalIdNumber"];
                objCustomer.PhoneNo = fCollection["txtMobileNumber"];
                objCustomer.Village = fCollection["txtVillage"];
                objCustomer.PostOffice = fCollection["txtPostOffice"];
                objCustomer.ThanaID = fCollection["ddlUpazillaThana"];
                objCustomer.DistrictCode = erpService.ReadUpazillaByID(fCollection["ddlUpazillaThana"]).DIST_CODE;
                objCustomer.UnitCode = objLoginHelper.LogInForUnitCode;
                objCustomer.CustomerType = fCollection["ddlCustomerType"];

                objCustomer.Occupation = string.IsNullOrEmpty(fCollection["ddlOccupation"].Trim()) ? null : fCollection["ddlOccupation"].Trim();
                objCustomer.IncomeRange = string.IsNullOrEmpty(fCollection["ddlIncomeRange"].Trim()) ? null : fCollection["ddlIncomeRange"].Trim();
                objCustomer.TotalFamilyMember = string.IsNullOrEmpty(fCollection["txtFamilyMember"].Trim()) ? Convert.ToByte(0) : Convert.ToByte(fCollection["txtFamilyMember"].Trim());

                if (!string.IsNullOrEmpty(fCollection["txtWomenMember"].Trim()))
                {
                    objCustomer.WomenInTotalFamilyMember = Convert.ToByte(fCollection["txtWomenMember"].Trim());
                }
                else
                {
                    objCustomer.WomenInTotalFamilyMember = 0;
                }

                if (fCollection["ddlIsConsultedWithWomenMemberForInstallationOfLamps"].Trim() == "0")
                    objCustomer.IsConsultedWithWomenMemberForInstallationOfLamps = false;
                else
                    objCustomer.IsConsultedWithWomenMemberForInstallationOfLamps = true;

                objCustomer.FuelUsedBeforeSHS = string.IsNullOrEmpty(fCollection["ddlFuelUsedBeforeSHS"].Trim()) ? null : fCollection["ddlFuelUsedBeforeSHS"].Trim();
                objCustomer.FuelConsumptionPerMonth = string.IsNullOrEmpty(fCollection["txtFuelConsumptionPerMonthLitre"].Trim()) ? Convert.ToByte(0) : Convert.ToByte(fCollection["txtFuelConsumptionPerMonthLitre"].Trim());
                objCustomer.RelationWithGuardian = string.IsNullOrEmpty(fCollection["ddlGurdianRelation"]) ? null : fCollection["ddlGurdianRelation"].Trim();
                objCustomer.UnionID = fCollection["ddlUnion"].Trim();

                objCustomer.CreatedBy = objLoginHelper.LogInID;
                objCustomer.CreatedOn = DateTime.Now;

                if (fCollection["ddlModeOfPayment"].ToLower().Contains("cash"))
                    objCustomer.Status = Helper.CashSalesCustomer;
                else
                    objCustomer.Status = Helper.Active;

                return objCustomer;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Sal_SalesAgreementPrePost AssignSalesAgreement(FormCollection fCollection, List<PackageDetails> lstPackageDetails)
        {
            try
            {
                Sal_SalesAgreementPrePost objSalesAgreement = new Sal_SalesAgreementPrePost();

                ServiceChargeInformation objServiceChargePolicy = new ServiceChargeInformation();
                DownPaymentPolicy objPackageVDownpaymnet = new DownPaymentPolicy();

                bool reSaleStatus = false;

                if (Convert.ToInt32(fCollection["hfIsReSalesOrSales"].Trim()) == 1)  // If The Sales Agreement Is Resale
                {
                    reSaleStatus = true;
                }

                if (fCollection["ddlProgram"] != "BIO001")
                {
                    objServiceChargePolicy = erpService.ReadServiceChargePolicy(fCollection["ddlProgram"], fCollection["ddlCustomerType"], fCollection["ddlModeOfPayment"]);
                    objPackageVDownpaymnet = erpService.ReadDownPaymentPolicy(fCollection["ddlModeOfPayment"], fCollection["ddlPackage"]);

                    objSalesAgreement.CustomerCode = fCollection["hfCustomerCode"];
                    objSalesAgreement.AgreementDate = Convert.ToDateTime(Helper.DateFormatMMDDYYYY(fCollection["dtpAgreementDate"]));
                    objSalesAgreement.PackageCode = fCollection["ddlPackage"];
                    objSalesAgreement.ModeOfPaymentID = fCollection["ddlModeOfPayment"];
                    objSalesAgreement.CustomerType = fCollection["ddlCustomerType"];
                    objSalesAgreement.NumberOfInstallment = Convert.ToByte(fCollection["txtInstallments"]);

                    objSalesAgreement.PackagePrice = Convert.ToDecimal(fCollection["txtPackagePrice"].Trim());

                    objSalesAgreement.DiscountID = lstPackageDetails[0].DiscountID;
                    objSalesAgreement.DiscountAmount = Convert.ToDecimal(fCollection["txtDiscountAmount"]);

                    objSalesAgreement.AmountAfterDiscount = Convert.ToDecimal(fCollection["txActualPayableAmount"]);

                    objSalesAgreement.DownPaymentID = objPackageVDownpaymnet.DownPaymentID;
                    objSalesAgreement.STDDownPaymentPercentage = objPackageVDownpaymnet.DownPaymentPercentage;
                    objSalesAgreement.DownPaymentAmount = Convert.ToDecimal(fCollection["txtDPAmount"]);

                    objSalesAgreement.ServiceChargeID = objServiceChargePolicy.ServiceChargeID;
                    objSalesAgreement.STDServiceChargePercentage = objServiceChargePolicy.ServiceChargePercentage;

                    objSalesAgreement.TotalPrincipalReceivable = Convert.ToDecimal(fCollection["txtRestAmount"]);
                    objSalesAgreement.TotalServiceChargeReceivable = Convert.ToDecimal(fCollection["txtTotalServiceCharge"]); ;
                    objSalesAgreement.TotalPrincipalPlusServiceChargeReceivable = Convert.ToDecimal(fCollection["txtTotalPayableWithServiceCharge"]);
                    objSalesAgreement.InstallmentSize = Convert.ToDecimal(fCollection["txtInstallmentAmount"]);
                    objSalesAgreement.InstallmentSizePrincipal = Convert.ToDecimal(fCollection["txtMonthlyPrincipalPayable"]);
                    objSalesAgreement.InstallmentSizeServiceCharge = Convert.ToDecimal(fCollection["txtMonthlyServiceChargePayable"]);
                    objSalesAgreement.ScheduledCollectionDay = Convert.ToByte(fCollection["ddlCollectionDay"].Trim());
                    objSalesAgreement.ProjectCode = fCollection["ddlProject"];
                    objSalesAgreement.ProgramCode = fCollection["ddlProgram"];

                    if (fCollection["txtMemoNumber"].Trim() != "")
                    {
                        if (!erpService.IsCashMemoManagementEnabled(Helper.CompanyName))
                        {
                            objSalesAgreement.RefMemoNo = fCollection["txtMemoNumber"];
                        }
                        else
                        {
                            objSalesAgreement.CashMemoNo = fCollection["txtMemoNumber"];
                            objSalesAgreement.CashMemoUsesID = Helper.CashMemuUsesIdFirst;
                        }
                    }

                    objSalesAgreement.TechnicalFees = 0;
                    objSalesAgreement.Subsidies = 0;
                    objSalesAgreement.DisbursementNo = 0;
                    objSalesAgreement.SalespersonCode = fCollection["ddlEmployee"];
                    objSalesAgreement.IsReSales = reSaleStatus;
                }
                else if (fCollection["ddlProgram"] == "BIO001")
                {

                }

                return objSalesAgreement;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Sal_SalesAgreementPrePost AssignSalesItemAndSave(FormCollection fCollection, List<PackageDetails> lstPackageDetails, LoginHelper objLoginHelper)
        {
            int modelCounter = 0, serialCounter = 0;
            string itemCode = string.Empty, modelCode = string.Empty;
            byte storeLocation = 1;
            byte warrantyInMonth=0;
            double itemFixedQuantity = 0;

            Sal_SalesAgreementPrePost objSalesAgreement = new Sal_SalesAgreementPrePost();
            Sal_CustomerPrePost objCustomer = new Sal_CustomerPrePost();

            Sal_SalesItemsPrePost objSalesItem;
            Sal_SalesItemsWithSerialNoPrePost objItemSalesWithSerialNo;
            List<Inv_ItemMaster> lstItemMaster = new List<Inv_ItemMaster>();
            List<Sal_SalesItemsWithSerialNoPrePost> lstItemWithSerials = new List<Sal_SalesItemsWithSerialNoPrePost>();
            List<Sal_SalesItemsPrePost> lstSalesItem = new List<Sal_SalesItemsPrePost>();

            try
            {
                objCustomer = this.AssignCustomerInfo(fCollection, objLoginHelper);
                objSalesAgreement = this.AssignSalesAgreement(fCollection, lstPackageDetails);

                string[] packageComponents = fCollection["hfStockLocationWiseComponenet"].Split('#');

                lstItemMaster = erpService.ReadItemMaster();

                foreach (PackageDetails pkgd in lstPackageDetails)
                {
                    objSalesItem = new Sal_SalesItemsPrePost();
                    objSalesItem.CustomerCode = fCollection["hfCustomerCode"].Trim();
                    objSalesItem.CompSeqNo = pkgd.CompSeqNo;
                    objSalesItem.ItemCategory = pkgd.ItemCategoryID;
                    objSalesItem.ItemCapacity = pkgd.ItemCapacity;
                    objSalesItem.UnitOfMeasure = pkgd.UnitOfMeasure;

                    if (pkgd.IsShowInSalesAgreementPage == true)
                    {
                        string[] componentStoreLocation = packageComponents[serialCounter].Split('_');
                        storeLocation = Convert.ToByte(componentStoreLocation[3].Trim());  // Item Store Location                       
                        objSalesItem.FromStoreLocation = storeLocation;
                    }


                    //
                    if(pkgd.IsItAWarrantyItem==true)
                    {
                        string[] componentStoreLocation = packageComponents[serialCounter].Split('_');
                        warrantyInMonth = Convert.ToByte(componentStoreLocation[5].Trim());  // warrenty                       
                        objSalesItem.WarrantyInMonth = warrantyInMonth;
                    
                    
                    }

                    //



                    string[] orderDetails = packageComponents[modelCounter].Split('_');

                    if (!string.IsNullOrEmpty(orderDetails[4].Trim()))
                    {
                        objSalesItem.ItemQuantity = Convert.ToDouble(orderDetails[4]);
                    }

                    if (pkgd.IsShowInSalesAgreementPage == true && pkgd.ItemModel.Trim() == "")
                    {
                       // string[] componentsDeatils = packageComponents[modelCounter].Split('_');

                        if (orderDetails[0] == pkgd.ItemCategoryID) //componentsDeatils[0]
                            objSalesItem.ItemModel = orderDetails[1]; // componentsDeatils[1];  // Item Model

                        modelCode = objSalesItem.ItemModel;

                        modelCounter++;
                    }
                    else
                    {
                        if (pkgd.ItemModel.Trim() != "")
                        {
                            objSalesItem.ItemModel = pkgd.ItemModel;
                            modelCode = objSalesItem.ItemModel;
                        }
                        else
                        {
                            objSalesItem.ItemModel = null;
                            modelCode = string.Empty;
                        }
                        modelCounter++;
                    }

                    if (modelCode != string.Empty)
                    {
                        if (pkgd.ItemCapacity.Trim() == "NULL" || pkgd.ItemCapacity.Trim() == "")
                        {
                            var vIt = (from itc in lstItemMaster where itc.ItemCategory == pkgd.ItemCategoryID && itc.ItemCapacity == null && itc.ItemModel == objSalesItem.ItemModel select itc.ItemCode).FirstOrDefault();
                            objSalesItem.ItemCode = vIt.ToString();
                        }
                        else
                        {
                            var vIt = (from itc in lstItemMaster where itc.ItemCategory == pkgd.ItemCategoryID && itc.ItemCapacity == pkgd.ItemCapacity && itc.ItemModel == objSalesItem.ItemModel select itc.ItemCode).FirstOrDefault();
                            objSalesItem.ItemCode = vIt.ToString();
                        }
                    }
                    else
                    {
                        if (pkgd.ItemCapacity.Trim() == "NULL" || pkgd.ItemCapacity.Trim() == "")
                        {
                            var vIt = (from itc in lstItemMaster where itc.ItemCategory == pkgd.ItemCategoryID && itc.ItemCapacity == null && itc.ItemModel == null select itc.ItemCode).FirstOrDefault();
                            objSalesItem.ItemCode = vIt.ToString();
                        }
                        else
                        {
                            var vIt = (from itc in lstItemMaster where itc.ItemCategory == pkgd.ItemCategoryID && itc.ItemCapacity == pkgd.ItemCapacity && itc.ItemModel == null select itc.ItemCode).FirstOrDefault();
                            objSalesItem.ItemCode = vIt.ToString();
                        }
                    }

                    objSalesItem.UnitCost = (from itc in lstItemMaster where itc.ItemCode == objSalesItem.ItemCode select itc.AverageUnitCost).FirstOrDefault();

                    if (pkgd.IsShowInSalesAgreementPage == true && pkgd.IsSerialNoMandatory == true)
                    {
                        string[] componentsDeatils = packageComponents[serialCounter].Split('_');

                        objItemSalesWithSerialNo = new Sal_SalesItemsWithSerialNoPrePost();

                        objItemSalesWithSerialNo.CustomerCode = fCollection["hfCustomerCode"];
                        objItemSalesWithSerialNo.CompSeqNo = pkgd.CompSeqNo;
                        objItemSalesWithSerialNo.ItemSerialNo = componentsDeatils[2].Trim();
                        objItemSalesWithSerialNo.Status = Helper.Active;
                        objItemSalesWithSerialNo.ItemCode = objSalesItem.ItemCode;
                        objItemSalesWithSerialNo.RefStoreLocation = storeLocation;  // Item Store Location
                        objItemSalesWithSerialNo.RefLocationCode = objLoginHelper.LocationCode;

                        lstItemWithSerials.Add(objItemSalesWithSerialNo);

                        //if (!string.IsNullOrEmpty(serialTempTableRows))
                        //    serialTempTableRows += "," + "(" + objSalesItem.ItemCode + ",'" + objItemSalesWithSerialNo.ItemSerialNo + "','" + pkgd.ItemCategoryID + "'," + Convert.ToByte(objItemSalesWithSerialNo.RefStoreLocation) + ",'" + objLoginHelper.LocationCode + "','" + fCollection["hfCustomerCode"].Trim() + "')";
                        //else
                        //    serialTempTableRows = "(" + objSalesItem.ItemCode + ",'" + objItemSalesWithSerialNo.ItemSerialNo + "','" + pkgd.ItemCategoryID + "'," + Convert.ToByte(objItemSalesWithSerialNo.RefStoreLocation) + ",'" + objLoginHelper.LocationCode + "','" + fCollection["hfCustomerCode"].Trim() + "')";

                    }

                    serialCounter++;
                    lstSalesItem.Add(objSalesItem);
                }

                objSalesAgreement = erpService.CreateSalesAgreement(objCustomer, objSalesAgreement, lstSalesItem, lstItemWithSerials);

            }
            catch (Exception ex)
            {
                throw;
            }

            return objSalesAgreement;
        }

        public List<CustomerLedgerReport> CustomerLedgerProcess(string customerCode, out decimal totalCollection)
        {
            List<CustomerLedgerReport> lstCustomerLedger = new List<CustomerLedgerReport>();
            lstCustomerLedger = erpService.ReadCustomerLedgerReport(customerCode);

            int rowCounter = 0;
            decimal totalCollectionSum = 0;

            if (lstCustomerLedger.Count > 0)
            {
                CustomerLedgerReport objCustomerLedger = new CustomerLedgerReport();

                objCustomerLedger.CustomerCode = "";
                objCustomerLedger.CustomerName = "";
                objCustomerLedger.PanelSerialNo = "";
                objCustomerLedger.BatterySerialNo = "";
                objCustomerLedger.CustomerType = "";
                objCustomerLedger.PhoneNo = "";
                objCustomerLedger.AgreementDate = new DateTime(1900, 1, 1);
                objCustomerLedger.Package = "";
                objCustomerLedger.PaymentMode = "";
                objCustomerLedger.PackagePrice = 0;
                objCustomerLedger.DiscountAmount = 0;
                objCustomerLedger.DiscountPercentage = 0;
                objCustomerLedger.AmountAfterDiscount = 0;
                objCustomerLedger.DownPaymentAmount = 0;
                objCustomerLedger.STDDownPaymentPercentage = 0;
                objCustomerLedger.TotalPrincipalReceivable = 0;
                objCustomerLedger.ServiceChargePercentage = 0;
                objCustomerLedger.TotalServiceChargeReceivable = 0;
                objCustomerLedger.TotalPrincipalPlusServiceChargeReceivable = lstCustomerLedger[0].TotalPrincipalPlusServiceChargeReceivable;
                objCustomerLedger.CollectionType = "Total Loan (Loan & Service Charge)"; ;
                objCustomerLedger.CollectionDate = new DateTime(1900, 1, 1);
                objCustomerLedger.CollectionAmount = 0;
                objCustomerLedger.OverdueOrAdvanceBalance = 0;
                objCustomerLedger.OutstandingBalance = 0;
                objCustomerLedger.Status = "";

                lstCustomerLedger.Insert(0, objCustomerLedger);

                foreach (CustomerLedgerReport clr in lstCustomerLedger)
                {
                    if (rowCounter != (lstCustomerLedger.Count - 1))
                    {
                        lstCustomerLedger[rowCounter + 1].TotalPrincipalPlusServiceChargeReceivable = Convert.ToDecimal(clr.TotalPrincipalPlusServiceChargeReceivable - lstCustomerLedger[rowCounter + 1].CollectionAmount);
                    }
                    totalCollectionSum += Convert.ToDecimal(clr.CollectionAmount);
                    rowCounter++;
                }
            }

            totalCollection = totalCollectionSum;
            return lstCustomerLedger;
        }
    }
}
