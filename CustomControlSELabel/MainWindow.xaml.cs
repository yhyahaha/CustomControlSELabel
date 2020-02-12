using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomControlSELabel
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var items = GetDivisions();
        }

        List<ISELabelItem> GetDivisions()
        {
            List<ISELabelItem> items = new List<ISELabelItem>();

            var dev1 = new Division("11000", "コーポ", "110", " 11000 ｺｰﾎﾟﾚｰﾄ", SELableDataType.String);
            var dev2 = new Division("13000", "制作", "130", " 13000 ｾｲｻｸ", SELableDataType.String);
            var dev3 = new Division("13500", "企画演出", "135", " 13500 ｷｶｸｴﾝｼｭﾂ", SELableDataType.String);
            var dev4 = new Division("14000", "撮影", "140", " 14000 ｻﾂｴｲ", SELableDataType.String);
            var dev5 = new Division("15000", "照明", "150", " 15000 ｼｮｳﾒｲ", SELableDataType.String);

            items.Add(dev1);
            items.Add(dev2);
            items.Add(dev3);
            items.Add(dev4);
            items.Add(dev5);

            return items;
        }

    }


}
