using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using RASolarERP.DomainModel.HRMSModel;

namespace RASolarHRMS.Model
{
    public interface IRASolarHRMSRepository
    {
        List<GetLocationWiseEmployeeForEmployeeRegister> ReadGetLocationWiseEmployeeForEmployeeRegister(string locationCode);

        Hrm_EmployeeInfo ReadEmployeeInfo(string employeeId);
        List<EmployeeDetailsInfo> ReadEmployeeInfo(List<string> employeeIds);

        List<EmployeeTransferInfo> ReadGetEmployeeTransferredButNotYetAccepted(string locationCode);
        List<EmployeeDetailsInfo> ReadEmployeeSearchDetails(string employeeID, string employeeName, string designation, string locationCode, byte employeeStatus);
        List<EmployeeDetailsInfo> ReadLocationWiseEmployee(string locationCode);
        List<PersonEmployeeDetailsInfo> ReadPersonLocationWiseEmployee(string locationCode);
        List<PersonEmployeeDetailsInfo> ReadLocationWiseEmployeeForAssEntry(string locationCode, string mullaynNam, string mullaynKal);
        List<EmployeeEvulationType> ReadEmployeeEvulationType();
        List<EmployeeEvulationSubType> ReadEmployeeEvulationSubType(string ddlMullaynerName);
        
        
        List<EmployeeInfoForAssEntry> ReadLocationWiseEmployeeForAssEntry(string locationCode, string EmpID);

        List<ERPValueForAssessment> GetDataForERPT(string viewScore, string locationCode, string EmployeeID, string mullayenerName, string ddlMullaynerKal, string valOfthis);
       

        List<AssesmentCriteriaNScore> ReadAssesmentCriteriaNScore(string optionID, string criteriaValue);

        string SaveTrainingAssesmentEntry(string prmLocationCode, string ddlIdNo, string empEvType, string empEvKal, string trainingDate, string assesmentDate, string prmIsAssessmentCompleted, string txtUnitManagerMontobbo, string chkbxVal, string aprovedDate, string crrArrId, string crrArrScore, string weight, string pPurboPNo, string prmIsAssessmentSelectedByERPData, string userLogin, string insertData, string rowWiseRemarks);

        List<AssesmentGrid> ReadlstForAssesGrid(string mullaynNam, string mullaynKal);
        

        List<EmployeeDetailsInfo> ReadLocationWiseEmployeeWithUMAcountManager(string locationCode);
        List<EmployeeDetailsInfo> ReadLocationWiseEmployeeWithTransferStatus(string locationCode);
        List<Hrm_HierarchicalDesignation> ReadEmployeeDesignation();

        byte ReadTransferSequenceNumber(string employeeId);
        Hrm_EmployeeTransfer EmployeeAcceptOrReject(Hrm_EmployeeTransfer objEmployeeTransfer, string acceptOrReject, string reasonOfLateJoining);

        List<Hrm_OperationalRole> ReadOperationalRole();

        Hrm_EmployeeTransfer CreateEmployeeTransfer(Hrm_EmployeeTransfer objEmployeeTransfer, Hrm_EmployeeTransferAdditionalInfo objEmployeeTransferAdditionalInfo);
        Hrm_EmployeeTransfer UpdateEmployeeTransfer(Hrm_EmployeeTransfer objEmployeeTransfer, Hrm_EmployeeTransferAdditionalInfo objEmployeeTransferAdditionalInfo);
        Hrm_EmployeeInfo CreateResignEmployee(Hrm_EmployeeInfo objEmployeeInfo, string reject);
        Hrm_EmployeeInfo Update(string employeeId, string operationalRole, string locationCode, string createdBy, string update);

        bool CheckEmployeeTransferOfficeOrderNo(string officeOrderNo);

        List<BankAdviceForSalaryReport> ReadBankAdviceForSalaryReport(string forMonth, string reportType, string generateType, string locationCode, string bankAccount);
        
        List<BankAdviceForSalaryLatterReport> ReadBankAdviceForSalaryLatterReport(string cd_Customer);

        List<LatterToChairmanReport> ReadLatterToChairmanReport(string cd_Customer);

        List<LatterToUpoZelaChairmanReport> ReadLatterToUpoZelaChairmanReport(string cd_Customer); 

        void GenerateSalaryAdviceForBank(string forMonth, string reportType, string generateType, string startLetterSequenceNo, string locationCode, string bankAccount);
        ArrayList AuditorEmployeeList();
        AuditingMaster AuditorSeupSave(AuditingMaster auditingMaster);
        List<EmployeeDetailsInfo> ReadLocationWiseEmployeeForHeadOffice(string locationCode);
        string ReadAuditSeqNumberAfterCheckFinishedDate(string locationCode);
        AuditingMaster ReadAuditingMasterDetails(string locationCode, string auditSeqNumber);
        DateTime? AuditPeriodFromDate(string locationCode);
        bool AuditMasterSetupAlreadyExistOrNot(string locationCode, string auditSeqNumber);
        AuditingMaster Update(AuditingMaster auditingMaster);

        ArrayList BankListForSalaryAdvice(string locationCode);

        ArrayList BankAdviceLcoation(string locationCode);
        EmployeeDetails EmployeeDetailsInfo(string employeeId);

        List<DistrictInfo> ReadDistrict();
        List<UpazillaInfo> ReadUpazillaInfo(string districtCode);
        List<BloodGroupInfo> ReadBloodGroupInfo();
        List<EducationInfo> ReadEducationInfo();
        List<ReligionInfo> ReadReligionInfo();
        List<MaritalStatusInfo> ReadMaritalStatusInfo();
        List<MajorSubject> ReadMajorSubject();
        List<EmploymentTypeInfo> ReadEmploymentTypeInfo();
        List<HierarchicalDesignation> ReadHierarchicalDesignation();
        List<JobGradeInfo> ReadJobGradeInfo();
        List<DepartmentInfo> ReadDepartmentInfo();
        List<OperationalRole> ReadOperationalRole(string designationid);

        string EmployeeIdMax();
        bool EmployeeExistOrNot(string employeeId);
        void CreateEmployeeBasicNEmploymentInfo(Hrm_EmployeeInfo objEmployeeInfo, Hrm_EmployeeWiseSalaryStructureMaster objEmployeeSalaryStructureMaster, List<Hrm_EmployeeWiseSalaryStructureDetails> lstEmloyeeSalaryStructureDetails, Hrm_EmployeeWiseBankAccount objEmployeeWiseBankAccount);
        void UpdateEmployeeBasicNEmploymentInfo(Hrm_EmployeeInfo objEmployeeInfo, Hrm_EmployeeWiseSalaryStructureMaster objEmployeeSalaryStructureMaster, List<Hrm_EmployeeWiseSalaryStructureDetails> lstEmloyeeSalaryStructureDetails, Hrm_EmployeeWiseBankAccount objEmployeeWiseBankAccount);
        byte EmployeeWiseSalaryStructureSeqenceNumberMax(string employeeId);
        List<Hrm_HierarchicalDesignationNEmploymentTypeWisePayBandStructure> DesignationNEmploymentTypeWisePayBandStructure(string designationId, string employmentTypeId);

        Hrm_EmployeeNDateWiseTADAEntry CreateTADAEntry(Hrm_EmployeeNDateWiseTADAEntry objEmployeeNDateWiseTADAEntry);
        Hrm_EmployeeNDateWiseTADAEntry UpdateTADAEntry(Hrm_EmployeeNDateWiseTADAEntry objEmployeeNDateWiseTADAEntry);
        List<TADADetails> ReadTADADetails(DateTime dateFrom, DateTime dateTo, string employeeID);
        List<TADADetails> ReviewTADAAmountNApproval(string locationCode, string reportType, string tADAAprrovalMonth);

        EmployeeTransferInfo ReadEmployeeTransfer(string employeeId, byte transferSequenceNumber);  

    }
}
