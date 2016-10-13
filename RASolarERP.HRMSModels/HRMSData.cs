using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

using RASolarHRMS.Model;
using RASolarERP.Model;
using RASolarHelper;
using RASolarERP.DomainModel.HRMSModel;
using RASolarAMS.Model;
using System.Collections;
using RASolarERP.DomainModel.AMSModel;

namespace RASolarERP.Web.Areas.HRMS.Models
{
    public class HRMSData : HRMSBaseData
    {
        RASolarAMSService AMSService = new RASolarAMSService();

        public Hrm_EmployeeInfo ReadEmployeeInfo(string employeeId)
        {
            return HRMSService.ReadEmployeeInfo(employeeId);
        }

        public List<EmployeeDetailsInfo> ReadEmployeeInfo(List<string> employeeIds)
        {
            return HRMSService.ReadEmployeeInfo(employeeIds);
        }

        public List<GetLocationWiseEmployeeForEmployeeRegister> ReadGetLocationWiseEmployeeForEmployeeRegister(string locationCode)
        {
            return HRMSService.ReadGetLocationWiseEmployeeForEmployeeRegister(locationCode);
        }

        public Hrm_EmployeeTransfer CreateEmployeeTransfer(Hrm_EmployeeTransfer objEmployeeTransfer, Hrm_EmployeeTransferAdditionalInfo objEmployeeTransferAdditionalInfo)
        {
            return HRMSService.CreateEmployeeTransfer(objEmployeeTransfer, objEmployeeTransferAdditionalInfo);
        }

        public Hrm_EmployeeTransfer UpdateEmployeeTransfer(Hrm_EmployeeTransfer objEmployeeTransfer, Hrm_EmployeeTransferAdditionalInfo objEmployeeTransferAdditionalInfo)
        {
            return HRMSService.UpdateEmployeeTransfer(objEmployeeTransfer, objEmployeeTransferAdditionalInfo);
        }

        public List<EmployeeTransferInfo> ReadGetEmployeeTransferredButNotYetAccepted(string locationCode)
        {
            return HRMSService.ReadGetEmployeeTransferredButNotYetAccepted(locationCode);
        }

        public List<EmployeeDetailsInfo> ReadEmployeeSearchDetails(string employeeID, string employeeName, string designation, string locationCode, byte employeeStatus)
        {
            List<EmployeeDetailsInfo> lstEmployeeDetails = new List<EmployeeDetailsInfo>();

            lstEmployeeDetails = HRMSService.ReadEmployeeSearchDetails(employeeID, employeeName, designation, locationCode, employeeStatus);
            lstEmployeeDetails = (from ss in lstEmployeeDetails
                                  select new EmployeeDetailsInfo
                                  {
                                      EmployeeID = ss.EmployeeID,
                                      EmployeeName = ss.EmployeeName,
                                      Department = ss.Department,
                                      Designation = ss.Designation,
                                      PresentLocation = (ss.PresentZone != null ? (ss.PresentZone + " / ") : ss.PresentZone) + (ss.PresentRegion != null ? ss.PresentRegion + " / " : ss.PresentRegion) + ss.PresentLocation

                                  }).ToList();


            return lstEmployeeDetails;
        }

        public List<EmployeeDetailsInfo> ReadLocationWiseEmployee(string locationCode)
        {
            return HRMSService.ReadLocationWiseEmployee(locationCode);
        }

        public List<PersonEmployeeDetailsInfo> ReadLocationWiseEmployeeForAssEntry(string locationCode, string mullaynNam, string mullaynKal)
        {
            return HRMSService.ReadLocationWiseEmployeeForAssEntry(locationCode, mullaynNam,mullaynKal); 
        }

        public List<EmployeeEvulationType> ReadEmployeeEvulationType() 
        {
            return HRMSService.ReadEmployeeEvulationType(); 
        }

        public List<EmployeeEvulationSubType> ReadEmployeeEvulationSubType(string ddlMullaynerName)
        {
            return HRMSService.ReadEmployeeEvulationSubType(ddlMullaynerName);
        }

        public List<AssesmentGrid> ReadlstForAssesGrid(string mullaynNam, string mullaynKal)
        {
            return HRMSService.ReadlstForAssesGrid(mullaynNam, mullaynKal);  
        }


        public List<EmployeeInfoForAssEntry> ReadLocationWiseEmployeeForAssEntry(string locationCode, string EmpID)
        {
            return HRMSService.ReadLocationWiseEmployeeForAssEntry(locationCode, EmpID); 
        }

        public List<ERPValueForAssessment> GetDataForERPT(string viewScore, string locationCode, string EmployeeID, string mullayenerName, string ddlMullaynerKal, string valOfthis)
        {
            return HRMSService.GetDataForERPT(viewScore, locationCode, EmployeeID, mullayenerName, ddlMullaynerKal, valOfthis); 
        }


        public List<AssesmentCriteriaNScore> ReadAssesmentCriteriaNScore(string optionID, string criteriaValue)
        {
            return HRMSService.ReadAssesmentCriteriaNScore(optionID, criteriaValue); 
        }

        //prmLocationCode, ddlIdNo, befrtr, trainingDate, assesmentDate, prmIsAssessmentCompleted, txtUnitManagerMontobbo, chkbxVal, aprovedDate, assesmentOptionId, assesmentOptionCriteria, prmIsAssessmentSelectedByERPData, userLogin, insertData

        public string SaveTrainingAssesmentEntry(string prmLocationCode, string ddlIdNo, string empEvType, string empEvKal, string trainingDate, string assesmentDate, string prmIsAssessmentCompleted, string txtUnitManagerMontobbo, string chkbxVal, string aprovedDate, string crArrID, string crArrScore, string weight, string pPurboPNo, string prmIsAssessmentSelectedByERPData, string userLogin, string insertData, string rowWiseRemarks)
        {
            return HRMSService.SaveTrainingAssesmentEntry(prmLocationCode, ddlIdNo, empEvType, empEvKal, trainingDate, assesmentDate, prmIsAssessmentCompleted, txtUnitManagerMontobbo, chkbxVal, aprovedDate, crArrID, crArrScore, weight, pPurboPNo, prmIsAssessmentSelectedByERPData, userLogin, insertData, rowWiseRemarks);
        }

        public List<PersonEmployeeDetailsInfo> ReadPersonLocationWiseEmployee(string locationCode)
        {
            return HRMSService.ReadPersonLocationWiseEmployee(locationCode);
        }
        public List<EmployeeDetailsInfo> ReadLocationWiseEmployeeWithUMAcountManager(string locationCode)
        {
            return HRMSService.ReadLocationWiseEmployeeWithUMAcountManager(locationCode);
        }

        public List<EmployeeDetailsInfo> ReadLocationWiseEmployeeWithTransferStatus(string locationCode)
        {
            return HRMSService.ReadLocationWiseEmployeeWithTransferStatus(locationCode);
        }

        public List<Hrm_HierarchicalDesignation> ReadEmployeeDesignation()
        {
            return HRMSService.ReadEmployeeDesignation();
        }

        public byte ReadTransferSequenceNumber(string employeeId)
        {
            return HRMSService.ReadTransferSequenceNumber(employeeId);
        }

        public Hrm_EmployeeTransfer EmployeeAcceptOrReject(Hrm_EmployeeTransfer objEmployeeTransfer, string acceptOrReject, string reasonOfLateJoining)
        {
            return HRMSService.EmployeeAcceptOrReject(objEmployeeTransfer, acceptOrReject, reasonOfLateJoining);
        }

        public List<Hrm_OperationalRole> ReadOperationalRole()
        {
            return HRMSService.ReadOperationalRole();
        }

        public Hrm_EmployeeInfo UpdateEmployeeAsUm(string employeeId, string operationalRole, string locationCode, string createdBy, string update)
        {
            return HRMSService.Update(employeeId, operationalRole, locationCode, createdBy, update);
        }

        public Hrm_EmployeeInfo CreateResignEmployee(Hrm_EmployeeInfo objEmployeeInfo, string reject)
        {
            return HRMSService.CreateResignEmployee(objEmployeeInfo, reject);
        }

        public bool CheckEmployeeTransferOfficeOrderNo(string officeOrderNo)
        {
            return HRMSService.CheckEmployeeTransferOfficeOrderNo(officeOrderNo);
        }

        public List<BankAdviceForSalaryReport> ReadBankAdviceForSalaryReport(string forMonth, string reportType, string generateType, string locationCode, string bankAccount)
        {
            return HRMSService.ReadBankAdviceForSalaryReport(forMonth, reportType, generateType, locationCode, bankAccount);
        }

        public List<BankAdviceForSalaryLatterReport> ReadBankAdviceForSalaryLatterReport(string OCT_End_CDODCUSTOMERS)
        {
            return HRMSService.ReadBankAdviceForSalaryLatterReport(OCT_End_CDODCUSTOMERS);
        }

        public List<LatterToChairmanReport> ReadLatterToChairmanReport(string OCT_End_CDODCUSTOMERS)
        {
            return HRMSService.ReadLatterToChairmanReport(OCT_End_CDODCUSTOMERS);
        }

        public List<LatterToUpoZelaChairmanReport> ReadLatterToUpoZelaChairmanReport(string OCT_End_CDODCUSTOMERS)
        {
            return HRMSService.ReadLatterToUpoZelaChairmanReport(OCT_End_CDODCUSTOMERS);
        }

        public void GenerateSalaryAdviceForBank(string forMonth, string reportType, string generateType, string startLetterSequenceNo, string locationCode, string bankAccount)
        {
            HRMSService.GenerateSalaryAdviceForBank(forMonth, reportType, generateType, startLetterSequenceNo, locationCode, bankAccount);
        }

        public ArrayList AuditorEmployeeList()
        {
            return HRMSService.AuditorEmployeeList();
        }

        public AuditingMaster AuditorSeupSave(AuditingMaster auditingMaster)
        {
            return HRMSService.AuditorSeupSave(auditingMaster);
        }

        public List<EmployeeDetailsInfo> ReadLocationWiseEmployeeForHeadOffice(string locationCode)
        {
            return HRMSService.ReadLocationWiseEmployeeForHeadOffice(locationCode);
        }

        public string ReadAuditSeqNumberAfterCheckFinishedDate(string locationCode)
        {
            return HRMSService.ReadAuditSeqNumberAfterCheckFinishedDate(locationCode);
        }

        public AuditingMaster ReadAuditingMasterDetails(string locationCode, string auditSeqNumber)
        {
            return HRMSService.ReadAuditingMasterDetails(locationCode, auditSeqNumber);
        }

        public ArrayList BankListForSalaryAdvice(string locationCode)
        {
            return HRMSService.BankListForSalaryAdvice(locationCode);
        }

        public ArrayList BankAdviceLcoation(string locationCode)
        {
            return HRMSService.BankListForSalaryAdvice(locationCode);
        }

        public AuditingMaster AuditMasterCheckNGetAuditMasterDetails(string locationCode, DateTime transactionOpenDate)
        {
            string auditSequenceNumber = this.ReadAuditSeqNumberAfterCheckFinishedDate(locationCode);

            AuditingMaster auditingMaster = new AuditingMaster();

            if (!string.IsNullOrEmpty(auditSequenceNumber))
            {
                auditingMaster = this.ReadAuditingMasterDetails(locationCode, auditSequenceNumber);
            }
            else
            {
                auditingMaster.AuditingDetails = new List<AuditingDetailsForAuditors>();
                auditingMaster.AuditStartDate = transactionOpenDate;

                DateTime? dt = HRMSService.AuditPeriodFromDate(locationCode);

                if (dt != null)
                {
                    auditingMaster.AuditPeriodFromDate = dt;
                }
            }

            return auditingMaster;
        }

        public bool AuditMasterSetupAlreadyExistOrNot(string locationCode, string auditSeqNumber)
        {
            return HRMSService.AuditMasterSetupAlreadyExistOrNot(locationCode, auditSeqNumber);
        }

        public AuditingMaster UpdateAuditMasterDetails(AuditingMaster auditingMaster)
        {
            return HRMSService.Update(auditingMaster);
        }

        public ArrayList GetBankAccountForSalaryBoucher(string locationCode)
        {
            return AMSService.GetBankAccountForSalaryBoucher(locationCode);
        }

        public List<BankInformation> ReadBankInformation(string locationCode)
        {
            return AMSService.ReadBankInformation(locationCode);
        }

        public List<BankInformation> ReadBankInformation(string locationCode, string bankId, string bankBranchID)
        {
            return AMSService.ReadBankInformation(locationCode, bankId, bankBranchID);
        }

        public List<BankBranchInfo> ReadBankBranchInformation(string locationCode, string bankId)
        {
            return AMSService.ReadBankBranchInformation(locationCode, bankId);
        }

        public List<BankAccountType> ReadBankAccountType()
        {
            return AMSService.ReadBankAccountType();
        }

        public ArrayList GetBankAccountForSalaryBoucher()
        {
            return AMSService.GetBankAccountForSalaryBoucher();
        }

        public EmployeeDetails EmployeeDetailsInfo(string employeeId)
        {
            EmployeeDetails objEmployeeDetails = new EmployeeDetails();

            if (!string.IsNullOrEmpty(employeeId))
            {
                objEmployeeDetails = HRMSService.EmployeeDetailsInfo(employeeId);

                if (objEmployeeDetails == null)
                {
                    objEmployeeDetails = new EmployeeDetails();

                    objEmployeeDetails.EmployeeID = Helper.EmployeeIdNew(this.EmployeeIdMax());
                    objEmployeeDetails.ProbationPeriodInMonth = Helper.ProbationPeriodDefault;
                    objEmployeeDetails.ContractualPeriodInMonth = Helper.ContractPeriodDefault;

                    objEmployeeDetails.IsEmployeeForEdit = false;
                }
                else
                    objEmployeeDetails.IsEmployeeForEdit = true;

                if (objEmployeeDetails.LastEmploymentType == "PERMANENT1" && objEmployeeDetails.ProbationPeriodInMonth == null)
                {
                    objEmployeeDetails.ProbationPeriodInMonth = 6;
                    objEmployeeDetails.ContractualPeriodInMonth = null;
                }
                else if (objEmployeeDetails.LastEmploymentType == "CONTRACTUL" && objEmployeeDetails.ContractualPeriodInMonth != null)
                {
                    objEmployeeDetails.ProbationPeriodInMonth = null;
                }
            }
            else
            {
                objEmployeeDetails.EmployeeID = Helper.EmployeeIdNew(this.EmployeeIdMax());
                objEmployeeDetails.Gender = Helper.GenderDefault;
                objEmployeeDetails.ProbationPeriodInMonth = Helper.ProbationPeriodDefault;
                objEmployeeDetails.ContractualPeriodInMonth = Helper.ContractPeriodDefault;
                objEmployeeDetails.IsEmployeeForEdit = false;
            }

            objEmployeeDetails.EmployeeImagePath = "../../" + Helper.ImagePath + "/" + objEmployeeDetails.EmployeeID + "-Img.jpg";
            objEmployeeDetails.EmployeeSignaturePath = "../../" + Helper.ImagePath + "/" + objEmployeeDetails.EmployeeID + "-Sin.jpg";

            objEmployeeDetails.DistrictInfo = this.ReadDistrict();

            objEmployeeDetails.HighestEducationInfo = this.ReadEducationInfo();
            objEmployeeDetails.BloodGroupInfo = this.ReadBloodGroupInfo();
            objEmployeeDetails.ReligionInfo = this.ReadReligionInfo();
            objEmployeeDetails.MajorSubjectInfo = this.ReadMajorSubject();
            objEmployeeDetails.EmploymentType = this.ReadEmploymentTypeInfo();
            objEmployeeDetails.Designation = this.ReadHierarchicalDesignation();
            objEmployeeDetails.GenderInfo = this.ReadGenderInfo();
            objEmployeeDetails.MaritalStatusInfo = this.ReadMaritalStatusInfo();

            objEmployeeDetails.DepartmentInfo = this.ReadDepartmentInfo();

            if (!string.IsNullOrEmpty(objEmployeeDetails.LastDesignation))
                objEmployeeDetails.OperationalRoleInfo = this.ReadOperationalRole(objEmployeeDetails.LastDesignation);
            else
                objEmployeeDetails.OperationalRoleInfo = new List<OperationalRole>();

            if (!string.IsNullOrEmpty(objEmployeeDetails.PresentUpazilla))
                objEmployeeDetails.PresentUpazillaInfo = this.ReadUpazillaInfo(objEmployeeDetails.PresentDistrictCode);
            else
                objEmployeeDetails.PresentUpazillaInfo = new List<UpazillaInfo>();

            if (!string.IsNullOrEmpty(objEmployeeDetails.PermanentUpazilla))
                objEmployeeDetails.ParmanentUpazillaInfo = this.ReadUpazillaInfo(objEmployeeDetails.PermanentDistrictCode);
            else
                objEmployeeDetails.ParmanentUpazillaInfo = new List<UpazillaInfo>();

            objEmployeeDetails.PeriodDurationInMonth = new PeriodDurationInMonth().PeriodDurationInMonthList();
            objEmployeeDetails.SalaryModeOfPayment = new SalaryModeOfPayment().SalaryModeOfPaymentList();

            objEmployeeDetails.JobGradeInfo = this.ReadJobGradeInfo();

            objEmployeeDetails.BankAccountTypeInfo = this.ReadBankAccountType();

            objEmployeeDetails.LastSalaryStructureSeqNo = objEmployeeDetails.LastSalaryStructureSeqNo ?? 1;

            return objEmployeeDetails;
        }

        public List<DistrictInfo> ReadDistrict()
        {
            return HRMSService.ReadDistrict();
        }

        public List<UpazillaInfo> ReadUpazillaInfo(string districtCode)
        {
            return HRMSService.ReadUpazillaInfo(districtCode);
        }

        public List<BloodGroupInfo> ReadBloodGroupInfo()
        {
            return HRMSService.ReadBloodGroupInfo();
        }

        public List<EducationInfo> ReadEducationInfo()
        {
            return HRMSService.ReadEducationInfo();
        }

        public List<ReligionInfo> ReadReligionInfo()
        {
            return HRMSService.ReadReligionInfo();
        }

        public List<MaritalStatusInfo> ReadMaritalStatusInfo()
        {
            return HRMSService.ReadMaritalStatusInfo();
        }

        public List<MajorSubject> ReadMajorSubject()
        {
            return HRMSService.ReadMajorSubject();
        }

        public List<EmploymentTypeInfo> ReadEmploymentTypeInfo()
        {
            return HRMSService.ReadEmploymentTypeInfo();
        }

        public List<HierarchicalDesignation> ReadHierarchicalDesignation()
        {
            return HRMSService.ReadHierarchicalDesignation();
        }

        public List<JobGradeInfo> ReadJobGradeInfo()
        {
            return HRMSService.ReadJobGradeInfo();
        }

        public List<DepartmentInfo> ReadDepartmentInfo()
        {
            return HRMSService.ReadDepartmentInfo();
        }

        public List<OperationalRole> ReadOperationalRole(string designationid)
        {
            return HRMSService.ReadOperationalRole(designationid);
        }

        public List<GenderInfo> ReadGenderInfo()
        {
            return new GenderInfo().GenderInfoList();
        }

        public string EmployeeIdMax()
        {
            return HRMSService.EmployeeIdMax();
        }

        public bool EmployeeExistOrNot(string employeeId)
        {
            return HRMSService.EmployeeExistOrNot(employeeId);
        }

        public void CreateEmployeeBasicNEmploymentInfo(Hrm_EmployeeInfo objEmployeeInfo, Hrm_EmployeeWiseSalaryStructureMaster objEmployeeSalaryStructureMaster, List<Hrm_EmployeeWiseSalaryStructureDetails> lstEmloyeeSalaryStructureDetails, Hrm_EmployeeWiseBankAccount objEmployeeWiseBankAccount)
        {
            HRMSService.CreateEmployeeBasicNEmploymentInfo(objEmployeeInfo, objEmployeeSalaryStructureMaster, lstEmloyeeSalaryStructureDetails, objEmployeeWiseBankAccount);
        }

        public void UpdateEmployeeBasicNEmploymentInfo(Hrm_EmployeeInfo objEmployeeInfo, Hrm_EmployeeWiseSalaryStructureMaster objEmployeeSalaryStructureMaster, List<Hrm_EmployeeWiseSalaryStructureDetails> lstEmloyeeSalaryStructureDetails, Hrm_EmployeeWiseBankAccount objEmployeeWiseBankAccount)
        {
            HRMSService.UpdateEmployeeBasicNEmploymentInfo(objEmployeeInfo, objEmployeeSalaryStructureMaster, lstEmloyeeSalaryStructureDetails, objEmployeeWiseBankAccount);
        }

        public Hrm_EmployeeInfo ProcessEmployeeBasicNEmploymentInfo(EmployeeDetails objEmployeeDetails)
        {
            try
            {
                Hrm_EmployeeInfo objEmployeeInfo = new Hrm_EmployeeInfo();

                if (objEmployeeDetails != null)
                {
                    objEmployeeInfo.EmployeeID = objEmployeeDetails.EmployeeID;
                    objEmployeeInfo.EmployeeName = objEmployeeDetails.EmployeeName;
                    objEmployeeInfo.EmployeeFathersName = objEmployeeDetails.EmployeeFathersName;
                    objEmployeeInfo.EmployeeMothersName = objEmployeeDetails.EmployeeMothersName;
                    objEmployeeInfo.DateOfBirth = Convert.ToDateTime(objEmployeeDetails.DateOfBirth);
                    objEmployeeInfo.Gender = objEmployeeDetails.Gender;
                    objEmployeeInfo.PlaceOfBirth = objEmployeeDetails.PlaceOfBirthId;
                    objEmployeeInfo.MaritalStatus = objEmployeeDetails.MaritalStatus;
                    objEmployeeInfo.Nationality = objEmployeeDetails.Nationality;
                    objEmployeeInfo.Religion = objEmployeeDetails.Religion;
                    objEmployeeInfo.HighestEducation = objEmployeeDetails.HighestEducation;
                    objEmployeeInfo.MajorSubject = objEmployeeDetails.MajorSubject;
                    objEmployeeInfo.BloodGroup = objEmployeeDetails.BloodGroup;
                    objEmployeeInfo.NationalIDCard = objEmployeeDetails.NationalIDCard;
                    objEmployeeInfo.EmailID = objEmployeeDetails.EmailID;
                    objEmployeeInfo.PresentDistrictCode = objEmployeeDetails.PresentDistrictCode;
                    objEmployeeInfo.PresentUpazillaCode = objEmployeeDetails.PresentUpazillaCode;
                    objEmployeeInfo.PresentPoliceStation = objEmployeeDetails.PresentPoliceStation;
                    objEmployeeInfo.PresentPostOffice = objEmployeeDetails.PresentPostOffice;
                    objEmployeeInfo.PresentStreetOrVillage = objEmployeeDetails.PresentStreetOrVillage;
                    objEmployeeInfo.PresentHouseNo = objEmployeeDetails.PresentHouseNo;
                    objEmployeeInfo.PresentMobileNo = objEmployeeDetails.PresentMobileNo;
                    objEmployeeInfo.PresentPhone = objEmployeeDetails.PresentPhone;
                    objEmployeeInfo.PermanentDistrictCode = objEmployeeDetails.PermanentDistrictCode;
                    objEmployeeInfo.PermanentUpazillaCode = objEmployeeDetails.PermanentUpazillaCode;
                    objEmployeeInfo.PermanentPoliceStation = objEmployeeDetails.PermanentPoliceStation;
                    objEmployeeInfo.PermanentPostOffice = objEmployeeDetails.PermanentPostOffice;
                    objEmployeeInfo.PermanentStreetVillage = objEmployeeDetails.PermanentStreetVillage;
                    objEmployeeInfo.PermanentHouseNo = objEmployeeDetails.PermanentHouseNo;
                    objEmployeeInfo.PermanentMobileNo = objEmployeeDetails.PermanentMobileNo;
                    objEmployeeInfo.PermanentPhone = objEmployeeDetails.PermanentPhone;
                    objEmployeeInfo.JoiningDate = Convert.ToDateTime(objEmployeeDetails.JoiningDate);
                    objEmployeeInfo.LastDesignation = objEmployeeDetails.LastDesignation;
                    objEmployeeInfo.LastOperationalRole = objEmployeeDetails.LastOperationalRole;
                    objEmployeeInfo.LastDepartment = objEmployeeDetails.LastDepartment;
                    //objEmployeeInfo.LastSection = objEmployeeDetails.LastSection;
                    objEmployeeInfo.LastEmploymentType = objEmployeeDetails.LastEmploymentType;
                    objEmployeeInfo.LastSalaryStructureSeqNo = objEmployeeDetails.LastSalaryStructureSeqNo;
                    objEmployeeInfo.ModeOfSalaryPayment = objEmployeeDetails.ModeOfSalaryPayment;
                    objEmployeeInfo.ProbationPeriodInMonth = objEmployeeDetails.ProbationPeriodInMonth;
                    objEmployeeInfo.ContractualPeriodInMonth = objEmployeeDetails.ContractualPeriodInMonth;
                    //objEmployeeInfo.ConfirmationDate = objEmployeeDetails.ConfirmationDate;
                    objEmployeeInfo.LastLocationCode = objEmployeeDetails.LastLocationCode;
                    //objEmployeeInfo.ReleaseDate = objEmployeeDetails.ReleaseDate;

                    objEmployeeInfo.PassportNo = objEmployeeDetails.PassportNo;
                    objEmployeeInfo.PassportExpiryDate = objEmployeeDetails.PassportExpiryDate;
                    objEmployeeInfo.EmployeeSpouseName = objEmployeeDetails.EmployeeSpouseName;
                    objEmployeeInfo.SpouseBloodGroup = objEmployeeDetails.SpouseBloodGroup;
                    objEmployeeInfo.DrivingLicenseNo = objEmployeeDetails.DrivingLicenseNo;

                    objEmployeeInfo.Status = Helper.Active;
                    //objEmployeeInfo.StatusChangedDate = objEmployeeDetails.StatusChangedDate;

                    objEmployeeInfo.CreatedBy = "";
                    objEmployeeInfo.CreatedOn = DateTime.Now;
                }

                return objEmployeeInfo;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Hrm_EmployeeWiseBankAccount ProcessEmployeeWiseBankAccount(EmployeeDetails objEmployeeDetails)
        {
            Hrm_EmployeeWiseBankAccount objEmployeeWiseBankAccount = new Hrm_EmployeeWiseBankAccount();

            try
            {
                objEmployeeWiseBankAccount.EmployeeID = objEmployeeDetails.EmployeeID;
                objEmployeeWiseBankAccount.BankID = objEmployeeDetails.BankID;
                objEmployeeWiseBankAccount.BankBranchID = objEmployeeDetails.BankBranchID;
                objEmployeeWiseBankAccount.BankAccountNumber = objEmployeeDetails.BankAccountNumber;
                objEmployeeWiseBankAccount.BankAccountType = objEmployeeDetails.BankAccountType;
                objEmployeeWiseBankAccount.Address = objEmployeeDetails.Address;
                objEmployeeWiseBankAccount.AccountOpeningDate = objEmployeeDetails.AccountOpeningDate;
                objEmployeeWiseBankAccount.SalaryLocationCode = objEmployeeDetails.SalaryLocationCode;
                objEmployeeWiseBankAccount.SalaryDisbursementBankAccountNumber = objEmployeeDetails.SalaryDisbursementBankAccountNumber;

                if (!string.IsNullOrEmpty(objEmployeeDetails.SalaryDisbursementBranchName))
                    objEmployeeWiseBankAccount.SalaryDisbursementBranchName = objEmployeeDetails.SalaryDisbursementBranchName;

                //objEmployeeWiseBankAccount.Remarks  = objEmployeeDetails. ;
                objEmployeeWiseBankAccount.Status = Helper.Active;

                return objEmployeeWiseBankAccount;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Hrm_EmployeeWiseSalaryStructureMaster ProcessEmployeeWiseSalaryStructureMaster(string employeeId, string lastDesignation, string lastEmploymentType, DateTime joiningDate)
        {
            try
            {
                Hrm_EmployeeWiseSalaryStructureMaster objEmployeeSalaryStructureMaster = new Hrm_EmployeeWiseSalaryStructureMaster();
                byte salaryStructureSeqNo = Helper.EmployeeWiseSalaryStructureSeqenceNumberGeneration(this.EmployeeWiseSalaryStructureSeqenceNumberMax(employeeId));

                objEmployeeSalaryStructureMaster.EmployeeID = employeeId;
                objEmployeeSalaryStructureMaster.SalaryStructureSeqNo = salaryStructureSeqNo;
                objEmployeeSalaryStructureMaster.HDesignationID = lastDesignation;
                objEmployeeSalaryStructureMaster.EmploymentTypeID = lastEmploymentType;
                objEmployeeSalaryStructureMaster.SSEffectiveFromDate = joiningDate.Date;
                objEmployeeSalaryStructureMaster.SSEffectiveToDate = new DateTime(2021, 12, 31);

                return objEmployeeSalaryStructureMaster;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Hrm_EmployeeWiseSalaryStructureMaster ProcessEmployeeWiseSalaryStructureMasterUpdate(string employeeId, string lastDesignation, string lastEmploymentType, DateTime joiningDate)
        {
            try
            {
                Hrm_EmployeeWiseSalaryStructureMaster objEmployeeSalaryStructureMaster = new Hrm_EmployeeWiseSalaryStructureMaster();
                byte salaryStructureSeqNo = this.EmployeeWiseSalaryStructureSeqenceNumberMax(employeeId);

                objEmployeeSalaryStructureMaster.EmployeeID = employeeId;
                objEmployeeSalaryStructureMaster.SalaryStructureSeqNo = salaryStructureSeqNo;
                objEmployeeSalaryStructureMaster.HDesignationID = lastDesignation;
                objEmployeeSalaryStructureMaster.EmploymentTypeID = lastEmploymentType;
                objEmployeeSalaryStructureMaster.SSEffectiveFromDate = joiningDate.Date;
                objEmployeeSalaryStructureMaster.SSEffectiveToDate = new DateTime(2021, 12, 31);

                return objEmployeeSalaryStructureMaster;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Hrm_EmployeeWiseSalaryStructureDetails> ProcessEmployeeWiseSalaryStructureDetails(string employeeId, byte salaryStructureSeqNo, string lastDesignation, string lastEmploymentType)
        {
            try
            {
                List<Hrm_EmployeeWiseSalaryStructureDetails> lstEmloyeeSalaryStructureDetails = new List<Hrm_EmployeeWiseSalaryStructureDetails>();

                List<Hrm_HierarchicalDesignationNEmploymentTypeWisePayBandStructure> lstEmployeeSalaryBandStructure = new List<Hrm_HierarchicalDesignationNEmploymentTypeWisePayBandStructure>();
                lstEmployeeSalaryBandStructure = this.DesignationNEmploymentTypeWisePayBandStructure(lastDesignation, lastEmploymentType);

                var v = from str in lstEmployeeSalaryBandStructure
                        select new Hrm_EmployeeWiseSalaryStructureDetails
                        {
                            EmployeeID = employeeId,
                            SalaryStructureSeqNo = salaryStructureSeqNo,
                            SalaryComponentID = str.SalaryComponentID,
                            RefHDesignationID = str.HDesignationID,
                            RefEmploymentTypeID = str.EmploymentTypeID,
                            Amount = (decimal)(str.MinBandAmountFrom != null ? str.MinBandAmountFrom : str.FixedAmount)
                        };

                lstEmloyeeSalaryStructureDetails = v.ToList();

                return lstEmloyeeSalaryStructureDetails;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public byte EmployeeWiseSalaryStructureSeqenceNumberMax(string employeeId)
        {
            return HRMSService.EmployeeWiseSalaryStructureSeqenceNumberMax(employeeId);
        }

        public List<Hrm_HierarchicalDesignationNEmploymentTypeWisePayBandStructure> DesignationNEmploymentTypeWisePayBandStructure(string designationId, string employmentTypeId)
        {
            return HRMSService.DesignationNEmploymentTypeWisePayBandStructure(designationId, employmentTypeId);
        }

        public List<TADADetails> ReadTADADetails(DateTime dateFrom, DateTime dateTo, string employeeID)
        {
            return HRMSService.ReadTADADetails(dateFrom, dateTo, employeeID);
        }

        public Hrm_EmployeeNDateWiseTADAEntry CreateTADAEntry(Hrm_EmployeeNDateWiseTADAEntry objEmployeeNDateWiseTADAEntry)
        {
            return HRMSService.CreateTADAEntry(objEmployeeNDateWiseTADAEntry);
        }

        public Hrm_EmployeeNDateWiseTADAEntry UpdateTADAEntry(Hrm_EmployeeNDateWiseTADAEntry objEmployeeNDateWiseTADAEntry)
        {
            return HRMSService.UpdateTADAEntry(objEmployeeNDateWiseTADAEntry);
        }

        public List<TADADetails> ReviewTADAAmountNApproval(string locationCode, string reportType, string tADAAprrovalMonth)
        {
            return HRMSService.ReviewTADAAmountNApproval(locationCode, reportType, tADAAprrovalMonth);
        }

        public EmployeeTransferInfo ReadEmployeeTransfer(string employeeId, byte transferSequenceNumber)
        {
            return HRMSService.ReadEmployeeTransfer(employeeId, transferSequenceNumber);
        }

    }
}