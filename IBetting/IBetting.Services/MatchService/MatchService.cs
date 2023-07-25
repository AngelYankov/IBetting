using IBetting.DataAccess;
using IBetting.DataAccess.Enums;
using IBetting.Services.MatchService.Models;
using Microsoft.EntityFrameworkCore;

namespace IBetting.Services.MatchService
{
    public class MatchService : IMatchService
    {
        private readonly IBettingDbContext dbContext;
        public MatchService(IBettingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Gets all Match objects starting in the next 24 hours along with all their active Bets and Odds
        /// </summary>
        /// <returns>List with all Match objects starting in the next 24 hours along with all their active Bets and Odds</returns>
        public async Task<List<MatchWithBetsDTO>> GetAllMatches()
        {
            var allFutureMatches = await this.dbContext.Matches
                .Where(m => m.StartDate > DateTime.UtcNow
                    && m.StartDate <= DateTime.UtcNow.AddHours(24)
                    && m.MatchType != MatchTypeEnum.OutRight)
                .Include(m => m.Bets.Where(b => b.IsActive == true
                    && (b.Name == Constants.MatchWinner
                        || b.Name == Constants.MapAdvantage
                        || b.Name == Constants.TotalMapsPlayed)))
                    .ThenInclude(b => b.Odds.Where(o => o.IsActive == true))
                .ToListAsync();

            foreach (var match in allFutureMatches)
            {
                foreach (var bet in match.Bets)
                {
                    var oddsWithSpecialValue = bet.Odds.Where(o => !string.IsNullOrEmpty(o.SpecialBetValue)).ToList();

                    if (oddsWithSpecialValue.Count > 0)
                    {
                        var groupedOdds = oddsWithSpecialValue.GroupBy(o => o.SpecialBetValue);
                        var firstGroup = groupedOdds.Select(group => group.ToList()).FirstOrDefault();
                        bet.Odds = firstGroup;
                    }
                }
            }

            return allFutureMatches.Select(m => new MatchWithBetsDTO(m)).ToList();
        }

        /// <summary>
        /// Gets a Match object according to a specific Id along with all of its active and past Bets and Odds
        /// </summary>
        /// <param name="matchXmlId">Id of the Match object according to the XML document</param>
        /// <returns>Match object with all of its active and past Bets and Odds</returns>
        public async Task<MatchWithBetsDTO> GetMatch(int matchXmlId)
        {
            var match = await this.dbContext.Matches
                .Include(m => m.Bets)
                    .ThenInclude(b => b.Odds)
                .FirstOrDefaultAsync(m => m.Id == matchXmlId)
                ?? throw new ArgumentException();

            var matchDTO = new MatchWithBetsDTO(match);

            return matchDTO;
        }
    }
}
