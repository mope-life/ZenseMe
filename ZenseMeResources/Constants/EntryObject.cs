using System;

namespace ZenseMe.Lib.Objects
{
    public class EntryObject
    {
        private string _sId;
        private string _sPersistentId;
        private string _iDevice;
        private string _sArtist;
        private string _sAlbum;
        private string _sGenre;
        private int _iTrack;
        private int _iPlayCount;
        private string _sName;
        private string _sFilename;
        private int _iLength;
        private int _sIgnored;

        private DateTime _dDateSubmitted;
        private int _sPlayCountHis;

        public string Id
        {
            get { return _sId; }
            set { _sId = value; }
        }

        public string PersistentId
        {
            get { return _sPersistentId; }
            set { _sPersistentId = value; }
        }

        public string Device
        {
            get { return _iDevice; }
            set { _iDevice = value; }
        }

        public string Artist
        {
            get { return _sArtist; }
            set { _sArtist = value; }
        }

        public string Album
        {
            get { return _sAlbum; }
            set { _sAlbum = value; }
        }

        public string Genre
        {
            get { return _sGenre; }
            set { _sGenre = value; }
        }

        public int Track
        {
            get { return _iTrack; }
            set { _iTrack = value; }
        }

        public int PlayCount
        {
            get { return _iPlayCount; }
            set { _iPlayCount = value; }
        }

        public string Name
        {
            get { return _sName; }
            set { _sName = value; }
        }

        public string Filename
        {
            get { return _sFilename; }
            set { _sFilename = value; }
        }

        public int Length
        {
            get { return _iLength; }
            set { _iLength = value; }
        }

        public DateTime DateSubmitted
        {
            get { return _dDateSubmitted; }
            set { _dDateSubmitted = value; }
        }

        public int PlayCountHis
        {
            get { return _sPlayCountHis; }
            set { _sPlayCountHis = value; }
        }

        public int Ignored
        {
            get { return _sIgnored; }
            set { _sIgnored = value; }
        }

        public int LengthSeconds
        {
            get
            {
                return Length / 1000;
            }
        }
    }
}