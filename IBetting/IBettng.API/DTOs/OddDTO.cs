using IBetting.DataAccess.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace IBettng.API.DTOs
{
    public class OddDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Value { get; set; }

        public string? SpecialBetValue { get; set; }

        public bool IsActive { get; set; }
    }
}
