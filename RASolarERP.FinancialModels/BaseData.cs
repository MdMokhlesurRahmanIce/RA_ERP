using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RASolarAMS.Model;

namespace RASolarERP.Web.Areas.Financial.Models
{
    public class BaseData
    {
        protected IRASolarAMSService _raSolarAmsService;

        public BaseData() { }

        public BaseData(IRASolarAMSService service)
            : this()
        {
            _raSolarAmsService = service;
        }

        public IRASolarAMSService AMSService
        {
            get
            {
                if (_raSolarAmsService == null)
                {
                    _raSolarAmsService = new RASolarAMSService() { };
                }

                return _raSolarAmsService;

            }
            set { _raSolarAmsService = value; }
        }

    }
}