using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace YunoBlog.Helpers
{
    public static class StringHelper
    {
        public static int IndexOf(string src, string txt, int count)
        {
            var pos = -1;
            for (int i = 0; i < count; i++)
            {
                pos = src.IndexOf(txt, pos + 1);
            }
            if (pos == -1) return src.LastIndexOf(txt);
            return pos;
        }
        public static string RandomString(int Length)
        {
            var rand = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Length; i++)
            {
                int ch = rand.Next(26 + 26 + 10);
                if (ch < 26) sb.Append((char)(ch + 'A'));
                else if (ch < 26 + 26) sb.Append((char)(ch - 26 + 'a'));
                else sb.Append((char)(ch - 26 - 26 + '0'));
            }
            return sb.ToString();
        }

        public static string ToCreationTime(DateTime time)
        {
            return time.ToString("yyyy年MM月dd日 HH:mm");
        }

        public static DateTime ToDateTime(string TimeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(TimeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime); return dtStart.Add(toNow);
        }

        public static DateTime ToDateTime(int TimeStamp)
        {
            return ToDateTime(TimeStamp.ToString());
        }

        public static int ToTimeStamp(System.DateTime Time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(Time - startTime).TotalSeconds;
        }

        public static int RandomInt(int Begin, int End)
        {
            Random rand = new Random();
            var ret = rand.Next(End - Begin + 1);
            return ret + Begin;
        }
    }
}