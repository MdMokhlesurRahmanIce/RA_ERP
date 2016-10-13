using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.HRMSModel
{
    public class EmployeeTransferInfo
    {
        public string EmployeeID { get; set; }
        public byte? TransferSeqNo { get; set; }

        public string  EmployeeName { get; set; }
        public string Designation { get; set; }

        public DateTime TransferDate { get; set; }
        public string TransferDateString { get; set; }

        public string TransferFromLocation { get; set; }

        public string TransferToLocation { get; set; }
        public string TransferToLocationName { get; set; }

        public string NoteForTransfer { get; set; }

        public DateTime? RequisitionReceivedOn { get; set; }
        public DateTime? OfficeOrderDate { get; set; }
        public string RequisitionReceivedOnString { get; set; }
        public string OfficeOrderDateString { get; set; }

        public string OfficeOrderNo { get; set; }
        public string OfficeOrderNoSequence { get; set; }

        public DateTime? TransferReleaseDate { get; set; }
        public string TransferReleaseDateString { get; set; }

        public string ResponsibilityHandoverTo { get; set; }
        public string ResponsibilityTakenoverFrom { get; set; }

        public string ResponsibilityHandoverToName { get; set; }
        public string ResponsibilityHandoverFromName { get; set; }

        public string ReasonForLateTransferJoining { get; set; }
        public string ReasonForLateTransferRelease { get; set; }

        public DateTime? ActualTransferJoiningDate { get; set; }
        public DateTime? ActualTransferReleaseDate { get; set; }
        public string ActualTransferJoiningDateString { get; set; }
        public string ActualTransferReleaseDateString { get; set; }

        public string NewOperationalRole { get; set; }

    }
}
