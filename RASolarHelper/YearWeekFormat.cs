using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarHelper
{
    public class YearWeekFormat
    {
        private string __YearWeekName;
        private string __YearWeekValue;

        public string YearWeekName
        {
            get { return __YearWeekName; }
            set { __YearWeekName = value; }
        }

        public string YearWeekValue
        {
            get { return __YearWeekValue; }
            set { __YearWeekValue = value; }
        }

        public List<YearWeekFormat> WeekFormat()
        {
            YearWeekFormat objYearWeekFormat;
            List<YearWeekFormat> lstYearWeekFormat = new List<YearWeekFormat>();

            objYearWeekFormat = new YearWeekFormat();
            objYearWeekFormat.YearWeekName = "--Select Week--";
            objYearWeekFormat.YearWeekValue = "0";
            lstYearWeekFormat.Add(objYearWeekFormat);

            int weekCounter = 33, initialWeek = 33;
            int yearCounter = 2012, initialYear = 2012, maxYear = 2013;
            string yearWeek = string.Empty;

            for (yearCounter = initialYear; yearCounter <= maxYear; yearCounter++)
            {
                if (yearCounter != 2012)
                    initialWeek = 1;

                for (weekCounter = initialWeek; weekCounter <= 52; weekCounter++)
                {
                    if (weekCounter > 9)
                    {
                        yearWeek = yearCounter.ToString() + weekCounter.ToString();
                    }
                    else
                    {
                        yearWeek = yearCounter.ToString() + weekCounter.ToString().PadLeft(2, '0');
                    }

                    objYearWeekFormat = new YearWeekFormat();

                    if (yearWeek == "201233")
                    {
                        objYearWeekFormat.YearWeekName = yearWeek + " [" + "01-Aug-2012" + " to " + "19-Aug-2012" + "]";
                        objYearWeekFormat.YearWeekValue = yearWeek;

                        lstYearWeekFormat.Add(objYearWeekFormat);
                        continue;
                    }

                    objYearWeekFormat.YearWeekName = yearWeek + " [" + Helper.YearWeekToStartDate(yearWeek).ToString("dd-MMM-yyyy") + " to " + Helper.YearWeekToEndDate(yearWeek).ToString("dd-MMM-yyyy") + "]";
                    objYearWeekFormat.YearWeekValue = yearWeek;

                    lstYearWeekFormat.Add(objYearWeekFormat);
                }
            }

            return lstYearWeekFormat;
        }
    }
}
