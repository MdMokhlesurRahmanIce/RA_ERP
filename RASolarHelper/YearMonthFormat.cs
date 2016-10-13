using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarHelper
{
    public class YearMonthFormat
    {
        private string __YearMonthName;
        private string __YearMonthValue;

        public string YearMonthName
        {
            get { return __YearMonthName; }
            set { __YearMonthName = value; }
        }

        public string YearMonthValue
        {
            get { return __YearMonthValue; }
            set { __YearMonthValue = value; }
        }

        public List<YearMonthFormat> MonthFormat()
        {
            YearMonthFormat objMonthYearFormat;
            List<YearMonthFormat> lstMonthYearFormat = new List<YearMonthFormat>();

            //objMonthYearFormat = new YearMonthFormat();
            //objMonthYearFormat.YearMonthName = "--Select Month--";
            //objMonthYearFormat.YearMonthValue = "0";
            //lstMonthYearFormat.Add(objMonthYearFormat);

            //objMonthYearFormat = new YearMonthFormat();
            //objMonthYearFormat.YearMonthName = "January/2013";
            //objMonthYearFormat.YearMonthValue = "201301";
            //lstMonthYearFormat.Add(objMonthYearFormat);

            //objMonthYearFormat = new YearMonthFormat();
            //objMonthYearFormat.YearMonthName = "February/2013";
            //objMonthYearFormat.YearMonthValue = "201302";
            //lstMonthYearFormat.Add(objMonthYearFormat);

            //objMonthYearFormat = new YearMonthFormat();
            //objMonthYearFormat.YearMonthName = "March/2013";
            //objMonthYearFormat.YearMonthValue = "201303";
            //lstMonthYearFormat.Add(objMonthYearFormat);

            //objMonthYearFormat = new YearMonthFormat();
            //objMonthYearFormat.YearMonthName = "April/2013";
            //objMonthYearFormat.YearMonthValue = "201304";
            //lstMonthYearFormat.Add(objMonthYearFormat);

            //objMonthYearFormat = new YearMonthFormat();
            //objMonthYearFormat.YearMonthName = "May/2013";
            //objMonthYearFormat.YearMonthValue = "201305";
            //lstMonthYearFormat.Add(objMonthYearFormat);

            //objMonthYearFormat = new YearMonthFormat();
            //objMonthYearFormat.YearMonthName = "June/2013";
            //objMonthYearFormat.YearMonthValue = "201306";
            //lstMonthYearFormat.Add(objMonthYearFormat);

            //objMonthYearFormat = new YearMonthFormat();
            //objMonthYearFormat.YearMonthName = "July/2013";
            //objMonthYearFormat.YearMonthValue = "201307";
            //lstMonthYearFormat.Add(objMonthYearFormat);

            //objMonthYearFormat = new YearMonthFormat();
            //objMonthYearFormat.YearMonthName = "August/2013";
            //objMonthYearFormat.YearMonthValue = "201308";
            //lstMonthYearFormat.Add(objMonthYearFormat);

            //objMonthYearFormat = new YearMonthFormat();
            //objMonthYearFormat.YearMonthName = "September/2013";
            //objMonthYearFormat.YearMonthValue = "201309";
            //lstMonthYearFormat.Add(objMonthYearFormat);

            //objMonthYearFormat = new YearMonthFormat();
            //objMonthYearFormat.YearMonthName = "October/2013";
            //objMonthYearFormat.YearMonthValue = "201310";
            //lstMonthYearFormat.Add(objMonthYearFormat);

            //objMonthYearFormat = new YearMonthFormat();
            //objMonthYearFormat.YearMonthName = "November/2013";
            //objMonthYearFormat.YearMonthValue = "201311";
            //lstMonthYearFormat.Add(objMonthYearFormat);

            //objMonthYearFormat = new YearMonthFormat();
            //objMonthYearFormat.YearMonthName = "December/2013";
            //objMonthYearFormat.YearMonthValue = "201312";
            //lstMonthYearFormat.Add(objMonthYearFormat);

            //objMonthYearFormat = new YearMonthFormat();
            //objMonthYearFormat.YearMonthName = "January/2014";
            //objMonthYearFormat.YearMonthValue = "201401";
            //lstMonthYearFormat.Add(objMonthYearFormat);

            //objMonthYearFormat = new YearMonthFormat();
            //objMonthYearFormat.YearMonthName = "February/2014";
            //objMonthYearFormat.YearMonthValue = "201402";
            //lstMonthYearFormat.Add(objMonthYearFormat);

            //objMonthYearFormat = new YearMonthFormat();
            //objMonthYearFormat.YearMonthName = "March/2014";
            //objMonthYearFormat.YearMonthValue = "201403";
            //lstMonthYearFormat.Add(objMonthYearFormat);

            //objMonthYearFormat = new YearMonthFormat();
            //objMonthYearFormat.YearMonthName = "April/2014";
            //objMonthYearFormat.YearMonthValue = "201404";
            //lstMonthYearFormat.Add(objMonthYearFormat);

            //objMonthYearFormat = new YearMonthFormat();
            //objMonthYearFormat.YearMonthName = "May/2014";
            //objMonthYearFormat.YearMonthValue = "201405";
            //lstMonthYearFormat.Add(objMonthYearFormat);

            //objMonthYearFormat = new YearMonthFormat();
            //objMonthYearFormat.YearMonthName = "June/2014";
            //objMonthYearFormat.YearMonthValue = "201406";
            //lstMonthYearFormat.Add(objMonthYearFormat);

            //objMonthYearFormat = new YearMonthFormat();
            //objMonthYearFormat.YearMonthName = "July/2014";
            //objMonthYearFormat.YearMonthValue = "201407";
            //lstMonthYearFormat.Add(objMonthYearFormat);

            //objMonthYearFormat = new YearMonthFormat();
            //objMonthYearFormat.YearMonthName = "August/2014";
            //objMonthYearFormat.YearMonthValue = "201408";
            //lstMonthYearFormat.Add(objMonthYearFormat);

            //objMonthYearFormat = new YearMonthFormat();
            //objMonthYearFormat.YearMonthName = "September/2014";
            //objMonthYearFormat.YearMonthValue = "201409";
            //lstMonthYearFormat.Add(objMonthYearFormat);

            //objMonthYearFormat = new YearMonthFormat();
            //objMonthYearFormat.YearMonthName = "October/2014";
            //objMonthYearFormat.YearMonthValue = "201410";
            //lstMonthYearFormat.Add(objMonthYearFormat);

            //objMonthYearFormat = new YearMonthFormat();
            //objMonthYearFormat.YearMonthName = "November/2014";
            //objMonthYearFormat.YearMonthValue = "201411";
            //lstMonthYearFormat.Add(objMonthYearFormat);

            //objMonthYearFormat = new YearMonthFormat();
            //objMonthYearFormat.YearMonthName = "December/2014";
            //objMonthYearFormat.YearMonthValue = "201412";
            //lstMonthYearFormat.Add(objMonthYearFormat);
            
            objMonthYearFormat = new YearMonthFormat();
            objMonthYearFormat.YearMonthName = "--Select Month--";
            objMonthYearFormat.YearMonthValue = "0";
            lstMonthYearFormat.Add(objMonthYearFormat);

           

            objMonthYearFormat = new YearMonthFormat();
            objMonthYearFormat.YearMonthName = "January/2014";
            objMonthYearFormat.YearMonthValue = "201401";
            lstMonthYearFormat.Add(objMonthYearFormat);

            objMonthYearFormat = new YearMonthFormat();
            objMonthYearFormat.YearMonthName = "February/2014";
            objMonthYearFormat.YearMonthValue = "201402";
            lstMonthYearFormat.Add(objMonthYearFormat);

            objMonthYearFormat = new YearMonthFormat();
            objMonthYearFormat.YearMonthName = "March/2014";
            objMonthYearFormat.YearMonthValue = "201403";
            lstMonthYearFormat.Add(objMonthYearFormat);

            objMonthYearFormat = new YearMonthFormat();
            objMonthYearFormat.YearMonthName = "April/2014";
            objMonthYearFormat.YearMonthValue = "201404";
            lstMonthYearFormat.Add(objMonthYearFormat);

            objMonthYearFormat = new YearMonthFormat();
            objMonthYearFormat.YearMonthName = "May/2014";
            objMonthYearFormat.YearMonthValue = "201405";
            lstMonthYearFormat.Add(objMonthYearFormat);

            objMonthYearFormat = new YearMonthFormat();
            objMonthYearFormat.YearMonthName = "June/2014";
            objMonthYearFormat.YearMonthValue = "201406";
            lstMonthYearFormat.Add(objMonthYearFormat);

            objMonthYearFormat = new YearMonthFormat();
            objMonthYearFormat.YearMonthName = "July/2014";
            objMonthYearFormat.YearMonthValue = "201407";
            lstMonthYearFormat.Add(objMonthYearFormat);

            objMonthYearFormat = new YearMonthFormat();
            objMonthYearFormat.YearMonthName = "August/2014";
            objMonthYearFormat.YearMonthValue = "201408";
            lstMonthYearFormat.Add(objMonthYearFormat);

            objMonthYearFormat = new YearMonthFormat();
            objMonthYearFormat.YearMonthName = "September/2014";
            objMonthYearFormat.YearMonthValue = "201409";
            lstMonthYearFormat.Add(objMonthYearFormat);

            objMonthYearFormat = new YearMonthFormat();
            objMonthYearFormat.YearMonthName = "October/2014";
            objMonthYearFormat.YearMonthValue = "201410";
            lstMonthYearFormat.Add(objMonthYearFormat);

            objMonthYearFormat = new YearMonthFormat();
            objMonthYearFormat.YearMonthName = "November/2014";
            objMonthYearFormat.YearMonthValue = "201411";
            lstMonthYearFormat.Add(objMonthYearFormat);

            objMonthYearFormat = new YearMonthFormat();
            objMonthYearFormat.YearMonthName = "December/2014";
            objMonthYearFormat.YearMonthValue = "201412";
            lstMonthYearFormat.Add(objMonthYearFormat);

            objMonthYearFormat = new YearMonthFormat();
            objMonthYearFormat.YearMonthName = "January/2015";
            objMonthYearFormat.YearMonthValue = "201501";
            lstMonthYearFormat.Add(objMonthYearFormat);

            objMonthYearFormat = new YearMonthFormat();
            objMonthYearFormat.YearMonthName = "February/2015";
            objMonthYearFormat.YearMonthValue = "201502";
            lstMonthYearFormat.Add(objMonthYearFormat);

            objMonthYearFormat = new YearMonthFormat();
            objMonthYearFormat.YearMonthName = "March/2015";
            objMonthYearFormat.YearMonthValue = "201503";
            lstMonthYearFormat.Add(objMonthYearFormat);

            objMonthYearFormat = new YearMonthFormat();
            objMonthYearFormat.YearMonthName = "April/2015";
            objMonthYearFormat.YearMonthValue = "201504";
            lstMonthYearFormat.Add(objMonthYearFormat);

            objMonthYearFormat = new YearMonthFormat();
            objMonthYearFormat.YearMonthName = "May/2015";
            objMonthYearFormat.YearMonthValue = "201505";
            lstMonthYearFormat.Add(objMonthYearFormat);

            objMonthYearFormat = new YearMonthFormat();
            objMonthYearFormat.YearMonthName = "June/2015";
            objMonthYearFormat.YearMonthValue = "201506";
            lstMonthYearFormat.Add(objMonthYearFormat);

            objMonthYearFormat = new YearMonthFormat();
            objMonthYearFormat.YearMonthName = "July/2015";
            objMonthYearFormat.YearMonthValue = "201507";
            lstMonthYearFormat.Add(objMonthYearFormat);

            objMonthYearFormat = new YearMonthFormat();
            objMonthYearFormat.YearMonthName = "August/2015";
            objMonthYearFormat.YearMonthValue = "201508";
            lstMonthYearFormat.Add(objMonthYearFormat);

            objMonthYearFormat = new YearMonthFormat();
            objMonthYearFormat.YearMonthName = "September/2015";
            objMonthYearFormat.YearMonthValue = "201509";
            lstMonthYearFormat.Add(objMonthYearFormat);

            objMonthYearFormat = new YearMonthFormat();
            objMonthYearFormat.YearMonthName = "October/2015";
            objMonthYearFormat.YearMonthValue = "201510";
            lstMonthYearFormat.Add(objMonthYearFormat);

            objMonthYearFormat = new YearMonthFormat();
            objMonthYearFormat.YearMonthName = "November/2015";
            objMonthYearFormat.YearMonthValue = "201511";
            lstMonthYearFormat.Add(objMonthYearFormat);

            objMonthYearFormat = new YearMonthFormat();
            objMonthYearFormat.YearMonthName = "December/2015";
            objMonthYearFormat.YearMonthValue = "201512";
            lstMonthYearFormat.Add(objMonthYearFormat);
            return lstMonthYearFormat;
        }

        public List<YearMonthFormat> MonthList()
        {
            List<YearMonthFormat> lstMonthList = new List<YearMonthFormat> { 
                
                new YearMonthFormat{YearMonthName = "January", YearMonthValue = "1"},
                new YearMonthFormat{YearMonthName = "February", YearMonthValue = "2"},
                new YearMonthFormat{YearMonthName = "March", YearMonthValue = "3"},
                new YearMonthFormat{YearMonthName = "April", YearMonthValue = "4"},
                new YearMonthFormat{YearMonthName = "May", YearMonthValue = "5"},
                new YearMonthFormat{YearMonthName = "June", YearMonthValue = "6"},
                new YearMonthFormat{YearMonthName = "July", YearMonthValue = "7"},
                new YearMonthFormat{YearMonthName = "August", YearMonthValue = "8"},
                new YearMonthFormat{YearMonthName = "September", YearMonthValue = "9"},
                new YearMonthFormat{YearMonthName = "October", YearMonthValue = "10"},
                new YearMonthFormat{YearMonthName = "November", YearMonthValue = "11"},
                new YearMonthFormat{YearMonthName = "December", YearMonthValue = "12"}
            };

            return lstMonthList;
        }
    }
}
