using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarHelper.HRMSHelperModel
{
    public class CashOrBankVoucherForAuditorAccountsDetails
    {
            public int AccountNo {get; set;}
            public string AccountName {get; set;}
            public string Particulars {get; set;}
            public double? DrAmount {get; set;}
            public double? CrAmount {get; set;}
    } 
}
