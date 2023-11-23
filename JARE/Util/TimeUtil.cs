using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JARE.util
{
    public static class TimeUtil
    {
        /// <summary>
        /// Time to string.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <param name="format">y-year, m-month, d-day, H-hour, M-minute, S-second, L-miliceconds.</param>
        /// <returns></returns>
        public static string TimeToString(DateTime t, String format)
        {
            // y-year, m-month, d-day, H-hour, M-minute, S-second, L-miliceconds
            String s = "";
            int i = 0;
            while (i < format.Length && i < 7)
            {
                if (i > 0) s += " ";
                string d = "";
                switch (format[i])
                {
                    case 'y':
                        d = s + t.Year;
                        break;

                    case 'm':
                        d = d + t.Month;
                        break;

                    case 'd':
                        d = d + t.Day;
                        break;

                    case 'H':
                        d = d + t.Hour;
                        break;

                    case 'M':
                        d = d + t.Minute;
                        break;

                    case 'S':
                        d = d + t.Second;
                        break;

                    case 'L':
                        d = d + t.Millisecond;
                        break;
                }
                s += d;
                i++;
            }
            return s;
        }

        public static string TimeToString(DateTime t)
        {
            return TimeToString(t, "HMSLdm");
        }
    }
}
