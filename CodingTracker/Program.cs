using CodingTracker;
using System.Configuration;

string connectionString = ConfigurationManager.AppSettings.Get("connectionString")!;
string path = ConfigurationManager.AppSettings.Get("path")!;

DatabaseController db = new DatabaseController(path, connectionString);
db.UpdateRowInTable(1, "3:00", "5:00", 120);
