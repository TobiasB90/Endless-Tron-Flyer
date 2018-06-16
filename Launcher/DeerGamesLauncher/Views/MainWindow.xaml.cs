using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DeerGamesCommonLibrary.Services;
using DeerGamesLauncher.Enums;
using DeerGamesLauncher.ViewModel;
using MahApps.Metro.Controls;

namespace DeerGamesLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private MainWindowViewModel viewModel;

        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();

            this.viewModel = viewModel;

            this.viewModel.WindowCloseHandler += this.Close;

            this.DataContext = this.viewModel;
        }

        private void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // check if any game process from launcher is still running

            // No process running

            if (!ConfigurationService.Instance.CredentialsSaved)
            {
                ConfigurationService.Instance.Token = string.Empty;
            }

            base.OnClosing(e);
        }
    }

    public class InstallBarTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            GameViewModel viewModel = item as GameViewModel;

            if (viewModel != null)
            {
                InstallState installState = viewModel.InstallState;
                Window win = Application.Current.MainWindow;


                // Select one of the DataTemplate objects, based on the 
                // value of the selected item in the ComboBox.
                if (installState == InstallState.NotInstalled)
                {
                    return win.FindResource("NotInstalled") as DataTemplate;
                }
                else if (installState == InstallState.Installed)
                {
                    return win.FindResource("Installed") as DataTemplate;
                }
                else if (installState == InstallState.Updating)
                {
                    return win.FindResource("Updating") as DataTemplate;
                }
                else
                {
                    return win.FindResource("Dummy") as DataTemplate;

                }
            }

            return null;
        }

    }
}
