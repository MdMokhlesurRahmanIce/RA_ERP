using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using RASolarHelper;
using RASolarERP.Web.Models;
using RASolarERP.Web.Areas.HRMS.Models;
using System.Collections;

namespace RASolarERP.Web.Areas.HRMS.Controllers
{
    public class HrmReportController : Controller
    {
        LoginHelper objLoginHelper = new LoginHelper();
        RASolarSecurityData securityDal = new RASolarSecurityData();

        private HRMSData hrmsData = new HRMSData();
        string message = string.Empty;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SalaryAdviceForBank()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForHRMS, "SalaryAdviceForBank", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForHRMS.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;
            ViewBag.CurrentDay = objLoginHelper.MonthOpenForHRMS.Date.ToString("dd-MMM-yyyy");

            ViewBag.ForMonth = new YearMonthFormat().MonthFormat();

            return View();
        }

        public JsonResult GenerateSalaryAdviceForBank(string forMonth, string reportType, string generateType, string startLetterSequenceNo, string locationCode, string bankAccount)
        {
            try
            {
                reportType = "FORALLLOCATION";
                hrmsData.GenerateSalaryAdviceForBank(forMonth, reportType, generateType, startLetterSequenceNo, locationCode, bankAccount);
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Successfully Generate Bank Advice") };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public JsonResult GetLocationWiseBankAccountForSalaryBoucher(string locationCode)
        {
            ArrayList arr = new ArrayList();

            if (!string.IsNullOrEmpty(locationCode))
                arr = hrmsData.GetBankAccountForSalaryBoucher(locationCode);
            else
                arr = hrmsData.GetBankAccountForSalaryBoucher();

            return new JsonResult { Data = arr };
        }


    }
}
