﻿using IBetting.DataAccess;
using IBetting.Services.MatchService.Models;
using IBetting.Services.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;

namespace IBetting.Tests.ServiceTests.MatchRepositoryTests
{
    public class GetMatch_Should
    {
        [Fact]
        public async Task ReturnsNotNull_AndCorrectEntity()
        {
            // Arrange
            var options = Utils.Utils.GetOptions(nameof(ReturnsNotNull_AndCorrectEntity));
            var testData = Utils.Utils.SeedMatchData();

            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(c => c.GetSection("ConnectionStrings")["DefaultConnection"]).Returns("TestConnectionString");

            var matchXmlId = 1;

            using (var arrContext = new IBettingDbContext(options))
            {
                arrContext.Matches.AddRange(testData);
                arrContext.SaveChanges();
            }

            using (var actContext = new IBettingDbContext(options))
            {
                var matchService = new MatchRepository(actContext, configurationMock.Object);

                // Act
                var result = await matchService.GetMatchAsync(matchXmlId);

                // Assert
                Assert.NotNull(result);
                Assert.IsType<MatchWithBetsDTO>(result);
                Assert.Equal(matchXmlId, result.Id);
            }
        }

        [Fact]
        public async Task ReturnsActiveAndInactive_Bets()
        {
            // Arrange
            var options = Utils.Utils.GetOptions(nameof(ReturnsActiveAndInactive_Bets));
            var testData = Utils.Utils.SeedMatchData();

            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(c => c.GetSection("ConnectionStrings")["DefaultConnection"]).Returns("TestConnectionString");

            var matchXmlId = 8;

            using (var arrContext = new IBettingDbContext(options))
            {
                arrContext.Matches.AddRange(testData);
                arrContext.SaveChanges();
            }

            using (var actContext = new IBettingDbContext(options))
            {
                var matchService = new MatchRepository(actContext, configurationMock.Object);

                // Act
                var result = await matchService.GetMatchAsync(matchXmlId);

                // Assert
                Assert.Contains(result.AllBets, b => b.IsActive);
                Assert.Contains(result.AllBets, b => !b.IsActive);
            }
        }

        [Fact]
        public async Task ReturnsActiveAndInactive_Odds()
        {
            // Arrange
            var options = Utils.Utils.GetOptions(nameof(ReturnsActiveAndInactive_Odds));
            var testData = Utils.Utils.SeedMatchData();

            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(c => c.GetSection("ConnectionStrings")["DefaultConnection"]).Returns("TestConnectionString");

            var matchXmlId = 8;

            using (var arrContext = new IBettingDbContext(options))
            {
                arrContext.Matches.AddRange(testData);
                arrContext.SaveChanges();
            }

            using (var actContext = new IBettingDbContext(options))
            {
                var matchService = new MatchRepository(actContext, configurationMock.Object);

                // Act
                var result = await matchService.GetMatchAsync(matchXmlId);

                // Assert
                foreach (var bet in result.AllBets)
                {
                    if (bet.IsActive)
                    {
                        Assert.Contains(bet.AllOdds, o => o.IsActive);
                    }
                    else
                    {
                        Assert.Contains(bet.AllOdds, o => !o.IsActive);
                    }
                }
            }
        }

        [Fact]
        public async Task ThrowsWhenMatch_DoesNotExist()
        {
            // Arrange
            var options = Utils.Utils.GetOptions(nameof(ThrowsWhenMatch_DoesNotExist));
            var testData = Utils.Utils.SeedMatchData();

            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(c => c.GetSection("ConnectionStrings")["DefaultConnection"]).Returns("TestConnectionString");

            var nonExistentMatchXmlId = 10;

            using (var arrContext = new IBettingDbContext(options))
            {
                arrContext.Matches.AddRange(testData);
                arrContext.SaveChanges();
            }

            using (var actContext = new IBettingDbContext(options))
            {
                var matchService = new MatchRepository(actContext, configurationMock.Object);

                // Act and Assert
                await Assert.ThrowsAsync<ArgumentException>(() => matchService.GetMatchAsync(nonExistentMatchXmlId));
            }
        }
    }
}
