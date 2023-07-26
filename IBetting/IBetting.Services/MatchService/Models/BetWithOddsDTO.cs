using IBetting.DataAccess.Models;
using IBetting.Services.BettingService.Models;

namespace IBetting.Services.MatchService.Models
{
    public class BetWithOddsDTO
    {
        public BetWithOddsDTO(Bet bet)
        {
            this.Id = bet.Id;
            this.IsLive = bet.IsLive;
            this.Name = bet.Name;
            this.IsActive = bet.IsActive;
            this.AllOdds = bet.Odds.Select(o => new OddDTO(o)).ToList();
        }

        public int Id { get; set; }

        public bool IsLive { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public List<OddDTO> AllOdds { get; set; }
    }
}
