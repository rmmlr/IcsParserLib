using System;

namespace Rca.IcsParser
{
    /// <summary>
    /// Testbench für dei IcsParser-DLL
    /// </summary>
    class Program
    {
        static Uri WebIcsFile = new Uri(@"https://www.google.com/calendar/ical/ahhulle352l5hbn2uk6ojccrk8%40group.calendar.google.com/public/basic.ics");
        static Uri LocalIcsFile = new Uri(new Uri(AppDomain.CurrentDomain.BaseDirectory), @"basic.ics");
        static Uri CloudIcs = new Uri(@"https://p09-calendars.icloud.com/published/2/X_gf8J1nymX3sWWrN2s8F5dyt3-VVaWvpRT-OOZq8JpP8tz5CyYX8JfwjrxfsksyV4L4nBvNRMmeqbj_WSGZwSav26nYSmcXChc7zZAP7UQ");

        static void Main(string[] args)
        {
            IcsParser myIcsParser = new IcsParser();
            EventEntryList myEventList = new EventEntryList();

            Boolean successSelected = false;
            while (!successSelected)
            {
                Console.Write("\nAuswahl der ICS-Quelle. (W: Web, L: Lokal, C: iCloud) ");

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.L:
                        myEventList = myIcsParser.ParseEvents(LocalIcsFile);
                        successSelected = true;
                        break;
                    case ConsoleKey.W:
                        myEventList = myIcsParser.ParseEvents(WebIcsFile);
                        successSelected = true;
                        break;
                    case ConsoleKey.C:
                        PlayGround myPlayGround = new PlayGround();
                        myPlayGround.Parse(CloudIcs);
                        successSelected = true;
                        break;
                    default:
                        Console.Write("\nFehlerhafte Eingabe!");
                        break;
                }
            }

            Console.WriteLine("");
            Console.WriteLine("Es wurden {0} Event-Einträge gefunden.", myEventList.Count);
            EventEntry[] myEventEntryAry = myEventList.GetRecentEvents();
            Console.WriteLine("Davon {0} Event-Einträge in der Vergangenheit...", myEventEntryAry.Length);
            myEventEntryAry = myEventList.GetNextEvents();
            Console.WriteLine("...und {0} Event-Einträge in für die Zukunft.", myEventEntryAry.Length);
            myEventEntryAry = myEventList.GetRunningEvents();
            Console.WriteLine("{0} Event-Einträge fallen auf den heutigen Tag.", myEventEntryAry.Length);
            Console.ReadKey();
        }
    }
}


