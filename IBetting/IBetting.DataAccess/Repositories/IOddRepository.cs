using IBetting.DataAccess.Models;

namespace IBetting.DataAccess.Repositories
{
    public interface IOddRepository
    {
        bool SaveOdds(IEnumerable<Odd> allOdds);
    }
}