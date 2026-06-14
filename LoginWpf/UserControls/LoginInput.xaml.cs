using System.Windows;
using System.Windows.Controls;

namespace LoginWpf.UserControls
{
    /// <summary>
    /// LoginInput.xaml 的交互逻辑
    /// </summary>
    public partial class LoginInput : UserControl
    {
        // 内部状态锁，防止因相互同步导致死循环
        private bool _isSyncing = false;

        public LoginInput()
        {
            InitializeComponent();
        }


        public static readonly DependencyProperty PlaceHolderProperty =
            DependencyProperty.Register("PlaceHolder", 
                typeof(string), 
                typeof(LoginInput),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPlaceHolderPropertyChanged));
        public string PlaceHolder
        {
            get => (string)GetValue(PlaceHolderProperty);
            set => SetValue(PlaceHolderProperty, value);
        }
        private static void OnPlaceHolderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (LoginInput)d;
            if (control._isSyncing) return;

            control._isSyncing = true;

            string newValue = e.NewValue as string ?? string.Empty;
            control.tbPlaceHolder.Text = newValue;

            control._isSyncing = false;
        }

        public static readonly DependencyProperty TextValueProperty =
            DependencyProperty.Register("TextValue", typeof(string), typeof(LoginInput),
                new FrameworkPropertyMetadata(string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, // 支持双向绑定
                    OnTextValuePropertyChanged));
        public string TextValue
        {
            get => (string)GetValue(TextValueProperty);
            set => SetValue(TextValueProperty, value);
        }
        // 当外部输入或绑定的 TextValue 改变时，同步更新内部的密码框
        private static void OnTextValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (LoginInput)d;
            if (control._isSyncing) return;

            control._isSyncing = true;

            string newValue = e.NewValue as string ?? string.Empty;
            control.txtInput.Text = newValue;

            control._isSyncing = false;
        }

        private void txtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbPlaceHolder.Visibility = string.IsNullOrWhiteSpace(txtInput.Text) ? Visibility.Visible : Visibility.Hidden;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtInput.Clear();
        }
    }
}
