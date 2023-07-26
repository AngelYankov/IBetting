using IBetting.Services.MatchService.Models;
using IBetting.Services.Repositories;

namespace IBetting.Services.MatchService
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository matchRepository;

        public MatchService(IMatchRepository matchRepository)
        {
            this.matchRepository = matchRepository;
        }

        /// <summary>
        /// Call to match repository to get matches in next 24 hours
        /// </summary>
        /// <returns>List with all Match objects starting in the next 24 hours along with all their active Bets and Odds</returns>
        public async Task<List<MatchWithBetsDTO>> GetAllMatchesAsync()
        {
            var matches = await this.matchRepository.GetAllMatchesAsync();

            return matches;
        }

        /// <summary>
        /// Call to the match repository to get Match by id
        /// </summary>
        /// <param name="matchXmlId">Id of the Match object according to the XML document</param>
        /// <returns>Match object with all of its active and past Bets and Odds</returns>
        public async Task<MatchWithBetsDTO> GetMatchAsync(int matchXmlId)
        {
            var match = await this.matchRepository.GetMatchAsync(matchXmlId);

            return match;
        }
    }
}
