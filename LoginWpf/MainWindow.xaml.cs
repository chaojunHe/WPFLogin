using LoginWpf.Model;
using System.Windows;
using System.Windows.Input;

namespace LoginWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnExitApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var _userLoginModel = new UserLogin();
            _userLoginModel.UserName = txtUserName.TextValue;
            _userLoginModel.Email = txtEmail.TextValue;
            _userLoginModel.Password = txtPassword.TextValue;
            _userLoginModel.ConfirmPassword = txtConfirmPassword.TextValue;
            var ss2 = _userLoginModel;
        }
    }
}
