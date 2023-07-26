using IBetting.DataAccess;
using IBetting.DataAccess.Enums;
using IBetting.DataAccess.Models;
using IBetting.Services;
using Microsoft.EntityFrameworkCore;

namespace IBetting.Tests.Utils
{
    public static class Utils
    {
        public static DbContextOptions<IBettingDbContext> GetOptions(string databaseName)
        {
            return new DbContextOptionsBuilder<IBettingDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
        }

        public static List<Match> SeedMatchData()
        {
            var testData = new List<Match>
            {
                new Match
                {
                    Id = 1,
                    Name = "Match 1",
                    StartDate = DateTime.UtcNow.AddHours(1),
                    MatchType = MatchTypeEnum.PreMatch,
                    Bets = new List<Bet>
                    {
                        new Bet
                        {
                            Id = 1,
                            Name = Constants.MatchWinner,
                            IsActive = true,
                            Odds = new List<Odd>
                            {
                                new Odd { Id = 1, Name = "Odd 1", Value = 1.5m, IsActive = true, SpecialBetValue = "1.5" },
                                new Odd { Id = 2, Name = "Odd 2", Value = 2.0m, IsActive = true, SpecialBetValue = "1.5" },
                                new Odd { Id = 13, Name = "Odd 13", Value = 2.0m, IsActive = true, SpecialBetValue = "3.5" },
                                new Odd { Id = 14, Name = "Odd 14", Value = 2.0m, IsActive = true, SpecialBetValue = "3.5" }
                            }
                        }
                    }
                },
                new Match
                {
                    Id = 2,
                    Name = "Match 2",
                    StartDate = DateTime.UtcNow.AddHours(1),
                    MatchType = MatchTypeEnum.PreMatch,
                    Bets = new List<Bet>
                    {
                        new Bet
                        {
                            Id = 2,
                            Name = Constants.TotalMapsPlayed,
                            IsActive = true,
                            Odds = new List<Odd>
                            {
                                new Odd { Id = 5, Name = "Odd 5", Value = 1.5m, IsActive = true },
                                new Odd { Id = 6, Name = "Odd 6", Value = 2.0m, IsActive = false }
                            }
                        }
                    }
                },
                new Match
                {
                    Id = 3,
                    Name = "Match 3",
                    StartDate = DateTime.UtcNow.AddHours(1),
                    MatchType = MatchTypeEnum.PreMatch,
                    Bets = new List<Bet>
                    {
                        new Bet
                        {
                            Id = 3,
                            Name = Constants.MapAdvantage,
                            IsActive = true,
                            Odds = new List<Odd>
                            {
                                new Odd { Id = 15, Name = "Odd 15", Value = 1.5m, IsActive = true },
                                new Odd { Id = 16, Name = "Odd 16", Value = 2.0m, IsActive = true }
                            }
                        }
                    }
                },
                new Match
                {
                    Id = 4,
                    Name = "Match 4",
                    StartDate = DateTime.UtcNow.AddHours(1),
                    MatchType = MatchTypeEnum.PreMatch,
                    Bets = new List<Bet>
                    {
                        new Bet
                        {
                            Id = 4,
                            Name = Constants.MapAdvantage,
                            IsActive = false,
                            Odds = new List<Odd>
                            {
                                new Odd { Id = 7, Name = "Odd 7", Value = 1.5m, IsActive = false },
                                new Odd { Id = 8, Name = "Odd 8", Value = 2.0m, IsActive = false }
                            }
                        }
                    }
                },
                new Match
                {
                    Id = 5,
                    Name = "Match 5",
                    StartDate = DateTime.UtcNow.AddHours(1),
                    MatchType = MatchTypeEnum.PreMatch,
                    Bets = new List<Bet>
                    {
                        new Bet
                        {
                            Id = 5,
                            Name = Constants.MatchWinner,
                            IsActive = false,
                            Odds = new List<Odd>
                            {
                                new Odd { Id = 9, Name = "Odd 9", Value = 1.5m, IsActive = true },
                                new Odd { Id = 10, Name = "Odd 10", Value = 2.0m, IsActive = true }
                            }
                        }
                    }
                },
                new Match
                {
                    Id = 6,
                    Name = "Match 6",
                    StartDate = DateTime.UtcNow.AddHours(1),
                    MatchType = MatchTypeEnum.PreMatch,
                    Bets = new List<Bet>
                    {
                        new Bet
                        {
                            Id = 6,
                            Name = "Total Goals",
                            IsActive = true,
                            Odds = new List<Odd>
                            {
                                new Odd { Id = 11, Name = "Odd 11", Value = 1.5m, IsActive = true },
                                new Odd { Id = 12, Name = "Odd 12", Value = 2.0m, IsActive = true }
                            }
                        }
                    }
                },
                new Match
                {
                    Id = 7,
                    Name = "Match 7",
                    StartDate = DateTime.UtcNow.AddMinutes(-10),
                    MatchType = MatchTypeEnum.Live,
                    Bets = new List<Bet>
                    {
                        new Bet
                        {
                            Id = 7,
                            Name = Constants.TotalMapsPlayed,
                            IsActive = true,
                            Odds = new List<Odd>
                            {
                                new Odd { Id = 3, Name = "Odd 3", Value = 1.7m, IsActive = true },
                                new Odd { Id = 4, Name = "Odd 4", Value = 2.2m, IsActive = true }
                            }
                        }
                    }
                },
                new Match
                {
                    Id = 8,
                    Name = "Match 8",
                    StartDate = DateTime.UtcNow.AddMinutes(-10),
                    MatchType = MatchTypeEnum.Live,
                    Bets = new List<Bet>
                    {
                        new Bet
                        {
                            Id = 8,
                            Name = Constants.TotalMapsPlayed,
                            IsActive = true,
                            Odds = new List<Odd>
                            {
                                new Odd { Id = 17, Name = "Odd 17", Value = 1.7m, IsActive = true },
                                new Odd { Id = 18, Name = "Odd 18", Value = 2.2m, IsActive = true }
                            }
                        },
                        new Bet
                        {
                            Id = 9,
                            Name = Constants.TotalMapsPlayed,
                            IsActive = false,
                            Odds = new List<Odd>
                            {
                                new Odd { Id = 19, Name = "Odd 19", Value = 1.7m, IsActive = false },
                                new Odd { Id = 20, Name = "Odd 20", Value = 2.2m, IsActive = false }
                            }
                        }
                    }
                }
            };

            return testData;
        }

    }
}
