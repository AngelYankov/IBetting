using IBetting.DataAccess.Enums;
using IBetting.DataAccess.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace IBettng.API.DTOs
{
    public class BetDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsLive { get; set; }

        public List<OddDTO> Odds { get; set; }

        public bool IsActive { get; set; }
    }
}
