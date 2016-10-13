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
using Telerik.Web.Mvc;
using System.IO;
using System.Text;
using RASolarERP.DomainModel.SalesModel;
using System.Collections;

namespace RASolarERP.Web.Areas.HRMS.Controllers
{
    public class EmployeeDeptController : Controller
    {
        LoginHelper objLoginHelper = new LoginHelper();
        private SalesData salesDal = new SalesData();
        private RASolarERPData erpDal = new RASolarERPData();
        private HRMSData hrmsData = new HRMSData();
        RASolarSecurityData securityDal = new RASolarSecurityData();
        HelperData helperDal = new HelperData();

        string message = string.Empty;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EmployeeInformation()
        {
            return View();
        }

        public ActionResult EmployeeSearch()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForHRMS, "EmployeeSearch", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            ViewBag.Designation = hrmsData.ReadEmployeeDesignation();

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

            ViewBag.Zone = erpDal.Zone(); // Need To Change After Riview

            return View();
        }

        [GridAction]
        public ActionResult LoadEmployeeDetails(string employeeID, string employeeName, string designation, string locationCode, string employeeStatus)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<EmployeeDetailsInfo> lstEmployeeDetails = new List<EmployeeDetailsInfo>();
            lstEmployeeDetails = hrmsData.ReadEmployeeSearchDetails(employeeID, employeeName, designation, locationCode, Convert.ToByte(employeeStatus));

            return View(new GridModel<EmployeeDetailsInfo>
            {
                Data = lstEmployeeDetails
            });
        }

        public ActionResult DailyTADAEntry()
        {

            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForHRMS, "DailyTADAEntry", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            ViewBag.Designation = hrmsData.ReadEmployeeDesignation();

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

            TADADetails objTADADetails = new TADADetails();
            objTADADetails.DateOfTADA = DateTime.Now.Date;
            objTADADetails.TADAEntryMonth = DateTime.Now.Month;
            objTADADetails.DaysOfPendingEntry = (byte)(DateTime.Now.Date - objLoginHelper.TransactionOpenDate.Date).Days;
            objTADADetails.Employee = hrmsData.ReadLocationWiseEmployee(objLoginHelper.LocationCode);
            objTADADetails.MonthList = new YearMonthFormat().MonthList();

            return View(objTADADetails);
        }


        public ActionResult EmployeeWiseTrainingAssessmentEntry()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForSales, "EmployeeWiseTrainingAssessmentEntry", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

           // ViewBag.IdNo = hrmsData.ReadLocationWiseEmployeeForAssEntry(objLoginHelper.LocationCode);
            ViewBag.MullaynerName = hrmsData.ReadEmployeeEvulationType();
           // ViewBag.MullaynerKal = hrmsData.ReadEmployeeEvulationSubType(); 

           // List<AssesmentGrid> lstForAssesGrid = new List<AssesmentGrid>();

           // lstForAssesGrid = hrmsData.ReadlstForAssesGrid();
            ViewBag.LoginId = objLoginHelper.LogInID;
            ViewBag.YearWeek = objLoginHelper.CurrentYearWeek;
            ViewBag.WeekStartDate = objLoginHelper.FirstDateOfCurrentWeek.ToString("dd-MMM-yyyy");
            ViewBag.WeekEndDate = objLoginHelper.LastDateOfCurrentWeek.ToString("dd-MMM-yyyy");

            ViewBag.LocationTitle = objLoginHelper.LocationTitle;
            ViewBag.Location = objLoginHelper.Location;
            ViewBag.ZoneTitle = objLoginHelper.ZoneTitle;
            ViewBag.ZoneName = objLoginHelper.LogInForZoneName;
            ViewBag.RegionTitle = objLoginHelper.RegionTitle;
            ViewBag.RegionName = objLoginHelper.LogInForRegionName;
            ViewBag.UnitTitle = objLoginHelper.UnitTitle;
            ViewBag.UnitName = objLoginHelper.LogInForUnitName;
            ViewBag.OpenMonthYear = "Month: " + objLoginHelper.MonthOpenForDailyProgressReview.ToString("MMMM-yy");
            ViewBag.OpenDay = objLoginHelper.MonthOpenForDailyProgressReview.ToString("dd-MMM-yyyy");
            ViewBag.LocationCode = objLoginHelper.LocationCode;
            ViewBag.ModuleName = objLoginHelper.ModluleTitle;
            ViewBag.TopMenu = objLoginHelper.TopMenu;

            return View();
        }

        public JsonResult MullaynerKal(string ddlMullaynerName) 
        {
            ArrayList arl = new ArrayList();

            List<EmployeeEvulationSubType> lstLocationInfo = new List<EmployeeEvulationSubType>();
            lstLocationInfo = hrmsData.ReadEmployeeEvulationSubType(ddlMullaynerName);

            foreach (EmployeeEvulationSubType li in lstLocationInfo)
            {
                arl.Add(new { Value = li.EvaluationSubTypeID, Display = li.EvaluationPeriodDetails });
            }

            return new JsonResult { Data = arl };
        
        }




        public JsonResult GetEmpID(string mullaynNam, string mullaynKal) 
        {

            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            
            ArrayList arl = new ArrayList();
            //string locationCode = objLoginHelper.LocationCode;
            List<PersonEmployeeDetailsInfo> lstPersonEmployeeDetailsInfo = new List<PersonEmployeeDetailsInfo>();
            lstPersonEmployeeDetailsInfo = hrmsData.ReadLocationWiseEmployeeForAssEntry(objLoginHelper.LocationCode, mullaynNam, mullaynKal);

            foreach (PersonEmployeeDetailsInfo li in lstPersonEmployeeDetailsInfo)
            {
                arl.Add(new { Value = li.EmployeeID, Display = li.EmployeeName });
            }

            return new JsonResult { Data = arl };
        
        }

        public JsonResult TodateCalcForConf(string fromdate, string todate)
        {

            string calctodate = string.Empty;

            if (todate == "1")
                calctodate = Convert.ToDateTime(fromdate).AddYears(1).ToString("dd-MMM-yyyy");
            if (todate == "3")
                calctodate = Convert.ToDateTime(fromdate).AddMonths(3).ToString("dd-MMM-yyyy");
            if (todate == "6")
                calctodate = Convert.ToDateTime(fromdate).AddMonths(6).ToString("dd-MMM-yyyy");

            ArrayList arl = new ArrayList();

            //List<EmployeeEvulationSubType> lstLocationInfo = new List<EmployeeEvulationSubType>();
            //lstLocationInfo = hrmsData.ReadEmployeeEvulationSubType(ddlMullaynerName);

            //foreach (EmployeeEvulationSubType li in lstLocationInfo)
            //{
            //    arl.Add(new { Value = li.EvaluationSubTypeID, Display = li.EvaluationPeriodDetails });
            //}

            arl.Add(new { calctodate = calctodate });

            return new JsonResult { Data = arl };

        }

        //do work for 

        public JsonResult GetDataForEmployee(string EmployeeID)  
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            string locationCode = objLoginHelper.LocationCode;
            ArrayList arr = new ArrayList();

            List<EmployeeInfoForAssEntry> lstGetLocationWiseEmployee = new List<EmployeeInfoForAssEntry>();
            lstGetLocationWiseEmployee = hrmsData.ReadLocationWiseEmployeeForAssEntry(locationCode, EmployeeID); 

            foreach (EmployeeInfoForAssEntry emp in lstGetLocationWiseEmployee)
            {
                arr.Add(new { EmployeeID = emp.EmployeeID, EmployeeName  = emp.EmployeeName, HDesignation=emp.HDesignationName, JoiningDate=emp.JoiningDate.ToString("dd-MMM-yyyy") });
            }

            return new JsonResult { Data = arr };
        }


        public JsonResult GetDataForERPT(string viewScore, string EmployeeID, string mullayenerName, string ddlMullaynerKal, string valOfthis)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];
            string locationCode = objLoginHelper.LocationCode;
            ArrayList arr = new ArrayList();

            List<ERPValueForAssessment> lstGetDataForERPT = new List<ERPValueForAssessment>();
            lstGetDataForERPT = hrmsData.GetDataForERPT(viewScore, locationCode, EmployeeID, mullayenerName, ddlMullaynerKal, valOfthis);

            foreach (ERPValueForAssessment emp in lstGetDataForERPT)
            {
                arr.Add(new { EmployeeID = emp.EmployeeID, ERPValue = emp.ERPValue, Score = emp.Score });
            }

            return new JsonResult { Data = arr };
        }

        [GridAction]
        public ActionResult LoadGridDetails(string mullaynNam, string mullaynKal)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            List<AssesmentGrid> lstAssesmentGrid = new List<AssesmentGrid>();
            lstAssesmentGrid = hrmsData.ReadlstForAssesGrid(mullaynNam, mullaynKal); 

            return View(new GridModel<AssesmentGrid>
            {
                Data = lstAssesmentGrid
            });
        }


        public JsonResult SaveTrainingAssesmentEntry(string ddlIdNo, string empEvType, string empEvKal, string trainingDate, string assesmentDate, string prmIsAssessmentCompleted, string txtUnitManagerMontobbo, string chkbxVal, string aprovedDate, string assesmentOptionId, string assesmentOptionCriteria, string weight, string pPurboPNo, string prmIsAssessmentSelectedByERPData, string userLogin, string insertERPData, Int32 gridLength, string rowWiseRemarks)
        {


            string successMessage = string.Empty;
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            string prmLocationCode = objLoginHelper.LogInForUnitCode.ToString();
            string prmUserName = objLoginHelper.LogInID.ToString();



            ArrayList arrCriteria = new ArrayList();
            ArrayList arrCriteriaScore = new ArrayList();
            ArrayList arrCriteriaBangla = new ArrayList(); 
           
             string[] assesmentOptionCriteriaArry = assesmentOptionCriteria.Split('_');
             for (int k = 0; k < assesmentOptionCriteriaArry.Length; k++)
             {
                 int optionIDone = k + 1;
                 string optionID = optionIDone.ToString();
                 string criteriaValue = assesmentOptionCriteriaArry[k].ToString();



                 if (criteriaValue!="")
                 {
                     List<AssesmentCriteriaNScore> lstAssesmentCriteriaNScore = new List<AssesmentCriteriaNScore>();
                     lstAssesmentCriteriaNScore = hrmsData.ReadAssesmentCriteriaNScore(optionID, criteriaValue);

                     foreach (AssesmentCriteriaNScore emp in lstAssesmentCriteriaNScore)
                     {
                        // arrCriteriaBangla.Add(new { AssessmentCriteriaOptionNameInEnglish = emp.AssessmentCriteriaOptionNameInEnglish });
                         arrCriteria.Add(new { EvaluationCriteriaOptionID = emp.EvaluationCriteriaOptionID });
                         arrCriteriaScore.Add(new { EvaluationCriteriaScore = emp.EvaluationCriteriaScore });
                     }
                    
                 
                 }

             }

            
            string crID = "";
            string crArrID="";

            string crScore = "";
            string crArrScore = "";

            for (int k = 0; k < arrCriteria.Count; k++)
            {

                crID = arrCriteria[k].ToString();
                int abd = crID.IndexOf('=');

                string cdf = crID.Substring(abd + 1);
                crArrID = crArrID + cdf.Substring(0, cdf.Length - 1).Trim() + "_";
            }

            for (int k = 0; k < arrCriteriaScore.Count; k++)
            {

                crScore = arrCriteriaScore[k].ToString();
                int abd = crScore.IndexOf('=');

                string cdf = crScore.Substring(abd + 1);
                crArrScore = crArrScore + cdf.Substring(0, cdf.Length - 1).Trim() + "_";
            }

            try
            {

                //if (objLoginHelper.Location == Helper.Zone || objLoginHelper.Location == Helper.HeadOffice)
                //{

                                                //ddlIdNo: ddlIdNo, empEvType: empEvType,empEvKal:empEvKal, trainingDate: trainingDate, assesmentDate: assesmentDate, prmIsAssessmentCompleted: prmIsAssessmentCompleted, txtUnitManagerMontobbo: txtUnitManagerMontobbo, chkbxVal: chkbxVal, aprovedDate: aprovedDate, assesmentOptionId: assesmentOptionId, assesmentOptionCriteria: assesmentOptionCriteria, weight: weight, pPurboPNo: pPurboPNo, prmIsAssessmentSelectedByERPData: prmIsAssessmentSelectedByERPData, userLogin: userLogin, insertData: insertData, gridLength: rowsCount 
                successMessage = hrmsData.SaveTrainingAssesmentEntry(prmLocationCode, ddlIdNo, empEvType, empEvKal, trainingDate, assesmentDate, prmIsAssessmentCompleted, txtUnitManagerMontobbo, chkbxVal, aprovedDate, crArrID, crArrScore, weight, pPurboPNo, prmIsAssessmentSelectedByERPData, userLogin, insertERPData, rowWiseRemarks); 

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(string.Empty) };

               
            }
            catch (Exception ex)
            {


                return new JsonResult
                {

                    Data = ExceptionHelper.ExceptionMessage(ex)
                };

            }
        }








        public ActionResult DailyTADATestEntry()
        {

            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForHRMS, "DailyTADAEntry", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
            {
                Session["messageInformation"] = message;
                return RedirectToAction("ErrorMessage", "../ErrorHnadle");
            }

            ViewBag.Designation = hrmsData.ReadEmployeeDesignation();

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

            TADADetails objTADADetails = new TADADetails();
            objTADADetails.DateOfTADA = DateTime.Now.Date;
            objTADADetails.TADAEntryMonth = DateTime.Now.Month;
            objTADADetails.DaysOfPendingEntry = (byte)(DateTime.Now.Date - objLoginHelper.TransactionOpenDate.Date).Days;
            objTADADetails.Employee = hrmsData.ReadLocationWiseEmployee(objLoginHelper.LocationCode);
            objTADADetails.MonthList = new YearMonthFormat().MonthList();

            return View(objTADADetails);
        }



        [GridAction]
        public ActionResult TADADetails(string TADAYearMonth, string employeeID)
        {
            var TADAEntryMonth = TADAYearMonth.Substring(4, 2); ;

            DateTime dateFrom = DateTime.Now, dateTo = DateTime.Now;

            if (string.IsNullOrEmpty(TADAEntryMonth))
            {
                dateFrom = Helper.MonthStartDate(DateTime.Now);
                dateTo = Helper.MonthEndDate(DateTime.Now);
            }
            else
            {
                if (DateTime.Now.Month < Convert.ToInt32(TADAEntryMonth))
                {
                    dateFrom = Helper.MonthStartDateFromMonthNumber(Convert.ToInt32(TADAEntryMonth), new DateTime(DateTime.Now.Year - 1, Convert.ToInt32(TADAEntryMonth), 1));
                    dateTo = Helper.MonthEndDateFromMonthNumber(Convert.ToInt32(TADAEntryMonth), new DateTime(DateTime.Now.Year - 1, Convert.ToInt32(TADAEntryMonth), 1));
                }
                else
                {
                    dateFrom = Helper.MonthStartDateFromMonthNumber(Convert.ToInt32(TADAEntryMonth), DateTime.Now);
                    dateTo = Helper.MonthEndDateFromMonthNumber(Convert.ToInt32(TADAEntryMonth), DateTime.Now);
                }
            }

            return View(new GridModel<TADADetails>
            {
                Data = hrmsData.ReadTADADetails(dateFrom, dateTo, employeeID)
            });

        }

        public JsonResult DailyTADAEntrySave(TADADetails objTADADetails)
        {
            try
            {
                objLoginHelper = (LoginHelper)Session["LogInInformation"];

                Hrm_EmployeeNDateWiseTADAEntry objEmployeeNDateWiseTADAEntry = new Hrm_EmployeeNDateWiseTADAEntry();
                objEmployeeNDateWiseTADAEntry.YearMonth = objLoginHelper.YearMonthCurrent;
                objEmployeeNDateWiseTADAEntry.LocationCode = objLoginHelper.LocationCode;
                objEmployeeNDateWiseTADAEntry.EmployeeID = objTADADetails.EmployeeID;
                objEmployeeNDateWiseTADAEntry.DateOfTADA = objTADADetails.DateOfTADA;
                objEmployeeNDateWiseTADAEntry.TAAmount = objTADADetails.TAAmount;
                objEmployeeNDateWiseTADAEntry.ParticularsForTA = objTADADetails.ParticularsForTA;
                objEmployeeNDateWiseTADAEntry.DAAmount = objTADADetails.DAAmount;
                objEmployeeNDateWiseTADAEntry.ParticularsForDA = objTADADetails.ParticularsForDA;
                objEmployeeNDateWiseTADAEntry.DaysOfPendingEntry = objTADADetails.DaysOfPendingEntry;

                if (string.IsNullOrEmpty(objTADADetails.EditEmployee))
                {
                    objEmployeeNDateWiseTADAEntry.CreatedBy = objLoginHelper.LogInID;
                    objEmployeeNDateWiseTADAEntry.CreatedOn = DateTime.Now;

                    hrmsData.CreateTADAEntry(objEmployeeNDateWiseTADAEntry);
                }
                else
                {
                    objEmployeeNDateWiseTADAEntry.ModifiedBy = objLoginHelper.LogInID;
                    objEmployeeNDateWiseTADAEntry.ModifiedOn = DateTime.Now;

                    hrmsData.UpdateTADAEntry(objEmployeeNDateWiseTADAEntry);
                }

                return new JsonResult { Data = ExceptionHelper.ExceptionMessage("") };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }

       /* public JsonResult DailyTADATestEntrySave(TADADetails objTADADetails)
        {
            try
            {
              /*  objLoginHelper = (LoginHelper)Session["LogInInformation"];

                Hrm_EmployeeNDateWiseTADAEntry objEmployeeNDateWiseTADAEntry = new Hrm_EmployeeNDateWiseTADAEntry();
                objEmployeeNDateWiseTADAEntry.YearMonth = objLoginHelper.YearMonthCurrent;
                objEmployeeNDateWiseTADAEntry.LocationCode = objLoginHelper.LocationCode;
                objEmployeeNDateWiseTADAEntry.EmployeeID = objTADADetails.EmployeeID;
                objEmployeeNDateWiseTADAEntry.DateOfTADA = objTADADetails.DateOfTADA;
                objEmployeeNDateWiseTADAEntry.TAAmount = objTADADetails.TAAmount;
                objEmployeeNDateWiseTADAEntry.ParticularsForTA = objTADADetails.ParticularsForTA;
                objEmployeeNDateWiseTADAEntry.DAAmount = objTADADetails.DAAmount;
                objEmployeeNDateWiseTADAEntry.ParticularsForDA = objTADADetails.ParticularsForDA;
                objEmployeeNDateWiseTADAEntry.DaysOfPendingEntry = objTADADetails.DaysOfPendingEntry;

                if (string.IsNullOrEmpty(objTADADetails.EditEmployee))
                {
                    objEmployeeNDateWiseTADAEntry.CreatedBy = objLoginHelper.LogInID;
                    objEmployeeNDateWiseTADAEntry.CreatedOn = DateTime.Now;

                    hrmsData.CreateTADAEntry(objEmployeeNDateWiseTADAEntry);
                }
                else
                {
                    objEmployeeNDateWiseTADAEntry.ModifiedBy = objLoginHelper.LogInID;
                    objEmployeeNDateWiseTADAEntry.ModifiedOn = DateTime.Now;

                    hrmsData.UpdateTADAEntry(objEmployeeNDateWiseTADAEntry);
                }*/

              /*  return new JsonResult { Data = ExceptionHelper.ExceptionMessage("") };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ExceptionHelper.ExceptionMessage(ex) };
            }
        }*/

        public ActionResult TADAReviewNApproval()
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            if (!securityDal.IsPageAccessible(Helper.ForHRMS, "TADAReviewNApproval", objLoginHelper.UerRoleOrGroupID, Helper.Inactive, out message))
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
            ViewBag.YearMonthFormat = helperDal.ReadYearMonthFormat().Where(s => Convert.ToInt32(s.YearMonthValue) >= 201309);

            return View();
        }

        public ActionResult ExportToCSVDailyTADA(int page, string groupby, string orderBy, string filter, string TADAEntryMonth, string employeeID)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            DateTime dateFrom = DateTime.Now, dateTo = DateTime.Now;

            if (string.IsNullOrEmpty(TADAEntryMonth))
            {
                dateFrom = Helper.MonthStartDate(DateTime.Now);
                dateTo = Helper.MonthEndDate(DateTime.Now);
            }
            else
            {
                if (DateTime.Now.Month < Convert.ToInt32(TADAEntryMonth))
                {
                    dateFrom = Helper.MonthStartDateFromMonthNumber(Convert.ToInt32(TADAEntryMonth), new DateTime(DateTime.Now.Year - 1, Convert.ToInt32(TADAEntryMonth), 1));
                    dateTo = Helper.MonthEndDateFromMonthNumber(Convert.ToInt32(TADAEntryMonth), new DateTime(DateTime.Now.Year - 1, Convert.ToInt32(TADAEntryMonth), 1));
                }
                else
                {
                    dateFrom = Helper.MonthStartDateFromMonthNumber(Convert.ToInt32(TADAEntryMonth), DateTime.Now);
                    dateTo = Helper.MonthEndDateFromMonthNumber(Convert.ToInt32(TADAEntryMonth), DateTime.Now);
                }
            }


            List<TADADetails> lstTADADetails = new List<TADADetails>();
            lstTADADetails = hrmsData.ReadTADADetails(dateFrom, dateTo, employeeID);

            MemoryStream output = new MemoryStream();
            StreamWriter writer = new StreamWriter(output, Encoding.UTF8);

            writer.Write("Date Of TA/DA,");
            writer.Write("TA Amount,");
            writer.Write("Reason For TA,");
            writer.Write("DA Amount,");
            writer.Write("Reason For DA,");
            writer.Write("Days Of Pending Entry,");
            writer.Write("Total TA/DA Amount");
            writer.WriteLine();

            foreach (TADADetails cr in lstTADADetails)
            {
                writer.Write(((DateTime)cr.DateOfTADA).ToString("dd-MMM-yyyy"));
                writer.Write(",");

                writer.Write("\"");
                writer.Write(((decimal)cr.TAAmount).ToString("0"));
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(cr.ParticularsForTA);
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(((decimal)cr.DAAmount).ToString("0"));
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(cr.ParticularsForDA);
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(((decimal)cr.DaysOfPendingEntry).ToString("0"));
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(cr.TotalTADAAmount);
                writer.Write("\"");
                writer.WriteLine();
            }

            writer.Flush();
            output.Position = 0;

            return File(output, "text/comma-separated-values", "RSF-TADAEntry.csv");
        }

        public ActionResult ExportToTADAReviewNApproval(int page, string groupby, string orderBy, string filter, string tADAAprrovalMonth)
        {
            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            string reportType = string.Empty;
            if (objLoginHelper.Location == Helper.Zone)
            {
                reportType = Helper.ZonalOffice;
            }
            else if (objLoginHelper.Location == Helper.Region)
            {
                //reportType = Helper.RegionalOffice;
                reportType = Helper.AreaOffice;
            }

            List<TADADetails> lstTADADetails = new List<TADADetails>();
            lstTADADetails = hrmsData.ReviewTADAAmountNApproval(objLoginHelper.LocationCode, reportType, tADAAprrovalMonth);

            lstTADADetails = (from ss in lstTADADetails
                              select new TADADetails
                              {
                                  EmployeeID = ss.EmployeeID,
                                  TAAmount = ss.TAAmount,
                                  DAAmount = ss.DAAmount,
                                  TotalTADAAmount = ss.TotalTADAAmount,
                                  EmployeeName = ss.EmployeeName,
                                  TotalDaysForTA = ss.TotalDaysForTA,
                                  TotalDaysForDA = ss.TotalDaysForDA,
                                  ZoneCode = ss.ZoneCode,
                                  ZoneName = ss.ZoneName + " [" + ss.ZoneCode + "]",
                                  RegionCode = ss.RegionCode,
                                  RegionName = ss.RegionName + " [" + ss.RegionCode + "]",
                                  ProgramName=ss.ProgramName,
                                  AreaName=ss.AreaName,
                                
                                  UnitCode = ss.UnitCode,
                                  UnitName = ss.UnitName + " [" + ss.UnitCode + "]",
                                  TotalCollection = ss.TotalCollection,
                                  TotalSales = ss.TotalSales,
                                  DesignationName = ss.DesignationName,
                                  DepartmentName = ss.DepartmentName,
                                  JoiningDate = ss.JoiningDate,
                                  TADAMonth = ss.TADAMonth

                              }).ToList();

            MemoryStream output = new MemoryStream();
            StreamWriter writer = new StreamWriter(output, Encoding.UTF8);

            writer.Write("Employee ID,");
            writer.Write("Employee Name,");
            writer.Write("Designation,");
            writer.Write("Department,");

            writer.Write("Work Station,");
            writer.Write("Program Name,");
            writer.Write("Area Name,");
            //writer.Write("Region,");
            //writer.Write("Zone,");
            writer.Write("Date of Joining,");

            //writer.Write("TA/DA Month,");

            writer.Write("From Date,");
            writer.Write("To Date,");
            writer.Write("TA Amount,");
            writer.Write("Total Days For TA,");
            writer.Write("DA Amount,");
            writer.Write("Total Days For DA,");
            writer.Write("Total (TA+DA),");
            writer.Write("Total Sales,");
            writer.Write("Total Collection,");
            writer.Write("Remarks");

            writer.WriteLine();

            foreach (TADADetails cr in lstTADADetails)
            {
                writer.Write(cr.EmployeeID);
                writer.Write(",");

                writer.Write("\"");
                writer.Write(cr.EmployeeName);
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(cr.DesignationName);
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(cr.DepartmentName);
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(cr.UnitName);
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(cr.ProgramName);
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(cr.AreaName);
                writer.Write("\"");
                writer.Write(",");

                //writer.Write("\"");
                //writer.Write(cr.RegionName);
                //writer.Write("\"");
                //writer.Write(",");

                //writer.Write("\"");
                //writer.Write(cr.ZoneName);
                //writer.Write("\"");
                //writer.Write(",");

                writer.Write("\"");
                writer.Write(cr.JoiningDate.ToString("dd-MMM-yyyy"));
                writer.Write("\"");
                writer.Write(",");


                writer.Write("\"");
                writer.Write(Helper.DateFrom(cr.TADAMonth).ToString("dd-MMM-yyyy"));
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(Helper.DateTo(cr.TADAMonth).ToString("dd-MMM-yyyy"));
                writer.Write("\"");
                writer.Write(",");

                //writer.Write("\"");
                //writer.Write(cr.TADAMonth);
                //writer.Write("\"");
                //writer.Write(",");

                writer.Write("\"");
                writer.Write(((decimal)cr.TAAmount).ToString("0"));
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(cr.TotalDaysForTA);
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(((decimal)cr.DAAmount).ToString("0"));
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(cr.TotalDaysForDA);
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(((decimal)cr.TotalTADAAmount).ToString("0"));
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(cr.TotalSales);
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(((decimal)cr.TotalCollection).ToString("0"));
                writer.Write("\"");
                writer.Write(",");

                writer.Write("\"");
                writer.Write(cr.Reason);
                writer.Write("\"");
                writer.WriteLine();
            }

            writer.Flush();
            output.Position = 0;

            return File(output, "text/comma-separated-values", "RSF-TADAReviewNApprovalDetails.xls");
        }

        [GridAction]
        public ActionResult TADAReviewNApprovalDetails(string tADAAprrovalMonth)
        {

            objLoginHelper = (LoginHelper)Session["LogInInformation"];

            string reportType = string.Empty;
            if (objLoginHelper.Location == Helper.Zone)
            {
                reportType = Helper.ZonalOffice;
            }
            else if (objLoginHelper.Location == Helper.Region)
            {
               // reportType = Helper.RegionalOffice;
                reportType = Helper.AreaOffice;
            }

            //tADAAprrovalMonth = "201312";

            List<TADADetails> lstTADADetails = new List<TADADetails>();
            lstTADADetails = hrmsData.ReviewTADAAmountNApproval(objLoginHelper.LocationCode, reportType, tADAAprrovalMonth);

            lstTADADetails = (from ss in lstTADADetails
                              select new TADADetails
                              {
                                  EmployeeID = ss.EmployeeID,
                                  TAAmount = ss.TAAmount,
                                  DAAmount = ss.DAAmount,
                                  TotalTADAAmount = ss.TotalTADAAmount,
                                  EmployeeName = ss.EmployeeName,
                                  TotalDaysForTA = ss.TotalDaysForTA,
                                  TotalDaysForDA = ss.TotalDaysForDA,
                                  ZoneCode = ss.ZoneCode,
                                  ZoneName = ss.ZoneName + " [" + ss.ZoneCode + "]",
                                  RegionCode = ss.RegionCode,
                                  RegionName = ss.RegionName + " [" + ss.RegionCode + "]",
                                  UnitCode = ss.UnitCode,
                                  UnitName = ss.UnitName + " [" + ss.UnitCode + "]",
                                  TotalCollection = ss.TotalCollection,
                                  TotalSales = ss.TotalSales

                              }).ToList();

            return View(new GridModel<TADADetails>
            {
                Data = lstTADADetails
            });
        }
    }
}
