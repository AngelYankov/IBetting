using IBetting.DataAccess.Models;

namespace IBetting.DataAccess.Repositories
{
    public interface ISportRepository
    {
        bool SaveSports(IEnumerable<Sport> allSports);
    }
}