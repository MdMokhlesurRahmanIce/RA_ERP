using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarHelper
{
    public class PeriodDurationInMonth
    {
        private string _MonthDurationName;
        private int _MonthDurationValue;

        public string MonthDurationName
        {
            get { return this._MonthDurationName; }
            set { _MonthDurationName = value; }
        }

        public int MonthDurationValue
        {
            get { return this._MonthDurationValue; }
            set { _MonthDurationValue = value; }
        }
        
        public List<PeriodDurationInMonth> PeriodDurationInMonthList()
        {
            PeriodDurationInMonth objPeriodDurationInMonth;
            List<PeriodDurationInMonth> lstPeriodDurationInMonth = new List<PeriodDurationInMonth>();

            objPeriodDurationInMonth = new PeriodDurationInMonth();
            objPeriodDurationInMonth.MonthDurationName = "1 Month";
            objPeriodDurationInMonth.MonthDurationValue = 1;

            lstPeriodDurationInMonth.Add(objPeriodDurationInMonth);

            string durationProcess = string.Empty;
            string[] years = { "Year", "Years" };
            string[] months = { "Month", "Months" };
            int yearIndex = 0, monthIndex = 0;

            for (int duration = 2; duration <= 30; duration++)
            {
                yearIndex = (duration / 12) < 2 ? 0 : 1;
                monthIndex = (duration % 12) == 1 ? 0 : 1;

                if (duration > 12 && (duration % 12 != 0))
                {
                    durationProcess = (duration / 12).ToString() + " " + years[yearIndex] + " " + (duration % 12).ToString() + " " + months[monthIndex];
                }
                else if ((duration % 12) == 0)
                {
                    durationProcess = (duration / 12).ToString() + " " + years[yearIndex];
                }
                else if (duration < 12)
                {
                    durationProcess = duration.ToString() + " " + months[monthIndex];
                }

                objPeriodDurationInMonth = new PeriodDurationInMonth();
                objPeriodDurationInMonth.MonthDurationName = durationProcess;
                objPeriodDurationInMonth.MonthDurationValue = duration;

                lstPeriodDurationInMonth.Add(objPeriodDurationInMonth);
            }

            return lstPeriodDurationInMonth;
        }
    }
}
