using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using System.Collections;
using System.Text.RegularExpressions;
using System.Text;

using RASolarERP.Web.Models;
using RASolarHelper;
using RASolarERP.Model;
using RASolarSecurity.Model;
using RASolarHRMS.Model;

using RASolarERP.Web.Areas.HRMS.Models;
using RASolarERP.Web.Areas.Sales.Models;
using RASolarERP.DomainModel.HRMSModel;

namespace RASolarERP.Web.Areas.HRMS.Controllers
{
    public class EmployeeRegisterController : Controller
    {
        LoginHelper objLoginHelper = new LoginHelper();
        private SalesData salesDal = new SalesData();
        private RASolarERPData erpDal = new RASolarERPData();
        private HRMSData hrmsData = new HRMSData();
        RASolarSecurityData securityDal = new RASolarSecurityData();
        string message = string.Empty;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EmployeeTransfer()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForHRMS, "EmployeeTransfer", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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
            //start for if its region loger 
            if (objLoginHelper.Location == "Region" || objLoginHelper.Location == "HO" || objLoginHelper.Location == "Unit"||objLoginHelper.Location == "Zone")
                ViewBag.OpenDay = objLoginHelper.CurrentDate.Date.ToString("dd-MMM-yyyy");
            else
                ViewBag.OpenDay = objLoginHelper.TransactionOpenDate.Date.ToString("dd-MMM-yyyy");
            //end for if its region loger
            ViewBag.CalenderDate = objLoginHelper.MonthOpenForHRMS;

            //ViewBag.OpenDay = objLoginHelper.TransactionOpenDate.Date.ToString("dd-MMM-yyyy");
            ViewBag.OpenBackDay = objLoginHelper.TransactionBackDate.Date.ToString("dd-MMM-yyyy");

            ViewBag.ReasonOfLateRelease = string.Empty;
            ViewBag.ReasonOfLateJoining = string.Empty;

            List<Hrm_OperationalRole> lstOperationalRole = new List<Hrm_OperationalRole>();
            if (objLoginHelper.LocationCode == "9000")
            {
                lstOperationalRole = hrmsData.ReadOperationalRole().ToList();
            }
            else
            {
                lstOperationalRole = hrmsData.ReadOperationalRole().Where(i => i.OperationalRoleID == "ACTUNTMNGR" || i.OperationalRoleID == "UNITMANAGR").ToList();
            }

            ArrayList arr = new ArrayList(lstOperationalRole);
            ViewBag.OperationalRole = arr;

            List<Hrm_OperationalRole> lstOperationalRoleForAssignUM = new List<Hrm_OperationalRole>();
            lstOperationalRoleForAssignUM = hrmsData.ReadOperationalRole().Where(i => i.OperationalRoleID == "ACTUNTMNGR" || i.OperationalRoleID == "UNITMANAGR").ToList();

            ArrayList ar = new ArrayList(lstOperationalRoleForAssignUM);
            ViewBag.OperationalRoleForAssignUM = ar;

            if (objLoginHelper.Location == Helper.Zone)
                ViewBag.EditModeShowOrNot = true;
            else
                ViewBag.EditModeShowOrNot = false;

            return View();
        }

        [HttpPost]
        public JsonResult EmployeeTransfer(EmployeeTransferInfo objEmployeeTransferInfo)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            byte transferSequenceNumber = 0;

            try
            {
                if (objEmployeeTransferInfo.TransferSeqNo == null)
                    transferSequenceNumber = hrmsData.ReadTransferSequenceNumber(objEmployeeTransferInfo.EmployeeID);
                else
                    transferSequenceNumber = Convert.ToByte(objEmployeeTransferInfo.TransferSeqNo);

                Hrm_EmployeeTransfer objEmployeeTransfer = new Hrm_EmployeeTransfer();
                objEmployeeTransfer.EmployeeID = objEmployeeTransferInfo.EmployeeID;
                objEmployeeTransfer.TransferSeqNo = transferSequenceNumber;
                //start for if its region loger 
                if (objLoginHelper.Location == "Region" || objLoginHelper.Location == "Unit")
                    objEmployeeTransfer.TransferDate = Convert.ToDateTime(objEmployeeTransferInfo.TransferReleaseDate);
                else //for ho and zone
                    objEmployeeTransfer.TransferDate = Convert.ToDateTime(Helper.DateFormatMMDDYYYY(objLoginHelper.MonthOpenForHRMS));
                //end for if its region loger

                //objEmployeeTransfer.TransferDate = Convert.ToDateTime(Helper.DateFormatMMDDYYYY(objLoginHelper.TransactionOpenDate));
                
                objEmployeeTransfer.TransferFromLocation = objEmployeeTransferInfo.TransferFromLocation;
                objEmployeeTransfer.TransferToLocation = objEmployeeTransferInfo.TransferToLocation;
                objEmployeeTransfer.IsItLastTransfer = false;
                objEmployeeTransfer.NoteForTransfer = objEmployeeTransferInfo.NoteForTransfer;
                objEmployeeTransfer.IsTransferAccepted = false;
                objEmployeeTransfer.CreatedBy = objLoginHelper.LogInID;
                objEmployeeTransfer.CreatedOn = objLoginHelper.MonthOpenForHRMS;
                objEmployeeTransfer.Status = Helper.Active;

                Hrm_EmployeeTransferAdditionalInfo objEmployeeTransferAdditionalInfo = new Hrm_EmployeeTransferAdditionalInfo();
                objEmployeeTransferAdditionalInfo.EmployeeID = objEmployeeTransferInfo.EmployeeID;
                objEmployeeTransferAdditionalInfo.TransferSeqNo = transferSequenceNumber;
                objEmployeeTransferAdditionalInfo.RequisitionReceivedOn = objEmployeeTransferInfo.RequisitionReceivedOn;
                objEmployeeTransferAdditionalInfo.OfficeOrderDate = objEmployeeTransferInfo.OfficeOrderDate;
                objEmployeeTransferAdditionalInfo.OfficeOrderNo = objEmployeeTransferInfo.OfficeOrderNo + objEmployeeTransferInfo.OfficeOrderNoSequence;
                objEmployeeTransferAdditionalInfo.TransferReleaseDate = objEmployeeTransferInfo.TransferReleaseDate;
                objEmployeeTransferAdditionalInfo.ResponsibilityHandoverTo = objEmployeeTransferInfo.ResponsibilityHandoverTo;
                objEmployeeTransferAdditionalInfo.ResponsibilityTakenoverFrom = objEmployeeTransferInfo.ResponsibilityTakenoverFrom;
                objEmployeeTransferAdditionalInfo.ReasonForLateTransferJoining = objEmployeeTransferInfo.ReasonForLateTransferJoining;
                objEmployeeTransferAdditionalInfo.ReasonForLateTransferRelease = objEmployeeTransferInfo.ReasonForLateTransferRelease;
                objEmployeeTransferAdditionalInfo.ActualTransferJoiningDate = objEmployeeTransferInfo.ActualTransferJoiningDate;
                objEmployeeTransferAdditionalInfo.ActualTransferReleaseDate = objEmployeeTransferInfo.ActualTransferReleaseDate;
                objEmployeeTransferAdditionalInfo.NewOperationalRole = objEmployeeTransferInfo.NewOperationalRole;
               
                if (objEmployeeTransferInfo.TransferSeqNo == null)
                {
                    hrmsData.CreateEmployeeTransfer(objEmployeeTransfer, objEmployeeTransferAdditionalInfo);
                }
                else
                {
                    objEmployeeTransfer.ModifiedBy = objLoginHelper.LogInID;
                    objEmployeeTransfer.ModifiedOn = DateTime.Now;

                    hrmsData.UpdateEmployeeTransfer(objEmployeeTransfer, objEmployeeTransferAdditionalInfo);
                }

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Transfer Is Succeed") };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public ActionResult EmployeeRegister()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForHRMS, "EmployeeRegister", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.TransactionOpenDate.ToString("MMMM-yy");
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;
            ViewBag.CurrentDay = objLoginHelper.TransactionOpenDate.Date.ToString("dd-MMM-yyyy");
            ViewBag.CalenderDate = objLoginHelper.TransactionOpenDate;

            ViewBag.Zone = erpDal.Zone(); // Need To Change After Riview

            List<GetLocationWiseEmployeeForEmployeeRegister> lstEmployeeRegister = new List<GetLocationWiseEmployeeForEmployeeRegister>();
            lstEmployeeRegister = hrmsData.ReadGetLocationWiseEmployeeForEmployeeRegister(objLoginHelper.LogInForUnitCode);
            TempData["EmployeeRegister"] = lstEmployeeRegister;

            return View();
        }

        //public JsonResult TransferEmployee(string employeeID, string employeeTransferDate, string transferToLocation, string transferNote)
        //{
        //    objLoginHelper = (LoginHelper)Session["LogInInformation"];

        //    try
        //    {
        //        // hrmsData.EmployeeTransfer(objLoginHelper.LocationCode, employeeID, "TRANSFER", transferToLocation.Trim(), employeeTransferDate, transferNote, objLoginHelper.LogInID, "INSERT");
        //        return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Employee Transfer" + Helper.SuccessMessage) };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
        //    }
        //}

        public ActionResult EmployeeAcceptReject()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForHRMS, "EmployeeAcceptReject", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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

            List<EmployeeTransferInfo> lstEmployeeTransferredButNotYetAccepted = new List<EmployeeTransferInfo>();
            lstEmployeeTransferredButNotYetAccepted = hrmsData.ReadGetEmployeeTransferredButNotYetAccepted(objLoginHelper.LocationCode);
            TempData["EmployeeRegister"] = lstEmployeeTransferredButNotYetAccepted;

            return View();

        }

        public JsonResult EmployeeAccept(string employeeID, string AcceptOrRejectDate, string officeOrderNo, string joiningDate, string reasonOfLateJoining, string transferSeqNo, string transferFromLocation, string transferToLocation)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            try
            {

                Hrm_EmployeeTransfer objEmployeeTransfer = new Hrm_EmployeeTransfer();

                objEmployeeTransfer.TransferFromLocation = transferFromLocation;
                objEmployeeTransfer.EmployeeID = employeeID;
                objEmployeeTransfer.TransferDate = Convert.ToDateTime(Helper.DateFormatMMDDYYYY(AcceptOrRejectDate));
                objEmployeeTransfer.TransferToLocation = transferToLocation;
                objEmployeeTransfer.NoteForTransfer = string.Empty;
                objEmployeeTransfer.CreatedBy = objLoginHelper.LogInID;
                objEmployeeTransfer.TransferSeqNo = Convert.ToByte(transferSeqNo);

                hrmsData.EmployeeAcceptOrReject(objEmployeeTransfer, "ACCEPT", reasonOfLateJoining);
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Employee Accept" + Helper.SuccessMessage) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }

        }

        public JsonResult EmployeeReject(string employeeID, string AcceptOrRejectDate, string transferSeqNo, string transferFromLocation, string transferToLocation)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            try
            {
                Hrm_EmployeeTransfer objEmployeeTransfer = new Hrm_EmployeeTransfer();

                objEmployeeTransfer.TransferFromLocation = transferFromLocation;
                objEmployeeTransfer.EmployeeID = employeeID;
                objEmployeeTransfer.TransferDate = Convert.ToDateTime(Helper.DateFormatMMDDYYYY(AcceptOrRejectDate));
                objEmployeeTransfer.TransferToLocation = transferToLocation;
                objEmployeeTransfer.NoteForTransfer = string.Empty;
                objEmployeeTransfer.CreatedBy = objLoginHelper.LogInID;
                objEmployeeTransfer.TransferSeqNo = Convert.ToByte(transferSeqNo);

                hrmsData.EmployeeAcceptOrReject(objEmployeeTransfer, "CANCEL", transferSeqNo);
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Employee Not-Accepted") };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public ActionResult EmployeeLocationSelectionPartial()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (objLoginHelper.Location == Helper.Zone)
            {
                ViewBag.LocationType = new UserLocationType().UserLocationTypeFormat().Where(l => l.LocationTypeID == "zone" || l.LocationTypeID == "unit");
            }
            else
                ViewBag.LocationType = new UserLocationType().UserLocationTypeFormat().Where(l => l.LocationTypeID != "region");

            return PartialView("EmployeeLocationSelection");
        }

        public ActionResult EmployeeLocationSelectionTransferPartial()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (objLoginHelper.Location == Helper.Zone)
            {
                ViewBag.LocationType = new UserLocationType().UserLocationTypeFormat().Where(l => l.LocationTypeID == "zone" || l.LocationTypeID == "unit");
            }
            else
                ViewBag.LocationType = new UserLocationType().UserLocationTypeFormat().Where(l => l.LocationTypeID != "region");

            return PartialView("EmployeeTransferLocationSelection");
        }

        [GridAction]
        public ActionResult __EmployeeListLocationWise(string locationCode)
        {
            return View(new GridModel<EmployeeDetailsInfo>()
            {
                Data = hrmsData.ReadLocationWiseEmployeeWithTransferStatus(locationCode)
            });
        }

        public ActionResult __LoadEmployeeSearchWindow()
        {
            ViewBag.Designation = hrmsData.ReadEmployeeDesignation();
            return PartialView("EmployeeDetailSearch");
        }

        public ActionResult SaveAssignEmployeeAsUm(string employeeId, string operationalRole, string currentLocationCode)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            try
            {
                hrmsData.UpdateEmployeeAsUm(employeeId, operationalRole, currentLocationCode, objLoginHelper.LogInID, "INSERT");
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public ActionResult SaveResignEmployee(string employeeId, string releaseDate, string currentLocationCode, string resignDate)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            try
            {
                Hrm_EmployeeInfo objEmployeeInfo = new Hrm_EmployeeInfo();
                objEmployeeInfo.EmployeeID = employeeId;
                objEmployeeInfo.ReleaseDate = Convert.ToDateTime(Helper.DateFormatMMDDYYYY(releaseDate));
                objEmployeeInfo.LastLocationCode = currentLocationCode;
                objEmployeeInfo.CreatedBy = objLoginHelper.LogInID;
                objEmployeeInfo.ResignDate = Convert.ToDateTime(Helper.DateFormatMMDDYYYY(resignDate));

                hrmsData.CreateResignEmployee(objEmployeeInfo, "INSERT");
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public JsonResult CheckTransferOfficeOrderNo(string officeOrderNo)
        {
            string st;
            bool responseOrderNo = hrmsData.CheckEmployeeTransferOfficeOrderNo(officeOrderNo);
            st = (responseOrderNo) ? "1" : "0";
            return new JsonResult { Data = st };
        }

        public JsonResult LoadEmployeeInfo(string employeeId, byte transferSequenceNumber)
        {
            EmployeeTransferInfo objEmployeeTransferInfo = new EmployeeTransferInfo();
            objEmployeeTransferInfo = hrmsData.ReadEmployeeTransfer(employeeId, transferSequenceNumber);

            objEmployeeTransferInfo.OfficeOrderNoSequence = objEmployeeTransferInfo.OfficeOrderNo.Split('-')[1];
            objEmployeeTransferInfo.OfficeOrderNo = objEmployeeTransferInfo.OfficeOrderNo.Split('-')[0] + "-";

            objEmployeeTransferInfo.TransferDateString = objEmployeeTransferInfo.TransferDate.ToString("dd-MMM-yyyy");
            objEmployeeTransferInfo.RequisitionReceivedOnString = Convert.ToDateTime(objEmployeeTransferInfo.RequisitionReceivedOn).ToString("dd-MMM-yyyy");
            objEmployeeTransferInfo.OfficeOrderDateString = Convert.ToDateTime(objEmployeeTransferInfo.OfficeOrderDate).ToString("dd-MMM-yyyy");
            objEmployeeTransferInfo.TransferReleaseDateString = Convert.ToDateTime(objEmployeeTransferInfo.TransferReleaseDate).ToString("dd-MMM-yyyy");
            objEmployeeTransferInfo.ActualTransferJoiningDateString = objEmployeeTransferInfo.ActualTransferJoiningDate != null ? Convert.ToDateTime(objEmployeeTransferInfo.ActualTransferJoiningDate).ToString("dd-MMM-yyyy") : null;
            objEmployeeTransferInfo.ActualTransferReleaseDateString = objEmployeeTransferInfo.ActualTransferReleaseDate != null ? Convert.ToDateTime(objEmployeeTransferInfo.ActualTransferReleaseDate).ToString("dd-MMM-yyyy") : null;

            return new JsonResult { Data = objEmployeeTransferInfo };
        }

        public JsonResult UpdateEmployeeTransfer(EmployeeTransferInfo objEmployeeTransferInfo)
        {
            try
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Transfer Is Succeed") };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }
    }
}
