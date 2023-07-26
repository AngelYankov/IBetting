using IBetting.DataAccess.Repositories;
using IBetting.Services.DeserializeService;
using IBetting.Services.MappingService;

namespace IBetting.Services.DataSavingService
{
    public class DataSavingService : IDataSavingService
    {
        private readonly IXmlService xmlService;
        private readonly IMappingService mappingService;
        private readonly ISportRepository sportRepository;
        private readonly IEventRepository eventRepository;
        private readonly IMatchRepository matchRepository;
        private readonly IBetRepository betRepository;
        private readonly IOddRepository oddRepository;

        public DataSavingService(
            IXmlService xmlService,
            IMappingService mappingService,
            ISportRepository sportRepository,
            IEventRepository eventRepository,
            IMatchRepository matchRepository,
            IBetRepository betRepository,
            IOddRepository oddRepository)
        {
            this.xmlService = xmlService;
            this.mappingService = mappingService;
            this.sportRepository = sportRepository;
            this.eventRepository = eventRepository;
            this.matchRepository = matchRepository;
            this.betRepository = betRepository;
            this.oddRepository = oddRepository;
        }

        /// <summary>
        /// Saves all data from the XML
        /// </summary>
        public async Task Save()
        {
            var document = await this.xmlService.TransformXml();

            var allSports = this.mappingService.MapSports(document);
            this.sportRepository.SaveSports(allSports);

            var allEvents = this.mappingService.MapEvents(document);
            this.eventRepository.SaveEvents(allEvents);

            var allMatches = this.mappingService.MapMatches(document);
            this.matchRepository.SaveMatches(allMatches);

            var allBets = this.mappingService.MapBets(document);
            this.betRepository.SaveBets(allBets);

            var allOdds = this.mappingService.MapOdds(document);
            this.oddRepository.SaveOdds(allOdds);
        }
    }
}
