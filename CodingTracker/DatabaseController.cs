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

    public bool AddDataToTable(string startTime, string endTime, float duration)
    {
        string query = 
            @$"INSERT INTO coding_periods (Start_Time, End_Time, Duration)
            VALUES ('{startTime}', '{endTime}', {duration})";

        MakeQuery(query);

        return true;
    }

    public bool UpdateRowInTable(int id, string startTime, string endTime, float duration)
    {
        string query =
            @$"UPDATE coding_periods
                SET Start_time = '{startTime}', End_Time = '{endTime}', Duration = {duration}
                WHERE Id = {id}";

        MakeQuery(query);

        return true;
    }

    public bool DeleteRowInTable(int id)
    {
        string query =
            @$"DELETE FROM coding_periods WHERE Id = {id}";

        MakeQuery(query);

        return true;
    }

    private void ConfirmTableExists() 
    {
        string query = 
            @"CREATE TABLE IF NOT EXISTS coding_periods (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Start_Time TEXT NOT NULL,
                End_Time TEXT NOT NULL,
                Duration FLOAT NOT NULL 
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