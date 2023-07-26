using IBetting.Services.BettingService.Models;
using IBetting.Services.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace IBetting.Services.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly string? connectionString;

        public EventRepository(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Adds, Updates and Deletes Event objects from Event database table according to current XML document
        /// </summary>
        /// <param name="allEvents">All Event objects from current XML document</param>
        public bool SaveEvents(IEnumerable<EventDTO> allEvents)
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

                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error saving Events: " + ex.ToString());
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
