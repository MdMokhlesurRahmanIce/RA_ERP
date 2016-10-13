using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using RASolarHelper;
using RASolarERP.Model;
using RASolarSecurity.Model;

using RASolarERP.Web.Areas.Sales.Models;
using Telerik.Web.Mvc;
using System.Collections;
using System.Text.RegularExpressions;
using System.Text;
using RASolarERP.DomainModel.SalesModel;
using RASolarERP.Web.Models;
using System.Web.Helpers;
using RASolarHRMS.Model;
using RASolarERP.Web.Areas.HRMS.Models;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;

namespace RASolarERP.Web.Areas.Sales.Controllers
{
    public class CustomerRegisterController : Controller
    {
        LoginHelper objLoginHelper = new LoginHelper();
        private SalesData salesDal = new SalesData();
        private SalesReportData salesReport = new SalesReportData();
        private RASolarERPData erpDal = new RASolarERPData();
        RASolarSecurityData securityDal = new RASolarSecurityData();
        HRMSData hrmDal = new HRMSData();
        string message = string.Empty;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UnitWiseCustomerRegister()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "UnitWiseCustomerRegister", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            //if (objLoginHelper.TransactionOpenDate == Helper.InvalidDate())
            //{
            //    Session["messageInformation"] = "Day Is Not Open";
            //    return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            //}

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Day Open Date: " + objLoginHelper.TransactionOpenDate.ToString("MMMM  dd, yyyy");
            //"Month: " + objLoginHelper.MonthOpenForSales.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.OpenDay = objLoginHelper.TransactionOpenDate.Date.ToString("dd-MMM-yyyy");
            ViewBag.OpenBackDay = objLoginHelper.TransactionBackDate.Date.ToString("dd-MMM-yyyy");

            ViewBag.CalenderDate = objLoginHelper.MonthOpenForSales;

            ViewBag.Zone = erpDal.Zone();
            ViewBag.Upazilla = salesDal.ReadUpazillaByUnitCode(objLoginHelper.LogInForUnitCode);

            List<UnitWiseCustomerRegisterReport> lstCustomerRegister = new List<UnitWiseCustomerRegisterReport>();

            try
            {
                lstCustomerRegister = salesDal.ReadUnitWiseCustomerRegisterReport(objLoginHelper.LogInForUnitCode);
            }

            catch(Exception ex)
            {
                ViewBag.CustomerFPRError = ex.InnerException.Message.ToString();
            }

            ViewBag.TotalCollectionInCurrentMonth = lstCustomerRegister.Sum(s => s.CollectionInCurrentMonthWithoutDP).ToString("0");

            ViewBag.TotalAdvance = ((decimal)lstCustomerRegister.Where(s => s.OverdueOrAdvanceBalance > 0).Sum(s => s.OverdueOrAdvanceBalance)).ToString("0");
            ViewBag.TotalOverdue = ((decimal)lstCustomerRegister.Where(s => s.OverdueOrAdvanceBalance < 0).Sum(s => s.OverdueOrAdvanceBalance * -1)).ToString("0");

            return View(lstCustomerRegister);
        }

        public ActionResult CustomerLedger(string customerCode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            decimal totalCollection;

            SalesDataProcess objSalesDataProcess = new SalesDataProcess();

            List<CustomerLedgerReport> lstCustomerLedger = new List<CustomerLedgerReport>();
            lstCustomerLedger = objSalesDataProcess.CustomerLedgerProcess(customerCode, out totalCollection);

            ViewBag.TotalCollection = totalCollection.ToString("0");

            return PartialView("CustomerLedger", lstCustomerLedger);
        }

        public ActionResult SystemReturnCustomer(string customerCode)
        {
            Session["CustomerCodeForSystemReturn"] = customerCode;
            return RedirectToAction("SystemReturn", "SystemReturns");
        }

        public JsonResult SaveCustomerCollection(FormCollection fCollection)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            int serial = 0;

            Sal_CollectionFromCustomersPrePost objCustomerCollection = new Sal_CollectionFromCustomersPrePost();

            try
            {
                serial = salesDal.CustomerCollectionSerial(fCollection["hfCustomerCodeForCollection"], Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForSales));

                if (!string.IsNullOrEmpty(fCollection["hfCollectionDelete"]))
                {

                    Sal_CollectionFromCustomers objCustomerCol = new Sal_CollectionFromCustomers();
                    objCustomerCol = salesDal.ReadCollectionFromCustomers(fCollection["hfCustomerCodeForCollection"], Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForSales), Convert.ToByte(fCollection["hfCollectionSerials"]));

                    if (objCustomerCol != null)
                    {

                        objCustomerCollection.CustomerCode = objCustomerCol.CustomerCode;
                        objCustomerCollection.YearMonth = objCustomerCol.YearMonth;
                        objCustomerCollection.SerialNo = objCustomerCol.SerialNo;

                        salesDal.CustomerCollectionDelete(objCustomerCollection);
                        return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Delete Successfully.") };
                    }

                }


                if (fCollection["hfCollectionSerials"] == "0")
                {
                    Sal_Customer objSalCustomerInfo = new Sal_Customer();

                    objCustomerCollection.CustomerCode = fCollection["hfCustomerCodeForCollection"];
                    objCustomerCollection.YearMonth = Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForSales);
                    objCustomerCollection.SerialNo = Convert.ToByte(serial);
                    objCustomerCollection.CollectionType = Helper.CollectionTypeIOC;
                    objCustomerCollection.CollectionAmount = Convert.ToDecimal(fCollection["txtCollectionAmount"]);
                    objCustomerCollection.CollectionAmount_Principal = Convert.ToDecimal(fCollection["CollectionAmount_Principal"]);
                    objCustomerCollection.CollectionAmount_ServiceCharge = Convert.ToDecimal(fCollection["CollectionAmount_ServiceCharge"]);
                    objCustomerCollection.CollectionDate = Convert.ToDateTime(Helper.DateFormatMMDDYYYY(fCollection["dtpCollectionDate"]));

                    if (!erpDal.IsCashMemoManagementEnabled(Helper.CompanyName))
                    {
                        objCustomerCollection.RefMemoNo = fCollection["txtMemoNumber"];
                    }
                    else
                    {
                        objCustomerCollection.CashMemoNo = fCollection["txtMemoNumber"];
                        objCustomerCollection.CashMemoUsesID = null; //Helper.CashMemuUsesIdFirst;

                        objCustomerCollection.RefAELocationCode = objLoginHelper.LocationCode;
                    }



                    objCustomerCollection.CollectedByEmployee = fCollection["hfCustomerFprPerson"].Trim();

                    objCustomerCollection.UserID = objLoginHelper.LogInID;
                    objCustomerCollection.EntryTime = DateTime.Now;

                    objSalCustomerInfo.CustomerCode = fCollection["hfCustomerCodeForCollection"];
                    objSalCustomerInfo.PhoneNo = fCollection["txtCustomerMobileNumber"];

                    objCustomerCollection = salesDal.SaveCustomerCollection(objCustomerCollection, objSalCustomerInfo);
                    erpDal.CreateUserLog(Helper.ClientIPAddress(), string.Empty, objLoginHelper.LocationCode, objLoginHelper.LogInID, "Customer Collection, Customer Code: " + fCollection["hfCustomerCodeForCollection"].Trim());
                }
                else
                {
                    Sal_CollectionFromCustomers objCustomerCol = new Sal_CollectionFromCustomers();
                    objCustomerCol = salesDal.ReadCollectionFromCustomers(fCollection["hfCustomerCodeForCollection"], Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForSales), Convert.ToByte(fCollection["hfCollectionSerials"]));

                    if (objCustomerCol != null)
                    {

                        objCustomerCollection.CustomerCode = objCustomerCol.CustomerCode;
                        objCustomerCollection.YearMonth = objCustomerCol.YearMonth;
                        objCustomerCollection.SerialNo = objCustomerCol.SerialNo;
                        objCustomerCollection.CollectionType = objCustomerCol.CollectionType;
                        objCustomerCollection.CollectionAmount = objCustomerCol.CollectionAmount;
                        objCustomerCollection.AdvanceAdjustmentAmount = objCustomerCol.AdvanceAdjustmentAmount;
                        objCustomerCollection.CollectionAmount_ServiceCharge = objCustomerCol.CollectionAmount_ServiceCharge;
                        objCustomerCollection.CollectionAmount_Principal = objCustomerCol.CollectionAmount_Principal;
                        objCustomerCollection.CollectionAmount_Advance = objCustomerCol.CollectionAmount_Advance;
                        objCustomerCollection.CollectionDate = objCustomerCol.CollectionDate;
                        objCustomerCollection.CashMemoNo = objCustomerCol.CashMemoNo;
                        objCustomerCollection.CashMemoUsesID = objCustomerCol.CashMemoUsesID;
                        objCustomerCollection.RefMemoNo = objCustomerCol.RefMemoNo;
                        objCustomerCollection.UserID = objCustomerCol.UserID;
                        objCustomerCollection.EntryTime = objCustomerCol.EntryTime;
                        objCustomerCollection.CollectionInUnit = objCustomerCol.CollectionInUnit;
                        objCustomerCollection.CollectedByEmployee = objCustomerCol.CollectedByEmployee;
                        objCustomerCollection.RefAELocationCode = objCustomerCol.RefAELocationCode;
                        objCustomerCollection.RefAETransDate = objCustomerCol.RefAETransDate;
                        objCustomerCollection.RefAEProjectCode = objCustomerCol.RefAEProjectCode;
                        objCustomerCollection.RefAETransNo = objCustomerCol.RefAETransNo;

                        objCustomerCollection.CollectionAmount = Convert.ToDecimal(fCollection["txtCollectionAmount"]);
                        objCustomerCollection.CollectionAmount_Principal = Convert.ToDecimal(fCollection["CollectionAmount_Principal"]);
                        objCustomerCollection.CollectionAmount_ServiceCharge = Convert.ToDecimal(fCollection["CollectionAmount_ServiceCharge"]);
                        objCustomerCollection.CollectionDate = Convert.ToDateTime(Helper.DateFormatMMDDYYYY(fCollection["dtpCollectionDate"]));

                        objCustomerCol.CollectionAmount = Convert.ToDecimal(fCollection["txtCollectionAmount"]);

                        if (!erpDal.IsCashMemoManagementEnabled(Helper.CompanyName))
                        {
                            objCustomerCollection.RefMemoNo = fCollection["txtMemoNumber"];
                        }
                        else
                        {
                            objCustomerCollection.CashMemoNo = fCollection["txtMemoNumber"];
                            objCustomerCollection.CashMemoUsesID = Helper.CashMemuUsesIdFirst;
                        }

                        objCustomerCollection = salesDal.UpdateCustomerCollection(objCustomerCollection, objCustomerCol);
                        erpDal.CreateUserLog(Helper.ClientIPAddress(), string.Empty, objLoginHelper.LocationCode, objLoginHelper.LogInID, "Customer Collection Edit, Customer Code: " + fCollection["hfCustomerCodeForCollection"].Trim());
                    }
                }

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public JsonResult UpdateCustomerStatus(string customerCode, string type)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            Sal_Customer objCustomer = new Sal_Customer();

            try
            {
                objCustomer = salesDal.ReadCustomer(customerCode);

                if (objCustomer != null)
                {
                    if (type == "FP")
                        objCustomer.Status = Helper.FullPaiedCustomer;
                    else if (type == "SR")
                        objCustomer.Status = Helper.SystemReturnCustomer;

                    objCustomer.StatusChangedDate = objLoginHelper.TransactionOpenDate;
                }

                objCustomer = salesDal.UpdateCustomer(objCustomer);

                if (objCustomer != null)
                {
                    if (type == "FP")
                        return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Full Paid" + Helper.SuccessMessage) };
                    else if (type == "SR")
                        return new JsonResult { Data = ExceptionHelper.ExceptionMessage("System Return" + Helper.SuccessMessage) };
                }
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }

            return new JsonResult { Data = "" };
        }

        [GridAction]
        public ActionResult __CutomerCollectionDetails(string customerCode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<CustomerCollectionDetails> lstCustomerCollection = new List<CustomerCollectionDetails>();
            lstCustomerCollection = salesDal.ReadCollectionFromCustomers(customerCode, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForSales));

            if (lstCustomerCollection.Count > 0)
            {
                string employeeName = string.Empty;
                Hrm_EmployeeInfo objEmployeeInfo = hrmDal.ReadEmployeeInfo(lstCustomerCollection[0].CollectedByEmployeeID);

                employeeName = objEmployeeInfo != null ? objEmployeeInfo.EmployeeName + " [" + lstCustomerCollection[0].CollectedByEmployeeID + "]" : string.Empty;

                var v = from ss in lstCustomerCollection
                        select new CustomerCollectionDetails
                        {
                            SerialNo = ss.SerialNo,
                            CollectionDate = ss.CollectionDate,
                            RefMemoNo = ss.RefMemoNo ?? ss.CashMemoNo,
                            CollectionAmount = ss.CollectionAmount,
                            CollectedByEmployeeID = ss.CollectedByEmployeeID,
                            CollectedByEmployeeName = employeeName
                        };

                lstCustomerCollection = v.ToList();
            }

            return View(new GridModel<CustomerCollectionDetails>
            {
                Data = lstCustomerCollection
            });
        }

        public ActionResult CustomerTransfer()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "CustomerTransfer", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
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
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;
            ViewBag.CurrentDay = objLoginHelper.MonthOpenForSales.Date.ToString("dd-MMM-yyyy");

            ViewBag.CalenderDate = objLoginHelper.MonthOpenForSales;

            return View();
        }

        public JsonResult TransferCustomer(string customerID, string customerTransferDate, string transferToLocation, string transferNote)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            try
            {
                salesDal.CustomerTransferV2(customerID, objLoginHelper.LogInForUnitCode, "TRANSFER", transferToLocation.Trim(), customerTransferDate, transferNote, objLoginHelper.LogInID, "INSERT");
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Transferred" + Helper.SuccessMessage) };

            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public JsonResult UpdateCustomerInfo(string customerID, string fatherHusbandName, string customerName, string villageName, string  postOfficeName, string upzillCode, string unionCode,string districtCode, string mobileNumber, string remarksNotes)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            Sal_Customer objCustomer = new Sal_Customer();

            try
            {
                objCustomer.CustomerCode = customerID;
                objCustomer.CustomerName = customerName;
                objCustomer.FathersOrHusbandName = fatherHusbandName;
                objCustomer.Village = villageName;
                objCustomer.PostOffice = postOfficeName;
                objCustomer.ThanaID = upzillCode;
                objCustomer.UnionID = unionCode;
                objCustomer.DistrictCode = districtCode;
                objCustomer.PhoneNo = mobileNumber;
                objCustomer.ModifiedBy = objLoginHelper.LogInID;
                objCustomer.ModifiedOn = objLoginHelper.CurrentDate;


                objCustomer = salesDal.UpdateCustomer(objCustomer, remarksNotes);

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Customer Information" + Helper.SuccessMessage) };

            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public ActionResult UnitWiseCustomerReceive()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "UnitWiseCustomerReceive", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
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
            ViewBag.OpenMonthYear = "Day Open Date: " + objLoginHelper.TransactionOpenDate.ToString("MMMM  dd, yyyy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;
            ViewBag.CurrentDay = objLoginHelper.MonthOpenForSales.Date.ToString("dd-MMM-yyyy");

            ViewBag.CalenderDate = objLoginHelper.MonthOpenForSales;
            List<GetCustomerTransferredButNotYetAccepted> lstCustomerTransferredButNotYetAccepted = new List<GetCustomerTransferredButNotYetAccepted>();
            lstCustomerTransferredButNotYetAccepted = salesDal.ReadCustomerTransferredButNotYetAccepted(objLoginHelper.LogInForUnitCode).ToList();
            TempData["CustomerTransferredButNotYetAccepted"] = lstCustomerTransferredButNotYetAccepted;

            return View();
        }

        public JsonResult CustomerAccept(string customerID, string AcceptOrRejectDate)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            try
            {
                salesDal.CustomerTransferV2(customerID, objLoginHelper.LogInForUnitCode, "ACCEPT", "", AcceptOrRejectDate, "", objLoginHelper.LogInID, "INSERT");
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Customer Accept" + Helper.SuccessMessage) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }

        }

        public JsonResult CustomerReject(string customerID, string AcceptOrRejectDate)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            try
            {
                salesDal.CustomerTransferV2(customerID, objLoginHelper.LogInForUnitCode, "CANCEL", "", AcceptOrRejectDate, "", objLoginHelper.LogInID, "INSERT");
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Customer Reject" + Helper.SuccessMessage) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public ActionResult CustomerWiseFPREntry()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "CustomerWiseFPREntry", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
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
            ViewBag.OpenMonthYear = "Month: " + (DateTime.Now.Date).ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;
            ViewBag.CurrentDay = objLoginHelper.MonthOpenForSales.Date.ToString("dd-MMM-yyyy");
            ViewBag.CalenderDate = objLoginHelper.MonthOpenForSales;
            ViewBag.Zone = erpDal.Zone();

            CustomerWiseFPR objCustomerWiseFPR = new CustomerWiseFPR();
            objCustomerWiseFPR = salesDal.GetCustomerFPRAndScheduledCollectionEntry(objLoginHelper.LocationCode, string.Empty, string.Empty, string.Empty, false);

            return View(objCustomerWiseFPR);
        }

        [HttpPost]
        public ActionResult CustomerWiseFPREntry(string btnSubmit, CustomerWiseFPR objCustomerFPR)
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
            ViewBag.CurrentDay = objLoginHelper.MonthOpenForSales.Date.ToString("dd-MMM-yyyy");
            ViewBag.CalenderDate = objLoginHelper.MonthOpenForSales;
            ViewBag.Zone = erpDal.Zone();

            string searchType = string.Empty;

            switch (btnSubmit)
            {
                case "Customer FPR Missing":
                    searchType = "FPR";
                    break;
                case "Collection Day Missing":
                    searchType = "DAY";
                    break;

                case "Show All Customer":
                    searchType = "ALL";
                    break;

                case "Show":
                    searchType = "EMPWCUS";
                    break;
            }

            CustomerWiseFPR objCustomerWiseFPR = new CustomerWiseFPR();

            try
            {
                objCustomerWiseFPR = salesDal.GetCustomerFPRAndScheduledCollectionEntry(objLoginHelper.LocationCode, searchType, objCustomerFPR.EmployeeID, objCustomerFPR.ScheduledCollectionDay.ToString(), true);

                if (searchType == "FPR")
                {
                    objCustomerWiseFPR.CustomerFPRMissing = objCustomerWiseFPR.CustomerFPRAndScheduledCollection.Where(fpr => fpr.EmployeeAsFPR.Trim() == "" || fpr.EmployeeAsFPR.Trim() == "0").Count();
                }
                else if (searchType == "DAY")
                {
                    objCustomerWiseFPR.CollectionDayMissing = objCustomerWiseFPR.CustomerFPRAndScheduledCollection.Where(day => day.ScheduledCollectionDay == 0).Count();
                }
                else if (searchType == "ALL")
                {
                    objCustomerWiseFPR.MissingAll = objCustomerWiseFPR.CustomerFPRAndScheduledCollection.Count;
                }
            }
            catch (Exception ex)
            {
                Session["messageInformation"] = ex.Message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            return View(objCustomerWiseFPR);
        }

        public JsonResult SaveFPRInfo(List<CustomerFPRAndScheduledCollectionEntry> lstCustomerFPREntry)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            try
            {
                lstCustomerFPREntry.RemoveAt(0);
                salesDal.UpdateFPR(lstCustomerFPREntry, objLoginHelper.LocationCode);
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public ActionResult UnitWiseEmployeeAcceptReject()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "UnitWiseEmployeeAcceptReject", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
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
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;
            ViewBag.CurrentDay = objLoginHelper.MonthOpenForSales.Date.ToString("dd-MMM-yyyy");

            ViewBag.CalenderDate = objLoginHelper.MonthOpenForSales;
            TempData["PeriodOpenClose"] = Convert.ToDateTime(objLoginHelper.MonthOpenForSales).ToString("dddd,  MMMM  dd,  yyyy");

            List<GetCustomerTransferredButNotYetAccepted> lstCustomerTransferredButNotYetAccepted = new List<GetCustomerTransferredButNotYetAccepted>();
            lstCustomerTransferredButNotYetAccepted = salesDal.ReadCustomerTransferredButNotYetAccepted(objLoginHelper.LogInForUnitCode).ToList();
            TempData["CustomerTransferredButNotYetAccepted"] = lstCustomerTransferredButNotYetAccepted;

            return View();
        }

        public ActionResult CustomerTrainingEntry()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "CustomerTrainingEntry", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
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
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForAccounting.ToString("MMMM-yy");

            List<GetUnitWiseCustomerTrainingSchedule> ss = new List<GetUnitWiseCustomerTrainingSchedule>();
            ss = salesDal.ReadCustomerTrainingSchedule(objLoginHelper.LocationCode, null, null);
            ss = (ss.GroupBy(g => g.ScheduledDate).Select(s => s.First())).ToList();

            ViewBag.UnitWiseCustomerTrainingSchedule = ss;

            return View();
        }

        [GridAction]
        public ActionResult __CustomerTraining(string trainingStatus, string trainingDate, string trainingBatchNo)
        {
            bool trainedOrNonTrained = false;
            byte trainingBatchNumber;

            if (Convert.ToInt32(trainingStatus) == 1)
            {
                trainedOrNonTrained = true;
            }
            if (!string.IsNullOrEmpty(trainingBatchNo))
            {
                trainingBatchNumber = Convert.ToByte(trainingBatchNo);
            }
            else
            {
                trainingBatchNumber = Convert.ToByte(0);
            }

            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            return View(new GridModel<CustomerTrainingInfo> { Data = salesDal.ReadCustomerTrainingInfo(objLoginHelper.LocationCode, trainedOrNonTrained, trainingBatchNumber) });
        }

        public ActionResult CustomerTraineeInformation(string trainerId)
        {
            try
            {
                string traineeName = salesDal.ReadCustomerTraineeName(trainerId);

                return Json(new
                {
                    Success = true, //error
                    TraineeName = traineeName  //return exception
                });

            }


            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public ActionResult CustomerTrainingInformationSave(string selectedCustomerForTraining, string trainingStatus, string trainingDate, string numberOfParticipant, string traineeId, string entertainmentExpence, string totalTrainingAllowance, string trainingBatchNo)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "CustomerTrainingInformationSave", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            Sal_CustomerTrainingTransMaster objSalCustomerTrainingMaster = new Sal_CustomerTrainingTransMaster();
            Sal_CustomerTrainingTransDetails objCustomerTrainingTransDetails;
            List<Sal_CustomerTrainingTransDetails> lstCustomerTrainingTransDetails = new List<Sal_CustomerTrainingTransDetails>();

            string[] customerForTraining = selectedCustomerForTraining.Split('#');
            int customerTotal = customerForTraining.Length, customerCount = 0;

            try
            {
                objSalCustomerTrainingMaster.UnitCode = objLoginHelper.LocationCode;
                objSalCustomerTrainingMaster.TrainingDate = Convert.ToDateTime(trainingDate.Trim()).Date;
                objSalCustomerTrainingMaster.TrainingBatchNo = Convert.ToByte(trainingBatchNo);
                objSalCustomerTrainingMaster.RefScheduledDate = Convert.ToDateTime(trainingDate.Trim()).Date;
                objSalCustomerTrainingMaster.RefScheduledBatchNo = Convert.ToByte(trainingBatchNo);
                objSalCustomerTrainingMaster.NoOfParticipantAttend = Convert.ToByte(numberOfParticipant.Trim());
                objSalCustomerTrainingMaster.TrainerEmployeeID = traineeId.ToString();
                objSalCustomerTrainingMaster.TotalTrainingAllowance = Convert.ToDecimal(totalTrainingAllowance);
                objSalCustomerTrainingMaster.TotalEntertainmentExp = Convert.ToDecimal(entertainmentExpence);
                objSalCustomerTrainingMaster.CreatedBy = objLoginHelper.LogInID;
                objSalCustomerTrainingMaster.CreatedOn = DateTime.Now;
                objSalCustomerTrainingMaster.Status = Helper.Active;


                for (customerCount = 0; customerCount < customerTotal; customerCount++)
                {
                    objCustomerTrainingTransDetails = new Sal_CustomerTrainingTransDetails();
                    objCustomerTrainingTransDetails.UnitCode = objLoginHelper.LocationCode;
                    objCustomerTrainingTransDetails.TrainingDate = Convert.ToDateTime(trainingDate.Trim()).Date;
                    objCustomerTrainingTransDetails.TrainingBatchNo = Convert.ToByte(trainingBatchNo);
                    objCustomerTrainingTransDetails.CustomerCode = customerForTraining[customerCount].Trim();
                    objCustomerTrainingTransDetails.CustomerTrainingDate = Convert.ToDateTime(trainingDate.Trim()).Date;

                    lstCustomerTrainingTransDetails.Add(objCustomerTrainingTransDetails);
                }

                salesDal.Create(objSalCustomerTrainingMaster, lstCustomerTrainingTransDetails);

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Customer Training Data" + Helper.SuccessMessage) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public ActionResult OtherAmountCollectionFromCustomer()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "OtherAmountCollectionFromCustomer", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            if (objLoginHelper.TransactionOpenDate == Helper.InvalidDate())
            {
                Session["messageInformation"] = "Day Is Not Open";
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
            ViewBag.OpenMonthYear = "Day Open Date: " + objLoginHelper.TransactionOpenDate.ToString("MMMM  dd, yyyy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;
            //ViewBag.AgreementDate = string.Empty;
            //ViewBag.UsedPeriod = string.Empty;

            ViewBag.TransactionOpenDate = objLoginHelper.TransactionOpenDate.ToString("dd-MMM-yyyy");
            ViewBag.OpenDay = Helper.DateTo(objLoginHelper.TransactionOpenDate.Date.ToString("yyyyMM")).ToString("dd-MMM-yyyy");
            ViewBag.OpenBackDay = Helper.DateFrom(objLoginHelper.TransactionOpenDate.Date.ToString("yyyyMM")).ToString("dd-MMM-yyyy");

            return View();
        }

        [GridAction]
        public ActionResult __CustomerDisasterRecoveryList(string collectionType, string customerStatus, string collectionDate)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            List<CustomerDisasterRecoveryDetails> ss = salesDal.ReadCustomerDisasterRecoveryList(collectionType, Convert.ToByte(customerStatus), Convert.ToDateTime(collectionDate), objLoginHelper.LocationCode);

            return View(new GridModel<CustomerDisasterRecoveryDetails> { Data = ss });
        }

        public ActionResult FundCollectionCustomerDetails(string collectionType, string customerCode, string collectionDate, string customerStatus)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<CustomerLedgerReport> lstCustomerLedger = new List<CustomerLedgerReport>();
            lstCustomerLedger = salesDal.ReadCustomerLedgerReport(customerCode);
            decimal DRFAmount = salesDal.ReadCustomerDetailsForDRFACollection(collectionType, customerCode, objLoginHelper.LocationCode, Convert.ToDateTime(collectionDate), Convert.ToByte(customerStatus));
            string customerAddress = salesDal.ReadSystemReturnInfo(customerCode, DateTime.Now.Date).CustomerAddress;

            CustomerLedgerReport objCustomerLadger = new CustomerLedgerReport();
            List<CustomerLedgerReport> lstCustomerLedgerForDRF = new List<CustomerLedgerReport>();
            lstCustomerLedgerForDRF.Add(lstCustomerLedger[0]);

            objCustomerLadger.PhoneNo = customerAddress;
            objCustomerLadger.AmountAfterDiscount = DRFAmount;
            objCustomerLadger.Package = Helper.CalculateDateDifferanceYearMonthDay(lstCustomerLedger[0].AgreementDate, Convert.ToDateTime(collectionDate));
            objCustomerLadger.BatterySerialNo = lstCustomerLedger[0].AgreementDate.ToString("dd-MMM-yyyy");

            lstCustomerLedgerForDRF.Add(objCustomerLadger);

            return Json(lstCustomerLedgerForDRF, JsonRequestBehavior.AllowGet);
        }

        public JsonResult OtherAmountCollectionFromCustomerSave(string collectionType, string customerCode, string customerStatus, string DRFAmount, string collectionDate, string memoNumber, string customerFprPersonCode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            int collectionSerial = 0;

            Sal_CollectionFromCustomersPrePost objCustomerCollection = new Sal_CollectionFromCustomersPrePost();
            Sal_Customer objSalCustomerInfo = new Sal_Customer();

            collectionSerial = salesDal.CustomerCollectionSerial(customerCode, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForSales), collectionType);

            if (collectionSerial > 1)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("DRF Amount Already Collected For This Customer.") };
            }

            collectionSerial = salesDal.CustomerCollectionSerial(customerCode, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForSales));

            try
            {
                objCustomerCollection.CustomerCode = customerCode;
                objCustomerCollection.YearMonth = Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForSales);
                objCustomerCollection.SerialNo = Convert.ToByte(collectionSerial);
                objCustomerCollection.CollectionType = collectionType.Trim();
                objCustomerCollection.CollectionAmount = Convert.ToDecimal(DRFAmount);
                objCustomerCollection.CollectionAmount_Principal = 0;
                objCustomerCollection.CollectionAmount_ServiceCharge = 0;
                objCustomerCollection.CollectionDate = objLoginHelper.TransactionOpenDate; //Convert.ToDateTime(Helper.DateFormatMMDDYYYY(dRFDate));
                objCustomerCollection.RefAELocationCode = objLoginHelper.LocationCode;

                if (!string.IsNullOrEmpty(customerFprPersonCode))
                    objCustomerCollection.CollectedByEmployee = customerFprPersonCode;

                if (!erpDal.IsCashMemoManagementEnabled(Helper.CompanyName))
                {
                    objCustomerCollection.RefMemoNo = memoNumber.Trim();
                }
                else
                {
                    objCustomerCollection.CashMemoNo = memoNumber.Trim();
                    objCustomerCollection.CashMemoUsesID = Helper.CashMemuUsesIdFirst;
                }

                // objCustomerCollection.CollectionInUnit = objLoginHelper.LocationCode;
                objCustomerCollection.UserID = objLoginHelper.LogInID;
                objCustomerCollection.EntryTime = DateTime.Now;

                objCustomerCollection = salesDal.SaveCustomerCollection(objCustomerCollection, objSalCustomerInfo);

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public string locationCode { get; set; }

        public ActionResult ExportToPdfCustomerRegister(int page, string groupby, string orderBy, string filter)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<UnitWiseCustomerRegisterReport> lstCustomerRegister = new List<UnitWiseCustomerRegisterReport>();
            lstCustomerRegister = salesDal.ReadUnitWiseCustomerRegisterReport(objLoginHelper.LogInForUnitCode);

            Document pdfDocumnet = new Document(PageSize.A4, 35, 20, 15, 10);
            MemoryStream memStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(pdfDocumnet, memStream);

            StringBuilder headerStringCompanyTitle = new StringBuilder();
            StringBuilder headerStringReportTitle = new StringBuilder();
            headerStringCompanyTitle.AppendFormat(Helper.RSFAddress);
            headerStringReportTitle.AppendFormat("Customer Register Report \n For Month: {0} \n Unit: {1}", objLoginHelper.TransactionOpenDate.ToString("MMMM  dd, yyyy"), objLoginHelper.LogInForUnitName);

            Chunk chunkCompanyTitle = new Chunk(headerStringCompanyTitle.ToString(), FontFactory.GetFont(FontFactory.COURIER, 10, Font.BOLD, new Color(0, 0, 0)));
            Chunk chunkReportTitle = new Chunk(headerStringReportTitle.ToString(), FontFactory.GetFont(FontFactory.COURIER, 10, Font.NORMAL, new Color(0, 0, 0)));

            HeaderFooter header = new HeaderFooter(new Phrase(chunkCompanyTitle), false);
            header.After = new Phrase(chunkReportTitle);
            header.Alignment = Element.ALIGN_CENTER;
            header.BorderWidthTop = 0;
            header.BorderColorBottom = new Color(166, 165, 165);

            HeaderFooter footer = new HeaderFooter(new Phrase("-"), new Phrase("-"));
            footer.Alignment = Element.ALIGN_CENTER;
            footer.Border = 0;
            footer.BorderColor = new Color(230, 230, 230);
            footer.BorderWidthTop = .25F;

            pdfDocumnet.Header = header;
            pdfDocumnet.Footer = footer;

            pdfDocumnet.Open();

            int numOfColumns = 6;
            float[] widths = new float[] { 14f, 35f, 14f, 12f, 12f, 13f };

            PdfPTable pdfTable = new PdfPTable(numOfColumns);
            pdfTable.SetWidths(widths);
            pdfTable.WidthPercentage = 100;
            pdfTable.DefaultCell.Padding = 3;
            pdfTable.DefaultCell.BorderWidth = 0.5F;
            pdfTable.DefaultCell.BorderColor = new Color(221, 218, 218);
            pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfTable.DefaultCell.BackgroundColor = new Color(235, 235, 235);

            Font fontForTableHeader = new Font(Font.COURIER, 10, Font.BOLD, new Color(0, 0, 0));
            pdfTable.AddCell(new Phrase("Customer ID", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Customer Name", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Agreement Date", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Collection In Current Month", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Overdue or Advance", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Outstanding Balance", fontForTableHeader));

            pdfTable.HeaderRows = 1;
            pdfTable.DefaultCell.BorderWidth = 0.5F;
            pdfTable.DefaultCell.BorderColor = new Color(195, 195, 195);
            pdfTable.DefaultCell.BackgroundColor = new Color(255, 255, 255);

            Font fontForContent = new Font(Font.COURIER, 8, Font.NORMAL, new Color(0, 0, 0));

            foreach (UnitWiseCustomerRegisterReport cr in lstCustomerRegister)
            {
                pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.AddCell(new Phrase(cr.CustomerCode, fontForContent));
                pdfTable.AddCell(new Phrase(cr.CustomerName, fontForContent));
                pdfTable.AddCell(new Phrase(cr.AgreementDate.ToString("dd-MMM-yyyy"), fontForContent));

                pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                pdfTable.AddCell(new Phrase(cr.CollectionInCurrentMonthWithoutDP.ToString("#,##0"), fontForContent));
                pdfTable.AddCell(new Phrase(((decimal)cr.OverdueOrAdvanceBalance).ToString("#,##0"), fontForContent));
                pdfTable.AddCell(new Phrase(((decimal)cr.OutstandingBalance).ToString("#,##0"), fontForContent));
            }

            pdfDocumnet.Add(pdfTable);
            pdfDocumnet.Close();

            return File(memStream.ToArray(), "application/pdf", "RSF-CustomerRegister.pdf");
        }

        public ActionResult ExportToPdfCustomerLedger(int page, string groupby, string orderBy, string filter, string customerCode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            SalesDataProcess objSalesDataProcess = new SalesDataProcess();

            decimal totalCollection;
            List<CustomerLedgerReport> lstCustomerLedger = new List<CustomerLedgerReport>();
            lstCustomerLedger = objSalesDataProcess.CustomerLedgerProcess(customerCode, out totalCollection);

            Document pdfDocumnet = new Document(PageSize.A4, 35, 20, 15, 10);
            MemoryStream memStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(pdfDocumnet, memStream);

            StringBuilder headerStringCompanyTitle = new StringBuilder();
            StringBuilder headerStringReportTitle = new StringBuilder();
            headerStringCompanyTitle.AppendFormat(Helper.RSFAddress);
            headerStringReportTitle.AppendFormat("Customer Ledger Report \n For Month: {0} \n Unit: {1}", objLoginHelper.TransactionOpenDate.ToString("MMMM  dd, yyyy"), objLoginHelper.LogInForUnitName);

            Chunk chunkCompanyTitle = new Chunk(headerStringCompanyTitle.ToString(), FontFactory.GetFont(FontFactory.COURIER, 10, Font.BOLD, new Color(0, 0, 0)));
            Chunk chunkReportTitle = new Chunk(headerStringReportTitle.ToString(), FontFactory.GetFont(FontFactory.COURIER, 10, Font.NORMAL, new Color(0, 0, 0)));

            HeaderFooter header = new HeaderFooter(new Phrase(chunkCompanyTitle), false);
            header.After = new Phrase(chunkReportTitle);
            header.Alignment = Element.ALIGN_CENTER;
            header.BorderWidthTop = 0;
            header.BorderColorBottom = new Color(166, 165, 165);

            HeaderFooter footer = new HeaderFooter(new Phrase("-"), new Phrase("-"));
            footer.Alignment = Element.ALIGN_CENTER;
            footer.Border = 0;
            footer.BorderColor = new Color(230, 230, 230);
            footer.BorderWidthTop = .25F;

            pdfDocumnet.Header = header;
            pdfDocumnet.Footer = footer;

            pdfDocumnet.Open();

            CustomerLedgerReport objCustomerLedger = new CustomerLedgerReport();
            objCustomerLedger = lstCustomerLedger.LastOrDefault();

            int numOfColumns1 = 4;
            PdfPTable pdfTable1 = new PdfPTable(numOfColumns1);
            pdfTable1.WidthPercentage = 100;
            pdfTable1.DefaultCell.Padding = 3;
            //pdfTable1.DefaultCell.BorderWidth = 0.5F;
            pdfTable1.DefaultCell.BorderColor = new Color(255, 255, 255);
            pdfTable1.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfTable1.DefaultCell.BackgroundColor = new Color(255, 255, 255);
            Font fontForContent = new Font(Font.COURIER, 8, Font.NORMAL, new Color(0, 0, 0));

            //1
            pdfTable1.AddCell(new Phrase("Customer Code:", fontForContent));
            pdfTable1.AddCell(new Phrase(objCustomerLedger.CustomerCode, fontForContent));
            pdfTable1.AddCell(new Phrase("Discount Amount( 0%):", fontForContent));
            pdfTable1.AddCell(new Phrase(objCustomerLedger.DiscountAmount.ToString("0"), fontForContent));

            //2
            pdfTable1.AddCell(new Phrase("Customer Name:", fontForContent));
            pdfTable1.AddCell(new Phrase(objCustomerLedger.CustomerName, fontForContent));
            pdfTable1.AddCell(new Phrase("Amount After Discount:", fontForContent));
            pdfTable1.AddCell(new Phrase(objCustomerLedger.AmountAfterDiscount.ToString("0"), fontForContent));

            //3
            pdfTable1.AddCell(new Phrase("Customer Type:", fontForContent));
            pdfTable1.AddCell(new Phrase(objCustomerLedger.CustomerType, fontForContent));
            pdfTable1.AddCell(new Phrase("DP Amount( 15%):", fontForContent));
            pdfTable1.AddCell(new Phrase(objCustomerLedger.DownPaymentAmount.ToString("0"), fontForContent));

            //4
            pdfTable1.AddCell(new Phrase("Phone/Mobile:", fontForContent));
            pdfTable1.AddCell(new Phrase(objCustomerLedger.PhoneNo, fontForContent));
            pdfTable1.AddCell(new Phrase("Loan Amount:", fontForContent));
            pdfTable1.AddCell(new Phrase(objCustomerLedger.TotalPrincipalReceivable.ToString("0"), fontForContent));

            //5
            pdfTable1.AddCell(new Phrase("Agreement Date:", fontForContent));
            pdfTable1.AddCell(new Phrase(objCustomerLedger.AgreementDate.ToString("dd-MMM-yyyy"), fontForContent));
            pdfTable1.AddCell(new Phrase("Total Service Charge( 8%): ", fontForContent));
            pdfTable1.AddCell(new Phrase(objCustomerLedger.TotalServiceChargeReceivable.ToString("0"), fontForContent));

            //6      
            pdfTable1.AddCell(new Phrase("Package:", fontForContent));
            pdfTable1.AddCell(new Phrase(objCustomerLedger.Package, fontForContent));
            pdfTable1.AddCell(new Phrase("Installment Size/Amount:", fontForContent));
            pdfTable1.AddCell(new Phrase(objCustomerLedger.InstallmentSize.ToString("0.00"), fontForContent));

            //7
            pdfTable1.AddCell(new Phrase("Payment Mode:", fontForContent));
            pdfTable1.AddCell(new Phrase(objCustomerLedger.PaymentMode, fontForContent));
            pdfTable1.AddCell(new Phrase("", fontForContent));
            pdfTable1.AddCell(new Phrase("", fontForContent));
            //pdfTable1.AddCell(new Phrase("Panel Serial No.:", fontForContent));
            //pdfTable1.AddCell(new Phrase(objCustomerLedger.PanelSerialNo, fontForContent));

            //8
            pdfTable1.AddCell(new Phrase("Package Price:", fontForContent));
            pdfTable1.AddCell(new Phrase(objCustomerLedger.PackagePrice.ToString("0"), fontForContent));
            pdfTable1.AddCell(new Phrase("", fontForContent));
            pdfTable1.AddCell(new Phrase("", fontForContent));
            //pdfTable1.AddCell(new Phrase("Battery Serial No.:", fontForContent));
            //pdfTable1.AddCell(new Phrase(objCustomerLedger.BatterySerialNo, fontForContent));

            //9
            pdfTable1.AddCell("");
            pdfTable1.AddCell("");
            pdfTable1.AddCell("");
            pdfTable1.AddCell("");

            int numOfColumns = 5;
            float[] widths = new float[] { 40f, 15f, 15f, 15f, 15f };

            PdfPTable pdfTable = new PdfPTable(numOfColumns);
            pdfTable.SetWidths(widths);
            pdfTable.WidthPercentage = 100;
            pdfTable.DefaultCell.Padding = 3;
            pdfTable.DefaultCell.BorderWidth = 0.5F;
            pdfTable.DefaultCell.BorderColor = new Color(221, 218, 218);
            pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfTable.DefaultCell.BackgroundColor = new Color(235, 235, 235);

            Font fontForTableHeader = new Font(Font.COURIER, 10, Font.BOLD, new Color(0, 0, 0));
            pdfTable.AddCell(new Phrase("Collection Type", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Memo No", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Date", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Amount", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Outstanding", fontForTableHeader));

            pdfTable.HeaderRows = 1;
            pdfTable.DefaultCell.BorderWidth = 0.5F;
            pdfTable.DefaultCell.BorderColor = new Color(195, 195, 195);
            pdfTable.DefaultCell.BackgroundColor = new Color(255, 255, 255);

            int counter = 0;
            foreach (CustomerLedgerReport cl in lstCustomerLedger)
            {
                pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.AddCell(new Phrase(cl.CollectionType, fontForContent));
                pdfTable.AddCell(new Phrase(cl.RefMemoNo, fontForContent));

                if (counter == 0)
                {
                    pdfTable.AddCell(new Phrase(string.Empty, fontForContent));
                    counter = 1;
                }
                else
                    pdfTable.AddCell(new Phrase(((DateTime)cl.CollectionDate).ToString("dd-MMM-yyyy"), fontForContent));

                pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                pdfTable.AddCell(new Phrase(((decimal)cl.CollectionAmount).ToString("#,##0"), fontForContent));
                pdfTable.AddCell(new Phrase(((decimal)cl.TotalPrincipalPlusServiceChargeReceivable).ToString("#,##0"), fontForContent));
            }

            Paragraph overdueAdvance = new Paragraph("Overdue/Advance:" + ((decimal)objCustomerLedger.OverdueOrAdvanceBalance).ToString("0"), fontForContent);
            Paragraph outstandingBalance = new Paragraph("Outstanding Balance:" + ((decimal)objCustomerLedger.OutstandingBalance).ToString("0"), fontForContent);
            Paragraph currentStatus = new Paragraph("Current Status:" + objCustomerLedger.Status, fontForContent);

            pdfDocumnet.Add(pdfTable1);
            pdfDocumnet.Add(pdfTable);
            pdfDocumnet.Add(overdueAdvance);
            pdfDocumnet.Add(outstandingBalance);
            pdfDocumnet.Add(currentStatus);

            pdfDocumnet.Close();

            return File(memStream.ToArray(), "application/pdf", "RSF-CustomerLedger.pdf");

        }

        public ActionResult ExportToPdfAllCustomerLedger(int page, string groupby, string orderBy, string filter, string customerCode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            decimal totalCollection;

            List<UnitWiseCustomerLedger> lstCustomerLedgerProcessed = new List<UnitWiseCustomerLedger>();
            List<UnitWiseCustomerLedger> lstCustomerLedger = new List<UnitWiseCustomerLedger>();
            List<UnitWiseCustomerLedger> lstIndividualCustomer = new List<UnitWiseCustomerLedger>();

            lstCustomerLedger = salesReport.ReadUnitWiseCustomerLedger("FORIDCOL", objLoginHelper.LocationCode, "", "");
            lstIndividualCustomer = lstCustomerLedger.GroupBy(g => g.CustomerCode).Select(s => s.First()).ToList();

            Document pdfDocumnet = new Document(PageSize.A4, 35, 20, 15, 10);
            MemoryStream memStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(pdfDocumnet, memStream);

            StringBuilder headerStringCompanyTitle = new StringBuilder();
            StringBuilder headerStringReportTitle = new StringBuilder();
            headerStringCompanyTitle.AppendFormat(Helper.RSFAddress);
            headerStringReportTitle.AppendFormat("Customer Ledger Report (For All Active Customers) \n Unit: {0}", objLoginHelper.LogInForUnitName);

            Chunk chunkCompanyTitle = new Chunk(headerStringCompanyTitle.ToString(), FontFactory.GetFont(FontFactory.COURIER, 10, Font.BOLD, new Color(0, 0, 0)));
            Chunk chunkReportTitle = new Chunk(headerStringReportTitle.ToString(), FontFactory.GetFont(FontFactory.COURIER, 10, Font.NORMAL, new Color(0, 0, 0)));

            HeaderFooter header = new HeaderFooter(new Phrase(chunkCompanyTitle), false);
            header.After = new Phrase(chunkReportTitle);
            header.Alignment = Element.ALIGN_CENTER;
            header.BorderWidthTop = 0;
            header.BorderColorBottom = new Color(166, 165, 165);

            HeaderFooter footer = new HeaderFooter(new Phrase("-"), new Phrase("-"));
            footer.Alignment = Element.ALIGN_CENTER;
            footer.Border = 0;
            footer.BorderColor = new Color(230, 230, 230);
            footer.BorderWidthTop = .25F;

            pdfDocumnet.Header = header;
            pdfDocumnet.Footer = footer;

            pdfDocumnet.Open();

            UnitWiseCustomerLedger objCustomerLedger;
            objCustomerLedger = lstCustomerLedger.LastOrDefault();

            Font fontForContent = new Font(Font.COURIER, 8, Font.NORMAL, new Color(0, 0, 0));
            Font fontForTableHeader = new Font(Font.COURIER, 10, Font.BOLD, new Color(0, 0, 0));

            string customerCodeForcheck = string.Empty;
            int numOfColumns1 = 4, numOfColumns = 5, totalCustomer = lstIndividualCustomer.Count;

            float[] widths = new float[] { 40f, 15f, 15f, 15f, 15f };
            Paragraph p;

            foreach (UnitWiseCustomerLedger cs in lstIndividualCustomer)
            {
                lstCustomerLedgerProcessed = CustomerLedgerProcess(lstCustomerLedger.Where(c => c.CustomerCode == cs.CustomerCode).ToList(), out totalCollection);

                objCustomerLedger = new UnitWiseCustomerLedger();
                objCustomerLedger = lstCustomerLedgerProcessed.LastOrDefault();

                PdfPTable pdfTable1 = new PdfPTable(numOfColumns1);
                pdfTable1.WidthPercentage = 100;
                pdfTable1.DefaultCell.Padding = 3;
                //pdfTable1.DefaultCell.BorderWidth = 0.5F;
                pdfTable1.DefaultCell.BorderColor = new Color(255, 255, 255);
                pdfTable1.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable1.DefaultCell.BackgroundColor = new Color(255, 255, 255);

                //1
                pdfTable1.AddCell(new Phrase("Customer Code:", fontForContent));
                pdfTable1.AddCell(new Phrase(objCustomerLedger.CustomerCode, fontForContent));
                pdfTable1.AddCell(new Phrase("Package Price:", fontForContent));
                pdfTable1.AddCell(new Phrase(objCustomerLedger.PackagePrice.ToString("0"), fontForContent));

                //2
                pdfTable1.AddCell(new Phrase("Customer Name:", fontForContent));
                pdfTable1.AddCell(new Phrase(objCustomerLedger.CustomerName, fontForContent));
                pdfTable1.AddCell(new Phrase("Installment Size:", fontForContent));
                pdfTable1.AddCell(new Phrase(objCustomerLedger.InstallmentSize.ToString("0.00"), fontForContent));

                //3
                pdfTable1.AddCell(new Phrase("Agreement Date( 0%):", fontForContent));
                pdfTable1.AddCell(new Phrase(objCustomerLedger.AgreementDate.ToString("dd-MMM-yyyy"), fontForContent));
                pdfTable1.AddCell(new Phrase("", fontForContent));
                pdfTable1.AddCell(new Phrase("", fontForContent));

                /*---------------------------------------------------------------------------------*/

                PdfPTable pdfTable = new PdfPTable(numOfColumns);
                pdfTable.SetWidths(widths);
                pdfTable.WidthPercentage = 100;
                pdfTable.DefaultCell.Padding = 3;
                pdfTable.DefaultCell.BorderWidth = 0.5F;
                pdfTable.DefaultCell.BorderColor = new Color(221, 218, 218);
                pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.DefaultCell.BackgroundColor = new Color(235, 235, 235);

                pdfTable.AddCell(new Phrase("Particulars", fontForTableHeader));
                pdfTable.AddCell(new Phrase("Memo No", fontForTableHeader));
                pdfTable.AddCell(new Phrase("Trans Date", fontForTableHeader));
                pdfTable.AddCell(new Phrase("Amount", fontForTableHeader));
                pdfTable.AddCell(new Phrase("Outstanding", fontForTableHeader));

                pdfTable.HeaderRows = 1;
                pdfTable.DefaultCell.BorderWidth = 0.5F;
                pdfTable.DefaultCell.BorderColor = new Color(195, 195, 195);
                pdfTable.DefaultCell.BackgroundColor = new Color(255, 255, 255);

                int counter = 0;
                foreach (UnitWiseCustomerLedger cl in lstCustomerLedgerProcessed)
                {
                    pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfTable.AddCell(new Phrase(cl.Particulars, fontForContent));
                    pdfTable.AddCell(new Phrase(cl.MemoNo, fontForContent));

                    if (counter == 0)
                    {
                        pdfTable.AddCell(new Phrase(string.Empty, fontForContent));
                        counter = 1;
                    }
                    else
                        pdfTable.AddCell(new Phrase(((DateTime)cl.TransDate).ToString("dd-MMM-yyyy"), fontForContent));

                    pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdfTable.AddCell(new Phrase(((decimal)cl.Amount).ToString("#,##0"), fontForContent));
                    pdfTable.AddCell(new Phrase(((decimal)cl.TotalPrincipalPlusServiceChargeReceivable).ToString("#,##0"), fontForContent));
                }

                pdfDocumnet.Add(pdfTable1);
                pdfDocumnet.Add(pdfTable);

                totalCustomer--;
                //p = new Paragraph("---------------------------------------------------------------------------------------------------------------------------------------\n");
                if (totalCustomer != 0)
                {
                    p = new Paragraph("_________________________________________________________________________________________________________________________________________\n");
                    pdfDocumnet.Add(p);
                }
            }

            pdfDocumnet.Close();

            return File(memStream.ToArray(), "application/pdf", "RSF-AllCustomerLedger.pdf");
        }

        private List<UnitWiseCustomerLedger> CustomerLedgerProcess(List<UnitWiseCustomerLedger> lstCustomerLedger, out decimal totalCollection)
        {
            int rowCounter = 0;
            decimal totalCollectionSum = 0;

            if (lstCustomerLedger.Count > 0)
            {
                UnitWiseCustomerLedger objCustomerLedger = new UnitWiseCustomerLedger();

                objCustomerLedger.CustomerCode = "";
                objCustomerLedger.CustomerName = "";
                objCustomerLedger.AgreementDate = new DateTime(1900, 1, 1);
                objCustomerLedger.PackagePrice = 0;
                objCustomerLedger.InstallmentSize = 0;
                objCustomerLedger.TotalPrincipalPlusServiceChargeReceivable = lstCustomerLedger[0].TotalPrincipalPlusServiceChargeReceivable;
                objCustomerLedger.Particulars = "Total Loan (Loan & Service Charge)";
                objCustomerLedger.MemoNo = "";
                objCustomerLedger.TransDate = new DateTime(1900, 1, 1);
                objCustomerLedger.Amount = 0;
                objCustomerLedger.Outstanding = 0;

                lstCustomerLedger.Insert(0, objCustomerLedger);

                foreach (UnitWiseCustomerLedger clr in lstCustomerLedger)
                {
                    if (rowCounter != (lstCustomerLedger.Count - 1))
                    {
                        lstCustomerLedger[rowCounter + 1].TotalPrincipalPlusServiceChargeReceivable = Convert.ToDecimal(clr.TotalPrincipalPlusServiceChargeReceivable - lstCustomerLedger[rowCounter + 1].Amount);
                    }
                    totalCollectionSum += Convert.ToDecimal(clr.Amount);
                    rowCounter++;
                }
            }

            totalCollection = totalCollectionSum;
            return lstCustomerLedger;
        }

        public ActionResult CashMemoInformation()
        {

            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "CashMemoInformation", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
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
            ViewBag.OpenMonthYear = "Day Open Date: " + objLoginHelper.TransactionOpenDate.ToString("MMMM  dd, yyyy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;
            ViewBag.CurrentDay = objLoginHelper.MonthOpenForSales.Date.ToString("dd-MMM-yyyy");

            ViewBag.toDate = objLoginHelper.TransactionOpenDate.ToString("MMMM  dd, yyyy");
            ViewBag.fromDate = objLoginHelper.MonthOpenForSales.Date.ToString("dd-MMM-yyyy");

            return View();
        }

        [GridAction]
        public ActionResult _CashMemoInformationList(string fromDate, string toDate)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            List<UnitWiseCashMemoInformation> lstCashMemoInfo = new List<UnitWiseCashMemoInformation>();
            lstCashMemoInfo = salesDal.ReadCashMemoInformation(objLoginHelper.LocationCode, fromDate, toDate);

            return View(new GridModel<UnitWiseCashMemoInformation> { Data = lstCashMemoInfo });
        }

        public JsonResult GetCustomerDataToCloseWithFullPaidOrWaive(string customerCode, string closedIn)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            try
            {
                CustomerDataToCloseWithFullPaidOrWaive objCustomerDataToCloseWithFullPaidOrWaive = new CustomerDataToCloseWithFullPaidOrWaive();
                objCustomerDataToCloseWithFullPaidOrWaive = salesDal.getCustomerDataToCloseWithFullPaidOrWaive(Helper.Select, customerCode, closedIn, objLoginHelper.LocationCode);


                objCustomerDataToCloseWithFullPaidOrWaive.AgreementClosedDateFormat = Convert.ToDateTime(objCustomerDataToCloseWithFullPaidOrWaive.AgreementClosedDate).ToString("dd-MMM-yyyy");
                objCustomerDataToCloseWithFullPaidOrWaive.TransDate = Convert.ToDateTime(objLoginHelper.TransactionOpenDate).ToString("dd-MMM-yyyy");
                objCustomerDataToCloseWithFullPaidOrWaive.AgreementDateStringFormat = Convert.ToDateTime(objCustomerDataToCloseWithFullPaidOrWaive.AgreementDate).ToString("dd-MMM-yyyy");

                return new JsonResult { Data = objCustomerDataToCloseWithFullPaidOrWaive };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }

        }

        public JsonResult GetCustomerDataPersonalInformation(string customerCode, string closedIn)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            try
            {
                CustomerDataToPersonalInformation objCustomerDataToPersonalInformation = new CustomerDataToPersonalInformation();
                objCustomerDataToPersonalInformation = salesDal.GetCustomerDataPersonalInformation(customerCode);

                ViewBag.UpzillName = objCustomerDataToPersonalInformation.UpzillName;
               // objCustomerDataToCloseWithFullPaidOrWaive.AgreementClosedDateFormat = Convert.ToDateTime(objCustomerDataToCloseWithFullPaidOrWaive.AgreementClosedDate).ToString("dd-MMM-yyyy");


                return new JsonResult { Data = objCustomerDataToPersonalInformation };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }

        }

        public JsonResult SaveCustomerFullPaidInfo(Sal_SalesAgreementClosedWithFullPaidOrWaive fullPaiedCustomerRegister, string closedIn, string approvalRequiredForFullPayed)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            bool? ifApprovalRequiredForFullPayed = true;

            Sal_SalesAgreementClosedWithFullPaidOrWaive objSalesAgreementClosedWithFullPaidOrWaive = new Sal_SalesAgreementClosedWithFullPaidOrWaive();

            try
            {
                if (string.IsNullOrEmpty(fullPaiedCustomerRegister.ApprovalNo))
                    fullPaiedCustomerRegister.ApprovalNo = null;

                fullPaiedCustomerRegister.CreatedOn = DateTime.Now;
                fullPaiedCustomerRegister.CreatedBy = objLoginHelper.LogInID;

                if (string.IsNullOrEmpty(approvalRequiredForFullPayed))
                {
                    ifApprovalRequiredForFullPayed = null;
                }
                else
                {
                    ifApprovalRequiredForFullPayed = Convert.ToBoolean(approvalRequiredForFullPayed); //== "false" ? false : true;
                }

                objSalesAgreementClosedWithFullPaidOrWaive = salesDal.SaveFullPaiedCustomer(fullPaiedCustomerRegister, closedIn, ifApprovalRequiredForFullPayed);
                erpDal.CreateUserLog(Helper.ClientIPAddress(), string.Empty, objLoginHelper.LocationCode, objLoginHelper.LogInID, "Customer Full Paid, Customer Code: " + fullPaiedCustomerRegister.CustomerCode);

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex, string.Empty) };
            }

        }

        public ActionResult ExportToPdfCashMemo(int page, string groupby, string orderBy, string filter, string fromDate, string toDate)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<UnitWiseCashMemoInformation> lstCashMemoInfo = new List<UnitWiseCashMemoInformation>();
            lstCashMemoInfo = salesDal.ReadCashMemoInformation(objLoginHelper.LocationCode, fromDate, toDate);

            Document pdfDocumnet = new Document(PageSize.A4, 35, 20, 15, 10);
            MemoryStream memStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(pdfDocumnet, memStream);

            StringBuilder headerStringCompanyTitle = new StringBuilder();
            StringBuilder headerStringReportTitle = new StringBuilder();
            headerStringCompanyTitle.AppendFormat(Helper.RSFAddress);
            headerStringReportTitle.AppendFormat("Cash Memo Information \n For Month: {0} \n Unit: {1}", objLoginHelper.TransactionOpenDate.ToString("MMMM  dd, yyyy"), objLoginHelper.LogInForUnitName);

            Chunk chunkCompanyTitle = new Chunk(headerStringCompanyTitle.ToString(), FontFactory.GetFont(FontFactory.COURIER, 10, Font.BOLD, new Color(0, 0, 0)));
            Chunk chunkReportTitle = new Chunk(headerStringReportTitle.ToString(), FontFactory.GetFont(FontFactory.COURIER, 10, Font.NORMAL, new Color(0, 0, 0)));

            HeaderFooter header = new HeaderFooter(new Phrase(chunkCompanyTitle), false);
            header.After = new Phrase(chunkReportTitle);
            header.Alignment = Element.ALIGN_CENTER;
            header.BorderWidthTop = 0;
            header.BorderColorBottom = new Color(166, 165, 165);

            HeaderFooter footer = new HeaderFooter(new Phrase("-"), new Phrase("-"));
            footer.Alignment = Element.ALIGN_CENTER;
            footer.Border = 0;
            footer.BorderColor = new Color(230, 230, 230);
            footer.BorderWidthTop = .25F;

            pdfDocumnet.Header = header;
            pdfDocumnet.Footer = footer;

            pdfDocumnet.Open();

            int numOfColumns = 7;
            float[] widths = new float[] { 15f, 12f, 23f, 10f, 10f, 10f, 20f };

            PdfPTable pdfTable = new PdfPTable(numOfColumns);
            pdfTable.SetWidths(widths);
            pdfTable.WidthPercentage = 100;
            pdfTable.DefaultCell.Padding = 3;
            pdfTable.DefaultCell.BorderWidth = 0.5F;
            pdfTable.DefaultCell.BorderColor = new Color(221, 218, 218);
            pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfTable.DefaultCell.BackgroundColor = new Color(235, 235, 235);

            Font fontForTableHeader = new Font(Font.COURIER, 10, Font.BOLD, new Color(0, 0, 0));
            pdfTable.AddCell(new Phrase("Collection Date", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Customer ID", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Customer Name", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Book No", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Page No", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Amount", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Collection Type", fontForTableHeader));

            pdfTable.HeaderRows = 1;
            pdfTable.DefaultCell.BorderWidth = 0.5F;
            pdfTable.DefaultCell.BorderColor = new Color(195, 195, 195);
            pdfTable.DefaultCell.BackgroundColor = new Color(255, 255, 255);

            Font fontForContent = new Font(Font.COURIER, 8, Font.NORMAL, new Color(0, 0, 0));

            foreach (UnitWiseCashMemoInformation cr in lstCashMemoInfo)
            {
                pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.AddCell(new Phrase(cr.TransDate.ToString("dd-MMM-yyyy"), fontForContent));
                pdfTable.AddCell(new Phrase(cr.CustomerCode, fontForContent));
                pdfTable.AddCell(new Phrase(cr.CustomerName, fontForContent));
                pdfTable.AddCell(new Phrase(cr.CashMemoBookNo, fontForContent));
                pdfTable.AddCell(new Phrase(cr.CashMemoNo, fontForContent));
                pdfTable.AddCell(new Phrase(((decimal)cr.Amount).ToString("#,##0"), fontForContent));
                pdfTable.AddCell(new Phrase(cr.Particulars, fontForContent));

                pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;


            }

            pdfDocumnet.Add(pdfTable);
            pdfDocumnet.Close();

            return File(memStream.ToArray(), "application/pdf", "RSF-CashMemo.pdf");
        }

        public ActionResult ExportToCsvCashMemo(int page, string groupby, string orderBy, string filter, string fromDate, string toDate)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<UnitWiseCashMemoInformation> lstCashMemoInfo = new List<UnitWiseCashMemoInformation>();
            lstCashMemoInfo = salesDal.ReadCashMemoInformation(objLoginHelper.LocationCode, fromDate, toDate);

            MemoryStream output = new MemoryStream();
            StreamWriter writer = new StreamWriter(output, Encoding.UTF8);

            writer.Write("Collection Date,");
            writer.Write("Customer ID,");
            writer.Write("Customer Name,");
            writer.Write("Book No,");
            writer.Write("Page No,");
            writer.Write("Amount,");
            writer.Write("Collection Type");
            writer.WriteLine();

            foreach (UnitWiseCashMemoInformation cr in lstCashMemoInfo)
            {
                writer.Write(((DateTime)cr.TransDate).ToString("dd-MMM-yyyy"));
                writer.Write(",");

                writer.Write("\"");
                writer.Write(cr.CustomerCode);
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(cr.CustomerName);
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(cr.CashMemoBookNo);
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(cr.CashMemoNo);
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(((decimal)cr.Amount).ToString("0"));
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(cr.Particulars);
                writer.Write("\"");
                writer.WriteLine();
            }

            writer.Flush();
            output.Position = 0;

            return File(output, "text/comma-separated-values", "RSF-Cashmemo.csv");
        }

        public JsonResult GetBatchNumberForCustomerTraining(string scheduledDate)
        {

            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<GetUnitWiseCustomerTrainingSchedule> lstBatchNumber = new List<GetUnitWiseCustomerTrainingSchedule>();
            lstBatchNumber = salesDal.ReadCustomerTrainingSchedule(objLoginHelper.LogInForUnitCode, Convert.ToDateTime(scheduledDate), null);

            ArrayList arr = new ArrayList();

            foreach (GetUnitWiseCustomerTrainingSchedule ui in lstBatchNumber)
            {
                arr.Add(new { Display = ui.BatchNoNParticipants, Value = ui.ScheduledBatchNo, noOfParticipantScheduled = ui.NoOfParticipantScheduled });
            }

            return new JsonResult { Data = arr };
        }
    }
}
