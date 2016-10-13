using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.Text.RegularExpressions;
using System.Text;

using RASolarERP.Model;
using RASolarHelper;
using RASolarERP.Web.Areas.Sales.Models;

using Telerik.Web.Mvc;
using RASolarERP.Web.Models;
using RASolarERP.DomainModel.SalesModel;

namespace RASolarERP.Web.Areas.Sales.Controllers
{
    public class SystemReturnsController : Controller
    {
        LoginHelper objLoginHelper = new LoginHelper();
        private SalesData salesDal = new SalesData();
        RASolarSecurityData securityDal = new RASolarSecurityData();
        private RASolarERPData erpDal = new RASolarERPData();
        string message = string.Empty;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SystemReturn()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "SystemReturn", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            if (objLoginHelper.TransactionOpenDate == Helper.InvalidDate())
            {
                Session["messageInformation"] = "Day Is Not Open For System Return";
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForSales.ToString("MMMM-yy");
            ViewBag.CurrentDay = DateTime.Now.Date.ToString("dd-MMM-yyyy");

            ViewBag.OpenDay = objLoginHelper.TransactionOpenDate.Date.ToString("dd-MMM-yyyy");
            ViewBag.OpenBackDay = objLoginHelper.TransactionBackDate.Date.ToString("dd-MMM-yyyy");

            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.CustomerPrefix = objLoginHelper.CustomerPrefix;

            string customerCodeSerial = (string)Session["CustomerCodeForSystemReturn"];
            customerCodeSerial = customerCodeSerial.Replace(objLoginHelper.CustomerPrefix, "");
            customerCodeSerial = customerCodeSerial.Replace("-", "");

            ViewBag.CustomerCode = (string)Session["CustomerCodeForSystemReturn"];
            ViewBag.CustomerCodeSerial = customerCodeSerial;

            return View();
        }

        [HttpPost]
        public ActionResult SystemReturn(FormCollection fCollection)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            Sal_SystemReturn objSystemReturn;
            Sal_SystemReturnItems objSystemReturnItems;
            Sal_SystemReturnItemsWithSerialNo objSystemReturnItemsWithSerialNo;

            List<Sal_SystemReturnItems> lstSystemReturnItems = new List<Sal_SystemReturnItems>();
            List<Sal_SystemReturnItemsWithSerialNo> lstSystemReturnItemsWithSerialNo = new List<Sal_SystemReturnItemsWithSerialNo>();

            int customerOrRsf = Convert.ToInt32(fCollection["hfCustomerOrRSF"]), serialCounter = 1;
            string serialTempTableRows = string.Empty;

            try
            {
                objSystemReturn = new Sal_SystemReturn();

                objSystemReturn.CustomerCode = fCollection["txtCustomerCode"].Trim();
                objSystemReturn.ReturnDate = Convert.ToDateTime(Helper.DateFormatMMDDYYYY(fCollection["dtpSystemReturn"].Trim()));
                objSystemReturn.MonthUsage = Convert.ToByte(fCollection["txtUsagesMonth"]);
                objSystemReturn.UsageChargeReceivable = Convert.ToDecimal(fCollection["txtUsageCharge"]);
                objSystemReturn.ServiceChargeReceivable = Convert.ToDecimal(fCollection["txtReceivableServiceCharge"]);
                objSystemReturn.NonReturnedItemValue = Convert.ToDecimal(fCollection["txtNonReturnItemsValue"]);
                objSystemReturn.TotalReceivable = Convert.ToDecimal(fCollection["txtTotalReceivable"]);
                objSystemReturn.TotalRecovered_DPPlusInstallment = Convert.ToDecimal(fCollection["txtTotalRecoveredDPAndInstallment"]);

                if (customerOrRsf == -1)
                {
                    objSystemReturn.ReceivableOrPayableAmount = Convert.ToDecimal(fCollection["txtReceaviblePaybleAmount"]) * -1;
                    objSystemReturn.ActualReceivedOrPaidAmount = Convert.ToDecimal(fCollection["txtActualPaidRSFORCustomer"]) * -1;
                }
                else
                {
                    if (fCollection["hfIsPaidToCustomerOnApprovalByManagementWhenReceivable"] == "1")
                    {
                       // objSystemReturn.ReceivableOrPayableAmount = Convert.ToDecimal(fCollection["txtReceaviblePaybleAmount"]) * -1;  -------Recommandatiion Md.Raiz vai to Chanage By Md.Sultan Mahmud
                        
                        objSystemReturn.ReceivableOrPayableAmount = Convert.ToDecimal(fCollection["txtReceaviblePaybleAmount"]) * 1;   //Newly added By Md.Sultan Mahmud
                        objSystemReturn.ActualReceivedOrPaidAmount = Convert.ToDecimal(fCollection["txtApprovedAmount"]) * -1;       
                        objSystemReturn.IsPaidToCustomerOnApprovalByManagementWhenReceivable = true;
                    }
                    else
                    {
                        objSystemReturn.ReceivableOrPayableAmount = Convert.ToDecimal(fCollection["txtReceaviblePaybleAmount"]);
                        objSystemReturn.ActualReceivedOrPaidAmount = Convert.ToDecimal(fCollection["txtActualPaidRSFORCustomer"]);
                        objSystemReturn.IsPaidToCustomerOnApprovalByManagementWhenReceivable = false;
                    }
                }

                objSystemReturn.CreatedBy = objLoginHelper.LogInID;
                objSystemReturn.CreatedOn = DateTime.Now;
                //itemCode + "," + soldQuantity + "," + unitCost + "," + returnQuantity + "," + returnItemsValue + "," + nonReturnItemsValue + "," + componentSequenceNumber + "," + stockLocation + "," + serialNumber;
                string[] systemReturnItemsCollection = fCollection["hfSystemReturnsItems"].Trim().Split('#');
                int sytemReturnItemsCollectionLength = systemReturnItemsCollection.Length;

                if (sytemReturnItemsCollectionLength > 0)
                {
                    for (int i = 0; i < sytemReturnItemsCollectionLength; i++)
                    {
                        string[] systemReturnItems = systemReturnItemsCollection[i].Trim().Split(',');

                        objSystemReturnItems = new Sal_SystemReturnItems();
                        objSystemReturnItems.CustomerCode = fCollection["txtCustomerCode"].Trim();
                        objSystemReturnItems.CompSeqNo = Convert.ToByte(systemReturnItems[6].Trim());  // Component Sequence Number
                        objSystemReturnItems.ItemCode = systemReturnItems[0].Trim();  // Item Code
                        objSystemReturnItems.SoldQuantity = Convert.ToDouble(systemReturnItems[1].Trim()); // Purchase Quantity/ Sold Quantity
                        objSystemReturnItems.ReturnQuantity = Convert.ToDouble(systemReturnItems[3].Trim()); // Return Item Quantity
                        objSystemReturnItems.ReturnInToStoreLocation = Convert.ToByte(systemReturnItems[7].Trim());  // Store Location
                        objSystemReturnItems.UnitCost = Convert.ToDecimal(systemReturnItems[4].Trim()); // Sold Item Depriciated Unit Cost
                        objSystemReturnItems.NonReturnItemsValue = Convert.ToDecimal(systemReturnItems[5].Trim()); // Non Return Items Cost

                        lstSystemReturnItems.Add(objSystemReturnItems);


                        if (!string.IsNullOrEmpty(systemReturnItems[8].Trim()))
                        {
                            objSystemReturnItemsWithSerialNo = new Sal_SystemReturnItemsWithSerialNo();
                            objSystemReturnItemsWithSerialNo.CustomerCode = fCollection["txtCustomerCode"].Trim();
                            objSystemReturnItemsWithSerialNo.CompSeqNo = Convert.ToByte(serialCounter);
                            objSystemReturnItemsWithSerialNo.ItemCode = systemReturnItems[0].Trim();  // Item Code
                            objSystemReturnItemsWithSerialNo.ItemSerialNo = systemReturnItems[8].Trim();  // Item Serial Number
                            objSystemReturnItemsWithSerialNo.Status = Helper.Active;

                            lstSystemReturnItemsWithSerialNo.Add(objSystemReturnItemsWithSerialNo);

                            if (!string.IsNullOrEmpty(serialTempTableRows))
                                serialTempTableRows += "," + "(" + objSystemReturnItems.ItemCode + ",'" + objSystemReturnItemsWithSerialNo.ItemSerialNo + "','" + systemReturnItems[9].Trim() + "'," + Convert.ToByte(systemReturnItems[7].Trim()) + ",'" + objLoginHelper.LocationCode + "','" + fCollection["txtCustomerCode"].Trim() + "')";
                            else
                                serialTempTableRows = "(" + objSystemReturnItems.ItemCode + ",'" + objSystemReturnItemsWithSerialNo.ItemSerialNo + "','" + systemReturnItems[9].Trim() + "'," + Convert.ToByte(systemReturnItems[7].Trim()) + ",'" + objLoginHelper.LocationCode + "','" + fCollection["txtCustomerCode"].Trim() + "')";

                            serialCounter++;
                        }
                    }
                }

                salesDal.SaveSystemReturn(objSystemReturn, lstSystemReturnItems, lstSystemReturnItemsWithSerialNo, objLoginHelper.LocationCode, serialTempTableRows);
                erpDal.CreateUserLog(Helper.ClientIPAddress(), string.Empty, objLoginHelper.LocationCode, objLoginHelper.LogInID, "System Return, Customer Code: " + fCollection["txtCustomerCode"].Trim());

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("System Return" + Helper.SuccessMessage) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public ActionResult CustomersOtherView()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "CustomersOtherView", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            List<RASolarERP.DomainModel.SalesModel.SystemReturnOrFullPaidCustomers> lstFullPaidSystemReturnCustomers = new List<RASolarERP.DomainModel.SalesModel.SystemReturnOrFullPaidCustomers>();

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForSales.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;
            ViewBag.fromDate = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewBag.toDate = DateTime.Now.ToString("dd-MMM-yyyy");

            return View(lstFullPaidSystemReturnCustomers);
        }

        //[HttpPost]
        //public ActionResult CustomersOtherView(FormCollection fCollection)
        //{
        //    objLoginHelper = (LoginHelper)Session["LogInInformation"];

        //    ViewBag.LocationTitle = objLoginHelper.LocationTitle;
        //    ViewBag.Location = objLoginHelper.Location;
        //    ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
        //    ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
        //    ViewBag.RegionTitle = objLoginHelper.RegionTitle;
        //    ViewBag.RegionName = objLoginHelper.LogInForRegionName;
        //    ViewBag.UnitTitle = objLoginHelper.UnitTitle;
        //    ViewBag.UnitName = objLoginHelper.LogInForUnitName;
        //    ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForSales.ToString("MMMM-yy");
        //    ViewBag.ModuleName = objLoginHelper.ModluleTitle;
        //    ViewBag.TopMenu = objLoginHelper.TopMenu;

        //    ViewBag.HideReportMode = true;

        //    ViewBag.fromDate = fCollection["txtStartDate"];
        //    ViewBag.toDate = fCollection["txtEndDate"];
        //    ViewBag.ReportMode = fCollection["ddlReportMode"];

        //    List<RASolarERP.DomainModel.SalesModel.SystemReturnOrFullPaidCustomers> lstFullPaidSystemReturnCustomers = new List<RASolarERP.DomainModel.SalesModel.SystemReturnOrFullPaidCustomers>();
        //    lstFullPaidSystemReturnCustomers = salesDal.ReadSystemReturnOrFullPaidCustomers(fCollection["ddlReportMode"], objLoginHelper.LocationCode, fCollection["txtStartDate"], fCollection["txtEndDate"]);

        //    return View(lstFullPaidSystemReturnCustomers);
        //}

        [GridAction(GridName = "gvFullPaidSystemReturnCustomers")]
        public ActionResult _CustomerOtherViewShow(string fromDate, string todate, string reportMode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForSales.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.HideReportMode = true;

            ViewBag.fromDate = Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy");
            ViewBag.toDate = Convert.ToDateTime(todate).ToString("dd-MMM-yyyy");
            ViewBag.ReportMode = reportMode;

             List<RASolarERP.DomainModel.SalesModel.SystemReturnOrFullPaidCustomers> lstFullPaidSystemReturnCustomers = new List<RASolarERP.DomainModel.SalesModel.SystemReturnOrFullPaidCustomers>();
             lstFullPaidSystemReturnCustomers = salesDal.ReadSystemReturnOrFullPaidCustomers(reportMode, objLoginHelper.LocationCode, fromDate, todate);

             return View(new GridModel<RASolarERP.DomainModel.SalesModel.SystemReturnOrFullPaidCustomers>
             {
                 Data = lstFullPaidSystemReturnCustomers
             });
        }

        public JsonResult LoadCustomerDetails(string customerCode, string returnDate)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            CustomerInfoNPackageDetailsForSystemReturn objSystemReturnInfo = new CustomerInfoNPackageDetailsForSystemReturn();

            //string customerCode = string.Empty;
            //int customerCodeLength = customerSerials.Length;

            //if (customerCodeLength < 9)
            //{
            //    StringBuilder sb = new StringBuilder();
            //    int remainLength = 9 - customerCodeLength - objLoginHelper.CustomerPrefix.Length;

            //    sb.Append(objLoginHelper.CustomerPrefix);
            //    sb.Append(customerSerials);
            //    sb.Append('-', remainLength);

            //    customerCode = sb.ToString();
            //}

            objSystemReturnInfo = salesDal.ReadSystemReturnInfo(customerCode, Convert.ToDateTime(Helper.DateFormatMMDDYYYY(returnDate.Trim())));

            if (objSystemReturnInfo != null)
            {
                ArrayList arr = new ArrayList();

                arr.Add(new
                {
                    CustomerCode = objSystemReturnInfo.CustomerCode,
                    CustomerName = objSystemReturnInfo.CustomerName,
                    CustomerAddress = objSystemReturnInfo.CustomerAddress,
                    PackageName = objSystemReturnInfo.PackageName,
                    AgreementDate = objSystemReturnInfo.AgreementDate.ToString("dd-MMM-yyyy"),
                    UsagesMonth = objSystemReturnInfo.UsagesMonth,
                    PackagePrice = objSystemReturnInfo.PackagePrice,
                    DownPaymentAmount = objSystemReturnInfo.DownPaymentAmount,
                    MonthlyServiceCharge = Convert.ToDecimal(objSystemReturnInfo.InstallmentSizeServiceCharge).ToString("0.00"),
                    Depreciation = Convert.ToDecimal(objSystemReturnInfo.UsageCharge).ToString("0"),
                    ReceivableServiceCharge = Convert.ToDecimal(objSystemReturnInfo.TotalServiceCHargeUpToCurrentMonth).ToString("0"),
                    TotalRecovered = Convert.ToDecimal(objSystemReturnInfo.TotalRecoverdDPWithInstallment).ToString("0")
                });

                return new JsonResult { Data = arr };
            }

            return new JsonResult { Data = "NoFound" };
        }

        [GridAction]
        public ActionResult __LoadPackageDetailsForSystemReturn(string customerCode, string returnDate)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (Session["CustomerCodeForSystemReturn"] != null)
            {
                customerCode = (string)Session["CustomerCodeForSystemReturn"];
                returnDate = objLoginHelper.TransactionOpenDate.Date.ToString("dd-MMM-yyyy");
                Session.Remove("CustomerCodeForSystemReturn");
            }

            return View(new GridModel<PackageDetailsForSystemReturn>
            {
                Data = salesDal.ReadPackageDetailsForSystemReturn(customerCode.Trim(), Convert.ToDateTime(Helper.DateFormatMMDDYYYY(returnDate.Trim())))
            });
        }

        //[GridAction]
        //public ActionResult __LoadSystemReturnOrFullPaid(string cs, string cm)
        //{
        //    objLoginHelper = (LoginHelper)Session["LogInInformation"];

        //    List<SystemReturnOrFullPaidCustomers> lstFullPaidSystemReturnCustomers = new List<SystemReturnOrFullPaidCustomers>();
        //    if (cm == "sr")
        //    {
        //        lstFullPaidSystemReturnCustomers = salesDal.ReadSystemReturnOrFullPaidCustomers(Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForSales), cs, Helper.SystemReturn);
        //    }
        //    else if (cm == "fp")
        //    {
        //        lstFullPaidSystemReturnCustomers = salesDal.ReadSystemReturnOrFullPaidCustomers(Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForSales), cs, Helper.FullPaiedCustomer);
        //    }
        //    else
        //    {
        //        lstFullPaidSystemReturnCustomers = salesDal.ReadSystemReturnOrFullPaidCustomers(Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForSales), objLoginHelper.LocationCode, Helper.FullPaiedCustomer);
        //    }

        //    return View(new GridModel<SystemReturnOrFullPaidCustomers>
        //    {
        //        Data = lstFullPaidSystemReturnCustomers
        //    });
        //}
    }
}
