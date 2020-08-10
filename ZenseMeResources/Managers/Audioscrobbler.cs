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

                HttpWebResponse scrobbleResponse = PostLastFmRequest(postData);
                XmlReader xReader = XmlReader.Create(scrobbleResponse.GetResponseStream());

                while (xReader.Read())
                {
                    if (xReader.Name == "lfm")
                    {
                        if (xReader.GetAttribute("status") != "ok")
                        {
                            XmlErrors(xReader);
                            throw new Exception();
                        }
                        else break;
                    }
                }
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

            HttpWebResponse getTokenResponse = PostLastFmRequest(postData);
            XmlReader xReader = XmlReader.Create(getTokenResponse.GetResponseStream());

            while (xReader.Read())
            {
                if (xReader.Name == "lfm" && xReader.GetAttribute("status") != "ok")
                {
                    XmlErrors(xReader);
                    return false;
                }
                else if (xReader.Name == "token")
                {
                    token = xReader.ReadElementContentAsString();
                    return true;
                }
            }

            MessageBox.Show("Somehow failed getting token from last.fm.", "ZenseMe");
            return false;
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

            HttpWebResponse getSessionResponse = PostLastFmRequest(postData);
            XmlReader xReader = XmlReader.Create(getSessionResponse.GetResponseStream());

            while (xReader.Read())
            {
                if (xReader.Name == "lfm" && xReader.GetAttribute("status") != "ok")
                {
                    XmlErrors(xReader);
                    return false;
                }
                else if (xReader.Name == "key")
                {
                    Configuration Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    sessionKey = xReader.ReadElementContentAsString();
                    Configuration.AppSettings.Settings["LastFM_SessionKey"].Value = sessionKey;
                    Configuration.Save(ConfigurationSaveMode.Modified);
                    return true;
                }
            }

            MessageBox.Show("Somehow failed obtaining session key.", "ZenseMe");
            return false;
        }


        public HttpWebResponse PostLastFmRequest(Dictionary<string, string> dataDict)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://ws.audioscrobbler.com/2.0/");

            byte[] Buffer = new UTF8Encoding().GetBytes(
                BuildRequest(dataDict)
                );

            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.UserAgent = string.Format("ZenseMe/{0} ( mope-life )", Application.ProductVersion);
            httpWebRequest.Timeout = 10000;
            httpWebRequest.ContentLength = Buffer.Length;

            Stream Stream = httpWebRequest.GetRequestStream();
            Stream.Write(Buffer, 0, Buffer.Length);
            Stream.Close();

            return (HttpWebResponse)httpWebRequest.GetResponse();
        }


        private string BuildRequest(Dictionary<string, string> dataDict)
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

            StringBuilder postDataBuilder = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in dataDict)
            {
                postDataBuilder.Append(
                    string.Format("{0}={1}&", kvp.Key, HttpUtility.UrlEncode(kvp.Value))
                    );
            }

            return postDataBuilder.ToString();
        }


        private void XmlErrors(XmlReader xReader)
        {
            xReader.ReadToDescendant("error");
            string err = "Failed authentication: {0}";
            MessageBox.Show(string.Format(err, xReader.ReadElementContentAsString()), "ZenseMe");
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