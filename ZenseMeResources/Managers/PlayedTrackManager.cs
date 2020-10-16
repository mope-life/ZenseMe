using System;
using System.Collections.Generic;
using ZenseMe.Lib.Objects;

namespace ZenseMe.Lib.Managers
{
    public class PlayedTrackManager
    {
        public List<EntryObject> GenerateScrobbleTimes(List<EntryObject> entries, DateTime startDateTime)
        {
            List<EntryObject> playedTracks = new List<EntryObject>();

            if (entries == null) return null;

            foreach (EntryObject entry in entries)
            {
                EntryObject playedTrack = new EntryObject();
                playedTrack.DateSubmitted = startDateTime;

                playedTrack.Id = entry.Id;
                playedTrack.PersistentId = entry.PersistentId;
                playedTrack.Name = entry.Name;
                playedTrack.Artist = entry.Artist;
                playedTrack.Album = entry.Album;
                playedTrack.Genre = entry.Genre;
                playedTrack.Length = entry.Length;
                playedTrack.Device = entry.Device;
                playedTrack.PlayCount = entry.PlayCount;
                playedTrack.Filename = entry.Filename;
                playedTrack.PlayCountHis = entry.PlayCountHis;

                playedTracks.Add(playedTrack);
                startDateTime = startDateTime.AddSeconds(-entry.LengthSeconds);
            }
            return playedTracks;
        }
    }
}