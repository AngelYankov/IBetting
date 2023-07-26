using IBetting.Services.DataConsumeService;
using IBetting.Services.DeserializeService;
using Moq;
using System.Xml;

namespace IBetting.Tests.ServiceTests.TransformXmlServiceTests
{
    public class TransformXml_Should
    {
        private const string XmlPath = @"..\..\..\TestDataXML.xml";

        [Fact]
        public async Task TransformXml_ShouldAddAttributesToXml()
        {
            // Arrange
            var xmlContent = File.ReadAllText(XmlPath);
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlContent);

            var mockDataConsumeService = new Mock<IDataConsumeService>();
            mockDataConsumeService.Setup(service => service.LoadFile()).ReturnsAsync(xmlDocument);

            var xmlService = new XmlService(mockDataConsumeService.Object);

            // Act
            var transformedDocument = await xmlService.TransformXml();

            // Assert
            Assert.NotNull(transformedDocument);
            Assert.True(transformedDocument.DocumentElement.HasAttribute("CreateDate"));

            // Verify attributes in Sport nodes
            var sportNodes = transformedDocument.SelectNodes("XmlSports/Sport");
            Assert.NotNull(sportNodes);
            Assert.Equal(1, sportNodes.Count);

            foreach (XmlNode sportNode in sportNodes)
            {
                Assert.True(sportNode.Attributes["ID"] != null);
                Assert.True(sportNode.ChildNodes.Count > 0);

                // Verify attributes in Event nodes
                var eventNodes = sportNode.SelectNodes("Event");
                Assert.NotNull(eventNodes);

                foreach (XmlNode eventNode in eventNodes)
                {
                    Assert.True(eventNode.Attributes["ID"] != null);
                    Assert.True(eventNode.ChildNodes.Count > 0);

                    // Verify attributes in Match nodes
                    var matchNodes = eventNode.SelectNodes("Match");
                    Assert.NotNull(matchNodes);

                    foreach (XmlNode matchNode in matchNodes)
                    {
                        Assert.True(matchNode.Attributes["ID"] != null);
                        Assert.True(matchNode.Attributes["MatchType"] != null);
                        Assert.True(matchNode.Attributes["StartDate"] != null);
                        Assert.True(matchNode.ChildNodes.Count > 0);

                        // Verify attributes in Bet nodes
                        var betNodes = matchNode.SelectNodes("Bet");
                        Assert.NotNull(betNodes);

                        foreach (XmlNode betNode in betNodes)
                        {
                            Assert.True(betNode.Attributes["ID"] != null);
                            Assert.True(betNode.Attributes["IsLive"] != null);
                            Assert.True(betNode.ChildNodes.Count > 0);
                        }
                    }
                }
            }
        }
    }
}
