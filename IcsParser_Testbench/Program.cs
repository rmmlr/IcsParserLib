using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rca.IcsParser;

namespace Rca.IcsParser
{
    class Program
    {
        //const String ICS_PATH = @"https://www.google.com/calendar/ical/ahhulle352l5hbn2uk6ojccrk8%40group.calendar.google.com/public/basic.ics";
        const String ICS_PATH = @"basic.ics";


        static void Main(string[] args)
        {            
            IcsParser myIcsParser = new IcsParser();
            EventEntryList myEventList = myIcsParser.Parse(ICS_PATH);
            Console.WriteLine("Es wurden {0} Event-Einträge gefunden.", myEventList.Count);
            Console.ReadKey();
        }
    }
}
