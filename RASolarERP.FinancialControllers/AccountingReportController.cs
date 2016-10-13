using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RASolarHelper;
using RASolarERP.Model;
using RASolarAMS.Model;
using RASolarERP.Web.Areas.Financial.Models;
using RASolarERP.Web.Models;
using RASolarERP.DomainModel.AMSModel;
using Telerik.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections;
using System.IO;
using System.Text;

namespace RASolarERP.Web.Areas.Financial.Controllers
{
    public class AccountingReportController : Controller
    {
        private AccountingReportData accountingDal = new AccountingReportData();
        private RASolarERPData erpDal = new RASolarERPData();
        LoginHelper objLoginHelper = new LoginHelper();
        RASolarSecurityData securityDal = new RASolarSecurityData();

        string message = string.Empty;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AccountingSummary()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAccounting, "AccountingSummary", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            List<AccountingDataEntryStatus> lstAccountingStatus = new List<AccountingDataEntryStatus>();
            lstAccountingStatus = accountingDal.ReadAccountingDataEntryStatus(objLoginHelper.ReportType, objLoginHelper.LocationCode, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForAccounting));

            ViewBag.IsAuthenticForSave = objLoginHelper.IsAuthenticApproverForThisLocation;

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

            ViewBag.TotalNumberofUnits = lstAccountingStatus.Count();
            ViewBag.EntryfinalizedbyRM = lstAccountingStatus.Count(s => s.FinalizedByRM != null);
            ViewBag.EntryFinalizedbyZM = lstAccountingStatus.Count(s => s.FinalizedByZM != null);
            ViewBag.EntryfinalizedbyHO = lstAccountingStatus.Count(s => s.FinalizedByHO != null);

            TempData["AccountOpenMonth"] = objLoginHelper.MonthOpenForAccounting.ToString("MMM, yyyy");

            return View(lstAccountingStatus);
        }

        public ActionResult TrialBalance()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAccounting, "TrialBalance", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            List<ProjectInfo> lstTproject = new List<ProjectInfo>();
            lstTproject = accountingDal.ReadProject().Where(i => i.ProjectCode == "100200" || i.ProjectCode == "100500" || i.ProjectCode == "100100" || i.ProjectCode == "101400").ToList();
            ArrayList arrayProject = new ArrayList();

            if (objLoginHelper.UerRoleOrGroupID == UserGroup.UnitAuditor || objLoginHelper.UerRoleOrGroupID == UserGroup.UnitOfficeUser)
            {
                arrayProject.Add(new { Display = "All Project", Value = "0" });
                foreach (ProjectInfo p in lstTproject)
                {
                    arrayProject.Add(new { Display = p.ProjectName, Value = p.ProjectCode });
                }

                ViewBag.ClosingBlanceUnitHide = false;
                ViewBag.ClosingBlanceHOHide = true;
                ViewBag.DisableReportType = true;
            }
            else
            {
                arrayProject.Add(new { Display = "All Project", Value = "0" });
                foreach (ProjectInfo p in lstTproject)
                {
                    arrayProject.Add(new { Display = p.ProjectName, Value = p.ProjectCode });
                }

                ViewBag.ClosingBlanceUnitHide = true;
                ViewBag.ClosingBlanceHOHide = false;
                ViewBag.DisableReportType = false;
            }
            TempData["Projects"] = arrayProject;

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForAccounting.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.FirstDayOfTheMonth = Helper.MonthStartDate(objLoginHelper.TransactionOpenDate.ToString("yyyyMM"));
            ViewBag.ToDate = objLoginHelper.TransactionOpenDate.ToString("dd-MMM-yyyy");

            return View();
        }

        public ActionResult CustomerTrainingSummaryReport()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAccounting, "CustomerTrainingSummaryReport", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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

            return View();
        }

        public ActionResult CustomerTrainingDetailsReport()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAccounting, "CustomerTrainingDetailsReport", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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

            return View();
        }

        public ActionResult MonthlyAccountSummaryStatement()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAccounting, "MonthlyAccountSummaryStatement", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            MonthlyAccountingSummaryStatement objMonthlyAccountingSummary = new MonthlyAccountingSummaryStatement();

            if (Session["unitCodeForMonthyAccountSummaryStatement"] != null)
            {
                objMonthlyAccountingSummary = accountingDal.ReadMonthlyAccountingSummaryStatement(Session["unitCodeForMonthyAccountSummaryStatement"].ToString(), "", "");
                //Session.Remove("unitCodeForMonthyAccountSummaryStatement");
            }
            else
            {
                objMonthlyAccountingSummary = accountingDal.ReadMonthlyAccountingSummaryStatement(objLoginHelper.LocationCode, "", "");
            }

            @ViewBag.AccountingSummary = objMonthlyAccountingSummary;

            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForAccounting.ToString("MMMM-yy");
            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.UnitCode = objLoginHelper.LocationCode;

            ViewBag.LocationCode = objLoginHelper.LocationCode;

            if (Session["unitCodeForMonthyAccountSummaryStatement"] != null)
            {
                ViewBag.UnitTitle = "Unit : ";
                ViewBag.UnitName = "[ " + Session["unitCodeForMonthyAccountSummaryStatement"].ToString() + " ] " + Session["unitNameForMonthyAccountSummaryStatement"].ToString();

                ViewBag.LocationCode = Session["unitCodeForMonthyAccountSummaryStatement"];

                Session.Remove("unitCodeForMonthyAccountSummaryStatement");
                Session.Remove("unitNameForMonthyAccountSummaryStatement");

            }

            return View();
        }


      

        public ActionResult MonthlyCollectionPaymentSummaryStatement()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAccounting, "MonthlyAccountSummaryStatement", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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


            return View();
        }

        public ActionResult MonthlyCollectionPaymentSave(decimal txtSolarMulProject, decimal txtSolarIdclProject, decimal txtKhusraMlBikrySolar, decimal txtBiogazProject, decimal txtTechFeeBiogaz, decimal txtKhusraMlBikryBiogaz, decimal txtJnTechTkPrapti, decimal txtUpVaraPrapti, decimal txtJTCSecurityJoma, decimal txtPrakritikDurjogTohbilePrapti, decimal txtTatkhonikCkriObatiBbdPrapti, decimal txtBondhuBati, decimal txtOuditApttiTkPraptiNam, decimal txtOnnoUnitThekePraptiUniterNam, decimal txt, decimal txtBankJoma, decimal txtOnlineTransfer, decimal txtMashikNogodKhoros, decimal txtGrahokkNogodFerot, decimal txtBiogazBbdConstrcKhoros, decimal txtBiogazBbdOgrimConstrcKhoros, decimal txtOnnoUniteProdan, decimal txtOditApttiTohbilTsrpNam, decimal txtOgrimBariVaraProdan, decimal txtTwo)
        { 
                    try
                    {

                        objLoginHelper = (LoginHelper)Session["LogInInformation"];

                        Acc_MonthlyCashReceiptPaymentTransaction objMonthlyCashReceiptPaymentTransaction = new Acc_MonthlyCashReceiptPaymentTransaction();

                        objMonthlyCashReceiptPaymentTransaction.YearMonth = objLoginHelper.MonthOpenForAccounting.ToString("yyMMdd");
                        objMonthlyCashReceiptPaymentTransaction.LocationCode = objLoginHelper.LocationCode;

                        objMonthlyCashReceiptPaymentTransaction.Received_SHSMainProject=txtSolarMulProject;
                        objMonthlyCashReceiptPaymentTransaction.Received_SHSIDCOLProject = txtSolarIdclProject;

                        objMonthlyCashReceiptPaymentTransaction.Received_SparePartsSalesForSHS = txtKhusraMlBikrySolar;

                        objMonthlyCashReceiptPaymentTransaction.Received_BioGas = txtBiogazProject;

                        objMonthlyCashReceiptPaymentTransaction.Received_TechnicalFeeForBioGas = txtTechFeeBiogaz;

                        objMonthlyCashReceiptPaymentTransaction.Received_SparePartsSalesForBioGas = txtKhusraMlBikryBiogaz;

                        objMonthlyCashReceiptPaymentTransaction.Received_SecurityDepositFromJTC = txtJTCSecurityJoma;

                        objMonthlyCashReceiptPaymentTransaction.Received_BandhuBatti = txtBondhuBati;

                        objMonthlyCashReceiptPaymentTransaction.Received_FromSubletRent = txtUpVaraPrapti;

                        objMonthlyCashReceiptPaymentTransaction.Received_DRFAgreement = 0;

                        objMonthlyCashReceiptPaymentTransaction.Received_InstantRelease = txtTatkhonikCkriObatiBbdPrapti;

                        objMonthlyCashReceiptPaymentTransaction.Received_AuditClaim = txtOuditApttiTkPraptiNam;

                        objMonthlyCashReceiptPaymentTransaction.Received_AuditClaimRemarks = "";

                        objMonthlyCashReceiptPaymentTransaction.Received_FromOtherUnit = txtOnnoUnitThekePraptiUniterNam;

                        //Payment decimal txtBankJoma, decimal txtOnlineTransfer, decimal txtMashikNogodKhoros, decimal txtGrahokkNogodFerot, decimal txtBiogazBbdConstrcKhoros, decimal txtBiogazBbdOgrimConstrcKhoros, decimal txtOnnoUniteProdan, decimal txtOditApttiTohbilTsrpNam, decimal txtOgrimBariVaraProdan, decimal txtTwo)

                        objMonthlyCashReceiptPaymentTransaction.Payment_BankDeposit = txtBankJoma;

                        objMonthlyCashReceiptPaymentTransaction.Payment_OnlineTransfer = txtOnlineTransfer;

                        objMonthlyCashReceiptPaymentTransaction.Payment_MonthlyExpenses = txtMashikNogodKhoros;

                        objMonthlyCashReceiptPaymentTransaction.Payment_ToCustomerForSystemReturn = txtGrahokkNogodFerot;

                        objMonthlyCashReceiptPaymentTransaction.Payment_ExpensesForBioGasConstruction = txtBiogazBbdConstrcKhoros;

                        objMonthlyCashReceiptPaymentTransaction.Payment_AdvanceForBioGasConstruction = txtBiogazBbdOgrimConstrcKhoros;

                        objMonthlyCashReceiptPaymentTransaction.Payment_ToOtherUnit = txtOnnoUniteProdan;

                        objMonthlyCashReceiptPaymentTransaction.Payment_CashShortageAndAuditClaimReceivable = txtOditApttiTohbilTsrpNam;

                        objMonthlyCashReceiptPaymentTransaction.Payment_AdvanceOfficeRent = txtOgrimBariVaraProdan;

                        objMonthlyCashReceiptPaymentTransaction.CheckedNFinalizedByUM = DateTime.Now;

                        objMonthlyCashReceiptPaymentTransaction = accountingDal.Create(objMonthlyCashReceiptPaymentTransaction);

                        return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Monthly Collection Receive Payment" + Helper.SuccessMessage) };
                    }
                    catch (Exception ex)
                    {
                        return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
                    }
        }

        public ActionResult MonthlyReceiptPaymentSummery() 
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAccounting, "MonthlyReceiptPaymentSummery", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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


            return View();
        }
        public ActionResult MonthlyReceiptPaymentSummeryForHoafadmin() 
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAccounting, "MonthlyReceiptPaymentSummery", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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


            return View();
        }

        public ActionResult UnitWiseMonthyAccountSummaryStatement(string unitCode, string unitName)
        {
            Session["unitCodeForMonthyAccountSummaryStatement"] = unitCode;
            Session["unitNameForMonthyAccountSummaryStatement"] = unitName;
            return RedirectToAction("MonthlyAccountSummaryStatement", "AccountingReport");
        }

        public ActionResult UnitWiseBreakupAnAccountHead(string accn, string accnam, string sd, string ed, string pc)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAccounting, "UnitWiseBreakupAnAccountHead", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForAccounting.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.DateFrom = sd;
            ViewBag.DateTo = ed;
            ViewBag.AccountName = accnam;
            ViewBag.AccountNumber = accn;

            if (pc == "0")
            {
                ViewBag.ProjectCode = "All Project";
                pc = string.Empty;
            }
            else
            {
                ViewBag.ProjectCode = accountingDal.ReadProject(pc).ProjectName + " [" + pc + "]";
            }

            List<AccTrialBalanceInDetailByAccount> lstTrialBlanceBrakeDown = new List<AccTrialBalanceInDetailByAccount>();
            lstTrialBlanceBrakeDown = accountingDal.ReadTrialBalanceInDetailByAccount(Convert.ToDateTime(Helper.DateFormatMMDDYYYY(sd)), Convert.ToDateTime(Helper.DateFormatMMDDYYYY(ed)), pc, accn);

            decimal openingBalance = 0, closingBalance = 0, debitAmount = 0, creditAmount = 0;

            openingBalance = lstTrialBlanceBrakeDown.Sum(s => s.OpeningBalance);
            closingBalance = (decimal)lstTrialBlanceBrakeDown.Sum(s => s.ClosingBalance);
            debitAmount = lstTrialBlanceBrakeDown.Sum(s => s.DebitAmount);
            creditAmount = lstTrialBlanceBrakeDown.Sum(s => s.CreditAmount);

            TempData["openingBalance"] = openingBalance.ToString("0,0.");
            TempData["closingBalance"] = closingBalance.ToString("0,0.");
            TempData["debitAmount"] = debitAmount.ToString("0,0.");
            TempData["creditAmount"] = creditAmount.ToString("0,0.");

            ViewBag.TrialBalanceBreakDown = lstTrialBlanceBrakeDown;

            return View();
        }

        public JsonResult SaveAccountingStatus(string unitCode)
        {
            string saveSuccessMessage = string.Empty;
            //objLoginHelper = (LoginHelper)Session["LogInInformation"];

            //tbl_UnitWiseEntryStatus objUnitWiseEntryStatus = new tbl_UnitWiseEntryStatus();
            //RASolarSecurityData objDal = new RASolarSecurityData();

            //objUnitWiseEntryStatus = erpDal.UnitWiseEntryStatus(unitCode, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForAccounting));

            //if (objLoginHelper.Location == Helper.Zone)
            //{
            //    objUnitWiseEntryStatus.AccFinalizedByZM_UserName = objLoginHelper.LogInID;
            //    objUnitWiseEntryStatus.AccCheckedNFinalizedByZM = DateTime.Now;
            //}
            //else if (objLoginHelper.Location == Helper.Region)
            //{
            //    objUnitWiseEntryStatus.AccFinalizedByRM_UserName = objLoginHelper.LogInID;
            //    objUnitWiseEntryStatus.AccCheckedNFinalizedByRM = DateTime.Now;
            //}
            //else if (objLoginHelper.Location == Helper.HeadOffice)
            //{
            //    objUnitWiseEntryStatus.AccFinalizedByHO_UserName = objLoginHelper.LogInID;
            //    objUnitWiseEntryStatus.AccCheckedNFinalizedByHO = DateTime.Now;
            //}

            //objUnitWiseEntryStatus.UserName = objLoginHelper.LogInUserName;
            //objUnitWiseEntryStatus = accountingDal.UpdateAccountEntryStatus(objUnitWiseEntryStatus);

            //if (objUnitWiseEntryStatus != null)
            //{
            //    saveSuccessMessage = "Succeed";
            //}
            //else
            //{
            //    saveSuccessMessage = "NotSucceed";
            //}

            return new JsonResult { Data = saveSuccessMessage };
        }

        [GridAction]
        public ActionResult _AccShowAccountWiseBreakdown(string transType, string unitCode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            return View(new GridModel<AccShowAccountWiseBreakdown>
            {
                Data = accountingDal.ReadAccShowAccountWiseBreakdown(unitCode, Helper.ConvertDateToYearMonth(objLoginHelper.MonthOpenForAccounting), transType)
            });
        }

        public ActionResult GeneralLedger()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            Session.Remove("LedgerParamaterAjaxCall");

            if (!securityDal.IsPageAccessible(Helper.ForAccounting, "GeneralLedger", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            TempData["Projects"] = accountingDal.ReadProject().Where(i => i.ProjectCode == "100200" || i.ProjectCode == "100500" || i.ProjectCode == "100100" || i.ProjectCode == "101400").ToList();

            TempData["AccountLevel"] = accountingDal.ChartOfAccountLevel();


            if (Session["accNumberProjectCode"] != null)
            {
                string[] accProject = ((string)Session["accNumberProjectCode"]).Split(',');
                ViewBag.SelectedProject = accProject[1];
                ViewBag.SelectedAccounts = accProject[0];
            }
            else
            {
                ViewBag.SelectedProject = "100200";
                ViewBag.SelectedAccounts = "105063010";
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

            ViewBag.FirstDayOfTheMonth = Helper.MonthStartDate(objLoginHelper.TransactionOpenDate.ToString("yyyyMM"));
            ViewBag.ToDate = objLoginHelper.TransactionOpenDate.ToString("dd-MMM-yyyy");

            string locationCode = string.Empty;
            string location = string.Empty;

            if (Session["unitCodeForSpeceficUnit"] != null)
            {
                ViewBag.UnitTitle = "Unit: ";
                ViewBag.UnitName = "[ " + Session["unitCodeForSpeceficUnit"].ToString() + " ] " + Session["unitNameForSpeceficUnitName"];
                ViewBag.UnitCode = Session["unitCodeForSpeceficUnit"].ToString();
                locationCode = Session["unitCodeForSpeceficUnit"].ToString();

                LocationInfo objLocation = new LocationInfo();
                objLocation = erpDal.Location(locationCode);
                location = Helper.UserLocationTypeName(objLocation.LocationType);
            }
            else
            {
                ViewBag.UnitCode = objLoginHelper.LocationCode;
                locationCode = objLoginHelper.LocationCode;
                location = objLoginHelper.Location;
            }

            TempData["ChartOfAccounts"] = accountingDal.ReadChartOfAccountsForGLLedger(locationCode, "100200", location, Helper.SubsidytAccount);


            return View();

        }

        [GridAction]
        public ActionResult _GeneralLedgerShow(string fromDate, string todate, string projectCode, string accountNo, bool allLocation)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];


            //if (string.IsNullOrEmpty(fromDate))
            //{
            //    if (Session["LedgerParamaterAjaxCall"] != null)
            //    {
            //        string[] ajaxParams = ((string)Session["LedgerParamaterAjaxCall"]).Split(',');

            //        fromDate = ajaxParams[0];
            //        todate = ajaxParams[1];
            //        projectCode = ajaxParams[2];
            //        accountNo = ajaxParams[3];
            //    }
            //}
            //else
            //{
            //    Session["LedgerParamaterAjaxCall"] = fromDate + "," + todate + "," + projectCode + "," + accountNo;
            //}

            try
            {
                List<GLAccountLedger> lstGeneralLedger = new List<GLAccountLedger>();
                if (Session["unitCodeForSpeceficUnit"] != null)
                {
                    lstGeneralLedger = accountingDal.ReadGLAccountLedger(Session["unitCodeForSpeceficUnit"].ToString(), Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy"), Convert.ToDateTime(todate).ToString("dd-MMM-yyyy"), accountNo, projectCode.ToString());
                }
                else
                {
                    if (allLocation == true)
                        lstGeneralLedger = accountingDal.ReadGLAccountLedger("", Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy"), Convert.ToDateTime(todate).ToString("dd-MMM-yyyy"), accountNo, projectCode.ToString());
                   else
                        lstGeneralLedger = accountingDal.ReadGLAccountLedger(objLoginHelper.LocationCode, Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy"), Convert.ToDateTime(todate).ToString("dd-MMM-yyyy"), accountNo, projectCode.ToString());
                }
                return View(new GridModel<GLAccountLedger>
                {
                    Data = lstGeneralLedger
                });
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = "" };
            }
        }

        [GridAction]
        public ActionResult _TrialBalanceShow(string startDate, string endDate, string projectCode, string reportType)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            int levelCode = 8;
            decimal openingB = 0, closingB = 0, debit = 0, credit = 0;
            string reportTypeToShow = string.Empty, locationCode = string.Empty;

            if (reportType == "INDIVIDUALLOCATION")
            {
                reportTypeToShow = reportType;
                locationCode = objLoginHelper.LocationCode;
            }
            else if (reportType == "SUMMARY")
            {
                if (objLoginHelper.UerRoleOrGroupID == UserGroup.HeadOfficeFinanceAccount || objLoginHelper.UerRoleOrGroupID == UserGroup.HeadOfficeAccountAdvanceUsers || objLoginHelper.UerRoleOrGroupID == UserGroup.RacoReviewer || objLoginHelper.UerRoleOrGroupID == UserGroup.HeadOfficeAccountEntryUsers)
                {
                    reportTypeToShow = Helper.HOAccounts;
                }
                else if (objLoginHelper.Location == Helper.Zone)
                {
                    reportTypeToShow = Helper.ZonalOffice;
                    locationCode = objLoginHelper.LocationCode;
                }
                else if (objLoginHelper.Location == Helper.Region)
                {
                    reportTypeToShow = Helper.RegionalOffice;
                    locationCode = objLoginHelper.LocationCode;
                }
            }

            projectCode = projectCode != "0" ? projectCode : string.Empty;

            try
            {
                List<TrialBalanceReport> trialBalanceReport = new List<TrialBalanceReport>();
                trialBalanceReport = accountingDal.ReadTrialBalance(reportTypeToShow, locationCode, startDate, endDate, projectCode);

                //if (objLoginHelper.UerRoleOrGroupID == UserGroup.HeadOfficeFinanceAccount)
                //{
                //    if (Project != "0")
                //    {
                //        projectCode = Project;
                //    }

                //    if (reportType == "INDIVIDUALLOCATION")
                //    {
                //        trialBalanceReport = accountingDal.ReadTrialBalance(reportType, objLoginHelper.LocationCode, startDate, endDate, projectCode);
                //    }
                //    else if (reportType == "SUMMARY")
                //    {
                //        trialBalanceReport = accountingDal.ReadTrialBalance(Helper.HOAccounts, string.Empty, startDate, endDate, projectCode);
                //    }
                //}
                //else if (objLoginHelper.Location == Helper.Zone)
                //{
                //    trialBalanceReport = accountingDal.ReadTrialBalance(Helper.ZonalOffice, objLoginHelper.LocationCode, startDate, endDate, projectCode);
                //}
                //else if (objLoginHelper.Location == Helper.Region)
                //{
                //    trialBalanceReport = accountingDal.ReadTrialBalance(Helper.RegionalOffice, objLoginHelper.LocationCode, startDate, endDate, projectCode);
                //}

                for (int i = 0; i < trialBalanceReport.Count; i++)
                {
                    if (trialBalanceReport[i].ParCapSub == "S")
                    {
                        openingB += Convert.ToDecimal(trialBalanceReport[i].OpeningB);
                        closingB += Convert.ToDecimal(trialBalanceReport[i].ClosingB);
                        debit += Convert.ToDecimal(trialBalanceReport[i].Debit);
                        credit += Convert.ToDecimal(trialBalanceReport[i].Credit);
                    }

                    if (Convert.ToInt32(trialBalanceReport[i].AccLevel) > levelCode)
                    {
                        trialBalanceReport.RemoveAt(i);
                        i = i - 1;
                    }
                }

                TempData["OpeningBalance"] = Convert.ToDecimal(trialBalanceReport.Sum(t => t.OpeningB)).ToString("0,0.");
                TempData["Debit"] = Convert.ToDecimal(trialBalanceReport.Sum(t => t.Debit)).ToString("0,0.");
                TempData["Credit"] = Convert.ToDecimal(trialBalanceReport.Sum(t => t.Credit)).ToString("0,0.");
                TempData["ClosingBalance"] = Convert.ToDecimal(trialBalanceReport.Sum(t => t.ClosingB)).ToString("0,0.");

                return View(new GridModel<TrialBalanceReport>
                {
                    Data = trialBalanceReport
                });
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = "" };
            }
        }

        public ActionResult ExportToPdfTrialBalance(int page, string groupby, string orderBy, string filter, string reportType, string datefrom, string dateTo, string projectCode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            List<TrialBalanceReport> trialBalanceReport = new List<TrialBalanceReport>();

            string reportTypeToShow = string.Empty, locationCode = string.Empty;

            if (reportType == "INDIVIDUALLOCATION")
            {
                reportTypeToShow = reportType;
                locationCode = objLoginHelper.LocationCode;
            }
            else if (reportType == "SUMMARY")
            {
                if (objLoginHelper.UerRoleOrGroupID == UserGroup.HeadOfficeFinanceAccount)
                {
                    reportTypeToShow = Helper.HOAccounts;
                }
                else if (objLoginHelper.Location == Helper.Zone)
                {
                    reportTypeToShow = Helper.ZonalOffice;
                }
                else if (objLoginHelper.Location == Helper.Region)
                {
                    reportTypeToShow = Helper.RegionalOffice;
                }
            }

            if (Session["unitCodeForSpeceficUnit"] != null)
            {
                locationCode = Session["unitCodeForSpeceficUnit"].ToString();
            }

            projectCode = projectCode != "0" ? projectCode : string.Empty;

            trialBalanceReport = accountingDal.ReadTrialBalance(reportTypeToShow, locationCode, datefrom, dateTo, projectCode);

            var v = from u in trialBalanceReport
                    where u.AccNo == "105063010" ||
                        u.AccNo == "105080001" || u.AccNo == "105080002" || u.AccNo == "105080003" || u.AccNo == "105080004" ||
                        u.AccNo == "105080005" || u.AccNo == "105080006" || u.AccNo == "105080007" || u.AccNo == "105080008" ||
                        u.AccNo == "105080009" || u.AccNo == "105080010" || u.AccNo == "105080011" || u.AccNo == "105080012" ||
                        u.AccNo == "105080013"
                    select u;

            trialBalanceReport = v.ToList();

            //if (projectCode == "0")
            //{
            //    trialBalanceReport = accountingDal.ReadTrialBalance(datefrom, dateTo, string.Empty, string.Empty);

            //    var v = from u in trialBalanceReport
            //            where u.AccNo == "105063010" ||
            //                u.AccNo == "105080001" || u.AccNo == "105080002" || u.AccNo == "105080003" || u.AccNo == "105080004" ||
            //                u.AccNo == "105080005" || u.AccNo == "105080006" || u.AccNo == "105080007" || u.AccNo == "105080008" ||
            //                u.AccNo == "105080009" || u.AccNo == "105080010" || u.AccNo == "105080011" || u.AccNo == "105080012" ||
            //                u.AccNo == "105080013"
            //            select u;

            //    trialBalanceReport = v.ToList();
            //}
            //else if (objLoginHelper.Location == Helper.HeadOffice)
            //{
            //    trialBalanceReport = accountingDal.ReadTrialBalance(datefrom, dateTo, string.Empty, projectCode);

            //    var v = from u in trialBalanceReport
            //            where u.AccNo == "105063010" ||
            //                u.AccNo == "105080001" || u.AccNo == "105080002" || u.AccNo == "105080003" || u.AccNo == "105080004" ||
            //                u.AccNo == "105080005" || u.AccNo == "105080006" || u.AccNo == "105080007" || u.AccNo == "105080008" ||
            //                u.AccNo == "105080009" || u.AccNo == "105080010" || u.AccNo == "105080011" || u.AccNo == "105080012" ||
            //                u.AccNo == "105080013"
            //            select u;

            //    trialBalanceReport = v.ToList();
            //}
            //else
            //{
            //    trialBalanceReport = accountingDal.ReadTrialBalance(datefrom, dateTo, objLoginHelper.LocationCode, projectCode);

            //    var v = from u in trialBalanceReport
            //            where u.AccNo == "105063010" ||
            //                u.AccNo == "105080001" || u.AccNo == "105080002" || u.AccNo == "105080003" || u.AccNo == "105080004" ||
            //                u.AccNo == "105080005" || u.AccNo == "105080006" || u.AccNo == "105080007" || u.AccNo == "105080008" ||
            //                u.AccNo == "105080009" || u.AccNo == "105080010" || u.AccNo == "105080011" || u.AccNo == "105080012" ||
            //                u.AccNo == "105080013"
            //            select u;
            //    trialBalanceReport = v.ToList();
            //}

            Document pdfDocumnet = new Document(PageSize.A4, 35, 20, 15, 10);
            MemoryStream memStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(pdfDocumnet, memStream);

            string projectName = accountingDal.ReadProject(projectCode).ProjectName;

            StringBuilder headerStringCompanyTitle = new StringBuilder();
            StringBuilder headerStringReportTitle = new StringBuilder();
            headerStringCompanyTitle.AppendFormat(Helper.RSFAddress);
            headerStringReportTitle.AppendFormat("Trial Balance Report \n Date From: {0} To: {1} \n {2}", datefrom, dateTo, projectName);

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
            float[] widths = new float[] { 12f, 40f, 12f, 12f, 12f, 12f };

            PdfPTable pdfTable = new PdfPTable(numOfColumns);
            pdfTable.SetWidths(widths);
            pdfTable.WidthPercentage = 100;
            pdfTable.DefaultCell.Padding = 3;
            pdfTable.DefaultCell.BorderWidth = 0.5F;
            pdfTable.DefaultCell.BorderColor = new Color(221, 218, 218);
            pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfTable.DefaultCell.BackgroundColor = new Color(235, 235, 235);

            Font fontForTableHeader = new Font(Font.COURIER, 10, Font.BOLD, new Color(0, 0, 0));
            pdfTable.AddCell(new Phrase("Account No", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Account Name", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Opening Balance", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Debit", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Credit", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Closing Balance", fontForTableHeader));

            pdfTable.HeaderRows = 1;
            pdfTable.DefaultCell.BorderWidth = 0.5F;
            pdfTable.DefaultCell.BorderColor = new Color(195, 195, 195);
            pdfTable.DefaultCell.BackgroundColor = new Color(255, 255, 255);
            Font fontForContent = new Font(Font.COURIER, 8, Font.NORMAL, new Color(0, 0, 0));

            foreach (TrialBalanceReport tb in trialBalanceReport)
            {
                pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.AddCell(new Phrase(tb.AccNo, fontForContent));
                pdfTable.AddCell(new Phrase(tb.AccName, fontForContent));

                pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                pdfTable.AddCell(new Phrase(tb.OpeningB.ToString("#,##0"), fontForContent));
                pdfTable.AddCell(new Phrase(tb.Debit.ToString("#,##0"), fontForContent));
                pdfTable.AddCell(new Phrase(tb.Credit.ToString("#,##0"), fontForContent));
                pdfTable.AddCell(new Phrase(tb.ClosingB.ToString("#,##0"), fontForContent));
            }

            pdfDocumnet.Add(pdfTable);
            pdfDocumnet.Close();

            return File(memStream.ToArray(), "application/pdf", "RSF-TrialBalance.pdf");
        }

        public ActionResult ExportToCsvTrialBalance(int page, string groupby, string orderBy, string filter, string reportType, string datefrom, string dateTo, string projectCode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            List<TrialBalanceReport> trialBalanceReport = new List<TrialBalanceReport>();

            string reportTypeToShow = string.Empty, locationCode = string.Empty;

            if (reportType == "INDIVIDUALLOCATION")
            {
                reportTypeToShow = reportType;
                locationCode = objLoginHelper.LocationCode;
            }
            else if (reportType == "SUMMARY")
            {
                if (objLoginHelper.UerRoleOrGroupID == UserGroup.HeadOfficeFinanceAccount)
                {
                    reportTypeToShow = Helper.HOAccounts;
                }
                else if (objLoginHelper.Location == Helper.Zone)
                {
                    reportTypeToShow = Helper.ZonalOffice;
                }
                else if (objLoginHelper.Location == Helper.Region)
                {
                    reportTypeToShow = Helper.RegionalOffice;
                }
            }

            if (Session["unitCodeForSpeceficUnit"] != null)
            {
                locationCode = Session["unitCodeForSpeceficUnit"].ToString();
            }

            projectCode = projectCode != "0" ? projectCode : string.Empty;

            trialBalanceReport = accountingDal.ReadTrialBalance(reportTypeToShow, locationCode, datefrom, dateTo, projectCode);

            //if (projectCode == "0")
            //{
            //    trialBalanceReport = accountingDal.ReadTrialBalance(datefrom, dateTo, string.Empty, string.Empty);
            //}
            //else if (objLoginHelper.Location == Helper.HeadOffice)
            //{
            //    trialBalanceReport = accountingDal.ReadTrialBalance(datefrom, dateTo, string.Empty, projectCode);
            //}
            //else
            //{
            //    trialBalanceReport = accountingDal.ReadTrialBalance(datefrom, dateTo, objLoginHelper.LocationCode, projectCode);
            //}

            MemoryStream output = new MemoryStream();
            StreamWriter writer = new StreamWriter(output, Encoding.UTF8);

            writer.Write("Acount No,");
            writer.Write("Acount Name,");
            writer.Write("Opening Balance,");
            writer.Write("Debit,");
            writer.Write("Credit,");
            writer.Write("Closing Balance");
            writer.WriteLine();

            foreach (TrialBalanceReport tb in trialBalanceReport)
            {
                writer.Write(tb.AccNo);
                writer.Write(",");

                writer.Write("\"");
                writer.Write(tb.AccName);
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(tb.OpeningB.ToString("0"));
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(tb.Debit.ToString("0"));
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(tb.Credit.ToString("0"));
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(tb.ClosingB.ToString("0"));
                writer.Write("\"");
                writer.WriteLine();
            }

            writer.Flush();
            output.Position = 0;

            return File(output, "text/comma-separated-values", "TrialBalanceReport.csv");
        }

        public ActionResult GeneralLedgerForSpecificUnit(string unitCode, string unitName)
        {
            Session["unitCodeForSpeceficUnit"] = unitCode;
            Session["unitNameForSpeceficUnitName"] = unitName;
            return RedirectToAction("GeneralLedger", "AccountingReport");
        }

        public ActionResult GeneralLedgerForSpecificUnitWithAccountSelection(string unitCode, string unitName, string accNumber, string projectCode)
        {
            Session["unitCodeForSpeceficUnit"] = unitCode;
            Session["unitNameForSpeceficUnitName"] = unitName;
            Session["accNumberProjectCode"] = accNumber + "," + projectCode;
            return RedirectToAction("GeneralLedger", "AccountingReport");
        }

        public ActionResult GeneralLedgerForLoginUnitWithAccountSelection(string accnam, string pc)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            Session["unitCodeForSpeceficUnit"] = objLoginHelper.LocationCode;
            Session["unitNameForSpeceficUnitName"] = objLoginHelper.LocationName;
            Session["accNumberProjectCode"] = accnam + "," + pc;
            return RedirectToAction("GeneralLedger", "AccountingReport");
        }

        public ActionResult AccGetMonthlyExpensesInDetail()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAccounting, "AccGetMonthlyExpensesInDetail", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            ViewBag.UnitCode = objLoginHelper.LocationCode;

            //openMonthStartDate =Helper.MonthStartDate(objLoginHelper.YearMonthCurrent).ToString();
            //openMonthEndDate = Helper.MonthEndDate(objLoginHelper.YearMonthCurrent).ToString();

            List<AccGetMonthlyExpensesInDetail> lstAccGetMonthlyExpensesInDetail = new List<AccGetMonthlyExpensesInDetail>();

            if (Session["unitCodeForMonthyAccountStatement"] != null)
            {
                lstAccGetMonthlyExpensesInDetail = accountingDal.ReadAccGetMonthlyExpensesInDetail(Session["unitCodeForMonthyAccountStatement"].ToString(), Session["projectCodeForMonthlyAccountSummaryStatement"].ToString());
            }
            else
            {
                lstAccGetMonthlyExpensesInDetail = accountingDal.ReadAccGetMonthlyExpensesInDetail(objLoginHelper.LocationCode, "100500");
            }

            TempData["AccGetMonthlyExpensesInDetail"] = lstAccGetMonthlyExpensesInDetail;

            return View();
        }

        public ActionResult UnitWiseMonthyExpense(string unitCode, string projectCode)
        {
            Session["unitCodeForMonthyAccountStatement"] = unitCode;
            Session["projectCodeForMonthlyAccountSummaryStatement"] = projectCode;
            return RedirectToAction("AccGetMonthlyExpensesInDetail", "AccountingReport");
        }

        public ActionResult TransactionDetailListByTransactionNo(string transactionNumber, string unitCode)
        {
            List<VoucherTransaction> lstVoucherTransaction = new List<VoucherTransaction>();
            lstVoucherTransaction = accountingDal.GetFinalTransactionDetailListByTransactionNo(transactionNumber, unitCode);
            return PartialView("TransactionDetailListByTransactionNo", lstVoucherTransaction);
        }

        public ActionResult IncomeStatement()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAccounting, "IncomeStatement", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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

            List<ProjectInfo> lstTproject = new List<ProjectInfo>();
            lstTproject = accountingDal.ReadProject().Where(i => i.ProjectCode == "100200" || i.ProjectCode == "100500" || i.ProjectCode == "100100" || i.ProjectCode == "101400").ToList();
            ArrayList arrayProject = new ArrayList();

            if (objLoginHelper.Location == Helper.Unit)
            {
                foreach (ProjectInfo p in lstTproject)
                {
                    arrayProject.Add(new { Display = p.ProjectName, Value = p.ProjectCode });
                }

                ViewBag.ClosingBlanceUnitHide = false;
                ViewBag.ClosingBlanceHOHide = true;
            }
            else
            {
                arrayProject.Add(new { Display = "All Project", Value = "0" });
                foreach (ProjectInfo p in lstTproject)
                {
                    arrayProject.Add(new { Display = p.ProjectName, Value = p.ProjectCode });
                }

                ViewBag.ClosingBlanceUnitHide = true;
                ViewBag.ClosingBlanceHOHide = false;
            }
            TempData["Projects"] = arrayProject;

            return View();
        }

        [GridAction]
        public ActionResult _IncomeStatementShow(string fromDate, string toDate, string projectCode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            try
            {
                if (projectCode == "0")
                {
                    projectCode = string.Empty;
                }

                List<AccountIncomeStatement> lstIncomeStatement = new List<AccountIncomeStatement>();
                lstIncomeStatement = accountingDal.ReadIncomeStatement(2, "INCOMESTMT", 1, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), projectCode);

                return View(new GridModel<AccountIncomeStatement>
                {
                    Data = lstIncomeStatement
                });
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = "" };
            }
        }

        public ActionResult ExportToCsvGeneralLedger(int page, string groupby, string orderBy, string filter, string datefrom, string dateTo, string projectCode, string accountNo)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<GLAccountLedger> lstGeneralLedger = new List<GLAccountLedger>();
            if (Session["unitCodeForSpeceficUnit"] != null)
            {
                lstGeneralLedger = accountingDal.ReadGLAccountLedger(Session["unitCodeForSpeceficUnit"].ToString(), Convert.ToDateTime(datefrom).ToString("dd-MMM-yyyy"), Convert.ToDateTime(dateTo).ToString("dd-MMM-yyyy"), accountNo, projectCode.ToString());
            }
            else
            {
                lstGeneralLedger = accountingDal.ReadGLAccountLedger(objLoginHelper.LocationCode.Trim(), Convert.ToDateTime(datefrom).ToString("dd-MMM-yyyy"), Convert.ToDateTime(dateTo).ToString("dd-MMM-yyyy"), accountNo, projectCode.ToString());
            }

            MemoryStream output = new MemoryStream();
            StreamWriter writer = new StreamWriter(output, Encoding.UTF8);

            writer.Write("Acount No,");
            writer.Write("Acount Name,");
            writer.Write("Opening Balance,");
            writer.Write("Debit,");
            writer.Write("Credit,");
            writer.Write("Closing Balance");
            writer.WriteLine();

            foreach (GLAccountLedger gl in lstGeneralLedger)
            {
                writer.Write(gl.TransNo);
                writer.Write(",");

                writer.Write("\"");
                writer.Write(((DateTime)gl.TransDate).ToString("dd-MMM-yyyy"));
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(gl.Particulars);
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(((decimal)gl.DrAmount).ToString("0"));
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(((decimal)gl.CrAmount).ToString("0"));
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(((decimal)gl.Balance).ToString("0"));
                writer.Write("\"");
                writer.WriteLine();
            }

            writer.Flush();
            output.Position = 0;

            return File(output, "text/comma-separated-values", "GeneralLedgerReport.csv");
        }

        public ActionResult ExportToPdfGeneralLedger(int page, string groupby, string orderBy, string filter, string datefrom, string dateTo, string projectCode, string accountNo)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            string locationCode = string.Empty;

            if (Session["unitCodeForSpeceficUnit"] != null)
            {
                locationCode = Session["unitCodeForSpeceficUnit"].ToString();
            }
            else
            {
                locationCode = objLoginHelper.LocationCode;
            }

            List<GLAccountLedger> generalLadgerReport = new List<GLAccountLedger>();
            if (Session["unitCodeForSpeceficUnit"] != null)
            {
                generalLadgerReport = accountingDal.ReadGLAccountLedger(Session["unitCodeForSpeceficUnit"].ToString(), Convert.ToDateTime(datefrom).ToString("dd-MMM-yyyy"), Convert.ToDateTime(dateTo).ToString("dd-MMM-yyyy"), accountNo, projectCode.ToString());
            }
            else
            {
                generalLadgerReport = accountingDal.ReadGLAccountLedger(objLoginHelper.LocationCode, Convert.ToDateTime(datefrom).ToString("dd-MMM-yyyy"), Convert.ToDateTime(dateTo).ToString("dd-MMM-yyyy"), accountNo, projectCode.ToString());
            }

            Document pdfDocumnet = new Document(PageSize.A4, 35, 20, 15, 10);
            MemoryStream memStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(pdfDocumnet, memStream);

            string projectName = accountingDal.ReadProject(projectCode).ProjectName;

            StringBuilder headerStringCompanyTitle = new StringBuilder();
            StringBuilder headerStringReportTitle = new StringBuilder();
            headerStringCompanyTitle.AppendFormat(Helper.RSFAddress);
            headerStringReportTitle.AppendFormat("General Ladger Report \n Date From: {0} To: {1} \n {2}", datefrom, dateTo, projectName);

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
            float[] widths = new float[] { 12f, 12f, 40f, 12f, 12f, 12f };

            PdfPTable pdfTable = new PdfPTable(numOfColumns);
            pdfTable.SetWidths(widths);
            pdfTable.WidthPercentage = 100;
            pdfTable.DefaultCell.Padding = 3;
            pdfTable.DefaultCell.BorderWidth = 0.5F;
            pdfTable.DefaultCell.BorderColor = new Color(221, 218, 218);
            pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfTable.DefaultCell.BackgroundColor = new Color(235, 235, 235);

            Font fontForTableHeader = new Font(Font.COURIER, 10, Font.BOLD, new Color(0, 0, 0));
            pdfTable.AddCell(new Phrase("Trans No", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Trans Date", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Particulars", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Dr Amount", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Cr Amount", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Balance", fontForTableHeader));

            pdfTable.HeaderRows = 1;
            pdfTable.DefaultCell.BorderWidth = 0.5F;
            pdfTable.DefaultCell.BorderColor = new Color(195, 195, 195);
            pdfTable.DefaultCell.BackgroundColor = new Color(255, 255, 255);

            Font fontForContent = new Font(Font.COURIER, 8, Font.NORMAL, new Color(0, 0, 0));

            foreach (GLAccountLedger tb in generalLadgerReport)
            {
                pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.AddCell(new Phrase(tb.TransNo, fontForContent));
                pdfTable.AddCell(new Phrase(Convert.ToDateTime(tb.TransDate).ToString("dd-MMM-yyyy"), fontForContent));
                pdfTable.AddCell(new Phrase(tb.Particulars.ToString(), fontForContent));

                pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                pdfTable.AddCell(new Phrase(((decimal)tb.DrAmount).ToString("0,0"), fontForContent));
                pdfTable.AddCell(new Phrase(((decimal)tb.CrAmount).ToString("0,0"), fontForContent));
                pdfTable.AddCell(new Phrase(((decimal)tb.Balance).ToString("0,0"), fontForContent));
            }

            pdfDocumnet.Add(pdfTable);
            pdfDocumnet.Close();

            return File(memStream.ToArray(), "application/pdf", "RSF-GeneralLadger.pdf");

        }

        public ActionResult ExportToPdfIncomeStatement(int page, string groupby, string orderBy, string filter, string fromDate, string toDate, string projectCode, string projectName)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (projectCode == "0")
            {
                projectCode = string.Empty;
            }

            List<AccountIncomeStatement> lstIncomeStatement = new List<AccountIncomeStatement>();
            lstIncomeStatement = accountingDal.ReadIncomeStatement(2, "INCOMESTMT", 1, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), projectCode);


            Document pdfDocumnet = new Document(PageSize.A4, 35, 20, 15, 10);
            MemoryStream memStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(pdfDocumnet, memStream);

            StringBuilder headerStringCompanyTitle = new StringBuilder();
            StringBuilder headerStringReportTitle = new StringBuilder();
            headerStringCompanyTitle.AppendFormat(Helper.RSFAddressWithoutPOID);
            headerStringReportTitle.AppendFormat("Income Statement  \n Project Name: {0} \n From: {1} To: {2} \n Report Date: {3}", projectName, fromDate, toDate, DateTime.Now);

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
            float[] widths = new float[] { 15f, 22f, 20f, 15f, 15f, 13f };

            PdfPTable pdfTable = new PdfPTable(numOfColumns);
            pdfTable.SetWidths(widths);
            pdfTable.WidthPercentage = 100;
            pdfTable.DefaultCell.Padding = 3;
            pdfTable.DefaultCell.BorderWidth = 0.5F;
            pdfTable.DefaultCell.BorderColor = new Color(221, 218, 218);
            pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfTable.DefaultCell.BackgroundColor = new Color(235, 235, 235);

            Font fontForTableHeader = new Font(Font.COURIER, 10, Font.BOLD, new Color(0, 0, 0));
            pdfTable.AddCell(new Phrase("Account NO", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Account Name", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Periodic Amount", fontForTableHeader));
            pdfTable.AddCell(new Phrase("% On Revenue", fontForTableHeader));
            pdfTable.AddCell(new Phrase("Cumulative Amount", fontForTableHeader));
            pdfTable.AddCell(new Phrase("% On Revenue", fontForTableHeader));

            pdfTable.HeaderRows = 1;
            pdfTable.DefaultCell.BorderWidth = 0.5F;
            pdfTable.DefaultCell.BorderColor = new Color(195, 195, 195);
            pdfTable.DefaultCell.BackgroundColor = new Color(255, 255, 255);

            Font fontForContent = new Font(Font.COURIER, 8, Font.NORMAL, new Color(0, 0, 0));

            foreach (AccountIncomeStatement ist in lstIncomeStatement)
            {
                pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.AddCell(new Phrase(ist.AccountNo, fontForContent));
                pdfTable.AddCell(new Phrase(ist.AccountName, fontForContent));

                pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                pdfTable.AddCell(new Phrase(((decimal)ist.PeriodicAmount).ToString("0,0"), fontForContent));
                pdfTable.AddCell(new Phrase(ist.PeriodicAmountPercentageOnRevenue, fontForContent));
                pdfTable.AddCell(new Phrase(((decimal)ist.YearToDateAmount).ToString("0,0"), fontForContent));
                pdfTable.AddCell(new Phrase(ist.YearToDateAmountPercentageOnRevenue, fontForContent));

            }

            pdfDocumnet.Add(pdfTable);
            pdfDocumnet.Close();

            return File(memStream.ToArray(), "application/pdf", "RSF-IncomeStatement.pdf");
        }

    }
}
