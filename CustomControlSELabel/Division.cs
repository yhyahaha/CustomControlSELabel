using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomControlSELabel
{
    class Division:ISELabelItem
    {
        private int _Code;
        public int Code
        {
            get { return this._Code; }
            private set
            {
                if (value == _Code) return;
                _Code = value;
            }
        }

        private string _DisplayString;
        public string DisplayString
        {
            get { return this._DisplayString; }
            private set
            {
                if (value == _DisplayString) return;
                _DisplayString = value;
            }
        }

        private string _SortKey;
        public string SortKey
        {
            get { return this._SortKey; }
            private set
            {
                if (value == _SortKey) return;
                _SortKey = value;
            }
        }

        private string _SearchKeys;
        public string SearchKeys
        {
            get { return this._SearchKeys; }
            private set
            {
                if (value == _SearchKeys) return;
                _SearchKeys = value;
            }
        }

        private SELableDataType _DataType;
        public SELableDataType DataType
        {
            get { return _DataType; }
            set
            {
                if (value == _DataType) return;
                _DataType = value;
            }
        }

        // コンストラクター
        public Division(int code, string displayString, string sortKey, 
                        string searchKeys, SELableDataType dataType)
        {
            this.Code = code;
            this.DisplayString = displayString;
            this.SortKey = sortKey;
            this.SearchKeys = searchKeys;
            this.DataType = dataType;
        }

    }
}
