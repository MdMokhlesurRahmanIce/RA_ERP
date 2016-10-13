using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RASolarERP.DomainModel.HRMSModel;
using System.Collections;

namespace RASolarHRMS.Model
{
    public class RASolarHRMSService : IRASolarHRMSService
    {
        #region Properties And Constructor

        private IRASolarHRMSRepository raSolarHRMSRepository;

        public RASolarHRMSService()
        {
            this.raSolarHRMSRepository = new RASolarHRMSRepository(new RASolarHRAEntities());
        }

        public RASolarHRMSService(IRASolarHRMSRepository raSolarRepository)
        {
            this.raSolarHRMSRepository = raSolarRepository;
        }
        #endregion

        #region Read Methods

        public Hrm_EmployeeInfo ReadEmployeeInfo(string employeeId)
        {
            return raSolarHRMSRepository.ReadEmployeeInfo(employeeId);
        }

        public List<EmployeeDetailsInfo> ReadEmployeeInfo(List<string> employeeIds)
        {
            return raSolarHRMSRepository.ReadEmployeeInfo(employeeIds);
        }

        public List<GetLocationWiseEmployeeForEmployeeRegister> ReadGetLocationWiseEmployeeForEmployeeRegister(string locationCode)
        {
            return raSolarHRMSRepository.ReadGetLocationWiseEmployeeForEmployeeRegister(locationCode);
        }

        public List<EmployeeTransferInfo> ReadGetEmployeeTransferredButNotYetAccepted(string locationCode)
        {
            return raSolarHRMSRepository.ReadGetEmployeeTransferredButNotYetAccepted(locationCode);
        }

        public List<EmployeeDetailsInfo> ReadEmployeeSearchDetails(string employeeID, string employeeName, string designation, string locationCode, byte employeeStatus)
        {
            return raSolarHRMSRepository.ReadEmployeeSearchDetails(employeeID, employeeName, designation, locationCode, employeeStatus);
        }

        public List<EmployeeDetailsInfo> ReadLocationWiseEmployee(string locationCode)
        {
            return raSolarHRMSRepository.ReadLocationWiseEmployee(locationCode);
        }

        public List<PersonEmployeeDetailsInfo> ReadPersonLocationWiseEmployee(string locationCode)
        {
            return raSolarHRMSRepository.ReadPersonLocationWiseEmployee(locationCode);
        }

        public List<AssesmentGrid> ReadlstForAssesGrid(string mullaynNam, string mullaynKal)
        {
            return raSolarHRMSRepository.ReadlstForAssesGrid(mullaynNam,mullaynKal); 
        }

        public List<ERPValueForAssessment> GetDataForERPT(string viewScore, string locationCode, string EmployeeID, string mullayenerName, string ddlMullaynerKal, string valOfthis)
        {
            return raSolarHRMSRepository.GetDataForERPT(viewScore, locationCode, EmployeeID, mullayenerName, ddlMullaynerKal, valOfthis);
        }


        public List<PersonEmployeeDetailsInfo> ReadLocationWiseEmployeeForAssEntry(string locationCode, string mullaynNam, string mullaynKal)
        {
            return raSolarHRMSRepository.ReadLocationWiseEmployeeForAssEntry(locationCode, mullaynNam, mullaynKal); 
        }


        public List<EmployeeEvulationType> ReadEmployeeEvulationType()
        {
            return raSolarHRMSRepository.ReadEmployeeEvulationType(); 
        }

        public List<EmployeeEvulationSubType> ReadEmployeeEvulationSubType(string ddlMullaynerName)
        {
            return raSolarHRMSRepository.ReadEmployeeEvulationSubType(ddlMullaynerName);
        }


        public List<EmployeeInfoForAssEntry> ReadLocationWiseEmployeeForAssEntry(string locationCode, string EmpID)
        {
            return raSolarHRMSRepository.ReadLocationWiseEmployeeForAssEntry(locationCode, EmpID);
        }

        public List<AssesmentCriteriaNScore> ReadAssesmentCriteriaNScore(string optionID, string criteriaValue)
        {
            return raSolarHRMSRepository.ReadAssesmentCriteriaNScore(optionID, criteriaValue);
        }

        public string SaveTrainingAssesmentEntry(string prmLocationCode, string ddlIdNo, string empEvType, string empEvKal, string trainingDate, string assesmentDate, string prmIsAssessmentCompleted, string txtUnitManagerMontobbo, string chkbxVal, string aprovedDate, string crArrID, string crArrScore, string weight, string pPurboPNo, string prmIsAssessmentSelectedByERPData, string userLogin, string insertData, string rowWiseRemarks)
        {
            return raSolarHRMSRepository.SaveTrainingAssesmentEntry(prmLocationCode, ddlIdNo, empEvType, empEvKal, trainingDate, assesmentDate, prmIsAssessmentCompleted, txtUnitManagerMontobbo, chkbxVal, aprovedDate, crArrID, crArrScore, weight, pPurboPNo, prmIsAssessmentSelectedByERPData, userLogin, insertData, rowWiseRemarks);
        }

        public List<EmployeeDetailsInfo> ReadLocationWiseEmployeeWithUMAcountManager(string locationCode)
        {
            return raSolarHRMSRepository.ReadLocationWiseEmployeeWithUMAcountManager(locationCode);
        }

        public List<EmployeeDetailsInfo> ReadLocationWiseEmployeeWithTransferStatus(string locationCode)
        {
            return raSolarHRMSRepository.ReadLocationWiseEmployeeWithTransferStatus(locationCode);
        }

        public List<Hrm_HierarchicalDesignation> ReadEmployeeDesignation()
        {
            return raSolarHRMSRepository.ReadEmployeeDesignation();
        }

        public byte ReadTransferSequenceNumber(string employeeId)
        {
            return raSolarHRMSRepository.ReadTransferSequenceNumber(employeeId);
        }

        public List<Hrm_OperationalRole> ReadOperationalRole()
        {
            return raSolarHRMSRepository.ReadOperationalRole();
        }

        public List<BankAdviceForSalaryReport> ReadBankAdviceForSalaryReport(string forMonth, string reportType, string generateType, string locationCode, string bankAccount)
        {
            return raSolarHRMSRepository.ReadBankAdviceForSalaryReport(forMonth, reportType, generateType, locationCode, bankAccount);
        }

        public List<BankAdviceForSalaryLatterReport> ReadBankAdviceForSalaryLatterReport(string cd_Cudtomer)
        {
            return raSolarHRMSRepository.ReadBankAdviceForSalaryLatterReport(cd_Cudtomer);
        }

        public List<LatterToChairmanReport> ReadLatterToChairmanReport(string cd_Cudtomer)
        {
            return raSolarHRMSRepository.ReadLatterToChairmanReport(cd_Cudtomer);
        }

        public List<LatterToUpoZelaChairmanReport> ReadLatterToUpoZelaChairmanReport(string cd_Cudtomer)
        {
            return raSolarHRMSRepository.ReadLatterToUpoZelaChairmanReport(cd_Cudtomer);
        }

        public void GenerateSalaryAdviceForBank(string forMonth, string reportType, string generateType, string startLetterSequenceNo, string locationCode, string bankAccount)
        {
            raSolarHRMSRepository.GenerateSalaryAdviceForBank(forMonth, reportType, generateType, startLetterSequenceNo, locationCode, bankAccount);
        }

        public ArrayList AuditorEmployeeList()
        {
            return raSolarHRMSRepository.AuditorEmployeeList();
        }

        public AuditingMaster AuditorSeupSave(AuditingMaster auditingMaster)
        {
            return raSolarHRMSRepository.AuditorSeupSave(auditingMaster);
        }

        public List<EmployeeDetailsInfo> ReadLocationWiseEmployeeForHeadOffice(string locationCode)
        {
            return raSolarHRMSRepository.ReadLocationWiseEmployeeForHeadOffice(locationCode);
        }

        public AuditingMaster ReadAuditingMasterDetails(string locationCode, string auditSeqNumber)
        {
            return raSolarHRMSRepository.ReadAuditingMasterDetails(locationCode, auditSeqNumber);
        }

        public string ReadAuditSeqNumberAfterCheckFinishedDate(string locationCode)
        {
            return raSolarHRMSRepository.ReadAuditSeqNumberAfterCheckFinishedDate(locationCode);
        }

        public DateTime? AuditPeriodFromDate(string locationCode)
        {
            return raSolarHRMSRepository.AuditPeriodFromDate(locationCode);
        }

        public bool AuditMasterSetupAlreadyExistOrNot(string locationCode, string auditSeqNumber)
        {
            return raSolarHRMSRepository.AuditMasterSetupAlreadyExistOrNot(locationCode, auditSeqNumber);
        }

        public ArrayList BankListForSalaryAdvice(string locationCode)
        {
            return raSolarHRMSRepository.BankListForSalaryAdvice(locationCode);
        }

        public ArrayList BankAdviceLcoation(string locationCode)
        {
            return raSolarHRMSRepository.BankListForSalaryAdvice(locationCode);
        }

        public string GetAuditSequenceNumber(string locationCode)
        {
            return raSolarHRMSRepository.ReadAuditSeqNumberAfterCheckFinishedDate(locationCode);
        }

        public EmployeeDetails EmployeeDetailsInfo(string employeeId)
        {
            return raSolarHRMSRepository.EmployeeDetailsInfo(employeeId);
        }

        public List<DistrictInfo> ReadDistrict()
        {
            return raSolarHRMSRepository.ReadDistrict();
        }

        public List<UpazillaInfo> ReadUpazillaInfo(string districtCode)
        {
            return raSolarHRMSRepository.ReadUpazillaInfo(districtCode);
        }

        public List<BloodGroupInfo> ReadBloodGroupInfo()
        {
            return raSolarHRMSRepository.ReadBloodGroupInfo();
        }

        public List<EducationInfo> ReadEducationInfo()
        {
            return raSolarHRMSRepository.ReadEducationInfo();
        }

        public List<ReligionInfo> ReadReligionInfo()
        {
            return raSolarHRMSRepository.ReadReligionInfo();
        }

        public List<MaritalStatusInfo> ReadMaritalStatusInfo()
        {
            return raSolarHRMSRepository.ReadMaritalStatusInfo();
        }

        public List<MajorSubject> ReadMajorSubject()
        {
            return raSolarHRMSRepository.ReadMajorSubject();
        }

        public List<EmploymentTypeInfo> ReadEmploymentTypeInfo()
        {
            return raSolarHRMSRepository.ReadEmploymentTypeInfo();
        }

        public List<HierarchicalDesignation> ReadHierarchicalDesignation()
        {
            return raSolarHRMSRepository.ReadHierarchicalDesignation();
        }

        public List<JobGradeInfo> ReadJobGradeInfo()
        {
            return raSolarHRMSRepository.ReadJobGradeInfo();
        }

        public List<DepartmentInfo> ReadDepartmentInfo()
        {
            return raSolarHRMSRepository.ReadDepartmentInfo();
        }

        public List<OperationalRole> ReadOperationalRole(string designationid)
        {
            return raSolarHRMSRepository.ReadOperationalRole(designationid);
        }

        public string EmployeeIdMax()
        {
            return raSolarHRMSRepository.EmployeeIdMax();
        }

        public bool EmployeeExistOrNot(string employeeId)
        {
            return raSolarHRMSRepository.EmployeeExistOrNot(employeeId);
        }

        public byte EmployeeWiseSalaryStructureSeqenceNumberMax(string employeeId)
        {
            return raSolarHRMSRepository.EmployeeWiseSalaryStructureSeqenceNumberMax(employeeId);
        }

        public List<Hrm_HierarchicalDesignationNEmploymentTypeWisePayBandStructure> DesignationNEmploymentTypeWisePayBandStructure(string designationId, string employmentTypeId)
        {
            return raSolarHRMSRepository.DesignationNEmploymentTypeWisePayBandStructure(designationId, employmentTypeId);
        }

        public List<TADADetails> ReadTADADetails(DateTime dateFrom, DateTime dateTo, string employeeID)
        {
            return raSolarHRMSRepository.ReadTADADetails(dateFrom, dateTo, employeeID);
        }

        public List<TADADetails> ReviewTADAAmountNApproval(string locationCode, string reportType, string tADAAprrovalMonth)
        {
            return raSolarHRMSRepository.ReviewTADAAmountNApproval(locationCode, reportType, tADAAprrovalMonth);
        }

        public EmployeeTransferInfo ReadEmployeeTransfer(string employeeId, byte transferSequenceNumber)
        { 
            return raSolarHRMSRepository.ReadEmployeeTransfer( employeeId,  transferSequenceNumber); 
        }

        #endregion

        #region Create Methods

        public Hrm_EmployeeTransfer CreateEmployeeTransfer(Hrm_EmployeeTransfer objEmployeeTransfer, Hrm_EmployeeTransferAdditionalInfo objEmployeeTransferAdditionalInfo)
        {
            return raSolarHRMSRepository.CreateEmployeeTransfer(objEmployeeTransfer, objEmployeeTransferAdditionalInfo);
        }

        public Hrm_EmployeeTransfer EmployeeAcceptOrReject(Hrm_EmployeeTransfer objEmployeeTransfer, string acceptOrReject, string reasonOfLateJoining)
        {
            return raSolarHRMSRepository.EmployeeAcceptOrReject(objEmployeeTransfer, acceptOrReject, reasonOfLateJoining);
        }

        public Hrm_EmployeeInfo CreateResignEmployee(Hrm_EmployeeInfo objEmployeeInfo, string reject)
        {
            return raSolarHRMSRepository.CreateResignEmployee(objEmployeeInfo, reject);
        }

        public void CreateEmployeeBasicNEmploymentInfo(Hrm_EmployeeInfo objEmployeeInfo, Hrm_EmployeeWiseSalaryStructureMaster objEmployeeSalaryStructureMaster, List<Hrm_EmployeeWiseSalaryStructureDetails> lstEmloyeeSalaryStructureDetails, Hrm_EmployeeWiseBankAccount objEmployeeWiseBankAccount)
        {
            raSolarHRMSRepository.CreateEmployeeBasicNEmploymentInfo(objEmployeeInfo, objEmployeeSalaryStructureMaster, lstEmloyeeSalaryStructureDetails, objEmployeeWiseBankAccount);
        }

        public Hrm_EmployeeNDateWiseTADAEntry CreateTADAEntry(Hrm_EmployeeNDateWiseTADAEntry objEmployeeNDateWiseTADAEntry)
        {
            return raSolarHRMSRepository.CreateTADAEntry(objEmployeeNDateWiseTADAEntry);
        }

        #endregion

        #region Update Method

        public Hrm_EmployeeInfo Update(string employeeId, string operationalRole, string locationCode, string createdBy, string update)
        {
            return raSolarHRMSRepository.Update(employeeId, operationalRole, locationCode, createdBy, update);
        }

        public void UpdateEmployeeBasicNEmploymentInfo(Hrm_EmployeeInfo objEmployeeInfo, Hrm_EmployeeWiseSalaryStructureMaster objEmployeeSalaryStructureMaster, List<Hrm_EmployeeWiseSalaryStructureDetails> lstEmloyeeSalaryStructureDetails, Hrm_EmployeeWiseBankAccount objEmployeeWiseBankAccount)
        {
            raSolarHRMSRepository.UpdateEmployeeBasicNEmploymentInfo(objEmployeeInfo, objEmployeeSalaryStructureMaster, lstEmloyeeSalaryStructureDetails, objEmployeeWiseBankAccount);
        }

        public bool CheckEmployeeTransferOfficeOrderNo(string officeOrderNo)
        {
            return raSolarHRMSRepository.CheckEmployeeTransferOfficeOrderNo(officeOrderNo);
        }

        public AuditingMaster Update(AuditingMaster auditingMaster)
        {
            return raSolarHRMSRepository.Update(auditingMaster);
        }

        public Hrm_EmployeeNDateWiseTADAEntry UpdateTADAEntry(Hrm_EmployeeNDateWiseTADAEntry objEmployeeNDateWiseTADAEntry)
        {
            return raSolarHRMSRepository.UpdateTADAEntry(objEmployeeNDateWiseTADAEntry);
        }

        public Hrm_EmployeeTransfer UpdateEmployeeTransfer(Hrm_EmployeeTransfer objEmployeeTransfer, Hrm_EmployeeTransferAdditionalInfo objEmployeeTransferAdditionalInfo)
        {
            return raSolarHRMSRepository.UpdateEmployeeTransfer(objEmployeeTransfer, objEmployeeTransferAdditionalInfo);
        }

        #endregion
    }
}
