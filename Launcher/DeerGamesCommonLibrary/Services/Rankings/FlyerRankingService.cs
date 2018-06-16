using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DeerGamesCommonLibrary.Constants;
using DeerGamesCommonLibrary.Models;
using Newtonsoft.Json;

namespace DeerGamesCommonLibrary.Services.Rankings
{
    public class FlyerRankingService : IRankingService
    {
        private static FlyerRankingService _instance;

        internal static FlyerRankingService Instance
        {
            get { return _instance ?? (_instance = new FlyerRankingService()); }
        }

        private FlyerRankingService()
        {

        }

        public async Task<Ranking> PersonalRanking()
        {
            Ranking returnValue = null;

            try
            {
                var response = await ConnectionService.Instance.Get(ApiEndpoints.FlyerRankingPrivate, null);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    var decryptedString = EncryptionService.Instance.Decrypt(responseBody);

                    returnValue = JsonConvert.DeserializeObject<Ranking>(decryptedString);
                }
            }
            catch (Exception e)
            {
                // todo: logging
            }

            return returnValue;
        }

        public async Task<Ranking> PostRanking(int score)
        {
            Ranking returnValue = null;

            var data = JsonConvert.SerializeObject(new HighScoreUploadData { Highscore = score});

            var body = new StringContent(EncryptionService.Instance.Encrypt(data));

            var response = await ConnectionService.Instance.Post(ApiEndpoints.FlyerRankingCreate, body);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                var decryptedString = EncryptionService.Instance.Decrypt(responseBody);

                returnValue = JsonConvert.DeserializeObject<Ranking>(decryptedString);
            }

            return returnValue;
        }

        public async Task<List<Ranking>> GetRankings()
        {
            var rankingList = new List<Ranking>();

            try
            {
                var response = await ConnectionService.Instance.Get(ApiEndpoints.FlyerRankingAll, null);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    var decryptedString = EncryptionService.Instance.Decrypt(responseBody);

                    var parsedObject = JsonConvert.DeserializeObject<RankingList>(decryptedString);

                    rankingList = parsedObject.Scores.ToList();
                }
            }
            catch (Exception e)
            {
                // todo: logging
            }

            return rankingList;
        }

        internal class RankingList
        {
            public IList<Ranking> Scores { get; set; }
        }

        internal class HighScoreUploadData
        {
            public int Highscore { get; set; }
        }
    }
}
