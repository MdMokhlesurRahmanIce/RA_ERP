using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarHelper.HRMSHelperModel
{
    public class GenderInfo
    {
        public string GenderId { get; set; }
        public string GenderName { get; set; }

        public List<GenderInfo> GenderInfoList()
        {
            List<GenderInfo> lstGenderInfo = new List<GenderInfo>();

            GenderInfo objGenderInfo;

            objGenderInfo = new GenderInfo();
            objGenderInfo.GenderId = "M";
            objGenderInfo.GenderName = "Male";
            lstGenderInfo.Add(objGenderInfo);

            objGenderInfo = new GenderInfo();
            objGenderInfo.GenderId = "F";
            objGenderInfo.GenderName = "Female";

            lstGenderInfo.Add(objGenderInfo);

            return lstGenderInfo;
        }
    }
}
