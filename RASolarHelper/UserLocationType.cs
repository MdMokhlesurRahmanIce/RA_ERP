using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarHelper
{
    public class UserLocationType
    {
        private string __LocationTypeID = string.Empty;
        private string __LocationTypeText = string.Empty;

        public string LocationTypeID
        {
            set { __LocationTypeID = value; }
            get { return __LocationTypeID; }
        }

        public string LocationTypeText
        {
            set { __LocationTypeText = value; }
            get { return __LocationTypeText; }
        }

        public List<UserLocationType> UserLocationTypeFormat()
        {
            UserLocationType objUserLocationType;
            List<UserLocationType> lstUserLocationType = new List<UserLocationType>();

            objUserLocationType = new UserLocationType();
            objUserLocationType.LocationTypeID = "ho";
            objUserLocationType.LocationTypeText = "Head Office";
            lstUserLocationType.Add(objUserLocationType);

            objUserLocationType = new UserLocationType();
            objUserLocationType.LocationTypeID = "zone";
            objUserLocationType.LocationTypeText = "Zonal Office";
            lstUserLocationType.Add(objUserLocationType);

            objUserLocationType = new UserLocationType();
            objUserLocationType.LocationTypeID = "region";
            objUserLocationType.LocationTypeText = "Regonal Office";
            lstUserLocationType.Add(objUserLocationType);

            objUserLocationType = new UserLocationType();
            objUserLocationType.LocationTypeID = "unit";
            objUserLocationType.LocationTypeText = "Unit Office";
            lstUserLocationType.Add(objUserLocationType);

            return lstUserLocationType;
        }

        public List<UserLocationType> UserLocationTypeFormatChallan(string issueTypeId)
        {
            UserLocationType objUserLocationType;
            List<UserLocationType> lstUserLocationType = new List<UserLocationType>();

            if (issueTypeId == "ISSTOHEADO")
            {
                objUserLocationType = new UserLocationType();
                objUserLocationType.LocationTypeID = "ho";
                objUserLocationType.LocationTypeText = "Head Office";
                lstUserLocationType.Add(objUserLocationType);
            }
            else if (issueTypeId == "ISSTOZNLOF")
            {

                objUserLocationType = new UserLocationType();
                objUserLocationType.LocationTypeID = "zone";
                objUserLocationType.LocationTypeText = "Zonal Office";
                lstUserLocationType.Add(objUserLocationType);
            }
            else if (issueTypeId == "ISSTOOTRUT")
            {
                objUserLocationType = new UserLocationType();
                objUserLocationType.LocationTypeID = "unit";
                objUserLocationType.LocationTypeText = "Unit Office";
                lstUserLocationType.Add(objUserLocationType);
            }

            else
            {
                objUserLocationType = new UserLocationType();
                objUserLocationType.LocationTypeID = "ho";
                objUserLocationType.LocationTypeText = "Head Office";
                lstUserLocationType.Add(objUserLocationType);

                objUserLocationType = new UserLocationType();
                objUserLocationType.LocationTypeID = "zone";
                objUserLocationType.LocationTypeText = "Zonal Office";
                lstUserLocationType.Add(objUserLocationType);

                objUserLocationType = new UserLocationType();
                objUserLocationType.LocationTypeID = "region";
                objUserLocationType.LocationTypeText = "Regonal Office";
                lstUserLocationType.Add(objUserLocationType);

                objUserLocationType = new UserLocationType();
                objUserLocationType.LocationTypeID = "unit";
                objUserLocationType.LocationTypeText = "Unit Office";
                lstUserLocationType.Add(objUserLocationType);
            }

            return lstUserLocationType;
        }
    }
}
