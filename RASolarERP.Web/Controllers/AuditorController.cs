using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using RASolarERP.Model;
using RASolarERP.Web.Areas.Inventory.Models;
using RASolarHelper;
using RASolarERP.Web.Models;
using System.Collections;
using Telerik.Web.Mvc;
using RASolarERP.DomainModel.InventoryModel;
using RASolarERP.Web.Areas.HRMS.Models;
using RASolarERP.DomainModel.HRMSModel;

namespace RASolarERP.Web.Controllers
{
    public class AuditorController : Controller
    {
        LoginHelper objLoginHelper = new LoginHelper();
        InventoryData inventoryDal = new InventoryData();
        private HRMSData hrmsData = new HRMSData();
        RASolarSecurityData securityDal = new RASolarSecurityData();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AuditSetup()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            PageAccessRightHelper objPageAccessRightHelper = new PageAccessRightHelper();
            objPageAccessRightHelper = securityDal.ReadPageAccessRight(Helper.ForAuditor, "AuditSetup", objLoginHelper.UerRoleOrGroupID, Helper.Inactive);
            if (objPageAccessRightHelper != null)
            {
                if (objPageAccessRightHelper.AccessStatus == Helper.Inactive)
                {
                    Session["messageInformation"] = objPageAccessRightHelper.MessageToShow;
                    return RedirectToAction("ErrorMessage", "ErrorHnadle");
                }
            }

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;

            ViewBag.OpenEndDay = Helper.MonthStartDate(objLoginHelper.TransactionOpenDate.Date.ToString("yyyyMM")).ToString("dd-MMM-yyyy");
            ViewBag.OpenStartDay = Helper.MonthEndDate(objLoginHelper.TransactionBackDate.Date.ToString("yyyyMM")).ToString("dd-MMM-yyyy");

            ViewBag.DayOpenningDate = objLoginHelper.TransactionOpenDate.ToString("dd-MMM-yyyy");
            ViewBag.OpenMonthYear = objLoginHelper.TransactionOpenDate.ToString("MMMM  dd, yyyy");

            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            string auditNumberMax = inventoryDal.AuditSequenceNumberMax(objLoginHelper.LocationCode, objLoginHelper.TransactionOpenDate.ToString("yyMMdd"));
            string auditSequenceNumberNew = Helper.ChallanCequenceNumberGeneration(auditNumberMax, objLoginHelper);

            ViewBag.Employee = hrmsData.AuditorEmployeeList();

            AuditingMaster auditingMaster = new AuditingMaster();
            auditingMaster = hrmsData.AuditMasterCheckNGetAuditMasterDetails(objLoginHelper.LocationCode, objLoginHelper.TransactionOpenDate.Date);

            if (string.IsNullOrEmpty(auditingMaster.AuditSeqNo))
                ViewBag.EditPermission = true;
            else
                ViewBag.EditPermission = false;

            auditingMaster.AuditSeqNo = string.IsNullOrEmpty(auditingMaster.AuditSeqNo) ? auditingMaster.AuditSeqNo = auditSequenceNumberNew : auditingMaster.AuditSeqNo;

            return View(auditingMaster);
        }

        public JsonResult AuditSetupSave(AuditingMaster auditingMaster)
        {
            try
            {
                objLoginHelper = (LoginHelper)Session["LogInInformation"];

                auditingMaster.LocationCode = objLoginHelper.LocationCode;
                auditingMaster.CreatedBy = objLoginHelper.LogInID;
                auditingMaster.CreatedOn = DateTime.Now;
                auditingMaster.ModifiedBy = objLoginHelper.LogInID;
                auditingMaster.ModifiedOn = DateTime.Now;
                auditingMaster.Status = Helper.Active;

                AuditingMaster objAuditMaster = new AuditingMaster();

                if (hrmsData.AuditMasterSetupAlreadyExistOrNot(auditingMaster.LocationCode, auditingMaster.AuditSeqNo) == false)
                {
                    objAuditMaster = hrmsData.AuditorSeupSave(auditingMaster);
                }
                else
                {
                    hrmsData.UpdateAuditMasterDetails(auditingMaster);
                }

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(Helper.SuccessMessage, Helper.ChallanCequenceNumberGeneration(objAuditMaster.AuditSeqNo, objLoginHelper)) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

    }
}
