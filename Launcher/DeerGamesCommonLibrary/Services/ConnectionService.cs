using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DeerGamesCommonLibrary.Enums;

namespace DeerGamesCommonLibrary.Services
{
    public class ConnectionService
    {
        private static ConnectionService _instance;

        private HttpClient _client;

        private const string _connectionUrl = "http://deergames.eu-central-1.elasticbeanstalk.com/api/"; //"http://localhost:8080/api/";//

        private LoginService _loginService = LoginService.Instance;
        private ConfigurationService _cofigurationService;

        public static ConnectionService Instance
        {
            get { return _instance ?? (_instance = new ConnectionService()); }
        }

        private ConnectionService()
        {
            _client = new HttpClient();
            _client.Timeout = TimeSpan.FromSeconds(5);
            _cofigurationService = ConfigurationService.Instance;
        }

        public async Task<HttpResponseMessage> Post(string path, HttpContent content)
        {
            if (_loginService.State == LoginState.LoggedIn || _loginService.State == LoginState.AttemptingLogin)
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _cofigurationService.Token);

            //content.Headers.Add("Content-Type", "application/json");
            return await _client.PostAsync(_connectionUrl + path, content).ConfigureAwait(false);
        }

        public async Task<HttpResponseMessage> Get(string path, HttpContent content)
        {
            if (_loginService.State == LoginState.LoggedIn || _loginService.State == LoginState.AttemptingLogin)
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _cofigurationService.Token);

            return await _client.GetAsync(_connectionUrl + path).ConfigureAwait(false);
        }
    }
}
