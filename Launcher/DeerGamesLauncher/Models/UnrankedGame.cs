using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeerGamesLauncher.Models
{
    public class UnrankedGame : Game
    {
        public override bool RankingsAvailable => false;
    }
}
