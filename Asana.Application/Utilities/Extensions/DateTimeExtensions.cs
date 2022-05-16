using System;

namespace Asana.Application.Utilities.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToTimeDifference(this DateTime dateTime)
        {
            var totlaDays = (int)(DateTime.Now - dateTime).TotalDays;

            if(totlaDays == 0)
            {
                var totalHours = (int)(DateTime.Now - dateTime).TotalHours;
                if(totalHours == 0)
                {
                    var totalMinutes = (int)(DateTime.Now - dateTime).TotalMinutes;
                    return totalMinutes +" دقیقه";
                }
                return totalHours +" ساعت";
            }

            return totlaDays + " روز";
        }
    }
}
