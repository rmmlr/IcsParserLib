using System;
using System.Text;
using System.IO;
using System.Net;

namespace Rca.IcsParser
{
    public class Schedule
    {

        /// <summary>
        /// Loads each iCalendar into our iCalendarCollection.
        /// </summary>        
        public void Schedule_Load(object sender, EventArgs e)
        {
            try
            {
                String username = "";
                String password = "";
                String caldavUrl = @"www.";
                String methodName = "OPTIONS";

                ExectueMethod(username, password, caldavUrl, methodName, null, caldavUrl + @" HTTP/1.1", null);
                WebHeaderCollection headers = new WebHeaderCollection();
                headers.Add(@"Translate", "F");
                string content = "<?xml version=\"1.0\" encoding=\"utf-8\"?><propfind xmlns=\"DAV:\"><allprop/></propfind>";
                ExectueMethod(username, password, caldavUrl, "PROPFIND", headers, content, "text/xml");

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static void ExectueMethod(String username, String password, String caldavUrl, String methodName, WebHeaderCollection headers, string content, string contentType)
        {
            //            <?xml version="1.0" encoding="utf-8"?>
            //<propfind xmlns="DAV:">
            //  <allprop/>
            //</propfind>



            // Create an HTTP request for the URL.
            HttpWebRequest httpGetRequest =
               (HttpWebRequest)WebRequest.Create(caldavUrl);

            // Set up new credentials.
            httpGetRequest.Credentials =
               new NetworkCredential(username, password);

            // Pre-authenticate the request.
            httpGetRequest.PreAuthenticate = true;

            // Define the HTTP method.
            httpGetRequest.Method = methodName;

            // Optional, but allows for larger files.
            httpGetRequest.SendChunked = true;


            // Specify the request for source code.
            //httpGetRequest.Headers.Add(@"Translate", "F");
            if (headers != null && headers.HasKeys())
                httpGetRequest.Headers = headers;

            byte[] optionsArray = Encoding.UTF8.GetBytes(content);
            httpGetRequest.ContentLength = optionsArray.Length;
            if (!String.IsNullOrWhiteSpace(contentType))
                httpGetRequest.ContentType = contentType;

            // Retrieve the request stream.
            Stream requestStream =
               httpGetRequest.GetRequestStream();

            // Write the string to the destination as a text file.
            requestStream.Write(optionsArray, 0, optionsArray.Length);

            // Close the request stream.
            requestStream.Close();



            // Retrieve the response.
            HttpWebResponse httpGetResponse =
               (HttpWebResponse)httpGetRequest.GetResponse();

            // Retrieve the response stream.
            Stream responseStream =
               httpGetResponse.GetResponseStream();

            // Retrieve the response length.
            long responseLength =
               httpGetResponse.ContentLength;

            // Create a stream reader for the response.
            StreamReader streamReader =
               new StreamReader(responseStream, Encoding.UTF8);
            StringBuilder sb = new StringBuilder();
            // Write the response status to the console.
            sb.AppendFormat(
               @"GET Response: {0}",
               httpGetResponse.StatusDescription).AppendLine();
            sb.AppendFormat(
               @"  Response Length: {0}",
               responseLength).AppendLine();
            sb.AppendFormat(
               @"  Response Text: {0}",
               streamReader.ReadToEnd()).AppendLine();

            // Close the response streams.
            streamReader.Close();
            responseStream.Close();
        }
    }
}