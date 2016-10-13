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

namespace RASolarERP.Web.Areas.Inventory.Controllers
{
    public class InventoryAuditController : Controller
    {
        InventoryData inventoryDal = new InventoryData();
        LoginHelper objLoginHelper = new LoginHelper();
        private RASolarERPData erpDal = new RASolarERPData();
        RASolarSecurityData securityDal = new RASolarSecurityData();
        private HRMSData hrmsDal = new HRMSData();

        string message = string.Empty;

        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult AuditAdjustmentNewItem()
        //{
        //    objLoginHelper = (LoginHelper)Session["LogInInformation"];

        //    PageAccessRightHelper objPageAccessRightHelper = new PageAccessRightHelper();
        //    objPageAccessRightHelper = securityDal.ReadPageAccessRight(Helper.ForInventory, "AuditAdjustmentNewItem", objLoginHelper.UerRoleOrGroupID, Helper.Inactive);
        //    if (objPageAccessRightHelper != null)
        //    {
        //        if (objPageAccessRightHelper.AccessStatus == Helper.Inactive)
        //        {
        //            Session["messageInformation"] = objPageAccessRightHelper.MessageToShow;
        //            return RedirectToAction("ErrorMessage", "../ErrorHnadle");
        //        }
        //    }

        //    string auditorYearMonthLocal = "201206";
        //    string auditorYearMonthLocalForWrite = "201209";

        //    List<InventoryAuditAdjustment> adjustmentForNew = new List<InventoryAuditAdjustment>();
        //    List<Aud_AdjustmentReasonCodes> newItemAdjustmentReason = new List<Aud_AdjustmentReasonCodes>();

        //    adjustmentForNew = inventoryDal.InventoryAdjustmentForItem(objLoginHelper.LogInForUnitCode, auditorYearMonthLocal, auditorYearMonthLocalForWrite, Helper.Active, Helper.NewItem);
        //    newItemAdjustmentReason = inventoryDal.AuditAdjustMentReasonForNew();

        //    ViewBag.NewReasons = newItemAdjustmentReason;

        //    Aud_AuditingMaster objAuditMaster = new Aud_AuditingMaster();
        //    objAuditMaster = inventoryDal.AuditingMasterByLocationCode(objLoginHelper.LogInForUnitCode);

        //    if (objAuditMaster != null)
        //    {
        //        ViewBag.AuditStartDate = objAuditMaster.AuditStartDate != null ? Helper.DateFormat(Convert.ToDateTime(objAuditMaster.AuditStartDate)) : Helper.DateFormat(DateTime.Now);
        //        ViewBag.AuditFinishedDate = objAuditMaster.AuditFinishDate != null ? Helper.DateFormat(Convert.ToDateTime(objAuditMaster.AuditFinishDate)) : "";
        //        ViewBag.AuditCode = objAuditMaster.AuditSequenceNo.ToString();
        //    }
        //    else
        //    {
        //        ViewBag.AuditStartDate = "";
        //        ViewBag.AuditFinishedDate = "";
        //        ViewBag.AuditCode = "";
        //    }

        //    ViewBag.LocationTitle = objLoginHelper.LocationTitle;
        //    ViewBag.Location = objLoginHelper.Location;
        //    ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
        //    ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
        //    ViewBag.RegionTitle = objLoginHelper.RegionTitle;
        //    ViewBag.RegionName = objLoginHelper.LogInForRegionName;
        //    ViewBag.UnitTitle = objLoginHelper.UnitTitle;
        //    ViewBag.UnitName = objLoginHelper.LogInForUnitName;
        //    ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForInventory.ToString("MMMM-yy");
        //    ViewBag.ModuleName = objLoginHelper.ModluleTitle;
        //    ViewBag.TopMenu = objLoginHelper.TopMenu;

        //    if (objLoginHelper.UerRoleOrGroupID == UserGroup.RegionManager)
        //    {
        //        ViewBag.ShowHideMenu = true;
        //    }
        //    else
        //        ViewBag.ShowHideMenu = false;

        //    return View(adjustmentForNew);
        //}

        //public ActionResult AuditAdjustmentCustomerSupportItem()
        //{
        //    objLoginHelper = (LoginHelper)Session["LogInInformation"];

        //    PageAccessRightHelper objPageAccessRightHelper = new PageAccessRightHelper();
        //    objPageAccessRightHelper = securityDal.ReadPageAccessRight(Helper.ForInventory, "StockPositionViewNUpdate", objLoginHelper.UerRoleOrGroupID, Helper.Inactive);
        //    if (objPageAccessRightHelper != null)
        //    {
        //        if (objPageAccessRightHelper.AccessStatus == Helper.Inactive)
        //        {
        //            Session["messageInformation"] = objPageAccessRightHelper.MessageToShow;
        //            return RedirectToAction("ErrorMessage", "../ErrorHnadle");
        //        }
        //    }

        //    string auditorYearMonthLocal = "201206";
        //    string auditorYearMonthLocalForWrite = "201209";

        //    List<InventoryAuditAdjustment> adjustmentForNew = new List<InventoryAuditAdjustment>();
        //    List<Aud_AdjustmentReasonCodes> newItemAdjustmentReason = new List<Aud_AdjustmentReasonCodes>();



        //    adjustmentForNew = inventoryDal.InventoryAdjustmentForItem(objLoginHelper.LogInForUnitCode, auditorYearMonthLocal, auditorYearMonthLocalForWrite, Helper.Active, Helper.CustomerSupport);
        //    newItemAdjustmentReason = inventoryDal.AuditAdjustMentReasonForNew();

        //    ViewBag.NewReasons = newItemAdjustmentReason;

        //    Aud_AuditingMaster objAuditMaster = new Aud_AuditingMaster();
        //    objAuditMaster = inventoryDal.AuditingMasterByLocationCode(objLoginHelper.LogInForUnitCode);

        //    if (objAuditMaster != null)
        //    {
        //        ViewBag.AuditStartDate = objAuditMaster.AuditStartDate != null ? Helper.DateFormat(Convert.ToDateTime(objAuditMaster.AuditStartDate)) : Helper.DateFormat(DateTime.Now);
        //        ViewBag.AuditFinishedDate = objAuditMaster.AuditFinishDate != null ? Helper.DateFormat(Convert.ToDateTime(objAuditMaster.AuditFinishDate)) : "";
        //        ViewBag.AuditCode = objAuditMaster.AuditSequenceNo.ToString();
        //    }
        //    else
        //    {
        //        ViewBag.AuditStartDate = "";
        //        ViewBag.AuditFinishedDate = "";
        //        ViewBag.AuditCode = "";
        //    }

        //    ViewBag.LocationTitle = objLoginHelper.LocationTitle;
        //    ViewBag.Location = objLoginHelper.Location;
        //    ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
        //    ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
        //    ViewBag.RegionTitle = objLoginHelper.RegionTitle;
        //    ViewBag.RegionName = objLoginHelper.LogInForRegionName;
        //    ViewBag.UnitTitle = objLoginHelper.UnitTitle;
        //    ViewBag.UnitName = objLoginHelper.LogInForUnitName;
        //    ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForInventory.ToString("MMMM-yy");
        //    ViewBag.ModuleName = objLoginHelper.ModluleTitle;
        //    ViewBag.TopMenu = objLoginHelper.TopMenu;

        //    if (objLoginHelper.UerRoleOrGroupID == UserGroup.RegionManager)
        //    {
        //        ViewBag.ShowHideMenu = true;
        //    }
        //    else
        //        ViewBag.ShowHideMenu = false;

        //    return View(adjustmentForNew);
        //}

        //public ActionResult AuditAdjustmentSystemReturnItem()
        //{
        //    objLoginHelper = (LoginHelper)Session["LogInInformation"];

        //    PageAccessRightHelper objPageAccessRightHelper = new PageAccessRightHelper();
        //    objPageAccessRightHelper = securityDal.ReadPageAccessRight(Helper.ForInventory, "StockPositionViewNUpdate", objLoginHelper.UerRoleOrGroupID, Helper.Inactive);
        //    if (objPageAccessRightHelper != null)
        //    {
        //        if (objPageAccessRightHelper.AccessStatus == Helper.Inactive)
        //        {
        //            Session["messageInformation"] = objPageAccessRightHelper.MessageToShow;
        //            return RedirectToAction("ErrorMessage", "../ErrorHnadle");
        //        }
        //    }

        //    string auditorYearMonthLocal = "201206";
        //    string auditorYearMonthLocalForWrite = "201209";

        //    List<InventoryAuditAdjustment> adjustmentForNew = new List<InventoryAuditAdjustment>();
        //    List<Aud_AdjustmentReasonCodes> newItemAdjustmentReason = new List<Aud_AdjustmentReasonCodes>();

        //    adjustmentForNew = inventoryDal.InventoryAdjustmentForItem(objLoginHelper.LogInForUnitCode, auditorYearMonthLocal, auditorYearMonthLocalForWrite, Helper.Active, Helper.SystemReturn);
        //    newItemAdjustmentReason = inventoryDal.AuditAdjustMentReasonForNew();

        //    ViewBag.NewReasons = newItemAdjustmentReason;

        //    Aud_AuditingMaster objAuditMaster = new Aud_AuditingMaster();
        //    objAuditMaster = inventoryDal.AuditingMasterByLocationCode(objLoginHelper.LogInForUnitCode);

        //    if (objAuditMaster != null)
        //    {
        //        ViewBag.AuditStartDate = objAuditMaster.AuditStartDate != null ? Helper.DateFormat(Convert.ToDateTime(objAuditMaster.AuditStartDate)) : Helper.DateFormat(DateTime.Now);
        //        ViewBag.AuditFinishedDate = objAuditMaster.AuditFinishDate != null ? Helper.DateFormat(Convert.ToDateTime(objAuditMaster.AuditFinishDate)) : "";
        //        ViewBag.AuditCode = objAuditMaster.AuditSequenceNo.ToString();
        //    }
        //    else
        //    {
        //        ViewBag.AuditStartDate = "";
        //        ViewBag.AuditFinishedDate = "";
        //        ViewBag.AuditCode = "";
        //    }

        //    ViewBag.LocationTitle = objLoginHelper.LocationTitle;
        //    ViewBag.Location = objLoginHelper.Location;
        //    ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
        //    ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
        //    ViewBag.RegionTitle = objLoginHelper.RegionTitle;
        //    ViewBag.RegionName = objLoginHelper.LogInForRegionName;
        //    ViewBag.UnitTitle = objLoginHelper.UnitTitle;
        //    ViewBag.UnitName = objLoginHelper.LogInForUnitName;
        //    ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForInventory.ToString("MMMM-yy");
        //    ViewBag.ModuleName = objLoginHelper.ModluleTitle;
        //    ViewBag.TopMenu = objLoginHelper.TopMenu;

        //    if (objLoginHelper.UerRoleOrGroupID == UserGroup.RegionManager)
        //    {
        //        ViewBag.ShowHideMenu = true;
        //    }
        //    else
        //        ViewBag.ShowHideMenu = false;

        //    return View(adjustmentForNew);
        //}

        public JsonResult AuditAdjustmentForNewItem(string itemCode, string closingBalanceAuditAdjustment, string resonforAdjustment, string damageClosingBalanceAuditAdjustment, string resonforDamageAdjustment, string remarks)
        {
            //string auditorYearMonthLocalForWrite = "201209";

            //short auditSequenceNumber = 0;
            string updateMessage = string.Empty;

            //objLoginHelper = (LoginHelper)Session["LogInInformation"];

            //tbl_InventoryTransaction objInventory = new tbl_InventoryTransaction();
            //tbl_InventoryTransaction objInventoryTrans = new tbl_InventoryTransaction();

            //AuditingMaster objAuditMaster = new AuditingMaster();

            //try
            //{
            //    objInventory = inventoryDal.ReadInventoryTransaction(objLoginHelper.LogInForUnitCode, itemCode, auditorYearMonthLocalForWrite, Helper.NewItem);

            //    objAuditMaster = inventoryDal.AuditingMasterByLocationCode(objLoginHelper.LogInForUnitCode);

            //    if (objAuditMaster != null)
            //    {
            //        auditSequenceNumber = objAuditMaster.AuditSequenceNo;
            //    }
            //    else
            //    {
            //        auditSequenceNumber += 1;

            //        objAuditMaster = new AuditingMaster();

            //        objAuditMaster.LocationCode = objLoginHelper.LogInForUnitCode;
            //        objAuditMaster.AuditForSalInvAccHrm = Helper.ForInventory;
            //        objAuditMaster.AuditSequenceNo = auditSequenceNumber;
            //        objAuditMaster.AuditStartDate = DateTime.Now;
            //        objAuditMaster.UserName = objLoginHelper.LogInID;
            //        objAuditMaster.EntryTime = DateTime.Now;

            //        inventoryDal.SaveAuditingMaster(objAuditMaster);
            //    }

            //    if (objInventory != null)
            //    {
            //        objInventoryTrans = AssignToInventoryObject(objInventory, itemCode, closingBalanceAuditAdjustment, resonforAdjustment, damageClosingBalanceAuditAdjustment, resonforDamageAdjustment, remarks, Helper.NewItem, auditSequenceNumber);

            //        objInventoryTrans = inventoryDal.UpdateInventoryTransaction(objInventoryTrans);

            //        if (objInventoryTrans != null)
            //            updateMessage = "succeed";
            //    }
            //    else
            //    {
            //        objInventoryTrans = AssignToInventoryObject(null, itemCode, closingBalanceAuditAdjustment, resonforAdjustment, damageClosingBalanceAuditAdjustment, resonforDamageAdjustment, remarks, Helper.NewItem, auditSequenceNumber);

            //        objInventoryTrans = inventoryDal.InsertInventoryTransaction(objInventoryTrans);

            //        if (objInventoryTrans != null)
            //            updateMessage = "succeed";
            //    }

            //}
            //catch (Exception ex)
            //{
            //    updateMessage = "nSucceed";
            //}

            return new JsonResult { Data = updateMessage };
        }

        public JsonResult AuditAdjustmentForCustomerSupportItem(string itemCode, string closingBalanceAuditAdjustment, string resonforAdjustment, string damageClosingBalanceAuditAdjustment, string resonforDamageAdjustment, string remarks)
        {
            string auditorYearMonthLocalForWrite = "201209";
            short auditSequenceNumber = 0;
            string updateMessage = string.Empty;

            //objLoginHelper = (LoginHelper)Session["LogInInformation"];

            //tbl_InventoryTransaction objInventory = new tbl_InventoryTransaction();
            //tbl_InventoryTransaction objInventoryTrans = new tbl_InventoryTransaction();

            //AuditingMaster objAuditMaster = new AuditingMaster();

            //try
            //{
            //    objInventory = inventoryDal.ReadInventoryTransaction(objLoginHelper.LogInForUnitCode, itemCode, auditorYearMonthLocalForWrite, Helper.CustomerSupport);

            //    objAuditMaster = inventoryDal.AuditingMasterByLocationCode(objLoginHelper.LogInForUnitCode);

            //    if (objAuditMaster != null)
            //    {
            //        auditSequenceNumber = objAuditMaster.AuditSequenceNo;
            //    }
            //    else
            //    {
            //        auditSequenceNumber += 1;

            //        objAuditMaster = new AuditingMaster();

            //        objAuditMaster.LocationCode = objLoginHelper.LogInForUnitCode;
            //        objAuditMaster.AuditForSalInvAccHrm = Helper.ForInventory;
            //        objAuditMaster.AuditSequenceNo = auditSequenceNumber;
            //        objAuditMaster.AuditStartDate = DateTime.Now;
            //        objAuditMaster.UserName = objLoginHelper.LogInID;
            //        objAuditMaster.EntryTime = DateTime.Now;

            //        inventoryDal.SaveAuditingMaster(objAuditMaster);
            //    }

            //    if (objInventory != null)
            //    {
            //        objInventoryTrans = AssignToInventoryObject(objInventory, itemCode, closingBalanceAuditAdjustment, resonforAdjustment, damageClosingBalanceAuditAdjustment, resonforDamageAdjustment, remarks, Helper.CustomerSupport, auditSequenceNumber);

            //        objInventoryTrans = inventoryDal.UpdateInventoryTransaction(objInventoryTrans);

            //        if (objInventoryTrans != null)
            //            updateMessage = "succeed";
            //    }
            //    else
            //    {
            //        objInventoryTrans = AssignToInventoryObject(null, itemCode, closingBalanceAuditAdjustment, resonforAdjustment, damageClosingBalanceAuditAdjustment, resonforDamageAdjustment, remarks, Helper.CustomerSupport, auditSequenceNumber);

            //        objInventoryTrans = inventoryDal.InsertInventoryTransaction(objInventoryTrans);

            //        if (objInventoryTrans != null)
            //            updateMessage = "succeed";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    updateMessage = "nSucceed";
            //}

            return new JsonResult { Data = updateMessage };
        }

        public JsonResult AuditAdjustmentForSystemReturnItem(string itemCode, string closingBalanceAuditAdjustment, string resonforAdjustment, string damageClosingBalanceAuditAdjustment, string resonforDamageAdjustment, string remarks)
        {
            string auditorYearMonthLocalForWrite = "201209";
            short auditSequenceNumber = 0;
            string updateMessage = string.Empty;

            //objLoginHelper = (LoginHelper)Session["LogInInformation"];

            //tbl_InventoryTransaction objInventory = new tbl_InventoryTransaction();
            //tbl_InventoryTransaction objInventoryTrans = new tbl_InventoryTransaction();

            //AuditingMaster objAuditMaster = new AuditingMaster();

            //try
            //{
            //    objInventory = inventoryDal.ReadInventoryTransaction(objLoginHelper.LogInForUnitCode, itemCode, auditorYearMonthLocalForWrite, Helper.SystemReturn);

            //    objAuditMaster = inventoryDal.AuditingMasterByLocationCode(objLoginHelper.LogInForUnitCode);

            //    if (objAuditMaster != null)
            //    {
            //        auditSequenceNumber = objAuditMaster.AuditSequenceNo;
            //    }
            //    else
            //    {
            //        auditSequenceNumber += 1;

            //        objAuditMaster = new AuditingMaster();

            //        objAuditMaster.LocationCode = objLoginHelper.LogInForUnitCode;
            //        objAuditMaster.AuditForSalInvAccHrm = Helper.ForInventory;
            //        objAuditMaster.AuditSequenceNo = auditSequenceNumber;
            //        objAuditMaster.AuditStartDate = DateTime.Now;
            //        objAuditMaster.UserName = objLoginHelper.LogInID;
            //        objAuditMaster.EntryTime = DateTime.Now;

            //        inventoryDal.SaveAuditingMaster(objAuditMaster);
            //    }

            //    if (objInventory != null)
            //    {
            //        objInventoryTrans = AssignToInventoryObject(objInventory, itemCode, closingBalanceAuditAdjustment, resonforAdjustment, damageClosingBalanceAuditAdjustment, resonforDamageAdjustment, remarks, Helper.SystemReturn, auditSequenceNumber);

            //        objInventoryTrans = inventoryDal.UpdateInventoryTransaction(objInventoryTrans);

            //        if (objInventoryTrans != null)
            //            updateMessage = "succeed";
            //    }
            //    else
            //    {
            //        objInventoryTrans = AssignToInventoryObject(null, itemCode, closingBalanceAuditAdjustment, resonforAdjustment, damageClosingBalanceAuditAdjustment, resonforDamageAdjustment, remarks, Helper.SystemReturn, auditSequenceNumber);

            //        objInventoryTrans = inventoryDal.InsertInventoryTransaction(objInventoryTrans);

            //        if (objInventoryTrans != null)
            //            updateMessage = "succeed";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    updateMessage = "nSucceed";
            //}

            return new JsonResult { Data = updateMessage };
        }

        //public JsonResult FinishedInventoryAuditAdjustment()
        //{
        //    short auditSequenceNumber = 0;
        //    string updateMessage = string.Empty;
        //    Aud_AuditingMaster objAuditMaster = new Aud_AuditingMaster();

        //    objLoginHelper = (LoginHelper)Session["LogInInformation"];

        //    try
        //    {
        //        objAuditMaster = inventoryDal.AuditingMasterByLocationCode(objLoginHelper.LogInForUnitCode);
        //        if (objAuditMaster != null)
        //        {
        //            if (objAuditMaster.AuditFinishDate == null)
        //            {
        //                objAuditMaster.AuditFinishDate = DateTime.Now;
        //                //objAuditMaster.AuditForSalInvAccHrm = Helper.ForInventory;
        //                inventoryDal.UpdateAuditingMaster(objAuditMaster);

        //                if (objAuditMaster != null)
        //                    updateMessage = "succeedSave";
        //                else
        //                    updateMessage = "failed";
        //            }
        //            else
        //            {
        //                updateMessage = "finished";
        //            }
        //        }
        //        else
        //        {
        //            auditSequenceNumber += 1;

        //            objAuditMaster = new Aud_AuditingMaster();

        //            objAuditMaster.LocationCode = objLoginHelper.LogInForUnitCode;
        //            //objAuditMaster.AuditForSalInvAccHrm = Helper.ForInventory;
        //            objAuditMaster.AuditSeqNo = auditSequenceNumber.ToString();
        //            objAuditMaster.AuditStartDate = DateTime.Now;
        //            objAuditMaster.CreatedBy = objLoginHelper.LogInID;
        //            objAuditMaster.CreatedOn = DateTime.Now;

        //            inventoryDal.SaveAuditingMaster(objAuditMaster);

        //            if (objAuditMaster != null)
        //                updateMessage = "succeedSave";
        //            else
        //                updateMessage = "failed";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        updateMessage = "failed";
        //    }

        //    return new JsonResult { Data = updateMessage };
        //}

        //private tbl_InventoryTransaction AssignToInventoryObject(tbl_InventoryTransaction objInventoryTrans, string itemCode, string closingBalanceAuditAdjustment, string resonforAdjustment, string damageClosingBalanceAuditAdjustment, string resonforDamageAdjustment, string remarks, int stockLocation, short auditSeqenceNumber)
        //{
        //    string auditorYearMonthLocalForWrite = "201209";

        //    int receivedAuditAdjustment = 0, issuedAuditAdjustment = 0, auditAdjustmentClosingValue = 0, damageAuditAdjustmentValue = 0;

        //    objLoginHelper = (LoginHelper)Session["LogInInformation"];

        //    tbl_InventoryTransaction objInventoryTransaction = new tbl_InventoryTransaction();

        //    if (objInventoryTrans == null)
        //    {
        //        objInventoryTransaction.COMP_CODE = itemCode;
        //        objInventoryTransaction.UNIT_CODE = objLoginHelper.LogInForUnitCode;
        //        objInventoryTransaction.YearMonth = auditorYearMonthLocalForWrite;

        //        objInventoryTransaction.Received_HeadOffice = 0;
        //        objInventoryTransaction.Received_OtherUnit = 0;
        //        objInventoryTransaction.Received_LocalPurchase = 0;
        //        objInventoryTransaction.Received_RRE = 0;
        //        objInventoryTransaction.Received_RALSC = 0;
        //        objInventoryTransaction.Received_RALFactory = 0;
        //        objInventoryTransaction.Received_FromOtherVendors = 0;
        //        objInventoryTransaction.Received_ZonalOffice = 0;
        //        objInventoryTransaction.Received_AdjustmentByUser = 0;

        //        objInventoryTransaction.Issued_HeadOffice = 0;
        //        objInventoryTransaction.Issued_Sales = 0;
        //        objInventoryTransaction.Issued_ToOtherUnit = 0;
        //        objInventoryTransaction.Issued_RRE = 0;
        //        objInventoryTransaction.Issued_RALSC = 0;
        //        objInventoryTransaction.Issued_ZonalOffice = 0;
        //        objInventoryTransaction.Issued_OtherVandor = 0;
        //        objInventoryTransaction.Issued_RALFactory = 0;
        //        objInventoryTransaction.Issued_ToCustomerSupport = 0;
        //        objInventoryTransaction.Issued_AdjustmentByUser = 0;

        //        objInventoryTransaction.OpeningBalance = 0;
        //        objInventoryTransaction.ClosingBalance = 0;

        //        objInventoryTransaction.Damage_Receive = 0;
        //        objInventoryTransaction.Damage_Issue = 0;

        //        objInventoryTransaction.Damage_Repairable_Opening = 0;
        //        objInventoryTransaction.Damage_Repairable_Closing = 0;

        //        objInventoryTransaction.StockLocation = stockLocation;
        //        objInventoryTransaction.EntryDate = DateTime.Now.Date;
        //    }
        //    else
        //    {
        //        objInventoryTransaction = objInventoryTrans;
        //    }

        //    if (closingBalanceAuditAdjustment != "")
        //        auditAdjustmentClosingValue = Convert.ToInt32(closingBalanceAuditAdjustment);

        //    if (damageClosingBalanceAuditAdjustment != "")
        //        damageAuditAdjustmentValue = Convert.ToInt32(damageClosingBalanceAuditAdjustment);

        //    if (auditAdjustmentClosingValue < 0)
        //    {
        //        issuedAuditAdjustment = auditAdjustmentClosingValue * -1;
        //    }
        //    else
        //    {
        //        receivedAuditAdjustment = auditAdjustmentClosingValue;
        //    }

        //    objInventoryTransaction.Received_AuditAdjustment = receivedAuditAdjustment;
        //    objInventoryTransaction.Issued_AuditAdjustment = issuedAuditAdjustment;

        //    if (resonforAdjustment != "0")
        //        objInventoryTransaction.RefReasonForAuditAdjustment = resonforAdjustment;
        //    else
        //        objInventoryTransaction.RefReasonForAuditAdjustment = null;

        //    objInventoryTransaction.RD_AuditAdjustment = damageAuditAdjustmentValue;

        //    if (resonforDamageAdjustment != "0")
        //        objInventoryTransaction.RefReasonForRD_AuditAdjustment = resonforDamageAdjustment;
        //    else
        //        objInventoryTransaction.RefReasonForRD_AuditAdjustment = null;

        //    objInventoryTransaction.Remarks = remarks;

        //    objInventoryTransaction.RefAuditSeqNo = auditSeqenceNumber;

        //    //if (objInventoryTrans != null)
        //    //{
        //    //    objInventoryTransaction.ClosingBalance += issuedAuditAdjustment + receivedAuditAdjustment;
        //    //    objInventoryTransaction.Damage_Repairable_Closing += damageAuditAdjustmentValue;
        //    //}

        //    objInventoryTransaction.User_Name = objLoginHelper.LogInID;
        //    objInventoryTransaction.LastTransDate = DateTime.Now;

        //    return objInventoryTransaction;
        //}

        public ActionResult ItemIssueForAuditAdjustment()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAuditor, "ItemIssueForAuditAdjustment", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            //if (objLoginHelper.TransactionOpenDate == Helper.InvalidDate())
            //{
            //    Session["messageInformation"] = "Day Is Not Open For Challan";
            //    return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            //}

            string CheckAuditSetup = hrmsDal.ReadAuditSeqNumberAfterCheckFinishedDate(objLoginHelper.LocationCode);
            if (string.IsNullOrEmpty(CheckAuditSetup))
            {
                Session["messageInformation"] = "Please Configure Audit Setup First Before Any Transaction";
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            ViewBag.ItemType = inventoryDal.ReadItemType();
            ViewBag.StoreLocation = inventoryDal.ReadStoreLocation();

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Day Open Date: " + objLoginHelper.TransactionOpenDate.ToString("MMMM  dd, yyyy");
            //objLoginHelper.MonthOpenForInventory.ToString("MMMM-yy");            

            ViewBag.OpenDay = objLoginHelper.TransactionOpenDate.Date.ToString("dd-MMM-yyyy");
            ViewBag.OpenBackDay = objLoginHelper.TransactionBackDate.Date.ToString("dd-MMM-yyyy");

            string challanNumberMax = inventoryDal.ChallanSequenceNumberMax(objLoginHelper.LocationCode, objLoginHelper.TransactionOpenDate.ToString("yyMMdd"));
            string challnSequenceNumberNew = Helper.ChallanCequenceNumberGeneration(challanNumberMax, objLoginHelper);

            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;
            ViewBag.ChallnSequenceNumberNew = challnSequenceNumberNew;

            ViewBag.Zone = erpDal.Zone();
            ViewBag.LocationType = new UserLocationType().UserLocationTypeFormat();

            if (objLoginHelper.LocationCode == "9000")
            { ViewBag.IsInventoryImplemented = 1; }
            else
            { ViewBag.IsInventoryImplemented = objLoginHelper.IsInventoryImplemented; }

            ViewBag.InventoryStockUpdateFinishClosed = true; //inventoryDal.IsInventoryStockUpdateFinish(objLoginHelper.LocationCode);

            List<Aud_AdjustmentReasonCodes> newItemAdjustmentReason = new List<Aud_AdjustmentReasonCodes>();
            newItemAdjustmentReason = inventoryDal.AuditAdjustMentReasons(Helper.ReasonPurposeForIssue, Helper.ReasonForUserOrAuditor, Helper.ForInventory);
            ViewBag.AuditReasons = newItemAdjustmentReason;

            return View();
        }

        public ActionResult ItemReceiveForAuditAdjustment()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAuditor, "ItemReceiveForAuditAdjustment", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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

            //if (objLoginHelper.TransactionOpenDate == Helper.InvalidDate())
            //{
            //    Session["messageInformation"] = "Day Is Not Open For MRR (Without Challan)";
            //    return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            //}

            ViewBag.ItemType = inventoryDal.ReadItemType();

            string challanNumberMax = inventoryDal.MRRSequenceNumberMax(objLoginHelper.LocationCode, objLoginHelper.TransactionOpenDate.ToString("yyMMdd"));
            ViewBag.ChallnSequenceNumberNew = Helper.ChallanCequenceNumberGeneration(challanNumberMax, objLoginHelper);

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Day Open Date: " + objLoginHelper.TransactionOpenDate.ToString("MMMM  dd, yyyy");
            //"Month: " + objLoginHelper.MonthOpenForInventory.ToString("MMMM-yy");

            ViewBag.OpenDay = objLoginHelper.TransactionOpenDate.Date.ToString("dd-MMM-yyyy");
            ViewBag.OpenBackDay = objLoginHelper.TransactionBackDate.Date.ToString("dd-MMM-yyyy");

            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.InventoryStockUpdateFinishClosed = true;

            if (objLoginHelper.LocationCode == "9000")
            { ViewBag.IsInventoryImplemented = 1; }
            else
            { ViewBag.IsInventoryImplemented = objLoginHelper.IsInventoryImplemented; }

            List<Aud_AdjustmentReasonCodes> newItemAdjustmentReason = new List<Aud_AdjustmentReasonCodes>();
            newItemAdjustmentReason = inventoryDal.AuditAdjustMentReasons(Helper.ReasonPurposeForReceive, Helper.ReasonForUserOrAuditor, Helper.ForInventory);
            ViewBag.AuditReasons = newItemAdjustmentReason;

            Session.Remove("ItemMasterForChallan");

            return View();
        }

        public JsonResult SaveItemIssueForAuditAdjustment(string challanMaster, string challanWithSerials, string challanDate, string challanSequenceNumber, string refChallanNumber, string itemType, string claimEmployee, string auditReason, string auditRemarks)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            Inv_ChallanMaster objChallanMaster;
            Inv_ChallanDetails objChallanDetails;
            Inv_ChallanDetailsWithSerialNo objChallanDetailsWithSerialNo;

            List<Inv_ChallanDetails> lstChallanDetails = new List<Inv_ChallanDetails>();
            List<Inv_ChallanDetailsWithSerialNo> lstChallanDetailsWithSerialNo = new List<Inv_ChallanDetailsWithSerialNo>();

            string validItemTransTypeForMRR = string.Empty, sparePartsChallanType = string.Empty;

            try
            {
                string[] challanMasterIssue = challanMaster.Split('#');
                string[] challanMasterWithSerialsIssue = challanWithSerials.Split('#');

                int challanLength = challanMasterIssue.Length, challanCounter = 0, serialLength = challanMasterWithSerialsIssue.Length, serialCounter = 0;
                string[] challanMasterItem = challanMasterIssue[0].Split(',');

                validItemTransTypeForMRR = Helper.MrrType(challanMasterItem[1].Trim(), objLoginHelper.Location);
                sparePartsChallanType = challanMasterItem[1].Trim();

                objChallanMaster = new Inv_ChallanMaster();
                objChallanMaster.LocationCode = objLoginHelper.LocationCode;
                objChallanMaster.ChallanSeqNo = challanSequenceNumber.Trim();
                objChallanMaster.ChallanDate = Convert.ToDateTime(Helper.DateFormatMMDDYYYY(challanDate.Trim()));
                objChallanMaster.FromStoreLocation = Convert.ToByte(challanMasterItem[2].Trim());        // Store Location Code

                if (challanMasterItem[1].Trim() == "ISSTRDFCSI" || challanMasterItem[1].Trim() == "ISSTRDFNEW" || challanMasterItem[1].Trim() == "ISSTRDFOLD")
                    objChallanMaster.RefChallanNo = null;
                else
                    objChallanMaster.RefChallanNo = refChallanNumber.Trim();

                objChallanMaster.ValidItemTransTypeForMRR = !string.IsNullOrEmpty(validItemTransTypeForMRR) ? validItemTransTypeForMRR : null;

                objChallanMaster.ChallanToLocationCode = null;
                objChallanMaster.IsItForMultipleLocations = false;

                objChallanMaster.ItemTransTypeID = challanMasterItem[1].Trim();                             // Issue Type
                objChallanMaster.RefCustomerCode = null;

                objChallanMaster.ItemType = itemType.Trim();
                objChallanMaster.CreatedBy = objLoginHelper.LogInID;
                objChallanMaster.CreatedOn = DateTime.Now;
                objChallanMaster.Status = Helper.Active;

                for (challanCounter = 0; challanCounter < challanLength; challanCounter++)
                {
                    string[] challanIssueItem = challanMasterIssue[challanCounter].Split(',');

                    objChallanDetails = new Inv_ChallanDetails();
                    objChallanDetails.LocationCode = objLoginHelper.LocationCode;
                    objChallanDetails.ChallanSeqNo = challanSequenceNumber;
                    objChallanDetails.CompSeqNo = Convert.ToByte(challanIssueItem[4].Trim());              // Component Sequence Number
                    objChallanDetails.ItemCode = challanIssueItem[0].Trim();                               // Item Code
                    objChallanDetails.DeliveryQuantity = Convert.ToDouble(challanIssueItem[3].Trim());     // Delivery Quantity
                    objChallanDetails.UnitCost = 0;
                    objChallanDetails.FromStoreLocation = Convert.ToByte(challanIssueItem[2].Trim());      // Store Location
                    lstChallanDetails.Add(objChallanDetails);

                    if (serialLength > 0)
                    {
                        for (serialCounter = 0; serialCounter < serialLength; serialCounter++)
                        {
                            string[] challanIssueSerialItem = challanMasterWithSerialsIssue[serialCounter].Split(',');

                            if (challanIssueSerialItem[0] == challanIssueItem[0])
                            {
                                objChallanDetailsWithSerialNo = new Inv_ChallanDetailsWithSerialNo();
                                objChallanDetailsWithSerialNo.LocationCode = objLoginHelper.LocationCode;
                                objChallanDetailsWithSerialNo.ChallanSeqNo = challanSequenceNumber.Trim();
                                objChallanDetailsWithSerialNo.CompSeqNo = Convert.ToByte(challanIssueItem[4].Trim());          // Component Sequence Number
                                objChallanDetailsWithSerialNo.ItemCode = challanIssueSerialItem[0].Trim();                     // Item Code
                                objChallanDetailsWithSerialNo.ItemSerialNo = challanIssueSerialItem[1].Trim();                 // Item Serial
                                objChallanDetailsWithSerialNo.RefStoreLocation = Convert.ToByte(challanIssueItem[2].Trim());   // Store Location Code
                                objChallanDetailsWithSerialNo.Status = Helper.Active;
                                lstChallanDetailsWithSerialNo.Add(objChallanDetailsWithSerialNo);
                            }
                        }
                    }
                }

                string challanOrMRRForAuditAdjustmentSequenceMax = inventoryDal.ChallanOrMRRForAuditAdjustmentSequenceMax(objLoginHelper.LocationCode, objLoginHelper.TransactionOpenDate.ToString("yyMMdd"));
                string auditChallanSequenceNumber = Helper.ChallanCequenceNumberGeneration(challanOrMRRForAuditAdjustmentSequenceMax, objLoginHelper);

                Aud_AuditAdjustmentRelatedChallanOrMRRForReference objChallanOrMRRForAuditAdjustment = new Aud_AuditAdjustmentRelatedChallanOrMRRForReference();

                objChallanOrMRRForAuditAdjustment.LocationCode = objLoginHelper.LocationCode;
                objChallanOrMRRForAuditAdjustment.DocSeqNo = auditChallanSequenceNumber;
                objChallanOrMRRForAuditAdjustment.RefChallanSeqNo = challanSequenceNumber.Trim();
                objChallanOrMRRForAuditAdjustment.RefMRRSeqNo = null;
                objChallanOrMRRForAuditAdjustment.ReasonCode = auditReason.Trim();

                if (string.IsNullOrEmpty(claimEmployee.Trim()))
                    objChallanOrMRRForAuditAdjustment.ResponsibleEmployeeID = null;
                else
                    objChallanOrMRRForAuditAdjustment.ResponsibleEmployeeID = claimEmployee.Trim();

                objChallanOrMRRForAuditAdjustment.AuditClaimAmount = 0;
                objChallanOrMRRForAuditAdjustment.Remarks = auditRemarks.Trim();

                objChallanMaster = inventoryDal.SaveChallanOrMRRForAuditAdjustment(objChallanMaster, lstChallanDetails, lstChallanDetailsWithSerialNo, objChallanOrMRRForAuditAdjustment);

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Issue Challan" + Helper.SuccessMessage, Helper.ChallanCequenceNumberGeneration(objChallanMaster.ChallanSeqNo, objLoginHelper)) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        //public JsonResult ClaimEmployeeList(string locationCode, string employeeLocationType)
        //{
        //    try
        //    {
        //        List<EmployeeDetailsInfo> lstLocationWiseEmployee = new List<EmployeeDetailsInfo>();
        //        lstLocationWiseEmployee = hrmsDal.ReadLocationWiseEmployee(locationCode);

        //        ArrayList arr = new ArrayList();

        //        foreach (EmployeeDetailsInfo lwemp in lstLocationWiseEmployee)
        //        {
        //            arr.Add(new { Display = lwemp.EmployeeName + " (" + lwemp.EmployeeID + ")", Value = lwemp.EmployeeID });
        //        }

        //        return new JsonResult { Data = arr };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
        //    }
        //}

        public ActionResult LocationSelectionPartial()
        {
            ViewBag.LocationType = new UserLocationType().UserLocationTypeFormat();
            return PartialView("UserLocationSelectionZoneReagonUnitWise");
        }

        public JsonResult IssueTypeList(string itemTypeId)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<Inv_ItemTransactionTypes> lstItemTransactionTypes = new List<Inv_ItemTransactionTypes>();
            lstItemTransactionTypes = inventoryDal.ReadItemTransactionIssuedTypes(itemTypeId, Helper.UserLocationType(objLoginHelper.Location), objLoginHelper.UerRoleOrGroupID);
            //inventoryDal.ReadItemTransactionIssuedReceivedTypes(itemTypeId, Helper.ReceiveOrIssue_Issue, objLoginHelper.UerRoleOrGroupID).Where(b => b.ItemTransTypeID == "ISSFAUDADJ").ToList();

            ArrayList arr = new ArrayList();

            foreach (Inv_ItemTransactionTypes itrnstyp in lstItemTransactionTypes)
            {
                arr.Add(new { Value = itrnstyp.ItemTransTypeID, Display = itrnstyp.ItemTransTypeDesc });
            }

            return new JsonResult { Data = arr };
        }

        public JsonResult StoreLocationList(string itemTypeId, string transactionType)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<Inv_StoreLocation> lstStoreLocation = new List<Inv_StoreLocation>();
            lstStoreLocation = inventoryDal.ReadStoreLocationByItemTypeAndTransaction(itemTypeId, transactionType, Helper.UnitOfficeModule, objLoginHelper.Location);

            ArrayList arr = new ArrayList();

            foreach (Inv_StoreLocation sl in lstStoreLocation)
            {
                arr.Add(new { Value = sl.StoreLocationID, Display = sl.Description });
            }

            return new JsonResult { Data = arr };
        }

        public JsonResult ItemCategoryList(string itemTypeId, string transactionType)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<Inv_ItemCategorySubCategory> lstItemCategory = new List<Inv_ItemCategorySubCategory>();
            lstItemCategory = inventoryDal.ReadItemCategorySubCatagoryByTransactionType(itemTypeId, transactionType, Helper.UnitOfficeModule);

            ArrayList arr = new ArrayList();

            foreach (Inv_ItemCategorySubCategory csc in lstItemCategory)
            {
                arr.Add(new { Value = csc.ItemCategoryID, Display = csc.Description });
            }

            return new JsonResult { Data = arr };
        }

        public JsonResult AvailAbleStoreItemQuantity(string itemCode, string storeLocation)
        {
            ArrayList arr = new ArrayList();

            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            string quantity = string.Empty;

            Inv_ItemStockByLocation objStockByLocation = new Inv_ItemStockByLocation();
            objStockByLocation = inventoryDal.ReadItemStockByLocation(Convert.ToByte(storeLocation), objLoginHelper.LocationCode, itemCode);

            Inv_ItemMaster objItemMaster = new Inv_ItemMaster();
            objItemMaster = inventoryDal.ReadItemMaster(itemCode);

            string itemName = objItemMaster.ItemName;

            if (objItemMaster.ItemName.Contains('-'))
            {
                itemName = objItemMaster.ItemName.Substring(0, (objItemMaster.ItemName.IndexOf('-') - 1));
            }

            string itemCapacityName = objItemMaster.Sal_PackageOrItemCapacity != null ? objItemMaster.Sal_PackageOrItemCapacity.Description : "";
            string itemModelName = objItemMaster.Inv_ItemModel != null ? objItemMaster.Inv_ItemModel.Description : "";

            if (objStockByLocation != null)
                quantity = objStockByLocation.AvailableQuantity.ToString("0");
            else
                quantity = "0";

            arr.Add(new { ItemName = itemName, ItemCapacityName = itemCapacityName, ItemModelName = itemModelName, AvailableQuantity = quantity });

            return new JsonResult { Data = arr };
        }

        public JsonResult MRRTypeList(string itemTypeId)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<Inv_ItemTransactionTypes> lstItemTransactionTypes = new List<Inv_ItemTransactionTypes>();
            lstItemTransactionTypes = inventoryDal.ReadItemTransactionReceivedTypes(itemTypeId, Helper.UserLocationType(objLoginHelper.Location), objLoginHelper.UerRoleOrGroupID);
            //inventoryDal.ReadItemTransactionIssuedReceivedTypes(itemTypeId, Helper.ReceiveOrIssue_Receive, objLoginHelper.UerRoleOrGroupID).Where(r => r.ItemTransTypeID == "RECFAUDADJ").ToList();

            ArrayList arr = new ArrayList();

            foreach (Inv_ItemTransactionTypes itrnstyp in lstItemTransactionTypes)
            {
                arr.Add(new { Value = itrnstyp.ItemTransTypeID, Display = itrnstyp.ItemTransTypeDesc });
            }

            return new JsonResult { Data = arr };
        }

        public JsonResult SaveItemMRRForAuditAdjustment(string mrrMaster, string mrrWithSerials, string mrrDate, string mrrSequenceNumber, string refMRRNumber, string refCustomerCode, string externalChallanNo, string externalChallanDate, string itemType, string auditReason, string auditRemarks, string claimEmployee)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            Inv_MRRMaster objMRRMaster;
            Inv_MRRDetails objMRRDetails;
            Inv_MRRDetailsWithSerialNo objMRRDetailsWithSerialNo;

            List<Inv_MRRDetails> lstMRRDetails = new List<Inv_MRRDetails>();
            List<Inv_MRRDetailsWithSerialNo> lstMRRDetailsWithSerialNo = new List<Inv_MRRDetailsWithSerialNo>();

            int challanLength = 0, challanCounter = 0, serialLength = 0, serialCounter = 0;
            string serialTempTableRows = string.Empty;

            try
            {
                string[] mrrMasterItems = mrrMaster.Split('#');
                string[] itemTransactiobWithSerials = mrrWithSerials.Split('#');

                string[] mrrMasterReceived = mrrMasterItems[0].Split(',');

                challanLength = mrrMasterItems.Length;
                serialLength = itemTransactiobWithSerials.Length;

                objMRRMaster = new Inv_MRRMaster();
                objMRRMaster.LocationCode = objLoginHelper.LocationCode;
                objMRRMaster.MRRSeqNo = mrrSequenceNumber.Trim();
                objMRRMaster.MRRDate = Convert.ToDateTime(Helper.DateFormatMMDDYYYY(mrrDate));
                objMRRMaster.ForStoreLocation = Convert.ToByte(mrrMasterReceived[2].Trim());  //Store Location

                if (!string.IsNullOrEmpty(refMRRNumber))
                    objMRRMaster.RefMRRNo = refMRRNumber.Trim();
                else
                    objMRRMaster.RefMRRNo = "NA";

                objMRRMaster.ChallanLocationCode = null;
                objMRRMaster.ChallanSeqNo = null;
                objMRRMaster.ItemTransTypeID = mrrMasterReceived[1].Trim();                  //Received Type

                if (objMRRMaster.ItemTransTypeID == "RCVCSIFCUS" || objMRRMaster.ItemTransTypeID == "RCVCUSFCUS")
                {
                    objMRRMaster.RefCustomerCode = refCustomerCode.Trim();
                }
                else
                {
                    objMRRMaster.RefCustomerCode = null;

                    if (!string.IsNullOrEmpty(externalChallanNo))
                        objMRRMaster.RefExternalChallanNo = externalChallanNo.Trim();

                    if (string.IsNullOrEmpty(externalChallanDate))
                        objMRRMaster.RefExternalChallanDate = null;
                    else
                        objMRRMaster.RefExternalChallanDate = Convert.ToDateTime(Helper.DateFormatMMDDYYYY(externalChallanDate));
                }

                objMRRMaster.ItemType = itemType;
                objMRRMaster.CreatedBy = objLoginHelper.LogInID;
                objMRRMaster.CreatedOn = DateTime.Now;
                objMRRMaster.Status = Helper.Active;

                for (challanCounter = 0; challanCounter < challanLength; challanCounter++)
                {
                    string[] mrrMasterReceivedItems = mrrMasterItems[challanCounter].Split(',');

                    objMRRDetails = new Inv_MRRDetails();
                    objMRRDetails.LocationCode = objLoginHelper.LocationCode;
                    objMRRDetails.MRRSeqNo = mrrSequenceNumber.Trim();
                    objMRRDetails.CompSeqNo = Convert.ToByte(mrrMasterReceivedItems[4].Trim());          // Component Sequence
                    objMRRDetails.ItemCode = mrrMasterReceivedItems[0].Trim(); //Item Code
                    objMRRDetails.ReceiveQuantity = Convert.ToDouble(mrrMasterReceivedItems[3].Trim());  // Transaction Quantity
                    objMRRDetails.ForStoreLocation = Convert.ToByte(mrrMasterReceivedItems[2].Trim());   // Store Location                   
                    objMRRDetails.UnitCost = Convert.ToDecimal(mrrMasterReceivedItems[6].Trim());        // Item Price
                    objMRRDetails.RefChallanLocationCode = null;
                    objMRRDetails.RefChallanSeqNo = null;
                    objMRRDetails.RefChallanCompSeqNo = null;
                    lstMRRDetails.Add(objMRRDetails);

                    if (serialLength > 0)
                    {
                        for (serialCounter = 0; serialCounter < serialLength; serialCounter++)
                        {
                            string[] mrrSerialItem = itemTransactiobWithSerials[serialCounter].Split(',');

                            if (mrrSerialItem[0] == mrrMasterReceivedItems[0])
                            {
                                objMRRDetailsWithSerialNo = new Inv_MRRDetailsWithSerialNo();
                                objMRRDetailsWithSerialNo.LocationCode = objLoginHelper.LocationCode;
                                objMRRDetailsWithSerialNo.MRRSeqNo = mrrSequenceNumber.Trim();
                                objMRRDetailsWithSerialNo.CompSeqNo = Convert.ToByte(mrrMasterReceivedItems[4].Trim());          // Component Sequence
                                objMRRDetailsWithSerialNo.ItemCode = mrrSerialItem[0].Trim();   //Item Code
                                objMRRDetailsWithSerialNo.ItemSerialNo = mrrSerialItem[1].Trim();   //Item Serial Number
                                objMRRDetailsWithSerialNo.RefChallanLocationCode = null;
                                objMRRDetailsWithSerialNo.RefChallanSeqNo = null;
                                objMRRDetailsWithSerialNo.RefChallanCompSeqNo = null;
                                objMRRDetailsWithSerialNo.Status = Helper.Active;
                                lstMRRDetailsWithSerialNo.Add(objMRRDetailsWithSerialNo);

                                if (!string.IsNullOrEmpty(serialTempTableRows))
                                    serialTempTableRows += "," + "(" + objMRRDetails.ItemCode + ",'" + objMRRDetailsWithSerialNo.ItemSerialNo + "','" + mrrMasterReceivedItems[5].Trim() + "'," + Convert.ToByte(objMRRDetails.ForStoreLocation) + ",'" + objLoginHelper.LocationCode + "','" + string.Empty + "')";
                                else
                                    serialTempTableRows = "(" + objMRRDetails.ItemCode + ",'" + objMRRDetailsWithSerialNo.ItemSerialNo + "','" + mrrMasterReceivedItems[5].Trim() + "'," + Convert.ToByte(objMRRDetails.ForStoreLocation) + ",'" + objLoginHelper.LocationCode + "','" + string.Empty + "')";
                            }
                        }
                    }
                }

                string challanOrMRRForAuditAdjustmentSequenceMax = inventoryDal.ChallanOrMRRForAuditAdjustmentSequenceMax(objLoginHelper.LocationCode, objLoginHelper.TransactionOpenDate.ToString("yyMMdd"));
                string auditChallanSequenceNumber = Helper.ChallanCequenceNumberGeneration(challanOrMRRForAuditAdjustmentSequenceMax, objLoginHelper);

                Aud_AuditAdjustmentRelatedChallanOrMRRForReference objChallanOrMRRForAuditAdjustment = new Aud_AuditAdjustmentRelatedChallanOrMRRForReference();

                objChallanOrMRRForAuditAdjustment.LocationCode = objLoginHelper.LocationCode;
                objChallanOrMRRForAuditAdjustment.DocSeqNo = auditChallanSequenceNumber;
                objChallanOrMRRForAuditAdjustment.RefChallanSeqNo = null;
                objChallanOrMRRForAuditAdjustment.RefMRRSeqNo = mrrSequenceNumber.Trim();
                objChallanOrMRRForAuditAdjustment.ReasonCode = auditReason.Trim();
                objChallanOrMRRForAuditAdjustment.ResponsibleEmployeeID = null;
                objChallanOrMRRForAuditAdjustment.AuditClaimAmount = 0;
                objChallanOrMRRForAuditAdjustment.Remarks = auditRemarks.Trim();

                if (string.IsNullOrEmpty(claimEmployee.Trim()))
                    objChallanOrMRRForAuditAdjustment.ResponsibleEmployeeID = null;
                else
                    objChallanOrMRRForAuditAdjustment.ResponsibleEmployeeID = claimEmployee.Trim();

                objMRRMaster = inventoryDal.SaveMRR(objMRRMaster, lstMRRDetails, lstMRRDetailsWithSerialNo, serialTempTableRows, objChallanOrMRRForAuditAdjustment);

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("MRR" + Helper.SuccessMessage, Helper.ChallanCequenceNumberGeneration(objMRRMaster.MRRSeqNo, objLoginHelper)) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public ActionResult ChallanInformationAtGlance()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAuditor, "ChallanInformationAtGlance", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            ViewBag.ItemType = inventoryDal.ReadItemType();
            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForInventory.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            return View();
        }

        public ActionResult MRRInformationAtGlance()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAuditor, "MRRInformationAtGlance", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            ViewBag.ItemType = inventoryDal.ReadItemType();
            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForInventory.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            return View();
        }

        public ActionResult StockInTransitAtGlance()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAuditor, "StockInTransitAtGlance", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForInventory.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;


            return View();
        }

        [GridAction]
        public ActionResult LoadChallanInformation(string itemType, DateTime dateFrom, DateTime dateTo)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<ChallanInformationGlanceDetails> lstChallanInformation = new List<ChallanInformationGlanceDetails>();
            lstChallanInformation = inventoryDal.ReadChallanInformationGlanceList(itemType, objLoginHelper.LocationCode, dateFrom, dateTo);

            return View(new GridModel<ChallanInformationGlanceDetails>
            {
                Data = lstChallanInformation
            });
        }

        [GridAction]
        public ActionResult LoadMrrInformation(string itemType, DateTime dateFrom, DateTime dateTo)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<MrrInformationGlanceDetails> lstMrrInformation = new List<MrrInformationGlanceDetails>();
            lstMrrInformation = inventoryDal.ReadMrrInformationGlanceList(itemType, objLoginHelper.LocationCode, dateFrom, dateTo);

            return View(new GridModel<MrrInformationGlanceDetails>
            {
                Data = lstMrrInformation
            });
        }

        [GridAction]
        public ActionResult LoadStockInTransitAtGlance(string challanType, string dateFrom, string dateTo)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<StockInTransitAtGlanceDetails> lstTransitAtGlance = new List<StockInTransitAtGlanceDetails>();
            lstTransitAtGlance = inventoryDal.ReadStockInTransitAtGlanceList(challanType, objLoginHelper.LocationCode, Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo));

            return View(new GridModel<StockInTransitAtGlanceDetails>
            {
                Data = lstTransitAtGlance
            });
        }

        public ActionResult ItemSerialCorrection()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForAuditor, "ItemSerialCorrection", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            ViewBag.ItemType = inventoryDal.ReadItemType();
            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForInventory.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            return View();
        }

        public JsonResult ItemSerialCorrectionChangeNClear(string locationCode, string itemCode, string optionForCorrection, string wrongSerialNo, string correctSerialNo)
        {
            try
            {
                objLoginHelper = (LoginHelper)Session["LogInInformation"];
                string serialCorrectionChangeNClear = string.Empty;

                serialCorrectionChangeNClear = inventoryDal.ItemSerialCorrectionChangeNClear(locationCode, itemCode, optionForCorrection, wrongSerialNo, correctSerialNo, objLoginHelper.LogInID);

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(Helper.SuccessMessage) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }


        }

        [GridAction]
        public ActionResult _GetItemSerialCorrectionWrongOrCorrect(string locationCode, string itemCode, string optionForCorrection, string wrongSerialNo, string correctSerialNo)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<ItemSerialCorrection> lstItemSerialCorrection = new List<ItemSerialCorrection>();
            lstItemSerialCorrection = inventoryDal.ReadItemSerialCorrectionInformation(locationCode, itemCode, optionForCorrection, wrongSerialNo, correctSerialNo, objLoginHelper.LogInID);

            return View(new GridModel<ItemSerialCorrection>
            {
                Data = lstItemSerialCorrection
            });
        }
    }
}
