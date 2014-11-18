using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Rca.IcsParser
{
    /// <summary>
    /// Liste mit Event-Einträgen
    /// </summary>
    [DebuggerDisplay("Count = {m_InnerList != null ? m_InnerList.Count : 0}")]
    [Serializable]
    public class EventEntryList
    {
        #region Klassenvariablen

        /// <summary>
        /// Innere Liste der Event-Einträge
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private List<EventEntry> m_InnerList = new List<EventEntry>();

        /// <summary>
        /// Aktuelles Datum ohne Uhrzeit
        /// </summary>
        private readonly DateTime m_Today = new DateTime(DateTime.Now.Date.Ticks);
        
        #endregion Klassenvariablen

        #region Poperties

        /// <summary>
        /// Zugriff auf die Event-Einträge
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Event-Eintrag</returns>
        public EventEntry this[int index]
        {
            get
            {
                return m_InnerList[index];
            }
        }

        /// <summary>
        /// Kalendername
        /// </summary>
        public String XWRCALNAME { get; set; }

        /// <summary>
        /// Kalender-Zeitzone
        /// </summary>
        public String XWRTIMEZONE { get; set; }

        /// <summary>
        /// Kalenderbeschreibung
        /// </summary>
        public String XWRCALDESC { get; set; }

        #endregion Poperties
        
        #region Dienstmethoden
        /// <summary>
        /// Gibt die Anzahl der Event-Einträge zurück
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Int32 Count
        {
            get
            {
                if (m_InnerList == null)
                    return 0;
                else
                    return m_InnerList.Count;
            }
        }

        /// <summary>
        /// Hängt ein neuen Event-Eintrag an
        /// </summary>
        /// <param name="entry">Event-Eintrag</param>
        public void Add(EventEntry entry)
        {
            if (m_InnerList == null)
                m_InnerList = new List<EventEntry>();

            m_InnerList.Add(entry);
        }

        /// <summary>
        /// Sortiert die Event-Einträge nach ihren Startdatum (Startuhrzeiten werden vernachlässigt)
        /// </summary>
        public void Sort()
        {
            if (m_InnerList != null)
                m_InnerList = m_InnerList.OrderBy(e => Convert.ToInt32(e.DTSTART.Ticks / new TimeSpan(24, 0, 0).Ticks)).ToList();
        }

        /// <summary>
        /// Löscht alle Einträge
        /// </summary>
        public void Clear()
        {
            if (m_InnerList != null)
            {
                m_InnerList.Clear();
                m_InnerList = null;
            }
        }

        /// <summary>
        /// Gibt alle kommenden Events zurück
        /// </summary>
        /// <returns>Kommende Events als Array, leeres Array wenn keine Einträge gefunden</returns>
        public EventEntry[] GetNextEvents()
        {
            if (m_InnerList != null)
                return m_InnerList.FindAll(e => (e.DTSTART >= m_Today)).ToArray();
            else
                return new EventEntry[0];
        }

        /// <summary>
        /// Gibt alle vorherigen Events zurück
        /// </summary>
        /// <returns>Vorherigen Events als Array in umgekehrter Reihenfolge, leeres Array wenn keine Einträge gefunden</returns>
        public EventEntry[] GetRecentEvents()
        {
            if (m_InnerList != null)
            {
                EventEntry[] result = m_InnerList.FindAll(e => (e.DTSTART <= m_Today)).ToArray();

                return result.Reverse().ToArray();
            }
            else
            {
                return new EventEntry[0];
            }
        }

        /// <summary>
        /// Gibt alle aktuell laufenden Events zurück
        /// </summary>
        /// <returns>Laufende Events</returns>
        public EventEntry[] GetRunningEvents()
        {
            if (m_InnerList != null)
            {
                List<EventEntry> result = m_InnerList.FindAll(e => (e.DTSTART >= m_Today));
                return result.FindAll(e => (e.DTEND <= m_Today)).ToArray();
            }
            else
            {
                return new EventEntry[0];
            }
        }

        #endregion Dienstmethoden
    }
}
