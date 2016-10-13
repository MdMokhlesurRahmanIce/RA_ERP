using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RASolarERP.DomainModel.InventoryModel
{
    public class ItemReceiveWithoutChallan
    {
        private string _itemCode = string.Empty;
        private string _itemName = string.Empty;
        private string _itemCategory = string.Empty;
        private string _itemModel = string.Empty;

        public string ItemCode
        {
            set { _itemCode = value; }
            get { return _itemCode; }

        }
        public string ItemName
        {
            set { _itemName = value; }
            get { return _itemName; }
        }
        public string ItemCategory
        {
            set { _itemCategory = value; }
            get { return _itemCategory; }
        }
        public string ItemModel
        {
            set { _itemModel = value; }
            get { return _itemModel; }
        }
    }
}
