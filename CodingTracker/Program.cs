using System.Configuration;
using Microsoft.Data.Sqlite;

namespace CodingTracker;
class Program
{
    static string connectionString = @"Data source=coding-tracker.db";

    static void Main(string[] args) 
    {
        string test1;
        test1 = ConfigurationManager.AppSettings.Get("connection-string")!;
        Console.WriteLine(test1);

        // using (var connection = new SqliteConnection(connectionString)) {
        //     connection.Open();
        //     var tableCmd = connection.CreateCommand();

        //     tableCmd.CommandText = 
        //     @"CREATE TABLE IF NOT EXISTS";

        //     tableCmd.ExecuteNonQuery();

        //     connection.Close();
        // }
    }
}