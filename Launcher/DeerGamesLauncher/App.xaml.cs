using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DeerGamesLauncher.BL.Services;

namespace DeerGamesLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var encString = EncryptionService.Instance.Encrypt("{\"Name\":\"hummel\", \"Password\": \"leet\", \"Email\":\"marian@mitschke.net\"}");

            base.OnStartup(e);
        }
    }
}
