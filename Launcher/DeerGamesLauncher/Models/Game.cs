using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeerGamesLauncher.Enums;

namespace DeerGamesLauncher.Models
{
    public abstract class Game : IGame
    {
        public abstract bool RankingsAvailable { get; }

        public string SmallLogo { get; set; }

        public string BigLogo { get; set; }

        public string Name { get; set; }

        public InstallState InstallState { get; set; }

        public string InstallFolder { get; set; }

        public Dictionary<string, string> FileCheckSums { get; set; }
    }
}
