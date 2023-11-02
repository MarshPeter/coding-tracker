using System.Collections;
using System.Data;
using System.Runtime.CompilerServices;

namespace CodingTracker;

public class UserInput 
{
    public UserInput(DatabaseController db, TableDisplay tableDisplayer)
    {
        DbAccess = db;
        TableDisplayer = tableDisplayer;
    }

    public bool DisplayOptions()
    {
        Console.Clear();
        Console.WriteLine("This is the Coding Tracker!");
        Console.WriteLine("Select any of the options below by entering their corresponding number!");
        Console.WriteLine("----------------------------------------------------------------------------");
        Console.WriteLine("0 - Quit the program");
        Console.WriteLine("1 - View previous Logs");
        Console.WriteLine("2 - Enter a new coding period");
        Console.WriteLine("3 - Update a coding period");
        Console.WriteLine("4 - Remove a coding period");
        Console.WriteLine("----------------------------------------------------------------------------");
        Console.Write("Enter your choice > ");
        string? choice = Console.ReadLine();

        switch (choice)
        {
            case "0":
                ExitMessages();
                return false;
            case "1":
                DisplayLogs();
                break;
            case "2":
                EnterNewPeriod();
                break;
            case "3":
                UpdateCodingPeriod();
                break;
            case "4":
                DeleteCodingPeriod();
                break;
            default:
                Console.WriteLine("that is not a correct command");
                break;

        } 
        return true;
    }

    private void ExitMessages() 
    {
        Console.Clear();
        Console.WriteLine("Thanks for using the program!");
        Console.WriteLine("Press any key to leave");
    }

    private void EnterNewPeriod()
    {
        string? startDateTime = null;
        string? endDateTime = null;
        
        while (startDateTime == null && endDateTime == null)
        {
            Console.Clear();
            Console.WriteLine("You will be adding a new period to the database.");
            Console.WriteLine("Enter a date and time in the following format for when you started: ");
            Console.WriteLine("FORMAT: dd/mm/yy hh:mm");
            startDateTime = Console.ReadLine();
            Console.Clear();
            Console.WriteLine($"You entered {startDateTime} as your start time");
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.WriteLine("Enter a date and time in the following format for when you ended: ");
            Console.WriteLine("FORMAT: dd/mm/yy hh:mm");
            endDateTime = Console.ReadLine();
        }

        bool validResponse = false;
        TimeCalculator timeCalc = new TimeCalculator(startDateTime!, endDateTime!);

        while (!validResponse)
        {
            Console.Clear();
            Console.WriteLine("You entered the following times: ");
            Console.WriteLine(startDateTime);
            Console.WriteLine(endDateTime);
            Console.WriteLine($"The duration was: {timeCalc.Duration} hours");
            Console.WriteLine("Are these details fine with you?");
            Console.WriteLine("Enter Y for yes, and N for no.");
            string? userIsOkWithSubmission = Console.ReadLine();
            if (userIsOkWithSubmission == null)
            {
                userIsOkWithSubmission = "X";
            } 
            switch (userIsOkWithSubmission!.Trim().ToLower())
            {
                case "y":
                    DbAccess.AddDataToTable(startDateTime!, endDateTime!, timeCalc.Duration);
                    Console.WriteLine("Your coding period has been added");
                    validResponse = true;
                    break;
                case "n":
                    Console.WriteLine("Your coding period was not added");
                    validResponse = true;
                    break;
                default:
                    Console.WriteLine("That was not a valid response.");
                    Console.WriteLine("Press any key and try again");
                    break;
            }
        }
    }

    private void DisplayLogs()
    {
        Console.Clear();
        Console.WriteLine("Here are your logs");
        List<CodingSession> sessions = DbAccess.RetrieveCodingSessions();
        TableDisplayer.DisplayCodingSessions(sessions);
        Console.WriteLine();
        Console.WriteLine("Press any button to continue");
        Console.ReadKey();
    }

    private void UpdateCodingPeriod()
    {
        int n = -1;
        bool validNumber = false;

        while (!validNumber)
        {
            Console.Clear();
            Console.WriteLine("Select a log to edit by entering a number for the ID you would like to edit");
            string res = Console.ReadLine()!;

            validNumber = Int32.TryParse(res, out n);
            if (!validNumber)
            {
                Console.WriteLine("That was an invalid input, you must input an integer");
                Console.WriteLine("Press any key to try again");
                Console.ReadLine();
            }
        }

        CodingSession session = DbAccess.RetrieveSingleSession(n);
        if (session.Id == -1)
        {
            Console.WriteLine("That is not a identifiable session id");
            Console.WriteLine("Press any key to return to the menu");
            Console.ReadLine();
            return;
        }

        bool validResponse = false;
        while (!validResponse)
        {
            Console.Clear();
            Console.WriteLine("The current log is as follows");
            TableDisplayer.DisplayCodingSessions(new List<CodingSession>(){session});
            Console.WriteLine("Enter what you would like to change");
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.WriteLine("1 - change the start datetime");
            Console.WriteLine("2 - change the end datetime");
            Console.WriteLine("3 - Update both the start and end datetime");
            Console.WriteLine("4 - cancel change");
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.Write("Enter your choice > ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    session.StartTime = UpdateTime("start", session.StartTime!);
                    validResponse = true;
                    break;
                case "2":
                    session.EndTime = UpdateTime("end", session.EndTime!);
                    validResponse = true;
                    break;
                case "3":
                    session.StartTime = UpdateTime("start", session.StartTime!);
                    session.EndTime = UpdateTime("end", session.EndTime!);
                    validResponse = true; 
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("that was not a valid response, try again");
                    break;
            } 
        }

        TimeCalculator calc = new TimeCalculator(session.StartTime!, session.EndTime!);
        session.Duration = calc.Duration;
        validResponse = false;

        while(!validResponse)
        {
            Console.Clear();
            Console.WriteLine("Your new session would now be:");
            TableDisplayer.DisplayCodingSessions(new List<CodingSession>(){session});
            Console.WriteLine("Are you sure this is what you want?");
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.WriteLine("1 - confirm change");
            Console.WriteLine("2 - abort change");
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.Write("Enter your choice > ");
            string choice = Console.ReadLine()!;

            switch (choice)
            {
                case "1":
                    DbAccess.UpdateRowInTable(session.Id, session.StartTime!, session.EndTime!, (float) session.Duration);
                    validResponse = true;
                    break;
                case "2":
                    return;
                default:
                    Console.WriteLine("That command was not recognized, press any key to try again!");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private string UpdateTime(string time, string datetime)
    {
        Console.Clear();
        Console.WriteLine($"You are going to update the {time} time.");
        Console.WriteLine($"The current {time} time is: {datetime}");
        Console.WriteLine($"Enter your new {time} time");
        return Console.ReadLine()!;
    }

    private void DeleteCodingPeriod()
    {
        int n = -1;
        bool validNumber = false;

        while (!validNumber)
        {
            Console.Clear();
            Console.WriteLine("Select a log to delete by entering a number for the ID you would like to remove");
            Console.WriteLine("Alternatively, you can type in 'n' to return to the main menu");
            string res = Console.ReadLine()!;

            if (res.Trim().ToLower() == "n")
            {
                return;
            }

            validNumber = Int32.TryParse(res, out n);
            if (!validNumber)
            {
                Console.WriteLine("That was an invalid input, you must input an integer");
                Console.WriteLine("Press any key to try again");
                Console.ReadLine();
            }
        }

        CodingSession session = DbAccess.RetrieveSingleSession(n);
        if (session.Id == -1)
        {
            Console.WriteLine("That is not a identifiable session id");
            Console.WriteLine("Press any key to return to the menu");
            Console.ReadLine();
            return;
        }

        bool validResponse = false;

        while (!validResponse)
        {
            Console.Clear();
            Console.WriteLine("This is your coding session you are about to delete");
            TableDisplayer.DisplayCodingSessions(new List<CodingSession>(){session});
            Console.WriteLine("Are you sure this is what you want?");
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.WriteLine("1 - confirm change");
            Console.WriteLine("2 - abort change");
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.Write("Enter your choice > ");
            string response = Console.ReadLine()!;

            switch (response)
            {
                case "1":
                    DbAccess.DeleteRowInTable(n);
                    validResponse = true;
                    break;
                case "2":
                    return;
                default:
                    Console.WriteLine("That is not a valid response.");
                    Console.WriteLine("Press any key to try again");
                    Console.ReadKey();
                    break;
            }
        }

        Console.WriteLine("The session has been deleted");
        Console.WriteLine("Press any key to continue");
        Console.ReadLine();
    }

    private TableDisplay TableDisplayer {get; set;}
    private DatabaseController DbAccess {get; set;}
}