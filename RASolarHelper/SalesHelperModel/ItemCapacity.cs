using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarHelper.SalesHelperModel
{
    public class ItemCapacity
    {
        private string __ItemCategory = string.Empty;
        private string __ItemCapacity = string.Empty;
        private string __CapacityDescription = string.Empty;

        public string ItemCategoryID
        {
            set { __ItemCategory = value; }
            get { return __ItemCategory; }
        }

        public string ItemCapacityID
        {
            set { __ItemCapacity = value; }
            get { return __ItemCapacity; }
        }

        public string CapacityDescription
        {
            set { __CapacityDescription = value; }
            get { return __CapacityDescription; }
        }
    }
}
