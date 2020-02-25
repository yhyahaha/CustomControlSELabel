using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;


namespace TestViewModel
{
    public class SelDivision : ISELabelItem
    {
        private string _ItemValue;
        public string ItemValue
        {
            get { return this._ItemValue; }
            set
            {
                if (value == _ItemValue) return;
                _ItemValue = value;
            }
        }

        private string _DisplayString;
        public string DisplayString
        {
            get { return this._DisplayString; }
            set
            {
                if (value == _DisplayString) return;
                _DisplayString = value;
            }
        }

        private string _SortKey;
        public string SortKey
        {
            get { return this._SortKey; }
            set
            {
                if (value == _SortKey) return;
                _SortKey = value;
            }
        }

        private string _SearchKeys;
        public string SearchKeys
        {
            get { return this._SearchKeys; }
            set
            {
                if (value == _SearchKeys) return;
                _SearchKeys = value;
            }
        }

        // コンストラクター
        public SelDivision(string code, string displayString, string sortKey, 
                        string searchKeys)
        {
            this.ItemValue = code;
            this.DisplayString = displayString;
            this.SortKey = sortKey;
            this.SearchKeys = searchKeys;
        }

        public override string ToString()
        {
            return _DisplayString + "(override ToString)";
        }

    }
}
