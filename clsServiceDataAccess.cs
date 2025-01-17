using System;
using System.Data;
using System.Data.SqlClient;

namespace MyDatabaseBackupService
{
    internal class clsServiceDataAccess
    {
        public static async void BackupDatabase()
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(clsGlobal.ConnectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(clsGlobal.procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Execute the stored procedure
                        await command.ExecuteNonQueryAsync();
                    }
                }

                // Log success
                clsLog.LogServiceEvent("Backup completed successfully at ");
            }
            catch (SqlException sqlex)
            {
                clsLog.LogServiceEvent("error, SQL exception : " + sqlex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                clsLog.LogServiceEvent("Error during backup: " + ex.Message);
            }
        }

    }
}
