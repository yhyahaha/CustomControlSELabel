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
using Interfaces;

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
        }

        public SELabel()
        {
            this.IsKeyboardFocusWithinChanged += SELabel_IsKeyboardFocusWithinChanged;
        }

        private void SELabel_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsKeyboardFocusWithin)
            {
                if(SxValue != null)
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
                    System.Diagnostics.Debug.WriteLine("SELECTED");

                    labelItemElement.Visibility = Visibility.Visible;
                    labelItemElement.Background = backgroundBrushControlHasFocus;

                    textBoxKeywordElement.Visibility = Visibility.Collapsed;

                    break;
                case SELabelStatus.Editing:
                    System.Diagnostics.Debug.WriteLine("EDITING");

                    labelItemElement.Visibility = Visibility.Collapsed;

                    textBoxKeywordElement.Visibility = Visibility.Visible;
                    textBoxKeywordElement.Background = backgroundBrushControlHasFocus;

                    break;
                case SELabelStatus.Default:
                    System.Diagnostics.Debug.WriteLine("DEFAULT");

                    labelItemElement.Visibility = Visibility.Visible;
                    labelItemElement.Background = Brushes.Transparent;

                    textBoxKeywordElement.Visibility = Visibility.Collapsed;
                    
                    break;
                default:
                    break;
            }
        }

        // Elements
        public override void OnApplyTemplate()
        {
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
        //public static readonly DependencyProperty SIValueProperty =
        //    DependencyProperty.Register("SIValue", typeof(object), typeof(SELabel));

        //public object SIValue
        //{
        //    get { return (object)GetValue(SIValueProperty); }
        //    set 
        //    { 
        //        SetValue(SIValueProperty, value);
        //        _status = SELabelStatus.Default;
        //        ChangeSELabelFunction();
        //    }
        //}

        public static readonly DependencyProperty SxValueProperty =
            DependencyProperty.Register("SxValue", typeof(string), typeof(SELabel));

        public string SxValue
        {
            get { return (string)GetValue(SxValueProperty); }
            set
            {
                SetValue(SxValueProperty, value);
                _status = SELabelStatus.Default;
                ChangeSELabelFunction();
            }
        }


        enum SELabelStatus
        {
            Default,
            Selected,
            Editing
        }
    }

}
