using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;

namespace CodingTracker;

// all class functionality assumes "dd/mm/yy hh:mm:ss" formatted date
// assumes all dates start and end within 24 hours
// duration is in hours
public class TimeCalculator
{
    private float _duration;
    public TimeCalculator(string startTime, string endTime)
    {
        _duration = CalculateDuration(startTime, endTime);
    }

    // calculation is done with units of # hrs.
    private float CalculateDuration(string startDateTime, string endDateTime)
    {
        string startTime = startDateTime.Split(" ")[1];
        string endTime = endDateTime.Split(" ")[1];
        float accumulatedTime = 0;

        // hours calculation
        int startHours = Int32.Parse(startTime.Split(":")[0]);
        int endHours = Int32.Parse(endTime.Split(":")[0]);

        if (startHours == 0)
        {
            startHours = 24;
        }

        if (endHours == 0)
        {
            endHours = 24;
        }

        // assumes 24 hours of time passes at a maximum 
        if (endHours > startHours)
        {
            accumulatedTime += endHours - startHours;
        } 
        else
        {
            accumulatedTime += 24 - startHours + endHours;
        }

        // minutes calculation
        int startMinutes = Int32.Parse(startTime.Split(":")[1]);
        int endMinutes = Int32.Parse(endTime.Split(":")[1]);

        if (endMinutes > startMinutes)
        {
            // this is to convert from minutes to an actual decimal with respect to hours
            accumulatedTime += (endMinutes - startMinutes) / ((float)60);
        }
        else if (endMinutes == startMinutes)
        {
            accumulatedTime += 0;
        }
        else
        {
            accumulatedTime -= 1 - ((59 - startMinutes + endMinutes) / ((float)60));
        }
        
        return accumulatedTime;
    }

    public float Duration
    {
        get {return _duration;}
    }
}