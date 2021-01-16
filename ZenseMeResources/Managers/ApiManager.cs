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
    public class ApiManager
    {

        private string token = "";
        private string sessionKey = ConfigurationManager.AppSettings["LastFM_SessionKey"];

        /**
         * Submits a track to last.fm. Returns true if successful.
         */
        public bool SubmitTrack(string artist, string track, string album, int duration, DateTime dateSubmitted)
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

            HttpWebRequest httpWebRequest = BuildLastfmRequest(postData);

            return ApplyToResponse(httpWebRequest, httpWebResponse =>
            {
                // In this case, if the request does not throw an error, we
                // don't care about the content of the response.
                return true;

                // TODO: In the future we may want to handle the case where
                // last.fm ignores or corrects the track:
                // https://www.last.fm/api/show/track.scrobble
            });
        }

        /**
         * Returns the url we need to authorize the app on the user's last.fm profile.
         */
        public string GetAuthUrl()
        {
            string userSignInData = string.Format("api_key={0}&token={1}", Resources.Keys.API_KEY, token);
            return string.Format("http://www.last.fm/api/auth/?{0}", userSignInData); ;
        }

        /**
         * Retrieves an API token from last.fm. Needed to get authorization from
         * user, then retrieve a session key (which we can use indefinitely).
         * Should only be called if we don't already have a stored session key.
         */
        public bool GetToken()
        {
            Dictionary<string, string> postData = new Dictionary<string, string>
            {
                {"method", "auth.getToken" },
                {"api_key", Resources.Keys.API_KEY }
            };

            HttpWebRequest httpWebRequest = BuildLastfmRequest(postData);
            return ApplyToResponse(httpWebRequest, httpWebResponse =>
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
                    Console.WriteLine("[Audioscrobbler] Error in GetToken: " + ex);
                    MessageBox.Show("Unexpected xml response while retrieving token.", "ZenseMe");
                    return false;
                }
            });
        }

        /**
         * Gets a session key from last.fm. Once this returs successfully, we
         * should never have to go through the authentication process again.
         */
        public bool GetSession()
        {
            Dictionary<string, string> postData = new Dictionary<string, string>
            {
                {"method", "auth.getSession" },
                {"api_key", Resources.Keys.API_KEY },
                {"token", token }
            };

            HttpWebRequest httpWebRequest = BuildLastfmRequest(postData);
            return ApplyToResponse(httpWebRequest, httpWebResponse => {

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
                    Console.WriteLine("[Audioscrobbler] Error in GetSession: " + ex);
                    MessageBox.Show("Unexpected xml response while retrieving session key.", "ZenseMe");
                    return false;
                }
            });
        }

        /**
         * Takes our POST data, arranges it according to API specifications, and
         * signs it:
         * https://www.last.fm/api/authspec#_8-signing-calls
         *
         * Not all methods need be signed (i.e. user.getInfo)
         */
        private string ProcessPostData(Dictionary<string, string> dataDict, bool signed)
        {
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

        /**
         * Puts together a web request to be sent to the last.fm API with the
         * provided POST data.
         */
        private HttpWebRequest BuildLastfmRequest(Dictionary<string, string> dataDict, bool signed = true)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://ws.audioscrobbler.com/2.0/");

            byte[] Buffer = new UTF8Encoding().GetBytes(
                ProcessPostData(dataDict, signed)
                );

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = string.Format("ZenseMe/{0} ( mope-life )", Application.ProductVersion);
            request.Timeout = 5000;
            request.ContentLength = Buffer.Length;

            Stream Stream = request.GetRequestStream();
            Stream.Write(Buffer, 0, Buffer.Length);
            Stream.Close();

            return request;
        }

        /**
         * Simplifies the error checking for last.fm API error codes.
         *
         * Takes a web request and a callback. Attempts to get an
         * HttpWebResponse from the web request, then apply the callback to that
         * response. Also ensures that the response is properly closed after
         * we're done with it.
         *
         * Callback should return false to indicate that something has gone
         * wrong, or throw an exception.
         */
        private bool ApplyToResponse(HttpWebRequest httpWebRequest, Func<HttpWebResponse, bool> action)
        {
            HttpWebResponse httpWebResponse = null;

            try
            {
                httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
                return action(httpWebResponse);
            }
            catch (WebException ex)
            {
                Console.WriteLine("[Audioscrobbler] http error: " + ex.Message);

                // I think this should occur as long as the request didn't time
                // out or get rejected by last.fm server
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    httpWebResponse = ex.Response as HttpWebResponse;

                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(httpWebResponse.GetResponseStream());

                    XmlNode errorNode = xmlDocument.GetElementsByTagName("error")[0];
                    string lastFmErrorCoode = errorNode.Attributes["code"].Value;
                    string lastFmErrorString = errorNode.InnerText;
                    Console.WriteLine("[Audioscrobbler] last.fm says: code " + lastFmErrorCoode + ": " + lastFmErrorString);

                }
                MessageBox.Show("There was a problem communicating with last.fm");

                return false;
            }
            finally
            {
                if (httpWebResponse != null)
                {
                    httpWebResponse.Close();
                }
            }
        }


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