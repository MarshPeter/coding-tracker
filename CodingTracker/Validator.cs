namespace CodingTracker;
using System.Text.RegularExpressions;

public static class Validator
{
    public static bool validateDateTime(string dateTime)
    {
        Regex rx = new Regex(@"^[0-9]{2}/[0-9]{2}/[0-9]{2} [0-9]{2}:[0-9]{2}$");

        return rx.IsMatch(dateTime);
    }
}