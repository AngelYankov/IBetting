﻿using IBetting.DataAccess.Models;

namespace IBetting.Services.MatchService
{
    public interface IMatchService
    {
        Task<List<Match>> GetAllMatchesAsync();

        Task<Match> GetMatchAsync(int matchXmlId);
    }
}