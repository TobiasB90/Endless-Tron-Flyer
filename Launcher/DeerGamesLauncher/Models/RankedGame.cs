using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeerGamesLauncher.Models
{
    class RankedGame : Game
    {
        public override bool RankingsAvailable
        {
            get => true;
        }

        public List<Ranking> Rankings { get; set; }
    }
}
