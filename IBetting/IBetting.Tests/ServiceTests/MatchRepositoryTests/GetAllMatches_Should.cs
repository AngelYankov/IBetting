using IBetting.DataAccess;
using IBetting.DataAccess.Enums;
using IBetting.DataAccess.Models;
using IBetting.DataAccess.Repositories;
using IBetting.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace IBetting.Tests.ServiceTests.MatchRepositoryTests
{
    public class GetAllMatches_Should
    {
        [Fact]
        public async Task GetAllMatches_ReturnsNotNullAndCorrectType()
        {
            // Arrange
            var options = Utils.Utils.GetOptions(nameof(GetAllMatches_ReturnsNotNullAndCorrectType));
            var testData = Utils.Utils.SeedMatchData();

            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(c => c.GetSection("ConnectionStrings")["DefaultConnection"]).Returns("TestConnectionString");

            using (var arrContext = new IBettingDbContext(options))
            {
                arrContext.Matches.AddRange(testData);
                arrContext.SaveChanges();
            }

            using (var actContext = new IBettingDbContext(options))
            {
                var matchService = new MatchRepository(actContext, configurationMock.Object);

                // Act
                var result = await matchService.GetAllMatchesAsync();

                // Assert
                Assert.NotNull(result);
                Assert.IsType<List<DataAccess.Models.Match>>(result);
            }
        }

        [Fact]
        public async Task GetAllMatches_ReturnsMatches_StartingInNext24Hours()
        {
            // Arrange
            var options = Utils.Utils.GetOptions(nameof(GetAllMatches_ReturnsMatches_StartingInNext24Hours));
            var testData = Utils.Utils.SeedMatchData();

            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(c => c.GetSection("ConnectionStrings")["DefaultConnection"]).Returns("TestConnectionString");

            using (var arrContext = new IBettingDbContext(options))
            {
                arrContext.Matches.AddRange(testData);
                arrContext.SaveChanges();
            }

            using (var actContext = new IBettingDbContext(options))
            {
                var matchService = new MatchRepository(actContext, configurationMock.Object);

                // Act
                var result = await matchService.GetAllMatchesAsync();

                // Assert
                foreach (var match in result)
                {
                    Assert.Equal(MatchTypeEnum.PreMatch, match.MatchType);
                    Assert.True(match.StartDate >= DateTime.UtcNow && match.StartDate < DateTime.UtcNow.AddHours(24));
                }
            }
        }

        [Fact]
        public async Task GetAllMatches_ReturnsMatches_WithCorrectBetType()
        {
            // Arrange
            var options = Utils.Utils.GetOptions(nameof(GetAllMatches_ReturnsMatches_WithCorrectBetType));
            var testData = Utils.Utils.SeedMatchData();

            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(c => c.GetSection("ConnectionStrings")["DefaultConnection"]).Returns("TestConnectionString");

            using (var context = new IBettingDbContext(options))
            {
                context.Matches.AddRange(testData);
                context.SaveChanges();
            }

            using (var context = new IBettingDbContext(options))
            {
                var matchService = new MatchRepository(context, configurationMock.Object);

                // Act
                var result = await matchService.GetAllMatchesAsync();

                // Assert
                foreach (var match in result)
                {
                    foreach (var bet in match.Bets)
                    {
                        Assert.True(bet.Name == Constants.MatchWinner
                            || bet.Name == Constants.TotalMapsPlayed
                            || bet.Name == Constants.MapAdvantage);
                    }
                }
            }
        }

        [Fact]
        public async Task GetAllMatches_ReturnsMatches_WithOnlyActiveBets()
        {
            // Arrange
            var options = Utils.Utils.GetOptions(nameof(GetAllMatches_ReturnsMatches_WithOnlyActiveBets));
            var testData = Utils.Utils.SeedMatchData();

            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(c => c.GetSection("ConnectionStrings")["DefaultConnection"]).Returns("TestConnectionString");

            using (var arrContext = new IBettingDbContext(options))
            {
                arrContext.Matches.AddRange(testData);
                arrContext.SaveChanges();
            }

            using (var actContext = new IBettingDbContext(options))
            {
                var matchService = new MatchRepository(actContext, configurationMock.Object);

                // Act
                var result = await matchService.GetAllMatchesAsync();

                // Assert
                foreach (var match in result)
                {
                    foreach (var bet in match.Bets)
                    {
                        Assert.True(bet.IsActive);
                    }
                }
            }
        }

        [Fact]
        public async Task GetAllMatches_ReturnsMatches_WithOnlyActiveOdds()
        {
            // Arrange
            var options = Utils.Utils.GetOptions(nameof(GetAllMatches_ReturnsMatches_WithOnlyActiveOdds));
            var testData = Utils.Utils.SeedMatchData();

            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(c => c.GetSection("ConnectionStrings")["DefaultConnection"]).Returns("TestConnectionString");

            using (var arrContext = new IBettingDbContext(options))
            {
                arrContext.Matches.AddRange(testData);
                arrContext.SaveChanges();
            }

            using (var actContext = new IBettingDbContext(options))
            {
                var matchService = new MatchRepository(actContext, configurationMock.Object);

                // Act
                var result = await matchService.GetAllMatchesAsync();

                // Assert
                foreach (var match in result)
                {
                    foreach (var bet in match.Bets)
                    {
                        foreach (var odd in bet.Odds)
                        {
                            Assert.True(odd.IsActive);
                        }
                    }
                }
            }
        }

        [Fact]
        public async Task GetAllMatches_ReturnsMatches_WhenHaveSpecialBetValue()
        {
            // Arrange
            var options = Utils.Utils.GetOptions(nameof(GetAllMatches_ReturnsMatches_WhenHaveSpecialBetValue));
            var testData = Utils.Utils.SeedMatchData();

            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(c => c.GetSection("ConnectionStrings")["DefaultConnection"]).Returns("TestConnectionString");

            using (var arrContext = new IBettingDbContext(options))
            {
                arrContext.Matches.AddRange(testData);
                arrContext.SaveChanges();
            }

            using (var actContext = new IBettingDbContext(options))
            {
                var matchService = new MatchRepository(actContext, configurationMock.Object);

                // Act
                var result = await matchService.GetAllMatchesAsync();

                // Assert
                foreach (var match in result)
                {
                    foreach (var bet in match.Bets)
                    {
                        var oddsWithSpecialValue = bet.Odds.Where(o => !string.IsNullOrEmpty(o.SpecialBetValue)).ToList();

                        if (oddsWithSpecialValue.Count > 0)
                        {
                            Assert.NotNull(bet.Odds.Select(o => o.SpecialBetValue));
                        }
                        else
                        {
                            Assert.True(bet.Odds.All(o => o.SpecialBetValue == null));
                        }
                    }
                }
            }
        }
    }
}
