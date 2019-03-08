using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using UserTest.Model;

namespace UserTest.Views
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class SplashActivity : Activity
    {
        RelativeLayout Foreground { get; set; }
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_splash);
            if(App.Current.User == null)
            {
                var isAnimated = new Random().NextDouble() >= .5;
                var user = new User() { HasMotion = isAnimated, IsSynced = false } ;
                using (var conn = App.Current.GetConnection())
                {
                    conn.Insert(user);
                }
                App.Current.User = user;
            }
            if (App.Current.User.HasMotion)
            {
                Foreground = FindViewById<RelativeLayout>(Resource.Id.splash_foreground);
                Foreground.TranslationX = -120;
                Foreground.Alpha = 0;
                Window.ExitTransition = null;
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (App.Current.User.HasMotion)
                Foreground.Animate().Alpha(1).X(0).SetStartDelay(400).SetInterpolator(new DecelerateInterpolator(2.0f)).SetDuration(800);

            Task startupWork = new Task(() => { Startup(); });
            startupWork.Start();
        }

        public override void OnBackPressed() { }

        async void Startup()
        {
            await Task.Delay(2000); // Simulate a bit of startup work.
            RunOnUiThread(() => {
                StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            });
        }
    }
}