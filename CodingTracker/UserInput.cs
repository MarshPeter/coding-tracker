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

    private TableDisplay TableDisplayer {get; set;}

    private DatabaseController DbAccess {get; set;}
}