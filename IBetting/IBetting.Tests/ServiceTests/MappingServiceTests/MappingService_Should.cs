using IBetting.DataAccess.Models;
using IBetting.Services.MappingService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace IBetting.Tests.ServiceTests.MappingServiceTests
{
    public class MappingService_Should
    {
        [Fact]
        public void Map_AllSports()
        {
            // Arrange
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(GetTransformedXml());

            var mappingService = new MappingService();

            // Act
            var result = mappingService.MapSports(xmlDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count());
            Assert.IsAssignableFrom<IEnumerable<Sport>>(result);
        }

        [Fact]
        public void Map_AllEvents()
        {
            // Arrange
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(GetTransformedXml());

            var mappingService = new MappingService();

            // Act
            var result = mappingService.MapEvents(xmlDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count());
        }

        [Fact]
        public void Map_AllMatches()
        {
            // Arrange
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(GetTransformedXml());

            var mappingService = new MappingService();

            // Act
            var result = mappingService.MapMatches(xmlDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.IsAssignableFrom<IEnumerable<Match>>(result);
        }

        [Fact]
        public void Map_AllBets()
        {
            // Arrange
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(GetTransformedXml());

            var mappingService = new MappingService();

            // Act
            var result = mappingService.MapBets(xmlDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count());
            Assert.IsAssignableFrom<IEnumerable<Bet>>(result);
        }

        [Fact]
        public void Map_AllOdds()
        {
            // Arrange
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(GetTransformedXml());

            var mappingService = new MappingService();

            // Act
            var result = mappingService.MapOdds(xmlDocument);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(14, result.Count());
            Assert.IsAssignableFrom<IEnumerable<Odd>>(result);
        }

        private string GetTransformedXml()
        {
            return @"<XmlSports xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" CreateDate=""2023-07-24T13:03:12.0551897Z""><Sport Name=""eSports"" ID=""2357""><Event Name=""FIFA, GT League"" ID=""62647"" IsLive=""false"" CategoryID=""8212"" SportId=""2357""><Match Name=""Juventus (nero) - SSC Napoli (spencer)"" ID=""3037424"" StartDate=""2023-07-24T12:59:00"" MatchType=""Live"" EventId=""62647""><Bet Name=""Match Odds"" ID=""47907418"" IsLive=""true"" MatchId=""3037424"" MatchType=""Live"" MatchStartDate=""24.7.2023 г. 12:59:00""><Odd Name=""1"" ID=""329682700"" Value=""1.55"" BetId=""47907418"" /><Odd Name=""X"" ID=""329682701"" Value=""5.43"" BetId=""47907418"" /><Odd Name=""2"" ID=""329682697"" Value=""3.70"" BetId=""47907418"" /></Bet><Bet Name=""Total Goals"" ID=""47907419"" IsLive=""true"" MatchId=""3037424"" MatchType=""Live"" MatchStartDate=""24.7.2023 г. 12:59:00""><Odd Name=""Over"" ID=""329682906"" Value=""1.73"" SpecialBetValue=""4.5"" BetId=""47907419"" /><Odd Name=""Under"" ID=""329682904"" Value=""2.01"" SpecialBetValue=""4.5"" BetId=""47907419"" /><Odd Name=""Over"" ID=""329682699"" Value=""1.81"" SpecialBetValue=""5.5"" BetId=""47907419"" /><Odd Name=""Under"" ID=""329682698"" Value=""1.91"" SpecialBetValue=""5.5"" BetId=""47907419"" /></Bet></Match><Match Name=""Lazio (aibothard) - AC Milan (general)"" ID=""3037426"" StartDate=""2023-07-24T12:59:00"" MatchType=""Live"" EventId=""62647""><Bet Name=""Match Odds"" ID=""47907416"" IsLive=""true"" MatchId=""3037426"" MatchType=""Live"" MatchStartDate=""24.7.2023 г. 12:59:00""><Odd Name=""1"" ID=""329682694"" Value=""3.49"" BetId=""47907416"" /><Odd Name=""X"" ID=""329682696"" Value=""4.32"" BetId=""47907416"" /><Odd Name=""2"" ID=""329682693"" Value=""1.72"" BetId=""47907416"" /></Bet><Bet Name=""Total Goals"" ID=""47907417"" IsLive=""true"" MatchId=""3037426"" MatchType=""Live"" MatchStartDate=""24.7.2023 г. 12:59:00""><Odd Name=""Over"" ID=""329682893"" Value=""1.81"" SpecialBetValue=""3.5"" BetId=""47907417"" /><Odd Name=""Under"" ID=""329682892"" Value=""1.91"" SpecialBetValue=""3.5"" BetId=""47907417"" /><Odd Name=""Over"" ID=""329682692"" Value=""2.26"" SpecialBetValue=""4.5"" BetId=""47907417"" /><Odd Name=""Under"" ID=""329682695"" Value=""1.58"" SpecialBetValue=""4.5"" BetId=""47907417"" /></Bet></Match></Event></Sport></XmlSports>";
        }
    }
}
