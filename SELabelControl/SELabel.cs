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
        // フィールド
        SELabelStatus _status = SELabelStatus.Default;
        Brush backgroundBrushControlHasFocus = Brushes.AliceBlue;       

        static SELabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SELabel), new FrameworkPropertyMetadata(typeof(SELabel)));
            System.Diagnostics.Debug.WriteLine("static SELabel");
        }

        public SELabel()
        {
            this.IsKeyboardFocusWithinChanged += SELabel_IsKeyboardFocusWithinChanged;

            this.Loaded += SELabel_Loaded;
        }

        private void SELabel_Loaded(object sender, RoutedEventArgs e)
        {
            ResetControl();
        }

        private void SELabel_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
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

        void ChangeSELabelFunction()
        {
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

        void ResetControl()
        {
            _status = SELabelStatus.Default;
            ChangeSELabelFunction();
        }

        // Elementsの定義
        public override void OnApplyTemplate()
        {
            WL("public override void OnApplyTemplate");

            base.OnApplyTemplate();

            labelItemElement = GetTemplateChild("labelItem") as Label;
            textBoxKeywordElement = GetTemplateChild("textBoxKeyword") as TextBox;
        }

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

        // 依存関係プロパティの定義
        public static readonly DependencyProperty SeValueProperty =
            DependencyProperty.Register("SeValue", typeof(object), typeof(SELabel));

        public object SeValue
        {
            get { return GetValue(SeValueProperty); }
            set
            {
                SetValue(SeValueProperty, value);
            }
        }

        enum SELabelStatus
        {
            Default,
            Selected,
            Editing
        }

        private void WL(string str)
        {
            str = DateTime.Now.ToString("mm:ss",CultureInfo.CurrentCulture) + " " + str;
            Console.WriteLine(str);
        }
    }

}
