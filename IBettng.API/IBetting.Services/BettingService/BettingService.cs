using IBetting.Services.BettingService.Models;
using IBetting.Services.DeserializeService;
using IBetting.Services.Extensions;
using IBetting.Services.MappingService;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace IBetting.Services.BettingService
{
    public class BettingService : IBettingService
    {
        private readonly string? connectionString;
        private readonly IXmlService xmlService;
        private readonly IMappingService mappingService;

        public BettingService(IConfiguration configuration, IXmlService xmlService, IMappingService mappingService)
        {
            this.connectionString = configuration.GetConnectionString("DefaultConnection");
            this.xmlService = xmlService;
            this.mappingService = mappingService;
        }

        /// <summary>
        /// Saves all data from the XML
        /// </summary>
        public async Task Save()
        {
            var document = await this.xmlService.TransformXml();

            var allSports = this.mappingService.MapSports(document);
            AddSports(allSports);

            var allEvents = this.mappingService.MapEvents(document);
            AddEvents(allEvents);

            var allMatches = this.mappingService.MapMatches(document);
            AddMatches(allMatches);

            var allBets = this.mappingService.MapBets(document);
            AddBets(allBets);

            var allOdds = this.mappingService.MapOdds(document);
            AddOdds(allOdds);
        }

        /// <summary>
        /// Adds, Updates and Deletes Sport objects from Sport database table according to current XML document
        /// </summary>
        /// <param name="allSports">All Sport objects from current XML document</param>
        private void AddSports(IEnumerable<SportDTO> allSports)
        {
            using (SqlConnection connection = new SqlConnection() { ConnectionString = connectionString })
            {
                using (SqlCommand command = new SqlCommand("", connection))
                {
                    try
                    {
                        connection.Open();

                        command.CommandText = @"CREATE TABLE #TmpSportsTable(
                            [Id] [INT],
                            [Name] [TEXT],
                            [IsActive] [BIT])";

                        command.ExecuteNonQuery();

                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                        {
                            bulkCopy.DestinationTableName = "#TmpSportsTable";
                            bulkCopy.WriteToServer(allSports.ToDataTable());
                        }

                        command.CommandText = @"
                            MERGE INTO dbo.Sports AS TARGET
                            USING dbo.#TmpSportsTable AS SOURCE
                            ON TARGET.Id = SOURCE.Id
                            WHEN MATCHED THEN
                                UPDATE SET TARGET.Name = SOURCE.Name
                            WHEN NOT MATCHED BY TARGET THEN
                                INSERT (Id, Name, IsActive)
                                VALUES (SOURCE.Id, SOURCE.Name, SOURCE.IsActive)
                            WHEN NOT MATCHED BY SOURCE THEN
                                UPDATE SET TARGET.IsActive = 0;
                            DROP TABLE #TmpSportsTable";

                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Adds, Updates and Deletes Event objects from Event database table according to current XML document
        /// </summary>
        /// <param name="allEvents">All Event objects from current XML document</param>
        private void AddEvents(IEnumerable<EventDTO> allEvents)
        {
            using (SqlConnection connection = new SqlConnection() { ConnectionString = connectionString })
            {
                using (SqlCommand command = new SqlCommand("", connection))
                {
                    try
                    {
                        connection.Open();

                        command.CommandText = @"CREATE TABLE #TmpEventTable(
                            [Id] [INT],
                            [Name] [TEXT],
                            [IsLive] [BIT],
                            [CategoryId] [INT],
                            [SportId] [INT],
                            [IsActive] [BIT])";

                        command.ExecuteNonQuery();

                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                        {
                            bulkCopy.DestinationTableName = "#TmpEventTable";
                            bulkCopy.WriteToServer(allEvents.ToDataTable());
                        }

                        command.CommandText = @"
                            MERGE INTO dbo.Events AS TARGET
                            USING dbo.#TmpEventTable AS SOURCE
                            ON TARGET.Id = SOURCE.Id
                            WHEN MATCHED THEN
                                UPDATE SET TARGET.IsLive = SOURCE.IsLive
                            WHEN NOT MATCHED BY TARGET THEN
                                INSERT (Id, Name, IsLive, CategoryId, SportId, IsActive)
                                VALUES (SOURCE.Id, SOURCE.Name, SOURCE.IsLive, SOURCE.CategoryId, SOURCE.SportId, SOURCE.IsActive)
                            WHEN NOT MATCHED BY SOURCE THEN
                                UPDATE SET TARGET.IsActive = 0;
                            DROP TABLE #TmpEventTable";

                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Adds, Updates and Deletes Match objects from Match database table according to current XML document
        /// Adds new MatchChangeLog objects to MatchChangeLogs database table when records are updated or deleted in Match database table
        /// </summary>
        /// <param name="allMatches">All Match objects from current XML document</param>
        private void AddMatches(IEnumerable<MatchDTO> allMatches)
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
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Adds, Updates and Deletes Bet objects from Bet database table according to current XML document
        /// Adds new BetChangeLog objects to BetChangeLogs database table when records are updated or deleted in Bet database table
        /// </summary>
        /// <param name="allBets">All Bet objects from current XML document</param>
        private void AddBets(IEnumerable<BetDTO> allBets)
        {
            using (SqlConnection connection = new SqlConnection() { ConnectionString = connectionString })
            {
                using (SqlCommand command = new SqlCommand("", connection))
                {
                    try
                    {
                        connection.Open();

                        command.CommandText = @"CREATE TABLE #TmpBetTable(
                            [Id] [INT],
                            [IsLive] [BIT],
                            [MatchId] [INT],
                            [Name] [TEXT],
                            [MatchType] [INT],
                            [MatchStartDate] [DATETIME],
                            [IsActive] [BIT])";

                        command.ExecuteNonQuery();

                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                        {
                            bulkCopy.DestinationTableName = "#TmpBetTable";
                            bulkCopy.WriteToServer(allBets.ToDataTable());
                        }

                        command.CommandText = @"
                            INSERT INTO dbo.BetChangeLogs
                                SELECT 
                                    BetXmlId,
                                    Name,
                                    IsLive,
                                    MatchId,
                                    ActionToTake,
                                    DateCreated
                                FROM (
                                    MERGE INTO dbo.Bets AS TARGET
                                    USING dbo.#TmpBetTable AS SOURCE
                                    ON TARGET.Id = SOURCE.Id AND
                                       TARGET.IsLive != SOURCE.IsLive
                                    WHEN MATCHED THEN
                                        UPDATE SET TARGET.IsLive = SOURCE.IsLive
                                    OUTPUT $action as ActionType, INSERTED.Id as BetXmlId, INSERTED.Name, INSERTED.IsLive, INSERTED.MatchId, GETUTCDATE() as DateCreated, 'UPDATE' as ActionToTake
                                ) AllChanges (ActionType, BetXmlId, Name, IsLive, MatchId, DateCreated, ActionToTake)
                                WHERE AllChanges.ActionType = 'UPDATE'

                            INSERT INTO dbo.BetChangeLogs
                                SELECT 
                                    BetXmlId,
                                    Name,
                                    IsLive,
                                    MatchId,
                                    ActionToTake,
                                    DateCreated
                                FROM (
                                    MERGE INTO dbo.Bets AS TARGET
                                    USING dbo.#TmpBetTable AS SOURCE
                                    ON TARGET.Id = SOURCE.Id 
                                    WHEN NOT MATCHED BY TARGET THEN
                                        INSERT (Id, Name, IsLive, MatchId, MatchStartDate, MatchType, IsActive)
                                        VALUES (SOURCE.Id, SOURCE.Name, SOURCE.IsLive, SOURCE.MatchId, SOURCE.MatchStartDate, SOURCE.MatchType, SOURCE.IsActive)
                                    WHEN NOT MATCHED BY SOURCE THEN
                                         UPDATE SET TARGET.IsActive = 0
                                    OUTPUT $action as ActionType, INSERTED.Id as BetXmlId, INSERTED.Name, INSERTED.IsLive, INSERTED.MatchId, GETUTCDATE() as DateCreated, 'DELETE' as ActionToTake
                                ) AllChanges (ActionType, BetXmlId, Name, IsLive, MatchId, DateCreated, ActionToTake)
                                WHERE AllChanges.ActionType = 'UPDATE'
                                DROP TABLE #TmpBetTable";

                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Adds, Updates and Deletes Odd objects from Odd database table according to current XML document
        /// Adds new OddChangeLog objects to OddChangeLogs database table when records are updated or deleted in Odd database table
        /// </summary>
        /// <param name="allOdds">All Odd objects from current XML document</param>
        private void AddOdds(IEnumerable<OddDTO> allOdds)
        {
            using (SqlConnection connection = new SqlConnection() { ConnectionString = connectionString })
            {
                using (SqlCommand command = new SqlCommand("", connection))
                {
                    try
                    {
                        connection.Open();

                        command.CommandText = @"CREATE TABLE #TmpOddTable(
                            [Id] [INT],
                            [Name] [TEXT],
                            [VALUE] [DECIMAL],
                            [BetId] [INT],
                            [SpecialBetValue] [TEXT] NULL,
                            [IsActive] [BIT])";

                        command.ExecuteNonQuery();

                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                        {
                            bulkCopy.DestinationTableName = "#TmpOddTable";
                            bulkCopy.WriteToServer(allOdds.ToDataTable());
                        }

                        command.CommandText = @"
                            INSERT INTO dbo.OddChangeLogs
                                SELECT 
                                    OddXmlId,
                                    Name,
                                    Value,
                                    SpecialBetValue,
                                    BetId,
                                    ActionToTake,
                                    DateCreated
                                FROM (
                                    MERGE INTO dbo.Odds AS TARGET
                                    USING dbo.#TmpOddTable AS SOURCE
                                    ON TARGET.Id = SOURCE.Id AND
                                       TARGET.Value != SOURCE.Value 
                                    WHEN MATCHED THEN
                                        UPDATE SET TARGET.Value = SOURCE.Value
                                    OUTPUT $action as ActionType, INSERTED.Id as OddXmlId, INSERTED.Name, INSERTED.Value, INSERTED.SpecialBetValue, INSERTED.BetId, GETUTCDATE() as DateCreated, 'UPDATE' as ActionToTake
                                ) AllChanges (ActionType, OddXmlId, Name, Value, SpecialBetValue, BetId, DateCreated, ActionToTake)
                                WHERE AllChanges.ActionType = 'UPDATE'

                            INSERT INTO dbo.OddChangeLogs
                                SELECT 
                                    OddXmlId,
                                    Name,
                                    Value,
                                    SpecialBetValue,
                                    BetId,
                                    ActionToTake,
                                    DateCreated
                                FROM (
                                    MERGE INTO dbo.Odds AS TARGET
                                    USING dbo.#TmpOddTable AS SOURCE
                                    ON TARGET.Id = SOURCE.Id
                                    WHEN NOT MATCHED BY TARGET THEN
                                        INSERT (Id, Name, Value, SpecialBetValue, BetId, IsActive)
                                        VALUES (SOURCE.Id, SOURCE.Name, SOURCE.Value, SOURCE.SpecialBetValue, SOURCE.BetId, SOURCE.IsActive)
                                    WHEN NOT MATCHED BY SOURCE THEN
                                         UPDATE SET TARGET.IsActive = 0
                                    OUTPUT $action as ActionType, INSERTED.Id as OddXmlId, INSERTED.Name, INSERTED.Value, INSERTED.SpecialBetValue, INSERTED.BetId, GETUTCDATE() as DateCreated, 'DELETE' as ActionToTake
                                ) AllChanges (ActionType, OddXmlId, Name, Value, SpecialBetValue, BetId, DateCreated, ActionToTake)
                                WHERE AllChanges.ActionType = 'UPDATE'
                                DROP TABLE #TmpOddTable";

                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
    }
}
