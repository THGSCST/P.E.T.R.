using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PETR_Robot
{
    public static class MarketHours
    {
        public static DateTime GetBraTime()
        {
            TimeZoneInfo brazilTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, brazilTimeZone);
        }

        public static DateTime GetUsaTime()
        {
            TimeZoneInfo usaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, usaTimeZone);
        }

        public static bool IsB3TradingHours(bool addDelay = true)
        {
            DateTime brazilTime = GetBraTime();
            DateTime usaTime = GetUsaTime();
            DateTime usaClosingTime = new DateTime(usaTime.Year, usaTime.Month, usaTime.Day, 16, 0, 0);

            // Adjusts B3's closing time to match NYSE's
            DateTime b3ClosingTimeAdjusted = TimeZoneInfo.ConvertTime(usaClosingTime, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"), TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

            TimeSpan delay = addDelay ? TimeSpan.FromMinutes(15) : TimeSpan.Zero;

            return brazilTime.DayOfWeek >= DayOfWeek.Monday && brazilTime.DayOfWeek <= DayOfWeek.Friday &&
                   brazilTime.TimeOfDay >= new TimeSpan(10, 0, 0).Add(delay) && brazilTime.TimeOfDay <= b3ClosingTimeAdjusted.TimeOfDay.Add(delay);
        }

        public static bool IsNYSETradingHours(bool addDelay = true)
        {
            DateTime usaTime = GetUsaTime();
            TimeSpan delay = addDelay ? TimeSpan.FromMinutes(15) : TimeSpan.Zero;

            return usaTime.DayOfWeek >= DayOfWeek.Monday && usaTime.DayOfWeek <= DayOfWeek.Friday &&
                   usaTime.TimeOfDay >= new TimeSpan(9, 30, 0).Add(delay) && usaTime.TimeOfDay <= new TimeSpan(16, 0, 0).Add(delay);
        }
    }
}
