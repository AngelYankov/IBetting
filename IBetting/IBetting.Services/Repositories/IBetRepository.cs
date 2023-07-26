using IBetting.Services.BettingService.Models;

namespace IBetting.Services.Repositories
{
    public interface IBetRepository
    {
        bool SaveBets(IEnumerable<BetDTO> allBets);
    }
}