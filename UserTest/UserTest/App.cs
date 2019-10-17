using System;
using System.Collections.Generic;
using Android.App;
using Android.Runtime;
using Newtonsoft.Json;
using UserTest.Enums;
using UserTest.Model;
using Xamarin.Essentials;

namespace UserTest
{
    [Application(AllowBackup = false)]
    public class App : Application
    {
        public static App Current { get; private set; }
        public UserData UserData { get; set; }

        public App(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Current = this;
        }

        public override void OnCreate()
        {
            base.OnCreate();

            GetData();

            if (Current.UserData == null || Current.UserData.Tasks == null || Current.UserData.Tasks.Count == 0)
            {
                PrepareUserData();
                SaveData();
            }
        }

        private void PrepareUserData()
        {
            var isAnimated = new Random().NextDouble() >= .5;
            var data = new UserData() { HasMotion = isAnimated, IsSynced = false, Tasks = new List<Task>() };

            var tasks = new List<Task>() { new Task() { TaskIdentifier = ETask.Theme } };
            for(int i = 1; i < 5; i++)
            {
                tasks.Insert((new Random().NextDouble() >= .5 ? 1 : tasks.Count),
                    new Task() {TaskIdentifier = (ETask)i });
            }
            data.Tasks.AddRange(tasks);

            Current.UserData = data;
        }

        public static void GetData()
        {
            Current.UserData = JsonConvert.DeserializeObject<UserData>(Preferences.Get("DATA", "{}"));
        }

        public static void SaveData()
        {
            Preferences.Set("DATA", JsonConvert.SerializeObject(Current.UserData));
        }

        public static void ToggleMotion()
        {
            Current.UserData.HasMotion ^= true;
        }
    }

}