using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Interfaces;

namespace TestViewModel
{

    public class ViewModel
    {
        public static List<ISELabelItem> SelItems { get; private set; }
        = new List<ISELabelItem>();

        public ViewModel()
        {
            CreateDivisionsSampleData();
            Item = null;
            Item = SelItems[1].ItemValue;
        }

        private object _Item;
        public object Item
        {
            get { return _Item; }
            set
            {
                _Item = value;
            }
        }

        void CreateDivisionsSampleData()
        {
            var dev1 = new SelDivision("11000", "コーポ", "110", " 11000 ｺｰﾎﾟﾚｰﾄ");
            var dev2 = new SelDivision("13000", "制作", "130", " 13000 ｾｲｻｸ");
            var dev3 = new SelDivision("13500", "企画演出", "135", " 13500 ｷｶｸｴﾝｼｭﾂ");
            var dev4 = new SelDivision("14000", "撮影", "140", " 14000 ｻﾂｴｲ");
            var dev5 = new SelDivision("15000", "照明", "150", " 15000 ｼｮｳﾒｲ");

            SelItems.Add(dev1);
            SelItems.Add(dev2);
            SelItems.Add(dev3);
            SelItems.Add(dev4);
            SelItems.Add(dev5);
        }
    }
}
