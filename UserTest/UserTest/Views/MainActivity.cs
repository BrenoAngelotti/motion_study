using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using UserTest.Model;
using UserTest.Helpers;
using Android.Views;

namespace UserTest.Views
{
    [Activity(Label="Main", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.KeyboardHidden | Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.ScreenLayout | Android.Content.PM.ConfigChanges.Density | Android.Content.PM.ConfigChanges.UiMode | Android.Content.PM.ConfigChanges.SmallestScreenSize)]
    public class MainActivity : AppCompatActivity
    {
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            if (!App.Current.User.HasMotion) Window.EnterTransition = null;
            HasMotion();

            var connection = Util.HasConnection(this);
            HasConnection(connection);

            await CanSync(connection);
        }

        private async System.Threading.Tasks.Task CanSync(bool connection)
        {
            if (connection)
            {
                var collection = await WebServices.IsDataCollectionActive();
                FindViewById<TextView>(Resource.Id.txvCollection).Text = $"Collection: {collection}";

                if (App.Current.User.IsSynced)
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
                        App.Current.User.IsSynced = true;
                        using (var conn = App.Current.GetConnection())
                        {
                            conn.Update(App.Current.User);
                        }
                    }
                }
                else
                {
                    Toast.MakeText(this, $"Obrigado pela colaboração, mas já encerramos 🙁", ToastLength.Long).Show();
                }
            }
        }

        private void HasConnection(bool connection)
        {
            FindViewById<TextView>(Resource.Id.txvConnection).Text = $"Connection: {connection}";
        }

        private void HasMotion()
        {
            FindViewById<TextView>(Resource.Id.txvMotion).Text = $"Motion: {App.Current.User.HasMotion}";
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
#if DEBUG
            MenuInflater.Inflate(Resource.Menu.menu_debug, menu);
            menu.FindItem(Resource.Id.toggle_motion).SetChecked(App.Current.User.HasMotion);
            menu.FindItem(Resource.Id.toggle_dark).SetChecked(App.Current.User.DarkTheme);
#endif
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.toggle_motion:
                    item.SetChecked(!item.IsChecked);
                    App.ToggleMotion();
                    HasMotion();
                    break;
                case Resource.Id.toggle_dark:
                    App.ToggleTheme();
                    break;
                default:
                    break;
            }

            return true;
        }
    }
}