using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using System.Collections;
using System.Collections.Generic;

using iTextSharp.text;
using iTextSharp.text.pdf;

using RASolarHelper;
using RASolarERP.Model;
using RASolarAMS.Model;
using RASolarERP.Web.Models;
using RASolarERP.DomainModel.AMSModel;
using RASolarERP.Web.Areas.Sales.Models;
using RASolarERP.Web.Areas.Financial.Models;
using RASolarERP.Web.Areas.HRMS.Models;
using RASolarERP.Web.Areas.Inventory.Models;

namespace RASolarERP.Web.Areas.Financial.Controllers
{
    public class AccountVoucherController : Controller
    {
        private SalesData salesDal = new SalesData();
        private AccountingReportData accountingReportDal = new AccountingReportData();
        private RASolarERPData erpDal = new RASolarERPData();
        private AccountDeptModel accountDeptDal = new AccountDeptModel();
        private VoucherProcess voucherProcess = new VoucherProcess();
        private HRMSData hrmsDal = new HRMSData();
        InventoryData inventoryDal = new InventoryData();
        RASolarSecurityData securityDal = new RASolarSecurityData();

        LoginHelper objLoginHelper = new LoginHelper();
        string message = string.Empty;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ContraVoucherForAuditor()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAuditor, "ContraVoucherForAuditor", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            string CheckAuditSetup = hrmsDal.ReadAuditSeqNumberAfterCheckFinishedDate(objLoginHelper.LocationCode);
            if (string.IsNullOrEmpty(CheckAuditSetup))
            {
                Session["messageInformation"] = "Please Configure Audit Setup First Before Any Transaction";
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

            ViewBag.Project = accountingReportDal.ReadProject();

            ViewBag.AccountInfo = accountDeptDal.ReadCashBankAccount(objLoginHelper.LocationCode, "100200", objLoginHelper.Location, Helper.SubsidytAccount);

            List<Aud_AdjustmentReasonCodes> newItemAdjustmentReason = new List<Aud_AdjustmentReasonCodes>();
            newItemAdjustmentReason = inventoryDal.AuditAdjustMentReasons(Helper.ContraVoucherForAuditor, Helper.ReasonForUserOrAuditor, Helper.ForAccounting);
            ViewBag.AuditReasons = newItemAdjustmentReason;

            ArrayList transactionNumberMax = accountDeptDal.ReadVoucherTransNoMax(objLoginHelper.LocationCode, objLoginHelper.CurrentDate.ToString("yyMMdd"));
            ViewBag.VoucherTransactionNumber = Helper.AccountSequenceNumberGeneration(transactionNumberMax, objLoginHelper);

            return View();
        }

        public ActionResult PaymentVoucherForAuditor()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAuditor, "PaymentVoucherForAuditor", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            string CheckAuditSetup = hrmsDal.ReadAuditSeqNumberAfterCheckFinishedDate(objLoginHelper.LocationCode);
            if (string.IsNullOrEmpty(CheckAuditSetup))
            {
                Session["messageInformation"] = "Please Configure Audit Setup First Before Any Transaction";
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

            ViewBag.Project = accountingReportDal.ReadProject();
            ViewBag.AccountInfo = accountDeptDal.ReadCashBankAccount(objLoginHelper.LocationCode, "100200", objLoginHelper.Location, Helper.SubsidytAccount);

            List<Aud_AdjustmentReasonCodes> newItemAdjustmentReason = new List<Aud_AdjustmentReasonCodes>();
            newItemAdjustmentReason = inventoryDal.AuditAdjustMentReasons(Helper.PaymentVoucherForAuditor, Helper.ReasonForUserOrAuditor, Helper.ForAccounting);
            ViewBag.AuditReasons = newItemAdjustmentReason;

            ArrayList transactionNumberMax = accountDeptDal.ReadVoucherTransNoMax(objLoginHelper.LocationCode, objLoginHelper.CurrentDate.ToString("yyMMdd"));
            ViewBag.VoucherTransactionNumber = Helper.AccountSequenceNumberGeneration(transactionNumberMax, objLoginHelper);

            return View();
        }

        public ActionResult ReceiveVoucherForAuditor()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAuditor, "ReceiveVoucherForAuditor", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            string CheckAuditSetup = hrmsDal.ReadAuditSeqNumberAfterCheckFinishedDate(objLoginHelper.LocationCode);
            if (string.IsNullOrEmpty(CheckAuditSetup))
            {
                Session["messageInformation"] = "Please Configure Audit Setup First Before Any Transaction";
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

            ViewBag.Project = accountingReportDal.ReadProject();
            ViewBag.AccountInfo = accountDeptDal.ReadCashBankAccount(objLoginHelper.LocationCode, "100200", objLoginHelper.Location, Helper.SubsidytAccount);

            List<Aud_AdjustmentReasonCodes> newItemAdjustmentReason = new List<Aud_AdjustmentReasonCodes>();
            newItemAdjustmentReason = inventoryDal.AuditAdjustMentReasons(Helper.ReceiveVoucherForAuditor, Helper.ReasonForUserOrAuditor, Helper.ForAccounting);
            ViewBag.AuditReasons = newItemAdjustmentReason;

            ArrayList transactionNumberMax = accountDeptDal.ReadVoucherTransNoMax(objLoginHelper.LocationCode, objLoginHelper.CurrentDate.ToString("yyMMdd"));
            ViewBag.VoucherTransactionNumber = Helper.AccountSequenceNumberGeneration(transactionNumberMax, objLoginHelper);

            return View();
        }

        public ActionResult NonCashVoucherForAuditor()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAuditor, "NonCashVoucherForAuditor", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            string CheckAuditSetup = hrmsDal.ReadAuditSeqNumberAfterCheckFinishedDate(objLoginHelper.LocationCode);
            if (string.IsNullOrEmpty(CheckAuditSetup))
            {
                Session["messageInformation"] = "Please Configure Audit Setup First Before Any Transaction";
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

            ViewBag.Project = accountingReportDal.ReadProject();
            ViewBag.AccountInfo = accountDeptDal.ReadCashBankAccount(objLoginHelper.LocationCode, "100200", objLoginHelper.Location, Helper.SubsidytAccount);

            List<Aud_AdjustmentReasonCodes> newItemAdjustmentReason = new List<Aud_AdjustmentReasonCodes>();
            newItemAdjustmentReason = inventoryDal.AuditAdjustMentReasons(Helper.NonCashVoucherForAuditor, Helper.ReasonForUserOrAuditor, Helper.ForAccounting);
            ViewBag.AuditReasons = newItemAdjustmentReason;

            ArrayList transactionNumberMax = accountDeptDal.ReadVoucherTransNoMax(objLoginHelper.LocationCode, objLoginHelper.CurrentDate.ToString("yyMMdd"));
            ViewBag.VoucherTransactionNumber = Helper.AccountSequenceNumberGeneration(transactionNumberMax, objLoginHelper);

            return View();
        }

        public ActionResult PaymentVoucher()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAccounting, "PaymentVoucher", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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

            ViewBag.Project = accountingReportDal.ReadProject();
            ViewBag.AccountInfo = accountDeptDal.ReadCashBankAccountWithDimension(objLoginHelper.LocationCode, "100200", objLoginHelper.Location, Helper.SubsidytAccount);

            ArrayList transactionNumberMax = accountDeptDal.ReadVoucherTransNoMax(objLoginHelper.LocationCode, objLoginHelper.CurrentDate.ToString("yyMMdd"));
            ViewBag.VoucherTransactionNumber = Helper.AccountSequenceNumberGeneration(transactionNumberMax, objLoginHelper);

            return View();
        }

        public ActionResult ReceiveVoucher()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAccounting, "ReceiveVoucher", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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

            ViewBag.Project = accountingReportDal.ReadProject();
            ViewBag.AccountInfo = accountDeptDal.ReadCashBankAccountWithDimension(objLoginHelper.LocationCode, "100200", objLoginHelper.Location, Helper.SubsidytAccount);

            ArrayList transactionNumberMax = accountDeptDal.ReadVoucherTransNoMax(objLoginHelper.LocationCode, objLoginHelper.CurrentDate.ToString("yyMMdd"));
            ViewBag.VoucherTransactionNumber = Helper.AccountSequenceNumberGeneration(transactionNumberMax, objLoginHelper);

            return View();
        }

        public ActionResult ContraVoucher()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAccounting, "ContraVoucher", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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

            ViewBag.Project = accountingReportDal.ReadProject();
            ViewBag.AccountInfo = accountDeptDal.ReadCashBankAccountWithDimension(objLoginHelper.LocationCode, "100200", objLoginHelper.Location, Helper.SubsidytAccount);

            ArrayList transactionNumberMax = accountDeptDal.ReadVoucherTransNoMax(objLoginHelper.LocationCode, objLoginHelper.CurrentDate.ToString("yyMMdd"));
            ViewBag.VoucherTransactionNumber = Helper.AccountSequenceNumberGeneration(transactionNumberMax, objLoginHelper);

            return View();
        }

        public ActionResult NonCashVoucher()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAccounting, "NonCashVoucher", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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

            ViewBag.Project = accountingReportDal.ReadProject();
            ViewBag.AccountInfo = accountDeptDal.ReadCashBankAccountWithDimension(objLoginHelper.LocationCode, "100200", objLoginHelper.Location, Helper.SubsidytAccount);

            ArrayList transactionNumberMax = accountDeptDal.ReadVoucherTransNoMax(objLoginHelper.LocationCode, objLoginHelper.CurrentDate.ToString("yyMMdd"));
            ViewBag.VoucherTransactionNumber = Helper.AccountSequenceNumberGeneration(transactionNumberMax, objLoginHelper);

            return View();
        }

        public ActionResult OtherAccountSelectionPartial(string projectCode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<ChartOfAccounts> lstChartOfAccounts = accountDeptDal.ReadOtherAccount(objLoginHelper.LocationCode, projectCode, objLoginHelper.Location, Helper.SubsidytAccount);

            return PartialView("OtherAccountSelectionPartial", lstChartOfAccounts);
        }

        public JsonResult SavePaymentOrReceieveVoucherForAuditor(VoucherTransaction objCashBankVoucher, List<VoucherTransaction> lstOtherVoucher, VoucherTransactionAudit objvoucherTransactionAudit, string voucherType)
        {
            try
            {
                objLoginHelper = (LoginHelper)Session["LogInInformation"];

                string EntrySource = string.Empty;
                if (voucherType == "C")
                {
                    EntrySource = Helper.ContraVoucherForAuditorEntrySource;
                }
                else if (voucherType == "N")
                {
                    EntrySource = Helper.NonCashVoucherForAuditorEntrySource;
                }
                else if (voucherType == "P")
                {
                    EntrySource = Helper.PaymentVoucherForAuditorEntrySource;
                }
                else if (voucherType == "R")
                {
                    EntrySource = Helper.ReceiveVoucherForAuditorEntrySource;
                }

                Acc_TransNoCount objAccTransNocount = new Acc_TransNoCount();
                objAccTransNocount = voucherProcess.TransNoCount(objCashBankVoucher, objLoginHelper.LocationCode); //Newly Add By Md.Sultan Mahmud
                objAccTransNocount.TransCount = null;

                Acc_PrePostTransMaster objPrePostTransMaster = new Acc_PrePostTransMaster();
                objPrePostTransMaster = voucherProcess.CashBankVoucherProcess(objCashBankVoucher, voucherType, objLoginHelper, EntrySource);

                List<Acc_PrePostTransDetail> lstPrePostTransDetail = new List<Acc_PrePostTransDetail>();
                lstPrePostTransDetail = voucherProcess.OtherBankVoucherProcess(lstOtherVoucher, voucherType, objLoginHelper, EntrySource, objCashBankVoucher.TransactionNo);

                Aud_AuditAdjustmentRelatedAccountingTransaction objAuditAdjustmentRelatedAccountingTransaction = new Aud_AuditAdjustmentRelatedAccountingTransaction();
                objAuditAdjustmentRelatedAccountingTransaction = voucherProcess.AuditAdjustmentVoucherOrAccount(objvoucherTransactionAudit, objLoginHelper);

                objPrePostTransMaster = accountDeptDal.CreateVoucher(objAccTransNocount, objPrePostTransMaster, lstPrePostTransDetail, objAuditAdjustmentRelatedAccountingTransaction);

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty, Helper.AccountSequenceNumberGeneration(objPrePostTransMaster.TransNo)) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public JsonResult SavePaymentVoucherForAudit(VoucherTransaction objCashBankVoucher, List<VoucherTransaction> lstOtherVoucher, VoucherTransactionAudit objvoucherTransactionAudit, string voucherType, List<SubLedgerHeadDetails> lstSubLedgerHeadDetails)
        {
            try
            {
                objLoginHelper = (LoginHelper)Session["LogInInformation"];

                string EntrySource = string.Empty;
                if (voucherType == "P")
                {
                    EntrySource = Helper.PaymentVoucherForAuditorEntrySource;
                }


                Acc_TransNoCount objAccTransNocount = new Acc_TransNoCount();
                objAccTransNocount = voucherProcess.TransNoCount(objCashBankVoucher, objLoginHelper.LocationCode);
                objAccTransNocount.TransCount = null;

                Acc_PrePostTransMaster objPrePostTransMaster = new Acc_PrePostTransMaster();
                objPrePostTransMaster = voucherProcess.CashBankVoucherProcess(objCashBankVoucher, voucherType, objLoginHelper, EntrySource);

                List<Acc_PrePostTransDetail> lstPrePostTransDetail = new List<Acc_PrePostTransDetail>();
                lstPrePostTransDetail = voucherProcess.OtherBankVoucherProcess(lstOtherVoucher, voucherType, objLoginHelper, EntrySource, objCashBankVoucher.TransactionNo);

                Aud_AuditAdjustmentRelatedAccountingTransaction objAuditAdjustmentRelatedAccountingTransaction = new Aud_AuditAdjustmentRelatedAccountingTransaction();
                objAuditAdjustmentRelatedAccountingTransaction = voucherProcess.AuditAdjustmentVoucherOrAccount(objvoucherTransactionAudit, objLoginHelper);

                List<Acc_PrePostTransDetailByDimension> lstPrePostTransDetailByDimension = new List<Acc_PrePostTransDetailByDimension>();
                lstPrePostTransDetailByDimension = voucherProcess.PrePostTransDetailByDimensionProcess(objCashBankVoucher, lstPrePostTransDetail, lstSubLedgerHeadDetails, objLoginHelper.LocationCode);

                objPrePostTransMaster = accountDeptDal.CreateVoucher(objAccTransNocount, objPrePostTransMaster, lstPrePostTransDetail, objAuditAdjustmentRelatedAccountingTransaction, lstPrePostTransDetailByDimension);

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty, Helper.AccountSequenceNumberGeneration(objPrePostTransMaster.TransNo)) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public JsonResult SaveVoucher(VoucherTransaction objCashBankVoucher, List<VoucherTransaction> lstOtherVoucher, string voucherType, List<SubLedgerHeadDetails> lstSubLedgerHeadDetails)
        {
            try
            {
                objLoginHelper = (LoginHelper)Session["LogInInformation"];

                string EntrySource = string.Empty;
                if (voucherType == "C")
                {
                    EntrySource = Helper.ContraVoucherForAccountsEntrySource;
                }
                else if (voucherType == "N")
                {
                    EntrySource = Helper.NonCashVoucherForAccountsEntrySource;
                }
                else if (voucherType == "P")
                {
                    EntrySource = Helper.PaymentVoucherForAccountsEntrySource;
                }
                else if (voucherType == "R")
                {
                    EntrySource = Helper.ReceiveVoucherForAccountsEntrySource;
                }

                ArrayList transactionNumberMax = accountDeptDal.ReadVoucherTransNoMax(objLoginHelper.LocationCode, objLoginHelper.CurrentDate.ToString("yyMMdd"));
                objCashBankVoucher.TransactionNo = Helper.AccountSequenceNumberGeneration(transactionNumberMax, objLoginHelper);

                Acc_TransNoCount objAccTransNocount = new Acc_TransNoCount();
                objAccTransNocount = voucherProcess.TransNoCount(objCashBankVoucher, objLoginHelper.LocationCode);
                objAccTransNocount.TransCount = null;

                Acc_PrePostTransMaster objPrePostTransMaster = new Acc_PrePostTransMaster();
                objPrePostTransMaster = voucherProcess.CashBankVoucherProcess(objCashBankVoucher, voucherType, objLoginHelper, EntrySource);
                objPrePostTransMaster.IsAutoEntry = false;

                List<Acc_PrePostTransDetail> lstPrePostTransDetail = new List<Acc_PrePostTransDetail>();
                lstPrePostTransDetail = voucherProcess.OtherBankVoucherProcess(lstOtherVoucher, voucherType, objLoginHelper, EntrySource, objCashBankVoucher.TransactionNo);

                List<Acc_PrePostTransDetailByDimension> lstPrePostTransDetailByDimension = new List<Acc_PrePostTransDetailByDimension>();

                if (lstSubLedgerHeadDetails != null)
                    lstPrePostTransDetailByDimension = voucherProcess.PrePostTransDetailByDimensionProcess(objCashBankVoucher, lstPrePostTransDetail, lstSubLedgerHeadDetails, objLoginHelper.LocationCode);

                objPrePostTransMaster = accountDeptDal.CreateVoucher(objAccTransNocount,objPrePostTransMaster, lstPrePostTransDetail, lstPrePostTransDetailByDimension);

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty, Helper.AccountSequenceNumberGeneration(objPrePostTransMaster.TransNo)) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        [GridAction]
        public ActionResult SubLedgerHeadDetailsLoad(string dimensionCode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<SubLedgerHeadDetails> lstSubLedgerHeadDetails = new List<SubLedgerHeadDetails>();
            lstSubLedgerHeadDetails = accountDeptDal.GetSubLedgerHeadDetails(dimensionCode, objLoginHelper.LocationCode);

            return View(new GridModel<SubLedgerHeadDetails>()
            {
                Data = lstSubLedgerHeadDetails
            });
        }

        public JsonResult AccountWiseSubledgerHead(string dimensionCode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<SubLedgerHeadDetails> lstSubLedgerHeadDetails = new List<SubLedgerHeadDetails>();
            lstSubLedgerHeadDetails = accountDeptDal.GetSubLedgerHeadDetails(dimensionCode, objLoginHelper.LocationCode);

            return new JsonResult { Data = lstSubLedgerHeadDetails };
        }

        public JsonResult SaveEmployeeWiseSalaryPayment(VoucherTransaction objCashBankVoucher, List<VoucherTransaction> lstOtherVoucher, List<EmployeeWiseSalaryPayment> lstEmployeeWiseSalary, string voucherType)
        {
            try
            {
                objLoginHelper = (LoginHelper)Session["LogInInformation"];

                string EntrySource = string.Empty;
                EntrySource = Helper.BankPaymentVoucherForAccountsEntrySource;

                ArrayList transactionNumberMax = accountDeptDal.ReadVoucherTransNoMax(objLoginHelper.LocationCode, objLoginHelper.CurrentDate.ToString("yyMMdd"));
                objCashBankVoucher.TransactionNo = Helper.AccountSequenceNumberGeneration(transactionNumberMax, objLoginHelper);

                Acc_TransNoCount objAccTransNocount = new Acc_TransNoCount();
                objAccTransNocount = voucherProcess.TransNoCount(objCashBankVoucher, objLoginHelper.LocationCode);
                objAccTransNocount.TransCount = null;

                Acc_PrePostTransMaster objPrePostTransMaster = new Acc_PrePostTransMaster();
                objPrePostTransMaster = voucherProcess.CashBankVoucherProcess(objCashBankVoucher, voucherType, objLoginHelper, EntrySource);
                objPrePostTransMaster.IsAutoEntry = false;

                List<Acc_PrePostTransDetail> lstPrePostTransDetail = new List<Acc_PrePostTransDetail>();
                lstPrePostTransDetail = voucherProcess.OtherBankVoucherProcess(lstOtherVoucher, voucherType, objLoginHelper, EntrySource, objCashBankVoucher.TransactionNo);

                List<SubLedgerHeadDetails> lstSubLedgerHeadDetails = new List<SubLedgerHeadDetails>();
                lstSubLedgerHeadDetails = voucherProcess.PrepareEmployeeWiseSalaryToSubLedgerDimension(lstEmployeeWiseSalary, objLoginHelper.LocationCode, objCashBankVoucher.TransactionNo);

                List<Acc_PrePostTransDetailByDimension> lstTransDetailByDimension = new List<Acc_PrePostTransDetailByDimension>();
                lstTransDetailByDimension = voucherProcess.PrePostTransDetailByDimensionProcess(objCashBankVoucher, lstPrePostTransDetail, lstSubLedgerHeadDetails, objLoginHelper.LocationCode);

              
                //start oroginal
                
                //objPrePostTransMaster = accountDeptDal.CreateVoucher(objPrePostTransMaster, lstEmployeeWiseSalary);

               // objPrePostTransMaster = accountDeptDal.CreateVoucher(objAccTransNocount, objPrePostTransMaster, lstPrePostTransDetail, lstTransDetailByDimension);
                
                //end oroginal


                //start new 
                string supportMethod = "ttd";//its use for support method
                objPrePostTransMaster = accountDeptDal.CreateVoucherForSaveEmployeeWiseSalaryPayment(objAccTransNocount, lstEmployeeWiseSalary, lstPrePostTransDetail, lstTransDetailByDimension, objPrePostTransMaster, supportMethod);
                //end new


                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty, Helper.AccountSequenceNumberGeneration(objPrePostTransMaster.TransNo)) };
            }
            catch (Exception ex)
            {
              
                    return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }
    }
}
