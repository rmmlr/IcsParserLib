using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Linq;

namespace Rca.IcsParser
{
    /// <summary>
    /// Parser für *ics.-Dateien
    /// </summary>
    public class IcsParser
    {
        /// <summary>
        /// Entspricht 24h in Ticks
        /// </summary>
        const long DAY_TICKS = 864000000000;

        /// <summary>
        /// Parst die angegebene *ics.-Datei und gibt Eventeinträge (VEVENT) zurück
        /// </summary>
        /// <param name="filePath">Pfad zur *isc.-Datei</param>
        /// <returns>Liste mit <seealso cref="EventEntry"/>EventEntry</seealso>-Objekten, sortiert nach Startdatum der Events</returns>
        public List<EventEntry> Parse(String filePath)
        {
            if (!filePath.EndsWith(".ics"))
            {
                throw new FileLoadException("Fehlerhafte Dateiendung, es wird eine *.ics-Datei erwartet");
            }

            String dataStr = null;
            String actLine = null;
            EventEntry actEventEntry = new EventEntry();
            List<EventEntry> eventList = new List<EventEntry>();
            StreamReader file = new StreamReader(filePath);

            while ((actLine = file.ReadLine()) != null) //HINT: Letzte Zeile wird nicht mehr geparst!
            {
                if (dataStr != null)
                {
                    if (actLine.StartsWith(" "))
                    {
                        dataStr += actLine.Substring(1, actLine.Length - 1);
                    }
                    else
                    {
                        try
                        {
                            KeyValuePair<String, String> p = ParseLine(dataStr);
                            String[] mainKey = p.Key.Split(';');

                            switch (mainKey[0])
                            {
                                case "BEGIN":
                                    //TODO: auf VEVENT prüfen
                                    actEventEntry = new EventEntry();
                                    break;
                                case "END":
                                    eventList.Add(actEventEntry);
                                    break;
                                default:
                                    PropertyInfo propertyInfo = actEventEntry.GetType().GetProperty(mainKey[0]);
                                    if (propertyInfo != null)
                                    {
                                        propertyInfo.SetValue(actEventEntry, ConvertToType(p.Value, propertyInfo.PropertyType), null);
                                    }
                                    else
                                    {
                                        Debug.WriteLine(p.Value);
                                    }
                                    break;
                            }
                        }
                        catch (MissingFieldException ex)
                        {
                            Debug.WriteLine(ex);
                        }
                        finally
                        {
                            dataStr = actLine;
                        }
                    }
                }
                else
                {
                    dataStr = actLine;
                }
            }

            file.Close();

            return eventList.OrderBy(x => Convert.ToInt32(x.DTSTART.Ticks / DAY_TICKS)).ToList();
        }

        /// <summary>
        /// Überführt eine eingelesene Zeile in ein KeyValuePair
        /// </summary>
        /// <param name="line">Zeilenstring</param>
        /// <returns>KeyValuePair</returns>
        private KeyValuePair<String, String> ParseLine(String line)
        {
            KeyValuePair<String, String> p;
            Regex exp = new Regex(@"^(?<key>.+?):(?<value>.+?)$");
            Match m = exp.Match(line);

            if (m.Success)
            {
                p = new KeyValuePair<String, String>(m.Groups["key"].Value.Replace("-", ""), m.Groups["value"].Value);
            }
            else
            {
                throw new MissingFieldException("Kein Key oder Value gefunden\nEingelesene Zeile: \"" + line + "\"");
            }

            return p;
        }

        /// <summary>
        /// Überführt einen Datumsstring in DateTime
        /// </summary>
        /// <param name="dateString">Datumsstring ("YYYYMMDD")</param>
        /// <returns>DateTime</returns>
        private DateTime ParseDate(String dateString)
        {
            //TODO: Uhrzeit ergänzen
            if (dateString.Length < 7)
            {
                throw new FormatException("Datumsstring zu kurz");
            }
            return new DateTime(Convert.ToInt32(dateString.Substring(0, 4)),
                Convert.ToInt32(dateString.Substring(4, 2)),
                Convert.ToInt32(dateString.Substring(6, 2)));
        }

        /// <summary>
        /// Überführt den Eventstatus in das StatusEnum
        /// Ausschließlich für Events ("VEVENT")
        /// </summary>
        /// <param name="eventStatus">Eventstatus-String</param>
        /// <returns>StatusEnum</returns>
        private StatusEnum ParseStatus(String eventStatus)
        {
            switch (eventStatus)
            {
                case "CONFIRMED":
                    return StatusEnum.Confirmed;
                case "CANCELLED":
                    return StatusEnum.Cancelled;
                case "TENTATIVE":
                    return StatusEnum.Tentative;
                default:
                    return StatusEnum.Default;
            }
        }

        /// <summary>
        /// Konvertiert einen String (Value-Teil eines Zeilendatensatzes) in den angegebenen Typ
        /// </summary>
        /// <param name="str">Value-String des Zeilendatensatzes</param>
        /// <param name="type">Rückgabetyp</param>
        /// <returns>Konvertierter Wert im angegebenen Typ</returns>
        private object ConvertToType(String str, Type type)
        {
            switch (type.ToString())
            {
                case "System.DateTime":
                    return ParseDate(str);
                case "Rca.IcsParser.StatusEnum":
                    return ParseStatus(str);
                case "System.String":
                    return str.Replace("\\","");
                default:
                    throw new TypeLoadException("Kein Sub-Parser für diesen Typ vorhanden");
            }
        }
    }
}
