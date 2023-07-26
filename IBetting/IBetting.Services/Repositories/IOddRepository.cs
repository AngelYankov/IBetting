using IBetting.Services.BettingService.Models;

namespace IBetting.Services.Repositories
{
    public interface IOddRepository
    {
        bool SaveOdds(IEnumerable<OddDTO> allOdds);
    }
}