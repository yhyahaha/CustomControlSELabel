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
        public ObservableCollection<ISELabelItem> Items { get; }
        = new ObservableCollection<ISELabelItem>();

        public ViewModel()
        {
            CreateDivisionsSampleData();
            Item = null;
            Item = Items[1];
        }

        private ISELabelItem _Item;
        public ISELabelItem Item
        {
            get { return _Item; }
            set
            {
                _Item = value;
            }
        }

        void CreateDivisionsSampleData()
        {
            var dev1 = new Division(11000, "コーポ", "110", " 11000 ｺｰﾎﾟﾚｰﾄ", typeof(string));
            var dev2 = new Division(13000, "制作", "130", " 13000 ｾｲｻｸ", typeof(string));
            var dev3 = new Division(13500, "企画演出", "135", " 13500 ｷｶｸｴﾝｼｭﾂ", typeof(string));
            var dev4 = new Division(14000, "撮影", "140", " 14000 ｻﾂｴｲ", typeof(string));
            var dev5 = new Division(15000, "照明", "150", " 15000 ｼｮｳﾒｲ", typeof(string));

            Items.Add(dev1);
            Items.Add(dev2);
            Items.Add(dev3);
            Items.Add(dev4);
            Items.Add(dev5);
        }
    }
}
