using IBetting.DataAccess.Enums;
using IBetting.DataAccess.Models;
using IBetting.Services.DataConsumeService;
using System.Xml;

namespace IBetting.Services.DeserializeService
{
    public class XmlService : IXmlService
    {
        private readonly IDataConsumeService dataConsumeService;

        public XmlService(IDataConsumeService dataConsumeService)
        {
            this.dataConsumeService = dataConsumeService;
        }

        /// <summary>
        /// Adds attributes with data need to XML
        /// </summary>
        /// <returns>New XML with added data</returns>
        public async Task<XmlDocument> TransformXml()
        {
            var document = await this.dataConsumeService.LoadFile();

            var sports = document.SelectNodes(Constants.SportNodes);

            for (int m = 0; m < sports.Count; m++)
            {
                var sportEntity = new Sport()
                {
                    Id = Int32.Parse(sports[m].Attributes[Constants.Id].Value)
                };

                var events = sports[m].ChildNodes;

                for (int i = 0; i < events.Count; i++)
                {
                    var sportIdAttribute = document.CreateAttribute(Constants.SportId);
                    sportIdAttribute.Value = sportEntity.Id.ToString();
                    events[i].Attributes.Append(sportIdAttribute);

                    var eventEntity = new Event()
                    {
                        Id = Int32.Parse(events[i].Attributes[Constants.Id].Value),
                    };

                    var matches = events[i].ChildNodes;

                    for (int j = 0; j < matches.Count; j++)
                    {
                        var eventIdAttribute = document.CreateAttribute(Constants.EventId);
                        eventIdAttribute.Value = eventEntity.Id.ToString();
                        matches[j].Attributes.Append(eventIdAttribute);

                        var matchEntity = new Match()
                        {
                            Id = Int32.Parse(matches[j].Attributes[Constants.Id].Value),
                            MatchType = Enum.Parse<MatchTypeEnum>(matches[j].Attributes[Constants.MatchType].Value),
                            StartDate = DateTime.Parse(matches[j].Attributes[Constants.StartDate].Value)
                        };

                        var bets = matches[j].ChildNodes;

                        for (int k = 0; k < bets.Count; k++)
                        {
                            var attributeMatchId = document.CreateAttribute(Constants.MatchId);
                            attributeMatchId.Value = matchEntity.Id.ToString();
                            bets[k].Attributes.Append(attributeMatchId);

                            var attributeMatchType = document.CreateAttribute(Constants.MatchType);
                            attributeMatchType.Value = matchEntity.MatchType.ToString();
                            bets[k].Attributes.Append(attributeMatchType);

                            var attributeMatchStartDate = document.CreateAttribute(Constants.MatchStartDate);
                            attributeMatchStartDate.Value = matchEntity.StartDate.ToString();
                            bets[k].Attributes.Append(attributeMatchStartDate);

                            var betEntity = new Bet()
                            {
                                Id = Int32.Parse(bets[k].Attributes[Constants.Id].Value),
                            };

                            var odds = bets[k].ChildNodes;

                            for (int l = 0; l < odds.Count; l++)
                            {
                                var attributeBetId = document.CreateAttribute(Constants.BetId);
                                attributeBetId.Value = betEntity.Id.ToString();
                                odds[l].Attributes.Append(attributeBetId);
                            }
                        }
                    }
                }
            }

            return document;
        }
    }
}
