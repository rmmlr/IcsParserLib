using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Net;

namespace Rca.IcsParser
{
    /// <summary>
    /// Parser für *ics.-Dateien
    /// </summary>
    public class IcsParser
    {
        #region Dienstmethoden
        /// <summary>
        /// Parst die angegebene *ics.-Datei und gibt Eventeinträge (VEVENT) zurück
        /// </summary>
        /// <param name="icsUri">Uri (Pfad) zur *isc.-Datei</param>
        /// <returns>Liste mit <seealso cref="EventEntry"/>-Objekten, sortiert nach Startdatum der Events</returns>
        public EventEntryList ParseEvents(Uri icsUri)
        {
            if (!icsUri.AbsoluteUri.EndsWith(".ics") && !icsUri.AbsoluteUri.EndsWith(".ICS"))
            {
                throw new FileLoadException("Fehlerhafte Dateiendung, es wird eine *.ics-Datei erwartet");
            }

            Boolean isIcsHead = true;
            String dataStr = null;
            String actLine = null;
            EventEntry actEventEntry = new EventEntry();
            EventEntryList eventList = new EventEntryList();
            StreamReader streamReader;

            if (icsUri.IsFile)
            {
                streamReader = new StreamReader(icsUri.LocalPath);
            }
            else
            {
                try
                {
                    WebResponse webResponse = WebRequest.Create(icsUri).GetResponse();
                    streamReader = new StreamReader(webResponse.GetResponseStream(), System.Text.Encoding.Default);
                }
                catch (WebException)
                {
                    //TODO: Verbindungsfehler abfangen.
                    throw;
                }
            }

            while ((actLine = streamReader.ReadLine()) != null) //HINT: Letzte Zeile wird nicht mehr geparst!
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
                            String[] pKeys = p.Key.Split(';');

                            if (isIcsHead && !(pKeys[0] == "BEGIN" && p.Value == "VEVENT"))
                            {
                                PropertyInfo propertyInfo = eventList.GetType().GetProperty(pKeys[0]);
                                if (propertyInfo != null)
                                {
                                    propertyInfo.SetValue(eventList, ConvertToType(p.Value,
                                        propertyInfo.PropertyType), null);
                                }
                                else
                                {
                                    Debug.WriteLine(p.Value);
                                }
                            }
                            else
                            {
                                isIcsHead = false;

                                switch (pKeys[0])
                                {
                                    case "BEGIN":
                                        if (p.Value == "VEVENT")
                                            actEventEntry = new EventEntry();
                                        break;
                                    case "END":
                                        if (p.Value == "VEVENT")
                                            eventList.Add(actEventEntry);
                                        break;
                                    default:
                                        PropertyInfo propertyInfo = actEventEntry.GetType().GetProperty(pKeys[0]);
                                        if (propertyInfo != null)
                                        {
                                            propertyInfo.SetValue(actEventEntry, ConvertToType(p.Value,
                                                propertyInfo.PropertyType), null);
                                        }
                                        else
                                        {
                                            Debug.WriteLine(p.Value);
                                        }
                                        break;
                                }
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

            streamReader.Close();
            eventList.Sort();

            return eventList;
        }

        #endregion Dienstmethoden

        #region interne Dienstmethoden
        /// <summary>
        /// Überführt eine eingelesene Zeile in ein KeyValuePair
        /// </summary>
        /// <param name="line">Zeilenstring</param>
        /// <returns>KeyValuePair</returns>
        private KeyValuePair<String, String> ParseLine(String line)
        {
            KeyValuePair<String, String> result;
            Regex exp = new Regex(@"^(?<key>.+?):(?<value>.+?)$");
            Match m = exp.Match(line);

            if (m.Success)
            {
                result = new KeyValuePair<String, String>(m.Groups["key"].Value.Replace("-", ""),
                    m.Groups["value"].Value);
            }
            else
            {
                throw new MissingFieldException("Kein Key oder Value gefunden\nEingelesene Zeile: \"" +
                    line + "\"");
            }

            return result;
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
        /// <param name="valueString">Value-String des Zeilendatensatzes</param>
        /// <param name="type">Rückgabetyp</param>
        /// <returns>Konvertierter Wert im angegebenen Typ</returns>
        private object ConvertToType(String valueString, Type type)
        {
            switch (type.ToString())
            {
                case "System.DateTime":
                    return ParseDate(valueString);
                case "Rca.IcsParser.StatusEnum":
                    return ParseStatus(valueString);
                case "System.String":
                    return valueString.Replace("\\","");
                default:
                    throw new TypeLoadException("Kein Sub-Parser für diesen Typ vorhanden");
            }
        }

        #endregion interne Dienstmethoden
    }
}
