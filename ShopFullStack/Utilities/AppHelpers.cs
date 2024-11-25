using System.Security.Cryptography;

namespace ShopApi.Utilities;

public class AppHelpers
{
    public static int GenerateRandomNumber()
    {
        var currentYear = DateTime.Now.Year;
        var nextNumber = RandomNumberGenerator.GetInt32(currentYear, int.MaxValue);
        return currentYear + nextNumber;
    }

    public static DateTime GetLastWeekMonday()
    {
        var today = DateTime.UtcNow;
        var daysSinceMonday = (int)today.DayOfWeek - (int)DayOfWeek.Monday;

        // If today is Sunday, subtract an extra 7 days for the previous week
        if (daysSinceMonday < 0) daysSinceMonday += 7;

        // Get the Monday of last week
        var lastWeekMonday = today.AddDays(-7 - daysSinceMonday);
        Console.WriteLine("**************************************");
        Console.WriteLine("Last week's Monday: " + lastWeekMonday.ToString("yyyy-MM-dd"));
        Console.WriteLine("**************************************");
        return lastWeekMonday;
    }
}