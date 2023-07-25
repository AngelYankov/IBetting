using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IBetting.DataAccess.Models
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsLive { get; set; }

        public int CategoryID { get; set; }

        public List<Match> Matches { get; set; }

        [ForeignKey("Sport")]
        public int SportId { get; set; }

        public Sport Sport { get; set; }

        public bool IsActive { get; set; }
    }
}