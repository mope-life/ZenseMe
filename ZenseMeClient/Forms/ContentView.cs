using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ZenseMe.Lib.Objects;

namespace ZenseMe.Client.Forms
{
    public partial class ContentView : UserControl
    {
        private List<EntryObject> ConfirmScrobbleList;

        public List<EntryObject> Entries
        {
            get { return ConfirmScrobbleList; }
            set { ConfirmScrobbleList = value; }
        }

        public List<EntryObject> CheckedItems
        {
            get
            {
                List<EntryObject> entries = new List<EntryObject>();

                foreach (ListViewItem listViewItem in _cTrackContentView.CheckedItems)
                {
                    entries.Add((EntryObject)listViewItem.Tag);
                }
                return entries;
            }
        }

        public List<EntryObject> SelectedItems
        {
            get
            {
                List<EntryObject> selectedEntries = new List<EntryObject>();

                foreach (ListViewItem item in _cTrackContentView.SelectedItems)
                {
                    selectedEntries.Add((EntryObject)item.Tag);
                }
                return selectedEntries;
            }
        }

        public ContentView()
        {
            InitializeComponent();
            ConfirmScrobbleList = new List<EntryObject>();
        }   
        
        public void BindData(List<EntryObject> entries, bool withCheckboxes)
        {
            ConfirmScrobbleList = entries;
            _cTrackContentView.CheckBoxes = withCheckboxes;

            if (ConfirmScrobbleList == null || ConfirmScrobbleList.Count == 0)
            {
                _cTrackContentView.Items.Clear();
            }

            if (ConfirmScrobbleList != null && ConfirmScrobbleList.Count > 0)
            {
                _cTrackContentView.Items.Clear();
                foreach (EntryObject entry in ConfirmScrobbleList)
                {
                    string[] items = new string[] {
                        entry.Name,
                        entry.Artist,
                        entry.Album,
                        entry.Genre,
                        entry.LengthSeconds + " sec",
                        entry.PlayCount + "/" + entry.PlayCountHis,
                        entry.Device
                    };

                    ListViewItem listItem = new ListViewItem(items);
                    listItem.Tag = entry;

                    _cTrackContentView.Items.Add(listItem);
                }
            }
        }

        public void SelectAll(bool onOrOff = true)
        {
            foreach (ListViewItem listItem in _cTrackContentView.Items)
            {
                listItem.Checked = onOrOff;
            }
        }

        public void DeselectAll()
        {
            SelectAll(false);
        }
        
        public void SelectGroup(string queryType, string queryText, bool onOrOff)
        {
            Func<ListViewItem, bool> queryFieldProc;

            switch (queryType)
            {
                case "Artist": queryFieldProc = item => ((EntryObject)item.Tag).Artist == queryText; break;
                case "Album": queryFieldProc = item => ((EntryObject)item.Tag).Album == queryText; break;
                case "Genre": queryFieldProc = item => ((EntryObject)item.Tag).Genre == queryText; break;
                case "Device": queryFieldProc = item => ((EntryObject)item.Tag).Device == queryText; break;
                default: queryFieldProc = item => true; break;
            };

            foreach (ListViewItem item in _cTrackContentView.Items)
            {
                if (queryFieldProc(item))
                {
                    item.Checked = onOrOff;
                }
            }
        }
    }
}