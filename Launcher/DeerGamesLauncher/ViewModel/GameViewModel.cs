using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeerGamesLauncher.Helper;
using DeerGamesLauncher.Models;

namespace DeerGamesLauncher.ViewModel
{
    public class GameViewModel : NotifyPropertyChangedImpl
    {
        private Game _model;
        private ObservableCollection<Ranking> _rankings;

        public GameViewModel(Game game)
        {
            this._model = game;
        }

        public string Name
        {
            get { return this._model.Name; }
            set
            {
                this._model.Name = value;
                this.RaisePropertyChanged();
            }
        }

        public bool IsRankedGame
        {
            get { return this._model.RankingsAvailable; }
        }

        public ObservableCollection<Ranking> Rankings
        {
            get
            {
                if (_rankings == null && this._model.RankingsAvailable)
                {
                    _rankings = new ObservableCollection<Ranking>(((RankedGame) this._model).Rankings);
                }

                return _rankings;
            }
        }
    }
}
