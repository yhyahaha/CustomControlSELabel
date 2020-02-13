﻿using System;
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
                if(labelItemElement.Content != null)
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
                    break;
                case SELabelStatus.Editing:
                    System.Diagnostics.Debug.WriteLine("EDITING");
                    break;
                case SELabelStatus.Default:
                    System.Diagnostics.Debug.WriteLine("DEFAULT");
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
            textBoxKeyWordElement = GetTemplateChild("textBoxKeywrod") as TextBox;
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

        private TextBox _textBoxKeyWordElement;
        private TextBox textBoxKeyWordElement
        {
            get { return _textBoxKeyWordElement; }
            set
            {
                _textBoxKeyWordElement = value;
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
