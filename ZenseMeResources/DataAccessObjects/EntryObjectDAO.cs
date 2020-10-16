using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using ZenseMe.Lib.Objects;
using ZenseMe.Lib.Storage;

namespace ZenseMe.Lib.DataAccessObjects
{
    public class EntryObjectDAO
    {
        private Database _hDatabase;

        public EntryObjectDAO()
        {
            _hDatabase = new Database();
        }

        public List<EntryObject> FetchSubmitted()
        {
            return FetchAll("WHERE play_count_his > 0");
        }

        public List<EntryObject> FetchNotSubmitted()
        {
            return FetchAll("WHERE play_count > play_count_his AND ignored == 0");
        }

        public List<EntryObject> FetchIgnored()
        {
            return FetchAll("WHERE ignored > 0");
        }

        public List<EntryObject> FetchPlayed()
        {
            return FetchAll("WHERE play_count > 0");
        }

        public List<EntryObject> FetchAll()
        {
            return FetchAll(null);
        }

        public List<EntryObject> FetchAll(string conditionStatement, params SQLiteParameter[] parameters)
        {
            DataSet dataSet;

            if (conditionStatement == null)
            {
                dataSet = _hDatabase.Fetch("SELECT * FROM device_tracks ORDER BY artist");
            }
            else
            {
                dataSet = _hDatabase.Fetch("SELECT * FROM device_tracks " + conditionStatement + " ORDER BY artist", parameters);
            }

            if (dataSet.Tables[0].Rows.Count > 0)
            {
                List<EntryObject> entries = new List<EntryObject>();

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    EntryObject entryObject = new EntryObject();
                    SetDatabaseFields(ref entryObject, row);

                    entries.Add(entryObject);
                }
                return entries;
            }
            else
            {
                return null;
            }
        }

        public List<string> FetchUnique(string field)
        {
            field = field.ToLower();

            DataSet dataSet = _hDatabase.Fetch("SELECT DISTINCT " + field + " FROM device_tracks");
            List<string> uniqueValues = new List<string>();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                uniqueValues.Add(row[field] as string);
            }

            return uniqueValues;
        }

        public EntryObject LoadObject(string persistentId)
        {
            DataSet dataSet = _hDatabase.Fetch("SELECT * FROM device_tracks WHERE persistent_id = '" + persistentId + "' LIMIT 1");

            if (dataSet.Tables[0].Rows.Count > 0)
            {
                EntryObject entryObject = new EntryObject();
                SetDatabaseFields(ref entryObject, dataSet.Tables[0].Rows[0]);

                return entryObject;
            }
            return null;
        }

        private bool ObjectExists(string persistentId)
        {
            string sqlQuery = "SELECT * FROM device_tracks WHERE persistent_id = @persistentId";
            DataSet dataSet = _hDatabase.Fetch(sqlQuery, new SQLiteParameter("@persistentId", persistentId));

            if (dataSet.Tables[0].Rows.Count == 0)
            {
                return false;
            }
            return true;
        }

        public uint CheckHis = 0;
        public void SaveObject(EntryObject entryObject)
        {
            if (!ObjectExists(entryObject.PersistentId))
            {
                string sqlQuery = "INSERT INTO device_tracks ("
                        + "[id], "
                        + "[persistent_id], "
                        + "[name], "
                        + "[artist], "
                        + "[album], "
                        + "[genre], "
                        + "[length], "
                        + "[device], "
                        + "[play_count], "
                        + "[filename] "
                        + ") VALUES ("
                        + "@id, "
                        + "@persistentId, "
                        + "@name, "
                        + "@artist, "
                        + "@album, "
                        + "@genre, "
                        + "@length, "
                        + "@device, "
                        + "@playCount, "
                        + "@filename "
                        + ")";

                _hDatabase.Execute(sqlQuery,
                    new SQLiteParameter("@id", entryObject.Id),
                    new SQLiteParameter("@persistentId", entryObject.PersistentId),
                    new SQLiteParameter("@name", entryObject.Name),
                    new SQLiteParameter("@artist", entryObject.Artist),
                    new SQLiteParameter("@album", entryObject.Album),
                    new SQLiteParameter("@genre", entryObject.Genre),
                    new SQLiteParameter("@length", entryObject.Length),
                    new SQLiteParameter("@device", entryObject.Device),
                    new SQLiteParameter("@playCount", entryObject.PlayCount),
                    new SQLiteParameter("@filename", entryObject.Filename)
                );
            }
            else
            {
                string sqlQuery = "SELECT play_count_his FROM device_tracks WHERE persistent_id = @persistentId";
                DataSet dataSet = _hDatabase.Fetch(sqlQuery, new SQLiteParameter("@persistentId", entryObject.PersistentId));

                foreach (DataTable thisTable in dataSet.Tables)
                {
                    foreach (DataRow DataRow in thisTable.Rows)
                    {
                        foreach (DataColumn DataCol in thisTable.Columns)
                        {
                            //Console.WriteLine("CheckNew: " + entryObject.PlayCount);
                            CheckHis = Convert.ToUInt32(DataRow[DataCol]);
                            //Console.WriteLine("CheckHis: " + CheckHis);

                            if (CheckHis > entryObject.PlayCount)
                            {
                                Console.WriteLine("Scrobblecount is higher resetting scrobble count.");
                                CheckHis = 0;
                            }
                        }
                    }
                }

                string sqlQueryUpdate = "UPDATE device_tracks SET id = @id, name = @name, artist = @artist, album = @album, genre = @genre, length = @length, device = @device, play_count = @playCount, play_count_his = @playCountHis, filename = @filename WHERE persistent_id = @persistentId";
                _hDatabase.Execute(sqlQueryUpdate,
                    new SQLiteParameter("@id", entryObject.Id),
                    new SQLiteParameter("@persistentId", entryObject.PersistentId),
                    new SQLiteParameter("@name", entryObject.Name),
                    new SQLiteParameter("@artist", entryObject.Artist),
                    new SQLiteParameter("@album", entryObject.Album),
                    new SQLiteParameter("@genre", entryObject.Genre),
                    new SQLiteParameter("@length", entryObject.Length),
                    new SQLiteParameter("@device", entryObject.Device),
                    new SQLiteParameter("@playCount", entryObject.PlayCount),
                    new SQLiteParameter("@playCountHis", CheckHis),
                    new SQLiteParameter("@filename", entryObject.Filename)
                );
            }
        }

        public void SetDatabaseFields(ref EntryObject entryObject, DataRow row)
        {
            entryObject.Id = row["id"] as string;
            entryObject.PersistentId = row["persistent_id"] as string;
            entryObject.Filename = row["filename"] as string;
            entryObject.Name = row["name"] as string;
            entryObject.Artist = row["artist"] as string;
            entryObject.Album = row["album"] as string;
            entryObject.Genre = row["genre"] as string;
            entryObject.Length = (int)((long)row["length"]);
            entryObject.PlayCount = (int)((long)row["play_count"]);
            entryObject.PlayCountHis = (int)((long)row["play_count_his"]);
            entryObject.Ignored = (int)((long)row["ignored"]);
            entryObject.Device = row["device"] as string;
        }
    }
}