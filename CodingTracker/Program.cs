using CodingTracker;
using System.Configuration;

string connectionString = ConfigurationManager.AppSettings.Get("connectionString")!;
string path = ConfigurationManager.AppSettings.Get("path")!;

DatabaseController db = new DatabaseController(path, connectionString);
db.AddDataToTable("4:00", "5:00", 60);
