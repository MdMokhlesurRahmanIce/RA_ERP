using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarAMS.Model
{
    class GeneralLedger
    {        

        private string _RecordOrd =string.Empty;
        private string _TransNo=string.Empty;
        private string _TransDate=string.Empty;
        private string _Particulars=string.Empty;
        private string _DrAmount=string.Empty;
        private string _CrAmount=string.Empty;
        private string _Balance=string.Empty;
        private string _LocatioCode=string.Empty;

        public string RecordOrd 
        {
            get { return _RecordOrd; }
            set {  _RecordOrd=value;}
        }
        public string TransNo 
        {
            get { return _TransNo; }
            set { _TransNo = value; }
        }
        public string TransDate 
        {
            get { return _TransDate; }
            set { _TransDate = value; }
        }
        public string Particulars 
        {
            get { return _Particulars; }
            set { _Particulars = value; }
        }
        public string DrAmount
        {
            get { return _DrAmount; }
            set { _DrAmount = value; }
        }
        
        //public string _Balance = string.Empty;
        //public string _LocatioCode = string.Empty;



    }
}
