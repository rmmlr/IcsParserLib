using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Rca.IcsParser
{
    /// <summary>
    /// Event-Objekt
    /// Ausschließlich lesender Zugriff gestattet
    /// </summary>
    [DebuggerDisplay("{SUMMARY}")]
    public class EventEntry
    {
        //Auszug aus *ics-Datei
        //
        //  BEGIN:VEVENT
        //  DTSTART;VALUE=DATE:20141003
        //  DTEND;VALUE=DATE:20141006
        //  DTSTAMP:20141113T105036Z
        //  UID:trdd77kflr96d8mt8m8kcu4dc0@google.com
        // *ATTENDEE;CUTYPE=INDIVIDUAL;ROLE=REQ-PARTICIPANT;PARTSTAT=ACCEPTED;X-NUM-GUE
        //   STS=0:mailto:ahhulle352l5hbn2uk6ojccrk8@group.calendar.google.com
        // *CLASS:PUBLIC
        //  CREATED:20131210T101131Z
        //  DESCRIPTION:OR8\, ORT\, ORE8B
        //  LAST-MODIFIED:20131215T153155Z
        //  LOCATION:Messe Leipzig
        // *SEQUENCE:1
        //  STATUS:CONFIRMED
        //  SUMMARY:MCL2014 modell-hobby-spiel
        // *TRANSP:TRANSPARENT
        //  END:VEVENT

        /// <summary>
        /// Startdatum und Uhrzeit
        /// </summary>
        public DateTime DTSTART { get; internal set; }

        /// <summary>
        /// Enddatum und Uhrzeit
        /// </summary>
        public DateTime DTEND { get; internal set; }

        /// <summary>
        /// Zeitstempel
        /// </summary>
        public DateTime DTSTAMP { get; internal set; }

        /// <summary>
        /// Erstellungsdatum und Uhrzeit
        /// </summary>
        public DateTime CREATED { get; internal set; }

        /// <summary>
        /// Letztes Änderungsdatum und Uhrzeit
        /// </summary>
        public DateTime LASTMODIFIED { get; internal set; }

        /// <summary>
        /// Event-Beschreibung
        /// </summary>
        public String DESCRIPTION { get; internal set; }

        /// <summary>
        /// Ort
        /// </summary>
        public String LOCATION { get; internal set; }

        /// <summary>
        /// Status
        /// </summary>
        public StatusEnum STATUS { get; internal set; }

        /// <summary>
        /// Event-Titel
        /// </summary>
        public String SUMMARY { get; internal set; }

        /// <summary>
        /// UID
        /// </summary>
        public String UID { get; internal set; }
    }
}
