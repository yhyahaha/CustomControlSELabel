using Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ConverterRomanLettersToKana;
using InterfaceKanaConverter;

namespace SELabelControl
{
    public class SELabel : Control
    {

        #region Field

        /***************************
           フィールド
        ****************************/

        enum SELabelStatus { Default, Selected, Editing }           // コントロールの状態
        SELabelStatus _status = SELabelStatus.Default;        
        Brush backgroundBrushControlHasFocus = Brushes.AliceBlue;   // コントロールがフォーカスを得ている状態の背景色

        #endregion

        #region Constractor

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
            this.Loaded += SELabel_Loaded;

            this.IsKeyboardFocusWithinChanged += SELabel_IsKeyboardFocusWithinChanged;            
            this.PreviewMouseLeftButtonDown += SELabel_PreviewMouseLeftButtonDown;
            this.PreviewKeyDown += SELabel_PreviewKeyDown;

            // Mouseイベントは直接ルートイベントであるためイベントをルートさせるための処理が必要
            AddHandler(FrameworkElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(SELabel_MouseLeftButtonDown), true);

            // 既定では中間一致検索
            _PrifixSearch = false;
        }

        #endregion

        #region Elementsの定義

        /***************************
            Elements の定義
        ****************************/

        private Label _labelItemElement;
        private Label labelItemElement
        {
            get { return _labelItemElement; }
            set { _labelItemElement = value; }
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

        private Popup _popupElement;
        public Popup popupElement
        {
            get { return _popupElement; }
            set { _popupElement = value; }
        }

        private ListBox _listBoxElement;
        public ListBox listBoxElement
        {
            get { return _listBoxElement; }
            set { _listBoxElement = value; }
        }

        #endregion

        #region DependencyProperty & Property

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

        private static IKanaConverter _KanaConverter;
        public IKanaConverter KanaConverter
        {
            get { return _KanaConverter; }
            set
            {
                if (_KanaConverter == null)
                {
                    _KanaConverter = value;
                }
            }
        }

        // 検索の前方一致を指定（規定ではFalse)
        private bool _PrifixSearch;
        public bool PrifixSearch
        {
            get { return _PrifixSearch; }
            set { _PrifixSearch = value; }
        }

        #endregion

        #region Event処理

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
            popupElement = GetTemplateChild("popup") as Popup;
            listBoxElement = GetTemplateChild("listBox") as ListBox;

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

            // コントロールフォーカスを取得
            if (!this.IsFocused)
            {
                this.Focus();
            }
        }

        private void SELabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WL("SELabel_MouseLeftButtonDown");

            if (_status == SELabelStatus.Editing)
            {
                // ListBoxでの選択中に...
                if (popupElement.IsOpen == true)
                {
                    // textBoxElementをクリック
                    if (textBoxKeywordElement.IsFocused)
                    {
                        listBoxElement.SelectedIndex = -1;
                    }
                    else
                    {
                        //ListBoxItemのクリックで確定
                        if(listBoxElement.SelectedIndex>-1)
                        {
                            var result = listBoxElement.SelectedItem.ToString();
                            var value = GetValueByDisplayString(result);
                            Commit(value);
                        }
                    }
                }
            }
        }

        // --- KeyEvent ---
        private void SELabel_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            WL("SELabel_PreviewKeyDown"); 
            
            // Delete : Itemを削除してEditngに設定
            if (e.Key == Key.Delete)
            {
                SeValue = string.Empty;
                SetDisplayString(SeValue);
                UpdateSeLabelStatus(SELabelStatus.Editing);
                e.Handled = true;
            }

            // Editing時のキー操作
            if (_status == SELabelStatus.Editing)
            {
                // Esc
                if (e.Key == Key.Escape)
                {
                    // KeyWord入力時　候補のリセット
                    if (textBoxKeywordElement.IsKeyboardFocusWithin)
                    {
                        listBoxElement.ItemsSource = null;
                        textBoxKeywordElement.Text = string.Empty;
                        e.Handled = true;
                    }

                    // List選択時　Keyword入力へ戻る
                    if (listBoxElement.IsKeyboardFocusWithin)
                    {
                        listBoxElement.ItemsSource = null;
                        popupElement.IsOpen = false;
                        textBoxKeywordElement.Focus();
                        e.Handled = true;
                    }
                }

                // 下矢印
                if (e.Key == Key.Down)
                {
                    // TextBoxElementからListboxElementへのフォーカス遷移
                    if (popupElement.IsOpen && listBoxElement.IsKeyboardFocusWithin == false)
                    {
                        listBoxElement.Focus();
                        listBoxElement.SelectedIndex = 0;
                    }
                }

                // 上矢印 TextBoxElementに戻る
                if(e.Key==Key.Up && listBoxElement.SelectedIndex==0)
                {
                    listBoxElement.SelectedIndex = -1;
                    textBoxKeywordElement.Focus();
                }

                // Enterキーで確定
                if (e.Key == Key.Enter && listBoxElement.SelectedIndex > -1)
                {
                    var result = listBoxElement.SelectedItem.ToString();
                    var value = GetValueByDisplayString(result);
                    Commit(value);
                }
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
                textBoxKeywordElement.Text = string.Empty;
                listBoxElement.ItemsSource = null;
            }
        }

        /************************************
               TextBoxKeyWordChangeEvent
        *************************************/

        private void _textBoxKeywordElement_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = textBoxKeywordElement.Text;
            //if (String.IsNullOrWhiteSpace(text)) return;
            //if (String.IsNullOrEmpty(text)) return;

            // ローマ字変換?
            string str = "";
            if(_KanaConverter != null) { str = _KanaConverter.ConvertToKana(text); }
            else                       { str = text; }

            // Ajust Input
            string keyword = AdjustToKeyword(str);
            textBoxKeywordElement.Text = keyword;
            textBoxKeywordElement.CaretIndex = keyword.Length;

            // Search by keyword
            
            // 前方一致検索ではkeywordの頭にスペースを挿入
            if (_PrifixSearch) {keyword = " " + keyword; }

            var candidates = SeItems.Where(x => x.SearchKeys.Contains(keyword))
                                    .OrderBy(x => x.SortKey)
                                    .Select(x => x.DisplayString);

            if (keyword.Trim().Length > 0 && candidates.Any())
            {
                listBoxElement.ItemsSource = candidates;
                popupElement.IsOpen = true;
            }
            else
            {
                listBoxElement.ItemsSource = null;
                popupElement.IsOpen = false;
            }

            // 完全なコード入力で確定
            var items = SeItems.Where(x => x.ItemValue == keyword.Trim()).Select(x => x.ItemValue);
            if(items !=null && items.Count() == 1)
            {
                string value = items.First();
                Commit(value);
            }
            
        }

        // 拗音,促音の補正
        private string AdjustToKeyword (string textString)
        {
            string str = textString.Replace('ｧ', 'ｱ')
                                   .Replace('ｨ', 'ｲ')
                                   .Replace('ｩ', 'ｳ')
                                   .Replace('ｪ', 'ｴ')
                                   .Replace('ｫ', 'ｵ')
                                   .Replace('ｬ', 'ﾔ')
                                   .Replace('ｭ', 'ﾕ')
                                   .Replace('ｮ', 'ﾖ')
                                   .Replace('ｯ', 'ﾂ');
            return str;
        }

        #endregion

        #region Methods

        /***************************
            Private Methods
        ****************************/

        // コントロールがフォーカスを持たない時の状態
        void ResetControl()
        {
            SetDisplayString(SeValue);
            textBoxKeywordElement.Text = string.Empty;
            listBoxElement.ItemsSource = null;
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
            WL("UpdateSeLabelStatus from " + _status.ToString() + " to " + status.ToString());

            _status = status;

            switch (_status)
            {
                case SELabelStatus.Selected:
                    labelItemElement.Visibility = Visibility.Visible;
                    labelItemElement.Background = backgroundBrushControlHasFocus;

                    textBoxKeywordElement.Visibility = Visibility.Collapsed;

                    popupElement.IsOpen = false;

                    break;

                case SELabelStatus.Editing:
                    labelItemElement.Visibility = Visibility.Collapsed;

                    textBoxKeywordElement.Visibility = Visibility.Visible;
                    textBoxKeywordElement.Background = backgroundBrushControlHasFocus;

                    textBoxKeywordElement.Focus();
                    textBoxKeywordElement.SelectAll();

                    InputMethod.Current.ImeState = InputMethodState.Off;

                    break;

                case SELabelStatus.Default:
                    labelItemElement.Visibility = Visibility.Visible;
                    labelItemElement.Background = Brushes.Transparent;

                    textBoxKeywordElement.Visibility = Visibility.Collapsed;

                    popupElement.IsOpen = false;

                    break;

                default:
                    break;
            }
        }

        // DisplayStringからitemValueを取得する
        string GetValueByDisplayString(string displayString)
        {
            return SeItems.Where(x => x.DisplayString == displayString).Select(x => x.ItemValue).FirstOrDefault();
        }

        //データの確定
        void Commit(string itemValue)
        {
            SeValue = itemValue;
            ResetControl();

            WL("Commit " + SeValue);
        }

        #endregion

        #region Utility

        /***************************
               開発用 Method
        ****************************/

        private void WL(string str)
        {
            str = DateTime.Now.ToString("mm:ss",CultureInfo.CurrentCulture) + " " + str;
            Console.WriteLine(str);
        }

        #endregion
    }

}
