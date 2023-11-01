using ConsoleTableExt;

namespace CodingTracker;

public class TableDisplay
{
    public TableDisplay(){}

    public void DisplayCodingSessions(List<CodingSession> sessions)
    {
        ConsoleTableBuilder.From(sessions).ExportAndWriteLine();   
    }

}