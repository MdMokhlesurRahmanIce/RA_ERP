using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RASolarHelper.SalesHelperModel
{
    public class PanelSerialList
    {
        string __ItemSerialNo = string.Empty;
        string __ItemModel = string.Empty;

        public string ItemSerialNo
        {
            set { __ItemSerialNo = value; }
            get { return __ItemSerialNo; }
        }

        public string ItemModel
        {
            set { __ItemModel = value; }
            get { return __ItemModel; }
        }
    }
}
