using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using RASolarHelper;
using System.Web.Configuration;
using System.Configuration;
using RASolarERP.Web.Models;
using RASolarERP.DomainModel.AMSModel;
using RASolarERP.Web.Areas.Financial.Models;
using RASolarAMS.Model;
using System.Collections;

namespace RASolarERP.Web.Areas.Financial.Controllers
{
    public class AccountDeptController : Controller
    {
        private AccountingReportData accountingDal = new AccountingReportData();
        private VoucherProcess voucherProcess = new VoucherProcess();
        RASolarSecurityData securityDal = new RASolarSecurityData();
        private AccountDeptModel accountDeptDal = new AccountDeptModel();
        
        LoginHelper objLoginHelper = new LoginHelper();
        string message = string.Empty;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AccountModule()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAccounting, "AccountModule", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            string uid = string.Empty, uName = string.Empty, location = string.Empty, userGrouptype = string.Empty, dayOpenDateForAccounting = string.Empty;
            uid = objLoginHelper.LogInID;
            //pass = objLoginHelper.LogInPassword;
            location = objLoginHelper.LogInForUnitCode;
            uName = objLoginHelper.LogInForUnitName;
            dayOpenDateForAccounting = objLoginHelper.TransactionOpenDate.ToString("dd-MMM-yy");

            string webAddress = ConfigurationManager.AppSettings["RASolarERP_AMSUnitLogin"];

            return Redirect(webAddress + "?uid=" + uid + "&lc=" + location + "&un=" + uName + "&odt=" + dayOpenDateForAccounting);
        }

        public ActionResult AccountingModuleForZO()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAccounting, "AccountingModuleForZO", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            string uid = string.Empty, uName = string.Empty, location = string.Empty, userGrouptype = string.Empty, dayOpenDateForAccounting = string.Empty;
            uid = objLoginHelper.LogInID;
            location = objLoginHelper.LogInForZoneCode;
            uName = objLoginHelper.LogInForZoneName;
            dayOpenDateForAccounting = objLoginHelper.TransactionOpenDate.ToString("dd-MMM-yy");

            string webAddress = ConfigurationManager.AppSettings["RASolarERP_AMSZonalLogin"];
            return Redirect(webAddress + "?uid=" + uid + "&lc=" + location + "&un=" + uName + "&odt=" + dayOpenDateForAccounting);
        }

        public ActionResult AccountingModuleForHeadOffice()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAccounting, "AccountingModuleForHeadOffice", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            string uid = string.Empty, uName = string.Empty, location = string.Empty, userGrouptype = string.Empty, dayOpenDateForAccounting = string.Empty;
            uid = objLoginHelper.LogInID;
            location = objLoginHelper.LocationCode;
            uName = objLoginHelper.LogInUserName;
            dayOpenDateForAccounting = objLoginHelper.TransactionOpenDate.ToString("dd-MMM-yy");


            string webAddress = ConfigurationManager.AppSettings["RASolarERP_AMSHOUserlLogin"];

            return Redirect(webAddress + "?uid=" + uid + "&lc=" + location + "&un=" + uName + "&odt=" + dayOpenDateForAccounting);
        }

        public ActionResult CustomerTrainingModule()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAccounting, "CustomerTrainingModule", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            string zid = string.Empty, uName = string.Empty, location = string.Empty, userGroupType = string.Empty, unitName = string.Empty, region = string.Empty;
            zid = objLoginHelper.LogInForZoneName;
            location = objLoginHelper.LocationCode;
            uName = objLoginHelper.LogInForUnitName;
            unitName = objLoginHelper.LogInForUnitName;
            region = objLoginHelper.LogInForRegionName;

            //?z=PATUAKHALI&r=Amtoli&lc=AMT001&un=Amtoli [AMT001]
            //zone = Request.QueryString["z"];
            //region = Request.QueryString["r"];
            //unitCode = Request.QueryString["lc"];
            //unitName = Request.QueryString["un"];

            string webAddress = ConfigurationManager.AppSettings["RASolarERP_CustomerTraining"];

            return Redirect(webAddress + "?z=" + zid + "&r=" + region + "&lc=" + location + "&un=" + unitName);
        }

        public ActionResult EmployeeWiseSalaryPayment()
        {

            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAccounting, "EmployeeWiseSalaryPayment", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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
            ViewBag.Project = accountingDal.ReadProject();
            ViewBag.TransactionOpenDate = Convert.ToDateTime(objLoginHelper.TransactionOpenDate).ToString("dd MMM yyyy");

            ArrayList arr = new ArrayList();

            if (objLoginHelper.Location == Helper.Unit)
            {
                arr.Add(new { Display = "CASH", value = "Cash" });
                arr.Add(new { Display = "BANK", value = "Bank" });
            }
            else
            {
                arr.Add(new { Display = "CASH", value = "Cash" });
                arr.Add(new { Display = "BANK", value = "Bank" });
                arr.Add(new { Display = "AREAR", value = "Arrear" });
            }

            ViewBag.PaymentFrom = arr;

            return View();
        }

        public JsonResult GetAccountForEmployeeWiseSalaryPayment(string paymentType)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<ChartOfAccounts> lstChartOfAccounts = new List<ChartOfAccounts>();

            if (paymentType == "CASH")
            {
                lstChartOfAccounts = VoucherWiseChartOfAccount.CashInHand();
            }
            else if (paymentType == "BANK" || paymentType == "AREAR")
            {
                lstChartOfAccounts = accountingDal.GetLocationWiseChartOfAccount(objLoginHelper.LocationCode);
            }

            return new JsonResult { Data = lstChartOfAccounts };
        }

        public JsonResult LoadEmployeeWiseSalaryPayment(string paymentType)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<EmployeeWiseSalaryPayment> esp = new List<DomainModel.AMSModel.EmployeeWiseSalaryPayment>();
            esp = accountingDal.ReadEmployeeWiseSalaryPaymentPosting(paymentType, objLoginHelper.LocationCode);

            return new JsonResult { Data = esp };
        }
    }
}
