using Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace SELabelControl
{
    public class SELabel : Control
    {

     /***************************
        フィールド
     ****************************/

        enum SELabelStatus { Default, Selected, Editing }           // コントロールの状態
        SELabelStatus _status = SELabelStatus.Default;
        
        Brush backgroundBrushControlHasFocus = Brushes.AliceBlue;   // コントロールがフォーカスを得ている状態の背景色

        static RomajiToKana kanaConverter = new RomajiToKana();

     /***************************
        コンストラクター
     ****************************/
        static SELabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SELabel), new FrameworkPropertyMetadata(typeof(SELabel)));
            System.Diagnostics.Debug.WriteLine("static SELabel");
        }

        public SELabel()
        {
            this.IsKeyboardFocusWithinChanged += SELabel_IsKeyboardFocusWithinChanged;

            this.Loaded += SELabel_Loaded;
            this.PreviewMouseLeftButtonDown += SELabel_PreviewMouseLeftButtonDown;
            this.KeyDown += SELabel_KeyDown;

            //System.Diagnostics.Debug.WriteLine("SelItems" + SelItems.Count);
        }

        /***************************
            Elements の定義
        ****************************/

        private Label _labelItemElement;
        private Label labelItemElement
        {
            get { return _labelItemElement; }
            set
            {
                _labelItemElement = value;
            }
        }

        private TextBox _textBoxKeywordElement;
        private TextBox textBoxKeywordElement
        {
            get { return _textBoxKeywordElement; }
            set
            {
                if(_textBoxKeywordElement != null)
                {
                    _textBoxKeywordElement.TextChanged
                        -= new TextChangedEventHandler(_textBoxKeywordElement_TextChanged);
                }
                
                _textBoxKeywordElement = value;

                if(_textBoxKeywordElement != null)
                {
                    _textBoxKeywordElement.TextChanged
                        += new TextChangedEventHandler(_textBoxKeywordElement_TextChanged);
                }
                
            }
        }


        private void _textBoxKeywordElement_TextChanged(object sender, TextChangedEventArgs e)
        {
            WL("textBox Changed " + textBoxKeywordElement.Text);
            string str = textBoxKeywordElement.Text;
            textBoxKeywordElement.Text = kanaConverter.ConvertToKana(str);
            textBoxKeywordElement.CaretIndex = str.Length;
        }


        /****************************
           依存関係プロパティ
        *****************************/

        public static readonly DependencyProperty SeValueProperty =
            DependencyProperty.Register("SeValue", typeof(string), typeof(SELabel));

        public string SeValue
        {
            get { return (string)GetValue(SeValueProperty); }
            set { SetValue(SeValueProperty, value); }
        }

        /****************************
           プロパティ
        *****************************/

        /// <summary>
        /// 選択候補 / 設定必須
        /// </summary>
        public List<ISELabelItem> SeItems { get; set; }


        /***************************
            Private Methods
        ****************************/

        // コントロールがフォーカスを持たない時の状態
        void ResetControl()
        {
            SetDisplayString(SeValue);
            UpdateSeLabelStatus(SELabelStatus.Default);
        }

        // SeValueを表示する
        void SetDisplayString(string value)
        {
            string displayString = string.Empty;

            var item = SeItems.Where(x => x.ItemValue == value).FirstOrDefault();
            if (item != null)
            {
                displayString = item.ToString();
            }

            labelItemElement.Content = displayString;

        }


        // ElementのVisibility,Backgroud等を操作する
        void UpdateSeLabelStatus(SELabelStatus status)
        {
            WL("UpdateSeLabelStatus from " + _status.ToString() + " to " + status.ToString() );

            _status = status;

            switch (_status)
            {
                case SELabelStatus.Selected:
                    labelItemElement.Visibility = Visibility.Visible;
                    labelItemElement.Background = backgroundBrushControlHasFocus;

                    textBoxKeywordElement.Visibility = Visibility.Collapsed;

                    break;

                case SELabelStatus.Editing:
                    labelItemElement.Visibility = Visibility.Collapsed;

                    textBoxKeywordElement.Visibility = Visibility.Visible;
                    textBoxKeywordElement.Background = backgroundBrushControlHasFocus;

                    textBoxKeywordElement.Focus();
                    textBoxKeywordElement.SelectAll();

                    break;

                case SELabelStatus.Default:
                    labelItemElement.Visibility = Visibility.Visible;
                    labelItemElement.Background = Brushes.Transparent;

                    textBoxKeywordElement.Visibility = Visibility.Collapsed;

                    break;

                default:
                    break;
            }
        }


        /***************************
               SELabel本体のEvent
        ****************************/

        // --- Initialize ---

        // -> static SELabel()
        // -> public SELabel()

        public override void OnApplyTemplate()
        {
            WL("public override void OnApplyTemplate");

            base.OnApplyTemplate();

            labelItemElement = GetTemplateChild("labelItem") as Label;
            textBoxKeywordElement = GetTemplateChild("textBoxKeyword") as TextBox;
        }

        private void SELabel_Loaded(object sender, RoutedEventArgs e)
        {
            // SelItems か SelObject を指定していなければならない
            if(SeItems == null)
            {
                throw new Exception("SelItemsを指定していなければならない");
            }
            
            if (SeValue == null)
            {
                //throw new Exception("SeValueはNullであってはならない");
                SeValue = string.Empty;
            }
            
            ResetControl();
        }

        // --- MouseEvent ---

        private void SELabel_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WL("SELabel_PreviewMouseLeftButtonDown");

            this.Focus();

            // → SELabel_IsKeyboardFocusWithinChanged ... _status is selected
            // → ChangeSELabelFunction
        }

        // -> SELabel_PreviewMouseDown
        // -> SELabel_IsKeyboardFocusWithinChanged
        // -> SELabel_PreviewMouseLeftButtonUp
        // -> SELabel_PreviewMouseUp  

        // ×　SELabel_MouseDown  
        // ×　SELabel_MouseUp


        // --- KeyEvent ---
        private void SELabel_KeyDown(object sender, KeyEventArgs e)
        {
            // Delete で Item を削除(Null)
            if (e.Key == Key.Delete)
            {
                WL("Del");

                SeValue = string.Empty;
                SetDisplayString(SeValue);
                UpdateSeLabelStatus(SELabelStatus.Editing);
            }


        }

        // --- KeybordFocusEvent ---

        private void SELabel_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            WL("SELabel_IsKeyboardFocusWithinChanged");
            
            if (this.IsKeyboardFocusWithin)
            {
                if (!string.IsNullOrEmpty(SeValue))
                {
                    UpdateSeLabelStatus(SELabelStatus.Selected);
                }
                else
                {
                    UpdateSeLabelStatus(SELabelStatus.Editing);
                }
            }
            else
            {
                UpdateSeLabelStatus(SELabelStatus.Default);
            }
        }


     /***************************
            開発用 Method
     ****************************/

        private void WL(string str)
        {
            str = DateTime.Now.ToString("mm:ss",CultureInfo.CurrentCulture) + " " + str;
            Console.WriteLine(str);
        }
    }

}
