using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarHelper.HRMSHelperModel
{
    public class EmployeeDetails
    {
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeFathersName { get; set; }
        public string EmployeeMothersName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PlaceOfBirthId { get; set; }
        public string MaritalStatus { get; set; }
        public string Nationality { get; set; }
        public string Religion { get; set; }
        public string HighestEducation { get; set; }
        public string MajorSubject { get; set; }
        public string BloodGroup { get; set; }
        public string NationalIDCard { get; set; }
        public string EmailID { get; set; }
        public string PresentDistrictCode { get; set; }
        public string PresentUpazillaCode { get; set; }
        public string PresentPoliceStation { get; set; }
        public string PresentPostOffice { get; set; }
        public string PresentStreetOrVillage { get; set; }
        public string PresentHouseNo { get; set; }
        public string PresentMobileNo { get; set; }
        public string PresentPhone { get; set; }
        public string PermanentDistrictCode { get; set; }
        public string PermanentUpazillaCode { get; set; }
        public string PermanentPoliceStation { get; set; }
        public string PermanentPostOffice { get; set; }
        public string PermanentStreetVillage { get; set; }
        public string PermanentHouseNo { get; set; }
        public string PermanentMobileNo { get; set; }
        public string PermanentPhone { get; set; }
        public DateTime? JoiningDate { get; set; }
        public string LastDesignation { get; set; }
        public string LastOperationalRole { get; set; }
        public string LastDepartment { get; set; }
        public string LastSection { get; set; }
        public string LastEmploymentType { get; set; }
        public byte? LastSalaryStructureSeqNo { get; set; }
        public string ModeOfSalaryPayment { get; set; }
        public string EmployeeGrade { get; set; }
        public byte? ProbationPeriodInMonth { get; set; }
        public byte? ContractualPeriodInMonth { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public string LastLocationCode { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public byte Status { get; set; }
        public DateTime? StatusChangedDate { get; set; }

        public string GenderDescription { get; set; }
        public string PlaceOfBirth { get; set; }
        public string MaritalStatusDescription { get; set; }
        public string ReligionName { get; set; }
        public string EducationName { get; set; }
        public string EducationLevel { get; set; }
        public string MajorSubjectName { get; set; }
        public string BloodGroupDescription { get; set; }
        public string PresentDistrict { get; set; }
        public string PresentUpazilla { get; set; }
        public string PermanentDistrict { get; set; }
        public string PermanentUpazilla { get; set; }
        public string HDesignationName { get; set; }
        public string OperationalRoleName { get; set; }
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        public string EmploymentTypeDescription { get; set; }
        public string LocationName { get; set; }
        public string EmployeeImagePath { get; set; }
        public string EmployeeSignaturePath { get; set; }

        public List<DistrictInfo> DistrictInfo { get; set; }

        public List<UpazillaInfo> PresentUpazillaInfo { get; set; }
        public List<UpazillaInfo> ParmanentUpazillaInfo { get; set; }

        public List<EducationInfo> HighestEducationInfo { get; set; }
        public List<BloodGroupInfo> BloodGroupInfo { get; set; }
        public List<ReligionInfo> ReligionInfo { get; set; }
        public List<MajorSubject> MajorSubjectInfo { get; set; }

        public List<EmploymentTypeInfo> EmploymentType { get; set; }
        public List<DepartmentInfo> DepartmentInfo { get; set; }
        public List<HierarchicalDesignation> Designation { get; set; }
        public List<OperationalRole> OperationalRoleInfo { get; set; }

        public List<GenderInfo> GenderInfo { get; set; }
        public List<MaritalStatusInfo> MaritalStatusInfo { get; set; }

        public List<PeriodDurationInMonth> PeriodDurationInMonth { get; set; }
        public List<SalaryModeOfPayment> SalaryModeOfPayment { get; set; }
    }
}
