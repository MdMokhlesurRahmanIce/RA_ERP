using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RASolarHRMS.Model;

namespace RASolarERP.Web.Areas.HRMS.Models
{
    public class HRMSBaseData
    {
        protected IRASolarHRMSService _raSolarHrmsService;

        public HRMSBaseData() { }

        public HRMSBaseData(IRASolarHRMSService service)
            : this()
        {
            _raSolarHrmsService = service;
        }

        public IRASolarHRMSService HRMSService
        {
            get
            {
                if (_raSolarHrmsService == null)
                {
                    _raSolarHrmsService = new RASolarHRMSService() { };
                }

                return _raSolarHrmsService;

            }
            set { _raSolarHrmsService = value; }
        }
    }
}