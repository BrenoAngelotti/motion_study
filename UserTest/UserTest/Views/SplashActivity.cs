using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Task = System.Threading.Tasks.Task;

namespace UserTest.Views
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.Light", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        RelativeLayout Foreground { get; set; }
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            if(App.Current.UserData != null)
                SetTheme(App.Current.UserData.DarkTheme ? Resource.Style.AppTheme : Resource.Style.AppTheme_Light);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_splash);
            if (App.Current.UserData.HasMotion)
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
            if (App.Current.UserData.HasMotion)
                Foreground.Animate().Alpha(1).X(0).SetStartDelay(400).SetInterpolator(new DecelerateInterpolator(2.0f)).SetDuration(800);

            Task startupWork = new Task(() => { Startup(); });
            startupWork.Start();
        }

        public override void OnBackPressed() { }

        async void Startup()
        {
            await Task.Delay(1000);
            RunOnUiThread(() => {
                if (App.Current.UserData.Tasks == null || App.Current.UserData.Tasks.Count == 0 || App.Current.UserData.Tasks.All(t => !t.Finished))
                    StartActivity(new Intent(Application.Context, typeof(MainActivity)));

                else if(App.Current.UserData.Tasks.Any(t => !t.Finished))
                    StartActivity(new Intent(Application.Context, typeof(MainActivity)));

                else if(App.Current.UserData.Tasks.All(t => t.Finished) && !App.Current.UserData.FinishedAnswers)
                    StartActivity(new Intent(Application.Context, typeof(FinalEvaluationActivity)));

                else
                    StartActivity(new Intent(Application.Context, typeof(ClosingActivity)));

            });
        }
    }
}