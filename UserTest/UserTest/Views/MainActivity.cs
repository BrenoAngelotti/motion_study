using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using UserTest.Helpers;
using Android.Views;
using Android.Content;
using System.Linq;

namespace UserTest.Views
{
    [Activity(Label="Main", NoHistory = true, ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.KeyboardHidden | Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.ScreenLayout | Android.Content.PM.ConfigChanges.Density | Android.Content.PM.ConfigChanges.UiMode | Android.Content.PM.ConfigChanges.SmallestScreenSize)]
    public class MainActivity : AppCompatActivity
    {
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            //await FillTestActivity();

            NextTask();
        }

        private async System.Threading.Tasks.Task FillTestActivity()
        {
            FindViewById<TextView>(Resource.Id.txvMotion).Text = $"Motion: {App.Current.UserData.HasMotion}";

            var connection = Util.HasConnection(this);

            FindViewById<TextView>(Resource.Id.txvConnection).Text = $"Connection: {connection}";

            if (connection)
            {
                var collection = await WebServices.IsDataCollectionActive();
                FindViewById<TextView>(Resource.Id.txvCollection).Text = $"Collection: {collection}";

                if (App.Current.UserData.IsSynced)
                    return;

                if (collection)
                {
                    var formTest = await WebServices.SendDataToForm();
                    if (!formTest)
                    {
                        Toast.MakeText(this, $"Dados não enviados, tente novamente mais tarde", ToastLength.Long).Show();
                    }
                    else
                    {
                        Toast.MakeText(this, $"Dados enviados com sucesso!", ToastLength.Long).Show();
                        App.Current.UserData.IsSynced = true;
                        App.SaveData();
                    }
                }
                else
                {
                    Toast.MakeText(this, $"Obrigado pela colaboração, mas já encerramos 🙁", ToastLength.Long).Show();
                }
            }
        }

#if DEBUG
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_debug, menu);
            menu.FindItem(Resource.Id.toggle_motion).SetChecked(App.Current.UserData.HasMotion);
            menu.FindItem(Resource.Id.toggle_dark).SetChecked(App.Current.UserData.DarkTheme);
            return true;
        }
#endif

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.toggle_motion:
                    item.SetChecked(!item.IsChecked);
                    App.ToggleMotion();
                    FillTestActivity();
                    App.SaveData();
                    break;
                default:
                    break;
            }

            return true;
        }

        void NextTask()
        {
            var nextTask = App.Current.UserData.Tasks.FirstOrDefault(t => !t.Finished);
            if (nextTask != null)
            {
                if (nextTask.TaskIdentifier == Enums.ETask.Theme)
                {
                    var intent = new Intent(this, typeof(ThemeActivity));
                    //intent.SetFlags(ActivityFlags.ClearTop);
                    StartActivity(intent);
                }
                else
                {
                    var intent = new Intent(this, typeof(TaskActivity));
                    intent.PutExtra("TASK", (int)nextTask.TaskIdentifier);
                    StartActivity(intent);
                }
            }
            else
            {
                StartActivity(new Intent(this, typeof(FinalEvaluationActivity)));
            }
        }
    }
}