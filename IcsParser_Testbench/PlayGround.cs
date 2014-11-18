using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Rca.IcsParser
{
    public class PlayGround
    {
        public void Parse(Uri icsUri)
        {
            String actLine = null;
            EventEntry actEventEntry = new EventEntry();
            EventEntryList eventList = new EventEntryList();
            StreamReader streamReader;

            Schedule mySandbox = new Schedule();
            mySandbox.Schedule_Load(null, null);

            try
            {
                //WebClient Client = new WebClient();
                //Stream strm = Client.OpenRead(icsUri);

                HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(icsUri);

                //WebResponse webResponse = WebRequest.Create(icsUri).GetResponse();
                streamReader = new StreamReader(objRequest.GetRequestStream());
            }
            catch (WebException)
            {
                //TODO: Verbindungsfehler abfangen.
                throw;
            }
            

            actLine = streamReader.ReadLine();
        }
    }
}
