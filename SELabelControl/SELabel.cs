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
                _textBoxKeywordElement = value;
            }
        }


        /****************************
           依存関係プロパティ
        *****************************/

        // SeValue ( ISELabelItem ) 定義上はISELabelItemが使えなかったのでobject型
        public static readonly DependencyProperty SeValueProperty =
            DependencyProperty.Register("SeValue", typeof(object), typeof(SELabel));

        public object SeValue
        {
            get { return GetValue(SeValueProperty); }
            set { SetValue(SeValueProperty, value); }
        }

        // 検索してリストから選択するか、入力値をISELabelオブジェクトとして返すか
        public static readonly DependencyProperty IsSeValueFromListProperty =
            DependencyProperty.Register("IsSeValueFromList", typeof(bool), typeof(SELabel));

        public bool IsSeValueFromList
        {
            get { return (bool)GetValue(IsSeValueFromListProperty); }
            set { SetValue(IsSeValueFromListProperty, value); }
        }


        /****************************
           プロパティ
        *****************************/

        // SelItems又はSelObjectの設定は必須

        /// <summary>
        /// 選択候補
        /// </summary>
        public List<ISELabelItem> SelItems { get; set; }

        /// <summary>
        /// 入力値をValueとするオブジェクト
        /// </summary>
        public ISELabelItem SelObject { get; set; }



        /***************************
            Private Methods
        ****************************/

        void ResetControl()
        {
            _status = SELabelStatus.Default;
            ChangeSELabelFunction();

        }

        void ChangeSELabelFunction()
        {
            WL("ChangeSELabelFunction");

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
            if((SelItems == null) && (SelObject == null))
            {
                throw new Exception("SelItems か SelObject を指定していなければならない");
            }
            
            
            ResetControl();
            System.Diagnostics.Debug.WriteLine("SelItems"  + (SelItems == null));
            System.Diagnostics.Debug.WriteLine("SelObject" + (SelObject == null));
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
                WL("Esc");

                SeValue = null;
                _status = SELabelStatus.Editing;
                ChangeSELabelFunction();
            }
        }

        // --- KeybordFocusEvent ---

        private void SELabel_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            WL("SELabel_IsKeyboardFocusWithinChanged");
            
            if (this.IsKeyboardFocusWithin)
            {
                if(SeValue != null)
                {
                    _status = SELabelStatus.Selected;
                    ChangeSELabelFunction();
                }
                else
                {
                    _status = SELabelStatus.Editing;
                    ChangeSELabelFunction();
                }
            }
            else
            {
                _status = SELabelStatus.Default;    
                ChangeSELabelFunction();
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
