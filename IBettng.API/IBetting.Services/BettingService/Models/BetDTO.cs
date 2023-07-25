using IBetting.DataAccess.Enums;
using IBetting.DataAccess.Models;
namespace IBetting.Services.BettingService.Models
{
    public class BetDTO
    {
        public BetDTO(Bet bet)
        {
            this.Id = bet.Id;
            this.IsLive = bet.IsLive;
            this.MatchId = bet.MatchId;
            this.Name = bet.Name;
            this.MatchStartDate = bet.MatchStartDate;
            this.MatchType = bet.MatchType;
            this.IsActive = bet.IsActive;
        }

        public int Id { get; set; }

        public bool IsLive { get; set; }

        public int MatchId { get; set; }

        public string Name { get; set; }

        public MatchTypeEnum MatchType { get; set; }

        public DateTime MatchStartDate { get; set; }

        public bool IsActive { get; set; }
    }
}
