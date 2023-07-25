using IBetting.Services.MatchService.Models;

namespace IBetting.Services.MatchService
{
    public interface IMatchService
    {
        Task<List<MatchWithBetsDTO>> GetAllMatches();

        Task<MatchWithBetsDTO> GetMatch(int matchXmlId);
    }
}