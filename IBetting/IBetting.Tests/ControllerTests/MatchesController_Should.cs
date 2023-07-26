using IBetting.Services.MatchService;
using IBettng.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace IBetting.Tests.ControllerTests
{
    public class MatchesController_Should
    {
        [Fact]
        public async Task GetActiveMatches_ReturnsOkResult()
        {
            // Arrange
            var mockMatchService = new Mock<IMatchService>();
            var controller = new MatchesController(mockMatchService.Object);

            // Act
            var result = await controller.GetActiveMatches();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetMatch_ReturnsOkResult()
        {
            // Arrange
            var mockMatchService = new Mock<IMatchService>();
            var controller = new MatchesController(mockMatchService.Object);

            var matchId = 1;

            // Act
            var result = await controller.GetMatch(matchId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetMatch_ReturnsNotFoundResult()
        {
            var matchId = 10;

            // Arrange
            var mockMatchService = new Mock<IMatchService>();
            mockMatchService.Setup(service => service.GetMatchAsync(matchId)).ThrowsAsync(new ArgumentException("Match not found."));

            var controller = new MatchesController(mockMatchService.Object);

            // Act
            var result = await controller.GetMatch(matchId);

            // Assert
            var okResult = Assert.IsType<NotFoundResult>;
        }
    }
}
