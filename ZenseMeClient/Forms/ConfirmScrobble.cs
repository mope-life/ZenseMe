using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using ZenseMe.Lib.DataAccessObjects;
using ZenseMe.Lib.Managers;
using ZenseMe.Lib.Objects;

namespace ZenseMe.Client.Forms
{
    public partial class ConfirmScrobble : Form
    {
        private List<EntryObject> ConfirmScrobbleList;
        private Main _Frontend;

        public ConfirmScrobble(Main Frontend)
        {
            InitializeComponent();
            _Frontend = Frontend;
        }

        public void DataBind(List<EntryObject> entries)
        {
            ConfirmScrobbleList = entries;
            if (ConfirmScrobbleList != null && ConfirmScrobbleList.Count > 0)
            {
                foreach (EntryObject track in ConfirmScrobbleList)
                {
                    int ScrobbleCount = track.PlayCount - track.PlayCountHis;
                    for (int i = 1; i <= ScrobbleCount; i++)
                    {
                        string[] items = new string[] {
                        track.Name,
                        track.Artist,
                        track.Album,
                        track.DateSubmitted.ToString()
                    };

                        ListViewItem listItem = new ListViewItem(items);
                        cs_ScrobbleList.Items.Add(listItem);
                    }
                }

                //Set total scrobble count text
                cs_ScrobbleStatusLabel.Text = "The following " + ConfirmScrobbleList.Count() + " tracks will be scrobbled:";
                cs_ScrobbleStatusLabel.Refresh();
            }
        }

        private void btn_scrobble_Click(object sender, EventArgs e)
        {
            if (ConfirmScrobbleList != null && ConfirmScrobbleList.Count > 0)
            {
                Authenticator authenticator = new Authenticator();
                if (!authenticator.Authenticate())
                {
                    Close();
                    return;
                }

                Audioscrobbler audioscrobbler = new Audioscrobbler();

                //Sort the too scrobble songs
                ConfirmScrobbleList = ConfirmScrobbleList.OrderBy(x => x.DateSubmitted).ToList();

                //Scrobble the songs to Last.fm
                foreach (EntryObject track in ConfirmScrobbleList)
                {
                    if (track.LengthSeconds < 30)
                    {
                        lbl_ScrobbleStatus.Text = "Not scrobbling: " + track.Artist + " - " + track.Name + " track is too short.";
                        lbl_ScrobbleStatus.Refresh();
                    }
                    else
                    {
                        lbl_ScrobbleStatus.Text = "Now scrobbling: " + track.Artist + " - " + track.Name;
                        lbl_ScrobbleStatus.Refresh();
                    }

                    int ScrobbleCount = track.PlayCount - track.PlayCountHis;
                    for (int i = 1; i <= ScrobbleCount; i++)
                    {
                        Console.WriteLine("Scrobbling: " + track.Artist + " - " + track.Name);
                        track.DateSubmitted = track.DateSubmitted.AddSeconds(Convert.ToInt32(ConfigurationManager.AppSettings["Scrobble_BetweenTime"]));

                        if (!audioscrobbler.SubmitTrack(track.Artist, track.Name, track.Album, track.LengthSeconds, track.DateSubmitted))
                        {
                            Close();
                            return;
                        }
                    }

                    Console.WriteLine("Track added to scrobble history.");
                    PlayedTrackDAO PlayedTrackDAO = new PlayedTrackDAO();
                    PlayedTrackDAO.SaveObject(track.PersistentId, track.DateSubmitted, track.PlayCount);
                }

                Configuration Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                Configuration.AppSettings.Settings["Scrobble_LastDate"].Value = DateTime.Now.ToString();
                Configuration.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");

                _Frontend.InitializeTabs();
                _Frontend.ToolBarStatusText = "Ready, completed scrobbling to Last.fm.";
                Console.WriteLine("Ready, completed scrobbling to Last.fm.");
                Close();
            }
        }
    }
}