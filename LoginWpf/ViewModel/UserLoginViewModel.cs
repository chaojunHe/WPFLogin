using LoginWpf.Model;
using LoginWpf.MVVM;

namespace LoginWpf.ViewModel
{
    internal class UserLoginViewModel : ViewModelBase
    {
        private UserLogin _userLogin;

        public UserLoginViewModel()
        {
            _userLogin = new UserLogin();
        }

        // 公开实体属性
        public string UserName
        {
            get => _userLogin.UserName;
            set
            {
                if (_userLogin.UserName != value)
                {
                    _userLogin.UserName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Email
        {
            get => _userLogin.Email;
            set
            {
                if (_userLogin.Email != value)
                {
                    _userLogin.Email = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Password
        {
            get => _userLogin.Password;
            set
            {
                if (_userLogin.Password != value)
                {
                    _userLogin.Password = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsPasswordValid));
                }
            }
        }

        public string ConfirmPassword
        {
            get => _userLogin.ConfirmPassword;
            set
            {
                if (_userLogin.ConfirmPassword != value)
                {
                    _userLogin.ConfirmPassword = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsPasswordValid));
                }
            }
        }

        // 验证属性
        public bool IsPasswordValid
        {
            get => !string.IsNullOrEmpty(Password) && Password == ConfirmPassword;
        }

        // 获取完整的 UserLogin 实体
        public UserLogin GetUserLogin()
        {
            return new UserLogin
            {
                UserName = this.UserName,
                Email = this.Email,
                Password = this.Password,
                ConfirmPassword = this.ConfirmPassword
            };
        }

        // 更新实体
        public void UpdateUserLogin(UserLogin userLogin)
        {
            if (userLogin != null)
            {
                this.UserName = userLogin.UserName;
                this.Email = userLogin.Email;
                this.Password = userLogin.Password;
                this.ConfirmPassword = userLogin.ConfirmPassword;
            }
        }

        // 重置所有字段
        public void Reset()
        {
            UserName = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
        }

        // 设置默认值（由主窗体调用）
        public void SetDefaultValues()
        {
            UserName = "默认用户名";
            Email = "default@example.com";
            Password = "123456";
            ConfirmPassword = "123456";
        }
    }
}
