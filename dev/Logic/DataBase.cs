using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using sl = System.Data.SQLite;

namespace Logic
{

    public class DataBase : IDisposable
    {
        public static string BasePath {get;set;}

        private sl.SQLiteConnection con;

        private DataBase()
        {
            Open(BasePath);
        }

        
        static DataBase instance;
        public static DataBase Instance { get { return instance ?? (instance = new DataBase()); } }
       

        void IDisposable.Dispose()
        {
            Close();
        }


        private void Close()
        {
            this.con.Close();
        }
        private void Open(string cinString)
        {
            this.con = new sl.SQLiteConnection("Data Source="+cinString+"; FailIfMissing=True");
            this.con.Open();
            var scheme = this.con.GetSchema();
            {
                sl.SQLiteCommand cmd = new sl.SQLiteCommand("select sqlite_version();", con);
                var res = cmd.ExecuteScalar();
                Debug.Assert(this.con.ServerVersion == res.ToString(), "Версия базы не совпадает");
            }
        }
        public DataTable Select(string sql)
        {
            
            sl.SQLiteCommand cmd = this.con.CreateCommand();
            cmd.CommandText = sql;
            sl.SQLiteDataAdapter da = new sl.SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable(sql);
            da.Fill(dt);
            return dt;
        }
        public void Exec(string sql)
        {
            sl.SQLiteCommand cmd = this.con.CreateCommand();
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }
    }
}
