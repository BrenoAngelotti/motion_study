using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;

namespace UserTest.Views
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.Light", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            if(App.Current.UserData != null)
                SetTheme(App.Current.UserData.DarkTheme ? Resource.Style.AppTheme : Resource.Style.AppTheme_Light);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_splash);

            Startup();
        }

        public override void OnBackPressed() { }

        void Startup()
        {
            if (App.Current.UserData.Tasks == null || App.Current.UserData.Tasks.Count == 0 || App.Current.UserData.Tasks.All(t => !t.Finished))
                StartActivity(new Intent(Application.Context, typeof(MainActivity)));

            else if (App.Current.UserData.Tasks.Any(t => !t.Finished))
                StartActivity(new Intent(Application.Context, typeof(MainActivity)));

            else if (App.Current.UserData.Tasks.All(t => t.Finished) && !App.Current.UserData.FinishedAnswers)
                StartActivity(new Intent(Application.Context, typeof(FinalEvaluationActivity)));

            else
                StartActivity(new Intent(Application.Context, typeof(ClosingActivity)));
        }
    }
}