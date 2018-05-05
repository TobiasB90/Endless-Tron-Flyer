using System.Collections.Generic;
using DeerGamesLauncher.Enums;

namespace DeerGamesLauncher.Models
{
    public interface IGame
    {
        string BigLogo { get; set; }
        Dictionary<string, string> FileCheckSums { get; set; }
        string InstallFolder { get; set; }
        InstallState InstallState { get; set; }
        string Name { get; set; }
        bool RankingsAvailable { get; }
        string SmallLogo { get; set; }
    }
}