using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using RASolarHelper;
using RASolarERP.Model;
using RASolarERP.Web.Areas.Inventory.Models;
using System.Collections;
using RASolarERP.Web.Models;
using RASolarERP.DomainModel.InventoryModel;

namespace RASolarERP.Web.Areas.Inventory.Controllers
{
    public class InventoryDailyTransactionController : Controller
    {
        LoginHelper objLoginHelper = new LoginHelper();
        InventoryData inventoryDal = new InventoryData();
        private RASolarERPData erpDal = new RASolarERPData();
        RASolarSecurityData securityDal = new RASolarSecurityData();

        string message = string.Empty;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StockPositionViewNUpdate()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForInventory, "StockPositionViewNUpdate", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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

            ViewBag.InventoryStockUpdateFinishClosed = inventoryDal.IsInventoryStockUpdateFinish(objLoginHelper.LocationCode);

            return View();
        }

        public ActionResult StockPositionViewNUpdateAuditObservation()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForInventory, "StockPositionViewNUpdate", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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

            ViewBag.InventoryStockUpdateFinishClosed = inventoryDal.IsInventoryStockUpdateFinish(objLoginHelper.LocationCode);

            return View();
        }

        public ActionResult InventoryLedger()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForInventory, "InventoryLedger", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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

        public ActionResult ItemIssue()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForInventory, "ItemIssue", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            if (objLoginHelper.TransactionOpenDate == Helper.InvalidDate())
            {
                Session["messageInformation"] = "Day Is Not Open For Challan";
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
            ViewBag.LocationType = new UserLocationType().UserLocationTypeFormat().Where(l => l.LocationTypeID != "region");

            if (objLoginHelper.LocationCode == "9000")
            { ViewBag.IsInventoryImplemented = 1; }
            else
            { ViewBag.IsInventoryImplemented = objLoginHelper.IsInventoryImplemented; }

            ViewBag.InventoryStockUpdateFinishClosed = true; //inventoryDal.IsInventoryStockUpdateFinish(objLoginHelper.LocationCode);

            return View();
        }

        public ActionResult ItemReceiveWithChallan()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForInventory, "ItemReceiveWithChallan", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            if (objLoginHelper.TransactionOpenDate == Helper.InvalidDate())
            {
                Session["messageInformation"] = "Day Is Not Open For MRR (With Challan)";
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            List<ChallanInboxForChallanWithMRR> lstChallanInbox = new List<ChallanInboxForChallanWithMRR>();

            ViewBag.ItemType = inventoryDal.ReadItemType();

            lstChallanInbox = inventoryDal.ReadChallanInbox(objLoginHelper.LocationCode, Helper.InventoryItem);

            string challanNumberMax = inventoryDal.MRRSequenceNumberMax(objLoginHelper.LocationCode, objLoginHelper.TransactionOpenDate.ToString("yyMMdd"));
            ViewBag.ChallnSequenceNumberNew = Helper.ChallanCequenceNumberGeneration(challanNumberMax, objLoginHelper);

            List<ChallanInboxForChallanWithMRR> cinb = new List<ChallanInboxForChallanWithMRR>();
            cinb = inventoryDal.ReadChallanInbox(objLoginHelper.LocationCode, Helper.InventoryItem);

            foreach (ChallanInboxForChallanWithMRR ci in cinb)
            {
                ci.ChallanSeqNo = ci.ChallanSeqNo + "-" + ci.LocationCode;
            }

            ViewBag.ChallanInbox = cinb;

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

            ViewBag.InventoryStockUpdateFinishClosed = inventoryDal.IsInventoryStockUpdateFinish(objLoginHelper.LocationCode);

            return View();
        }

        public ActionResult ItemReceiveWithoutChallan()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForInventory, "ItemReceiveWithoutChallan", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            if (objLoginHelper.TransactionOpenDate == Helper.InvalidDate())
            {
                Session["messageInformation"] = "Day Is Not Open For MRR (Without Challan)";
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

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

            ViewBag.OpenDay = Convert.ToDateTime(objLoginHelper.TransactionOpenDate.Date).ToString("dd-MMM-yyyy");
            ViewBag.OpenBackDay = objLoginHelper.TransactionBackDate.Date.ToString("dd-MMM-yyyy");

            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            ViewBag.InventoryStockUpdateFinishClosed = true;

            if (objLoginHelper.LocationCode == "9000")
            { ViewBag.IsInventoryImplemented = 1; }
            else
            { ViewBag.IsInventoryImplemented = objLoginHelper.IsInventoryImplemented; }

            Session.Remove("ItemMasterForChallan");

            return View();
        }

        public ActionResult ChallanWithMultipleLocation()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForInventory, "ChallanWithMultipleLocation", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            if (objLoginHelper.TransactionOpenDate == Helper.InvalidDate())
            {
                Session["messageInformation"] = "Day Is Not Open For Challan";
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
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForInventory.ToString("MMMM-yy");

            ViewBag.OpenDay = objLoginHelper.TransactionOpenDate.Date.ToString("dd-MMM-yyyy");
            ViewBag.OpenBackDay = objLoginHelper.TransactionBackDate.Date.ToString("dd-MMM-yyyy");

            string challanNumberMax = inventoryDal.ChallanSequenceNumberMax(objLoginHelper.LocationCode, objLoginHelper.TransactionOpenDate.ToString("yyMMdd"));
            string challnSequenceNumberNew = Helper.ChallanCequenceNumberGeneration(challanNumberMax, objLoginHelper);

            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;
            ViewBag.ChallnSequenceNumberNew = challnSequenceNumberNew;

            ViewBag.Zone = erpDal.Zone();
            ViewBag.LocationType = new UserLocationType().UserLocationTypeFormat().Where(l => l.LocationTypeID != "ho");

            if (objLoginHelper.LocationCode == "9000")
            { ViewBag.IsInventoryImplemented = 1; }
            else
            { ViewBag.IsInventoryImplemented = objLoginHelper.IsInventoryImplemented; }

            ViewBag.InventoryStockUpdateFinishClosed = true; //inventoryDal.IsInventoryStockUpdateFinish(objLoginHelper.LocationCode);

            return View();
        }

        public JsonResult SaveItemIssue(string challanMaster, string challanWithSerials, string challanDate, string challanSequenceNumber, string refChallanNumber, string refCustomerCode, string itemType, string particulars, string vendorId)
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

                if (challanMasterItem[3].Trim() != "")
                {
                    objChallanMaster.ChallanToLocationCode = challanMasterItem[3].Trim();                   // Challan To Location
                    objChallanMaster.IsItForMultipleLocations = false;
                }
                else
                {
                    objChallanMaster.ChallanToLocationCode = null;
                    objChallanMaster.IsItForMultipleLocations = false;
                }

                objChallanMaster.ItemTransTypeID = challanMasterItem[1].Trim();                         // Issue Type

               // if (objChallanMaster.ItemTransTypeID == "ISSCSITCUS" || objChallanMaster.ItemTransTypeID == "ISSCUSTCUS")  // original
                if (objChallanMaster.ItemTransTypeID == "ISSCSITCUS" || objChallanMaster.ItemTransTypeID == "ISSCUSTCUS" || objChallanMaster.ItemTransTypeID == "ISSTOCUSPG")    // modified for ISSTOCUSPG
                    objChallanMaster.RefCustomerCode = refCustomerCode.Trim();
                else
                    objChallanMaster.RefCustomerCode = null;


                objChallanMaster.RefVendorID = string.IsNullOrEmpty(vendorId) ? null : vendorId;

                objChallanMaster.ItemType = itemType.Trim();
                objChallanMaster.CreatedBy = objLoginHelper.LogInID;
                objChallanMaster.CreatedOn = DateTime.Now;
                objChallanMaster.Status = Helper.Active;
                objChallanMaster.Particulars = particulars.Trim();

                for (challanCounter = 0; challanCounter < challanLength; challanCounter++)
                {
                    string[] challanIssueItem = challanMasterIssue[challanCounter].Split(',');

                    objChallanDetails = new Inv_ChallanDetails();
                    objChallanDetails.LocationCode = objLoginHelper.LocationCode;
                    objChallanDetails.ChallanSeqNo = challanSequenceNumber;
                    objChallanDetails.CompSeqNo = Convert.ToByte(challanIssueItem[5].Trim());              // Component Sequence Number
                    objChallanDetails.ItemCode = challanIssueItem[0].Trim();                               // Item Code
                    objChallanDetails.DeliveryQuantity = Convert.ToDouble(challanIssueItem[4].Trim());     // Delivery Quantity
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
                                objChallanDetailsWithSerialNo.CompSeqNo = Convert.ToByte(challanIssueItem[5].Trim());          // Component Sequence Number
                                objChallanDetailsWithSerialNo.ItemCode = challanIssueSerialItem[0].Trim();                     // Item Code
                                objChallanDetailsWithSerialNo.ItemSerialNo = challanIssueSerialItem[1].Trim();                 // Item Serial
                                objChallanDetailsWithSerialNo.RefStoreLocation = Convert.ToByte(challanIssueItem[2].Trim());   // Store Location Code
                                objChallanDetailsWithSerialNo.Status = Helper.Active;
                                lstChallanDetailsWithSerialNo.Add(objChallanDetailsWithSerialNo);
                            }
                        }
                    }
                }

                objChallanMaster = inventoryDal.SaveIssueChallan(objChallanMaster, lstChallanDetails, lstChallanDetailsWithSerialNo);
                erpDal.CreateUserLog(Helper.ClientIPAddress(), string.Empty, objLoginHelper.LocationCode, objLoginHelper.LogInID, "Challan, Challan Sequence Number: " + challanSequenceNumber.Trim());

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Issue Challan" + Helper.SuccessMessage, Helper.ChallanCequenceNumberGeneration(objChallanMaster.ChallanSeqNo, objLoginHelper)) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public JsonResult SaveMRRWithChallan(string mrrMaster, string mrrWithSerials, string mrrDate, string mrrSequenceNumber, string refMRRNumber, string itemCondition, string itemType, string particulars)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            Inv_MRRMaster objMRRMaster;
            Inv_MRRDetails objMRRDetails;
            Inv_MRRDetailsWithSerialNo objMRRDetailsWithSerialNo;

            List<Inv_MRRDetails> lstMRRDetails = new List<Inv_MRRDetails>();
            List<Inv_MRRDetailsWithSerialNo> lstMRRDetailsWithSerialNo = new List<Inv_MRRDetailsWithSerialNo>();

            int challanLength = 0, challanCounter = 0, serialLength = 0, serialCounter = 0;
            double challanQuantity = 0, alreadyReceiveQuantity = 0, currentReceiveQuantity = 0;

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
                objMRRMaster.RefMRRNo = refMRRNumber.Trim();
                objMRRMaster.ChallanLocationCode = mrrMasterReceived[5].Trim();              //Challan From Location
                objMRRMaster.ChallanSeqNo = mrrMasterReceived[4].Trim();                     //Challan Sequence Number
                objMRRMaster.ItemTransTypeID = mrrMasterReceived[1].Trim();                  //Received Type
                objMRRMaster.ItemType = itemType.Trim();
                objMRRMaster.CreatedBy = objLoginHelper.LogInID;
                objMRRMaster.CreatedOn = DateTime.Now;
                objMRRMaster.Status = Helper.Active;
                objMRRMaster.Particulars = particulars.Trim();

                for (challanCounter = 0; challanCounter < challanLength; challanCounter++)
                {
                    string[] mrrMasterReceivedItems = mrrMasterItems[challanCounter].Split(',');

                    objMRRDetails = new Inv_MRRDetails();
                    objMRRDetails.LocationCode = objLoginHelper.LocationCode;
                    objMRRDetails.MRRSeqNo = mrrSequenceNumber.Trim();
                    objMRRDetails.CompSeqNo = Convert.ToByte(mrrMasterReceivedItems[6].Trim());          // Component Sequence
                    objMRRDetails.ItemCode = mrrMasterReceivedItems[0].Trim(); //Item Code

                    challanQuantity = Convert.ToDouble(mrrMasterReceivedItems[8].Trim());         // Challan Quantity
                    alreadyReceiveQuantity = Convert.ToDouble(mrrMasterReceivedItems[9].Trim());  // Already Receive Quantity
                    currentReceiveQuantity = Convert.ToDouble(mrrMasterReceivedItems[3].Trim());  // Receive Quantity

                    objMRRDetails.ReceiveQuantity = currentReceiveQuantity;
                    objMRRDetails.ForStoreLocation = Helper.MRRItemsCondition(Convert.ToByte(mrrMasterReceivedItems[2].Trim()), Convert.ToByte(itemCondition));                     //Convert.ToByte(mrrMasterReceivedItems[2].Trim());  //Store Location;
                    objMRRDetails.RefChallanLocationCode = mrrMasterReceivedItems[5];          //Challan Location Code
                    objMRRDetails.RefChallanSeqNo = mrrMasterReceivedItems[4];          //Challan Seqence Number
                    objMRRDetails.RefChallanCompSeqNo = Convert.ToByte(mrrMasterReceivedItems[7]);  // Ref Challan Seqence Number
                    objMRRDetails.UnitCost = 0;
                    lstMRRDetails.Add(objMRRDetails);

                    if (serialLength > 0 && mrrWithSerials.Trim() != string.Empty)
                    {
                        for (serialCounter = 0; serialCounter < serialLength; serialCounter++)
                        {
                            string[] mrrSerialItem = itemTransactiobWithSerials[serialCounter].Split(',');

                            if (mrrSerialItem[0] == mrrMasterReceivedItems[0])
                            {
                                objMRRDetailsWithSerialNo = new Inv_MRRDetailsWithSerialNo();
                                objMRRDetailsWithSerialNo.LocationCode = objLoginHelper.LocationCode;
                                objMRRDetailsWithSerialNo.MRRSeqNo = mrrSequenceNumber.Trim();
                                objMRRDetailsWithSerialNo.CompSeqNo = Convert.ToByte(mrrMasterReceivedItems[6].Trim());          // Component Sequence
                                objMRRDetailsWithSerialNo.ItemCode = mrrSerialItem[0].Trim();   //Item Code
                                objMRRDetailsWithSerialNo.ItemSerialNo = mrrSerialItem[1].Trim();   //Item Serial Number
                                objMRRDetailsWithSerialNo.RefChallanLocationCode = mrrMasterReceivedItems[5];          //Challan Location Code
                                objMRRDetailsWithSerialNo.RefChallanSeqNo = mrrMasterReceivedItems[4];          //Challan Seqence Number
                                objMRRDetailsWithSerialNo.RefChallanCompSeqNo = Convert.ToByte(mrrMasterReceivedItems[7]);  // Ref Challan Seqence Number
                                objMRRDetailsWithSerialNo.Status = Helper.Active;
                                lstMRRDetailsWithSerialNo.Add(objMRRDetailsWithSerialNo);
                            }
                        }
                    }
                }

                objMRRMaster = inventoryDal.SaveMRR(objMRRMaster, lstMRRDetails, lstMRRDetailsWithSerialNo);
                erpDal.CreateUserLog(Helper.ClientIPAddress(), string.Empty, objLoginHelper.LocationCode, objLoginHelper.LogInID, "MRR, MRR Sequence Number: " + mrrSequenceNumber.Trim());

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("MRR" + Helper.SuccessMessage, Helper.ChallanCequenceNumberGeneration(objMRRMaster.MRRSeqNo, objLoginHelper)) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public JsonResult SaveMRRWithoutChallan(string mrrMaster, string mrrWithSerials, string mrrDate, string mrrSequenceNumber, string refMRRNumber, string refCustomerCode, string externalChallanNo, string externalChallanDate, string itemType, string particulars, string vendorId)
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
                objMRRMaster.RefMRRNo = refMRRNumber.Trim();
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
                    objMRRMaster.RefExternalChallanNo = externalChallanNo.Trim();
                    objMRRMaster.RefExternalChallanDate = Convert.ToDateTime(Helper.DateFormatMMDDYYYY(externalChallanDate));
                }

                objMRRMaster.RefVendorID = string.IsNullOrEmpty(vendorId) ? null : vendorId;

                objMRRMaster.ItemType = itemType;
                objMRRMaster.CreatedBy = objLoginHelper.LogInID;
                objMRRMaster.CreatedOn = DateTime.Now;
                objMRRMaster.Status = Helper.Active;
                objMRRMaster.Particulars = particulars.Trim();

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

                objMRRMaster = inventoryDal.SaveMRR(objMRRMaster, lstMRRDetails, lstMRRDetailsWithSerialNo, serialTempTableRows);
                erpDal.CreateUserLog(Helper.ClientIPAddress(), string.Empty, objLoginHelper.LocationCode, objLoginHelper.LogInID, "MRR Without Challan, MRR Sequence Number: " + mrrSequenceNumber.Trim());

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("MRR" + Helper.SuccessMessage, Helper.ChallanCequenceNumberGeneration(objMRRMaster.MRRSeqNo, objLoginHelper)) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public JsonResult SaveMRRWithaChallanWithMultipleLocation(string challanMaster, string challanDetails, string challanWithSerials, string challanWithMultipleLocation, string challanItemDistribution, string itemType)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            Inv_ChallanMaster objChallanMaster;
            Inv_ChallanDetails objChallanDetails;
            Inv_ChallanDetailsWithSerialNo objChallanDetailsWithSerialNo;
            Inv_ChallanWithMultipleLocations objChallanWithMultipleLocations;
            Inv_ChallanWithMultipleLocationsWithItemWiseDistribution objChallanItemDistribution;

            List<Inv_ChallanDetails> lstChallanDetails = new List<Inv_ChallanDetails>();
            List<Inv_ChallanDetailsWithSerialNo> lstChallanDetailsWithSerialNo = new List<Inv_ChallanDetailsWithSerialNo>();
            List<Inv_ChallanWithMultipleLocations> lstChallanWithMultipleLocations = new List<Inv_ChallanWithMultipleLocations>();
            List<Inv_ChallanWithMultipleLocationsWithItemWiseDistribution> lstChallanItemDistribution = new List<Inv_ChallanWithMultipleLocationsWithItemWiseDistribution>();

            int rowLength = 0, counter = 0;
            string[] challanIssueMaster = challanMaster.Trim().Split(',');
            string[] challanIssueDetails = challanDetails.Trim().Split('#');
            string[] challanIssueWithSerial = challanWithSerials.Trim().Split('#');
            string[] challanIssueWithMultipleLocation = challanWithMultipleLocation.Trim().Split('#');
            string[] challanIssueItemDistribution = challanItemDistribution.Trim().Split('#');

            string validItemTransTypeForMRR = string.Empty;
            validItemTransTypeForMRR = Helper.MrrType(challanIssueMaster[4].Trim(), objLoginHelper.Location);

            try
            {
                objChallanMaster = new Inv_ChallanMaster();
                objChallanMaster.LocationCode = objLoginHelper.LocationCode;
                objChallanMaster.ChallanSeqNo = challanIssueMaster[0].Trim();  // Challan Sequence Number
                objChallanMaster.ChallanDate = Convert.ToDateTime(Helper.DateFormatMMDDYYYY(challanIssueMaster[1].Trim())); // Challan Date
                objChallanMaster.FromStoreLocation = Convert.ToByte(challanIssueMaster[2].Trim());        // Store Location Code
                objChallanMaster.RefChallanNo = challanIssueMaster[3].Trim();
                objChallanMaster.ValidItemTransTypeForMRR = !string.IsNullOrEmpty(validItemTransTypeForMRR) ? validItemTransTypeForMRR : null;
                objChallanMaster.ChallanToLocationCode = null;                   // Challan To Location
                objChallanMaster.IsItForMultipleLocations = true;
                objChallanMaster.RefCustomerCode = null;
                objChallanMaster.ItemTransTypeID = challanIssueMaster[4].Trim();                         // Issue Type
                objChallanMaster.ItemType = itemType.Trim();
                objChallanMaster.CreatedBy = objLoginHelper.LogInID;
                objChallanMaster.CreatedOn = DateTime.Now;
                objChallanMaster.Status = Helper.Active;

                rowLength = challanIssueDetails.Length;
                for (counter = 0; counter < rowLength; counter++)
                {
                    string[] challanItemDetails = challanIssueDetails[counter].Split(',');

                    objChallanDetails = new Inv_ChallanDetails();
                    objChallanDetails.LocationCode = objLoginHelper.LocationCode;
                    objChallanDetails.ChallanSeqNo = challanItemDetails[0];                                  // Challan Sequence Number
                    objChallanDetails.CompSeqNo = Convert.ToByte(challanItemDetails[1].Trim());              // Component Sequence Number
                    objChallanDetails.ItemCode = challanItemDetails[2].Trim();                               // Item Code
                    objChallanDetails.DeliveryQuantity = Convert.ToDouble(challanItemDetails[3].Trim());     // Delivery Quantity
                    objChallanDetails.UnitCost = 0;
                    objChallanDetails.FromStoreLocation = Convert.ToByte(challanItemDetails[4].Trim());      // Store Location
                    lstChallanDetails.Add(objChallanDetails);
                }

                rowLength = challanIssueWithSerial.Length;

                if (challanWithSerials.Trim() != string.Empty)
                {
                    for (counter = 0; counter < rowLength; counter++)
                    {
                        string[] challanSerialNumber = challanIssueWithSerial[counter].Split(',');

                        objChallanDetailsWithSerialNo = new Inv_ChallanDetailsWithSerialNo();
                        objChallanDetailsWithSerialNo.LocationCode = objLoginHelper.LocationCode;
                        objChallanDetailsWithSerialNo.ChallanSeqNo = challanSerialNumber[0].Trim();                 // Challan Sequence Number
                        objChallanDetailsWithSerialNo.CompSeqNo = Convert.ToByte(challanSerialNumber[1].Trim());    // Component Sequence Number
                        objChallanDetailsWithSerialNo.ItemCode = challanSerialNumber[2].Trim();                     // Item Code
                        objChallanDetailsWithSerialNo.ItemSerialNo = challanSerialNumber[3].Trim();                 // Item Serial
                        objChallanDetailsWithSerialNo.RefStoreLocation = Convert.ToByte(challanSerialNumber[4].Trim());   // Store Location Code
                        objChallanDetailsWithSerialNo.Status = Helper.Active;
                        lstChallanDetailsWithSerialNo.Add(objChallanDetailsWithSerialNo);
                    }
                }

                rowLength = challanIssueWithMultipleLocation.Length;
                for (counter = 0; counter < rowLength; counter++)
                {
                    string[] challanLocation = challanIssueWithMultipleLocation[counter].Split(',');

                    objChallanWithMultipleLocations = new Inv_ChallanWithMultipleLocations();
                    objChallanWithMultipleLocations.ChallanToLocationCode = challanLocation[0].Trim();  // Location Code
                    objChallanWithMultipleLocations.ChallanFromLocationCode = objLoginHelper.LocationCode;
                    objChallanWithMultipleLocations.ChallanSeqNo = challanLocation[1].Trim(); // Challan Sequence Number
                    objChallanWithMultipleLocations.IsReceivedByLocation = false;
                    lstChallanWithMultipleLocations.Add(objChallanWithMultipleLocations);
                }

                rowLength = challanIssueItemDistribution.Length;
                for (counter = 0; counter < rowLength; counter++)
                {
                    string[] itemDistribution = challanIssueItemDistribution[counter].Split(',');

                    objChallanItemDistribution = new Inv_ChallanWithMultipleLocationsWithItemWiseDistribution();
                    objChallanItemDistribution.ChallanToLocationCode = itemDistribution[0].Trim();  //Challan To Location Code
                    objChallanItemDistribution.ChallanFromLocationCode = objLoginHelper.LocationCode;
                    objChallanItemDistribution.ChallanSeqNo = itemDistribution[1].Trim();  //Challan Sequence Number
                    objChallanItemDistribution.CompSeqNo = Convert.ToByte(itemDistribution[2].Trim());   //Component Sequence Number
                    objChallanItemDistribution.ItemCode = itemDistribution[3].Trim();  // Item Code
                    objChallanItemDistribution.DeliveryQuantity = Convert.ToDouble(itemDistribution[4].Trim());  // Delivery Quantity
                    lstChallanItemDistribution.Add(objChallanItemDistribution);
                }

                objChallanMaster = inventoryDal.SaveIssueChallan(objChallanMaster, lstChallanDetails, lstChallanDetailsWithSerialNo, lstChallanWithMultipleLocations, lstChallanItemDistribution);
                erpDal.CreateUserLog(Helper.ClientIPAddress(), string.Empty, objLoginHelper.LocationCode, objLoginHelper.LogInID, "Challan With Multiple Location, Challan Sequence Number: " + objChallanMaster.ChallanSeqNo);

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Issue Challan" + Helper.SuccessMessage, Helper.ChallanCequenceNumberGeneration(objChallanMaster.ChallanSeqNo, objLoginHelper)) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        [GridAction]
        public ActionResult _ItemDetailsForChallan(string itemCode, string deleteAction)
        {
            List<Inv_ItemMaster> lstItemMasterProcess = new List<Inv_ItemMaster>();

            if (itemCode == null)
            { return View(new GridModel<Inv_ItemMaster> { Data = lstItemMasterProcess }); }

            Inv_ItemMaster objItemMaster = new Inv_ItemMaster();

            if (deleteAction == string.Empty)
                objItemMaster = inventoryDal.ReadItemMaster(itemCode);

            if (Session["ItemMasterForChallan"] == null)
            {
                lstItemMasterProcess.Add(objItemMaster);
                Session["ItemMasterForChallan"] = lstItemMasterProcess;
            }
            else
            {
                if (Session["ItemMasterForChallan"] != null)
                    lstItemMasterProcess = (List<Inv_ItemMaster>)Session["ItemMasterForChallan"];

                if (deleteAction == "")
                {
                    if (objItemMaster != null)
                        lstItemMasterProcess.Add(objItemMaster);
                }
                else
                {
                    objItemMaster = lstItemMasterProcess.Where(i => i.ItemCode == itemCode).FirstOrDefault();

                    if (objItemMaster != null)
                        lstItemMasterProcess.Remove(objItemMaster);
                }

                Session.Remove("ItemMasterForChallan");
                Session["ItemMasterForChallan"] = lstItemMasterProcess;
            }

            return View(new GridModel<Inv_ItemMaster> { Data = lstItemMasterProcess });
        }

        public ActionResult ChallanOrMRRItemSerials(string challanSeqNo, string mrrSeqNo, string itemCode, string storeLocation)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<ChallanOrMRRItemsSerial> itemSerial = new List<ChallanOrMRRItemsSerial>();

            if (storeLocation == "0")
            {
                itemSerial = inventoryDal.ReadChallanOrMRRItemsSerial(objLoginHelper.LocationCode, challanSeqNo, mrrSeqNo, itemCode);
            }
            else
            {
                List<Inv_ItemStockWithSerialNoByLocation> lstItemStockWise = new List<Inv_ItemStockWithSerialNoByLocation>();
                lstItemStockWise = inventoryDal.ReadItemStockWithSerialNoByLocation(Convert.ToByte(storeLocation), objLoginHelper.LocationCode, itemCode);
                ChallanOrMRRItemsSerial objChallanOrMRRItemsSerial;

                foreach (Inv_ItemStockWithSerialNoByLocation ss in lstItemStockWise)
                {
                    objChallanOrMRRItemsSerial = new ChallanOrMRRItemsSerial();
                    objChallanOrMRRItemsSerial.ItemCode = ss.ItemCode;
                    objChallanOrMRRItemsSerial.ItemName = string.Empty;
                    objChallanOrMRRItemsSerial.ItemSerialNumber = ss.ItemSerialNo;

                    itemSerial.Add(objChallanOrMRRItemsSerial);
                }
            }

            return PartialView("ChallanOrMrrItemSerialDisplay", itemSerial);
        }

        [HttpGet]
        public ActionResult _PartialPick(string itemCode)
        {
            List<InvAvailableItemInALocation> lstItemInLocation = new List<InvAvailableItemInALocation>();

            lstItemInLocation = (List<InvAvailableItemInALocation>)Session["ItemInLocation"];
            string itemDetails = (string)Session["ItemDetails"];
            string[] iit = itemDetails.Split('#');

            InvAvailableItemInALocation objAvailableItem = (from kk in lstItemInLocation where kk.ItemCode == itemCode select kk).FirstOrDefault();

            ViewBag.ItemCode = itemCode;
            ViewBag.ItemCategory = iit[1];
            ViewBag.ItemType = iit[2];
            ViewBag.StoreLocation = iit[0];
            ViewBag.ItemName = objAvailableItem.ItemName;
            ViewBag.AvailableQuantity = Convert.ToString(objAvailableItem.AvailableQuantity);
            ViewBag.CapacityModel = objAvailableItem.ItemModel + " / " + objAvailableItem.ItemCapacity;

            return PartialView("SerialMappingWithItem");
        }

        [GridAction]
        public ActionResult __LoadInventoryItemLocationWise(string storeLocation, string itemCatagory, string itemDetails)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<InvAvailableItemInALocation> lstItemInLocation = new List<InvAvailableItemInALocation>();
            lstItemInLocation = inventoryDal.ReadInvAvailableItemInALocation(objLoginHelper.LocationCode, Convert.ToByte(storeLocation), itemCatagory);

            Session["ItemInLocation"] = lstItemInLocation;
            Session["ItemDetails"] = itemDetails;

            return View(new GridModel<InvAvailableItemInALocation>
            {
                Data = lstItemInLocation
            });
        }

        [HttpGet]
        public ActionResult _PartialAuditObservation(string itemCode)
        {
            List<InvAvailableItemInALocation> lstItemInLocation = new List<InvAvailableItemInALocation>();

            lstItemInLocation = (List<InvAvailableItemInALocation>)Session["ItemInLocation"];
            string itemDetails = (string)Session["ItemDetails"];
            string[] iit = itemDetails.Split('#');

            InvAvailableItemInALocation objAvailableItem = (from kk in lstItemInLocation where kk.ItemCode == itemCode select kk).FirstOrDefault();

            ViewBag.ItemCode = itemCode;
            ViewBag.ItemCategory = iit[1];
            ViewBag.ItemType = iit[2];
            ViewBag.StoreLocation = iit[0];
            ViewBag.ItemName = objAvailableItem.ItemName;
            ViewBag.AvailableQuantity = Convert.ToString(objAvailableItem.AvailableQuantity);
            ViewBag.CapacityModel = objAvailableItem.ItemModel + " / " + objAvailableItem.ItemCapacity;

            return PartialView("SerialMappingWithItemAuditObservation");
        }

        public ActionResult SerialMapping(string ic)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            PageAccessRightHelper objPageAccessRightHelper = new PageAccessRightHelper();
            objPageAccessRightHelper = securityDal.ReadPageAccessRight(Helper.ForInventory, "SerialMapping", objLoginHelper.UerRoleOrGroupID, Helper.Inactive);
            if (objPageAccessRightHelper != null)
            {
                if (objPageAccessRightHelper.AccessStatus == Helper.Inactive)
                {
                    Session["messageInformation"] = objPageAccessRightHelper.MessageToShow;
                    return RedirectToAction("ErrorMessage", "../ErrorHnadle");
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
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForInventory.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            List<InvAvailableItemInALocation> lstItemInLocation = new List<InvAvailableItemInALocation>();

            lstItemInLocation = (List<InvAvailableItemInALocation>)Session["ItemInLocation"];
            string itemDetails = (string)Session["ItemDetails"];
            string[] iit = itemDetails.Split('#');

            InvAvailableItemInALocation objAvailableItem = (from kk in lstItemInLocation where kk.ItemCode == ic select kk).FirstOrDefault();

            ViewBag.ItemCode = ic;
            ViewBag.ItemCategory = iit[1];
            ViewBag.ItemType = iit[2];
            ViewBag.StoreLocation = iit[0];
            ViewBag.ItemName = objAvailableItem.ItemName;
            ViewBag.AvailableQuantity = Convert.ToString(objAvailableItem.AvailableQuantity);

            return View();
        }

        public JsonResult ItemCategoryList(string itemTypeId, string transactionType)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<Inv_ItemCategorySubCategory> lstItemCategory = new List<Inv_ItemCategorySubCategory>();
            lstItemCategory = inventoryDal.ReadItemCategorySubCatagoryByTransactionType(itemTypeId, transactionType, Helper.UserLocationType(objLoginHelper.Location));

            string IsRangeWiseSerialSelectionForChallanEnabled = "0";
            ArrayList arr = new ArrayList();


            foreach (Inv_ItemCategorySubCategory csc in lstItemCategory)
            {
                if (csc.IsRangeWiseSerialSelectionForChallanEnabled)
                    IsRangeWiseSerialSelectionForChallanEnabled = "1";

                arr.Add(new { Value = csc.ItemCategoryID, Display = csc.Description, IsRangeWiseSerialSelectionForChallanEnabled = IsRangeWiseSerialSelectionForChallanEnabled }); //, IsItemCodeWiseValidationExist = csc.IsItemCodeWiseValidationExist 
            }

            return new JsonResult { Data = arr };
        }

        public JsonResult ItemCategorySubCategoryList(string itemTypeId)
        {
            List<Inv_ItemCategorySubCategory> lstItemCategory = new List<Inv_ItemCategorySubCategory>();
            lstItemCategory = inventoryDal.ReadItemCategorySubCatagory(itemTypeId);

            ArrayList arr = new ArrayList();

            foreach (Inv_ItemCategorySubCategory csc in lstItemCategory)
            {
                arr.Add(new { Value = csc.ItemCategoryID, Display = csc.Description });
            }

            return new JsonResult { Data = arr };
        }

        public JsonResult MRRTypeList(string itemTypeId)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<Inv_ItemTransactionTypes> lstItemTransactionTypes = new List<Inv_ItemTransactionTypes>();
            lstItemTransactionTypes = inventoryDal.ReadItemTransactionReceivedTypes(itemTypeId, Helper.UserLocationType(objLoginHelper.Location), objLoginHelper.UerRoleOrGroupID).Where(m => m.ItemTransTypeID != "RCVEFROMHO" && m.ItemTransTypeID != "RCVFRMORUT" && m.ItemTransTypeID != "RCVFRMZNLO").ToList();

            ArrayList arr = new ArrayList();

            foreach (Inv_ItemTransactionTypes itrnstyp in lstItemTransactionTypes)
            {
                arr.Add(new { Value = itrnstyp.ItemTransTypeID, Display = itrnstyp.ItemTransTypeDesc });
            }

            return new JsonResult { Data = arr };
        }

        public JsonResult MRRTypeListForMRRWithChallan(string itemTypeId, string validItemTransTypeForMRR)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<Inv_ItemTransactionTypes> lstItemTransactionTypes = new List<Inv_ItemTransactionTypes>();
            lstItemTransactionTypes = inventoryDal.ReadItemTransactionReceivedTypes(itemTypeId, Helper.UserLocationType(objLoginHelper.Location), objLoginHelper.UerRoleOrGroupID).Where(i => i.ItemTransTypeID == validItemTransTypeForMRR.Trim()).ToList();

            ArrayList arr = new ArrayList();

            foreach (Inv_ItemTransactionTypes itrnstyp in lstItemTransactionTypes)
            {
                arr.Add(new { Value = itrnstyp.ItemTransTypeID, Display = itrnstyp.ItemTransTypeDesc });
            }

            return new JsonResult { Data = arr };
        }

        public JsonResult IssueTypeList(string itemTypeId)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<Inv_ItemTransactionTypes> lstItemTransactionTypes = new List<Inv_ItemTransactionTypes>();
            lstItemTransactionTypes = inventoryDal.ReadItemTransactionIssuedTypes(itemTypeId, Helper.UserLocationType(objLoginHelper.Location), objLoginHelper.UerRoleOrGroupID).Where(s => s.ItemTransTypeID != "ISSFSPSALE").ToList();

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
            lstStoreLocation = inventoryDal.ReadStoreLocationByItemTypeAndTransaction(itemTypeId, transactionType, Helper.UserLocationType(objLoginHelper.Location), objLoginHelper.Location);

            ArrayList arr = new ArrayList();

            foreach (Inv_StoreLocation sl in lstStoreLocation)
            {
                arr.Add(new { Value = sl.StoreLocationID, Display = sl.Description });
            }

            return new JsonResult { Data = arr };
        }

        public JsonResult StoreLocationListForMRRWithChallan(string storeLocationForMrrMaster, string itemTypeId)
        {
            List<Inv_StoreLocation> lstStoreLocation = new List<Inv_StoreLocation>();
            lstStoreLocation = inventoryDal.ReadStoreLocation(Convert.ToInt32(storeLocationForMrrMaster), itemTypeId);

            ArrayList arr = new ArrayList();

            foreach (Inv_StoreLocation sl in lstStoreLocation)
            {
                arr.Add(new { Value = sl.StoreLocationID, Display = sl.Description });
            }

            return new JsonResult { Data = arr };
        }

        public JsonResult ItemList(string itemCategory, string itemTransType)
        {
            string SerializableItemOrNot = "0";

            List<Inv_ItemMaster> lstItems = new List<Inv_ItemMaster>();


            Inv_ItemCategorySubCategory objItemCategorySubCategory = new Inv_ItemCategorySubCategory();
            objItemCategorySubCategory = inventoryDal.ReadItemCategorySubCategoryByCategoryID(itemCategory);

            if (inventoryDal.ReadIsItemCodeWiseValidationExist(itemCategory, itemTransType))
            {
                lstItems = inventoryDal.ReadInvItems(itemCategory, itemTransType);
            }
            else
            {
                lstItems = inventoryDal.ReadInvItems(itemCategory);
            }

            if (objItemCategorySubCategory != null && objItemCategorySubCategory.IsItASerializableItem == true)
            {
                SerializableItemOrNot = "1";
            }

            ArrayList arr = new ArrayList();

            foreach (Inv_ItemMaster csc in lstItems)
            {
                arr.Add(new { Value = csc.ItemCode + "-" + csc.ItemCapacity + "-" + csc.ItemModel, Display = csc.ItemName, IsItASerializableItem = SerializableItemOrNot });
            }

            return new JsonResult { Data = arr };
        }

        public JsonResult ItemListByCategory(string itemCategory)
        {
            string SerializableItemOrNot = "0";

            List<Inv_ItemMaster> lstItems = new List<Inv_ItemMaster>();
            lstItems = inventoryDal.ReadInvItems(itemCategory);

            Inv_ItemCategorySubCategory objItemCategorySubCategory = new Inv_ItemCategorySubCategory();

            objItemCategorySubCategory = inventoryDal.ReadItemCategorySubCategoryByCategoryID(itemCategory);

            if (objItemCategorySubCategory != null && objItemCategorySubCategory.IsItASerializableItem == true)
            {
                SerializableItemOrNot = "1";
            }

            ArrayList arr = new ArrayList();

            foreach (Inv_ItemMaster csc in lstItems)
            {
                arr.Add(new { Value = csc.ItemCode + "-" + csc.ItemCapacity + "-" + csc.ItemModel, Display = csc.ItemName, IsItASerializableItem = SerializableItemOrNot });
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

        public JsonResult SerialMappingSave(string itemSerials, string itemCode, string itemCategory, string storeLocation)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            string itemDetails = (string)Session["ItemDetails"]; // StoreLocation, ItemCategory, ItemType
            string[] itemArr = itemDetails.Split('#');

            string[] itemWithSerial = itemSerials.Split('#');
            int itemSerialLength = itemWithSerial.Length;
            string insertIntoTempTableRows = string.Empty;

            //List<Inv_ItemStockWithSerialNoByLocation> lstItemSeralsScan = new List<Inv_ItemStockWithSerialNoByLocation>();
            //List<Inv_ItemSerialNoMaster> lstItemSerialMaster = new List<Inv_ItemSerialNoMaster>();

            //Inv_ItemStockWithSerialNoByLocation objItemStok;
            //Inv_ItemSerialNoMaster objItemSerialMaster;

            try
            {
                for (int i = 0; i < itemSerialLength; i++)
                {
                    //objItemStok = new Inv_ItemStockWithSerialNoByLocation();
                    //objItemSerialMaster = new Inv_ItemSerialNoMaster();

                    string[] specificItemSerial = itemWithSerial[i].Split('_');

                    if (i != 0)
                        insertIntoTempTableRows += "," + "(" + Convert.ToByte(itemArr[3].Trim()) + ",'" + objLoginHelper.LocationCode + "','" + itemCode.Trim() + "','" + specificItemSerial[0].Trim() + "'," + Helper.StockLocationSelection(Convert.ToByte(itemArr[3])) + ",'" + itemCategory.Trim() + "')";
                    else
                        insertIntoTempTableRows = "(" + Convert.ToByte(itemArr[3].Trim()) + ",'" + objLoginHelper.LocationCode + "','" + itemCode.Trim() + "','" + specificItemSerial[0].Trim() + "'," + Helper.StockLocationSelection(Convert.ToByte(itemArr[3])) + ",'" + itemCategory.Trim() + "')";

                    //objItemStok.StoreLocation = Convert.ToByte(itemArr[3]);
                    //objItemStok.LocationCode = objLoginHelper.LocationCode;
                    //objItemStok.ItemCode = itemCode;
                    //objItemStok.ItemSerialNo = specificItemSerial[0];   //Serial Number
                    //objItemStok.ItemConditionOrStatus = Helper.StockLocationSelection(Convert.ToByte(itemArr[3]));
                    //objItemStok.IsAvailableInStore = true;
                    //lstItemSeralsScan.Add(objItemStok);

                    //objItemSerialMaster.ItemCategory = itemCategory;
                    //objItemSerialMaster.ItemCode = itemCode;
                    //objItemSerialMaster.ItemSerialNo = specificItemSerial[0];     //Serial Number
                    //objItemSerialMaster.ItemConditionOrStatus = objItemStok.ItemConditionOrStatus;
                    //objItemSerialMaster.Status = Helper.InsideRSF;
                    //lstItemSerialMaster.Add(objItemSerialMaster);
                }

                //Inv_ItemStockByLocation objItemStockByLocation = new Inv_ItemStockByLocation();
                //objItemStockByLocation = inventoryDal.ReadItemStockByLocation(Convert.ToByte(storeLocation), objLoginHelper.LocationCode, itemCode);

                //if (objItemStockByLocation != null)
                //{
                inventoryDal.SaveItemStockWithSerialNoByLocation(insertIntoTempTableRows, objLoginHelper.LocationCode);
                //}
                //else
                //{
                //    Inv_ItemStockByLocation objItemStockByLocationNew = new Inv_ItemStockByLocation();
                //    objItemStockByLocationNew.StoreLocation = Convert.ToByte(storeLocation);
                //    objItemStockByLocationNew.LocationCode = objLoginHelper.LocationCode;
                //    objItemStockByLocationNew.ItemCode = itemCode;
                //    objItemStockByLocationNew.AvailableQuantity = 0;
                //    objItemStockByLocationNew.SerialQuantity = 0;
                //    objItemStockByLocationNew.LastUpdateOn = DateTime.Now;

                //    inventoryDal.SaveItemStockWithSerialNoByLocation(insertIntoTempTableRows, objLoginHelper.LocationCode);
                //}

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty) };

            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public JsonResult AuditObservationSave(string itemSerials, string itemCode, string itemCategory, string storeLocation, string auditObservationQty, string unCheckItemSerials)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            //Sal_CustomerTrainingTransMaster objSalCustomerTrainingMaster = new Sal_CustomerTrainingTransMaster();
            Inv_ItemStockByLocation objItemStockByLocation;
            List<Inv_ItemStockByLocation> lstItemStockByLocation = new List<Inv_ItemStockByLocation>();
            Inv_ItemStockWithSerialNoByLocation objItemStockWithSerialNoByLocation;
            List<Inv_ItemStockWithSerialNoByLocation> lstItemStockWithSerialNoByLocation = new List<Inv_ItemStockWithSerialNoByLocation>();
            int itemSerialsTotal = 0, serialCount = 0;
            int unCheckItemSerialsTotal = 0, unCheckItemSerialCount = 0;

            string[] selectedItemSerials = itemSerials.Split('#');
            string[] UnCheckselectedItemSerials = unCheckItemSerials.Split('#');
            string[] selectedItemCode = itemCode.Split('#');
            string[] selectedAuditObservationQty = auditObservationQty.Split('#');
            int itemCodeTotal = selectedItemCode.Length, itemCount = 0;
            if (!String.IsNullOrEmpty(selectedItemSerials[0]))
            {
                itemSerialsTotal = selectedItemSerials.Length;
                serialCount = 0;
            }
            if (!String.IsNullOrEmpty(UnCheckselectedItemSerials[0]))
            {
                unCheckItemSerialsTotal = UnCheckselectedItemSerials.Length;
                unCheckItemSerialCount = 0;
            }

            try
            {

                for (itemCount = 0; itemCount < itemCodeTotal; itemCount++)
                {
                    objItemStockByLocation = new Inv_ItemStockByLocation();
                    objItemStockByLocation.StoreLocation = Convert.ToByte(storeLocation.Trim());
                    objItemStockByLocation.LocationCode = objLoginHelper.LocationCode;

                    objItemStockByLocation.ItemCode = selectedItemCode[itemCount].Trim();
                    objItemStockByLocation.QuantityUnderAuditObservation = Convert.ToDouble(selectedAuditObservationQty[itemCount].Trim());

                    lstItemStockByLocation.Add(objItemStockByLocation);
                }

                for (serialCount = 0; serialCount < itemSerialsTotal; serialCount++)
                {
                    objItemStockWithSerialNoByLocation = new Inv_ItemStockWithSerialNoByLocation();
                    objItemStockWithSerialNoByLocation.StoreLocation = Convert.ToByte(storeLocation.Trim());
                    objItemStockWithSerialNoByLocation.LocationCode = objLoginHelper.LocationCode;
                    objItemStockWithSerialNoByLocation.ItemSerialNo = selectedItemSerials[serialCount].Trim();
                    objItemStockWithSerialNoByLocation.ItemCode = selectedItemCode[0].Trim();
                    objItemStockWithSerialNoByLocation.IsItUnderAuditObservation = true;

                    lstItemStockWithSerialNoByLocation.Add(objItemStockWithSerialNoByLocation);
                }

                for (unCheckItemSerialCount = 0; unCheckItemSerialCount < unCheckItemSerialsTotal; unCheckItemSerialCount++)
                {
                    objItemStockWithSerialNoByLocation = new Inv_ItemStockWithSerialNoByLocation();
                    objItemStockWithSerialNoByLocation.StoreLocation = Convert.ToByte(storeLocation.Trim());
                    objItemStockWithSerialNoByLocation.LocationCode = objLoginHelper.LocationCode;
                    objItemStockWithSerialNoByLocation.ItemSerialNo = UnCheckselectedItemSerials[unCheckItemSerialCount].Trim();
                    objItemStockWithSerialNoByLocation.ItemCode = selectedItemCode[0].Trim();
                    objItemStockWithSerialNoByLocation.IsItUnderAuditObservation = false;

                    lstItemStockWithSerialNoByLocation.Add(objItemStockWithSerialNoByLocation);
                }



                inventoryDal.Update(lstItemStockByLocation, lstItemStockWithSerialNoByLocation);

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Audit Observation" + Helper.SuccessMessage) };
            }


            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }

        }

        public JsonResult SerialListOfItem(string storeLocation, string itemCode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<Inv_ItemStockWithSerialNoByLocation> lstItemStockWise = new List<Inv_ItemStockWithSerialNoByLocation>();
            lstItemStockWise = inventoryDal.ReadItemStockWithSerialNoByLocation(Convert.ToByte(storeLocation), objLoginHelper.LocationCode, itemCode);

            bool inventoryStockUpdateFinishClosed = inventoryDal.IsInventoryStockUpdateFinish(objLoginHelper.LocationCode);

            ArrayList arrItemList = new ArrayList();

            foreach (Inv_ItemStockWithSerialNoByLocation its in lstItemStockWise)
            {
                arrItemList.Add(new { SerialNumber = its.ItemSerialNo, IsInventoryStockUpdateFinish = inventoryStockUpdateFinishClosed, IsItUnderAuditObservation = its.IsItUnderAuditObservation });
            }

            return new JsonResult { Data = arrItemList };
        }

        public JsonResult DeleteItemSerial(string storeLocation, string itemCode, string itemSerial, string itemCategory)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            string message = string.Empty;

            Inv_ItemNItemCategoryWithSerialNoMaster objItemSerialMaster = new Inv_ItemNItemCategoryWithSerialNoMaster();
            Inv_ItemStockWithSerialNoByLocation objItemSerials = new Inv_ItemStockWithSerialNoByLocation();

            try
            {
                objItemSerials = inventoryDal.ReadItemStockWithSerialNoByLocation(Convert.ToByte(storeLocation), objLoginHelper.LocationCode, itemCode, itemSerial);
                objItemSerialMaster = inventoryDal.ReadItemSerialNoMasterByItemCategoryWise(itemCode, itemSerial, itemCategory);

                inventoryDal.Delete(objItemSerialMaster, objItemSerials);

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(Helper.DeleteMessage) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public JsonResult ChallanDetailsForMRR(string challanSeaquenceNumber, string fromLocation)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            Inv_ChallanMaster objChallanMasterTem = new Inv_ChallanMaster();
            Inv_ChallanMaster objChallanMaster = new Inv_ChallanMaster();

            LocationInfo objLocationInfo = new LocationInfo();

            ArrayList arr = new ArrayList();

            try
            {
                objChallanMaster = inventoryDal.ReadChallanMasterByChallanSequence(challanSeaquenceNumber.Trim(), fromLocation.Trim());

                objLocationInfo = inventoryDal.Location(objChallanMaster.LocationCode);//erpDal.Unit(objChallanMaster.LocationCode);

                arr.Add(new
                {
                    ChallanLocation = objLocationInfo.LocationName + " [" + objChallanMaster.LocationCode + "]",
                    ChallanLocationCode = objChallanMaster.LocationCode,
                    ChallanDate = objChallanMaster.ChallanDate.ToString("dd-MMM-yyyy"),
                    ChallanSequenceNumber = objChallanMaster.ChallanSeqNo,
                    RefChallanNumber = objChallanMaster.RefChallanNo,
                    FromStoreLocation = objChallanMaster.FromStoreLocation,
                    ValidItemTransTypeForMRR = objChallanMaster.ValidItemTransTypeForMRR,
                    IsItForMultipleLocations = objChallanMasterTem.IsItForMultipleLocations == true ? 1 : 0,
                    FromLocationCode = objChallanMaster.LocationCode
                });

                return new JsonResult { Data = arr };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        [GridAction]
        public ActionResult __MRRDetailsList(string challanSeaquenceNumber, string isItForMultipleLocations, string fromLocationCode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (Convert.ToInt32(isItForMultipleLocations) == 1)
            {
                return View(new GridModel<MRRDetails> { Data = inventoryDal.ReadMRRDetails(fromLocationCode, objLoginHelper.LocationCode, challanSeaquenceNumber) });
            }
            else
            {
                return View(new GridModel<MRRDetails> { Data = inventoryDal.ReadMRRDetails(fromLocationCode, objLoginHelper.LocationCode, challanSeaquenceNumber) });
            }
        }

        public JsonResult ItemWiseItemDetails(string itemCode)
        {
            ArrayList arr = new ArrayList();

            Inv_ItemMaster objItemMaster = new Inv_ItemMaster();
            objItemMaster = inventoryDal.ReadItemMaster(itemCode.Trim());

            string itemName = objItemMaster.ItemName;
            string itemCapacityName = objItemMaster.Sal_PackageOrItemCapacity != null ? objItemMaster.Sal_PackageOrItemCapacity.Description : "";
            string itemModelName = objItemMaster.Inv_ItemModel != null ? objItemMaster.Inv_ItemModel.Description : "";

            arr.Add(new { ItemName = itemName, ItemCapacityName = itemCapacityName, ItemModelName = itemModelName });

            return new JsonResult { Data = arr };
        }

        public JsonResult LoadChallanItemSerials(string challanSequenceNumber, string itemCode, string challanLocationCode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            ArrayList arr = new ArrayList();
            string IsAllDeliveredSerialSelectionForMRREnabled = "";

            Inv_ItemMaster objItemMaster = inventoryDal.ReadItemMaster(itemCode);
            Inv_ItemCategorySubCategory objss = inventoryDal.ReadItemCategorySubCategoryByCategoryID(objItemMaster.ItemCategory);

            if (objss.IsAllDeliveredSerialSelectionForMRREnabled)
                IsAllDeliveredSerialSelectionForMRREnabled = "1";

            List<SalGetAvailableSerialNoForMRR> lsthallanDetailsWithSerialNo = new List<SalGetAvailableSerialNoForMRR>();
            lsthallanDetailsWithSerialNo = inventoryDal.ReadChallanDetailsWithSerialNo(challanSequenceNumber.Trim(), itemCode.Trim(), "MRRWITHCHALLAN", challanLocationCode);

            foreach (SalGetAvailableSerialNoForMRR objChallanSerials in lsthallanDetailsWithSerialNo)
            {
                arr.Add(new { Display = objChallanSerials.ItemSerialNo, Value = objChallanSerials.ItemSerialNo, IsAllDeliveredSerialSelectionForMRREnabled = IsAllDeliveredSerialSelectionForMRREnabled });
            }

            return new JsonResult { Data = arr };
        }

        public JsonResult LoadChallanInbox(string itemTypeId)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            ArrayList arr = new ArrayList();

            List<ChallanInboxForChallanWithMRR> lstChallanInbox = new List<ChallanInboxForChallanWithMRR>();
            lstChallanInbox = inventoryDal.ReadChallanInbox(objLoginHelper.LocationCode, itemTypeId);

            foreach (ChallanInboxForChallanWithMRR objChallanInbox in lstChallanInbox)
            {
                arr.Add(new { Display = objChallanInbox.ReceivedChallan, Value = (objChallanInbox.ChallanSeqNo + "-" + objChallanInbox.LocationCode) });
            }

            return new JsonResult { Data = arr };
        }

        public JsonResult LoadAvailableSerialForItem(string itemCode, string storeLocation)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            ArrayList arr = new ArrayList();

            List<Inv_ItemStockWithSerialNoByLocation> lstItemStockWithSerialNoByLocation = new List<Inv_ItemStockWithSerialNoByLocation>();
            lstItemStockWithSerialNoByLocation = inventoryDal.ReadItemStockWithSerialNoByLocation(Convert.ToByte(storeLocation.Trim()), objLoginHelper.LocationCode, itemCode.Trim());

            foreach (Inv_ItemStockWithSerialNoByLocation objStokItemSerial in lstItemStockWithSerialNoByLocation)
            {
                arr.Add(new { Display = objStokItemSerial.ItemSerialNo, Value = objStokItemSerial.ItemSerialNo });
            }

            return new JsonResult { Data = arr };
        }

        public ActionResult MultipleLocationSelectionPartial()
        {
            ViewBag.LocationType = new UserLocationType().UserLocationTypeFormat();
            return PartialView("UserLocationMultipleSelection");
        }

        public ActionResult LocationSelectionPartial(string issueTypeId)
        {
            ViewBag.LocationType = new UserLocationType().UserLocationTypeFormatChallan(issueTypeId);
            return PartialView("UserLocationSelection");
        }

        public ActionResult InventoryTransaction()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForInventory, "InventoryTransaction", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            string uid = string.Empty, uName = string.Empty, location = string.Empty, userGrouptype = string.Empty, uType = string.Empty;

            uid = objLoginHelper.LogInForUnitName;
            uName = objLoginHelper.LogInUserName;


            if (objLoginHelper.UerRoleOrGroupID == "UNITOFUSER")
            {
                uType = "unit";
                location = objLoginHelper.LocationCode;
                return Redirect("http://172.25.40.50/MonthlyEntryInventory/default.aspx?lc=" + location.Trim() + "&ud=" + uName.Trim() + "&securitycode=" + 0.5560977188159826 + "&uType=" + uType);
            }
            else
            {
                uType = "Zone";
                location = objLoginHelper.LocationCode;
                return Redirect("http://172.25.40.50/MonthlyEntryInventoryZOHO/default.aspx?zc=" + location.Trim() + "&ud=" + uName.Trim() + "&securitycode=" + 0.5560977188159826 + "&uType=" + uType);
            }
        }

        public JsonResult StoreLocationListByItemType(string itemTypeId)
        {
            List<Inv_StoreLocation> lstStoreLocation = new List<Inv_StoreLocation>();
            lstStoreLocation = inventoryDal.ReadStoreLocation(itemTypeId);

            ArrayList arr = new ArrayList();

            foreach (Inv_StoreLocation sl in lstStoreLocation)
            {
                arr.Add(new { Value = sl.StoreLocationID, Display = sl.Description });
            }

            return new JsonResult { Data = arr };
        }

        public ActionResult ChallanReceive()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForInventory, "ChallanReceive", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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
            ViewBag.ChallanOrMRR = Session["ChallanOrMRR"].ToString();

            List<InvChallanOrMRRDetailsForItemLedger> lstChallanDetails = new List<InvChallanOrMRRDetailsForItemLedger>();
            if (Session["ChallanOrMRRSeqNo"] != null)
            {
                string locationCode = objLoginHelper.LocationCode;
                if (Session["ChallanOrMrrLocationCode"] != null)
                {
                    locationCode = Session["ChallanOrMrrLocationCode"].ToString();
                }

                lstChallanDetails = inventoryDal.ReadInvChallanDetails(locationCode, Session["ChallanOrMRRSeqNo"].ToString(), Session["ChallanOrMRR"].ToString());
                TempData["ChallanDetails"] = lstChallanDetails;
            }

            return View();

        }

        public ActionResult ChallanDetails(string ChallanSeqNo, string Challan, string locationCode)
        {
            Session["ChallanOrMRRSeqNo"] = ChallanSeqNo;
            Session["ChallanOrMRR"] = Challan;

            if (!string.IsNullOrEmpty(locationCode))
                Session["ChallanOrMrrLocationCode"] = locationCode;
            else
                Session.Remove("ChallanOrMrrLocationCode");

            return RedirectToAction("ChallanReceive", "InventoryDailyTransaction");
        }

        [GridAction]
        public ActionResult __LoadItemLedger(string ItemCategory, string item, string storeLocation, string fromDate, string toDate)
        {
            try
            {
                objLoginHelper = (LoginHelper)Session["LogInInformation"];
                string itemCode = item.Substring(0, 4);

                List<ItemLedgerReport> lstItemLedger = new List<ItemLedgerReport>();
                lstItemLedger = inventoryDal.ReadInvItemLedger(Convert.ToByte(storeLocation), objLoginHelper.LocationCode, itemCode, fromDate, toDate);

                Session["ItemInLocation"] = lstItemLedger;

                return View(new GridModel<ItemLedgerReport>
                {
                    Data = lstItemLedger
                });
            }
            catch (Exception e)
            {
                //Logger.Error(scenario + e.Message, e);
                var result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.DenyGet,
                    Data = new
                    {
                        ErrorCode = 1,
                        ErrorMessage = e.GetType() + ": " + e.Message
                    }
                };

                return result;
            }


        }

        public JsonResult TransactineWiseVendorList(string transactionType)
        {
            ArrayList vendorList = new ArrayList();

            if (inventoryDal.IsItemTransationRelatedToVendor(transactionType))
            {
                vendorList = inventoryDal.GetVendorList(transactionType);
            }

            return new JsonResult { Data = vendorList };
        }

    }
}
