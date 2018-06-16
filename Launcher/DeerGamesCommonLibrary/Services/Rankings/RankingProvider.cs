using System.Collections.Generic;

namespace DeerGamesCommonLibrary.Services.Rankings
{
    public class RankingProvider
    {
        private static RankingProvider _instance;
        private readonly Dictionary<string, IRankingService> rankingsServices;

        public static RankingProvider Instance
        {
            get { return _instance ?? (_instance = new RankingProvider()); }
        }

        private RankingProvider()
        {
            this.rankingsServices = new Dictionary<string, IRankingService>();
            this.rankingsServices.Add("Flyer", FlyerRankingService.Instance);
        }

        public IRankingService GetRankingProvider(string gameName)
        {
            IRankingService returnValue = null;
            this.rankingsServices.TryGetValue(gameName, out returnValue);
            return returnValue;
        }

    }
}
