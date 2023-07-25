﻿namespace IBetting.DataAccess.Models
{
    public class OddChangeLog
    {
        public int Id { get; set; }

        public int OddXmlId { get; set; }

        public string Name { get; set; }

        public decimal Value { get; set; }

        public string? SpecialBetValue { get; set; }

        public int BetId { get; set; }

        public string ActionToTake { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
