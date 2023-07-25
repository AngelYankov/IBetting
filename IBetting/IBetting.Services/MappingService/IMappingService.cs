using IBetting.Services.BettingService.Models;
using System.Xml;

namespace IBetting.Services.MappingService
{
    public interface IMappingService
    {
        IEnumerable<SportDTO> MapSports(XmlDocument document);

        IEnumerable<EventDTO> MapEvents(XmlDocument document);

        IEnumerable<MatchDTO> MapMatches(XmlDocument document);

        IEnumerable<BetDTO> MapBets(XmlDocument document);

        IEnumerable<OddDTO> MapOdds(XmlDocument document);
    }
}