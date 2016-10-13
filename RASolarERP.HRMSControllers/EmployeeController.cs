using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using RASolarERP.Web.Models;
using RASolarHelper;
using RASolarERP.Model;
using RASolarSecurity.Model;
using RASolarHRMS.Model;

using RASolarERP.Web.Areas.HRMS.Models;
using RASolarERP.Web.Areas.Sales.Models;
using RASolarERP.DomainModel.HRMSModel;
using System.IO;

namespace RASolarERP.Web.Areas.HRMS.Controllers
{
    public class EmployeeController : Controller
    {
        LoginHelper objLoginHelper = new LoginHelper();
        RASolarSecurityData securityDal = new RASolarSecurityData();
        private HRMSData hrmsData = new HRMSData();
        string message = string.Empty;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EmployeeDetails()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForHRMS, "EmployeeDetails", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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
            ViewBag.CalenderDate = objLoginHelper.MonthOpenForHRMS;

            ViewBag.EmployeeId = "";
            ViewBag.EmployeeSearchId = "";

            EmployeeDetails objEmployeeDetails = new EmployeeDetails();

            if (!System.IO.File.Exists(Path.Combine(Server.MapPath("~/" + Helper.EmployeeImagePath(objEmployeeDetails.EmployeeID)))))
            {
                objEmployeeDetails.EmployeeImagePath = "../../" + Helper.NoEmployeeImage;
            }

            if (!System.IO.File.Exists(Path.Combine(Server.MapPath("~/" + Helper.EmployeeSignaturePath(objEmployeeDetails.EmployeeID)))))
            {
                objEmployeeDetails.EmployeeSignaturePath = "../../" + Helper.NoEmployeeSignature;
            }

            return View(objEmployeeDetails);
        }

        [HttpPost]
        public ActionResult EmployeeDetails(FormCollection fCollection)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

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
            ViewBag.CalenderDate = objLoginHelper.MonthOpenForHRMS;

            string EmployeeID = string.Empty;
            if (!string.IsNullOrEmpty(fCollection["hfSearchEmployeeId"]))
            {
                EmployeeID = fCollection["hfSearchEmployeeId"];

                ViewBag.EmployeeId = "";
                ViewBag.EmployeeSearchId = fCollection["txtSearchEmployeeId"];
            }
            else
            {
                EmployeeID = Helper.EmployeeIdProcess(fCollection["txtEmployeeId"]);

                ViewBag.EmployeeId = fCollection["txtEmployeeId"];
                ViewBag.EmployeeSearchId = "";
            }

            EmployeeDetails objEmployeeDetails = new EmployeeDetails();
            objEmployeeDetails = hrmsData.EmployeeDetailsInfo(EmployeeID);

            if (!System.IO.File.Exists(Path.Combine(Server.MapPath("~/" + Helper.EmployeeImagePath(objEmployeeDetails.EmployeeID)))))
            {
                objEmployeeDetails.EmployeeImagePath = "../../" + Helper.NoEmployeeImage;
            }

            if (!System.IO.File.Exists(Path.Combine(Server.MapPath("~/" + Helper.EmployeeSignaturePath(objEmployeeDetails.EmployeeID)))))
            {
                objEmployeeDetails.EmployeeSignaturePath = "../../" + Helper.NoEmployeeSignature;
            }

            return View(objEmployeeDetails);
        }

        public ActionResult AddEditEmployee()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForHRMS, "AddEditEmployee", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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
            ViewBag.CalenderDate = objLoginHelper.MonthOpenForHRMS;

            //start for save btn hide under show 
            ViewBag.hfForSavehide = "0";
            //end  for save btn hide under show 

            ViewBag.EmployeeId = "";
            ViewBag.EmployeeSearchId = "";

            EmployeeDetails objEmployeeDetails = new EmployeeDetails();
            objEmployeeDetails = hrmsData.EmployeeDetailsInfo(string.Empty);

            if (!System.IO.File.Exists(Path.Combine(Server.MapPath("~/" + Helper.EmployeeImagePath(objEmployeeDetails.EmployeeID)))))
            {
                objEmployeeDetails.EmployeeImagePath = "../../" + Helper.NoEmployeeImage;
            }

            if (!System.IO.File.Exists(Path.Combine(Server.MapPath("~/" + Helper.EmployeeSignaturePath(objEmployeeDetails.EmployeeID)))))
            {
                objEmployeeDetails.EmployeeSignaturePath = "../../" + Helper.NoEmployeeSignature;
            }

            return View(objEmployeeDetails);
        }

        [HttpPost]
        public ActionResult AddEditEmployee(FormCollection fCollection)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

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
            ViewBag.CalenderDate = objLoginHelper.MonthOpenForHRMS;
            //start for hidden save under show btn
            ViewBag.hfForSavehide = fCollection["hfForhidenSaveBtnUnderShowbtnContr"];
            //end  for save btn hide under show 
            string EmployeeID = string.Empty;
            if (!string.IsNullOrEmpty(fCollection["hfSearchEmployeeId"]))
            {
                EmployeeID = fCollection["hfSearchEmployeeId"];

                ViewBag.EmployeeId = "";
                ViewBag.EmployeeSearchId = fCollection["txtSearchEmployeeId"];
              
            }
            else
            {
                EmployeeID = Helper.EmployeeIdProcess(fCollection["txtEmployeeId"]);

                ViewBag.EmployeeId = fCollection["txtEmployeeId"];
                ViewBag.EmployeeSearchId = "";
            }

            EmployeeDetails objEmployeeDetails = new EmployeeDetails();
            objEmployeeDetails = hrmsData.EmployeeDetailsInfo(EmployeeID);

            if (!System.IO.File.Exists(Path.Combine(Server.MapPath("~/" + Helper.EmployeeImagePath(objEmployeeDetails.EmployeeID)))))
            {
                objEmployeeDetails.EmployeeImagePath = "../../" + Helper.NoEmployeeImage;
            }

            if (!System.IO.File.Exists(Path.Combine(Server.MapPath("~/" + Helper.EmployeeSignaturePath(objEmployeeDetails.EmployeeID)))))
            {
                objEmployeeDetails.EmployeeSignaturePath = "../../" + Helper.NoEmployeeSignature;
            }

            return View(objEmployeeDetails);
        }

        public ActionResult EmployeeCV()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForHRMS, "EmployeeCV", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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

            ViewBag.EmployeeId = "";
            ViewBag.EmployeeSearchId = "";

            EmployeeDetails objEmployeeDetails = new EmployeeDetails();

            if (!System.IO.File.Exists(Path.Combine(Server.MapPath("~/" + Helper.EmployeeImagePath(objEmployeeDetails.EmployeeID)))))
            {
                objEmployeeDetails.EmployeeImagePath = "../../" + Helper.NoEmployeeImage;
            }

            if (!System.IO.File.Exists(Path.Combine(Server.MapPath("~/" + Helper.EmployeeSignaturePath(objEmployeeDetails.EmployeeID)))))
            {
                objEmployeeDetails.EmployeeSignaturePath = "../../" + Helper.NoEmployeeSignature;
            }

            return View(objEmployeeDetails);
        }

        [HttpPost]
        public ActionResult EmployeeCV(FormCollection fCollection)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

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
            ViewBag.CalenderDate = objLoginHelper.MonthOpenForHRMS;

            string EmployeeID = string.Empty;
            if (!string.IsNullOrEmpty(fCollection["hfSearchEmployeeId"]))
            {
                EmployeeID = fCollection["hfSearchEmployeeId"];

                ViewBag.EmployeeId = "";
                ViewBag.EmployeeSearchId = fCollection["txtSearchEmployeeId"];
            }
            else
            {
                EmployeeID = Helper.EmployeeIdProcess(fCollection["txtEmployeeId"]);

                ViewBag.EmployeeId = fCollection["txtEmployeeId"];
                ViewBag.EmployeeSearchId = "";
            }

            EmployeeDetails objEmployeeDetails = new EmployeeDetails();
            objEmployeeDetails = hrmsData.EmployeeDetailsInfo(EmployeeID);

            if (!System.IO.File.Exists(Path.Combine(Server.MapPath("~/" + Helper.EmployeeImagePath(objEmployeeDetails.EmployeeID)))))
            {
                objEmployeeDetails.EmployeeImagePath = "../../" + Helper.NoEmployeeImage;
            }

            if (!System.IO.File.Exists(Path.Combine(Server.MapPath("~/" + Helper.EmployeeSignaturePath(objEmployeeDetails.EmployeeID)))))
            {
                objEmployeeDetails.EmployeeSignaturePath = "../../" + Helper.NoEmployeeSignature;
            }

            return View(objEmployeeDetails);
        }

        public JsonResult SaveEmployeeBasicNEmploymentInfo(EmployeeDetails objEmployeeDetails)
        {
            try
            {

                objLoginHelper = (LoginHelper)Session["LogInInformation"];

                Hrm_EmployeeInfo objEmployeeInfo = new Hrm_EmployeeInfo();
                objEmployeeInfo = hrmsData.ProcessEmployeeBasicNEmploymentInfo(objEmployeeDetails);

                Hrm_EmployeeWiseBankAccount objEmployeeWiseBankAccount = new Hrm_EmployeeWiseBankAccount();
                objEmployeeWiseBankAccount = hrmsData.ProcessEmployeeWiseBankAccount(objEmployeeDetails);

                Hrm_EmployeeWiseSalaryStructureMaster objEmployeeSalaryStructureMaster = new Hrm_EmployeeWiseSalaryStructureMaster();
                List<Hrm_EmployeeWiseSalaryStructureDetails> lstEmloyeeSalaryStructureDetails = new List<Hrm_EmployeeWiseSalaryStructureDetails>();

                if (!hrmsData.EmployeeExistOrNot(objEmployeeDetails.EmployeeID))
                {

                    if (!Helper.EmployeeIdValidation(objEmployeeDetails.EmployeeID))
                    {
                        return new JsonResult { Data = ExceptionHelper.ExceptionCustomErrorMessage("Employee Id Is Not Valid") };
                    }

                    objEmployeeSalaryStructureMaster = hrmsData.ProcessEmployeeWiseSalaryStructureMaster(objEmployeeInfo.EmployeeID, objEmployeeInfo.LastDesignation, objEmployeeInfo.LastEmploymentType, objEmployeeInfo.JoiningDate);
                    lstEmloyeeSalaryStructureDetails = hrmsData.ProcessEmployeeWiseSalaryStructureDetails(objEmployeeInfo.EmployeeID, objEmployeeSalaryStructureMaster.SalaryStructureSeqNo, objEmployeeInfo.LastDesignation, objEmployeeInfo.LastEmploymentType);

                    objEmployeeInfo.CreatedBy = objLoginHelper.LogInID;
                    objEmployeeInfo.CreatedOn = DateTime.Now;

                    hrmsData.CreateEmployeeBasicNEmploymentInfo(objEmployeeInfo, objEmployeeSalaryStructureMaster, lstEmloyeeSalaryStructureDetails, objEmployeeWiseBankAccount);
                }
                else
                {

                    objEmployeeInfo.ModifiedBy = objLoginHelper.LogInID;
                    objEmployeeInfo.ModifiedOn = DateTime.Now;

                    //objEmployeeSalaryStructureMaster = hrmsData.ProcessEmployeeWiseSalaryStructureMasterUpdate(objEmployeeInfo.EmployeeID, objEmployeeInfo.LastDesignation, objEmployeeInfo.LastEmploymentType, objEmployeeInfo.JoiningDate);
                    //lstEmloyeeSalaryStructureDetails = hrmsData.ProcessEmployeeWiseSalaryStructureDetails(objEmployeeInfo.EmployeeID, objEmployeeSalaryStructureMaster.SalaryStructureSeqNo, objEmployeeInfo.LastDesignation, objEmployeeInfo.LastEmploymentType);

                    hrmsData.UpdateEmployeeBasicNEmploymentInfo(objEmployeeInfo, objEmployeeSalaryStructureMaster, lstEmloyeeSalaryStructureDetails, objEmployeeWiseBankAccount);
                }

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty) };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

        public JsonResult SaveEmployeeEducationalQualification(EmployeeDetails objEmployeeDetails)
        {
            if (!hrmsData.EmployeeExistOrNot(objEmployeeDetails.EmployeeID))
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Employee Is Not Exist.\nPlease Save Employee Basic Information First.\n Then Try Again.") };
            }
            return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Employee Is Not Exist.\nPlease Save Employee Basic Information First.\n Then Try Again.") };
        }

        public JsonResult SaveEmployeeProfessionalDegree(EmployeeDetails objEmployeeDetails)
        {
            if (!hrmsData.EmployeeExistOrNot(objEmployeeDetails.EmployeeID))
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Employee Is Not Exist.\nPlease Save Employee Basic Information First.\n Then Try Again.") };
            }
            return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Employee Is Not Exist.\nPlease Save Employee Basic Information First.\n Then Try Again.") };
        }

        public JsonResult SaveEmployeeTrainingSummary(EmployeeDetails objEmployeeDetails)
        {
            if (!hrmsData.EmployeeExistOrNot(objEmployeeDetails.EmployeeID))
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Employee Is Not Exist.\nPlease Save Employee Basic Information First.\n Then Try Again.") };
            }
            return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Employee Is Not Exist.\nPlease Save Employee Basic Information First.\n Then Try Again.") };
        }

        public JsonResult SaveEmployeeExperiance(EmployeeDetails objEmployeeDetails)
        {
            if (!hrmsData.EmployeeExistOrNot(objEmployeeDetails.EmployeeID))
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Employee Is Not Exist.\nPlease Save Employee Basic Information First.\n Then Try Again.") };
            }
            return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Employee Is Not Exist.\nPlease Save Employee Basic Information First.\n Then Try Again.") };
        }

        public JsonResult SaveEmployeeInterviewStatus(EmployeeDetails objEmployeeDetails)
        {
            if (!hrmsData.EmployeeExistOrNot(objEmployeeDetails.EmployeeID))
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Employee Is Not Exist.\nPlease Save Employee Basic Information First.\n Then Try Again.") };
            }
            return new JsonResult { Data = ExceptionHelper.ExceptionMessage("Employee Is Not Exist.\nPlease Save Employee Basic Information First.\n Then Try Again.") };
        }

        public ActionResult AddEmployeeImage(HttpPostedFileBase uploadEmployeeImage, string employeeId)
        {
            var fileName = "";

            try
            {
                if (!hrmsData.EmployeeExistOrNot(employeeId))
                {
                    return Json(new { data = ExceptionHelper.ExceptionMessage("Employee Is Not Exist.\nPlease Save Employee Basic Information First.\n Then Try Again.", "../../" + Helper.ImagePath + "/" + fileName) });
                }

                if (uploadEmployeeImage != null)
                {
                    fileName = employeeId + "-Img" + Path.GetExtension(uploadEmployeeImage.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/" + Helper.ImagePath), fileName);

                    uploadEmployeeImage.SaveAs(physicalPath);
                }

                return Json(new { data = ExceptionHelper.ExceptionMessage("Image Upload Is Complete", "../../" + Helper.ImagePath + "/" + fileName) });
            }
            catch (Exception ex)
            {
                return Json(new { data = ExceptionHelper.ExceptionMessage(ex, "../../" + Helper.ImagePath + "/" + fileName) }); //Content(fileName.ToString());
            }
        }

        public ActionResult AddEmployeeSignature(HttpPostedFileBase uploadEmployeeSignature, string employeeId)
        {
            var fileName = "";

            try
            {
                if (!hrmsData.EmployeeExistOrNot(employeeId))
                {
                    return Json(new { data = ExceptionHelper.ExceptionMessage("Employee Is Not Exist.\nPlease Save Employee Basic Information First.\n Then Try Again.", "../../" + Helper.ImagePath + "/" + fileName) });
                }

                if (uploadEmployeeSignature != null)
                {
                    fileName = employeeId + "-Sin" + Path.GetExtension(uploadEmployeeSignature.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/" + Helper.ImagePath), fileName);

                    uploadEmployeeSignature.SaveAs(physicalPath);
                }
                return Json(new { data = ExceptionHelper.ExceptionMessage("Signature Upload Is Complete", "../../" + Helper.ImagePath + "/" + fileName) });
            }
            catch (Exception ex)
            {
                return Json(new { data = ExceptionHelper.ExceptionMessage(ex, "../../" + Helper.ImagePath + "/" + fileName) }); //Content(fileName.ToString());
            }
        }

        public ActionResult OperationalRoleList(string designationId)
        {
            return new JsonResult { Data = hrmsData.ReadOperationalRole(designationId) };
        }

        public ActionResult UpazillaList(string districtId)
        {
            return new JsonResult { Data = hrmsData.ReadUpazillaInfo(districtId) };
        }

        public JsonResult LoadEmployeeBankInfo(string locationCode)
        {
            return new JsonResult { Data = hrmsData.ReadBankInformation(locationCode) };
        }

        public JsonResult LoadBranchName(string bankId, string locationCode)
        {
            return new JsonResult { Data = hrmsData.ReadBankBranchInformation(locationCode, bankId) };
        }

        public JsonResult SalaryDisbursementBankAccountNumber(string locationCode, string bankId, string bankBranchID)
        {
            return new JsonResult { Data = hrmsData.ReadBankInformation(locationCode, bankId, bankBranchID) };
        }
    }
}
