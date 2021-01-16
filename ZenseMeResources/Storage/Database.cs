using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace ZenseMe.Lib.Storage
{
    public class Database
    {
        private static SQLiteConnection SQLiteConnection;

        public void Connect()
        {
            CreateDatabase();

            if (SQLiteConnection == null || SQLiteConnection.State == ConnectionState.Closed)
            {
                try
                {
                    SQLiteConnection = new SQLiteConnection();
                    SQLiteConnection.ConnectionString = "Data Source=Data\\storage.db3;Version=3;Journal Mode=Off;Compress=True;CharSet=UTF8;";
                    SQLiteConnection.Open();
                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine("SQLite connect error: " + ex);
                }
                CreateStructure();
            }
        }

        public void CreateDatabase()
        {
            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }

            if (!File.Exists("Data\\storage.db3"))
            {
                SQLiteConnection.CreateFile("Data\\storage.db3");
            }
        }

        public void CreateStructure()
        {
            if (SQLiteConnection.GetSchema("Tables").Rows.Count == 0)
            {
                Execute("CREATE TABLE device_tracks (" +
                    "[id] NVARCHAR(30), " +
                    "[persistent_id] NVARCHAR(30) PRIMARY KEY, " +
                    "[filename] TEXT, " +
                    "[name] NVARCHAR(256), " +
                    "[artist] NVARCHAR(256), " +
                    "[album] NVARCHAR(256), " +
                    "[genre] NVARCHAR(256), " +
                    "[length] INTEGER, " +
                    "[play_count] INTEGER, " +
                    "[play_count_his] INTEGER DEFAULT '0', " +
                    "[date_submitted] NVARCHAR(20) DEFAULT '0', " +
                    "[ignored] INTEGER DEFAULT '0', " +
                    "[device] NVARCHAR(30))");
            }

            // The genre column was added in v2.0.1. Add it if it doesn't
            // already exist.
            else if (SQLiteConnection.GetSchema("Columns").Select("COLUMN_NAME='genre' AND TABLE_NAME='device_tracks'").Length == 0)
            {
                Execute("ALTER TABLE device_tracks ADD COLUMN [genre] NVARCHAR(256)");
            }

            // The AutoIgnore feature was added in v2.0.2. Add table if it
            // doesn't exist.
            if (SQLiteConnection.GetSchema("Tables").Rows.Count == 1)
            {
                Execute("CREATE TABLE auto_ignore_rules(" +
                    "[field] NVARCHAR(20) NOT NULL, " +
                    "[value] NVARCHAR(256) NOT NULL," +
                    "CONSTRAINT id PRIMARY KEY (field,value))");
            }
        }

        public DataSet Fetch(string sqlQuery, params SQLiteParameter[] parameters)
        {
            Connect();
            DataSet fetchedData = new DataSet();

            try
            {
                using (SQLiteTransaction transaction = SQLiteConnection.BeginTransaction())
                {
                    using (SQLiteCommand command = new SQLiteCommand())
                    {
                        command.Parameters.AddRange(parameters);
                        command.CommandText = sqlQuery;
                        command.Connection = SQLiteConnection;

                        using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command))
                        {
                            using (SQLiteCommandBuilder commandBuilder = new SQLiteCommandBuilder(dataAdapter))
                            {
                                dataAdapter.Fill(fetchedData);
                                transaction.Commit();
                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("SQLite fetch error: " + ex);
            }
            return fetchedData;
        }

        public int Execute(string sqlQuery, params SQLiteParameter[] parameters)
        {
            Connect();
            int affectedRows = 0;

            try
            {
                using (SQLiteTransaction transaction = SQLiteConnection.BeginTransaction())
                {
                    using (SQLiteCommand command = new SQLiteCommand())
                    {
                        command.Parameters.AddRange(parameters);

                        command.CommandText = sqlQuery;
                        command.Connection = SQLiteConnection;

                        affectedRows = command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("SQLite execute error: " + ex);
            }
            return affectedRows;
        }

        public SQLiteDataAdapter GetDataAdapter(string selectCommandText)
        {
            Connect();
            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(selectCommandText, SQLiteConnection);
            SQLiteCommandBuilder commandBuilder = new SQLiteCommandBuilder(dataAdapter);

            dataAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
            dataAdapter.InsertCommand = commandBuilder.GetInsertCommand();
            dataAdapter.DeleteCommand = commandBuilder.GetDeleteCommand();

            return dataAdapter;
        }
    }
}