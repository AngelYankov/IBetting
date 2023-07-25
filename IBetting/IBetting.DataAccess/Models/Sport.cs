using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IBetting.DataAccess.Models
{
    public class Sport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Event> Events { get; set; }

        public bool IsActive { get; set; }
    }
}
