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
using SELabelControl;
using TestViewModel;

namespace CustomControlSELabel
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ViewModel _viewModel = new ViewModel();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = _viewModel;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            message.Text = "Is Item Null? " + (_viewModel.Item == null).ToString();
            
            
            if(_viewModel.Item != null)
            {
                message.Text += Environment.NewLine;
                message.Text += "ItemValue: " +_viewModel.Item.ItemValue + Environment.NewLine;
                message.Text += "Disp: " + _viewModel.Item.DisplayString + Environment.NewLine;
                message.Text += "SortKey: " + _viewModel.Item.SortKey + Environment.NewLine;
                message.Text += "SearchKey: " + _viewModel.Item.SearchKeys + Environment.NewLine;

            }
        }
    }
}
