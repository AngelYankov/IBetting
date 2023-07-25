using IBetting.DataAccess.Enums;

namespace IBetting.DataAccess.Models
{
    public class MatchChangeLog
    {
        public int Id { get; set; }

        public int MatchXmlId { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public MatchTypeEnum MatchType { get; set; }

        public int EventId { get; set; }

        public string ActionToTake { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
