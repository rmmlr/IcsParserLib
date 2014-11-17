using System;

namespace Rca.IcsParser
{
    class Program
    {
        static Uri WebIcsFile = new Uri(@"https://www.google.com/calendar/ical/ahhulle352l5hbn2uk6ojccrk8%40group.calendar.google.com/public/basic.ics");
        static Uri LocalIcsFile = new Uri(new Uri(AppDomain.CurrentDomain.BaseDirectory), @"basic.ics");

        static void Main(string[] args)
        {
            IcsParser myIcsParser = new IcsParser();
            EventEntryList myEventList = myIcsParser.Parse(WebIcsFile);
            //EventEntryList myEventList = myIcsParser.Parse(LocalIcsFile);
            Console.WriteLine("Es wurden {0} Event-Einträge gefunden.", myEventList.Count);
            Console.ReadKey();
        }
    }
}
