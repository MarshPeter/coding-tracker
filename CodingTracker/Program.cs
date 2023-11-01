using CodingTracker;
using ConsoleTableExt;
using System.Configuration;

string connectionString = ConfigurationManager.AppSettings.Get("connectionString")!;
string path = ConfigurationManager.AppSettings.Get("path")!;

DatabaseController db = new DatabaseController(path, connectionString);
TableDisplay tableDisplayer = new();
UserInput userConsole = new UserInput(db, tableDisplayer);

userConsole.DisplayOptions();


