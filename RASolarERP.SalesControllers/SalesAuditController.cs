using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using RASolarERP.Model;
using RASolarHelper;

using RASolarERP.Web.Areas.Sales.Models;
using RASolarERP.Web.Models;
using RASolarERP.Web.Areas.Inventory.Models;
using RASolarERP.DomainModel.SalesModel;
using RASolarERP.Web.Areas.HRMS.Models;
using Telerik.Web.Mvc;
using RASolarHRMS.Model;
using System.Collections;


namespace RASolarERP.Web.Areas.Sales.Controllers
{
    public class SalesAuditController : Controller
    {
        private SalesData salesDal = new SalesData();
        RASolarSecurityData securityDal = new RASolarSecurityData();
        private RASolarERPData erpDal = new RASolarERPData();
        InventoryData inventoryDal = new InventoryData();
        HRMSData hraDal = new HRMSData();
        string message = string.Empty;

        private LoginHelper objLoginHelper = new LoginHelper();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CustomerRegisterForAuditor()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "CustomerRegisterForAuditor", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            string CheckAuditSetup = hraDal.ReadAuditSeqNumberAfterCheckFinishedDate(objLoginHelper.LocationCode);
            if (string.IsNullOrEmpty(CheckAuditSetup))
            {
                Session["messageInformation"] = "Please Configure Audit Setup First Before Any Transaction";
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

            List<UnitWiseCustomerRegisterReport> lstCustomerRegister = new List<UnitWiseCustomerRegisterReport>();
            lstCustomerRegister = salesDal.ReadUnitWiseCustomerRegisterReport(objLoginHelper.LogInForUnitCode);

            ViewBag.TotalCollectionInCurrentMonth = lstCustomerRegister.Sum(s => s.CollectionInCurrentMonthWithoutDP).ToString("0");

            ViewBag.TotalAdvance = ((decimal)lstCustomerRegister.Where(s => s.OverdueOrAdvanceBalance > 0).Sum(s => s.OverdueOrAdvanceBalance)).ToString("0");
            ViewBag.TotalOverdue = ((decimal)lstCustomerRegister.Where(s => s.OverdueOrAdvanceBalance < 0).Sum(s => s.OverdueOrAdvanceBalance * -1)).ToString("0");

            List<Aud_AdjustmentReasonCodes> newItemAdjustmentReason = new List<Aud_AdjustmentReasonCodes>();
            newItemAdjustmentReason = inventoryDal.AuditAdjustMentReasons(Helper.ReasonPurposeForCollection[2], Helper.ReasonForUserOrAuditor, Helper.ForSales);

            ViewBag.AuditReasons = newItemAdjustmentReason;

            return View(lstCustomerRegister);
        }

        public ActionResult SalesAuditAdjustment(string customerCode)
        {
            ViewBag.CustomerType = salesDal.ReadCustomerTypes();
            //ViewBag.PackageCapacity = salesDal.ReadCapacityByProjectCode("100200", Helper.IsCapacityOnlyForPackagesAndItems);
            //ViewBag.PackageLight = salesDal.ReadLightByCapacityId("0020WP");
            //ViewBag.Package = salesDal.ReadPackages("0020WP", "02LIGHT", Helper.NewSalesAgreement);
            //ViewBag.ModeOfPayment = salesDal.ReadModeOfPayment("SHS001", "SHS010", "HOUHLD"); changes for salesresalesagreement page for ddlPackage change
            ViewBag.ModeOfPayment = salesDal.ReadModeOfPayment("","SHS001", "SHS010", "HOUHLD","");
            //ViewBag.PanelModel = salesDal.ReadItemModelByCategoryId("PANEL1");
            //ViewBag.PanelStructureModel = salesDal.ReadItemModelByCategoryId("PNLSR1");
            //ViewBag.BatteryModel = salesDal.ReadItemModelByCategoryId("BAT001");
            //ViewBag.HolderModel = salesDal.ReadItemModelByCategoryId("HOL001");

            CustomerNAgreementNItemDetails objCustomerNAgreementNItemDetails = new CustomerNAgreementNItemDetails();
            objCustomerNAgreementNItemDetails = salesDal.ReadCustomerNAgreementItemDetails(customerCode, "100200");

            Session["SalesAdjustmentPreviousData"] = objCustomerNAgreementNItemDetails;

            return PartialView("AuditAdjustmentForSalesAgreement", objCustomerNAgreementNItemDetails);
        }

        public JsonResult AuditReasonList()
        {

            List<Aud_AdjustmentReasonCodes> newItemAdjustmentReason = new List<Aud_AdjustmentReasonCodes>();
            newItemAdjustmentReason = inventoryDal.CollectionAdjustMentReasons(Helper.ReasonPurposeForCollection[0], Helper.ReasonPurposeForCollection[1], Helper.ReasonForUserOrAuditor, Helper.ForSales);

            ArrayList arr = new ArrayList();

            foreach (Aud_AdjustmentReasonCodes auditReason in newItemAdjustmentReason)
            {
                arr.Add(new { Value = auditReason.ReasonCode, Display = auditReason.ReasonDescription });
            }

            return new JsonResult { Data = arr };
        }

        [GridAction]
        public ActionResult __CutomerCollectionDetailsForAudit(string customerCode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            string auditSeqNo = string.Empty;
            auditSeqNo = hraDal.ReadAuditSeqNumberAfterCheckFinishedDate(objLoginHelper.LocationCode);

            List<CustomerCollectionAdjustmentForAudit> lstCustomerCollection = new List<CustomerCollectionAdjustmentForAudit>();
            lstCustomerCollection = salesDal.ReadCustomerCollectionAdjustmentForAudit(customerCode, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForSales), auditSeqNo);

            if (lstCustomerCollection.Count > 0)
            {
                string collectedEmployeeName = string.Empty, responsibleEmployeeName = string.Empty;
                Hrm_EmployeeInfo objEmployeeInfo = new Hrm_EmployeeInfo();

                objEmployeeInfo = hraDal.ReadEmployeeInfo(lstCustomerCollection[0].CollectedByEmployeeID);
                collectedEmployeeName = objEmployeeInfo != null ? objEmployeeInfo.EmployeeName + " [" + lstCustomerCollection[0].CollectedByEmployeeID + "]" : string.Empty;

                objEmployeeInfo = hraDal.ReadEmployeeInfo(lstCustomerCollection[0].ResponsibleEmployeeID);
                responsibleEmployeeName = objEmployeeInfo != null ? objEmployeeInfo.EmployeeName + " [" + lstCustomerCollection[0].ResponsibleEmployeeID + "]" : string.Empty;

                var v = from ss in lstCustomerCollection
                        select new CustomerCollectionAdjustmentForAudit
                        {
                            SerialNo = ss.SerialNo,
                            CollectionDate = ss.CollectionDate,
                            RefMemoNo = ss.RefMemoNo,
                            CollectionAmount = ss.CollectionAmount,
                            CollectedByEmployeeID = ss.CollectedByEmployeeID,
                            CollectedByEmployeeName = collectedEmployeeName,
                            AuditReason = ss.AuditReason,
                            ResponsibleEmployeeID = ss.ResponsibleEmployeeID,
                            ResponsibleEmployeeName = responsibleEmployeeName,
                            Remarks = ss.Remarks
                        };

                lstCustomerCollection = v.ToList();
            }

            return View(new GridModel<CustomerCollectionAdjustmentForAudit>
            {
                Data = lstCustomerCollection
            });
        }

        //public ActionResult SalesAuditAdjsutment()
        //{
        //    objLoginHelper = (LoginHelper)Session["LogInInformation"];

        //    PageAccessRightHelper objPageAccessRightHelper = new PageAccessRightHelper();
        //    objPageAccessRightHelper = securityDal.ReadPageAccessRight(Helper.ForSales, "SalesAuditAdjsutment", objLoginHelper.UerRoleOrGroupID, Helper.Inactive);
        //    if (objPageAccessRightHelper != null)
        //    {
        //        if (objPageAccessRightHelper.AccessStatus == Helper.Inactive)
        //        {
        //            Session["messageInformation"] = objPageAccessRightHelper.MessageToShow;
        //            return RedirectToAction("ErrorMessage", "../ErrorHnadle");
        //        }
        //    }

        //    ViewBag.CustomerStatus = salesDal.CustomerStatus();
        //    TempData["selectValue"] = "0";

        //    Aud_AuditingMaster objAuditMaster = new Aud_AuditingMaster();
        //    objAuditMaster = salesDal.AuditingMasterByLocationCode(objLoginHelper.LogInForUnitCode);

        //    if (objAuditMaster != null)
        //    {
        //        ViewBag.AuditStartDate = objAuditMaster.AuditStartDate != null ? Helper.DateFormat(Convert.ToDateTime(objAuditMaster.AuditStartDate)) : Helper.DateFormat(DateTime.Now);
        //        ViewBag.AuditFinishedDate = objAuditMaster.AuditFinishDate != null ? Helper.DateFormat(Convert.ToDateTime(objAuditMaster.AuditFinishDate)) : "";
        //        ViewBag.AuditCode = objAuditMaster.AuditSeqNo;
        //    }
        //    else
        //    {
        //        ViewBag.AuditStartDate = "";
        //        ViewBag.AuditFinishedDate = "";
        //        ViewBag.AuditCode = "";
        //    }

        //    if (string.IsNullOrEmpty(Request.Params["grvSalesAdjustment-page"]))
        //    {
        //        Session.Remove("CustomerType");
        //    }

        //    if (Session["CustomerType"] != null)
        //    {
        //        string ctype = Session["CustomerType"].ToString();
        //        byte customerType = Convert.ToByte(ctype);

        //        TempData["CStatus"] = salesDal.ReadCustomerListWithRecoveryStatus(objLoginHelper.LogInForUnitCode, Convert.ToByte(ctype), Helper.DateTo(objLoginHelper.AuditorYearMonth));

        //        List<Aud_AdjustmentReasonCodes> salesAuditReasons = new List<Aud_AdjustmentReasonCodes>();
        //        //salesAuditReasons = salesDal.SalesAuditResonCodes();

        //        TempData["SalesAuditReason"] = salesAuditReasons;

        //        TempData["selectValue"] = ctype;

        //        if (customerType == Helper.FullPaiedCustomer || customerType == Helper.CashSalesCustomer || customerType == Helper.SystemReturnCustomer)
        //        {
        //            TempData["DisplayOrNot"] = true;
        //        }
        //        else
        //        {
        //            TempData["DisplayOrNot"] = false;
        //        }

        //        string recoveredUpToWithoutDP = "";

        //        recoveredUpToWithoutDP = "Recovered Up to " + Helper.DateTo(objLoginHelper.AuditorYearMonth).ToString("dd-MMM-yy") + " Without DP";

        //        TempData["recoveredUpToWithoutDP"] = recoveredUpToWithoutDP;
        //    }

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

        //    if (objLoginHelper.UerRoleOrGroupID == UserGroup.RegionManager)
        //    {
        //        ViewBag.ShowHideMenu = true;
        //    }
        //    else
        //        ViewBag.ShowHideMenu = false;

        //    return View();
        //}

        //[HttpPost]
        //public ActionResult SalesAuditAdjsutment(FormCollection fCollection)
        //{
        //    objLoginHelper = (LoginHelper)Session["LogInInformation"];

        //    ViewBag.CustomerStatus = salesDal.CustomerStatus();
        //    TempData["selectValue"] = fCollection["ddlCustomerStatus"].ToString();

        //    TempData["CStatus"] = salesDal.ReadCustomerListWithRecoveryStatus(objLoginHelper.LogInForUnitCode, Convert.ToByte(fCollection["ddlCustomerStatus"].ToString()), Helper.DateTo(objLoginHelper.AuditorYearMonth));

        //    List<Aud_AdjustmentReasonCodes> salesAuditReasons = new List<Aud_AdjustmentReasonCodes>();
        //    // salesAuditReasons = salesDal.SalesAuditResonCodes();

        //    TempData["SalesAuditReason"] = salesAuditReasons;

        //    Aud_AuditingMaster objAuditMaster = new Aud_AuditingMaster();
        //    objAuditMaster = salesDal.AuditingMasterByLocationCode(objLoginHelper.LogInForUnitCode);

        //    if (objAuditMaster != null)
        //    {
        //        ViewBag.AuditStartDate = objAuditMaster.AuditStartDate != null ? Helper.DateFormat(Convert.ToDateTime(objAuditMaster.AuditStartDate)) : Helper.DateFormat(DateTime.Now);
        //        ViewBag.AuditFinishedDate = objAuditMaster.AuditFinishDate != null ? Helper.DateFormat(Convert.ToDateTime(objAuditMaster.AuditFinishDate)) : "";
        //        ViewBag.AuditCode = objAuditMaster.AuditSeqNo;
        //    }

        //    ViewBag.LocationTitle = objLoginHelper.LocationTitle;
        //    ViewBag.Location = objLoginHelper.Location;
        //    ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
        //    ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
        //    ViewBag.RegionTitle = objLoginHelper.RegionTitle;
        //    ViewBag.RegionName = objLoginHelper.LogInForRegionName;
        //    ViewBag.UnitTitle = objLoginHelper.UnitTitle;
        //    ViewBag.UnitName = objLoginHelper.LogInForUnitName;

        //    ViewBag.ModuleName = objLoginHelper.ModluleTitle;
        //    ViewBag.TopMenu = objLoginHelper.TopMenu;

        //    Session["CustomerType"] = fCollection["ddlCustomerStatus"].ToString();
        //    byte customerType = Convert.ToByte(fCollection["ddlCustomerStatus"]);

        //    if (customerType == Helper.FullPaiedCustomer || customerType == Helper.CashSalesCustomer || customerType == Helper.SystemReturnCustomer)
        //    {
        //        TempData["DisplayOrNot"] = true;
        //    }
        //    else
        //    {
        //        TempData["DisplayOrNot"] = false;
        //    }

        //    if (objLoginHelper.UerRoleOrGroupID == UserGroup.RegionManager)
        //    {
        //        ViewBag.ShowHideMenu = true;
        //    }
        //    else
        //        ViewBag.ShowHideMenu = false;

        //    string recoveredUpToWithoutDP = "";

        //    recoveredUpToWithoutDP = "Recovered Up to " + Helper.DateTo(objLoginHelper.AuditorYearMonth).ToString("dd-MMM-yy") + " Without DP";

        //    TempData["recoveredUpToWithoutDP"] = recoveredUpToWithoutDP;

        //    return View();
        //}

        //public JsonResult SalesAuditAdjsutmentSave(string customerCode, string salesAdjustmentValue, string auditReason, string remarks)
        //{
        //    short auditSequenceNumber = 0;
        //    string saveMessage = string.Empty;

        //    //tbl_UnitWiseCustomerStatus objCustomerStatus = new tbl_UnitWiseCustomerStatus();
        //    Aud_AuditingMaster objAuditMaster = new Aud_AuditingMaster();

        //    try
        //    {
        //        objLoginHelper = (LoginHelper)Session["LogInInformation"];

        //        // objCustomerStatus = salesDal.ReadUnitWiseCustomerStatus(customerCode);
        //        objAuditMaster = salesDal.AuditingMasterByLocationCode(objLoginHelper.LogInForUnitCode);

        //        if (objAuditMaster != null)
        //        {
        //            auditSequenceNumber = Convert.ToInt16(objAuditMaster.AuditSeqNo);
        //        }
        //        else
        //        {
        //            auditSequenceNumber += 1;

        //            objAuditMaster = new Aud_AuditingMaster();

        //            objAuditMaster.LocationCode = objLoginHelper.LogInForUnitCode;
        //            //objAuditMaster.AuditForSalInvAccHrm = Helper.ForSales;
        //            objAuditMaster.AuditSeqNo = auditSequenceNumber.ToString();
        //            objAuditMaster.AuditStartDate = DateTime.Now;
        //            objAuditMaster.CreatedBy = objLoginHelper.LogInID;
        //            objAuditMaster.CreatedOn = DateTime.Now;

        //            salesDal.SaveAuditingMaster(objAuditMaster);
        //        }

        //        //if (objCustomerStatus != null)
        //        //{
        //        //    objCustomerStatus.AuditAdjustmentInSep = Convert.ToDecimal(salesAdjustmentValue);
        //        //    objCustomerStatus.RefAuditSeqNo = auditSequenceNumber;
        //        //    objCustomerStatus.Remarks = remarks;

        //        //    if (auditReason == "0")
        //        //    {
        //        //        objCustomerStatus.RefReasonForAuditAdjustment = null;
        //        //    }
        //        //    else
        //        //        objCustomerStatus.RefReasonForAuditAdjustment = auditReason;

        //        //    objCustomerStatus = salesDal.UpdateUnitWiseCustomerStatus(objCustomerStatus);

        //        //    if (objCustomerStatus != null)
        //        //    {
        //        //        saveMessage = "Save is Succeed";
        //        //    }

        //        //}
        //        //else
        //        //{ saveMessage = "Entry Is Not Valid."; }

        //    }
        //    catch (Exception ex)
        //    {
        //        saveMessage = "Save is not Succeed.";
        //    }

        //    return new JsonResult { Data = saveMessage };
        //}

        //public JsonResult FinishedAuditAdjustment()
        //{
        //    short auditSequenceNumber = 0;
        //    string updateMessage = string.Empty;
        //    Aud_AuditingMaster objAuditMaster = new Aud_AuditingMaster();

        //    objLoginHelper = (LoginHelper)Session["LogInInformation"];

        //    try
        //    {
        //        objAuditMaster = salesDal.AuditingMasterByLocationCode(objLoginHelper.LogInForUnitCode);
        //        if (objAuditMaster != null && objAuditMaster.AuditFinishDate == null)
        //        {
        //            objAuditMaster.AuditFinishDate = DateTime.Now;
        //            salesDal.UpdateAuditingMaster(objAuditMaster);

        //            if (objAuditMaster != null)
        //                updateMessage = "succeedSave";
        //            else
        //                updateMessage = "failed";
        //        }
        //        else if (objAuditMaster == null)
        //        {
        //            auditSequenceNumber += 1;

        //            objAuditMaster = new Aud_AuditingMaster();

        //            objAuditMaster.LocationCode = objLoginHelper.LogInForUnitCode;
        //            //objAuditMaster.AuditForSalInvAccHrm = Helper.ForInventory;
        //            objAuditMaster.AuditSeqNo = auditSequenceNumber.ToString();
        //            objAuditMaster.AuditStartDate = DateTime.Now;
        //            objAuditMaster.CreatedBy = objLoginHelper.LogInID;
        //            objAuditMaster.CreatedOn = DateTime.Now;

        //            salesDal.SaveAuditingMaster(objAuditMaster);

        //            if (objAuditMaster != null)
        //                updateMessage = "succeedSave";
        //            else
        //                updateMessage = "failed";
        //        }
        //        else
        //        {
        //            updateMessage = "alreadyFinished";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        updateMessage = "failed";
        //    }

        //    return new JsonResult { Data = updateMessage };
        //}

        public JsonResult SaveCustomerCollectionAdjustment(FormCollection fCollection)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            int serial = 0;
            string auditSequenceNumber = string.Empty;

            Sal_CollectionFromCustomers objCustomerCollection = new Sal_CollectionFromCustomers();
            Aud_AuditAdjustmentRelatedCollectionFromCustomers objCollectionAuditAdjustnment = new Aud_AuditAdjustmentRelatedCollectionFromCustomers();

            try
            {
                if (fCollection["hfCollectionSerials"] == "0")
                {
                    serial = salesDal.CustomerCollectionSerial(fCollection["hfCustomerCodeForCollection"], Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForSales));
                }
                else
                {
                    serial = Convert.ToInt32(fCollection["hfCollectionSerials"]);
                }

                auditSequenceNumber = hraDal.ReadAuditSeqNumberAfterCheckFinishedDate(objLoginHelper.LocationCode);

                objCustomerCollection.CustomerCode = fCollection["hfCustomerCodeForCollection"];
                objCustomerCollection.YearMonth = Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForSales);
                objCustomerCollection.SerialNo = Convert.ToByte(serial);
                objCustomerCollection.CollectionType = Helper.CollectionTypeAAA;
                objCustomerCollection.CollectionAmount = Convert.ToDecimal(fCollection["txtCollectionAmount"]);
                objCustomerCollection.CollectionAmount_Principal = Convert.ToDecimal(fCollection["CollectionAmount_Principal"]);
                objCustomerCollection.CollectionAmount_ServiceCharge = Convert.ToDecimal(fCollection["CollectionAmount_ServiceCharge"]);
                objCustomerCollection.CollectionDate = Convert.ToDateTime(Helper.DateFormatMMDDYYYY(fCollection["dtpCollectionDate"]));

                if (!string.IsNullOrEmpty(fCollection["txtMemoNumber"]) || String.IsNullOrEmpty(fCollection["txtMemoNumber"]))
                {
                    objCustomerCollection.RefMemoNo = fCollection["txtMemoNumber"];

                    //string firstCharacterOfCashMemo = fCollection["txtMemoNumber"].Trim().Substring(0, 1);

                    //if (firstCharacterOfCashMemo.ToUpper() == "A" || !String.IsNullOrEmpty(firstCharacterOfCashMemo))
                    //{
                    string firstCharacterOfCashMemo = fCollection["txtMemoNumber"].Trim();

                   

                    if (firstCharacterOfCashMemo.ToUpper() == "BLANK")
                    {
                        objCustomerCollection.CashMemoNo = fCollection["txtMemoNumber"];
                        objCustomerCollection.CashMemoUsesID ="NULL";
                        objCustomerCollection.RefMemoNo = null;
                    }
                    else
                    {
                        objCustomerCollection.CashMemoNo = fCollection["txtMemoNumber"];
                        objCustomerCollection.CashMemoUsesID = Helper.CashMemuUsesIdFirst;
                        objCustomerCollection.RefMemoNo = null;
                    }

                    objCustomerCollection.RefAELocationCode = objLoginHelper.LocationCode;
                    //}
                }


                objCustomerCollection.CollectedByEmployee = fCollection["hfCustomerFprPerson"].Trim();

                objCustomerCollection.UserID = objLoginHelper.LogInID;
                objCustomerCollection.EntryTime = DateTime.Now;

                objCollectionAuditAdjustnment.CustomerCode = fCollection["hfCustomerCodeForCollection"];
                objCollectionAuditAdjustnment.YearMonth = Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForSales);
                objCollectionAuditAdjustnment.SerialNo = Convert.ToByte(serial);
                objCollectionAuditAdjustnment.AuditSeqNo = auditSequenceNumber;
                objCollectionAuditAdjustnment.CollectionInUnit = objLoginHelper.LocationCode;
                objCollectionAuditAdjustnment.ReasonCode = fCollection["ddlAuditReason"].Trim();
                objCollectionAuditAdjustnment.ResponsibleEmployeeID = fCollection["hfResponsibeEmployee"].Trim();
                objCollectionAuditAdjustnment.Remarks = fCollection["txtClaimRemarks"].Trim();

                //if (fCollection["ddlAuditReason"].Trim() == "CASCOBDEPM" || fCollection["ddlAuditReason"].Trim() == "CASCONDEPM" || fCollection["ddlAuditReason"].Trim() == "CASCONDEPW")
                //{ }

                if (fCollection["hfCollectionSerials"] == "0")
                {
                    objCollectionAuditAdjustnment = salesDal.SaveCustomerCollectionForAudit(objCustomerCollection, objCollectionAuditAdjustnment, Helper.CashMemuUsesIdFirst);
                }
                else
                {
                    objCustomerCollection = salesDal.UpdateCustomerCollectionAuditAdjustment(objCustomerCollection, objCollectionAuditAdjustnment);
                }

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public JsonResult SalesAgreementAuditAdjustmentSave(Aud_AuditAdjustmentObservationOnSalesAgreement objAuditAdjustmentObservationOnSalesAgreement)
        {
            string auditSequenceNumber = string.Empty, saveMessage = string.Empty;

            try
            {
                objLoginHelper = (LoginHelper)Session["LogInInformation"];
                auditSequenceNumber = hraDal.ReadAuditSeqNumberAfterCheckFinishedDate(objLoginHelper.LocationCode);

                objAuditAdjustmentObservationOnSalesAgreement.LocationCode = objLoginHelper.LocationCode;
                objAuditAdjustmentObservationOnSalesAgreement.YearMonth = Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForSales);
                objAuditAdjustmentObservationOnSalesAgreement.DataStatusFlag = "1RC";
                objAuditAdjustmentObservationOnSalesAgreement.AuditSeqNo = auditSequenceNumber;

                if (!string.IsNullOrEmpty(objAuditAdjustmentObservationOnSalesAgreement.RefMemoNo))
                {
                    objAuditAdjustmentObservationOnSalesAgreement.RefMemoNo = objAuditAdjustmentObservationOnSalesAgreement.RefMemoNo;

                    string firstCharacterOfCashMemo = objAuditAdjustmentObservationOnSalesAgreement.RefMemoNo.Trim().Substring(0, 1);

                    if (firstCharacterOfCashMemo.ToUpper() == "A")
                    {
                        objAuditAdjustmentObservationOnSalesAgreement.CashMemoNo = objAuditAdjustmentObservationOnSalesAgreement.RefMemoNo;
                        objAuditAdjustmentObservationOnSalesAgreement.CashMemoUsesID = Helper.CashMemuUsesIdFirst;
                        objAuditAdjustmentObservationOnSalesAgreement.RefMemoNo = null;
                    }
                }

                Aud_AuditAdjustmentObservationOnSalesAgreement objPreviousDataAuditAdjustmentObservationOnSalesAgreement = new Aud_AuditAdjustmentObservationOnSalesAgreement();

                CustomerNAgreementNItemDetails objCustomerNAgreementNItemDetails = new CustomerNAgreementNItemDetails();
                objCustomerNAgreementNItemDetails = (CustomerNAgreementNItemDetails)Session["SalesAdjustmentPreviousData"];

                objPreviousDataAuditAdjustmentObservationOnSalesAgreement.LocationCode = objLoginHelper.LocationCode;
                objPreviousDataAuditAdjustmentObservationOnSalesAgreement.YearMonth = Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForSales);
                objPreviousDataAuditAdjustmentObservationOnSalesAgreement.CustomerCode = objCustomerNAgreementNItemDetails.CustomerCode;
                objPreviousDataAuditAdjustmentObservationOnSalesAgreement.DataStatusFlag = "0BC";
                objPreviousDataAuditAdjustmentObservationOnSalesAgreement.AuditSeqNo = auditSequenceNumber;
                objPreviousDataAuditAdjustmentObservationOnSalesAgreement.CustomerName = objCustomerNAgreementNItemDetails.CustomerName;
                objPreviousDataAuditAdjustmentObservationOnSalesAgreement.IsReSales = objCustomerNAgreementNItemDetails.IsResales;
                objPreviousDataAuditAdjustmentObservationOnSalesAgreement.AgreementDate = Convert.ToDateTime(objCustomerNAgreementNItemDetails.AgreementDate);
                objPreviousDataAuditAdjustmentObservationOnSalesAgreement.CustomerType = objCustomerNAgreementNItemDetails.CustomerType;
                objPreviousDataAuditAdjustmentObservationOnSalesAgreement.PackageCapacity = objCustomerNAgreementNItemDetails.Capacity;
                objPreviousDataAuditAdjustmentObservationOnSalesAgreement.PackageLight = objCustomerNAgreementNItemDetails.Light;
                objPreviousDataAuditAdjustmentObservationOnSalesAgreement.PackageCode = objCustomerNAgreementNItemDetails.PackageCode;

                objPreviousDataAuditAdjustmentObservationOnSalesAgreement.StoreLocationForPanel = objCustomerNAgreementNItemDetails.StoreLocationForPanel;
                objPreviousDataAuditAdjustmentObservationOnSalesAgreement.PanelItemCode = objCustomerNAgreementNItemDetails.PanelItemCode;
                objPreviousDataAuditAdjustmentObservationOnSalesAgreement.PanelSerialNo = objCustomerNAgreementNItemDetails.PanelSerialNo;

                objPreviousDataAuditAdjustmentObservationOnSalesAgreement.StoreLocationForBattery = objCustomerNAgreementNItemDetails.StoreLocationForBattery;
                objPreviousDataAuditAdjustmentObservationOnSalesAgreement.BatteryItemCode = objCustomerNAgreementNItemDetails.BatteryItemCode;
                objPreviousDataAuditAdjustmentObservationOnSalesAgreement.BatterySerialNo = objCustomerNAgreementNItemDetails.BatterySerialNo;

                objPreviousDataAuditAdjustmentObservationOnSalesAgreement.ModeOfPaymentID = objCustomerNAgreementNItemDetails.ModeOfPaymentID;

                objPreviousDataAuditAdjustmentObservationOnSalesAgreement.DownPaymentID = objCustomerNAgreementNItemDetails.DownPaymentID;
                objPreviousDataAuditAdjustmentObservationOnSalesAgreement.DownPaymentAmount = Convert.ToDecimal(objCustomerNAgreementNItemDetails.DownPaymentAmount);

                objPreviousDataAuditAdjustmentObservationOnSalesAgreement.ServiceChargeID = objCustomerNAgreementNItemDetails.ServiceChargeID;
                objPreviousDataAuditAdjustmentObservationOnSalesAgreement.STDServiceChargePercentage = objCustomerNAgreementNItemDetails.STDServiceChargePercentage;

                objPreviousDataAuditAdjustmentObservationOnSalesAgreement.RefMemoNo = objCustomerNAgreementNItemDetails.RefMemoNo;
                objPreviousDataAuditAdjustmentObservationOnSalesAgreement.CashMemoNo = objCustomerNAgreementNItemDetails.CashMemoNo;
                objPreviousDataAuditAdjustmentObservationOnSalesAgreement.CashMemoUsesID = objCustomerNAgreementNItemDetails.CashMemoUsesID;

                objPreviousDataAuditAdjustmentObservationOnSalesAgreement.PackagePrice = Convert.ToDecimal(objCustomerNAgreementNItemDetails.PackagePrice);

                objPreviousDataAuditAdjustmentObservationOnSalesAgreement.PanelStructureItemCode = objCustomerNAgreementNItemDetails.PanelStructureItemCode;
                objPreviousDataAuditAdjustmentObservationOnSalesAgreement.HolderItemCode = objCustomerNAgreementNItemDetails.HolderItemCode;

                if (salesDal.IsAuditAdjustmentObservationOnSalesAgreementExistOrNot(objAuditAdjustmentObservationOnSalesAgreement.LocationCode, objAuditAdjustmentObservationOnSalesAgreement.CustomerCode) == false)
                {
                    salesDal.Create(objAuditAdjustmentObservationOnSalesAgreement, objPreviousDataAuditAdjustmentObservationOnSalesAgreement);
                }
                else
                {
                    // salesDal.Update(objAuditAdjustmentObservationOnSalesAgreement);
                    saveMessage = "Multiple edit for a customer is not allowed.";
                }

                Session.Remove("SalesAdjustmentPreviousData");
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(saveMessage) };
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

                if (fullPaiedCustomerRegister.IsClosedWithWaive != false)
                {
                    objSalesAgreementClosedWithFullPaidOrWaive.IsClosedWithWaive = true;
                    objSalesAgreementClosedWithFullPaidOrWaive.ApprovalNo = fullPaiedCustomerRegister.ApprovalNo;
                }

                objSalesAgreementClosedWithFullPaidOrWaive = salesDal.SaveFullPaiedCustomer(fullPaiedCustomerRegister, closedIn, ifApprovalRequiredForFullPayed);

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }

        }

        public JsonResult PackageCapacity(string projectCode)
        {
            return new JsonResult { Data = salesDal.ReadPackageOrItemCapacityForAuditor(projectCode, Helper.IsCapacityOnlyForPackagesAndItems) };
        }

        public JsonResult PackageLight(string capacityId)
        {
            return new JsonResult { Data = salesDal.ReadLightForAuditor(capacityId) };
        }

        public JsonResult PackageInfo(string capacityId, string lightId)
        {
            return new JsonResult { Data = salesDal.ReadPackagesForAuditor(capacityId, lightId) };
        }

        public JsonResult PanelInfo(string itemCapacity)
        {
            return new JsonResult { Data = salesDal.ReadInvItemsForAuditors(Helper.InventoryItem, "PANEL1", itemCapacity) };
        }

        public JsonResult BatteryInfo()
        {
            return new JsonResult { Data = salesDal.ReadInvItemsForAuditors(Helper.InventoryItem, "BAT001") };
        }

        public JsonResult ItemSerialInfo(string storeLocation, string itemCode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            return new JsonResult { Data = salesDal.ReadItemStockWithSerialNoByLocationForAuditor(Convert.ToByte(storeLocation), objLoginHelper.LocationCode, itemCode) };
        }

        public JsonResult ServiceChargePolicy(string customerType, string modeOfPayment)
        {
            return new JsonResult { Data = salesDal.ReadServiceChargePolicyforAuditor(customerType, modeOfPayment) };
        }

        public JsonResult DownPaymentPolicy(string modeOfPayment, string packageCode)
        {
            return new JsonResult { Data = salesDal.ReadDownPaymentPolicyForAuditor(modeOfPayment, packageCode) };
        }

        public ActionResult SaveCustomerCollectionAdjustmentForUpdateNDelete(FormCollection fCollection)
        {
            try
            {
                objLoginHelper = (LoginHelper)Session["LogInInformation"];

                byte entrySerialNo = Convert.ToByte(fCollection["hfCollectionSerialsForUpdateNDelete"]);
                string yearMonth = fCollection["hfCollectionYearMonthForUpdateNDelete"];
                string yearMonth1 = fCollection["dtpCollectionDateForUpdateNDelete"].ToString();

                Sal_CollectionFromCustomers objCollectionFromCustomers = new Sal_CollectionFromCustomers();

                objCollectionFromCustomers = salesDal.ReadCollectionFromCustomers(fCollection["hfCustomerCodeForCollectionForUpdateNDelete"], yearMonth, Convert.ToByte(fCollection["hfCollectionSerialsForUpdateNDelete"]));

                objCollectionFromCustomers.CustomerCode = fCollection["hfCustomerCodeForCollectionForUpdateNDelete"];
                objCollectionFromCustomers.YearMonth = yearMonth; //Helper.ConvertDateToYearMonth(Convert.ToDateTime(yearMonth1));
                objCollectionFromCustomers.SerialNo = entrySerialNo;
                if (fCollection["hfCustomerCollectionTypeForUpdateNDelete"].Trim() == "Installment/Overdue Collection Amount")
                    objCollectionFromCustomers.CollectionType = Helper.CollectionTypeIOC;
                else
                    objCollectionFromCustomers.CollectionType = Helper.CollectionTypeAAA;
                objCollectionFromCustomers.CollectionAmount = Convert.ToDecimal(fCollection["txtCollectionAmountForUpdateNDelete"]);
                objCollectionFromCustomers.CollectionDate = Convert.ToDateTime(fCollection["dtpCollectionDateForUpdateNDelete"]);

                if (!string.IsNullOrEmpty(fCollection["txtMemoNumberForUpdateNDelete"]))
                {
                    objCollectionFromCustomers.RefMemoNo = fCollection["txtMemoNumberForUpdateNDelete"];

                    string firstCharacterOfCashMemo = objCollectionFromCustomers.RefMemoNo.Trim().Substring(0, 1);

                    if (firstCharacterOfCashMemo.ToUpper() == "A")
                    {
                        objCollectionFromCustomers.CashMemoNo = objCollectionFromCustomers.RefMemoNo;
                        objCollectionFromCustomers.CashMemoUsesID = Helper.CashMemuUsesIdFirst;
                        objCollectionFromCustomers.RefMemoNo = null;
                    }

                }

                else
                {
                    objCollectionFromCustomers.CashMemoNo = "";
                    objCollectionFromCustomers.CashMemoUsesID = Helper.CashMemuUsesIdFirst;
                    objCollectionFromCustomers.RefMemoNo = null;
                }

                objCollectionFromCustomers.CollectedByEmployee = null;
                objCollectionFromCustomers.RefAELocationCode = objLoginHelper.LocationCode;
                objCollectionFromCustomers.UserID = objLoginHelper.LogInID;
                objCollectionFromCustomers.EntryTime = DateTime.Now;

                salesDal.CustomerCollectionSaveForUpdateOrDelete(fCollection["hfUpdateNDeleteOptions"], objCollectionFromCustomers);
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public ActionResult CustomerLedger(string customerCode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            decimal totalCollection;
            SalesDataProcess objSalesDataProcess = new SalesDataProcess();
            List<CustomerLedgerReport> lstCustomerLedger = new List<CustomerLedgerReport>();
            lstCustomerLedger = objSalesDataProcess.CustomerLedgerProcess(customerCode, out totalCollection);

            ViewBag.TotalCollection = totalCollection.ToString("0");

            return PartialView("CustomerLedgerForAuditor", lstCustomerLedger);
        }

    }
}
