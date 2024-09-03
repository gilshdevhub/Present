namespace Core.Extensions;

public static class DateTimeExtension
{
    public static string ToIsraelString(this DateTime dateTime)
    {
        return $"{dateTime:HH:mm} {dateTime:dd/MM/yyyy}";
    }
}
