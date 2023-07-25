using IBetting.DataAccess.Enums;
using IBetting.DataAccess.Models;
using IBetting.Services.BettingService.Models;
using System.Globalization;
using System.Xml;

namespace IBetting.Services.MappingService
{
    public class MappingService : IMappingService
    {
        /// <summary>
        /// Maps all Sport nodes to objects from XML document
        /// </summary>
        /// <param name="document">XML document with data</param>
        /// <returns>List with all Sport objects mapped from XML document</returns>
        public IEnumerable<SportDTO> MapSports(XmlDocument document)
        {
            var allSports = new List<Sport>();
            var sports = document.SelectNodes(Constants.SportNodes);
            for (int i = 0; i < sports.Count; i++)
            {
                var sport = new Sport()
                {
                    Id = Int32.Parse(sports[i].Attributes[Constants.Id].Value),
                    Name = sports[i].Attributes[Constants.Name].Value,
                    IsActive = true
                };

                allSports.Add(sport);
            }

            return allSports.Select(s => new SportDTO(s));
        }

        /// <summary>
        /// Maps all Event nodes to objects from XML document
        /// </summary>
        /// <param name="document">XML document with data</param>
        /// <returns>List with all Event objects mapped from XML document</returns>
        public IEnumerable<EventDTO> MapEvents(XmlDocument document)
        {
            var allEvents = new List<Event>();
            var events = document.SelectNodes(Constants.EventNodes);
            for (int i = 0; i < events.Count; i++)
            {
                var eventHistory = new Event()
                {
                    Id = Int32.Parse(events[i].Attributes[Constants.Id].Value),
                    CategoryID = Int32.Parse(events[i].Attributes[Constants.CategoryId].Value),
                    Name = events[i].Attributes[Constants.Name].Value,
                    IsLive = events[i].Attributes[Constants.IsLive].Value == "true",
                    SportId = Int32.Parse(events[i].Attributes[Constants.SportId].Value),
                    IsActive = true
                };

                allEvents.Add(eventHistory);
            }

            return allEvents.Select(e => new EventDTO(e));
        }

        /// <summary>
        /// Maps all Match nodes to objects from XML document
        /// </summary>
        /// <param name="document">XML document with data</param>
        /// <returns>List with all Match objects mapped from XML document</returns>
        public IEnumerable<MatchDTO> MapMatches(XmlDocument document)
        {
            var allMatches = new List<Match>();
            var matches = document.SelectNodes(Constants.MatchNodes);
            for (int i = 0; i < matches.Count; i++)
            {
                var match = new Match()
                {
                    Id = Int32.Parse(matches[i].Attributes[Constants.Id].Value),
                    Name = matches[i].Attributes[Constants.Name].Value,
                    StartDate = DateTime.Parse(matches[i].Attributes[Constants.StartDate].Value),
                    MatchType = Enum.Parse<MatchTypeEnum>(matches[i].Attributes[Constants.MatchType].Value),
                    EventId = Int32.Parse(matches[i].Attributes[Constants.EventId].Value),
                    IsActive = true
                };
                allMatches.Add(match);
            }

            return allMatches.Select(m => new MatchDTO(m));
        }

        /// <summary>
        /// Maps all Bet nodes to objects from XML document
        /// </summary>
        /// <param name="document">XML document with data</param>
        /// <returns>List with all Bet objects mapped from XML document</returns>
        public IEnumerable<BetDTO> MapBets(XmlDocument document)
        {
            var allBets = new List<Bet>();
            var bets = document.SelectNodes(Constants.BetNodes);
            for (int i = 0; i < bets.Count; i++)
            {
                var bet = new Bet()
                {
                    Id = Int32.Parse(bets[i].Attributes[Constants.Id].Value),
                    Name = bets[i].Attributes[Constants.Name].Value,
                    IsLive = bets[i].Attributes[Constants.IsLive].Value == "true",
                    MatchId = Int32.Parse(bets[i].Attributes[Constants.MatchId].Value),
                    MatchType = Enum.Parse<MatchTypeEnum>(bets[i].Attributes[Constants.MatchType].Value),
                    MatchStartDate = DateTime.Parse(bets[i].Attributes[Constants.MatchStartDate].Value),
                    IsActive = true
                };

                allBets.Add(bet);
            }

            return allBets.Select(b => new BetDTO(b));
        }

        /// <summary>
        /// Maps all Odd nodes to objects from XML document
        /// </summary>
        /// <param name="document">XML document with data</param>
        /// <returns>List with all Odd objects mapped from XML document</returns>
        public IEnumerable<OddDTO> MapOdds(XmlDocument document)
        {
            var allOdds = new List<Odd>();
            var odds = document.SelectNodes(Constants.OddNodes);
            for (int i = 0; i < odds.Count; i++)
            {
                var odd = new Odd()
                {
                    Id = Int32.Parse(odds[i].Attributes["ID"].Value),
                    Name = odds[i].Attributes["Name"].Value,
                    Value = decimal.Parse(odds[i].Attributes[Constants.Value].Value, CultureInfo.InvariantCulture),
                    BetId = Int32.Parse(odds[i].Attributes[Constants.BetId].Value),
                    SpecialBetValue = odds[i].Attributes[Constants.SpecialBetValue]?.Value,
                    IsActive = true
                };

                allOdds.Add(odd);
            }

            return allOdds.Select(o => new OddDTO(o));
        }
    }
}
