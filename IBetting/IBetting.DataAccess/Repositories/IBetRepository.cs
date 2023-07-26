using IBetting.DataAccess.Models;

namespace IBetting.DataAccess.Repositories
{
    public interface IBetRepository
    {
        bool SaveBets(IEnumerable<Bet> allBets);
    }
}