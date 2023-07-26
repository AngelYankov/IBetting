using AutoMapper;
using IBetting.Services.MatchService;
using IBettng.API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace IBettng.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchService matchService;
        private readonly IMapper mapper;

        public MatchesController(IMatchService matchService, IMapper mapper)
        {
            this.matchService = matchService;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get all Mathes starting in the next 24 hours along with all their active Bets and Odds
        /// </summary>
        /// <returns>Returns all Mathes starting in the next 24 hours along with all their active Bets and Odds 
        /// or just the Match info if no active Bets and Odds</returns>
        [HttpGet]
        public async Task<IActionResult> GetActiveMatches()
        {
            var matches = await this.matchService.GetAllMatchesAsync();
            var result = this.mapper.Map<List<MatchDTO>>(matches);

            return Ok(result);
        }

        /// <summary>
        /// Get Match by Id from Xml
        /// </summary>
        /// <param name="matchXmlId">Id of the Match from the XML document</param>
        /// <returns>Returns Match object with all active and past Bets and Odds 
        /// or 404 NotFound response if no Match object with such Id exists</returns>
        [HttpGet]
        [Route("{matchXmlId}")]
        public async Task<IActionResult> GetMatch(int matchXmlId)
        {
            try
            {
                var match = await this.matchService.GetMatchAsync(matchXmlId);
                var result = this.mapper.Map<MatchDTO>(match);

                return Ok(result);
            }
            catch (Exception)
            {
                Console.WriteLine("Match not found.");
                return NotFound();
            }
        }
    }
}
