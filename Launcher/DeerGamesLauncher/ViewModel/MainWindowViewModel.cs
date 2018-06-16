using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DeerGamesCommonLibrary.Services;
using DeerGamesLauncher.BL.Services;
using DeerGamesLauncher.Helper;
using DeerGamesLauncher.Views;

namespace DeerGamesLauncher.ViewModel
{
    public class MainWindowViewModel : NotifyPropertyChangedImpl
    {
        private DummyGameProvider _dummyGameProvider = DummyGameProvider.Instance;
        private GameViewModel _selectedGame;
        private ICommand _logout;

        public bool OfflineMode { get; set; }

        public MainWindowViewModel(bool offlineMode)
        {
            var games = _dummyGameProvider.GetLocalGames();

            this.OfflineMode = offlineMode;
            this.Games = new ObservableCollection<GameViewModel>(games.Select(x => new GameViewModel(x)));

            this.SelectedGame = this.Games.FirstOrDefault();
        }

        public ObservableCollection<GameViewModel> Games { get; set; }

        public GameViewModel SelectedGame
        {
            get => _selectedGame;
            set
            {
                _selectedGame = value;
                this.RaisePropertyChanged();
            }
        }

        public ICommand Logout
        {
            get { return _logout ?? (_logout = new RelayCommand(param =>
            {
                LoginService.Instance.Logout();

                var viewModel = new LoginViewModel();
                var loginWindow = new LoginWindow(viewModel);

                Application.Current.MainWindow = loginWindow;
                
                loginWindow.Show();

                WindowCloseHandler?.Invoke();

            }, param => !this.OfflineMode)); }
        }

        public WindowCloseHandlerDelegate WindowCloseHandler { get; set; }

        public delegate void WindowCloseHandlerDelegate();
    }
}
