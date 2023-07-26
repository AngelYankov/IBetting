using IBetting.Services.MatchService.Models;

namespace IBetting.Services.MatchService
{
    public interface IMatchService
    {
        Task<List<MatchWithBetsDTO>> GetAllMatchesAsync();

        Task<MatchWithBetsDTO> GetMatchAsync(int matchXmlId);
    }
}