using IBetting.DataAccess.Models;

namespace IBetting.DataAccess.Repositories
{
    public interface IEventRepository
    {
        bool SaveEvents(IEnumerable<Event> allEvents);
    }
}