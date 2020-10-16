using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace ZenseMe.Client.Forms
{
    public partial class Settings : Form
    {
        private Main _Frontend;

        public Settings(Main Frontend)
        {
            InitializeComponent();
            _Frontend = Frontend;
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            lbl_version.Text = Application.ProductVersion;
            string Scrobble_BetweenTime = ConfigurationManager.AppSettings["Scrobble_BetweenTime"];
            string FetchAlbumArtist = ConfigurationManager.AppSettings["FetchAlbumArtist"];
            string FixUtcNowTime = ConfigurationManager.AppSettings["FixUtcNowTime"];
            string HttpsConnection = ConfigurationManager.AppSettings["HttpsConnection"];

            txt_Scrobble_BetweenTime.Text = Scrobble_BetweenTime;
            cb_FetchAlbumArtist.Checked = FetchAlbumArtist == "1";
            cb_FixUtcNowTime.Checked = FixUtcNowTime == "1";
            cb_HttpsConnection.Checked = HttpsConnection == "1";
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void btn_Save_Click(object sender, EventArgs e)
        {
            Configuration Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            string Scrobble_BetweenTime = txt_Scrobble_BetweenTime.Text;
            int FetchAlbumArtist = cb_FetchAlbumArtist.Checked ? 1 : 0;
            int FixUtcNowTime = cb_FixUtcNowTime.Checked ? 1 : 0;
            int HttpsConnection = cb_HttpsConnection.Checked ? 1 : 0;

            Configuration.AppSettings.Settings["FetchAlbumArtist"].Value = FetchAlbumArtist.ToString();
            Configuration.AppSettings.Settings["FixUtcNowTime"].Value = FixUtcNowTime.ToString();
            Configuration.AppSettings.Settings["HttpsConnection"].Value = HttpsConnection.ToString();

            if (Convert.ToUInt32(Scrobble_BetweenTime) > 0 && Scrobble_BetweenTime != "")
            {
                Configuration.AppSettings.Settings["Scrobble_BetweenTime"].Value = Scrobble_BetweenTime.ToString();
            }
            else
            {
                MessageBox.Show("Double scrobble time must be atleast 1 second.", "ZenseMe");
                return;
            }

            Configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            Close();

            _Frontend.InitializeTabs();
            _Frontend.ToolBarStatusText = "Ready, your settings have been saved.";
        }
    }
}