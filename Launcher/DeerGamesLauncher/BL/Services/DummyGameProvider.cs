using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeerGamesLauncher.Enums;
using DeerGamesLauncher.Models;

namespace DeerGamesLauncher.BL.Services
{
    public class DummyGameProvider
    {
        private static DummyGameProvider _instance;

        public static DummyGameProvider Instance
        {
            get { return _instance ?? (_instance = new DummyGameProvider()); }
        }

        private DummyGameProvider()
        {

        }

        public List<Game> GetLocalGames()
        {
            var localGames = new List<Game>();

            var flyer = new RankedGame();

            flyer.Name = "Tron Flyer";
            flyer.SmallLogo = "tron.png";
            flyer.BigLogo = "tron.png";
            flyer.Identifier = "Flyer";
            flyer.InstallState = InstallState.Installed;

            var rndgame1 = new UnrankedGame();

            rndgame1.Name = "New Game 1";
            rndgame1.InstallState = InstallState.Updating;

            var rndgame2 = new UnrankedGame();

            rndgame2.Name = "New Game 2";
            rndgame2.InstallState = InstallState.NotInstalled;

            localGames.Add(flyer);
            localGames.Add(rndgame1);
            localGames.Add(rndgame2);

            return localGames;
        }

        public List<Game> GetCloudGames()
        {
            return null;
        }

    }
}
