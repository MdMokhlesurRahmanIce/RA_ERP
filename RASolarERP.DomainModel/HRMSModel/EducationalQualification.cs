using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarERP.DomainModel.HRMSModel
{
    public class EducationalQualification
    {
        public string LevelofEdu { get; set; }
        public string NameOfDegree { get; set; }
        public string FacultyOrBoard { get; set; }
        public string NameOfInstitution { get; set; }
        public string MajorSujectOrGroup { get; set; }
        public string CGPAOrDivision { get; set; }
        public string PassingYear { get; set; }
    }
}
