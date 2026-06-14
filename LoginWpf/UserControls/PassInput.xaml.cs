using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LoginWpf.UserControls
{
    /// <summary>
    /// PassInput.xaml 的交互逻辑
    /// </summary>
    public partial class PassInput : UserControl
    {
        // 内部状态锁，防止因相互同步导致死循环
        private bool _isSyncing = false;

        public PassInput()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty PlaceHolderProperty =
            DependencyProperty.Register("PlaceHolder", typeof(string), typeof(PassInput),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPlaceHolderPropertyChanged));
        public string PlaceHolder
        {
            get => (string)GetValue(PlaceHolderProperty);
            set => SetValue(PlaceHolderProperty, value);
        }
        private static void OnPlaceHolderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (PassInput)d;
            if (control._isSyncing) return;

            control._isSyncing = true;

            string newValue = e.NewValue as string ?? string.Empty;
            control.tbPlaceHolder.Text = newValue;

            control._isSyncing = false;
        }

        public static readonly DependencyProperty TextValueProperty =
            DependencyProperty.Register("TextValue", typeof(string), typeof(PassInput),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextValuePropertyChanged));
        public string TextValue
        {
            get => (string)GetValue(TextValueProperty);
            set => SetValue(TextValueProperty, value);
        }
        // 当外部输入或绑定的 TextValue 改变时，同步更新内部的密码框
        private static void OnTextValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (PassInput)d;
            if (control._isSyncing) return;

            control._isSyncing = true;

            string newValue = e.NewValue as string ?? string.Empty;
            control.NormalTextBox.Text = newValue;
            control.SecretPasswordBox.Password = newValue;
            control.tbPlaceHolder.Visibility = string.IsNullOrWhiteSpace(newValue) ? Visibility.Visible : Visibility.Hidden;

            control._isSyncing = false;
        }


        public static readonly DependencyProperty IsPasswordProperty =
            DependencyProperty.Register("IsPassword", typeof(bool), typeof(PassInput),
                new PropertyMetadata(true, OnIsPasswordPropertyChanged));

        public bool IsPassword
        {
            get => (bool)GetValue(IsPasswordProperty);
            set => SetValue(IsPasswordProperty, value);
        }

        // 当 IsPassword 改变时，切换控件的显示与隐藏，并同步数据
        private static void OnIsPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (PassInput)d;
            bool isPassMode = (bool)e.NewValue;

            control.EyeBtn.IsChecked = !isPassMode;
            control.EyeBtn.Content = control.EyeBtn.IsChecked == true ? "👁️" : "👁‍🗨";

            // 执行状态切换函数
            control.ApplyModeChange();
        }

        /// <summary>
        /// 统一控制明密文切换、可见性调度及光标修复的核心方法
        /// </summary>
        private void ApplyModeChange()
        {
            if (NormalTextBox == null || SecretPasswordBox == null) return;

            _isSyncing = true;

            if (IsPassword)
            {
                // 【密文模式】
                NormalTextBox.Visibility = Visibility.Collapsed;
                SecretPasswordBox.Visibility = Visibility.Visible;

                SecretPasswordBox.Password = TextValue;
                SecretPasswordBox.Focus();

                // 异步模拟 End 键，让密码框光标也停在末尾
                Dispatcher.BeginInvoke(new System.Action(() =>
                {
                    KeyEventArgs args = new KeyEventArgs(
                        Keyboard.PrimaryDevice,
                        PresentationSource.FromVisual(SecretPasswordBox),
                        0,
                        Key.End)
                    { RoutedEvent = Keyboard.KeyDownEvent };

                    SecretPasswordBox.RaiseEvent(args);
                }), System.Windows.Threading.DispatcherPriority.Background);
            }
            else
            {
                // 【明文模式】
                NormalTextBox.Visibility = Visibility.Visible;
                SecretPasswordBox.Visibility = Visibility.Collapsed;

                NormalTextBox.Text = TextValue;
                NormalTextBox.Focus();
                NormalTextBox.SelectionStart = NormalTextBox.Text.Length;
            }

            _isSyncing = false;
        }

        #region 核心事件处理

        // 1. 用户在密文框（PasswordBox）打字时触发
        private void SecretPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (_isSyncing) return;

            tbPlaceHolder.Visibility = string.IsNullOrWhiteSpace(SecretPasswordBox.Password) ? Visibility.Visible : Visibility.Hidden;
            _isSyncing = true;

            TextValue = SecretPasswordBox.Password;      // 实时更新依赖属性，主窗体立刻同步拿到值
            NormalTextBox.Text = SecretPasswordBox.Password; // 悄悄把值同步给隐藏的文本框，防止切换时掉字

            _isSyncing = false;
        }

        // 2. 用户在明文框（TextBox）打字时触发
        private void NormalTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isSyncing) return;

            tbPlaceHolder.Visibility = string.IsNullOrWhiteSpace(NormalTextBox.Text) ? Visibility.Visible : Visibility.Hidden;
            _isSyncing = true;

            TextValue = NormalTextBox.Text;              // 实时更新依赖属性，主窗体立刻同步拿到值
            SecretPasswordBox.Password = NormalTextBox.Text; // 悄悄把值同步给隐藏的密码框

            _isSyncing = false;
        }

        // 3. 点击小眼睛按钮切换模式时触发
        private void EyeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_isSyncing) return;
            // 按钮被勾选(Checked)代表看明文 -> IsPassword 变为 False
            IsPassword = !EyeBtn.IsChecked.Value;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            NormalTextBox.Clear();
            SecretPasswordBox.Clear();
        }

        #endregion
    }
}
