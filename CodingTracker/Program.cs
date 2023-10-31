﻿using CodingTracker;
using System.Configuration;

string connectionString = ConfigurationManager.AppSettings.Get("connectionString")!;
string path = ConfigurationManager.AppSettings.Get("path")!;

DatabaseController db = new DatabaseController(path, connectionString);
UserInput userConsole = new UserInput(db);
userConsole.DisplayOptions();


