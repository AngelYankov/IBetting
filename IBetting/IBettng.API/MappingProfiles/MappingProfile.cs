using AutoMapper;
using IBetting.DataAccess.Models;
using IBettng.API.DTOs;

namespace IBettng.API.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Match, MatchDTO>();

            CreateMap<Bet, BetDTO>();

            CreateMap<Odd, OddDTO>();
        }
    }
}
