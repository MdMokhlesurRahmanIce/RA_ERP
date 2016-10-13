﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarHelper.HRMSHelperModel
{
    public class EmployeeDetailsInfo
    {
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string OperationalRole { get; set; }
        public string PresentLocation { get; set; }
        public string PresentLocationId { get; set; }
        public byte Status { get; set; }
    }    
}
