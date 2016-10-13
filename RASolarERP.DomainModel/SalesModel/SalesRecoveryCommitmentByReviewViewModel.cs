using RASolarHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.SalesModel
{
    public class SalesRecoveryCommitmentByReviewViewModel
    {
        public string ODCustomerGrading { get; set; }
        public string ZoneCode { get; set; }

        public List<LocationInfo> Zone { get; set; }
        public List<SalesRecoveryCommitmentByReview> SalesRecoveryCommitmentByReview { get; set; }


    }
}
