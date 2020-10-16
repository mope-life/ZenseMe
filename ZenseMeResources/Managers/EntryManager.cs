using PortableDeviceApiLib;
using System;
using System.Collections;
using System.Configuration;
using System.Windows.Forms;
using ZenseMe.Lib.Constants;
using ZenseMe.Lib.Objects;

namespace ZenseMe.Lib.Managers
{
    public class EntryManager
    {
        private PortableDeviceClass _hDeviceClass;
        private IPortableDeviceContent _hDeviceContent;
        private DeviceManager _hDeviceManager;
        private Device _hDevice;

        public EntryManager(Device device)
        {
            _hDevice = device;
            _hDeviceManager = new DeviceManager();
            try
            {
                _hDeviceClass = _hDeviceManager.GetDevice(device.Id);
            }
            catch
            {
                MessageBox.Show("Please connect a device before fetching songs.", "ZenseMe");
                return;
            }
        }

        public void BeginFetchContent()
        {
            _hDeviceClass.Content(out _hDeviceContent);
            EnumerateFetchContent("DEVICE", 0);
        }

        public delegate void FoundTrackEntryDelegate(EntryObject entryObject);
        public event FoundTrackEntryDelegate FoundNewMusicEntryEvent;

        public void EnumerateFetchContent(string parentId, int currentLevel)
        {
            EntryObject entryObject = new EntryObject();
            UpdateEntryProperties(parentId, ref entryObject);

            if (entryObject.Track >= 0 || (entryObject.Filename != null && entryObject.Filename.EndsWith(".")))
            {
                FoundNewMusicEntryEvent(entryObject);
            }

            try
            {
                IEnumPortableDeviceObjectIDs deviceObjectIds;
                _hDeviceContent.EnumObjects(0, parentId, null, out deviceObjectIds);

                uint fetchedCount = 0;
                do
                {
                    string objectId;
                    deviceObjectIds.Next(1, out objectId, ref fetchedCount);

                    if (fetchedCount > 0)
                    {
                        EnumerateFetchContent(objectId, currentLevel + 1);

                        if (objectId == "RenderingInformation" || objectId == _hDevice.FriendlyName)
                        {
                            Console.WriteLine("Song fetching successfully completed.");
                            MessageBox.Show("Song fetching successfully completed.", "ZenseMe");
                            return;
                        }
                    }
                }
                while (fetchedCount > 0);
            }
            catch
            {
                Console.WriteLine("Your device is no longer connected?");
            }
        }

        public void UpdateEntryProperties(string objectId, ref EntryObject entry)
        {
            Hashtable properties = _hDeviceManager.GetDeviceProperties(_hDevice.Id, objectId);

            if (properties.Count != 0)
            {
                entry.Id = (string)properties[PropertyNames.OBJECT_ID];
                entry.PersistentId = (string)properties[PropertyNames.OBJECT_PERSISTENT_ID];

                if (int.Parse(ConfigurationManager.AppSettings["FetchAlbumArtist"]) >= 1)
                {
                    if (properties[PropertyNames.OBJECT_ALBUM_ARTIST] != null)
                    {
                        entry.Artist = (string)properties[PropertyNames.OBJECT_ALBUM_ARTIST];
                    }
                    else
                    {
                        entry.Artist = (string)properties[PropertyNames.OBJECT_ARTIST];
                    }
                }
                else
                {
                    if (properties[PropertyNames.OBJECT_ARTIST] != null)
                    {
                        entry.Artist = (string)properties[PropertyNames.OBJECT_ARTIST];
                    }
                    else
                    {
                        entry.Artist = "Unknown Artist";
                    }
                }

                if (properties[PropertyNames.OBJECT_NAME] != null)
                {
                    entry.Name = (string)properties[PropertyNames.OBJECT_NAME];
                }
                else
                {
                    entry.Name = "Unknown Track Name";
                }

                if (properties[PropertyNames.OBJECT_ALBUM] != null)
                {
                    entry.Album = (string)properties[PropertyNames.OBJECT_ALBUM];
                }
                else
                {
                    entry.Album = "Unknown Album";
                }

                if (properties[PropertyNames.OBJECT_GENRE] != null)
                {
                    entry.Genre = (string)properties[PropertyNames.OBJECT_GENRE];
                }
                else
                {
                    entry.Genre = "Unknown Genre";
                }

                if (properties[PropertyNames.OBJECT_FILENAME] != null)
                {
                    entry.Filename = (string)properties[PropertyNames.OBJECT_FILENAME];
                }
                else
                {
                    entry.Filename = "Unknown Filename";
                }

                if (properties[PropertyNames.OBJECT_DURATION] != null)
                {
                    entry.Length = (int)((long)properties[PropertyNames.OBJECT_DURATION]);
                }
                else
                {
                    entry.Length = 0;
                }

                if (properties[PropertyNames.OBJECT_PLAY_COUNT] != null)
                {
                    entry.PlayCount = (int)((uint)properties[PropertyNames.OBJECT_PLAY_COUNT]);
                }
                else
                {
                    entry.PlayCount = 0;
                }

                if (properties[PropertyNames.OBJECT_TRACK] != null)
                {
                    entry.Track = (int)((uint)properties[PropertyNames.OBJECT_TRACK]);
                }
                else
                {
                    entry.Track = -1;
                }

                if (properties[PropertyNames.DEVICE_MODEL] != null && properties[PropertyNames.DEVICE_FRIENDLY_NAME] != null)
                {
                    entry.Device = (string)properties[PropertyNames.DEVICE_MODEL] + " (" + (string)properties[PropertyNames.DEVICE_FRIENDLY_NAME] + ")";
                }
                else
                {
                    entry.Device = _hDevice.Model + " (" + _hDevice.FriendlyName + ")";
                }
                Console.WriteLine("Current artist: " + entry.Artist + " - album: " + entry.Album);
            }
        }
    }
}