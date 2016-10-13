using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data;
using System.Data.Objects;
using RASolarERP.DomainModel.AMSModel;
using RASolarERP.DomainModel.HRMSModel;
using RASolarHelper;
using System.Data.SqlClient;
using System.Collections;
using System.Data.Entity;

namespace RASolarHRMS.Model
{
    public class RASolarHRMSRepository : IRASolarHRMSRepository
    {
        #region Properties And Constructor

        private RASolarHRAEntities HrmContext { get; set; }

        public RASolarHRMSRepository(RASolarHRAEntities _hrmContext)
        {
            HrmContext = _hrmContext;
        }

        #endregion

        #region Read Methods

        public Hrm_EmployeeInfo ReadEmployeeInfo(string employeeId)
        {
            return HrmContext.Hrm_EmployeeInfo.Where(e => e.EmployeeID == employeeId).FirstOrDefault();
        }

        public List<EmployeeDetailsInfo> ReadEmployeeInfo(List<string> employeeIds)
        {
            var v = (from emi in HrmContext.Hrm_EmployeeInfo
                     where employeeIds.Contains(emi.EmployeeID)
                     select new EmployeeDetailsInfo
                     {
                         EmployeeID = emi.EmployeeID,
                         EmployeeName = emi.EmployeeName,
                         DateOfJoining = emi.JoiningDate,
                         Department = emi.Hrm_DepartmentInfo.DepartmentName,
                         PresentLocation = emi.Common_LocationInfo.LocationName,
                         PresentLocationId = emi.LastLocationCode
                     }).Distinct();

            return v.ToList();
        }

        public List<GetLocationWiseEmployeeForEmployeeRegister> ReadGetLocationWiseEmployeeForEmployeeRegister(string locationCode)
        {
            string locationWiseEmployee = string.Empty;
            locationWiseEmployee = "EXEC REP_GetLocationWiseEmployeeRegister '" + locationCode.ToString() + "'";

            try
            {
                return HrmContext.Database.SqlQuery<GetLocationWiseEmployeeForEmployeeRegister>(locationWiseEmployee).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<EmployeeTransferInfo> ReadGetEmployeeTransferredButNotYetAccepted(string locationCode)
        {
            string locationWiseEmployee = string.Empty;
            locationWiseEmployee = "EXEC REP_GetEmployeeTransferredButNotYetAccepted '" + locationCode.ToString() + "'";

            try
            {
                return HrmContext.Database.SqlQuery<EmployeeTransferInfo>(locationWiseEmployee).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<PersonEmployeeDetailsInfo> ReadPersonLocationWiseEmployee(string locationCode)
        {
            string personlocationWiseEmployee = string.Empty;
            personlocationWiseEmployee = "EXEC SP_LocationWiseEmployee '" + locationCode.ToString() + "'";

            try
            {
                return HrmContext.Database.SqlQuery<PersonEmployeeDetailsInfo>(personlocationWiseEmployee).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<PersonEmployeeDetailsInfo> ReadLocationWiseEmployeeForAssEntry(string locationCode, string mullaynNam, string mullaynKal)
        {
            try
            {
                //var resultList = HrmContext.Database.SqlQuery<PersonEmployeeDetailsInfo>("EXEC [SP_HrmEmployeeWiseEvaluationEntry] 'VIEW_LOCATION_WISE_EMPLOYEES', '" + locationCode + "', NULL, "
                //                 + " NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,NULL");
               
                var resultList = HrmContext.Database.SqlQuery<PersonEmployeeDetailsInfo>("EXEC [SP_HrmEmployeeWiseEvaluationEntry] 'VIEW_LOCATION_WISE_EMPLOYEES', '" + locationCode + "', NULL, '"
                                 + mullaynNam+"','"+mullaynKal+"',NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL");

                return resultList.ToList();
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public List<EmployeeEvulationType> ReadEmployeeEvulationType()
        {
            try
            {
                var resultList = HrmContext.Database.SqlQuery<EmployeeEvulationType>(" SELECT EvaluationTypeID, EvaluationTypeDetails FROM Hrm_EmployeeEvaluationType WHERE [Status] = 0 ");

                return resultList.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<EmployeeEvulationSubType> ReadEmployeeEvulationSubType(string ddlMullaynerName)
        {
            try
            {
                var resultList = HrmContext.Database.SqlQuery<EmployeeEvulationSubType>(" SELECT EvaluationSubTypeID, EvaluationPeriodDetails FROM Hrm_EmployeeEvaluationSubType WHERE [EvaluationTypeID] = '"+ddlMullaynerName+"' AND [Status] = 0 ");

                return resultList.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<AssesmentGrid> ReadlstForAssesGrid(string mullaynNam, string mullaynKal)
        {
            try
            {
                var resultList = HrmContext.Database.SqlQuery<AssesmentGrid>("SELECT EvaluationCriteriaID, EvaluationCriteriaNameInBangla,EvaluationCriteriaWeight ,'' as FalseFieldForERPT  FROM Hrm_EmployeeEvaluationCriteria WHERE EvaluationTypeID = '" + mullaynNam + "' AND EvaluationSubTypeID = '" + mullaynKal + "' AND [Status] = 0");

                return resultList.ToList(); 
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public List<EmployeeInfoForAssEntry> ReadLocationWiseEmployeeForAssEntry(string locationCode, string EmpID)
        {
            try
            {
                var resultList = HrmContext.Database.SqlQuery<EmployeeInfoForAssEntry>("EXEC [SP_HrmEmployeeWiseEvaluationEntry] 'VIEW_EMPLOYEE_WISE_DETAILS', '" + locationCode + "', '" + EmpID + "', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL");

                return resultList.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public List<ERPValueForAssessment> GetDataForERPT(string viewScore, string locationCode, string EmployeeID, string mullayenerName, string ddlMullaynerKal, string valOfthis)
        {
           // return HRMSService.GetDataForERPT(viewScore, locationCode, EmployeeID, mullayenerName, ddlMullaynerKal);

            try
            {
                var resultList = HrmContext.Database.SqlQuery<ERPValueForAssessment>("EXEC [SP_HrmEmployeeNEvaluationCriteriaWiseScore] '" + viewScore + "', '" + locationCode + "', '" + EmployeeID + "', '" + mullayenerName + "','"+ddlMullaynerKal+"','"+valOfthis+"'");

                return resultList.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        public List<AssesmentCriteriaNScore> ReadAssesmentCriteriaNScore(string optionID, string criteriaValue)
        {
            try
            {
                var resultList = HrmContext.Database.SqlQuery<AssesmentCriteriaNScore>("SELECT EvaluationCriteriaOptionID,EvaluationCriteriaScore FROM [RASolarERP].[dbo].[Hrm_EmployeeEvaluationCriteriaOptionDetails] where EvaluationCriteriaID = '" + optionID + "' and EvaluationCriteriaOptionID = '" + criteriaValue + "'");

                return resultList.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public string SaveTrainingAssesmentEntry(string prmLocationCode, string ddlIdNo, string empEvType, string empEvKal, string trainingDate, string assesmentDate, string prmIsAssessmentCompleted, string txtUnitManagerMontobbo, string chkbxVal, string aprovedDate, string crArrID, string crArrScore, string weight, string pPurboPNo, string prmIsAssessmentSelectedByERPData, string userLogin, string insertData, string rowWiseRemarks)
        {
            string successMessage = string.Empty;

            using (HrmContext)
            {

                using (TransactionScope tx = new TransactionScope())
                {

                    try
                    {


                        ((System.Data.Entity.Infrastructure.IObjectContextAdapter)HrmContext).ObjectContext.CommandTimeout = 3080;



                        HrmContext.Database.ExecuteSqlCommand("EXEC [SP_HrmEmployeeWiseEvaluationEntry] 'INSERT_EVALUATION_DATA_MASTER', '" + prmLocationCode + "', '" + ddlIdNo + "', '" + empEvType + "', '" + empEvKal + "', 1, '" + trainingDate + "', '" + assesmentDate + "', '" + prmIsAssessmentCompleted + "', '" + txtUnitManagerMontobbo + "', '" + chkbxVal + "', '" + aprovedDate + "', NULL, NULL, NULL, NULL, NULL, NULL,NULL, NULL,  NULL, '" + userLogin + "'");
                       
                        
                        string[] assesmentOptionCriteriaArryID = crArrID.Split('_');
                        string[] assesmentOptionCriteriaArryScore = crArrScore.Split('_');
                        string[] weightArr = weight.Split('_');
                        string[] pPurboPNoArr = pPurboPNo.Split('_');
                        string[] rowWiseRemark = rowWiseRemarks.Split('_');
                        string[] insertERPData = insertData.Split('_'); 

                        for (int k = 0; k < assesmentOptionCriteriaArryID.Length; k++ )
                        {
                            int rowsNo = k + 1; 
                            string criteriaValueID = assesmentOptionCriteriaArryID[k].ToString();
                            string criteriaValueScore = assesmentOptionCriteriaArryScore[k].ToString();
                            string weightArrRowVal = weightArr[k].ToString();
                            string pPurboPNoArrRowVal = pPurboPNoArr[k].ToString();
                            string rowWiseRemarksVal = rowWiseRemark[k].ToString();
                            string insertERPDataVal = insertERPData[k].ToString()==""?"0":insertERPData[k].ToString();
                            if (criteriaValueID!="")
                            {

                                HrmContext.Database.ExecuteSqlCommand("EXEC [SP_HrmEmployeeWiseEvaluationEntry]  'INSERT_EVALUATION_DATA_DETAILS' , '" + prmLocationCode + "', '" + ddlIdNo + "', '" + empEvType + "','" + empEvKal + "',1, NULL, NULL, NULL, NULL, NULL, NULL,'" + rowsNo + "', '" + criteriaValueID + "', '" + criteriaValueScore + "', '" + weightArrRowVal + "', '" + pPurboPNoArrRowVal + "', 0, '"+insertERPDataVal+"','" + rowWiseRemarksVal + "', null,'" + userLogin + "'");
                        
                            }
                        }
                        
                        HrmContext.SaveChanges();

                        tx.Complete();

                        return successMessage;
                    }

                    catch (Exception ex)
                    {
                        tx.Dispose();
                        throw;
                    }
                }
            }
        }

        public List<EmployeeDetailsInfo> ReadLocationWiseEmployee(string locationCode)
        {
            var v = from emi in HrmContext.Hrm_EmployeeInfo
                    join lwe in HrmContext.Hrm_LocationWiseEmployee
                    on emi.EmployeeID equals lwe.EmployeeID
                    join harcdsgn in HrmContext.Hrm_HierarchicalDesignation
                    on emi.LastDesignation equals harcdsgn.HDesignationID
                    join oprl in HrmContext.Hrm_OperationalRole
                    on emi.LastOperationalRole equals oprl.OperationalRoleID
                    into oprlLeftJoin
                    from oprle in oprlLeftJoin.DefaultIfEmpty()
                    where lwe.LocationCode == locationCode && emi.Status == Helper.Active && lwe.Status == Helper.Active
                    select new EmployeeDetailsInfo
                    {
                        EmployeeID = emi.EmployeeID,
                        EmployeeName = emi.EmployeeName + " [ " + emi.EmployeeID + " ] ",
                        DateOfJoining = emi.JoiningDate,
                        Department = emi.Hrm_DepartmentInfo.DepartmentName,
                        PresentLocation = emi.Common_LocationInfo.LocationName,
                        PresentLocationId = emi.LastLocationCode,
                        Designation = harcdsgn.HDesignationName,
                        OperationalRole = oprle.OperationalRoleName
                    };

            return v.ToList();
        }

        public List<EmployeeDetailsInfo> ReadLocationWiseEmployeeWithTransferStatus(string locationCode)
        {
            var v = from emi in HrmContext.Hrm_EmployeeInfo
                    join lwe in HrmContext.Hrm_LocationWiseEmployee
                    on emi.EmployeeID equals lwe.EmployeeID
                    join harcdsgn in HrmContext.Hrm_HierarchicalDesignation
                    on emi.LastDesignation equals harcdsgn.HDesignationID
                    join oprl in HrmContext.Hrm_OperationalRole
                    on emi.LastOperationalRole equals oprl.OperationalRoleID
                    into oprlLeftJoin
                    from oprle in oprlLeftJoin.DefaultIfEmpty()
                    join et in HrmContext.Hrm_EmployeeTransfer
                    on emi.EmployeeID equals et.EmployeeID
                    into etLeftJoin
                    from eti in etLeftJoin.DefaultIfEmpty()
                    where lwe.LocationCode == locationCode && emi.Status == Helper.Active && lwe.Status == Helper.Active
                    select new EmployeeDetailsInfo
                    {
                        EmployeeID = emi.EmployeeID,
                        EmployeeName = emi.EmployeeName + " [ " + emi.EmployeeID + " ] ",
                        DateOfJoining = emi.JoiningDate,
                        Department = emi.Hrm_DepartmentInfo.DepartmentName,
                        PresentLocation = emi.Common_LocationInfo.LocationName,
                        PresentLocationId = emi.LastLocationCode,
                        Designation = harcdsgn.HDesignationName,
                        OperationalRole = oprle.OperationalRoleName,
                        IsTransferOrderInProgress = eti.Status == Helper.Active ? true : false,
                        TransferSeqNo = eti.TransferSeqNo
                    };

            var vv = v.ToList().GroupBy(p => p.EmployeeID).Select(s=>s.First()).ToList();

            return vv;
        }

        public List<EmployeeDetailsInfo> ReadLocationWiseEmployeeWithUMAcountManager(string locationCode)
        {
            string locationWiseEmployeeWithUMQuery = "EXEC SP_LocationWiseEmployee '" + locationCode + "'";
            var v = HrmContext.Database.SqlQuery<EmployeeDetailsInfo>(locationWiseEmployeeWithUMQuery);

            return v.ToList();
        }

        public List<EmployeeDetailsInfo> ReadEmployeeSearchDetails(string employeeID, string employeeName, string designation, string locationCode, byte employeeStatus)
        {
            string searchCondition = string.Empty;

            if (!string.IsNullOrEmpty(employeeID))
            {
                searchCondition += "WHERE EmployeeID LIKE '%" + employeeID + "%' ";
            }

            if (!string.IsNullOrEmpty(employeeName))
            {
                if (!string.IsNullOrEmpty(searchCondition))
                    searchCondition += "AND EmployeeName LIKE '%" + employeeName + "%' ";
                else
                    searchCondition += "WHERE EmployeeName LIKE '%" + employeeName + "%' ";
            }

            if (!string.IsNullOrEmpty(designation))
            {
                if (!string.IsNullOrEmpty(searchCondition))
                    searchCondition += "AND LastDesignation = '" + designation + "' ";
                else
                    searchCondition += "WHERE LastDesignation = '" + designation + "' ";
            }

            if (!string.IsNullOrEmpty(locationCode))
            {
                if (!string.IsNullOrEmpty(searchCondition))
                    searchCondition += "AND LastLocationCode = '" + locationCode + "' ";
                else
                    searchCondition += "WHERE LastLocationCode = '" + locationCode + "'";
            }

            if (!string.IsNullOrEmpty(searchCondition))
            {
                searchCondition += " AND ei.Status = " + employeeStatus;
            }
            else
            {
                searchCondition += " WHERE ei.Status = " + employeeStatus;
            }

            string employeeQuery = "SELECT empinfo.EmployeeID, empinfo.EmployeeName, " +
                                    "hdsgn.HDesignationName Designation, depi.DepartmentName Department, " +
                                    "zone.LocationName + ' [' + zone.LocationCode +']' PresentZone, " +
                                    "region.LocationName + ' [' + region.LocationCode +']' PresentRegion, " +
                                    "linfo.LocationName + ' [' + empinfo.LastLocationCode +']' PresentLocation " +
                                    "FROM " +
                                    "( " +
                                    "	SELECT ei.EmployeeID, ei.EmployeeName, " +
                                    "	ei.LastDesignation, ei.LastDepartment, ei.LastLocationCode " +
                                    "	FROM Hrm_EmployeeInfo ei " +
                                    searchCondition +

                                    ")empinfo " +
                                    "INNER JOIN Hrm_HierarchicalDesignation hdsgn " +
                                    "ON  empinfo.LastDesignation = hdsgn.HDesignationID " +
                                    "INNER JOIN Hrm_DepartmentInfo depi " +
                                    "ON empinfo.LastDepartment = depi.DepartmentID " +
                                    "INNER JOIN Common_LocationInfo linfo " +
                                    "ON empinfo.LastLocationCode = linfo.LocationCode " +
                                    "LEFT OUTER JOIN (SELECT * FROM Common_LocationInfo WHERE LocationType = 6) region " +
                                    "ON linfo.ParentLocation = region.LocationCode " +
                                    "LEFT OUTER JOIN (SELECT * FROM Common_LocationInfo WHERE LocationType = 7 ) zone " +
                                    "ON region.ParentLocation = zone.LocationCode";

            var v = HrmContext.Database.SqlQuery<EmployeeDetailsInfo>(employeeQuery);

            return v.ToList();
        }

        public List<Hrm_HierarchicalDesignation> ReadEmployeeDesignation()
        {
            return HrmContext.Hrm_HierarchicalDesignation.Where(s => s.Status == Helper.Active).OrderBy(i => i.SortingOrder).ToList();
        }

        public byte ReadTransferSequenceNumber(string employeeId)
        {
            byte employeeTransferSequenceNumber = 0;

            try
            {
                // employeeTransferSequenceNumber = Convert.ToByte(HrmContext.Hrm_EmployeeTransfer.Where(e => e.EmployeeID == employeeId && e.Status == Helper.Active).Count()); comment by mahin
                employeeTransferSequenceNumber = Convert.ToByte(HrmContext.Hrm_EmployeeTransfer.Where(e => e.EmployeeID == employeeId).Count());
                employeeTransferSequenceNumber += 1;
            }
            catch (Exception ex)
            {
                throw;
            }

            return employeeTransferSequenceNumber;
        }

        public List<Hrm_OperationalRole> ReadOperationalRole()
        {
            return HrmContext.Hrm_OperationalRole.Where(r => r.Status == Helper.Active).ToList();
        }

        public bool CheckEmployeeTransferOfficeOrderNo(string officeOrderNo)
        {
            Hrm_EmployeeTransferAdditionalInfo objEmployeeTransferAdditionalInfo = new Hrm_EmployeeTransferAdditionalInfo();
            objEmployeeTransferAdditionalInfo = HrmContext.Hrm_EmployeeTransferAdditionalInfo.Where(r => r.OfficeOrderNo == officeOrderNo).FirstOrDefault();
            if (objEmployeeTransferAdditionalInfo == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<BankAdviceForSalaryReport> ReadBankAdviceForSalaryReport(string forMonth, string reportType, string generateType, string locationCode, string bankAccount)
        {
            string strSalaryAdviseQuery = string.Empty;

            SqlParameter[] storeParam = new SqlParameter[7];
            storeParam[0] = new SqlParameter("prmOption", reportType);
            storeParam[1] = new SqlParameter("prmGenerateOrView", "ViewBankAdvice");

            if (locationCode == string.Empty)
                storeParam[2] = new SqlParameter("prmSalaryLocationCode", DBNull.Value);
            else
                storeParam[2] = new SqlParameter("prmSalaryLocationCode", locationCode);

            storeParam[3] = new SqlParameter("prmYearMonth", forMonth);
            storeParam[4] = new SqlParameter("prmSalarySlipNo", 1);
            storeParam[5] = new SqlParameter("prmStartAdviceLetterReferenceSeqNo", 1);

            if (string.IsNullOrEmpty(bankAccount))
                storeParam[6] = new SqlParameter("prmSalaryDisbursementBankID", DBNull.Value);
            else
                storeParam[6] = new SqlParameter("prmSalaryDisbursementBankID", bankAccount);

            var resultList = HrmContext.Database.SqlQuery<BankAdviceForSalaryReport>
                ("Exec SP_HrmGenerateNGetBankAdviceToDisburseSalary @prmOption, @prmGenerateOrView, @prmSalaryLocationCode, @prmYearMonth,@prmSalarySlipNo,@prmStartAdviceLetterReferenceSeqNo, @prmSalaryDisbursementBankID", storeParam);

            try
            {
                return resultList.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public List<BankAdviceForSalaryLatterReport> ReadBankAdviceForSalaryLatterReport(string cd_Customer)
        {
            //string strSalaryAdviseQuery = string.Empty;

            //SqlParameter[] storeParam = new SqlParameter[1];
            //storeParam[0] = new SqlParameter("prmSMSBatchTag", cd_Customer);
            //storeParam[1] = new SqlParameter("prmGenerateOrView", "ViewBankAdvice");

            //if (locationCode == string.Empty)
            //    storeParam[2] = new SqlParameter("prmSalaryLocationCode", DBNull.Value);
            //else
            //    storeParam[2] = new SqlParameter("prmSalaryLocationCode", locationCode);

            //storeParam[3] = new SqlParameter("prmYearMonth", forMonth);
            //storeParam[4] = new SqlParameter("prmSalarySlipNo", 1);
            //storeParam[5] = new SqlParameter("prmStartAdviceLetterReferenceSeqNo", 1);

            //if (string.IsNullOrEmpty(bankAccount))
            //    storeParam[6] = new SqlParameter("prmSalaryDisbursementBankID", DBNull.Value);
            //else
            //    storeParam[6] = new SqlParameter("prmSalaryDisbursementBankID", bankAccount);

            ((System.Data.Entity.Infrastructure.IObjectContextAdapter)HrmContext).ObjectContext.CommandTimeout = 3800;

            var resultList = HrmContext.Database.SqlQuery<BankAdviceForSalaryLatterReport>
                ("Exec REP_SMSGateway_GetCustomersToSendSMS '" + cd_Customer +"'");

            try
            {
                return resultList.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public List<LatterToChairmanReport> ReadLatterToChairmanReport(string cd_Customer)
        {
            //string strSalaryAdviseQuery = string.Empty;

            //SqlParameter[] storeParam = new SqlParameter[1];
            //storeParam[0] = new SqlParameter("prmSMSBatchTag", cd_Customer);
            //storeParam[1] = new SqlParameter("prmGenerateOrView", "ViewBankAdvice");

            //if (locationCode == string.Empty)
            //    storeParam[2] = new SqlParameter("prmSalaryLocationCode", DBNull.Value);
            //else
            //    storeParam[2] = new SqlParameter("prmSalaryLocationCode", locationCode);

            //storeParam[3] = new SqlParameter("prmYearMonth", forMonth);
            //storeParam[4] = new SqlParameter("prmSalarySlipNo", 1);
            //storeParam[5] = new SqlParameter("prmStartAdviceLetterReferenceSeqNo", 1);

            //if (string.IsNullOrEmpty(bankAccount))
            //    storeParam[6] = new SqlParameter("prmSalaryDisbursementBankID", DBNull.Value);
            //else
            //    storeParam[6] = new SqlParameter("prmSalaryDisbursementBankID", bankAccount);

            var resultList = HrmContext.Database.SqlQuery<LatterToChairmanReport>
                ("Exec REP_SMSGateway_GetCustomersToSendSMS '" + cd_Customer +"'");

            try
            {
                return resultList.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<LatterToUpoZelaChairmanReport> ReadLatterToUpoZelaChairmanReport(string cd_Customer)
        {
            //string strSalaryAdviseQuery = string.Empty;

            //SqlParameter[] storeParam = new SqlParameter[1];
            //storeParam[0] = new SqlParameter("prmSMSBatchTag", cd_Customer);
            //storeParam[1] = new SqlParameter("prmGenerateOrView", "ViewBankAdvice");

            //if (locationCode == string.Empty)
            //    storeParam[2] = new SqlParameter("prmSalaryLocationCode", DBNull.Value);
            //else
            //    storeParam[2] = new SqlParameter("prmSalaryLocationCode", locationCode);

            //storeParam[3] = new SqlParameter("prmYearMonth", forMonth);
            //storeParam[4] = new SqlParameter("prmSalarySlipNo", 1);
            //storeParam[5] = new SqlParameter("prmStartAdviceLetterReferenceSeqNo", 1);

            //if (string.IsNullOrEmpty(bankAccount))
            //    storeParam[6] = new SqlParameter("prmSalaryDisbursementBankID", DBNull.Value);
            //else
            //    storeParam[6] = new SqlParameter("prmSalaryDisbursementBankID", bankAccount);

            var resultList = HrmContext.Database.SqlQuery<LatterToUpoZelaChairmanReport>
                ("Exec REP_SMSGateway_GetCustomersToSendSMS '" + cd_Customer + "'");

            try
            {
                return resultList.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void GenerateSalaryAdviceForBank(string forMonth, string reportType, string generateType, string startLetterSequenceNo, string locationCode, string bankAccount)
        {
            if (locationCode == "")
            {
                locationCode = Helper.DBNullValue;
            }
            else
            {
                locationCode = "'" + locationCode + "'";
            }

            bankAccount = Helper.DBNullValue;

            using (var Ts = new TransactionScope())
            {
                string generateSalaryAdvice = string.Empty;

                try
                {
                    generateSalaryAdvice = "EXEC SP_HrmGenerateNGetBankAdviceToDisburseSalary '" + reportType + "', 'GenerateBankAdvice', " + locationCode + ", '" +
                                                forMonth + "', " + 1 + ", " + startLetterSequenceNo + ", " + bankAccount;

                    HrmContext.Database.ExecuteSqlCommand(generateSalaryAdvice);

                    Ts.Complete();
                }
                catch (Exception ex)
                {
                    Ts.Dispose();
                    throw;
                }
            }
        }

        public ArrayList AuditorEmployeeList()
        {
            List<Hrm_EmployeeInfo> employeeList = new List<Hrm_EmployeeInfo>();
            employeeList = HrmContext.Hrm_EmployeeInfo.Where(e => (e.LastOperationalRole == Helper.UnitAudit || e.LastOperationalRole == Helper.ZonalAuditor || e.LastOperationalRole == Helper.RegionalAuditor || e.LastOperationalRole == Helper.HeadOfficeAuditor) && e.Status == 0).ToList();

            ArrayList arlst = new ArrayList();
            if (employeeList != null)
            {
                foreach (Hrm_EmployeeInfo ss in employeeList)
                    arlst.Add(new { EmployeeID = ss.EmployeeID, EmployeeName = ss.EmployeeName + "  " + "[" + ss.EmployeeID + "]" });
            }

            return arlst;
        }

        public List<EmployeeDetailsInfo> ReadLocationWiseEmployeeForHeadOffice(string locationCode)
        {
            var v = from emi in HrmContext.Hrm_EmployeeInfo
                    join lws in HrmContext.Sal_LocationWiseSalespersonForHOnZO
                    on emi.EmployeeID equals lws.SalespersonCode
                    where lws.LocationCode == locationCode && emi.Status == Helper.Active
                    select new EmployeeDetailsInfo
                    {
                        EmployeeID = emi.EmployeeID,
                        EmployeeName = emi.EmployeeName,
                    };


            return v.ToList();
        }

        public ArrayList BankListForSalaryAdvice(string locationCode)
        {
            ArrayList arr = new ArrayList();

            // string ss = "";

            return arr;
        }

        public ArrayList BankAdviceLcoation(string locationCode)
        {
            ArrayList arr = new ArrayList();

            //string ss = "";

            return arr;
        }

        public string ReadAuditSeqNumberAfterCheckFinishedDate(string locationCode)
        {
            string auditSeqNumber = string.Empty;
            Aud_AuditingMaster objAuditingMaster = HrmContext.Aud_AuditingMaster.Where(a => a.LocationCode == locationCode && a.AuditFinishDate == null).FirstOrDefault();

            if (objAuditingMaster != null)
            {
                auditSeqNumber = objAuditingMaster.AuditSeqNo;
            }

            return auditSeqNumber;
        }

        public AuditingMaster ReadAuditingMasterDetails(string locationCode, string auditSeqNumber)
        {
            AuditingMaster objAuditingMaster = new AuditingMaster();

            objAuditingMaster = (from am in HrmContext.Aud_AuditingMaster
                                 where am.LocationCode == locationCode && am.AuditSeqNo == auditSeqNumber
                                 select new AuditingMaster
                                 {
                                     LocationCode = am.LocationCode,
                                     AuditSeqNo = am.AuditSeqNo,
                                     AuditStartDate = am.AuditStartDate,
                                     AuditFinishDate = am.AuditFinishDate,
                                     AuditPeriodFromDate = am.AuditPeriodFromDate,
                                     AuditPeriodToDate = am.AuditPeriodToDate,
                                     Remarks = am.Remarks,
                                     CreatedBy = am.CreatedBy,
                                     CreatedOn = am.CreatedOn,
                                     ModifiedBy = am.ModifiedBy,
                                     ModifiedOn = am.ModifiedOn,
                                     Status = am.Status

                                 }).FirstOrDefault();


            List<AuditingDetailsForAuditors> lstAuditingDetailsForAuditors = new List<AuditingDetailsForAuditors>();
            lstAuditingDetailsForAuditors = (from ad in HrmContext.Aud_AuditingDetailsForAuditors
                                             join em in HrmContext.Hrm_EmployeeInfo
                                             on ad.AuditorsEmployeeID equals em.EmployeeID
                                             where ad.LocationCode == locationCode && ad.AuditSeqNo == auditSeqNumber
                                             select new AuditingDetailsForAuditors
                                             {
                                                 AuditorsEmployeeID = ad.AuditorsEmployeeID,
                                                 EmployeeName = em.EmployeeName

                                             }).ToList();

            objAuditingMaster.AuditingDetails = lstAuditingDetailsForAuditors;

            return objAuditingMaster;
        }

        public DateTime? AuditPeriodFromDate(string locationCode)
        {
            DateTime uditPeriodFromDate;
            Aud_AuditingMaster objAuditingMaster = HrmContext.Aud_AuditingMaster.Where(a => a.LocationCode == locationCode).OrderByDescending(o => o.AuditPeriodToDate).FirstOrDefault();

            if (objAuditingMaster != null)
            {
                uditPeriodFromDate = objAuditingMaster.AuditPeriodToDate.AddDays(1);
                return uditPeriodFromDate;
            }
            else
                return null;
        }

        public bool AuditMasterSetupAlreadyExistOrNot(string locationCode, string auditSeqNumber)
        {
            bool isAuditSetupExists = false;

            Aud_AuditingMaster objAuditingMaster = new Aud_AuditingMaster();
            objAuditingMaster = HrmContext.Aud_AuditingMaster.Where(am => am.LocationCode == locationCode && am.AuditSeqNo == auditSeqNumber).FirstOrDefault();

            if (objAuditingMaster != null)
            {
                isAuditSetupExists = true;
            }

            return isAuditSetupExists;
        }

        public EmployeeDetails EmployeeDetailsInfo(string employeeId)
        {
            var v = from empinf in HrmContext.Hrm_EmployeeInfo
                    where empinf.EmployeeID == employeeId &&
                    empinf.Status == Helper.Active
                    select new EmployeeDetails
                    {
                        EmployeeID = empinf.EmployeeID,
                        EmployeeName = empinf.EmployeeName,
                        EmployeeFathersName = empinf.EmployeeFathersName,
                        EmployeeMothersName = empinf.EmployeeMothersName,
                        DateOfBirth = empinf.DateOfBirth,
                        Gender = empinf.Gender,
                        PlaceOfBirthId = empinf.PlaceOfBirth,
                        MaritalStatus = empinf.MaritalStatus,
                        Nationality = empinf.Nationality,
                        Religion = empinf.Religion,
                        HighestEducation = empinf.HighestEducation,
                        MajorSubject = empinf.MajorSubject,
                        BloodGroup = empinf.BloodGroup,
                        NationalIDCard = empinf.NationalIDCard,
                        EmailID = empinf.EmailID,

                        PresentDistrictCode = empinf.PresentDistrictCode,
                        PresentUpazillaCode = empinf.PresentUpazillaCode,
                        PresentPoliceStation = empinf.PresentPoliceStation,
                        PresentPostOffice = empinf.PresentPostOffice,
                        PresentStreetOrVillage = empinf.PresentStreetOrVillage,
                        PresentHouseNo = empinf.PresentHouseNo,
                        PresentMobileNo = empinf.PresentMobileNo,
                        PresentPhone = empinf.PresentPhone,

                        PermanentDistrictCode = empinf.PermanentDistrictCode,
                        PermanentUpazillaCode = empinf.PermanentUpazillaCode,
                        PermanentPoliceStation = empinf.PermanentPoliceStation,
                        PermanentPostOffice = empinf.PermanentPostOffice,
                        PermanentStreetVillage = empinf.PermanentStreetVillage,
                        PermanentHouseNo = empinf.PermanentHouseNo,
                        PermanentMobileNo = empinf.PermanentMobileNo,
                        PermanentPhone = empinf.PermanentPhone,

                        JoiningDate = empinf.JoiningDate,
                        LastDesignation = empinf.LastDesignation,
                        LastOperationalRole = empinf.LastOperationalRole,
                        LastDepartment = empinf.LastDepartment,
                        LastSection = empinf.LastSection,
                        LastEmploymentType = empinf.LastEmploymentType,
                        LastSalaryStructureSeqNo = empinf.LastSalaryStructureSeqNo,
                        ModeOfSalaryPayment = empinf.ModeOfSalaryPayment,
                        EmployeeGrade = empinf.Hrm_HierarchicalDesignation.EmployeeGrade,
                        ProbationPeriodInMonth = empinf.ProbationPeriodInMonth,
                        ContractualPeriodInMonth = empinf.ContractualPeriodInMonth,
                        ConfirmationDate = empinf.ConfirmationDate,
                        LastLocationCode = empinf.LastLocationCode,
                        ReleaseDate = empinf.ReleaseDate,
                        Status = empinf.Status,
                        StatusChangedDate = empinf.StatusChangedDate,

                        GenderDescription = (empinf.Gender == "M" ? "Male" : "Female"),
                        PlaceOfBirth = empinf.Common_DistrictInfo.DIST_NAME,
                        MaritalStatusDescription = empinf.Hrm_MaritalStatusInfo.MaritalStatusDescription,
                        ReligionName = empinf.Hrm_ReligionInfo.ReligionName,
                        EducationName = empinf.Hrm_EducationInfo.EducationName,
                        EducationLevel = empinf.Hrm_EducationInfo.EducationLevel,
                        MajorSubjectName = empinf.Hrm_MajorSubject.MajorSubjectName,
                        BloodGroupDescription = empinf.Hrm_BloodGroupInfo.BloodGroupDescription,

                        PresentDistrict = empinf.Common_DistrictInfo1.DIST_NAME,
                        PresentUpazilla = empinf.Common_UpazillaInfo.UPAZ_NAME,

                        PermanentDistrict = empinf.Common_DistrictInfo2.DIST_NAME,
                        PermanentUpazilla = empinf.Common_UpazillaInfo1.UPAZ_NAME,

                        HDesignationName = empinf.Hrm_HierarchicalDesignation.HDesignationName,
                        OperationalRoleName = empinf.Hrm_HierarchicalDesignationVsOperationalRole.Hrm_OperationalRole.OperationalRoleName,
                        DepartmentName = empinf.Hrm_DepartmentInfo.DepartmentName,
                        SectionName = empinf.Hrm_SubDepartmentOrSection.SectionName,
                        EmploymentTypeDescription = empinf.Hrm_EmploymentTypeInfo.EmploymentTypeDescription,
                        LocationName = empinf.Common_LocationInfo.LocationName,

                        PassportNo = empinf.PassportNo,
                        PassportExpiryDate = empinf.PassportExpiryDate,
                        EmployeeSpouseName = empinf.EmployeeSpouseName,
                        SpouseBloodGroup = empinf.SpouseBloodGroup,
                        DrivingLicenseNo = empinf.DrivingLicenseNo

                    };

            return v.FirstOrDefault();
        }

        public List<DistrictInfo> ReadDistrict()
        {
            var district = from dst in HrmContext.Common_DistrictInfo
                           select new DistrictInfo
                           {
                               DistrictCode = dst.DIST_CODE,
                               DistrictName = dst.DIST_NAME
                           };

            return district.ToList();
        }

        public List<UpazillaInfo> ReadUpazillaInfo(string districtCode)
        {
            var upazilla = from upzla in HrmContext.Common_UpazillaInfo
                           where upzla.DIST_CODE == districtCode && upzla.Status == Helper.Active
                           select new UpazillaInfo
                           {
                               UpazillaCode = upzla.UPAZ_CODE,
                               UpazillaName = upzla.UPAZ_NAME
                           };

            return upazilla.ToList();
        }

        public List<BloodGroupInfo> ReadBloodGroupInfo()
        {
            var bloodGroup = from bldgrp in HrmContext.Hrm_BloodGroupInfo
                             orderby bldgrp.SortingOrder
                             select new BloodGroupInfo
                             {
                                 BloodGroupID = bldgrp.BloodGroupID,
                                 BloodGroupDescription = bldgrp.BloodGroupDescription
                             };

            return bloodGroup.ToList();
        }

        public List<EducationInfo> ReadEducationInfo()
        {
            var education = from edu in HrmContext.Hrm_EducationInfo
                            orderby edu.SortingOrder
                            select new EducationInfo
                             {
                                 EducationID = edu.EducationID,
                                 EducationName = edu.EducationName,
                                 EducationLevel = edu.EducationLevel
                             };

            return education.ToList();
        }

        public List<ReligionInfo> ReadReligionInfo()
        {
            var religion = from relgn in HrmContext.Hrm_ReligionInfo
                           orderby relgn.SortingOrder
                           select new ReligionInfo
                           {
                               ReligionID = relgn.ReligionID,
                               ReligionName = relgn.ReligionName
                           };

            return religion.ToList();
        }

        public List<MaritalStatusInfo> ReadMaritalStatusInfo()
        {
            var maritalStatus = from marital in HrmContext.Hrm_MaritalStatusInfo
                                orderby marital.SortingOrder
                                select new MaritalStatusInfo
                                {
                                    MaritalStatusID = marital.MaritalStatusID,
                                    MaritalStatusDescription = marital.MaritalStatusDescription
                                };

            return maritalStatus.ToList();
        }

        public List<MajorSubject> ReadMajorSubject()
        {
            var majorsubject = from majorsub in HrmContext.Hrm_MajorSubject
                               orderby majorsub.SortingOrder
                               select new MajorSubject
                               {
                                   MajorSubjectID = majorsub.MajorSubjectID,
                                   MajorSubjectName = majorsub.MajorSubjectName
                               };

            return majorsubject.ToList();
        }

        public List<EmploymentTypeInfo> ReadEmploymentTypeInfo()
        {
            var employmentInfo = from employment in HrmContext.Hrm_EmploymentTypeInfo
                                 orderby employment.SortingOrder
                                 select new EmploymentTypeInfo
                                 {
                                     EmploymentTypeID = employment.EmploymentTypeID,
                                     EmploymentTypeDescription = employment.EmploymentTypeDescription
                                 };

            return employmentInfo.ToList();
        }

        public List<HierarchicalDesignation> ReadHierarchicalDesignation()
        {
            var designationInfo = from designation in HrmContext.Hrm_HierarchicalDesignation
                                  orderby designation.SortingOrder
                                  select new HierarchicalDesignation
                                  {
                                      HDesignationID = designation.HDesignationID,
                                      HDesignationName = designation.HDesignationName,
                                      EmployeeGrade = designation.EmployeeGrade != null ? designation.EmployeeGrade : Helper.NotApplicable,
                                  };

            return designationInfo.ToList();
        }

        public List<JobGradeInfo> ReadJobGradeInfo()
        {
            var designationInfo = (from designation in HrmContext.Hrm_HierarchicalDesignation
                                   orderby designation.SortingOrder
                                   select new JobGradeInfo
                                   {
                                       EmployeeGrade = designation.EmployeeGrade != null ? designation.EmployeeGrade : "NA",
                                       EmployeeGradeName = designation.EmployeeGrade != null ? designation.EmployeeGrade : "NA"

                                   }).Distinct();

            return designationInfo.ToList();
        }

        public List<DepartmentInfo> ReadDepartmentInfo()
        {
            var designationInfo = from dept in HrmContext.Hrm_DepartmentInfo
                                  orderby dept.SortingOrder
                                  select new DepartmentInfo
                                  {
                                      DepartmentID = dept.DepartmentID,
                                      DepartmentName = dept.DepartmentName
                                  };

            return designationInfo.ToList();
        }

        public List<OperationalRole> ReadOperationalRole(string designationid)
        {
            var designationInfo = from oprnrol in HrmContext.Hrm_OperationalRole
                                  join desigvoprol in HrmContext.Hrm_HierarchicalDesignationVsOperationalRole
                                  on oprnrol.OperationalRoleID equals desigvoprol.OperationalRoleID
                                  where desigvoprol.HDesignationID == designationid
                                  orderby oprnrol.SortingOrder
                                  select new OperationalRole
                                  {
                                      OperationalRoleID = oprnrol.OperationalRoleID,
                                      OperationalRoleName = oprnrol.OperationalRoleName
                                  };

            return designationInfo.ToList();
        }

        public string EmployeeIdMax()
        {
            string employeeSql = "SELECT TOP(1) ISNULL(emp.EmployeeID,'') EmployeeID FROM " +
                                  "Hrm_EmployeeInfo emp  " +
                                  "ORDER BY CONVERT(INT,SUBSTRING(emp.EmployeeID,4, LEN(emp.EmployeeID))) DESC ";

            var employeeMaxId = HrmContext.Database.SqlQuery<string>(employeeSql);

            string employeeId = string.Empty;

            if (employeeMaxId != null)
                employeeId = employeeMaxId.FirstOrDefault();

            return employeeId;
        }

        public byte EmployeeWiseSalaryStructureSeqenceNumberMax(string employeeId)
        {
            var v = HrmContext.Hrm_EmployeeWiseSalaryStructureMaster.Where(d => d.EmployeeID == employeeId).Max(m => (byte?)m.SalaryStructureSeqNo);

            byte salarySequenceNumber = 0;

            if (v != null)
            {
                salarySequenceNumber = Convert.ToByte(v);
            }

            return salarySequenceNumber;
        }

        public bool EmployeeExistOrNot(string employeeId)
        {
            bool existOrNot = true;

            var v = HrmContext.Hrm_EmployeeInfo.Where(e => e.EmployeeID == employeeId && e.Status == Helper.Active).AsNoTracking().FirstOrDefault();

            if (v == null)
                existOrNot = false;


            return existOrNot;
        }

        public List<Hrm_HierarchicalDesignationNEmploymentTypeWisePayBandStructure> DesignationNEmploymentTypeWisePayBandStructure(string designationId, string employmentTypeId)
        {
            List<Hrm_HierarchicalDesignationNEmploymentTypeWisePayBandStructure> lstDesignationNEmploymentTypeWisePayBandStructure = new List<Hrm_HierarchicalDesignationNEmploymentTypeWisePayBandStructure>();
            lstDesignationNEmploymentTypeWisePayBandStructure = HrmContext.Hrm_HierarchicalDesignationNEmploymentTypeWisePayBandStructure.Where(s => s.HDesignationID == designationId && s.EmploymentTypeID == employmentTypeId).ToList();

            return lstDesignationNEmploymentTypeWisePayBandStructure;
        }

        public List<TADADetails> ReadTADADetails(DateTime dateFrom, DateTime dateTo, string employeeID)
        {
            var v = from tada in HrmContext.Hrm_EmployeeNDateWiseTADAEntry
                    where (tada.DateOfTADA >= dateFrom && tada.DateOfTADA <= dateTo) &&
                    tada.EmployeeID == employeeID
                    select new TADADetails
                    {
                        LocationCode = tada.LocationCode,
                        YearMonth = tada.YearMonth,
                        EmployeeID = tada.EmployeeID,
                        DateOfTADA = tada.DateOfTADA,
                        TAAmount = tada.TAAmount,
                        ParticularsForTA = tada.ParticularsForTA,
                        DAAmount = tada.DAAmount,
                        ParticularsForDA = tada.ParticularsForDA,
                        DaysOfPendingEntry = tada.DaysOfPendingEntry,
                        TotalTADAAmount = tada.TAAmount + tada.DAAmount
                    };

            return v.ToList();
        }

        public List<TADADetails> ReviewTADAAmountNApproval(string locationCode, string reportType, string tADAAprrovalMonth)
        {
            try
            {
                SqlParameter[] storeParam = new SqlParameter[3];
                storeParam[0] = new SqlParameter("prmReportType", reportType);
                storeParam[1] = new SqlParameter("prmLocationCode", locationCode);
                storeParam[2] = new SqlParameter("prmYearMonth", tADAAprrovalMonth);

                ((System.Data.Entity.Infrastructure.IObjectContextAdapter)HrmContext).ObjectContext.CommandTimeout = 180;
                var resultList = HrmContext.Database.SqlQuery<TADADetails>("Exec SP_HrmGetTADAForReviewNApproval @prmReportType, @prmLocationCode, @prmYearMonth", storeParam);

                return resultList.ToList();
            }

            catch(Exception ex)
            { 
               throw;
            }
        }

        public EmployeeTransferInfo ReadEmployeeTransfer(string employeeId, byte transferSequenceNumber)
        {
            try
            {
                var vv = from aab in
                             (from aa in
                                  (from tr in HrmContext.Hrm_EmployeeTransfer
                                   join trad in HrmContext.Hrm_EmployeeTransferAdditionalInfo
                                   on new { tr.EmployeeID, tr.TransferSeqNo } equals new { trad.EmployeeID, trad.TransferSeqNo }
                                   where tr.EmployeeID == employeeId && tr.TransferSeqNo == transferSequenceNumber && tr.Status == Helper.Active
                                   select new
                                   {
                                       tr.EmployeeID,
                                       tr.TransferSeqNo,
                                       tr.TransferDate,
                                       tr.TransferFromLocation,
                                       tr.TransferToLocation,
                                       tr.NoteForTransfer,

                                       trad.RequisitionReceivedOn,
                                       trad.OfficeOrderDate,
                                       trad.OfficeOrderNo,
                                       trad.TransferReleaseDate,
                                       trad.ResponsibilityHandoverTo,
                                       trad.ResponsibilityTakenoverFrom,
                                       trad.ReasonForLateTransferJoining,
                                       trad.ReasonForLateTransferRelease,
                                       trad.ActualTransferJoiningDate,
                                       trad.ActualTransferReleaseDate,
                                       trad.NewOperationalRole
                                   })
                              join li in HrmContext.Common_LocationInfo
                              on aa.TransferToLocation equals li.LocationCode
                              join em in HrmContext.Hrm_EmployeeInfo 
                              on aa.ResponsibilityHandoverTo equals em.EmployeeID
                              into emLeftJoin_1 //Added By Md.Sultan Mahmud
                              from emleft1 in emLeftJoin_1.DefaultIfEmpty() //Added By Md.Sultan Mahmud
                              select new
                              {
                                  aa.EmployeeID,
                                  aa.TransferSeqNo,
                                  aa.TransferDate,
                                  aa.TransferFromLocation,
                                  aa.TransferToLocation,
                                  aa.NoteForTransfer,

                                  aa.RequisitionReceivedOn,
                                  aa.OfficeOrderDate,
                                  aa.OfficeOrderNo,
                                  aa.TransferReleaseDate,
                                  aa.ResponsibilityHandoverTo,
                                  aa.ResponsibilityTakenoverFrom,
                                  aa.ReasonForLateTransferJoining,
                                  aa.ReasonForLateTransferRelease,
                                  aa.ActualTransferJoiningDate,
                                  aa.ActualTransferReleaseDate,
                                  aa.NewOperationalRole,
                                  TransferToLocationName = li.LocationName,
                                  //ResponsibilityHandoverToName = em.EmployeeName  -----------Stopped By Md.Sultan Mahmud
                                  ResponsibilityHandoverToName = emleft1.EmployeeName  //Added By Md.Sultan Mahmud
                              })
                        
                         join em in HrmContext.Hrm_EmployeeInfo 
                         on aab.ResponsibilityTakenoverFrom equals em.EmployeeID
                         into emLeftJoin_2  //Added By Md.Sultan Mahmud
                         from emleft2 in emLeftJoin_2.DefaultIfEmpty()  //Added By Md.Sultan Mahmud
                         select new EmployeeTransferInfo
                         {
                             EmployeeID = aab.EmployeeID,
                             TransferSeqNo = aab.TransferSeqNo,
                             TransferDate = aab.TransferDate,
                             TransferFromLocation = aab.TransferFromLocation,
                             TransferToLocation = aab.TransferToLocation,
                             NoteForTransfer = aab.NoteForTransfer,

                             RequisitionReceivedOn = aab.RequisitionReceivedOn,
                             OfficeOrderDate = aab.OfficeOrderDate,
                             OfficeOrderNo = aab.OfficeOrderNo,
                             TransferReleaseDate = aab.TransferReleaseDate,
                             ResponsibilityHandoverTo = aab.ResponsibilityHandoverTo,
                             ResponsibilityTakenoverFrom = aab.ResponsibilityTakenoverFrom,
                             ReasonForLateTransferJoining = aab.ReasonForLateTransferJoining,
                             ReasonForLateTransferRelease = aab.ReasonForLateTransferRelease,
                             ActualTransferJoiningDate = aab.ActualTransferJoiningDate,
                             ActualTransferReleaseDate = aab.ActualTransferReleaseDate,
                             NewOperationalRole = aab.NewOperationalRole,
                             TransferToLocationName = aab.TransferToLocationName,
                             ResponsibilityHandoverToName = aab.ResponsibilityHandoverToName,
                             // ResponsibilityHandoverFromName = em.EmployeeName  -----------Stopped By Md.Sultan Mahmud
                             ResponsibilityHandoverFromName = emleft2.EmployeeName  //Added By Md.Sultan Mahmud
                         };

                return vv.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Create Methods

        public Hrm_EmployeeTransfer CreateEmployeeTransfer(Hrm_EmployeeTransfer objEmployeeTransfer, Hrm_EmployeeTransferAdditionalInfo objEmployeeTransferAdditionalInfo)
        {
            using (HrmContext)
            {
                using (var Ts = new TransactionScope())
                {
                    try
                    {

                        HrmContext.Hrm_EmployeeTransferAdditionalInfo.Add(objEmployeeTransferAdditionalInfo);
                        HrmContext.Hrm_EmployeeTransfer.Add(objEmployeeTransfer);

                        HrmContext.SaveChanges();

                        string employeeTransfer = string.Empty;
                        employeeTransfer = "EXEC SP_HrmEmployeeTransfer '" + objEmployeeTransfer.TransferFromLocation + "','"
                                                                           + objEmployeeTransfer.EmployeeID + "', "
                                                                           + objEmployeeTransfer.TransferSeqNo + ", '"
                                                                           + "TRANSFER" + "','"
                            // + objEmployeeTransfer.TransferToLocation + "','" 
                            // + objEmployeeTransfer.TransferDate.ToString("dd-MMM-yyyy") + "','" 
                            // + objEmployeeTransfer.NoteForTransfer + "','"
                            // + objEmployeeTransfer.CreatedBy + "','" 
                                                                           + "INSERT" + "'";

                        HrmContext.Database.ExecuteSqlCommand(employeeTransfer);

                        Ts.Complete();
                    }
                    catch (Exception ex)
                    {
                        Ts.Dispose();
                        throw;
                    }
                }

                return objEmployeeTransfer;
            }
        }

        public Hrm_EmployeeTransfer EmployeeAcceptOrReject(Hrm_EmployeeTransfer objEmployeeTransfer, string acceptOrReject, string reasonForLateTransferJoining)
        {
            using (HrmContext)
            {
                using (var Ts = new TransactionScope())
                {
                    try
                    {
                        string employeeTransfer = string.Empty, updateOrDelete = string.Empty;
                        updateOrDelete = acceptOrReject == "ACCEPT" ? "UPDATE" : "DELETE";

                        if (updateOrDelete == "UPDATE")
                        {
                            Hrm_EmployeeTransferAdditionalInfo objT = new Hrm_EmployeeTransferAdditionalInfo();
                            objT.EmployeeID = objEmployeeTransfer.EmployeeID;
                            objT.TransferSeqNo = objEmployeeTransfer.TransferSeqNo;
                            objT.ReasonForLateTransferJoining = reasonForLateTransferJoining;

                            HrmContext.Hrm_EmployeeTransferAdditionalInfo.Attach(objT);
                            var TransferEntry = HrmContext.Entry(objT);
                            TransferEntry.State = EntityState.Modified;

                            TransferEntry.Property(p => p.RequisitionReceivedOn).IsModified = false;
                            TransferEntry.Property(p => p.OfficeOrderNo).IsModified = false;
                            TransferEntry.Property(p => p.OfficeOrderDate).IsModified = false;
                            TransferEntry.Property(p => p.TransferReleaseDate).IsModified = false;
                            TransferEntry.Property(p => p.ResponsibilityHandoverTo).IsModified = false;
                            TransferEntry.Property(p => p.ResponsibilityTakenoverFrom).IsModified = false;
                            TransferEntry.Property(p => p.ReasonForLateTransferRelease).IsModified = false;
                            TransferEntry.Property(p => p.ActualTransferReleaseDate).IsModified = false;
                            TransferEntry.Property(p => p.ActualTransferJoiningDate).IsModified = false;
                            TransferEntry.Property(p => p.NewOperationalRole).IsModified = false;

                            HrmContext.SaveChanges();
                        }

                        employeeTransfer = "EXEC SP_HrmEmployeeTransfer '" + objEmployeeTransfer.TransferToLocation + "','" + objEmployeeTransfer.EmployeeID + "'," + objEmployeeTransfer.TransferSeqNo + ",'" + acceptOrReject + "','" + updateOrDelete + "'";
                        HrmContext.Database.ExecuteSqlCommand(employeeTransfer);

                        Ts.Complete();
                    }
                    catch (Exception ex)
                    {
                        Ts.Dispose();
                        throw;
                    }
                }

                return objEmployeeTransfer;
            }
        }

        public Hrm_EmployeeInfo CreateResignEmployee(Hrm_EmployeeInfo objEmployeeInfo, string reject)
        {
            using (HrmContext)
            {
                using (var Ts = new TransactionScope())
                {
                    try
                    {

                        string regianEmployee = "EXEC SP_HrmProcessResignReleaseOfAnEmployee   RESIGN,'" + objEmployeeInfo.EmployeeID + "','" + objEmployeeInfo.ResignDate + "','" + objEmployeeInfo.ReleaseDate + "','" + objEmployeeInfo.LastLocationCode + "','" + objEmployeeInfo.CreatedBy + "','" + reject + "'";
                        HrmContext.Database.ExecuteSqlCommand(regianEmployee);
                        Ts.Complete();
                    }
                    catch (Exception ex)
                    {
                        Ts.Dispose();
                        throw;
                    }
                }
                return objEmployeeInfo;
            }
        }

        public AuditingMaster AuditorSeupSave(AuditingMaster auditingMaster)
        {
            using (HrmContext)
            {
                using (var tx = new TransactionScope())
                {
                    try
                    {
                        Aud_AuditingMaster objAuditingMaster = new Aud_AuditingMaster();
                        objAuditingMaster.LocationCode = auditingMaster.LocationCode;
                        objAuditingMaster.AuditSeqNo = auditingMaster.AuditSeqNo;
                        objAuditingMaster.AuditStartDate = Convert.ToDateTime(auditingMaster.AuditStartDate);

                        if (auditingMaster.AuditFinishDate == null)
                            objAuditingMaster.AuditFinishDate = null;
                        else
                            objAuditingMaster.AuditFinishDate = Convert.ToDateTime(auditingMaster.AuditFinishDate);

                        objAuditingMaster.AuditPeriodFromDate = Convert.ToDateTime(auditingMaster.AuditPeriodFromDate);
                        objAuditingMaster.AuditPeriodToDate = Convert.ToDateTime(auditingMaster.AuditPeriodToDate);
                        objAuditingMaster.Remarks = auditingMaster.Remarks;
                        objAuditingMaster.CreatedBy = auditingMaster.CreatedBy;
                        objAuditingMaster.CreatedOn = Convert.ToDateTime(auditingMaster.CreatedOn);
                        objAuditingMaster.ModifiedBy = auditingMaster.ModifiedBy;
                        objAuditingMaster.ModifiedOn = auditingMaster.ModifiedOn;

                        HrmContext.Aud_AuditingMaster.Add(objAuditingMaster);

                        HrmContext.SaveChanges();

                        if (auditingMaster.AuditingDetails.Count > 0)
                        {
                            Aud_AuditingDetailsForAuditors objAuditDetails;

                            foreach (AuditingDetailsForAuditors lst in auditingMaster.AuditingDetails)
                            {
                                objAuditDetails = new Aud_AuditingDetailsForAuditors();
                                objAuditDetails.LocationCode = auditingMaster.LocationCode;
                                objAuditDetails.AuditSeqNo = auditingMaster.AuditSeqNo;
                                objAuditDetails.AuditorsEmployeeID = lst.AuditorsEmployeeID;
                                HrmContext.Aud_AuditingDetailsForAuditors.Add(objAuditDetails);
                            }
                        }

                        HrmContext.SaveChanges();
                        tx.Complete();
                    }
                    catch (Exception ex)
                    {
                        tx.Dispose();
                        throw;
                    }
                }
                return auditingMaster;
            }
        }

        public void CreateEmployeeBasicNEmploymentInfo(Hrm_EmployeeInfo objEmployeeInfo, Hrm_EmployeeWiseSalaryStructureMaster objEmployeeSalaryStructureMaster, List<Hrm_EmployeeWiseSalaryStructureDetails> lstEmloyeeSalaryStructureDetails, Hrm_EmployeeWiseBankAccount objEmployeeWiseBankAccount)
        {
            using (HrmContext)
            {
                using (var Ts = new TransactionScope())
                {
                    try
                    {
              
                        Hrm_Validation_EmployeeWiseDesignationVsEmploymentType objDesignationVsEmploymentType = new Hrm_Validation_EmployeeWiseDesignationVsEmploymentType();
                        objDesignationVsEmploymentType.EmployeeID = objEmployeeInfo.EmployeeID;
                        objDesignationVsEmploymentType.HDesignationID = objEmployeeInfo.LastDesignation;
                        objDesignationVsEmploymentType.EmploymentTypeID = objEmployeeInfo.LastEmploymentType;

                        Hrm_LocationWiseEmployee objLocationWiseEmployee = new Hrm_LocationWiseEmployee();
                        objLocationWiseEmployee.EmployeeID = objEmployeeInfo.EmployeeID;
                        objLocationWiseEmployee.LocationCode = objEmployeeInfo.LastLocationCode;

                        objEmployeeInfo.LastLocationCode = null;
                        objEmployeeInfo.LastEmploymentType = null;
                        objEmployeeInfo.LastSalaryStructureSeqNo = null;

                        HrmContext.Hrm_EmployeeInfo.Add(objEmployeeInfo);
                        HrmContext.Hrm_LocationWiseEmployee.Add(objLocationWiseEmployee);
                        HrmContext.Hrm_Validation_EmployeeWiseDesignationVsEmploymentType.Add(objDesignationVsEmploymentType);

                        if (objEmployeeInfo.ModeOfSalaryPayment == "BANK")
                            HrmContext.Hrm_EmployeeWiseBankAccount.Add(objEmployeeWiseBankAccount);

                        HrmContext.SaveChanges();

                        HrmContext.Hrm_EmployeeWiseSalaryStructureMaster.Add(objEmployeeSalaryStructureMaster);

                        foreach (Hrm_EmployeeWiseSalaryStructureDetails ssd in lstEmloyeeSalaryStructureDetails)
                        {
                            HrmContext.Hrm_EmployeeWiseSalaryStructureDetails.Add(ssd);
                        }

                        HrmContext.SaveChanges();

                        objEmployeeInfo.LastLocationCode = objLocationWiseEmployee.LocationCode;
                        objEmployeeInfo.LastEmploymentType = objDesignationVsEmploymentType.EmploymentTypeID;
                        objEmployeeInfo.LastSalaryStructureSeqNo = objEmployeeSalaryStructureMaster.SalaryStructureSeqNo;

                        HrmContext.SaveChanges();

                        SqlParameter[] storeParamCheck = new SqlParameter[2];
                        storeParamCheck[0] = new SqlParameter("prmOption", "NEWEMPLOYEEATJOINING");
                        storeParamCheck[1] = new SqlParameter("prmEmployeeID", objEmployeeInfo.EmployeeID);

                        HrmContext.Database.ExecuteSqlCommand("SP_HrmCheckValidityOfEmployeeInfo @prmOption, @prmEmployeeID", storeParamCheck);

                        Ts.Complete();
                    }
                    catch (Exception ex)
                    {
                        Ts.Dispose();
                        throw;
                    }
                }
            }
        }

        public Hrm_EmployeeNDateWiseTADAEntry CreateTADAEntry(Hrm_EmployeeNDateWiseTADAEntry objEmployeeNDateWiseTADAEntry)
        {
            using (HrmContext)
            {
                try
                {
                    HrmContext.Hrm_EmployeeNDateWiseTADAEntry.Add(objEmployeeNDateWiseTADAEntry);
                    HrmContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return objEmployeeNDateWiseTADAEntry;
        }

        #endregion

        #region Update Method

        public Hrm_EmployeeInfo Update(string employeeId, string operationalRole, string locationCode, string createdBy, string update)
        {
            Hrm_EmployeeInfo objEmployeeInfo = new Hrm_EmployeeInfo();
            using (HrmContext)
            {
                using (var Ts = new TransactionScope())
                {
                    try
                    {
                        string assignUm = "EXEC SP_HrmAssignUM '" + employeeId + "','" + operationalRole + "','" + locationCode + "','" + createdBy + "','" + update + "'";
                        HrmContext.Database.ExecuteSqlCommand(assignUm);
                        Ts.Complete();


                        //Hrm_EmployeeInfo dbRecord = ReadEmployeeInfo(employeeId);
                        //dbRecord.LastOperationalRole = operationalRole;
                        //HrmContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Ts.Dispose();
                        throw;
                    }
                }
                return objEmployeeInfo;
            }
        }

        public void UpdateEmployeeBasicNEmploymentInfo(Hrm_EmployeeInfo objEmployeeInfo, Hrm_EmployeeWiseSalaryStructureMaster objEmployeeSalaryStructureMaster, List<Hrm_EmployeeWiseSalaryStructureDetails> lstEmloyeeSalaryStructureDetails, Hrm_EmployeeWiseBankAccount objEmployeeWiseBankAccount)
        {
            using (HrmContext)
            {
                using (var Ts = new TransactionScope())
                {
                    try
                    {
                        //Hrm_Validation_EmployeeWiseDesignationVsEmploymentType objDesignationVsEmploymentType = new Hrm_Validation_EmployeeWiseDesignationVsEmploymentType();
                        //objDesignationVsEmploymentType.EmployeeID = objEmployeeInfo.EmployeeID;
                        //objDesignationVsEmploymentType.HDesignationID = objEmployeeInfo.LastDesignation;
                        //objDesignationVsEmploymentType.EmploymentTypeID = objEmployeeInfo.LastEmploymentType;

                        //Hrm_LocationWiseEmployee objLocationWiseEmployee = new Hrm_LocationWiseEmployee();
                        //objLocationWiseEmployee.EmployeeID = objEmployeeInfo.EmployeeID;
                        //objLocationWiseEmployee.LocationCode = objEmployeeInfo.LastLocationCode;

                        //objEmployeeInfo.LastLocationCode = null;
                        //objEmployeeInfo.LastEmploymentType = null;

                        HrmContext.Hrm_EmployeeInfo.Attach(objEmployeeInfo);
                        var EmployeeEntry = HrmContext.Entry(objEmployeeInfo);
                        EmployeeEntry.State = EntityState.Modified;

                        EmployeeEntry.Property(p => p.CreatedBy).IsModified = false;
                        EmployeeEntry.Property(p => p.CreatedOn).IsModified = false;

                        //HrmContext.Hrm_Validation_EmployeeWiseDesignationVsEmploymentType.Attach(objDesignationVsEmploymentType);
                        //var EmployeeWiseDesignationVsEmploymentTypeEntry = HrmContext.Entry(objDesignationVsEmploymentType);
                        //EmployeeWiseDesignationVsEmploymentTypeEntry.State = EntityState.Modified;

                        //HrmContext.Hrm_LocationWiseEmployee.Attach(objLocationWiseEmployee);
                        //var LocationWiseEmployeeEntry = HrmContext.Entry(objLocationWiseEmployee);
                        //LocationWiseEmployeeEntry.State = EntityState.Modified;

                        //HrmContext.SaveChanges();

                        //string DeleteQueryForEmployeeWiseSalaryStructureDetails = string.Format("DELETE FROM Hrm_EmployeeWiseSalaryStructureDetails WHERE EmployeeID = '{0}' AND SalaryStructureSeqNo = '{1}' ", objEmployeeSalaryStructureMaster.EmployeeID, objEmployeeSalaryStructureMaster.SalaryStructureSeqNo);
                        //HrmContext.Database.ExecuteSqlCommand(DeleteQueryForEmployeeWiseSalaryStructureDetails);

                        //objEmployeeInfo.LastLocationCode = objLocationWiseEmployee.LocationCode;
                        //objEmployeeInfo.LastEmploymentType = objDesignationVsEmploymentType.EmploymentTypeID;

                        //HrmContext.Hrm_EmployeeWiseSalaryStructureMaster.Attach(objEmployeeSalaryStructureMaster);
                        //var EmployeeWiseSalaryStructureMasterEntry = HrmContext.Entry(objEmployeeSalaryStructureMaster);
                        //EmployeeWiseSalaryStructureMasterEntry.State = EntityState.Modified;

                        //foreach (Hrm_EmployeeWiseSalaryStructureDetails ssd in lstEmloyeeSalaryStructureDetails)
                        //{
                        //    HrmContext.Hrm_EmployeeWiseSalaryStructureDetails.Add(ssd);
                        //}
                        HrmContext.SaveChanges();


                        ///<summary>
                        /// <para>Update for Hrm_Validation_EmployeeWiseDesignationVsEmploymentType,Hrm_EmployeeWiseSalaryStructureMaster,Hrm_EmployeeWiseSalaryStructureDetails is Added By Md.Sultan Mahmud
                        /// <seealso cref="HRM Module"/>
                                             
                        int countDesignation = HrmContext.Hrm_EmployeeInfo.Count(D => D.EmployeeID == objEmployeeInfo.EmployeeID && D.LastDesignation == objEmployeeInfo.LastDesignation);

                        int countEmploymentType = HrmContext.Hrm_EmployeeInfo.Count(ET => ET.EmployeeID == objEmployeeInfo.EmployeeID && ET.LastEmploymentType == objEmployeeInfo.LastEmploymentType);

                        if (countDesignation == 0 && countEmploymentType == 0)
                        {
                            SqlParameter[] storeParam = new SqlParameter[4];
                            storeParam[0] = new SqlParameter("Function", "DESIGNATION_EMPLOYMENTTYPE");
                            storeParam[1] = new SqlParameter("EmployeeID", objEmployeeInfo.EmployeeID);
                            storeParam[2] = new SqlParameter("CorrectDesignationID", objEmployeeInfo.LastDesignation);
                            storeParam[3] = new SqlParameter("@CorrectEmploymentTypeID", objEmployeeInfo.LastEmploymentType);

                            HrmContext.Database.ExecuteSqlCommand("Support_DesignationNEmploymentTypeCorrection @Function, @EmployeeID, @CorrectDesignationID, @CorrectEmploymentTypeID", storeParam);
                        }

                        else
                        {

                            if (countDesignation == 0)
                            {


                                SqlParameter[] storeParam = new SqlParameter[4];
                                storeParam[0] = new SqlParameter("Function", "DESIGNATION");
                                storeParam[1] = new SqlParameter("EmployeeID", objEmployeeInfo.EmployeeID);
                                storeParam[2] = new SqlParameter("CorrectDesignationID", objEmployeeInfo.LastDesignation);
                                storeParam[3] = new SqlParameter("@CorrectEmploymentTypeID", "NULL");

                                HrmContext.Database.ExecuteSqlCommand("Support_DesignationNEmploymentTypeCorrection @Function, @EmployeeID, @CorrectDesignationID, @CorrectEmploymentTypeID", storeParam);
                            }

                            else if (countEmploymentType == 0)
                            {
                                SqlParameter[] storeParam = new SqlParameter[4];
                                storeParam[0] = new SqlParameter("Function", "EMPLOYMENTTYPE");
                                storeParam[1] = new SqlParameter("EmployeeID", objEmployeeInfo.EmployeeID);
                                storeParam[2] = new SqlParameter("CorrectDesignationID", "NULL");
                                storeParam[3] = new SqlParameter("@CorrectEmploymentTypeID", objEmployeeInfo.LastEmploymentType);

                                HrmContext.Database.ExecuteSqlCommand("Support_DesignationNEmploymentTypeCorrection @Function, @EmployeeID, @CorrectDesignationID, @CorrectEmploymentTypeID", storeParam);
                            }
                        }

                        ///</summary>

                        SqlParameter[] storeParamCheck = new SqlParameter[2];
                        storeParamCheck[0] = new SqlParameter("prmOption", "EXISTINGEMPLOYEE");
                        storeParamCheck[1] = new SqlParameter("prmEmployeeID", objEmployeeInfo.EmployeeID);

                        HrmContext.Database.ExecuteSqlCommand("SP_HrmCheckValidityOfEmployeeInfo @prmOption, @prmEmployeeID", storeParamCheck);


                        Ts.Complete();
                    }
                    catch (Exception ex)
                    {
                        Ts.Dispose();
                        throw;
                    }
                }
            }
        }

        public AuditingMaster Update(AuditingMaster auditingMaster)
        {
            using (HrmContext)
            {
                Aud_AuditingMaster objAuditMaster = new Aud_AuditingMaster();
                objAuditMaster = HrmContext.Aud_AuditingMaster.Where(am => am.LocationCode == auditingMaster.LocationCode && am.AuditSeqNo == auditingMaster.AuditSeqNo).FirstOrDefault();

                if (auditingMaster.AuditFinishDate == null)
                    objAuditMaster.AuditFinishDate = null;
                else
                    objAuditMaster.AuditFinishDate = Convert.ToDateTime(auditingMaster.AuditFinishDate);

                //objAuditMaster.AuditFinishDate = auditingMaster.AuditFinishDate;
                objAuditMaster.Remarks = auditingMaster.Remarks;
                objAuditMaster.AuditPeriodFromDate = Convert.ToDateTime(auditingMaster.AuditPeriodFromDate);
                objAuditMaster.AuditPeriodToDate = Convert.ToDateTime(auditingMaster.AuditPeriodToDate);
                objAuditMaster.AuditStartDate = Convert.ToDateTime(auditingMaster.AuditStartDate);
                objAuditMaster.AuditSeqNo = auditingMaster.AuditSeqNo;
                objAuditMaster.ModifiedBy = auditingMaster.CreatedBy;
                objAuditMaster.ModifiedOn = DateTime.Now;

                HrmContext.SaveChanges();

                return auditingMaster;
            }
        }

        public Hrm_EmployeeNDateWiseTADAEntry UpdateTADAEntry(Hrm_EmployeeNDateWiseTADAEntry objEmployeeNDateWiseTADAEntry)
        {
            using (HrmContext)
            {
                try
                {
                    HrmContext.Hrm_EmployeeNDateWiseTADAEntry.Attach(objEmployeeNDateWiseTADAEntry);
                    var EmployeeNDateWiseTADAEntry = HrmContext.Entry(objEmployeeNDateWiseTADAEntry);
                    EmployeeNDateWiseTADAEntry.State = EntityState.Modified;

                    EmployeeNDateWiseTADAEntry.Property(p => p.CreatedBy).IsModified = false;
                    EmployeeNDateWiseTADAEntry.Property(p => p.CreatedOn).IsModified = false;
                    EmployeeNDateWiseTADAEntry.Property(p => p.Status).IsModified = false;

                    HrmContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return objEmployeeNDateWiseTADAEntry;
        }

        public Hrm_EmployeeTransfer UpdateEmployeeTransfer(Hrm_EmployeeTransfer objEmployeeTransfer, Hrm_EmployeeTransferAdditionalInfo objEmployeeTransferAdditionalInfo)
        {
            using (HrmContext)
            {
                using (var Ts = new TransactionScope())
                {
                    try
                    {
                        HrmContext.Hrm_EmployeeTransferAdditionalInfo.Attach(objEmployeeTransferAdditionalInfo);
                        var EmployeeTransferAdditionalEntry = HrmContext.Entry(objEmployeeTransferAdditionalInfo);
                        EmployeeTransferAdditionalEntry.State = EntityState.Modified;


                        HrmContext.Hrm_EmployeeTransfer.Attach(objEmployeeTransfer);
                        var EmployeeTransferEntry = HrmContext.Entry(objEmployeeTransfer);
                        EmployeeTransferEntry.State = EntityState.Modified;

                        EmployeeTransferEntry.Property(p => p.CreatedBy).IsModified = false;
                        EmployeeTransferEntry.Property(p => p.CreatedOn).IsModified = false;
                        EmployeeTransferEntry.Property(p => p.Status).IsModified = false;
                        EmployeeTransferEntry.Property(p => p.RefAEProjectCode).IsModified = false;
                        EmployeeTransferEntry.Property(p => p.RefAETransNo).IsModified = false;

                        EmployeeTransferEntry.Property(p => p.IsTransferAccepted).IsModified = false;
                        EmployeeTransferEntry.Property(p => p.IsItLastTransfer).IsModified = false;

                        //HrmContext.Hrm_EmployeeTransferAdditionalInfo.Add(objEmployeeTransferAdditionalInfo);
                        //HrmContext.Hrm_EmployeeTransfer.Add(objEmployeeTransfer);



                        HrmContext.SaveChanges();

                        string employeeTransfer = string.Empty;
                        employeeTransfer = "EXEC SP_HrmEmployeeTransfer '" + objEmployeeTransfer.TransferFromLocation + "','"
                                                                           + objEmployeeTransfer.EmployeeID + "', "
                                                                           + objEmployeeTransfer.TransferSeqNo + ", '"
                                                                           + "TRANSFER" + "','"
                            // + objEmployeeTransfer.TransferToLocation + "','" 
                            // + objEmployeeTransfer.TransferDate.ToString("dd-MMM-yyyy") + "','" 
                            // + objEmployeeTransfer.NoteForTransfer + "','"
                            // + objEmployeeTransfer.CreatedBy + "','" 
                                                                           + "UPDATE" + "'";

                        HrmContext.Database.ExecuteSqlCommand(employeeTransfer);

                        Ts.Complete();
                    }
                    catch (Exception ex)
                    {
                        Ts.Dispose();
                        throw;
                    }
                }

                return objEmployeeTransfer;
            }
        }

        #endregion

    }
}
