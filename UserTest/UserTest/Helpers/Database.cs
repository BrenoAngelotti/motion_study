using System;
using SQLite;
using System.Diagnostics;
using UserTest.Model;

namespace UserTest.Helpers
{
    public class Database : IDisposable
    {
        private SQLiteConnection Connection { get; set; }

        public Database(SQLiteConnection conn)
        {
            Connection = conn;
        }

        public bool CreateDataBase()
        {
            try
            {
                Connection.CreateTable<User>();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SQLiteException: {ex.Message}");
                return false;
            }
        }
        
        public void Dispose()
        {
            if (Connection != null)
                Connection.Dispose();
        }

        public void Insert<T>(T item)
        {
            Connection.Insert(item);
        }

        public void Update<T>(T item)
        {
            Connection.Update(item);
        }
    }
}