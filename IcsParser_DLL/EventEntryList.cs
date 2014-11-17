using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

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

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private List<EventEntry> m_InnerList = new List<EventEntry>();
        
        #endregion Klassenvariablen

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

        /// <summary>
        /// Gibt die Anzahl der Event-Einträge zurück
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Int32 Count
        {
            get
            {
                return m_InnerList.Count;
            }
        }

        /// <summary>
        /// Hängt ein neuen Event-Eintrag an
        /// </summary>
        /// <param name="entry">Event-Eintrag</param>
        public void Add(EventEntry entry)
        {
            m_InnerList.Add(entry);
        }

        /// <summary>
        /// Sortiert die Event-Einträge nach ihren Startdatum (Startuhrzeiten werden vernachlässigt)
        /// </summary>
        public void Sort()
        {
            m_InnerList = m_InnerList.OrderBy(x => Convert.ToInt32(x.DTSTART.Ticks / new TimeSpan(24, 0, 0).Ticks)).ToList();
        }

        /// <summary>
        /// Löscht alle Einträge
        /// </summary>
        public void Clear()
        {
            m_InnerList.Clear();
        }

        /// <summary>
        /// Gibt das kommende Event zurück, sollten mehrere Events am selben Tag anfangen, werden alle zurückgegeben
        /// </summary>
        /// <returns>Kommende Event/-s</returns>
        public EventEntry GetNextEvents()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gibt das letzte Event zurück, sollten mehrere Events am selben Tag angefangen haben, werden alle zurückgegeben
        /// </summary>
        /// <returns>Letzte Event/-s</returns>
        public EventEntry GetRecentEvents()
        {
            throw new NotImplementedException();
        }
    }
}
