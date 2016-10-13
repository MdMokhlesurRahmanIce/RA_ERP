using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarHelper.InventoryHelperModel
{
    public class ChallanInformationGlanceDetails
    {       
        
        public DateTime TransDate {get;set;}	
        public string ChallanType {get;set;}	
        public string ChallanNo	{get;set;}
        public string ChallanSeqNo	{get;set;}
        public string IssuedTo {get;set;}
        public string ChallanLocationCode { get; set; }
        public string MrrLocationCode { get; set; }
    }
}
