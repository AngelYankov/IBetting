using IBetting.Services.BettingService.Models;
using IBetting.Services.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace IBetting.Services.Repositories
{
    public class OddRepository : IOddRepository
    {
        private readonly string? connectionString;

        public OddRepository(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Adds, Updates and Deletes Odd objects from Odd database table according to current XML document
        /// Adds new OddChangeLog objects to OddChangeLogs database table when records are updated or deleted in Odd database table
        /// </summary>
        /// <param name="allOdds">All Odd objects from current XML document</param>
        public bool SaveOdds(IEnumerable<OddDTO> allOdds)
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
    }
}
