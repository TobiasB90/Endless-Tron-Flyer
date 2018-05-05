using System;
using System.Collections.Generic;
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
    }
}
