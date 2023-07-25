using IBetting.DataAccess.Models;

namespace IBetting.Services.BettingService.Models
{
    public class OddDTO
    {
        public OddDTO(Odd odd)
        {
            this.Id = odd.Id;
            this.Name = odd.Name;
            this.Value = odd.Value;
            this.BetId = odd.BetId;
            this.SpecialBetValue = odd.SpecialBetValue;
            this.IsActive = odd.IsActive;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Value { get; set; }

        public int BetId { get; set; }

        public string SpecialBetValue { get; set; }

        public bool IsActive { get; set; }
    }
}
