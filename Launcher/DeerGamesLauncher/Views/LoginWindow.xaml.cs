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
using System.Windows.Shapes;
using DeerGamesLauncher.ViewModel;
using MahApps.Metro.Controls;

namespace DeerGamesLauncher.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : MetroWindow
    {
        public LoginWindow(LoginViewModel viewModel)
        {
            this.DataContext = viewModel;

            InitializeComponent();

            viewModel.WindowCloseHandler += () => this.Close();
        }

        private void LoginWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
