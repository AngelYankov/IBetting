using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IBetting.DataAccess.Models
{
    public class Odd
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Value { get; set; }

        public string? SpecialBetValue { get; set; }

        [ForeignKey("Bet")]
        public int BetId { get; set; }

        public Bet Bet { get; set; }

        public bool IsActive { get; set; }
    }
}