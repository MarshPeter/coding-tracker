using Microsoft.Data.Sqlite;

namespace CodingTracker;

class DatabaseController
{
    internal DatabaseController(string path, string connectionString)
    {
        ConnectionString = connectionString;
        Path = path;
        ConfirmTableExists();       
    }

    private void ConfirmTableExists() 
    {
        string query = 
            @"CREATE TABLE IF NOT EXISTS coding_periods (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Start_Time TEXT NOT NULL,
                End_Time TEXT NOT NULL,
                Duration FLOAT
            )";

        MakeQuery(query);
    }

    private void MakeQuery(string query)
    {
        using (SqliteConnection connection = new(this.ConnectionString))
        {
            connection.Open();
            SqliteCommand tableCmd = connection.CreateCommand();

            tableCmd.CommandText = query;
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    private string Path {get; set;}
    private string ConnectionString {get; set;}
}