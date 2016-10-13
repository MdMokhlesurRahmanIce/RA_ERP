using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarERP.DomainModel.AMSModel
{
    public class GeneralLedger
    {
        private int _RecordOrd = 0;
        private int _TransNo = 0;
        private DateTime _TransDate = DateTime.Now;
        private string _Particulars = string.Empty;
        private decimal _DrAmount = 0;
        private decimal _CrAmount = 0;
        private decimal _Balance = 0;
        private string _LocationCode = string.Empty;
       
        public int RecordOrd
        {
            get { return _RecordOrd; }
            set { _RecordOrd = value; }
        }
        public int TransNo
        {
            get { return _TransNo; }
            set { _TransNo = value; }
        }
        public DateTime TransDate
        {
            get { return _TransDate; }
            set { _TransDate = value; }
        }
        public string Particulars
        {
            get { return _Particulars; }
            set { _Particulars = value; }
        }
        public decimal DrAmount
        {
            get { return _DrAmount; }
            set { _DrAmount = value; }
        }
        public decimal CrAmount
        {
            get { return _CrAmount; }
            set { _CrAmount = value; }
        }
        public decimal Balance
        {
            get { return _Balance; }
            set { _Balance = value; }
        }
        public string _LocatioCode
        {
            get { return _LocationCode; }
            set { _LocationCode = value; }
        }
    }
}
