using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DeerGamesCommonLibrary.Enums;
using DeerGamesCommonLibrary.Helper;
using DeerGamesCommonLibrary.Services;
using DeerGamesLauncher.Helper;

namespace DeerGamesLauncher.ViewModel
{
    public class LoginViewModel : NotifyPropertyChangedImpl
    {
        private ICommand _login;
        private bool _isLoading;
        private SecureString _password;
        private LoginService loginService = LoginService.Instance;
        private string _errorText;
        private string _username;
        private bool _rememberCredentials;

        public SecureString Password
        {
            get => _password;
            set
            {
                _password = value;
                this.RaisePropertyChanged();
            }
        }

        public bool RememberCredentials
        {
            get => _rememberCredentials;
            set
            {
                _rememberCredentials = value;
                this.RaisePropertyChanged();
            }
        }

        public String Username
        {
            get => _username;
            set
            {
                _username = value;
                this.RaisePropertyChanged();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                this.RaisePropertyChanged();
            }
        }

        public bool ErrorTextPresent => !String.IsNullOrEmpty(this.ErrorText);

        public String ErrorText
        {
            get => _errorText;
            set
            {
                _errorText = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged("ErrorTextPresent");
            }
        }

        public LoginViewModel()
        {
            this.Username = String.Empty;
            this.Password = new SecureString();
        }

        public async Task Load()
        {
            this.IsLoading = true;
            this.Username = ConfigurationService.Instance.Username;
            this.Password = ConfigurationService.Instance.Password;
            this.RememberCredentials = ConfigurationService.Instance.CredentialsSaved;

            await Task.Delay(1000);

            // Get Loginservice
            var loginService = LoginService.Instance;

            LoginState state;
            // Load inital LoginState
            try
            {
                state = await loginService.Initialize();
            }
            catch (Exception)
            {
                state = LoginState.LoginFailed;
            }

            if (state == LoginState.LoggedIn)
            {
                this.StartMainWindow(false);
                return;
            }

            if (state == LoginState.LoginFailed)
            {
                this.Username = string.Empty;
                this.Password = new SecureString();
                this.RememberCredentials = false;
                this.ErrorText = "The login with the saved credentials failed.";
            }

            this.IsLoading = false;
        }

        public ICommand Login
        {
            get
            {
                return _login ?? (_login = new RelayCommand(async param =>
                {
                    this.IsLoading = true;

                    LoginResult loginResult;

                    try
                    {
                        loginResult = await this.loginService.AttemptLogin(this.Username, this.Password, this.RememberCredentials);

                    }
                    catch (Exception e)
                    {
                        loginResult = LoginResult.ConnectionIssue;
                    }
                    
                    // login successfull
                    if (loginResult == LoginResult.Success || loginResult == LoginResult.AlreadyLoggedIn)
                    {
                        StartMainWindow(false);
                        return;
                    }
                    else if (loginResult == LoginResult.ConnectionIssue)
                    {
                        this.ErrorText = "The Server could not be reached.";
                    }
                    else if (loginResult == LoginResult.WrongCredentials)
                    {
                        this.ErrorText = "You entered the wrong credentials.";
                    }

                    await Task.Delay(100);

                    this.IsLoading = false;

                }, param => 
                    !String.IsNullOrEmpty(this.Username) && this.Password != null && this.Password.Length > 0));
            }
        }

        private void StartMainWindow(bool offlineMode)
        {
            var viewModel = new MainWindowViewModel(offlineMode);
            var mainWindow = new MainWindow(viewModel);
            
            Application.Current.MainWindow = mainWindow;

            // close Mainwindow
            WindowCloseHandler?.Invoke();

            mainWindow.Show();
        }


        public WindowCloseHandlerDelegate WindowCloseHandler { get; set; }

        public delegate void WindowCloseHandlerDelegate();
    }
}
