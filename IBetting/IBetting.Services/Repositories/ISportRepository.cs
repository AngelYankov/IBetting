using IBetting.Services.BettingService.Models;

namespace IBetting.Services.Repositories
{
    public interface ISportRepository
    {
        bool SaveSports(IEnumerable<SportDTO> allSports);
    }
}