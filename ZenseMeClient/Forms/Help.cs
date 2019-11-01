﻿using System.Windows.Forms;

namespace ZenseMe.Client.Forms
{
    public partial class Help : Form
    {
        public Help()
        {
            InitializeComponent();

            //Set ZenseMe text/version number
            string appVersion = Application.ProductVersion;
            txtbox_HelpText.Text = "| What is ZenseMe?\r\nZenseMe is a Windows Phone, Zune and MTP device Last.fm song scrobble application.\r\n\r\n| Last.fm connection problems?\r\nWhen the songs don't show up on your Last.fm profile make sure ZenseMe is not blocked by your firewall.\r\n\r\n| How do I quickly select songs?\r\nBy right clicking on the 'Recently Played/Ignored Tracks' list you can select all tracks, artists, albums or devices.\r\n\r\n| Why do my songs turn up with a 0 playcount?\r\nYour device only saves the playcount from stored tracks, tracks streamed with your Zune Pass or Google Music will not count, it can also be that your device does not support MTP or is set to MSC.\r\n\r\n| How can I see if my device supports MTP?\r\nYou can check if your device is compatible with ZenseMe in the device list found on the ZenseMe website forums.\r\n\r\n| Development donation support\r\nFeel free to make a donation to support me with my developing projects, you can find a donation page on the project website." + "\r\n\r\n| ZenseMe by Arnold Vink - v" + appVersion;
        }
    }
}