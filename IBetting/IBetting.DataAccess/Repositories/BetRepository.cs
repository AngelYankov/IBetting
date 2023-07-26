using IBetting.DataAccess.Extensions;
using IBetting.DataAccess.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace IBetting.DataAccess.Repositories
{
    public class BetRepository : IBetRepository
    {
        private readonly string? connectionString;

        public BetRepository(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Adds, Updates and Deletes Bet objects from Bet database table according to current XML document
        /// Adds new BetChangeLog objects to BetChangeLogs database table when records are updated or deleted in Bet database table
        /// </summary>
        /// <param name="allBets">All Bet objects from current XML document</param>
        public bool SaveBets(IEnumerable<Bet> allBets)
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
                            [Name] [TEXT],
                            [IsLive] [BIT],
                            [MatchId] [INT],
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

                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error saving Bets: " + ex.ToString());
                        return false;
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
