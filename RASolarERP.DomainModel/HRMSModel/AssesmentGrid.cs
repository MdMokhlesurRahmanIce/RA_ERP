using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarERP.DomainModel.HRMSModel
{
    public class AssesmentGrid 
    {
        public int EvaluationCriteriaID { get; set; }
        public string EvaluationCriteriaNameInBangla { get; set; }
        public int EvaluationCriteriaWeight { get; set; }
        public string FalseFieldForERPT { get; set; }
    }
}
