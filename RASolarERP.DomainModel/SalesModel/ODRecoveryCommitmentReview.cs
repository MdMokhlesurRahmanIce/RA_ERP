using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.SalesModel
{
    public class ODRecoveryCommitmentReview
    {
           public string Zone { get; set; }
           public string Region { get; set; }
           public string UnitCode { get; set; }
           public string UnitName { get; set; }
           public string LastMonthODCollectionEfficiency { get; set; }
           public decimal TotalODAmount { get; set; }
           public string TotalRecoveredThisMonth { get; set; }
           public decimal RemainingODAmount { get; set; }
           public string ThisMonthODCollectionEfficiency { get; set; }
           public DateTime RMLastRemarksDate { get; set; }
           public DateTime ZMLastRemarksDate { get; set; }
    }
}
