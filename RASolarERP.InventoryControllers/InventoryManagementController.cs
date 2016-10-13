using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using RASolarHelper;
using RASolarERP.Model;

using RASolarERP.Web.Areas.Inventory.Models;
using RASolarERP.Web.Models;
using System.Collections;
using Telerik.Web.Mvc;
using RASolarERP.DomainModel.InventoryModel;
using RASolarERP.Web.Areas.HRMS.Models;
using RASolarERP.DomainModel.HRMSModel;
using RASolarERP.DomainModel.SalesModel;
using RASolarERP.Web.Areas.Sales.Models;

namespace RASolarERP.Web.Areas.Inventory.Controllers
{
    public class InventoryManagementController : Controller
    {
        LoginHelper objLoginHelper = new LoginHelper();
        InventoryData inventoryDal = new InventoryData();
        SalesData salesDal = new SalesData();
        RASolarSecurityData securityDal = new RASolarSecurityData();
        private HRMSData hrmsDal = new HRMSData();
        RASolarERPData erpDal = new RASolarERPData();
        string message = string.Empty;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FixedAssetRegister()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForInventory, "FixedAssetRegister", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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

            ViewBag.FixedAssetCategory = inventoryDal.ReadItemCategorySubCatagory(Helper.FixedAssets);
            ViewBag.StoreLocation = inventoryDal.ReadStoreLocation(Helper.FixedAssets);

            List<EmployeeDetailsInfo> lstLocationWiseEmployee = new List<EmployeeDetailsInfo>();
            lstLocationWiseEmployee = hrmsDal.ReadLocationWiseEmployee(objLoginHelper.LogInForUnitCode);
            ArrayList arrEmployee = new ArrayList();

            arrEmployee.Add(new
            {
                EmployeeId = "ALL",
                EmployeeName = "All Employee"
            });
            //arrEmployee.Add(new
            //{
            //    EmployeeId = "UN-ASSIGNED",
            //    EmployeeName = "Un-Assigned"
            //});

            foreach (EmployeeDetailsInfo lwe in lstLocationWiseEmployee)
            {
                arrEmployee.Add(new
                {
                    EmployeeId = lwe.EmployeeID,
                    EmployeeName = lwe.EmployeeName
                });
            }

            ViewBag.Employee = arrEmployee;

            return View();
        }

        public JsonResult SaveAssetAssign(string employeeId, string itemCode, string storeLocation, string allocatedQuantity, string remarks, string serialItems)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            Fix_EmployeeWiseFixedAssetsAllocation objEmployeeWiseFixedAssetsAllocation = new Fix_EmployeeWiseFixedAssetsAllocation();
            Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo objEmployeeWiseFixedAssetsAllocationWithSerialNo;
            List<Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo> lstAssetsAllocationWithSerialNo = new List<Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo>();

            try
            {
                string sequenceNumber = Helper.ChallanCequenceNumberGeneration(inventoryDal.EmployeeWiseFixedAssetsAllocationSequenceNumberMax(objLoginHelper.LocationCode, employeeId, itemCode, Convert.ToByte(storeLocation)), objLoginHelper);

                objEmployeeWiseFixedAssetsAllocation.StoreLocation = Convert.ToByte(storeLocation.Trim());
                objEmployeeWiseFixedAssetsAllocation.LocationCode = objLoginHelper.LocationCode.Trim();
                objEmployeeWiseFixedAssetsAllocation.ItemCode = itemCode.Trim();
                objEmployeeWiseFixedAssetsAllocation.EmployeeID = employeeId.Trim();
                objEmployeeWiseFixedAssetsAllocation.AllocationSeqNo = sequenceNumber;
                objEmployeeWiseFixedAssetsAllocation.AllocationDate = DateTime.Now;
                objEmployeeWiseFixedAssetsAllocation.AllocatedQuantity = Convert.ToInt16(allocatedQuantity);
                objEmployeeWiseFixedAssetsAllocation.AllocatedBy = objLoginHelper.LogInID;
                objEmployeeWiseFixedAssetsAllocation.Remarks = remarks;
                objEmployeeWiseFixedAssetsAllocation.CreatedBy = objLoginHelper.LogInID;
                objEmployeeWiseFixedAssetsAllocation.CreatedOn = DateTime.Now;
                objEmployeeWiseFixedAssetsAllocation.Status = Helper.Active;

                if (!string.IsNullOrEmpty(serialItems))
                {
                    string[] serialItemAssign = serialItems.Split('#');
                    int serialCounter = 0, serialItemLength = serialItemAssign.Length;

                    for (serialCounter = 0; serialCounter < serialItemLength; serialCounter++)
                    {
                        objEmployeeWiseFixedAssetsAllocationWithSerialNo = new Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo();
                        objEmployeeWiseFixedAssetsAllocationWithSerialNo.StoreLocation = Convert.ToByte(storeLocation.Trim());
                        objEmployeeWiseFixedAssetsAllocationWithSerialNo.LocationCode = objLoginHelper.LocationCode;
                        objEmployeeWiseFixedAssetsAllocationWithSerialNo.ItemCode = itemCode;
                        objEmployeeWiseFixedAssetsAllocationWithSerialNo.EmployeeID = employeeId;
                        objEmployeeWiseFixedAssetsAllocationWithSerialNo.AllocationSeqNo = sequenceNumber;
                        objEmployeeWiseFixedAssetsAllocationWithSerialNo.ItemSerialNo = serialItemAssign[0];

                        if (objEmployeeWiseFixedAssetsAllocation.AllocatedQuantity > 0)
                            objEmployeeWiseFixedAssetsAllocationWithSerialNo.IsItAllocated = true;
                        else
                            objEmployeeWiseFixedAssetsAllocationWithSerialNo.IsItAllocated = false;

                        objEmployeeWiseFixedAssetsAllocationWithSerialNo.Status = Helper.Active;

                        lstAssetsAllocationWithSerialNo.Add(objEmployeeWiseFixedAssetsAllocationWithSerialNo);
                    }
                }

                inventoryDal.SaveFisedAssetFroEmployee(objEmployeeWiseFixedAssetsAllocation, lstAssetsAllocationWithSerialNo);

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty) };

            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        [GridAction]
        public ActionResult __AvailableItemInALocation(string categoryId, string storeLocation, string employeeId)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            string employeeSelectionOption = "EMPLOYEE-ID";

            if (!employeeId.Contains("RSF-"))
            {
                employeeSelectionOption = "ALL-EMPLOYEE";
                employeeId = string.Empty;
            }
            else
            {
                employeeSelectionOption = "INDIVIDUALEMPLOYEE";
            }

            return View(new GridModel<AvailableNAssignFixedAsset>
            {
                Data = inventoryDal.ReadAvailableNAssignFixedAsset(Convert.ToByte(storeLocation), objLoginHelper.LocationCode, categoryId, employeeSelectionOption, employeeId)
            });
        }

        [GridAction]
        public ActionResult __FixedAssetSerialList(string storeLocation, string itemCode, string employeeId, string option)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            string employeeSelectionOption = "EMPLOYEE-ID";

            if (!employeeId.Contains("RSF-"))
            {
                employeeSelectionOption = employeeId;
            }

            List<FixedAssetSerialList> lstFixedAssetAssignedSerial = new List<FixedAssetSerialList>();
            lstFixedAssetAssignedSerial = inventoryDal.GetFixedAssetSerialList(Convert.ToByte(storeLocation), objLoginHelper.LocationCode, itemCode, option);

            //if (employeeSelectionOption == "UN-ASSIGNED")
            //{
            //    lstFixedAssetAssignedUnassignedSerial = (from srllist in lstFixedAssetAssignedSerial where srllist.EmployeeID == null select srllist).ToList();
            //}
            //else if (employeeSelectionOption == "EMPLOYEE-ID")
            //{
            //    lstFixedAssetAssignedUnassignedSerial = (from srllist in lstFixedAssetAssignedSerial where srllist.EmployeeID == employeeId select srllist).ToList();
            //}
            //else
            //{
            //    lstFixedAssetAssignedUnassignedSerial = lstFixedAssetAssignedSerial;
            //}

            return View(new GridModel<FixedAssetSerialList>
            {
                Data = lstFixedAssetAssignedSerial
            });
        }

        [GridAction]
        public ActionResult __FixedAssetAssignToEmployee(string storeLocation, string itemCode, string employeeId, string unAssignedQuantity)
        {
            if (!employeeId.Contains("RSF-"))
            {
                employeeId = string.Empty;
            }

            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            return View(new GridModel<AssetAssign>
            {
                Data = inventoryDal.FixedAssetAssignUnassign(objLoginHelper.LocationCode, itemCode, Convert.ToByte(storeLocation), employeeId, Helper.Active, unAssignedQuantity)
            });
        }

        public JsonResult MapSerialForAssignAssetList(string storeLocation, string itemCode, string employeeId, string assignQuantity)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            return new JsonResult { Data = inventoryDal.GetFixedAssetAssignedOrUnassignedSeialList(Convert.ToByte(storeLocation), objLoginHelper.LocationCode, itemCode, employeeId, Convert.ToInt32(assignQuantity)) };
        }

        public ActionResult CashMemoBookAllocation()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForInventory, "CashMemoBookAllocation", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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

            ViewBag.FixedAssetCategory = inventoryDal.ReadItemCategorySubCatagory(Helper.FixedAssets);
            ViewBag.StoreLocation = inventoryDal.ReadStoreLocation(Helper.FixedAssets);

            List<EmployeeDetailsInfo> lstLocationWiseEmployee = new List<EmployeeDetailsInfo>();
            lstLocationWiseEmployee = hrmsDal.ReadLocationWiseEmployee(objLoginHelper.LocationCode);
            ArrayList arrEmployee = new ArrayList();

            foreach (EmployeeDetailsInfo lwe in lstLocationWiseEmployee)
            {
                arrEmployee.Add(new
                {
                    EmployeeId = lwe.EmployeeID,
                    EmployeeName = lwe.EmployeeName
                });
            }

            arrEmployee.Add(new
            {
                EmployeeId = "ALL-EMPLOYEE",
                EmployeeName = "All Employee"
            });

            ViewBag.Employee = arrEmployee;

            return View();
        }

        [GridAction]
        public ActionResult __AvailableItemInALocationForCashMemo(string categoryId, string storeLocation, string employeeId)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            string employeeSelectionOption = "EMPLOYEE-ID";

            if (!employeeId.Contains("RSF-"))
            {
                employeeSelectionOption = "ALL-EMPLOYEE";
                employeeId = string.Empty;
            }
            else
            {
                employeeSelectionOption = "INDIVIDUALEMPLOYEE";
            }

            return View(new GridModel<AvailableNAssignFixedAsset>
            {
                Data = inventoryDal.ReadAvailableNAssignCashMemo(Convert.ToByte(storeLocation), objLoginHelper.LocationCode, categoryId, employeeSelectionOption, employeeId)
            });
        }

        [GridAction]
        public ActionResult __CashMemoAllocationToEmployee(string storeLocation, string itemCode, string employeeId, string unAssignedQuantity)
        {
            if (!employeeId.Contains("RSF-"))
            {
                employeeId = string.Empty;
            }

            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<AssignCashMemoBook> lstAssignCashMemo = new List<AssignCashMemoBook>();
            lstAssignCashMemo = inventoryDal.CashMemoAssignUnassign(objLoginHelper.LocationCode, itemCode, Convert.ToByte(storeLocation), employeeId, Helper.Active, unAssignedQuantity);

            return View(new GridModel<AssignCashMemoBook>
            {
                Data = lstAssignCashMemo
            });
        }

        public JsonResult MapSerialListForCashBook(string storeLocation, string itemCode, string employeeId, string assignQuantity)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            return new JsonResult { Data = inventoryDal.GetCashMemoAssignedOrUnassignedSeialList(Convert.ToByte(storeLocation), objLoginHelper.LocationCode, itemCode, employeeId, Convert.ToInt32(assignQuantity)) };
        }

        public JsonResult SaveForCashMemoAllocation(string employeeId, string itemCode, string storeLocation, string allocatedQuantity, string serialItems)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            Fix_EmployeeWiseFixedAssetsAllocation objEmployeeWiseFixedAssetsAllocation = new Fix_EmployeeWiseFixedAssetsAllocation();
            Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo objEmployeeWiseFixedAssetsAllocationWithSerialNo;
            List<Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo> lstAssetsAllocationWithSerialNo = new List<Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo>();

            try
            {
                //if (!erpDal.IsMemoStillAvailableToUse(employeeId))
                //{
                //    return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Memo Still Available To Use. New Memo Cannot Be Assigned.") };
                //}

                string sequenceNumber = Helper.ChallanCequenceNumberGeneration(inventoryDal.EmployeeWiseFixedAssetsAllocationSequenceNumberMax(objLoginHelper.LocationCode, employeeId, itemCode, Convert.ToByte(storeLocation)), objLoginHelper);

                objEmployeeWiseFixedAssetsAllocation.StoreLocation = Convert.ToByte(storeLocation.Trim());
                objEmployeeWiseFixedAssetsAllocation.LocationCode = objLoginHelper.LocationCode.Trim();
                objEmployeeWiseFixedAssetsAllocation.ItemCode = itemCode.Trim();
                objEmployeeWiseFixedAssetsAllocation.EmployeeID = employeeId.Trim();
                objEmployeeWiseFixedAssetsAllocation.AllocationSeqNo = sequenceNumber;
                objEmployeeWiseFixedAssetsAllocation.AllocationDate = DateTime.Now;
                objEmployeeWiseFixedAssetsAllocation.AllocatedQuantity = Convert.ToInt16(allocatedQuantity);
                //objEmployeeWiseFixedAssetsAllocation.AllocatedBy = objLoginHelper.LogInID;
                objEmployeeWiseFixedAssetsAllocation.CreatedBy = objLoginHelper.LogInID;
                objEmployeeWiseFixedAssetsAllocation.CreatedOn = DateTime.Now;
                objEmployeeWiseFixedAssetsAllocation.Status = Helper.Active;

                if (!string.IsNullOrEmpty(serialItems))
                {
                    string[] serialItemAssign = serialItems.Split('#');
                    int serialCounter = 0, serialItemLength = serialItemAssign.Length;

                    for (serialCounter = 0; serialCounter < serialItemLength; serialCounter++)
                    {
                        objEmployeeWiseFixedAssetsAllocationWithSerialNo = new Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo();
                        objEmployeeWiseFixedAssetsAllocationWithSerialNo.StoreLocation = Convert.ToByte(storeLocation.Trim());
                        objEmployeeWiseFixedAssetsAllocationWithSerialNo.LocationCode = objLoginHelper.LocationCode;
                        objEmployeeWiseFixedAssetsAllocationWithSerialNo.ItemCode = itemCode;
                        objEmployeeWiseFixedAssetsAllocationWithSerialNo.EmployeeID = employeeId;
                        objEmployeeWiseFixedAssetsAllocationWithSerialNo.AllocationSeqNo = sequenceNumber;
                        objEmployeeWiseFixedAssetsAllocationWithSerialNo.ItemSerialNo = serialItemAssign[serialCounter];

                        if (objEmployeeWiseFixedAssetsAllocation.AllocatedQuantity > 0)
                            objEmployeeWiseFixedAssetsAllocationWithSerialNo.IsItAllocated = true;
                        else
                            objEmployeeWiseFixedAssetsAllocationWithSerialNo.IsItAllocated = false;

                        objEmployeeWiseFixedAssetsAllocationWithSerialNo.Status = Helper.Active;

                        lstAssetsAllocationWithSerialNo.Add(objEmployeeWiseFixedAssetsAllocationWithSerialNo);
                    }
                }


                inventoryDal.SaveForCashMemoAllocation(objEmployeeWiseFixedAssetsAllocation, lstAssetsAllocationWithSerialNo);

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty) };

            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public JsonResult UnAssignAssetToEmployee(string employeeID, string itemSerialNo, string allocationSeqNo, string storeLocation, string itemCode, string totalSerialInStock)
        {
            try
            {
                objLoginHelper = (LoginHelper)Session["LogInInformation"];

                string sequenceNumber = Helper.ChallanCequenceNumberGeneration(inventoryDal.EmployeeWiseFixedAssetsAllocationSequenceNumberMax(objLoginHelper.LocationCode, employeeID, itemCode, Convert.ToByte(storeLocation)), objLoginHelper);

                Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo objEmployeeWiseFixedAssetsAllocationWithSerialNo = new Fix_EmployeeWiseFixedAssetsAllocationWithSerialNo();
                objEmployeeWiseFixedAssetsAllocationWithSerialNo.StoreLocation = Convert.ToByte(storeLocation);
                objEmployeeWiseFixedAssetsAllocationWithSerialNo.LocationCode = objLoginHelper.LocationCode;
                objEmployeeWiseFixedAssetsAllocationWithSerialNo.ItemCode = itemCode;
                objEmployeeWiseFixedAssetsAllocationWithSerialNo.EmployeeID = employeeID;
                objEmployeeWiseFixedAssetsAllocationWithSerialNo.AllocationSeqNo = sequenceNumber;
                objEmployeeWiseFixedAssetsAllocationWithSerialNo.ItemSerialNo = itemSerialNo;
                objEmployeeWiseFixedAssetsAllocationWithSerialNo.IsItAllocated = false;
                objEmployeeWiseFixedAssetsAllocationWithSerialNo.Status = Helper.Active;


                Fix_EmployeeWiseFixedAssetsAllocation objEmployeeWiseFixedAssetsAllocation = new Fix_EmployeeWiseFixedAssetsAllocation();



                objEmployeeWiseFixedAssetsAllocation.StoreLocation = Convert.ToByte(storeLocation.Trim());
                objEmployeeWiseFixedAssetsAllocation.LocationCode = objLoginHelper.LocationCode.Trim();
                objEmployeeWiseFixedAssetsAllocation.ItemCode = itemCode.Trim();
                objEmployeeWiseFixedAssetsAllocation.EmployeeID = employeeID.Trim();
                objEmployeeWiseFixedAssetsAllocation.AllocationSeqNo = sequenceNumber;
                objEmployeeWiseFixedAssetsAllocation.AllocationDate = DateTime.Now;
                objEmployeeWiseFixedAssetsAllocation.AllocatedQuantity = -1;
                //objEmployeeWiseFixedAssetsAllocation.AllocatedBy = objLoginHelper.LogInID;
                objEmployeeWiseFixedAssetsAllocation.CreatedBy = objLoginHelper.LogInID;
                objEmployeeWiseFixedAssetsAllocation.CreatedOn = DateTime.Now;
                objEmployeeWiseFixedAssetsAllocation.Status = Helper.Active;


                inventoryDal.UpdateCashmemoAssign(objEmployeeWiseFixedAssetsAllocationWithSerialNo, objEmployeeWiseFixedAssetsAllocation);

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        [GridAction]
        public ActionResult __CashMemoSerialList(string storeLocation, string itemCode, string employeeId, string option)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            string employeeSelectionOption = "EMPLOYEE-ID";

            if (!employeeId.Contains("RSF-"))
            {
                employeeSelectionOption = employeeId;
            }

            List<FixedAssetSerialList> lstFixedAssetAssignedSerial = new List<FixedAssetSerialList>();
            lstFixedAssetAssignedSerial = inventoryDal.GetCashMemoSerialList(Convert.ToByte(storeLocation), objLoginHelper.LocationCode, itemCode, option);

            //if (employeeSelectionOption == "UN-ASSIGNED")
            //{
            //    lstFixedAssetAssignedUnassignedSerial = (from srllist in lstFixedAssetAssignedSerial where srllist.EmployeeID == null select srllist).ToList();
            //}
            //else if (employeeSelectionOption == "EMPLOYEE-ID")
            //{
            //    lstFixedAssetAssignedUnassignedSerial = (from srllist in lstFixedAssetAssignedSerial where srllist.EmployeeID == employeeId select srllist).ToList();
            //}
            //else
            //{
            //    lstFixedAssetAssignedUnassignedSerial = lstFixedAssetAssignedSerial;
            //}

            return View(new GridModel<FixedAssetSerialList>
            {
                Data = lstFixedAssetAssignedSerial
            });
        }

        [GridAction]
        public ActionResult _CashMemoBooKPagesStatus(string itemSerialNo)
        {
            return View(new GridModel<CashMemoBookPagesStatus>
            {
                Data = inventoryDal.GetStatusForCashMemoBookAllocation(itemSerialNo)
            });
        }

        //---------------------------------- Warranty Claim & Settlement ----------------------------------------

        public ActionResult WarrantyClaimNSettlement()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForInventory, "WarrantyClaimNSettlement", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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
            ViewBag.OpenMonthYear = "Day Open Date: " + objLoginHelper.TransactionOpenDate.ToString("MMMM  dd, yyyy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;
            ViewBag.OpenMonth = objLoginHelper.TransactionOpenDate.ToString("dd-MMM-yyyy");

            List<WarrantyClaimReason> lstWarrantyClaimReason = new List<WarrantyClaimReason>();
            lstWarrantyClaimReason = inventoryDal.GetWarrantyClaimReason();

            lstWarrantyClaimReason.Add(new WarrantyClaimReason
            {
                ReasonCode = "0",
                ReasonDescription = "Others"
            });

            ViewBag.ReasonForClaim = lstWarrantyClaimReason;

            return View();
        }

        [GridAction]
        public ActionResult __AvailableWarrantyItems(string customerCode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<WarrantyItemsDetails> lstWarrantyItemsDetails = new List<WarrantyItemsDetails>();
            lstWarrantyItemsDetails = inventoryDal.GetWarrantyItemsList(customerCode, objLoginHelper.MonthOpenForInventory);

            return View(new GridModel<WarrantyItemsDetails> { Data = lstWarrantyItemsDetails });
        }

        public JsonResult GetCustomerDetails(string customerCode)
        {

            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            CustomerDetails objCustomerDetails = new CustomerDetails();
            objCustomerDetails = inventoryDal.GetCustomerDetailsForWarrantyClaim(customerCode, objLoginHelper.LogInForUnitCode);

            if (objCustomerDetails != null)
            {
                objCustomerDetails.AgreementDateStringFormat = objCustomerDetails.AgreementDate.ToString("dd-MMM-yyyy");
            }

            return new JsonResult { Data = objCustomerDetails };
        }

        public JsonResult GetMrrNChallanSquenceNo()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            ArrayList lstSequenceNumber = new ArrayList();

            string MRRSequenceMax = inventoryDal.MRRSequenceNumberMax(objLoginHelper.LocationCode, objLoginHelper.TransactionOpenDate.ToString("yyMMdd"));
            string MRRSequenceNo = Helper.ChallanCequenceNumberGeneration(MRRSequenceMax, objLoginHelper);



            string challanNumberMax = inventoryDal.ChallanSequenceNumberMax(objLoginHelper.LocationCode, objLoginHelper.TransactionOpenDate.ToString("yyMMdd"));
            string challnSequenceNo = Helper.ChallanCequenceNumberGeneration(challanNumberMax, objLoginHelper);


            lstSequenceNumber.Add(new
            {
                MRRSequenceNo = MRRSequenceNo,
                challnSequenceNo = challnSequenceNo,
            });

            return new JsonResult { Data = lstSequenceNumber };
        }

        public JsonResult SaveWarrantyClaimNSettlement(List<ReceiveItem> lstReceiveItem, List<IssueItem> lstIssueItem, string customerCode)
        {

            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            // Receiev item section 
            Inv_MRRMaster objMRRMaster;
            Inv_MRRDetails objMRRDetails;
            Inv_MRRDetailsWithSerialNo objMRRDetailsWithSerialNo;

            List<Inv_MRRDetails> lstMRRDetails = new List<Inv_MRRDetails>();
            List<Inv_MRRDetailsWithSerialNo> lstMRRDetailsWithSerialNo = new List<Inv_MRRDetailsWithSerialNo>();

            try
            {
                objMRRMaster = new Inv_MRRMaster();
                objMRRMaster.LocationCode = objLoginHelper.LocationCode;
                objMRRMaster.MRRSeqNo = lstReceiveItem[0].MrrSequenceNo;
                objMRRMaster.MRRDate = Convert.ToDateTime(Helper.DateFormatMMDDYYYY(objLoginHelper.TransactionOpenDate));
                objMRRMaster.ForStoreLocation = lstReceiveItem[0].StoreLocation;  //Store Location
                objMRRMaster.RefMRRNo = lstReceiveItem[0].MrrNumber;
                objMRRMaster.ChallanLocationCode = null;             // Challan From Location add later 
                objMRRMaster.ChallanSeqNo = null;                     //Challan Sequence Number
                objMRRMaster.RefCustomerCode = customerCode;
                objMRRMaster.ItemType = lstReceiveItem[0].ItemType;
                objMRRMaster.CreatedBy = objLoginHelper.LogInID;
                objMRRMaster.CreatedOn = DateTime.Now;
                objMRRMaster.Status = Helper.Active;
                objMRRMaster.Particulars = lstReceiveItem[0].Particulars;

                if (lstReceiveItem[0].TransactionType == "RCVCSIWCSI")
                {
                    objMRRMaster.ItemTransTypeID = "RCVCSIWCSI";
                }
                else if (lstReceiveItem[0].TransactionType == "ISSCSTCSAM")
                {
                    objMRRMaster.ItemTransTypeID = "RCVCSIFCUS";
                }
                else if (lstReceiveItem[0].TransactionType == "ISSTCUSAWR")
                {
                    if (lstReceiveItem[0].StoreLocation == 3)
                    {
                        objMRRMaster.ItemTransTypeID = "RCVCSIFCUS";
                    }
                    else if (lstReceiveItem[0].StoreLocation == 9)
                    {
                        objMRRMaster.ItemTransTypeID = "RCVCSIWCSI";
                    }
                }


                byte componentSequence = 0;

                foreach (ReceiveItem lwt in lstReceiveItem)
                {
                    componentSequence++;

                    objMRRDetails = new Inv_MRRDetails();
                    objMRRDetails.LocationCode = objLoginHelper.LocationCode;
                    objMRRDetails.MRRSeqNo = lwt.MrrSequenceNo;
                    objMRRDetails.CompSeqNo = Convert.ToByte(componentSequence);          // Component Sequence
                    objMRRDetails.ItemCode = lwt.ItemCode;                                //Item Code

                    objMRRDetails.ReceiveQuantity = lwt.ReceivedQuantity;
                    objMRRDetails.ForStoreLocation = lwt.StoreLocation;                     //Convert.ToByte(mrrMasterReceivedItems[2].Trim());  //Store Location;
                    objMRRDetails.RefChallanLocationCode = objLoginHelper.LocationCode;          //Challan Location Code
                    // objMRRDetails.RefChallanSeqNo = lstIssueItem[0].ChallanSequenceNo;          //Challan Seqence Number
                    //objMRRDetails.RefChallanCompSeqNo = Convert.ToByte(componentSequence);  // Ref Challan Seqence Number
                    objMRRDetails.UnitCost = 0;
                    lstMRRDetails.Add(objMRRDetails);


                    objMRRDetailsWithSerialNo = new Inv_MRRDetailsWithSerialNo();
                    objMRRDetailsWithSerialNo.LocationCode = objLoginHelper.LocationCode;
                    objMRRDetailsWithSerialNo.MRRSeqNo = lwt.MrrSequenceNo;
                    objMRRDetailsWithSerialNo.CompSeqNo = componentSequence;          // Component Sequence
                    objMRRDetailsWithSerialNo.ItemCode = lwt.ItemCode;   //Item Code
                    objMRRDetailsWithSerialNo.ItemSerialNo = lwt.ItemSerialNo;   //Item Serial Number
                    objMRRDetailsWithSerialNo.RefChallanLocationCode = null;          //Challan Location Code
                    //objMRRDetailsWithSerialNo.RefChallanSeqNo = mrrMasterReceivedItems[4];          //Challan Seqence Number
                    //objMRRDetailsWithSerialNo.RefChallanCompSeqNo = Convert.ToByte(mrrMasterReceivedItems[7]);  // Ref Challan Seqence Number
                    objMRRDetailsWithSerialNo.Status = Helper.Active;
                    lstMRRDetailsWithSerialNo.Add(objMRRDetailsWithSerialNo);
                }


                // Issue Item Save Part

                Inv_ChallanMaster objChallanMaster;
                Inv_ChallanDetails objChallanDetails;
                Inv_ChallanDetailsWithSerialNo objChallanDetailsWithSerialNo;

                List<Inv_ChallanDetails> lstChallanDetails = new List<Inv_ChallanDetails>();
                List<Inv_ChallanDetailsWithSerialNo> lstChallanDetailsWithSerialNo = new List<Inv_ChallanDetailsWithSerialNo>();

                objChallanMaster = new Inv_ChallanMaster();
                objChallanMaster.LocationCode = objLoginHelper.LocationCode;
                objChallanMaster.ChallanSeqNo = lstIssueItem[0].ChallanSequenceNo;
                objChallanMaster.ChallanDate = Convert.ToDateTime(Helper.DateFormatMMDDYYYY(objLoginHelper.TransactionOpenDate));
                objChallanMaster.FromStoreLocation = lstIssueItem[0].StoreLocation;        // Store Location Code
                objChallanMaster.RefCustomerCode = customerCode;
                objChallanMaster.ItemType = lstReceiveItem[0].ItemType;
                objChallanMaster.CreatedBy = objLoginHelper.LogInID;
                objChallanMaster.CreatedOn = DateTime.Now;
                objChallanMaster.Status = Helper.Active;
                objChallanMaster.Particulars = string.Empty;

                if (lstReceiveItem[0].TransactionType == "RCVCSIWCSI")
                {
                    objChallanMaster.ItemTransTypeID = "ISSCSITCUS";
                }
                else if (lstReceiveItem[0].TransactionType == "ISSCSTCSAM")
                {
                    objChallanMaster.ItemTransTypeID = "ISSCSTCSAM";
                }
                else if (lstReceiveItem[0].TransactionType == "ISSTCUSAWR")
                {
                    objChallanMaster.ItemTransTypeID = "ISSTCUSAWR";
                }


                byte ComponentSequenceForIssue = 0;

                foreach (IssueItem lstIssue in lstIssueItem)
                {
                    ComponentSequenceForIssue++;

                    objChallanDetails = new Inv_ChallanDetails();
                    objChallanDetails.LocationCode = objLoginHelper.LocationCode;
                    objChallanDetails.ChallanSeqNo = lstIssue.ChallanSequenceNo;
                    objChallanDetails.CompSeqNo = Convert.ToByte(ComponentSequenceForIssue);              // Component Sequence Number
                    objChallanDetails.ItemCode = lstIssue.ItemCode;                               // Item Code
                    objChallanDetails.DeliveryQuantity = lstIssue.ReceivedQuantity;     // Delivery Quantity
                    objChallanDetails.UnitCost = 0;
                    objChallanDetails.FromStoreLocation = lstIssue.StoreLocation;      // Store Location
                    lstChallanDetails.Add(objChallanDetails);

                    objChallanDetailsWithSerialNo = new Inv_ChallanDetailsWithSerialNo();
                    objChallanDetailsWithSerialNo.LocationCode = objLoginHelper.LocationCode;
                    objChallanDetailsWithSerialNo.ChallanSeqNo = lstIssue.ChallanSequenceNo;
                    objChallanDetailsWithSerialNo.CompSeqNo = ComponentSequenceForIssue;          // Component Sequence Number
                    objChallanDetailsWithSerialNo.ItemCode = lstIssue.ItemCode;                     // Item Code
                    objChallanDetailsWithSerialNo.ItemSerialNo = lstIssue.ItemSerialNO;                 // Item Serial
                    objChallanDetailsWithSerialNo.RefStoreLocation = lstIssue.StoreLocation;    // Store Location Code
                    objChallanDetailsWithSerialNo.Status = Helper.Active;

                    lstChallanDetailsWithSerialNo.Add(objChallanDetailsWithSerialNo);
                }

                objMRRMaster = inventoryDal.SaveWarrantyClaimNSettlement(objMRRMaster, lstMRRDetails, lstMRRDetailsWithSerialNo, objChallanMaster, lstChallanDetails, lstChallanDetailsWithSerialNo);

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(Helper.SuccessMessage) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public JsonResult CheckingIssueItemSerailNO(string itemSerial, string itemCode, string storeLocationForIssue)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            string checkItemSerialNo = inventoryDal.CheckingIssueItemSerailNO(objLoginHelper.LocationCode, itemSerial, itemCode, storeLocationForIssue);
            
            return new JsonResult { Data = checkItemSerialNo };
        }

        public JsonResult GetSubstituteItemCode(string itemCode)
        {
            List<ItemInfo> objItemInfo = new List<ItemInfo>();
            objItemInfo = inventoryDal.GetSubstituteItemCode(itemCode);

            return new JsonResult { Data = objItemInfo};
        }
    }
}
