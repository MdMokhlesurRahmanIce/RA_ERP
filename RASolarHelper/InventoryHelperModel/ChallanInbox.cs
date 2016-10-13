using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarHelper.InventoryHelperModel
{
    public class ChallanInbox
    {
        private string __LocationCode = string.Empty;
        private string __ChallanSeqNo = string.Empty;
        private string __ReceivedChallan = string.Empty;

        public string LocationCode
        {
            set { __LocationCode = value; }
            get { return __LocationCode; }
        }

        public string ChallanSeqNo
        {
            set { __ChallanSeqNo = value; }
            get { return __ChallanSeqNo; }
        }

        public string ReceivedChallan
        {
            set { __ReceivedChallan = value; }
            get { return __ReceivedChallan; }
        }

        //public ChallanInbox(string LCode, string ChSeqNo, string RecCh)
        //{
        //    LocationCode = LCode;
        //    ChallanSeqNo = ChSeqNo;
        //    ReceivedChallan = RecCh;
        //}
    }
}
