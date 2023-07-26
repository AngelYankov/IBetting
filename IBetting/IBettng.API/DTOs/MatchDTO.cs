using IBetting.DataAccess.Enums;
using IBetting.DataAccess.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace IBettng.API.DTOs
{
    public class MatchDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public string MatchType { get; set; }

        public List<BetDTO> Bets { get; set; }

        public int EventId { get; set; }

        public bool IsActive { get; set; }
    }
}
