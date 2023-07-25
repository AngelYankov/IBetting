using IBetting.DataAccess.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IBetting.DataAccess.Models
{
    public class Bet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsLive { get; set; }

        [ForeignKey("Match")]
        public int MatchId { get; set; }

        public Match Match { get; set; }

        public List<Odd> Odds { get; set; }

        public MatchTypeEnum MatchType { get; set; }

        public DateTime MatchStartDate { get; set; }

        public bool IsActive { get; set; }
    }
}