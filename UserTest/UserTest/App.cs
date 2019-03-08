using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using UserTest.Helpers;
using UserTest.Model;

namespace UserTest
{
    [Application]
    public class App : Application
    {
        public static App Current { get; private set; }
        public User User { get; set; } 
        public static string DbPath { get { return System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "database.db"); } }

        public App(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Current = this;
        }

        public override void OnCreate()
        {
            base.OnCreate();

            using (var conn = GetConnection())
            {
                new Database(conn).CreateDataBase();
                Current.User = conn.Table<User>().FirstOrDefault();
            }
        }

        public SQLiteConnection GetConnection()
        {
            var conn = new SQLiteConnection(DbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
            return conn;
        }
    }
}