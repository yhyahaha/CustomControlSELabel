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
using System.Collections.ObjectModel;
using Interfaces;
using SELabelControl;
using TestViewModel;


namespace CustomControlSELabel
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<ISELabelItem> Items { get; set; }
        = new ObservableCollection<ISELabelItem>();

        private ISELabelItem _Item;
        public ISELabelItem Item
        {
            get { return _Item; }
            set
            {
                _Item = value;
            }
        }
        
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GetDivisions();
            Item = Items[1];

            this.DataContext = Items;

            SELabel sELabel = new SELabel();
            Grid.SetColumn(sELabel, 1);
            Grid.SetRow(sELabel, 1);
            grid.Children.Add(sELabel);

        }

        void GetDivisions()
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
