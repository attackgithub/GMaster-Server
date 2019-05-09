using System;

namespace GMaster.Common.Extensions
{
    public static class DateTimeExtension
    {

        public static DateTime ToLocalTime(this DateTime date)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(date, TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
        }
    }
}
