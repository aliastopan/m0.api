namespace m0.api.Extensions;

public static class DateTimeExtensions
{
    public static bool IsToday(this DateTime dateTime)
    {
        return dateTime == DateTime.Today;
    }

    public static bool AtDate(this DateTime dateTime, DateOnly date)
    {
        return dateTime.Day == date.Day
            && dateTime.Month == date.Month
            && dateTime.Year == date.Year;
    }
}
