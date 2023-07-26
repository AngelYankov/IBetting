using IBetting.DataAccess.Models;
using System.Xml;

namespace IBetting.Services.MappingService
{
    public interface IMappingService
    {
        IEnumerable<Sport> MapSports(XmlDocument document);

        IEnumerable<Event> MapEvents(XmlDocument document);

        IEnumerable<Match> MapMatches(XmlDocument document);

        IEnumerable<Bet> MapBets(XmlDocument document);

        IEnumerable<Odd> MapOdds(XmlDocument document);
    }
}