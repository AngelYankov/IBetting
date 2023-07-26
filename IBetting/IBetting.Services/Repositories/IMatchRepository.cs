using IBetting.Services.BettingService.Models;
using IBetting.Services.MatchService.Models;

namespace IBetting.Services.Repositories
{
    public interface IMatchRepository
    {
        bool SaveMatches(IEnumerable<MatchDTO> allMatches);

        Task<List<MatchWithBetsDTO>> GetAllMatchesAsync();

        Task<MatchWithBetsDTO> GetMatchAsync(int matchXmlId);
    }
}