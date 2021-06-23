using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace ARF_Editor.Tools
{
    static class Database
    {
        public static class Settings
        {
            public static string dbPath = @"C:\Users\Taysir\source\repos\ARF-Editor\Database\index.db";
        }
        public static bool connectionHergestellt = false;
        static SQLiteConnection con;
        public static void Setup()
        {
            con = createConnection();
            connectionHergestellt = true;
        }


        private static SQLiteConnection createConnection()
        {
            //string dbpath = Path.Combine(Directory.GetCurrentDirectory(), "Database", "index.db");
            SQLiteConnection con = new SQLiteConnection($"Data Source={Settings.dbPath};Version=3;New=False;UseUTF16Encoding=True;Journal Mode=Off;");
            con.Open();

            return con;
        }
        
        public static SQLiteDataReader Read(string query)
        {
            SQLiteCommand cmd = con.CreateCommand();
            cmd.CommandText = query;

            return cmd.ExecuteReader();
        }
        public static int Write(string query)
        {
            SQLiteCommand cmd = con.CreateCommand();
            cmd.CommandText = query;
            return cmd.ExecuteNonQuery();
        }


        public static (ushort, ushort, string, string)[] GetCards()
        {
            var reader = Read("SELECT * FROM cards");
            // PK       ID      name        descriptor
            List<(ushort, ushort, string, string)> cards = new List<(ushort, ushort, string, string)>();

            while (reader.Read())
                cards.Add( (Convert.ToUInt16(reader["pk"]), Convert.ToUInt16(reader["ID"]), (string)reader["name"], (string)reader["descriptor"]) );
            
            return cards.ToArray();
        }
        public static (ushort, ushort, string, string)[] GetAttacks()
        {
            var reader = Read("SELECT * FROM attacks");
            List<(ushort, ushort, string, string)> attacks = new List<(ushort, ushort, string, string)>();

            while (reader.Read())
                attacks.Add( (Convert.ToUInt16(reader["pk"]), Convert.ToUInt16(reader["ID"]), (string)reader["name"], (string)reader["descriptor"]) );

            return attacks.ToArray();
        }
    }
}
