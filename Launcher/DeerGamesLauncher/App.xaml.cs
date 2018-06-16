using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DeerGamesCommonLibrary.Enums;
using DeerGamesCommonLibrary.Helper;
using DeerGamesCommonLibrary.Services;
using DeerGamesLauncher.ViewModel;
using DeerGamesLauncher.Views;

namespace DeerGamesLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            // Read current launcher version

            // Get latest launcher version from bbackend

            // if New Version available

            // Trigger SelfUpdate.exe with launchparam to install latest *.msi

            // Close App
            
            var viewModel = new LoginViewModel();
            var loginWindow = new LoginWindow(viewModel);

            Application.Current.MainWindow = loginWindow;
            
            loginWindow.Show();

            await viewModel.Load();
          
            base.OnStartup(e);
        }


    }
}
