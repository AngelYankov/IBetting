using IBetting.DataAccess.Models;

namespace IBetting.Services.MatchService.Models
{
    public class MatchWithBetsDTO
    {
        public MatchWithBetsDTO(Match match)
        {
            this.Id = match.Id;
            this.Name = match.Name;
            this.StartDate = match.StartDate;
            this.MatchType = match.MatchType.ToString();
            this.AllBets = match.Bets.Select(b => new BetWithOddsDTO(b)).ToList();
            this.EventId = match.EventId;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public string MatchType { get; set; }

        public List<BetWithOddsDTO> AllBets { get; set; }

        public int EventId { get; set; }
    }
}
