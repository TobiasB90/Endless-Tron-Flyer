using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using DeerGamesCommonLibrary.Constants;
using DeerGamesCommonLibrary.Enums;
using DeerGamesCommonLibrary.Helper;
using Newtonsoft.Json;

namespace DeerGamesCommonLibrary.Services
{
    public class LoginService
    {
        private readonly ConfigurationService configurationService;
        private LoginState _state;

        private static LoginService _instance;

        public static LoginService Instance
        {
            get { return _instance ?? (_instance = new LoginService()); }
        }

        private LoginService()
        {
            configurationService = ConfigurationService.Instance;;
        }

        public async Task<LoginState> Initialize()
        {
            if (!String.IsNullOrEmpty(configurationService.Token) || configurationService.CredentialsSaved)
            {
                if (!String.IsNullOrEmpty(configurationService.Token))
                {
                    var msg = await ConnectionService.Instance.Get(ApiEndpoints.ConnectionTest, null);

                    if (msg.IsSuccessStatusCode)
                    {
                        this._state = LoginState.LoggedIn;

                        return LoginState.LoggedIn;
                    }
                    else
                    {
                        configurationService.Token = string.Empty;
                    }
                }

                if (configurationService.CredentialsSaved)
                {
                    var loginData = new LoginDto(configurationService.Username, SecureStringHelper.SecureStringToString(configurationService.Password));

                    var encryptedContent = EncryptionService.Instance.Encrypt(JsonConvert.SerializeObject(loginData, Formatting.None));

                    var content = new StringContent(encryptedContent, Encoding.UTF8, "application/json"); ;

                    HttpResponseMessage msg = await ConnectionService.Instance.Post(ApiEndpoints.Login, content).ConfigureAwait(false);

                    if (msg.IsSuccessStatusCode)
                    {
                        configurationService.Token = msg.Headers.GetValues("Authorization").FirstOrDefault()?.Substring(6);

                        this.State = LoginState.LoggedIn;
                    }
                    else
                    {
                        this.State = LoginState.LoginFailed;
                    }
                }
            }
            else
            {
                this._state = LoginState.NotLoggedIn;
            }

            return _state;
        }

        public void Logout()
        {
            configurationService.Token = String.Empty;
            configurationService.Password = new SecureString();
            configurationService.Username = String.Empty;

            this._state = LoginState.NotLoggedIn;
        }

        public void InitalizeBackgroundChecker()
        {
            Task.Factory.StartNew(() =>
            {
                
            });
        }

        public async Task<LoginResult> AttemptLogin(string name, SecureString password, bool rememberCredentials)
        {
            if (this.State == LoginState.LoggedIn || this.State == LoginState.AttemptingLogin)
            {
                return LoginResult.AlreadyLoggedIn;
            }

            var loginData = new LoginDto(name, SecureStringHelper.SecureStringToString(password));

            var encryptedContent = EncryptionService.Instance.Encrypt(JsonConvert.SerializeObject(loginData, Formatting.None));

            var content = new StringContent(encryptedContent, Encoding.UTF8, "application/json");

            HttpResponseMessage msg = await ConnectionService.Instance.Post(ApiEndpoints.Login, content);

            if (msg.IsSuccessStatusCode)
            {
                configurationService.Token = msg.Headers.GetValues("Authorization").FirstOrDefault();

                this.State = LoginState.LoggedIn;

                if (rememberCredentials)
                {
                    configurationService.Username = name;
                    configurationService.Password = password;
                }

                return LoginResult.Success;
            }
            else if (msg.StatusCode == HttpStatusCode.BadRequest || msg.StatusCode == HttpStatusCode.Unauthorized)
            {
                this.State = LoginState.LoginFailed;

                return LoginResult.WrongCredentials;
            }

            return LoginResult.ConnectionIssue;
        }

        private object _stateChangeLock = new object();

        public LoginState State
        {
            get
            {
                lock (_stateChangeLock)
                {
                    return _state;
                }
            }
            set
            {
                lock (_stateChangeLock)
                {
                    if (_state == value) return;

                    var oldValue = _state;
                    _state = value;

                    RaiseLoginStateChanged(oldValue, _state);
                }
            }
        }

        private void RaiseLoginStateChanged(LoginState oldState, LoginState newState)
        {
            if (LoginStateChanged == null)
                return;

            var eventArgs = new LoginStateChangedEventArgs();
            eventArgs.OldState = oldState;
            eventArgs.NewState = newState;

            if (Application.Current.Dispatcher == null)
                LoginStateChanged(this, eventArgs);
            else
                Application.Current.Dispatcher.BeginInvoke(((Action)(() => { LoginStateChanged(this, eventArgs); })), DispatcherPriority.Send);
        }

        public delegate void LoginStateChangedEventHandler(object sender, LoginStateChangedEventArgs e);

        public event LoginStateChangedEventHandler LoginStateChanged;

        internal class LoginDto
        {
            public string Name { get; set; }

            public string Password { get; set; }

            public LoginDto(string username, string password)
            {
                this.Name = username;
                this.Password = password;
            }
        }
    }

    public class LoginStateChangedEventArgs : EventArgs
    {
        public LoginState OldState { get; set; }
        public LoginState NewState { get; set; }
    }
    
}
