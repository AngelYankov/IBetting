using IBetting.Services.BettingService.Models;

namespace IBetting.Services.Repositories
{
    public interface IEventRepository
    {
        bool SaveEvents(IEnumerable<EventDTO> allEvents);
    }
}