using System.Collections.Generic;
using System.Threading.Tasks;
using DeerGamesCommonLibrary.Models;

namespace DeerGamesCommonLibrary.Services.Rankings
{
    public interface IRankingService
    {
        Task<Ranking> PostRanking(int score);
        Task<List<Ranking>> GetRankings();
        Task<Ranking> PersonalRanking();
    }
}
