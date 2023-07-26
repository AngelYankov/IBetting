using IBetting.DataAccess;
using IBetting.DataAccess.Enums;
using IBetting.Services.BettingService.Models;
using IBetting.Services.Extensions;
using IBetting.Services.MatchService.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IBetting.Services.Repositories
{
    public class MatchRepository : BaseRepository, IMatchRepository
    {
        private readonly string? connectionString;

        public MatchRepository(IBettingDbContext dbContext, IConfiguration configuration)
            : base(dbContext)
        {
            this.connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Adds, Updates and Deletes Match objects from Match database table according to current XML document
        /// Adds new MatchChangeLog objects to MatchChangeLogs database table when records are updated or deleted in Match database table
        /// </summary>
        /// <param name="allMatches">All Match objects from current XML document</param>
        public bool SaveMatches(IEnumerable<MatchDTO> allMatches)
        {
            using (SqlConnection connection = new SqlConnection() { ConnectionString = connectionString })
            {
                using (SqlCommand command = new SqlCommand("", connection))
                {
                    try
                    {
                        connection.Open();

                        command.CommandText = @"CREATE TABLE #TmpMatchTable(
                            [Id] [INT],
                            [Name] [TEXT],
                            [StartDate] [DATETIME],
                            [MatchType] [INT],
                            [EventId] [INT],
                            [IsActive] [BIT])";

                        command.ExecuteNonQuery();

                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                        {
                            bulkCopy.DestinationTableName = "#TmpMatchTable";
                            bulkCopy.WriteToServer(allMatches.ToDataTable());
                        }

                        command.CommandText = @"
                            INSERT INTO dbo.MatchChangeLogs
                                SELECT 
                                    MatchXmlId,
                                    Name,
                                    StartDate,
                                    MatchType,
                                    EventId,
                                    ActionToTake,
                                    DateCreated
                                FROM (
                                    MERGE INTO dbo.Matches AS TARGET
                                    USING dbo.#TmpMatchTable AS SOURCE
                                    ON TARGET.Id = SOURCE.Id AND (
                                       TARGET.MatchType != SOURCE.MatchType OR
                                       TARGET.StartDate != SOURCE.StartDate )
                                    WHEN MATCHED THEN
                                        UPDATE SET TARGET.MatchType = SOURCE.MatchType,
                                                   TARGET.StartDate = SOURCE.StartDate
                                    OUTPUT $action as ActionType, INSERTED.Id as MatchXmlId, INSERTED.Name, INSERTED.StartDate, INSERTED.MatchType, INSERTED.EventId, GETUTCDATE() as DateCreated, 'UPDATE' as ActionToTake
                                ) AllChanges (ActionType, MatchXmlId, Name, StartDate, MatchType, EventId, DateCreated, ActionToTake)
                                WHERE AllChanges.ActionType = 'UPDATE'

                            INSERT INTO dbo.MatchChangeLogs
                                SELECT 
                                    MatchXmlId,
                                    Name,
                                    StartDate,
                                    MatchType,
                                    EventId,
                                    ActionToTake,
                                    DateCreated
                                FROM (
                                    MERGE INTO dbo.Matches AS TARGET
                                    USING dbo.#TmpMatchTable AS SOURCE
                                    ON TARGET.Id = SOURCE.Id 
                                    WHEN NOT MATCHED BY TARGET THEN
                                        INSERT (Id, Name, StartDate, MatchType, EventId, IsActive)
                                        VALUES (SOURCE.Id, SOURCE.Name, SOURCE.StartDate, SOURCE.MatchType, SOURCE.EventId, SOURCE.IsActive)
                                    WHEN NOT MATCHED BY SOURCE THEN
                                         UPDATE SET TARGET.IsActive = 0
                                    OUTPUT $action as ActionType, INSERTED.Id as MatchXmlId, INSERTED.Name, INSERTED.StartDate, INSERTED.MatchType, INSERTED.EventId, GETUTCDATE() as DateCreated, 'DELETE' as ActionToTake
                                ) AllChanges (ActionType, MatchXmlId, Name, StartDate, MatchType, EventId, DateCreated, ActionToTake)
                                WHERE AllChanges.ActionType = 'UPDATE'
                                DROP TABLE #TmpMatchTable";

                        command.ExecuteNonQuery();

                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Gets all Match objects starting in the next 24 hours along with all their active Bets and Odds
        /// </summary>
        /// <returns>List with all Match objects starting in the next 24 hours along with all their active Bets and Odds</returns>
        public async Task<List<MatchWithBetsDTO>> GetAllMatchesAsync()
        {
            var allFutureMatches = await this.dbContext.Matches
                .Where(m => m.StartDate > DateTime.UtcNow
                    && m.StartDate < DateTime.UtcNow.AddHours(24)
                    && m.MatchType != MatchTypeEnum.OutRight)
                .Include(m => m.Bets.Where(b => b.IsActive == true
                    && (b.Name == Constants.MatchWinner
                        || b.Name == Constants.MapAdvantage
                        || b.Name == Constants.TotalMapsPlayed)))
                    .ThenInclude(b => b.Odds.Where(o => o.IsActive == true))
                .ToListAsync();

            foreach (var match in allFutureMatches)
            {
                foreach (var bet in match.Bets)
                {
                    var oddsWithSpecialValue = bet.Odds.Where(o => !string.IsNullOrEmpty(o.SpecialBetValue)).ToList();

                    if (oddsWithSpecialValue.Count > 0)
                    {
                        var groupedOdds = oddsWithSpecialValue.GroupBy(o => o.SpecialBetValue);
                        var firstGroup = groupedOdds.Select(group => group.ToList()).FirstOrDefault();
                        bet.Odds = firstGroup;
                    }
                }
            }

            return allFutureMatches.Select(m => new MatchWithBetsDTO(m)).ToList();
        }

        /// <summary>
        /// Gets a Match object according to a specific Id along with all of its active and past Bets and Odds
        /// </summary>
        /// <param name="matchXmlId">Id of the Match object according to the XML document</param>
        /// <returns>Match object with all of its active and past Bets and Odds</returns>
        public async Task<MatchWithBetsDTO> GetMatchAsync(int matchXmlId)
        {
            var match = await this.dbContext.Matches
                .Include(m => m.Bets)
                    .ThenInclude(b => b.Odds)
                .FirstOrDefaultAsync(m => m.Id == matchXmlId)
                ?? throw new ArgumentException("Match not found.");

            var matchDTO = new MatchWithBetsDTO(match);

            return matchDTO;
        }
    }
}
