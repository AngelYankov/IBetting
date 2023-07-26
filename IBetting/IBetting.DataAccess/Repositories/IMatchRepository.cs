using IBetting.DataAccess.Models;

namespace IBetting.DataAccess.Repositories
{
    public interface IMatchRepository
    {
        bool SaveMatches(IEnumerable<Match> allMatches);

        Task<List<Match>> GetAllMatchesAsync();

        Task<Match> GetMatchAsync(int matchXmlId);
    }
}