using System;
using System.Collections.Generic;
using System.Linq;

namespace Latoken_CSharp_Client_Library.Utils.DateTime
{
    public class DateTimeUtils
    {
        private static readonly System.DateTime Epoch = new System.DateTime(
            1970,
            1,
            1,
            0,
            0,
            0,
            DateTimeKind.Utc
        );

        public static long ConvertToTimestamp(System.DateTime value)
        {
            TimeSpan elapsedTime = value - Epoch;
            
            return (long) elapsedTime.TotalMilliseconds;
        }
        
        public static System.DateTime ConvertToDateTime(long timestamp) => Epoch.Add(TimeSpan.FromMilliseconds(timestamp));
        
        public static System.DateTime RoundToMinutes(System.DateTime dateTime, DateTimeKind kind = DateTimeKind.Utc)
        {
            return new System.DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, kind);
        }
        
        public static IReadOnlyCollection<System.DateTime> GetMinutesBetween(System.DateTime start, System.DateTime end)
        {
            var startMinute = RoundToMinutes(start).AddMinutes(1);
            var endMinute = RoundToMinutes(end);
            if (endMinute < startMinute)
                return Array.Empty<System.DateTime>();
            if (endMinute == startMinute)
                return new []{ startMinute };

            var totalMinutes = (int)(endMinute - startMinute).TotalMinutes + 1;
            return Enumerable.Range(0, totalMinutes).Select(x => startMinute.AddMinutes(x)).ToArray();
        }
    }
}