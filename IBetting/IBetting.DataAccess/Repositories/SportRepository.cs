﻿using IBetting.DataAccess.Extensions;
using IBetting.DataAccess.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace IBetting.DataAccess.Repositories
{
    public class SportRepository : ISportRepository
    {
        private readonly string? connectionString;

        public SportRepository(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Adds, Updates and Deletes Sport objects from Sport database table according to current XML document
        /// </summary>
        /// <param name="allSports">All Sport objects from current XML document</param>
        public bool SaveSports(IEnumerable<Sport> allSports)
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

                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error saving Sports: " + ex.ToString());
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
