using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }
        private void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
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
