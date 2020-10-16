using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Windows.Forms;
using System.Xml;

using System.Web.UI.WebControls;
using System.Data.SQLite;
using System.Linq;
using System.Security;

namespace ZenseMe.Lib.Managers
{
    public class Audioscrobbler
    {


        /* Begin major changes by mope-life */



        private string token = "";
        private string sessionKey = ConfigurationManager.AppSettings["LastFM_SessionKey"];

        public bool SubmitTrack(string artist, string track, string album, int duration, DateTime dateSubmitted)
        {
            try
            {
                DateTimeFormatInfo DateTimeInfo = new DateTimeFormatInfo();
                DateTimeInfo.ShortDatePattern = @"dd/MM/yyyy HH:mm:ss";

                TimeSpan MinTimeZone = DateTime.Now - DateTime.UtcNow;
                long TrackUnixTime = dateSubmitted.Ticks - MinTimeZone.Ticks - DateTime.Parse("01/01/1970 00:00:00", DateTimeInfo).Ticks;
                TrackUnixTime /= 10000000;

                Dictionary<string, string> postData = new Dictionary<string, string>
                {
                    { "method", "track.scrobble" },
                    { "artist", artist },
                    { "track", track },
                    { "timestamp", TrackUnixTime.ToString() },
                    { "album", album },
                    { "duration", duration.ToString() },
                    { "api_key", Resources.Keys.API_KEY },
                    { "sk", sessionKey }
                };

                HttpWebRequest httpWebRequest = BuildHttpRequest(postData);
                HttpWebResponse httpWebResponse = CheckResponse(httpWebRequest);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Submit error: " + ex);
                MessageBox.Show("Failed to submit track to the last.fm server.", "ZenseMe");
                return false;
            }
        }



        public bool GetToken()
        {
            Dictionary<string, string> postData = new Dictionary<string, string>
            {
                {"method", "auth.getToken" },
                {"api_key", Resources.Keys.API_KEY }
            };

            HttpWebRequest httpWebRequest = BuildHttpRequest(postData);
            HttpWebResponse httpWebResponse = CheckResponse(httpWebRequest);

            if (httpWebResponse != null)
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(httpWebResponse.GetResponseStream());

                try
                {
                    token = xmlDocument.GetElementsByTagName("token")[0].InnerText;
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("getToken error: " + ex);
                    MessageBox.Show("Unexpected xml response while retrieving token.", "ZenseMe");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public string GetAuthUrl()
        {
            string userSignInData = string.Format("api_key={0}&token={1}", Resources.Keys.API_KEY, token);
            return string.Format("http://www.last.fm/api/auth/?{0}", userSignInData); ;
        }

        public bool GetSession()
        {
            Dictionary<string, string> postData = new Dictionary<string, string>
            {
                {"method", "auth.getSession" },
                {"api_key", Resources.Keys.API_KEY },
                {"token", token }
            };

            HttpWebRequest httpWebRequest = BuildHttpRequest(postData);
            HttpWebResponse httpWebResponse = CheckResponse(httpWebRequest);

            if (httpWebResponse != null)
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(httpWebResponse.GetResponseStream());
                Configuration Configuration =
                    ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                try
                {
                    sessionKey = xmlDocument.GetElementsByTagName("key")[0].InnerText;
                    string userName = xmlDocument.GetElementsByTagName("name")[0].InnerText;

                    Configuration.AppSettings.Settings["LastFM_SessionKey"].Value = sessionKey;
                    Configuration.AppSettings.Settings["LastFM_Username"].Value = userName;
                    Configuration.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("getSession error: " + ex);
                    MessageBox.Show("Unexpected xml response while retrieving session key.", "ZenseMe");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private HttpWebRequest BuildHttpRequest(Dictionary<string, string> dataDict, bool signed = true)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://ws.audioscrobbler.com/2.0/");

            byte[] Buffer = new UTF8Encoding().GetBytes(
                ProcessPostData(dataDict, signed)
                );

            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.UserAgent = string.Format("ZenseMe/{0} ( mope-life )", Application.ProductVersion);
            httpWebRequest.Timeout = 5000;
            httpWebRequest.ContentLength = Buffer.Length;

            Stream Stream = httpWebRequest.GetRequestStream();
            Stream.Write(Buffer, 0, Buffer.Length);
            Stream.Close();

            return httpWebRequest;
        }

        private string ProcessPostData(Dictionary<string, string> dataDict, bool signed)
        {
            // Signing process described in Section 8, here: https://www.last.fm/api/authspec#_8-signing-calls
            // Not all methods need be signed (i.e. user.getInfo), which is why this is conditional
            if (signed)
            {
                List<string> alphaKeys = dataDict.Keys.ToList();
                alphaKeys.Sort();

                StringBuilder sigBuilder = new StringBuilder();
                foreach (string k in alphaKeys)
                {
                    sigBuilder.Append(k);
                    sigBuilder.Append(dataDict[k]);
                }
                sigBuilder.Append(Resources.Keys.API_SECRET);
                dataDict["api_sig"] = CalculateMD5(sigBuilder.ToString());
            }

            StringBuilder postDataBuilder = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in dataDict)
            {
                postDataBuilder.Append(
                    string.Format("{0}={1}&", kvp.Key, HttpUtility.UrlEncode(kvp.Value))
                    );
            }

            return postDataBuilder.ToString();
        }

        private HttpWebResponse CheckResponse(HttpWebRequest request)
        {
            try
            {
                return request.GetResponse() as HttpWebResponse;
            }
            catch (WebException ex)
            {
                Console.WriteLine("http error: " + ex.Message);

                // I think this should occur as long as the request didn't time
                // out or get rejected by last.fm server
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse httpWebResponse = ex.Response as HttpWebResponse;

                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(httpWebResponse.GetResponseStream());

                    XmlNode errorNode = xmlDocument.GetElementsByTagName("error")[0];
                    string lastFmErrorCoode = errorNode.Attributes["code"].Value;
                    string lastFmErrorString = errorNode.InnerText;
                    Console.WriteLine("last.fm says: code " + lastFmErrorCoode + ": " + lastFmErrorString);
                    MessageBox.Show("last.fm says: code " + lastFmErrorCoode + ": " + lastFmErrorString);

                    MessageBox.Show("There was a problem getting authentication from last.fm");
                }
                else
                {
                    throw new WebException(ex.Message, ex);
                }

                return null;
            }
        }


        /* End major changes by mope-life */



        private string CalculateMD5(string input)
        {
            MD5 md = MD5.Create();
            byte[] buffer = new UTF8Encoding().GetBytes(input);
            byte[] hash = md.ComputeHash(buffer);
            string md5 = string.Empty;
            for (int i = 0; i < hash.Length; i++)
            {
                md5 = md5 + hash[i].ToString("x2");
            }
            return md5;
        }
    }
}