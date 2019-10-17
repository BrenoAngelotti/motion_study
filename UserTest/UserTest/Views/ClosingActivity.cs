using Android.App;
using Android.OS;
using Android.Widget;
using UserTest.Helpers;

namespace UserTest.Views
{
    [Activity(Label = "ClosingActivity", NoHistory = true)]
    public class ClosingActivity : Activity
    {
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(App.Current.UserData.DarkTheme ? Resource.Style.AppTheme : Resource.Style.AppTheme_Light);
            base.OnCreate(savedInstanceState);

            await TestActivity();
        }

        private async System.Threading.Tasks.Task TestActivity()
        {
            var connection = Util.HasConnection(this);

            if (connection)
            {
                var collection = await WebServices.IsDataCollectionActive();

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
            }
        }
    }
}
