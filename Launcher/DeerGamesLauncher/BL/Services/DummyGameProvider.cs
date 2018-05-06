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
            flyer.InstallState = InstallState.Installed;

            flyer.Rankings = new List<Ranking>();
            flyer.Rankings.Add(new Ranking { PlayerName = "Fritz", Rank = 1, Score = "124012" });
            flyer.Rankings.Add(new Ranking { PlayerName = "Willi", Rank = 2, Score = "122000" });
            flyer.Rankings.Add(new Ranking { PlayerName = "Norbert", Rank = 3, Score = "119000" });
            flyer.Rankings.Add(new Ranking { PlayerName = "Fredi", Rank = 4, Score = "101002" });
            flyer.Rankings.Add(new Ranking { PlayerName = "Ferdi", Rank = 5, Score = "98000" });
            flyer.Rankings.Add(new Ranking { PlayerName = "Walter", Rank = 6, Score = "91000" });
            flyer.Rankings.Add(new Ranking { PlayerName = "Jan", Rank = 7, Score = "90000" });
            flyer.Rankings.Add(new Ranking { PlayerName = "Marietheres", Rank = 8, Score = "89000" });
            flyer.Rankings.Add(new Ranking { PlayerName = "Sascha", Rank = 9, Score = "51000" });
            flyer.Rankings.Add(new Ranking {PlayerName = "Timon", Rank = 10, Score = "3000"});

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
