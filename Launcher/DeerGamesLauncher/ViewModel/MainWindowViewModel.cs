using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeerGamesLauncher.BL.Services;
using DeerGamesLauncher.Helper;

namespace DeerGamesLauncher.ViewModel
{
    public class MainWindowViewModel : NotifyPropertyChangedImpl
    {
        private DummyGameProvider _dummyGameProvider = DummyGameProvider.Instance;
        private GameViewModel _selectedGame;

        public MainWindowViewModel()
        {
            var games = _dummyGameProvider.GetLocalGames();

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
    }
}
