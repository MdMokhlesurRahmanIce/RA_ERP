using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarHelper.SalesHelperModel
{
    public class SalesDay
    {
        private string __VisitDay = string.Empty;
        private string __VisitDayValue = string.Empty;

        public string VisitDay
        {
            get { return __VisitDay; }
            set { __VisitDay = value; }
        }

        public string VisitDayValue
        {
            get { return __VisitDayValue; }
            set { __VisitDayValue = value; }
        }

        public List<SalesDay> SalesCollectionDay()
        {
            SalesDay objSalesDay;
            List<SalesDay> lstSalesDay = new List<SalesDay>();

            for (int i = 1; i < 31; i++)
            {
                objSalesDay = new SalesDay();
                objSalesDay.VisitDay = i.ToString();
                objSalesDay.VisitDayValue = i.ToString();
                lstSalesDay.Add(objSalesDay);
            }

            return lstSalesDay;
        }

    }
}
