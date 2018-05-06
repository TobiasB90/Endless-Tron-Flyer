using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DeerGamesLauncher.Enums;
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

        public InstallState InstallState
        {
            get { return this._model.InstallState; }
            set
            {
                this._model.InstallState = value;
                this.RaisePropertyChanged();
            }
        }

        public string LogoSmall
        {
            get
            {
                string iconPath = @"Resources\gameicons\";

                if (!string.IsNullOrEmpty(this._model.SmallLogo))
                {
                    iconPath += this._model.SmallLogo;
                }
                else
                {
                    iconPath += "default-game-icon.png";
                }

                return Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, iconPath);

            }
        }

        public string LogoBig
        {
            get
            {
                string logoPath = @"Resources\gamelogos\";

                if (!string.IsNullOrEmpty(this._model.BigLogo))
                {
                    logoPath += this._model.BigLogo;
                }
                else
                {
                    logoPath += "default-game-logo.png";
                }

                return Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, logoPath);

            }
        }
    }
}
